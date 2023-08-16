using NorthWind.Entities;
using NorthWind.Sales.BusinessObjects;

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
                .AddEntityServices()
                .AddDTOValidators()
                .AddRepositories(configuration, connectionStringName)
                .AddUseCasesServices()
                .AddPresenters()
                .AddNorthWindSalesControllers();

            return services;
        }
    }
}
