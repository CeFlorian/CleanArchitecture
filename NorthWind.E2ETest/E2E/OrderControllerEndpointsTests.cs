using Microsoft.Extensions.DependencyInjection;
using NorthWind.EFCore.Repositories.DataContexts;
using NorthWind.Sales.BusinessObjects.DTOs.CreateOrder;
using NorthWind.Sales.BusinessObjects.POCOEntities;
using System.Net.Http.Json;

namespace NorthWind.E2ETest.E2E
{
    public class OrderControllerEndpointsTests : IAsyncLifetime
    {

        readonly ContainerFixtureE2E ContainerFixture;

        public OrderControllerEndpointsTests()
        {
            ContainerFixture = new ContainerFixtureE2E();
        }

        public Task InitializeAsync()
        {

            // Inicializa los recursos necesarios para las pruebas
            return ContainerFixture.InitializeAsync();

            // Puedes agregar aquí cualquier otra inicialización específica de las pruebas
        }

        public Task DisposeAsync()
        {
            // Realiza la limpieza de recursos después de las pruebas
            return ContainerFixture.DisposeAsync().AsTask();

            // Puedes agregar aquí cualquier otra limpieza específica de las pruebas
        }


        /* 
         * Para este test no se esta valdiando ni la suscripcion del consumidor ni el consumo del evento para su procesamiento, 
         * solamente llegamos hasta la publicacion del evento
        */
        [Fact]
        public async Task Post_Endpoint_Returns_SuccessStatusCode_And_Get_Endpoint()
        {

            using var application = E2EApplicationBuilder.Build();
            using var client = application.CreateClient();

            using var scope = application.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<NorthWindSalesContext>();

            // Para asegurar de que la base de datos física exista y tenga la estructura adecuada según el modelo de datos definido en el contexto
            context.Database.EnsureCreated();


            var orderDetails = new List<CreateOrderDetailDTO>
            {
                new CreateOrderDetailDTO { ProductId = 1, UnitPrice = 10.99M, Quantity = 2 },
                new CreateOrderDetailDTO { ProductId = 2, UnitPrice = 5.99M, Quantity = 3 }
            };

            var order = new CreateOrderDTO
            {
                CustomerId = "12345",
                ShipAddress = "1234 Main St",
                ShipCity = "City",
                ShipCountry = "Country",
                ShipPostalCode = "44444",
                OrderDetails = orderDetails
            };

            // Envia la solicitud POST al controlador
            var responsePost = await client.PostAsJsonAsync("/api/Order", order);

            // Verifica que la respuesta sea exitosa (código 200 OK)
            responsePost.EnsureSuccessStatusCode();

            var orderId = responsePost.Content.ReadFromJsonAsync<ResponseOrder<int>>().Result.Value;

            // Verifica que el ID del pedido sea el primero registrado en la base de datos ya que el Identity inicia en 1
            Assert.Equal(1, orderId);

            // Envia la solicitud GET a la ruta del controlador
            var responseGet = await client.GetAsync("/api/Order");

            // Verifica que la respuesta sea exitosa (código 200 OK)
            responseGet.EnsureSuccessStatusCode();

            var orders = responseGet.Content.ReadFromJsonAsync<ResponseOrder<IEnumerable<Order>>>().Result.Value;

            // Verifica que se haya devuelto una colección no nula y que contenga al menos un elemento
            Assert.NotNull(orders);
            Assert.NotEmpty(orders);
            Assert.True(orders.Count() == 1);
            Assert.Equal(1, orders.Single().Id);

        }


        public class ResponseOrder<T>
        {
            public T Value { get; set; }
            public int StatusCode { get; set; }
            public string ContentType { get; set; }

        }


    }


}