using Moq;
using NorthWind.Sales.BusinessObjects.DTOs.CreateOrder;
using NorthWind.Sales.BusinessObjects.Interfaces.Ports;
using NorthWind.Sales.BusinessObjects.Interfaces.Presenters;
using NorthWind.Sales.Controllers;

namespace NorthWInd.UnitTest.Unit
{
    public class CreateOrderControllerTests
    {

        [Fact]
        public async Task CreateOrder_ValidInput_ReturnsOrderId()
        {
            // Arrange
            var mockInputPort = new Mock<ICreateOrderInputPort>();
            var mockPresenter = new Mock<ICreateOrderPresenter>();

            var expectedOrderId = 321; // Simulated Order ID

            mockPresenter.SetupGet(p => p.OrderId).Returns(expectedOrderId);

            var controller = new CreateOrderController(mockInputPort.Object, mockPresenter.Object);

            var orderDetails = new List<CreateOrderDetailDTO>
            {
                new CreateOrderDetailDTO { ProductId = 1, UnitPrice = 10.99M, Quantity = 2 },
                new CreateOrderDetailDTO { ProductId = 2, UnitPrice = 5.99M, Quantity = 3 }
            };

            var orderDTO = new CreateOrderDTO
            {
                CustomerId = "12345",
                ShipAddress = "1234 Main St",
                ShipCity = "City",
                ShipCountry = "Country",
                ShipPostalCode = "44444",
                OrderDetails = orderDetails
            };

            // Act
            var result = await controller.CreateOrder(orderDTO);

            // Assert
            mockInputPort.Verify(ip => ip.Handle(orderDTO), Times.Once);
            Assert.Equal(expectedOrderId, result);
        }
    }
}
