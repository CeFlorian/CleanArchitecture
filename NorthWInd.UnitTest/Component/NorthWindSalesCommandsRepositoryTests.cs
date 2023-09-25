using Microsoft.EntityFrameworkCore;
using Moq;
using NorthWind.EFCore.Repositories.DataContexts;
using NorthWind.EFCore.Repositories.Repositories;
using NorthWind.Entities.Interfaces;
using NorthWind.Sales.BusinessObjects.Aggregates;

namespace NorthWInd.UnitTest.Component
{
    public class NorthWindSalesCommandsRepositoryTests
    {
        [Fact]
        public async Task CreateOrder_AddsOrderAndDetailsToContextInMemory()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NorthWindSalesContext>()
                .UseInMemoryDatabase(databaseName: "NorthWindDBInMemoryTest")
                .Options;

            var context = new NorthWindSalesContext(options);
            var mockLogger = new Mock<IApplicationStatusLogger>();

            // No es necesario cuando se usa un context de DB en memoria
            //context.Database.EnsureCreated();

            var commandsRepository = new NorthWindSalesCommandsRepository(context, mockLogger.Object);
            var querysRepository = new NorthWindSalesQuerysRepository(context);

            var orderAggregate = new OrderAggregate
            {
                //Id = 1,
                CustomerId = "C1234",
                ShipAddress = "1234 Main St",
                ShipCity = "Cityville",
                ShipCountry = "Countryland",
                ShipPostalCode = "12345",
            };

            orderAggregate.AddDetail(1001, 10.0m, 2);
            orderAggregate.AddDetail(1002, 15.0m, 3);


            // Act
            await commandsRepository.CreateOrder(orderAggregate);
            await commandsRepository.SaveChanges();

            // Assert
            var orderInContext = await querysRepository.GetOrderById(orderAggregate.Id);
            Assert.NotNull(orderInContext);
            Assert.Equal(orderAggregate.Id, orderInContext.Id);

        }
    }
}
