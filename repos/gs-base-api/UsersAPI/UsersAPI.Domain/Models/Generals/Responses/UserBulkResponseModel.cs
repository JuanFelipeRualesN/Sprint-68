using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// Handles the user Bulk Upload details.
    /// </summary>
    public class UserBulkResponseModel
    {
        public UserBulkResponseModel()
        {
            Messages = new List<string>();
        }
        /// <summary>
        /// Gets or sets the list of errors.
        /// </summary>
        public List<UserErrorModel> UserErrors { get; set; }

        /// <summary>
        /// Gets or sets the report path.
        /// </summary>
        public string ReportPath { get; set; }

        /// <summary>
        /// Gets or sets the message list.
        /// </summary>
        public List<string> Messages { get; set; }
    }

    /// <summary>
    /// To handle the error details.
    /// </summary>
    public class UserErrorModel : UsersAPI.Domain.Models.Generals.Requests.RegisterRequestModel
    {
        public UserErrorModel()
        {
            ErrorDetails = new List<string>();
        }

        /// <summary>
        /// Gets or sets the error details.
        /// </summary>
        public List<string> ErrorDetails { get; set; }
    }
}