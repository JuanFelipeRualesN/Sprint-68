using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.CloudFront;
using Amazon.S3;
using Amazon.S3.Model;
using CacheHandlerLibrary;

namespace TeachersAPI.Utils
{
    /// <summary>
    /// AWS Assets helper to manage files uploading.
    /// </summary>
    public class AwsAssets
    {
        /// <summary>
        /// Physical path prefix for files stored in S3 (key prefix).
        /// </summary>
        public static string FolderPath => AppConfiguration.Configuration["AppSettings:aws_folder_path"];

        /// <summary>
        /// Uploads a file to an Amazon S3 bucket asynchronously.
        /// </summary>
        public static async Task<PutObjectResponse> WritingObjectToS3BucketAsync(string keyName, Stream file, string contentType)
        {
            try
            {
                using var s3Client = new AmazonS3Client(RegionEndpoint.USEast1);
                var bucketName = AppConfiguration.Configuration["AppSettings:aws_bucket"];

                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    ContentType = contentType,
                    InputStream = file,
                };

                return await s3Client.PutObjectAsync(putRequest);
            }
            catch (AmazonS3Exception e)
            {
                throw new AmazonS3Exception(e.Message);
            }
        }

        /// <summary>
        /// Generates a signed URL for accessing a CloudFront resource.
        /// </summary>
        public static string GetSignedUrlFronCloudFront(string fileName)
        {
            try
            {
                var cloudFrontDistribution = AppConfiguration.Configuration["AppSettings:aws_cloudfront_distribution"]; // e.g. https://dxxxx.cloudfront.net/
                var resourcePath = AppConfiguration.Configuration["AppSettings:aws_resource_path"];                   // e.g. * or a path prefix
                var timeoutHours = Convert.ToDouble(AppConfiguration.Configuration["AppSettings:aws_url_duration"]);
                var limitToIP = AppConfiguration.Configuration["AppSettings:aws_limit_to"];                           // optional IP (can be empty)
                var cloudFrontPublicKeyId = AppConfiguration.Configuration["AppSettings:aws_cloudfront_public_key_id"];
                var privateKeyPathCfg = AppConfiguration.Configuration["AppSettings:aws_private_key_path_file"];      // relative or absolute

                // Resolve private key path for .NET 8 (no HttpContext.MapPath)
                var privateKeyPath = ResolvePath(privateKeyPathCfg);

                var expiresOn = DateTime.UtcNow.AddHours(timeoutHours);

                // Build a custom policy (wildcard/path support)
                var policyDoc = Amazon.CloudFront.AmazonCloudFrontUrlSigner.BuildPolicyForSignedUrl(
                    $"{cloudFrontDistribution}{resourcePath}",
                    expiresOn,
                    limitToIP);

                // Sign the actual file URL
                using var reader = File.OpenText(privateKeyPath);
                var signedUrl = Amazon.CloudFront.AmazonCloudFrontUrlSigner.SignUrl(
                    $"{cloudFrontDistribution}{fileName}",
                    cloudFrontPublicKeyId,
                    reader,
                    policyDoc);

                return signedUrl;
            }
            catch (AmazonCloudFrontException e)
            {
                throw new AmazonCloudFrontException(e.Message);
            }
        }

        /// <summary>
        /// Delete a file in an Amazon S3 bucket asynchronously.
        /// </summary>
        public static async Task<DeleteObjectResponse> DeleteObjectIntoS3BucketAsync(string keyName)
        {
            try
            {
                using var s3Client = new AmazonS3Client(RegionEndpoint.USEast1);
                var bucketName = AppConfiguration.Configuration["AppSettings:aws_bucket"];
                return await s3Client.DeleteObjectAsync(bucketName, keyName);
            }
            catch (AmazonS3Exception e)
            {
                throw new AmazonS3Exception(e.Message);
            }
        }

        /// <summary>
        /// Resolves a relative/virtual path (e.g., "~/" or relative) to an absolute filesystem path for .NET 8.
        /// </summary>
        private static string ResolvePath(string configuredPath)
        {
            if (string.IsNullOrWhiteSpace(configuredPath))
                throw new ArgumentException("Private key path is not configured.", nameof(configuredPath));

            if (Path.IsPathRooted(configuredPath))
                return configuredPath;

            // Trim common virtual path prefixes and normalize separators
            var trimmed = configuredPath.TrimStart('~', '/', '\\')
                                        .Replace('/', Path.DirectorySeparatorChar)
                                        .Replace('\\', Path.DirectorySeparatorChar);

            return Path.Combine(AppContext.BaseDirectory, trimmed);
        }
    }
}
