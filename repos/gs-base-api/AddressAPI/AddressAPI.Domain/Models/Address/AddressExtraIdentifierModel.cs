namespace AddressAPI.AddressAPI.Domain.Models.Address
{
    public class AddressExtraIdentifierModel
    {
        private string _id;
        private string _countryId;
        private string _desc;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string CountryId { get { return _countryId; } set { _countryId = value; } }
        public string Desc { get { return _desc; } set { _desc = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}