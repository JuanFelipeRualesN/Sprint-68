namespace GSWebAPI.GSWebAPI.Domain.Models.GameUserReward
{
    public class RewardStoreTransactionModel
    {
        private string _id;
        private string _userId;
        private string _transactionId;
        private string _code;
        private string _productIdStore;
        private string _requestDate;
        private string _purchaseDate;
        private string _isTrialPeriod;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string UserId { get { return _userId; } set { _userId = value; } }
        public string TransactionId { get { return _transactionId; } set { _transactionId = value; } }
        public string Code { get { return _code; } set { _code = value; } }
        public string ProductIdStore { get { return _productIdStore; } set { _productIdStore = value; } }
        public string RequestDate { get { return _requestDate; } set { _requestDate = value; } }
        public string PurchaseDate { get { return _purchaseDate; } set { _purchaseDate = value; } }
        public string IsTrialPeriod { get { return _isTrialPeriod; } set { _isTrialPeriod = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}