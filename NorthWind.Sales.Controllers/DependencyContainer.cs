namespace NorthWind.Sales.Controllers
{
    public static class DependencyContainer
    {
        /* 
         * Agregar que servicios se implementan en este proyecto (interfaces de las que se hereden
         * en una clase de este proyecto, ademas, hacer el recorrido de las herencias de la interface
         * heredada para tambien agregarlas a la coleccion de servicios).
         * Los servicios que se inyectan (usan) en metodos de una clase no es necesario agregarlos, solamente 
         * si aplica a lo mencionado arriba
        */
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
