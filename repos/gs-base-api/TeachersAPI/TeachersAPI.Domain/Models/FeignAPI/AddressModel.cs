namespace TeachersAPI.Domain.Models.FeignAPI
{
    /// <summary>
    /// Represents physical address and location details fetched from external services (Feign API).
    /// </summary>
    public class AddressModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the address record.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the associated entity identifier (such as a school ID) linked to this address.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Gets or sets the display name or label for the address.
        /// </summary>
        public string NameAddress { get; set; }

        /// <summary>
        /// Gets or sets the primary street address line.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the secondary address line (e.g., apartment, suite, unit).
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the city where the address is located.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the ISO code for the state or province.
        /// </summary>
        public string StateIsoCode { get; set; }

        /// <summary>
        /// Gets or sets the full name of the state or province.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the postal or ZIP code.
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the region associated with the address.
        /// </summary>
        public string RegionId { get; set; }

        /// <summary>
        /// Gets or sets the country name.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the two-letter ISO code for the country.
        /// </summary>
        public string CountryIsoCode2 { get; set; }

        /// <summary>
        /// Gets or sets any extra or supplementary address information.
        /// </summary>
        public string AddressExtra { get; set; }

        /// <summary>
        /// Gets or sets the phone number associated with the address location.
        /// </summary>
        public string AddressPhone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the primary or main address.
        /// </summary>
        public string MainAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the address record is active or enabled.
        /// </summary>
        public string Enable { get; set; }

        /// <summary>
        /// Gets or sets the user or system process that last modified the record.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of when the address record was created.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of when the address record was last updated.
        /// </summary>
        public string UpdatedAt { get; set; }
    }
}