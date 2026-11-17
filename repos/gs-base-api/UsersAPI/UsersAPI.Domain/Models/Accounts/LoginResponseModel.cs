using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.UsersAPI.Domain.Models.Accounts
{
    /// <summary>
    /// To handle user validation login account.
    /// </summary>
    public class LoginResponseModel : ResponseModel
    {
        /// <summary>
        /// Gets or sets the invitation code of the account. 
        /// </summary>
        public string InvitationCode { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
    }
}
