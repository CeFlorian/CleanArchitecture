namespace NorthWindRabbitMQConsumer.Services
{
    public class RabbitMQSettingsConsumer
    {
        public string ConsumerConnectionName { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; } = 5672;
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public string QueueName { get; set; }
        public string RoutingKey { get; set; }
        public string Relativekey { get; set; }
    }
}
