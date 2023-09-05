using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace NorthWind.Shared
{
    public class ProducerWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {

                // Podemos personalizar más la configuración de nuestra aplicación aquí
                // Utilizar un appsettings diferente dio problema desde aqui 

                //services.AddDbContext<NorthWindSalesContext>(options =>
                //   options.UseInMemoryDatabase("OrdersDbContextInMemoryTest"));

                //var sp = services.BuildServiceProvider();
                //using var scope = sp.CreateScope();

                //var scopedServices = scope.ServiceProvider;
                //var context = scopedServices.GetRequiredService<NorthWindSalesContext>();
                //context.Database.EnsureCreated();

            });

            //builder.UseEnvironment("Development");

        }
    }
}
