namespace GSWebAPI.GSWebAPI.Domain.Models.PlatformSetting
{
    public class PlatformSettingModel
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string SchoolManagementAdmin { get; set; }
        public string EnrollmentManagementAdmin { get; set; }
        public string UserManagementAdmin { get; set; }
        public string MessageManagementAdmin { get; set; }
        public string MessageManagementSendAdmin { get; set; }
        public string ConfigurationManagerAdmin { get; set; }
        public string PasswordManagementAdmin { get; set; }
        public string SchoolManagementAdminOre { get; set; }
        public string EnrollmentManagementAdminOre { get; set; }
        public string UserManagementAdminOre { get; set; }
        public string MessageManagementAdminOre { get; set; }
        public string MessageManagementSendAdminOre { get; set; }
        public string ConfigurationManagerAdminOre { get; set; }
        public string PasswordManagementAdminOre { get; set; }
        public string UserManagementPrincipal { get; set; }
        public string MessageManagementPrincipal { get; set; }
        public string MessageManagementSendPrincipal { get; set; }
        public string ConfigurationManagerPrincipal { get; set; }
        public string PasswordManagementPrincipal { get; set; }
        public string UserManagementTeacher { get; set; }
        public string GroupInternalManagementTeacher { get; set; }
        public string InvitationManagementTeacher { get; set; }
        public string InvitationTeacherToParentManagementTeacher { get; set; }
        public string ChatManagementTeacher { get; set; }
        public string MessageManagementTeacher { get; set; }
        public string MessageManagementSendTeacher { get; set; }
        public string TeamMeetsManagementTeacher { get; set; }
        public string ConfigurationManagerTeacher { get; set; }
        public string InvitationTeacherToStudentManagementTeacher { get; set; }
        public string PasswordManagementTeacher { get; set; }
        public string ProgressManagementParent { get; set; }
        public string InvitationManagementParent { get; set; }
        public string ChatManagementParent { get; set; }
        public string MessageManagementParent { get; set; }
        public string MessageManagementSendParent { get; set; }
        public string ConfigurationManagerParent { get; set; }
        public string PasswordManagementParent { get; set; }
        public string MessageManagementStudent { get; set; }
        public string MessageManagementSendStudent { get; set; }
        public string ProgressManagementStudent { get; set; }
        public string ChatManagementStudent { get; set; }
        public string TeamsManagementStudent { get; set; }
        public string MicrosoftSSOManagement { get; set; }
        public string InternalLoginManagement { get; set; }
        public string InternalPasswordReset { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}