using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Response
{
    /// <summary>
    /// To handle any pagination response.
    /// </summary>
    /// <typeparam name="T">Any class.</typeparam>
    public class ResponsePagination<T>
    {
        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int PagesTotal { get; set; }

        /// <summary>
        /// Gets or sets the total items.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// The constructor.
        /// </summary>
        public ResponsePagination()
        {
            Items = new List<T>();
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="pagesTotal">The total of pages.</param>
        /// <param name="totalItems">The total of items.</param>
        /// <param name="items">The items.</param>
        public ResponsePagination(int pagesTotal, int totalItems, List<T> items)
        {
            PagesTotal = pagesTotal;
            TotalItems = totalItems;
            Items = items;
        }
    }
}