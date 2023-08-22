using NorthWind.API.Services;
using NorthWind.Token.Services;

namespace NorthWind.Sales.IoC
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddNorthWindSalesServices(
            this IServiceCollection services,
            IConfiguration configuration, string jwtSettingsName,
            string connectionStringName, string apiSettingsName)
        {
            services
                .AddRepositories(configuration, connectionStringName)
                .AddUseCasesServices()
                .AddPresenters()
                .AddNorthWindSalesControllers()
                .AddTokenServices(configuration, jwtSettingsName)
                .AddAPIServices(configuration, apiSettingsName);

            return services;
        }
    }
}
