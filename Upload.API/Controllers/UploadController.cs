using Amazon.S3.Transfer;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Amazon;

namespace Upload.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly ILogger<UploadController> _logger;

        [HttpPost]
        public async Task<IActionResult> UploadVideo(List<IFormFile> video)
        {
            if (video.Count > 3)
                return BadRequest();

            var error = false;
            video.ForEach(async x =>
            {
                if (!HasValidVideoExtension(x))
                {
                    error = true;
                    return;
                }

                UploadFileToS3(x);
            });

            if (error)
            {
                return BadRequest($"The file is not a valid video.");
            }

            return Ok(new { Message = "Video uploaded successfully." });
        }

        private async Task UploadFileToS3(IFormFile file)
        {
            using (var client = new AmazonS3Client("yourAwsAccessKeyId", "yourAwsSecretAccessKey", "acessToken", RegionEndpoint.USEast1))
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = file.FileName,
                        BucketName = "videouploadtc",
                        CannedACL = S3CannedACL.Private
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }
            }
        }

        private bool HasValidVideoExtension(IFormFile file)
        {
            var allowedExtensions = new HashSet<string> { ".mp4", ".avi", ".mkv", ".mov", ".webm" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(extension);
        }
    }
}
