using NorthWind.Mongo.Repositories;
using NorthWind.RabbitMQ.Service;

namespace NorthWind.Sales.IoC
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddNorthWindSalesServices(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionStringName, string connectionStringNameMongo)
        {
            services
                .AddRepositories(configuration, connectionStringName)
                .AddMongoRepositories(configuration, connectionStringNameMongo)
                .AddUseCasesServices()
                .AddPresenters()
                .AddNorthWindSalesControllers()
                .AddBusServices(configuration);

            return services;
        }

        public static IServiceCollection AddNorthWindConsumerServices(
            this IServiceCollection services,
            IConfiguration configuration, string connectionStringNameMongo)
        {
            services
                .AddMongoRepositories(configuration, connectionStringNameMongo)
                .AddBusServices(configuration);

            return services;
        }

    }
}
