using Countries.BLL.ApiModels;
using Countries.BLL.Models;
using Countries.BLL.Services.Interfaces;

namespace Countries.BLL.Services
{
    public class CountryService : ICountryService
    {
        private readonly IExternalApiService _externalApiService;

        private readonly ResponseProcessor _responseProcessor = new();

        public CountryService(IExternalApiService externalApiService)
        {
            _externalApiService = externalApiService;
        }

        public async Task<ICollection<Country>> GetCountriesAsync(CountriesFilters filters, Sorting sorting)
        {
            ICollection<CountryResponseModel> countriesResponseData =
                await _externalApiService.ReadApiDataAsync(filters.CountryName);

            ICollection<CountryResponseModel> filteredCountries = _responseProcessor
                .Filter(countriesResponseData, filters);

            return RemapAndSort(filteredCountries, sorting);
        }

        private static ICollection<Country> RemapAndSort(
            ICollection<CountryResponseModel>? countriesResponseData,
            Sorting sorting)
        {
            if (countriesResponseData is null || countriesResponseData.Count == 0)
            {
                return new List<Country>();
            }

            IEnumerable<Country> remappedList = countriesResponseData
                .Select(x =>
                    new Country(x.Name?.Common, x.Capital?.FirstOrDefault(), x.Region, x.Population, x.Area));

            return sorting == Sorting.Ascend
                ? remappedList.OrderBy(x => x.Name).ToList()
                : remappedList.OrderByDescending(x => x.Name).ToList();
        }
    }
}
