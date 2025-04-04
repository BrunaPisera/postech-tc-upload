using RabbitMQ.Client;
using System.Text;

namespace Pedidos.Infrastructure.Broker
{
    public interface IBrokerPublisher
    {
        void PublishMessage(string exchange, string message);
    }

    public class BrokerPublisher : IBrokerPublisher
    {
        IBrokerConnection _brokerConnection;
        public BrokerPublisher(IBrokerConnection brokerConnection)
        {
            _brokerConnection = brokerConnection;
        }

        public void PublishMessage(string exchange, string message)
        {
            using (var channel = _brokerConnection.CreateChannel())
            {
                channel.ExchangeDeclare(exchange: exchange,
                                         type: "fanout",
                                         durable: true,
                                         autoDelete: false);
                
                var body = Encoding.UTF8.GetBytes(message);

                channel.QueueDeclare("videoToProcess", true, false, false, null);

                channel.QueueBind(queue: "videoToProcess",
                                    exchange: exchange,
                                    routingKey: "");

                channel.BasicPublish(exchange: exchange,
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);               
            }
        }
    }
}
