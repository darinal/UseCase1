namespace Countries.BLL.ApiModels;

public record CountryResponseModel
{
    public CountryNameResponseModel? Name { get; set; }

    public string[]? Capital { get; set; }

    public string? Region { get; set; }

    public double? Area { get; set; }

    public int? Population { get; set; }
}