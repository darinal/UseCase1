using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add Swagger services
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Countries.API", Version = "v1" });
});

var app = builder.Build();

// Inside the Configure method
app.UseSwagger();
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Countries.API v1");
});

app.MapGet("/", () => "Hello World!");

app.Run();
