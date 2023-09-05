using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus;
using NorthWind.Sales.UseCases.CreateOrder;

namespace NorthWindRabbitMQConsumer.Services
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddConsumerServices(
            this IServiceCollection services,
            IConfiguration configuration, string rabbitMQSettingsName)
        {

            services.Configure<RabbitMQSettingsConsumer>(configuration.GetRequiredSection(rabbitMQSettingsName));
            var rabbitMQSettings = configuration.GetRequiredSection(rabbitMQSettingsName).Get<RabbitMQSettingsConsumer>();

            //Domain Bus
            services.AddSingleton<IEventBusConsumer, RabbitMQBusConsumer>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBusConsumer(scopeFactory, rabbitMQSettings, sp.GetRequiredService<ILogger<RabbitMQBusConsumer>>(), sp.GetRequiredService<IRabbitMqConsumerConnectionService>());
            });

            //Subscriptions
            services.AddTransient<OrderCreatedEventHandler>();


            services.AddSingleton<IRabbitMqConsumerConnectionService, RabbitMqConsumerConnectionService>();


            return services;
        }
    }
}
