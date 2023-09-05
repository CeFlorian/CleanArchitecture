using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace NorthWindRabbitMQConsumer.Services
{
    public class RabbitMqConsumerConnectionService : IRabbitMqConsumerConnectionService
    {
        private readonly RabbitMQSettingsConsumer Settings;
        public RabbitMqConsumerConnectionService(IOptions<RabbitMQSettingsConsumer> options)
        {
            Settings = options.Value;
        }
        public IConnection CreateConnection()
        {
            ConnectionFactory connection = new ConnectionFactory()
            {
                HostName = Settings.HostName,
                Port = Convert.ToInt32(Settings.Port),
                UserName = Settings.UserName,
                Password = Settings.Password,
                DispatchConsumersAsync = true
            };

            var channel = connection.CreateConnection(Settings.ConsumerConnectionName);
            return channel;
        }
    }
}
