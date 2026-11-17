using UsersAPI.UsersAPI.Domain.Models.PersonalDetails;

namespace UsersAPI.UsersAPI.Domain.Models.Accounts
{
    /// <summary>
    /// Model for all user general details 
    /// </summary>
    public class UserAccountPublicDetailsModel
    {
        public UserAccountModel UserAccountModel { get; set; } 
        public UserFullNameModel UserFullNameModel { get;  set; }
        public UserEmailModel UserEmailModel { get; set; }
        public UserPhoneNumberModel UserPhoneNumber { get; set; }
        public UserAddressModel UserAddressModel { get; set; }
        public UserDobModel UserDobModel { get; set; }
        public UserGenderModel UserGenderModel { get; set; }
    }
}
