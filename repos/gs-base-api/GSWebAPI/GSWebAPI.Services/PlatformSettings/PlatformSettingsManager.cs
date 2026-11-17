using GSWebAPI.GSWebAPI.Domain.Models.PlatformSetting;
using System;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.PlatformSettings;

namespace GSWebAPI.GSWebAPI.Services.PlatformSettings
{
    /// <summary>
    /// To handle the platform settings.
    /// </summary>
    public class PlatformSettingsManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">Connection string to CUD.</param>
        /// <param name="connectionStringRead">Connection string to read.</param>
        public PlatformSettingsManager(string connectionString, string connectionStringRead)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
        }

        /// <summary>
        /// To check specific setting status.
        /// </summary>
        /// <param name="plaformSetting">The platform setting to check.</param>
        /// <returns>Returns true if available, else, false.</returns>
        public bool CheckPlatformSettingStatus(string plaformSetting)
        {
            var platformSettings = GetPlatformSettings();
            return platformSettings.CheckSetting(plaformSetting);
        }

        /// <summary>
        /// To get the platform settings. Returns the id.
        /// </summary>
        /// <returns>Returns the platform settings models.s</returns>
        public Domain.Models.PlatformSetting.Request.PlatformSettingModel GetPlatformSettings()
        {
            var platformSettings = new PlatformSettingDAO().Read(this._connectionStringRead, new
                PlatformSettingModel(), false, false);
            if (platformSettings == null || platformSettings.Count == 0)
            {
                return new Domain.Models.PlatformSetting.Request.PlatformSettingModel();
            }
            return new Domain.Models.PlatformSetting.Request.PlatformSettingModel()
            {
                SchoolManagementAdmin = platformSettings[0].SchoolManagementAdmin == "1",
                SchoolManagementAdminOre = platformSettings[0].SchoolManagementAdminOre == "1",
                MessageManagementSendAdmin = platformSettings[0].MessageManagementSendAdmin == "1",
                MessageManagementSendAdminOre = platformSettings[0].MessageManagementSendAdminOre == "1",
                MessageManagementSendParent = platformSettings[0].MessageManagementSendParent == "1",
                MessageManagementSendPrincipal = platformSettings[0].MessageManagementSendPrincipal == "1",
                MessageManagementSendTeacher = platformSettings[0].MessageManagementSendTeacher == "1",
                MicrosoftSSOManagement = platformSettings[0].MicrosoftSSOManagement == "1",
                InvitationTeacherToStudentManagementTeacher = platformSettings[0].InvitationTeacherToStudentManagementTeacher == "1",
                ChatManagementParent = platformSettings[0].ChatManagementParent == "1",
                ChatManagementTeacher = platformSettings[0].ChatManagementTeacher == "1",
                ConfigurationManagerAdmin = platformSettings[0].ConfigurationManagerAdmin == "1",
                ConfigurationManagerAdminOre = platformSettings[0].ConfigurationManagerAdminOre == "1",
                ConfigurationManagerTeacher = platformSettings[0].ConfigurationManagerTeacher == "1",
                EnrollmentManagementAdmin = platformSettings[0].EnrollmentManagementAdmin == "1",
                EnrollmentManagementAdminOre = platformSettings[0].EnrollmentManagementAdminOre == "1",
                GroupInternalManagementTeacher = platformSettings[0].GroupInternalManagementTeacher == "1",
                InternalLoginManagement = platformSettings[0].InternalLoginManagement == "1",
                InternalPasswordReset = platformSettings[0].InternalPasswordReset == "1",
                InvitationManagementParent = platformSettings[0].InvitationManagementParent == "1",
                InvitationManagementTeacher = platformSettings[0].InvitationManagementTeacher == "1",
                InvitationTeacherToParentManagementTeacher = platformSettings[0].InvitationTeacherToParentManagementTeacher == "1",
                MessageManagementAdmin = platformSettings[0].MessageManagementAdmin == "1",
                MessageManagementAdminOre = platformSettings[0].MessageManagementAdminOre == "1",
                MessageManagementParent = platformSettings[0].MessageManagementParent == "1",
                MessageManagementPrincipal = platformSettings[0].MessageManagementPrincipal == "1",
                MessageManagementTeacher = platformSettings[0].MessageManagementTeacher == "1",
                PasswordManagementAdmin = platformSettings[0].PasswordManagementAdmin == "1",
                PasswordManagementAdminOre = platformSettings[0].PasswordManagementAdminOre == "1",
                PasswordManagementTeacher = platformSettings[0].PasswordManagementTeacher == "1",
                ProgressManagementParent = platformSettings[0].ProgressManagementParent == "1",
                TeamMeetsManagementTeacher = platformSettings[0].TeamMeetsManagementTeacher == "1",
                UserManagementAdmin = platformSettings[0].UserManagementAdmin == "1",
                UserManagementAdminOre = platformSettings[0].UserManagementAdminOre == "1",
                UserManagementPrincipal = platformSettings[0].UserManagementPrincipal == "1",
                UserManagementTeacher = platformSettings[0].UserManagementTeacher == "1",
                TeamsManagementStudent = platformSettings[0].TeamsManagementStudent == "1",
                ChatManagementStudent = platformSettings[0].ChatManagementStudent == "1",
                MessageManagementSendStudent = platformSettings[0].MessageManagementSendStudent == "1",
                MessageManagementStudent = platformSettings[0].MessageManagementStudent == "1",
                ProgressManagementStudent = platformSettings[0].ProgressManagementStudent == "1",
                ConfigurationManagerParent = platformSettings[0].ConfigurationManagerParent == "1",
                ConfigurationManagerPrincipal = platformSettings[0].ConfigurationManagerPrincipal == "1",
                PasswordManagementPrincipal = platformSettings[0].PasswordManagementPrincipal == "1",
                PasswordManagementParent = platformSettings[0].PasswordManagementParent == "1",
                Id = platformSettings[0].Id,
                Valid = true,
            };
        }

        /// <summary>
        /// To modify or create a platform.
        /// </summary>
        /// <param name="platformSetting">Platform setting.</param>
        /// <returns>Returns the plaform settings.</returns>
        public Domain.Models.PlatformSetting.Request.PlatformSettingModel ModifyPlatformSetting(Domain.Models.PlatformSetting.Request.PlatformSettingModel platformSetting)
        {
            // If read, update, else, insert
            var platformSettingsList = new PlatformSettingDAO().Read(this._connectionString,
                new PlatformSettingModel()
                {
                    Id = platformSetting.Id,
                }, true, true);

            if (platformSettingsList != null && platformSettingsList.Count > 0)
            {
                try
                {
                    new PlatformSettingDAO().Update(this._connectionString,
                        new PlatformSettingModel()
                        {
                            SchoolManagementAdmin = platformSetting.SchoolManagementAdmin ? "1" : "0",
                            SchoolManagementAdminOre = platformSetting.SchoolManagementAdminOre ? "1" : "0",
                            MessageManagementSendAdmin = platformSetting.MessageManagementSendAdmin ? "1" : "0",
                            MessageManagementSendAdminOre = platformSetting.MessageManagementSendAdminOre ? "1" : "0",
                            MessageManagementSendParent = platformSetting.MessageManagementSendParent ? "1" : "0",
                            MessageManagementSendPrincipal = platformSetting.MessageManagementSendPrincipal ? "1" : "0",
                            MessageManagementSendTeacher = platformSetting.MessageManagementSendTeacher ? "1" : "0",
                            MicrosoftSSOManagement = platformSetting.MicrosoftSSOManagement ? "1" : "0",
                            InvitationTeacherToStudentManagementTeacher = platformSetting.InvitationTeacherToStudentManagementTeacher ? "1" : "0",
                            ChatManagementParent = platformSetting.ChatManagementParent ? "1" : "0",
                            ChatManagementTeacher = platformSetting.ChatManagementTeacher ? "1" : "0",
                            ConfigurationManagerAdmin = platformSetting.ConfigurationManagerAdmin ? "1" : "0",
                            ConfigurationManagerAdminOre = platformSetting.ConfigurationManagerAdminOre ? "1" : "0",
                            ConfigurationManagerTeacher = platformSetting.ConfigurationManagerTeacher ? "1" : "0",
                            EnrollmentManagementAdmin = platformSetting.EnrollmentManagementAdmin ? "1" : "0",
                            EnrollmentManagementAdminOre = platformSetting.EnrollmentManagementAdminOre ? "1" : "0",
                            GroupInternalManagementTeacher = platformSetting.GroupInternalManagementTeacher ? "1" : "0",
                            InternalLoginManagement = platformSetting.InternalLoginManagement ? "1" : "0",
                            InternalPasswordReset = platformSetting.InternalPasswordReset ? "1" : "0",
                            InvitationManagementParent = platformSetting.InvitationManagementParent ? "1" : "0",
                            InvitationManagementTeacher = platformSetting.InvitationManagementTeacher ? "1" : "0",
                            InvitationTeacherToParentManagementTeacher = platformSetting.InvitationTeacherToParentManagementTeacher ? "1" : "0",
                            MessageManagementAdmin = platformSetting.MessageManagementAdmin ? "1" : "0",
                            MessageManagementAdminOre = platformSetting.MessageManagementAdminOre ? "1" : "0",
                            MessageManagementParent = platformSetting.MessageManagementParent ? "1" : "0",
                            MessageManagementPrincipal = platformSetting.MessageManagementPrincipal ? "1" : "0",
                            MessageManagementTeacher = platformSetting.MessageManagementTeacher ? "1" : "0",
                            PasswordManagementAdmin = platformSetting.PasswordManagementAdmin ? "1" : "0",
                            PasswordManagementAdminOre = platformSetting.PasswordManagementAdminOre ? "1" : "0",
                            PasswordManagementTeacher = platformSetting.PasswordManagementTeacher ? "1" : "0",
                            ProgressManagementParent = platformSetting.ProgressManagementParent ? "1" : "0",
                            TeamMeetsManagementTeacher = platformSetting.TeamMeetsManagementTeacher ? "1" : "0",
                            UserManagementAdmin = platformSetting.UserManagementAdmin ? "1" : "0",
                            UserManagementAdminOre = platformSetting.UserManagementAdminOre ? "1" : "0",
                            UserManagementPrincipal = platformSetting.UserManagementPrincipal ? "1" : "0",
                            UserManagementTeacher = platformSetting.UserManagementTeacher ? "1" : "0",
                            TeamsManagementStudent = platformSetting.TeamsManagementStudent ? "1" : "0",
                            ChatManagementStudent = platformSetting.ChatManagementStudent ? "1" : "0",
                            MessageManagementSendStudent = platformSetting.MessageManagementSendStudent ? "1" : "0",
                            MessageManagementStudent = platformSetting.MessageManagementStudent ? "1" : "0",
                            ProgressManagementStudent = platformSetting.ProgressManagementStudent ? "1" : "0",
                            ConfigurationManagerParent = platformSetting.ConfigurationManagerParent ? "1" : "0",
                            ConfigurationManagerPrincipal = platformSetting.ConfigurationManagerPrincipal ? "1" : "0",
                            PasswordManagementParent = platformSetting.PasswordManagementParent ? "1" : "0",
                            PasswordManagementPrincipal = platformSetting.PasswordManagementPrincipal ? "1" : "0",
                        }, new PlatformSettingModel()
                        {
                            Id = platformSettingsList[0].Id,
                        }); ;
                    platformSetting.Valid = true;
                    return platformSetting;
                }
                catch (Exception)
                {
                    platformSetting.Valid = false;
                    return platformSetting;
                }
            }
            else
            {
                try
                {
                    new PlatformSettingDAO().Create(this._connectionString,
                        new PlatformSettingModel()
                        {
                            SchoolManagementAdmin = platformSetting.SchoolManagementAdmin ? "1" : "0",
                            SchoolManagementAdminOre = platformSetting.SchoolManagementAdminOre ? "1" : "0",
                            MessageManagementSendAdmin = platformSetting.MessageManagementSendAdmin ? "1" : "0",
                            MessageManagementSendAdminOre = platformSetting.MessageManagementSendAdminOre ? "1" : "0",
                            MessageManagementSendParent = platformSetting.MessageManagementSendParent ? "1" : "0",
                            MessageManagementSendPrincipal = platformSetting.MessageManagementSendPrincipal ? "1" : "0",
                            MessageManagementSendTeacher = platformSetting.MessageManagementSendTeacher ? "1" : "0",
                            MicrosoftSSOManagement = platformSetting.MicrosoftSSOManagement ? "1" : "0",
                            InvitationTeacherToStudentManagementTeacher = platformSetting.InvitationTeacherToStudentManagementTeacher ? "1" : "0",
                            ChatManagementParent = platformSetting.ChatManagementParent ? "1" : "0",
                            ChatManagementTeacher = platformSetting.ChatManagementTeacher ? "1" : "0",
                            ConfigurationManagerAdmin = platformSetting.ConfigurationManagerAdmin ? "1" : "0",
                            ConfigurationManagerAdminOre = platformSetting.ConfigurationManagerAdminOre ? "1" : "0",
                            ConfigurationManagerTeacher = platformSetting.ConfigurationManagerTeacher ? "1" : "0",
                            EnrollmentManagementAdmin = platformSetting.EnrollmentManagementAdmin ? "1" : "0",
                            EnrollmentManagementAdminOre = platformSetting.EnrollmentManagementAdminOre ? "1" : "0",
                            GroupInternalManagementTeacher = platformSetting.GroupInternalManagementTeacher ? "1" : "0",
                            InternalLoginManagement = platformSetting.InternalLoginManagement ? "1" : "0",
                            InternalPasswordReset = platformSetting.InternalPasswordReset ? "1" : "0",
                            InvitationManagementParent = platformSetting.InvitationManagementParent ? "1" : "0",
                            InvitationManagementTeacher = platformSetting.InvitationManagementTeacher ? "1" : "0",
                            InvitationTeacherToParentManagementTeacher = platformSetting.InvitationTeacherToParentManagementTeacher ? "1" : "0",
                            MessageManagementAdmin = platformSetting.MessageManagementAdmin ? "1" : "0",
                            MessageManagementAdminOre = platformSetting.MessageManagementAdminOre ? "1" : "0",
                            MessageManagementParent = platformSetting.MessageManagementParent ? "1" : "0",
                            MessageManagementPrincipal = platformSetting.MessageManagementPrincipal ? "1" : "0",
                            MessageManagementTeacher = platformSetting.MessageManagementTeacher ? "1" : "0",
                            PasswordManagementAdmin = platformSetting.PasswordManagementAdmin ? "1" : "0",
                            PasswordManagementAdminOre = platformSetting.PasswordManagementAdminOre ? "1" : "0",
                            PasswordManagementTeacher = platformSetting.PasswordManagementTeacher ? "1" : "0",
                            ProgressManagementParent = platformSetting.ProgressManagementParent ? "1" : "0",
                            TeamMeetsManagementTeacher = platformSetting.TeamMeetsManagementTeacher ? "1" : "0",
                            UserManagementAdmin = platformSetting.UserManagementAdmin ? "1" : "0",
                            UserManagementAdminOre = platformSetting.UserManagementAdminOre ? "1" : "0",
                            UserManagementPrincipal = platformSetting.UserManagementPrincipal ? "1" : "0",
                            UserManagementTeacher = platformSetting.UserManagementTeacher ? "1" : "0",
                            TeamsManagementStudent = platformSetting.TeamsManagementStudent ? "1" : "0",
                            ChatManagementStudent = platformSetting.ChatManagementStudent ? "1" : "0",
                            MessageManagementSendStudent = platformSetting.MessageManagementSendStudent ? "1" : "0",
                            MessageManagementStudent = platformSetting.MessageManagementStudent ? "1" : "0",
                            ProgressManagementStudent = platformSetting.ProgressManagementStudent ? "1" : "0",
                            ConfigurationManagerPrincipal = platformSetting.ConfigurationManagerPrincipal ? "1" : "0",
                            ConfigurationManagerParent = platformSetting.ConfigurationManagerParent ? "1" : "0",
                            PasswordManagementParent = platformSetting.PasswordManagementParent ? "1" : "0",
                            PasswordManagementPrincipal = platformSetting.PasswordManagementPrincipal ? "1" : "0"
                        });
                    platformSetting.Valid = true;
                    return platformSetting;
                }
                catch (Exception)
                {
                    platformSetting.Valid = false;
                    return platformSetting;
                }
            }
        }
    }
}