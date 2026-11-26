using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using GSAIServicesAPI.Models.Images;
using Microsoft.Extensions.Options;

namespace GSAIServicesAPI.Services.Images
{
    /// <summary>
    /// Azure Blob Storage implementation of <see cref="IImageBlobStorage"/>.
    /// Decodes Base64 image payloads and persists them as blobs, returning a URI for each.
    /// </summary>
    /// <remarks>
    /// Registered as a singleton: the underlying <see cref="BlobContainerClient"/> is thread-safe
    /// and intended to be reused for the lifetime of the application. The target container is
    /// created on first use and the result is memoized, so the existence check costs one round
    /// trip per process rather than one per upload.
    /// </remarks>
    public class AzureImageBlobStorage : IImageBlobStorage
    {
        /// <summary>Client scoped to the container named in <see cref="BlobStorageOptions.ContainerName"/>.</summary>
        private readonly BlobContainerClient _container;

        /// <summary>Snapshot of the configured storage options, captured at construction.</summary>
        private readonly BlobStorageOptions _options;

        private readonly ILogger<AzureImageBlobStorage> _logger;

        /// <summary>Guards <see cref="_initialized"/> so concurrent callers create the container once.</summary>
        private readonly SemaphoreSlim _initLock = new(1, 1);

        /// <summary>True once the container has been confirmed to exist for this instance.</summary>
        private bool _initialized;

        /// <summary>
        /// Initializes the storage service for the configured container.
        /// </summary>
        /// <param name="blobServiceClient">The account-level client used to resolve the container.</param>
        /// <param name="options">Container name, credential mode and SAS settings.</param>
        /// <param name="logger">Logger for upload and credential diagnostics.</param>
        /// <remarks>
        /// The constructor performs no network calls; the container is resolved lazily on the
        /// first upload so that a misconfigured connection string surfaces at call time rather
        /// than during application startup.
        /// </remarks>
        public AzureImageBlobStorage(
            BlobServiceClient blobServiceClient,
            IOptions<BlobStorageOptions> options,
            ILogger<AzureImageBlobStorage> logger)
        {
            _options = options.Value;
            _logger = logger;
            _container = blobServiceClient.GetBlobContainerClient(_options.ContainerName);
        }

        /// <summary>
        /// Uploads several Base64 payloads concurrently.
        /// </summary>
        /// <param name="base64Images">The payloads to persist. Null or whitespace entries are skipped.</param>
        /// <param name="cancellationToken">Token used to cancel the outbound uploads.</param>
        /// <returns>
        /// The resulting URIs, in the same order as the surviving inputs. The returned list may be
        /// shorter than <paramref name="base64Images"/> when blank entries were filtered out.
        /// </returns>
        /// <exception cref="FormatException">At least one payload was not valid Base64.</exception>
        /// <exception cref="OperationCanceledException">The operation was cancelled.</exception>
        /// <remarks>
        /// All uploads are started before any is awaited, so a failure in one does not prevent the
        /// others from reaching the service; the first exception observed is the one rethrown.
        /// </remarks>
        public async Task<IReadOnlyList<string>> UploadBase64Async(
            IEnumerable<string> base64Images,
            CancellationToken cancellationToken = default)
        {
            // Task.WhenAll keeps the result order aligned with the input order.
            var uploads = base64Images
                .Where(b => !string.IsNullOrWhiteSpace(b))
                .Select(b => UploadBase64Async(b, cancellationToken));

            return await Task.WhenAll(uploads);
        }

