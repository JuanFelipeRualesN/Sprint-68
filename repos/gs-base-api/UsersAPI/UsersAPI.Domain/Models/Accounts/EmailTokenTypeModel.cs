namespace UsersAPI.UsersAPI.Domain.Models.Accounts
{
    public class EmailTokenTypeModel
    {
        private string _id;
        private string _code;
        private string _desc;
        private string _duration;
        private string _modifiedBy;
        private string _CreatedAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string Code { get { return _code; } set { _code = value; } }
        public string Desc { get { return _desc; } set { _desc = value; } }
        public string Duration { get { return _duration; } set { _duration = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _CreatedAt; } set { _CreatedAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}