using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using NorthWind.Sales.BusinessObjects.Aggregates;
using NorthWind.Sales.BusinessObjects.Interfaces.Repositories.Consumer;

namespace NorthWind.Mongo.Repositories
{
    public class NorthWindConsumerQuerysRepository : INorthWindConsumerQuerysRepository
    {
        readonly IMongoCollection<OrderAggregate> OrderCollection;
        readonly IOptions<MongoDBSettings> Settings;

        public NorthWindConsumerQuerysRepository(IMongoDatabase database, IOptions<MongoDBSettings> settings)
        {
            Settings = settings;
            OrderCollection = database.GetCollection<OrderAggregate>(Settings.Value.CollectionNames.Orders);
        }

        public async Task<IEnumerable<OrderAggregate>> GetAllOrders()
        {
            return await OrderCollection.Find(new BsonDocument()).ToListAsync();
            //return await OrderCollection.Find(_ => true).ToListAsync();

        }

        public async Task<OrderAggregate> GetOrderById(int id)
        {
            return await OrderCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
            //return await OrderCollection.Find(Builders<OrderAggregate>.Filter.Eq("_id", new ObjectId("id"))).FirstOrDefaultAsync();

        }
    }
}
