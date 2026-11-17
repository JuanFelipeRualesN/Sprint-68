namespace UsersAPI.UsersAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// To handle the user info update.
    /// </summary>
    public class UserInfoUpdateRequestModel : RegisterRequestModel
    {
        /// <summary>
        /// Gets or sets whether the username changes or not.
        /// </summary>
        public bool UsernameChange { get; set; }

        /// <summary>
        /// Gets or sets whether the password changes or not.
        /// </summary>
        public bool PasswordChange { get; set; }

        /// <summary>
        /// Gets or sets whether the email changes or not.
        /// </summary>
        public bool EmailChange { get; set; }

        /// <summary>
        /// Gets or sets whether the name changes or not.
        /// </summary>
        public bool NameChange { get; set; }

        /// <summary>
        /// Gets or sets whether the phone changes or not.
        /// </summary>
        public bool PhoneChange { get; set; }

        /// <summary>
        /// Gets or sets whether the address changes or not.
        /// </summary>
        public bool AddressChange { get; set; }

        /// <summary>
        /// Gets or sets whether the role changes or not.
        /// </summary>
        public bool RoleChange { get; set; }

        /// <summary>
        /// Gets or sets whether the ore changes or not.
        /// </summary>
        public bool OreChange { get; set; }

        /// <summary>
        /// Gets or sets whether the payment method changes or not.
        /// </summary>
        public bool PaymentMethodChange { get; set; }

        /// <summary>
        /// Gets or setsthe user id.
        /// </summary>
        public new string UserId { get; set; }

        /// <summary>
        /// Gets or sets the account status id.
        /// </summary>
        public string StatusId { get; set; }
    }
}