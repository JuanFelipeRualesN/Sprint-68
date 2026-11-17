namespace UsersAPI.UsersAPI.Domain.Models.Generals
{
    /// <summary>
    /// To handle SSO (single sign-on) sign in.
    /// </summary>
    public class LoginSsoModel
    {
        /// <summary>
        /// Gets or sets the token (must be the idToken that the SSO returns when making the SSO in the frontend. Frontend should return this). 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the domain, or subdomain or any other reference.
        /// </summary>
        public string Domain { get; set; }
    }
}