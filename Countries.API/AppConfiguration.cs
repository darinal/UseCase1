using Countries.BLL.Configurations;
using Countries.BLL.Services;
using Countries.BLL.Services.Interfaces;
using Microsoft.OpenApi.Models;

namespace Countries.API;

public static class AppConfiguration
{
    public static void ConfigureAppServices(this WebApplicationBuilder webApplicationBuilder)
    {
        IConfigurationSection GetApiSettings(ConfigurationManager configurationManager)
        {
            configurationManager.AddJsonFile("appsettings.json");
            IConfigurationSection configurationSection = configurationManager.GetSection("ExternalApiSettings");

            return configurationSection;
        }

        IServiceCollection services = webApplicationBuilder.Services;
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Countries.API", Version = "v1" }); });

        services.Configure<ExternalApiSettings>(GetApiSettings(webApplicationBuilder.Configuration));
        services.AddHttpClient();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IExternalApiService, ExternalApiService>();
    }

    public static void ConfigureSwaggerEndpoint(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Countries.API v1"); });
    }
}