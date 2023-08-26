using NorthWind.Mongo.Repositories;
using NorthWind.RabbitMQProducer.Services;
using NorthWindRabbitMQConsumer.Services;

namespace NorthWind.Sales.IoC
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddNorthWindSalesServices(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionStringName, string rabbitMQSettingsName)
        {
            services
                .AddRepositories(configuration, connectionStringName)
                .AddUseCasesServices()
                .AddPresenters()
                .AddNorthWindSalesControllers()
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
