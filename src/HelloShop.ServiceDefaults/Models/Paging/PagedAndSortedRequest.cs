namespace HelloShop.ServiceDefaults.Models.Paging;

public class PagedAndSortedRequest : PagedRequest
{
    public string? OrderBy { get; init; }
}