namespace TeachersAPI.Domain.Models.FeignAPI
{
    /// <summary>
    /// Represents a user's associated school or institution.
    /// </summary>
    public class UserSchoolModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user-school relationship.
        /// </summary>
        public string UserSchoolId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the institution.
        /// </summary>
        public string InstitutionId { get; set; }

        /// <summary>
        /// Gets or sets the official code or identifier for the institution (e.g., Department of Education code).
        /// </summary>
        public string InstitutionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the full name of the school or institution.
        /// </summary>
        public string InstitutionName { get; set; }

        /// <summary>
        /// Gets or sets the city where the institution is located.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state or territory where the institution is located (nullable).
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the country where the institution is located.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the postal or zip code of the institution's location.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user's association with this school has been confirmed.
        /// </summary>
        public bool Confirmed { get; set; }
    }
}
