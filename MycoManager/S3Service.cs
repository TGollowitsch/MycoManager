using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace MycoManager
{
    public class S3Service
    {
        private const string bucketName = "myco-manager-bucket";
        private static readonly AmazonS3Client s3Client = new();

        public static async Task<bool> UploadFile(Stream imageStream, string objectKey)
        {
            try
            {
                // Upload file to S3
                var fileTransferUtility = new TransferUtility(s3Client);
                await fileTransferUtility.UploadAsync(imageStream, bucketName, objectKey);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine("Error uploading file to S3: " + ex.Message);
                return false;
            }
            return true;
        }

        public static async Task<bool> DeleteObject(string objectKey)
        {
            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = objectKey
                };

                // Delete the object from the S3 bucket
                var response = await s3Client.DeleteObjectAsync(request);

                // Check if the deletion was successful
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (AmazonS3Exception ex)
            {
                // Handle the exception
                Console.WriteLine("Error deleting object from S3: " + ex.Message);
                return false;
            }
        }

        public static async Task<string> GetS3SignedUrl(string objectKey)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = objectKey,
                Expires = DateTime.UtcNow.AddHours(1) // Expiration time for the URL
            };
            
            try
            {
                string url = await s3Client.GetPreSignedURLAsync(request);
                return url;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine("Error generating PreSigned url: " + ex.Message);
                return null;
            }
        }

        public static async Task<GetObjectResponse> GetS3Object(string objectKey)
        {
            var request = new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = objectKey
            };

            try
            {
                var s3Object = await s3Client.GetObjectAsync(request);
                return s3Object;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine("Error generating PreSigned url: " + ex.Message);
                return null;
            }
        }
    }
}
