namespace Upload.Domain.Interfaces
{
    public interface IMessageBroker
    {
        void PublishVideoUploaded(string videoId);
    }
}
