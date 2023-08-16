using Microsoft.Extensions.DependencyInjection;
using NorthWind.Entities.Validators;

namespace NorthWind.Sales.BusinessObjects
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddDTOValidators(
            this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateOrderDTO>,
                CreateOrderDTOValidator>();
            return services;
        }
    }
}
