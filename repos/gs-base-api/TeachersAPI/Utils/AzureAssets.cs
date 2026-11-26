using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace TeachersAPI.Utils
{
    public class AzureAssets
    {
        /// <sumary>
        /// define the connection string, account name, account key and container name
        /// </sumary>
        static string connectionString = AppConfiguration.Configuration["AppSettings:AzureStorageConnectionString"];

        static string accountName = AppConfiguration.Configuration["AppSettings:azure_storage_account_name"];

        static string accountKey = AppConfiguration.Configuration["AppSettings:AzureStorageAccountKey"];

        static string containerName = AppConfiguration.Configuration["AppSettings:azure_storage_container_name_teachers"];

        /// <sumary>
        /// Upload file to azure blob storage with shared access signature
        /// </sumary>
        /// <param name="file">The file to upload as an <see cref="Stream"/>.</param>
        /// <param name="newFileName">The new file name</param>
        /// <returns>Return the url with the sas token</returns>
        public static async Task<string> UploadFileToAzureStorage(Stream file, string newFileName)
        {

            try
            {
                // Create a BlobServiceClient object which will be used to create a container client
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                // Create the container and return a container client object
                BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

                // Create a sasBuilder for share access signature with 1 day
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = containerName,
                    BlobName = newFileName,
                    StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                    ExpiresOn = DateTimeOffset.UtcNow.AddDays(1),

                };

                // set permissions to write and read because we need the url for return

                sasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Write);

                // Create a SharedKeyCredential that we can use to sign the SAS token
                var storageSharedKeyCredential = new StorageSharedKeyCredential(accountName, accountKey);

                // Use the key to get the SAS token.
                BlobSasQueryParameters sasQueryParameters = sasBuilder.ToSasQueryParameters(storageSharedKeyCredential);

                //  Builds the URI to the blob storage.
                UriBuilder fullUri = new UriBuilder()
                {
                    Scheme = "https",
                    Host = string.Format("{0}.blob.core.windows.net", accountName),
                    Path = string.Format("{0}/{1}", containerName, newFileName),
                    Query = sasQueryParameters.ToString()
                };


                // Get an instance of BlobClient using the URI.
                var blobClient = new BlobClient(fullUri.Uri, null);


                // Upload stuff in the blob.
                var response = await blobClient.UploadAsync(file, overwrite: true);

                // return the url with the sas token
                return fullUri.Uri.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <sumary>
        /// Get file from azure blob storage with shared access signature
        /// </sumary>
        /// <param name="filename"> The file name</param>
        /// <returns> Return the url with the sas token </returns>
        public static string GetFileFromAzureStorage(string filename)
        {

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

        /// <sumary>
        /// Delete file to azure blob storage with shared access signature
        /// </sumary>
        /// <param name="filename"> The file name</param>
        /// <returns> A task that represents the asynchronous operation. The task result contains a <see cref="bool"/> value that indicates if the file was deleted</returns>
        public static async Task<bool> DeleteFileFromAzureStorage(string filename)
        {
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

                // set permissions to delete because we need to delete the file
                sasBuilder.SetPermissions(BlobSasPermissions.Delete);

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

                // get an instance of BlobClient using the URI
                var blobClient = new BlobClient(fullUri.Uri, null);

                // delete stuff in the blob
                var response = await blobClient.DeleteIfExistsAsync();

                // return the url with the sas token
                return response.Value;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
