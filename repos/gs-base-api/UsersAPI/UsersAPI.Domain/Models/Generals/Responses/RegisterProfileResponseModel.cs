using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle the JSON response for profile registration.
    /// </summary>
    public class RegisterProfileResponseModel
    {

        /// <summary>
        ///  Initializes a new instance of the <see cref="RegisterProfileResponseModel{T}"/> class.
        /// </summary>
        public RegisterProfileResponseModel()
        {
            Messages = new List<string>();
        }

        /// <summary>
        /// Gets or sets whether transaction was valid or not.
        /// </summary>
        public bool TransactionValid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the messages of the transaction.
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// To Check if there is any subscription space left. 
        /// </summary>
        public bool SpacesInSubscriptionLeft { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }
    }
}