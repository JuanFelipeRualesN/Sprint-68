namespace UsersAPI.UsersAPI.Domain.Models.Accounts
{
    public class EmailValidationUserModel
    {
        private string _id;
        private string _email;
        private string _userId;
        private string _emailTypeId;
        private string _token;
        private string _used;
        private string _modifiedBy;
        private string _CreatedAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string Email { get { return _email; } set { _email = value; } }
        public string UserId { get { return _userId; } set { _userId = value; } }
        public string EmailTypeId { get { return _emailTypeId; } set { _emailTypeId = value; } }
        public string Token { get { return _token; } set { _token = value; } }
        public string Used { get { return _used; } set { _used = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _CreatedAt; } set { _CreatedAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}