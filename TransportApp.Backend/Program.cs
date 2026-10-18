using TransportApp.Infrastructure.Extensions;
using TransportApp.Backend.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.AddCorsConfiguration()
    .AddRepository()
    .AddAuthorizationWithSwagger("Transport App")
    .AddServices()
    .UseLogger()
    .AddWorkers()
    .AddApi();

var app = builder.Build();

app.ConfigureApplication();

// Minimal Apis
app.MapLocationEndpoints();
app.MapUserEndpoints();
app.MapRouteEndpoints();

app.Run();
