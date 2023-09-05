using Moq;
using NorthWind.Sales.BusinessObjects.Enums;
using NorthWind.Sales.BusinessObjects.Interfaces.Ports;
using NorthWind.Sales.BusinessObjects.Interfaces.Presenters;
using NorthWind.Sales.BusinessObjects.POCOEntities;
using NorthWind.Sales.Controllers;

namespace NorthWInd.UnitTest.Unit
{
    public class GetAllOrdersControllerTests
    {
        [Fact]
        public async Task GetAllOrders_ReturnsOrdersFromPresenter()
        {
            // Arrange
            var mockInputPort = new Mock<IGetAllOrdersInputPort>();
            var mockPresenter = new Mock<IGetAllOrdersPresenter>();

            var expectedOrders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    CustomerId = "C1235",
                    ShipAddress = "1234 Main St",
                    ShipCity = "Cityville",
                    ShipCountry = "Countryland",
                    ShipPostalCode = "12345",
                    ShippingType = ShippingType.Road,
                    DiscountType = DiscountType.Percentage,
                    Discount = 10,
                },
                new Order
                {
                    Id = 2,
                    CustomerId = "C4565",
                    ShipAddress = "456 Elm St",
                    ShipCity = "Townsville",
                    ShipCountry = "Countryland",
                    ShipPostalCode = "54321",
                    ShippingType = ShippingType.Road,
                    DiscountType = DiscountType.Percentage,
                    Discount = 15,
                }
            };

            mockPresenter.SetupGet(p => p.Orders).Returns(expectedOrders);

            var controller = new GetAllOrdersController(mockInputPort.Object, mockPresenter.Object);

            // Act
            var result = await controller.GetAllOrders();

            // Assert
            mockInputPort.Verify(ip => ip.Handle(), Times.Once);
            Assert.Same(expectedOrders, result);
        }

    }
}
