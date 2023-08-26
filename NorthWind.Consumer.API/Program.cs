using NorthWind.Sales.IoC;
using NorthWindRabbitMQConsumer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Registrar los servicios de la aplicación
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

// Opcion No.1 - Suscripción directa en el método Configure:
//var eventBus = app.Services.GetRequiredService<IEventBus>();
//eventBus.Subscribe<OrderCreatedEvent, OrderCreatedEventHandler>();

app.Run();
