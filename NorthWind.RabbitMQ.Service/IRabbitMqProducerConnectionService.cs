using RabbitMQ.Client;

namespace NorthWind.RabbitMQProducer.Services
{
    public interface IRabbitMqProducerConnectionService
    {
        IConnection CreateConnection();
    }
}
