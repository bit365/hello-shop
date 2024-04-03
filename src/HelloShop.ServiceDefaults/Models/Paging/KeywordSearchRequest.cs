namespace HelloShop.ServiceDefaults.Models.Paging;

public class KeywordSearchRequest : PagedAndSortedRequest
{
    public string? Keyword { get; init; }
}
