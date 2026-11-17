using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using DataAnalysisAPI.Utils;
using Azure;

namespace DataAnalysisAPI.DataAnalysisAPI.Services.Utils
{
    /// <sumary>
    /// Azure Assets helper to manage files uploads with shared access signature.
    /// Created by Gu.Perez Sept 2023.
    /// </sumary>
    public static class AzureAssets
    {
        /// <sumary>
        /// Uploads file to azure blob storage with shared access signature.
        /// </sumary>
        /// <param name="file">The file to upload.</param>
        /// <param name="newFileName">The new file name.</param>\
        /// <param name="applyDelay">Optional: To apply delay. If it is NPOI workbook, set to true, because there is a problem with the stream that cannot be fixed by us.</param>
        /// <param name="expectedUploadSpeedBytesPerSecond">If apply delay is true, </param>
        /// <returns>Returns the url with the sas token.</returns>
        public static async Task<string> UploadFileToAzureStorage(MemoryStream file, string newFileName, bool applyDelay = false, double? expectedUploadSpeedBytesPerSecond = null)
        {
            try
            {
                UriBuilder fullUri = GetBlobStorageUri(newFileName);

                // Get an instance of BlobClient using the URI.
                var blobClient = new BlobClient(fullUri.Uri, null);

                //set the position to the start of the stream before uploading
                file.Position = 0;

                // Upload stuff in the blob.
                await blobClient.UploadAsync(file);

                file.Close();

                // return the url with the sas token
                return fullUri.Uri.ToString();
            }
            catch (RequestFailedException ex)
            {
                throw new RequestFailedException(ex.Message);
            }
        }

        /// <summary>
        /// Construct the Url with blob storage access specification. Original code created by Gl.Perez Sept.2023.
        /// </summary>
        /// <param name="fileName">The new file name.</param>
        /// <returns></returns>
        private static UriBuilder GetBlobStorageUri(string fileName)
        {
            // Create a sasBuilder for share access signature with 1 day
            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = Configurations.GetStorageContainerName(),
                BlobName = fileName,
                StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
            };

            // set permissions to write and read because we need the url for return
            sasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Write);

            // Create a SharedKeyCredential that we can use to sign the SAS token
            var storageSharedKeyCredential = new StorageSharedKeyCredential(Configurations.GetStorageAccountName(), Configurations.GetStorageAccountKey());

            // Use the key to get the SAS token.
            BlobSasQueryParameters sasQueryParameters = sasBuilder.ToSasQueryParameters(storageSharedKeyCredential);

            // Builds the URI to the blob storage.
            var fullUri = new UriBuilder()
            {
                Scheme = "https",
                Host = $"{Configurations.GetStorageAccountName()}.blob.core.windows.net",
                Path = $"{Configurations.GetStorageContainerName()}/{fileName}",
                Query = sasQueryParameters.ToString()
            };

            return fullUri;
        }

    }
}