using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthWind.API.Services.APIClients;
using NorthWind.API.Services.APIServices;
using NorthWind.Sales.BusinessObjects.Interfaces.Services;
using NorthWind.Sales.BusinessObjects.POCOEntities;

namespace NorthWind.API.Services
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddAPIServices(
            this IServiceCollection services,
            IConfiguration configuration, string apiSettingsName)
        {

            services.Configure<APISettings>(configuration.GetSection(apiSettingsName));
            var apiSettings = configuration.GetSection(apiSettingsName).Get<APISettings>();



            services.AddHttpClient(apiSettings.WeatherAPI.ClientName, client =>
            {
                client.BaseAddress = new Uri(apiSettings.WeatherAPI.BaseAddress);
                // Configurar otros aspectos de seguridad, headers, etc.
            });

            services.AddScoped<IAPIHttpClientFactoryService, APIHttpClientFactoryService>();
            services.AddScoped<IWeatherAPIService, WeatherAPIService>();

            return services;
        }
    }
}
