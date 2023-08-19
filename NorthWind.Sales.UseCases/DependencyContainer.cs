﻿using NorthWind.Sales.UseCases.GetAllOrders;
using NorthWind.Sales.UseCases.Login;

namespace NorthWind.Sales.UseCases
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddUseCasesServices(
            this IServiceCollection services)
        {
            services.AddScoped<ICreateOrderInputPort, CreateOrderInteractor>();
            services.AddScoped<IGetAllOrdersInputPort, GetAllOrdersInteractor>();
            services.AddScoped<ILoginInputPort, LoginInteractor>();

            return services;
        }
    }
}
