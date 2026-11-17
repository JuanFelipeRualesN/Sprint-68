using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Emails;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Emails
{
    class EmailSettingDAO
    {
        private MySqlConnection _connection;
        public List<EmailSettingModel> Read(string connectionString, EmailSettingModel emailSetting, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailSettingModel> response = new List<EmailSettingModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `mail_domain_com`, `email_sender`, `sender_password`, `email_port`, `default_credential`, `enable_ssl`, `modified_by`, `created_at`, `updated_at`  FROM em_email_settings ";
                {
                    if (emailSetting != null)
                    {
                        if (!string.IsNullOrEmpty(emailSetting.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", emailSetting.Id);
                        }
                        if (!string.IsNullOrEmpty(emailSetting.MailDomainCom))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `mail_domain_com` = @mailDomainCom ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `mail_domain_com` = @mailDomainCom ");
                            }
                            cmd.Parameters.AddWithValue("@mailDomainCom", emailSetting.MailDomainCom);
                        }
                        if (!string.IsNullOrEmpty(emailSetting.EmailSender))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `email_sender` = @emailSender ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `email_sender` = @emailSender ");
                            }
                            cmd.Parameters.AddWithValue("@emailSender", emailSetting.EmailSender);
                        }
                        if (!string.IsNullOrEmpty(emailSetting.SenderPassword))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `sender_password` = @senderPassword ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `sender_password` = @senderPassword ");
                            }
                            cmd.Parameters.AddWithValue("@senderPassword", emailSetting.SenderPassword);
                        }
                        if (!string.IsNullOrEmpty(emailSetting.EmailPort))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `email_port` = @emailPort ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `email_port` = @emailPort ");
                            }
                            cmd.Parameters.AddWithValue("@emailPort", emailSetting.EmailPort);
                        }
                        if (!string.IsNullOrEmpty(emailSetting.DefaultCredential))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `default_credential` = @defaultCredential ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `default_credential` = @defaultCredential ");
                            }
                            cmd.Parameters.AddWithValue("@defaultCredential", emailSetting.DefaultCredential);
                        }
                        if (!string.IsNullOrEmpty(emailSetting.EnableSsl))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `enable_ssl` = @enableSsl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `enable_ssl` = @enableSsl ");
                            }
                            cmd.Parameters.AddWithValue("@enableSsl", emailSetting.EnableSsl);
                        }
                        if (!string.IsNullOrEmpty(emailSetting.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", emailSetting.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailSetting.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", emailSetting.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailSetting.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", emailSetting.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        if (firstOne)
                            sql = string.Concat(sql, " LIMIT 1 ");
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader Reader = cmd.ExecuteReader();
                    try
                    {
                        if (Reader.HasRows)
                            while (Reader.Read())
                            {
                                if (!Reader.IsDBNull(0))
                                    emailSetting.Id = Reader.GetValue(0).ToString();
                                else
                                    emailSetting.Id = null;
                                if (!Reader.IsDBNull(1))
                                    emailSetting.MailDomainCom = Reader.GetValue(1).ToString();
                                else
                                    emailSetting.MailDomainCom = null;
                                if (!Reader.IsDBNull(2))
                                    emailSetting.EmailSender = Reader.GetValue(2).ToString();
                                else
                                    emailSetting.EmailSender = null;
                                if (!Reader.IsDBNull(3))
                                    emailSetting.SenderPassword = Reader.GetValue(3).ToString();
                                else
                                    emailSetting.SenderPassword = null;
                                if (!Reader.IsDBNull(4))
                                    emailSetting.EmailPort = Reader.GetValue(4).ToString();
                                else
                                    emailSetting.EmailPort = null;
                                if (!Reader.IsDBNull(5))
                                    emailSetting.DefaultCredential = Reader.GetValue(5).ToString();
                                else
                                    emailSetting.DefaultCredential = null;
                                if (!Reader.IsDBNull(6))
                                    emailSetting.EnableSsl = Reader.GetValue(6).ToString();
                                else
                                    emailSetting.EnableSsl = null;
                                if (!Reader.IsDBNull(7))
                                    emailSetting.ModifiedBy = Reader.GetValue(7).ToString();
                                else
                                    emailSetting.ModifiedBy = null;
                                if (!Reader.IsDBNull(8))
                                    emailSetting.CreatedAt = Reader.GetValue(8).ToString();
                                else
                                    emailSetting.CreatedAt = null;
                                if (!Reader.IsDBNull(9))
                                    emailSetting.UpdatedAt = Reader.GetValue(9).ToString();
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
                        Reader.Close();
                    }
                    finally
                    {
                        if (Reader != null)
                            Reader.Close();
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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `mail_domain_com`, `email_sender`, `sender_password`, `email_port`, `default_credential`, `enable_ssl`, `modified_by`, `created_at`, `updated_at`  FROM em_email_settings ";
                if (emailSettingWhere != null && emailSettingWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailSettingWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(emailSettingWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailSettingWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(emailSettingWhere[i].MailDomainCom))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `mail_domain_com` = @mailDomainCom" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@mailDomainCom" + i, emailSettingWhere[i].MailDomainCom);
                        }
                        if (!string.IsNullOrEmpty(emailSettingWhere[i].EmailSender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email_sender` = @emailSender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@emailSender" + i, emailSettingWhere[i].EmailSender);
                        }
                        if (!string.IsNullOrEmpty(emailSettingWhere[i].SenderPassword))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sender_password` = @senderPassword" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@senderPassword" + i, emailSettingWhere[i].SenderPassword);
                        }
                        if (!string.IsNullOrEmpty(emailSettingWhere[i].EmailPort))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email_port` = @emailPort" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@emailPort" + i, emailSettingWhere[i].EmailPort);
                        }
                        if (!string.IsNullOrEmpty(emailSettingWhere[i].DefaultCredential))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `default_credential` = @defaultCredential" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@defaultCredential" + i, emailSettingWhere[i].DefaultCredential);
                        }
                        if (!string.IsNullOrEmpty(emailSettingWhere[i].EnableSsl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enable_ssl` = @enableSsl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@enableSsl" + i, emailSettingWhere[i].EnableSsl);
                        }
                        if (!string.IsNullOrEmpty(emailSettingWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailSettingWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailSettingWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailSettingWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailSettingWhere[i].UpdatedAt))
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
                MySqlDataReader Reader = cmd.ExecuteReader();
                try
                {
                    if (Reader.HasRows)
                        while (Reader.Read())
                        {
                            EmailSettingModel emailSetting = new EmailSettingModel(); if (!Reader.IsDBNull(0))
                                emailSetting.Id = Reader.GetValue(0).ToString();
                            else
                                emailSetting.Id = null;
                            if (!Reader.IsDBNull(1))
                                emailSetting.MailDomainCom = Reader.GetValue(1).ToString();
                            else
                                emailSetting.MailDomainCom = null;
                            if (!Reader.IsDBNull(2))
                                emailSetting.EmailSender = Reader.GetValue(2).ToString();
                            else
                                emailSetting.EmailSender = null;
                            if (!Reader.IsDBNull(3))
                                emailSetting.SenderPassword = Reader.GetValue(3).ToString();
                            else
                                emailSetting.SenderPassword = null;
                            if (!Reader.IsDBNull(4))
                                emailSetting.EmailPort = Reader.GetValue(4).ToString();
                            else
                                emailSetting.EmailPort = null;
                            if (!Reader.IsDBNull(5))
                                emailSetting.DefaultCredential = Reader.GetValue(5).ToString();
                            else
                                emailSetting.DefaultCredential = null;
                            if (!Reader.IsDBNull(6))
                                emailSetting.EnableSsl = Reader.GetValue(6).ToString();
                            else
                                emailSetting.EnableSsl = null;
                            if (!Reader.IsDBNull(7))
                                emailSetting.ModifiedBy = Reader.GetValue(7).ToString();
                            else
                                emailSetting.ModifiedBy = null;
                            if (!Reader.IsDBNull(8))
                                emailSetting.CreatedAt = Reader.GetValue(8).ToString();
                            else
                                emailSetting.CreatedAt = null;
                            if (!Reader.IsDBNull(9))
                                emailSetting.UpdatedAt = Reader.GetValue(9).ToString();
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
                    Reader.Close();
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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO em_email_settings (";
                string values = "(";
                if (!string.IsNullOrEmpty(emailSetting.Id))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `id` ");
                        values = string.Concat(values, ", @id");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `id` ");
                        values = string.Concat(values, " @id");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", emailSetting.Id);
                }
                if (!string.IsNullOrEmpty(emailSetting.MailDomainCom))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `mail_domain_com` ");
                        values = string.Concat(values, ", @mailDomainCom");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `mail_domain_com` ");
                        values = string.Concat(values, " @mailDomainCom");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@mailDomainCom", emailSetting.MailDomainCom);
                }
                if (!string.IsNullOrEmpty(emailSetting.EmailSender))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `email_sender` ");
                        values = string.Concat(values, ", @emailSender");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `email_sender` ");
                        values = string.Concat(values, " @emailSender");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@emailSender", emailSetting.EmailSender);
                }
                if (!string.IsNullOrEmpty(emailSetting.SenderPassword))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `sender_password` ");
                        values = string.Concat(values, ", @senderPassword");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `sender_password` ");
                        values = string.Concat(values, " @senderPassword");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@senderPassword", emailSetting.SenderPassword);
                }
                if (!string.IsNullOrEmpty(emailSetting.EmailPort))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `email_port` ");
                        values = string.Concat(values, ", @emailPort");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `email_port` ");
                        values = string.Concat(values, " @emailPort");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@emailPort", emailSetting.EmailPort);
                }
                if (!string.IsNullOrEmpty(emailSetting.DefaultCredential))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `default_credential` ");
                        values = string.Concat(values, ", @defaultCredential");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `default_credential` ");
                        values = string.Concat(values, " @defaultCredential");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@defaultCredential", emailSetting.DefaultCredential);
                }
                if (!string.IsNullOrEmpty(emailSetting.EnableSsl))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `enable_ssl` ");
                        values = string.Concat(values, ", @enableSsl");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `enable_ssl` ");
                        values = string.Concat(values, " @enableSsl");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@enableSsl", emailSetting.EnableSsl);
                }
                if (!string.IsNullOrEmpty(emailSetting.ModifiedBy))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `modified_by` ");
                        values = string.Concat(values, ", @modifiedBy");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `modified_by` ");
                        values = string.Concat(values, " @modifiedBy");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", emailSetting.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(emailSetting.CreatedAt))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `created_at` ");
                        values = string.Concat(values, ", @createdAt");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `created_at` ");
                        values = string.Concat(values, " @createdAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", emailSetting.CreatedAt);
                }
                if (!string.IsNullOrEmpty(emailSetting.UpdatedAt))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `updated_at` ");
                        values = string.Concat(values, ", @updatedAt");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `updated_at` ");
                        values = string.Concat(values, " @updatedAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", emailSetting.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
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
                                if (!string.IsNullOrEmpty(emailSettingList[i].Id))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `id` ");
                                        values = string.Concat(values, ", @id" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `id` ");
                                        values = string.Concat(values, "@id" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, emailSettingList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].MailDomainCom))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `mail_domain_com` ");
                                        values = string.Concat(values, ", @mailDomainCom" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `mail_domain_com` ");
                                        values = string.Concat(values, "@mailDomainCom" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@mailDomainCom" + i, emailSettingList[i].MailDomainCom);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].EmailSender))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `email_sender` ");
                                        values = string.Concat(values, ", @emailSender" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `email_sender` ");
                                        values = string.Concat(values, "@emailSender" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@emailSender" + i, emailSettingList[i].EmailSender);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].SenderPassword))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `sender_password` ");
                                        values = string.Concat(values, ", @senderPassword" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `sender_password` ");
                                        values = string.Concat(values, "@senderPassword" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@senderPassword" + i, emailSettingList[i].SenderPassword);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].EmailPort))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `email_port` ");
                                        values = string.Concat(values, ", @emailPort" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `email_port` ");
                                        values = string.Concat(values, "@emailPort" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@emailPort" + i, emailSettingList[i].EmailPort);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].DefaultCredential))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `default_credential` ");
                                        values = string.Concat(values, ", @defaultCredential" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `default_credential` ");
                                        values = string.Concat(values, "@defaultCredential" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@defaultCredential" + i, emailSettingList[i].DefaultCredential);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].EnableSsl))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `enable_ssl` ");
                                        values = string.Concat(values, ", @enableSsl" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `enable_ssl` ");
                                        values = string.Concat(values, "@enableSsl" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@enableSsl" + i, emailSettingList[i].EnableSsl);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `modified_by` ");
                                        values = string.Concat(values, ", @modifiedBy" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `modified_by` ");
                                        values = string.Concat(values, "@modifiedBy" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailSettingList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].CreatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `created_at` ");
                                        values = string.Concat(values, ", @createdAt" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `created_at` ");
                                        values = string.Concat(values, "@createdAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailSettingList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `updated_at` ");
                                        values = string.Concat(values, ", @updatedAt" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `updated_at` ");
                                        values = string.Concat(values, "@updatedAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailSettingList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE em_email_settings SET ";
                if (!string.IsNullOrEmpty(emailSetting.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", emailSetting.Id);
                }
                if (!string.IsNullOrEmpty(emailSetting.MailDomainCom))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `mail_domain_com` = @mailDomainCom");
                    else
                        sql = string.Concat(sql, " `mail_domain_com` = @mailDomainCom");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@mailDomainCom", emailSetting.MailDomainCom);
                }
                if (!string.IsNullOrEmpty(emailSetting.EmailSender))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `email_sender` = @emailSender");
                    else
                        sql = string.Concat(sql, " `email_sender` = @emailSender");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@emailSender", emailSetting.EmailSender);
                }
                if (!string.IsNullOrEmpty(emailSetting.SenderPassword))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `sender_password` = @senderPassword");
                    else
                        sql = string.Concat(sql, " `sender_password` = @senderPassword");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@senderPassword", emailSetting.SenderPassword);
                }
                if (!string.IsNullOrEmpty(emailSetting.EmailPort))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `email_port` = @emailPort");
                    else
                        sql = string.Concat(sql, " `email_port` = @emailPort");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@emailPort", emailSetting.EmailPort);
                }
                if (!string.IsNullOrEmpty(emailSetting.DefaultCredential))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `default_credential` = @defaultCredential");
                    else
                        sql = string.Concat(sql, " `default_credential` = @defaultCredential");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@defaultCredential", emailSetting.DefaultCredential);
                }
                if (!string.IsNullOrEmpty(emailSetting.EnableSsl))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `enable_ssl` = @enableSsl");
                    else
                        sql = string.Concat(sql, " `enable_ssl` = @enableSsl");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@enableSsl", emailSetting.EnableSsl);
                }
                if (!string.IsNullOrEmpty(emailSetting.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", emailSetting.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(emailSetting.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", emailSetting.CreatedAt);
                }
                if (!string.IsNullOrEmpty(emailSetting.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", emailSetting.UpdatedAt);
                }
                if (emailSettingWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(emailSettingWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", emailSettingWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(emailSettingWhere.MailDomainCom))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `mail_domain_com` = @mailDomainComWhere");
                        else
                            sql = string.Concat(sql, " `mail_domain_com` = @mailDomainComWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@mailDomainComWhere", emailSettingWhere.MailDomainCom);
                    }
                    if (!string.IsNullOrEmpty(emailSettingWhere.EmailSender))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `email_sender` = @emailSenderWhere");
                        else
                            sql = string.Concat(sql, " `email_sender` = @emailSenderWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@emailSenderWhere", emailSettingWhere.EmailSender);
                    }
                    if (!string.IsNullOrEmpty(emailSettingWhere.SenderPassword))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `sender_password` = @senderPasswordWhere");
                        else
                            sql = string.Concat(sql, " `sender_password` = @senderPasswordWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@senderPasswordWhere", emailSettingWhere.SenderPassword);
                    }
                    if (!string.IsNullOrEmpty(emailSettingWhere.EmailPort))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `email_port` = @emailPortWhere");
                        else
                            sql = string.Concat(sql, " `email_port` = @emailPortWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@emailPortWhere", emailSettingWhere.EmailPort);
                    }
                    if (!string.IsNullOrEmpty(emailSettingWhere.DefaultCredential))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `default_credential` = @defaultCredentialWhere");
                        else
                            sql = string.Concat(sql, " `default_credential` = @defaultCredentialWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@defaultCredentialWhere", emailSettingWhere.DefaultCredential);
                    }
                    if (!string.IsNullOrEmpty(emailSettingWhere.EnableSsl))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `enable_ssl` = @enableSslWhere");
                        else
                            sql = string.Concat(sql, " `enable_ssl` = @enableSslWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@enableSslWhere", emailSettingWhere.EnableSsl);
                    }
                    if (!string.IsNullOrEmpty(emailSettingWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", emailSettingWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(emailSettingWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", emailSettingWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(emailSettingWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
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
                                if (!string.IsNullOrEmpty(emailSettingList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, emailSettingList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].MailDomainCom))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `mail_domain_com` = @mailDomainCom" + i);
                                    else
                                        sql = string.Concat(sql, " `mail_domain_com` = @mailDomainCom" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@mailDomainCom" + i, emailSettingList[i].MailDomainCom);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].EmailSender))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `email_sender` = @emailSender" + i);
                                    else
                                        sql = string.Concat(sql, " `email_sender` = @emailSender" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@emailSender" + i, emailSettingList[i].EmailSender);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].SenderPassword))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `sender_password` = @senderPassword" + i);
                                    else
                                        sql = string.Concat(sql, " `sender_password` = @senderPassword" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@senderPassword" + i, emailSettingList[i].SenderPassword);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].EmailPort))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `email_port` = @emailPort" + i);
                                    else
                                        sql = string.Concat(sql, " `email_port` = @emailPort" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@emailPort" + i, emailSettingList[i].EmailPort);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].DefaultCredential))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `default_credential` = @defaultCredential" + i);
                                    else
                                        sql = string.Concat(sql, " `default_credential` = @defaultCredential" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@defaultCredential" + i, emailSettingList[i].DefaultCredential);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].EnableSsl))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `enable_ssl` = @enableSsl" + i);
                                    else
                                        sql = string.Concat(sql, " `enable_ssl` = @enableSsl" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@enableSsl" + i, emailSettingList[i].EnableSsl);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailSettingList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailSettingList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(emailSettingList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailSettingList[i].UpdatedAt);
                                }
                                if (emailSettingWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(emailSettingWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, emailSettingWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(emailSettingWhereList[i].MailDomainCom))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `mail_domain_com` = @mailDomainComWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `mail_domain_com` = @mailDomainComWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@mailDomainComWhere" + i, emailSettingWhereList[i].MailDomainCom);
                                    }
                                    if (!string.IsNullOrEmpty(emailSettingWhereList[i].EmailSender))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `email_sender` = @emailSenderWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `email_sender` = @emailSenderWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@emailSenderWhere" + i, emailSettingWhereList[i].EmailSender);
                                    }
                                    if (!string.IsNullOrEmpty(emailSettingWhereList[i].SenderPassword))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `sender_password` = @senderPasswordWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `sender_password` = @senderPasswordWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@senderPasswordWhere" + i, emailSettingWhereList[i].SenderPassword);
                                    }
                                    if (!string.IsNullOrEmpty(emailSettingWhereList[i].EmailPort))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `email_port` = @emailPortWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `email_port` = @emailPortWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@emailPortWhere" + i, emailSettingWhereList[i].EmailPort);
                                    }
                                    if (!string.IsNullOrEmpty(emailSettingWhereList[i].DefaultCredential))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `default_credential` = @defaultCredentialWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `default_credential` = @defaultCredentialWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@defaultCredentialWhere" + i, emailSettingWhereList[i].DefaultCredential);
                                    }
                                    if (!string.IsNullOrEmpty(emailSettingWhereList[i].EnableSsl))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `enable_ssl` = @enableSslWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `enable_ssl` = @enableSslWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@enableSslWhere" + i, emailSettingWhereList[i].EnableSsl);
                                    }
                                    if (!string.IsNullOrEmpty(emailSettingWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, emailSettingWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(emailSettingWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, emailSettingWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(emailSettingWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM em_email_settings  ";
                if (emailSetting != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(emailSetting.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", emailSetting.Id);
                    }
                    if (!string.IsNullOrEmpty(emailSetting.MailDomainCom))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `mail_domain_com` = @mailDomainCom");
                        else
                            sql = string.Concat(sql, " `mail_domain_com` = @mailDomainCom");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@mailDomainCom", emailSetting.MailDomainCom);
                    }
                    if (!string.IsNullOrEmpty(emailSetting.EmailSender))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `email_sender` = @emailSender");
                        else
                            sql = string.Concat(sql, " `email_sender` = @emailSender");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@emailSender", emailSetting.EmailSender);
                    }
                    if (!string.IsNullOrEmpty(emailSetting.SenderPassword))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `sender_password` = @senderPassword");
                        else
                            sql = string.Concat(sql, " `sender_password` = @senderPassword");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@senderPassword", emailSetting.SenderPassword);
                    }
                    if (!string.IsNullOrEmpty(emailSetting.EmailPort))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `email_port` = @emailPort");
                        else
                            sql = string.Concat(sql, " `email_port` = @emailPort");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@emailPort", emailSetting.EmailPort);
                    }
                    if (!string.IsNullOrEmpty(emailSetting.DefaultCredential))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `default_credential` = @defaultCredential");
                        else
                            sql = string.Concat(sql, " `default_credential` = @defaultCredential");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@defaultCredential", emailSetting.DefaultCredential);
                    }
                    if (!string.IsNullOrEmpty(emailSetting.EnableSsl))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `enable_ssl` = @enableSsl");
                        else
                            sql = string.Concat(sql, " `enable_ssl` = @enableSsl");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@enableSsl", emailSetting.EnableSsl);
                    }
                    if (!string.IsNullOrEmpty(emailSetting.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", emailSetting.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(emailSetting.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", emailSetting.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(emailSetting.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
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
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(emailSetting[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, emailSetting[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(emailSetting[i].MailDomainCom))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `mail_domain_com` = @mailDomainCom" + i);
                                        else
                                            sql = string.Concat(sql, " `mail_domain_com` = @mailDomainCom" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@mailDomainCom" + i, emailSetting[i].MailDomainCom);
                                    }
                                    if (!string.IsNullOrEmpty(emailSetting[i].EmailSender))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `email_sender` = @emailSender" + i);
                                        else
                                            sql = string.Concat(sql, " `email_sender` = @emailSender" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@emailSender" + i, emailSetting[i].EmailSender);
                                    }
                                    if (!string.IsNullOrEmpty(emailSetting[i].SenderPassword))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `sender_password` = @senderPassword" + i);
                                        else
                                            sql = string.Concat(sql, " `sender_password` = @senderPassword" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@senderPassword" + i, emailSetting[i].SenderPassword);
                                    }
                                    if (!string.IsNullOrEmpty(emailSetting[i].EmailPort))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `email_port` = @emailPort" + i);
                                        else
                                            sql = string.Concat(sql, " `email_port` = @emailPort" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@emailPort" + i, emailSetting[i].EmailPort);
                                    }
                                    if (!string.IsNullOrEmpty(emailSetting[i].DefaultCredential))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `default_credential` = @defaultCredential" + i);
                                        else
                                            sql = string.Concat(sql, " `default_credential` = @defaultCredential" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@defaultCredential" + i, emailSetting[i].DefaultCredential);
                                    }
                                    if (!string.IsNullOrEmpty(emailSetting[i].EnableSsl))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `enable_ssl` = @enableSsl" + i);
                                        else
                                            sql = string.Concat(sql, " `enable_ssl` = @enableSsl" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@enableSsl" + i, emailSetting[i].EnableSsl);
                                    }
                                    if (!string.IsNullOrEmpty(emailSetting[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, emailSetting[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(emailSetting[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, emailSetting[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(emailSetting[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

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
