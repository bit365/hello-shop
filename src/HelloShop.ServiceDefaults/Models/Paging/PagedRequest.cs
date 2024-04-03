using HelloShop.ServiceDefaults.Constants;

namespace HelloShop.ServiceDefaults.Models.Paging;

public class PagedRequest
{
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = PagingConstants.DefaultPageSize;
}