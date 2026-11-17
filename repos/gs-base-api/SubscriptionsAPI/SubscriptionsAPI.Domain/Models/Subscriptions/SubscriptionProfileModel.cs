namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions
{
    public class SubscriptionProfileModel
    {
        private string _id;
        private string _subscriptionId;
        private string _profileId;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string SubscriptionId { get { return _subscriptionId; } set { _subscriptionId = value; } }
        public string ProfileId { get { return _profileId; } set { _profileId = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}