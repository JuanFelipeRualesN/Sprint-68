using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses.APIs
{
    public class SubscriptionModel
    {
        /// <summary>
        /// Gets or sets the subscription id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the platform id.
        /// </summary>
        public string PlatformId { get; set; }

        /// <summary>
        /// Gets or sets the customer platform id.
        /// </summary>
        public string CustomerPlatformId { get; set; }

        /// <summary>
        /// Gets the plan platform id.
        /// </summary>
        public string PlanPlatformId { get; set; }

        /// <summary>
        /// Gets or sets the Quantity.
        /// </summary>
        public string Quantity { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets current period end.
        /// </summary>
        public string CurrentPeriodEnd { get; set; }

        /// <summary>
        /// Gets or sets the platform code.
        /// </summary>
        public string PlatformCode { get; set; }

        /// <summary>
        /// Gets or sets the user id who modified it.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the created at date.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the updated at date.
        /// </summary>
        public string UpdatedAt { get; set; }
    }
}