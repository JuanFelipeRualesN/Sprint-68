namespace UsersAPI.UsersAPI.Domain.Models.PersonalDetails
{
    public class UserFullNameModel
    {
        private string _userId;
        private string _givenName;
        private string _surname;
        private string _suffix;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string UserId { get { return _userId; } set { _userId = value; } }
        public string GivenName { get { return _givenName; } set { _givenName = value; } }
        public string Surname { get { return _surname; } set { _surname = value; } }
        public string Suffix { get { return _suffix; } set { _suffix = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}