namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions
{
    public class PlanAvailableIsoModel
    {
        private string _id;
        private string _planId;
        private string _stateIso;
        private string _countryIso;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string PlanId { get { return _planId; } set { _planId = value; } }
        public string StateIso { get { return _stateIso; } set { _stateIso = value; } }
        public string CountryIso { get { return _countryIso; } set { _countryIso = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}