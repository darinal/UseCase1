using Countries.API;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.ConfigureAppServices();

WebApplication app = builder.Build();
app.ConfigureSwaggerEndpoint();
app.MapAppEndpoints();

app.Run();