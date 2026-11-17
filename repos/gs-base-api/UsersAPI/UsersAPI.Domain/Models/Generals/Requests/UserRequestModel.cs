using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// User filter requests.
    /// </summary>
    public class UserRequestModel : UsersAPI.Domain.Models.Generals.DapperHandler.Models.Filters.AccountAdvance
    {
        /// <summary>
        /// Gets or sets page from.
        /// </summary>
        public int PageFrom { get; set; }

        /// <summary>
        /// Gets or sets how many items per page.
        /// </summary>
        public int Limiter { get; set; }

        /// <summary>
        /// Gets or sets the subscription id.
        /// </summary>
        public string SubscriptionId { get; set; }
    }
}