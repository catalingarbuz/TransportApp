using TransportApp.Infrastructure.Extensions;
using TransportApp.Backend.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddCorsConfiguration()
    .AddRepository()
    .AddAuthorizationWithSwagger("Transport App")
    .AddServices()
    .UseLogger()
    .AddWorkers()
    .AddApi()
    .AddRedisCache();

var app = builder.Build();

app.ConfigureApplication();

// Minimal Apis
app.MapAuthorizationEndpoints();
app.MapBookingEndpoints();
app.MapCarEndpoints();
app.MapCarRouteEndpoints();
app.MapDriverEndpoints();
app.MapDriverInfoEndpoints();
app.MapLocationEndpoints();
app.MapRouteEndpoints();
app.MapUserEndpoints();
app.MapUserFileEndpoints();


app.Run();
