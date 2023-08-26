using Countries.BLL.Models;

namespace Countries.BLL.Services;

public interface ICountryService
{
    Task<ICollection<Country>> GetCountriesAsync();
}