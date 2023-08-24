using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus;

namespace NorthWind.RabbitMQ.Service
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddBusServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IEventBus, RabbitMqBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMqBus(scopeFactory, configuration.GetSection("MessageBroker:Host").Value, sp.GetService<ILogger<RabbitMqBus>>());
            });

            return services;
        }
    }
}