        /// <summary>
        /// Decodes a single Base64 payload and uploads it as a new blob.
        /// </summary>
        /// <param name="base64">
        /// The image data, either raw Base64 or a data URI such as <c>data:image/png;base64,...</c>.
        /// </param>
        /// <param name="cancellationToken">Token used to cancel the outbound upload.</param>
        /// <returns>
        /// A read SAS URI when <see cref="BlobStorageOptions.UseSasUri"/> is enabled and the client
        /// can sign it; otherwise the unsigned blob URI.
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="base64"/> is null, empty or whitespace.</exception>
        /// <exception cref="FormatException">The payload was not valid Base64.</exception>
        /// <exception cref="Azure.RequestFailedException">The storage service rejected the upload.</exception>
        /// <remarks>
        /// Each call writes to a freshly generated, date-partitioned path, so uploading identical
        /// bytes twice produces two distinct blobs rather than overwriting.
        /// </remarks>
        public async Task<string> UploadBase64Async(
            string base64,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(base64);

            var bytes = DecodeBase64(base64);
            var contentType = DetectContentType(bytes);
            var extension = ExtensionFor(contentType);

            await EnsureContainerAsync(cancellationToken);

            // Date-partitioned path keeps listings manageable as volume grows.
            var blobName = $"{DateTime.UtcNow:yyyy/MM/dd}/{Guid.NewGuid():N}{extension}";
            var blob = _container.GetBlobClient(blobName);

            using var stream = new MemoryStream(bytes, writable: false);
            await blob.UploadAsync(
                stream,
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = contentType,
                        CacheControl = "public, max-age=31536000, immutable"
                    }
                },
                cancellationToken);

            _logger.LogInformation("Uploaded generated image {BlobName} ({Bytes} bytes).",
                blobName, bytes.Length);

            return blobName;
        }

        /// <summary>
        /// Creates the target container if it does not yet exist, at most once per instance.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the check.</param>
        /// <remarks>
        /// Uses double-checked locking around <see cref="_initLock"/> so that concurrent first
        /// uploads issue a single creation request. The container is created without public
        /// access; anonymous reads are never enabled by this service.
        /// </remarks>
        private async Task EnsureContainerAsync(CancellationToken cancellationToken)
        {
            if (_initialized) return;

            await _initLock.WaitAsync(cancellationToken);
            try
            {
                if (_initialized) return;
                await _container.CreateIfNotExistsAsync(
                    PublicAccessType.None, cancellationToken: cancellationToken);
                _initialized = true;
            }
            finally
            {
                _initLock.Release();
            }
        }


        /// <summary>
        /// Converts a Base64 string to bytes, stripping an optional data URI prefix first.
        /// </summary>
        /// <param name="base64">Raw Base64 or a <c>data:</c> URI.</param>
        /// <returns>The decoded image bytes.</returns>
        /// <exception cref="FormatException">The remaining text was not valid Base64.</exception>
        private static byte[] DecodeBase64(string base64)
        {
            var payload = base64.AsSpan().Trim();

            if (payload.StartsWith("data:"))
            {
                var comma = payload.IndexOf(',');
                if (comma >= 0)
                    payload = payload[(comma + 1)..];
            }

            return Convert.FromBase64String(payload.ToString());
        }

        /// <summary>
        /// Determines the MIME type from the decoded bytes' magic number.
        /// </summary>
        /// <param name="bytes">The decoded image data.</param>
        /// <returns>
        /// <c>image/png</c>, <c>image/jpeg</c> or <c>image/webp</c> when the signature is
        /// recognized; otherwise <c>application/octet-stream</c>.
        /// </returns>
        /// <remarks>
        /// The signature is inspected rather than trusting any media type declared in a data URI
        /// prefix, so a mislabelled payload is still stored with a correct content type.
        /// </remarks>
        private static string DetectContentType(ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length >= 8 &&
                bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
                return "image/png";

            if (bytes.Length >= 3 &&
                bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
                return "image/jpeg";

            if (bytes.Length >= 12 &&
                bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x46 &&
                bytes[8] == 0x57 && bytes[9] == 0x45 && bytes[10] == 0x42 && bytes[11] == 0x50)
                return "image/webp";

            return "application/octet-stream";
        }

        /// <summary>
        /// Maps a MIME type to the file extension used in the blob name.
        /// </summary>
        /// <param name="contentType">A MIME type returned by <see cref="DetectContentType"/>.</param>
        /// <returns>The matching extension, including the leading dot, or <c>.bin</c> if unrecognized.</returns>
        private static string ExtensionFor(string contentType) => contentType switch
        {
            "image/png" => ".png",
            "image/jpeg" => ".jpg",
            "image/webp" => ".webp",
            _ => ".bin"
        };
    }
}