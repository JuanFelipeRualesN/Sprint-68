namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions
{
    public class SubscriptionReceiptModel
    {
        public string Id { get; set; }
        public string SubscriptionId { get; set; }
        public string TrialDays { get; set; }
        public string TrialDateEnd { get; set; }
        public string SubTotal { get; set; }
        public string TaxTotal { get; set; }
        public string Total { get; set; }
        public string Currency { get; set; }
        public string CouponId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}