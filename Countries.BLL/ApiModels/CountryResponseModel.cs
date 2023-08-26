namespace Countries.BLL.ApiModels;

public class CountryResponseModel
{
    public CountryNameResponseModel? Name { get; set; }

    public string[]? Capital { get; set; }

    public string? Region { get; set; }

    public float? Area { get; set; }
}