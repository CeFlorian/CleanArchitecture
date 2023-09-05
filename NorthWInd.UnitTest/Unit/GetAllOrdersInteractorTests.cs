using Moq;
using NorthWind.Sales.BusinessObjects.Enums;
using NorthWind.Sales.BusinessObjects.Interfaces.Ports;
using NorthWind.Sales.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.BusinessObjects.POCOEntities;
using NorthWind.Sales.UseCases.GetAllOrders;

namespace NorthWInd.UnitTest.Unit
{
    public class GetAllOrdersInteractorTests
    {
        [Fact]
        public async Task Handle_GetsAndOutputsOrders()
        {
            // Arrange
            var mockOutputPort = new Mock<IGetAllOrdersOutputPort>();
            var mockRepository = new Mock<INorthWindSalesQuerysRepository>();

            var interactor = new GetAllOrdersInteractor(mockOutputPort.Object, mockRepository.Object);

            var expectedOrders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    CustomerId = "C1234",
                    ShipAddress = "1234 Main St",
                    ShipCity = "Cityville",
                    ShipCountry = "Countryland",
                    ShipPostalCode = "12345",
                    ShippingType = ShippingType.Road,
                    DiscountType = DiscountType.Percentage,
                    Discount = 10,
                    OrderDate = DateTime.Now
                },
                new Order
                {
                    Id = 2,
                    CustomerId = "C4567",
                    ShipAddress = "456 Elm St",
                    ShipCity = "Townsville",
                    ShipCountry = "Countryland",
                    ShipPostalCode = "54321",
                    ShippingType = ShippingType.Road,
                    DiscountType = DiscountType.Percentage,
                    Discount = 15,
                }
            };

            mockRepository.Setup(r => r.GetAllOrders()).ReturnsAsync(expectedOrders);

            // Act
            await interactor.Handle();

            // Assert
            mockRepository.Verify(r => r.GetAllOrders(), Times.Once);
            // Se peude pasar el parametro exacto que se pasa (expectedOrders) o que sea cualquiera con tipo por ejp: op.Handle(It.IsAny<List<Order>>())
            mockOutputPort.Verify(op => op.Handle(expectedOrders), Times.Once);

            // Additional assertions
            var actualOutputPortArg = mockOutputPort.Invocations[0].Arguments[0] as List<Order>;
            Assert.NotNull(actualOutputPortArg); // Verify the argument passed to Handle is not null
            Assert.Equal(expectedOrders, actualOutputPortArg); // Verify that the expected and actual lists are equal
        }

    }
}
