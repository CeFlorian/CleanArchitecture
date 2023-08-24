using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus;
using NorthWind.Sales.BusinessObjects.POCOEntities.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NorthWind.RabbitMQ.Service
{
    public sealed class RabbitMqBus : IEventBus
    {
        private readonly ILogger<RabbitMqBus> _logger;
        //private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _evenTypes;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _hostName;

        public RabbitMqBus(IServiceScopeFactory serviceScopeFactory, string hostName, ILogger<RabbitMqBus> logger)
        {
            //_mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _handlers = new Dictionary<string, List<Type>>();
            _evenTypes = new List<Type>();
            _hostName = hostName;
            _logger = logger;
        }

        //public Task SendCommand<T>(T command) where T : Command
        //{
        //    return _mediator.Send(command);
        //}

        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var eventName = @event.GetType().Name;
            channel.QueueDeclare(eventName, false, false, false, null);
            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", eventName, null, body);
        }

        public void Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!_evenTypes.Contains(typeof(T)))
            {
                _evenTypes.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(s => s == handlerType))
            {
                _logger.LogError($"Handler Type {handlerType.Name} already is registered for '{eventName}'", nameof(handlerType));
                throw new ArgumentException($"Handler Type {handlerType.Name} already is registered for '{eventName}'", nameof(handlerType));
            }

            _handlers[eventName].Add(handlerType);

            StartBasicConsumer<T>();

        }

        private void StartBasicConsumer<T>() where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = typeof(T).Name;
            channel.QueueDeclare(eventName, false, false, false, null);

            channel.BasicQos(0, 1, false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            channel.BasicConsume(eventName, false, consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.Span);

            try
            {
                var result = await ProcessEvent(eventName, message).ConfigureAwait(false);

                if (result)
                {
                    ((AsyncDefaultBasicConsumer)sender).Model.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
                }
                else
                {
                    ((AsyncDefaultBasicConsumer)sender).Model.BasicNack(deliveryTag: e.DeliveryTag, multiple: false, requeue: true);

                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Something went wrong with Consumer_Received!");
            }
        }

        private async Task<bool> ProcessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _handlers[eventName];
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scope.ServiceProvider.GetService(subscription);

                        if (handler == null)
                        {
                            continue;
                        }

                        var eventType = _evenTypes.SingleOrDefault(t => t.Name == eventName);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

                        return await (Task<bool>)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });

                    }
                }

            }

            return false;
        }
    }
}