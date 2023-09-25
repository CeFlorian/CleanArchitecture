using NorthWind.Sales.BusinessObjects.Interfaces.Services;

namespace NorthWind.API.Services.APIClients
{
    public class APIHttpClientFactoryService : IAPIHttpClientFactoryService
    {
        private readonly IHttpClientFactory HttpClientFactory;

        public APIHttpClientFactoryService(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        public HttpClient CreateClient(string clientName)
        {
            return HttpClientFactory.CreateClient(clientName);
        }
    }
}
