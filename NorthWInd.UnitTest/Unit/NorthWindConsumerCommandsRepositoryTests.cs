using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using NorthWind.Mongo.Repositories;
using NorthWind.Sales.BusinessObjects.Aggregates;

namespace NorthWInd.UnitTest.Unit
{
    public class NorthWindConsumerCommandsRepositoryTests
    {


        [Fact]
        public async Task CreateOrder_InsertsOrderIntoCollection()
        {
            // Arrange
            var mockCollection = new Mock<IMongoCollection<OrderAggregate>>();
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<OrderAggregate>(It.IsAny<string>(), null)).Returns(mockCollection.Object);

            // Use an actual instance of MongoDBSettings instead of Moq
            var settings = new MongoDBSettings
            {
                CollectionNames = new CollectionNames
                {
                    Orders = "Orders"
                }
            };

            var options = Options.Create(settings);

            var repository = new NorthWindConsumerCommandsRepository(mockDatabase.Object, options);

            var orderAggregate = new OrderAggregate
            {
                Id = 1,
                CustomerId = "C1234",
                ShipAddress = "1234 Main St",
                ShipCity = "Cityville",
                ShipCountry = "Countryland",
                ShipPostalCode = "12345",
            };

            orderAggregate.AddDetail(1001, 10.0m, 2);
            orderAggregate.AddDetail(1002, 15.0m, 3);

            // Act
            await repository.CreateOrder(orderAggregate);

            // Assert
            mockCollection.Verify(col => col.InsertOneAsync(orderAggregate, null, default), Times.Once);

            // Additional assertion to compare the object passed to InsertOneAsync
            Assert.True(orderAggregate.Equals(mockCollection.Invocations[0].Arguments[0]));
        }



        //[Fact]
        //public async Task CreateOrder_InsertsOrderIntoCollection()
        //{
        //    // Arrange
        //    var mockCollection = new Mock<IMongoCollection<OrderAggregate>>();
        //    var mockDatabase = new Mock<IMongoDatabase>();
        //    mockDatabase.Setup(db => db.GetCollection<OrderAggregate>(It.IsAny<string>(), null)).Returns(mockCollection.Object);

        //    var mockSettings = new Mock<IOptions<MongoDBSettings>>();
        //    mockSettings.Setup(settings => settings.Value.CollectionNames.Orders).Returns("orders");

        //    var repository = new NorthWindConsumerCommandsRepository(mockDatabase.Object, mockSettings.Object);

        //    var orderAggregate = new OrderAggregate
        //    {
        //        Id = 1,
        //        CustomerId = "C1234",
        //        // Initialize other properties
        //    };

        //    // Act
        //    await repository.CreateOrder(orderAggregate);

        //    // Assert
        //    mockCollection.Verify(col => col.InsertOneAsync(orderAggregate, null, default), Times.Once);

        //    // Additional assertion to verify that the same object was passed to InsertOneAsync
        //    mockCollection.Verify(col => col.InsertOneAsync(It.Is<OrderAggregate>(o => o == orderAggregate), null, default), Times.Once);

        //    // Assert.True() to validate that the same object was passed to InsertOneAsync
        //    Assert.True(orderAggregate == mockCollection.Object.InsertOneAsyncCallbackArg());
        //}



    }
}
