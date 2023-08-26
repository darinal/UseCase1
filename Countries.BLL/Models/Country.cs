namespace Countries.BLL.Models;

public record Country
{
    const string UndefinedValue = "Undefined";

    public Country(string? name, string? capital, string? region, float? area)
    {
        Name = name ?? UndefinedValue;
        Capital = capital ?? UndefinedValue;
        Region = region ?? UndefinedValue;
        Area = area ?? 0;
    }

    public string Name { get; set; }

    public string Capital { get; set; }

    public string Region { get; set; }

    public float Area { get; set; }
}