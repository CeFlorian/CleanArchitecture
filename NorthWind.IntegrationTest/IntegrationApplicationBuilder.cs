using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using NorthWind.Mongo.Repositories;
using NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus;
using NorthWind.Sales.BusinessObjects.Interfaces.Repositories.Consumer;
using NorthWind.Sales.UseCases.CreateOrder;
using NorthWind.Shared;
using NorthWindRabbitMQConsumer.Services;

namespace NorthWind.IntegrationTest
{
    public static class IntegrationApplicationBuilder
    {
        public static WebApplicationFactory<Program> Build(Action<IServiceCollection, IConfiguration> configureServices, Dictionary<string, string?>? keyValues = null)
            => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {

                IConfigurationRoot testConfiguration = TestConfiguration.Get();

                builder.UseConfiguration(testConfiguration);


                //builder.ConfigureTestServices(services => configureServices(services, testConfiguration));
                builder.ConfigureTestServices(services =>
                {

                    //services.AddAuthentication().AddBasicAuthentication(credentials => Task.FromResult(credentials.username == Username && credentials.password == Password));
                    //services.AddAuthorization(config =>
                    //{
                    //    config.DefaultPolicy = new AuthorizationPolicyBuilder(config.DefaultPolicy).AddAuthenticationSchemes(BasicAuthenticationDefaults.AuthenticationScheme).Build();
                    //});


                    // Agrregar servicios necesarios para las pruebas y que no estan registrados en el contenedor de dependencias de la aplicacion a la que hace referencia la clase Program

                    // MongoDB Consumer
                    services.Configure<MongoDBSettings>(testConfiguration.GetRequiredSection("MongoDBSettings"));
                    var mongoDBSettings = testConfiguration.GetRequiredSection("MongoDBSettings").Get<MongoDBSettings>();

                    services.AddTransient(options =>
                    {
                        var client = new MongoClient(testConfiguration.GetConnectionString("MongoDB"));
                        return client.GetDatabase(mongoDBSettings.Database);
                    });

                    services.AddScoped<INorthWindConsumerCommandsRepository, NorthWindConsumerCommandsRepository>();
                    services.AddScoped<INorthWindConsumerQuerysRepository, NorthWindConsumerQuerysRepository>();

                    // RabbitMQ Consumer
                    services.Configure<RabbitMQSettingsConsumer>(testConfiguration.GetRequiredSection("RabbitMQSettingsConsumer"));
                    var rabbitMQSettings = testConfiguration.GetRequiredSection("RabbitMQSettingsConsumer").Get<RabbitMQSettingsConsumer>();

                    //Domain Bus
                    services.AddTransient<IEventBusConsumer, RabbitMQBusConsumer>(sp =>
                    {
                        var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                        return new RabbitMQBusConsumer(scopeFactory, rabbitMQSettings, sp.GetRequiredService<ILogger<RabbitMQBusConsumer>>(), sp.GetRequiredService<IRabbitMqConsumerConnectionService>());
                    });

                    //Subscriptions
                    services.AddTransient<OrderCreatedEventHandler>();

                    services.AddSingleton<IRabbitMqConsumerConnectionService, RabbitMqConsumerConnectionService>();

                });



                builder.ConfigureServices(services => configureServices(services, testConfiguration));
                //builder.ConfigureServices(services =>
                //{


                //});

                if (keyValues?.Count > 0)
                {
                    builder.ConfigureAppConfiguration((context, configBuilder) =>
                    {
                        configBuilder.AddInMemoryCollection(keyValues);

                        //configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
                        //{
                        //    ["RawConfigProperty"] = "OverriddenValue"
                        //});

                    });
                }


            });

        //Anular propiedad con colección en memoria utilizando Dictionary<string, string?>
        public static WebApplicationFactory<Program> Build(Dictionary<string, string?>? keyValues = null)
        {
            /* Si la llamada al metodo Build no trae el parametro ("Action<IServiceCollection, IConfiguration> configureServices" que es un delegado, action, lambda), como resultado,
               en vez de la accion configureServices(services, testConfiguration) se asignara una accion vacia (un lambda vacío) { } que no realiza ninguna operacion en services, por lo tanto,
               se crea una instancia con una configuración predeterminada para la aplicacion a la que hace referencia la clase Program
            */
            return Build((_, __) => { }, keyValues);
        }

    }
}
