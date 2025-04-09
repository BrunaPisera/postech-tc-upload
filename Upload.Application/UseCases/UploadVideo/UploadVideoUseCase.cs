using Microsoft.AspNetCore.Http;
using Upload.Domain.Entities;
using Upload.Domain.Interfaces;

namespace Upload.Application.UseCases.UploadVideo
{
    public class UploadVideoUseCase
    {
        private readonly IVideoStorage _videoStorage;
        private readonly IMessageBroker _messageBroker;

        public UploadVideoUseCase(IVideoStorage videoStorage, IMessageBroker messageBroker)
        {
            _videoStorage = videoStorage;
            _messageBroker = messageBroker;
        }

        public async Task<UploadVideoResult> ExecuteAsync(IFormFile file)
        {
            var video = Video.Create(
                Path.GetFileNameWithoutExtension(file.FileName),
                Path.GetExtension(file.FileName)
            );

            if (!video.HasValidExtension())
                return UploadVideoResult.Invalid("Invalid video format");

            await _videoStorage.StoreAsync(file, video.Id + video.Extension);
            _messageBroker.PublishVideoUploaded(video.Id + video.Extension);

            return UploadVideoResult.Success(video.Id);
        }
    }

    public class UploadVideoResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public string VideoId { get; private set; }

        private UploadVideoResult(bool isSuccess, string message, string? videoId = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            VideoId = videoId;
        }

        public static UploadVideoResult Success(string videoId) =>
            new UploadVideoResult(true, "Video uploaded successfully", videoId);

        public static UploadVideoResult Invalid(string message) =>
            new UploadVideoResult(false, message);
    }
}
