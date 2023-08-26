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

        public async Task<ICollection<Country>> GetCountriesAsync(CountriesFilters filters)
        {
            ICollection<CountryResponseModel> countriesResponseData =
                await _externalApiService.ReadApiDataAsync(filters.CountryName);

            ICollection<CountryResponseModel> filteredCountries = _responseProcessor
                .Filter(countriesResponseData, filters);

            return Remap(filteredCountries);
        }

        private static ICollection<Country> Remap(ICollection<CountryResponseModel>? countriesResponseData)
        {
            if (countriesResponseData is null || countriesResponseData.Count == 0)
            {
                return new List<Country>();
            }

            return countriesResponseData
                .Select(x =>
                    new Country(x.Name?.Official, x.Capital?.FirstOrDefault(), x.Region, x.Area))
                .ToList();
        }
    }
}
