namespace UsersAPI.UsersAPI.Domain.Models.PersonalDetails
{
    public class UserDetailsResponseModel
    {
        private string _fullName;
        private string _givenName;
        private string _surname;
        private string _suffix;
        private string _mainPhone;
        private string _sex;
        private string _dob;

        public string FullName { get { return _fullName; } set { _fullName = value; } }
        public string GivenName { get { return _givenName; } set { _givenName = value; } }
        public string Surname { get { return _surname; } set { _surname = value; } }
        public string Suffix { get { return _suffix; } set { _suffix = value; } }
        public string MainPhone { get { return _mainPhone; } set { _mainPhone = value; } }
        public string Sex { get { return _sex; } set { _sex = value; } }
        public string Dob { get { return _dob; } set { _dob = value; } }

    }
}