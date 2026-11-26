namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    /// <summary>One page of a list, with the totals the client needs to build the pager.</summary>
    /// <typeparam name="T">Item type.</typeparam>
    public class PagedResponseModel<T>
    {
        /// <summary>Zero-based page returned (after normalizing: negative values become 0).</summary>
        public int Page { get; set; }

        /// <summary>Page size used (after clamping).</summary>
        public int PageSize { get; set; }

        /// <summary>Number of items across all pages, with the same filters.</summary>
        public int TotalItems { get; set; }

        /// <summary>Number of pages for <see cref="PageSize"/> (0 when there are no items).</summary>
        public int TotalPages { get; set; }

        /// <summary>Items of this page (empty past the last page).</summary>
        public List<T> Items { get; set; } = new();

        /// <summary>Builds a page and computes <see cref="TotalPages"/>.</summary>
        /// <param name="items">Items of this page.</param>
        /// <param name="page">Zero-based page.</param>
        /// <param name="pageSize">Page size (at least 1).</param>
        /// <param name="totalItems">Total items across all pages.</param>
        /// <returns>The page.</returns>
        public static PagedResponseModel<T> Create(List<T> items, int page, int pageSize, int totalItems) => new()
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (totalItems + pageSize - 1) / pageSize,
        };
    }
}
