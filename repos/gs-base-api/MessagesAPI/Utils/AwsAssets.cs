using Amazon.CloudFront;
using Amazon.S3.Model;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Amazon;
using Microsoft.AspNetCore.Http;

namespace MessagesAPI.Utils
{
    /// <summary>
    /// AWS Assets helper to manage files uploading.
    /// </summary>
    public class AwsAssets
    {
        /// <summary>
        /// physical path of the files stored in S3.
        /// </summary>
        public static string FolderPath => AppConfiguration.Configuration["AppSettings:aws_folder_path"];
        private readonly static string configBucket = AppConfiguration.Configuration["AppSettings:aws_bucket"];
        private readonly static string configDistribution = AppConfiguration.Configuration["AppSettings:aws_cloudfront_distribution"];
        private readonly static string configPrivateKey = AppConfiguration.Configuration["AppSettings:aws_private_key"];
        private readonly static string configPublicKey = AppConfiguration.Configuration["AppSettings:aws_cloudfront_public_key_id"];
        private readonly static string configAwsResourcePath = AppConfiguration.Configuration["AppSettings:aws_resource_path"];
        private readonly static string configAwsUrlDuration = AppConfiguration.Configuration["AppSettings:aws_url_duration"];
        private readonly static string configAwsLimitTo = AppConfiguration.Configuration["AppSettings:aws_limit_to"];
 
        /// <summary>
        /// Uploads a file to an Amazon S3 bucket asynchronously.
        /// </summary>
        /// <param name="keyName">The desired file name for the uploaded file with the path.</param>
        /// <param name="file">The file to upload as an <see cref="HttpPostedFile"/>.</param>
        /// <returns>The <see cref="PutObjectResponse"/> returned by Amazon S3.</returns>
        /// <exception cref="AmazonS3Exception"></exception>
        public static async Task<PutObjectResponse> WritingObjectToS3BucketAsync(string keyName, IFormFile file)
        {
            try
            {
                // Use IConfiguration if you want to remove ConfigurationManager later
                string bucketName = configBucket;
                using IAmazonS3 s3Client = new AmazonS3Client(RegionEndpoint.USEast1);

                using var stream = file.OpenReadStream();

                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    ContentType = file.ContentType,
                    InputStream = stream
                };

                PutObjectResponse response = await s3Client.PutObjectAsync(putRequest);
                return response;
            }
            catch (AmazonS3Exception e)
            {
                throw new AmazonS3Exception(e.Message, e);
            }
        }

        /// <summary>
        /// Generates a signed URL for accessing a CloudFront resource.
        /// </summary>
        /// <param name="fileName">The name of the file to generate a signed URL with the path.</param>
        /// <returns>The signed URL for accessing the specified CloudFront resource.</returns>
        /// <exception cref="AmazonCloudFrontException"></exception>
        public static string GetSignedUrlFronCloudFront(string fileName)
        {
            return nameof(NotImplementedException);
            /*
             * TODO: Change this to Azure
            try
            { 
                string cloudFrontDistribution = configDistribution;
                string resourcePath = configAwsResourcePath;
                double timeoutDuration = Convert.ToDouble(configAwsUrlDuration);
                string limitToIP = configAwsLimitTo;
                string cloudFrontPublicKeyId = configPublicKey;

                string policyDoc = AmazonCloudFrontUrlSigner.BuildPolicyForSignedUrl(
                    $"{cloudFrontDistribution}{resourcePath}",
                    DateTime.UtcNow.AddHours(timeoutDuration),
                    limitToIP
                );

                string signedUrl = AmazonCloudFrontUrlSigner.SignUrl(
                    $"{cloudFrontDistribution}{fileName}",
                    cloudFrontPublicKeyId,
                    new StreamReader(configPrivateKey),
                    policyDoc
                );

                return signedUrl;
            }
            catch (AmazonCloudFrontException e)
            {
                throw new AmazonCloudFrontException(e.Message, e);
            } */
        }
    }
}