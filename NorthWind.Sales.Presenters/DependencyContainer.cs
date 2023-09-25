namespace NorthWind.Sales.Presenters
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddPresenters(
            this IServiceCollection services)
        {
            services.AddScoped<CreateOrderPresenter>();
            services.AddScoped<GetAllOrdersPresenter>();
            services.AddScoped<LoginPresenter>();

            /* 
             * La misma instancia de ese servicio será devuelta cuando sea requerido 
             * el servicio ICreateOrderOutputPort y cuando sea requerido el 
             * servicio ICreateOrderPresenter.
             * Tanto el Controlador como el Interactor recibirán la misma instancia,
             * uno en forma de Presenter y el otro en forma de Output Port.
             */
            services.AddScoped<ICreateOrderOutputPort>(
                provider => provider.GetRequiredService<CreateOrderPresenter>());

            services.AddScoped<ICreateOrderPresenter>(
                provider => provider.GetRequiredService<CreateOrderPresenter>());

            services.AddScoped<IGetAllOrdersOutputPort>(
                provider => provider.GetRequiredService<GetAllOrdersPresenter>());

            services.AddScoped<IGetAllOrdersPresenter>(
                provider => provider.GetRequiredService<GetAllOrdersPresenter>());

            services.AddScoped<ILoginOutputPort>(
                provider => provider.GetService<LoginPresenter>());
            services.AddScoped<ILoginPresenter>(
                provider => provider.GetService<LoginPresenter>());

            return services;
        }
    }
}
