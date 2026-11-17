using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Domain.Models.PlatformSetting;
namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.PlatformSettings
{
    class PlatformSettingDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, PlatformSettingModel> ReadD(string connectionString, PlatformSettingModel platformSetting, bool firstOne, bool orderByLastDate, PlatformSettingModel platformSettingWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlatformSettingModel> response = new Dictionary<string, PlatformSettingModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `school_management_admin`, `enrollment_management_admin`, `user_management_admin`, `message_management_admin`, `message_management_send_admin`, `configuration_manager_admin`, `password_management_admin`, `school_management_admin_ore`, `enrollment_management_admin_ore`, `user_management_admin_ore`, `message_management_admin_ore`, `message_management_send_admin_ore`, `configuration_manager_admin_ore`, `password_management_admin_ore`, `user_management_principal`, `message_management_principal`, `message_management_send_principal`, `configuration_manager_principal`, `password_management_principal`, `user_management_teacher`, `group_internal_management_teacher`, `invitation_management_teacher`, `invitation_teacher_to_parent_management_teacher`, `chat_management_teacher`, `message_management_teacher`, `message_management_send_teacher`, `team_meets_management_teacher`, `configuration_manager_teacher`, `invitation_teacher_to_student_management_teacher`, `password_management_teacher`, `progress_management_parent`, `invitation_management_parent`, `chat_management_parent`, `message_management_parent`, `message_management_send_parent`, `configuration_manager_parent`, `password_management_parent`, `message_management_student`, `message_management_send_student`, `progress_management_student`, `chat_management_student`, `teams_management_student`, `microsoft_sso_management`, `internal_login_management`, `internal_password_reset`, `modified_by`, `created_at`, `updated_at`  FROM platform_settings ";
                {
                    if (platformSetting != null)
                    {
                        if (!String.IsNullOrEmpty(platformSetting.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @Id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @Id ");
                            }
                            cmd.Parameters.AddWithValue("@Id", platformSetting.Id);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.SchoolManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `school_management_admin` = @SchoolManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `school_management_admin` = @SchoolManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@SchoolManagementAdmin", platformSetting.SchoolManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.EnrollmentManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin` = @EnrollmentManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin` = @EnrollmentManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdmin", platformSetting.EnrollmentManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_admin` = @UserManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_admin` = @UserManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementAdmin", platformSetting.UserManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_admin` = @MessageManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_admin` = @MessageManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementAdmin", platformSetting.MessageManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin` = @MessageManagementSendAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin` = @MessageManagementSendAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdmin", platformSetting.MessageManagementSendAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin` = @ConfigurationManagerAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin` = @ConfigurationManagerAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdmin", platformSetting.ConfigurationManagerAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_admin` = @PasswordManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_admin` = @PasswordManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementAdmin", platformSetting.PasswordManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.SchoolManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `school_management_admin_ore` = @SchoolManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `school_management_admin_ore` = @SchoolManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminOre", platformSetting.SchoolManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.EnrollmentManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOre", platformSetting.EnrollmentManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_admin_ore` = @UserManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_admin_ore` = @UserManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementAdminOre", platformSetting.UserManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_admin_ore` = @MessageManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_admin_ore` = @MessageManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementAdminOre", platformSetting.MessageManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin_ore` = @MessageManagementSendAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin_ore` = @MessageManagementSendAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminOre", platformSetting.MessageManagementSendAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOre", platformSetting.ConfigurationManagerAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_admin_ore` = @PasswordManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_admin_ore` = @PasswordManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminOre", platformSetting.PasswordManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_principal` = @UserManagementPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_principal` = @UserManagementPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementPrincipal", platformSetting.UserManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_principal` = @MessageManagementPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_principal` = @MessageManagementPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementPrincipal", platformSetting.MessageManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_principal` = @MessageManagementSendPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_principal` = @MessageManagementSendPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendPrincipal", platformSetting.MessageManagementSendPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_principal` = @ConfigurationManagerPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_principal` = @ConfigurationManagerPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipal", platformSetting.ConfigurationManagerPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_principal` = @PasswordManagementPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_principal` = @PasswordManagementPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementPrincipal", platformSetting.PasswordManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_teacher` = @UserManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_teacher` = @UserManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementTeacher", platformSetting.UserManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.GroupInternalManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `group_internal_management_teacher` = @GroupInternalManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `group_internal_management_teacher` = @GroupInternalManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@GroupInternalManagementTeacher", platformSetting.GroupInternalManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_management_teacher` = @InvitationManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_management_teacher` = @InvitationManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationManagementTeacher", platformSetting.InvitationManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationTeacherToParentManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacher", platformSetting.InvitationTeacherToParentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ChatManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `chat_management_teacher` = @ChatManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `chat_management_teacher` = @ChatManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@ChatManagementTeacher", platformSetting.ChatManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_teacher` = @MessageManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_teacher` = @MessageManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementTeacher", platformSetting.MessageManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_teacher` = @MessageManagementSendTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_teacher` = @MessageManagementSendTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendTeacher", platformSetting.MessageManagementSendTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.TeamMeetsManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `team_meets_management_teacher` = @TeamMeetsManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `team_meets_management_teacher` = @TeamMeetsManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacher", platformSetting.TeamMeetsManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_teacher` = @ConfigurationManagerTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_teacher` = @ConfigurationManagerTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerTeacher", platformSetting.ConfigurationManagerTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationTeacherToStudentManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacher", platformSetting.InvitationTeacherToStudentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_teacher` = @PasswordManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_teacher` = @PasswordManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementTeacher", platformSetting.PasswordManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ProgressManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `progress_management_parent` = @ProgressManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `progress_management_parent` = @ProgressManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@ProgressManagementParent", platformSetting.ProgressManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_management_parent` = @InvitationManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_management_parent` = @InvitationManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationManagementParent", platformSetting.InvitationManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ChatManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `chat_management_parent` = @ChatManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `chat_management_parent` = @ChatManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@ChatManagementParent", platformSetting.ChatManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_parent` = @MessageManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_parent` = @MessageManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementParent", platformSetting.MessageManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_parent` = @MessageManagementSendParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_parent` = @MessageManagementSendParent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendParent", platformSetting.MessageManagementSendParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_parent` = @ConfigurationManagerParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_parent` = @ConfigurationManagerParent ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerParent", platformSetting.ConfigurationManagerParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_parent` = @PasswordManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_parent` = @PasswordManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementParent", platformSetting.PasswordManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_student` = @MessageManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_student` = @MessageManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementStudent", platformSetting.MessageManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_student` = @MessageManagementSendStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_student` = @MessageManagementSendStudent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendStudent", platformSetting.MessageManagementSendStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ProgressManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `progress_management_student` = @ProgressManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `progress_management_student` = @ProgressManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@ProgressManagementStudent", platformSetting.ProgressManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ChatManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `chat_management_student` = @ChatManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `chat_management_student` = @ChatManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@ChatManagementStudent", platformSetting.ChatManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.TeamsManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teams_management_student` = @TeamsManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teams_management_student` = @TeamsManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@TeamsManagementStudent", platformSetting.TeamsManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MicrosoftSSOManagement))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `microsoft_sso_management` = @MicrosoftSSOManagement ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `microsoft_sso_management` = @MicrosoftSSOManagement ");
                            }
                            cmd.Parameters.AddWithValue("@MicrosoftSSOManagement", platformSetting.MicrosoftSSOManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InternalLoginManagement))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `internal_login_management` = @InternalLoginManagement ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `internal_login_management` = @InternalLoginManagement ");
                            }
                            cmd.Parameters.AddWithValue("@InternalLoginManagement", platformSetting.InternalLoginManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InternalPasswordReset))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `internal_password_reset` = @InternalPasswordReset ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `internal_password_reset` = @InternalPasswordReset ");
                            }
                            cmd.Parameters.AddWithValue("@InternalPasswordReset", platformSetting.InternalPasswordReset);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @ModifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @ModifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@ModifiedBy", platformSetting.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @CreatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @CreatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@CreatedAt", platformSetting.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @UpdatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @UpdatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@UpdatedAt", platformSetting.UpdatedAt);
                        }
                    }
                    if (platformSettingWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @IdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @IdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@IdWhereLike", "%" + platformSettingWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.SchoolManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `school_management_admin` LIKE @SchoolManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `school_management_admin` LIKE @SchoolManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminWhereLike", "%" + platformSettingWhereLike.SchoolManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.EnrollmentManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin` LIKE @EnrollmentManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin` LIKE @EnrollmentManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminWhereLike", "%" + platformSettingWhereLike.EnrollmentManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_admin` LIKE @UserManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_admin` LIKE @UserManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdminWhereLike", "%" + platformSettingWhereLike.UserManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_admin` LIKE @MessageManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_admin` LIKE @MessageManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdminWhereLike", "%" + platformSettingWhereLike.MessageManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin` LIKE @MessageManagementSendAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin` LIKE @MessageManagementSendAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminWhereLike", "%" + platformSettingWhereLike.MessageManagementSendAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin` LIKE @ConfigurationManagerAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin` LIKE @ConfigurationManagerAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_admin` LIKE @PasswordManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_admin` LIKE @PasswordManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminWhereLike", "%" + platformSettingWhereLike.PasswordManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.SchoolManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `school_management_admin_ore` LIKE @SchoolManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `school_management_admin_ore` LIKE @SchoolManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminOreWhereLike", "%" + platformSettingWhereLike.SchoolManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.EnrollmentManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin_ore` LIKE @EnrollmentManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin_ore` LIKE @EnrollmentManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOreWhereLike", "%" + platformSettingWhereLike.EnrollmentManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_admin_ore` LIKE @UserManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_admin_ore` LIKE @UserManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdminOreWhereLike", "%" + platformSettingWhereLike.UserManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_admin_ore` LIKE @MessageManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_admin_ore` LIKE @MessageManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdminOreWhereLike", "%" + platformSettingWhereLike.MessageManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin_ore` LIKE @MessageManagementSendAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin_ore` LIKE @MessageManagementSendAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminOreWhereLike", "%" + platformSettingWhereLike.MessageManagementSendAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin_ore` LIKE @ConfigurationManagerAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin_ore` LIKE @ConfigurationManagerAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOreWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_admin_ore` LIKE @PasswordManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_admin_ore` LIKE @PasswordManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminOreWhereLike", "%" + platformSettingWhereLike.PasswordManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_principal` LIKE @UserManagementPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_principal` LIKE @UserManagementPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementPrincipalWhereLike", "%" + platformSettingWhereLike.UserManagementPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_principal` LIKE @MessageManagementPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_principal` LIKE @MessageManagementPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementPrincipalWhereLike", "%" + platformSettingWhereLike.MessageManagementPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_principal` LIKE @MessageManagementSendPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_principal` LIKE @MessageManagementSendPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendPrincipalWhereLike", "%" + platformSettingWhereLike.MessageManagementSendPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_principal` LIKE @ConfigurationManagerPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_principal` LIKE @ConfigurationManagerPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipalWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_principal` LIKE @PasswordManagementPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_principal` LIKE @PasswordManagementPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementPrincipalWhereLike", "%" + platformSettingWhereLike.PasswordManagementPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_teacher` LIKE @UserManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_teacher` LIKE @UserManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementTeacherWhereLike", "%" + platformSettingWhereLike.UserManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.GroupInternalManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `group_internal_management_teacher` LIKE @GroupInternalManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `group_internal_management_teacher` LIKE @GroupInternalManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@GroupInternalManagementTeacherWhereLike", "%" + platformSettingWhereLike.GroupInternalManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_management_teacher` LIKE @InvitationManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_management_teacher` LIKE @InvitationManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementTeacherWhereLike", "%" + platformSettingWhereLike.InvitationManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationTeacherToParentManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_parent_management_teacher` LIKE @InvitationTeacherToParentManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_parent_management_teacher` LIKE @InvitationTeacherToParentManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacherWhereLike", "%" + platformSettingWhereLike.InvitationTeacherToParentManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ChatManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `chat_management_teacher` LIKE @ChatManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `chat_management_teacher` LIKE @ChatManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ChatManagementTeacherWhereLike", "%" + platformSettingWhereLike.ChatManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_teacher` LIKE @MessageManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_teacher` LIKE @MessageManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementTeacherWhereLike", "%" + platformSettingWhereLike.MessageManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_teacher` LIKE @MessageManagementSendTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_teacher` LIKE @MessageManagementSendTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendTeacherWhereLike", "%" + platformSettingWhereLike.MessageManagementSendTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.TeamMeetsManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `team_meets_management_teacher` LIKE @TeamMeetsManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `team_meets_management_teacher` LIKE @TeamMeetsManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacherWhereLike", "%" + platformSettingWhereLike.TeamMeetsManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_teacher` LIKE @ConfigurationManagerTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_teacher` LIKE @ConfigurationManagerTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerTeacherWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationTeacherToStudentManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_student_management_teacher` LIKE @InvitationTeacherToStudentManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_student_management_teacher` LIKE @InvitationTeacherToStudentManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacherWhereLike", "%" + platformSettingWhereLike.InvitationTeacherToStudentManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_teacher` LIKE @PasswordManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_teacher` LIKE @PasswordManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementTeacherWhereLike", "%" + platformSettingWhereLike.PasswordManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ProgressManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `progress_management_parent` LIKE @ProgressManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `progress_management_parent` LIKE @ProgressManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementParentWhereLike", "%" + platformSettingWhereLike.ProgressManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_management_parent` LIKE @InvitationManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_management_parent` LIKE @InvitationManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementParentWhereLike", "%" + platformSettingWhereLike.InvitationManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ChatManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `chat_management_parent` LIKE @ChatManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `chat_management_parent` LIKE @ChatManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ChatManagementParentWhereLike", "%" + platformSettingWhereLike.ChatManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_parent` LIKE @MessageManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_parent` LIKE @MessageManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementParentWhereLike", "%" + platformSettingWhereLike.MessageManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_parent` LIKE @MessageManagementSendParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_parent` LIKE @MessageManagementSendParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendParentWhereLike", "%" + platformSettingWhereLike.MessageManagementSendParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_parent` LIKE @ConfigurationManagerParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_parent` LIKE @ConfigurationManagerParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerParentWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_parent` LIKE @PasswordManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_parent` LIKE @PasswordManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementParentWhereLike", "%" + platformSettingWhereLike.PasswordManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_student` LIKE @MessageManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_student` LIKE @MessageManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementStudentWhereLike", "%" + platformSettingWhereLike.MessageManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_student` LIKE @MessageManagementSendStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_student` LIKE @MessageManagementSendStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendStudentWhereLike", "%" + platformSettingWhereLike.MessageManagementSendStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ProgressManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `progress_management_student` LIKE @ProgressManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `progress_management_student` LIKE @ProgressManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementStudentWhereLike", "%" + platformSettingWhereLike.ProgressManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ChatManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `chat_management_student` LIKE @ChatManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `chat_management_student` LIKE @ChatManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ChatManagementStudentWhereLike", "%" + platformSettingWhereLike.ChatManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.TeamsManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teams_management_student` LIKE @TeamsManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teams_management_student` LIKE @TeamsManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@TeamsManagementStudentWhereLike", "%" + platformSettingWhereLike.TeamsManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MicrosoftSSOManagement))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `microsoft_sso_management` LIKE @MicrosoftSSOManagementWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `microsoft_sso_management` LIKE @MicrosoftSSOManagementWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MicrosoftSSOManagementWhereLike", "%" + platformSettingWhereLike.MicrosoftSSOManagement + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InternalLoginManagement))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `internal_login_management` LIKE @InternalLoginManagementWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `internal_login_management` LIKE @InternalLoginManagementWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InternalLoginManagementWhereLike", "%" + platformSettingWhereLike.InternalLoginManagement + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InternalPasswordReset))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `internal_password_reset` LIKE @InternalPasswordResetWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `internal_password_reset` LIKE @InternalPasswordResetWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InternalPasswordResetWhereLike", "%" + platformSettingWhereLike.InternalPasswordReset + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @ModifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @ModifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ModifiedByWhereLike", "%" + platformSettingWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @CreatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @CreatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@CreatedAtWhereLike", "%" + platformSettingWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @UpdatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @UpdatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UpdatedAtWhereLike", "%" + platformSettingWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = String.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                platformSetting = new PlatformSettingModel();
                                if (!reader.IsDBNull(0))
                                    platformSetting.Id = reader.GetValue(0).ToString();
                                else
                                    platformSetting.Id = null;
                                if (!reader.IsDBNull(1))
                                    platformSetting.SchoolManagementAdmin = reader.GetValue(1).ToString();
                                else
                                    platformSetting.SchoolManagementAdmin = null;
                                if (!reader.IsDBNull(2))
                                    platformSetting.EnrollmentManagementAdmin = reader.GetValue(2).ToString();
                                else
                                    platformSetting.EnrollmentManagementAdmin = null;
                                if (!reader.IsDBNull(3))
                                    platformSetting.UserManagementAdmin = reader.GetValue(3).ToString();
                                else
                                    platformSetting.UserManagementAdmin = null;
                                if (!reader.IsDBNull(4))
                                    platformSetting.MessageManagementAdmin = reader.GetValue(4).ToString();
                                else
                                    platformSetting.MessageManagementAdmin = null;
                                if (!reader.IsDBNull(5))
                                    platformSetting.MessageManagementSendAdmin = reader.GetValue(5).ToString();
                                else
                                    platformSetting.MessageManagementSendAdmin = null;
                                if (!reader.IsDBNull(6))
                                    platformSetting.ConfigurationManagerAdmin = reader.GetValue(6).ToString();
                                else
                                    platformSetting.ConfigurationManagerAdmin = null;
                                if (!reader.IsDBNull(7))
                                    platformSetting.PasswordManagementAdmin = reader.GetValue(7).ToString();
                                else
                                    platformSetting.PasswordManagementAdmin = null;
                                if (!reader.IsDBNull(8))
                                    platformSetting.SchoolManagementAdminOre = reader.GetValue(8).ToString();
                                else
                                    platformSetting.SchoolManagementAdminOre = null;
                                if (!reader.IsDBNull(9))
                                    platformSetting.EnrollmentManagementAdminOre = reader.GetValue(9).ToString();
                                else
                                    platformSetting.EnrollmentManagementAdminOre = null;
                                if (!reader.IsDBNull(10))
                                    platformSetting.UserManagementAdminOre = reader.GetValue(10).ToString();
                                else
                                    platformSetting.UserManagementAdminOre = null;
                                if (!reader.IsDBNull(11))
                                    platformSetting.MessageManagementAdminOre = reader.GetValue(11).ToString();
                                else
                                    platformSetting.MessageManagementAdminOre = null;
                                if (!reader.IsDBNull(12))
                                    platformSetting.MessageManagementSendAdminOre = reader.GetValue(12).ToString();
                                else
                                    platformSetting.MessageManagementSendAdminOre = null;
                                if (!reader.IsDBNull(13))
                                    platformSetting.ConfigurationManagerAdminOre = reader.GetValue(13).ToString();
                                else
                                    platformSetting.ConfigurationManagerAdminOre = null;
                                if (!reader.IsDBNull(14))
                                    platformSetting.PasswordManagementAdminOre = reader.GetValue(14).ToString();
                                else
                                    platformSetting.PasswordManagementAdminOre = null;
                                if (!reader.IsDBNull(15))
                                    platformSetting.UserManagementPrincipal = reader.GetValue(15).ToString();
                                else
                                    platformSetting.UserManagementPrincipal = null;
                                if (!reader.IsDBNull(16))
                                    platformSetting.MessageManagementPrincipal = reader.GetValue(16).ToString();
                                else
                                    platformSetting.MessageManagementPrincipal = null;
                                if (!reader.IsDBNull(17))
                                    platformSetting.MessageManagementSendPrincipal = reader.GetValue(17).ToString();
                                else
                                    platformSetting.MessageManagementSendPrincipal = null;
                                if (!reader.IsDBNull(18))
                                    platformSetting.ConfigurationManagerPrincipal = reader.GetValue(18).ToString();
                                else
                                    platformSetting.ConfigurationManagerPrincipal = null;
                                if (!reader.IsDBNull(19))
                                    platformSetting.PasswordManagementPrincipal = reader.GetValue(19).ToString();
                                else
                                    platformSetting.PasswordManagementPrincipal = null;
                                if (!reader.IsDBNull(20))
                                    platformSetting.UserManagementTeacher = reader.GetValue(20).ToString();
                                else
                                    platformSetting.UserManagementTeacher = null;
                                if (!reader.IsDBNull(21))
                                    platformSetting.GroupInternalManagementTeacher = reader.GetValue(21).ToString();
                                else
                                    platformSetting.GroupInternalManagementTeacher = null;
                                if (!reader.IsDBNull(22))
                                    platformSetting.InvitationManagementTeacher = reader.GetValue(22).ToString();
                                else
                                    platformSetting.InvitationManagementTeacher = null;
                                if (!reader.IsDBNull(23))
                                    platformSetting.InvitationTeacherToParentManagementTeacher = reader.GetValue(23).ToString();
                                else
                                    platformSetting.InvitationTeacherToParentManagementTeacher = null;
                                if (!reader.IsDBNull(24))
                                    platformSetting.ChatManagementTeacher = reader.GetValue(24).ToString();
                                else
                                    platformSetting.ChatManagementTeacher = null;
                                if (!reader.IsDBNull(25))
                                    platformSetting.MessageManagementTeacher = reader.GetValue(25).ToString();
                                else
                                    platformSetting.MessageManagementTeacher = null;
                                if (!reader.IsDBNull(26))
                                    platformSetting.MessageManagementSendTeacher = reader.GetValue(26).ToString();
                                else
                                    platformSetting.MessageManagementSendTeacher = null;
                                if (!reader.IsDBNull(27))
                                    platformSetting.TeamMeetsManagementTeacher = reader.GetValue(27).ToString();
                                else
                                    platformSetting.TeamMeetsManagementTeacher = null;
                                if (!reader.IsDBNull(28))
                                    platformSetting.ConfigurationManagerTeacher = reader.GetValue(28).ToString();
                                else
                                    platformSetting.ConfigurationManagerTeacher = null;
                                if (!reader.IsDBNull(29))
                                    platformSetting.InvitationTeacherToStudentManagementTeacher = reader.GetValue(29).ToString();
                                else
                                    platformSetting.InvitationTeacherToStudentManagementTeacher = null;
                                if (!reader.IsDBNull(30))
                                    platformSetting.PasswordManagementTeacher = reader.GetValue(30).ToString();
                                else
                                    platformSetting.PasswordManagementTeacher = null;
                                if (!reader.IsDBNull(31))
                                    platformSetting.ProgressManagementParent = reader.GetValue(31).ToString();
                                else
                                    platformSetting.ProgressManagementParent = null;
                                if (!reader.IsDBNull(32))
                                    platformSetting.InvitationManagementParent = reader.GetValue(32).ToString();
                                else
                                    platformSetting.InvitationManagementParent = null;
                                if (!reader.IsDBNull(33))
                                    platformSetting.ChatManagementParent = reader.GetValue(33).ToString();
                                else
                                    platformSetting.ChatManagementParent = null;
                                if (!reader.IsDBNull(34))
                                    platformSetting.MessageManagementParent = reader.GetValue(34).ToString();
                                else
                                    platformSetting.MessageManagementParent = null;
                                if (!reader.IsDBNull(35))
                                    platformSetting.MessageManagementSendParent = reader.GetValue(35).ToString();
                                else
                                    platformSetting.MessageManagementSendParent = null;
                                if (!reader.IsDBNull(36))
                                    platformSetting.ConfigurationManagerParent = reader.GetValue(36).ToString();
                                else
                                    platformSetting.ConfigurationManagerParent = null;
                                if (!reader.IsDBNull(37))
                                    platformSetting.PasswordManagementParent = reader.GetValue(37).ToString();
                                else
                                    platformSetting.PasswordManagementParent = null;
                                if (!reader.IsDBNull(38))
                                    platformSetting.MessageManagementStudent = reader.GetValue(38).ToString();
                                else
                                    platformSetting.MessageManagementStudent = null;
                                if (!reader.IsDBNull(39))
                                    platformSetting.MessageManagementSendStudent = reader.GetValue(39).ToString();
                                else
                                    platformSetting.MessageManagementSendStudent = null;
                                if (!reader.IsDBNull(40))
                                    platformSetting.ProgressManagementStudent = reader.GetValue(40).ToString();
                                else
                                    platformSetting.ProgressManagementStudent = null;
                                if (!reader.IsDBNull(41))
                                    platformSetting.ChatManagementStudent = reader.GetValue(41).ToString();
                                else
                                    platformSetting.ChatManagementStudent = null;
                                if (!reader.IsDBNull(42))
                                    platformSetting.TeamsManagementStudent = reader.GetValue(42).ToString();
                                else
                                    platformSetting.TeamsManagementStudent = null;
                                if (!reader.IsDBNull(43))
                                    platformSetting.MicrosoftSSOManagement = reader.GetValue(43).ToString();
                                else
                                    platformSetting.MicrosoftSSOManagement = null;
                                if (!reader.IsDBNull(44))
                                    platformSetting.InternalLoginManagement = reader.GetValue(44).ToString();
                                else
                                    platformSetting.InternalLoginManagement = null;
                                if (!reader.IsDBNull(45))
                                    platformSetting.InternalPasswordReset = reader.GetValue(45).ToString();
                                else
                                    platformSetting.InternalPasswordReset = null;
                                if (!reader.IsDBNull(46))
                                    platformSetting.ModifiedBy = reader.GetValue(46).ToString();
                                else
                                    platformSetting.ModifiedBy = null;
                                if (!reader.IsDBNull(47))
                                    platformSetting.CreatedAt = reader.GetValue(47).ToString();
                                else
                                    platformSetting.CreatedAt = null;
                                if (!reader.IsDBNull(48))
                                    platformSetting.UpdatedAt = reader.GetValue(48).ToString();
                                else
                                    platformSetting.UpdatedAt = null;
                                response.Add(platformSetting.Id, platformSetting);
                                platformSetting = new PlatformSettingModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PlatformSettingModel>();
                        reader.Close();
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlatformSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlatformSettingModel> ReadD(string connectionString, PlatformSettingModel platformSetting, bool orderByLastDate, int fromRow, int limit, PlatformSettingModel platformSettingWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlatformSettingModel> response = new Dictionary<string, PlatformSettingModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `school_management_admin`, `enrollment_management_admin`, `user_management_admin`, `message_management_admin`, `message_management_send_admin`, `configuration_manager_admin`, `password_management_admin`, `school_management_admin_ore`, `enrollment_management_admin_ore`, `user_management_admin_ore`, `message_management_admin_ore`, `message_management_send_admin_ore`, `configuration_manager_admin_ore`, `password_management_admin_ore`, `user_management_principal`, `message_management_principal`, `message_management_send_principal`, `configuration_manager_principal`, `password_management_principal`, `user_management_teacher`, `group_internal_management_teacher`, `invitation_management_teacher`, `invitation_teacher_to_parent_management_teacher`, `chat_management_teacher`, `message_management_teacher`, `message_management_send_teacher`, `team_meets_management_teacher`, `configuration_manager_teacher`, `invitation_teacher_to_student_management_teacher`, `password_management_teacher`, `progress_management_parent`, `invitation_management_parent`, `chat_management_parent`, `message_management_parent`, `message_management_send_parent`, `configuration_manager_parent`, `password_management_parent`, `message_management_student`, `message_management_send_student`, `progress_management_student`, `chat_management_student`, `teams_management_student`, `microsoft_sso_management`, `internal_login_management`, `internal_password_reset`, `modified_by`, `created_at`, `updated_at`  FROM platform_settings ";
                {
                    if (platformSetting != null)
                    {
                        if (!String.IsNullOrEmpty(platformSetting.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @Id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @Id ");
                            }
                            cmd.Parameters.AddWithValue("@Id", platformSetting.Id);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.SchoolManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `school_management_admin` = @SchoolManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `school_management_admin` = @SchoolManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@SchoolManagementAdmin", platformSetting.SchoolManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.EnrollmentManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin` = @EnrollmentManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin` = @EnrollmentManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdmin", platformSetting.EnrollmentManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_admin` = @UserManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_admin` = @UserManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementAdmin", platformSetting.UserManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_admin` = @MessageManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_admin` = @MessageManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementAdmin", platformSetting.MessageManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin` = @MessageManagementSendAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin` = @MessageManagementSendAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdmin", platformSetting.MessageManagementSendAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin` = @ConfigurationManagerAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin` = @ConfigurationManagerAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdmin", platformSetting.ConfigurationManagerAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_admin` = @PasswordManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_admin` = @PasswordManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementAdmin", platformSetting.PasswordManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.SchoolManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `school_management_admin_ore` = @SchoolManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `school_management_admin_ore` = @SchoolManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminOre", platformSetting.SchoolManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.EnrollmentManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOre", platformSetting.EnrollmentManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_admin_ore` = @UserManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_admin_ore` = @UserManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementAdminOre", platformSetting.UserManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_admin_ore` = @MessageManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_admin_ore` = @MessageManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementAdminOre", platformSetting.MessageManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin_ore` = @MessageManagementSendAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin_ore` = @MessageManagementSendAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminOre", platformSetting.MessageManagementSendAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOre", platformSetting.ConfigurationManagerAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_admin_ore` = @PasswordManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_admin_ore` = @PasswordManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminOre", platformSetting.PasswordManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_principal` = @UserManagementPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_principal` = @UserManagementPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementPrincipal", platformSetting.UserManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_principal` = @MessageManagementPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_principal` = @MessageManagementPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementPrincipal", platformSetting.MessageManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_principal` = @MessageManagementSendPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_principal` = @MessageManagementSendPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendPrincipal", platformSetting.MessageManagementSendPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_principal` = @ConfigurationManagerPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_principal` = @ConfigurationManagerPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipal", platformSetting.ConfigurationManagerPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_principal` = @PasswordManagementPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_principal` = @PasswordManagementPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementPrincipal", platformSetting.PasswordManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_teacher` = @UserManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_teacher` = @UserManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementTeacher", platformSetting.UserManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.GroupInternalManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `group_internal_management_teacher` = @GroupInternalManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `group_internal_management_teacher` = @GroupInternalManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@GroupInternalManagementTeacher", platformSetting.GroupInternalManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_management_teacher` = @InvitationManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_management_teacher` = @InvitationManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationManagementTeacher", platformSetting.InvitationManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationTeacherToParentManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacher", platformSetting.InvitationTeacherToParentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ChatManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `chat_management_teacher` = @ChatManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `chat_management_teacher` = @ChatManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@ChatManagementTeacher", platformSetting.ChatManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_teacher` = @MessageManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_teacher` = @MessageManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementTeacher", platformSetting.MessageManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_teacher` = @MessageManagementSendTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_teacher` = @MessageManagementSendTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendTeacher", platformSetting.MessageManagementSendTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.TeamMeetsManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `team_meets_management_teacher` = @TeamMeetsManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `team_meets_management_teacher` = @TeamMeetsManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacher", platformSetting.TeamMeetsManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_teacher` = @ConfigurationManagerTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_teacher` = @ConfigurationManagerTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerTeacher", platformSetting.ConfigurationManagerTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationTeacherToStudentManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacher", platformSetting.InvitationTeacherToStudentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_teacher` = @PasswordManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_teacher` = @PasswordManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementTeacher", platformSetting.PasswordManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ProgressManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `progress_management_parent` = @ProgressManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `progress_management_parent` = @ProgressManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@ProgressManagementParent", platformSetting.ProgressManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_management_parent` = @InvitationManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_management_parent` = @InvitationManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationManagementParent", platformSetting.InvitationManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ChatManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `chat_management_parent` = @ChatManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `chat_management_parent` = @ChatManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@ChatManagementParent", platformSetting.ChatManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_parent` = @MessageManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_parent` = @MessageManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementParent", platformSetting.MessageManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_parent` = @MessageManagementSendParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_parent` = @MessageManagementSendParent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendParent", platformSetting.MessageManagementSendParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_parent` = @ConfigurationManagerParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_parent` = @ConfigurationManagerParent ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerParent", platformSetting.ConfigurationManagerParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_parent` = @PasswordManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_parent` = @PasswordManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementParent", platformSetting.PasswordManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_student` = @MessageManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_student` = @MessageManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementStudent", platformSetting.MessageManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_student` = @MessageManagementSendStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_student` = @MessageManagementSendStudent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendStudent", platformSetting.MessageManagementSendStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ProgressManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `progress_management_student` = @ProgressManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `progress_management_student` = @ProgressManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@ProgressManagementStudent", platformSetting.ProgressManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ChatManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `chat_management_student` = @ChatManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `chat_management_student` = @ChatManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@ChatManagementStudent", platformSetting.ChatManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.TeamsManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teams_management_student` = @TeamsManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teams_management_student` = @TeamsManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@TeamsManagementStudent", platformSetting.TeamsManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MicrosoftSSOManagement))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `microsoft_sso_management` = @MicrosoftSSOManagement ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `microsoft_sso_management` = @MicrosoftSSOManagement ");
                            }
                            cmd.Parameters.AddWithValue("@MicrosoftSSOManagement", platformSetting.MicrosoftSSOManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InternalLoginManagement))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `internal_login_management` = @InternalLoginManagement ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `internal_login_management` = @InternalLoginManagement ");
                            }
                            cmd.Parameters.AddWithValue("@InternalLoginManagement", platformSetting.InternalLoginManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InternalPasswordReset))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `internal_password_reset` = @InternalPasswordReset ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `internal_password_reset` = @InternalPasswordReset ");
                            }
                            cmd.Parameters.AddWithValue("@InternalPasswordReset", platformSetting.InternalPasswordReset);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @ModifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @ModifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@ModifiedBy", platformSetting.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @CreatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @CreatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@CreatedAt", platformSetting.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @UpdatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @UpdatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@UpdatedAt", platformSetting.UpdatedAt);
                        }
                    }
                    if (platformSettingWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @IdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @IdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@IdWhereLike", "%" + platformSettingWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.SchoolManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `school_management_admin` LIKE @SchoolManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `school_management_admin` LIKE @SchoolManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminWhereLike", "%" + platformSettingWhereLike.SchoolManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.EnrollmentManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin` LIKE @EnrollmentManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin` LIKE @EnrollmentManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminWhereLike", "%" + platformSettingWhereLike.EnrollmentManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_admin` LIKE @UserManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_admin` LIKE @UserManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdminWhereLike", "%" + platformSettingWhereLike.UserManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_admin` LIKE @MessageManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_admin` LIKE @MessageManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdminWhereLike", "%" + platformSettingWhereLike.MessageManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin` LIKE @MessageManagementSendAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin` LIKE @MessageManagementSendAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminWhereLike", "%" + platformSettingWhereLike.MessageManagementSendAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin` LIKE @ConfigurationManagerAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin` LIKE @ConfigurationManagerAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_admin` LIKE @PasswordManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_admin` LIKE @PasswordManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminWhereLike", "%" + platformSettingWhereLike.PasswordManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.SchoolManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `school_management_admin_ore` LIKE @SchoolManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `school_management_admin_ore` LIKE @SchoolManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminOreWhereLike", "%" + platformSettingWhereLike.SchoolManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.EnrollmentManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin_ore` LIKE @EnrollmentManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin_ore` LIKE @EnrollmentManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOreWhereLike", "%" + platformSettingWhereLike.EnrollmentManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_admin_ore` LIKE @UserManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_admin_ore` LIKE @UserManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdminOreWhereLike", "%" + platformSettingWhereLike.UserManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_admin_ore` LIKE @MessageManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_admin_ore` LIKE @MessageManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdminOreWhereLike", "%" + platformSettingWhereLike.MessageManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin_ore` LIKE @MessageManagementSendAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin_ore` LIKE @MessageManagementSendAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminOreWhereLike", "%" + platformSettingWhereLike.MessageManagementSendAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin_ore` LIKE @ConfigurationManagerAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin_ore` LIKE @ConfigurationManagerAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOreWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_admin_ore` LIKE @PasswordManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_admin_ore` LIKE @PasswordManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminOreWhereLike", "%" + platformSettingWhereLike.PasswordManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_principal` LIKE @UserManagementPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_principal` LIKE @UserManagementPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementPrincipalWhereLike", "%" + platformSettingWhereLike.UserManagementPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_principal` LIKE @MessageManagementPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_principal` LIKE @MessageManagementPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementPrincipalWhereLike", "%" + platformSettingWhereLike.MessageManagementPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_principal` LIKE @MessageManagementSendPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_principal` LIKE @MessageManagementSendPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendPrincipalWhereLike", "%" + platformSettingWhereLike.MessageManagementSendPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_principal` LIKE @ConfigurationManagerPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_principal` LIKE @ConfigurationManagerPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipalWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_principal` LIKE @PasswordManagementPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_principal` LIKE @PasswordManagementPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementPrincipalWhereLike", "%" + platformSettingWhereLike.PasswordManagementPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_teacher` LIKE @UserManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_teacher` LIKE @UserManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementTeacherWhereLike", "%" + platformSettingWhereLike.UserManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.GroupInternalManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `group_internal_management_teacher` LIKE @GroupInternalManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `group_internal_management_teacher` LIKE @GroupInternalManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@GroupInternalManagementTeacherWhereLike", "%" + platformSettingWhereLike.GroupInternalManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_management_teacher` LIKE @InvitationManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_management_teacher` LIKE @InvitationManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementTeacherWhereLike", "%" + platformSettingWhereLike.InvitationManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationTeacherToParentManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_parent_management_teacher` LIKE @InvitationTeacherToParentManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_parent_management_teacher` LIKE @InvitationTeacherToParentManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacherWhereLike", "%" + platformSettingWhereLike.InvitationTeacherToParentManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ChatManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `chat_management_teacher` LIKE @ChatManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `chat_management_teacher` LIKE @ChatManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ChatManagementTeacherWhereLike", "%" + platformSettingWhereLike.ChatManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_teacher` LIKE @MessageManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_teacher` LIKE @MessageManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementTeacherWhereLike", "%" + platformSettingWhereLike.MessageManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_teacher` LIKE @MessageManagementSendTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_teacher` LIKE @MessageManagementSendTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendTeacherWhereLike", "%" + platformSettingWhereLike.MessageManagementSendTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.TeamMeetsManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `team_meets_management_teacher` LIKE @TeamMeetsManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `team_meets_management_teacher` LIKE @TeamMeetsManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacherWhereLike", "%" + platformSettingWhereLike.TeamMeetsManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_teacher` LIKE @ConfigurationManagerTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_teacher` LIKE @ConfigurationManagerTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerTeacherWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationTeacherToStudentManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_student_management_teacher` LIKE @InvitationTeacherToStudentManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_student_management_teacher` LIKE @InvitationTeacherToStudentManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacherWhereLike", "%" + platformSettingWhereLike.InvitationTeacherToStudentManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_teacher` LIKE @PasswordManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_teacher` LIKE @PasswordManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementTeacherWhereLike", "%" + platformSettingWhereLike.PasswordManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ProgressManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `progress_management_parent` LIKE @ProgressManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `progress_management_parent` LIKE @ProgressManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementParentWhereLike", "%" + platformSettingWhereLike.ProgressManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_management_parent` LIKE @InvitationManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_management_parent` LIKE @InvitationManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementParentWhereLike", "%" + platformSettingWhereLike.InvitationManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ChatManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `chat_management_parent` LIKE @ChatManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `chat_management_parent` LIKE @ChatManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ChatManagementParentWhereLike", "%" + platformSettingWhereLike.ChatManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_parent` LIKE @MessageManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_parent` LIKE @MessageManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementParentWhereLike", "%" + platformSettingWhereLike.MessageManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_parent` LIKE @MessageManagementSendParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_parent` LIKE @MessageManagementSendParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendParentWhereLike", "%" + platformSettingWhereLike.MessageManagementSendParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_parent` LIKE @ConfigurationManagerParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_parent` LIKE @ConfigurationManagerParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerParentWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_parent` LIKE @PasswordManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_parent` LIKE @PasswordManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementParentWhereLike", "%" + platformSettingWhereLike.PasswordManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_student` LIKE @MessageManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_student` LIKE @MessageManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementStudentWhereLike", "%" + platformSettingWhereLike.MessageManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_student` LIKE @MessageManagementSendStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_student` LIKE @MessageManagementSendStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendStudentWhereLike", "%" + platformSettingWhereLike.MessageManagementSendStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ProgressManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `progress_management_student` LIKE @ProgressManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `progress_management_student` LIKE @ProgressManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementStudentWhereLike", "%" + platformSettingWhereLike.ProgressManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ChatManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `chat_management_student` LIKE @ChatManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `chat_management_student` LIKE @ChatManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ChatManagementStudentWhereLike", "%" + platformSettingWhereLike.ChatManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.TeamsManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teams_management_student` LIKE @TeamsManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teams_management_student` LIKE @TeamsManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@TeamsManagementStudentWhereLike", "%" + platformSettingWhereLike.TeamsManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MicrosoftSSOManagement))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `microsoft_sso_management` LIKE @MicrosoftSSOManagementWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `microsoft_sso_management` LIKE @MicrosoftSSOManagementWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MicrosoftSSOManagementWhereLike", "%" + platformSettingWhereLike.MicrosoftSSOManagement + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InternalLoginManagement))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `internal_login_management` LIKE @InternalLoginManagementWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `internal_login_management` LIKE @InternalLoginManagementWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InternalLoginManagementWhereLike", "%" + platformSettingWhereLike.InternalLoginManagement + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InternalPasswordReset))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `internal_password_reset` LIKE @InternalPasswordResetWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `internal_password_reset` LIKE @InternalPasswordResetWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InternalPasswordResetWhereLike", "%" + platformSettingWhereLike.InternalPasswordReset + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @ModifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @ModifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ModifiedByWhereLike", "%" + platformSettingWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @CreatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @CreatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@CreatedAtWhereLike", "%" + platformSettingWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @UpdatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @UpdatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UpdatedAtWhereLike", "%" + platformSettingWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    sql = String.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                platformSetting = new PlatformSettingModel();
                                if (!reader.IsDBNull(0))
                                    platformSetting.Id = reader.GetValue(0).ToString();
                                else
                                    platformSetting.Id = null;
                                if (!reader.IsDBNull(1))
                                    platformSetting.SchoolManagementAdmin = reader.GetValue(1).ToString();
                                else
                                    platformSetting.SchoolManagementAdmin = null;
                                if (!reader.IsDBNull(2))
                                    platformSetting.EnrollmentManagementAdmin = reader.GetValue(2).ToString();
                                else
                                    platformSetting.EnrollmentManagementAdmin = null;
                                if (!reader.IsDBNull(3))
                                    platformSetting.UserManagementAdmin = reader.GetValue(3).ToString();
                                else
                                    platformSetting.UserManagementAdmin = null;
                                if (!reader.IsDBNull(4))
                                    platformSetting.MessageManagementAdmin = reader.GetValue(4).ToString();
                                else
                                    platformSetting.MessageManagementAdmin = null;
                                if (!reader.IsDBNull(5))
                                    platformSetting.MessageManagementSendAdmin = reader.GetValue(5).ToString();
                                else
                                    platformSetting.MessageManagementSendAdmin = null;
                                if (!reader.IsDBNull(6))
                                    platformSetting.ConfigurationManagerAdmin = reader.GetValue(6).ToString();
                                else
                                    platformSetting.ConfigurationManagerAdmin = null;
                                if (!reader.IsDBNull(7))
                                    platformSetting.PasswordManagementAdmin = reader.GetValue(7).ToString();
                                else
                                    platformSetting.PasswordManagementAdmin = null;
                                if (!reader.IsDBNull(8))
                                    platformSetting.SchoolManagementAdminOre = reader.GetValue(8).ToString();
                                else
                                    platformSetting.SchoolManagementAdminOre = null;
                                if (!reader.IsDBNull(9))
                                    platformSetting.EnrollmentManagementAdminOre = reader.GetValue(9).ToString();
                                else
                                    platformSetting.EnrollmentManagementAdminOre = null;
                                if (!reader.IsDBNull(10))
                                    platformSetting.UserManagementAdminOre = reader.GetValue(10).ToString();
                                else
                                    platformSetting.UserManagementAdminOre = null;
                                if (!reader.IsDBNull(11))
                                    platformSetting.MessageManagementAdminOre = reader.GetValue(11).ToString();
                                else
                                    platformSetting.MessageManagementAdminOre = null;
                                if (!reader.IsDBNull(12))
                                    platformSetting.MessageManagementSendAdminOre = reader.GetValue(12).ToString();
                                else
                                    platformSetting.MessageManagementSendAdminOre = null;
                                if (!reader.IsDBNull(13))
                                    platformSetting.ConfigurationManagerAdminOre = reader.GetValue(13).ToString();
                                else
                                    platformSetting.ConfigurationManagerAdminOre = null;
                                if (!reader.IsDBNull(14))
                                    platformSetting.PasswordManagementAdminOre = reader.GetValue(14).ToString();
                                else
                                    platformSetting.PasswordManagementAdminOre = null;
                                if (!reader.IsDBNull(15))
                                    platformSetting.UserManagementPrincipal = reader.GetValue(15).ToString();
                                else
                                    platformSetting.UserManagementPrincipal = null;
                                if (!reader.IsDBNull(16))
                                    platformSetting.MessageManagementPrincipal = reader.GetValue(16).ToString();
                                else
                                    platformSetting.MessageManagementPrincipal = null;
                                if (!reader.IsDBNull(17))
                                    platformSetting.MessageManagementSendPrincipal = reader.GetValue(17).ToString();
                                else
                                    platformSetting.MessageManagementSendPrincipal = null;
                                if (!reader.IsDBNull(18))
                                    platformSetting.ConfigurationManagerPrincipal = reader.GetValue(18).ToString();
                                else
                                    platformSetting.ConfigurationManagerPrincipal = null;
                                if (!reader.IsDBNull(19))
                                    platformSetting.PasswordManagementPrincipal = reader.GetValue(19).ToString();
                                else
                                    platformSetting.PasswordManagementPrincipal = null;
                                if (!reader.IsDBNull(20))
                                    platformSetting.UserManagementTeacher = reader.GetValue(20).ToString();
                                else
                                    platformSetting.UserManagementTeacher = null;
                                if (!reader.IsDBNull(21))
                                    platformSetting.GroupInternalManagementTeacher = reader.GetValue(21).ToString();
                                else
                                    platformSetting.GroupInternalManagementTeacher = null;
                                if (!reader.IsDBNull(22))
                                    platformSetting.InvitationManagementTeacher = reader.GetValue(22).ToString();
                                else
                                    platformSetting.InvitationManagementTeacher = null;
                                if (!reader.IsDBNull(23))
                                    platformSetting.InvitationTeacherToParentManagementTeacher = reader.GetValue(23).ToString();
                                else
                                    platformSetting.InvitationTeacherToParentManagementTeacher = null;
                                if (!reader.IsDBNull(24))
                                    platformSetting.ChatManagementTeacher = reader.GetValue(24).ToString();
                                else
                                    platformSetting.ChatManagementTeacher = null;
                                if (!reader.IsDBNull(25))
                                    platformSetting.MessageManagementTeacher = reader.GetValue(25).ToString();
                                else
                                    platformSetting.MessageManagementTeacher = null;
                                if (!reader.IsDBNull(26))
                                    platformSetting.MessageManagementSendTeacher = reader.GetValue(26).ToString();
                                else
                                    platformSetting.MessageManagementSendTeacher = null;
                                if (!reader.IsDBNull(27))
                                    platformSetting.TeamMeetsManagementTeacher = reader.GetValue(27).ToString();
                                else
                                    platformSetting.TeamMeetsManagementTeacher = null;
                                if (!reader.IsDBNull(28))
                                    platformSetting.ConfigurationManagerTeacher = reader.GetValue(28).ToString();
                                else
                                    platformSetting.ConfigurationManagerTeacher = null;
                                if (!reader.IsDBNull(29))
                                    platformSetting.InvitationTeacherToStudentManagementTeacher = reader.GetValue(29).ToString();
                                else
                                    platformSetting.InvitationTeacherToStudentManagementTeacher = null;
                                if (!reader.IsDBNull(30))
                                    platformSetting.PasswordManagementTeacher = reader.GetValue(30).ToString();
                                else
                                    platformSetting.PasswordManagementTeacher = null;
                                if (!reader.IsDBNull(31))
                                    platformSetting.ProgressManagementParent = reader.GetValue(31).ToString();
                                else
                                    platformSetting.ProgressManagementParent = null;
                                if (!reader.IsDBNull(32))
                                    platformSetting.InvitationManagementParent = reader.GetValue(32).ToString();
                                else
                                    platformSetting.InvitationManagementParent = null;
                                if (!reader.IsDBNull(33))
                                    platformSetting.ChatManagementParent = reader.GetValue(33).ToString();
                                else
                                    platformSetting.ChatManagementParent = null;
                                if (!reader.IsDBNull(34))
                                    platformSetting.MessageManagementParent = reader.GetValue(34).ToString();
                                else
                                    platformSetting.MessageManagementParent = null;
                                if (!reader.IsDBNull(35))
                                    platformSetting.MessageManagementSendParent = reader.GetValue(35).ToString();
                                else
                                    platformSetting.MessageManagementSendParent = null;
                                if (!reader.IsDBNull(36))
                                    platformSetting.ConfigurationManagerParent = reader.GetValue(36).ToString();
                                else
                                    platformSetting.ConfigurationManagerParent = null;
                                if (!reader.IsDBNull(37))
                                    platformSetting.PasswordManagementParent = reader.GetValue(37).ToString();
                                else
                                    platformSetting.PasswordManagementParent = null;
                                if (!reader.IsDBNull(38))
                                    platformSetting.MessageManagementStudent = reader.GetValue(38).ToString();
                                else
                                    platformSetting.MessageManagementStudent = null;
                                if (!reader.IsDBNull(39))
                                    platformSetting.MessageManagementSendStudent = reader.GetValue(39).ToString();
                                else
                                    platformSetting.MessageManagementSendStudent = null;
                                if (!reader.IsDBNull(40))
                                    platformSetting.ProgressManagementStudent = reader.GetValue(40).ToString();
                                else
                                    platformSetting.ProgressManagementStudent = null;
                                if (!reader.IsDBNull(41))
                                    platformSetting.ChatManagementStudent = reader.GetValue(41).ToString();
                                else
                                    platformSetting.ChatManagementStudent = null;
                                if (!reader.IsDBNull(42))
                                    platformSetting.TeamsManagementStudent = reader.GetValue(42).ToString();
                                else
                                    platformSetting.TeamsManagementStudent = null;
                                if (!reader.IsDBNull(43))
                                    platformSetting.MicrosoftSSOManagement = reader.GetValue(43).ToString();
                                else
                                    platformSetting.MicrosoftSSOManagement = null;
                                if (!reader.IsDBNull(44))
                                    platformSetting.InternalLoginManagement = reader.GetValue(44).ToString();
                                else
                                    platformSetting.InternalLoginManagement = null;
                                if (!reader.IsDBNull(45))
                                    platformSetting.InternalPasswordReset = reader.GetValue(45).ToString();
                                else
                                    platformSetting.InternalPasswordReset = null;
                                if (!reader.IsDBNull(46))
                                    platformSetting.ModifiedBy = reader.GetValue(46).ToString();
                                else
                                    platformSetting.ModifiedBy = null;
                                if (!reader.IsDBNull(47))
                                    platformSetting.CreatedAt = reader.GetValue(47).ToString();
                                else
                                    platformSetting.CreatedAt = null;
                                if (!reader.IsDBNull(48))
                                    platformSetting.UpdatedAt = reader.GetValue(48).ToString();
                                else
                                    platformSetting.UpdatedAt = null;
                                response.Add(platformSetting.Id, platformSetting);
                                platformSetting = new PlatformSettingModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PlatformSettingModel>();
                        reader.Close();
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlatformSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlatformSettingModel> ReadD(string connectionString, List<PlatformSettingModel> platformSettingWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlatformSettingModel> response = new Dictionary<string, PlatformSettingModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `school_management_admin`, `enrollment_management_admin`, `user_management_admin`, `message_management_admin`, `message_management_send_admin`, `configuration_manager_admin`, `password_management_admin`, `school_management_admin_ore`, `enrollment_management_admin_ore`, `user_management_admin_ore`, `message_management_admin_ore`, `message_management_send_admin_ore`, `configuration_manager_admin_ore`, `password_management_admin_ore`, `user_management_principal`, `message_management_principal`, `message_management_send_principal`, `configuration_manager_principal`, `password_management_principal`, `user_management_teacher`, `group_internal_management_teacher`, `invitation_management_teacher`, `invitation_teacher_to_parent_management_teacher`, `chat_management_teacher`, `message_management_teacher`, `message_management_send_teacher`, `team_meets_management_teacher`, `configuration_manager_teacher`, `invitation_teacher_to_student_management_teacher`, `password_management_teacher`, `progress_management_parent`, `invitation_management_parent`, `chat_management_parent`, `message_management_parent`, `message_management_send_parent`, `configuration_manager_parent`, `password_management_parent`, `message_management_student`, `message_management_send_student`, `progress_management_student`, `chat_management_student`, `teams_management_student`, `microsoft_sso_management`, `internal_login_management`, `internal_password_reset`, `modified_by`, `created_at`, `updated_at`  FROM platform_settings ";
                if (platformSettingWhere != null && platformSettingWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < platformSettingWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @Id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@Id" + i, platformSettingWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].SchoolManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `school_management_admin` = @SchoolManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdmin" + i, platformSettingWhere[i].SchoolManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].EnrollmentManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enrollment_management_admin` = @EnrollmentManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdmin" + i, platformSettingWhere[i].EnrollmentManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_admin` = @UserManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdmin" + i, platformSettingWhere[i].UserManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_admin` = @MessageManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdmin" + i, platformSettingWhere[i].MessageManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_admin` = @MessageManagementSendAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdmin" + i, platformSettingWhere[i].MessageManagementSendAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_admin` = @ConfigurationManagerAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdmin" + i, platformSettingWhere[i].ConfigurationManagerAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_admin` = @PasswordManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdmin" + i, platformSettingWhere[i].PasswordManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].SchoolManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `school_management_admin_ore` = @SchoolManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminOre" + i, platformSettingWhere[i].SchoolManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].EnrollmentManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOre" + i, platformSettingWhere[i].EnrollmentManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_admin_ore` = @UserManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdminOre" + i, platformSettingWhere[i].UserManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_admin_ore` = @MessageManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdminOre" + i, platformSettingWhere[i].MessageManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_admin_ore` = @MessageManagementSendAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminOre" + i, platformSettingWhere[i].MessageManagementSendAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOre" + i, platformSettingWhere[i].ConfigurationManagerAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_admin_ore` = @PasswordManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminOre" + i, platformSettingWhere[i].PasswordManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_principal` = @UserManagementPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementPrincipal" + i, platformSettingWhere[i].UserManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_principal` = @MessageManagementPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementPrincipal" + i, platformSettingWhere[i].MessageManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_principal` = @MessageManagementSendPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendPrincipal" + i, platformSettingWhere[i].MessageManagementSendPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_principal` = @ConfigurationManagerPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipal" + i, platformSettingWhere[i].ConfigurationManagerPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_principal` = @PasswordManagementPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementPrincipal" + i, platformSettingWhere[i].PasswordManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_teacher` = @UserManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementTeacher" + i, platformSettingWhere[i].UserManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].GroupInternalManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `group_internal_management_teacher` = @GroupInternalManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@GroupInternalManagementTeacher" + i, platformSettingWhere[i].GroupInternalManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_management_teacher` = @InvitationManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementTeacher" + i, platformSettingWhere[i].InvitationManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationTeacherToParentManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacher" + i, platformSettingWhere[i].InvitationTeacherToParentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ChatManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `chat_management_teacher` = @ChatManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ChatManagementTeacher" + i, platformSettingWhere[i].ChatManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_teacher` = @MessageManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementTeacher" + i, platformSettingWhere[i].MessageManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_teacher` = @MessageManagementSendTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendTeacher" + i, platformSettingWhere[i].MessageManagementSendTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].TeamMeetsManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `team_meets_management_teacher` = @TeamMeetsManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacher" + i, platformSettingWhere[i].TeamMeetsManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_teacher` = @ConfigurationManagerTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerTeacher" + i, platformSettingWhere[i].ConfigurationManagerTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationTeacherToStudentManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacher" + i, platformSettingWhere[i].InvitationTeacherToStudentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_teacher` = @PasswordManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementTeacher" + i, platformSettingWhere[i].PasswordManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ProgressManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `progress_management_parent` = @ProgressManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementParent" + i, platformSettingWhere[i].ProgressManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_management_parent` = @InvitationManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementParent" + i, platformSettingWhere[i].InvitationManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ChatManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `chat_management_parent` = @ChatManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ChatManagementParent" + i, platformSettingWhere[i].ChatManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_parent` = @MessageManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementParent" + i, platformSettingWhere[i].MessageManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_parent` = @MessageManagementSendParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendParent" + i, platformSettingWhere[i].MessageManagementSendParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_parent` = @ConfigurationManagerParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerParent" + i, platformSettingWhere[i].ConfigurationManagerParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_parent` = @PasswordManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementParent" + i, platformSettingWhere[i].PasswordManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_student` = @MessageManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementStudent" + i, platformSettingWhere[i].MessageManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_student` = @MessageManagementSendStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendStudent" + i, platformSettingWhere[i].MessageManagementSendStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ProgressManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `progress_management_student` = @ProgressManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementStudent" + i, platformSettingWhere[i].ProgressManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ChatManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `chat_management_student` = @ChatManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ChatManagementStudent" + i, platformSettingWhere[i].ChatManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].TeamsManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teams_management_student` = @TeamsManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@TeamsManagementStudent" + i, platformSettingWhere[i].TeamsManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MicrosoftSSOManagement))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `microsoft_sso_management` = @MicrosoftSSOManagement" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MicrosoftSSOManagement" + i, platformSettingWhere[i].MicrosoftSSOManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InternalLoginManagement))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `internal_login_management` = @InternalLoginManagement" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InternalLoginManagement" + i, platformSettingWhere[i].InternalLoginManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InternalPasswordReset))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `internal_password_reset` = @InternalPasswordReset" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InternalPasswordReset" + i, platformSettingWhere[i].InternalPasswordReset);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @ModifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ModifiedBy" + i, platformSettingWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @CreatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@CreatedAt" + i, platformSettingWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @UpdatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UpdatedAt" + i, platformSettingWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != platformSettingWhere.Count - 1)
                            sqlIn += " OR ";
                    }
                }
                cmd.CommandText = sql + " " + sqlIn;
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            PlatformSettingModel platformSetting = new PlatformSettingModel(); if (!reader.IsDBNull(0))
                                platformSetting.Id = reader.GetValue(0).ToString();
                            else
                                platformSetting.Id = null;
                            if (!reader.IsDBNull(1))
                                platformSetting.SchoolManagementAdmin = reader.GetValue(1).ToString();
                            else
                                platformSetting.SchoolManagementAdmin = null;
                            if (!reader.IsDBNull(2))
                                platformSetting.EnrollmentManagementAdmin = reader.GetValue(2).ToString();
                            else
                                platformSetting.EnrollmentManagementAdmin = null;
                            if (!reader.IsDBNull(3))
                                platformSetting.UserManagementAdmin = reader.GetValue(3).ToString();
                            else
                                platformSetting.UserManagementAdmin = null;
                            if (!reader.IsDBNull(4))
                                platformSetting.MessageManagementAdmin = reader.GetValue(4).ToString();
                            else
                                platformSetting.MessageManagementAdmin = null;
                            if (!reader.IsDBNull(5))
                                platformSetting.MessageManagementSendAdmin = reader.GetValue(5).ToString();
                            else
                                platformSetting.MessageManagementSendAdmin = null;
                            if (!reader.IsDBNull(6))
                                platformSetting.ConfigurationManagerAdmin = reader.GetValue(6).ToString();
                            else
                                platformSetting.ConfigurationManagerAdmin = null;
                            if (!reader.IsDBNull(7))
                                platformSetting.PasswordManagementAdmin = reader.GetValue(7).ToString();
                            else
                                platformSetting.PasswordManagementAdmin = null;
                            if (!reader.IsDBNull(8))
                                platformSetting.SchoolManagementAdminOre = reader.GetValue(8).ToString();
                            else
                                platformSetting.SchoolManagementAdminOre = null;
                            if (!reader.IsDBNull(9))
                                platformSetting.EnrollmentManagementAdminOre = reader.GetValue(9).ToString();
                            else
                                platformSetting.EnrollmentManagementAdminOre = null;
                            if (!reader.IsDBNull(10))
                                platformSetting.UserManagementAdminOre = reader.GetValue(10).ToString();
                            else
                                platformSetting.UserManagementAdminOre = null;
                            if (!reader.IsDBNull(11))
                                platformSetting.MessageManagementAdminOre = reader.GetValue(11).ToString();
                            else
                                platformSetting.MessageManagementAdminOre = null;
                            if (!reader.IsDBNull(12))
                                platformSetting.MessageManagementSendAdminOre = reader.GetValue(12).ToString();
                            else
                                platformSetting.MessageManagementSendAdminOre = null;
                            if (!reader.IsDBNull(13))
                                platformSetting.ConfigurationManagerAdminOre = reader.GetValue(13).ToString();
                            else
                                platformSetting.ConfigurationManagerAdminOre = null;
                            if (!reader.IsDBNull(14))
                                platformSetting.PasswordManagementAdminOre = reader.GetValue(14).ToString();
                            else
                                platformSetting.PasswordManagementAdminOre = null;
                            if (!reader.IsDBNull(15))
                                platformSetting.UserManagementPrincipal = reader.GetValue(15).ToString();
                            else
                                platformSetting.UserManagementPrincipal = null;
                            if (!reader.IsDBNull(16))
                                platformSetting.MessageManagementPrincipal = reader.GetValue(16).ToString();
                            else
                                platformSetting.MessageManagementPrincipal = null;
                            if (!reader.IsDBNull(17))
                                platformSetting.MessageManagementSendPrincipal = reader.GetValue(17).ToString();
                            else
                                platformSetting.MessageManagementSendPrincipal = null;
                            if (!reader.IsDBNull(18))
                                platformSetting.ConfigurationManagerPrincipal = reader.GetValue(18).ToString();
                            else
                                platformSetting.ConfigurationManagerPrincipal = null;
                            if (!reader.IsDBNull(19))
                                platformSetting.PasswordManagementPrincipal = reader.GetValue(19).ToString();
                            else
                                platformSetting.PasswordManagementPrincipal = null;
                            if (!reader.IsDBNull(20))
                                platformSetting.UserManagementTeacher = reader.GetValue(20).ToString();
                            else
                                platformSetting.UserManagementTeacher = null;
                            if (!reader.IsDBNull(21))
                                platformSetting.GroupInternalManagementTeacher = reader.GetValue(21).ToString();
                            else
                                platformSetting.GroupInternalManagementTeacher = null;
                            if (!reader.IsDBNull(22))
                                platformSetting.InvitationManagementTeacher = reader.GetValue(22).ToString();
                            else
                                platformSetting.InvitationManagementTeacher = null;
                            if (!reader.IsDBNull(23))
                                platformSetting.InvitationTeacherToParentManagementTeacher = reader.GetValue(23).ToString();
                            else
                                platformSetting.InvitationTeacherToParentManagementTeacher = null;
                            if (!reader.IsDBNull(24))
                                platformSetting.ChatManagementTeacher = reader.GetValue(24).ToString();
                            else
                                platformSetting.ChatManagementTeacher = null;
                            if (!reader.IsDBNull(25))
                                platformSetting.MessageManagementTeacher = reader.GetValue(25).ToString();
                            else
                                platformSetting.MessageManagementTeacher = null;
                            if (!reader.IsDBNull(26))
                                platformSetting.MessageManagementSendTeacher = reader.GetValue(26).ToString();
                            else
                                platformSetting.MessageManagementSendTeacher = null;
                            if (!reader.IsDBNull(27))
                                platformSetting.TeamMeetsManagementTeacher = reader.GetValue(27).ToString();
                            else
                                platformSetting.TeamMeetsManagementTeacher = null;
                            if (!reader.IsDBNull(28))
                                platformSetting.ConfigurationManagerTeacher = reader.GetValue(28).ToString();
                            else
                                platformSetting.ConfigurationManagerTeacher = null;
                            if (!reader.IsDBNull(29))
                                platformSetting.InvitationTeacherToStudentManagementTeacher = reader.GetValue(29).ToString();
                            else
                                platformSetting.InvitationTeacherToStudentManagementTeacher = null;
                            if (!reader.IsDBNull(30))
                                platformSetting.PasswordManagementTeacher = reader.GetValue(30).ToString();
                            else
                                platformSetting.PasswordManagementTeacher = null;
                            if (!reader.IsDBNull(31))
                                platformSetting.ProgressManagementParent = reader.GetValue(31).ToString();
                            else
                                platformSetting.ProgressManagementParent = null;
                            if (!reader.IsDBNull(32))
                                platformSetting.InvitationManagementParent = reader.GetValue(32).ToString();
                            else
                                platformSetting.InvitationManagementParent = null;
                            if (!reader.IsDBNull(33))
                                platformSetting.ChatManagementParent = reader.GetValue(33).ToString();
                            else
                                platformSetting.ChatManagementParent = null;
                            if (!reader.IsDBNull(34))
                                platformSetting.MessageManagementParent = reader.GetValue(34).ToString();
                            else
                                platformSetting.MessageManagementParent = null;
                            if (!reader.IsDBNull(35))
                                platformSetting.MessageManagementSendParent = reader.GetValue(35).ToString();
                            else
                                platformSetting.MessageManagementSendParent = null;
                            if (!reader.IsDBNull(36))
                                platformSetting.ConfigurationManagerParent = reader.GetValue(36).ToString();
                            else
                                platformSetting.ConfigurationManagerParent = null;
                            if (!reader.IsDBNull(37))
                                platformSetting.PasswordManagementParent = reader.GetValue(37).ToString();
                            else
                                platformSetting.PasswordManagementParent = null;
                            if (!reader.IsDBNull(38))
                                platformSetting.MessageManagementStudent = reader.GetValue(38).ToString();
                            else
                                platformSetting.MessageManagementStudent = null;
                            if (!reader.IsDBNull(39))
                                platformSetting.MessageManagementSendStudent = reader.GetValue(39).ToString();
                            else
                                platformSetting.MessageManagementSendStudent = null;
                            if (!reader.IsDBNull(40))
                                platformSetting.ProgressManagementStudent = reader.GetValue(40).ToString();
                            else
                                platformSetting.ProgressManagementStudent = null;
                            if (!reader.IsDBNull(41))
                                platformSetting.ChatManagementStudent = reader.GetValue(41).ToString();
                            else
                                platformSetting.ChatManagementStudent = null;
                            if (!reader.IsDBNull(42))
                                platformSetting.TeamsManagementStudent = reader.GetValue(42).ToString();
                            else
                                platformSetting.TeamsManagementStudent = null;
                            if (!reader.IsDBNull(43))
                                platformSetting.MicrosoftSSOManagement = reader.GetValue(43).ToString();
                            else
                                platformSetting.MicrosoftSSOManagement = null;
                            if (!reader.IsDBNull(44))
                                platformSetting.InternalLoginManagement = reader.GetValue(44).ToString();
                            else
                                platformSetting.InternalLoginManagement = null;
                            if (!reader.IsDBNull(45))
                                platformSetting.InternalPasswordReset = reader.GetValue(45).ToString();
                            else
                                platformSetting.InternalPasswordReset = null;
                            if (!reader.IsDBNull(46))
                                platformSetting.ModifiedBy = reader.GetValue(46).ToString();
                            else
                                platformSetting.ModifiedBy = null;
                            if (!reader.IsDBNull(47))
                                platformSetting.CreatedAt = reader.GetValue(47).ToString();
                            else
                                platformSetting.CreatedAt = null;
                            if (!reader.IsDBNull(48))
                                platformSetting.UpdatedAt = reader.GetValue(48).ToString();
                            else
                                platformSetting.UpdatedAt = null;
                            response.Add(platformSetting.Id, platformSetting);
                            platformSetting = new PlatformSettingModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PlatformSettingModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlatformSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlatformSettingModel> ReadD(string connectionString, List<PlatformSettingModel> platformSettingWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlatformSettingModel> response = new Dictionary<string, PlatformSettingModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `school_management_admin`, `enrollment_management_admin`, `user_management_admin`, `message_management_admin`, `message_management_send_admin`, `configuration_manager_admin`, `password_management_admin`, `school_management_admin_ore`, `enrollment_management_admin_ore`, `user_management_admin_ore`, `message_management_admin_ore`, `message_management_send_admin_ore`, `configuration_manager_admin_ore`, `password_management_admin_ore`, `user_management_principal`, `message_management_principal`, `message_management_send_principal`, `configuration_manager_principal`, `password_management_principal`, `user_management_teacher`, `group_internal_management_teacher`, `invitation_management_teacher`, `invitation_teacher_to_parent_management_teacher`, `chat_management_teacher`, `message_management_teacher`, `message_management_send_teacher`, `team_meets_management_teacher`, `configuration_manager_teacher`, `invitation_teacher_to_student_management_teacher`, `password_management_teacher`, `progress_management_parent`, `invitation_management_parent`, `chat_management_parent`, `message_management_parent`, `message_management_send_parent`, `configuration_manager_parent`, `password_management_parent`, `message_management_student`, `message_management_send_student`, `progress_management_student`, `chat_management_student`, `teams_management_student`, `microsoft_sso_management`, `internal_login_management`, `internal_password_reset`, `modified_by`, `created_at`, `updated_at`  FROM platform_settings ";
                if (platformSettingWhere != null && platformSettingWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < platformSettingWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @Id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@Id" + i, platformSettingWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].SchoolManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `school_management_admin` = @SchoolManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdmin" + i, platformSettingWhere[i].SchoolManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].EnrollmentManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enrollment_management_admin` = @EnrollmentManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdmin" + i, platformSettingWhere[i].EnrollmentManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_admin` = @UserManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdmin" + i, platformSettingWhere[i].UserManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_admin` = @MessageManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdmin" + i, platformSettingWhere[i].MessageManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_admin` = @MessageManagementSendAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdmin" + i, platformSettingWhere[i].MessageManagementSendAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_admin` = @ConfigurationManagerAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdmin" + i, platformSettingWhere[i].ConfigurationManagerAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_admin` = @PasswordManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdmin" + i, platformSettingWhere[i].PasswordManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].SchoolManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `school_management_admin_ore` = @SchoolManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminOre" + i, platformSettingWhere[i].SchoolManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].EnrollmentManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOre" + i, platformSettingWhere[i].EnrollmentManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_admin_ore` = @UserManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdminOre" + i, platformSettingWhere[i].UserManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_admin_ore` = @MessageManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdminOre" + i, platformSettingWhere[i].MessageManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_admin_ore` = @MessageManagementSendAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminOre" + i, platformSettingWhere[i].MessageManagementSendAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOre" + i, platformSettingWhere[i].ConfigurationManagerAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_admin_ore` = @PasswordManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminOre" + i, platformSettingWhere[i].PasswordManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_principal` = @UserManagementPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementPrincipal" + i, platformSettingWhere[i].UserManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_principal` = @MessageManagementPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementPrincipal" + i, platformSettingWhere[i].MessageManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_principal` = @MessageManagementSendPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendPrincipal" + i, platformSettingWhere[i].MessageManagementSendPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_principal` = @ConfigurationManagerPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipal" + i, platformSettingWhere[i].ConfigurationManagerPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_principal` = @PasswordManagementPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementPrincipal" + i, platformSettingWhere[i].PasswordManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_teacher` = @UserManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementTeacher" + i, platformSettingWhere[i].UserManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].GroupInternalManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `group_internal_management_teacher` = @GroupInternalManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@GroupInternalManagementTeacher" + i, platformSettingWhere[i].GroupInternalManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_management_teacher` = @InvitationManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementTeacher" + i, platformSettingWhere[i].InvitationManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationTeacherToParentManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacher" + i, platformSettingWhere[i].InvitationTeacherToParentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ChatManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `chat_management_teacher` = @ChatManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ChatManagementTeacher" + i, platformSettingWhere[i].ChatManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_teacher` = @MessageManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementTeacher" + i, platformSettingWhere[i].MessageManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_teacher` = @MessageManagementSendTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendTeacher" + i, platformSettingWhere[i].MessageManagementSendTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].TeamMeetsManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `team_meets_management_teacher` = @TeamMeetsManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacher" + i, platformSettingWhere[i].TeamMeetsManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_teacher` = @ConfigurationManagerTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerTeacher" + i, platformSettingWhere[i].ConfigurationManagerTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationTeacherToStudentManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacher" + i, platformSettingWhere[i].InvitationTeacherToStudentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_teacher` = @PasswordManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementTeacher" + i, platformSettingWhere[i].PasswordManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ProgressManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `progress_management_parent` = @ProgressManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementParent" + i, platformSettingWhere[i].ProgressManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_management_parent` = @InvitationManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementParent" + i, platformSettingWhere[i].InvitationManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ChatManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `chat_management_parent` = @ChatManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ChatManagementParent" + i, platformSettingWhere[i].ChatManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_parent` = @MessageManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementParent" + i, platformSettingWhere[i].MessageManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_parent` = @MessageManagementSendParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendParent" + i, platformSettingWhere[i].MessageManagementSendParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_parent` = @ConfigurationManagerParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerParent" + i, platformSettingWhere[i].ConfigurationManagerParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_parent` = @PasswordManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementParent" + i, platformSettingWhere[i].PasswordManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_student` = @MessageManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementStudent" + i, platformSettingWhere[i].MessageManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_student` = @MessageManagementSendStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendStudent" + i, platformSettingWhere[i].MessageManagementSendStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ProgressManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `progress_management_student` = @ProgressManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementStudent" + i, platformSettingWhere[i].ProgressManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ChatManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `chat_management_student` = @ChatManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ChatManagementStudent" + i, platformSettingWhere[i].ChatManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].TeamsManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teams_management_student` = @TeamsManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@TeamsManagementStudent" + i, platformSettingWhere[i].TeamsManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MicrosoftSSOManagement))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `microsoft_sso_management` = @MicrosoftSSOManagement" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MicrosoftSSOManagement" + i, platformSettingWhere[i].MicrosoftSSOManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InternalLoginManagement))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `internal_login_management` = @InternalLoginManagement" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InternalLoginManagement" + i, platformSettingWhere[i].InternalLoginManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InternalPasswordReset))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `internal_password_reset` = @InternalPasswordReset" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InternalPasswordReset" + i, platformSettingWhere[i].InternalPasswordReset);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @ModifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ModifiedBy" + i, platformSettingWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @CreatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@CreatedAt" + i, platformSettingWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @UpdatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UpdatedAt" + i, platformSettingWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != platformSettingWhere.Count - 1)
                            sqlIn += " OR ";
                    }
                }
                cmd.CommandText = sql + " " + sqlIn + " LIMIT @fromRow, @limit ";
                cmd.Parameters.AddWithValue("@fromRow", fromRow);
                cmd.Parameters.AddWithValue("@limit", limit);
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            PlatformSettingModel platformSetting = new PlatformSettingModel(); if (!reader.IsDBNull(0))
                                platformSetting.Id = reader.GetValue(0).ToString();
                            else
                                platformSetting.Id = null;
                            if (!reader.IsDBNull(1))
                                platformSetting.SchoolManagementAdmin = reader.GetValue(1).ToString();
                            else
                                platformSetting.SchoolManagementAdmin = null;
                            if (!reader.IsDBNull(2))
                                platformSetting.EnrollmentManagementAdmin = reader.GetValue(2).ToString();
                            else
                                platformSetting.EnrollmentManagementAdmin = null;
                            if (!reader.IsDBNull(3))
                                platformSetting.UserManagementAdmin = reader.GetValue(3).ToString();
                            else
                                platformSetting.UserManagementAdmin = null;
                            if (!reader.IsDBNull(4))
                                platformSetting.MessageManagementAdmin = reader.GetValue(4).ToString();
                            else
                                platformSetting.MessageManagementAdmin = null;
                            if (!reader.IsDBNull(5))
                                platformSetting.MessageManagementSendAdmin = reader.GetValue(5).ToString();
                            else
                                platformSetting.MessageManagementSendAdmin = null;
                            if (!reader.IsDBNull(6))
                                platformSetting.ConfigurationManagerAdmin = reader.GetValue(6).ToString();
                            else
                                platformSetting.ConfigurationManagerAdmin = null;
                            if (!reader.IsDBNull(7))
                                platformSetting.PasswordManagementAdmin = reader.GetValue(7).ToString();
                            else
                                platformSetting.PasswordManagementAdmin = null;
                            if (!reader.IsDBNull(8))
                                platformSetting.SchoolManagementAdminOre = reader.GetValue(8).ToString();
                            else
                                platformSetting.SchoolManagementAdminOre = null;
                            if (!reader.IsDBNull(9))
                                platformSetting.EnrollmentManagementAdminOre = reader.GetValue(9).ToString();
                            else
                                platformSetting.EnrollmentManagementAdminOre = null;
                            if (!reader.IsDBNull(10))
                                platformSetting.UserManagementAdminOre = reader.GetValue(10).ToString();
                            else
                                platformSetting.UserManagementAdminOre = null;
                            if (!reader.IsDBNull(11))
                                platformSetting.MessageManagementAdminOre = reader.GetValue(11).ToString();
                            else
                                platformSetting.MessageManagementAdminOre = null;
                            if (!reader.IsDBNull(12))
                                platformSetting.MessageManagementSendAdminOre = reader.GetValue(12).ToString();
                            else
                                platformSetting.MessageManagementSendAdminOre = null;
                            if (!reader.IsDBNull(13))
                                platformSetting.ConfigurationManagerAdminOre = reader.GetValue(13).ToString();
                            else
                                platformSetting.ConfigurationManagerAdminOre = null;
                            if (!reader.IsDBNull(14))
                                platformSetting.PasswordManagementAdminOre = reader.GetValue(14).ToString();
                            else
                                platformSetting.PasswordManagementAdminOre = null;
                            if (!reader.IsDBNull(15))
                                platformSetting.UserManagementPrincipal = reader.GetValue(15).ToString();
                            else
                                platformSetting.UserManagementPrincipal = null;
                            if (!reader.IsDBNull(16))
                                platformSetting.MessageManagementPrincipal = reader.GetValue(16).ToString();
                            else
                                platformSetting.MessageManagementPrincipal = null;
                            if (!reader.IsDBNull(17))
                                platformSetting.MessageManagementSendPrincipal = reader.GetValue(17).ToString();
                            else
                                platformSetting.MessageManagementSendPrincipal = null;
                            if (!reader.IsDBNull(18))
                                platformSetting.ConfigurationManagerPrincipal = reader.GetValue(18).ToString();
                            else
                                platformSetting.ConfigurationManagerPrincipal = null;
                            if (!reader.IsDBNull(19))
                                platformSetting.PasswordManagementPrincipal = reader.GetValue(19).ToString();
                            else
                                platformSetting.PasswordManagementPrincipal = null;
                            if (!reader.IsDBNull(20))
                                platformSetting.UserManagementTeacher = reader.GetValue(20).ToString();
                            else
                                platformSetting.UserManagementTeacher = null;
                            if (!reader.IsDBNull(21))
                                platformSetting.GroupInternalManagementTeacher = reader.GetValue(21).ToString();
                            else
                                platformSetting.GroupInternalManagementTeacher = null;
                            if (!reader.IsDBNull(22))
                                platformSetting.InvitationManagementTeacher = reader.GetValue(22).ToString();
                            else
                                platformSetting.InvitationManagementTeacher = null;
                            if (!reader.IsDBNull(23))
                                platformSetting.InvitationTeacherToParentManagementTeacher = reader.GetValue(23).ToString();
                            else
                                platformSetting.InvitationTeacherToParentManagementTeacher = null;
                            if (!reader.IsDBNull(24))
                                platformSetting.ChatManagementTeacher = reader.GetValue(24).ToString();
                            else
                                platformSetting.ChatManagementTeacher = null;
                            if (!reader.IsDBNull(25))
                                platformSetting.MessageManagementTeacher = reader.GetValue(25).ToString();
                            else
                                platformSetting.MessageManagementTeacher = null;
                            if (!reader.IsDBNull(26))
                                platformSetting.MessageManagementSendTeacher = reader.GetValue(26).ToString();
                            else
                                platformSetting.MessageManagementSendTeacher = null;
                            if (!reader.IsDBNull(27))
                                platformSetting.TeamMeetsManagementTeacher = reader.GetValue(27).ToString();
                            else
                                platformSetting.TeamMeetsManagementTeacher = null;
                            if (!reader.IsDBNull(28))
                                platformSetting.ConfigurationManagerTeacher = reader.GetValue(28).ToString();
                            else
                                platformSetting.ConfigurationManagerTeacher = null;
                            if (!reader.IsDBNull(29))
                                platformSetting.InvitationTeacherToStudentManagementTeacher = reader.GetValue(29).ToString();
                            else
                                platformSetting.InvitationTeacherToStudentManagementTeacher = null;
                            if (!reader.IsDBNull(30))
                                platformSetting.PasswordManagementTeacher = reader.GetValue(30).ToString();
                            else
                                platformSetting.PasswordManagementTeacher = null;
                            if (!reader.IsDBNull(31))
                                platformSetting.ProgressManagementParent = reader.GetValue(31).ToString();
                            else
                                platformSetting.ProgressManagementParent = null;
                            if (!reader.IsDBNull(32))
                                platformSetting.InvitationManagementParent = reader.GetValue(32).ToString();
                            else
                                platformSetting.InvitationManagementParent = null;
                            if (!reader.IsDBNull(33))
                                platformSetting.ChatManagementParent = reader.GetValue(33).ToString();
                            else
                                platformSetting.ChatManagementParent = null;
                            if (!reader.IsDBNull(34))
                                platformSetting.MessageManagementParent = reader.GetValue(34).ToString();
                            else
                                platformSetting.MessageManagementParent = null;
                            if (!reader.IsDBNull(35))
                                platformSetting.MessageManagementSendParent = reader.GetValue(35).ToString();
                            else
                                platformSetting.MessageManagementSendParent = null;
                            if (!reader.IsDBNull(36))
                                platformSetting.ConfigurationManagerParent = reader.GetValue(36).ToString();
                            else
                                platformSetting.ConfigurationManagerParent = null;
                            if (!reader.IsDBNull(37))
                                platformSetting.PasswordManagementParent = reader.GetValue(37).ToString();
                            else
                                platformSetting.PasswordManagementParent = null;
                            if (!reader.IsDBNull(38))
                                platformSetting.MessageManagementStudent = reader.GetValue(38).ToString();
                            else
                                platformSetting.MessageManagementStudent = null;
                            if (!reader.IsDBNull(39))
                                platformSetting.MessageManagementSendStudent = reader.GetValue(39).ToString();
                            else
                                platformSetting.MessageManagementSendStudent = null;
                            if (!reader.IsDBNull(40))
                                platformSetting.ProgressManagementStudent = reader.GetValue(40).ToString();
                            else
                                platformSetting.ProgressManagementStudent = null;
                            if (!reader.IsDBNull(41))
                                platformSetting.ChatManagementStudent = reader.GetValue(41).ToString();
                            else
                                platformSetting.ChatManagementStudent = null;
                            if (!reader.IsDBNull(42))
                                platformSetting.TeamsManagementStudent = reader.GetValue(42).ToString();
                            else
                                platformSetting.TeamsManagementStudent = null;
                            if (!reader.IsDBNull(43))
                                platformSetting.MicrosoftSSOManagement = reader.GetValue(43).ToString();
                            else
                                platformSetting.MicrosoftSSOManagement = null;
                            if (!reader.IsDBNull(44))
                                platformSetting.InternalLoginManagement = reader.GetValue(44).ToString();
                            else
                                platformSetting.InternalLoginManagement = null;
                            if (!reader.IsDBNull(45))
                                platformSetting.InternalPasswordReset = reader.GetValue(45).ToString();
                            else
                                platformSetting.InternalPasswordReset = null;
                            if (!reader.IsDBNull(46))
                                platformSetting.ModifiedBy = reader.GetValue(46).ToString();
                            else
                                platformSetting.ModifiedBy = null;
                            if (!reader.IsDBNull(47))
                                platformSetting.CreatedAt = reader.GetValue(47).ToString();
                            else
                                platformSetting.CreatedAt = null;
                            if (!reader.IsDBNull(48))
                                platformSetting.UpdatedAt = reader.GetValue(48).ToString();
                            else
                                platformSetting.UpdatedAt = null;
                            response.Add(platformSetting.Id, platformSetting);
                            platformSetting = new PlatformSettingModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PlatformSettingModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlatformSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlatformSettingModel> Read(string connectionString, PlatformSettingModel platformSetting, bool firstOne, bool orderByLastDate, PlatformSettingModel platformSettingWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformSettingModel> response = new List<PlatformSettingModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `school_management_admin`, `enrollment_management_admin`, `user_management_admin`, `message_management_admin`, `message_management_send_admin`, `configuration_manager_admin`, `password_management_admin`, `school_management_admin_ore`, `enrollment_management_admin_ore`, `user_management_admin_ore`, `message_management_admin_ore`, `message_management_send_admin_ore`, `configuration_manager_admin_ore`, `password_management_admin_ore`, `user_management_principal`, `message_management_principal`, `message_management_send_principal`, `configuration_manager_principal`, `password_management_principal`, `user_management_teacher`, `group_internal_management_teacher`, `invitation_management_teacher`, `invitation_teacher_to_parent_management_teacher`, `chat_management_teacher`, `message_management_teacher`, `message_management_send_teacher`, `team_meets_management_teacher`, `configuration_manager_teacher`, `invitation_teacher_to_student_management_teacher`, `password_management_teacher`, `progress_management_parent`, `invitation_management_parent`, `chat_management_parent`, `message_management_parent`, `message_management_send_parent`, `configuration_manager_parent`, `password_management_parent`, `message_management_student`, `message_management_send_student`, `progress_management_student`, `chat_management_student`, `teams_management_student`, `microsoft_sso_management`, `internal_login_management`, `internal_password_reset`, `modified_by`, `created_at`, `updated_at`  FROM platform_settings ";
                {
                    if (platformSetting != null)
                    {
                        if (!String.IsNullOrEmpty(platformSetting.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @Id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @Id ");
                            }
                            cmd.Parameters.AddWithValue("@Id", platformSetting.Id);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.SchoolManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `school_management_admin` = @SchoolManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `school_management_admin` = @SchoolManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@SchoolManagementAdmin", platformSetting.SchoolManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.EnrollmentManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin` = @EnrollmentManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin` = @EnrollmentManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdmin", platformSetting.EnrollmentManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_admin` = @UserManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_admin` = @UserManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementAdmin", platformSetting.UserManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_admin` = @MessageManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_admin` = @MessageManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementAdmin", platformSetting.MessageManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin` = @MessageManagementSendAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin` = @MessageManagementSendAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdmin", platformSetting.MessageManagementSendAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin` = @ConfigurationManagerAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin` = @ConfigurationManagerAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdmin", platformSetting.ConfigurationManagerAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_admin` = @PasswordManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_admin` = @PasswordManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementAdmin", platformSetting.PasswordManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.SchoolManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `school_management_admin_ore` = @SchoolManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `school_management_admin_ore` = @SchoolManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminOre", platformSetting.SchoolManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.EnrollmentManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOre", platformSetting.EnrollmentManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_admin_ore` = @UserManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_admin_ore` = @UserManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementAdminOre", platformSetting.UserManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_admin_ore` = @MessageManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_admin_ore` = @MessageManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementAdminOre", platformSetting.MessageManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin_ore` = @MessageManagementSendAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin_ore` = @MessageManagementSendAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminOre", platformSetting.MessageManagementSendAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOre", platformSetting.ConfigurationManagerAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_admin_ore` = @PasswordManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_admin_ore` = @PasswordManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminOre", platformSetting.PasswordManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_principal` = @UserManagementPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_principal` = @UserManagementPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementPrincipal", platformSetting.UserManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_principal` = @MessageManagementPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_principal` = @MessageManagementPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementPrincipal", platformSetting.MessageManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_principal` = @MessageManagementSendPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_principal` = @MessageManagementSendPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendPrincipal", platformSetting.MessageManagementSendPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_principal` = @ConfigurationManagerPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_principal` = @ConfigurationManagerPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipal", platformSetting.ConfigurationManagerPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_principal` = @PasswordManagementPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_principal` = @PasswordManagementPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementPrincipal", platformSetting.PasswordManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_teacher` = @UserManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_teacher` = @UserManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementTeacher", platformSetting.UserManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.GroupInternalManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `group_internal_management_teacher` = @GroupInternalManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `group_internal_management_teacher` = @GroupInternalManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@GroupInternalManagementTeacher", platformSetting.GroupInternalManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_management_teacher` = @InvitationManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_management_teacher` = @InvitationManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationManagementTeacher", platformSetting.InvitationManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationTeacherToParentManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacher", platformSetting.InvitationTeacherToParentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ChatManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `chat_management_teacher` = @ChatManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `chat_management_teacher` = @ChatManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@ChatManagementTeacher", platformSetting.ChatManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_teacher` = @MessageManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_teacher` = @MessageManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementTeacher", platformSetting.MessageManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_teacher` = @MessageManagementSendTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_teacher` = @MessageManagementSendTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendTeacher", platformSetting.MessageManagementSendTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.TeamMeetsManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `team_meets_management_teacher` = @TeamMeetsManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `team_meets_management_teacher` = @TeamMeetsManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacher", platformSetting.TeamMeetsManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_teacher` = @ConfigurationManagerTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_teacher` = @ConfigurationManagerTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerTeacher", platformSetting.ConfigurationManagerTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationTeacherToStudentManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacher", platformSetting.InvitationTeacherToStudentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_teacher` = @PasswordManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_teacher` = @PasswordManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementTeacher", platformSetting.PasswordManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ProgressManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `progress_management_parent` = @ProgressManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `progress_management_parent` = @ProgressManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@ProgressManagementParent", platformSetting.ProgressManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_management_parent` = @InvitationManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_management_parent` = @InvitationManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationManagementParent", platformSetting.InvitationManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ChatManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `chat_management_parent` = @ChatManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `chat_management_parent` = @ChatManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@ChatManagementParent", platformSetting.ChatManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_parent` = @MessageManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_parent` = @MessageManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementParent", platformSetting.MessageManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_parent` = @MessageManagementSendParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_parent` = @MessageManagementSendParent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendParent", platformSetting.MessageManagementSendParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_parent` = @ConfigurationManagerParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_parent` = @ConfigurationManagerParent ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerParent", platformSetting.ConfigurationManagerParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_parent` = @PasswordManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_parent` = @PasswordManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementParent", platformSetting.PasswordManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_student` = @MessageManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_student` = @MessageManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementStudent", platformSetting.MessageManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_student` = @MessageManagementSendStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_student` = @MessageManagementSendStudent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendStudent", platformSetting.MessageManagementSendStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ProgressManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `progress_management_student` = @ProgressManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `progress_management_student` = @ProgressManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@ProgressManagementStudent", platformSetting.ProgressManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ChatManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `chat_management_student` = @ChatManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `chat_management_student` = @ChatManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@ChatManagementStudent", platformSetting.ChatManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.TeamsManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teams_management_student` = @TeamsManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teams_management_student` = @TeamsManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@TeamsManagementStudent", platformSetting.TeamsManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MicrosoftSSOManagement))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `microsoft_sso_management` = @MicrosoftSSOManagement ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `microsoft_sso_management` = @MicrosoftSSOManagement ");
                            }
                            cmd.Parameters.AddWithValue("@MicrosoftSSOManagement", platformSetting.MicrosoftSSOManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InternalLoginManagement))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `internal_login_management` = @InternalLoginManagement ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `internal_login_management` = @InternalLoginManagement ");
                            }
                            cmd.Parameters.AddWithValue("@InternalLoginManagement", platformSetting.InternalLoginManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InternalPasswordReset))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `internal_password_reset` = @InternalPasswordReset ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `internal_password_reset` = @InternalPasswordReset ");
                            }
                            cmd.Parameters.AddWithValue("@InternalPasswordReset", platformSetting.InternalPasswordReset);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @ModifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @ModifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@ModifiedBy", platformSetting.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @CreatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @CreatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@CreatedAt", platformSetting.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @UpdatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @UpdatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@UpdatedAt", platformSetting.UpdatedAt);
                        }
                    }
                    if (platformSettingWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @IdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @IdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@IdWhereLike", "%" + platformSettingWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.SchoolManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `school_management_admin` LIKE @SchoolManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `school_management_admin` LIKE @SchoolManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminWhereLike", "%" + platformSettingWhereLike.SchoolManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.EnrollmentManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin` LIKE @EnrollmentManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin` LIKE @EnrollmentManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminWhereLike", "%" + platformSettingWhereLike.EnrollmentManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_admin` LIKE @UserManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_admin` LIKE @UserManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdminWhereLike", "%" + platformSettingWhereLike.UserManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_admin` LIKE @MessageManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_admin` LIKE @MessageManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdminWhereLike", "%" + platformSettingWhereLike.MessageManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin` LIKE @MessageManagementSendAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin` LIKE @MessageManagementSendAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminWhereLike", "%" + platformSettingWhereLike.MessageManagementSendAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin` LIKE @ConfigurationManagerAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin` LIKE @ConfigurationManagerAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_admin` LIKE @PasswordManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_admin` LIKE @PasswordManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminWhereLike", "%" + platformSettingWhereLike.PasswordManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.SchoolManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `school_management_admin_ore` LIKE @SchoolManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `school_management_admin_ore` LIKE @SchoolManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminOreWhereLike", "%" + platformSettingWhereLike.SchoolManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.EnrollmentManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin_ore` LIKE @EnrollmentManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin_ore` LIKE @EnrollmentManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOreWhereLike", "%" + platformSettingWhereLike.EnrollmentManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_admin_ore` LIKE @UserManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_admin_ore` LIKE @UserManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdminOreWhereLike", "%" + platformSettingWhereLike.UserManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_admin_ore` LIKE @MessageManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_admin_ore` LIKE @MessageManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdminOreWhereLike", "%" + platformSettingWhereLike.MessageManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin_ore` LIKE @MessageManagementSendAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin_ore` LIKE @MessageManagementSendAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminOreWhereLike", "%" + platformSettingWhereLike.MessageManagementSendAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin_ore` LIKE @ConfigurationManagerAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin_ore` LIKE @ConfigurationManagerAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOreWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_admin_ore` LIKE @PasswordManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_admin_ore` LIKE @PasswordManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminOreWhereLike", "%" + platformSettingWhereLike.PasswordManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_principal` LIKE @UserManagementPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_principal` LIKE @UserManagementPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementPrincipalWhereLike", "%" + platformSettingWhereLike.UserManagementPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_principal` LIKE @MessageManagementPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_principal` LIKE @MessageManagementPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementPrincipalWhereLike", "%" + platformSettingWhereLike.MessageManagementPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_principal` LIKE @MessageManagementSendPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_principal` LIKE @MessageManagementSendPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendPrincipalWhereLike", "%" + platformSettingWhereLike.MessageManagementSendPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_principal` LIKE @ConfigurationManagerPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_principal` LIKE @ConfigurationManagerPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipalWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_principal` LIKE @PasswordManagementPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_principal` LIKE @PasswordManagementPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementPrincipalWhereLike", "%" + platformSettingWhereLike.PasswordManagementPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_teacher` LIKE @UserManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_teacher` LIKE @UserManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementTeacherWhereLike", "%" + platformSettingWhereLike.UserManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.GroupInternalManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `group_internal_management_teacher` LIKE @GroupInternalManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `group_internal_management_teacher` LIKE @GroupInternalManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@GroupInternalManagementTeacherWhereLike", "%" + platformSettingWhereLike.GroupInternalManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_management_teacher` LIKE @InvitationManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_management_teacher` LIKE @InvitationManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementTeacherWhereLike", "%" + platformSettingWhereLike.InvitationManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationTeacherToParentManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_parent_management_teacher` LIKE @InvitationTeacherToParentManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_parent_management_teacher` LIKE @InvitationTeacherToParentManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacherWhereLike", "%" + platformSettingWhereLike.InvitationTeacherToParentManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ChatManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `chat_management_teacher` LIKE @ChatManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `chat_management_teacher` LIKE @ChatManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ChatManagementTeacherWhereLike", "%" + platformSettingWhereLike.ChatManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_teacher` LIKE @MessageManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_teacher` LIKE @MessageManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementTeacherWhereLike", "%" + platformSettingWhereLike.MessageManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_teacher` LIKE @MessageManagementSendTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_teacher` LIKE @MessageManagementSendTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendTeacherWhereLike", "%" + platformSettingWhereLike.MessageManagementSendTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.TeamMeetsManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `team_meets_management_teacher` LIKE @TeamMeetsManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `team_meets_management_teacher` LIKE @TeamMeetsManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacherWhereLike", "%" + platformSettingWhereLike.TeamMeetsManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_teacher` LIKE @ConfigurationManagerTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_teacher` LIKE @ConfigurationManagerTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerTeacherWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationTeacherToStudentManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_student_management_teacher` LIKE @InvitationTeacherToStudentManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_student_management_teacher` LIKE @InvitationTeacherToStudentManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacherWhereLike", "%" + platformSettingWhereLike.InvitationTeacherToStudentManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_teacher` LIKE @PasswordManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_teacher` LIKE @PasswordManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementTeacherWhereLike", "%" + platformSettingWhereLike.PasswordManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ProgressManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `progress_management_parent` LIKE @ProgressManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `progress_management_parent` LIKE @ProgressManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementParentWhereLike", "%" + platformSettingWhereLike.ProgressManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_management_parent` LIKE @InvitationManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_management_parent` LIKE @InvitationManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementParentWhereLike", "%" + platformSettingWhereLike.InvitationManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ChatManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `chat_management_parent` LIKE @ChatManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `chat_management_parent` LIKE @ChatManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ChatManagementParentWhereLike", "%" + platformSettingWhereLike.ChatManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_parent` LIKE @MessageManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_parent` LIKE @MessageManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementParentWhereLike", "%" + platformSettingWhereLike.MessageManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_parent` LIKE @MessageManagementSendParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_parent` LIKE @MessageManagementSendParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendParentWhereLike", "%" + platformSettingWhereLike.MessageManagementSendParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_parent` LIKE @ConfigurationManagerParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_parent` LIKE @ConfigurationManagerParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerParentWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_parent` LIKE @PasswordManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_parent` LIKE @PasswordManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementParentWhereLike", "%" + platformSettingWhereLike.PasswordManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_student` LIKE @MessageManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_student` LIKE @MessageManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementStudentWhereLike", "%" + platformSettingWhereLike.MessageManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_student` LIKE @MessageManagementSendStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_student` LIKE @MessageManagementSendStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendStudentWhereLike", "%" + platformSettingWhereLike.MessageManagementSendStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ProgressManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `progress_management_student` LIKE @ProgressManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `progress_management_student` LIKE @ProgressManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementStudentWhereLike", "%" + platformSettingWhereLike.ProgressManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ChatManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `chat_management_student` LIKE @ChatManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `chat_management_student` LIKE @ChatManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ChatManagementStudentWhereLike", "%" + platformSettingWhereLike.ChatManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.TeamsManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teams_management_student` LIKE @TeamsManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teams_management_student` LIKE @TeamsManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@TeamsManagementStudentWhereLike", "%" + platformSettingWhereLike.TeamsManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MicrosoftSSOManagement))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `microsoft_sso_management` LIKE @MicrosoftSSOManagementWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `microsoft_sso_management` LIKE @MicrosoftSSOManagementWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MicrosoftSSOManagementWhereLike", "%" + platformSettingWhereLike.MicrosoftSSOManagement + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InternalLoginManagement))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `internal_login_management` LIKE @InternalLoginManagementWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `internal_login_management` LIKE @InternalLoginManagementWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InternalLoginManagementWhereLike", "%" + platformSettingWhereLike.InternalLoginManagement + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InternalPasswordReset))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `internal_password_reset` LIKE @InternalPasswordResetWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `internal_password_reset` LIKE @InternalPasswordResetWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InternalPasswordResetWhereLike", "%" + platformSettingWhereLike.InternalPasswordReset + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @ModifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @ModifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ModifiedByWhereLike", "%" + platformSettingWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @CreatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @CreatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@CreatedAtWhereLike", "%" + platformSettingWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @UpdatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @UpdatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UpdatedAtWhereLike", "%" + platformSettingWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = String.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                platformSetting = new PlatformSettingModel();
                                if (!reader.IsDBNull(0))
                                    platformSetting.Id = reader.GetValue(0).ToString();
                                else
                                    platformSetting.Id = null;
                                if (!reader.IsDBNull(1))
                                    platformSetting.SchoolManagementAdmin = reader.GetValue(1).ToString();
                                else
                                    platformSetting.SchoolManagementAdmin = null;
                                if (!reader.IsDBNull(2))
                                    platformSetting.EnrollmentManagementAdmin = reader.GetValue(2).ToString();
                                else
                                    platformSetting.EnrollmentManagementAdmin = null;
                                if (!reader.IsDBNull(3))
                                    platformSetting.UserManagementAdmin = reader.GetValue(3).ToString();
                                else
                                    platformSetting.UserManagementAdmin = null;
                                if (!reader.IsDBNull(4))
                                    platformSetting.MessageManagementAdmin = reader.GetValue(4).ToString();
                                else
                                    platformSetting.MessageManagementAdmin = null;
                                if (!reader.IsDBNull(5))
                                    platformSetting.MessageManagementSendAdmin = reader.GetValue(5).ToString();
                                else
                                    platformSetting.MessageManagementSendAdmin = null;
                                if (!reader.IsDBNull(6))
                                    platformSetting.ConfigurationManagerAdmin = reader.GetValue(6).ToString();
                                else
                                    platformSetting.ConfigurationManagerAdmin = null;
                                if (!reader.IsDBNull(7))
                                    platformSetting.PasswordManagementAdmin = reader.GetValue(7).ToString();
                                else
                                    platformSetting.PasswordManagementAdmin = null;
                                if (!reader.IsDBNull(8))
                                    platformSetting.SchoolManagementAdminOre = reader.GetValue(8).ToString();
                                else
                                    platformSetting.SchoolManagementAdminOre = null;
                                if (!reader.IsDBNull(9))
                                    platformSetting.EnrollmentManagementAdminOre = reader.GetValue(9).ToString();
                                else
                                    platformSetting.EnrollmentManagementAdminOre = null;
                                if (!reader.IsDBNull(10))
                                    platformSetting.UserManagementAdminOre = reader.GetValue(10).ToString();
                                else
                                    platformSetting.UserManagementAdminOre = null;
                                if (!reader.IsDBNull(11))
                                    platformSetting.MessageManagementAdminOre = reader.GetValue(11).ToString();
                                else
                                    platformSetting.MessageManagementAdminOre = null;
                                if (!reader.IsDBNull(12))
                                    platformSetting.MessageManagementSendAdminOre = reader.GetValue(12).ToString();
                                else
                                    platformSetting.MessageManagementSendAdminOre = null;
                                if (!reader.IsDBNull(13))
                                    platformSetting.ConfigurationManagerAdminOre = reader.GetValue(13).ToString();
                                else
                                    platformSetting.ConfigurationManagerAdminOre = null;
                                if (!reader.IsDBNull(14))
                                    platformSetting.PasswordManagementAdminOre = reader.GetValue(14).ToString();
                                else
                                    platformSetting.PasswordManagementAdminOre = null;
                                if (!reader.IsDBNull(15))
                                    platformSetting.UserManagementPrincipal = reader.GetValue(15).ToString();
                                else
                                    platformSetting.UserManagementPrincipal = null;
                                if (!reader.IsDBNull(16))
                                    platformSetting.MessageManagementPrincipal = reader.GetValue(16).ToString();
                                else
                                    platformSetting.MessageManagementPrincipal = null;
                                if (!reader.IsDBNull(17))
                                    platformSetting.MessageManagementSendPrincipal = reader.GetValue(17).ToString();
                                else
                                    platformSetting.MessageManagementSendPrincipal = null;
                                if (!reader.IsDBNull(18))
                                    platformSetting.ConfigurationManagerPrincipal = reader.GetValue(18).ToString();
                                else
                                    platformSetting.ConfigurationManagerPrincipal = null;
                                if (!reader.IsDBNull(19))
                                    platformSetting.PasswordManagementPrincipal = reader.GetValue(19).ToString();
                                else
                                    platformSetting.PasswordManagementPrincipal = null;
                                if (!reader.IsDBNull(20))
                                    platformSetting.UserManagementTeacher = reader.GetValue(20).ToString();
                                else
                                    platformSetting.UserManagementTeacher = null;
                                if (!reader.IsDBNull(21))
                                    platformSetting.GroupInternalManagementTeacher = reader.GetValue(21).ToString();
                                else
                                    platformSetting.GroupInternalManagementTeacher = null;
                                if (!reader.IsDBNull(22))
                                    platformSetting.InvitationManagementTeacher = reader.GetValue(22).ToString();
                                else
                                    platformSetting.InvitationManagementTeacher = null;
                                if (!reader.IsDBNull(23))
                                    platformSetting.InvitationTeacherToParentManagementTeacher = reader.GetValue(23).ToString();
                                else
                                    platformSetting.InvitationTeacherToParentManagementTeacher = null;
                                if (!reader.IsDBNull(24))
                                    platformSetting.ChatManagementTeacher = reader.GetValue(24).ToString();
                                else
                                    platformSetting.ChatManagementTeacher = null;
                                if (!reader.IsDBNull(25))
                                    platformSetting.MessageManagementTeacher = reader.GetValue(25).ToString();
                                else
                                    platformSetting.MessageManagementTeacher = null;
                                if (!reader.IsDBNull(26))
                                    platformSetting.MessageManagementSendTeacher = reader.GetValue(26).ToString();
                                else
                                    platformSetting.MessageManagementSendTeacher = null;
                                if (!reader.IsDBNull(27))
                                    platformSetting.TeamMeetsManagementTeacher = reader.GetValue(27).ToString();
                                else
                                    platformSetting.TeamMeetsManagementTeacher = null;
                                if (!reader.IsDBNull(28))
                                    platformSetting.ConfigurationManagerTeacher = reader.GetValue(28).ToString();
                                else
                                    platformSetting.ConfigurationManagerTeacher = null;
                                if (!reader.IsDBNull(29))
                                    platformSetting.InvitationTeacherToStudentManagementTeacher = reader.GetValue(29).ToString();
                                else
                                    platformSetting.InvitationTeacherToStudentManagementTeacher = null;
                                if (!reader.IsDBNull(30))
                                    platformSetting.PasswordManagementTeacher = reader.GetValue(30).ToString();
                                else
                                    platformSetting.PasswordManagementTeacher = null;
                                if (!reader.IsDBNull(31))
                                    platformSetting.ProgressManagementParent = reader.GetValue(31).ToString();
                                else
                                    platformSetting.ProgressManagementParent = null;
                                if (!reader.IsDBNull(32))
                                    platformSetting.InvitationManagementParent = reader.GetValue(32).ToString();
                                else
                                    platformSetting.InvitationManagementParent = null;
                                if (!reader.IsDBNull(33))
                                    platformSetting.ChatManagementParent = reader.GetValue(33).ToString();
                                else
                                    platformSetting.ChatManagementParent = null;
                                if (!reader.IsDBNull(34))
                                    platformSetting.MessageManagementParent = reader.GetValue(34).ToString();
                                else
                                    platformSetting.MessageManagementParent = null;
                                if (!reader.IsDBNull(35))
                                    platformSetting.MessageManagementSendParent = reader.GetValue(35).ToString();
                                else
                                    platformSetting.MessageManagementSendParent = null;
                                if (!reader.IsDBNull(36))
                                    platformSetting.ConfigurationManagerParent = reader.GetValue(36).ToString();
                                else
                                    platformSetting.ConfigurationManagerParent = null;
                                if (!reader.IsDBNull(37))
                                    platformSetting.PasswordManagementParent = reader.GetValue(37).ToString();
                                else
                                    platformSetting.PasswordManagementParent = null;
                                if (!reader.IsDBNull(38))
                                    platformSetting.MessageManagementStudent = reader.GetValue(38).ToString();
                                else
                                    platformSetting.MessageManagementStudent = null;
                                if (!reader.IsDBNull(39))
                                    platformSetting.MessageManagementSendStudent = reader.GetValue(39).ToString();
                                else
                                    platformSetting.MessageManagementSendStudent = null;
                                if (!reader.IsDBNull(40))
                                    platformSetting.ProgressManagementStudent = reader.GetValue(40).ToString();
                                else
                                    platformSetting.ProgressManagementStudent = null;
                                if (!reader.IsDBNull(41))
                                    platformSetting.ChatManagementStudent = reader.GetValue(41).ToString();
                                else
                                    platformSetting.ChatManagementStudent = null;
                                if (!reader.IsDBNull(42))
                                    platformSetting.TeamsManagementStudent = reader.GetValue(42).ToString();
                                else
                                    platformSetting.TeamsManagementStudent = null;
                                if (!reader.IsDBNull(43))
                                    platformSetting.MicrosoftSSOManagement = reader.GetValue(43).ToString();
                                else
                                    platformSetting.MicrosoftSSOManagement = null;
                                if (!reader.IsDBNull(44))
                                    platformSetting.InternalLoginManagement = reader.GetValue(44).ToString();
                                else
                                    platformSetting.InternalLoginManagement = null;
                                if (!reader.IsDBNull(45))
                                    platformSetting.InternalPasswordReset = reader.GetValue(45).ToString();
                                else
                                    platformSetting.InternalPasswordReset = null;
                                if (!reader.IsDBNull(46))
                                    platformSetting.ModifiedBy = reader.GetValue(46).ToString();
                                else
                                    platformSetting.ModifiedBy = null;
                                if (!reader.IsDBNull(47))
                                    platformSetting.CreatedAt = reader.GetValue(47).ToString();
                                else
                                    platformSetting.CreatedAt = null;
                                if (!reader.IsDBNull(48))
                                    platformSetting.UpdatedAt = reader.GetValue(48).ToString();
                                else
                                    platformSetting.UpdatedAt = null;
                                response.Add(platformSetting);
                                platformSetting = new PlatformSettingModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlatformSettingModel>();
                        reader.Close();
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlatformSettingModel> Read(string connectionString, PlatformSettingModel platformSetting, bool orderByLastDate, int fromRow, int limit, PlatformSettingModel platformSettingWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformSettingModel> response = new List<PlatformSettingModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `school_management_admin`, `enrollment_management_admin`, `user_management_admin`, `message_management_admin`, `message_management_send_admin`, `configuration_manager_admin`, `password_management_admin`, `school_management_admin_ore`, `enrollment_management_admin_ore`, `user_management_admin_ore`, `message_management_admin_ore`, `message_management_send_admin_ore`, `configuration_manager_admin_ore`, `password_management_admin_ore`, `user_management_principal`, `message_management_principal`, `message_management_send_principal`, `configuration_manager_principal`, `password_management_principal`, `user_management_teacher`, `group_internal_management_teacher`, `invitation_management_teacher`, `invitation_teacher_to_parent_management_teacher`, `chat_management_teacher`, `message_management_teacher`, `message_management_send_teacher`, `team_meets_management_teacher`, `configuration_manager_teacher`, `invitation_teacher_to_student_management_teacher`, `password_management_teacher`, `progress_management_parent`, `invitation_management_parent`, `chat_management_parent`, `message_management_parent`, `message_management_send_parent`, `configuration_manager_parent`, `password_management_parent`, `message_management_student`, `message_management_send_student`, `progress_management_student`, `chat_management_student`, `teams_management_student`, `microsoft_sso_management`, `internal_login_management`, `internal_password_reset`, `modified_by`, `created_at`, `updated_at`  FROM platform_settings ";
                {
                    if (platformSetting != null)
                    {
                        if (!String.IsNullOrEmpty(platformSetting.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @Id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @Id ");
                            }
                            cmd.Parameters.AddWithValue("@Id", platformSetting.Id);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.SchoolManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `school_management_admin` = @SchoolManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `school_management_admin` = @SchoolManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@SchoolManagementAdmin", platformSetting.SchoolManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.EnrollmentManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin` = @EnrollmentManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin` = @EnrollmentManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdmin", platformSetting.EnrollmentManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_admin` = @UserManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_admin` = @UserManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementAdmin", platformSetting.UserManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_admin` = @MessageManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_admin` = @MessageManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementAdmin", platformSetting.MessageManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin` = @MessageManagementSendAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin` = @MessageManagementSendAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdmin", platformSetting.MessageManagementSendAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin` = @ConfigurationManagerAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin` = @ConfigurationManagerAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdmin", platformSetting.ConfigurationManagerAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementAdmin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_admin` = @PasswordManagementAdmin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_admin` = @PasswordManagementAdmin ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementAdmin", platformSetting.PasswordManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.SchoolManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `school_management_admin_ore` = @SchoolManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `school_management_admin_ore` = @SchoolManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminOre", platformSetting.SchoolManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.EnrollmentManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOre", platformSetting.EnrollmentManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_admin_ore` = @UserManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_admin_ore` = @UserManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementAdminOre", platformSetting.UserManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_admin_ore` = @MessageManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_admin_ore` = @MessageManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementAdminOre", platformSetting.MessageManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin_ore` = @MessageManagementSendAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin_ore` = @MessageManagementSendAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminOre", platformSetting.MessageManagementSendAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOre", platformSetting.ConfigurationManagerAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementAdminOre))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_admin_ore` = @PasswordManagementAdminOre ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_admin_ore` = @PasswordManagementAdminOre ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminOre", platformSetting.PasswordManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_principal` = @UserManagementPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_principal` = @UserManagementPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementPrincipal", platformSetting.UserManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_principal` = @MessageManagementPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_principal` = @MessageManagementPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementPrincipal", platformSetting.MessageManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_principal` = @MessageManagementSendPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_principal` = @MessageManagementSendPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendPrincipal", platformSetting.MessageManagementSendPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_principal` = @ConfigurationManagerPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_principal` = @ConfigurationManagerPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipal", platformSetting.ConfigurationManagerPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementPrincipal))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_principal` = @PasswordManagementPrincipal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_principal` = @PasswordManagementPrincipal ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementPrincipal", platformSetting.PasswordManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UserManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_management_teacher` = @UserManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_management_teacher` = @UserManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@UserManagementTeacher", platformSetting.UserManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.GroupInternalManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `group_internal_management_teacher` = @GroupInternalManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `group_internal_management_teacher` = @GroupInternalManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@GroupInternalManagementTeacher", platformSetting.GroupInternalManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_management_teacher` = @InvitationManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_management_teacher` = @InvitationManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationManagementTeacher", platformSetting.InvitationManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationTeacherToParentManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacher", platformSetting.InvitationTeacherToParentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ChatManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `chat_management_teacher` = @ChatManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `chat_management_teacher` = @ChatManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@ChatManagementTeacher", platformSetting.ChatManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_teacher` = @MessageManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_teacher` = @MessageManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementTeacher", platformSetting.MessageManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_teacher` = @MessageManagementSendTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_teacher` = @MessageManagementSendTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendTeacher", platformSetting.MessageManagementSendTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.TeamMeetsManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `team_meets_management_teacher` = @TeamMeetsManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `team_meets_management_teacher` = @TeamMeetsManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacher", platformSetting.TeamMeetsManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_teacher` = @ConfigurationManagerTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_teacher` = @ConfigurationManagerTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerTeacher", platformSetting.ConfigurationManagerTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationTeacherToStudentManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacher", platformSetting.InvitationTeacherToStudentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_teacher` = @PasswordManagementTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_teacher` = @PasswordManagementTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementTeacher", platformSetting.PasswordManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ProgressManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `progress_management_parent` = @ProgressManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `progress_management_parent` = @ProgressManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@ProgressManagementParent", platformSetting.ProgressManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InvitationManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `invitation_management_parent` = @InvitationManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `invitation_management_parent` = @InvitationManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@InvitationManagementParent", platformSetting.InvitationManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ChatManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `chat_management_parent` = @ChatManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `chat_management_parent` = @ChatManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@ChatManagementParent", platformSetting.ChatManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_parent` = @MessageManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_parent` = @MessageManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementParent", platformSetting.MessageManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_parent` = @MessageManagementSendParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_parent` = @MessageManagementSendParent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendParent", platformSetting.MessageManagementSendParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `configuration_manager_parent` = @ConfigurationManagerParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `configuration_manager_parent` = @ConfigurationManagerParent ");
                            }
                            cmd.Parameters.AddWithValue("@ConfigurationManagerParent", platformSetting.ConfigurationManagerParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.PasswordManagementParent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `password_management_parent` = @PasswordManagementParent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `password_management_parent` = @PasswordManagementParent ");
                            }
                            cmd.Parameters.AddWithValue("@PasswordManagementParent", platformSetting.PasswordManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_student` = @MessageManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_student` = @MessageManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementStudent", platformSetting.MessageManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `message_management_send_student` = @MessageManagementSendStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `message_management_send_student` = @MessageManagementSendStudent ");
                            }
                            cmd.Parameters.AddWithValue("@MessageManagementSendStudent", platformSetting.MessageManagementSendStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ProgressManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `progress_management_student` = @ProgressManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `progress_management_student` = @ProgressManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@ProgressManagementStudent", platformSetting.ProgressManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ChatManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `chat_management_student` = @ChatManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `chat_management_student` = @ChatManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@ChatManagementStudent", platformSetting.ChatManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.TeamsManagementStudent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teams_management_student` = @TeamsManagementStudent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teams_management_student` = @TeamsManagementStudent ");
                            }
                            cmd.Parameters.AddWithValue("@TeamsManagementStudent", platformSetting.TeamsManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.MicrosoftSSOManagement))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `microsoft_sso_management` = @MicrosoftSSOManagement ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `microsoft_sso_management` = @MicrosoftSSOManagement ");
                            }
                            cmd.Parameters.AddWithValue("@MicrosoftSSOManagement", platformSetting.MicrosoftSSOManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InternalLoginManagement))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `internal_login_management` = @InternalLoginManagement ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `internal_login_management` = @InternalLoginManagement ");
                            }
                            cmd.Parameters.AddWithValue("@InternalLoginManagement", platformSetting.InternalLoginManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.InternalPasswordReset))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `internal_password_reset` = @InternalPasswordReset ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `internal_password_reset` = @InternalPasswordReset ");
                            }
                            cmd.Parameters.AddWithValue("@InternalPasswordReset", platformSetting.InternalPasswordReset);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @ModifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @ModifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@ModifiedBy", platformSetting.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @CreatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @CreatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@CreatedAt", platformSetting.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(platformSetting.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @UpdatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @UpdatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@UpdatedAt", platformSetting.UpdatedAt);
                        }
                    }
                    if (platformSettingWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @IdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @IdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@IdWhereLike", "%" + platformSettingWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.SchoolManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `school_management_admin` LIKE @SchoolManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `school_management_admin` LIKE @SchoolManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminWhereLike", "%" + platformSettingWhereLike.SchoolManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.EnrollmentManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin` LIKE @EnrollmentManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin` LIKE @EnrollmentManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminWhereLike", "%" + platformSettingWhereLike.EnrollmentManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_admin` LIKE @UserManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_admin` LIKE @UserManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdminWhereLike", "%" + platformSettingWhereLike.UserManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_admin` LIKE @MessageManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_admin` LIKE @MessageManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdminWhereLike", "%" + platformSettingWhereLike.MessageManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin` LIKE @MessageManagementSendAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin` LIKE @MessageManagementSendAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminWhereLike", "%" + platformSettingWhereLike.MessageManagementSendAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin` LIKE @ConfigurationManagerAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin` LIKE @ConfigurationManagerAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementAdmin))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_admin` LIKE @PasswordManagementAdminWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_admin` LIKE @PasswordManagementAdminWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminWhereLike", "%" + platformSettingWhereLike.PasswordManagementAdmin + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.SchoolManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `school_management_admin_ore` LIKE @SchoolManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `school_management_admin_ore` LIKE @SchoolManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminOreWhereLike", "%" + platformSettingWhereLike.SchoolManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.EnrollmentManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `enrollment_management_admin_ore` LIKE @EnrollmentManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `enrollment_management_admin_ore` LIKE @EnrollmentManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOreWhereLike", "%" + platformSettingWhereLike.EnrollmentManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_admin_ore` LIKE @UserManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_admin_ore` LIKE @UserManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdminOreWhereLike", "%" + platformSettingWhereLike.UserManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_admin_ore` LIKE @MessageManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_admin_ore` LIKE @MessageManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdminOreWhereLike", "%" + platformSettingWhereLike.MessageManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_admin_ore` LIKE @MessageManagementSendAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_admin_ore` LIKE @MessageManagementSendAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminOreWhereLike", "%" + platformSettingWhereLike.MessageManagementSendAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_admin_ore` LIKE @ConfigurationManagerAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_admin_ore` LIKE @ConfigurationManagerAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOreWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementAdminOre))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_admin_ore` LIKE @PasswordManagementAdminOreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_admin_ore` LIKE @PasswordManagementAdminOreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminOreWhereLike", "%" + platformSettingWhereLike.PasswordManagementAdminOre + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_principal` LIKE @UserManagementPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_principal` LIKE @UserManagementPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementPrincipalWhereLike", "%" + platformSettingWhereLike.UserManagementPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_principal` LIKE @MessageManagementPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_principal` LIKE @MessageManagementPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementPrincipalWhereLike", "%" + platformSettingWhereLike.MessageManagementPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_principal` LIKE @MessageManagementSendPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_principal` LIKE @MessageManagementSendPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendPrincipalWhereLike", "%" + platformSettingWhereLike.MessageManagementSendPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_principal` LIKE @ConfigurationManagerPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_principal` LIKE @ConfigurationManagerPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipalWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementPrincipal))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_principal` LIKE @PasswordManagementPrincipalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_principal` LIKE @PasswordManagementPrincipalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementPrincipalWhereLike", "%" + platformSettingWhereLike.PasswordManagementPrincipal + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UserManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_management_teacher` LIKE @UserManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_management_teacher` LIKE @UserManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UserManagementTeacherWhereLike", "%" + platformSettingWhereLike.UserManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.GroupInternalManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `group_internal_management_teacher` LIKE @GroupInternalManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `group_internal_management_teacher` LIKE @GroupInternalManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@GroupInternalManagementTeacherWhereLike", "%" + platformSettingWhereLike.GroupInternalManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_management_teacher` LIKE @InvitationManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_management_teacher` LIKE @InvitationManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementTeacherWhereLike", "%" + platformSettingWhereLike.InvitationManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationTeacherToParentManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_parent_management_teacher` LIKE @InvitationTeacherToParentManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_parent_management_teacher` LIKE @InvitationTeacherToParentManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacherWhereLike", "%" + platformSettingWhereLike.InvitationTeacherToParentManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ChatManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `chat_management_teacher` LIKE @ChatManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `chat_management_teacher` LIKE @ChatManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ChatManagementTeacherWhereLike", "%" + platformSettingWhereLike.ChatManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_teacher` LIKE @MessageManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_teacher` LIKE @MessageManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementTeacherWhereLike", "%" + platformSettingWhereLike.MessageManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_teacher` LIKE @MessageManagementSendTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_teacher` LIKE @MessageManagementSendTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendTeacherWhereLike", "%" + platformSettingWhereLike.MessageManagementSendTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.TeamMeetsManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `team_meets_management_teacher` LIKE @TeamMeetsManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `team_meets_management_teacher` LIKE @TeamMeetsManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacherWhereLike", "%" + platformSettingWhereLike.TeamMeetsManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_teacher` LIKE @ConfigurationManagerTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_teacher` LIKE @ConfigurationManagerTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerTeacherWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationTeacherToStudentManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_teacher_to_student_management_teacher` LIKE @InvitationTeacherToStudentManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_teacher_to_student_management_teacher` LIKE @InvitationTeacherToStudentManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacherWhereLike", "%" + platformSettingWhereLike.InvitationTeacherToStudentManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_teacher` LIKE @PasswordManagementTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_teacher` LIKE @PasswordManagementTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementTeacherWhereLike", "%" + platformSettingWhereLike.PasswordManagementTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ProgressManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `progress_management_parent` LIKE @ProgressManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `progress_management_parent` LIKE @ProgressManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementParentWhereLike", "%" + platformSettingWhereLike.ProgressManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InvitationManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `invitation_management_parent` LIKE @InvitationManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `invitation_management_parent` LIKE @InvitationManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementParentWhereLike", "%" + platformSettingWhereLike.InvitationManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ChatManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `chat_management_parent` LIKE @ChatManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `chat_management_parent` LIKE @ChatManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ChatManagementParentWhereLike", "%" + platformSettingWhereLike.ChatManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_parent` LIKE @MessageManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_parent` LIKE @MessageManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementParentWhereLike", "%" + platformSettingWhereLike.MessageManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_parent` LIKE @MessageManagementSendParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_parent` LIKE @MessageManagementSendParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendParentWhereLike", "%" + platformSettingWhereLike.MessageManagementSendParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ConfigurationManagerParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `configuration_manager_parent` LIKE @ConfigurationManagerParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `configuration_manager_parent` LIKE @ConfigurationManagerParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerParentWhereLike", "%" + platformSettingWhereLike.ConfigurationManagerParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.PasswordManagementParent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `password_management_parent` LIKE @PasswordManagementParentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `password_management_parent` LIKE @PasswordManagementParentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementParentWhereLike", "%" + platformSettingWhereLike.PasswordManagementParent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_student` LIKE @MessageManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_student` LIKE @MessageManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementStudentWhereLike", "%" + platformSettingWhereLike.MessageManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MessageManagementSendStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `message_management_send_student` LIKE @MessageManagementSendStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `message_management_send_student` LIKE @MessageManagementSendStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendStudentWhereLike", "%" + platformSettingWhereLike.MessageManagementSendStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ProgressManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `progress_management_student` LIKE @ProgressManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `progress_management_student` LIKE @ProgressManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementStudentWhereLike", "%" + platformSettingWhereLike.ProgressManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ChatManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `chat_management_student` LIKE @ChatManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `chat_management_student` LIKE @ChatManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ChatManagementStudentWhereLike", "%" + platformSettingWhereLike.ChatManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.TeamsManagementStudent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teams_management_student` LIKE @TeamsManagementStudentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teams_management_student` LIKE @TeamsManagementStudentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@TeamsManagementStudentWhereLike", "%" + platformSettingWhereLike.TeamsManagementStudent + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.MicrosoftSSOManagement))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `microsoft_sso_management` LIKE @MicrosoftSSOManagementWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `microsoft_sso_management` LIKE @MicrosoftSSOManagementWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@MicrosoftSSOManagementWhereLike", "%" + platformSettingWhereLike.MicrosoftSSOManagement + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InternalLoginManagement))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `internal_login_management` LIKE @InternalLoginManagementWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `internal_login_management` LIKE @InternalLoginManagementWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InternalLoginManagementWhereLike", "%" + platformSettingWhereLike.InternalLoginManagement + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.InternalPasswordReset))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `internal_password_reset` LIKE @InternalPasswordResetWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `internal_password_reset` LIKE @InternalPasswordResetWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@InternalPasswordResetWhereLike", "%" + platformSettingWhereLike.InternalPasswordReset + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @ModifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @ModifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@ModifiedByWhereLike", "%" + platformSettingWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @CreatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @CreatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@CreatedAtWhereLike", "%" + platformSettingWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @UpdatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @UpdatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@UpdatedAtWhereLike", "%" + platformSettingWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    sql = String.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                platformSetting = new PlatformSettingModel();
                                if (!reader.IsDBNull(0))
                                    platformSetting.Id = reader.GetValue(0).ToString();
                                else
                                    platformSetting.Id = null;
                                if (!reader.IsDBNull(1))
                                    platformSetting.SchoolManagementAdmin = reader.GetValue(1).ToString();
                                else
                                    platformSetting.SchoolManagementAdmin = null;
                                if (!reader.IsDBNull(2))
                                    platformSetting.EnrollmentManagementAdmin = reader.GetValue(2).ToString();
                                else
                                    platformSetting.EnrollmentManagementAdmin = null;
                                if (!reader.IsDBNull(3))
                                    platformSetting.UserManagementAdmin = reader.GetValue(3).ToString();
                                else
                                    platformSetting.UserManagementAdmin = null;
                                if (!reader.IsDBNull(4))
                                    platformSetting.MessageManagementAdmin = reader.GetValue(4).ToString();
                                else
                                    platformSetting.MessageManagementAdmin = null;
                                if (!reader.IsDBNull(5))
                                    platformSetting.MessageManagementSendAdmin = reader.GetValue(5).ToString();
                                else
                                    platformSetting.MessageManagementSendAdmin = null;
                                if (!reader.IsDBNull(6))
                                    platformSetting.ConfigurationManagerAdmin = reader.GetValue(6).ToString();
                                else
                                    platformSetting.ConfigurationManagerAdmin = null;
                                if (!reader.IsDBNull(7))
                                    platformSetting.PasswordManagementAdmin = reader.GetValue(7).ToString();
                                else
                                    platformSetting.PasswordManagementAdmin = null;
                                if (!reader.IsDBNull(8))
                                    platformSetting.SchoolManagementAdminOre = reader.GetValue(8).ToString();
                                else
                                    platformSetting.SchoolManagementAdminOre = null;
                                if (!reader.IsDBNull(9))
                                    platformSetting.EnrollmentManagementAdminOre = reader.GetValue(9).ToString();
                                else
                                    platformSetting.EnrollmentManagementAdminOre = null;
                                if (!reader.IsDBNull(10))
                                    platformSetting.UserManagementAdminOre = reader.GetValue(10).ToString();
                                else
                                    platformSetting.UserManagementAdminOre = null;
                                if (!reader.IsDBNull(11))
                                    platformSetting.MessageManagementAdminOre = reader.GetValue(11).ToString();
                                else
                                    platformSetting.MessageManagementAdminOre = null;
                                if (!reader.IsDBNull(12))
                                    platformSetting.MessageManagementSendAdminOre = reader.GetValue(12).ToString();
                                else
                                    platformSetting.MessageManagementSendAdminOre = null;
                                if (!reader.IsDBNull(13))
                                    platformSetting.ConfigurationManagerAdminOre = reader.GetValue(13).ToString();
                                else
                                    platformSetting.ConfigurationManagerAdminOre = null;
                                if (!reader.IsDBNull(14))
                                    platformSetting.PasswordManagementAdminOre = reader.GetValue(14).ToString();
                                else
                                    platformSetting.PasswordManagementAdminOre = null;
                                if (!reader.IsDBNull(15))
                                    platformSetting.UserManagementPrincipal = reader.GetValue(15).ToString();
                                else
                                    platformSetting.UserManagementPrincipal = null;
                                if (!reader.IsDBNull(16))
                                    platformSetting.MessageManagementPrincipal = reader.GetValue(16).ToString();
                                else
                                    platformSetting.MessageManagementPrincipal = null;
                                if (!reader.IsDBNull(17))
                                    platformSetting.MessageManagementSendPrincipal = reader.GetValue(17).ToString();
                                else
                                    platformSetting.MessageManagementSendPrincipal = null;
                                if (!reader.IsDBNull(18))
                                    platformSetting.ConfigurationManagerPrincipal = reader.GetValue(18).ToString();
                                else
                                    platformSetting.ConfigurationManagerPrincipal = null;
                                if (!reader.IsDBNull(19))
                                    platformSetting.PasswordManagementPrincipal = reader.GetValue(19).ToString();
                                else
                                    platformSetting.PasswordManagementPrincipal = null;
                                if (!reader.IsDBNull(20))
                                    platformSetting.UserManagementTeacher = reader.GetValue(20).ToString();
                                else
                                    platformSetting.UserManagementTeacher = null;
                                if (!reader.IsDBNull(21))
                                    platformSetting.GroupInternalManagementTeacher = reader.GetValue(21).ToString();
                                else
                                    platformSetting.GroupInternalManagementTeacher = null;
                                if (!reader.IsDBNull(22))
                                    platformSetting.InvitationManagementTeacher = reader.GetValue(22).ToString();
                                else
                                    platformSetting.InvitationManagementTeacher = null;
                                if (!reader.IsDBNull(23))
                                    platformSetting.InvitationTeacherToParentManagementTeacher = reader.GetValue(23).ToString();
                                else
                                    platformSetting.InvitationTeacherToParentManagementTeacher = null;
                                if (!reader.IsDBNull(24))
                                    platformSetting.ChatManagementTeacher = reader.GetValue(24).ToString();
                                else
                                    platformSetting.ChatManagementTeacher = null;
                                if (!reader.IsDBNull(25))
                                    platformSetting.MessageManagementTeacher = reader.GetValue(25).ToString();
                                else
                                    platformSetting.MessageManagementTeacher = null;
                                if (!reader.IsDBNull(26))
                                    platformSetting.MessageManagementSendTeacher = reader.GetValue(26).ToString();
                                else
                                    platformSetting.MessageManagementSendTeacher = null;
                                if (!reader.IsDBNull(27))
                                    platformSetting.TeamMeetsManagementTeacher = reader.GetValue(27).ToString();
                                else
                                    platformSetting.TeamMeetsManagementTeacher = null;
                                if (!reader.IsDBNull(28))
                                    platformSetting.ConfigurationManagerTeacher = reader.GetValue(28).ToString();
                                else
                                    platformSetting.ConfigurationManagerTeacher = null;
                                if (!reader.IsDBNull(29))
                                    platformSetting.InvitationTeacherToStudentManagementTeacher = reader.GetValue(29).ToString();
                                else
                                    platformSetting.InvitationTeacherToStudentManagementTeacher = null;
                                if (!reader.IsDBNull(30))
                                    platformSetting.PasswordManagementTeacher = reader.GetValue(30).ToString();
                                else
                                    platformSetting.PasswordManagementTeacher = null;
                                if (!reader.IsDBNull(31))
                                    platformSetting.ProgressManagementParent = reader.GetValue(31).ToString();
                                else
                                    platformSetting.ProgressManagementParent = null;
                                if (!reader.IsDBNull(32))
                                    platformSetting.InvitationManagementParent = reader.GetValue(32).ToString();
                                else
                                    platformSetting.InvitationManagementParent = null;
                                if (!reader.IsDBNull(33))
                                    platformSetting.ChatManagementParent = reader.GetValue(33).ToString();
                                else
                                    platformSetting.ChatManagementParent = null;
                                if (!reader.IsDBNull(34))
                                    platformSetting.MessageManagementParent = reader.GetValue(34).ToString();
                                else
                                    platformSetting.MessageManagementParent = null;
                                if (!reader.IsDBNull(35))
                                    platformSetting.MessageManagementSendParent = reader.GetValue(35).ToString();
                                else
                                    platformSetting.MessageManagementSendParent = null;
                                if (!reader.IsDBNull(36))
                                    platformSetting.ConfigurationManagerParent = reader.GetValue(36).ToString();
                                else
                                    platformSetting.ConfigurationManagerParent = null;
                                if (!reader.IsDBNull(37))
                                    platformSetting.PasswordManagementParent = reader.GetValue(37).ToString();
                                else
                                    platformSetting.PasswordManagementParent = null;
                                if (!reader.IsDBNull(38))
                                    platformSetting.MessageManagementStudent = reader.GetValue(38).ToString();
                                else
                                    platformSetting.MessageManagementStudent = null;
                                if (!reader.IsDBNull(39))
                                    platformSetting.MessageManagementSendStudent = reader.GetValue(39).ToString();
                                else
                                    platformSetting.MessageManagementSendStudent = null;
                                if (!reader.IsDBNull(40))
                                    platformSetting.ProgressManagementStudent = reader.GetValue(40).ToString();
                                else
                                    platformSetting.ProgressManagementStudent = null;
                                if (!reader.IsDBNull(41))
                                    platformSetting.ChatManagementStudent = reader.GetValue(41).ToString();
                                else
                                    platformSetting.ChatManagementStudent = null;
                                if (!reader.IsDBNull(42))
                                    platformSetting.TeamsManagementStudent = reader.GetValue(42).ToString();
                                else
                                    platformSetting.TeamsManagementStudent = null;
                                if (!reader.IsDBNull(43))
                                    platformSetting.MicrosoftSSOManagement = reader.GetValue(43).ToString();
                                else
                                    platformSetting.MicrosoftSSOManagement = null;
                                if (!reader.IsDBNull(44))
                                    platformSetting.InternalLoginManagement = reader.GetValue(44).ToString();
                                else
                                    platformSetting.InternalLoginManagement = null;
                                if (!reader.IsDBNull(45))
                                    platformSetting.InternalPasswordReset = reader.GetValue(45).ToString();
                                else
                                    platformSetting.InternalPasswordReset = null;
                                if (!reader.IsDBNull(46))
                                    platformSetting.ModifiedBy = reader.GetValue(46).ToString();
                                else
                                    platformSetting.ModifiedBy = null;
                                if (!reader.IsDBNull(47))
                                    platformSetting.CreatedAt = reader.GetValue(47).ToString();
                                else
                                    platformSetting.CreatedAt = null;
                                if (!reader.IsDBNull(48))
                                    platformSetting.UpdatedAt = reader.GetValue(48).ToString();
                                else
                                    platformSetting.UpdatedAt = null;
                                response.Add(platformSetting);
                                platformSetting = new PlatformSettingModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlatformSettingModel>();
                        reader.Close();
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlatformSettingModel> Read(string connectionString, List<PlatformSettingModel> platformSettingWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformSettingModel> response = new List<PlatformSettingModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `school_management_admin`, `enrollment_management_admin`, `user_management_admin`, `message_management_admin`, `message_management_send_admin`, `configuration_manager_admin`, `password_management_admin`, `school_management_admin_ore`, `enrollment_management_admin_ore`, `user_management_admin_ore`, `message_management_admin_ore`, `message_management_send_admin_ore`, `configuration_manager_admin_ore`, `password_management_admin_ore`, `user_management_principal`, `message_management_principal`, `message_management_send_principal`, `configuration_manager_principal`, `password_management_principal`, `user_management_teacher`, `group_internal_management_teacher`, `invitation_management_teacher`, `invitation_teacher_to_parent_management_teacher`, `chat_management_teacher`, `message_management_teacher`, `message_management_send_teacher`, `team_meets_management_teacher`, `configuration_manager_teacher`, `invitation_teacher_to_student_management_teacher`, `password_management_teacher`, `progress_management_parent`, `invitation_management_parent`, `chat_management_parent`, `message_management_parent`, `message_management_send_parent`, `configuration_manager_parent`, `password_management_parent`, `message_management_student`, `message_management_send_student`, `progress_management_student`, `chat_management_student`, `teams_management_student`, `microsoft_sso_management`, `internal_login_management`, `internal_password_reset`, `modified_by`, `created_at`, `updated_at`  FROM platform_settings ";
                if (platformSettingWhere != null && platformSettingWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < platformSettingWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @Id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@Id" + i, platformSettingWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].SchoolManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `school_management_admin` = @SchoolManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdmin" + i, platformSettingWhere[i].SchoolManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].EnrollmentManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enrollment_management_admin` = @EnrollmentManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdmin" + i, platformSettingWhere[i].EnrollmentManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_admin` = @UserManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdmin" + i, platformSettingWhere[i].UserManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_admin` = @MessageManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdmin" + i, platformSettingWhere[i].MessageManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_admin` = @MessageManagementSendAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdmin" + i, platformSettingWhere[i].MessageManagementSendAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_admin` = @ConfigurationManagerAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdmin" + i, platformSettingWhere[i].ConfigurationManagerAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_admin` = @PasswordManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdmin" + i, platformSettingWhere[i].PasswordManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].SchoolManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `school_management_admin_ore` = @SchoolManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminOre" + i, platformSettingWhere[i].SchoolManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].EnrollmentManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOre" + i, platformSettingWhere[i].EnrollmentManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_admin_ore` = @UserManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdminOre" + i, platformSettingWhere[i].UserManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_admin_ore` = @MessageManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdminOre" + i, platformSettingWhere[i].MessageManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_admin_ore` = @MessageManagementSendAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminOre" + i, platformSettingWhere[i].MessageManagementSendAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOre" + i, platformSettingWhere[i].ConfigurationManagerAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_admin_ore` = @PasswordManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminOre" + i, platformSettingWhere[i].PasswordManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_principal` = @UserManagementPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementPrincipal" + i, platformSettingWhere[i].UserManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_principal` = @MessageManagementPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementPrincipal" + i, platformSettingWhere[i].MessageManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_principal` = @MessageManagementSendPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendPrincipal" + i, platformSettingWhere[i].MessageManagementSendPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_principal` = @ConfigurationManagerPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipal" + i, platformSettingWhere[i].ConfigurationManagerPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_principal` = @PasswordManagementPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementPrincipal" + i, platformSettingWhere[i].PasswordManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_teacher` = @UserManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementTeacher" + i, platformSettingWhere[i].UserManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].GroupInternalManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `group_internal_management_teacher` = @GroupInternalManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@GroupInternalManagementTeacher" + i, platformSettingWhere[i].GroupInternalManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_management_teacher` = @InvitationManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementTeacher" + i, platformSettingWhere[i].InvitationManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationTeacherToParentManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacher" + i, platformSettingWhere[i].InvitationTeacherToParentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ChatManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `chat_management_teacher` = @ChatManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ChatManagementTeacher" + i, platformSettingWhere[i].ChatManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_teacher` = @MessageManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementTeacher" + i, platformSettingWhere[i].MessageManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_teacher` = @MessageManagementSendTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendTeacher" + i, platformSettingWhere[i].MessageManagementSendTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].TeamMeetsManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `team_meets_management_teacher` = @TeamMeetsManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacher" + i, platformSettingWhere[i].TeamMeetsManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_teacher` = @ConfigurationManagerTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerTeacher" + i, platformSettingWhere[i].ConfigurationManagerTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationTeacherToStudentManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacher" + i, platformSettingWhere[i].InvitationTeacherToStudentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_teacher` = @PasswordManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementTeacher" + i, platformSettingWhere[i].PasswordManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ProgressManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `progress_management_parent` = @ProgressManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementParent" + i, platformSettingWhere[i].ProgressManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_management_parent` = @InvitationManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementParent" + i, platformSettingWhere[i].InvitationManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ChatManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `chat_management_parent` = @ChatManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ChatManagementParent" + i, platformSettingWhere[i].ChatManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_parent` = @MessageManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementParent" + i, platformSettingWhere[i].MessageManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_parent` = @MessageManagementSendParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendParent" + i, platformSettingWhere[i].MessageManagementSendParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_parent` = @ConfigurationManagerParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerParent" + i, platformSettingWhere[i].ConfigurationManagerParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_parent` = @PasswordManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementParent" + i, platformSettingWhere[i].PasswordManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_student` = @MessageManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementStudent" + i, platformSettingWhere[i].MessageManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_student` = @MessageManagementSendStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendStudent" + i, platformSettingWhere[i].MessageManagementSendStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ProgressManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `progress_management_student` = @ProgressManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementStudent" + i, platformSettingWhere[i].ProgressManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ChatManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `chat_management_student` = @ChatManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ChatManagementStudent" + i, platformSettingWhere[i].ChatManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].TeamsManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teams_management_student` = @TeamsManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@TeamsManagementStudent" + i, platformSettingWhere[i].TeamsManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MicrosoftSSOManagement))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `microsoft_sso_management` = @MicrosoftSSOManagement" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MicrosoftSSOManagement" + i, platformSettingWhere[i].MicrosoftSSOManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InternalLoginManagement))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `internal_login_management` = @InternalLoginManagement" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InternalLoginManagement" + i, platformSettingWhere[i].InternalLoginManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InternalPasswordReset))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `internal_password_reset` = @InternalPasswordReset" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InternalPasswordReset" + i, platformSettingWhere[i].InternalPasswordReset);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @ModifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ModifiedBy" + i, platformSettingWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @CreatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@CreatedAt" + i, platformSettingWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @UpdatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UpdatedAt" + i, platformSettingWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != platformSettingWhere.Count - 1)
                            sqlIn += " OR ";
                    }
                }
                cmd.CommandText = sql + " " + sqlIn;
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            PlatformSettingModel platformSetting = new PlatformSettingModel(); if (!reader.IsDBNull(0))
                                platformSetting.Id = reader.GetValue(0).ToString();
                            else
                                platformSetting.Id = null;
                            if (!reader.IsDBNull(1))
                                platformSetting.SchoolManagementAdmin = reader.GetValue(1).ToString();
                            else
                                platformSetting.SchoolManagementAdmin = null;
                            if (!reader.IsDBNull(2))
                                platformSetting.EnrollmentManagementAdmin = reader.GetValue(2).ToString();
                            else
                                platformSetting.EnrollmentManagementAdmin = null;
                            if (!reader.IsDBNull(3))
                                platformSetting.UserManagementAdmin = reader.GetValue(3).ToString();
                            else
                                platformSetting.UserManagementAdmin = null;
                            if (!reader.IsDBNull(4))
                                platformSetting.MessageManagementAdmin = reader.GetValue(4).ToString();
                            else
                                platformSetting.MessageManagementAdmin = null;
                            if (!reader.IsDBNull(5))
                                platformSetting.MessageManagementSendAdmin = reader.GetValue(5).ToString();
                            else
                                platformSetting.MessageManagementSendAdmin = null;
                            if (!reader.IsDBNull(6))
                                platformSetting.ConfigurationManagerAdmin = reader.GetValue(6).ToString();
                            else
                                platformSetting.ConfigurationManagerAdmin = null;
                            if (!reader.IsDBNull(7))
                                platformSetting.PasswordManagementAdmin = reader.GetValue(7).ToString();
                            else
                                platformSetting.PasswordManagementAdmin = null;
                            if (!reader.IsDBNull(8))
                                platformSetting.SchoolManagementAdminOre = reader.GetValue(8).ToString();
                            else
                                platformSetting.SchoolManagementAdminOre = null;
                            if (!reader.IsDBNull(9))
                                platformSetting.EnrollmentManagementAdminOre = reader.GetValue(9).ToString();
                            else
                                platformSetting.EnrollmentManagementAdminOre = null;
                            if (!reader.IsDBNull(10))
                                platformSetting.UserManagementAdminOre = reader.GetValue(10).ToString();
                            else
                                platformSetting.UserManagementAdminOre = null;
                            if (!reader.IsDBNull(11))
                                platformSetting.MessageManagementAdminOre = reader.GetValue(11).ToString();
                            else
                                platformSetting.MessageManagementAdminOre = null;
                            if (!reader.IsDBNull(12))
                                platformSetting.MessageManagementSendAdminOre = reader.GetValue(12).ToString();
                            else
                                platformSetting.MessageManagementSendAdminOre = null;
                            if (!reader.IsDBNull(13))
                                platformSetting.ConfigurationManagerAdminOre = reader.GetValue(13).ToString();
                            else
                                platformSetting.ConfigurationManagerAdminOre = null;
                            if (!reader.IsDBNull(14))
                                platformSetting.PasswordManagementAdminOre = reader.GetValue(14).ToString();
                            else
                                platformSetting.PasswordManagementAdminOre = null;
                            if (!reader.IsDBNull(15))
                                platformSetting.UserManagementPrincipal = reader.GetValue(15).ToString();
                            else
                                platformSetting.UserManagementPrincipal = null;
                            if (!reader.IsDBNull(16))
                                platformSetting.MessageManagementPrincipal = reader.GetValue(16).ToString();
                            else
                                platformSetting.MessageManagementPrincipal = null;
                            if (!reader.IsDBNull(17))
                                platformSetting.MessageManagementSendPrincipal = reader.GetValue(17).ToString();
                            else
                                platformSetting.MessageManagementSendPrincipal = null;
                            if (!reader.IsDBNull(18))
                                platformSetting.ConfigurationManagerPrincipal = reader.GetValue(18).ToString();
                            else
                                platformSetting.ConfigurationManagerPrincipal = null;
                            if (!reader.IsDBNull(19))
                                platformSetting.PasswordManagementPrincipal = reader.GetValue(19).ToString();
                            else
                                platformSetting.PasswordManagementPrincipal = null;
                            if (!reader.IsDBNull(20))
                                platformSetting.UserManagementTeacher = reader.GetValue(20).ToString();
                            else
                                platformSetting.UserManagementTeacher = null;
                            if (!reader.IsDBNull(21))
                                platformSetting.GroupInternalManagementTeacher = reader.GetValue(21).ToString();
                            else
                                platformSetting.GroupInternalManagementTeacher = null;
                            if (!reader.IsDBNull(22))
                                platformSetting.InvitationManagementTeacher = reader.GetValue(22).ToString();
                            else
                                platformSetting.InvitationManagementTeacher = null;
                            if (!reader.IsDBNull(23))
                                platformSetting.InvitationTeacherToParentManagementTeacher = reader.GetValue(23).ToString();
                            else
                                platformSetting.InvitationTeacherToParentManagementTeacher = null;
                            if (!reader.IsDBNull(24))
                                platformSetting.ChatManagementTeacher = reader.GetValue(24).ToString();
                            else
                                platformSetting.ChatManagementTeacher = null;
                            if (!reader.IsDBNull(25))
                                platformSetting.MessageManagementTeacher = reader.GetValue(25).ToString();
                            else
                                platformSetting.MessageManagementTeacher = null;
                            if (!reader.IsDBNull(26))
                                platformSetting.MessageManagementSendTeacher = reader.GetValue(26).ToString();
                            else
                                platformSetting.MessageManagementSendTeacher = null;
                            if (!reader.IsDBNull(27))
                                platformSetting.TeamMeetsManagementTeacher = reader.GetValue(27).ToString();
                            else
                                platformSetting.TeamMeetsManagementTeacher = null;
                            if (!reader.IsDBNull(28))
                                platformSetting.ConfigurationManagerTeacher = reader.GetValue(28).ToString();
                            else
                                platformSetting.ConfigurationManagerTeacher = null;
                            if (!reader.IsDBNull(29))
                                platformSetting.InvitationTeacherToStudentManagementTeacher = reader.GetValue(29).ToString();
                            else
                                platformSetting.InvitationTeacherToStudentManagementTeacher = null;
                            if (!reader.IsDBNull(30))
                                platformSetting.PasswordManagementTeacher = reader.GetValue(30).ToString();
                            else
                                platformSetting.PasswordManagementTeacher = null;
                            if (!reader.IsDBNull(31))
                                platformSetting.ProgressManagementParent = reader.GetValue(31).ToString();
                            else
                                platformSetting.ProgressManagementParent = null;
                            if (!reader.IsDBNull(32))
                                platformSetting.InvitationManagementParent = reader.GetValue(32).ToString();
                            else
                                platformSetting.InvitationManagementParent = null;
                            if (!reader.IsDBNull(33))
                                platformSetting.ChatManagementParent = reader.GetValue(33).ToString();
                            else
                                platformSetting.ChatManagementParent = null;
                            if (!reader.IsDBNull(34))
                                platformSetting.MessageManagementParent = reader.GetValue(34).ToString();
                            else
                                platformSetting.MessageManagementParent = null;
                            if (!reader.IsDBNull(35))
                                platformSetting.MessageManagementSendParent = reader.GetValue(35).ToString();
                            else
                                platformSetting.MessageManagementSendParent = null;
                            if (!reader.IsDBNull(36))
                                platformSetting.ConfigurationManagerParent = reader.GetValue(36).ToString();
                            else
                                platformSetting.ConfigurationManagerParent = null;
                            if (!reader.IsDBNull(37))
                                platformSetting.PasswordManagementParent = reader.GetValue(37).ToString();
                            else
                                platformSetting.PasswordManagementParent = null;
                            if (!reader.IsDBNull(38))
                                platformSetting.MessageManagementStudent = reader.GetValue(38).ToString();
                            else
                                platformSetting.MessageManagementStudent = null;
                            if (!reader.IsDBNull(39))
                                platformSetting.MessageManagementSendStudent = reader.GetValue(39).ToString();
                            else
                                platformSetting.MessageManagementSendStudent = null;
                            if (!reader.IsDBNull(40))
                                platformSetting.ProgressManagementStudent = reader.GetValue(40).ToString();
                            else
                                platformSetting.ProgressManagementStudent = null;
                            if (!reader.IsDBNull(41))
                                platformSetting.ChatManagementStudent = reader.GetValue(41).ToString();
                            else
                                platformSetting.ChatManagementStudent = null;
                            if (!reader.IsDBNull(42))
                                platformSetting.TeamsManagementStudent = reader.GetValue(42).ToString();
                            else
                                platformSetting.TeamsManagementStudent = null;
                            if (!reader.IsDBNull(43))
                                platformSetting.MicrosoftSSOManagement = reader.GetValue(43).ToString();
                            else
                                platformSetting.MicrosoftSSOManagement = null;
                            if (!reader.IsDBNull(44))
                                platformSetting.InternalLoginManagement = reader.GetValue(44).ToString();
                            else
                                platformSetting.InternalLoginManagement = null;
                            if (!reader.IsDBNull(45))
                                platformSetting.InternalPasswordReset = reader.GetValue(45).ToString();
                            else
                                platformSetting.InternalPasswordReset = null;
                            if (!reader.IsDBNull(46))
                                platformSetting.ModifiedBy = reader.GetValue(46).ToString();
                            else
                                platformSetting.ModifiedBy = null;
                            if (!reader.IsDBNull(47))
                                platformSetting.CreatedAt = reader.GetValue(47).ToString();
                            else
                                platformSetting.CreatedAt = null;
                            if (!reader.IsDBNull(48))
                                platformSetting.UpdatedAt = reader.GetValue(48).ToString();
                            else
                                platformSetting.UpdatedAt = null;
                            response.Add(platformSetting);
                            platformSetting = new PlatformSettingModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlatformSettingModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlatformSettingModel> Read(string connectionString, List<PlatformSettingModel> platformSettingWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformSettingModel> response = new List<PlatformSettingModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `school_management_admin`, `enrollment_management_admin`, `user_management_admin`, `message_management_admin`, `message_management_send_admin`, `configuration_manager_admin`, `password_management_admin`, `school_management_admin_ore`, `enrollment_management_admin_ore`, `user_management_admin_ore`, `message_management_admin_ore`, `message_management_send_admin_ore`, `configuration_manager_admin_ore`, `password_management_admin_ore`, `user_management_principal`, `message_management_principal`, `message_management_send_principal`, `configuration_manager_principal`, `password_management_principal`, `user_management_teacher`, `group_internal_management_teacher`, `invitation_management_teacher`, `invitation_teacher_to_parent_management_teacher`, `chat_management_teacher`, `message_management_teacher`, `message_management_send_teacher`, `team_meets_management_teacher`, `configuration_manager_teacher`, `invitation_teacher_to_student_management_teacher`, `password_management_teacher`, `progress_management_parent`, `invitation_management_parent`, `chat_management_parent`, `message_management_parent`, `message_management_send_parent`, `configuration_manager_parent`, `password_management_parent`, `message_management_student`, `message_management_send_student`, `progress_management_student`, `chat_management_student`, `teams_management_student`, `microsoft_sso_management`, `internal_login_management`, `internal_password_reset`, `modified_by`, `created_at`, `updated_at`  FROM platform_settings ";
                if (platformSettingWhere != null && platformSettingWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < platformSettingWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @Id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@Id" + i, platformSettingWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].SchoolManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `school_management_admin` = @SchoolManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdmin" + i, platformSettingWhere[i].SchoolManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].EnrollmentManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enrollment_management_admin` = @EnrollmentManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdmin" + i, platformSettingWhere[i].EnrollmentManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_admin` = @UserManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdmin" + i, platformSettingWhere[i].UserManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_admin` = @MessageManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdmin" + i, platformSettingWhere[i].MessageManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_admin` = @MessageManagementSendAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdmin" + i, platformSettingWhere[i].MessageManagementSendAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_admin` = @ConfigurationManagerAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdmin" + i, platformSettingWhere[i].ConfigurationManagerAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementAdmin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_admin` = @PasswordManagementAdmin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdmin" + i, platformSettingWhere[i].PasswordManagementAdmin);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].SchoolManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `school_management_admin_ore` = @SchoolManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@SchoolManagementAdminOre" + i, platformSettingWhere[i].SchoolManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].EnrollmentManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOre" + i, platformSettingWhere[i].EnrollmentManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_admin_ore` = @UserManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementAdminOre" + i, platformSettingWhere[i].UserManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_admin_ore` = @MessageManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementAdminOre" + i, platformSettingWhere[i].MessageManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_admin_ore` = @MessageManagementSendAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendAdminOre" + i, platformSettingWhere[i].MessageManagementSendAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOre" + i, platformSettingWhere[i].ConfigurationManagerAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementAdminOre))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_admin_ore` = @PasswordManagementAdminOre" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementAdminOre" + i, platformSettingWhere[i].PasswordManagementAdminOre);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_principal` = @UserManagementPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementPrincipal" + i, platformSettingWhere[i].UserManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_principal` = @MessageManagementPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementPrincipal" + i, platformSettingWhere[i].MessageManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_principal` = @MessageManagementSendPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendPrincipal" + i, platformSettingWhere[i].MessageManagementSendPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_principal` = @ConfigurationManagerPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipal" + i, platformSettingWhere[i].ConfigurationManagerPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementPrincipal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_principal` = @PasswordManagementPrincipal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementPrincipal" + i, platformSettingWhere[i].PasswordManagementPrincipal);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UserManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_management_teacher` = @UserManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UserManagementTeacher" + i, platformSettingWhere[i].UserManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].GroupInternalManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `group_internal_management_teacher` = @GroupInternalManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@GroupInternalManagementTeacher" + i, platformSettingWhere[i].GroupInternalManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_management_teacher` = @InvitationManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementTeacher" + i, platformSettingWhere[i].InvitationManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationTeacherToParentManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacher" + i, platformSettingWhere[i].InvitationTeacherToParentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ChatManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `chat_management_teacher` = @ChatManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ChatManagementTeacher" + i, platformSettingWhere[i].ChatManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_teacher` = @MessageManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementTeacher" + i, platformSettingWhere[i].MessageManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_teacher` = @MessageManagementSendTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendTeacher" + i, platformSettingWhere[i].MessageManagementSendTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].TeamMeetsManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `team_meets_management_teacher` = @TeamMeetsManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacher" + i, platformSettingWhere[i].TeamMeetsManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_teacher` = @ConfigurationManagerTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerTeacher" + i, platformSettingWhere[i].ConfigurationManagerTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationTeacherToStudentManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacher" + i, platformSettingWhere[i].InvitationTeacherToStudentManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_teacher` = @PasswordManagementTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementTeacher" + i, platformSettingWhere[i].PasswordManagementTeacher);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ProgressManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `progress_management_parent` = @ProgressManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementParent" + i, platformSettingWhere[i].ProgressManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InvitationManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invitation_management_parent` = @InvitationManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InvitationManagementParent" + i, platformSettingWhere[i].InvitationManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ChatManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `chat_management_parent` = @ChatManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ChatManagementParent" + i, platformSettingWhere[i].ChatManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_parent` = @MessageManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementParent" + i, platformSettingWhere[i].MessageManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_parent` = @MessageManagementSendParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendParent" + i, platformSettingWhere[i].MessageManagementSendParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ConfigurationManagerParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `configuration_manager_parent` = @ConfigurationManagerParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ConfigurationManagerParent" + i, platformSettingWhere[i].ConfigurationManagerParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].PasswordManagementParent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `password_management_parent` = @PasswordManagementParent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@PasswordManagementParent" + i, platformSettingWhere[i].PasswordManagementParent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_student` = @MessageManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementStudent" + i, platformSettingWhere[i].MessageManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MessageManagementSendStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_management_send_student` = @MessageManagementSendStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MessageManagementSendStudent" + i, platformSettingWhere[i].MessageManagementSendStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ProgressManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `progress_management_student` = @ProgressManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ProgressManagementStudent" + i, platformSettingWhere[i].ProgressManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ChatManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `chat_management_student` = @ChatManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ChatManagementStudent" + i, platformSettingWhere[i].ChatManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].TeamsManagementStudent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teams_management_student` = @TeamsManagementStudent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@TeamsManagementStudent" + i, platformSettingWhere[i].TeamsManagementStudent);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].MicrosoftSSOManagement))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `microsoft_sso_management` = @MicrosoftSSOManagement" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@MicrosoftSSOManagement" + i, platformSettingWhere[i].MicrosoftSSOManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InternalLoginManagement))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `internal_login_management` = @InternalLoginManagement" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InternalLoginManagement" + i, platformSettingWhere[i].InternalLoginManagement);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].InternalPasswordReset))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `internal_password_reset` = @InternalPasswordReset" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@InternalPasswordReset" + i, platformSettingWhere[i].InternalPasswordReset);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @ModifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ModifiedBy" + i, platformSettingWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @CreatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@CreatedAt" + i, platformSettingWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(platformSettingWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @UpdatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@UpdatedAt" + i, platformSettingWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != platformSettingWhere.Count - 1)
                            sqlIn += " OR ";
                    }
                }
                cmd.CommandText = sql + " " + sqlIn + " LIMIT @fromRow, @limit ";
                cmd.Parameters.AddWithValue("@fromRow", fromRow);
                cmd.Parameters.AddWithValue("@limit", limit);
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            PlatformSettingModel platformSetting = new PlatformSettingModel(); if (!reader.IsDBNull(0))
                                platformSetting.Id = reader.GetValue(0).ToString();
                            else
                                platformSetting.Id = null;
                            if (!reader.IsDBNull(1))
                                platformSetting.SchoolManagementAdmin = reader.GetValue(1).ToString();
                            else
                                platformSetting.SchoolManagementAdmin = null;
                            if (!reader.IsDBNull(2))
                                platformSetting.EnrollmentManagementAdmin = reader.GetValue(2).ToString();
                            else
                                platformSetting.EnrollmentManagementAdmin = null;
                            if (!reader.IsDBNull(3))
                                platformSetting.UserManagementAdmin = reader.GetValue(3).ToString();
                            else
                                platformSetting.UserManagementAdmin = null;
                            if (!reader.IsDBNull(4))
                                platformSetting.MessageManagementAdmin = reader.GetValue(4).ToString();
                            else
                                platformSetting.MessageManagementAdmin = null;
                            if (!reader.IsDBNull(5))
                                platformSetting.MessageManagementSendAdmin = reader.GetValue(5).ToString();
                            else
                                platformSetting.MessageManagementSendAdmin = null;
                            if (!reader.IsDBNull(6))
                                platformSetting.ConfigurationManagerAdmin = reader.GetValue(6).ToString();
                            else
                                platformSetting.ConfigurationManagerAdmin = null;
                            if (!reader.IsDBNull(7))
                                platformSetting.PasswordManagementAdmin = reader.GetValue(7).ToString();
                            else
                                platformSetting.PasswordManagementAdmin = null;
                            if (!reader.IsDBNull(8))
                                platformSetting.SchoolManagementAdminOre = reader.GetValue(8).ToString();
                            else
                                platformSetting.SchoolManagementAdminOre = null;
                            if (!reader.IsDBNull(9))
                                platformSetting.EnrollmentManagementAdminOre = reader.GetValue(9).ToString();
                            else
                                platformSetting.EnrollmentManagementAdminOre = null;
                            if (!reader.IsDBNull(10))
                                platformSetting.UserManagementAdminOre = reader.GetValue(10).ToString();
                            else
                                platformSetting.UserManagementAdminOre = null;
                            if (!reader.IsDBNull(11))
                                platformSetting.MessageManagementAdminOre = reader.GetValue(11).ToString();
                            else
                                platformSetting.MessageManagementAdminOre = null;
                            if (!reader.IsDBNull(12))
                                platformSetting.MessageManagementSendAdminOre = reader.GetValue(12).ToString();
                            else
                                platformSetting.MessageManagementSendAdminOre = null;
                            if (!reader.IsDBNull(13))
                                platformSetting.ConfigurationManagerAdminOre = reader.GetValue(13).ToString();
                            else
                                platformSetting.ConfigurationManagerAdminOre = null;
                            if (!reader.IsDBNull(14))
                                platformSetting.PasswordManagementAdminOre = reader.GetValue(14).ToString();
                            else
                                platformSetting.PasswordManagementAdminOre = null;
                            if (!reader.IsDBNull(15))
                                platformSetting.UserManagementPrincipal = reader.GetValue(15).ToString();
                            else
                                platformSetting.UserManagementPrincipal = null;
                            if (!reader.IsDBNull(16))
                                platformSetting.MessageManagementPrincipal = reader.GetValue(16).ToString();
                            else
                                platformSetting.MessageManagementPrincipal = null;
                            if (!reader.IsDBNull(17))
                                platformSetting.MessageManagementSendPrincipal = reader.GetValue(17).ToString();
                            else
                                platformSetting.MessageManagementSendPrincipal = null;
                            if (!reader.IsDBNull(18))
                                platformSetting.ConfigurationManagerPrincipal = reader.GetValue(18).ToString();
                            else
                                platformSetting.ConfigurationManagerPrincipal = null;
                            if (!reader.IsDBNull(19))
                                platformSetting.PasswordManagementPrincipal = reader.GetValue(19).ToString();
                            else
                                platformSetting.PasswordManagementPrincipal = null;
                            if (!reader.IsDBNull(20))
                                platformSetting.UserManagementTeacher = reader.GetValue(20).ToString();
                            else
                                platformSetting.UserManagementTeacher = null;
                            if (!reader.IsDBNull(21))
                                platformSetting.GroupInternalManagementTeacher = reader.GetValue(21).ToString();
                            else
                                platformSetting.GroupInternalManagementTeacher = null;
                            if (!reader.IsDBNull(22))
                                platformSetting.InvitationManagementTeacher = reader.GetValue(22).ToString();
                            else
                                platformSetting.InvitationManagementTeacher = null;
                            if (!reader.IsDBNull(23))
                                platformSetting.InvitationTeacherToParentManagementTeacher = reader.GetValue(23).ToString();
                            else
                                platformSetting.InvitationTeacherToParentManagementTeacher = null;
                            if (!reader.IsDBNull(24))
                                platformSetting.ChatManagementTeacher = reader.GetValue(24).ToString();
                            else
                                platformSetting.ChatManagementTeacher = null;
                            if (!reader.IsDBNull(25))
                                platformSetting.MessageManagementTeacher = reader.GetValue(25).ToString();
                            else
                                platformSetting.MessageManagementTeacher = null;
                            if (!reader.IsDBNull(26))
                                platformSetting.MessageManagementSendTeacher = reader.GetValue(26).ToString();
                            else
                                platformSetting.MessageManagementSendTeacher = null;
                            if (!reader.IsDBNull(27))
                                platformSetting.TeamMeetsManagementTeacher = reader.GetValue(27).ToString();
                            else
                                platformSetting.TeamMeetsManagementTeacher = null;
                            if (!reader.IsDBNull(28))
                                platformSetting.ConfigurationManagerTeacher = reader.GetValue(28).ToString();
                            else
                                platformSetting.ConfigurationManagerTeacher = null;
                            if (!reader.IsDBNull(29))
                                platformSetting.InvitationTeacherToStudentManagementTeacher = reader.GetValue(29).ToString();
                            else
                                platformSetting.InvitationTeacherToStudentManagementTeacher = null;
                            if (!reader.IsDBNull(30))
                                platformSetting.PasswordManagementTeacher = reader.GetValue(30).ToString();
                            else
                                platformSetting.PasswordManagementTeacher = null;
                            if (!reader.IsDBNull(31))
                                platformSetting.ProgressManagementParent = reader.GetValue(31).ToString();
                            else
                                platformSetting.ProgressManagementParent = null;
                            if (!reader.IsDBNull(32))
                                platformSetting.InvitationManagementParent = reader.GetValue(32).ToString();
                            else
                                platformSetting.InvitationManagementParent = null;
                            if (!reader.IsDBNull(33))
                                platformSetting.ChatManagementParent = reader.GetValue(33).ToString();
                            else
                                platformSetting.ChatManagementParent = null;
                            if (!reader.IsDBNull(34))
                                platformSetting.MessageManagementParent = reader.GetValue(34).ToString();
                            else
                                platformSetting.MessageManagementParent = null;
                            if (!reader.IsDBNull(35))
                                platformSetting.MessageManagementSendParent = reader.GetValue(35).ToString();
                            else
                                platformSetting.MessageManagementSendParent = null;
                            if (!reader.IsDBNull(36))
                                platformSetting.ConfigurationManagerParent = reader.GetValue(36).ToString();
                            else
                                platformSetting.ConfigurationManagerParent = null;
                            if (!reader.IsDBNull(37))
                                platformSetting.PasswordManagementParent = reader.GetValue(37).ToString();
                            else
                                platformSetting.PasswordManagementParent = null;
                            if (!reader.IsDBNull(38))
                                platformSetting.MessageManagementStudent = reader.GetValue(38).ToString();
                            else
                                platformSetting.MessageManagementStudent = null;
                            if (!reader.IsDBNull(39))
                                platformSetting.MessageManagementSendStudent = reader.GetValue(39).ToString();
                            else
                                platformSetting.MessageManagementSendStudent = null;
                            if (!reader.IsDBNull(40))
                                platformSetting.ProgressManagementStudent = reader.GetValue(40).ToString();
                            else
                                platformSetting.ProgressManagementStudent = null;
                            if (!reader.IsDBNull(41))
                                platformSetting.ChatManagementStudent = reader.GetValue(41).ToString();
                            else
                                platformSetting.ChatManagementStudent = null;
                            if (!reader.IsDBNull(42))
                                platformSetting.TeamsManagementStudent = reader.GetValue(42).ToString();
                            else
                                platformSetting.TeamsManagementStudent = null;
                            if (!reader.IsDBNull(43))
                                platformSetting.MicrosoftSSOManagement = reader.GetValue(43).ToString();
                            else
                                platformSetting.MicrosoftSSOManagement = null;
                            if (!reader.IsDBNull(44))
                                platformSetting.InternalLoginManagement = reader.GetValue(44).ToString();
                            else
                                platformSetting.InternalLoginManagement = null;
                            if (!reader.IsDBNull(45))
                                platformSetting.InternalPasswordReset = reader.GetValue(45).ToString();
                            else
                                platformSetting.InternalPasswordReset = null;
                            if (!reader.IsDBNull(46))
                                platformSetting.ModifiedBy = reader.GetValue(46).ToString();
                            else
                                platformSetting.ModifiedBy = null;
                            if (!reader.IsDBNull(47))
                                platformSetting.CreatedAt = reader.GetValue(47).ToString();
                            else
                                platformSetting.CreatedAt = null;
                            if (!reader.IsDBNull(48))
                                platformSetting.UpdatedAt = reader.GetValue(48).ToString();
                            else
                                platformSetting.UpdatedAt = null;
                            response.Add(platformSetting);
                            platformSetting = new PlatformSettingModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlatformSettingModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlatformSettingModel> Create(string connectionString, PlatformSettingModel platformSetting)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformSettingModel> response = new List<PlatformSettingModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO platform_settings (";
                string values = "(";
                if (!String.IsNullOrEmpty(platformSetting.Id))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `id` ");
                        values = String.Concat(values, ", @Id");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `id` ");
                        values = String.Concat(values, " @Id");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@Id", platformSetting.Id);
                }
                if (!String.IsNullOrEmpty(platformSetting.SchoolManagementAdmin))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `school_management_admin` ");
                        values = String.Concat(values, ", @SchoolManagementAdmin");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `school_management_admin` ");
                        values = String.Concat(values, " @SchoolManagementAdmin");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@SchoolManagementAdmin", platformSetting.SchoolManagementAdmin);
                }
                if (!String.IsNullOrEmpty(platformSetting.EnrollmentManagementAdmin))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `enrollment_management_admin` ");
                        values = String.Concat(values, ", @EnrollmentManagementAdmin");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `enrollment_management_admin` ");
                        values = String.Concat(values, " @EnrollmentManagementAdmin");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@EnrollmentManagementAdmin", platformSetting.EnrollmentManagementAdmin);
                }
                if (!String.IsNullOrEmpty(platformSetting.UserManagementAdmin))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `user_management_admin` ");
                        values = String.Concat(values, ", @UserManagementAdmin");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `user_management_admin` ");
                        values = String.Concat(values, " @UserManagementAdmin");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@UserManagementAdmin", platformSetting.UserManagementAdmin);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementAdmin))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `message_management_admin` ");
                        values = String.Concat(values, ", @MessageManagementAdmin");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `message_management_admin` ");
                        values = String.Concat(values, " @MessageManagementAdmin");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementAdmin", platformSetting.MessageManagementAdmin);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendAdmin))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `message_management_send_admin` ");
                        values = String.Concat(values, ", @MessageManagementSendAdmin");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `message_management_send_admin` ");
                        values = String.Concat(values, " @MessageManagementSendAdmin");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementSendAdmin", platformSetting.MessageManagementSendAdmin);
                }
                if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerAdmin))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `configuration_manager_admin` ");
                        values = String.Concat(values, ", @ConfigurationManagerAdmin");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `configuration_manager_admin` ");
                        values = String.Concat(values, " @ConfigurationManagerAdmin");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ConfigurationManagerAdmin", platformSetting.ConfigurationManagerAdmin);
                }
                if (!String.IsNullOrEmpty(platformSetting.PasswordManagementAdmin))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `password_management_admin` ");
                        values = String.Concat(values, ", @PasswordManagementAdmin");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `password_management_admin` ");
                        values = String.Concat(values, " @PasswordManagementAdmin");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@PasswordManagementAdmin", platformSetting.PasswordManagementAdmin);
                }
                if (!String.IsNullOrEmpty(platformSetting.SchoolManagementAdminOre))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `school_management_admin_ore` ");
                        values = String.Concat(values, ", @SchoolManagementAdminOre");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `school_management_admin_ore` ");
                        values = String.Concat(values, " @SchoolManagementAdminOre");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@SchoolManagementAdminOre", platformSetting.SchoolManagementAdminOre);
                }
                if (!String.IsNullOrEmpty(platformSetting.EnrollmentManagementAdminOre))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `enrollment_management_admin_ore` ");
                        values = String.Concat(values, ", @EnrollmentManagementAdminOre");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `enrollment_management_admin_ore` ");
                        values = String.Concat(values, " @EnrollmentManagementAdminOre");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOre", platformSetting.EnrollmentManagementAdminOre);
                }
                if (!String.IsNullOrEmpty(platformSetting.UserManagementAdminOre))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `user_management_admin_ore` ");
                        values = String.Concat(values, ", @UserManagementAdminOre");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `user_management_admin_ore` ");
                        values = String.Concat(values, " @UserManagementAdminOre");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@UserManagementAdminOre", platformSetting.UserManagementAdminOre);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementAdminOre))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `message_management_admin_ore` ");
                        values = String.Concat(values, ", @MessageManagementAdminOre");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `message_management_admin_ore` ");
                        values = String.Concat(values, " @MessageManagementAdminOre");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementAdminOre", platformSetting.MessageManagementAdminOre);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendAdminOre))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `message_management_send_admin_ore` ");
                        values = String.Concat(values, ", @MessageManagementSendAdminOre");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `message_management_send_admin_ore` ");
                        values = String.Concat(values, " @MessageManagementSendAdminOre");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementSendAdminOre", platformSetting.MessageManagementSendAdminOre);
                }
                if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerAdminOre))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `configuration_manager_admin_ore` ");
                        values = String.Concat(values, ", @ConfigurationManagerAdminOre");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `configuration_manager_admin_ore` ");
                        values = String.Concat(values, " @ConfigurationManagerAdminOre");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOre", platformSetting.ConfigurationManagerAdminOre);
                }
                if (!String.IsNullOrEmpty(platformSetting.PasswordManagementAdminOre))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `password_management_admin_ore` ");
                        values = String.Concat(values, ", @PasswordManagementAdminOre");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `password_management_admin_ore` ");
                        values = String.Concat(values, " @PasswordManagementAdminOre");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@PasswordManagementAdminOre", platformSetting.PasswordManagementAdminOre);
                }
                if (!String.IsNullOrEmpty(platformSetting.UserManagementPrincipal))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `user_management_principal` ");
                        values = String.Concat(values, ", @UserManagementPrincipal");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `user_management_principal` ");
                        values = String.Concat(values, " @UserManagementPrincipal");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@UserManagementPrincipal", platformSetting.UserManagementPrincipal);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementPrincipal))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `message_management_principal` ");
                        values = String.Concat(values, ", @MessageManagementPrincipal");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `message_management_principal` ");
                        values = String.Concat(values, " @MessageManagementPrincipal");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementPrincipal", platformSetting.MessageManagementPrincipal);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendPrincipal))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `message_management_send_principal` ");
                        values = String.Concat(values, ", @MessageManagementSendPrincipal");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `message_management_send_principal` ");
                        values = String.Concat(values, " @MessageManagementSendPrincipal");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementSendPrincipal", platformSetting.MessageManagementSendPrincipal);
                }
                if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerPrincipal))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `configuration_manager_principal` ");
                        values = String.Concat(values, ", @ConfigurationManagerPrincipal");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `configuration_manager_principal` ");
                        values = String.Concat(values, " @ConfigurationManagerPrincipal");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipal", platformSetting.ConfigurationManagerPrincipal);
                }
                if (!String.IsNullOrEmpty(platformSetting.PasswordManagementPrincipal))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `password_management_principal` ");
                        values = String.Concat(values, ", @PasswordManagementPrincipal");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `password_management_principal` ");
                        values = String.Concat(values, " @PasswordManagementPrincipal");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@PasswordManagementPrincipal", platformSetting.PasswordManagementPrincipal);
                }
                if (!String.IsNullOrEmpty(platformSetting.UserManagementTeacher))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `user_management_teacher` ");
                        values = String.Concat(values, ", @UserManagementTeacher");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `user_management_teacher` ");
                        values = String.Concat(values, " @UserManagementTeacher");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@UserManagementTeacher", platformSetting.UserManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.GroupInternalManagementTeacher))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `group_internal_management_teacher` ");
                        values = String.Concat(values, ", @GroupInternalManagementTeacher");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `group_internal_management_teacher` ");
                        values = String.Concat(values, " @GroupInternalManagementTeacher");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@GroupInternalManagementTeacher", platformSetting.GroupInternalManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.InvitationManagementTeacher))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `invitation_management_teacher` ");
                        values = String.Concat(values, ", @InvitationManagementTeacher");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `invitation_management_teacher` ");
                        values = String.Concat(values, " @InvitationManagementTeacher");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@InvitationManagementTeacher", platformSetting.InvitationManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.InvitationTeacherToParentManagementTeacher))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `invitation_teacher_to_parent_management_teacher` ");
                        values = String.Concat(values, ", @InvitationTeacherToParentManagementTeacher");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `invitation_teacher_to_parent_management_teacher` ");
                        values = String.Concat(values, " @InvitationTeacherToParentManagementTeacher");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacher", platformSetting.InvitationTeacherToParentManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.ChatManagementTeacher))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `chat_management_teacher` ");
                        values = String.Concat(values, ", @ChatManagementTeacher");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `chat_management_teacher` ");
                        values = String.Concat(values, " @ChatManagementTeacher");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ChatManagementTeacher", platformSetting.ChatManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementTeacher))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `message_management_teacher` ");
                        values = String.Concat(values, ", @MessageManagementTeacher");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `message_management_teacher` ");
                        values = String.Concat(values, " @MessageManagementTeacher");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementTeacher", platformSetting.MessageManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendTeacher))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `message_management_send_teacher` ");
                        values = String.Concat(values, ", @MessageManagementSendTeacher");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `message_management_send_teacher` ");
                        values = String.Concat(values, " @MessageManagementSendTeacher");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementSendTeacher", platformSetting.MessageManagementSendTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.TeamMeetsManagementTeacher))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `team_meets_management_teacher` ");
                        values = String.Concat(values, ", @TeamMeetsManagementTeacher");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `team_meets_management_teacher` ");
                        values = String.Concat(values, " @TeamMeetsManagementTeacher");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacher", platformSetting.TeamMeetsManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerTeacher))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `configuration_manager_teacher` ");
                        values = String.Concat(values, ", @ConfigurationManagerTeacher");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `configuration_manager_teacher` ");
                        values = String.Concat(values, " @ConfigurationManagerTeacher");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ConfigurationManagerTeacher", platformSetting.ConfigurationManagerTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.InvitationTeacherToStudentManagementTeacher))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `invitation_teacher_to_student_management_teacher` ");
                        values = String.Concat(values, ", @InvitationTeacherToStudentManagementTeacher");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `invitation_teacher_to_student_management_teacher` ");
                        values = String.Concat(values, " @InvitationTeacherToStudentManagementTeacher");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacher", platformSetting.InvitationTeacherToStudentManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.PasswordManagementTeacher))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `password_management_teacher` ");
                        values = String.Concat(values, ", @PasswordManagementTeacher");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `password_management_teacher` ");
                        values = String.Concat(values, " @PasswordManagementTeacher");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@PasswordManagementTeacher", platformSetting.PasswordManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.ProgressManagementParent))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `progress_management_parent` ");
                        values = String.Concat(values, ", @ProgressManagementParent");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `progress_management_parent` ");
                        values = String.Concat(values, " @ProgressManagementParent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ProgressManagementParent", platformSetting.ProgressManagementParent);
                }
                if (!String.IsNullOrEmpty(platformSetting.InvitationManagementParent))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `invitation_management_parent` ");
                        values = String.Concat(values, ", @InvitationManagementParent");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `invitation_management_parent` ");
                        values = String.Concat(values, " @InvitationManagementParent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@InvitationManagementParent", platformSetting.InvitationManagementParent);
                }
                if (!String.IsNullOrEmpty(platformSetting.ChatManagementParent))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `chat_management_parent` ");
                        values = String.Concat(values, ", @ChatManagementParent");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `chat_management_parent` ");
                        values = String.Concat(values, " @ChatManagementParent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ChatManagementParent", platformSetting.ChatManagementParent);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementParent))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `message_management_parent` ");
                        values = String.Concat(values, ", @MessageManagementParent");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `message_management_parent` ");
                        values = String.Concat(values, " @MessageManagementParent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementParent", platformSetting.MessageManagementParent);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendParent))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `message_management_send_parent` ");
                        values = String.Concat(values, ", @MessageManagementSendParent");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `message_management_send_parent` ");
                        values = String.Concat(values, " @MessageManagementSendParent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementSendParent", platformSetting.MessageManagementSendParent);
                }
                if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerParent))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `configuration_manager_parent` ");
                        values = String.Concat(values, ", @ConfigurationManagerParent");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `configuration_manager_parent` ");
                        values = String.Concat(values, " @ConfigurationManagerParent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ConfigurationManagerParent", platformSetting.ConfigurationManagerParent);
                }
                if (!String.IsNullOrEmpty(platformSetting.PasswordManagementParent))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `password_management_parent` ");
                        values = String.Concat(values, ", @PasswordManagementParent");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `password_management_parent` ");
                        values = String.Concat(values, " @PasswordManagementParent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@PasswordManagementParent", platformSetting.PasswordManagementParent);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementStudent))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `message_management_student` ");
                        values = String.Concat(values, ", @MessageManagementStudent");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `message_management_student` ");
                        values = String.Concat(values, " @MessageManagementStudent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementStudent", platformSetting.MessageManagementStudent);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendStudent))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `message_management_send_student` ");
                        values = String.Concat(values, ", @MessageManagementSendStudent");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `message_management_send_student` ");
                        values = String.Concat(values, " @MessageManagementSendStudent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementSendStudent", platformSetting.MessageManagementSendStudent);
                }
                if (!String.IsNullOrEmpty(platformSetting.ProgressManagementStudent))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `progress_management_student` ");
                        values = String.Concat(values, ", @ProgressManagementStudent");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `progress_management_student` ");
                        values = String.Concat(values, " @ProgressManagementStudent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ProgressManagementStudent", platformSetting.ProgressManagementStudent);
                }
                if (!String.IsNullOrEmpty(platformSetting.ChatManagementStudent))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `chat_management_student` ");
                        values = String.Concat(values, ", @ChatManagementStudent");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `chat_management_student` ");
                        values = String.Concat(values, " @ChatManagementStudent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ChatManagementStudent", platformSetting.ChatManagementStudent);
                }
                if (!String.IsNullOrEmpty(platformSetting.TeamsManagementStudent))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `teams_management_student` ");
                        values = String.Concat(values, ", @TeamsManagementStudent");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `teams_management_student` ");
                        values = String.Concat(values, " @TeamsManagementStudent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@TeamsManagementStudent", platformSetting.TeamsManagementStudent);
                }
                if (!String.IsNullOrEmpty(platformSetting.MicrosoftSSOManagement))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `microsoft_sso_management` ");
                        values = String.Concat(values, ", @MicrosoftSSOManagement");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `microsoft_sso_management` ");
                        values = String.Concat(values, " @MicrosoftSSOManagement");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MicrosoftSSOManagement", platformSetting.MicrosoftSSOManagement);
                }
                if (!String.IsNullOrEmpty(platformSetting.InternalLoginManagement))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `internal_login_management` ");
                        values = String.Concat(values, ", @InternalLoginManagement");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `internal_login_management` ");
                        values = String.Concat(values, " @InternalLoginManagement");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@InternalLoginManagement", platformSetting.InternalLoginManagement);
                }
                if (!String.IsNullOrEmpty(platformSetting.InternalPasswordReset))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `internal_password_reset` ");
                        values = String.Concat(values, ", @InternalPasswordReset");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `internal_password_reset` ");
                        values = String.Concat(values, " @InternalPasswordReset");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@InternalPasswordReset", platformSetting.InternalPasswordReset);
                }
                if (!String.IsNullOrEmpty(platformSetting.ModifiedBy))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `modified_by` ");
                        values = String.Concat(values, ", @ModifiedBy");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `modified_by` ");
                        values = String.Concat(values, " @ModifiedBy");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ModifiedBy", platformSetting.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(platformSetting.CreatedAt))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `created_at` ");
                        values = String.Concat(values, ", @CreatedAt");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `created_at` ");
                        values = String.Concat(values, " @CreatedAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@CreatedAt", platformSetting.CreatedAt);
                }
                if (!String.IsNullOrEmpty(platformSetting.UpdatedAt))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `updated_at` ");
                        values = String.Concat(values, ", @UpdatedAt");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `updated_at` ");
                        values = String.Concat(values, " @UpdatedAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@UpdatedAt", platformSetting.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                response.Add(platformSetting);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformSettingModel>();
            }
            finally
            {
                if (_connection != null)
                {
                    dbConn.CloseConnection(_connection);
                }
            }
            return response;
        }
        public List<PlatformSettingModel> Create(string connectionString, List<PlatformSettingModel> platformSettingList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformSettingModel> response = new List<PlatformSettingModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < platformSettingList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO platform_settings (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(platformSettingList[i].Id))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `id` ");
                                        values = String.Concat(values, ", @Id" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `id` ");
                                        values = String.Concat(values, "@Id" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@Id" + i, platformSettingList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].SchoolManagementAdmin))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `school_management_admin` ");
                                        values = String.Concat(values, ", @SchoolManagementAdmin" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `school_management_admin` ");
                                        values = String.Concat(values, "@SchoolManagementAdmin" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@SchoolManagementAdmin" + i, platformSettingList[i].SchoolManagementAdmin);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].EnrollmentManagementAdmin))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `enrollment_management_admin` ");
                                        values = String.Concat(values, ", @EnrollmentManagementAdmin" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `enrollment_management_admin` ");
                                        values = String.Concat(values, "@EnrollmentManagementAdmin" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@EnrollmentManagementAdmin" + i, platformSettingList[i].EnrollmentManagementAdmin);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].UserManagementAdmin))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `user_management_admin` ");
                                        values = String.Concat(values, ", @UserManagementAdmin" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `user_management_admin` ");
                                        values = String.Concat(values, "@UserManagementAdmin" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@UserManagementAdmin" + i, platformSettingList[i].UserManagementAdmin);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementAdmin))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `message_management_admin` ");
                                        values = String.Concat(values, ", @MessageManagementAdmin" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `message_management_admin` ");
                                        values = String.Concat(values, "@MessageManagementAdmin" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementAdmin" + i, platformSettingList[i].MessageManagementAdmin);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementSendAdmin))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `message_management_send_admin` ");
                                        values = String.Concat(values, ", @MessageManagementSendAdmin" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `message_management_send_admin` ");
                                        values = String.Concat(values, "@MessageManagementSendAdmin" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementSendAdmin" + i, platformSettingList[i].MessageManagementSendAdmin);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ConfigurationManagerAdmin))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `configuration_manager_admin` ");
                                        values = String.Concat(values, ", @ConfigurationManagerAdmin" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `configuration_manager_admin` ");
                                        values = String.Concat(values, "@ConfigurationManagerAdmin" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ConfigurationManagerAdmin" + i, platformSettingList[i].ConfigurationManagerAdmin);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].PasswordManagementAdmin))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `password_management_admin` ");
                                        values = String.Concat(values, ", @PasswordManagementAdmin" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `password_management_admin` ");
                                        values = String.Concat(values, "@PasswordManagementAdmin" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@PasswordManagementAdmin" + i, platformSettingList[i].PasswordManagementAdmin);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].SchoolManagementAdminOre))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `school_management_admin_ore` ");
                                        values = String.Concat(values, ", @SchoolManagementAdminOre" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `school_management_admin_ore` ");
                                        values = String.Concat(values, "@SchoolManagementAdminOre" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@SchoolManagementAdminOre" + i, platformSettingList[i].SchoolManagementAdminOre);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].EnrollmentManagementAdminOre))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `enrollment_management_admin_ore` ");
                                        values = String.Concat(values, ", @EnrollmentManagementAdminOre" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `enrollment_management_admin_ore` ");
                                        values = String.Concat(values, "@EnrollmentManagementAdminOre" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOre" + i, platformSettingList[i].EnrollmentManagementAdminOre);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].UserManagementAdminOre))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `user_management_admin_ore` ");
                                        values = String.Concat(values, ", @UserManagementAdminOre" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `user_management_admin_ore` ");
                                        values = String.Concat(values, "@UserManagementAdminOre" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@UserManagementAdminOre" + i, platformSettingList[i].UserManagementAdminOre);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementAdminOre))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `message_management_admin_ore` ");
                                        values = String.Concat(values, ", @MessageManagementAdminOre" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `message_management_admin_ore` ");
                                        values = String.Concat(values, "@MessageManagementAdminOre" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementAdminOre" + i, platformSettingList[i].MessageManagementAdminOre);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementSendAdminOre))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `message_management_send_admin_ore` ");
                                        values = String.Concat(values, ", @MessageManagementSendAdminOre" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `message_management_send_admin_ore` ");
                                        values = String.Concat(values, "@MessageManagementSendAdminOre" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementSendAdminOre" + i, platformSettingList[i].MessageManagementSendAdminOre);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ConfigurationManagerAdminOre))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `configuration_manager_admin_ore` ");
                                        values = String.Concat(values, ", @ConfigurationManagerAdminOre" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `configuration_manager_admin_ore` ");
                                        values = String.Concat(values, "@ConfigurationManagerAdminOre" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOre" + i, platformSettingList[i].ConfigurationManagerAdminOre);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].PasswordManagementAdminOre))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `password_management_admin_ore` ");
                                        values = String.Concat(values, ", @PasswordManagementAdminOre" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `password_management_admin_ore` ");
                                        values = String.Concat(values, "@PasswordManagementAdminOre" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@PasswordManagementAdminOre" + i, platformSettingList[i].PasswordManagementAdminOre);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].UserManagementPrincipal))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `user_management_principal` ");
                                        values = String.Concat(values, ", @UserManagementPrincipal" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `user_management_principal` ");
                                        values = String.Concat(values, "@UserManagementPrincipal" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@UserManagementPrincipal" + i, platformSettingList[i].UserManagementPrincipal);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementPrincipal))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `message_management_principal` ");
                                        values = String.Concat(values, ", @MessageManagementPrincipal" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `message_management_principal` ");
                                        values = String.Concat(values, "@MessageManagementPrincipal" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementPrincipal" + i, platformSettingList[i].MessageManagementPrincipal);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementSendPrincipal))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `message_management_send_principal` ");
                                        values = String.Concat(values, ", @MessageManagementSendPrincipal" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `message_management_send_principal` ");
                                        values = String.Concat(values, "@MessageManagementSendPrincipal" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementSendPrincipal" + i, platformSettingList[i].MessageManagementSendPrincipal);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ConfigurationManagerPrincipal))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `configuration_manager_principal` ");
                                        values = String.Concat(values, ", @ConfigurationManagerPrincipal" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `configuration_manager_principal` ");
                                        values = String.Concat(values, "@ConfigurationManagerPrincipal" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipal" + i, platformSettingList[i].ConfigurationManagerPrincipal);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].PasswordManagementPrincipal))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `password_management_principal` ");
                                        values = String.Concat(values, ", @PasswordManagementPrincipal" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `password_management_principal` ");
                                        values = String.Concat(values, "@PasswordManagementPrincipal" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@PasswordManagementPrincipal" + i, platformSettingList[i].PasswordManagementPrincipal);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].UserManagementTeacher))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `user_management_teacher` ");
                                        values = String.Concat(values, ", @UserManagementTeacher" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `user_management_teacher` ");
                                        values = String.Concat(values, "@UserManagementTeacher" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@UserManagementTeacher" + i, platformSettingList[i].UserManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].GroupInternalManagementTeacher))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `group_internal_management_teacher` ");
                                        values = String.Concat(values, ", @GroupInternalManagementTeacher" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `group_internal_management_teacher` ");
                                        values = String.Concat(values, "@GroupInternalManagementTeacher" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@GroupInternalManagementTeacher" + i, platformSettingList[i].GroupInternalManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].InvitationManagementTeacher))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `invitation_management_teacher` ");
                                        values = String.Concat(values, ", @InvitationManagementTeacher" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `invitation_management_teacher` ");
                                        values = String.Concat(values, "@InvitationManagementTeacher" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@InvitationManagementTeacher" + i, platformSettingList[i].InvitationManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].InvitationTeacherToParentManagementTeacher))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `invitation_teacher_to_parent_management_teacher` ");
                                        values = String.Concat(values, ", @InvitationTeacherToParentManagementTeacher" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `invitation_teacher_to_parent_management_teacher` ");
                                        values = String.Concat(values, "@InvitationTeacherToParentManagementTeacher" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacher" + i, platformSettingList[i].InvitationTeacherToParentManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ChatManagementTeacher))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `chat_management_teacher` ");
                                        values = String.Concat(values, ", @ChatManagementTeacher" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `chat_management_teacher` ");
                                        values = String.Concat(values, "@ChatManagementTeacher" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ChatManagementTeacher" + i, platformSettingList[i].ChatManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementTeacher))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `message_management_teacher` ");
                                        values = String.Concat(values, ", @MessageManagementTeacher" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `message_management_teacher` ");
                                        values = String.Concat(values, "@MessageManagementTeacher" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementTeacher" + i, platformSettingList[i].MessageManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementSendTeacher))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `message_management_send_teacher` ");
                                        values = String.Concat(values, ", @MessageManagementSendTeacher" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `message_management_send_teacher` ");
                                        values = String.Concat(values, "@MessageManagementSendTeacher" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementSendTeacher" + i, platformSettingList[i].MessageManagementSendTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].TeamMeetsManagementTeacher))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `team_meets_management_teacher` ");
                                        values = String.Concat(values, ", @TeamMeetsManagementTeacher" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `team_meets_management_teacher` ");
                                        values = String.Concat(values, "@TeamMeetsManagementTeacher" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacher" + i, platformSettingList[i].TeamMeetsManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ConfigurationManagerTeacher))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `configuration_manager_teacher` ");
                                        values = String.Concat(values, ", @ConfigurationManagerTeacher" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `configuration_manager_teacher` ");
                                        values = String.Concat(values, "@ConfigurationManagerTeacher" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ConfigurationManagerTeacher" + i, platformSettingList[i].ConfigurationManagerTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].InvitationTeacherToStudentManagementTeacher))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `invitation_teacher_to_student_management_teacher` ");
                                        values = String.Concat(values, ", @InvitationTeacherToStudentManagementTeacher" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `invitation_teacher_to_student_management_teacher` ");
                                        values = String.Concat(values, "@InvitationTeacherToStudentManagementTeacher" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacher" + i, platformSettingList[i].InvitationTeacherToStudentManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].PasswordManagementTeacher))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `password_management_teacher` ");
                                        values = String.Concat(values, ", @PasswordManagementTeacher" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `password_management_teacher` ");
                                        values = String.Concat(values, "@PasswordManagementTeacher" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@PasswordManagementTeacher" + i, platformSettingList[i].PasswordManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ProgressManagementParent))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `progress_management_parent` ");
                                        values = String.Concat(values, ", @ProgressManagementParent" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `progress_management_parent` ");
                                        values = String.Concat(values, "@ProgressManagementParent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ProgressManagementParent" + i, platformSettingList[i].ProgressManagementParent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].InvitationManagementParent))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `invitation_management_parent` ");
                                        values = String.Concat(values, ", @InvitationManagementParent" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `invitation_management_parent` ");
                                        values = String.Concat(values, "@InvitationManagementParent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@InvitationManagementParent" + i, platformSettingList[i].InvitationManagementParent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ChatManagementParent))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `chat_management_parent` ");
                                        values = String.Concat(values, ", @ChatManagementParent" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `chat_management_parent` ");
                                        values = String.Concat(values, "@ChatManagementParent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ChatManagementParent" + i, platformSettingList[i].ChatManagementParent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementParent))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `message_management_parent` ");
                                        values = String.Concat(values, ", @MessageManagementParent" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `message_management_parent` ");
                                        values = String.Concat(values, "@MessageManagementParent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementParent" + i, platformSettingList[i].MessageManagementParent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementSendParent))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `message_management_send_parent` ");
                                        values = String.Concat(values, ", @MessageManagementSendParent" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `message_management_send_parent` ");
                                        values = String.Concat(values, "@MessageManagementSendParent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementSendParent" + i, platformSettingList[i].MessageManagementSendParent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ConfigurationManagerParent))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `configuration_manager_parent` ");
                                        values = String.Concat(values, ", @ConfigurationManagerParent" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `configuration_manager_parent` ");
                                        values = String.Concat(values, "@ConfigurationManagerParent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ConfigurationManagerParent" + i, platformSettingList[i].ConfigurationManagerParent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].PasswordManagementParent))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `password_management_parent` ");
                                        values = String.Concat(values, ", @PasswordManagementParent" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `password_management_parent` ");
                                        values = String.Concat(values, "@PasswordManagementParent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@PasswordManagementParent" + i, platformSettingList[i].PasswordManagementParent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementStudent))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `message_management_student` ");
                                        values = String.Concat(values, ", @MessageManagementStudent" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `message_management_student` ");
                                        values = String.Concat(values, "@MessageManagementStudent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementStudent" + i, platformSettingList[i].MessageManagementStudent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementSendStudent))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `message_management_send_student` ");
                                        values = String.Concat(values, ", @MessageManagementSendStudent" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `message_management_send_student` ");
                                        values = String.Concat(values, "@MessageManagementSendStudent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementSendStudent" + i, platformSettingList[i].MessageManagementSendStudent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ProgressManagementStudent))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `progress_management_student` ");
                                        values = String.Concat(values, ", @ProgressManagementStudent" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `progress_management_student` ");
                                        values = String.Concat(values, "@ProgressManagementStudent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ProgressManagementStudent" + i, platformSettingList[i].ProgressManagementStudent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ChatManagementStudent))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `chat_management_student` ");
                                        values = String.Concat(values, ", @ChatManagementStudent" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `chat_management_student` ");
                                        values = String.Concat(values, "@ChatManagementStudent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ChatManagementStudent" + i, platformSettingList[i].ChatManagementStudent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].TeamsManagementStudent))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `teams_management_student` ");
                                        values = String.Concat(values, ", @TeamsManagementStudent" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `teams_management_student` ");
                                        values = String.Concat(values, "@TeamsManagementStudent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@TeamsManagementStudent" + i, platformSettingList[i].TeamsManagementStudent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MicrosoftSSOManagement))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `microsoft_sso_management` ");
                                        values = String.Concat(values, ", @MicrosoftSSOManagement" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `microsoft_sso_management` ");
                                        values = String.Concat(values, "@MicrosoftSSOManagement" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MicrosoftSSOManagement" + i, platformSettingList[i].MicrosoftSSOManagement);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].InternalLoginManagement))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `internal_login_management` ");
                                        values = String.Concat(values, ", @InternalLoginManagement" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `internal_login_management` ");
                                        values = String.Concat(values, "@InternalLoginManagement" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@InternalLoginManagement" + i, platformSettingList[i].InternalLoginManagement);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].InternalPasswordReset))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `internal_password_reset` ");
                                        values = String.Concat(values, ", @InternalPasswordReset" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `internal_password_reset` ");
                                        values = String.Concat(values, "@InternalPasswordReset" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@InternalPasswordReset" + i, platformSettingList[i].InternalPasswordReset);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `modified_by` ");
                                        values = String.Concat(values, ", @ModifiedBy" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `modified_by` ");
                                        values = String.Concat(values, "@ModifiedBy" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ModifiedBy" + i, platformSettingList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].CreatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `created_at` ");
                                        values = String.Concat(values, ", @CreatedAt" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `created_at` ");
                                        values = String.Concat(values, "@CreatedAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@CreatedAt" + i, platformSettingList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `updated_at` ");
                                        values = String.Concat(values, ", @UpdatedAt" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `updated_at` ");
                                        values = String.Concat(values, "@UpdatedAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@UpdatedAt" + i, platformSettingList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(platformSettingList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<PlatformSettingModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlatformSettingModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformSettingModel>();
            }
            finally
            {
                if (_connection != null)
                {
                    dbConn.CloseConnection(_connection);
                }
            }
            return response;
        }

        public List<PlatformSettingModel> Update(string connectionString, PlatformSettingModel platformSetting, PlatformSettingModel platformSettingWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformSettingModel> response = new List<PlatformSettingModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE platform_settings SET ";
                if (!String.IsNullOrEmpty(platformSetting.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @Id");
                    else
                        sql = String.Concat(sql, " `id` = @Id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@Id", platformSetting.Id);
                }
                if (!String.IsNullOrEmpty(platformSetting.SchoolManagementAdmin))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `school_management_admin` = @SchoolManagementAdmin");
                    else
                        sql = String.Concat(sql, " `school_management_admin` = @SchoolManagementAdmin");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@SchoolManagementAdmin", platformSetting.SchoolManagementAdmin);
                }
                if (!String.IsNullOrEmpty(platformSetting.EnrollmentManagementAdmin))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `enrollment_management_admin` = @EnrollmentManagementAdmin");
                    else
                        sql = String.Concat(sql, " `enrollment_management_admin` = @EnrollmentManagementAdmin");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@EnrollmentManagementAdmin", platformSetting.EnrollmentManagementAdmin);
                }
                if (!String.IsNullOrEmpty(platformSetting.UserManagementAdmin))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_management_admin` = @UserManagementAdmin");
                    else
                        sql = String.Concat(sql, " `user_management_admin` = @UserManagementAdmin");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@UserManagementAdmin", platformSetting.UserManagementAdmin);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementAdmin))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `message_management_admin` = @MessageManagementAdmin");
                    else
                        sql = String.Concat(sql, " `message_management_admin` = @MessageManagementAdmin");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementAdmin", platformSetting.MessageManagementAdmin);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendAdmin))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `message_management_send_admin` = @MessageManagementSendAdmin");
                    else
                        sql = String.Concat(sql, " `message_management_send_admin` = @MessageManagementSendAdmin");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementSendAdmin", platformSetting.MessageManagementSendAdmin);
                }
                if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerAdmin))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `configuration_manager_admin` = @ConfigurationManagerAdmin");
                    else
                        sql = String.Concat(sql, " `configuration_manager_admin` = @ConfigurationManagerAdmin");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ConfigurationManagerAdmin", platformSetting.ConfigurationManagerAdmin);
                }
                if (!String.IsNullOrEmpty(platformSetting.PasswordManagementAdmin))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `password_management_admin` = @PasswordManagementAdmin");
                    else
                        sql = String.Concat(sql, " `password_management_admin` = @PasswordManagementAdmin");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@PasswordManagementAdmin", platformSetting.PasswordManagementAdmin);
                }
                if (!String.IsNullOrEmpty(platformSetting.SchoolManagementAdminOre))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `school_management_admin_ore` = @SchoolManagementAdminOre");
                    else
                        sql = String.Concat(sql, " `school_management_admin_ore` = @SchoolManagementAdminOre");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@SchoolManagementAdminOre", platformSetting.SchoolManagementAdminOre);
                }
                if (!String.IsNullOrEmpty(platformSetting.EnrollmentManagementAdminOre))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre");
                    else
                        sql = String.Concat(sql, " `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOre", platformSetting.EnrollmentManagementAdminOre);
                }
                if (!String.IsNullOrEmpty(platformSetting.UserManagementAdminOre))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_management_admin_ore` = @UserManagementAdminOre");
                    else
                        sql = String.Concat(sql, " `user_management_admin_ore` = @UserManagementAdminOre");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@UserManagementAdminOre", platformSetting.UserManagementAdminOre);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementAdminOre))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `message_management_admin_ore` = @MessageManagementAdminOre");
                    else
                        sql = String.Concat(sql, " `message_management_admin_ore` = @MessageManagementAdminOre");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementAdminOre", platformSetting.MessageManagementAdminOre);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendAdminOre))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `message_management_send_admin_ore` = @MessageManagementSendAdminOre");
                    else
                        sql = String.Concat(sql, " `message_management_send_admin_ore` = @MessageManagementSendAdminOre");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementSendAdminOre", platformSetting.MessageManagementSendAdminOre);
                }
                if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerAdminOre))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre");
                    else
                        sql = String.Concat(sql, " `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOre", platformSetting.ConfigurationManagerAdminOre);
                }
                if (!String.IsNullOrEmpty(platformSetting.PasswordManagementAdminOre))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `password_management_admin_ore` = @PasswordManagementAdminOre");
                    else
                        sql = String.Concat(sql, " `password_management_admin_ore` = @PasswordManagementAdminOre");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@PasswordManagementAdminOre", platformSetting.PasswordManagementAdminOre);
                }
                if (!String.IsNullOrEmpty(platformSetting.UserManagementPrincipal))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_management_principal` = @UserManagementPrincipal");
                    else
                        sql = String.Concat(sql, " `user_management_principal` = @UserManagementPrincipal");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@UserManagementPrincipal", platformSetting.UserManagementPrincipal);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementPrincipal))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `message_management_principal` = @MessageManagementPrincipal");
                    else
                        sql = String.Concat(sql, " `message_management_principal` = @MessageManagementPrincipal");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementPrincipal", platformSetting.MessageManagementPrincipal);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendPrincipal))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `message_management_send_principal` = @MessageManagementSendPrincipal");
                    else
                        sql = String.Concat(sql, " `message_management_send_principal` = @MessageManagementSendPrincipal");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementSendPrincipal", platformSetting.MessageManagementSendPrincipal);
                }
                if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerPrincipal))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `configuration_manager_principal` = @ConfigurationManagerPrincipal");
                    else
                        sql = String.Concat(sql, " `configuration_manager_principal` = @ConfigurationManagerPrincipal");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipal", platformSetting.ConfigurationManagerPrincipal);
                }
                if (!String.IsNullOrEmpty(platformSetting.PasswordManagementPrincipal))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `password_management_principal` = @PasswordManagementPrincipal");
                    else
                        sql = String.Concat(sql, " `password_management_principal` = @PasswordManagementPrincipal");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@PasswordManagementPrincipal", platformSetting.PasswordManagementPrincipal);
                }
                if (!String.IsNullOrEmpty(platformSetting.UserManagementTeacher))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_management_teacher` = @UserManagementTeacher");
                    else
                        sql = String.Concat(sql, " `user_management_teacher` = @UserManagementTeacher");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@UserManagementTeacher", platformSetting.UserManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.GroupInternalManagementTeacher))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `group_internal_management_teacher` = @GroupInternalManagementTeacher");
                    else
                        sql = String.Concat(sql, " `group_internal_management_teacher` = @GroupInternalManagementTeacher");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@GroupInternalManagementTeacher", platformSetting.GroupInternalManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.InvitationManagementTeacher))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `invitation_management_teacher` = @InvitationManagementTeacher");
                    else
                        sql = String.Concat(sql, " `invitation_management_teacher` = @InvitationManagementTeacher");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@InvitationManagementTeacher", platformSetting.InvitationManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.InvitationTeacherToParentManagementTeacher))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher");
                    else
                        sql = String.Concat(sql, " `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacher", platformSetting.InvitationTeacherToParentManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.ChatManagementTeacher))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `chat_management_teacher` = @ChatManagementTeacher");
                    else
                        sql = String.Concat(sql, " `chat_management_teacher` = @ChatManagementTeacher");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ChatManagementTeacher", platformSetting.ChatManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementTeacher))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `message_management_teacher` = @MessageManagementTeacher");
                    else
                        sql = String.Concat(sql, " `message_management_teacher` = @MessageManagementTeacher");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementTeacher", platformSetting.MessageManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendTeacher))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `message_management_send_teacher` = @MessageManagementSendTeacher");
                    else
                        sql = String.Concat(sql, " `message_management_send_teacher` = @MessageManagementSendTeacher");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementSendTeacher", platformSetting.MessageManagementSendTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.TeamMeetsManagementTeacher))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `team_meets_management_teacher` = @TeamMeetsManagementTeacher");
                    else
                        sql = String.Concat(sql, " `team_meets_management_teacher` = @TeamMeetsManagementTeacher");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacher", platformSetting.TeamMeetsManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerTeacher))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `configuration_manager_teacher` = @ConfigurationManagerTeacher");
                    else
                        sql = String.Concat(sql, " `configuration_manager_teacher` = @ConfigurationManagerTeacher");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ConfigurationManagerTeacher", platformSetting.ConfigurationManagerTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.InvitationTeacherToStudentManagementTeacher))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher");
                    else
                        sql = String.Concat(sql, " `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacher", platformSetting.InvitationTeacherToStudentManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.PasswordManagementTeacher))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `password_management_teacher` = @PasswordManagementTeacher");
                    else
                        sql = String.Concat(sql, " `password_management_teacher` = @PasswordManagementTeacher");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@PasswordManagementTeacher", platformSetting.PasswordManagementTeacher);
                }
                if (!String.IsNullOrEmpty(platformSetting.ProgressManagementParent))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `progress_management_parent` = @ProgressManagementParent");
                    else
                        sql = String.Concat(sql, " `progress_management_parent` = @ProgressManagementParent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ProgressManagementParent", platformSetting.ProgressManagementParent);
                }
                if (!String.IsNullOrEmpty(platformSetting.InvitationManagementParent))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `invitation_management_parent` = @InvitationManagementParent");
                    else
                        sql = String.Concat(sql, " `invitation_management_parent` = @InvitationManagementParent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@InvitationManagementParent", platformSetting.InvitationManagementParent);
                }
                if (!String.IsNullOrEmpty(platformSetting.ChatManagementParent))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `chat_management_parent` = @ChatManagementParent");
                    else
                        sql = String.Concat(sql, " `chat_management_parent` = @ChatManagementParent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ChatManagementParent", platformSetting.ChatManagementParent);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementParent))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `message_management_parent` = @MessageManagementParent");
                    else
                        sql = String.Concat(sql, " `message_management_parent` = @MessageManagementParent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementParent", platformSetting.MessageManagementParent);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendParent))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `message_management_send_parent` = @MessageManagementSendParent");
                    else
                        sql = String.Concat(sql, " `message_management_send_parent` = @MessageManagementSendParent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementSendParent", platformSetting.MessageManagementSendParent);
                }
                if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerParent))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `configuration_manager_parent` = @ConfigurationManagerParent");
                    else
                        sql = String.Concat(sql, " `configuration_manager_parent` = @ConfigurationManagerParent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ConfigurationManagerParent", platformSetting.ConfigurationManagerParent);
                }
                if (!String.IsNullOrEmpty(platformSetting.PasswordManagementParent))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `password_management_parent` = @PasswordManagementParent");
                    else
                        sql = String.Concat(sql, " `password_management_parent` = @PasswordManagementParent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@PasswordManagementParent", platformSetting.PasswordManagementParent);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementStudent))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `message_management_student` = @MessageManagementStudent");
                    else
                        sql = String.Concat(sql, " `message_management_student` = @MessageManagementStudent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementStudent", platformSetting.MessageManagementStudent);
                }
                if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendStudent))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `message_management_send_student` = @MessageManagementSendStudent");
                    else
                        sql = String.Concat(sql, " `message_management_send_student` = @MessageManagementSendStudent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MessageManagementSendStudent", platformSetting.MessageManagementSendStudent);
                }
                if (!String.IsNullOrEmpty(platformSetting.ProgressManagementStudent))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `progress_management_student` = @ProgressManagementStudent");
                    else
                        sql = String.Concat(sql, " `progress_management_student` = @ProgressManagementStudent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ProgressManagementStudent", platformSetting.ProgressManagementStudent);
                }
                if (!String.IsNullOrEmpty(platformSetting.ChatManagementStudent))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `chat_management_student` = @ChatManagementStudent");
                    else
                        sql = String.Concat(sql, " `chat_management_student` = @ChatManagementStudent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ChatManagementStudent", platformSetting.ChatManagementStudent);
                }
                if (!String.IsNullOrEmpty(platformSetting.TeamsManagementStudent))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `teams_management_student` = @TeamsManagementStudent");
                    else
                        sql = String.Concat(sql, " `teams_management_student` = @TeamsManagementStudent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@TeamsManagementStudent", platformSetting.TeamsManagementStudent);
                }
                if (!String.IsNullOrEmpty(platformSetting.MicrosoftSSOManagement))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `microsoft_sso_management` = @MicrosoftSSOManagement");
                    else
                        sql = String.Concat(sql, " `microsoft_sso_management` = @MicrosoftSSOManagement");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@MicrosoftSSOManagement", platformSetting.MicrosoftSSOManagement);
                }
                if (!String.IsNullOrEmpty(platformSetting.InternalLoginManagement))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `internal_login_management` = @InternalLoginManagement");
                    else
                        sql = String.Concat(sql, " `internal_login_management` = @InternalLoginManagement");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@InternalLoginManagement", platformSetting.InternalLoginManagement);
                }
                if (!String.IsNullOrEmpty(platformSetting.InternalPasswordReset))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `internal_password_reset` = @InternalPasswordReset");
                    else
                        sql = String.Concat(sql, " `internal_password_reset` = @InternalPasswordReset");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@InternalPasswordReset", platformSetting.InternalPasswordReset);
                }
                if (!String.IsNullOrEmpty(platformSetting.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @ModifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @ModifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ModifiedBy", platformSetting.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(platformSetting.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @CreatedAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @CreatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@CreatedAt", platformSetting.CreatedAt);
                }
                if (!String.IsNullOrEmpty(platformSetting.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @UpdatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @UpdatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@UpdatedAt", platformSetting.UpdatedAt);
                }
                if (platformSettingWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(platformSettingWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @IdWhere");
                        else
                            sql = String.Concat(sql, " `id` = @IdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@IdWhere", platformSettingWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.SchoolManagementAdmin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `school_management_admin` = @SchoolManagementAdminWhere");
                        else
                            sql = String.Concat(sql, " `school_management_admin` = @SchoolManagementAdminWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@SchoolManagementAdminWhere", platformSettingWhere.SchoolManagementAdmin);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.EnrollmentManagementAdmin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `enrollment_management_admin` = @EnrollmentManagementAdminWhere");
                        else
                            sql = String.Concat(sql, " `enrollment_management_admin` = @EnrollmentManagementAdminWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@EnrollmentManagementAdminWhere", platformSettingWhere.EnrollmentManagementAdmin);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.UserManagementAdmin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_management_admin` = @UserManagementAdminWhere");
                        else
                            sql = String.Concat(sql, " `user_management_admin` = @UserManagementAdminWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@UserManagementAdminWhere", platformSettingWhere.UserManagementAdmin);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.MessageManagementAdmin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_admin` = @MessageManagementAdminWhere");
                        else
                            sql = String.Concat(sql, " `message_management_admin` = @MessageManagementAdminWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementAdminWhere", platformSettingWhere.MessageManagementAdmin);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.MessageManagementSendAdmin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_send_admin` = @MessageManagementSendAdminWhere");
                        else
                            sql = String.Concat(sql, " `message_management_send_admin` = @MessageManagementSendAdminWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementSendAdminWhere", platformSettingWhere.MessageManagementSendAdmin);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.ConfigurationManagerAdmin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `configuration_manager_admin` = @ConfigurationManagerAdminWhere");
                        else
                            sql = String.Concat(sql, " `configuration_manager_admin` = @ConfigurationManagerAdminWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ConfigurationManagerAdminWhere", platformSettingWhere.ConfigurationManagerAdmin);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.PasswordManagementAdmin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `password_management_admin` = @PasswordManagementAdminWhere");
                        else
                            sql = String.Concat(sql, " `password_management_admin` = @PasswordManagementAdminWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@PasswordManagementAdminWhere", platformSettingWhere.PasswordManagementAdmin);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.SchoolManagementAdminOre))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `school_management_admin_ore` = @SchoolManagementAdminOreWhere");
                        else
                            sql = String.Concat(sql, " `school_management_admin_ore` = @SchoolManagementAdminOreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@SchoolManagementAdminOreWhere", platformSettingWhere.SchoolManagementAdminOre);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.EnrollmentManagementAdminOre))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `enrollment_management_admin_ore` = @EnrollmentManagementAdminOreWhere");
                        else
                            sql = String.Concat(sql, " `enrollment_management_admin_ore` = @EnrollmentManagementAdminOreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOreWhere", platformSettingWhere.EnrollmentManagementAdminOre);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.UserManagementAdminOre))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_management_admin_ore` = @UserManagementAdminOreWhere");
                        else
                            sql = String.Concat(sql, " `user_management_admin_ore` = @UserManagementAdminOreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@UserManagementAdminOreWhere", platformSettingWhere.UserManagementAdminOre);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.MessageManagementAdminOre))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_admin_ore` = @MessageManagementAdminOreWhere");
                        else
                            sql = String.Concat(sql, " `message_management_admin_ore` = @MessageManagementAdminOreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementAdminOreWhere", platformSettingWhere.MessageManagementAdminOre);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.MessageManagementSendAdminOre))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_send_admin_ore` = @MessageManagementSendAdminOreWhere");
                        else
                            sql = String.Concat(sql, " `message_management_send_admin_ore` = @MessageManagementSendAdminOreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementSendAdminOreWhere", platformSettingWhere.MessageManagementSendAdminOre);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.ConfigurationManagerAdminOre))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `configuration_manager_admin_ore` = @ConfigurationManagerAdminOreWhere");
                        else
                            sql = String.Concat(sql, " `configuration_manager_admin_ore` = @ConfigurationManagerAdminOreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOreWhere", platformSettingWhere.ConfigurationManagerAdminOre);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.PasswordManagementAdminOre))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `password_management_admin_ore` = @PasswordManagementAdminOreWhere");
                        else
                            sql = String.Concat(sql, " `password_management_admin_ore` = @PasswordManagementAdminOreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@PasswordManagementAdminOreWhere", platformSettingWhere.PasswordManagementAdminOre);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.UserManagementPrincipal))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_management_principal` = @UserManagementPrincipalWhere");
                        else
                            sql = String.Concat(sql, " `user_management_principal` = @UserManagementPrincipalWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@UserManagementPrincipalWhere", platformSettingWhere.UserManagementPrincipal);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.MessageManagementPrincipal))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_principal` = @MessageManagementPrincipalWhere");
                        else
                            sql = String.Concat(sql, " `message_management_principal` = @MessageManagementPrincipalWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementPrincipalWhere", platformSettingWhere.MessageManagementPrincipal);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.MessageManagementSendPrincipal))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_send_principal` = @MessageManagementSendPrincipalWhere");
                        else
                            sql = String.Concat(sql, " `message_management_send_principal` = @MessageManagementSendPrincipalWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementSendPrincipalWhere", platformSettingWhere.MessageManagementSendPrincipal);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.ConfigurationManagerPrincipal))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `configuration_manager_principal` = @ConfigurationManagerPrincipalWhere");
                        else
                            sql = String.Concat(sql, " `configuration_manager_principal` = @ConfigurationManagerPrincipalWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipalWhere", platformSettingWhere.ConfigurationManagerPrincipal);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.PasswordManagementPrincipal))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `password_management_principal` = @PasswordManagementPrincipalWhere");
                        else
                            sql = String.Concat(sql, " `password_management_principal` = @PasswordManagementPrincipalWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@PasswordManagementPrincipalWhere", platformSettingWhere.PasswordManagementPrincipal);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.UserManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_management_teacher` = @UserManagementTeacherWhere");
                        else
                            sql = String.Concat(sql, " `user_management_teacher` = @UserManagementTeacherWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@UserManagementTeacherWhere", platformSettingWhere.UserManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.GroupInternalManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `group_internal_management_teacher` = @GroupInternalManagementTeacherWhere");
                        else
                            sql = String.Concat(sql, " `group_internal_management_teacher` = @GroupInternalManagementTeacherWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@GroupInternalManagementTeacherWhere", platformSettingWhere.GroupInternalManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.InvitationManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `invitation_management_teacher` = @InvitationManagementTeacherWhere");
                        else
                            sql = String.Concat(sql, " `invitation_management_teacher` = @InvitationManagementTeacherWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@InvitationManagementTeacherWhere", platformSettingWhere.InvitationManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.InvitationTeacherToParentManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacherWhere");
                        else
                            sql = String.Concat(sql, " `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacherWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacherWhere", platformSettingWhere.InvitationTeacherToParentManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.ChatManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `chat_management_teacher` = @ChatManagementTeacherWhere");
                        else
                            sql = String.Concat(sql, " `chat_management_teacher` = @ChatManagementTeacherWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ChatManagementTeacherWhere", platformSettingWhere.ChatManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.MessageManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_teacher` = @MessageManagementTeacherWhere");
                        else
                            sql = String.Concat(sql, " `message_management_teacher` = @MessageManagementTeacherWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementTeacherWhere", platformSettingWhere.MessageManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.MessageManagementSendTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_send_teacher` = @MessageManagementSendTeacherWhere");
                        else
                            sql = String.Concat(sql, " `message_management_send_teacher` = @MessageManagementSendTeacherWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementSendTeacherWhere", platformSettingWhere.MessageManagementSendTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.TeamMeetsManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `team_meets_management_teacher` = @TeamMeetsManagementTeacherWhere");
                        else
                            sql = String.Concat(sql, " `team_meets_management_teacher` = @TeamMeetsManagementTeacherWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacherWhere", platformSettingWhere.TeamMeetsManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.ConfigurationManagerTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `configuration_manager_teacher` = @ConfigurationManagerTeacherWhere");
                        else
                            sql = String.Concat(sql, " `configuration_manager_teacher` = @ConfigurationManagerTeacherWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ConfigurationManagerTeacherWhere", platformSettingWhere.ConfigurationManagerTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.InvitationTeacherToStudentManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacherWhere");
                        else
                            sql = String.Concat(sql, " `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacherWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacherWhere", platformSettingWhere.InvitationTeacherToStudentManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.PasswordManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `password_management_teacher` = @PasswordManagementTeacherWhere");
                        else
                            sql = String.Concat(sql, " `password_management_teacher` = @PasswordManagementTeacherWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@PasswordManagementTeacherWhere", platformSettingWhere.PasswordManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.ProgressManagementParent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `progress_management_parent` = @ProgressManagementParentWhere");
                        else
                            sql = String.Concat(sql, " `progress_management_parent` = @ProgressManagementParentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ProgressManagementParentWhere", platformSettingWhere.ProgressManagementParent);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.InvitationManagementParent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `invitation_management_parent` = @InvitationManagementParentWhere");
                        else
                            sql = String.Concat(sql, " `invitation_management_parent` = @InvitationManagementParentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@InvitationManagementParentWhere", platformSettingWhere.InvitationManagementParent);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.ChatManagementParent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `chat_management_parent` = @ChatManagementParentWhere");
                        else
                            sql = String.Concat(sql, " `chat_management_parent` = @ChatManagementParentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ChatManagementParentWhere", platformSettingWhere.ChatManagementParent);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.MessageManagementParent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_parent` = @MessageManagementParentWhere");
                        else
                            sql = String.Concat(sql, " `message_management_parent` = @MessageManagementParentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementParentWhere", platformSettingWhere.MessageManagementParent);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.MessageManagementSendParent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_send_parent` = @MessageManagementSendParentWhere");
                        else
                            sql = String.Concat(sql, " `message_management_send_parent` = @MessageManagementSendParentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementSendParentWhere", platformSettingWhere.MessageManagementSendParent);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.ConfigurationManagerParent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `configuration_manager_parent` = @ConfigurationManagerParentWhere");
                        else
                            sql = String.Concat(sql, " `configuration_manager_parent` = @ConfigurationManagerParentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ConfigurationManagerParentWhere", platformSettingWhere.ConfigurationManagerParent);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.PasswordManagementParent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `password_management_parent` = @PasswordManagementParentWhere");
                        else
                            sql = String.Concat(sql, " `password_management_parent` = @PasswordManagementParentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@PasswordManagementParentWhere", platformSettingWhere.PasswordManagementParent);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.MessageManagementStudent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_student` = @MessageManagementStudentWhere");
                        else
                            sql = String.Concat(sql, " `message_management_student` = @MessageManagementStudentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementStudentWhere", platformSettingWhere.MessageManagementStudent);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.MessageManagementSendStudent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_send_student` = @MessageManagementSendStudentWhere");
                        else
                            sql = String.Concat(sql, " `message_management_send_student` = @MessageManagementSendStudentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementSendStudentWhere", platformSettingWhere.MessageManagementSendStudent);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.ProgressManagementStudent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `progress_management_student` = @ProgressManagementStudentWhere");
                        else
                            sql = String.Concat(sql, " `progress_management_student` = @ProgressManagementStudentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ProgressManagementStudentWhere", platformSettingWhere.ProgressManagementStudent);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.ChatManagementStudent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `chat_management_student` = @ChatManagementStudentWhere");
                        else
                            sql = String.Concat(sql, " `chat_management_student` = @ChatManagementStudentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ChatManagementStudentWhere", platformSettingWhere.ChatManagementStudent);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.TeamsManagementStudent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teams_management_student` = @TeamsManagementStudentWhere");
                        else
                            sql = String.Concat(sql, " `teams_management_student` = @TeamsManagementStudentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@TeamsManagementStudentWhere", platformSettingWhere.TeamsManagementStudent);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.MicrosoftSSOManagement))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `microsoft_sso_management` = @MicrosoftSSOManagementWhere");
                        else
                            sql = String.Concat(sql, " `microsoft_sso_management` = @MicrosoftSSOManagementWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MicrosoftSSOManagementWhere", platformSettingWhere.MicrosoftSSOManagement);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.InternalLoginManagement))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `internal_login_management` = @InternalLoginManagementWhere");
                        else
                            sql = String.Concat(sql, " `internal_login_management` = @InternalLoginManagementWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@InternalLoginManagementWhere", platformSettingWhere.InternalLoginManagement);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.InternalPasswordReset))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `internal_password_reset` = @InternalPasswordResetWhere");
                        else
                            sql = String.Concat(sql, " `internal_password_reset` = @InternalPasswordResetWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@InternalPasswordResetWhere", platformSettingWhere.InternalPasswordReset);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @ModifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @ModifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ModifiedByWhere", platformSettingWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @CreatedAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @CreatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@CreatedAtWhere", platformSettingWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(platformSettingWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @UpdatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @UpdatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@UpdatedAtWhere", platformSettingWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(platformSetting);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformSettingModel>();
            }
            finally
            {
                if (_connection != null)
                {
                    dbConn.CloseConnection(_connection);
                }
            }
            return response;
        }

        public List<PlatformSettingModel> Update(string connectionString, List<PlatformSettingModel> platformSettingList, List<PlatformSettingModel> platformSettingWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformSettingModel> response = new List<PlatformSettingModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < platformSettingList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE platform_settings SET ";
                                if (!String.IsNullOrEmpty(platformSettingList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @Id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @Id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@Id" + i, platformSettingList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].SchoolManagementAdmin))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `school_management_admin` = @SchoolManagementAdmin" + i);
                                    else
                                        sql = String.Concat(sql, " `school_management_admin` = @SchoolManagementAdmin" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@SchoolManagementAdmin" + i, platformSettingList[i].SchoolManagementAdmin);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].EnrollmentManagementAdmin))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `enrollment_management_admin` = @EnrollmentManagementAdmin" + i);
                                    else
                                        sql = String.Concat(sql, " `enrollment_management_admin` = @EnrollmentManagementAdmin" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@EnrollmentManagementAdmin" + i, platformSettingList[i].EnrollmentManagementAdmin);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].UserManagementAdmin))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_management_admin` = @UserManagementAdmin" + i);
                                    else
                                        sql = String.Concat(sql, " `user_management_admin` = @UserManagementAdmin" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@UserManagementAdmin" + i, platformSettingList[i].UserManagementAdmin);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementAdmin))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `message_management_admin` = @MessageManagementAdmin" + i);
                                    else
                                        sql = String.Concat(sql, " `message_management_admin` = @MessageManagementAdmin" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementAdmin" + i, platformSettingList[i].MessageManagementAdmin);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementSendAdmin))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `message_management_send_admin` = @MessageManagementSendAdmin" + i);
                                    else
                                        sql = String.Concat(sql, " `message_management_send_admin` = @MessageManagementSendAdmin" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementSendAdmin" + i, platformSettingList[i].MessageManagementSendAdmin);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ConfigurationManagerAdmin))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `configuration_manager_admin` = @ConfigurationManagerAdmin" + i);
                                    else
                                        sql = String.Concat(sql, " `configuration_manager_admin` = @ConfigurationManagerAdmin" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ConfigurationManagerAdmin" + i, platformSettingList[i].ConfigurationManagerAdmin);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].PasswordManagementAdmin))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `password_management_admin` = @PasswordManagementAdmin" + i);
                                    else
                                        sql = String.Concat(sql, " `password_management_admin` = @PasswordManagementAdmin" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@PasswordManagementAdmin" + i, platformSettingList[i].PasswordManagementAdmin);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].SchoolManagementAdminOre))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `school_management_admin_ore` = @SchoolManagementAdminOre" + i);
                                    else
                                        sql = String.Concat(sql, " `school_management_admin_ore` = @SchoolManagementAdminOre" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@SchoolManagementAdminOre" + i, platformSettingList[i].SchoolManagementAdminOre);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].EnrollmentManagementAdminOre))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre" + i);
                                    else
                                        sql = String.Concat(sql, " `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOre" + i, platformSettingList[i].EnrollmentManagementAdminOre);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].UserManagementAdminOre))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_management_admin_ore` = @UserManagementAdminOre" + i);
                                    else
                                        sql = String.Concat(sql, " `user_management_admin_ore` = @UserManagementAdminOre" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@UserManagementAdminOre" + i, platformSettingList[i].UserManagementAdminOre);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementAdminOre))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `message_management_admin_ore` = @MessageManagementAdminOre" + i);
                                    else
                                        sql = String.Concat(sql, " `message_management_admin_ore` = @MessageManagementAdminOre" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementAdminOre" + i, platformSettingList[i].MessageManagementAdminOre);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementSendAdminOre))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `message_management_send_admin_ore` = @MessageManagementSendAdminOre" + i);
                                    else
                                        sql = String.Concat(sql, " `message_management_send_admin_ore` = @MessageManagementSendAdminOre" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementSendAdminOre" + i, platformSettingList[i].MessageManagementSendAdminOre);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ConfigurationManagerAdminOre))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre" + i);
                                    else
                                        sql = String.Concat(sql, " `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOre" + i, platformSettingList[i].ConfigurationManagerAdminOre);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].PasswordManagementAdminOre))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `password_management_admin_ore` = @PasswordManagementAdminOre" + i);
                                    else
                                        sql = String.Concat(sql, " `password_management_admin_ore` = @PasswordManagementAdminOre" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@PasswordManagementAdminOre" + i, platformSettingList[i].PasswordManagementAdminOre);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].UserManagementPrincipal))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_management_principal` = @UserManagementPrincipal" + i);
                                    else
                                        sql = String.Concat(sql, " `user_management_principal` = @UserManagementPrincipal" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@UserManagementPrincipal" + i, platformSettingList[i].UserManagementPrincipal);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementPrincipal))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `message_management_principal` = @MessageManagementPrincipal" + i);
                                    else
                                        sql = String.Concat(sql, " `message_management_principal` = @MessageManagementPrincipal" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementPrincipal" + i, platformSettingList[i].MessageManagementPrincipal);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementSendPrincipal))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `message_management_send_principal` = @MessageManagementSendPrincipal" + i);
                                    else
                                        sql = String.Concat(sql, " `message_management_send_principal` = @MessageManagementSendPrincipal" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementSendPrincipal" + i, platformSettingList[i].MessageManagementSendPrincipal);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ConfigurationManagerPrincipal))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `configuration_manager_principal` = @ConfigurationManagerPrincipal" + i);
                                    else
                                        sql = String.Concat(sql, " `configuration_manager_principal` = @ConfigurationManagerPrincipal" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipal" + i, platformSettingList[i].ConfigurationManagerPrincipal);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].PasswordManagementPrincipal))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `password_management_principal` = @PasswordManagementPrincipal" + i);
                                    else
                                        sql = String.Concat(sql, " `password_management_principal` = @PasswordManagementPrincipal" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@PasswordManagementPrincipal" + i, platformSettingList[i].PasswordManagementPrincipal);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].UserManagementTeacher))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_management_teacher` = @UserManagementTeacher" + i);
                                    else
                                        sql = String.Concat(sql, " `user_management_teacher` = @UserManagementTeacher" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@UserManagementTeacher" + i, platformSettingList[i].UserManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].GroupInternalManagementTeacher))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `group_internal_management_teacher` = @GroupInternalManagementTeacher" + i);
                                    else
                                        sql = String.Concat(sql, " `group_internal_management_teacher` = @GroupInternalManagementTeacher" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@GroupInternalManagementTeacher" + i, platformSettingList[i].GroupInternalManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].InvitationManagementTeacher))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `invitation_management_teacher` = @InvitationManagementTeacher" + i);
                                    else
                                        sql = String.Concat(sql, " `invitation_management_teacher` = @InvitationManagementTeacher" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@InvitationManagementTeacher" + i, platformSettingList[i].InvitationManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].InvitationTeacherToParentManagementTeacher))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher" + i);
                                    else
                                        sql = String.Concat(sql, " `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacher" + i, platformSettingList[i].InvitationTeacherToParentManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ChatManagementTeacher))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `chat_management_teacher` = @ChatManagementTeacher" + i);
                                    else
                                        sql = String.Concat(sql, " `chat_management_teacher` = @ChatManagementTeacher" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ChatManagementTeacher" + i, platformSettingList[i].ChatManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementTeacher))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `message_management_teacher` = @MessageManagementTeacher" + i);
                                    else
                                        sql = String.Concat(sql, " `message_management_teacher` = @MessageManagementTeacher" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementTeacher" + i, platformSettingList[i].MessageManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementSendTeacher))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `message_management_send_teacher` = @MessageManagementSendTeacher" + i);
                                    else
                                        sql = String.Concat(sql, " `message_management_send_teacher` = @MessageManagementSendTeacher" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementSendTeacher" + i, platformSettingList[i].MessageManagementSendTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].TeamMeetsManagementTeacher))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `team_meets_management_teacher` = @TeamMeetsManagementTeacher" + i);
                                    else
                                        sql = String.Concat(sql, " `team_meets_management_teacher` = @TeamMeetsManagementTeacher" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacher" + i, platformSettingList[i].TeamMeetsManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ConfigurationManagerTeacher))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `configuration_manager_teacher` = @ConfigurationManagerTeacher" + i);
                                    else
                                        sql = String.Concat(sql, " `configuration_manager_teacher` = @ConfigurationManagerTeacher" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ConfigurationManagerTeacher" + i, platformSettingList[i].ConfigurationManagerTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].InvitationTeacherToStudentManagementTeacher))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher" + i);
                                    else
                                        sql = String.Concat(sql, " `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacher" + i, platformSettingList[i].InvitationTeacherToStudentManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].PasswordManagementTeacher))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `password_management_teacher` = @PasswordManagementTeacher" + i);
                                    else
                                        sql = String.Concat(sql, " `password_management_teacher` = @PasswordManagementTeacher" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@PasswordManagementTeacher" + i, platformSettingList[i].PasswordManagementTeacher);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ProgressManagementParent))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `progress_management_parent` = @ProgressManagementParent" + i);
                                    else
                                        sql = String.Concat(sql, " `progress_management_parent` = @ProgressManagementParent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ProgressManagementParent" + i, platformSettingList[i].ProgressManagementParent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].InvitationManagementParent))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `invitation_management_parent` = @InvitationManagementParent" + i);
                                    else
                                        sql = String.Concat(sql, " `invitation_management_parent` = @InvitationManagementParent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@InvitationManagementParent" + i, platformSettingList[i].InvitationManagementParent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ChatManagementParent))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `chat_management_parent` = @ChatManagementParent" + i);
                                    else
                                        sql = String.Concat(sql, " `chat_management_parent` = @ChatManagementParent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ChatManagementParent" + i, platformSettingList[i].ChatManagementParent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementParent))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `message_management_parent` = @MessageManagementParent" + i);
                                    else
                                        sql = String.Concat(sql, " `message_management_parent` = @MessageManagementParent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementParent" + i, platformSettingList[i].MessageManagementParent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementSendParent))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `message_management_send_parent` = @MessageManagementSendParent" + i);
                                    else
                                        sql = String.Concat(sql, " `message_management_send_parent` = @MessageManagementSendParent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementSendParent" + i, platformSettingList[i].MessageManagementSendParent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ConfigurationManagerParent))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `configuration_manager_parent` = @ConfigurationManagerParent" + i);
                                    else
                                        sql = String.Concat(sql, " `configuration_manager_parent` = @ConfigurationManagerParent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ConfigurationManagerParent" + i, platformSettingList[i].ConfigurationManagerParent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].PasswordManagementParent))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `password_management_parent` = @PasswordManagementParent" + i);
                                    else
                                        sql = String.Concat(sql, " `password_management_parent` = @PasswordManagementParent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@PasswordManagementParent" + i, platformSettingList[i].PasswordManagementParent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementStudent))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `message_management_student` = @MessageManagementStudent" + i);
                                    else
                                        sql = String.Concat(sql, " `message_management_student` = @MessageManagementStudent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementStudent" + i, platformSettingList[i].MessageManagementStudent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MessageManagementSendStudent))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `message_management_send_student` = @MessageManagementSendStudent" + i);
                                    else
                                        sql = String.Concat(sql, " `message_management_send_student` = @MessageManagementSendStudent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MessageManagementSendStudent" + i, platformSettingList[i].MessageManagementSendStudent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ProgressManagementStudent))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `progress_management_student` = @ProgressManagementStudent" + i);
                                    else
                                        sql = String.Concat(sql, " `progress_management_student` = @ProgressManagementStudent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ProgressManagementStudent" + i, platformSettingList[i].ProgressManagementStudent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ChatManagementStudent))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `chat_management_student` = @ChatManagementStudent" + i);
                                    else
                                        sql = String.Concat(sql, " `chat_management_student` = @ChatManagementStudent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ChatManagementStudent" + i, platformSettingList[i].ChatManagementStudent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].TeamsManagementStudent))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `teams_management_student` = @TeamsManagementStudent" + i);
                                    else
                                        sql = String.Concat(sql, " `teams_management_student` = @TeamsManagementStudent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@TeamsManagementStudent" + i, platformSettingList[i].TeamsManagementStudent);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].MicrosoftSSOManagement))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `microsoft_sso_management` = @MicrosoftSSOManagement" + i);
                                    else
                                        sql = String.Concat(sql, " `microsoft_sso_management` = @MicrosoftSSOManagement" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@MicrosoftSSOManagement" + i, platformSettingList[i].MicrosoftSSOManagement);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].InternalLoginManagement))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `internal_login_management` = @InternalLoginManagement" + i);
                                    else
                                        sql = String.Concat(sql, " `internal_login_management` = @InternalLoginManagement" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@InternalLoginManagement" + i, platformSettingList[i].InternalLoginManagement);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].InternalPasswordReset))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `internal_password_reset` = @InternalPasswordReset" + i);
                                    else
                                        sql = String.Concat(sql, " `internal_password_reset` = @InternalPasswordReset" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@InternalPasswordReset" + i, platformSettingList[i].InternalPasswordReset);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @ModifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @ModifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ModifiedBy" + i, platformSettingList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @CreatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @CreatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@CreatedAt" + i, platformSettingList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(platformSettingList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @UpdatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @UpdatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@UpdatedAt" + i, platformSettingList[i].UpdatedAt);
                                }
                                if (platformSettingWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @IdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @IdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@IdWhere" + i, platformSettingWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].SchoolManagementAdmin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `school_management_admin` = @SchoolManagementAdminWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `school_management_admin` = @SchoolManagementAdminWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@SchoolManagementAdminWhere" + i, platformSettingWhereList[i].SchoolManagementAdmin);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].EnrollmentManagementAdmin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `enrollment_management_admin` = @EnrollmentManagementAdminWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `enrollment_management_admin` = @EnrollmentManagementAdminWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@EnrollmentManagementAdminWhere" + i, platformSettingWhereList[i].EnrollmentManagementAdmin);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].UserManagementAdmin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_management_admin` = @UserManagementAdminWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_management_admin` = @UserManagementAdminWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@UserManagementAdminWhere" + i, platformSettingWhereList[i].UserManagementAdmin);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].MessageManagementAdmin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_admin` = @MessageManagementAdminWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_admin` = @MessageManagementAdminWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementAdminWhere" + i, platformSettingWhereList[i].MessageManagementAdmin);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].MessageManagementSendAdmin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_send_admin` = @MessageManagementSendAdminWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_send_admin` = @MessageManagementSendAdminWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementSendAdminWhere" + i, platformSettingWhereList[i].MessageManagementSendAdmin);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].ConfigurationManagerAdmin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `configuration_manager_admin` = @ConfigurationManagerAdminWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `configuration_manager_admin` = @ConfigurationManagerAdminWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ConfigurationManagerAdminWhere" + i, platformSettingWhereList[i].ConfigurationManagerAdmin);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].PasswordManagementAdmin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `password_management_admin` = @PasswordManagementAdminWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `password_management_admin` = @PasswordManagementAdminWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@PasswordManagementAdminWhere" + i, platformSettingWhereList[i].PasswordManagementAdmin);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].SchoolManagementAdminOre))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `school_management_admin_ore` = @SchoolManagementAdminOreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `school_management_admin_ore` = @SchoolManagementAdminOreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@SchoolManagementAdminOreWhere" + i, platformSettingWhereList[i].SchoolManagementAdminOre);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].EnrollmentManagementAdminOre))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `enrollment_management_admin_ore` = @EnrollmentManagementAdminOreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `enrollment_management_admin_ore` = @EnrollmentManagementAdminOreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOreWhere" + i, platformSettingWhereList[i].EnrollmentManagementAdminOre);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].UserManagementAdminOre))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_management_admin_ore` = @UserManagementAdminOreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_management_admin_ore` = @UserManagementAdminOreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@UserManagementAdminOreWhere" + i, platformSettingWhereList[i].UserManagementAdminOre);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].MessageManagementAdminOre))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_admin_ore` = @MessageManagementAdminOreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_admin_ore` = @MessageManagementAdminOreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementAdminOreWhere" + i, platformSettingWhereList[i].MessageManagementAdminOre);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].MessageManagementSendAdminOre))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_send_admin_ore` = @MessageManagementSendAdminOreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_send_admin_ore` = @MessageManagementSendAdminOreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementSendAdminOreWhere" + i, platformSettingWhereList[i].MessageManagementSendAdminOre);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].ConfigurationManagerAdminOre))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `configuration_manager_admin_ore` = @ConfigurationManagerAdminOreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `configuration_manager_admin_ore` = @ConfigurationManagerAdminOreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOreWhere" + i, platformSettingWhereList[i].ConfigurationManagerAdminOre);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].PasswordManagementAdminOre))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `password_management_admin_ore` = @PasswordManagementAdminOreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `password_management_admin_ore` = @PasswordManagementAdminOreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@PasswordManagementAdminOreWhere" + i, platformSettingWhereList[i].PasswordManagementAdminOre);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].UserManagementPrincipal))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_management_principal` = @UserManagementPrincipalWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_management_principal` = @UserManagementPrincipalWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@UserManagementPrincipalWhere" + i, platformSettingWhereList[i].UserManagementPrincipal);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].MessageManagementPrincipal))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_principal` = @MessageManagementPrincipalWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_principal` = @MessageManagementPrincipalWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementPrincipalWhere" + i, platformSettingWhereList[i].MessageManagementPrincipal);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].MessageManagementSendPrincipal))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_send_principal` = @MessageManagementSendPrincipalWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_send_principal` = @MessageManagementSendPrincipalWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementSendPrincipalWhere" + i, platformSettingWhereList[i].MessageManagementSendPrincipal);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].ConfigurationManagerPrincipal))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `configuration_manager_principal` = @ConfigurationManagerPrincipalWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `configuration_manager_principal` = @ConfigurationManagerPrincipalWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipalWhere" + i, platformSettingWhereList[i].ConfigurationManagerPrincipal);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].PasswordManagementPrincipal))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `password_management_principal` = @PasswordManagementPrincipalWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `password_management_principal` = @PasswordManagementPrincipalWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@PasswordManagementPrincipalWhere" + i, platformSettingWhereList[i].PasswordManagementPrincipal);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].UserManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_management_teacher` = @UserManagementTeacherWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_management_teacher` = @UserManagementTeacherWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@UserManagementTeacherWhere" + i, platformSettingWhereList[i].UserManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].GroupInternalManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `group_internal_management_teacher` = @GroupInternalManagementTeacherWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `group_internal_management_teacher` = @GroupInternalManagementTeacherWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@GroupInternalManagementTeacherWhere" + i, platformSettingWhereList[i].GroupInternalManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].InvitationManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `invitation_management_teacher` = @InvitationManagementTeacherWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `invitation_management_teacher` = @InvitationManagementTeacherWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@InvitationManagementTeacherWhere" + i, platformSettingWhereList[i].InvitationManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].InvitationTeacherToParentManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacherWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacherWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacherWhere" + i, platformSettingWhereList[i].InvitationTeacherToParentManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].ChatManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `chat_management_teacher` = @ChatManagementTeacherWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `chat_management_teacher` = @ChatManagementTeacherWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ChatManagementTeacherWhere" + i, platformSettingWhereList[i].ChatManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].MessageManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_teacher` = @MessageManagementTeacherWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_teacher` = @MessageManagementTeacherWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementTeacherWhere" + i, platformSettingWhereList[i].MessageManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].MessageManagementSendTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_send_teacher` = @MessageManagementSendTeacherWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_send_teacher` = @MessageManagementSendTeacherWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementSendTeacherWhere" + i, platformSettingWhereList[i].MessageManagementSendTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].TeamMeetsManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `team_meets_management_teacher` = @TeamMeetsManagementTeacherWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `team_meets_management_teacher` = @TeamMeetsManagementTeacherWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacherWhere" + i, platformSettingWhereList[i].TeamMeetsManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].ConfigurationManagerTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `configuration_manager_teacher` = @ConfigurationManagerTeacherWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `configuration_manager_teacher` = @ConfigurationManagerTeacherWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ConfigurationManagerTeacherWhere" + i, platformSettingWhereList[i].ConfigurationManagerTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].InvitationTeacherToStudentManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacherWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacherWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacherWhere" + i, platformSettingWhereList[i].InvitationTeacherToStudentManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].PasswordManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `password_management_teacher` = @PasswordManagementTeacherWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `password_management_teacher` = @PasswordManagementTeacherWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@PasswordManagementTeacherWhere" + i, platformSettingWhereList[i].PasswordManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].ProgressManagementParent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `progress_management_parent` = @ProgressManagementParentWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `progress_management_parent` = @ProgressManagementParentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ProgressManagementParentWhere" + i, platformSettingWhereList[i].ProgressManagementParent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].InvitationManagementParent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `invitation_management_parent` = @InvitationManagementParentWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `invitation_management_parent` = @InvitationManagementParentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@InvitationManagementParentWhere" + i, platformSettingWhereList[i].InvitationManagementParent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].ChatManagementParent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `chat_management_parent` = @ChatManagementParentWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `chat_management_parent` = @ChatManagementParentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ChatManagementParentWhere" + i, platformSettingWhereList[i].ChatManagementParent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].MessageManagementParent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_parent` = @MessageManagementParentWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_parent` = @MessageManagementParentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementParentWhere" + i, platformSettingWhereList[i].MessageManagementParent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].MessageManagementSendParent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_send_parent` = @MessageManagementSendParentWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_send_parent` = @MessageManagementSendParentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementSendParentWhere" + i, platformSettingWhereList[i].MessageManagementSendParent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].ConfigurationManagerParent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `configuration_manager_parent` = @ConfigurationManagerParentWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `configuration_manager_parent` = @ConfigurationManagerParentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ConfigurationManagerParentWhere" + i, platformSettingWhereList[i].ConfigurationManagerParent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].PasswordManagementParent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `password_management_parent` = @PasswordManagementParentWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `password_management_parent` = @PasswordManagementParentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@PasswordManagementParentWhere" + i, platformSettingWhereList[i].PasswordManagementParent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].MessageManagementStudent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_student` = @MessageManagementStudentWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_student` = @MessageManagementStudentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementStudentWhere" + i, platformSettingWhereList[i].MessageManagementStudent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].MessageManagementSendStudent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_send_student` = @MessageManagementSendStudentWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_send_student` = @MessageManagementSendStudentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementSendStudentWhere" + i, platformSettingWhereList[i].MessageManagementSendStudent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].ProgressManagementStudent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `progress_management_student` = @ProgressManagementStudentWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `progress_management_student` = @ProgressManagementStudentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ProgressManagementStudentWhere" + i, platformSettingWhereList[i].ProgressManagementStudent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].ChatManagementStudent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `chat_management_student` = @ChatManagementStudentWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `chat_management_student` = @ChatManagementStudentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ChatManagementStudentWhere" + i, platformSettingWhereList[i].ChatManagementStudent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].TeamsManagementStudent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teams_management_student` = @TeamsManagementStudentWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `teams_management_student` = @TeamsManagementStudentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@TeamsManagementStudentWhere" + i, platformSettingWhereList[i].TeamsManagementStudent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].MicrosoftSSOManagement))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `microsoft_sso_management` = @MicrosoftSSOManagementWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `microsoft_sso_management` = @MicrosoftSSOManagementWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MicrosoftSSOManagementWhere" + i, platformSettingWhereList[i].MicrosoftSSOManagement);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].InternalLoginManagement))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `internal_login_management` = @InternalLoginManagementWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `internal_login_management` = @InternalLoginManagementWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@InternalLoginManagementWhere" + i, platformSettingWhereList[i].InternalLoginManagement);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].InternalPasswordReset))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `internal_password_reset` = @InternalPasswordResetWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `internal_password_reset` = @InternalPasswordResetWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@InternalPasswordResetWhere" + i, platformSettingWhereList[i].InternalPasswordReset);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @ModifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @ModifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ModifiedByWhere" + i, platformSettingWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @CreatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @CreatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@CreatedAtWhere" + i, platformSettingWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(platformSettingWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @UpdatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @UpdatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@UpdatedAtWhere" + i, platformSettingWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(platformSettingList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlatformSettingModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlatformSettingModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformSettingModel>();
            }
            finally
            {
                {
                    if (_connection != null)
                    {
                        dbConn.CloseConnection(_connection);
                    }
                }
            }
            return response;
        }

        public List<PlatformSettingModel> Delete(string connectionString, PlatformSettingModel platformSetting)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformSettingModel> response = new List<PlatformSettingModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM platform_settings  ";
                if (platformSetting != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(platformSetting.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @Id");
                        else
                            sql = String.Concat(sql, " `id` = @Id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@Id", platformSetting.Id);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.SchoolManagementAdmin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `school_management_admin` = @SchoolManagementAdmin");
                        else
                            sql = String.Concat(sql, " `school_management_admin` = @SchoolManagementAdmin");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@SchoolManagementAdmin", platformSetting.SchoolManagementAdmin);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.EnrollmentManagementAdmin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `enrollment_management_admin` = @EnrollmentManagementAdmin");
                        else
                            sql = String.Concat(sql, " `enrollment_management_admin` = @EnrollmentManagementAdmin");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@EnrollmentManagementAdmin", platformSetting.EnrollmentManagementAdmin);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.UserManagementAdmin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_management_admin` = @UserManagementAdmin");
                        else
                            sql = String.Concat(sql, " `user_management_admin` = @UserManagementAdmin");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@UserManagementAdmin", platformSetting.UserManagementAdmin);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.MessageManagementAdmin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_admin` = @MessageManagementAdmin");
                        else
                            sql = String.Concat(sql, " `message_management_admin` = @MessageManagementAdmin");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementAdmin", platformSetting.MessageManagementAdmin);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendAdmin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_send_admin` = @MessageManagementSendAdmin");
                        else
                            sql = String.Concat(sql, " `message_management_send_admin` = @MessageManagementSendAdmin");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementSendAdmin", platformSetting.MessageManagementSendAdmin);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerAdmin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `configuration_manager_admin` = @ConfigurationManagerAdmin");
                        else
                            sql = String.Concat(sql, " `configuration_manager_admin` = @ConfigurationManagerAdmin");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ConfigurationManagerAdmin", platformSetting.ConfigurationManagerAdmin);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.PasswordManagementAdmin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `password_management_admin` = @PasswordManagementAdmin");
                        else
                            sql = String.Concat(sql, " `password_management_admin` = @PasswordManagementAdmin");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@PasswordManagementAdmin", platformSetting.PasswordManagementAdmin);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.SchoolManagementAdminOre))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `school_management_admin_ore` = @SchoolManagementAdminOre");
                        else
                            sql = String.Concat(sql, " `school_management_admin_ore` = @SchoolManagementAdminOre");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@SchoolManagementAdminOre", platformSetting.SchoolManagementAdminOre);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.EnrollmentManagementAdminOre))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre");
                        else
                            sql = String.Concat(sql, " `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOre", platformSetting.EnrollmentManagementAdminOre);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.UserManagementAdminOre))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_management_admin_ore` = @UserManagementAdminOre");
                        else
                            sql = String.Concat(sql, " `user_management_admin_ore` = @UserManagementAdminOre");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@UserManagementAdminOre", platformSetting.UserManagementAdminOre);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.MessageManagementAdminOre))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_admin_ore` = @MessageManagementAdminOre");
                        else
                            sql = String.Concat(sql, " `message_management_admin_ore` = @MessageManagementAdminOre");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementAdminOre", platformSetting.MessageManagementAdminOre);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendAdminOre))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_send_admin_ore` = @MessageManagementSendAdminOre");
                        else
                            sql = String.Concat(sql, " `message_management_send_admin_ore` = @MessageManagementSendAdminOre");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementSendAdminOre", platformSetting.MessageManagementSendAdminOre);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerAdminOre))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre");
                        else
                            sql = String.Concat(sql, " `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOre", platformSetting.ConfigurationManagerAdminOre);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.PasswordManagementAdminOre))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `password_management_admin_ore` = @PasswordManagementAdminOre");
                        else
                            sql = String.Concat(sql, " `password_management_admin_ore` = @PasswordManagementAdminOre");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@PasswordManagementAdminOre", platformSetting.PasswordManagementAdminOre);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.UserManagementPrincipal))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_management_principal` = @UserManagementPrincipal");
                        else
                            sql = String.Concat(sql, " `user_management_principal` = @UserManagementPrincipal");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@UserManagementPrincipal", platformSetting.UserManagementPrincipal);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.MessageManagementPrincipal))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_principal` = @MessageManagementPrincipal");
                        else
                            sql = String.Concat(sql, " `message_management_principal` = @MessageManagementPrincipal");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementPrincipal", platformSetting.MessageManagementPrincipal);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendPrincipal))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_send_principal` = @MessageManagementSendPrincipal");
                        else
                            sql = String.Concat(sql, " `message_management_send_principal` = @MessageManagementSendPrincipal");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementSendPrincipal", platformSetting.MessageManagementSendPrincipal);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerPrincipal))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `configuration_manager_principal` = @ConfigurationManagerPrincipal");
                        else
                            sql = String.Concat(sql, " `configuration_manager_principal` = @ConfigurationManagerPrincipal");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipal", platformSetting.ConfigurationManagerPrincipal);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.PasswordManagementPrincipal))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `password_management_principal` = @PasswordManagementPrincipal");
                        else
                            sql = String.Concat(sql, " `password_management_principal` = @PasswordManagementPrincipal");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@PasswordManagementPrincipal", platformSetting.PasswordManagementPrincipal);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.UserManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_management_teacher` = @UserManagementTeacher");
                        else
                            sql = String.Concat(sql, " `user_management_teacher` = @UserManagementTeacher");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@UserManagementTeacher", platformSetting.UserManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.GroupInternalManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `group_internal_management_teacher` = @GroupInternalManagementTeacher");
                        else
                            sql = String.Concat(sql, " `group_internal_management_teacher` = @GroupInternalManagementTeacher");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@GroupInternalManagementTeacher", platformSetting.GroupInternalManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.InvitationManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `invitation_management_teacher` = @InvitationManagementTeacher");
                        else
                            sql = String.Concat(sql, " `invitation_management_teacher` = @InvitationManagementTeacher");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@InvitationManagementTeacher", platformSetting.InvitationManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.InvitationTeacherToParentManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher");
                        else
                            sql = String.Concat(sql, " `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacher", platformSetting.InvitationTeacherToParentManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.ChatManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `chat_management_teacher` = @ChatManagementTeacher");
                        else
                            sql = String.Concat(sql, " `chat_management_teacher` = @ChatManagementTeacher");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ChatManagementTeacher", platformSetting.ChatManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.MessageManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_teacher` = @MessageManagementTeacher");
                        else
                            sql = String.Concat(sql, " `message_management_teacher` = @MessageManagementTeacher");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementTeacher", platformSetting.MessageManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_send_teacher` = @MessageManagementSendTeacher");
                        else
                            sql = String.Concat(sql, " `message_management_send_teacher` = @MessageManagementSendTeacher");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementSendTeacher", platformSetting.MessageManagementSendTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.TeamMeetsManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `team_meets_management_teacher` = @TeamMeetsManagementTeacher");
                        else
                            sql = String.Concat(sql, " `team_meets_management_teacher` = @TeamMeetsManagementTeacher");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacher", platformSetting.TeamMeetsManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `configuration_manager_teacher` = @ConfigurationManagerTeacher");
                        else
                            sql = String.Concat(sql, " `configuration_manager_teacher` = @ConfigurationManagerTeacher");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ConfigurationManagerTeacher", platformSetting.ConfigurationManagerTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.InvitationTeacherToStudentManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher");
                        else
                            sql = String.Concat(sql, " `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacher", platformSetting.InvitationTeacherToStudentManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.PasswordManagementTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `password_management_teacher` = @PasswordManagementTeacher");
                        else
                            sql = String.Concat(sql, " `password_management_teacher` = @PasswordManagementTeacher");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@PasswordManagementTeacher", platformSetting.PasswordManagementTeacher);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.ProgressManagementParent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `progress_management_parent` = @ProgressManagementParent");
                        else
                            sql = String.Concat(sql, " `progress_management_parent` = @ProgressManagementParent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ProgressManagementParent", platformSetting.ProgressManagementParent);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.InvitationManagementParent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `invitation_management_parent` = @InvitationManagementParent");
                        else
                            sql = String.Concat(sql, " `invitation_management_parent` = @InvitationManagementParent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@InvitationManagementParent", platformSetting.InvitationManagementParent);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.ChatManagementParent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `chat_management_parent` = @ChatManagementParent");
                        else
                            sql = String.Concat(sql, " `chat_management_parent` = @ChatManagementParent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ChatManagementParent", platformSetting.ChatManagementParent);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.MessageManagementParent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_parent` = @MessageManagementParent");
                        else
                            sql = String.Concat(sql, " `message_management_parent` = @MessageManagementParent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementParent", platformSetting.MessageManagementParent);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendParent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_send_parent` = @MessageManagementSendParent");
                        else
                            sql = String.Concat(sql, " `message_management_send_parent` = @MessageManagementSendParent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementSendParent", platformSetting.MessageManagementSendParent);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.ConfigurationManagerParent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `configuration_manager_parent` = @ConfigurationManagerParent");
                        else
                            sql = String.Concat(sql, " `configuration_manager_parent` = @ConfigurationManagerParent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ConfigurationManagerParent", platformSetting.ConfigurationManagerParent);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.PasswordManagementParent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `password_management_parent` = @PasswordManagementParent");
                        else
                            sql = String.Concat(sql, " `password_management_parent` = @PasswordManagementParent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@PasswordManagementParent", platformSetting.PasswordManagementParent);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.MessageManagementStudent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_student` = @MessageManagementStudent");
                        else
                            sql = String.Concat(sql, " `message_management_student` = @MessageManagementStudent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementStudent", platformSetting.MessageManagementStudent);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.MessageManagementSendStudent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `message_management_send_student` = @MessageManagementSendStudent");
                        else
                            sql = String.Concat(sql, " `message_management_send_student` = @MessageManagementSendStudent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MessageManagementSendStudent", platformSetting.MessageManagementSendStudent);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.ProgressManagementStudent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `progress_management_student` = @ProgressManagementStudent");
                        else
                            sql = String.Concat(sql, " `progress_management_student` = @ProgressManagementStudent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ProgressManagementStudent", platformSetting.ProgressManagementStudent);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.ChatManagementStudent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `chat_management_student` = @ChatManagementStudent");
                        else
                            sql = String.Concat(sql, " `chat_management_student` = @ChatManagementStudent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ChatManagementStudent", platformSetting.ChatManagementStudent);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.TeamsManagementStudent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teams_management_student` = @TeamsManagementStudent");
                        else
                            sql = String.Concat(sql, " `teams_management_student` = @TeamsManagementStudent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@TeamsManagementStudent", platformSetting.TeamsManagementStudent);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.MicrosoftSSOManagement))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `microsoft_sso_management` = @MicrosoftSSOManagement");
                        else
                            sql = String.Concat(sql, " `microsoft_sso_management` = @MicrosoftSSOManagement");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@MicrosoftSSOManagement", platformSetting.MicrosoftSSOManagement);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.InternalLoginManagement))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `internal_login_management` = @InternalLoginManagement");
                        else
                            sql = String.Concat(sql, " `internal_login_management` = @InternalLoginManagement");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@InternalLoginManagement", platformSetting.InternalLoginManagement);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.InternalPasswordReset))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `internal_password_reset` = @InternalPasswordReset");
                        else
                            sql = String.Concat(sql, " `internal_password_reset` = @InternalPasswordReset");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@InternalPasswordReset", platformSetting.InternalPasswordReset);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @ModifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @ModifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ModifiedBy", platformSetting.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @CreatedAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @CreatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@CreatedAt", platformSetting.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(platformSetting.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @UpdatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @UpdatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@UpdatedAt", platformSetting.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(platformSetting);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformSettingModel>();
            }
            finally
            {
                if (_connection != null)
                {
                    dbConn.CloseConnection(_connection);
                }
            }
            return response;
        }

        public List<PlatformSettingModel> Delete(string connectionString, List<PlatformSettingModel> platformSetting)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformSettingModel> response = new List<PlatformSettingModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < platformSetting.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM platform_settings  ";
                                if (platformSetting[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(platformSetting[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @Id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @Id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@Id" + i, platformSetting[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].SchoolManagementAdmin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `school_management_admin` = @SchoolManagementAdmin" + i);
                                        else
                                            sql = String.Concat(sql, " `school_management_admin` = @SchoolManagementAdmin" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@SchoolManagementAdmin" + i, platformSetting[i].SchoolManagementAdmin);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].EnrollmentManagementAdmin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `enrollment_management_admin` = @EnrollmentManagementAdmin" + i);
                                        else
                                            sql = String.Concat(sql, " `enrollment_management_admin` = @EnrollmentManagementAdmin" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@EnrollmentManagementAdmin" + i, platformSetting[i].EnrollmentManagementAdmin);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].UserManagementAdmin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_management_admin` = @UserManagementAdmin" + i);
                                        else
                                            sql = String.Concat(sql, " `user_management_admin` = @UserManagementAdmin" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@UserManagementAdmin" + i, platformSetting[i].UserManagementAdmin);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].MessageManagementAdmin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_admin` = @MessageManagementAdmin" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_admin` = @MessageManagementAdmin" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementAdmin" + i, platformSetting[i].MessageManagementAdmin);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].MessageManagementSendAdmin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_send_admin` = @MessageManagementSendAdmin" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_send_admin` = @MessageManagementSendAdmin" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementSendAdmin" + i, platformSetting[i].MessageManagementSendAdmin);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].ConfigurationManagerAdmin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `configuration_manager_admin` = @ConfigurationManagerAdmin" + i);
                                        else
                                            sql = String.Concat(sql, " `configuration_manager_admin` = @ConfigurationManagerAdmin" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ConfigurationManagerAdmin" + i, platformSetting[i].ConfigurationManagerAdmin);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].PasswordManagementAdmin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `password_management_admin` = @PasswordManagementAdmin" + i);
                                        else
                                            sql = String.Concat(sql, " `password_management_admin` = @PasswordManagementAdmin" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@PasswordManagementAdmin" + i, platformSetting[i].PasswordManagementAdmin);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].SchoolManagementAdminOre))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `school_management_admin_ore` = @SchoolManagementAdminOre" + i);
                                        else
                                            sql = String.Concat(sql, " `school_management_admin_ore` = @SchoolManagementAdminOre" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@SchoolManagementAdminOre" + i, platformSetting[i].SchoolManagementAdminOre);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].EnrollmentManagementAdminOre))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre" + i);
                                        else
                                            sql = String.Concat(sql, " `enrollment_management_admin_ore` = @EnrollmentManagementAdminOre" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@EnrollmentManagementAdminOre" + i, platformSetting[i].EnrollmentManagementAdminOre);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].UserManagementAdminOre))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_management_admin_ore` = @UserManagementAdminOre" + i);
                                        else
                                            sql = String.Concat(sql, " `user_management_admin_ore` = @UserManagementAdminOre" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@UserManagementAdminOre" + i, platformSetting[i].UserManagementAdminOre);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].MessageManagementAdminOre))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_admin_ore` = @MessageManagementAdminOre" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_admin_ore` = @MessageManagementAdminOre" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementAdminOre" + i, platformSetting[i].MessageManagementAdminOre);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].MessageManagementSendAdminOre))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_send_admin_ore` = @MessageManagementSendAdminOre" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_send_admin_ore` = @MessageManagementSendAdminOre" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementSendAdminOre" + i, platformSetting[i].MessageManagementSendAdminOre);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].ConfigurationManagerAdminOre))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre" + i);
                                        else
                                            sql = String.Concat(sql, " `configuration_manager_admin_ore` = @ConfigurationManagerAdminOre" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ConfigurationManagerAdminOre" + i, platformSetting[i].ConfigurationManagerAdminOre);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].PasswordManagementAdminOre))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `password_management_admin_ore` = @PasswordManagementAdminOre" + i);
                                        else
                                            sql = String.Concat(sql, " `password_management_admin_ore` = @PasswordManagementAdminOre" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@PasswordManagementAdminOre" + i, platformSetting[i].PasswordManagementAdminOre);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].UserManagementPrincipal))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_management_principal` = @UserManagementPrincipal" + i);
                                        else
                                            sql = String.Concat(sql, " `user_management_principal` = @UserManagementPrincipal" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@UserManagementPrincipal" + i, platformSetting[i].UserManagementPrincipal);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].MessageManagementPrincipal))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_principal` = @MessageManagementPrincipal" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_principal` = @MessageManagementPrincipal" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementPrincipal" + i, platformSetting[i].MessageManagementPrincipal);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].MessageManagementSendPrincipal))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_send_principal` = @MessageManagementSendPrincipal" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_send_principal` = @MessageManagementSendPrincipal" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementSendPrincipal" + i, platformSetting[i].MessageManagementSendPrincipal);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].ConfigurationManagerPrincipal))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `configuration_manager_principal` = @ConfigurationManagerPrincipal" + i);
                                        else
                                            sql = String.Concat(sql, " `configuration_manager_principal` = @ConfigurationManagerPrincipal" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ConfigurationManagerPrincipal" + i, platformSetting[i].ConfigurationManagerPrincipal);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].PasswordManagementPrincipal))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `password_management_principal` = @PasswordManagementPrincipal" + i);
                                        else
                                            sql = String.Concat(sql, " `password_management_principal` = @PasswordManagementPrincipal" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@PasswordManagementPrincipal" + i, platformSetting[i].PasswordManagementPrincipal);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].UserManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_management_teacher` = @UserManagementTeacher" + i);
                                        else
                                            sql = String.Concat(sql, " `user_management_teacher` = @UserManagementTeacher" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@UserManagementTeacher" + i, platformSetting[i].UserManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].GroupInternalManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `group_internal_management_teacher` = @GroupInternalManagementTeacher" + i);
                                        else
                                            sql = String.Concat(sql, " `group_internal_management_teacher` = @GroupInternalManagementTeacher" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@GroupInternalManagementTeacher" + i, platformSetting[i].GroupInternalManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].InvitationManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `invitation_management_teacher` = @InvitationManagementTeacher" + i);
                                        else
                                            sql = String.Concat(sql, " `invitation_management_teacher` = @InvitationManagementTeacher" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@InvitationManagementTeacher" + i, platformSetting[i].InvitationManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].InvitationTeacherToParentManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher" + i);
                                        else
                                            sql = String.Concat(sql, " `invitation_teacher_to_parent_management_teacher` = @InvitationTeacherToParentManagementTeacher" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@InvitationTeacherToParentManagementTeacher" + i, platformSetting[i].InvitationTeacherToParentManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].ChatManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `chat_management_teacher` = @ChatManagementTeacher" + i);
                                        else
                                            sql = String.Concat(sql, " `chat_management_teacher` = @ChatManagementTeacher" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ChatManagementTeacher" + i, platformSetting[i].ChatManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].MessageManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_teacher` = @MessageManagementTeacher" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_teacher` = @MessageManagementTeacher" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementTeacher" + i, platformSetting[i].MessageManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].MessageManagementSendTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_send_teacher` = @MessageManagementSendTeacher" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_send_teacher` = @MessageManagementSendTeacher" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementSendTeacher" + i, platformSetting[i].MessageManagementSendTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].TeamMeetsManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `team_meets_management_teacher` = @TeamMeetsManagementTeacher" + i);
                                        else
                                            sql = String.Concat(sql, " `team_meets_management_teacher` = @TeamMeetsManagementTeacher" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@TeamMeetsManagementTeacher" + i, platformSetting[i].TeamMeetsManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].ConfigurationManagerTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `configuration_manager_teacher` = @ConfigurationManagerTeacher" + i);
                                        else
                                            sql = String.Concat(sql, " `configuration_manager_teacher` = @ConfigurationManagerTeacher" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ConfigurationManagerTeacher" + i, platformSetting[i].ConfigurationManagerTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].InvitationTeacherToStudentManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher" + i);
                                        else
                                            sql = String.Concat(sql, " `invitation_teacher_to_student_management_teacher` = @InvitationTeacherToStudentManagementTeacher" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@InvitationTeacherToStudentManagementTeacher" + i, platformSetting[i].InvitationTeacherToStudentManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].PasswordManagementTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `password_management_teacher` = @PasswordManagementTeacher" + i);
                                        else
                                            sql = String.Concat(sql, " `password_management_teacher` = @PasswordManagementTeacher" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@PasswordManagementTeacher" + i, platformSetting[i].PasswordManagementTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].ProgressManagementParent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `progress_management_parent` = @ProgressManagementParent" + i);
                                        else
                                            sql = String.Concat(sql, " `progress_management_parent` = @ProgressManagementParent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ProgressManagementParent" + i, platformSetting[i].ProgressManagementParent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].InvitationManagementParent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `invitation_management_parent` = @InvitationManagementParent" + i);
                                        else
                                            sql = String.Concat(sql, " `invitation_management_parent` = @InvitationManagementParent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@InvitationManagementParent" + i, platformSetting[i].InvitationManagementParent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].ChatManagementParent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `chat_management_parent` = @ChatManagementParent" + i);
                                        else
                                            sql = String.Concat(sql, " `chat_management_parent` = @ChatManagementParent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ChatManagementParent" + i, platformSetting[i].ChatManagementParent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].MessageManagementParent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_parent` = @MessageManagementParent" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_parent` = @MessageManagementParent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementParent" + i, platformSetting[i].MessageManagementParent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].MessageManagementSendParent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_send_parent` = @MessageManagementSendParent" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_send_parent` = @MessageManagementSendParent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementSendParent" + i, platformSetting[i].MessageManagementSendParent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].ConfigurationManagerParent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `configuration_manager_parent` = @ConfigurationManagerParent" + i);
                                        else
                                            sql = String.Concat(sql, " `configuration_manager_parent` = @ConfigurationManagerParent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ConfigurationManagerParent" + i, platformSetting[i].ConfigurationManagerParent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].PasswordManagementParent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `password_management_parent` = @PasswordManagementParent" + i);
                                        else
                                            sql = String.Concat(sql, " `password_management_parent` = @PasswordManagementParent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@PasswordManagementParent" + i, platformSetting[i].PasswordManagementParent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].MessageManagementStudent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_student` = @MessageManagementStudent" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_student` = @MessageManagementStudent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementStudent" + i, platformSetting[i].MessageManagementStudent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].MessageManagementSendStudent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `message_management_send_student` = @MessageManagementSendStudent" + i);
                                        else
                                            sql = String.Concat(sql, " `message_management_send_student` = @MessageManagementSendStudent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MessageManagementSendStudent" + i, platformSetting[i].MessageManagementSendStudent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].ProgressManagementStudent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `progress_management_student` = @ProgressManagementStudent" + i);
                                        else
                                            sql = String.Concat(sql, " `progress_management_student` = @ProgressManagementStudent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ProgressManagementStudent" + i, platformSetting[i].ProgressManagementStudent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].ChatManagementStudent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `chat_management_student` = @ChatManagementStudent" + i);
                                        else
                                            sql = String.Concat(sql, " `chat_management_student` = @ChatManagementStudent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ChatManagementStudent" + i, platformSetting[i].ChatManagementStudent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].TeamsManagementStudent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teams_management_student` = @TeamsManagementStudent" + i);
                                        else
                                            sql = String.Concat(sql, " `teams_management_student` = @TeamsManagementStudent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@TeamsManagementStudent" + i, platformSetting[i].TeamsManagementStudent);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].MicrosoftSSOManagement))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `microsoft_sso_management` = @MicrosoftSSOManagement" + i);
                                        else
                                            sql = String.Concat(sql, " `microsoft_sso_management` = @MicrosoftSSOManagement" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@MicrosoftSSOManagement" + i, platformSetting[i].MicrosoftSSOManagement);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].InternalLoginManagement))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `internal_login_management` = @InternalLoginManagement" + i);
                                        else
                                            sql = String.Concat(sql, " `internal_login_management` = @InternalLoginManagement" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@InternalLoginManagement" + i, platformSetting[i].InternalLoginManagement);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].InternalPasswordReset))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `internal_password_reset` = @InternalPasswordReset" + i);
                                        else
                                            sql = String.Concat(sql, " `internal_password_reset` = @InternalPasswordReset" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@InternalPasswordReset" + i, platformSetting[i].InternalPasswordReset);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @ModifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @ModifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ModifiedBy" + i, platformSetting[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @CreatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @CreatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@CreatedAt" + i, platformSetting[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(platformSetting[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @UpdatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @UpdatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@UpdatedAt" + i, platformSetting[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(platformSetting[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlatformSettingModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlatformSettingModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformSettingModel>();
            }
            finally
            {
                {
                    if (_connection != null)
                    {
                        dbConn.CloseConnection(_connection);
                    }
                }
            }
            return response;
        }
    }
}
