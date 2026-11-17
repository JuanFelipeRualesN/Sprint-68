namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions
{
    public class ProductItemProductModel
    {
        private string _id;
        private string _itemId;
        private string _productId;
        private string _available;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string ItemId { get { return _itemId; } set { _itemId = value; } }
        public string ProductId { get { return _productId; } set { _productId = value; } }
        public string Available { get { return _available; } set { _available = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}