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

            services.Configure<MongoDBSettings>(configuration.GetRequiredSection(mongoSettingsName));
            var mongoDBSettings = configuration.GetRequiredSection(mongoSettingsName).Get<MongoDBSettings>();

            services.AddTransient<IMongoDatabase>(options =>
            {
                var client = new MongoClient(configuration.GetConnectionString(connectionStringName));
                return client.GetDatabase(mongoDBSettings.Database);
            });

            services.AddScoped<INorthWindConsumerCommandsRepository, NorthWindConsumerCommandsRepository>();

            services.AddScoped<INorthWindConsumerQuerysRepository, NorthWindConsumerQuerysRepository>();


            return services;
        }
    }
}
