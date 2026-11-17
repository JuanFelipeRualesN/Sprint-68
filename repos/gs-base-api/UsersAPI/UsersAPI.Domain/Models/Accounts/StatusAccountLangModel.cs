namespace UsersAPI.UsersAPI.Domain.Models.Accounts
{
    public class StatusAccountLangModel
    {
        private string _id;
        private string _statusId;
        private string _langCode;
        private string _desc;
        private string _modifiedBy;
        private string _CreatedAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string StatusId { get { return _statusId; } set { _statusId = value; } }
        public string LangCode { get { return _langCode; } set { _langCode = value; } }
        public string Desc { get { return _desc; } set { _desc = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _CreatedAt; } set { _CreatedAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}