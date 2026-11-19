using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// Represent a pagination of elements
    /// </summary>
    /// <typeparam name="T">Items type</typeparam>
    public class Paged<T> where T : class
    {
        private Paged(int page, int limit, long total, List<T> items)
        {
            Page = page;
            Limit = limit;
            Total = total;
            Items = items;
        }
        /// <summary>
        /// Current page
        /// </summary>
        public int Page { get; private set; }
    
        /// <summary>
        /// Page size (how many items by page)
        /// </summary>
        public int Limit { get; private set; }
    
        /// <summary>
        /// Total records available
        /// </summary>
        public long Total { get; private set; }

        /// <summary>
        /// Total pages  
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (Total == 0)
                    return 0;

                return (int)Math.Ceiling(Total / (double)Limit);
            }
        }

        /// <summary>
        /// Items in the current page
        /// </summary>
        public List<T> Items { get; private set; }

        /// <summary>
        /// Factory method for create a Paged object
        /// </summary>
        /// <param name="page">Current page</param>
        /// <param name="limit">Page size (how many items by page)</param>
        /// <param name="total">Total records available</param>
        /// <param name="items">Items in the current page</param>
        /// <returns></returns>
        public static Paged<T> Create(int page, int limit, long total, List<T> items)
        {
            return new Paged<T>(page, limit, total, items);
        }
    }
}