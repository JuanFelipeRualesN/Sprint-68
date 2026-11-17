
namespace UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler.Models
{
    /// <summary>
    /// To handle user schools roles.
    /// </summary>
    public class UserSchoolRoleModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the school id.
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the user's id.
        /// </summary>
        public string UserId { get; set; }
    }
}