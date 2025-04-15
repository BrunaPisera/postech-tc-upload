using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Upload.Domain.Interfaces;

namespace Upload.Infrastructure.Storage
{
    public class S3VideoStorage : IVideoStorage
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3VideoStorage(IConfiguration configuration)
        {
            _s3Client = new AmazonS3Client(
                configuration["Aws:AwsAccessKeyId"],
                configuration["Aws:AwsSecretAccessKey"],
                configuration["Aws:AwsSessionToken"],
                RegionEndpoint.USEast1
            );
            _bucketName = configuration["Aws:BucketName"] ?? throw new ArgumentNullException("Aws:BucketName", "S3 bucket name must be configured");
        }

        public async Task StoreAsync(IFormFile file, string fileName)
        {
            using var transferUtility = new TransferUtility(_s3Client);
            using var stream = file.OpenReadStream();
            
            await transferUtility.UploadAsync(stream, _bucketName, $"videos/{fileName}");
        }
    }
}
