using Countries.BLL.ApiModels;
using Countries.BLL.Models;

namespace Countries.BLL.Services;

public class ResponseProcessor
{
    private ICollection<CountryResponseModel> _countries = null!;

    private CountriesFilters _filters = null!;

    public ICollection<CountryResponseModel> Filter(
        ICollection<CountryResponseModel> countries,
        CountriesFilters filters)
    {
        if (countries.Count == 0)
        {
            return new List<CountryResponseModel>();
        }

        _countries = countries;
        _filters = filters;

        _countries = FilterByPopulation();

        return _countries;
    }

    private ICollection<CountryResponseModel> FilterByPopulation()
    {
        if (_filters.Population.HasValue)
        {
            double population = _filters.Population.Value * 1000000;

            return _countries.Where(country => country.Population < population).ToList();
        }

        return _countries;
    }
}