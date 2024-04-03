namespace HelloShop.ServiceDefaults.Models.Paging;

public class SortingOrder
{
    public required string PropertyName { get; init; }

    public bool Ascending { get; init; }
}
