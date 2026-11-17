using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// To handle plan requests
    /// </summary>
    public class PlanRequestQueryModel
    {
        /// <summary>
        /// Gets or sets the plan Id.
        /// </summary>
        public string PlanId { get; set; }

        /// <summary>
        /// Gets or sets whether is a school or not.
        /// </summary>
        public bool IsSchool { get; set; }

        /// <summary>
        /// Gets or sets whether is private or not.
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the Plan Request.
        /// </summary>
        public SubscriptionsAPI.Domain.Models.Subscriptions.Requests.PlanRequestModel PlanRequest { get; set; }

        /// <summary>
        /// Gets or sets whether the details are needed or not.
        /// </summary>
        public bool DetailsNeeded { get; set; }

        /// <summary>
        /// Gets or sets the page from (for pagination).
        /// </summary>
        public int PageFrom { get; set; }

        /// <summary>
        /// Gets or sets how many items per page.
        /// </summary>
        public int Limiter { get; set; }

        /// <summary>
        /// Gets or sets the country alpha ISO 3.
        /// </summary>
        public string CountryIsoAlpha3 { get; set; }

        /// <summary>
        /// Gets or sets whether filter by language or not.
        /// </summary>
        public bool FilterByLanguage { get; set; }

        /// <summary>
        /// Gets or sets whether order by date or not.
        /// </summary>
        public bool OrderByDate { get; set; }

        /// <summary>
        /// If get or sets whether only availables or not.
        /// </summary>
        public bool OnlyAvailables { get; set; }
    }
}