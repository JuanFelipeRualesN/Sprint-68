using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;

namespace MessagesAPI.Utils
{
    /// <sumary>
    /// Azure Assets helper to manage files uploads with shared access signature.
    /// Created by Gu.Perez Sept 2023.
    /// </sumary
    public class AzureAssets
    {
        /// <sumary>
        /// Uploads file to azure blob storage with shared access signature.
        /// </sumary>
        /// <param name="file">The file to upload.</param>
        /// <param name="newFileName">The new file name.</param>
        /// <returns>Returns the url with the sas token.</returns>
        public static async Task<string> UploadFileToAzureStorage(IFormFile file, string newFileName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            // Settings for Azure (adapt to your configuration source)
            string connectionString = AppConfiguration.Configuration["AppSettings:AzureStorageConnectionString"];
            string accountName = AppConfiguration.Configuration["AppSettings:azure_storage_account_name"];
            string accountKey = AppConfiguration.Configuration["AppSettings:AzureStorageAccountKey"];
            string containerName = AppConfiguration.Configuration["AppSettings:azure_storage_container_name_message"];
            
            try
            {
                // Create service & container clients
                var blobServiceClient = new BlobServiceClient(connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Ensure container exists (optional)
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

                // Create a blob client for the target name
                var blobClient = containerClient.GetBlobClient(newFileName);

                // Upload with content type and overwrite if exists
                var httpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType ?? "application/octet-stream"
                };

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = httpHeaders, TransferOptions = new StorageTransferOptions() }, cancellationToken: default);
                }

                // Build a READ-ONLY SAS URL valid for 1 day (adjust as needed)
                var sasBuilder = new BlobSasBuilder
                {
                    BlobContainerName = containerName,
                    BlobName = newFileName,
                    Resource = "b", // blob
                    StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
                    ExpiresOn = DateTimeOffset.UtcNow.AddDays(1)
                };
                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                var cred = new StorageSharedKeyCredential(accountName, accountKey);
                var sas = sasBuilder.ToSasQueryParameters(cred).ToString();

                var sasUri = new UriBuilder(blobClient.Uri) { Query = sas }.Uri.ToString();
                return sasUri;
            }
            catch (Exception ex)
            {
                // bubble up with context
                throw new Exception($"Azure upload failed: {ex.Message};", ex);
            }
        }

        /// <sumary>
        /// Gets the file from azure blob storage with shared access signature.
        /// </sumary>
        /// <param name="filename"> The file name.</param>
        /// <returns> Return the url with the sas token.</returns>
        public static string GetFileFromAzureStorage(string filename)
        {

            string connectionString = AppConfiguration.Configuration["AppSettings:AzureStorageConnectionString"];
            string accountName = AppConfiguration.Configuration["AppSettings:azure_storage_account_name"];
            string accountKey = AppConfiguration.Configuration["AppSettings:AzureStorageAccountKey"];
            string containerName = AppConfiguration.Configuration["AppSettings:azure_storage_container_name_message"];

            try
            {
                // Create a BlobServiceClient object which will be used to create a container client
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                // create the cointaner and return a container client object
                BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

                // create a sasBuilder for share access signature with 1 day
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = containerName,
                    BlobName = filename,
                    StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                    ExpiresOn = DateTimeOffset.UtcNow.AddDays(1),

                };

                // set permissions to read because we need the url for return
                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                // create a SharedKeyCredential that we can use to sign the SAS token
                var storageSharedKeyCredential = new StorageSharedKeyCredential(accountName, accountKey);

                // use the key to get the SAS token
                BlobSasQueryParameters sasQueryParameters = sasBuilder.ToSasQueryParameters(storageSharedKeyCredential);

                // builds the URI to the blob storage
                UriBuilder fullUri = new UriBuilder()
                {
                    Scheme = "https",
                    Host = string.Format("{0}.blob.core.windows.net", accountName),
                    Path = string.Format("{0}/{1}", containerName, filename),
                    Query = sasQueryParameters.ToString()
                };

                //// get an instance of BlobClient using the URI
                //var blobClient = new BlobClient(fullUri.Uri, null);

                //// get stuff in the blob
                //var response = await blobClient.DownloadAsync();

                // return the url with the sas token
                return fullUri.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// To sign the azure blob URL (just the signing and nothing else).
        /// </summary>
        /// <param name="blobUrl">The blob URL.</param>
        /// <returns>Returns the signed blob.</returns>
        /// <exception cref="Exception"></exception>
        public static string SignAzureBlobUrl(string blobUrl)
        {
            string accountName = AppConfiguration.Configuration["AppSettings:azure_storage_account_name"];
            string accountKey = AppConfiguration.Configuration["AppSettings:AzureStorageAccountKey"];

            try
            {
                // Parse the blob URL
                var blobUriBuilder = new BlobUriBuilder(new Uri(blobUrl));

                // Create SAS builder (read-only, valid for 1 day)
                var sasBuilder = new BlobSasBuilder
                {
                    BlobContainerName = blobUriBuilder.BlobContainerName,
                    BlobName = blobUriBuilder.BlobName,
                    Resource = "b", // 'b' means blob
                    StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5), // Start 5 mins early to avoid clock skew
                    ExpiresOn = DateTimeOffset.UtcNow.AddDays(1)
                };

                // Grant read permission
                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                // Sign the SAS with your account credentials
                var sharedKeyCredential = new StorageSharedKeyCredential(accountName, accountKey);
                var sasQueryParameters = sasBuilder.ToSasQueryParameters(sharedKeyCredential);

                // Attach the SAS to the blob URL
                blobUriBuilder.Sas = sasQueryParameters;

                // Return the full signed URL
                return blobUriBuilder.ToUri().ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
