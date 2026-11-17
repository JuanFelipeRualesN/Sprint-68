using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using System.Collections.Generic;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs
{
    /// <summary>
    /// To handle the login API.
    /// </summary>
    public class LoginAPIResponseModel : GeneralResponseModel
    {
        /// <summary>
        /// Gets or sets the roles of the user.
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// Gets or sets invitatin code.
        /// </summary>
        public string InvitationCode { get; set; }
    }
}