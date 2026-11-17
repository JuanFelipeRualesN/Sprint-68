namespace AddressAPI.AddressAPI.Domain.Models.Address
{
    public class ResponseISOModel
    {
        public string IsoCode2 { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string CountryISOCode { get; set; }
        public bool HasStates { get; set; }
    }
}
