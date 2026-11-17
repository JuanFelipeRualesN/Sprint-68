namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals
{
    public class ExamTryMaxModel : IResponse
    {
        public ExamTryMaxModel()
        {
            Messages = new System.Collections.Generic.List<string>();
        }
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string SubscriptionId { get; set; }
        public string TryMax { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}