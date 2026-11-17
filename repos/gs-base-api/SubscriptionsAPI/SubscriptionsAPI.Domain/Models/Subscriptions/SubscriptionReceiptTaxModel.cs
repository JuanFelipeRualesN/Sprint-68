namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions
{
    public class SubscriptionReceiptTaxModel
    {
        private string _id;
        private string _subscriptionReceiptId;
        private string _taxId;
        private string _taxName;
        private string _taxPercent;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string SubscriptionReceiptId { get { return _subscriptionReceiptId; } set { _subscriptionReceiptId = value; } }
        public string TaxId { get { return _taxId; } set { _taxId = value; } }
        public string TaxName { get { return _taxName; } set { _taxName = value; } }
        public string TaxPercent { get { return _taxPercent; } set { _taxPercent = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}