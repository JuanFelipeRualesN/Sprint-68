using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs.Schools
{
    /// <summary>
    /// To handle the general academic details of the entity.
    /// </summary>
    public class EntityAcademicDetailModel : IResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the transaction was valid or not.
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// Gets or sets the messages related to the transaction.
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the entity-subscription's id.
        /// </summary>
        public string EntitySubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the term's id.
        /// </summary>
        public string TermId { get; set; }

        /// <summary>
        /// Gets or sets the period quantity.
        /// </summary>
        public string PeriodQuantity { get; set; }

        /// <summary>
        /// Gets or sets the days to close the period.
        /// </summary>
        public string DaysToClosePeriod { get; set; }

        /// <summary>
        /// Gets or sets the date when the term starts.
        /// </summary>
        public string FullTermStartDate { get; set; }

        /// <summary>
        /// Gets or sets the date when the term ends.
        /// </summary>
        public string FullTermEndDate { get; set; }

        /// <summary>
        /// Gets or sets the user's id that modified the row.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the date when the row was created.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date when the row was updated.
        /// </summary>
        public string UpdatedAt { get; set; }
    }
}