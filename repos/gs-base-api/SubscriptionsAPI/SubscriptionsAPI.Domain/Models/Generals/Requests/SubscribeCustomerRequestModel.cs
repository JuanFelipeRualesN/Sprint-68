using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// To handle subscribe customer request.
    /// </summary>
    public class SubscribeCustomerRequestModel
    {
        /// <summary>
        /// Gets or sets the school id if any.
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// Gets or sets the program id.
        /// </summary>
        public string ProgramId { get; set; }

        /// <summary>
        /// Gets or sets the subscritpion token stripe.
        /// </summary>
        public string SubTokenStripe { get; set; }

        /// <summary>
        /// Gets or sets the plan Id.
        /// </summary>
        public string PlanId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of subscription.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the subscription id.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the coupon platform id.
        /// </summary>
        public string CouponPlatformId { get; set; }

        /// <summary>
        /// Gets or sets the platform code.
        /// </summary>
        public string PlatformCode { get; set; }

        /// <summary>
        /// Gets or sets the cancel period end.
        /// </summary>
        public string CurrentPeriodEnd { get; set; }

        /// <summary>
        /// Gets or sets the type. H for Homeschooling, T for tutoring, S for school
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the entity academic detail to relate.
        /// </summary>
        public string EntityAcademicDetailId { get; set; }

    }
}