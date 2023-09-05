using Microsoft.Extensions.DependencyInjection;
using NorthWind.Sales.BusinessObjects.DTOs.CreateOrder;
using NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus;
using NorthWind.Sales.BusinessObjects.Interfaces.Repositories.Consumer;
using NorthWind.Sales.BusinessObjects.POCOEntities;
using NorthWind.Sales.UseCases.CreateOrder;

namespace NorthWind.IntegrationTest.Integration
{
    //public class RabbitMQBusProducerIntegrationTests : IClassFixture<ProducerWebApplicationFactory<Program>>
    public class RabbitMQBusProducerIntegrationTests : IAsyncLifetime //: IClassFixture<WebApplicationFactory<Program>>
    {

        // Para utilizar la fabrica con configuraciones para toda la clase
        //readonly ProducerWebApplicationFactory<Program> ApplicationFactory;

        //public RabbitMQBusProducerIntegrationTests(ProducerWebApplicationFactory<Program> factory)
        //{
        //    ApplicationFactory = factory;
        //}

        readonly ContainerFixtureIntegration ContainerFixture;

        public RabbitMQBusProducerIntegrationTests()
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
        public async Task Publish_WithCustomConnectionFactory_ProducesMessage()
        {

            //using var application = CustomApplicationBuilder.Build((services, configuration) => services.AddScoped<IEventBusProducer, RabbitMQBusProducer>());
            using var application = IntegrationApplicationBuilder.Build();
            using var scope = application.Services.CreateScope();

            // Arrange
            var rabbitMQBusProducer = application.Services.GetRequiredService<IEventBusProducer>();
            var rabbitMQBusConsumer = application.Services.GetRequiredService<IEventBusConsumer>();

            // Se utiliza un IServiceScope.ServiceProvider para resolver servicios scoped desde el proveedor raíz
            var repository = scope.ServiceProvider.GetRequiredService<INorthWindConsumerQuerysRepository>();


            await rabbitMQBusConsumer.Subscribe<OrderCreatedEvent, OrderCreatedEventHandler>();

            // Crear un evento para publicar 
            var orderDetails = new List<CreateOrderDetailDTO>
            {
                new CreateOrderDetailDTO { ProductId = 1001, UnitPrice = 10.0m, Quantity = 2 },
                new CreateOrderDetailDTO { ProductId = 1002, UnitPrice = 15.0m, Quantity = 3 }
            };
            var orderCreatedEvent = new OrderCreatedEvent
            {
                Id = 123,
                CustomerId = "C1234",
                ShipAddress = "1234 Main St",
                ShipCity = "City",
                ShipCountry = "Country",
                ShipPostalCode = "12345",
                OrderDetails = orderDetails
            };

            // Act
            // Publica el evento utilizando RabbitMQBusProducer
            await rabbitMQBusProducer.Publish(orderCreatedEvent);

            // Agrega alguna pausa o espera aquí si es necesario para darle tiempo a RabbitMQ para procesar el mensaje
            await Task.Delay(1000);
            // Assert
            // Verifica que el mensaje se haya producido correctamente
            // Esto podría incluir la verificación de que el mensaje se encuentra en una cola de RabbitMQ o que ha sido procesado correctamente


            await rabbitMQBusConsumer.Unsubscribe<OrderCreatedEvent, OrderCreatedEventHandler>();


            var registedOrder = await repository.GetOrderById(orderCreatedEvent.Id);
            Assert.NotNull(registedOrder);


        }


    }
}
