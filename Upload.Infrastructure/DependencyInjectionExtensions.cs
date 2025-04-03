using Microsoft.Extensions.DependencyInjection;
using Pedidos.Infrastructure.Broker;

namespace Upload.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IBrokerConnection, BrokerConnection>();
            services.AddScoped<IBrokerPublisher, BrokerPublisher>();

            return services;
        }
    }
}
