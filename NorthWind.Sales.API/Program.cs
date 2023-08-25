using NorthWind.Sales.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Registrar los servicios de la aplicación
builder.Services.AddNorthWindSalesServices(
    builder.Configuration, "NorthWindDB", "MongoDB");

builder.Services.AddControllers();

// Configurar APIExplorer para descubrir y exponer
// los metadatos de los endpoints de la aplicación.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Agregar el generador que construye los objetos de
// documentación de Swagger con la funcionalidad del APIExplorer.
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
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}


// Agregar el Middleware CORS
app.UseCors();


app.UseAuthorization();

app.MapControllers();

app.Run();
