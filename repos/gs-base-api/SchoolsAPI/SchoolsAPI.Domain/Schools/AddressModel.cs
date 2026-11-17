namespace SchoolsAPI.SchoolsAPI.Domain.Models.Schools
{
    public class AddressModel
    {
        public string Id { get; set; }
        public string EntityId { get; set; }
        public string NameAddress { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateIsoCode { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string RegionId { get; set; }
        public string Country { get; set; }
        public string CountryIsoCode2 { get; set; }
        public string AddressExtra { get; set; }
        public string AddressPhone { get; set; }
        public string MainAddress { get; set; }
        public string Enable { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}