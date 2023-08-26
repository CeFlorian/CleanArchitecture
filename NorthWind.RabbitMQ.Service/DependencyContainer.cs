using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus;

namespace NorthWind.RabbitMQProducer.Services
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddProducerServices(
            this IServiceCollection services,
            IConfiguration configuration, string rabbitMQSettingsName)
        {

            services.Configure<RabbitMQSettingsProducer>(configuration.GetSection(rabbitMQSettingsName));
            var rabbitMQSettings = configuration.GetSection(rabbitMQSettingsName).Get<RabbitMQSettingsProducer>();

            //Domain Bus
            services.AddTransient<IEventBusProducer, RabbitMQBusProducer>(sp =>
            {
                return new RabbitMQBusProducer(rabbitMQSettings, sp.GetService<ILogger<RabbitMQBusProducer>>());
            });

            //services.AddTransient<IEventBusProducer, RabbitMQBusProducer>();

            //Subscriptions
            //services.AddTransient<OrderCreatedEventHandler>();

            return services;
        }
    }
}
