namespace UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler.Models.Filters
{
    public class AccountAdvance : AccountGeneral
    {
        /// <summary>
        /// Gets or sets the status' id.
        /// </summary>
        public string StatusId { get; set; }

        /// <summary>
        /// Gets or sets the email to filter.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number to filter.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets whether call only the profiles that can be managed.
        /// </summary>
        public bool CallOnlyProfilesThatCanBeManaged { get; set; }

        /// <summary>
        /// Gets or sets the date from when the account was created to apply as a filter.
        /// </summary>
        public string CreatedFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to when the account was created to apply as a filter.
        /// </summary>
        public string CreatedTo { get; set; }

        /// <summary>
        /// Gets or sets the date from when an account activity was registered.
        /// </summary>

        public string LastSessionFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to when an account activity was registered.
        /// </summary>
        public string LastSessionTo { get; set; }

    }
}