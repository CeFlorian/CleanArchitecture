using Microsoft.Extensions.Hosting;
using NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus;
using NorthWind.Sales.BusinessObjects.POCOEntities;
using NorthWind.Sales.UseCases.CreateOrder;

namespace NorthWindRabbitMQConsumer.Services
{
    public class ConsumerHostedService : BackgroundService
    {
        private readonly IEventBusConsumer EventBus;

        public ConsumerHostedService(IEventBusConsumer eventBus)
        {
            EventBus = eventBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await EventBus.Subscribe<OrderCreatedEvent, OrderCreatedEventHandler>();
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await EventBus.Unsubscribe<OrderCreatedEvent, OrderCreatedEventHandler>();
            EventBus.Dispose(); // Cierra conexiones y libera recursos de RabbitMQ
        }
    }
}
