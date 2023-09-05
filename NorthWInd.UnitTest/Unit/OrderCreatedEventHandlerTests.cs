using Moq;
using NorthWind.Sales.BusinessObjects.Aggregates;
using NorthWind.Sales.BusinessObjects.DTOs.CreateOrder;
using NorthWind.Sales.BusinessObjects.Interfaces.Repositories.Consumer;
using NorthWind.Sales.BusinessObjects.POCOEntities;
using NorthWind.Sales.UseCases.CreateOrder;

namespace NorthWInd.UnitTest.Unit
{
    public class OrderCreatedEventHandlerTests
    {
        [Fact]
        public async Task Handle_ValidEvent_CreatesOrderAndReturnsTrue()
        {
            // Arrange
            var mockRepository = new Mock<INorthWindConsumerCommandsRepository>();
            var handler = new OrderCreatedEventHandler(mockRepository.Object);

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
                ShipCity = "Cityville",
                ShipCountry = "Countryland",
                ShipPostalCode = "12345",
                OrderDetails = orderDetails
            };

            // Act
            var result = await handler.Handle(orderCreatedEvent);

            // Assert
            mockRepository.Verify(r => r.CreateOrder(It.IsAny<OrderAggregate>()), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_Exception_ReturnsFalse()
        {
            // Arrange
            var mockRepository = new Mock<INorthWindConsumerCommandsRepository>();
            mockRepository.Setup(r => r.CreateOrder(It.IsAny<OrderAggregate>())).ThrowsAsync(new Exception());

            var handler = new OrderCreatedEventHandler(mockRepository.Object);

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
                ShipCity = "Cityville",
                ShipCountry = "Countryland",
                ShipPostalCode = "12345",
                OrderDetails = orderDetails
            };

            // Act
            var result = await handler.Handle(orderCreatedEvent);

            // Assert
            Assert.False(result);
        }
    }
}
