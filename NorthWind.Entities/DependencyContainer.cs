using Microsoft.Extensions.DependencyInjection;
using NorthWind.Entities.Validators;

namespace NorthWind.Entities
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddEntityServices(
            this IServiceCollection services)
        {
            services.AddSingleton(typeof(IValidatorService<>),
                typeof(ValidatorService<>));

            return services;
        }
    }
}
