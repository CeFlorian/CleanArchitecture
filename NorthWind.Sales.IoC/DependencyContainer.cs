using NorthWind.SqlClient.Repositories;

namespace NorthWind.Sales.IoC
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddNorthWindSalesServices(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionStringNameEF, string connectionStringNameSqlClient)
        {
            services
                .AddEFRepositories(configuration, connectionStringNameEF)
                .AddSqlClientRepositories(configuration, connectionStringNameSqlClient)
                .AddUseCasesServices()
                .AddPresenters()
                .AddNorthWindSalesControllers();

            return services;
        }
    }
}
