using Microsoft.Extensions.DependencyInjection;
using NorthWind.EFCore.Repositories.DataContexts;
using NorthWind.Sales.BusinessObjects.Aggregates;
using NorthWind.Sales.BusinessObjects.Interfaces.Repositories;

namespace NorthWind.IntegrationTest.Integration
{
    public class NorthWindSalesCommandsRepositoryTests : IAsyncLifetime
    {

        readonly ContainerFixtureIntegration ContainerFixture;

        public NorthWindSalesCommandsRepositoryTests()
        {
            ContainerFixture = new ContainerFixtureIntegration();
        }

        public Task InitializeAsync()
        {
            return ContainerFixture.InitializeAsync();
        }

        public Task DisposeAsync()
        {
            return ContainerFixture.DisposeAsync().AsTask();
        }


        [Fact]
        public async Task CreateOrder_AddsOrderAndDetailsToContext()
        {

            //using var application = CustomApplicationBuilder.Build((services, configuration) => services.AddScoped<IEventBusProducer, RabbitMQBusProducer>());
            using var application = IntegrationApplicationBuilder.Build();
            using var scope = application.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<NorthWindSalesContext>();

            // Para asegurar de que la base de datos física exista y tenga la estructura adecuada según el modelo de datos definido en el contexto
            context.Database.EnsureCreated();

            // Arrange
            // Se utiliza un IServiceScope.ServiceProvider para resolver servicios scoped desde el proveedor raíz
            var querysRepository = scope.ServiceProvider.GetRequiredService<INorthWindSalesQuerysRepository>();
            var commandsRepository = scope.ServiceProvider.GetRequiredService<INorthWindSalesCommandsRepository>();

            var orderAggregate = new OrderAggregate
            {
                //Id = 1,
                CustomerId = "C1234",
                ShipAddress = "1234 Main St",
                ShipCity = "Cityville",
                ShipCountry = "Countryland",
                ShipPostalCode = "12345",
            };

            orderAggregate.AddDetail(1001, 10.0m, 2);
            orderAggregate.AddDetail(1002, 15.0m, 3);


            // Act
            await commandsRepository.CreateOrder(orderAggregate);
            await commandsRepository.SaveChanges();

            // Assert
            var orderInContext = await querysRepository.GetOrderById(orderAggregate.Id);
            Assert.NotNull(orderInContext);
            Assert.Equal(orderAggregate.Id, orderInContext.Id);

        }

    }
}
