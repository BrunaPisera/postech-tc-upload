using Upload.Domain.Interfaces;
using Pedidos.Infrastructure.Broker;

namespace Upload.Infrastructure.MessageBroker
{
    public class RabbitMQMessageBroker : IMessageBroker
    {
        private readonly IBrokerPublisher _brokerPublisher;
        private const string Exchange = "videoOperations";

        public RabbitMQMessageBroker(IBrokerPublisher brokerPublisher)
        {
            _brokerPublisher = brokerPublisher;
        }

        public void PublishVideoUploaded(string videoId)
        {
            _brokerPublisher.PublishMessage(Exchange, videoId, "video.uploaded");
        }
    }
}
