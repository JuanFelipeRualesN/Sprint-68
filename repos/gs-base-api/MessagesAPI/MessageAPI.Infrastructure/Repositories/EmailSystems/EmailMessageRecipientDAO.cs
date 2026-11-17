using System;
using System.Collections.Generic;
using MySqlConnector;
using MessagesAPI.MessageAPI.Domain.Models.EmailSystem;
using MessagesAPI.MessageAPI.Services.EmailSystems.CustomClasses;

namespace MessagesAPI.MessageAPI.Infrastructure.Repositories.EmailSystems
{
    class EmailMessageRecipientDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, EmailMessageRecipientModel> ReadD(string connectionString, EmailMessageRecipientModel emailMessageRecipient, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailMessageRecipientModel> response = new Dictionary<string, EmailMessageRecipientModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `recipient_id`, `message_id`, `is_read`, `date_read`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_message_recipients ";
                {
                    if (emailMessageRecipient != null)
                    {
                        if (!string.IsNullOrEmpty(emailMessageRecipient.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailMessageRecipient.Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.RecipientId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `recipient_id` = @recipientId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `recipient_id` = @recipientId ");
                            }
                            cmd.Parameters.AddWithValue("@recipientId", emailMessageRecipient.RecipientId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.MessageId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `message_id` = @messageId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `message_id` = @messageId ");
                            }
                            cmd.Parameters.AddWithValue("@messageId", emailMessageRecipient.MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.IsRead))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_read` = @isRead ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_read` = @isRead ");
                            }
                            cmd.Parameters.AddWithValue("@isRead", emailMessageRecipient.IsRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.DateRead))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `date_read` = @dateRead ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `date_read` = @dateRead ");
                            }
                            cmd.Parameters.AddWithValue("@dateRead", emailMessageRecipient.DateRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailMessageRecipient.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailMessageRecipient.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailMessageRecipient.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        if (firstOne)
                            sql = string.Concat(sql, " LIMIT 1 ");
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    emailMessageRecipient.Id = reader.GetValue(0).ToString();
                                else
                                    emailMessageRecipient.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailMessageRecipient.RecipientId = reader.GetValue(1).ToString();
                                else
                                    emailMessageRecipient.RecipientId = null;
                                if (!reader.IsDBNull(2))
                                    emailMessageRecipient.MessageId = reader.GetValue(2).ToString();
                                else
                                    emailMessageRecipient.MessageId = null;
                                if (!reader.IsDBNull(3))
                                    emailMessageRecipient.IsRead = reader.GetValue(3).ToString();
                                else
                                    emailMessageRecipient.IsRead = null;
                                if (!reader.IsDBNull(4))
                                    emailMessageRecipient.DateRead = reader.GetValue(4).ToString();
                                else
                                    emailMessageRecipient.DateRead = null;
                                if (!reader.IsDBNull(5))
                                    emailMessageRecipient.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    emailMessageRecipient.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    emailMessageRecipient.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    emailMessageRecipient.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    emailMessageRecipient.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    emailMessageRecipient.UpdatedAt = null;
                                response.Add(emailMessageRecipient.Id, emailMessageRecipient);
                                emailMessageRecipient = new EmailMessageRecipientModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EmailMessageRecipientModel>();
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
                response = new Dictionary<string, EmailMessageRecipientModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EmailMessageRecipientModel> ReadD(string connectionString, EmailMessageRecipientModel emailMessageRecipient, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailMessageRecipientModel> response = new Dictionary<string, EmailMessageRecipientModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `recipient_id`, `message_id`, `is_read`, `date_read`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_message_recipients ";
                {
                    if (emailMessageRecipient != null)
                    {
                        if (!string.IsNullOrEmpty(emailMessageRecipient.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailMessageRecipient.Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.RecipientId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `recipient_id` = @recipientId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `recipient_id` = @recipientId ");
                            }
                            cmd.Parameters.AddWithValue("@recipientId", emailMessageRecipient.RecipientId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.MessageId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `message_id` = @messageId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `message_id` = @messageId ");
                            }
                            cmd.Parameters.AddWithValue("@messageId", emailMessageRecipient.MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.IsRead))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_read` = @isRead ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_read` = @isRead ");
                            }
                            cmd.Parameters.AddWithValue("@isRead", emailMessageRecipient.IsRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.DateRead))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `date_read` = @dateRead ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `date_read` = @dateRead ");
                            }
                            cmd.Parameters.AddWithValue("@dateRead", emailMessageRecipient.DateRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailMessageRecipient.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailMessageRecipient.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailMessageRecipient.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                        cmd.Parameters.AddWithValue("@fromRow", fromRow);
                        cmd.Parameters.AddWithValue("@limit", limit);
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    emailMessageRecipient.Id = reader.GetValue(0).ToString();
                                else
                                    emailMessageRecipient.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailMessageRecipient.RecipientId = reader.GetValue(1).ToString();
                                else
                                    emailMessageRecipient.RecipientId = null;
                                if (!reader.IsDBNull(2))
                                    emailMessageRecipient.MessageId = reader.GetValue(2).ToString();
                                else
                                    emailMessageRecipient.MessageId = null;
                                if (!reader.IsDBNull(3))
                                    emailMessageRecipient.IsRead = reader.GetValue(3).ToString();
                                else
                                    emailMessageRecipient.IsRead = null;
                                if (!reader.IsDBNull(4))
                                    emailMessageRecipient.DateRead = reader.GetValue(4).ToString();
                                else
                                    emailMessageRecipient.DateRead = null;
                                if (!reader.IsDBNull(5))
                                    emailMessageRecipient.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    emailMessageRecipient.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    emailMessageRecipient.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    emailMessageRecipient.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    emailMessageRecipient.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    emailMessageRecipient.UpdatedAt = null;
                                response.Add(emailMessageRecipient.Id, emailMessageRecipient);
                                emailMessageRecipient = new EmailMessageRecipientModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EmailMessageRecipientModel>();
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
                response = new Dictionary<string, EmailMessageRecipientModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EmailMessageRecipientModel> ReadD(string connectionString, List<EmailMessageRecipientModel> emailMessageRecipientWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailMessageRecipientModel> response = new Dictionary<string, EmailMessageRecipientModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `recipient_id`, `message_id`, `is_read`, `date_read`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_message_recipients ";
                if (emailMessageRecipientWhere != null && emailMessageRecipientWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailMessageRecipientWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailMessageRecipientWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].RecipientId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `recipient_id` = @recipientId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@recipientId" + i, emailMessageRecipientWhere[i].RecipientId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].MessageId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_id` = @messageId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@messageId" + i, emailMessageRecipientWhere[i].MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].IsRead))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_read` = @isRead" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isRead" + i, emailMessageRecipientWhere[i].IsRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].DateRead))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_read` = @dateRead" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateRead" + i, emailMessageRecipientWhere[i].DateRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageRecipientWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageRecipientWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageRecipientWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailMessageRecipientWhere.Count - 1)
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
                            EmailMessageRecipientModel emailMessageRecipient = new EmailMessageRecipientModel(); if (!reader.IsDBNull(0))
                                emailMessageRecipient.Id = reader.GetValue(0).ToString();
                            else
                                emailMessageRecipient.Id = null;
                            if (!reader.IsDBNull(1))
                                emailMessageRecipient.RecipientId = reader.GetValue(1).ToString();
                            else
                                emailMessageRecipient.RecipientId = null;
                            if (!reader.IsDBNull(2))
                                emailMessageRecipient.MessageId = reader.GetValue(2).ToString();
                            else
                                emailMessageRecipient.MessageId = null;
                            if (!reader.IsDBNull(3))
                                emailMessageRecipient.IsRead = reader.GetValue(3).ToString();
                            else
                                emailMessageRecipient.IsRead = null;
                            if (!reader.IsDBNull(4))
                                emailMessageRecipient.DateRead = reader.GetValue(4).ToString();
                            else
                                emailMessageRecipient.DateRead = null;
                            if (!reader.IsDBNull(5))
                                emailMessageRecipient.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                emailMessageRecipient.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                emailMessageRecipient.CreatedAt = reader.GetValue(6).ToString();
                            else
                                emailMessageRecipient.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                emailMessageRecipient.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                emailMessageRecipient.UpdatedAt = null;
                            response.Add(emailMessageRecipient.Id, emailMessageRecipient);
                            emailMessageRecipient = new EmailMessageRecipientModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EmailMessageRecipientModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EmailMessageRecipientModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EmailMessageRecipientModel> ReadD(string connectionString, List<EmailMessageRecipientModel> emailMessageRecipientWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailMessageRecipientModel> response = new Dictionary<string, EmailMessageRecipientModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `recipient_id`, `message_id`, `is_read`, `date_read`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_message_recipients ";
                if (emailMessageRecipientWhere != null && emailMessageRecipientWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailMessageRecipientWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailMessageRecipientWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].RecipientId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `recipient_id` = @recipientId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@recipientId" + i, emailMessageRecipientWhere[i].RecipientId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].MessageId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_id` = @messageId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@messageId" + i, emailMessageRecipientWhere[i].MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].IsRead))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_read` = @isRead" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isRead" + i, emailMessageRecipientWhere[i].IsRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].DateRead))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_read` = @dateRead" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateRead" + i, emailMessageRecipientWhere[i].DateRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageRecipientWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageRecipientWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageRecipientWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailMessageRecipientWhere.Count - 1)
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
                            EmailMessageRecipientModel emailMessageRecipient = new EmailMessageRecipientModel(); if (!reader.IsDBNull(0))
                                emailMessageRecipient.Id = reader.GetValue(0).ToString();
                            else
                                emailMessageRecipient.Id = null;
                            if (!reader.IsDBNull(1))
                                emailMessageRecipient.RecipientId = reader.GetValue(1).ToString();
                            else
                                emailMessageRecipient.RecipientId = null;
                            if (!reader.IsDBNull(2))
                                emailMessageRecipient.MessageId = reader.GetValue(2).ToString();
                            else
                                emailMessageRecipient.MessageId = null;
                            if (!reader.IsDBNull(3))
                                emailMessageRecipient.IsRead = reader.GetValue(3).ToString();
                            else
                                emailMessageRecipient.IsRead = null;
                            if (!reader.IsDBNull(4))
                                emailMessageRecipient.DateRead = reader.GetValue(4).ToString();
                            else
                                emailMessageRecipient.DateRead = null;
                            if (!reader.IsDBNull(5))
                                emailMessageRecipient.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                emailMessageRecipient.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                emailMessageRecipient.CreatedAt = reader.GetValue(6).ToString();
                            else
                                emailMessageRecipient.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                emailMessageRecipient.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                emailMessageRecipient.UpdatedAt = null;
                            response.Add(emailMessageRecipient.Id, emailMessageRecipient);
                            emailMessageRecipient = new EmailMessageRecipientModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EmailMessageRecipientModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EmailMessageRecipientModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailMessageRecipientModel> Read(string connectionString, EmailMessageRecipientModel emailMessageRecipient, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageRecipientModel> response = new List<EmailMessageRecipientModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `recipient_id`, `message_id`, `is_read`, `date_read`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_message_recipients ";
                {
                    if (emailMessageRecipient != null)
                    {
                        if (!string.IsNullOrEmpty(emailMessageRecipient.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailMessageRecipient.Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.RecipientId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `recipient_id` = @recipientId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `recipient_id` = @recipientId ");
                            }
                            cmd.Parameters.AddWithValue("@recipientId", emailMessageRecipient.RecipientId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.MessageId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `message_id` = @messageId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `message_id` = @messageId ");
                            }
                            cmd.Parameters.AddWithValue("@messageId", emailMessageRecipient.MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.IsRead))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_read` = @isRead ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_read` = @isRead ");
                            }
                            cmd.Parameters.AddWithValue("@isRead", emailMessageRecipient.IsRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.DateRead))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `date_read` = @dateRead ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `date_read` = @dateRead ");
                            }
                            cmd.Parameters.AddWithValue("@dateRead", emailMessageRecipient.DateRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailMessageRecipient.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailMessageRecipient.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailMessageRecipient.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        if (firstOne)
                            sql = string.Concat(sql, " LIMIT 1 ");
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    emailMessageRecipient.Id = reader.GetValue(0).ToString();
                                else
                                    emailMessageRecipient.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailMessageRecipient.RecipientId = reader.GetValue(1).ToString();
                                else
                                    emailMessageRecipient.RecipientId = null;
                                if (!reader.IsDBNull(2))
                                    emailMessageRecipient.MessageId = reader.GetValue(2).ToString();
                                else
                                    emailMessageRecipient.MessageId = null;
                                if (!reader.IsDBNull(3))
                                    emailMessageRecipient.IsRead = reader.GetValue(3).ToString();
                                else
                                    emailMessageRecipient.IsRead = null;
                                if (!reader.IsDBNull(4))
                                    emailMessageRecipient.DateRead = reader.GetValue(4).ToString();
                                else
                                    emailMessageRecipient.DateRead = null;
                                if (!reader.IsDBNull(5))
                                    emailMessageRecipient.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    emailMessageRecipient.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    emailMessageRecipient.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    emailMessageRecipient.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    emailMessageRecipient.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    emailMessageRecipient.UpdatedAt = null;
                                response.Add(emailMessageRecipient);
                                emailMessageRecipient = new EmailMessageRecipientModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EmailMessageRecipientModel>();
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
                response = new List<EmailMessageRecipientModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailMessageRecipientModel> Read(string connectionString, EmailMessageRecipientModel emailMessageRecipient, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageRecipientModel> response = new List<EmailMessageRecipientModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `recipient_id`, `message_id`, `is_read`, `date_read`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_message_recipients ";
                {
                    if (emailMessageRecipient != null)
                    {
                        if (!string.IsNullOrEmpty(emailMessageRecipient.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailMessageRecipient.Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.RecipientId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `recipient_id` = @recipientId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `recipient_id` = @recipientId ");
                            }
                            cmd.Parameters.AddWithValue("@recipientId", emailMessageRecipient.RecipientId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.MessageId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `message_id` = @messageId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `message_id` = @messageId ");
                            }
                            cmd.Parameters.AddWithValue("@messageId", emailMessageRecipient.MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.IsRead))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_read` = @isRead ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_read` = @isRead ");
                            }
                            cmd.Parameters.AddWithValue("@isRead", emailMessageRecipient.IsRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.DateRead))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `date_read` = @dateRead ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `date_read` = @dateRead ");
                            }
                            cmd.Parameters.AddWithValue("@dateRead", emailMessageRecipient.DateRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailMessageRecipient.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailMessageRecipient.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipient.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailMessageRecipient.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                        cmd.Parameters.AddWithValue("@fromRow", fromRow);
                        cmd.Parameters.AddWithValue("@limit", limit);
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    emailMessageRecipient.Id = reader.GetValue(0).ToString();
                                else
                                    emailMessageRecipient.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailMessageRecipient.RecipientId = reader.GetValue(1).ToString();
                                else
                                    emailMessageRecipient.RecipientId = null;
                                if (!reader.IsDBNull(2))
                                    emailMessageRecipient.MessageId = reader.GetValue(2).ToString();
                                else
                                    emailMessageRecipient.MessageId = null;
                                if (!reader.IsDBNull(3))
                                    emailMessageRecipient.IsRead = reader.GetValue(3).ToString();
                                else
                                    emailMessageRecipient.IsRead = null;
                                if (!reader.IsDBNull(4))
                                    emailMessageRecipient.DateRead = reader.GetValue(4).ToString();
                                else
                                    emailMessageRecipient.DateRead = null;
                                if (!reader.IsDBNull(5))
                                    emailMessageRecipient.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    emailMessageRecipient.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    emailMessageRecipient.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    emailMessageRecipient.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    emailMessageRecipient.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    emailMessageRecipient.UpdatedAt = null;
                                response.Add(emailMessageRecipient);
                                emailMessageRecipient = new EmailMessageRecipientModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EmailMessageRecipientModel>();
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
                response = new List<EmailMessageRecipientModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailMessageRecipientModel> Read(string connectionString, List<EmailMessageRecipientModel> emailMessageRecipientWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageRecipientModel> response = new List<EmailMessageRecipientModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `recipient_id`, `message_id`, `is_read`, `date_read`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_message_recipients ";
                if (emailMessageRecipientWhere != null && emailMessageRecipientWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailMessageRecipientWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailMessageRecipientWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].RecipientId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `recipient_id` = @recipientId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@recipientId" + i, emailMessageRecipientWhere[i].RecipientId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].MessageId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_id` = @messageId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@messageId" + i, emailMessageRecipientWhere[i].MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].IsRead))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_read` = @isRead" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isRead" + i, emailMessageRecipientWhere[i].IsRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].DateRead))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_read` = @dateRead" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateRead" + i, emailMessageRecipientWhere[i].DateRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageRecipientWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageRecipientWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageRecipientWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailMessageRecipientWhere.Count - 1)
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
                            EmailMessageRecipientModel emailMessageRecipient = new EmailMessageRecipientModel(); if (!reader.IsDBNull(0))
                                emailMessageRecipient.Id = reader.GetValue(0).ToString();
                            else
                                emailMessageRecipient.Id = null;
                            if (!reader.IsDBNull(1))
                                emailMessageRecipient.RecipientId = reader.GetValue(1).ToString();
                            else
                                emailMessageRecipient.RecipientId = null;
                            if (!reader.IsDBNull(2))
                                emailMessageRecipient.MessageId = reader.GetValue(2).ToString();
                            else
                                emailMessageRecipient.MessageId = null;
                            if (!reader.IsDBNull(3))
                                emailMessageRecipient.IsRead = reader.GetValue(3).ToString();
                            else
                                emailMessageRecipient.IsRead = null;
                            if (!reader.IsDBNull(4))
                                emailMessageRecipient.DateRead = reader.GetValue(4).ToString();
                            else
                                emailMessageRecipient.DateRead = null;
                            if (!reader.IsDBNull(5))
                                emailMessageRecipient.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                emailMessageRecipient.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                emailMessageRecipient.CreatedAt = reader.GetValue(6).ToString();
                            else
                                emailMessageRecipient.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                emailMessageRecipient.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                emailMessageRecipient.UpdatedAt = null;
                            response.Add(emailMessageRecipient);
                            emailMessageRecipient = new EmailMessageRecipientModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EmailMessageRecipientModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageRecipientModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailMessageRecipientModel> Read(string connectionString, List<EmailMessageRecipientModel> emailMessageRecipientWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageRecipientModel> response = new List<EmailMessageRecipientModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `recipient_id`, `message_id`, `is_read`, `date_read`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_message_recipients ";
                if (emailMessageRecipientWhere != null && emailMessageRecipientWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailMessageRecipientWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailMessageRecipientWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].RecipientId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `recipient_id` = @recipientId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@recipientId" + i, emailMessageRecipientWhere[i].RecipientId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].MessageId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_id` = @messageId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@messageId" + i, emailMessageRecipientWhere[i].MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].IsRead))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_read` = @isRead" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isRead" + i, emailMessageRecipientWhere[i].IsRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].DateRead))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_read` = @dateRead" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateRead" + i, emailMessageRecipientWhere[i].DateRead);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageRecipientWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageRecipientWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessageRecipientWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageRecipientWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailMessageRecipientWhere.Count - 1)
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
                            EmailMessageRecipientModel emailMessageRecipient = new EmailMessageRecipientModel(); if (!reader.IsDBNull(0))
                                emailMessageRecipient.Id = reader.GetValue(0).ToString();
                            else
                                emailMessageRecipient.Id = null;
                            if (!reader.IsDBNull(1))
                                emailMessageRecipient.RecipientId = reader.GetValue(1).ToString();
                            else
                                emailMessageRecipient.RecipientId = null;
                            if (!reader.IsDBNull(2))
                                emailMessageRecipient.MessageId = reader.GetValue(2).ToString();
                            else
                                emailMessageRecipient.MessageId = null;
                            if (!reader.IsDBNull(3))
                                emailMessageRecipient.IsRead = reader.GetValue(3).ToString();
                            else
                                emailMessageRecipient.IsRead = null;
                            if (!reader.IsDBNull(4))
                                emailMessageRecipient.DateRead = reader.GetValue(4).ToString();
                            else
                                emailMessageRecipient.DateRead = null;
                            if (!reader.IsDBNull(5))
                                emailMessageRecipient.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                emailMessageRecipient.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                emailMessageRecipient.CreatedAt = reader.GetValue(6).ToString();
                            else
                                emailMessageRecipient.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                emailMessageRecipient.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                emailMessageRecipient.UpdatedAt = null;
                            response.Add(emailMessageRecipient);
                            emailMessageRecipient = new EmailMessageRecipientModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EmailMessageRecipientModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageRecipientModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailMessageRecipientModel> Create(string connectionString, EmailMessageRecipientModel emailMessageRecipient)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageRecipientModel> response = new List<EmailMessageRecipientModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO cm_email_message_recipients (";
                string values = "(";
                if (!string.IsNullOrEmpty(emailMessageRecipient.Id))
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
                    cmd.Parameters.AddWithValue("@id", emailMessageRecipient.Id);
                }
                if (!string.IsNullOrEmpty(emailMessageRecipient.RecipientId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `recipient_id` ");
                        values = string.Concat(values, ", @recipientId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `recipient_id` ");
                        values = string.Concat(values, " @recipientId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@recipientId", emailMessageRecipient.RecipientId);
                }
                if (!string.IsNullOrEmpty(emailMessageRecipient.MessageId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `message_id` ");
                        values = string.Concat(values, ", @messageId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `message_id` ");
                        values = string.Concat(values, " @messageId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@messageId", emailMessageRecipient.MessageId);
                }
                if (!string.IsNullOrEmpty(emailMessageRecipient.IsRead))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `is_read` ");
                        values = string.Concat(values, ", @isRead");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `is_read` ");
                        values = string.Concat(values, " @isRead");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isRead", emailMessageRecipient.IsRead);
                }
                if (!string.IsNullOrEmpty(emailMessageRecipient.DateRead))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `date_read` ");
                        values = string.Concat(values, ", @dateRead");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `date_read` ");
                        values = string.Concat(values, " @dateRead");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dateRead", emailMessageRecipient.DateRead);
                }
                if (!string.IsNullOrEmpty(emailMessageRecipient.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", emailMessageRecipient.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(emailMessageRecipient.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", emailMessageRecipient.CreatedAt);
                }
                if (!string.IsNullOrEmpty(emailMessageRecipient.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", emailMessageRecipient.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                emailMessageRecipient.Id = cmd.LastInsertedId.ToString();
                response.Add(emailMessageRecipient);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageRecipientModel>();
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
        public List<EmailMessageRecipientModel> Create(string connectionString, List<EmailMessageRecipientModel> emailMessageRecipientList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageRecipientModel> response = new List<EmailMessageRecipientModel>();
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
                        for (int i = 0; i < emailMessageRecipientList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO cm_email_message_recipients (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, emailMessageRecipientList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].RecipientId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `recipient_id` ");
                                        values = string.Concat(values, ", @recipientId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `recipient_id` ");
                                        values = string.Concat(values, "@recipientId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@recipientId" + i, emailMessageRecipientList[i].RecipientId);
                                }
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].MessageId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `message_id` ");
                                        values = string.Concat(values, ", @messageId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `message_id` ");
                                        values = string.Concat(values, "@messageId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@messageId" + i, emailMessageRecipientList[i].MessageId);
                                }
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].IsRead))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `is_read` ");
                                        values = string.Concat(values, ", @isRead" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `is_read` ");
                                        values = string.Concat(values, "@isRead" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isRead" + i, emailMessageRecipientList[i].IsRead);
                                }
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].DateRead))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `date_read` ");
                                        values = string.Concat(values, ", @dateRead" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `date_read` ");
                                        values = string.Concat(values, "@dateRead" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dateRead" + i, emailMessageRecipientList[i].DateRead);
                                }
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageRecipientList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageRecipientList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageRecipientList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(emailMessageRecipientList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailMessageRecipientModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailMessageRecipientModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageRecipientModel>();
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

        public List<EmailMessageRecipientModel> Update(string connectionString, EmailMessageRecipientModel emailMessageRecipient, EmailMessageRecipientModel emailMessageRecipientWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageRecipientModel> response = new List<EmailMessageRecipientModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE cm_email_message_recipients SET ";
                if (!string.IsNullOrEmpty(emailMessageRecipient.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", emailMessageRecipient.Id);
                }
                if (!string.IsNullOrEmpty(emailMessageRecipient.RecipientId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `recipient_id` = @recipientId");
                    else
                        sql = string.Concat(sql, " `recipient_id` = @recipientId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@recipientId", emailMessageRecipient.RecipientId);
                }
                if (!string.IsNullOrEmpty(emailMessageRecipient.MessageId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `message_id` = @messageId");
                    else
                        sql = string.Concat(sql, " `message_id` = @messageId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@messageId", emailMessageRecipient.MessageId);
                }
                if (!string.IsNullOrEmpty(emailMessageRecipient.IsRead))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `is_read` = @isRead");
                    else
                        sql = string.Concat(sql, " `is_read` = @isRead");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isRead", emailMessageRecipient.IsRead);
                }
                if (!string.IsNullOrEmpty(emailMessageRecipient.DateRead))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `date_read` = @dateRead");
                    else
                        sql = string.Concat(sql, " `date_read` = @dateRead");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dateRead", emailMessageRecipient.DateRead);
                }
                if (!string.IsNullOrEmpty(emailMessageRecipient.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", emailMessageRecipient.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(emailMessageRecipient.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", emailMessageRecipient.CreatedAt);
                }
                if (!string.IsNullOrEmpty(emailMessageRecipient.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", emailMessageRecipient.UpdatedAt);
                }
                if (emailMessageRecipientWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(emailMessageRecipientWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", emailMessageRecipientWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(emailMessageRecipientWhere.RecipientId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `recipient_id` = @recipientIdWhere");
                        else
                            sql = string.Concat(sql, " `recipient_id` = @recipientIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@recipientIdWhere", emailMessageRecipientWhere.RecipientId);
                    }
                    if (!string.IsNullOrEmpty(emailMessageRecipientWhere.MessageId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `message_id` = @messageIdWhere");
                        else
                            sql = string.Concat(sql, " `message_id` = @messageIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@messageIdWhere", emailMessageRecipientWhere.MessageId);
                    }
                    if (!string.IsNullOrEmpty(emailMessageRecipientWhere.IsRead))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `is_read` = @isReadWhere");
                        else
                            sql = string.Concat(sql, " `is_read` = @isReadWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isReadWhere", emailMessageRecipientWhere.IsRead);
                    }
                    if (!string.IsNullOrEmpty(emailMessageRecipientWhere.DateRead))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `date_read` = @dateReadWhere");
                        else
                            sql = string.Concat(sql, " `date_read` = @dateReadWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dateReadWhere", emailMessageRecipientWhere.DateRead);
                    }
                    if (!string.IsNullOrEmpty(emailMessageRecipientWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", emailMessageRecipientWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(emailMessageRecipientWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", emailMessageRecipientWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(emailMessageRecipientWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", emailMessageRecipientWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(emailMessageRecipient);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageRecipientModel>();
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

        public List<EmailMessageRecipientModel> Update(string connectionString, List<EmailMessageRecipientModel> emailMessageRecipientList, List<EmailMessageRecipientModel> emailMessageRecipientWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageRecipientModel> response = new List<EmailMessageRecipientModel>();
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
                        for (int i = 0; i < emailMessageRecipientList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE cm_email_message_recipients SET ";
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, emailMessageRecipientList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].RecipientId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `recipient_id` = @recipientId" + i);
                                    else
                                        sql = string.Concat(sql, " `recipient_id` = @recipientId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@recipientId" + i, emailMessageRecipientList[i].RecipientId);
                                }
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].MessageId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `message_id` = @messageId" + i);
                                    else
                                        sql = string.Concat(sql, " `message_id` = @messageId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@messageId" + i, emailMessageRecipientList[i].MessageId);
                                }
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].IsRead))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `is_read` = @isRead" + i);
                                    else
                                        sql = string.Concat(sql, " `is_read` = @isRead" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isRead" + i, emailMessageRecipientList[i].IsRead);
                                }
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].DateRead))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `date_read` = @dateRead" + i);
                                    else
                                        sql = string.Concat(sql, " `date_read` = @dateRead" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dateRead" + i, emailMessageRecipientList[i].DateRead);
                                }
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageRecipientList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageRecipientList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(emailMessageRecipientList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageRecipientList[i].UpdatedAt);
                                }
                                if (emailMessageRecipientWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(emailMessageRecipientWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, emailMessageRecipientWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageRecipientWhereList[i].RecipientId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `recipient_id` = @recipientIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `recipient_id` = @recipientIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@recipientIdWhere" + i, emailMessageRecipientWhereList[i].RecipientId);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageRecipientWhereList[i].MessageId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `message_id` = @messageIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `message_id` = @messageIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@messageIdWhere" + i, emailMessageRecipientWhereList[i].MessageId);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageRecipientWhereList[i].IsRead))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `is_read` = @isReadWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `is_read` = @isReadWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isReadWhere" + i, emailMessageRecipientWhereList[i].IsRead);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageRecipientWhereList[i].DateRead))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `date_read` = @dateReadWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `date_read` = @dateReadWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dateReadWhere" + i, emailMessageRecipientWhereList[i].DateRead);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageRecipientWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, emailMessageRecipientWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageRecipientWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, emailMessageRecipientWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageRecipientWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, emailMessageRecipientWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailMessageRecipientList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailMessageRecipientModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailMessageRecipientModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageRecipientModel>();
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

        public List<EmailMessageRecipientModel> Delete(string connectionString, EmailMessageRecipientModel emailMessageRecipient)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageRecipientModel> response = new List<EmailMessageRecipientModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM cm_email_message_recipients  ";
                if (emailMessageRecipient != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(emailMessageRecipient.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", emailMessageRecipient.Id);
                    }
                    if (!string.IsNullOrEmpty(emailMessageRecipient.RecipientId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `recipient_id` = @recipientId");
                        else
                            sql = string.Concat(sql, " `recipient_id` = @recipientId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@recipientId", emailMessageRecipient.RecipientId);
                    }
                    if (!string.IsNullOrEmpty(emailMessageRecipient.MessageId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `message_id` = @messageId");
                        else
                            sql = string.Concat(sql, " `message_id` = @messageId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@messageId", emailMessageRecipient.MessageId);
                    }
                    if (!string.IsNullOrEmpty(emailMessageRecipient.IsRead))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `is_read` = @isRead");
                        else
                            sql = string.Concat(sql, " `is_read` = @isRead");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isRead", emailMessageRecipient.IsRead);
                    }
                    if (!string.IsNullOrEmpty(emailMessageRecipient.DateRead))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `date_read` = @dateRead");
                        else
                            sql = string.Concat(sql, " `date_read` = @dateRead");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dateRead", emailMessageRecipient.DateRead);
                    }
                    if (!string.IsNullOrEmpty(emailMessageRecipient.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", emailMessageRecipient.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(emailMessageRecipient.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", emailMessageRecipient.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(emailMessageRecipient.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", emailMessageRecipient.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(emailMessageRecipient);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageRecipientModel>();
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

        public List<EmailMessageRecipientModel> Delete(string connectionString, List<EmailMessageRecipientModel> emailMessageRecipient)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageRecipientModel> response = new List<EmailMessageRecipientModel>();
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
                        for (int i = 0; i < emailMessageRecipient.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM cm_email_message_recipients  ";
                                if (emailMessageRecipient[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(emailMessageRecipient[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, emailMessageRecipient[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageRecipient[i].RecipientId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `recipient_id` = @recipientId" + i);
                                        else
                                            sql = string.Concat(sql, " `recipient_id` = @recipientId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@recipientId" + i, emailMessageRecipient[i].RecipientId);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageRecipient[i].MessageId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `message_id` = @messageId" + i);
                                        else
                                            sql = string.Concat(sql, " `message_id` = @messageId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@messageId" + i, emailMessageRecipient[i].MessageId);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageRecipient[i].IsRead))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `is_read` = @isRead" + i);
                                        else
                                            sql = string.Concat(sql, " `is_read` = @isRead" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isRead" + i, emailMessageRecipient[i].IsRead);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageRecipient[i].DateRead))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `date_read` = @dateRead" + i);
                                        else
                                            sql = string.Concat(sql, " `date_read` = @dateRead" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dateRead" + i, emailMessageRecipient[i].DateRead);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageRecipient[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageRecipient[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageRecipient[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageRecipient[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageRecipient[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageRecipient[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailMessageRecipient[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailMessageRecipientModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailMessageRecipientModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageRecipientModel>();
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
