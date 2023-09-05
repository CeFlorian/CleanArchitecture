using Moq;
using NorthWind.Sales.BusinessObjects.Aggregates;
using NorthWind.Sales.BusinessObjects.DTOs.CreateOrder;
using NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus;
using NorthWind.Sales.BusinessObjects.Interfaces.Ports;
using NorthWind.Sales.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.BusinessObjects.POCOEntities;
using NorthWind.Sales.UseCases.CreateOrder;

namespace NorthWInd.UnitTest.Unit
{
    public class CreateOrderInteractorTests
    {
        [Fact]
        public async Task Handle_ValidOrderDTO_CreatesOrderAndPublishesEvent()
        {
            // Arrange
            var mockOutputPort = new Mock<ICreateOrderOutputPort>();
            var mockRepository = new Mock<INorthWindSalesCommandsRepository>();
            var mockEventBus = new Mock<IEventBusProducer>();

            var interactor = new CreateOrderInteractor(mockOutputPort.Object, mockRepository.Object, mockEventBus.Object);

            var orderDTO = new CreateOrderDTO
            {
                CustomerId = "C1234",
                ShipAddress = "1234 Main St",
                ShipCity = "Cityville",
                ShipCountry = "Countryland",
                ShipPostalCode = "12345",
                OrderDetails = new List<CreateOrderDetailDTO>
            {
                new CreateOrderDetailDTO { ProductId = 1001, UnitPrice = 10.0m, Quantity = 2 },
                new CreateOrderDetailDTO { ProductId = 1002, UnitPrice = 15.0m, Quantity = 3 }
            }
            };

            var expectedOrderId = 456; // The Id set in the callback

            // Configure the CreateOrder method to set the Id
            mockRepository.Setup(r => r.CreateOrder(It.IsAny<OrderAggregate>()))
                .Callback<OrderAggregate>(order =>
                {
                    order.Id = expectedOrderId;
                });

            // Act
            await interactor.Handle(orderDTO);

            // Assert
            // Se peude pasar el parametro exacto que se pasa o que sea cualquiera con tipo por ejp: CreateOrder(It.IsAny<OrderAggregate>())
            mockRepository.Verify(r => r.CreateOrder(It.IsAny<OrderAggregate>()), Times.Once);
            mockRepository.Verify(r => r.SaveChanges(), Times.Once);
            mockOutputPort.Verify(op => op.Handle(It.IsAny<int>()), Times.Once);
            mockEventBus.Verify(eb => eb.Publish(It.IsAny<OrderCreatedEvent>()), Times.Once);

            // Additional assertions
            var actualOutputPortArg = mockOutputPort.Invocations[0].Arguments[0] as int?;
            Assert.NotNull(actualOutputPortArg); // Verify the argument passed to Handle is not null
            Assert.Equal(expectedOrderId, actualOutputPortArg); // Verify that the expected and actual id are equal

        }
    }
}