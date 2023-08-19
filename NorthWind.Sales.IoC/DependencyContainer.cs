using NorthWind.Token.Services;

namespace NorthWind.Sales.IoC
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddNorthWindSalesServices(
            this IServiceCollection services,
            IConfiguration configuration, string jwtSettingsName,
            string connectionStringName)
        {
            services
                .AddRepositories(configuration, connectionStringName)
                .AddUseCasesServices()
                .AddPresenters()
                .AddNorthWindSalesControllers()
                .AddTokenServices(configuration, jwtSettingsName);

            return services;
        }
    }
}
