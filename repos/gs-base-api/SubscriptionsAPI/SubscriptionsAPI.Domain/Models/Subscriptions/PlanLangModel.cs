namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions
{
    public class PlanLangModel
    {
        private string _id;
        private string _planId;
        private string _langCode;
        private string _name;
        private string _description;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string PlanId { get { return _planId; } set { _planId = value; } }
        public string LangCode { get { return _langCode; } set { _langCode = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}