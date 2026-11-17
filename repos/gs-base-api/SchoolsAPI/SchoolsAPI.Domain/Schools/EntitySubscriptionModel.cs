using SchoolsAPI.Dependencies.SchoolManagerLibV2.Model;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Schools
{
    public class EntitySubscriptionModel : IResponse
    {
        public EntitySubscriptionModel()
        {
            Messages = new System.Collections.Generic.List<string>();
        }
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string EntityId { get; set; }
        public string AcademicDetailId { get; set; }
        public string SubscriptionId { get; set; }
        public string PlatformId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}