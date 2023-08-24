using NorthWind.RabbitMQ.Service;

namespace NorthWind.Sales.IoC
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddNorthWindSalesServices(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionStringName)
        {
            services
                .AddRepositories(configuration, connectionStringName)
                .AddUseCasesServices()
                .AddPresenters()
                .AddNorthWindSalesControllers();

            return services;
        }

        public static IServiceCollection AddNorthWindConsumerServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddBusServices(configuration);

            return services;
        }

    }
}
