namespace NorthWind.Sales.BusinessObjects.POCOEntities
{
    public class APISettings
    {
        public WeatherAPI WeatherAPI { get; set; }
    }

    public class WeatherAPI
    {
        public string BaseAddress { get; set; }
        public string ClientName { get; set; }
    }
}
