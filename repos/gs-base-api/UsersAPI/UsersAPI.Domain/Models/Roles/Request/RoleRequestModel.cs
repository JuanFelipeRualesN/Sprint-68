using System.Collections.Generic;


namespace UsersAPI.UsersAPI.Domain.Models.Roles.Request
{
    /// <summary>
    /// To handle the role requests.
    /// </summary>
    public class RoleRequestModel : RoleTypeModel
    {
        /// <summary>
        /// Gets or sets the role permissions.
        /// </summary>
        public List<RolePermissionModel> RolePermissionModelList { get; set; }
    }
}
