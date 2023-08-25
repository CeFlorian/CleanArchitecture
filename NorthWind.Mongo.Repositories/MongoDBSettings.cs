namespace NorthWind.Mongo.Repositories
{
    public class MongoDBSettings
    {
        public string Database { get; set; }
        public CollectionNames CollectionNames { get; set; }
    }

    public class CollectionNames
    {
        public string Orders { get; set; }
    }

}
