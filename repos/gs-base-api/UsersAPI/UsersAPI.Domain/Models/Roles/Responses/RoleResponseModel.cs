using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.UsersAPI.Domain.Models.Roles.Responses
{
    /// <summary>
    /// To handle the role response.
    /// </summary>
    public class RoleResponseModel : RoleTypeModel, IResponse
    {
        public RoleResponseModel()
        {
            Messages = new List<string>();
        }

        /// <summary>
        /// Gets or sets whether the transaction was valid or not.
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Gets or sets the permission model list.
        /// </summary>
        public List<PermissionModel> PermissionModelList { get; set; }

        /// <summary>
        /// Gets or sets the role permission model list.
        /// </summary>
        public List<RolePermissionModel> RolePermissionModelList { get; set; }
    }
}
