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

            services.Configure<RabbitMQSettingsConsumer>(configuration.GetSection(rabbitMQSettingsName));
            var rabbitMQSettings = configuration.GetSection(rabbitMQSettingsName).Get<RabbitMQSettingsConsumer>();

            //Domain Bus
            services.AddTransient<IEventBusConsumer, RabbitMQBusConsumer>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBusConsumer(scopeFactory, rabbitMQSettings, sp.GetService<ILogger<RabbitMQBusConsumer>>());
            });

            //Subscriptions
            services.AddTransient<OrderCreatedEventHandler>();

            return services;
        }
    }
}
