using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using NorthWind.Sales.BusinessObjects.Interfaces.Repositories.Consumer;

namespace NorthWind.Mongo.Repositories
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddMongoRepositories(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionStringName)
        {

            var mongoClient = new MongoClient(configuration.GetConnectionString(connectionStringName));
            var mongoDatabase = mongoClient.GetDatabase("NorthWindDB");

            services.AddSingleton<IMongoDatabase>(mongoDatabase);
            services.AddScoped<INorthWindConsumerCommandsRepository, NorthWindConsumerCommandsRepository>();

            return services;
        }
    }
}
