using Upload.Domain.Entities;

namespace Upload.Domain.Tests
{
    [TestFixture]
    public class VideoTests
    {
        [Test]
        public void Create_WithValidFileNameAndExtension_ShouldCreateVideo()
        {
            var fileName = "video_teste";
            var extension = ".mp4";

            var video = Video.Create(fileName, extension);

            Assert.IsNotNull(video);
            Assert.AreEqual(fileName, video.FileName);
            Assert.AreEqual(extension.ToLower(), video.Extension);
            Assert.IsFalse(string.IsNullOrEmpty(video.Id));
        }

        [TestCase(null)]
        [TestCase("")]
        public void Create_WithInvalidFileName_ShouldThrowArgumentException(string invalidFileName)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                Video.Create(invalidFileName, ".mp4"));

            Assert.That(exception.ParamName, Is.EqualTo("fileName"));
        }

        [TestCase(null)]
        [TestCase("")]
        public void Create_WithInvalidExtension_ShouldThrowArgumentException(string invalidExtension)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                Video.Create("video_teste", invalidExtension));

            Assert.That(exception.ParamName, Is.EqualTo("extension"));
        }

        [TestCase(".mp4", true)]
        [TestCase(".avi", true)]
        [TestCase(".mkv", true)]
        [TestCase(".mov", true)]
        [TestCase(".webm", true)]
        [TestCase(".MP4", true)] 
        [TestCase(".exe", false)]
        [TestCase(".txt", false)]
        public void HasValidExtension_ShouldReturnExpectedResult(string extension, bool expected)
        {
            var video = Video.Create("video_teste", extension);
            var result = video.HasValidExtension();

            Assert.AreEqual(expected, result);
        }
    }
}