using System;
using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// Handle pagination details
    /// </summary>
    /// <typeparam name="T">The item</typeparam>
    public class PaginationModel<T>
    {
        /// <summary>
        /// Constructor for the pagination
        /// </summary>
        /// <param name="total">The total number of items</param>
        /// <param name="limitByPage">The limit per page</param>
        /// <param name="items">The items</param>
        public PaginationModel(int total, int limitByPage, List<T> items)
        {
            TotalPages = SetTotalPages(total, limitByPage);
            Total = total;
            Items = items;
        }

        public int Total { get; set; }

        public int TotalPages { get; set; }

        public List<T> Items { get; set; }

        /// <summary>
        /// Set total pages
        /// </summary>
        /// <param name="total">Total items</param>
        /// <param name="limit">Items limiter</param>
        /// <returns>Total pages</returns>
        public static int SetTotalPages(int total, int limit)
        {
            if (total == 0)
                return 0;

            return (int)Math.Ceiling(total / (double)limit);
        }
    }
}