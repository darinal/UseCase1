using Countries.BLL.Models;

namespace Countries.API.Models
{
    public record CountryResponse(ICollection<Country> Items, int TotalCount, int Page, int PageSize)
    {
        public int Page { get; set; } = Page;

        public int PageSize { get; set; } = PageSize;

        public int TotalCount { get; set; } = TotalCount;

        public ICollection<Country> Items { get; set; } = Items;
    }
}
