using Countries.BLL.ApiModels;
using Countries.BLL.Configurations;
using Countries.BLL.Exceptions;
using Countries.BLL.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Countries.BLL.Services
{
    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;
        private readonly string _externalApiUrl;

        public CountryService(HttpClient httpClient, IOptions<ExternalApiSettings> externalApiSettings)
        {
            _httpClient = httpClient;

            if (externalApiSettings is null || string.IsNullOrWhiteSpace(externalApiSettings.Value.BaseUrl))
            {
                throw new InvalidSettingException("Base URL is not defined.");
            }

            _externalApiUrl = externalApiSettings.Value.BaseUrl;
        }

        public async Task<ICollection<Country>> GetCountriesAsync(string countryName)
        {
            ICollection<CountryResponseModel>? countriesResponseData = await ReadApiDataAsync(countryName);

            ICollection<Country> countries = Remap(countriesResponseData);

            return countries;
        }

        private async Task<ICollection<CountryResponseModel>?> ReadApiDataAsync(string countryName)
        {
            string apiUrl = DefineUrl(countryName);

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            List<CountryResponseModel>? countriesData = JsonConvert.DeserializeObject<List<CountryResponseModel>>(content);

            return countriesData;
        }

        private string DefineUrl(string countryName)
        {
            return  string.IsNullOrWhiteSpace(countryName)
                ? $"{_externalApiUrl}/all"
                : $"{_externalApiUrl}/name/{Uri.EscapeDataString(countryName)}";
        }

        private static ICollection<Country> Remap(ICollection<CountryResponseModel>? countriesResponseData)
        {
            if (countriesResponseData is null || countriesResponseData.Count == 0)
            {
                return new List<Country>();
            }

            List<Country> countries = countriesResponseData
                .Select(x => new Country(x.Name?.Official, x.Capital?.FirstOrDefault(), x.Region, x.Area))
                .ToList();

            return countries;
        }
    }
}
