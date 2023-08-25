using MongoDB.Driver;
using NorthWind.Sales.BusinessObjects.Aggregates;
using NorthWind.Sales.BusinessObjects.Interfaces.Repositories.Consumer;

namespace NorthWind.Mongo.Repositories
{
    public class NorthWindConsumerCommandsRepository : INorthWindConsumerCommandsRepository
    {
        readonly IMongoCollection<OrderAggregate> OrderCollection;

        public NorthWindConsumerCommandsRepository(IMongoDatabase database)
        {
            OrderCollection = database.GetCollection<OrderAggregate>("users");
        }

        public async ValueTask CreateOrder(OrderAggregate order)
        {
            await OrderCollection.InsertOneAsync(order);
        }


    }
}
