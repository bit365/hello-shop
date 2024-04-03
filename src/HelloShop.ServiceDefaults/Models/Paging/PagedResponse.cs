namespace HelloShop.ServiceDefaults.Models.Paging;

public class PagedResponse<T>(IReadOnlyList<T> items, int totalCount)
{
    public IReadOnlyList<T> Items { get; init; } = items;

    public int TotalCount { get; init; } = totalCount;
}