using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Emails;
namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Emails
{
    class EmailSettingDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, EmailSettingModel> ReadD(string connectionString, EmailSettingModel emailSetting, bool firstOne, bool orderByLastDate, EmailSettingModel emailSettingWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailSettingModel> response = new Dictionary<string, EmailSettingModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `mail_domain_com`, `email_sender`, `sender_user`, `sender_password`, `email_port`, `default_credential`, `enable_ssl`, `modified_by`, `created_at`, `updated_at`  FROM em_email_settings ";
                {
                    if (emailSetting != null)
                    {
                        if (!String.IsNullOrEmpty(emailSetting.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", emailSetting.Id);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.MailDomainCom))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `mail_domain_com` = @mailDomainCom ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `mail_domain_com` = @mailDomainCom ");
                            }
                            cmd.Parameters.AddWithValue("@mailDomainCom", emailSetting.MailDomainCom);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.EmailSender))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `email_sender` = @emailSender ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `email_sender` = @emailSender ");
                            }
                            cmd.Parameters.AddWithValue("@emailSender", emailSetting.EmailSender);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.SenderUser))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `sender_user` = @senderUser ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `sender_user` = @senderUser ");
                            }
                            cmd.Parameters.AddWithValue("@senderUser", emailSetting.SenderUser);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.SenderPassword))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `sender_password` = @senderPassword ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `sender_password` = @senderPassword ");
                            }
                            cmd.Parameters.AddWithValue("@senderPassword", emailSetting.SenderPassword);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.EmailPort))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `email_port` = @emailPort ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `email_port` = @emailPort ");
                            }
                            cmd.Parameters.AddWithValue("@emailPort", emailSetting.EmailPort);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.DefaultCredential))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `default_credential` = @defaultCredential ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `default_credential` = @defaultCredential ");
                            }
                            cmd.Parameters.AddWithValue("@defaultCredential", emailSetting.DefaultCredential);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.EnableSsl))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `enable_ssl` = @enableSsl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `enable_ssl` = @enableSsl ");
                            }
                            cmd.Parameters.AddWithValue("@enableSsl", emailSetting.EnableSsl);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", emailSetting.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", emailSetting.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", emailSetting.UpdatedAt);
                        }
                    }
                    if (emailSettingWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + emailSettingWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.MailDomainCom))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `mail_domain_com` LIKE @mailDomainComWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `mail_domain_com` LIKE @mailDomainComWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@mailDomainComWhereLike", "%" + emailSettingWhereLike.MailDomainCom + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.EmailSender))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `email_sender` LIKE @emailSenderWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `email_sender` LIKE @emailSenderWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@emailSenderWhereLike", "%" + emailSettingWhereLike.EmailSender + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.SenderUser))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `sender_user` LIKE @senderUserWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `sender_user` LIKE @senderUserWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@senderUserWhereLike", "%" + emailSettingWhereLike.SenderUser + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.SenderPassword))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `sender_password` LIKE @senderPasswordWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `sender_password` LIKE @senderPasswordWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@senderPasswordWhereLike", "%" + emailSettingWhereLike.SenderPassword + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.EmailPort))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `email_port` LIKE @emailPortWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `email_port` LIKE @emailPortWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@emailPortWhereLike", "%" + emailSettingWhereLike.EmailPort + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.DefaultCredential))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `default_credential` LIKE @defaultCredentialWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `default_credential` LIKE @defaultCredentialWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@defaultCredentialWhereLike", "%" + emailSettingWhereLike.DefaultCredential + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.EnableSsl))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `enable_ssl` LIKE @enableSslWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `enable_ssl` LIKE @enableSslWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@enableSslWhereLike", "%" + emailSettingWhereLike.EnableSsl + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + emailSettingWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + emailSettingWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + emailSettingWhereLike.UpdatedAt + "%");
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
                                emailSetting = new EmailSettingModel();
                                if (!reader.IsDBNull(0))
                                    emailSetting.Id = reader.GetValue(0).ToString();
                                else
                                    emailSetting.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailSetting.MailDomainCom = reader.GetValue(1).ToString();
                                else
                                    emailSetting.MailDomainCom = null;
                                if (!reader.IsDBNull(2))
                                    emailSetting.EmailSender = reader.GetValue(2).ToString();
                                else
                                    emailSetting.EmailSender = null;
                                if (!reader.IsDBNull(3))
                                    emailSetting.SenderUser = reader.GetValue(3).ToString();
                                else
                                    emailSetting.SenderUser = null;
                                if (!reader.IsDBNull(4))
                                    emailSetting.SenderPassword = reader.GetValue(4).ToString();
                                else
                                    emailSetting.SenderPassword = null;
                                if (!reader.IsDBNull(5))
                                    emailSetting.EmailPort = reader.GetValue(5).ToString();
                                else
                                    emailSetting.EmailPort = null;
                                if (!reader.IsDBNull(6))
                                    emailSetting.DefaultCredential = reader.GetValue(6).ToString();
                                else
                                    emailSetting.DefaultCredential = null;
                                if (!reader.IsDBNull(7))
                                    emailSetting.EnableSsl = reader.GetValue(7).ToString();
                                else
                                    emailSetting.EnableSsl = null;
                                if (!reader.IsDBNull(8))
                                    emailSetting.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    emailSetting.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    emailSetting.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    emailSetting.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    emailSetting.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    emailSetting.UpdatedAt = null;
                                response.Add(emailSetting.Id, emailSetting);
                                emailSetting = new EmailSettingModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EmailSettingModel>();
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
                response = new Dictionary<string, EmailSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EmailSettingModel> ReadD(string connectionString, EmailSettingModel emailSetting, bool orderByLastDate, int fromRow, int limit, EmailSettingModel emailSettingWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailSettingModel> response = new Dictionary<string, EmailSettingModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `mail_domain_com`, `email_sender`, `sender_user`, `sender_password`, `email_port`, `default_credential`, `enable_ssl`, `modified_by`, `created_at`, `updated_at`  FROM em_email_settings ";
                {
                    if (emailSetting != null)
                    {
                        if (!String.IsNullOrEmpty(emailSetting.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", emailSetting.Id);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.MailDomainCom))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `mail_domain_com` = @mailDomainCom ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `mail_domain_com` = @mailDomainCom ");
                            }
                            cmd.Parameters.AddWithValue("@mailDomainCom", emailSetting.MailDomainCom);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.EmailSender))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `email_sender` = @emailSender ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `email_sender` = @emailSender ");
                            }
                            cmd.Parameters.AddWithValue("@emailSender", emailSetting.EmailSender);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.SenderUser))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `sender_user` = @senderUser ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `sender_user` = @senderUser ");
                            }
                            cmd.Parameters.AddWithValue("@senderUser", emailSetting.SenderUser);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.SenderPassword))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `sender_password` = @senderPassword ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `sender_password` = @senderPassword ");
                            }
                            cmd.Parameters.AddWithValue("@senderPassword", emailSetting.SenderPassword);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.EmailPort))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `email_port` = @emailPort ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `email_port` = @emailPort ");
                            }
                            cmd.Parameters.AddWithValue("@emailPort", emailSetting.EmailPort);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.DefaultCredential))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `default_credential` = @defaultCredential ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `default_credential` = @defaultCredential ");
                            }
                            cmd.Parameters.AddWithValue("@defaultCredential", emailSetting.DefaultCredential);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.EnableSsl))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `enable_ssl` = @enableSsl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `enable_ssl` = @enableSsl ");
                            }
                            cmd.Parameters.AddWithValue("@enableSsl", emailSetting.EnableSsl);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", emailSetting.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", emailSetting.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", emailSetting.UpdatedAt);
                        }
                    }
                    if (emailSettingWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + emailSettingWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.MailDomainCom))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `mail_domain_com` LIKE @mailDomainComWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `mail_domain_com` LIKE @mailDomainComWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@mailDomainComWhereLike", "%" + emailSettingWhereLike.MailDomainCom + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.EmailSender))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `email_sender` LIKE @emailSenderWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `email_sender` LIKE @emailSenderWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@emailSenderWhereLike", "%" + emailSettingWhereLike.EmailSender + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.SenderUser))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `sender_user` LIKE @senderUserWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `sender_user` LIKE @senderUserWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@senderUserWhereLike", "%" + emailSettingWhereLike.SenderUser + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.SenderPassword))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `sender_password` LIKE @senderPasswordWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `sender_password` LIKE @senderPasswordWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@senderPasswordWhereLike", "%" + emailSettingWhereLike.SenderPassword + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.EmailPort))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `email_port` LIKE @emailPortWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `email_port` LIKE @emailPortWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@emailPortWhereLike", "%" + emailSettingWhereLike.EmailPort + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.DefaultCredential))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `default_credential` LIKE @defaultCredentialWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `default_credential` LIKE @defaultCredentialWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@defaultCredentialWhereLike", "%" + emailSettingWhereLike.DefaultCredential + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.EnableSsl))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `enable_ssl` LIKE @enableSslWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `enable_ssl` LIKE @enableSslWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@enableSslWhereLike", "%" + emailSettingWhereLike.EnableSsl + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + emailSettingWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + emailSettingWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + emailSettingWhereLike.UpdatedAt + "%");
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
                                emailSetting = new EmailSettingModel();
                                if (!reader.IsDBNull(0))
                                    emailSetting.Id = reader.GetValue(0).ToString();
                                else
                                    emailSetting.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailSetting.MailDomainCom = reader.GetValue(1).ToString();
                                else
                                    emailSetting.MailDomainCom = null;
                                if (!reader.IsDBNull(2))
                                    emailSetting.EmailSender = reader.GetValue(2).ToString();
                                else
                                    emailSetting.EmailSender = null;
                                if (!reader.IsDBNull(3))
                                    emailSetting.SenderUser = reader.GetValue(3).ToString();
                                else
                                    emailSetting.SenderUser = null;
                                if (!reader.IsDBNull(4))
                                    emailSetting.SenderPassword = reader.GetValue(4).ToString();
                                else
                                    emailSetting.SenderPassword = null;
                                if (!reader.IsDBNull(5))
                                    emailSetting.EmailPort = reader.GetValue(5).ToString();
                                else
                                    emailSetting.EmailPort = null;
                                if (!reader.IsDBNull(6))
                                    emailSetting.DefaultCredential = reader.GetValue(6).ToString();
                                else
                                    emailSetting.DefaultCredential = null;
                                if (!reader.IsDBNull(7))
                                    emailSetting.EnableSsl = reader.GetValue(7).ToString();
                                else
                                    emailSetting.EnableSsl = null;
                                if (!reader.IsDBNull(8))
                                    emailSetting.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    emailSetting.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    emailSetting.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    emailSetting.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    emailSetting.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    emailSetting.UpdatedAt = null;
                                response.Add(emailSetting.Id, emailSetting);
                                emailSetting = new EmailSettingModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EmailSettingModel>();
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
                response = new Dictionary<string, EmailSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EmailSettingModel> ReadD(string connectionString, List<EmailSettingModel> emailSettingWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailSettingModel> response = new Dictionary<string, EmailSettingModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `mail_domain_com`, `email_sender`, `sender_user`, `sender_password`, `email_port`, `default_credential`, `enable_ssl`, `modified_by`, `created_at`, `updated_at`  FROM em_email_settings ";
                if (emailSettingWhere != null && emailSettingWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailSettingWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailSettingWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].MailDomainCom))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `mail_domain_com` = @mailDomainCom" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@mailDomainCom" + i, emailSettingWhere[i].MailDomainCom);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].EmailSender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email_sender` = @emailSender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@emailSender" + i, emailSettingWhere[i].EmailSender);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].SenderUser))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sender_user` = @senderUser" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@senderUser" + i, emailSettingWhere[i].SenderUser);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].SenderPassword))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sender_password` = @senderPassword" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@senderPassword" + i, emailSettingWhere[i].SenderPassword);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].EmailPort))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email_port` = @emailPort" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@emailPort" + i, emailSettingWhere[i].EmailPort);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].DefaultCredential))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `default_credential` = @defaultCredential" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@defaultCredential" + i, emailSettingWhere[i].DefaultCredential);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].EnableSsl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enable_ssl` = @enableSsl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@enableSsl" + i, emailSettingWhere[i].EnableSsl);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailSettingWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailSettingWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailSettingWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailSettingWhere.Count - 1)
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
                            EmailSettingModel emailSetting = new EmailSettingModel(); if (!reader.IsDBNull(0))
                                emailSetting.Id = reader.GetValue(0).ToString();
                            else
                                emailSetting.Id = null;
                            if (!reader.IsDBNull(1))
                                emailSetting.MailDomainCom = reader.GetValue(1).ToString();
                            else
                                emailSetting.MailDomainCom = null;
                            if (!reader.IsDBNull(2))
                                emailSetting.EmailSender = reader.GetValue(2).ToString();
                            else
                                emailSetting.EmailSender = null;
                            if (!reader.IsDBNull(3))
                                emailSetting.SenderUser = reader.GetValue(3).ToString();
                            else
                                emailSetting.SenderUser = null;
                            if (!reader.IsDBNull(4))
                                emailSetting.SenderPassword = reader.GetValue(4).ToString();
                            else
                                emailSetting.SenderPassword = null;
                            if (!reader.IsDBNull(5))
                                emailSetting.EmailPort = reader.GetValue(5).ToString();
                            else
                                emailSetting.EmailPort = null;
                            if (!reader.IsDBNull(6))
                                emailSetting.DefaultCredential = reader.GetValue(6).ToString();
                            else
                                emailSetting.DefaultCredential = null;
                            if (!reader.IsDBNull(7))
                                emailSetting.EnableSsl = reader.GetValue(7).ToString();
                            else
                                emailSetting.EnableSsl = null;
                            if (!reader.IsDBNull(8))
                                emailSetting.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                emailSetting.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                emailSetting.CreatedAt = reader.GetValue(9).ToString();
                            else
                                emailSetting.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                emailSetting.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                emailSetting.UpdatedAt = null;
                            response.Add(emailSetting.Id, emailSetting);
                            emailSetting = new EmailSettingModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EmailSettingModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EmailSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EmailSettingModel> ReadD(string connectionString, List<EmailSettingModel> emailSettingWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailSettingModel> response = new Dictionary<string, EmailSettingModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `mail_domain_com`, `email_sender`, `sender_user`, `sender_password`, `email_port`, `default_credential`, `enable_ssl`, `modified_by`, `created_at`, `updated_at`  FROM em_email_settings ";
                if (emailSettingWhere != null && emailSettingWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailSettingWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailSettingWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].MailDomainCom))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `mail_domain_com` = @mailDomainCom" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@mailDomainCom" + i, emailSettingWhere[i].MailDomainCom);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].EmailSender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email_sender` = @emailSender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@emailSender" + i, emailSettingWhere[i].EmailSender);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].SenderUser))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sender_user` = @senderUser" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@senderUser" + i, emailSettingWhere[i].SenderUser);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].SenderPassword))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sender_password` = @senderPassword" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@senderPassword" + i, emailSettingWhere[i].SenderPassword);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].EmailPort))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email_port` = @emailPort" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@emailPort" + i, emailSettingWhere[i].EmailPort);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].DefaultCredential))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `default_credential` = @defaultCredential" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@defaultCredential" + i, emailSettingWhere[i].DefaultCredential);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].EnableSsl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enable_ssl` = @enableSsl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@enableSsl" + i, emailSettingWhere[i].EnableSsl);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailSettingWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailSettingWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailSettingWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailSettingWhere.Count - 1)
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
                            EmailSettingModel emailSetting = new EmailSettingModel(); if (!reader.IsDBNull(0))
                                emailSetting.Id = reader.GetValue(0).ToString();
                            else
                                emailSetting.Id = null;
                            if (!reader.IsDBNull(1))
                                emailSetting.MailDomainCom = reader.GetValue(1).ToString();
                            else
                                emailSetting.MailDomainCom = null;
                            if (!reader.IsDBNull(2))
                                emailSetting.EmailSender = reader.GetValue(2).ToString();
                            else
                                emailSetting.EmailSender = null;
                            if (!reader.IsDBNull(3))
                                emailSetting.SenderUser = reader.GetValue(3).ToString();
                            else
                                emailSetting.SenderUser = null;
                            if (!reader.IsDBNull(4))
                                emailSetting.SenderPassword = reader.GetValue(4).ToString();
                            else
                                emailSetting.SenderPassword = null;
                            if (!reader.IsDBNull(5))
                                emailSetting.EmailPort = reader.GetValue(5).ToString();
                            else
                                emailSetting.EmailPort = null;
                            if (!reader.IsDBNull(6))
                                emailSetting.DefaultCredential = reader.GetValue(6).ToString();
                            else
                                emailSetting.DefaultCredential = null;
                            if (!reader.IsDBNull(7))
                                emailSetting.EnableSsl = reader.GetValue(7).ToString();
                            else
                                emailSetting.EnableSsl = null;
                            if (!reader.IsDBNull(8))
                                emailSetting.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                emailSetting.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                emailSetting.CreatedAt = reader.GetValue(9).ToString();
                            else
                                emailSetting.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                emailSetting.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                emailSetting.UpdatedAt = null;
                            response.Add(emailSetting.Id, emailSetting);
                            emailSetting = new EmailSettingModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EmailSettingModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EmailSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailSettingModel> Read(string connectionString, EmailSettingModel emailSetting, bool firstOne, bool orderByLastDate, EmailSettingModel emailSettingWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailSettingModel> response = new List<EmailSettingModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `mail_domain_com`, `email_sender`, `sender_user`, `sender_password`, `email_port`, `default_credential`, `enable_ssl`, `modified_by`, `created_at`, `updated_at`  FROM em_email_settings ";
                {
                    if (emailSetting != null)
                    {
                        if (!String.IsNullOrEmpty(emailSetting.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", emailSetting.Id);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.MailDomainCom))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `mail_domain_com` = @mailDomainCom ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `mail_domain_com` = @mailDomainCom ");
                            }
                            cmd.Parameters.AddWithValue("@mailDomainCom", emailSetting.MailDomainCom);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.EmailSender))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `email_sender` = @emailSender ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `email_sender` = @emailSender ");
                            }
                            cmd.Parameters.AddWithValue("@emailSender", emailSetting.EmailSender);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.SenderUser))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `sender_user` = @senderUser ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `sender_user` = @senderUser ");
                            }
                            cmd.Parameters.AddWithValue("@senderUser", emailSetting.SenderUser);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.SenderPassword))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `sender_password` = @senderPassword ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `sender_password` = @senderPassword ");
                            }
                            cmd.Parameters.AddWithValue("@senderPassword", emailSetting.SenderPassword);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.EmailPort))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `email_port` = @emailPort ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `email_port` = @emailPort ");
                            }
                            cmd.Parameters.AddWithValue("@emailPort", emailSetting.EmailPort);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.DefaultCredential))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `default_credential` = @defaultCredential ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `default_credential` = @defaultCredential ");
                            }
                            cmd.Parameters.AddWithValue("@defaultCredential", emailSetting.DefaultCredential);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.EnableSsl))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `enable_ssl` = @enableSsl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `enable_ssl` = @enableSsl ");
                            }
                            cmd.Parameters.AddWithValue("@enableSsl", emailSetting.EnableSsl);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", emailSetting.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", emailSetting.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", emailSetting.UpdatedAt);
                        }
                    }
                    if (emailSettingWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + emailSettingWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.MailDomainCom))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `mail_domain_com` LIKE @mailDomainComWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `mail_domain_com` LIKE @mailDomainComWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@mailDomainComWhereLike", "%" + emailSettingWhereLike.MailDomainCom + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.EmailSender))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `email_sender` LIKE @emailSenderWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `email_sender` LIKE @emailSenderWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@emailSenderWhereLike", "%" + emailSettingWhereLike.EmailSender + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.SenderUser))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `sender_user` LIKE @senderUserWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `sender_user` LIKE @senderUserWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@senderUserWhereLike", "%" + emailSettingWhereLike.SenderUser + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.SenderPassword))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `sender_password` LIKE @senderPasswordWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `sender_password` LIKE @senderPasswordWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@senderPasswordWhereLike", "%" + emailSettingWhereLike.SenderPassword + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.EmailPort))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `email_port` LIKE @emailPortWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `email_port` LIKE @emailPortWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@emailPortWhereLike", "%" + emailSettingWhereLike.EmailPort + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.DefaultCredential))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `default_credential` LIKE @defaultCredentialWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `default_credential` LIKE @defaultCredentialWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@defaultCredentialWhereLike", "%" + emailSettingWhereLike.DefaultCredential + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.EnableSsl))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `enable_ssl` LIKE @enableSslWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `enable_ssl` LIKE @enableSslWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@enableSslWhereLike", "%" + emailSettingWhereLike.EnableSsl + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + emailSettingWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + emailSettingWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + emailSettingWhereLike.UpdatedAt + "%");
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
                                emailSetting = new EmailSettingModel();
                                if (!reader.IsDBNull(0))
                                    emailSetting.Id = reader.GetValue(0).ToString();
                                else
                                    emailSetting.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailSetting.MailDomainCom = reader.GetValue(1).ToString();
                                else
                                    emailSetting.MailDomainCom = null;
                                if (!reader.IsDBNull(2))
                                    emailSetting.EmailSender = reader.GetValue(2).ToString();
                                else
                                    emailSetting.EmailSender = null;
                                if (!reader.IsDBNull(3))
                                    emailSetting.SenderUser = reader.GetValue(3).ToString();
                                else
                                    emailSetting.SenderUser = null;
                                if (!reader.IsDBNull(4))
                                    emailSetting.SenderPassword = reader.GetValue(4).ToString();
                                else
                                    emailSetting.SenderPassword = null;
                                if (!reader.IsDBNull(5))
                                    emailSetting.EmailPort = reader.GetValue(5).ToString();
                                else
                                    emailSetting.EmailPort = null;
                                if (!reader.IsDBNull(6))
                                    emailSetting.DefaultCredential = reader.GetValue(6).ToString();
                                else
                                    emailSetting.DefaultCredential = null;
                                if (!reader.IsDBNull(7))
                                    emailSetting.EnableSsl = reader.GetValue(7).ToString();
                                else
                                    emailSetting.EnableSsl = null;
                                if (!reader.IsDBNull(8))
                                    emailSetting.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    emailSetting.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    emailSetting.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    emailSetting.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    emailSetting.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    emailSetting.UpdatedAt = null;
                                response.Add(emailSetting);
                                emailSetting = new EmailSettingModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EmailSettingModel>();
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
                response = new List<EmailSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailSettingModel> Read(string connectionString, EmailSettingModel emailSetting, bool orderByLastDate, int fromRow, int limit, EmailSettingModel emailSettingWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailSettingModel> response = new List<EmailSettingModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `mail_domain_com`, `email_sender`, `sender_user`, `sender_password`, `email_port`, `default_credential`, `enable_ssl`, `modified_by`, `created_at`, `updated_at`  FROM em_email_settings ";
                {
                    if (emailSetting != null)
                    {
                        if (!String.IsNullOrEmpty(emailSetting.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", emailSetting.Id);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.MailDomainCom))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `mail_domain_com` = @mailDomainCom ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `mail_domain_com` = @mailDomainCom ");
                            }
                            cmd.Parameters.AddWithValue("@mailDomainCom", emailSetting.MailDomainCom);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.EmailSender))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `email_sender` = @emailSender ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `email_sender` = @emailSender ");
                            }
                            cmd.Parameters.AddWithValue("@emailSender", emailSetting.EmailSender);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.SenderUser))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `sender_user` = @senderUser ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `sender_user` = @senderUser ");
                            }
                            cmd.Parameters.AddWithValue("@senderUser", emailSetting.SenderUser);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.SenderPassword))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `sender_password` = @senderPassword ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `sender_password` = @senderPassword ");
                            }
                            cmd.Parameters.AddWithValue("@senderPassword", emailSetting.SenderPassword);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.EmailPort))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `email_port` = @emailPort ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `email_port` = @emailPort ");
                            }
                            cmd.Parameters.AddWithValue("@emailPort", emailSetting.EmailPort);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.DefaultCredential))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `default_credential` = @defaultCredential ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `default_credential` = @defaultCredential ");
                            }
                            cmd.Parameters.AddWithValue("@defaultCredential", emailSetting.DefaultCredential);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.EnableSsl))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `enable_ssl` = @enableSsl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `enable_ssl` = @enableSsl ");
                            }
                            cmd.Parameters.AddWithValue("@enableSsl", emailSetting.EnableSsl);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", emailSetting.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", emailSetting.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailSetting.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", emailSetting.UpdatedAt);
                        }
                    }
                    if (emailSettingWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + emailSettingWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.MailDomainCom))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `mail_domain_com` LIKE @mailDomainComWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `mail_domain_com` LIKE @mailDomainComWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@mailDomainComWhereLike", "%" + emailSettingWhereLike.MailDomainCom + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.EmailSender))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `email_sender` LIKE @emailSenderWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `email_sender` LIKE @emailSenderWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@emailSenderWhereLike", "%" + emailSettingWhereLike.EmailSender + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.SenderUser))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `sender_user` LIKE @senderUserWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `sender_user` LIKE @senderUserWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@senderUserWhereLike", "%" + emailSettingWhereLike.SenderUser + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.SenderPassword))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `sender_password` LIKE @senderPasswordWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `sender_password` LIKE @senderPasswordWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@senderPasswordWhereLike", "%" + emailSettingWhereLike.SenderPassword + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.EmailPort))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `email_port` LIKE @emailPortWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `email_port` LIKE @emailPortWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@emailPortWhereLike", "%" + emailSettingWhereLike.EmailPort + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.DefaultCredential))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `default_credential` LIKE @defaultCredentialWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `default_credential` LIKE @defaultCredentialWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@defaultCredentialWhereLike", "%" + emailSettingWhereLike.DefaultCredential + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.EnableSsl))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `enable_ssl` LIKE @enableSslWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `enable_ssl` LIKE @enableSslWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@enableSslWhereLike", "%" + emailSettingWhereLike.EnableSsl + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + emailSettingWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + emailSettingWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + emailSettingWhereLike.UpdatedAt + "%");
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
                                emailSetting = new EmailSettingModel();
                                if (!reader.IsDBNull(0))
                                    emailSetting.Id = reader.GetValue(0).ToString();
                                else
                                    emailSetting.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailSetting.MailDomainCom = reader.GetValue(1).ToString();
                                else
                                    emailSetting.MailDomainCom = null;
                                if (!reader.IsDBNull(2))
                                    emailSetting.EmailSender = reader.GetValue(2).ToString();
                                else
                                    emailSetting.EmailSender = null;
                                if (!reader.IsDBNull(3))
                                    emailSetting.SenderUser = reader.GetValue(3).ToString();
                                else
                                    emailSetting.SenderUser = null;
                                if (!reader.IsDBNull(4))
                                    emailSetting.SenderPassword = reader.GetValue(4).ToString();
                                else
                                    emailSetting.SenderPassword = null;
                                if (!reader.IsDBNull(5))
                                    emailSetting.EmailPort = reader.GetValue(5).ToString();
                                else
                                    emailSetting.EmailPort = null;
                                if (!reader.IsDBNull(6))
                                    emailSetting.DefaultCredential = reader.GetValue(6).ToString();
                                else
                                    emailSetting.DefaultCredential = null;
                                if (!reader.IsDBNull(7))
                                    emailSetting.EnableSsl = reader.GetValue(7).ToString();
                                else
                                    emailSetting.EnableSsl = null;
                                if (!reader.IsDBNull(8))
                                    emailSetting.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    emailSetting.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    emailSetting.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    emailSetting.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    emailSetting.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    emailSetting.UpdatedAt = null;
                                response.Add(emailSetting);
                                emailSetting = new EmailSettingModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EmailSettingModel>();
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
                response = new List<EmailSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailSettingModel> Read(string connectionString, List<EmailSettingModel> emailSettingWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailSettingModel> response = new List<EmailSettingModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `mail_domain_com`, `email_sender`, `sender_user`, `sender_password`, `email_port`, `default_credential`, `enable_ssl`, `modified_by`, `created_at`, `updated_at`  FROM em_email_settings ";
                if (emailSettingWhere != null && emailSettingWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailSettingWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailSettingWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].MailDomainCom))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `mail_domain_com` = @mailDomainCom" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@mailDomainCom" + i, emailSettingWhere[i].MailDomainCom);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].EmailSender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email_sender` = @emailSender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@emailSender" + i, emailSettingWhere[i].EmailSender);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].SenderUser))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sender_user` = @senderUser" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@senderUser" + i, emailSettingWhere[i].SenderUser);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].SenderPassword))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sender_password` = @senderPassword" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@senderPassword" + i, emailSettingWhere[i].SenderPassword);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].EmailPort))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email_port` = @emailPort" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@emailPort" + i, emailSettingWhere[i].EmailPort);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].DefaultCredential))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `default_credential` = @defaultCredential" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@defaultCredential" + i, emailSettingWhere[i].DefaultCredential);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].EnableSsl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enable_ssl` = @enableSsl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@enableSsl" + i, emailSettingWhere[i].EnableSsl);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailSettingWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailSettingWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailSettingWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailSettingWhere.Count - 1)
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
                            EmailSettingModel emailSetting = new EmailSettingModel(); if (!reader.IsDBNull(0))
                                emailSetting.Id = reader.GetValue(0).ToString();
                            else
                                emailSetting.Id = null;
                            if (!reader.IsDBNull(1))
                                emailSetting.MailDomainCom = reader.GetValue(1).ToString();
                            else
                                emailSetting.MailDomainCom = null;
                            if (!reader.IsDBNull(2))
                                emailSetting.EmailSender = reader.GetValue(2).ToString();
                            else
                                emailSetting.EmailSender = null;
                            if (!reader.IsDBNull(3))
                                emailSetting.SenderUser = reader.GetValue(3).ToString();
                            else
                                emailSetting.SenderUser = null;
                            if (!reader.IsDBNull(4))
                                emailSetting.SenderPassword = reader.GetValue(4).ToString();
                            else
                                emailSetting.SenderPassword = null;
                            if (!reader.IsDBNull(5))
                                emailSetting.EmailPort = reader.GetValue(5).ToString();
                            else
                                emailSetting.EmailPort = null;
                            if (!reader.IsDBNull(6))
                                emailSetting.DefaultCredential = reader.GetValue(6).ToString();
                            else
                                emailSetting.DefaultCredential = null;
                            if (!reader.IsDBNull(7))
                                emailSetting.EnableSsl = reader.GetValue(7).ToString();
                            else
                                emailSetting.EnableSsl = null;
                            if (!reader.IsDBNull(8))
                                emailSetting.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                emailSetting.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                emailSetting.CreatedAt = reader.GetValue(9).ToString();
                            else
                                emailSetting.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                emailSetting.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                emailSetting.UpdatedAt = null;
                            response.Add(emailSetting);
                            emailSetting = new EmailSettingModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EmailSettingModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailSettingModel> Read(string connectionString, List<EmailSettingModel> emailSettingWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailSettingModel> response = new List<EmailSettingModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `mail_domain_com`, `email_sender`, `sender_user`, `sender_password`, `email_port`, `default_credential`, `enable_ssl`, `modified_by`, `created_at`, `updated_at`  FROM em_email_settings ";
                if (emailSettingWhere != null && emailSettingWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailSettingWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailSettingWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].MailDomainCom))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `mail_domain_com` = @mailDomainCom" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@mailDomainCom" + i, emailSettingWhere[i].MailDomainCom);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].EmailSender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email_sender` = @emailSender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@emailSender" + i, emailSettingWhere[i].EmailSender);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].SenderUser))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sender_user` = @senderUser" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@senderUser" + i, emailSettingWhere[i].SenderUser);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].SenderPassword))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sender_password` = @senderPassword" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@senderPassword" + i, emailSettingWhere[i].SenderPassword);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].EmailPort))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email_port` = @emailPort" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@emailPort" + i, emailSettingWhere[i].EmailPort);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].DefaultCredential))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `default_credential` = @defaultCredential" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@defaultCredential" + i, emailSettingWhere[i].DefaultCredential);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].EnableSsl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enable_ssl` = @enableSsl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@enableSsl" + i, emailSettingWhere[i].EnableSsl);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailSettingWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailSettingWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailSettingWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailSettingWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailSettingWhere.Count - 1)
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
                            EmailSettingModel emailSetting = new EmailSettingModel(); if (!reader.IsDBNull(0))
                                emailSetting.Id = reader.GetValue(0).ToString();
                            else
                                emailSetting.Id = null;
                            if (!reader.IsDBNull(1))
                                emailSetting.MailDomainCom = reader.GetValue(1).ToString();
                            else
                                emailSetting.MailDomainCom = null;
                            if (!reader.IsDBNull(2))
                                emailSetting.EmailSender = reader.GetValue(2).ToString();
                            else
                                emailSetting.EmailSender = null;
                            if (!reader.IsDBNull(3))
                                emailSetting.SenderUser = reader.GetValue(3).ToString();
                            else
                                emailSetting.SenderUser = null;
                            if (!reader.IsDBNull(4))
                                emailSetting.SenderPassword = reader.GetValue(4).ToString();
                            else
                                emailSetting.SenderPassword = null;
                            if (!reader.IsDBNull(5))
                                emailSetting.EmailPort = reader.GetValue(5).ToString();
                            else
                                emailSetting.EmailPort = null;
                            if (!reader.IsDBNull(6))
                                emailSetting.DefaultCredential = reader.GetValue(6).ToString();
                            else
                                emailSetting.DefaultCredential = null;
                            if (!reader.IsDBNull(7))
                                emailSetting.EnableSsl = reader.GetValue(7).ToString();
                            else
                                emailSetting.EnableSsl = null;
                            if (!reader.IsDBNull(8))
                                emailSetting.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                emailSetting.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                emailSetting.CreatedAt = reader.GetValue(9).ToString();
                            else
                                emailSetting.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                emailSetting.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                emailSetting.UpdatedAt = null;
                            response.Add(emailSetting);
                            emailSetting = new EmailSettingModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EmailSettingModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailSettingModel> Create(string connectionString, EmailSettingModel emailSetting)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailSettingModel> response = new List<EmailSettingModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO em_email_settings (";
                string values = "(";
                if (!String.IsNullOrEmpty(emailSetting.Id))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `id` ");
                        values = String.Concat(values, ", @id");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `id` ");
                        values = String.Concat(values, " @id");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", emailSetting.Id);
                }
                if (!String.IsNullOrEmpty(emailSetting.MailDomainCom))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `mail_domain_com` ");
                        values = String.Concat(values, ", @mailDomainCom");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `mail_domain_com` ");
                        values = String.Concat(values, " @mailDomainCom");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@mailDomainCom", emailSetting.MailDomainCom);
                }
                if (!String.IsNullOrEmpty(emailSetting.EmailSender))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `email_sender` ");
                        values = String.Concat(values, ", @emailSender");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `email_sender` ");
                        values = String.Concat(values, " @emailSender");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@emailSender", emailSetting.EmailSender);
                }
                if (!String.IsNullOrEmpty(emailSetting.SenderUser))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `sender_user` ");
                        values = String.Concat(values, ", @senderUser");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `sender_user` ");
                        values = String.Concat(values, " @senderUser");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@senderUser", emailSetting.SenderUser);
                }
                if (!String.IsNullOrEmpty(emailSetting.SenderPassword))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `sender_password` ");
                        values = String.Concat(values, ", @senderPassword");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `sender_password` ");
                        values = String.Concat(values, " @senderPassword");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@senderPassword", emailSetting.SenderPassword);
                }
                if (!String.IsNullOrEmpty(emailSetting.EmailPort))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `email_port` ");
                        values = String.Concat(values, ", @emailPort");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `email_port` ");
                        values = String.Concat(values, " @emailPort");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@emailPort", emailSetting.EmailPort);
                }
                if (!String.IsNullOrEmpty(emailSetting.DefaultCredential))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `default_credential` ");
                        values = String.Concat(values, ", @defaultCredential");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `default_credential` ");
                        values = String.Concat(values, " @defaultCredential");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@defaultCredential", emailSetting.DefaultCredential);
                }
                if (!String.IsNullOrEmpty(emailSetting.EnableSsl))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `enable_ssl` ");
                        values = String.Concat(values, ", @enableSsl");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `enable_ssl` ");
                        values = String.Concat(values, " @enableSsl");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@enableSsl", emailSetting.EnableSsl);
                }
                if (!String.IsNullOrEmpty(emailSetting.ModifiedBy))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `modified_by` ");
                        values = String.Concat(values, ", @modifiedBy");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `modified_by` ");
                        values = String.Concat(values, " @modifiedBy");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", emailSetting.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(emailSetting.CreatedAt))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `created_at` ");
                        values = String.Concat(values, ", @createdAt");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `created_at` ");
                        values = String.Concat(values, " @createdAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", emailSetting.CreatedAt);
                }
                if (!String.IsNullOrEmpty(emailSetting.UpdatedAt))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `updated_at` ");
                        values = String.Concat(values, ", @updatedAt");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `updated_at` ");
                        values = String.Concat(values, " @updatedAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", emailSetting.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                emailSetting.Id = cmd.LastInsertedId.ToString();
                response.Add(emailSetting);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailSettingModel>();
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
        public List<EmailSettingModel> Create(string connectionString, List<EmailSettingModel> emailSettingList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailSettingModel> response = new List<EmailSettingModel>();
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
                        for (int i = 0; i < emailSettingList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO em_email_settings (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(emailSettingList[i].Id))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `id` ");
                                        values = String.Concat(values, ", @id" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `id` ");
                                        values = String.Concat(values, "@id" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, emailSettingList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].MailDomainCom))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `mail_domain_com` ");
                                        values = String.Concat(values, ", @mailDomainCom" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `mail_domain_com` ");
                                        values = String.Concat(values, "@mailDomainCom" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@mailDomainCom" + i, emailSettingList[i].MailDomainCom);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].EmailSender))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `email_sender` ");
                                        values = String.Concat(values, ", @emailSender" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `email_sender` ");
                                        values = String.Concat(values, "@emailSender" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@emailSender" + i, emailSettingList[i].EmailSender);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].SenderUser))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `sender_user` ");
                                        values = String.Concat(values, ", @senderUser" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `sender_user` ");
                                        values = String.Concat(values, "@senderUser" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@senderUser" + i, emailSettingList[i].SenderUser);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].SenderPassword))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `sender_password` ");
                                        values = String.Concat(values, ", @senderPassword" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `sender_password` ");
                                        values = String.Concat(values, "@senderPassword" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@senderPassword" + i, emailSettingList[i].SenderPassword);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].EmailPort))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `email_port` ");
                                        values = String.Concat(values, ", @emailPort" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `email_port` ");
                                        values = String.Concat(values, "@emailPort" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@emailPort" + i, emailSettingList[i].EmailPort);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].DefaultCredential))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `default_credential` ");
                                        values = String.Concat(values, ", @defaultCredential" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `default_credential` ");
                                        values = String.Concat(values, "@defaultCredential" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@defaultCredential" + i, emailSettingList[i].DefaultCredential);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].EnableSsl))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `enable_ssl` ");
                                        values = String.Concat(values, ", @enableSsl" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `enable_ssl` ");
                                        values = String.Concat(values, "@enableSsl" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@enableSsl" + i, emailSettingList[i].EnableSsl);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `modified_by` ");
                                        values = String.Concat(values, ", @modifiedBy" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `modified_by` ");
                                        values = String.Concat(values, "@modifiedBy" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailSettingList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].CreatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `created_at` ");
                                        values = String.Concat(values, ", @createdAt" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `created_at` ");
                                        values = String.Concat(values, "@createdAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailSettingList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `updated_at` ");
                                        values = String.Concat(values, ", @updatedAt" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `updated_at` ");
                                        values = String.Concat(values, "@updatedAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailSettingList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(emailSettingList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailSettingModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailSettingModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailSettingModel>();
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

        public List<EmailSettingModel> Update(string connectionString, EmailSettingModel emailSetting, EmailSettingModel emailSettingWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailSettingModel> response = new List<EmailSettingModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE em_email_settings SET ";
                if (!String.IsNullOrEmpty(emailSetting.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", emailSetting.Id);
                }
                if (!String.IsNullOrEmpty(emailSetting.MailDomainCom))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `mail_domain_com` = @mailDomainCom");
                    else
                        sql = String.Concat(sql, " `mail_domain_com` = @mailDomainCom");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@mailDomainCom", emailSetting.MailDomainCom);
                }
                if (!String.IsNullOrEmpty(emailSetting.EmailSender))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `email_sender` = @emailSender");
                    else
                        sql = String.Concat(sql, " `email_sender` = @emailSender");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@emailSender", emailSetting.EmailSender);
                }
                if (!String.IsNullOrEmpty(emailSetting.SenderUser))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `sender_user` = @senderUser");
                    else
                        sql = String.Concat(sql, " `sender_user` = @senderUser");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@senderUser", emailSetting.SenderUser);
                }
                if (!String.IsNullOrEmpty(emailSetting.SenderPassword))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `sender_password` = @senderPassword");
                    else
                        sql = String.Concat(sql, " `sender_password` = @senderPassword");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@senderPassword", emailSetting.SenderPassword);
                }
                if (!String.IsNullOrEmpty(emailSetting.EmailPort))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `email_port` = @emailPort");
                    else
                        sql = String.Concat(sql, " `email_port` = @emailPort");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@emailPort", emailSetting.EmailPort);
                }
                if (!String.IsNullOrEmpty(emailSetting.DefaultCredential))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `default_credential` = @defaultCredential");
                    else
                        sql = String.Concat(sql, " `default_credential` = @defaultCredential");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@defaultCredential", emailSetting.DefaultCredential);
                }
                if (!String.IsNullOrEmpty(emailSetting.EnableSsl))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `enable_ssl` = @enableSsl");
                    else
                        sql = String.Concat(sql, " `enable_ssl` = @enableSsl");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@enableSsl", emailSetting.EnableSsl);
                }
                if (!String.IsNullOrEmpty(emailSetting.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", emailSetting.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(emailSetting.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", emailSetting.CreatedAt);
                }
                if (!String.IsNullOrEmpty(emailSetting.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", emailSetting.UpdatedAt);
                }
                if (emailSettingWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(emailSettingWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", emailSettingWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(emailSettingWhere.MailDomainCom))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `mail_domain_com` = @mailDomainComWhere");
                        else
                            sql = String.Concat(sql, " `mail_domain_com` = @mailDomainComWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@mailDomainComWhere", emailSettingWhere.MailDomainCom);
                    }
                    if (!String.IsNullOrEmpty(emailSettingWhere.EmailSender))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `email_sender` = @emailSenderWhere");
                        else
                            sql = String.Concat(sql, " `email_sender` = @emailSenderWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@emailSenderWhere", emailSettingWhere.EmailSender);
                    }
                    if (!String.IsNullOrEmpty(emailSettingWhere.SenderUser))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `sender_user` = @senderUserWhere");
                        else
                            sql = String.Concat(sql, " `sender_user` = @senderUserWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@senderUserWhere", emailSettingWhere.SenderUser);
                    }
                    if (!String.IsNullOrEmpty(emailSettingWhere.SenderPassword))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `sender_password` = @senderPasswordWhere");
                        else
                            sql = String.Concat(sql, " `sender_password` = @senderPasswordWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@senderPasswordWhere", emailSettingWhere.SenderPassword);
                    }
                    if (!String.IsNullOrEmpty(emailSettingWhere.EmailPort))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `email_port` = @emailPortWhere");
                        else
                            sql = String.Concat(sql, " `email_port` = @emailPortWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@emailPortWhere", emailSettingWhere.EmailPort);
                    }
                    if (!String.IsNullOrEmpty(emailSettingWhere.DefaultCredential))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `default_credential` = @defaultCredentialWhere");
                        else
                            sql = String.Concat(sql, " `default_credential` = @defaultCredentialWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@defaultCredentialWhere", emailSettingWhere.DefaultCredential);
                    }
                    if (!String.IsNullOrEmpty(emailSettingWhere.EnableSsl))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `enable_ssl` = @enableSslWhere");
                        else
                            sql = String.Concat(sql, " `enable_ssl` = @enableSslWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@enableSslWhere", emailSettingWhere.EnableSsl);
                    }
                    if (!String.IsNullOrEmpty(emailSettingWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", emailSettingWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(emailSettingWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", emailSettingWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(emailSettingWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", emailSettingWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(emailSetting);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailSettingModel>();
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

        public List<EmailSettingModel> Update(string connectionString, List<EmailSettingModel> emailSettingList, List<EmailSettingModel> emailSettingWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailSettingModel> response = new List<EmailSettingModel>();
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
                        for (int i = 0; i < emailSettingList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE em_email_settings SET ";
                                if (!String.IsNullOrEmpty(emailSettingList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, emailSettingList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].MailDomainCom))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `mail_domain_com` = @mailDomainCom" + i);
                                    else
                                        sql = String.Concat(sql, " `mail_domain_com` = @mailDomainCom" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@mailDomainCom" + i, emailSettingList[i].MailDomainCom);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].EmailSender))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `email_sender` = @emailSender" + i);
                                    else
                                        sql = String.Concat(sql, " `email_sender` = @emailSender" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@emailSender" + i, emailSettingList[i].EmailSender);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].SenderUser))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `sender_user` = @senderUser" + i);
                                    else
                                        sql = String.Concat(sql, " `sender_user` = @senderUser" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@senderUser" + i, emailSettingList[i].SenderUser);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].SenderPassword))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `sender_password` = @senderPassword" + i);
                                    else
                                        sql = String.Concat(sql, " `sender_password` = @senderPassword" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@senderPassword" + i, emailSettingList[i].SenderPassword);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].EmailPort))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `email_port` = @emailPort" + i);
                                    else
                                        sql = String.Concat(sql, " `email_port` = @emailPort" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@emailPort" + i, emailSettingList[i].EmailPort);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].DefaultCredential))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `default_credential` = @defaultCredential" + i);
                                    else
                                        sql = String.Concat(sql, " `default_credential` = @defaultCredential" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@defaultCredential" + i, emailSettingList[i].DefaultCredential);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].EnableSsl))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `enable_ssl` = @enableSsl" + i);
                                    else
                                        sql = String.Concat(sql, " `enable_ssl` = @enableSsl" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@enableSsl" + i, emailSettingList[i].EnableSsl);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailSettingList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailSettingList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(emailSettingList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailSettingList[i].UpdatedAt);
                                }
                                if (emailSettingWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(emailSettingWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, emailSettingWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(emailSettingWhereList[i].MailDomainCom))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `mail_domain_com` = @mailDomainComWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `mail_domain_com` = @mailDomainComWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@mailDomainComWhere" + i, emailSettingWhereList[i].MailDomainCom);
                                    }
                                    if (!String.IsNullOrEmpty(emailSettingWhereList[i].EmailSender))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `email_sender` = @emailSenderWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `email_sender` = @emailSenderWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@emailSenderWhere" + i, emailSettingWhereList[i].EmailSender);
                                    }
                                    if (!String.IsNullOrEmpty(emailSettingWhereList[i].SenderUser))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `sender_user` = @senderUserWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `sender_user` = @senderUserWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@senderUserWhere" + i, emailSettingWhereList[i].SenderUser);
                                    }
                                    if (!String.IsNullOrEmpty(emailSettingWhereList[i].SenderPassword))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `sender_password` = @senderPasswordWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `sender_password` = @senderPasswordWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@senderPasswordWhere" + i, emailSettingWhereList[i].SenderPassword);
                                    }
                                    if (!String.IsNullOrEmpty(emailSettingWhereList[i].EmailPort))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `email_port` = @emailPortWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `email_port` = @emailPortWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@emailPortWhere" + i, emailSettingWhereList[i].EmailPort);
                                    }
                                    if (!String.IsNullOrEmpty(emailSettingWhereList[i].DefaultCredential))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `default_credential` = @defaultCredentialWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `default_credential` = @defaultCredentialWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@defaultCredentialWhere" + i, emailSettingWhereList[i].DefaultCredential);
                                    }
                                    if (!String.IsNullOrEmpty(emailSettingWhereList[i].EnableSsl))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `enable_ssl` = @enableSslWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `enable_ssl` = @enableSslWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@enableSslWhere" + i, emailSettingWhereList[i].EnableSsl);
                                    }
                                    if (!String.IsNullOrEmpty(emailSettingWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, emailSettingWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(emailSettingWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, emailSettingWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(emailSettingWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, emailSettingWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailSettingList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailSettingModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailSettingModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailSettingModel>();
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

        public List<EmailSettingModel> Delete(string connectionString, EmailSettingModel emailSetting)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailSettingModel> response = new List<EmailSettingModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM em_email_settings  ";
                if (emailSetting != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(emailSetting.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", emailSetting.Id);
                    }
                    if (!String.IsNullOrEmpty(emailSetting.MailDomainCom))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `mail_domain_com` = @mailDomainCom");
                        else
                            sql = String.Concat(sql, " `mail_domain_com` = @mailDomainCom");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@mailDomainCom", emailSetting.MailDomainCom);
                    }
                    if (!String.IsNullOrEmpty(emailSetting.EmailSender))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `email_sender` = @emailSender");
                        else
                            sql = String.Concat(sql, " `email_sender` = @emailSender");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@emailSender", emailSetting.EmailSender);
                    }
                    if (!String.IsNullOrEmpty(emailSetting.SenderUser))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `sender_user` = @senderUser");
                        else
                            sql = String.Concat(sql, " `sender_user` = @senderUser");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@senderUser", emailSetting.SenderUser);
                    }
                    if (!String.IsNullOrEmpty(emailSetting.SenderPassword))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `sender_password` = @senderPassword");
                        else
                            sql = String.Concat(sql, " `sender_password` = @senderPassword");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@senderPassword", emailSetting.SenderPassword);
                    }
                    if (!String.IsNullOrEmpty(emailSetting.EmailPort))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `email_port` = @emailPort");
                        else
                            sql = String.Concat(sql, " `email_port` = @emailPort");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@emailPort", emailSetting.EmailPort);
                    }
                    if (!String.IsNullOrEmpty(emailSetting.DefaultCredential))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `default_credential` = @defaultCredential");
                        else
                            sql = String.Concat(sql, " `default_credential` = @defaultCredential");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@defaultCredential", emailSetting.DefaultCredential);
                    }
                    if (!String.IsNullOrEmpty(emailSetting.EnableSsl))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `enable_ssl` = @enableSsl");
                        else
                            sql = String.Concat(sql, " `enable_ssl` = @enableSsl");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@enableSsl", emailSetting.EnableSsl);
                    }
                    if (!String.IsNullOrEmpty(emailSetting.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", emailSetting.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(emailSetting.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", emailSetting.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(emailSetting.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", emailSetting.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(emailSetting);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailSettingModel>();
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

        public List<EmailSettingModel> Delete(string connectionString, List<EmailSettingModel> emailSetting)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailSettingModel> response = new List<EmailSettingModel>();
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
                        for (int i = 0; i < emailSetting.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM em_email_settings  ";
                                if (emailSetting[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(emailSetting[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, emailSetting[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(emailSetting[i].MailDomainCom))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `mail_domain_com` = @mailDomainCom" + i);
                                        else
                                            sql = String.Concat(sql, " `mail_domain_com` = @mailDomainCom" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@mailDomainCom" + i, emailSetting[i].MailDomainCom);
                                    }
                                    if (!String.IsNullOrEmpty(emailSetting[i].EmailSender))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `email_sender` = @emailSender" + i);
                                        else
                                            sql = String.Concat(sql, " `email_sender` = @emailSender" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@emailSender" + i, emailSetting[i].EmailSender);
                                    }
                                    if (!String.IsNullOrEmpty(emailSetting[i].SenderUser))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `sender_user` = @senderUser" + i);
                                        else
                                            sql = String.Concat(sql, " `sender_user` = @senderUser" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@senderUser" + i, emailSetting[i].SenderUser);
                                    }
                                    if (!String.IsNullOrEmpty(emailSetting[i].SenderPassword))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `sender_password` = @senderPassword" + i);
                                        else
                                            sql = String.Concat(sql, " `sender_password` = @senderPassword" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@senderPassword" + i, emailSetting[i].SenderPassword);
                                    }
                                    if (!String.IsNullOrEmpty(emailSetting[i].EmailPort))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `email_port` = @emailPort" + i);
                                        else
                                            sql = String.Concat(sql, " `email_port` = @emailPort" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@emailPort" + i, emailSetting[i].EmailPort);
                                    }
                                    if (!String.IsNullOrEmpty(emailSetting[i].DefaultCredential))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `default_credential` = @defaultCredential" + i);
                                        else
                                            sql = String.Concat(sql, " `default_credential` = @defaultCredential" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@defaultCredential" + i, emailSetting[i].DefaultCredential);
                                    }
                                    if (!String.IsNullOrEmpty(emailSetting[i].EnableSsl))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `enable_ssl` = @enableSsl" + i);
                                        else
                                            sql = String.Concat(sql, " `enable_ssl` = @enableSsl" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@enableSsl" + i, emailSetting[i].EnableSsl);
                                    }
                                    if (!String.IsNullOrEmpty(emailSetting[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, emailSetting[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(emailSetting[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, emailSetting[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(emailSetting[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, emailSetting[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailSetting[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailSettingModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailSettingModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailSettingModel>();
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
