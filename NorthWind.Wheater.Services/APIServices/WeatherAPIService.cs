using NorthWind.Entities.POCOEntities;
using NorthWind.Sales.BusinessObjects.Interfaces.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace NorthWind.API.Services.APIServices
{
    public class WeatherAPIService : IWeatherAPIService
    {
        private readonly HttpClient HttpClient;

        public WeatherAPIService(IAPIHttpClientFactoryService clientFactory)
        {
            HttpClient = clientFactory.CreateClient("WeatherAPI");
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(string accesToken)
        {
            var result = new List<WeatherForecast>();

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesToken);
            var response = await HttpClient.GetAsync($"api/WeatherForecast");


            // Aquí maneja la respuesta y deserializa los datos
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<List<WeatherForecast>>();
            }
            return result;
        }


    }
}
