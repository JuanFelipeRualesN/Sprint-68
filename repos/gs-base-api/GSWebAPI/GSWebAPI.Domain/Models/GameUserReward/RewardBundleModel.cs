namespace GSWebAPI.GSWebAPI.Domain.Models.GameUserReward
{
    /// <summary>
    /// This is for the Rewards Bundles in Google Play and the App Store. The ID must be the same of each store to now what the 
    /// user is validating
    /// </summary>
    public class RewardBundleModel
    {
        private string _id;
        private string _productIdStore;
        private string _rewardType;
        private string _rewardQuantity;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string ProductIdStore { get { return _productIdStore; } set { _productIdStore = value; } }
        public string RewardType { get { return _rewardType; } set { _rewardType = value; } }
        public string RewardQuantity { get { return _rewardQuantity; } set { _rewardQuantity = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}