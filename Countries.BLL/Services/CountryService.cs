using Countries.BLL.ApiModels;
using Countries.BLL.Models;
using Countries.BLL.Services.Interfaces;

namespace Countries.BLL.Services
{
    public class CountryService : ICountryService
    {
        private readonly IExternalApiService _externalApiService;

        public CountryService(IExternalApiService externalApiService)
        {
            _externalApiService = externalApiService;
        }

        public async Task<(ICollection<Country> Countries, int TotalCount)> GetCountriesAsync(
            CountriesFilters filters,
            Sorting sorting,
            Pagination pagination)
        {
            ICollection<CountryResponseModel> countriesResponseData =
                await _externalApiService.ReadApiDataAsync(filters.CountryName);

            ICollection<Country> countries = Remap(countriesResponseData);

            CountryDataProcessor dataProcessor = new CountryDataProcessor(countries)
                .Filter(filters)
                .Sort(sorting)
                .Trim(pagination);

            return (dataProcessor.GetResult(), dataProcessor.GetTotalCount());
        }

        private static ICollection<Country> Remap(ICollection<CountryResponseModel>? countriesResponseData)
        {
            if (countriesResponseData is null || countriesResponseData.Count == 0)
            {
                return new List<Country>();
            }

            return countriesResponseData
                .Select(x =>
                    new Country(x.Name?.Common, x.Capital?.FirstOrDefault(), x.Region, x.Population, x.Area))
                .ToList();
        }
    }
}
