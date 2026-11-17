namespace UsersAPI.UsersAPI.Domain.Models.Accounts
{
    public class UserDetailsExternalResponseModel
    {
        private string _username;
        private string _email;
        private string _roleId;
        private string _roleDesc; 
        private string _name;
        private string _lastName;
        private string _phoneNumber; 

        public string username { get { return _username; } set { _username = value; } }
        public string email { get { return _email; } set { _email = value; } }
        public string roleId { get { return _roleId; } set { _roleId = value; } }
        public string roleDesc { get { return _roleDesc; } set { _roleDesc = value; } }
        public string name { get { return _name; } set { _name = value; } }
        public string lastName { get { return _lastName; } set { _lastName = value; } }
        public string phoneNumber { get { return _phoneNumber; } set { _phoneNumber = value; } }
    }
}
