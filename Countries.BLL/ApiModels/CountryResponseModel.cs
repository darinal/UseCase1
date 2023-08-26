namespace Countries.BLL.ApiModels;

public record CountryResponseModel
{
    public CountryNameResponseModel? Name { get; set; }

    public string[]? Capital { get; set; }

    public string? Region { get; set; }

    public float? Area { get; set; }

    public int? Population { get; set; }
}