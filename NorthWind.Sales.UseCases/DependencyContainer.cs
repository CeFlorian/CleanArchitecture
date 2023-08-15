using NorthWind.Sales.UseCases.GetAllOrders;

namespace NorthWind.Sales.UseCases
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddUseCasesServices(
            this IServiceCollection services)
        {
            services.AddScoped<ICreateOrderInputPort, CreateOrderInteractor>();
            services.AddScoped<IGetAllOrdersInputPort, GetAllOrdersInteractor>();

            return services;
        }
    }
}
