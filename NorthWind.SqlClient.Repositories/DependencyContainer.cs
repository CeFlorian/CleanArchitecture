using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NorthWind.SqlClient.Repositories
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddSqlClientRepositories(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionStringName)
        {
            //// Para reemplazar y utilizar SQLCLient en vez de EF
            //services.AddSingleton<INorthWindSalesQuerysRepository, NorthWindSqlClientQueryRepository>();

            //services.AddSingleton(provider =>
            //{
            //    return new NorthWindDataAccess(configuration.GetConnectionString(connectionStringName));
            //});

            return services;
        }
    }
}
