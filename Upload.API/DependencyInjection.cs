using Upload.Application.UseCases.UploadVideo;
using Upload.Domain.Interfaces;
using Upload.Infrastructure.MessageBroker;
using Upload.Infrastructure.Storage;
using Pedidos.Infrastructure.Broker;

namespace Upload.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUploadServices(this IServiceCollection services)
        {
            // Application Layer
services.AddScoped<IUploadVideoUseCase, UploadVideoUseCase>();

            // Infrastructure Layer
            services.AddScoped<IVideoStorage, S3VideoStorage>();
            services.AddScoped<IMessageBroker, RabbitMQMessageBroker>();
            services.AddScoped<IBrokerConnection, BrokerConnection>();
            services.AddScoped<IBrokerPublisher, BrokerPublisher>();

            return services;
        }
    }
}
