using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle the report subscription detail.
    /// </summary>
    public class ReportSubscriptionDetailModel
    {
        /// <summary>
        /// Gets or sets the subscription id.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the subscription platform id.
        /// </summary>
        public string SubscriptionPlatformId { get; set; }

        /// <summary>
        /// Gets or sets the client name.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        public string Quantity { get; set; }

        /// <summary>
        /// Gets or sets the active total.
        /// </summary>
        public string Active { get; set; }

        /// <summary>
        /// Gets or sets cancelled total.
        /// </summary>
        public string Cancelled { get; set; }

        /// <summary>
        /// Gets or sets the activation date of the subscription.
        /// </summary>
        public string ActivationDate { get; set; }

        /// <summary>
        /// Gets or sets the termination date of the subscription.
        /// </summary>
        public string TerminationDate { get; set; }

        /// <summary>
        /// Gets or sets the cancellation reason.
        /// </summary>
        public string CancellationReason { get; set; }

        /// <summary>
        /// Gets or sets the plan.
        /// </summary>
        public string Plan { get; set; }

        /// <summary>
        /// Gets or sets the plan type.
        /// </summary>
        public string PlanType { get; set; }

        /// <summary>
        /// Gets or sets the plan cost.
        /// </summary>
        public string PlanCost { get; set; }

        /// <summary>
        /// Gets or sets client total payment.
        /// </summary>
        public string ClientPayment { get; set; }

    }
}