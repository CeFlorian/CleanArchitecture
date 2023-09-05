using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NorthWind.RabbitMQProducer.Services;
using NorthWind.Sales.BusinessObjects.DTOs.CreateOrder;
using NorthWind.Sales.BusinessObjects.POCOEntities;
using RabbitMQ.Client;
using System.Text;

namespace NorthWInd.UnitTest.Unit
{
    public class RabbitMQBusProducerTests
    {

        [Fact]
        public async Task Publish_PublishesEventToExchange()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<RabbitMQBusProducer>>();
            var mockConnectionService = new Mock<IRabbitMqProducerConnectionService>();

            var mockSettings = new RabbitMQSettingsProducer
            {
                ProducerConnectionName = "TestConnection",
                HostName = "localhost",
                Port = "5672",
                UserName = "guest",
                Password = "guest",
                ExchangeName = "test-exchange",
                ExchangeType = "direct",
                QueueName = "test-queue",
                RoutingKey = "test-routing-key"
            };

            var producer = new RabbitMQBusProducer(mockSettings, mockLogger.Object, mockConnectionService.Object);

            var mockConnection = new Mock<IConnection>();
            var mockChannel = new Mock<IModel>();
            var mockProperties = new Mock<IBasicProperties>();
            //// Hace que todas las propiedades del objeto simulado se comporten como propiedades normales con el comportamiento get/set
            //mockProperties.SetupAllProperties();
            // Hace que una propiedad en especifico del objeto simulado se comporte como propiedad normal con el comportamiento get/set
            mockProperties.SetupProperty(p => p.DeliveryMode);

            mockConnection.Setup(conn => conn.CreateModel()).Returns(mockChannel.Object);
            mockConnection.Setup(conn => conn.CreateModel().CreateBasicProperties()).Returns(mockProperties.Object);
            mockConnectionService.Setup(service => service.CreateConnection()).Returns(mockConnection.Object);

            //var mockEvent = new Mock<Event>();

            var orderDetails = new List<CreateOrderDetailDTO>
            {
                new CreateOrderDetailDTO { ProductId = 1001, UnitPrice = 10.0m, Quantity = 2 },
                new CreateOrderDetailDTO { ProductId = 1002, UnitPrice = 15.0m, Quantity = 3 }
            };

            var orderCreatedEvent = new OrderCreatedEvent
            {
                Id = 123,
                CustomerId = "C1234",
                ShipAddress = "1234 Main St",
                ShipCity = "City",
                ShipCountry = "Country",
                ShipPostalCode = "12345",
                OrderDetails = orderDetails
            };

            // Act
            await producer.Publish(orderCreatedEvent);

            // Assert
            mockConnectionService.Verify(service => service.CreateConnection(), Times.Once);
            mockConnection.Verify(conn => conn.CreateModel(), Times.Once);
            mockChannel.Verify(chan => chan.ExchangeDeclare(mockSettings.ExchangeName, mockSettings.ExchangeType, true, false, It.IsAny<IDictionary<string, object>>()), Times.Once);
            mockChannel.Verify(chan => chan.QueueDeclare(mockSettings.QueueName, true, false, false, It.IsAny<IDictionary<string, object>>()), Times.Once);


            //IBasicProperties props = (IBasicProperties)mockChannel.Invocations[2].Arguments[3];
            //var body = (ReadOnlyMemory<Byte>)mockChannel.Invocations[2].Arguments[4];
            //mockChannel.Verify(model => model.BasicPublish(mockSettings.ExchangeName, mockSettings.RoutingKey, props, body.ToArray()), Times.Once);

            var message = JsonConvert.SerializeObject(orderCreatedEvent);
            var body = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(message));

            //mockProperties.Object.DeliveryMode = 2;

            // Para verificar datos de entrada de invocacion del metodo BasicPublish
            Assert.True(mockChannel.Invocations?[3].Arguments?.Count > 0);
            var basicPublishInvocationArgs = mockChannel.Invocations[3].Arguments;

            var bodyBytes = (ReadOnlyMemory<byte>)basicPublishInvocationArgs[4];
            string bodyMessage = Encoding.UTF8.GetString(bodyBytes.ToArray());

            var exchangeName = (string)basicPublishInvocationArgs[0];
            var routingKey = (string)basicPublishInvocationArgs[1];
            var props = (IBasicProperties)basicPublishInvocationArgs[3];

            Assert.NotNull(bodyMessage);
            Assert.Equal(body, bodyMessage);

            Assert.NotNull(exchangeName);
            Assert.Equal(mockSettings.ExchangeName, exchangeName);

            Assert.NotNull(routingKey);
            Assert.Equal(mockSettings.RoutingKey, routingKey);

            Assert.NotNull(props);
            Assert.Equal(mockProperties.Object, props);
            //Assert.Equal(2, props.DeliveryMode);

        }


    }
}
