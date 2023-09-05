using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace NorthWind.RabbitMQProducer.Services
{
    public class RabbitMqProducerConnectionService : IRabbitMqProducerConnectionService
    {
        private readonly RabbitMQSettingsProducer Settings;
        public RabbitMqProducerConnectionService(IOptions<RabbitMQSettingsProducer> options)
        {
            Settings = options.Value;
        }
        public IConnection CreateConnection()
        {
            ConnectionFactory connection = new ConnectionFactory()
            {
                HostName = Settings.HostName,
                Port = Settings.Port,
                UserName = Settings.UserName,
                Password = Settings.Password,
            };

            var channel = connection.CreateConnection();
            return channel;
        }
    }
}
