using Countries.BLL.Models;
using Countries.BLL.Services.Interfaces;
using Countries.API.Extensions;
using System.Text.Json;

namespace Countries.API;

public static class AppEndpoints
{
    public static void MapAppEndpoints(this WebApplication app)
    {
        app.MapGet("/countries", async (ICountryService countryService, HttpContext httpContext) =>
        {
            try
            {
                CountriesFilters countryName = DefineFilters(httpContext.Request.Query);

                ICollection<Country> countries = await countryService.GetCountriesAsync(countryName);

                string prettifyResponse = PrettifyResponse(countries);
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsync(prettifyResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                httpContext.Response.ContentType = "text/plain";
                await httpContext.Response.WriteAsync("Not now, sorry");
            }
        });
    }

    private static CountriesFilters DefineFilters(IQueryCollection queryCollection)
    {
        string? countryName = queryCollection.GetString("name");
        int? population = queryCollection.GetInt("population");

        CountriesFilters filter = new CountriesFilters(countryName, population);

        return filter;
    }

    private static string PrettifyResponse(ICollection<Country> countries)
    {
        JsonSerializerOptions jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(new { countries.Count, Countries = countries }, jsonOptions);

        return json;
    }
}