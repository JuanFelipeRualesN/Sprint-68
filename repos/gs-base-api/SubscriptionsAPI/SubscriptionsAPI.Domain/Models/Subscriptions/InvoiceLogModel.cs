namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions
{
    public class InvoiceLogModel
    {
        private string _id;
        private string _invoicePlatformId;
        private string _customerPlatformId;
        private string _subscriptionPlatformId;
        private string _amtDue;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string InvoicePlatformId { get { return _invoicePlatformId; } set { _invoicePlatformId = value; } }
        public string CustomerPlatformId { get { return _customerPlatformId; } set { _customerPlatformId = value; } }
        public string SubscriptionPlatformId { get { return _subscriptionPlatformId; } set { _subscriptionPlatformId = value; } }
        public string AmtDue { get { return _amtDue; } set { _amtDue = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}