using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthWind.Sales.BusinessObjects.Interfaces.Repositories;
using NorthWind.SqlClient.Repositories.DataServices;
using NorthWind.SqlClient.Repositories.Repositories;

namespace NorthWind.SqlClient.Repositories
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddSqlClientRepositories(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionStringName)
        {
            services.AddSingleton<INorthWindSalesQuerysRepository, NorthWindSqlClientQueryRepository>();

            services.AddSingleton(provider =>
            {
                return new NorthWindDataAccess(configuration.GetConnectionString(connectionStringName));
            });

            return services;
        }
    }
}
