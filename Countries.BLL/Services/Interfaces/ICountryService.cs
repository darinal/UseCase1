using Countries.BLL.Models;

namespace Countries.BLL.Services.Interfaces;

public interface ICountryService
{
    Task<(ICollection<Country> Countries, int TotalCount)> GetCountriesAsync(
        CountriesFilters filters,
        Sorting sorting,
        Pagination pagination);
}