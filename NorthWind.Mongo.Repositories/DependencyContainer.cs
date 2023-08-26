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
            string connectionStringName, string mongoSettingsName)
        {

            services.Configure<MongoDBSettings>(configuration.GetSection(mongoSettingsName));
            var mongoDBSettings = configuration.GetSection(mongoSettingsName).Get<MongoDBSettings>();

            services.AddSingleton<IMongoDatabase>(options =>
            {
                var client = new MongoClient(configuration.GetConnectionString(connectionStringName));
                return client.GetDatabase(mongoDBSettings.Database);
            });

            services.AddScoped<INorthWindConsumerCommandsRepository, NorthWindConsumerCommandsRepository>();

            return services;
        }
    }
}
