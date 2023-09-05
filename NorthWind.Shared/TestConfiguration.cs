using Microsoft.Extensions.Configuration;

namespace NorthWind.Shared
{
    public static class TestConfiguration
    {
        public static IConfigurationRoot Get()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json")
                .Build();
            return configuration;
        }
    }
}
