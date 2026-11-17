namespace UsersAPI.UsersAPI.Domain.Models.Accounts
{
    public class IpAddressUserModel
    {
        private string _id;
        private string _ipAddress;
        private string _userId;
        private string _modifiedBy;
        private string _CreatedAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string IpAddress { get { return _ipAddress; } set { _ipAddress = value; } }
        public string UserId { get { return _userId; } set { _userId = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _CreatedAt; } set { _CreatedAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}