namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions
{
    public class PlanModel
    {
        public string Id { get; set; }
        public string PlatformId { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Interval { get; set; }
        public string IntervalCount { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
        public string IsForSchool { get; set; }
        public string IsPrivate { get; set; }
        public string ProfilesQuantity { get; set; }
        public string IsSpecialPromo { get; set; }
        public string SpecialPromoLimit { get; set; }
        public string SpecialPromoDays { get; set; }
        public string Available { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}