namespace Countries.BLL.Models;

public record CountriesFilters(string? CountryName, int? Population)
{
    public string? CountryName { get; set; } = CountryName;

    public int? Population { get; set; } = Population;
}