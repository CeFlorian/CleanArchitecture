using NorthWind.Mongo.Repositories;
using NorthWind.RabbitMQ.Service;

namespace NorthWind.Sales.IoC
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddNorthWindSalesServices(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionStringName, string connectionStringNameMongo, string mongoSettingsName,
            string messageBrokerHost)
        {
            services
                .AddRepositories(configuration, connectionStringName)
                .AddMongoRepositories(configuration, connectionStringNameMongo, mongoSettingsName)
                .AddUseCasesServices()
                .AddPresenters()
                .AddNorthWindSalesControllers()
                .AddBusServices(configuration, messageBrokerHost);

            return services;
        }

        public static IServiceCollection AddNorthWindConsumerServices(
            this IServiceCollection services,
            IConfiguration configuration, string connectionStringNameMongo, string mongoSettingsName,
            string messageBrokerHost)
        {
            services
                .AddMongoRepositories(configuration, connectionStringNameMongo, mongoSettingsName)
                .AddBusServices(configuration, messageBrokerHost);

            return services;
        }

    }
}
