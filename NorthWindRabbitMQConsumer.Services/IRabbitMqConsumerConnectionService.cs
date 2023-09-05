using RabbitMQ.Client;

namespace NorthWindRabbitMQConsumer.Services
{
    public interface IRabbitMqConsumerConnectionService
    {
        IConnection CreateConnection();

    }
}
