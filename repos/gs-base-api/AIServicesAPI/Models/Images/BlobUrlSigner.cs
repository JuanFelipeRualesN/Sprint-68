using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using GSAIServicesAPI.Models.Images;
using Microsoft.Extensions.Options;

namespace GSAIServicesAPI.Services.Images
{
    public sealed class BlobUrlSigner : IBlobUrlSigner
    {
        private readonly BlobContainerClient _container;
        private readonly BlobStorageOptions _options;
        private readonly HashSet<string> _knownHosts;

        public BlobUrlSigner(
            BlobServiceClient blobServiceClient,
            IOptions<BlobStorageOptions> options)
        {
            _options = options.Value;
            _container = blobServiceClient.GetBlobContainerClient(_options.ContainerName);

            _knownHosts = new HashSet<string>(
                _options.KnownHosts.Append(_container.Uri.Host),
                StringComparer.OrdinalIgnoreCase);
        }

        public bool TryGetBlobName(string url, out string? blobName)
        {
            blobName = null;

            if (string.IsNullOrWhiteSpace(url))
                return false;

            // Relative or malformed values (e.g. "data:", "cid:", "/assets/logo.png") are not ours.
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                return false;

            if (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
                return false;

            if (!_knownHosts.Contains(uri.Host))
                return false;

            // AbsolutePath is "/{container}/{blobName}"; the query holds the SAS and is discarded.
            var path = Uri.UnescapeDataString(uri.AbsolutePath).TrimStart('/');
            var prefix = _options.ContainerName + "/";

            if (!path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return false;

            var name = path[prefix.Length..];
            if (string.IsNullOrWhiteSpace(name))
                return false;

            blobName = name;
            return true;
        }

        public string ToCanonicalUrl(string blobName)
            => _container.GetBlobClient(blobName).Uri.ToString();

        public string ToSignedUrl(string blobName)
        {
            var blob = _container.GetBlobClient(blobName);

            if (!_options.UseSasUri || !blob.CanGenerateSasUri)
                return blob.Uri.ToString();

            var builder = new BlobSasBuilder
            {
                BlobContainerName = blob.BlobContainerName,
                BlobName = blob.Name,
                Resource = "b",
                ExpiresOn = NextExpiryBoundary()
            };
            builder.SetPermissions(BlobSasPermissions.Read);

            return blob.GenerateSasUri(builder).ToString();
        }

        /// <summary>
        /// Snaps the expiry to a fixed window boundary so repeated reads of the same document
        /// produce a byte-identical URL, which keeps browser and CDN caches working.
        /// </summary>
        /// <remarks>
        /// Rounding up by two windows guarantees every issued URL is valid for at least one
        /// full <see cref="BlobStorageOptions.SasValidForHours"/> period, even when generated
        /// moments before a boundary.
        /// </remarks>
        private DateTimeOffset NextExpiryBoundary()
        {
            var window = TimeSpan.FromHours(Math.Max(1, _options.SasValidForHours)).Ticks;
            var ticks = (DateTimeOffset.UtcNow.Ticks / window + 2) * window;
            return new DateTimeOffset(ticks, TimeSpan.Zero);
        }
    }
}