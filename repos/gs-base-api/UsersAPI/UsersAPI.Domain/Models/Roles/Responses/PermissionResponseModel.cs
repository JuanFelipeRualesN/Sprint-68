using System.Collections.Generic;

namespace UsersAPI.UsersAPI.Domain.Models.Roles.Responses
{
    /// <summary>
    /// To handle the permission details.
    /// </summary>
    public class PermissionResponseModel : PermissionModel, IResponse
    {
        public PermissionResponseModel()
        {
            Messages = new List<string>();
        }

        /// <summary>
        /// Gets or serts whether the transaction was valid or not. 
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// Gets or sets the message list.
        /// </summary>
        public List<string> Messages { get; set; }

    }
}
