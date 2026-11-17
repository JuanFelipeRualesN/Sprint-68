namespace UsersAPI.UsersAPI.Domain.Models.Accounts
{
    public class UserAccountInternalDetailsModel
    {
        private string _userId;
        private UserAccountModel _userAccountModel;
        private UserEmailModel _emailModel; 

        public string UserId { get { return _userId; } set { _userId = value; } }
        public UserAccountModel UserAccountModel { get { return _userAccountModel; } set { _userAccountModel = value; } }
        public UserEmailModel EmailModel { get { return _emailModel; } set { _emailModel = value; } }
    }
}
