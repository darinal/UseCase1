using Countries.BLL.Models;
using Countries.BLL.Services.Interfaces;
using Countries.API.Extensions;
using System.Text.Json;
using Countries.API.Models;

namespace Countries.API;

public static class AppEndpoints
{
    public static void MapAppEndpoints(this WebApplication app)
    {
        app.MapGet("/countries", async (ICountryService countryService, HttpContext httpContext) =>
        {
            try
            {
                IQueryCollection queryParams = httpContext.Request.Query;

                CountriesFilters countryName = DefineFilters(queryParams);
                Sorting sorting = DefineSorting(queryParams);
                Pagination pagination = DefinePagination(queryParams);

                (ICollection<Country> Countries, int TotalCount) result =
                    await countryService.GetCountriesAsync(countryName, sorting, pagination);

                string prettifyResponse = PrettifyResponse(result, pagination);
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsync(prettifyResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                httpContext.Response.ContentType = "text/plain";
                await httpContext.Response.WriteAsync("Don't go there");
            }
        });
    }

    private static CountriesFilters DefineFilters(IQueryCollection queryCollection)
    {
        string? countryName = queryCollection.GetString("name");
        int? population = queryCollection.GetPositiveInt("population");

        CountriesFilters filter = new CountriesFilters(countryName, population);

        return filter;
    }

    private static Sorting DefineSorting(IQueryCollection queryParams)
    {
        string? sortValue = queryParams.GetString("sort");

        if (Enum.TryParse(sortValue, ignoreCase: true, out Sorting parsedSort))
        {
            return parsedSort;
        }

        return Sorting.Ascend;
    }

    private static Pagination DefinePagination(IQueryCollection queryParams)
    {
        int page = GetParameter("page", 1);
        int pageSize = GetParameter("pageSize", 10);
        
        return new Pagination(page, pageSize);

        int GetParameter(string paramName, int defaultValue)
        {
            int? value = queryParams.GetPositiveInt(paramName);

            return value is null or <= 0 ? defaultValue : value.Value;
        }
    }

    private static string PrettifyResponse((ICollection<Country> Countries, int TotalCount) result, Pagination pagination)
    {
        JsonSerializerOptions jsonOptions = new JsonSerializerOptions { WriteIndented = true, PropertyNameCaseInsensitive = true };

        CountryResponse response = new(
            result.Countries,
            result.TotalCount,
            pagination.Page,
            pagination.PageSize);

        string json = JsonSerializer.Serialize(response, jsonOptions);

        return json;
    }
}