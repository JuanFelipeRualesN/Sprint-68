namespace UsersAPI.UsersAPI.Domain.Models.Accounts
{
    public class StatusAccountModel
    {
        private string _id;
        private string _desc;
        private string _code;
        private string _allowAccess;
        private string _modifiedBy;
        private string _CreatedAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string Desc { get { return _desc; } set { _desc = value; } }
        public string Code { get { return _code; } set { _code = value; } }
        public string AllowAccess { get { return _allowAccess; } set { _allowAccess = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _CreatedAt; } set { _CreatedAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}