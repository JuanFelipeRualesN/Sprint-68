using System;
using System.Collections.Generic;
using MySqlConnector;
using MessagesAPI.MessageAPI.Domain.Models.EmailSystem;
using MessagesAPI.MessageAPI.Services.EmailSystems.CustomClasses;

namespace MessagesAPI.MessageAPI.Infrastructure.Repositories.EmailSystems
{
    class AttachmentRuleDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, AttachmentRuleModel> ReadD(string connectionString, AttachmentRuleModel attachmentRule, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AttachmentRuleModel> response = new Dictionary<string, AttachmentRuleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `file_type`, `max_size`, `modified_by`, `created_at`, `updated_at`  FROM cm_attachment_rules ";
                {
                    if (attachmentRule != null)
                    {
                        if (!string.IsNullOrEmpty(attachmentRule.Id))
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
                            cmd.Parameters.AddWithValue("@id", attachmentRule.Id);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.FileType))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `file_type` = @fileType ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `file_type` = @fileType ");
                            }
                            cmd.Parameters.AddWithValue("@fileType", attachmentRule.FileType);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.MaxSize))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_size` = @maxSize ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_size` = @maxSize ");
                            }
                            cmd.Parameters.AddWithValue("@maxSize", attachmentRule.MaxSize);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", attachmentRule.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", attachmentRule.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", attachmentRule.UpdatedAt);
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
                                    attachmentRule.Id = reader.GetValue(0).ToString();
                                else
                                    attachmentRule.Id = null;
                                if (!reader.IsDBNull(1))
                                    attachmentRule.FileType = reader.GetValue(1).ToString();
                                else
                                    attachmentRule.FileType = null;
                                if (!reader.IsDBNull(2))
                                    attachmentRule.MaxSize = reader.GetValue(2).ToString();
                                else
                                    attachmentRule.MaxSize = null;
                                if (!reader.IsDBNull(3))
                                    attachmentRule.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    attachmentRule.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    attachmentRule.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    attachmentRule.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    attachmentRule.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    attachmentRule.UpdatedAt = null;
                                response.Add(attachmentRule.Id, attachmentRule);
                                attachmentRule = new AttachmentRuleModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, AttachmentRuleModel>();
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
                response = new Dictionary<string, AttachmentRuleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, AttachmentRuleModel> ReadD(string connectionString, AttachmentRuleModel attachmentRule, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AttachmentRuleModel> response = new Dictionary<string, AttachmentRuleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `file_type`, `max_size`, `modified_by`, `created_at`, `updated_at`  FROM cm_attachment_rules ";
                {
                    if (attachmentRule != null)
                    {
                        if (!string.IsNullOrEmpty(attachmentRule.Id))
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
                            cmd.Parameters.AddWithValue("@id", attachmentRule.Id);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.FileType))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `file_type` = @fileType ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `file_type` = @fileType ");
                            }
                            cmd.Parameters.AddWithValue("@fileType", attachmentRule.FileType);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.MaxSize))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_size` = @maxSize ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_size` = @maxSize ");
                            }
                            cmd.Parameters.AddWithValue("@maxSize", attachmentRule.MaxSize);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", attachmentRule.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", attachmentRule.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", attachmentRule.UpdatedAt);
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
                                    attachmentRule.Id = reader.GetValue(0).ToString();
                                else
                                    attachmentRule.Id = null;
                                if (!reader.IsDBNull(1))
                                    attachmentRule.FileType = reader.GetValue(1).ToString();
                                else
                                    attachmentRule.FileType = null;
                                if (!reader.IsDBNull(2))
                                    attachmentRule.MaxSize = reader.GetValue(2).ToString();
                                else
                                    attachmentRule.MaxSize = null;
                                if (!reader.IsDBNull(3))
                                    attachmentRule.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    attachmentRule.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    attachmentRule.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    attachmentRule.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    attachmentRule.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    attachmentRule.UpdatedAt = null;
                                response.Add(attachmentRule.Id, attachmentRule);
                                attachmentRule = new AttachmentRuleModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, AttachmentRuleModel>();
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
                response = new Dictionary<string, AttachmentRuleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, AttachmentRuleModel> ReadD(string connectionString, List<AttachmentRuleModel> attachmentRuleWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AttachmentRuleModel> response = new Dictionary<string, AttachmentRuleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `file_type`, `max_size`, `modified_by`, `created_at`, `updated_at`  FROM cm_attachment_rules ";
                if (attachmentRuleWhere != null && attachmentRuleWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < attachmentRuleWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, attachmentRuleWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].FileType))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `file_type` = @fileType" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fileType" + i, attachmentRuleWhere[i].FileType);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].MaxSize))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_size` = @maxSize" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxSize" + i, attachmentRuleWhere[i].MaxSize);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, attachmentRuleWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, attachmentRuleWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, attachmentRuleWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != attachmentRuleWhere.Count - 1)
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
                            AttachmentRuleModel attachmentRule = new AttachmentRuleModel(); if (!reader.IsDBNull(0))
                                attachmentRule.Id = reader.GetValue(0).ToString();
                            else
                                attachmentRule.Id = null;
                            if (!reader.IsDBNull(1))
                                attachmentRule.FileType = reader.GetValue(1).ToString();
                            else
                                attachmentRule.FileType = null;
                            if (!reader.IsDBNull(2))
                                attachmentRule.MaxSize = reader.GetValue(2).ToString();
                            else
                                attachmentRule.MaxSize = null;
                            if (!reader.IsDBNull(3))
                                attachmentRule.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                attachmentRule.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                attachmentRule.CreatedAt = reader.GetValue(4).ToString();
                            else
                                attachmentRule.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                attachmentRule.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                attachmentRule.UpdatedAt = null;
                            response.Add(attachmentRule.Id, attachmentRule);
                            attachmentRule = new AttachmentRuleModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, AttachmentRuleModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, AttachmentRuleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, AttachmentRuleModel> ReadD(string connectionString, List<AttachmentRuleModel> attachmentRuleWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AttachmentRuleModel> response = new Dictionary<string, AttachmentRuleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `file_type`, `max_size`, `modified_by`, `created_at`, `updated_at`  FROM cm_attachment_rules ";
                if (attachmentRuleWhere != null && attachmentRuleWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < attachmentRuleWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, attachmentRuleWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].FileType))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `file_type` = @fileType" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fileType" + i, attachmentRuleWhere[i].FileType);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].MaxSize))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_size` = @maxSize" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxSize" + i, attachmentRuleWhere[i].MaxSize);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, attachmentRuleWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, attachmentRuleWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, attachmentRuleWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != attachmentRuleWhere.Count - 1)
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
                            AttachmentRuleModel attachmentRule = new AttachmentRuleModel(); if (!reader.IsDBNull(0))
                                attachmentRule.Id = reader.GetValue(0).ToString();
                            else
                                attachmentRule.Id = null;
                            if (!reader.IsDBNull(1))
                                attachmentRule.FileType = reader.GetValue(1).ToString();
                            else
                                attachmentRule.FileType = null;
                            if (!reader.IsDBNull(2))
                                attachmentRule.MaxSize = reader.GetValue(2).ToString();
                            else
                                attachmentRule.MaxSize = null;
                            if (!reader.IsDBNull(3))
                                attachmentRule.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                attachmentRule.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                attachmentRule.CreatedAt = reader.GetValue(4).ToString();
                            else
                                attachmentRule.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                attachmentRule.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                attachmentRule.UpdatedAt = null;
                            response.Add(attachmentRule.Id, attachmentRule);
                            attachmentRule = new AttachmentRuleModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, AttachmentRuleModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, AttachmentRuleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AttachmentRuleModel> Read(string connectionString, AttachmentRuleModel attachmentRule, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AttachmentRuleModel> response = new List<AttachmentRuleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `file_type`, `max_size`, `modified_by`, `created_at`, `updated_at`  FROM cm_attachment_rules ";
                {
                    if (attachmentRule != null)
                    {
                        if (!string.IsNullOrEmpty(attachmentRule.Id))
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
                            cmd.Parameters.AddWithValue("@id", attachmentRule.Id);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.FileType))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `file_type` = @fileType ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `file_type` = @fileType ");
                            }
                            cmd.Parameters.AddWithValue("@fileType", attachmentRule.FileType);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.MaxSize))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_size` = @maxSize ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_size` = @maxSize ");
                            }
                            cmd.Parameters.AddWithValue("@maxSize", attachmentRule.MaxSize);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", attachmentRule.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", attachmentRule.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", attachmentRule.UpdatedAt);
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
                                    attachmentRule.Id = reader.GetValue(0).ToString();
                                else
                                    attachmentRule.Id = null;
                                if (!reader.IsDBNull(1))
                                    attachmentRule.FileType = reader.GetValue(1).ToString();
                                else
                                    attachmentRule.FileType = null;
                                if (!reader.IsDBNull(2))
                                    attachmentRule.MaxSize = reader.GetValue(2).ToString();
                                else
                                    attachmentRule.MaxSize = null;
                                if (!reader.IsDBNull(3))
                                    attachmentRule.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    attachmentRule.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    attachmentRule.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    attachmentRule.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    attachmentRule.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    attachmentRule.UpdatedAt = null;
                                response.Add(attachmentRule);
                                attachmentRule = new AttachmentRuleModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<AttachmentRuleModel>();
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
                response = new List<AttachmentRuleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AttachmentRuleModel> Read(string connectionString, AttachmentRuleModel attachmentRule, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AttachmentRuleModel> response = new List<AttachmentRuleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `file_type`, `max_size`, `modified_by`, `created_at`, `updated_at`  FROM cm_attachment_rules ";
                {
                    if (attachmentRule != null)
                    {
                        if (!string.IsNullOrEmpty(attachmentRule.Id))
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
                            cmd.Parameters.AddWithValue("@id", attachmentRule.Id);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.FileType))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `file_type` = @fileType ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `file_type` = @fileType ");
                            }
                            cmd.Parameters.AddWithValue("@fileType", attachmentRule.FileType);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.MaxSize))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_size` = @maxSize ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_size` = @maxSize ");
                            }
                            cmd.Parameters.AddWithValue("@maxSize", attachmentRule.MaxSize);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", attachmentRule.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", attachmentRule.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(attachmentRule.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", attachmentRule.UpdatedAt);
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
                                    attachmentRule.Id = reader.GetValue(0).ToString();
                                else
                                    attachmentRule.Id = null;
                                if (!reader.IsDBNull(1))
                                    attachmentRule.FileType = reader.GetValue(1).ToString();
                                else
                                    attachmentRule.FileType = null;
                                if (!reader.IsDBNull(2))
                                    attachmentRule.MaxSize = reader.GetValue(2).ToString();
                                else
                                    attachmentRule.MaxSize = null;
                                if (!reader.IsDBNull(3))
                                    attachmentRule.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    attachmentRule.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    attachmentRule.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    attachmentRule.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    attachmentRule.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    attachmentRule.UpdatedAt = null;
                                response.Add(attachmentRule);
                                attachmentRule = new AttachmentRuleModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<AttachmentRuleModel>();
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
                response = new List<AttachmentRuleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AttachmentRuleModel> Read(string connectionString, List<AttachmentRuleModel> attachmentRuleWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AttachmentRuleModel> response = new List<AttachmentRuleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `file_type`, `max_size`, `modified_by`, `created_at`, `updated_at`  FROM cm_attachment_rules ";
                if (attachmentRuleWhere != null && attachmentRuleWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < attachmentRuleWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, attachmentRuleWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].FileType))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `file_type` = @fileType" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fileType" + i, attachmentRuleWhere[i].FileType);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].MaxSize))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_size` = @maxSize" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxSize" + i, attachmentRuleWhere[i].MaxSize);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, attachmentRuleWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, attachmentRuleWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, attachmentRuleWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != attachmentRuleWhere.Count - 1)
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
                            AttachmentRuleModel attachmentRule = new AttachmentRuleModel(); if (!reader.IsDBNull(0))
                                attachmentRule.Id = reader.GetValue(0).ToString();
                            else
                                attachmentRule.Id = null;
                            if (!reader.IsDBNull(1))
                                attachmentRule.FileType = reader.GetValue(1).ToString();
                            else
                                attachmentRule.FileType = null;
                            if (!reader.IsDBNull(2))
                                attachmentRule.MaxSize = reader.GetValue(2).ToString();
                            else
                                attachmentRule.MaxSize = null;
                            if (!reader.IsDBNull(3))
                                attachmentRule.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                attachmentRule.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                attachmentRule.CreatedAt = reader.GetValue(4).ToString();
                            else
                                attachmentRule.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                attachmentRule.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                attachmentRule.UpdatedAt = null;
                            response.Add(attachmentRule);
                            attachmentRule = new AttachmentRuleModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<AttachmentRuleModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AttachmentRuleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AttachmentRuleModel> Read(string connectionString, List<AttachmentRuleModel> attachmentRuleWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AttachmentRuleModel> response = new List<AttachmentRuleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `file_type`, `max_size`, `modified_by`, `created_at`, `updated_at`  FROM cm_attachment_rules ";
                if (attachmentRuleWhere != null && attachmentRuleWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < attachmentRuleWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, attachmentRuleWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].FileType))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `file_type` = @fileType" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fileType" + i, attachmentRuleWhere[i].FileType);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].MaxSize))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_size` = @maxSize" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxSize" + i, attachmentRuleWhere[i].MaxSize);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, attachmentRuleWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, attachmentRuleWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(attachmentRuleWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, attachmentRuleWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != attachmentRuleWhere.Count - 1)
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
                            AttachmentRuleModel attachmentRule = new AttachmentRuleModel(); if (!reader.IsDBNull(0))
                                attachmentRule.Id = reader.GetValue(0).ToString();
                            else
                                attachmentRule.Id = null;
                            if (!reader.IsDBNull(1))
                                attachmentRule.FileType = reader.GetValue(1).ToString();
                            else
                                attachmentRule.FileType = null;
                            if (!reader.IsDBNull(2))
                                attachmentRule.MaxSize = reader.GetValue(2).ToString();
                            else
                                attachmentRule.MaxSize = null;
                            if (!reader.IsDBNull(3))
                                attachmentRule.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                attachmentRule.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                attachmentRule.CreatedAt = reader.GetValue(4).ToString();
                            else
                                attachmentRule.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                attachmentRule.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                attachmentRule.UpdatedAt = null;
                            response.Add(attachmentRule);
                            attachmentRule = new AttachmentRuleModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<AttachmentRuleModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AttachmentRuleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AttachmentRuleModel> Create(string connectionString, AttachmentRuleModel attachmentRule)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AttachmentRuleModel> response = new List<AttachmentRuleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO cm_attachment_rules (";
                string values = "(";
                if (!string.IsNullOrEmpty(attachmentRule.Id))
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
                    cmd.Parameters.AddWithValue("@id", attachmentRule.Id);
                }
                if (!string.IsNullOrEmpty(attachmentRule.FileType))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `file_type` ");
                        values = string.Concat(values, ", @fileType");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `file_type` ");
                        values = string.Concat(values, " @fileType");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@fileType", attachmentRule.FileType);
                }
                if (!string.IsNullOrEmpty(attachmentRule.MaxSize))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `max_size` ");
                        values = string.Concat(values, ", @maxSize");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `max_size` ");
                        values = string.Concat(values, " @maxSize");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@maxSize", attachmentRule.MaxSize);
                }
                if (!string.IsNullOrEmpty(attachmentRule.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", attachmentRule.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(attachmentRule.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", attachmentRule.CreatedAt);
                }
                if (!string.IsNullOrEmpty(attachmentRule.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", attachmentRule.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                attachmentRule.Id = cmd.LastInsertedId.ToString();
                response.Add(attachmentRule);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AttachmentRuleModel>();
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
        public List<AttachmentRuleModel> Create(string connectionString, List<AttachmentRuleModel> attachmentRuleList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AttachmentRuleModel> response = new List<AttachmentRuleModel>();
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
                        for (int i = 0; i < attachmentRuleList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO cm_attachment_rules (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(attachmentRuleList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, attachmentRuleList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(attachmentRuleList[i].FileType))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `file_type` ");
                                        values = string.Concat(values, ", @fileType" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `file_type` ");
                                        values = string.Concat(values, "@fileType" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@fileType" + i, attachmentRuleList[i].FileType);
                                }
                                if (!string.IsNullOrEmpty(attachmentRuleList[i].MaxSize))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `max_size` ");
                                        values = string.Concat(values, ", @maxSize" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `max_size` ");
                                        values = string.Concat(values, "@maxSize" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@maxSize" + i, attachmentRuleList[i].MaxSize);
                                }
                                if (!string.IsNullOrEmpty(attachmentRuleList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, attachmentRuleList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(attachmentRuleList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, attachmentRuleList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(attachmentRuleList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, attachmentRuleList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(attachmentRuleList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<AttachmentRuleModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AttachmentRuleModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AttachmentRuleModel>();
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

        public List<AttachmentRuleModel> Update(string connectionString, AttachmentRuleModel attachmentRule, AttachmentRuleModel attachmentRuleWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AttachmentRuleModel> response = new List<AttachmentRuleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE cm_attachment_rules SET ";
                if (!string.IsNullOrEmpty(attachmentRule.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", attachmentRule.Id);
                }
                if (!string.IsNullOrEmpty(attachmentRule.FileType))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `file_type` = @fileType");
                    else
                        sql = string.Concat(sql, " `file_type` = @fileType");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@fileType", attachmentRule.FileType);
                }
                if (!string.IsNullOrEmpty(attachmentRule.MaxSize))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `max_size` = @maxSize");
                    else
                        sql = string.Concat(sql, " `max_size` = @maxSize");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@maxSize", attachmentRule.MaxSize);
                }
                if (!string.IsNullOrEmpty(attachmentRule.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", attachmentRule.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(attachmentRule.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", attachmentRule.CreatedAt);
                }
                if (!string.IsNullOrEmpty(attachmentRule.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", attachmentRule.UpdatedAt);
                }
                if (attachmentRuleWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(attachmentRuleWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", attachmentRuleWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(attachmentRuleWhere.FileType))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `file_type` = @fileTypeWhere");
                        else
                            sql = string.Concat(sql, " `file_type` = @fileTypeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@fileTypeWhere", attachmentRuleWhere.FileType);
                    }
                    if (!string.IsNullOrEmpty(attachmentRuleWhere.MaxSize))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `max_size` = @maxSizeWhere");
                        else
                            sql = string.Concat(sql, " `max_size` = @maxSizeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxSizeWhere", attachmentRuleWhere.MaxSize);
                    }
                    if (!string.IsNullOrEmpty(attachmentRuleWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", attachmentRuleWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(attachmentRuleWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", attachmentRuleWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(attachmentRuleWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", attachmentRuleWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(attachmentRule);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<AttachmentRuleModel>();
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

        public List<AttachmentRuleModel> Update(string connectionString, List<AttachmentRuleModel> attachmentRuleList, List<AttachmentRuleModel> attachmentRuleWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AttachmentRuleModel> response = new List<AttachmentRuleModel>();
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
                        for (int i = 0; i < attachmentRuleList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE cm_attachment_rules SET ";
                                if (!string.IsNullOrEmpty(attachmentRuleList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, attachmentRuleList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(attachmentRuleList[i].FileType))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `file_type` = @fileType" + i);
                                    else
                                        sql = string.Concat(sql, " `file_type` = @fileType" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@fileType" + i, attachmentRuleList[i].FileType);
                                }
                                if (!string.IsNullOrEmpty(attachmentRuleList[i].MaxSize))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `max_size` = @maxSize" + i);
                                    else
                                        sql = string.Concat(sql, " `max_size` = @maxSize" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@maxSize" + i, attachmentRuleList[i].MaxSize);
                                }
                                if (!string.IsNullOrEmpty(attachmentRuleList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, attachmentRuleList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(attachmentRuleList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, attachmentRuleList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(attachmentRuleList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, attachmentRuleList[i].UpdatedAt);
                                }
                                if (attachmentRuleWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(attachmentRuleWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, attachmentRuleWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(attachmentRuleWhereList[i].FileType))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `file_type` = @fileTypeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `file_type` = @fileTypeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@fileTypeWhere" + i, attachmentRuleWhereList[i].FileType);
                                    }
                                    if (!string.IsNullOrEmpty(attachmentRuleWhereList[i].MaxSize))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `max_size` = @maxSizeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `max_size` = @maxSizeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxSizeWhere" + i, attachmentRuleWhereList[i].MaxSize);
                                    }
                                    if (!string.IsNullOrEmpty(attachmentRuleWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, attachmentRuleWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(attachmentRuleWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, attachmentRuleWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(attachmentRuleWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, attachmentRuleWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(attachmentRuleList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<AttachmentRuleModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AttachmentRuleModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AttachmentRuleModel>();
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

        public List<AttachmentRuleModel> Delete(string connectionString, AttachmentRuleModel attachmentRule)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AttachmentRuleModel> response = new List<AttachmentRuleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM cm_attachment_rules  ";
                if (attachmentRule != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(attachmentRule.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", attachmentRule.Id);
                    }
                    if (!string.IsNullOrEmpty(attachmentRule.FileType))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `file_type` = @fileType");
                        else
                            sql = string.Concat(sql, " `file_type` = @fileType");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@fileType", attachmentRule.FileType);
                    }
                    if (!string.IsNullOrEmpty(attachmentRule.MaxSize))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `max_size` = @maxSize");
                        else
                            sql = string.Concat(sql, " `max_size` = @maxSize");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxSize", attachmentRule.MaxSize);
                    }
                    if (!string.IsNullOrEmpty(attachmentRule.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", attachmentRule.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(attachmentRule.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", attachmentRule.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(attachmentRule.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", attachmentRule.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(attachmentRule);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<AttachmentRuleModel>();
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

        public List<AttachmentRuleModel> Delete(string connectionString, List<AttachmentRuleModel> attachmentRule)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AttachmentRuleModel> response = new List<AttachmentRuleModel>();
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
                        for (int i = 0; i < attachmentRule.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM cm_attachment_rules  ";
                                if (attachmentRule[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(attachmentRule[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, attachmentRule[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(attachmentRule[i].FileType))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `file_type` = @fileType" + i);
                                        else
                                            sql = string.Concat(sql, " `file_type` = @fileType" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@fileType" + i, attachmentRule[i].FileType);
                                    }
                                    if (!string.IsNullOrEmpty(attachmentRule[i].MaxSize))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `max_size` = @maxSize" + i);
                                        else
                                            sql = string.Concat(sql, " `max_size` = @maxSize" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxSize" + i, attachmentRule[i].MaxSize);
                                    }
                                    if (!string.IsNullOrEmpty(attachmentRule[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, attachmentRule[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(attachmentRule[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, attachmentRule[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(attachmentRule[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, attachmentRule[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(attachmentRule[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<AttachmentRuleModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AttachmentRuleModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AttachmentRuleModel>();
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
