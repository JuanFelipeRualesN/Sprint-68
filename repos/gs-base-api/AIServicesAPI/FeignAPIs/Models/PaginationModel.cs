namespace GSAIServicesAPI.FeignAPIs.Models
{
    /// <summary>
    /// To handle the pagination details.
    /// </summary>
    /// <typeparam name="T">The item.</typeparam>
    public class PaginationModel<T>
    {
        /// <summary>
        /// Parameterless constructor for deserialization.
        /// </summary>
        public PaginationModel() { }

        /// <summary>
        /// Constructor for the pagination.
        /// </summary>
        /// <param name="total">The total of items.</param>
        /// <param name="limitByPage">The limit per page.</param>
        /// <param name="items">The items.</param>
        public PaginationModel(int total, int limitByPage, List<T> items)
        {
            TotalPages = SetTotalPages(total, limitByPage);
            Total = total;
            Items = items;
        }

        /// <summary>
        /// Gets or sets the total of items.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// Sets the total of pages.
        /// </summary>
        /// <param name="total">Total of items.</param>
        /// <param name="limit">Items limiter.</param>
        /// <returns>Returns the total of pages.</returns>
        public static int SetTotalPages(int total, int limit)
        {
            if (total == 0)
                return 0;

            return (int)Math.Ceiling(total / (double)limit);
        }
    }
}
