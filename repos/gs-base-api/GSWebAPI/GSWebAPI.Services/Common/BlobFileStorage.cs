using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using GSWebAPI.GSWebAPI.Services.Utils;
using GSWebAPI.Utils;
using Microsoft.Extensions.Logging;

namespace GSWebAPI.GSWebAPI.Services.Common
{
    /// <summary>Stores uploaded files (custom plan evidences, community resources) in Azure Blob Storage.</summary>
    public interface IBlobFileStorage
    {
        /// <summary>Uploads the file and returns its blob URL without SAS. Null if Blob Storage could not be reached.</summary>
        /// <param name="blobName">Blob name (path inside the container).</param>
        /// <param name="content">File content.</param>
        /// <param name="contentType">Content type stored with the blob.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The blob URL without query string, or null when the upload failed.</returns>
        Task<string?> UploadAsync(string blobName, Stream content, string contentType, CancellationToken cancellationToken = default);

        /// <summary>A read-only link to the blob, valid for a short time.</summary>
        /// <param name="blobName">Blob name (path inside the container) saved with the row.</param>
        /// <returns>The blob URL with a newly signed read-only SAS.</returns>
        string GetReadUrl(string blobName);
    }

    /// <summary>
    /// Azure Blob Storage, same account and container as the rest of GSWebAPI (AppSettings:azure_storage_account_name,
    /// AzureStorageAccountKey, azure_storage_container_name_gsweb, read through <see cref="Configurations"/>).
    /// <list type="bullet">
    /// <item>Upload: reuses <see cref="AzureAssets.GetBlobStorageUri"/> (the write SAS used by the existing reports
    /// upload) and sets the content type, so PDFs and images open in the browser.</item>
    /// <item>Read: the database keeps the blob name, never a SAS URL (those expire). Each read signs a new read-only
    /// link valid for <see cref="ReadLinkLifetime"/>.</item>
    /// </list>
    /// </summary>
    public sealed class AzureBlobFileStorage : IBlobFileStorage
    {
        /// <summary>How long a read link stays valid (1 hour; the SAS also starts 5 minutes in the past to allow for clock skew).</summary>
        public static readonly TimeSpan ReadLinkLifetime = TimeSpan.FromHours(1);

        private readonly ILogger<AzureBlobFileStorage> _logger;

        /// <summary>Creates the storage.</summary>
        /// <param name="logger">Logger for failed uploads.</param>
        public AzureBlobFileStorage(ILogger<AzureBlobFileStorage> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<string?> UploadAsync(string blobName, Stream content, string contentType, CancellationToken cancellationToken = default)
        {
            try
            {
                var uploadUri = AzureAssets.GetBlobStorageUri(blobName).Uri;
                var blob = new BlobClient(uploadUri);
                await blob.UploadAsync(content, new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = contentType } }, cancellationToken);
                return new UriBuilder(uploadUri) { Query = string.Empty }.Uri.ToString();
            }
            catch (Exception ex) when (ex is RequestFailedException or IOException or ArgumentException or InvalidOperationException)
            {
                _logger.LogError(ex, "Could not upload file {BlobName}.", blobName);
                return null;
            }
        }

        /// <inheritdoc/>
        public string GetReadUrl(string blobName)
        {
            var accountName = Configurations.GetStorageAccountName();
            var containerName = Configurations.GetStorageContainerName();
            var sas = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
                ExpiresOn = DateTimeOffset.UtcNow.Add(ReadLinkLifetime),
            };
            sas.SetPermissions(BlobSasPermissions.Read);
            var query = sas.ToSasQueryParameters(new StorageSharedKeyCredential(accountName, Configurations.GetStorageAccountKey())).ToString();

            return new UriBuilder
            {
                Scheme = "https",
                Host = $"{accountName}.blob.core.windows.net",
                Path = $"{containerName}/{blobName}",
                Query = query,
            }.Uri.ToString();
        }
    }
}
