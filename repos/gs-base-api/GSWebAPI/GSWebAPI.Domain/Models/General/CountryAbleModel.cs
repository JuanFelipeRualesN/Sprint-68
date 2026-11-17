namespace GSWebAPI.GSWebAPI.Domain.Models.General
{
    public class CountryAbleModel
    {
        private string _id;
        private string _countryIsoCode2;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string CountryIsoCode2 { get { return _countryIsoCode2; } set { _countryIsoCode2 = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}