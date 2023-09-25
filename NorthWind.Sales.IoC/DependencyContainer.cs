using NorthWind.SqlClient.Repositories;

using NorthWind.API.Services;
using NorthWind.Token.Services;

using NorthWind.Mongo.Repositories;
using NorthWind.RabbitMQProducer.Services;
using NorthWindRabbitMQConsumer.Services;

namespace NorthWind.Sales.IoC
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddNorthWindSalesServices(
            this IServiceCollection services,
            IConfiguration configuration, string connectionStringNameEF, string jwtSettingsName,
             string connectionStringNameSqlClient, string apiSettingsName, string rabbitMQSettingsName)
        {
            services
                .AddEFRepositories(configuration, connectionStringNameEF)
                .AddSqlClientRepositories(configuration, connectionStringNameSqlClient)
                .AddUseCasesServices()
                .AddPresenters()
                .AddNorthWindSalesControllers()
                .AddTokenServices(configuration, jwtSettingsName)
                .AddAPIServices(configuration, apiSettingsName)
                .AddProducerServices(configuration, rabbitMQSettingsName);

            return services;
        }

        public static IServiceCollection AddNorthWindConsumerServices(
            this IServiceCollection services,
            IConfiguration configuration, string connectionStringNameMongo, string mongoSettingsName,
            string rabbitMQSettingsName)
        {
            services
                .AddMongoRepositories(configuration, connectionStringNameMongo, mongoSettingsName)
                .AddConsumerServices(configuration, rabbitMQSettingsName);

            return services;
        }

    }
}
