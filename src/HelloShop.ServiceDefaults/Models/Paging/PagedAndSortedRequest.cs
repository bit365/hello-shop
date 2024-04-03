namespace HelloShop.ServiceDefaults.Models.Paging;

public class PagedAndSortedRequest : PagedRequest
{
    public IEnumerable<SortingOrder>? Sorts { get; init; }
}
