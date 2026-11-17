using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.LearningGap
{
    /// <summary>
    /// To handle generic pagination.
    /// </summary>
    /// <typeparam name="T">The T generic class. </typeparam>
    public class PaginatedResult<T>
    {
        /// <summary>
        /// Gets or sets how many pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedResult{T}"/> class.
        /// </summary>
        /// <param name="totalPages">The total pages.</param>
        /// <param name="items">The items.</param>
        public PaginatedResult(int totalPages, List<T> items)
        {
            TotalPages = totalPages;
            Items = items;
        }
    }
}