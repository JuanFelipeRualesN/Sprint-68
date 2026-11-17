namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions
{
    public class TaxModel
    {
        private string _id;
        private string _taxName;
        private string _stateIso;
        private string _countryIso;
        private string _percent;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string TaxName { get { return _taxName; } set { _taxName = value; } }
        public string StateIso { get { return _stateIso; } set { _stateIso = value; } }
        public string CountryIso { get { return _countryIso; } set { _countryIso = value; } }
        public string Percent { get { return _percent; } set { _percent = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}