using System;
using Countries.BLL.ApiModels;
using Countries.BLL.Configurations;
using Countries.BLL.Services;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq.Protected;
using Xunit;

namespace Countries.UnitTests;

public class ExternalApiServiceTests
{
    [Fact]
    public async Task ReadApiDataAsync_ValidResponse_ReturnsCountryData()
    {
        // Arrange
        Mock<HttpMessageHandler> httpMessageHandlerMock = new();
        HttpClient httpClient = new(httpMessageHandlerMock.Object)
        {
            BaseAddress = new("https://example.com")
        };

        IOptions<ExternalApiSettings> externalApiSettings = Options.Create(new ExternalApiSettings
        {
            BaseUrl = "https://example.com/api"
        });

        List<CountryResponseModel> expectedResponse = new()
        {
            new() {Name = new() {Common = "Country1"}},
            new() {Name = new() {Common = "Country2"}}
        };

        HttpResponseMessage httpResponseMessage = new(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(expectedResponse))
        };

        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(httpResponseMessage);

        ExternalApiService externalApiService = new(httpClient, externalApiSettings);

        // Act
        List<CountryResponseModel> result = await externalApiService.ReadApiDataAsync("Country");

        // Assert
        Assert.Equal(expectedResponse.Count, result.Count);
        Assert.Equal(expectedResponse[0].Name, result[0].Name);
        Assert.Equal(expectedResponse[1].Name, result[1].Name);
    }

    [Fact]
    public async Task ReadApiDataAsync_EmptyResponse_ReturnsEmptyList()
    {
        Mock<HttpMessageHandler> httpMessageHandlerMock = new();
        HttpClient httpClient = new(httpMessageHandlerMock.Object)
        {
            BaseAddress = new("https://example.com")
        };

        IOptions<ExternalApiSettings> externalApiSettings = Options.Create(new ExternalApiSettings
        {
            BaseUrl = "https://example.com/api"
        });

        List<CountryResponseModel> expectedResponse = new()
        {
            new() {Name = new() {Common = "Country1"}},
            new() {Name = new() {Common = "Country2"}}
        };

        HttpResponseMessage httpResponseMessage = new(HttpStatusCode.OK)
        {
            Content = new StringContent("")
        };

        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(httpResponseMessage);

        ExternalApiService externalApiService = new(httpClient, externalApiSettings);

        // Act
        List<CountryResponseModel> result = await externalApiService.ReadApiDataAsync(null);

        // Assert
        Assert.Empty(result);
    }
}