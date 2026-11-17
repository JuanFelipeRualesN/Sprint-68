namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions
{
    public class CustomerSourceTokenModel
    {
        private string _id;
        private string _customerPlatformId;
        private string _sourceToken;
        private string _active;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string CustomerPlatformId { get { return _customerPlatformId; } set { _customerPlatformId = value; } }
        public string SourceToken { get { return _sourceToken; } set { _sourceToken = value; } }
        public string Active { get { return _active; } set { _active = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}