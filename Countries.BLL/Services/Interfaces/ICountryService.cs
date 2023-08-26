using Countries.BLL.Models;

namespace Countries.BLL.Services.Interfaces;

public interface ICountryService
{
    Task<ICollection<Country>> GetCountriesAsync(CountriesFilters filters, Sorting sorting);
}