namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// A user related to a subscription (a row of <c>subst_subscription_profiles</c>) with the details read from the Users API.
    /// The names and role are null when the Users API does not return them (or cannot be reached); the row is still listed.
    /// </summary>
    public class SubscriptionProfileUserResponseModel
    {
        /// <summary>
        /// subst_subscription_profiles.id.
        /// </summary>
        public string? SubscriptionProfileId { get; set; }

        /// <summary>
        /// The subscription the user is related to.
        /// </summary>
        public string? SubscriptionId { get; set; }

        /// <summary>
        /// The user id of the related profile (subst_subscription_profiles.profile_id).
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Given name, from the Users API.
        /// </summary>
        public string? GivenName { get; set; }

        /// <summary>
        /// Surname, from the Users API.
        /// </summary>
        public string? Surname { get; set; }

        /// <summary>
        /// Suffix, from the Users API. Optional.
        /// </summary>
        public string? Suffix { get; set; }

        /// <summary>
        /// Given name, surname and suffix joined; null when the Users API did not return the name.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// The user's role id, from the Users API (for example student or parent).
        /// </summary>
        public string? RoleId { get; set; }

        /// <summary>
        /// True for the user in session.
        /// </summary>
        public bool IsCurrentUser { get; set; }

        /// <summary>
        /// When the user was related to the subscription (subst_subscription_profiles.created_at, as the DAO formats it).
        /// </summary>
        public string? CreatedAt { get; set; }
    }
}
