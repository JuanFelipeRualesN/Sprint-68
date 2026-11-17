namespace UsersAPI.UsersAPI.Domain.Models.PersonalDetails
{
    public class UserPhoneNumberModel
    {
        private string _userId;
        private string _phoneNumberTypeCode;
        private string _phoneNumber;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string UserId { get { return _userId; } set { _userId = value; } }
        public string PhoneNumberTypeCode { get { return _phoneNumberTypeCode; } set { _phoneNumberTypeCode = value; } }
        public string PhoneNumber { get { return _phoneNumber; } set { _phoneNumber = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}