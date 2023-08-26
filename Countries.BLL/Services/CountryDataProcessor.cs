using Countries.BLL.Exceptions;
using Countries.BLL.Models;

namespace Countries.BLL.Services;

public class CountryDataProcessor
{
    private ICollection<Country> _countries;

    private int _totalCountryCount;

    public CountryDataProcessor (ICollection<Country> countries)
    {
        _countries = countries;
        _totalCountryCount = countries.Count;
    }

    public CountryDataProcessor Filter(CountriesFilters filters)
    {
        if (_countries.Count == 0)
        {
            return this;
        }

        if (filters.Population.HasValue)
        {
            double population = filters.Population.Value * 1000000;

            _countries = _countries.Where(country => country.Population <= population).ToList();

            _totalCountryCount = _countries.Count;
        }

        return this;
    }

    public CountryDataProcessor Sort(Sorting sorting)
    {
        if (_countries.Count == 0)
        {
            return this;
        }

        _countries = sorting == Sorting.Ascend
            ? _countries.OrderBy(x => x.Name).ToList()
            : _countries.OrderByDescending(x => x.Name).ToList();

        return this;
    }

    public CountryDataProcessor Trim(Pagination pagination)
    {
        if (_countries.Count == 0)
        {
            return this;
        }

        int startIndex = (pagination.Page - 1) * pagination.PageSize;

        if (startIndex >= _totalCountryCount)
        {
            throw new StepOverOffsetException();
        }

        int endIndex = startIndex + pagination.PageSize;

        if (startIndex < 0)
        {
            startIndex = 0;
        }

        if (endIndex > _countries.Count)
        {
            endIndex = _countries.Count;
        }

        _countries = _countries.Skip(startIndex).Take(endIndex - startIndex).ToList();

        return this;
    }

    public ICollection<Country> GetResult() => _countries;

    public int GetTotalCount() => _totalCountryCount;
}