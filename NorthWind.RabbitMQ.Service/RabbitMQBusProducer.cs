using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus;
using NorthWind.Sales.BusinessObjects.POCOEntities.Events;
using RabbitMQ.Client;
using System.Text;

namespace NorthWind.RabbitMQProducer.Services
{
    public class RabbitMQBusProducer : IEventBusProducer
    {
        readonly ILogger<RabbitMQBusProducer> Logger;
        readonly RabbitMQSettingsProducer Settings;

        public RabbitMQBusProducer(RabbitMQSettingsProducer settings, ILogger<RabbitMQBusProducer> logger)
        {
            Settings = settings;
            Logger = logger;
        }

        public async Task Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = Settings.HostName,
                UserName = Settings.UserName,
                Password = Settings.Password,
            };

            // Ya que solo se publica se uiliza "using" para que se liberen los recurso de connection y channel
            using var connection = factory.CreateConnection(Settings.ProducerConnectionName);
            using var channel = connection.CreateModel();
            var eventName = @event.GetType().Name;
            //channel.QueueDeclare(eventName, false, false, false, null);

            channel.ExchangeDeclare(exchange: Settings.ExchangeName, type: Settings.ExchangeType);

            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2; // persistent

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);
            //channel.BasicPublish("", eventName, null, body);

            channel.BasicPublish(Settings.ExchangeName, Settings.RoutingKey, properties, body);
            Logger.LogInformation("Message produced");

        }

    }
}