using NorthWind.Sales.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Registrar los servicios de la aplicaci�n
builder.Services.AddNorthWindSalesServices(
    builder.Configuration, "JwtSettings", "NorthWindDB", "APISettings", "RabbitMQSettingsProducer");

builder.Services.AddControllers();

// Configurar APIExplorer para descubrir y exponer
// los metadatos de los endpoints de la aplicaci�n.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Agregar el generador que construye los objetos de
// documentaci�n de Swagger con la funcionalidad del APIExplorer.
builder.Services.AddSwaggerGen();

// Agregar el servicio CORS para clientes que se ejecutan
// en el navegador Web (como Blazor WebAssembly).
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(config =>
    {
        config.AllowAnyMethod();
        config.AllowAnyHeader();
        config.AllowAnyOrigin();
    });
});

var app = builder.Build();

// Habilitar el middleware para servir el documento
// JSON generado y la interfaz UI de Swagger en el
// ambiente de desarrollo.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}


// Agregar el Middleware CORS
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }