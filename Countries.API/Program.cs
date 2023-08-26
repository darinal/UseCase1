using Countries.API;
using Countries.BLL.Configurations;
using Countries.BLL.Services;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

configuration.AddJsonFile("appsettings.json");
configuration.GetSection("ExternalApiSettings").Get<ExternalApiSettings>();

IServiceCollection services = builder.Services;
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Countries.API", Version = "v1" });
});

services.Configure<ExternalApiSettings>(configuration.GetSection("ExternalApiSettings"));
services.AddHttpClient();
services.AddScoped<ICountryService, CountryService>();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Countries.API v1"); });
app.MapEndpoints();

app.Run();