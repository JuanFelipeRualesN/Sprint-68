using System;
using System.Configuration;
using System.Web;
using Amazon;
using Amazon.CloudFront;
using Amazon.S3.Model;
using Amazon.S3;
using System.Threading.Tasks;
using System.IO;

namespace GSWebAPI.GSWebAPI.Services.Utils
{
    /// <Sumary>
    /// AWS Assets helper to manage files uploads with shared access signature.
    /// Created by Gu. Perez Sept 2023, based on Gl. Perez
    /*
    public class AwsAssets
    {
        /// <summary> 
        /// physical path of the files stored  in S3
        /// </summary>
        public static string FolderPath => AppConfiguration.Configuration["AppSettings:gs_web_aws_folder_path"];

        /// <summary>
        /// Uploads a file to an Amazon S3 bucket asynchronously.
        /// </summary>
        /// <param name="keyName">The desired file name for the uploaded file with the path.</param>
        /// <param name="file">The file to upload as an <see cref="Stream"/>.</param>
        /// <param name="contentType">The content type of the sent file.</param>
        /// <returns>The <see cref="PutObjectResponse"/> returned by Amazon S3.</returns>
        /// <exception cref="AmazonS3Exception"></exception>
        public static async Task<PutObjectResponse> WritingObjectToS3BucketAsync(string keyName, MemoryStream file, string contentType)
        {
            try
            {
                IAmazonS3 s3Client = new AmazonS3Client(RegionEndpoint.USEast1);
                string bucketName = AppConfiguration.Configuration["AppSettings:gs_web_aws_bucket"];

                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    ContentType = contentType,
                    InputStream = file,
                };

                PutObjectResponse response = await s3Client.PutObjectAsync(putRequest);

                return response;
            }
            catch (AmazonS3Exception e)
            {
                throw new AmazonS3Exception(e.Message);
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
            try
            {
                string cloudFrontDistribution = AppConfiguration.Configuration["AppSettings:gs_web_aws_cloudfront_distribution"];
                string resourcePath = AppConfiguration.Configuration["AppSettings:gs_web_aws_resource_path"];
                double timeoutDuration = Convert.ToDouble(AppConfiguration.Configuration["AppSettings:gs_web_aws_url_duration"]);
                string limitToIP = AppConfiguration.Configuration["AppSettings:gs_web_aws_limit_to"];
                string cloudFrontPublicKeyId = AppConfiguration.Configuration["AppSettings:gs_web_aws_cloudfront_public_key_id"];
                string path = HttpContext.Current.Server.MapPath(AppConfiguration.Configuration["AppSettings:gs_web_aws_private_key_path_file"]);

                string policyDoc = AmazonCloudFrontUrlSigner.BuildPolicyForSignedUrl(
                    $"{cloudFrontDistribution}{resourcePath}",
                    DateTime.Today.AddHours(timeoutDuration),
                    limitToIP);

                string signurl = AmazonCloudFrontUrlSigner.SignUrl(
                    $"{cloudFrontDistribution}{fileName}",
                    cloudFrontPublicKeyId,
                    new StreamReader(path),
                    policyDoc
                );

                return signurl;
            }
            catch (AmazonCloudFrontException e)
            {
                throw new AmazonCloudFrontException(e.Message);
            }
        }
    } */
}