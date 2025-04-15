using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Upload.API.Controllers;
using Upload.Application.UseCases.UploadVideo;

namespace Upload.API.Tests
{
    public class UploadControllerTests
    {
        private Mock<ILogger<UploadController>> _loggerMock;
        private Mock<IUploadVideoUseCase> _useCaseMock;
        private UploadController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<UploadController>>();
            _useCaseMock = new Mock<IUploadVideoUseCase>();

            _controller = new UploadController(_loggerMock.Object, _useCaseMock.Object);
        }

        private IFormFile CreateFakeFormFile(string fileName)
        {
            var content = "dummy content";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            return new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "video/mp4"
            };
        }

        [Test]
        public async Task UploadVideo_WithValidVideos_ReturnsOk()
        {
            var video1 = CreateFakeFormFile("video1.mp4");
            var video2 = CreateFakeFormFile("video2.mp4");

            _useCaseMock
                .Setup(x => x.ExecuteAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync((IFormFile file) =>
                    UploadVideoResult.Success(Path.GetFileNameWithoutExtension(file.FileName)));

            var videos = new List<IFormFile> { video1, video2 };

            var result = await _controller.UploadVideo(videos);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task UploadVideo_WithTooManyVideos_ReturnsBadRequest()
        {
            var videos = new List<IFormFile>
            {
                CreateFakeFormFile("v1.mp4"),
                CreateFakeFormFile("v2.mp4"),
                CreateFakeFormFile("v3.mp4"),
                CreateFakeFormFile("v4.mp4"),
            };

            var result = await _controller.UploadVideo(videos);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.AreEqual("Maximum of 3 videos allowed", badRequest.Value);
        }

        [Test]
        public async Task UploadVideo_WhenOneFails_ReturnsBadRequest()
        {
            var video1 = CreateFakeFormFile("ok.mp4");
            var video2 = CreateFakeFormFile("fail.mp4");

            _useCaseMock
                .SetupSequence(x => x.ExecuteAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync(UploadVideoResult.Success("ok-id"))
                .ReturnsAsync(UploadVideoResult.Invalid("invalid format"));

            var videos = new List<IFormFile> { video1, video2 };

            var result = await _controller.UploadVideo(videos);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.AreEqual("invalid format", badRequest.Value);
        }
    }
}