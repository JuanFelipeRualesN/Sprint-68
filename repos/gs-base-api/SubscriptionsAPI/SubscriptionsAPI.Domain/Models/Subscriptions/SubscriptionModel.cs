namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions
{
    public class SubscriptionModel : IResponse
    {
        public SubscriptionModel()
        {
            Messages = new System.Collections.Generic.List<string>();
        }
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string PlatformId { get; set; }
        public string CustomerPlatformId { get; set; }
        public string PlanPlatformId { get; set; }
        public string Quantity { get; set; }
        public string Status { get; set; }
        public string CurrentPeriodEnd { get; set; }
        public string PlatformCode { get; set; }
        public string Type { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}