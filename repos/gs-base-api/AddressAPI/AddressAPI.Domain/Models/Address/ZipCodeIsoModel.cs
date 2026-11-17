namespace AddressAPI.AddressAPI.Domain.Models.Address
{
    public class ZipCodeIsoModel
    {
        private string _id;
        private string _stateName;
        private string _zipCodeFrom;
        private string _zipCodeTo;
        private string _isoCode;
        private string _countryIsoCode;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string StateName { get { return _stateName; } set { _stateName = value; } }
        public string ZipCodeFrom { get { return _zipCodeFrom; } set { _zipCodeFrom = value; } }
        public string ZipCodeTo { get { return _zipCodeTo; } set { _zipCodeTo = value; } }
        public string IsoCode { get { return _isoCode; } set { _isoCode = value; } }
        public string CountryIsoCode { get { return _countryIsoCode; } set { _countryIsoCode = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}