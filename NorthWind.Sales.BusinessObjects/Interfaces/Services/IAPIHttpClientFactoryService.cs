namespace NorthWind.Sales.BusinessObjects.Interfaces.Services
{
    public interface IAPIHttpClientFactoryService
    {
        HttpClient CreateClient(string clientName);

    }
}
