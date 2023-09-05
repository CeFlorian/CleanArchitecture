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

            services.Configure<RabbitMQSettingsProducer>(configuration.GetRequiredSection(rabbitMQSettingsName));
            var rabbitMQSettings = configuration.GetRequiredSection(rabbitMQSettingsName).Get<RabbitMQSettingsProducer>();

            //Domain Bus
            services.AddTransient<IEventBusProducer, RabbitMQBusProducer>(sp =>
            {
                return new RabbitMQBusProducer(rabbitMQSettings, sp.GetRequiredService<ILogger<RabbitMQBusProducer>>(), sp.GetRequiredService<IRabbitMqProducerConnectionService>());
            });

            services.AddTransient<IRabbitMqProducerConnectionService, RabbitMqProducerConnectionService>();

            //services.AddTransient<IEventBusProducer, RabbitMQBusProducer>();

            //Subscriptions
            //services.AddTransient<OrderCreatedEventHandler>();

            return services;
        }
    }
}
