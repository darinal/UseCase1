namespace Countries.BLL.Models;

public record Country
{
    const string UndefinedValue = "Undefined";

    public Country(string? name, string? capital, string? region, int? population, double? area)
    {
        Name = name ?? UndefinedValue;
        Capital = capital ?? UndefinedValue;
        Region = region ?? UndefinedValue;
        Population = population ?? 0;
        Area = area ?? 0;
    }

    public string Name { get; set; }

    public string Capital { get; set; }

    public string Region { get; set; }

    public int Population { get; set; }

    public double Area { get; set; }
}