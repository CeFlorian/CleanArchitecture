using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus;
using NorthWind.Sales.BusinessObjects.POCOEntities.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NorthWindRabbitMQConsumer.Services
{
    public class RabbitMQBusConsumer : IEventBusConsumer
    {
        readonly ILogger<RabbitMQBusConsumer> Logger;
        public readonly Dictionary<string, List<Type>> Handlers;
        readonly List<Type> EvenTypes;
        readonly IServiceScopeFactory ServiceScopeFactory;
        readonly RabbitMQSettingsConsumer Settings;

        readonly IModel Channel;
        readonly IConnection Connection;

        public RabbitMQBusConsumer(IServiceScopeFactory serviceScopeFactory, RabbitMQSettingsConsumer settings, ILogger<RabbitMQBusConsumer> logger, IRabbitMqConsumerConnectionService connection)
        {
            ServiceScopeFactory = serviceScopeFactory;
            Handlers = new Dictionary<string, List<Type>>();
            EvenTypes = new List<Type>();
            Settings = settings;
            Logger = logger;


            //var factory = new ConnectionFactory()
            //{
            //    HostName = Settings.HostName,
            //    UserName = Settings.UserName,
            //    Password = Settings.Password,
            //    DispatchConsumersAsync = true
            //};

            //// Sin utilizar "using" para que se mantenga abierta connection y channel y no se liberen los recursos
            //Connection = factory.CreateConnection(Settings.ConsumerConnectionName);
            Connection = connection.CreateConnection();
            Channel = Connection.CreateModel();
        }



        public async Task Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!EvenTypes.Contains(typeof(T)))
            {
                EvenTypes.Add(typeof(T));
            }

            if (!Handlers.ContainsKey(eventName))
            {
                Handlers.Add(eventName, new List<Type>());
            }

            if (Handlers[eventName].Any(s => s == handlerType))
            {
                string error = $"Handler Type {handlerType.Name} already is registered for '{eventName}'";
                Logger.LogError(error, nameof(handlerType));
                throw new ArgumentException(error, nameof(handlerType));
            }

            Handlers[eventName].Add(handlerType);

            await StartBasicConsumer<T>();

        }

        public async Task Unsubscribe<T, TH>() where T : Event where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (EvenTypes.Contains(typeof(T)))
            {
                EvenTypes.Remove(typeof(T));
            }

            if (Handlers.ContainsKey(eventName))
            {
                if (!Handlers[eventName].Any(s => s == handlerType))
                {
                    //Handlers[eventName].Remove(handlerType);

                    string error = $"Handler type {handlerType.Name} is not registered for '{eventName}'";
                    Logger.LogError(error, nameof(handlerType));
                    throw new ArgumentException(error, nameof(handlerType));
                }

                Handlers.Remove(eventName);
            }
            else
            {
                string error = $"Handler type {handlerType.Name} is not registered for '{eventName}'";
                Logger.LogError(error, nameof(handlerType));
                throw new ArgumentException(error, nameof(handlerType));
            }

        }

        private async Task StartBasicConsumer<T>() where T : Event
        {


            //// Sin utilizar "using" para que se mantenga abierta connection y channel y no se liberen los recursos
            //var connection = factory.CreateConnection(Settings.ConsumerConnectionName);
            //var channel = connection.CreateModel();

            var eventName = typeof(T).Name;
            //channel.QueueDeclare(eventName, false, false, false, null);

            Channel.ExchangeDeclare(Settings.ExchangeName, Settings.ExchangeType, true, false);
            Channel.QueueDeclare(Settings.QueueName, true, false, false, null);

            Channel.QueueBind(Settings.QueueName, Settings.ExchangeName, Settings.Relativekey);

            //Aceptar solo un mensaje no confirmado a la vez
            Channel.BasicQos(0, 1, false);

            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.Received += Consumer_Received;

            Channel.BasicConsume(Settings.QueueName, false, consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey.Split('.').FirstOrDefault();
            var message = Encoding.UTF8.GetString(e.Body.Span);

            try
            {
                var result = await ProcessEvent(eventName, message).ConfigureAwait(false);

                if (result)
                {
                    ((AsyncDefaultBasicConsumer)sender).Model.BasicAck(e.DeliveryTag, false);
                }
                else
                {
                    ((AsyncDefaultBasicConsumer)sender).Model.BasicNack(e.DeliveryTag, false, true);

                }
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Something went wrong with Consumer_Received!");
            }
        }

        private async Task<bool> ProcessEvent(string eventName, string message)
        {
            if (Handlers.ContainsKey(eventName))
            {
                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    var subscriptions = Handlers[eventName];
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scope.ServiceProvider.GetService(subscription);

                        if (handler == null)
                        {
                            continue;
                        }

                        var eventType = EvenTypes.SingleOrDefault(t => t.Name == eventName);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

                        return await (Task<bool>)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });

                    }
                }

            }

            return false;
        }

        public void Dispose()
        {
            if (Channel.IsOpen)
                Channel.Close();
            if (Connection.IsOpen)
                Connection.Close();
        }
    }
}
