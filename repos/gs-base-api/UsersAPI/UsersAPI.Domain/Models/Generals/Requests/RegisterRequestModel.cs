using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Requests
{

    /// <summary>
    /// Manages the register, the update information, and more.
    /// </summary>
    public class RegisterRequestModel : LoginRequestModel
    {
        /// <summary>
        /// Gets or sets User Id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets subscription Id.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets Date of Birth.
        /// </summary>
        public string DoB { get; set; }

        /// <summary>
        /// Gets or sets Password Retype.
        /// </summary>
        public string PasswordRetype { get; set; }

        /// <summary>
        /// Gets or sets Phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets Address Line 1.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets Address Line 2.
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets state iso.
        /// </summary>
        public string StateIso { get; set; }

        /// <summary>
        /// Gets or sets country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets zip code.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the country ISO.
        /// </summary>
        public string CountryIso { get; set; } // The country ISO. The Web API will find the correct name.

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        public string Gender { get; set; } // Gender of the user

        /// <summary>
        /// Gets or sets the plan ID.
        /// </summary>
        public string PlanId { get; set; }

        /// <summary>
        /// Gets or sets the Stripe Token.
        /// </summary>
        public string StripeToken { get; set; }

        /// <summary>
        /// Gets or sets the subscriptions quantity.
        /// </summary>
        public int SubscriptionsQuantity { get; set; }

        /// <summary>
        /// Gets or sets the coupon platform ID.
        /// </summary>
        public string CouponPlatformId { get; set; } // The coupon ID. It has to be sent, and prepared when calculating.

        /// <summary>
        /// Gets or sets the Role ID.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the user school identifier.
        /// </summary>
        public string SchoolUserIdentifier { get; set; } // SIE or Teacher identifier

        /// <summary>
        /// Gets or sets the Id of school.
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// Gets or sets the School Code Identifier (License number, or DEPR School Unique Code (Example).
        /// </summary>
        public string SchoolExternalIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the registration connection request token. For example, if a 
        /// teacher invites a parent, the parent may received this registration connection so
        /// they can directly relate the student with the user.
        /// </summary>
        public string RegistrationConnectionRequestToken { get; set; }

        /// <summary>
        /// Gets or sets the max tries for exams. This is used for homeschoolers. If applied for schools, this will be overwritten by teacher 
        /// when assigning a lesson.
        /// </summary>
        public string MaxTriesExam { get; set; }

        /// <summary>
        /// To register the user with an ore. User must be an staff.
        /// </summary>
        public string OreId { get; set; }
    }
}