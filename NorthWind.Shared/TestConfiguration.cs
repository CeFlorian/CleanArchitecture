using Microsoft.Extensions.Configuration;

namespace NorthWind.Shared
{
    public static class TestConfiguration
    {
        public static IConfigurationRoot Get(Dictionary<string, string>? keyValues = null)
        {
            var configuration =
                keyValues?.Count > 0
                ?
                    new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.test.json")
                    .AddInMemoryCollection(keyValues)
                    .Build()
                :
                    new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.test.json")
                    .Build();
            return configuration;
        }
    }
}
