using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NorthWind.Sales.BusinessObjects.Aggregates;
using NorthWind.Sales.BusinessObjects.Interfaces.Repositories.Consumer;

namespace NorthWind.Mongo.Repositories
{
    public class NorthWindConsumerCommandsRepository : INorthWindConsumerCommandsRepository
    {
        readonly IMongoCollection<OrderAggregate> OrderCollection;
        readonly IOptions<MongoDBSettings> Settings;

        public NorthWindConsumerCommandsRepository(IMongoDatabase database, IOptions<MongoDBSettings> settings)
        {
            Settings = settings;
            OrderCollection = database.GetCollection<OrderAggregate>(Settings.Value.CollectionNames.Orders);
        }

        public async ValueTask CreateOrder(OrderAggregate order)
        {
            await OrderCollection.InsertOneAsync(order);
        }


    }
}
