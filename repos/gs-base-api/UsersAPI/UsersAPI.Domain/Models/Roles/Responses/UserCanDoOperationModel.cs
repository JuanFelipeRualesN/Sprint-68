namespace UsersAPI.UsersAPI.Domain.Models.Roles.Responses
{
    /// <summary>
    /// To handle if user can do the operation.
    /// </summary>
    public class UserCanDoOperationModel
    {
        /// <summary>
        /// Gets or sets whether the user can do the operation or not.
        /// </summary>
        public bool UserCanDoOperation { get; set; }

        /// <summary>
        ///  Gets or sets whether the user is staff or not.
        /// </summary>
        public bool IsStaff { get; set; }

        /// <summary>
        /// Gets or sets whether the user is director or not.
        /// </summary>
        public bool IsDirector { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the user's email.
        /// </summary>
        public string Email { get; set; }
    }
}
