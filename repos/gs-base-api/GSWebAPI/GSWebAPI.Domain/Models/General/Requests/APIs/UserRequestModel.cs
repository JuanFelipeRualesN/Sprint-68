using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests.APIs
{
    /// <summary>
    /// User list request model to request users.
    /// </summary>
    public class UserRequestModel : GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets User Ids to call.
        /// </summary>
        public List<string> UserIds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is a staff making the request.
        /// </summary>
        public bool IsStaff { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is the principal role or not.
        /// </summary>
        public bool IsDirector { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is a client account that is making the request.
        /// </summary>
        public bool IsClientAccount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the same school rule required.
        /// </summary>
        public bool SameSchoolRequired { get; set; }

        /// <summary>
        /// Gets or sets the school id to filter.
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// Gets or sets the username to filter.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the first name to filter.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name to filter.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's role.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the user's school record to filter.
        /// </summary>
        public string UserSchoolRecord { get; set; }

        /// <summary>
        /// Gets or sets the user making the request.
        /// </summary>
        public string UserAlphaId { get; set; }
        /// <summary>
        /// Gets or sets the email to filter.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number to filter.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether call only the profiles that can be managed.
        /// </summary>
        public bool CallOnlyProfilesThatCanBeManaged { get; set; }

        /// <summary>
        /// Gets or sets the date from when the account was created to apply as a filter.
        /// </summary>
        public string CreatedFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to when the account was created to apply as a filter.
        /// </summary>
        public string CreatedTo { get; set; }

        /// <summary>
        /// Gets or sets the date from when an account activity was registered.
        /// </summary>
        public string LastSessionFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to when an account activity was registered.
        /// </summary>
        public string LastSessionTo { get; set; }

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

        /// <summary>
        /// Gets or sets the IP Address.
        /// </summary>
        public string IpAddress { get; set; }
    }
}