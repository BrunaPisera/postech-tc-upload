using Amazon.S3.Transfer;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Amazon;
using Pedidos.Infrastructure.Broker;

namespace Upload.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly ILogger<UploadController> _logger;
        private readonly IBrokerPublisher BrokerPublisher;
        private readonly IConfiguration _configuration;
        private readonly string Exchange = "videoOperations";

        public UploadController(ILogger<UploadController> logger, IBrokerPublisher brokerPublisher, IConfiguration configuration)
        {
            _logger = logger;
            BrokerPublisher = brokerPublisher;
            _configuration = configuration;
        }
    
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

                var videoFileName = Guid.NewGuid() + Path.GetExtension(x.FileName).ToLower();
             
                await UploadFileToS3(x, videoFileName);

                BrokerPublisher.PublishMessage(Exchange, videoFileName, "video.uploaded");
            });

            if (error)
            {
                return BadRequest($"The file is not a valid video.");
            }

            return Ok(new { Message = "Video uploaded successfully." });
        }

        private async Task UploadFileToS3(IFormFile file, string videoFileName)
        {
            using (var client = new AmazonS3Client(_configuration["Aws:AwsAccessKeyId"],
                                                _configuration["Aws:AwsSecretAccessKey"],
                                                _configuration["Aws:AwsSessionToken"],
                                                RegionEndpoint.USEast1))
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = $"videos/{videoFileName}",
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
