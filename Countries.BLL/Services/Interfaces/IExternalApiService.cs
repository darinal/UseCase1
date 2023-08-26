using Countries.BLL.ApiModels;

namespace Countries.BLL.Services.Interfaces;

public interface IExternalApiService
{
    Task<List<CountryResponseModel>> ReadApiDataAsync(string? countryName);
}