using Countries.BLL.ApiModels;
using Countries.BLL.Configurations;
using Countries.BLL.Exceptions;
using Countries.BLL.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Countries.BLL.Services;

public class ExternalApiService : IExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _externalApiUrl;

    public ExternalApiService(HttpClient httpClient, IOptions<ExternalApiSettings> externalApiSettings)
    {
        _httpClient = httpClient;

        if (externalApiSettings is null || string.IsNullOrWhiteSpace(externalApiSettings.Value.BaseUrl))
        {
            throw new InvalidSettingException("Base URL is not defined.");
        }

        _externalApiUrl = externalApiSettings.Value.BaseUrl;
    }

    public async Task<List<CountryResponseModel>> ReadApiDataAsync(string? countryName)
    {
        string apiUrl = DefineUrl(countryName);

        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();
        List<CountryResponseModel>? countriesData = JsonConvert.DeserializeObject<List<CountryResponseModel>>(content);

        return countriesData is null || countriesData.Count == 0
            ? new List<CountryResponseModel>()
            : countriesData;
    }

    private string DefineUrl(string? countryName)
    {
        return string.IsNullOrWhiteSpace(countryName)
            ? $"{_externalApiUrl}/all"
            : $"{_externalApiUrl}/name/{Uri.EscapeDataString(countryName)}";
    }
}