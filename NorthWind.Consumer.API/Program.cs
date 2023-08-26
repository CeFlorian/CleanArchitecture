using NorthWind.Sales.IoC;
using NorthWindRabbitMQConsumer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Registrar los servicios de la aplicaci�n
builder.Services.AddNorthWindConsumerServices(
    builder.Configuration, "MongoDB", "MongoDBSettings", "RabbitMQSettings");

// Opcion No.2 - Usar un servicio hospedado:
builder.Services.AddHostedService<ConsumerHostedService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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


app.UseAuthorization();

app.MapControllers();

// Opcion No.1 - Suscripci�n directa en el m�todo Configure:
//var eventBus = app.Services.GetRequiredService<IEventBus>();
//eventBus.Subscribe<OrderCreatedEvent, OrderCreatedEventHandler>();

app.Run();
