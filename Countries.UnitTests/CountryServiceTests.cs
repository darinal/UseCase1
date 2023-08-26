using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Countries.BLL.ApiModels;
using Countries.BLL.Models;
using Countries.BLL.Services;
using Countries.BLL.Services.Interfaces;
using Moq;
using Xunit;

namespace Countries.UnitTests;

public class CountryServiceTests
{
    [Fact]
    public async Task GetCountriesAsync_ShouldReturnFilteredAndProcessedData()
    {
        // Arrange
        Mock<IExternalApiService> externalApiServiceMock = new();
        externalApiServiceMock
            .Setup(service => service.ReadApiDataAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<CountryResponseModel>
            {
                new()
                {
                    Name = new() { Common = "Country A" },
                    Capital = new [] { "Capital A" },
                    Region = "Region X",
                    Population = 1000,
                    Area = 200.5
                },
                new()
                {
                    Name = new() { Common = "Country B" },
                    Capital = new [] { "Capital B" },
                    Region = "Region Y",
                    Population = 1500,
                    Area = 300.7
                }
            });

        CountryService countryService = new(externalApiServiceMock.Object);

        CountriesFilters filters = new("Country A", 1000);

        Sorting sorting = new();
        Pagination pagination = new(1, 10);

        // Act
        (ICollection<Country> Countries, int TotalCount) result =
            await countryService.GetCountriesAsync(filters, sorting, pagination);

        // Assert
        Assert.Equal(2, result.Countries.Count);
        Assert.Equal("Country A", result.Countries.First().Name);
        Assert.Equal("Capital A", result.Countries.First().Capital);
        Assert.Equal("Region X", result.Countries.First().Region);
        Assert.Equal(1000, result.Countries.First().Population);
        Assert.Equal(200.5, result.Countries.First().Area);
        Assert.Equal(2, result.TotalCount);
    }
}