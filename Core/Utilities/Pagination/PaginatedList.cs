namespace Core.Utilities.Pagination;

public class PaginatedList<T>
{
    public PaginatedList(IEnumerable<T> items, int totalItems, int page, int pageSize)
    {
        Items = items;
        TotalItems = totalItems;
        Page = page;
        PageSize = pageSize;
    }

    public IEnumerable<T> Items { get; set; }

    public int TotalItems { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize); 
}
