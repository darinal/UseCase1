using Countries.BLL.Models;
using Countries.BLL.Services;
using Microsoft.AspNetCore.Http;

namespace Countries.API;

public static class AppEndpoints
{
    public static void MapAppEndpoints(this WebApplication app)
    {
        app.MapGet("/countries", async (ICountryService countryService, HttpContext httpContext) =>
        {
            string countryName = httpContext.Request.Query["name"].ToString();

            try
            {
                ICollection<Country> countries = await countryService.GetCountriesAsync(countryName);
                return Results.Ok(countries);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.Ok("Not now, sorry");
            }
        });
    }
}