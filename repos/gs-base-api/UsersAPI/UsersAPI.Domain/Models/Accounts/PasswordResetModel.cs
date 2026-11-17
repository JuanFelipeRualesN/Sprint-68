namespace UsersAPI.UsersAPI.Domain.Models.Accounts
{
    class PasswordResetModel
    {
        private string _id;
        private string _userID;
        private string _token;
        private string _modifiedBy;
        private string _CreatedAt;
        private string _updatedAt;

        public string id { get { return _id; } set { _id = value; } }
        public string userID { get { return _userID; } set { _userID = value; } }
        public string token { get { return _token; } set { _token = value; } }
        public string modifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _CreatedAt; } set { _CreatedAt = value; } }
        public string updatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}
