
//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

//app.Run();

WebApplication.CreateBuilder(args)
    .CreateWebApplication()
    .ConfigureWebApplication()
    .Run();
