using Countries.BLL.Models;
using Countries.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Countries.API.Models;
using Xunit;

namespace Countries.UnitTests;

public class AppEndpointsTests
{
    [Fact]
    public async void GetAllCustomersTest()
    {
        // Arrange
        ICollection<Country> countries = new List<Country>
        {
            new("Country A", "Capital A", "Region X", 1000, 200.5),
            new("Country B", "Capital A", "Region X", 1500, 300.7),
        };

        Mock<ICountryService> externalApiServiceMock = new();

        externalApiServiceMock
            .Setup(service => service
                .GetCountriesAsync(It.IsAny<CountriesFilters>(), It.IsAny<Sorting>(), It.IsAny<Pagination>()))
            .ReturnsAsync(() => new(countries, 2));

        await using WebApplicationFactory<Program> application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
            {
                services.AddScoped(x => externalApiServiceMock.Object);
            }));

        using HttpClient client = application.CreateClient();

        // Act
        HttpResponseMessage response = await client.GetAsync("/countries");

        // Assert
        string data = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Assert against the expected JSON structure
        string expectedJson = @"{
  ""Page"": 1,
  ""PageSize"": 10,
  ""TotalCount"": 2,
  ""Items"": [
    {
      ""Name"": ""Country A"",
      ""Capital"": ""Capital A"",
      ""Region"": ""Region X"",
      ""Population"": 1000,
      ""Area"": 200.5
    },
    {
      ""Name"": ""Country B"",
      ""Capital"": ""Capital A"",
      ""Region"": ""Region X"",
      ""Population"": 1500,
      ""Area"": 300.7
    }
  ]
}";

        Assert.Equal(expectedJson, data);
    }
}