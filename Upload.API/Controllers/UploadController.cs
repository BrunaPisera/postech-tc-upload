using Microsoft.AspNetCore.Mvc;
using Upload.Application.UseCases.UploadVideo;

namespace Upload.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly ILogger<UploadController> _logger;
        private readonly IUploadVideoUseCase _uploadVideoUseCase;

        public UploadController(
            ILogger<UploadController> logger,
            IUploadVideoUseCase uploadVideoUseCase)
        {
            _logger = logger;
            _uploadVideoUseCase = uploadVideoUseCase;
        }
    
        [HttpPost]
        public async Task<IActionResult> UploadVideo(List<IFormFile> videos)
        {
            if (videos.Count > 3)
                return BadRequest("Maximum of 3 videos allowed");

            var results = new List<UploadVideoResult>();
            
            foreach (var video in videos)
            {
                var result = await _uploadVideoUseCase.ExecuteAsync(video);
                results.Add(result);
            }

            if (results.Any(r => !r.IsSuccess))
            {
                return BadRequest(results.First(r => !r.IsSuccess).Message);
            }

            return Ok(new { VideoIds = results.Select(r => r.VideoId).ToList() });
        }
    }
}
