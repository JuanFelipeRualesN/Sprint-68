namespace UsersAPI.UsersAPI.Domain.Models.Accounts
{
    public class UserLoginAttemptModel
    {
        private string _id;
        private string _userId;
        private string _attempt;
        private string _lastLogin;
        private string _modifiedBy;
        private string _CreatedAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string UserId { get { return _userId; } set { _userId = value; } }
        public string Attempt { get { return _attempt; } set { _attempt = value; } }
        public string LastLogin { get { return _lastLogin; } set { _lastLogin = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _CreatedAt; } set { _CreatedAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}