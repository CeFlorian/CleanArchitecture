using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NorthWind.Sales.BusinessObjects.POCOEntities;
using NorthWind.Sales.UseCases.CreateOrder;
using NorthWindRabbitMQConsumer.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NorthWInd.UnitTest.Unit
{
    public class RabbitMQBusConsumerTests
    {
        [Fact]
        public async Task Subscribe_AddsHandlerAndStartsConsumer()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<RabbitMQBusConsumer>>();
            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            var mockScope = new Mock<IServiceScope>();
            var mockProvider = new Mock<IServiceProvider>();

            mockServiceScopeFactory.Setup(factory => factory.CreateScope()).Returns(mockScope.Object);
            mockScope.SetupGet(scope => scope.ServiceProvider).Returns(mockProvider.Object);

            var mockConnectionService = new Mock<IRabbitMqConsumerConnectionService>();
            var mockConnection = new Mock<IConnection>();
            var mockChannel = new Mock<IModel>();

            mockConnectionService.Setup(conn => conn.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateModel()).Returns(mockChannel.Object);

            var consumer = new RabbitMQBusConsumer(mockServiceScopeFactory.Object, GetMockSettings(), mockLogger.Object, mockConnectionService.Object);

            // Act
            await consumer.Subscribe<OrderCreatedEvent, OrderCreatedEventHandler>();

            // Assert
            // Verify that the handler was added to the Handlers dictionary
            Assert.Single(consumer.Handlers);
            Assert.Contains(typeof(OrderCreatedEvent).Name, consumer.Handlers.Keys);
            Assert.Contains(typeof(OrderCreatedEventHandler), consumer.Handlers[typeof(OrderCreatedEvent).Name]);

            var settings = GetMockSettings();
            // Verify that StartBasicConsumer was called
            mockConnectionService.Verify(conn => conn.CreateConnection(), Times.Once);
            mockChannel.Verify(model => model.ExchangeDeclare(settings.ExchangeName, settings.ExchangeType, true, false, null), Times.Once);
            mockChannel.Verify(model => model.QueueDeclare(settings.QueueName, true, false, false, null), Times.Once);
            mockChannel.Verify(model => model.QueueBind(settings.QueueName, settings.ExchangeName, settings.Relativekey, null), Times.Once);
            mockChannel.Verify(model => model.BasicQos(0, 1, false), Times.Once);

            //mockChannel.Verify(model => model.BasicConsume(It.IsAny<string>(), false, It.IsAny<IBasicConsumer>()), Times.Once);

            // Para verificar datos de entrada de invocacion del metodo BasicConsume
            Assert.True(mockChannel.Invocations?[4].Arguments?.Count > 0);
            var basicPublishInvocationArgs = mockChannel.Invocations[4].Arguments;

            var queueName = (string)basicPublishInvocationArgs[0];
            var autoAck = (bool)basicPublishInvocationArgs[1];
            var basicConsumer = (AsyncEventingBasicConsumer)basicPublishInvocationArgs[6];

            Assert.NotNull(queueName);
            Assert.Equal(settings.QueueName, queueName);

            Assert.NotNull(autoAck);
            Assert.Equal(false, autoAck);

            var eventingBasicConsumer = new AsyncEventingBasicConsumer(mockChannel.Object);
            Assert.NotNull(basicConsumer);
            Assert.NotEqual(eventingBasicConsumer, basicConsumer);

            await Unsubscribe_RemovesHandler(consumer);

        }

        public async Task Unsubscribe_RemovesHandler(RabbitMQBusConsumer consumer)
        {
            // Arrange

            // Act
            await consumer.Unsubscribe<OrderCreatedEvent, OrderCreatedEventHandler>();

            // Assert
            // Verify that the handler was removed from the Handlers dictionary
            Assert.Empty(consumer.Handlers);
            //Assert.DoesNotContain(typeof(OrderCreatedEvent).Name, consumer.Handlers.Keys);

            consumer.Dispose();

        }

        [Fact]
        public async Task Unsubscribe_RemovesHandler_Exception()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<RabbitMQBusConsumer>>();
            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            var mockScope = new Mock<IServiceScope>();
            var mockProvider = new Mock<IServiceProvider>();

            mockServiceScopeFactory.Setup(factory => factory.CreateScope()).Returns(mockScope.Object);
            mockScope.Setup(scope => scope.ServiceProvider).Returns(mockProvider.Object);

            var mockConnectionService = new Mock<IRabbitMqConsumerConnectionService>();
            var mockConnection = new Mock<IConnection>();
            var mockChannel = new Mock<IModel>();

            mockConnectionService.Setup(conn => conn.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateModel()).Returns(mockChannel.Object);

            var consumer = new RabbitMQBusConsumer(mockServiceScopeFactory.Object, GetMockSettings(), mockLogger.Object, mockConnectionService.Object);

            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await consumer.Unsubscribe<OrderCreatedEvent, OrderCreatedEventHandler>());

            // Assert
            Assert.NotNull(ex?.Message);
            string error = $"Handler type {typeof(OrderCreatedEventHandler).Name} is not registered for '{typeof(OrderCreatedEvent).Name}'";
            Assert.StartsWith(error, ex.Message);

            consumer.Dispose();

        }

        private RabbitMQSettingsConsumer GetMockSettings()
        {
            return new RabbitMQSettingsConsumer
            {
                ConsumerConnectionName = "test-connection",
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                ExchangeName = "test-exchange",
                ExchangeType = "direct",
                QueueName = "test-queue",
                RoutingKey = "test-routing-key",
                Relativekey = "test-key"
            };
        }
    }
}
