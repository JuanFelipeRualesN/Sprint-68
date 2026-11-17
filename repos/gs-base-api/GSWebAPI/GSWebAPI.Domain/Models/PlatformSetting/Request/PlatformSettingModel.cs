using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.PlatformSetting.Request
{
    public class PlatformSettingModel
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public bool SchoolManagementAdmin { get; set; }
        public bool EnrollmentManagementAdmin { get; set; }
        public bool UserManagementAdmin { get; set; }
        public bool MessageManagementAdmin { get; set; }
        public bool MessageManagementSendAdmin { get; set; }
        public bool ConfigurationManagerAdmin { get; set; }
        public bool PasswordManagementAdmin { get; set; }
        public bool SchoolManagementAdminOre { get; set; }
        public bool EnrollmentManagementAdminOre { get; set; }
        public bool UserManagementAdminOre { get; set; }
        public bool MessageManagementAdminOre { get; set; }
        public bool MessageManagementSendAdminOre { get; set; }
        public bool ConfigurationManagerAdminOre { get; set; }
        public bool PasswordManagementAdminOre { get; set; }
        public bool UserManagementPrincipal { get; set; }
        public bool MessageManagementPrincipal { get; set; }
        public bool MessageManagementSendPrincipal { get; set; }
        public bool ConfigurationManagerPrincipal { get; set; }
        public bool PasswordManagementPrincipal { get; set; }
        public bool UserManagementTeacher { get; set; }
        public bool GroupInternalManagementTeacher { get; set; }
        public bool InvitationManagementTeacher { get; set; }
        public bool InvitationTeacherToParentManagementTeacher { get; set; }
        public bool ChatManagementTeacher { get; set; }
        public bool MessageManagementTeacher { get; set; }
        public bool MessageManagementSendTeacher { get; set; }
        public bool TeamMeetsManagementTeacher { get; set; }
        public bool ConfigurationManagerTeacher { get; set; }
        public bool InvitationTeacherToStudentManagementTeacher { get; set; }
        public bool PasswordManagementTeacher { get; set; }
        public bool ProgressManagementParent { get; set; }
        public bool InvitationManagementParent { get; set; }
        public bool ChatManagementParent { get; set; }
        public bool MessageManagementParent { get; set; }
        public bool MessageManagementSendParent { get; set; }
        public bool ConfigurationManagerParent { get; set; }
        public bool PasswordManagementParent { get; set; }
        public bool MessageManagementStudent { get; set; }
        public bool MessageManagementSendStudent { get; set; }
        public bool ProgressManagementStudent { get; set; }
        public bool ChatManagementStudent { get; set; }
        public bool TeamsManagementStudent { get; set; }
        public bool MicrosoftSSOManagement { get; set; }
        public bool InternalLoginManagement { get; set; }
        public bool InternalPasswordReset { get; set; }

        public bool CheckSetting(string propertyToCheck)
        {
            PropertyInfo propertyInfo = typeof(PlatformSettingModel).GetProperty(propertyToCheck, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo != null && propertyInfo.PropertyType == typeof(bool))
            {
                bool value = (bool)propertyInfo.GetValue(this);
                return value;
            }

            // Property not found or not of type bool
            // You might want to handle this case accordingly
            return false;
        }
    }
}