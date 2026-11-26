namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs.Users
{
    /// <summary>
    /// A user's full name as the Users API returns it (<c>POST usersapi/users/fullnames</c>).
    /// </summary>
    public class UserFullNameModel
    {
        /// <summary>
        /// The user id.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// The given name.
        /// </summary>
        public string? GivenName { get; set; }

        /// <summary>
        /// The surname.
        /// </summary>
        public string? Surname { get; set; }

        /// <summary>
        /// The suffix (Jr., III...). Optional.
        /// </summary>
        public string? Suffix { get; set; }
    }
}
