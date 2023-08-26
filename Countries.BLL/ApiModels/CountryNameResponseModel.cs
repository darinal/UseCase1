namespace Countries.BLL.ApiModels;

public record CountryNameResponseModel
{
    public string? Official { get; set; }

    public string? Common { get; set; }
}