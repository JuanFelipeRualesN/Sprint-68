using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests.APIs
{
    public class ClientSubscriptionRegisterRequestModel
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Stripe Token.
        /// </summary>
        public string StripeToken { get; set; }

        /// <summary>
        /// Gets or sets the state iso.
        /// </summary>
        public string StateIso { get; set; }

        /// <summary>
        /// Gets or sets the country iso. 
        /// </summary>
        public string CountryIsoCode { get; set; }

        /// <summary>
        /// Gets or sets the subscription quantity.
        /// </summary>
        public int SubscriptionsQuantity { get; set; }

        /// <summary>
        /// Gets or sets the coupon platform.
        /// </summary>
        public string CouponPlatformId { get; set; }

        /// <summary>
        /// Gets or sets the plan.
        /// </summary>
        public string PlanId { get; set; }

        /// <summary>
        /// Gets or sets the temporary password.
        /// </summary>
        public string TemporaryPassword { get; set; }

        /// <summary>
        /// Gets or sets whether the client enter by a promo or not.
        /// </summary>
        public int IsPromo { get; set; }

        /// <summary>
        /// Gets or sets the max tries for exams. This is used for homeschoolers. If applied for schools, this will be overwritten by teacher 
        /// when assigning a lesson.
        /// </summary>
        public string MaxTriesExam { get; set; }

    }
}