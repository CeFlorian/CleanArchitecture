namespace NorthWind.Sales.Controllers
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddNorthWindSalesControllers(
            this IServiceCollection services)
        {
            services.AddScoped<ICreateOrderController,
                CreateOrderController>();
            services.AddScoped<IGetAllOrdersController,
                GetAllOrdersController>();

            return services;
        }
    }
}
