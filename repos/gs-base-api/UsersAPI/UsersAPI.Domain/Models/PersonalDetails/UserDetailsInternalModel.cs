namespace UsersAPI.UsersAPI.Domain.Models.PersonalDetails
{
    public class UserDetailsInternalModel
    {
        private string _userId;
        private UserFullNameModel _userFullNameModel;
        private UserPhoneNumberModel _userPhoneNumberModel;
        private UserDobModel _userDobModel;
        private UserGenderModel _userGenderModel; 

        public string UserId { get { return _userId; } set { _userId = value; } }
        public UserFullNameModel UserFullNameModel { get { return _userFullNameModel; } set { _userFullNameModel = value; } }
        public UserPhoneNumberModel UserPhoneNumberModel { get { return _userPhoneNumberModel; } set { _userPhoneNumberModel = value; } }
        public UserDobModel UserDobModel { get { return _userDobModel; } set { _userDobModel = value; } }
        public UserGenderModel UserGenderModel { get { return _userGenderModel; } set { _userGenderModel = value; } }
        public UserAddressModel UserAddressModel { get; set; }
    }
}
