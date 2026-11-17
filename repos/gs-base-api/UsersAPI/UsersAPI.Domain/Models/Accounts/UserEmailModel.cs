namespace UsersAPI.UsersAPI.Domain.Models.Accounts
{
    public class UserEmailModel
    {
        private string _id;
        private string _userId;
        private string _email;
        private string _validated;
        private string _modifiedBy;
        private string _CreatedAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string UserId { get { return _userId; } set { _userId = value; } }
        public string Email { get { return _email; } set { _email = value; } }
        public string Validated { get { return _validated; } set { _validated = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _CreatedAt; } set { _CreatedAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}