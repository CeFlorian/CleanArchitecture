using NorthWind.Entities.POCOEntities;

namespace NorthWind.Sales.BusinessObjects.Interfaces.Services
{
    public interface IWeatherAPIService
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(string accesToken);
    }
}
