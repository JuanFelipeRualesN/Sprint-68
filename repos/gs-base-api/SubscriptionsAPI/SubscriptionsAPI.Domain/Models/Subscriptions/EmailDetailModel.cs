namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions
{
    public class EmailDetailModel
    {
        private string _id;
        private string _code;
        private string _recipients;
        private string _title;
        private string _message;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string Code { get { return _code; } set { _code = value; } }
        public string Recipients { get { return _recipients; } set { _recipients = value; } }
        public string Title { get { return _title; } set { _title = value; } }
        public string Message { get { return _message; } set { _message = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}