using Countries.BLL.Models;
using Countries.BLL.Services;

namespace Countries.API
{
    public static class Endpoints
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapGet("/countries", async (ICountryService countryService) =>
            {
                try
                {
                    ICollection<Country> countries = await countryService.GetCountriesAsync();
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
}
