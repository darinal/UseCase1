using Countries.BLL.Models;
using Countries.BLL.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Countries.UnitTests;

public class CountryDataProcessorTests
{
    [Fact]
    public void Filter_NoCountries_ReturnsSameInstance()
    {
        // Arrange
        CountryDataProcessor processor = new(new List<Country>());

        CountriesFilters filters = new("", 15);

        // Act
        CountryDataProcessor result = processor.Filter(filters);

        // Assert
        Assert.Same(processor, result);
    }

    [Fact]
    public void Filter_PopulationFilterApplied_CorrectFilteringAndCount()
    {
        // Arrange
        List<Country> countries = new()
        {
            new("Country1", "Capital1", "Region1", 5000000, 1000),
            new("Country2", "Capital2", "Region2", 10000000, 1500),
            new("Country3", "Capital3", "Region3", 20000000, 2000)
        };

        CountryDataProcessor processor = new(countries);

        CountriesFilters filters = new("", 15);

        // Act
        processor.Filter(filters);

        // Assert
        Assert.Equal(2, processor.GetTotalCount());
        Assert.Equal(2, processor.GetResult().Count);

        foreach (var country in processor.GetResult())
        {
            Assert.True(country.Population <= 15000000);
        }
    }

    [Fact]
    public void Sort_Ascending_SortsByNameAscending()
    {
        // Arrange
        List<Country> countries = new()
        {
            new("B", "Capital1", "Region1", 100, 500),
            new("C", "Capital2", "Region2", 200, 600),
            new("A", "Capital3", "Region3", 300, 700),
        };

        CountryDataProcessor processor = new(countries);

        Sorting sorting = Sorting.Ascend;

        // Act
        processor.Sort(sorting);

        // Assert
        ICollection<Country> resultCountries = processor.GetResult();

        Assert.Equal("A", resultCountries.ElementAt(0).Name);
        Assert.Equal("B", resultCountries.ElementAt(1).Name);
        Assert.Equal("C", resultCountries.ElementAt(2).Name);
    }

    [Fact]
    public void Sort_Descending_SortsByNameDescending()
    {
        // Arrange
        List<Country> countries = new()
        {
            new("B", "Capital1", "Region1", 100, 500),
            new("C", "Capital2", "Region2", 200, 600),
            new("A", "Capital3", "Region3", 300, 700),
        };

        CountryDataProcessor processor = new(countries);

        Sorting sorting = Sorting.Descend;

        // Act
        processor.Sort(sorting);

        // Assert
        ICollection<Country> resultCountries = processor.GetResult();

        Assert.Equal("C", resultCountries.ElementAt(0).Name);
        Assert.Equal("B", resultCountries.ElementAt(1).Name);
        Assert.Equal("A", resultCountries.ElementAt(2).Name);
    }

    [Fact]
    public void Sort_EmptyList_ReturnsSameInstance()
    {
        // Arrange
        List<Country> countries = new();

        CountryDataProcessor processor = new(countries);

        Sorting sorting = Sorting.Ascend;

        // Act
        CountryDataProcessor result = processor.Sort(sorting);

        // Assert
        Assert.Same(processor, result);
        Assert.Empty(processor.GetResult());
    }

    [Fact]
    public void Trim_ValidPagination_TrimsCorrectly()
    {
        // Arrange
        List<Country> countries = new()
        {
            new("Country1", "Capital1", "Region1", 100, 500),
            new("Country2", "Capital2", "Region2", 200, 600),
            new("Country3", "Capital3", "Region3", 300, 700),
            new("Country4", "Capital4", "Region4", 400, 800),
        };

        CountryDataProcessor processor = new(countries);

        Pagination pagination = new(2, 2);

        // Act
        CountryDataProcessor result = processor.Trim(pagination);

        // Assert
        ICollection<Country> resultCountries = processor.GetResult();

        Assert.Equal(2, resultCountries.Count);
        Assert.Equal("Country3", resultCountries.ElementAt(0).Name);
        Assert.Equal("Country4", resultCountries.ElementAt(1).Name);
    }

    [Fact]
    public void Trim_StartIndexLessThanZero_StartsFromBeginning()
    {
        // Arrange
        List<Country> countries = new()
        {
            new("Country1", "Capital1", "Region1", 100, 500),
            new("Country2", "Capital2", "Region2", 200, 600),
        };

        CountryDataProcessor processor = new(countries);

        Pagination pagination = new(1, 3);

        // Act
        CountryDataProcessor result = processor.Trim(pagination);

        // Assert
        Assert.Same(processor, result);

        Assert.Equal(2, processor.GetResult().Count);
    }

    [Fact]
    public void Trim_EmptyList_ReturnsSameInstance()
    {
        // Arrange
        List<Country> countries = new();

        CountryDataProcessor processor = new(countries);

        Pagination pagination = new(1, 10);


        // Act
        CountryDataProcessor result = processor.Trim(pagination);

        // Assert
        Assert.Empty(processor.GetResult());
    }
}