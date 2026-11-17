namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions
{
    public class CancellationLogModel : IResponse
    {
        public CancellationLogModel()
        {
            Messages = new System.Collections.Generic.List<string>();
        }
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string SubscriptionId { get; set; }
        public string CancellationNow { get; set; }
        public string CancellationReasonId { get; set; }
        public string Reason { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}