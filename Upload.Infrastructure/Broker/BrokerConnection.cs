using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Pedidos.Infrastructure.Broker
{
    public interface IBrokerConnection
    {
        IModel CreateChannel();
        void Dispose();
    }

    public class BrokerConnection : IBrokerConnection
    {
        private readonly IConnection _connection;

        public BrokerConnection(IConfiguration configuration)
        {
            var hostName = configuration["Broker:HostName"];
            var portString = configuration["Broker:Port"];
            var userName = configuration["Broker:UserName"];
            var password = configuration["Broker:Password"];
            var virtualHost = configuration["Broker:VirtualHost"];

            if (!int.TryParse(portString, out var port))
            {
                throw new ArgumentException("Porta inválida ou não definida.");
            }

            var factory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password,
                VirtualHost = virtualHost
            };

            _connection = factory.CreateConnection();
        }

        public IModel CreateChannel()
        {
            return _connection.CreateModel();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
