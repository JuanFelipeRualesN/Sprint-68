using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Generals.Responses.APIs
{
    /// <summary>
    /// To handle the validations.
    /// </summary>
    public class ValidationResponseModel
    {
        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        public string LangCode { get; set; }

        /// <summary>
        /// Gets or sets whether the session stills valid or not.
        /// </summary>
        public bool SessionValid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the messages of the transaction.
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Gets or sets a value of user id (only when neccessary).
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets whether if Staff or not.
        /// </summary>
        public bool IsStaff { get; set; }

        /// <summary>
        /// Gets or sets whether the user is a Director or not. The Director may have access to all the users of their school.
        /// </summary>
        public bool IsDirector { get; set; }

        /// <summary>
        /// Gets or sets the token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the Role Id.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the User Time Zone.
        /// </summary>
        public string UserTimeZone { get; set; }

        /// <summary>
        /// Gets or sets the user's full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the user's email.
        /// </summary>
        public string Email { get; set; }
    }
}