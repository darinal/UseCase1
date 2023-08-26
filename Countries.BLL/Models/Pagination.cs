namespace Countries.BLL.Models;

public record Pagination(int Page, int PageSize)
{
    public int Page { get; set; } = Page;

    public int PageSize { get; set; } = PageSize;
}