namespace GSWebAPI.GSWebAPI.Domain.Models.GameUserReward
{
    public class RewardCodeModel
    {
        private string _id;
        private string _rewardCode;
        private string _rewardType;
        private string _rewardQuantity;
        private string _multipleUse;
        private string _startDate;
        private string _endDate;
        private string _couponQuantity;
        private string _oneUsePerCustomer;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string RewardCode { get { return _rewardCode; } set { _rewardCode = value; } }
        public string RewardType { get { return _rewardType; } set { _rewardType = value; } }
        public string RewardQuantity { get { return _rewardQuantity; } set { _rewardQuantity = value; } }
        public string MultipleUse { get { return _multipleUse; } set { _multipleUse = value; } }
        public string StartDate { get { return _startDate; } set { _startDate = value; } }
        public string EndDate { get { return _endDate; } set { _endDate = value; } }
        public string CouponQuantity { get { return _couponQuantity; } set { _couponQuantity = value; } }
        public string OneUsePerCustomer { get { return _oneUsePerCustomer; } set { _oneUsePerCustomer = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}