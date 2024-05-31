// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ServiceDefaults.Models.Paging;

public class PagedAndSortedRequest : PagedRequest
{
    public string? OrderBy { get; init; }
}