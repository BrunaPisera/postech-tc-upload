using Microsoft.AspNetCore.Http;
using Moq;
using Upload.Application.UseCases.UploadVideo;
using Upload.Domain.Interfaces;

namespace Upload.Application.Tests
{
    [TestFixture]
    public class UploadVideoUseCaseTests
    {
        private Mock<IVideoStorage> _videoStorageMock;
        private Mock<IMessageBroker> _messageBrokerMock;
        private UploadVideoUseCase _uploadVideoUseCase;

        [SetUp]
        public void SetUp()
        {
            _videoStorageMock = new Mock<IVideoStorage>();
            _messageBrokerMock = new Mock<IMessageBroker>();

            _uploadVideoUseCase = new UploadVideoUseCase(_videoStorageMock.Object, _messageBrokerMock.Object);
        }

        private IFormFile CreateFakeFormFile(string fileName, string contentType = "video/mp4")
        {
            var content = "dummy content";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            return new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        [Test]
        public async Task ExecuteAsync_WithInvalidVideoExtension_ReturnsInvalidResult()
        {
            var video = CreateFakeFormFile("video.xyz");
            var useCase = _uploadVideoUseCase;

            var result = await useCase.ExecuteAsync(video);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Invalid video format", result.Message);
        }

        [Test]
        public async Task ExecuteAsync_WithValidVideoExtension_ReturnsSuccessAndCallsDependencies()
        {
            var video = CreateFakeFormFile("video.mp4");  // formato válido

            // Mockar o comportamento do vídeoStorage e messageBroker
            _videoStorageMock
                .Setup(vs => vs.StoreAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);  // Simulando sucesso no armazenamento

            _messageBrokerMock
                .Setup(mb => mb.PublishVideoUploaded(It.IsAny<string>()))
                .Verifiable();  // Verifica se o método PublishVideoUploaded é chamado

            // Act
            var result = await _uploadVideoUseCase.ExecuteAsync(video);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.That(Guid.TryParse(result.VideoId, out _), Is.True);  // Verifica se o VideoId é um UUID válido

            // Verificar se os métodos mockados foram chamados corretamente
            _videoStorageMock.Verify(vs => vs.StoreAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Once);
            _messageBrokerMock.Verify(mb => mb.PublishVideoUploaded(It.IsAny<string>()), Times.Once);
        }

    }
}