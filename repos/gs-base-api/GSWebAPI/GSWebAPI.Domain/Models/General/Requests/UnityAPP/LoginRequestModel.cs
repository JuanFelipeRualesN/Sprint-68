namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests.UnityAPP
{
    /// <summary>
    /// Handles the Login request for Unity APP only.
    /// </summary>
    public class LoginRequestModel : GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets the APP version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the OS Version.
        /// </summary>
        public string OsVersion { get; set; }
    }
}