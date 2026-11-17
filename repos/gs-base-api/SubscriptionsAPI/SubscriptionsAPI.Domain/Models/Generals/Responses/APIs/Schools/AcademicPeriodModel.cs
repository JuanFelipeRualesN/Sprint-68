using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs.Schools
{
    /// <summary>
    /// To handle the academic periods.
    /// </summary>
    public class AcademicPeriodModel
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
        /// Gets or sets the entity academic detail's id.
        /// </summary>
        public string EntityAcademicDetailId { get; set; }

        /// <summary>
        /// Gets or sets the period's number.
        /// </summary>
        public string PeriodNumber { get; set; }

        /// <summary>
        /// Gets or sets when the period starts.
        /// </summary>
        public string PeriodFrom { get; set; }

        /// <summary>
        /// Gets or sets when the period ends.
        /// </summary>
        public string PeriodTo { get; set; }

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