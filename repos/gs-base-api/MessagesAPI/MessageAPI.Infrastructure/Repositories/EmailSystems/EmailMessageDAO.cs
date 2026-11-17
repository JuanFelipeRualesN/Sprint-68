using System;
using System.Collections.Generic;
using MySqlConnector;
using MessagesAPI.MessageAPI.Domain.Models.EmailSystem;
using MessagesAPI.MessageAPI.Services.EmailSystems.CustomClasses;

namespace MessagesAPI.MessageAPI.Infrastructure.Repositories.EmailSystems
{
    class EmailMessageDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, EmailMessageModel> ReadD(string connectionString, EmailMessageModel emailMessage, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailMessageModel> response = new Dictionary<string, EmailMessageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subject`, `body`, `date_created`, `parent_message_id`, `author_id`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_messages ";
                {
                    if (emailMessage != null)
                    {
                        if (!string.IsNullOrEmpty(emailMessage.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailMessage.Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.Subject))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject` = @subject ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject` = @subject ");
                            }
                            cmd.Parameters.AddWithValue("@subject", emailMessage.Subject);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.Body))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `body` = @body ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `body` = @body ");
                            }
                            cmd.Parameters.AddWithValue("@body", emailMessage.Body);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.DateCreated))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `date_created` = @dateCreated ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `date_created` = @dateCreated ");
                            }
                            cmd.Parameters.AddWithValue("@dateCreated", emailMessage.DateCreated);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.ParentMessageId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `parent_message_id` = @parentMessageId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `parent_message_id` = @parentMessageId ");
                            }
                            cmd.Parameters.AddWithValue("@parentMessageId", emailMessage.ParentMessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.AuthorId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `author_id` = @authorId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `author_id` = @authorId ");
                            }
                            cmd.Parameters.AddWithValue("@authorId", emailMessage.AuthorId);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailMessage.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailMessage.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailMessage.UpdatedAt);
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
                                    emailMessage.Id = reader.GetValue(0).ToString();
                                else
                                    emailMessage.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailMessage.Subject = reader.GetValue(1).ToString();
                                else
                                    emailMessage.Subject = null;
                                if (!reader.IsDBNull(2))
                                    emailMessage.Body = reader.GetValue(2).ToString();
                                else
                                    emailMessage.Body = null;
                                if (!reader.IsDBNull(3))
                                    emailMessage.DateCreated = reader.GetValue(3).ToString();
                                else
                                    emailMessage.DateCreated = null;
                                if (!reader.IsDBNull(4))
                                    emailMessage.ParentMessageId = reader.GetValue(4).ToString();
                                else
                                    emailMessage.ParentMessageId = null;
                                if (!reader.IsDBNull(5))
                                    emailMessage.AuthorId = reader.GetValue(5).ToString();
                                else
                                    emailMessage.AuthorId = null;
                                if (!reader.IsDBNull(6))
                                    emailMessage.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    emailMessage.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    emailMessage.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    emailMessage.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    emailMessage.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    emailMessage.UpdatedAt = null;
                                response.Add(emailMessage.Id, emailMessage);
                                emailMessage = new EmailMessageModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EmailMessageModel>();
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
                response = new Dictionary<string, EmailMessageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EmailMessageModel> ReadD(string connectionString, EmailMessageModel emailMessage, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailMessageModel> response = new Dictionary<string, EmailMessageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subject`, `body`, `date_created`, `parent_message_id`, `author_id`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_messages ";
                {
                    if (emailMessage != null)
                    {
                        if (!string.IsNullOrEmpty(emailMessage.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailMessage.Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.Subject))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject` = @subject ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject` = @subject ");
                            }
                            cmd.Parameters.AddWithValue("@subject", emailMessage.Subject);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.Body))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `body` = @body ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `body` = @body ");
                            }
                            cmd.Parameters.AddWithValue("@body", emailMessage.Body);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.DateCreated))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `date_created` = @dateCreated ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `date_created` = @dateCreated ");
                            }
                            cmd.Parameters.AddWithValue("@dateCreated", emailMessage.DateCreated);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.ParentMessageId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `parent_message_id` = @parentMessageId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `parent_message_id` = @parentMessageId ");
                            }
                            cmd.Parameters.AddWithValue("@parentMessageId", emailMessage.ParentMessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.AuthorId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `author_id` = @authorId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `author_id` = @authorId ");
                            }
                            cmd.Parameters.AddWithValue("@authorId", emailMessage.AuthorId);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailMessage.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailMessage.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailMessage.UpdatedAt);
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
                                    emailMessage.Id = reader.GetValue(0).ToString();
                                else
                                    emailMessage.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailMessage.Subject = reader.GetValue(1).ToString();
                                else
                                    emailMessage.Subject = null;
                                if (!reader.IsDBNull(2))
                                    emailMessage.Body = reader.GetValue(2).ToString();
                                else
                                    emailMessage.Body = null;
                                if (!reader.IsDBNull(3))
                                    emailMessage.DateCreated = reader.GetValue(3).ToString();
                                else
                                    emailMessage.DateCreated = null;
                                if (!reader.IsDBNull(4))
                                    emailMessage.ParentMessageId = reader.GetValue(4).ToString();
                                else
                                    emailMessage.ParentMessageId = null;
                                if (!reader.IsDBNull(5))
                                    emailMessage.AuthorId = reader.GetValue(5).ToString();
                                else
                                    emailMessage.AuthorId = null;
                                if (!reader.IsDBNull(6))
                                    emailMessage.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    emailMessage.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    emailMessage.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    emailMessage.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    emailMessage.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    emailMessage.UpdatedAt = null;
                                response.Add(emailMessage.Id, emailMessage);
                                emailMessage = new EmailMessageModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EmailMessageModel>();
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
                response = new Dictionary<string, EmailMessageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EmailMessageModel> ReadD(string connectionString, List<EmailMessageModel> emailMessageWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailMessageModel> response = new Dictionary<string, EmailMessageModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subject`, `body`, `date_created`, `parent_message_id`, `author_id`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_messages ";
                if (emailMessageWhere != null && emailMessageWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailMessageWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailMessageWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].Subject))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject` = @subject" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subject" + i, emailMessageWhere[i].Subject);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].Body))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `body` = @body" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@body" + i, emailMessageWhere[i].Body);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].DateCreated))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_created` = @dateCreated" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateCreated" + i, emailMessageWhere[i].DateCreated);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].ParentMessageId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `parent_message_id` = @parentMessageId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@parentMessageId" + i, emailMessageWhere[i].ParentMessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].AuthorId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `author_id` = @authorId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@authorId" + i, emailMessageWhere[i].AuthorId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailMessageWhere.Count - 1)
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
                            EmailMessageModel emailMessage = new EmailMessageModel(); if (!reader.IsDBNull(0))
                                emailMessage.Id = reader.GetValue(0).ToString();
                            else
                                emailMessage.Id = null;
                            if (!reader.IsDBNull(1))
                                emailMessage.Subject = reader.GetValue(1).ToString();
                            else
                                emailMessage.Subject = null;
                            if (!reader.IsDBNull(2))
                                emailMessage.Body = reader.GetValue(2).ToString();
                            else
                                emailMessage.Body = null;
                            if (!reader.IsDBNull(3))
                                emailMessage.DateCreated = reader.GetValue(3).ToString();
                            else
                                emailMessage.DateCreated = null;
                            if (!reader.IsDBNull(4))
                                emailMessage.ParentMessageId = reader.GetValue(4).ToString();
                            else
                                emailMessage.ParentMessageId = null;
                            if (!reader.IsDBNull(5))
                                emailMessage.AuthorId = reader.GetValue(5).ToString();
                            else
                                emailMessage.AuthorId = null;
                            if (!reader.IsDBNull(6))
                                emailMessage.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                emailMessage.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                emailMessage.CreatedAt = reader.GetValue(7).ToString();
                            else
                                emailMessage.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                emailMessage.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                emailMessage.UpdatedAt = null;
                            response.Add(emailMessage.Id, emailMessage);
                            emailMessage = new EmailMessageModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EmailMessageModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EmailMessageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EmailMessageModel> ReadD(string connectionString, List<EmailMessageModel> emailMessageWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailMessageModel> response = new Dictionary<string, EmailMessageModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subject`, `body`, `date_created`, `parent_message_id`, `author_id`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_messages ";
                if (emailMessageWhere != null && emailMessageWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailMessageWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailMessageWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].Subject))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject` = @subject" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subject" + i, emailMessageWhere[i].Subject);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].Body))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `body` = @body" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@body" + i, emailMessageWhere[i].Body);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].DateCreated))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_created` = @dateCreated" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateCreated" + i, emailMessageWhere[i].DateCreated);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].ParentMessageId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `parent_message_id` = @parentMessageId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@parentMessageId" + i, emailMessageWhere[i].ParentMessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].AuthorId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `author_id` = @authorId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@authorId" + i, emailMessageWhere[i].AuthorId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailMessageWhere.Count - 1)
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
                            EmailMessageModel emailMessage = new EmailMessageModel(); if (!reader.IsDBNull(0))
                                emailMessage.Id = reader.GetValue(0).ToString();
                            else
                                emailMessage.Id = null;
                            if (!reader.IsDBNull(1))
                                emailMessage.Subject = reader.GetValue(1).ToString();
                            else
                                emailMessage.Subject = null;
                            if (!reader.IsDBNull(2))
                                emailMessage.Body = reader.GetValue(2).ToString();
                            else
                                emailMessage.Body = null;
                            if (!reader.IsDBNull(3))
                                emailMessage.DateCreated = reader.GetValue(3).ToString();
                            else
                                emailMessage.DateCreated = null;
                            if (!reader.IsDBNull(4))
                                emailMessage.ParentMessageId = reader.GetValue(4).ToString();
                            else
                                emailMessage.ParentMessageId = null;
                            if (!reader.IsDBNull(5))
                                emailMessage.AuthorId = reader.GetValue(5).ToString();
                            else
                                emailMessage.AuthorId = null;
                            if (!reader.IsDBNull(6))
                                emailMessage.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                emailMessage.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                emailMessage.CreatedAt = reader.GetValue(7).ToString();
                            else
                                emailMessage.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                emailMessage.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                emailMessage.UpdatedAt = null;
                            response.Add(emailMessage.Id, emailMessage);
                            emailMessage = new EmailMessageModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EmailMessageModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EmailMessageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailMessageModel> Read(string connectionString, EmailMessageModel emailMessage, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageModel> response = new List<EmailMessageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subject`, `body`, `date_created`, `parent_message_id`, `author_id`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_messages ";
                {
                    if (emailMessage != null)
                    {
                        if (!string.IsNullOrEmpty(emailMessage.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailMessage.Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.Subject))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject` = @subject ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject` = @subject ");
                            }
                            cmd.Parameters.AddWithValue("@subject", emailMessage.Subject);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.Body))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `body` = @body ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `body` = @body ");
                            }
                            cmd.Parameters.AddWithValue("@body", emailMessage.Body);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.DateCreated))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `date_created` = @dateCreated ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `date_created` = @dateCreated ");
                            }
                            cmd.Parameters.AddWithValue("@dateCreated", emailMessage.DateCreated);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.ParentMessageId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `parent_message_id` = @parentMessageId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `parent_message_id` = @parentMessageId ");
                            }
                            cmd.Parameters.AddWithValue("@parentMessageId", emailMessage.ParentMessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.AuthorId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `author_id` = @authorId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `author_id` = @authorId ");
                            }
                            cmd.Parameters.AddWithValue("@authorId", emailMessage.AuthorId);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailMessage.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailMessage.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailMessage.UpdatedAt);
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
                                    emailMessage.Id = reader.GetValue(0).ToString();
                                else
                                    emailMessage.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailMessage.Subject = reader.GetValue(1).ToString();
                                else
                                    emailMessage.Subject = null;
                                if (!reader.IsDBNull(2))
                                    emailMessage.Body = reader.GetValue(2).ToString();
                                else
                                    emailMessage.Body = null;
                                if (!reader.IsDBNull(3))
                                    emailMessage.DateCreated = reader.GetValue(3).ToString();
                                else
                                    emailMessage.DateCreated = null;
                                if (!reader.IsDBNull(4))
                                    emailMessage.ParentMessageId = reader.GetValue(4).ToString();
                                else
                                    emailMessage.ParentMessageId = null;
                                if (!reader.IsDBNull(5))
                                    emailMessage.AuthorId = reader.GetValue(5).ToString();
                                else
                                    emailMessage.AuthorId = null;
                                if (!reader.IsDBNull(6))
                                    emailMessage.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    emailMessage.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    emailMessage.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    emailMessage.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    emailMessage.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    emailMessage.UpdatedAt = null;
                                response.Add(emailMessage);
                                emailMessage = new EmailMessageModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EmailMessageModel>();
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
                response = new List<EmailMessageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailMessageModel> Read(string connectionString, EmailMessageModel emailMessage, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageModel> response = new List<EmailMessageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subject`, `body`, `date_created`, `parent_message_id`, `author_id`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_messages ";
                {
                    if (emailMessage != null)
                    {
                        if (!string.IsNullOrEmpty(emailMessage.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailMessage.Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.Subject))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject` = @subject ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject` = @subject ");
                            }
                            cmd.Parameters.AddWithValue("@subject", emailMessage.Subject);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.Body))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `body` = @body ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `body` = @body ");
                            }
                            cmd.Parameters.AddWithValue("@body", emailMessage.Body);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.DateCreated))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `date_created` = @dateCreated ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `date_created` = @dateCreated ");
                            }
                            cmd.Parameters.AddWithValue("@dateCreated", emailMessage.DateCreated);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.ParentMessageId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `parent_message_id` = @parentMessageId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `parent_message_id` = @parentMessageId ");
                            }
                            cmd.Parameters.AddWithValue("@parentMessageId", emailMessage.ParentMessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.AuthorId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `author_id` = @authorId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `author_id` = @authorId ");
                            }
                            cmd.Parameters.AddWithValue("@authorId", emailMessage.AuthorId);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailMessage.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailMessage.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessage.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailMessage.UpdatedAt);
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
                                    emailMessage.Id = reader.GetValue(0).ToString();
                                else
                                    emailMessage.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailMessage.Subject = reader.GetValue(1).ToString();
                                else
                                    emailMessage.Subject = null;
                                if (!reader.IsDBNull(2))
                                    emailMessage.Body = reader.GetValue(2).ToString();
                                else
                                    emailMessage.Body = null;
                                if (!reader.IsDBNull(3))
                                    emailMessage.DateCreated = reader.GetValue(3).ToString();
                                else
                                    emailMessage.DateCreated = null;
                                if (!reader.IsDBNull(4))
                                    emailMessage.ParentMessageId = reader.GetValue(4).ToString();
                                else
                                    emailMessage.ParentMessageId = null;
                                if (!reader.IsDBNull(5))
                                    emailMessage.AuthorId = reader.GetValue(5).ToString();
                                else
                                    emailMessage.AuthorId = null;
                                if (!reader.IsDBNull(6))
                                    emailMessage.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    emailMessage.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    emailMessage.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    emailMessage.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    emailMessage.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    emailMessage.UpdatedAt = null;
                                response.Add(emailMessage);
                                emailMessage = new EmailMessageModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EmailMessageModel>();
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
                response = new List<EmailMessageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailMessageModel> Read(string connectionString, List<EmailMessageModel> emailMessageWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageModel> response = new List<EmailMessageModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subject`, `body`, `date_created`, `parent_message_id`, `author_id`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_messages ";
                if (emailMessageWhere != null && emailMessageWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailMessageWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailMessageWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].Subject))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject` = @subject" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subject" + i, emailMessageWhere[i].Subject);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].Body))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `body` = @body" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@body" + i, emailMessageWhere[i].Body);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].DateCreated))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_created` = @dateCreated" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateCreated" + i, emailMessageWhere[i].DateCreated);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].ParentMessageId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `parent_message_id` = @parentMessageId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@parentMessageId" + i, emailMessageWhere[i].ParentMessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].AuthorId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `author_id` = @authorId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@authorId" + i, emailMessageWhere[i].AuthorId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailMessageWhere.Count - 1)
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
                            EmailMessageModel emailMessage = new EmailMessageModel(); if (!reader.IsDBNull(0))
                                emailMessage.Id = reader.GetValue(0).ToString();
                            else
                                emailMessage.Id = null;
                            if (!reader.IsDBNull(1))
                                emailMessage.Subject = reader.GetValue(1).ToString();
                            else
                                emailMessage.Subject = null;
                            if (!reader.IsDBNull(2))
                                emailMessage.Body = reader.GetValue(2).ToString();
                            else
                                emailMessage.Body = null;
                            if (!reader.IsDBNull(3))
                                emailMessage.DateCreated = reader.GetValue(3).ToString();
                            else
                                emailMessage.DateCreated = null;
                            if (!reader.IsDBNull(4))
                                emailMessage.ParentMessageId = reader.GetValue(4).ToString();
                            else
                                emailMessage.ParentMessageId = null;
                            if (!reader.IsDBNull(5))
                                emailMessage.AuthorId = reader.GetValue(5).ToString();
                            else
                                emailMessage.AuthorId = null;
                            if (!reader.IsDBNull(6))
                                emailMessage.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                emailMessage.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                emailMessage.CreatedAt = reader.GetValue(7).ToString();
                            else
                                emailMessage.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                emailMessage.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                emailMessage.UpdatedAt = null;
                            response.Add(emailMessage);
                            emailMessage = new EmailMessageModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EmailMessageModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailMessageModel> Read(string connectionString, List<EmailMessageModel> emailMessageWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageModel> response = new List<EmailMessageModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subject`, `body`, `date_created`, `parent_message_id`, `author_id`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_messages ";
                if (emailMessageWhere != null && emailMessageWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailMessageWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailMessageWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].Subject))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject` = @subject" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subject" + i, emailMessageWhere[i].Subject);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].Body))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `body` = @body" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@body" + i, emailMessageWhere[i].Body);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].DateCreated))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_created` = @dateCreated" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateCreated" + i, emailMessageWhere[i].DateCreated);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].ParentMessageId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `parent_message_id` = @parentMessageId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@parentMessageId" + i, emailMessageWhere[i].ParentMessageId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].AuthorId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `author_id` = @authorId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@authorId" + i, emailMessageWhere[i].AuthorId);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailMessageWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailMessageWhere.Count - 1)
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
                            EmailMessageModel emailMessage = new EmailMessageModel(); if (!reader.IsDBNull(0))
                                emailMessage.Id = reader.GetValue(0).ToString();
                            else
                                emailMessage.Id = null;
                            if (!reader.IsDBNull(1))
                                emailMessage.Subject = reader.GetValue(1).ToString();
                            else
                                emailMessage.Subject = null;
                            if (!reader.IsDBNull(2))
                                emailMessage.Body = reader.GetValue(2).ToString();
                            else
                                emailMessage.Body = null;
                            if (!reader.IsDBNull(3))
                                emailMessage.DateCreated = reader.GetValue(3).ToString();
                            else
                                emailMessage.DateCreated = null;
                            if (!reader.IsDBNull(4))
                                emailMessage.ParentMessageId = reader.GetValue(4).ToString();
                            else
                                emailMessage.ParentMessageId = null;
                            if (!reader.IsDBNull(5))
                                emailMessage.AuthorId = reader.GetValue(5).ToString();
                            else
                                emailMessage.AuthorId = null;
                            if (!reader.IsDBNull(6))
                                emailMessage.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                emailMessage.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                emailMessage.CreatedAt = reader.GetValue(7).ToString();
                            else
                                emailMessage.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                emailMessage.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                emailMessage.UpdatedAt = null;
                            response.Add(emailMessage);
                            emailMessage = new EmailMessageModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EmailMessageModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailMessageModel> Create(string connectionString, EmailMessageModel emailMessage)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageModel> response = new List<EmailMessageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO cm_email_messages (";
                string values = "(";
                if (!string.IsNullOrEmpty(emailMessage.Id))
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
                    cmd.Parameters.AddWithValue("@id", emailMessage.Id);
                }
                if (!string.IsNullOrEmpty(emailMessage.Subject))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `subject` ");
                        values = string.Concat(values, ", @subject");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `subject` ");
                        values = string.Concat(values, " @subject");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subject", emailMessage.Subject);
                }
                if (!string.IsNullOrEmpty(emailMessage.Body))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `body` ");
                        values = string.Concat(values, ", @body");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `body` ");
                        values = string.Concat(values, " @body");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@body", emailMessage.Body);
                }
                if (!string.IsNullOrEmpty(emailMessage.DateCreated))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `date_created` ");
                        values = string.Concat(values, ", @dateCreated");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `date_created` ");
                        values = string.Concat(values, " @dateCreated");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dateCreated", emailMessage.DateCreated);
                }
                if (!string.IsNullOrEmpty(emailMessage.ParentMessageId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `parent_message_id` ");
                        values = string.Concat(values, ", @parentMessageId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `parent_message_id` ");
                        values = string.Concat(values, " @parentMessageId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@parentMessageId", emailMessage.ParentMessageId);
                }
                if (!string.IsNullOrEmpty(emailMessage.AuthorId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `author_id` ");
                        values = string.Concat(values, ", @authorId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `author_id` ");
                        values = string.Concat(values, " @authorId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@authorId", emailMessage.AuthorId);
                }
                if (!string.IsNullOrEmpty(emailMessage.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", emailMessage.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(emailMessage.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", emailMessage.CreatedAt);
                }
                if (!string.IsNullOrEmpty(emailMessage.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", emailMessage.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                emailMessage.Id = cmd.LastInsertedId.ToString();
                response.Add(emailMessage);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageModel>();
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
        public List<EmailMessageModel> Create(string connectionString, List<EmailMessageModel> emailMessageList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageModel> response = new List<EmailMessageModel>();
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
                        for (int i = 0; i < emailMessageList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO cm_email_messages (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(emailMessageList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, emailMessageList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].Subject))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `subject` ");
                                        values = string.Concat(values, ", @subject" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `subject` ");
                                        values = string.Concat(values, "@subject" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subject" + i, emailMessageList[i].Subject);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].Body))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `body` ");
                                        values = string.Concat(values, ", @body" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `body` ");
                                        values = string.Concat(values, "@body" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@body" + i, emailMessageList[i].Body);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].DateCreated))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `date_created` ");
                                        values = string.Concat(values, ", @dateCreated" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `date_created` ");
                                        values = string.Concat(values, "@dateCreated" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dateCreated" + i, emailMessageList[i].DateCreated);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].ParentMessageId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `parent_message_id` ");
                                        values = string.Concat(values, ", @parentMessageId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `parent_message_id` ");
                                        values = string.Concat(values, "@parentMessageId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@parentMessageId" + i, emailMessageList[i].ParentMessageId);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].AuthorId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `author_id` ");
                                        values = string.Concat(values, ", @authorId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `author_id` ");
                                        values = string.Concat(values, "@authorId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@authorId" + i, emailMessageList[i].AuthorId);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(emailMessageList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailMessageModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailMessageModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageModel>();
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

        public List<EmailMessageModel> Update(string connectionString, EmailMessageModel emailMessage, EmailMessageModel emailMessageWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageModel> response = new List<EmailMessageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE cm_email_messages SET ";
                if (!string.IsNullOrEmpty(emailMessage.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", emailMessage.Id);
                }
                if (!string.IsNullOrEmpty(emailMessage.Subject))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `subject` = @subject");
                    else
                        sql = string.Concat(sql, " `subject` = @subject");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subject", emailMessage.Subject);
                }
                if (!string.IsNullOrEmpty(emailMessage.Body))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `body` = @body");
                    else
                        sql = string.Concat(sql, " `body` = @body");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@body", emailMessage.Body);
                }
                if (!string.IsNullOrEmpty(emailMessage.DateCreated))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `date_created` = @dateCreated");
                    else
                        sql = string.Concat(sql, " `date_created` = @dateCreated");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dateCreated", emailMessage.DateCreated);
                }
                if (!string.IsNullOrEmpty(emailMessage.ParentMessageId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `parent_message_id` = @parentMessageId");
                    else
                        sql = string.Concat(sql, " `parent_message_id` = @parentMessageId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@parentMessageId", emailMessage.ParentMessageId);
                }
                if (!string.IsNullOrEmpty(emailMessage.AuthorId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `author_id` = @authorId");
                    else
                        sql = string.Concat(sql, " `author_id` = @authorId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@authorId", emailMessage.AuthorId);
                }
                if (!string.IsNullOrEmpty(emailMessage.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", emailMessage.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(emailMessage.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", emailMessage.CreatedAt);
                }
                if (!string.IsNullOrEmpty(emailMessage.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", emailMessage.UpdatedAt);
                }
                if (emailMessageWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(emailMessageWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", emailMessageWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(emailMessageWhere.Subject))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subject` = @subjectWhere");
                        else
                            sql = string.Concat(sql, " `subject` = @subjectWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectWhere", emailMessageWhere.Subject);
                    }
                    if (!string.IsNullOrEmpty(emailMessageWhere.Body))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `body` = @bodyWhere");
                        else
                            sql = string.Concat(sql, " `body` = @bodyWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@bodyWhere", emailMessageWhere.Body);
                    }
                    if (!string.IsNullOrEmpty(emailMessageWhere.DateCreated))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `date_created` = @dateCreatedWhere");
                        else
                            sql = string.Concat(sql, " `date_created` = @dateCreatedWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dateCreatedWhere", emailMessageWhere.DateCreated);
                    }
                    if (!string.IsNullOrEmpty(emailMessageWhere.ParentMessageId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `parent_message_id` = @parentMessageIdWhere");
                        else
                            sql = string.Concat(sql, " `parent_message_id` = @parentMessageIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@parentMessageIdWhere", emailMessageWhere.ParentMessageId);
                    }
                    if (!string.IsNullOrEmpty(emailMessageWhere.AuthorId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `author_id` = @authorIdWhere");
                        else
                            sql = string.Concat(sql, " `author_id` = @authorIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@authorIdWhere", emailMessageWhere.AuthorId);
                    }
                    if (!string.IsNullOrEmpty(emailMessageWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", emailMessageWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(emailMessageWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", emailMessageWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(emailMessageWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", emailMessageWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(emailMessage);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageModel>();
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

        public List<EmailMessageModel> Update(string connectionString, List<EmailMessageModel> emailMessageList, List<EmailMessageModel> emailMessageWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageModel> response = new List<EmailMessageModel>();
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
                        for (int i = 0; i < emailMessageList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE cm_email_messages SET ";
                                if (!string.IsNullOrEmpty(emailMessageList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, emailMessageList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].Subject))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `subject` = @subject" + i);
                                    else
                                        sql = string.Concat(sql, " `subject` = @subject" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subject" + i, emailMessageList[i].Subject);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].Body))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `body` = @body" + i);
                                    else
                                        sql = string.Concat(sql, " `body` = @body" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@body" + i, emailMessageList[i].Body);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].DateCreated))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `date_created` = @dateCreated" + i);
                                    else
                                        sql = string.Concat(sql, " `date_created` = @dateCreated" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dateCreated" + i, emailMessageList[i].DateCreated);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].ParentMessageId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `parent_message_id` = @parentMessageId" + i);
                                    else
                                        sql = string.Concat(sql, " `parent_message_id` = @parentMessageId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@parentMessageId" + i, emailMessageList[i].ParentMessageId);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].AuthorId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `author_id` = @authorId" + i);
                                    else
                                        sql = string.Concat(sql, " `author_id` = @authorId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@authorId" + i, emailMessageList[i].AuthorId);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(emailMessageList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageList[i].UpdatedAt);
                                }
                                if (emailMessageWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(emailMessageWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, emailMessageWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageWhereList[i].Subject))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subject` = @subjectWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `subject` = @subjectWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectWhere" + i, emailMessageWhereList[i].Subject);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageWhereList[i].Body))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `body` = @bodyWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `body` = @bodyWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@bodyWhere" + i, emailMessageWhereList[i].Body);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageWhereList[i].DateCreated))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `date_created` = @dateCreatedWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `date_created` = @dateCreatedWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dateCreatedWhere" + i, emailMessageWhereList[i].DateCreated);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageWhereList[i].ParentMessageId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `parent_message_id` = @parentMessageIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `parent_message_id` = @parentMessageIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@parentMessageIdWhere" + i, emailMessageWhereList[i].ParentMessageId);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageWhereList[i].AuthorId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `author_id` = @authorIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `author_id` = @authorIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@authorIdWhere" + i, emailMessageWhereList[i].AuthorId);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, emailMessageWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, emailMessageWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessageWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, emailMessageWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailMessageList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailMessageModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailMessageModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageModel>();
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

        public List<EmailMessageModel> Delete(string connectionString, EmailMessageModel emailMessage)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageModel> response = new List<EmailMessageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM cm_email_messages  ";
                if (emailMessage != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(emailMessage.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", emailMessage.Id);
                    }
                    if (!string.IsNullOrEmpty(emailMessage.Subject))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subject` = @subject");
                        else
                            sql = string.Concat(sql, " `subject` = @subject");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subject", emailMessage.Subject);
                    }
                    if (!string.IsNullOrEmpty(emailMessage.Body))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `body` = @body");
                        else
                            sql = string.Concat(sql, " `body` = @body");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@body", emailMessage.Body);
                    }
                    if (!string.IsNullOrEmpty(emailMessage.DateCreated))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `date_created` = @dateCreated");
                        else
                            sql = string.Concat(sql, " `date_created` = @dateCreated");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dateCreated", emailMessage.DateCreated);
                    }
                    if (!string.IsNullOrEmpty(emailMessage.ParentMessageId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `parent_message_id` = @parentMessageId");
                        else
                            sql = string.Concat(sql, " `parent_message_id` = @parentMessageId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@parentMessageId", emailMessage.ParentMessageId);
                    }
                    if (!string.IsNullOrEmpty(emailMessage.AuthorId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `author_id` = @authorId");
                        else
                            sql = string.Concat(sql, " `author_id` = @authorId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@authorId", emailMessage.AuthorId);
                    }
                    if (!string.IsNullOrEmpty(emailMessage.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", emailMessage.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(emailMessage.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", emailMessage.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(emailMessage.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", emailMessage.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(emailMessage);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageModel>();
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

        public List<EmailMessageModel> Delete(string connectionString, List<EmailMessageModel> emailMessage)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailMessageModel> response = new List<EmailMessageModel>();
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
                        for (int i = 0; i < emailMessage.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM cm_email_messages  ";
                                if (emailMessage[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(emailMessage[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, emailMessage[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessage[i].Subject))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subject` = @subject" + i);
                                        else
                                            sql = string.Concat(sql, " `subject` = @subject" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subject" + i, emailMessage[i].Subject);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessage[i].Body))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `body` = @body" + i);
                                        else
                                            sql = string.Concat(sql, " `body` = @body" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@body" + i, emailMessage[i].Body);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessage[i].DateCreated))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `date_created` = @dateCreated" + i);
                                        else
                                            sql = string.Concat(sql, " `date_created` = @dateCreated" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dateCreated" + i, emailMessage[i].DateCreated);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessage[i].ParentMessageId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `parent_message_id` = @parentMessageId" + i);
                                        else
                                            sql = string.Concat(sql, " `parent_message_id` = @parentMessageId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@parentMessageId" + i, emailMessage[i].ParentMessageId);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessage[i].AuthorId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `author_id` = @authorId" + i);
                                        else
                                            sql = string.Concat(sql, " `author_id` = @authorId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@authorId" + i, emailMessage[i].AuthorId);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessage[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessage[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessage[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, emailMessage[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(emailMessage[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessage[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailMessage[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailMessageModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailMessageModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailMessageModel>();
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
