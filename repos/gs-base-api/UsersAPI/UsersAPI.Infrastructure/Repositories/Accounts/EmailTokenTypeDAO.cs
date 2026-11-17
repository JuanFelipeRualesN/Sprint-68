using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts
{
    class EmailTokenTypeDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, EmailTokenTypeModel> ReadD(string connectionString, EmailTokenTypeModel emailTokenType, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailTokenTypeModel> response = new Dictionary<string, EmailTokenTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `code`, `desc`, `duration`, `modified_by`, `created_at`, `updated_at`  FROM ul_email_token_types ";
                {
                    if (emailTokenType != null)
                    {
                        if (!String.IsNullOrEmpty(emailTokenType.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailTokenType.Id);
                        }
                        if (!String.IsNullOrEmpty(emailTokenType.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", emailTokenType.Code);
                        }
                        if (!String.IsNullOrEmpty(emailTokenType.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", emailTokenType.Desc);
                        }
                        if (!String.IsNullOrEmpty(emailTokenType.Duration))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `duration` = @duration ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `duration` = @duration ");
                            }
                            cmd.Parameters.AddWithValue("@duration", emailTokenType.Duration);
                        }
                        if (!String.IsNullOrEmpty(emailTokenType.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailTokenType.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailTokenType.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailTokenType.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailTokenType.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailTokenType.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = String.Concat(sql, " ORDER BY updated_at DESC");
                        if (firstOne)
                            sql = String.Concat(sql, " LIMIT 1 ");
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    emailTokenType.Id = reader.GetValue(0).ToString();
                                else
                                    emailTokenType.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailTokenType.Code = reader.GetValue(1).ToString();
                                else
                                    emailTokenType.Code = null;
                                if (!reader.IsDBNull(2))
                                    emailTokenType.Desc = reader.GetValue(2).ToString();
                                else
                                    emailTokenType.Desc = null;
                                if (!reader.IsDBNull(3))
                                    emailTokenType.Duration = reader.GetValue(3).ToString();
                                else
                                    emailTokenType.Duration = null;
                                if (!reader.IsDBNull(4))
                                    emailTokenType.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    emailTokenType.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    emailTokenType.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    emailTokenType.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    emailTokenType.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    emailTokenType.UpdatedAt = null;
                                response.Add(emailTokenType.Id, emailTokenType);
                                emailTokenType = new EmailTokenTypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EmailTokenTypeModel>();
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
                response = new Dictionary<string, EmailTokenTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EmailTokenTypeModel> ReadD(string connectionString, List<EmailTokenTypeModel> emailTokenTypeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailTokenTypeModel> response = new Dictionary<string, EmailTokenTypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `code`, `desc`, `duration`, `modified_by`, `created_at`, `updated_at`  FROM ul_email_token_types ";
                if (emailTokenTypeWhere != null && emailTokenTypeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailTokenTypeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(emailTokenTypeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailTokenTypeWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(emailTokenTypeWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, emailTokenTypeWhere[i].Code);
                        }
                        if (!String.IsNullOrEmpty(emailTokenTypeWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, emailTokenTypeWhere[i].Desc);
                        }
                        if (!String.IsNullOrEmpty(emailTokenTypeWhere[i].Duration))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `duration` = @duration" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@duration" + i, emailTokenTypeWhere[i].Duration);
                        }
                        if (!String.IsNullOrEmpty(emailTokenTypeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailTokenTypeWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailTokenTypeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailTokenTypeWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailTokenTypeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailTokenTypeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailTokenTypeWhere.Count - 1)
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
                            EmailTokenTypeModel emailTokenType = new EmailTokenTypeModel(); if (!reader.IsDBNull(0))
                                emailTokenType.Id = reader.GetValue(0).ToString();
                            else
                                emailTokenType.Id = null;
                            if (!reader.IsDBNull(1))
                                emailTokenType.Code = reader.GetValue(1).ToString();
                            else
                                emailTokenType.Code = null;
                            if (!reader.IsDBNull(2))
                                emailTokenType.Desc = reader.GetValue(2).ToString();
                            else
                                emailTokenType.Desc = null;
                            if (!reader.IsDBNull(3))
                                emailTokenType.Duration = reader.GetValue(3).ToString();
                            else
                                emailTokenType.Duration = null;
                            if (!reader.IsDBNull(4))
                                emailTokenType.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                emailTokenType.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                emailTokenType.CreatedAt = reader.GetValue(5).ToString();
                            else
                                emailTokenType.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                emailTokenType.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                emailTokenType.UpdatedAt = null;
                            response.Add(emailTokenType.Id, emailTokenType);
                            emailTokenType = new EmailTokenTypeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EmailTokenTypeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EmailTokenTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailTokenTypeModel> Read(string connectionString, EmailTokenTypeModel emailTokenType, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailTokenTypeModel> response = new List<EmailTokenTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `code`, `desc`, `duration`, `modified_by`, `created_at`, `updated_at`  FROM ul_email_token_types ";
                {
                    if (emailTokenType != null)
                    {
                        if (!String.IsNullOrEmpty(emailTokenType.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailTokenType.Id);
                        }
                        if (!String.IsNullOrEmpty(emailTokenType.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", emailTokenType.Code);
                        }
                        if (!String.IsNullOrEmpty(emailTokenType.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", emailTokenType.Desc);
                        }
                        if (!String.IsNullOrEmpty(emailTokenType.Duration))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `duration` = @duration ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `duration` = @duration ");
                            }
                            cmd.Parameters.AddWithValue("@duration", emailTokenType.Duration);
                        }
                        if (!String.IsNullOrEmpty(emailTokenType.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailTokenType.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailTokenType.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailTokenType.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailTokenType.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailTokenType.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = String.Concat(sql, " ORDER BY updated_at DESC");
                        if (firstOne)
                            sql = String.Concat(sql, " LIMIT 1 ");
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    emailTokenType.Id = reader.GetValue(0).ToString();
                                else
                                    emailTokenType.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailTokenType.Code = reader.GetValue(1).ToString();
                                else
                                    emailTokenType.Code = null;
                                if (!reader.IsDBNull(2))
                                    emailTokenType.Desc = reader.GetValue(2).ToString();
                                else
                                    emailTokenType.Desc = null;
                                if (!reader.IsDBNull(3))
                                    emailTokenType.Duration = reader.GetValue(3).ToString();
                                else
                                    emailTokenType.Duration = null;
                                if (!reader.IsDBNull(4))
                                    emailTokenType.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    emailTokenType.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    emailTokenType.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    emailTokenType.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    emailTokenType.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    emailTokenType.UpdatedAt = null;
                                response.Add(emailTokenType);
                                emailTokenType = new EmailTokenTypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EmailTokenTypeModel>();
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
                response = new List<EmailTokenTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailTokenTypeModel> Read(string connectionString, List<EmailTokenTypeModel> emailTokenTypeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailTokenTypeModel> response = new List<EmailTokenTypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `code`, `desc`, `duration`, `modified_by`, `created_at`, `updated_at`  FROM ul_email_token_types ";
                if (emailTokenTypeWhere != null && emailTokenTypeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailTokenTypeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(emailTokenTypeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailTokenTypeWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(emailTokenTypeWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, emailTokenTypeWhere[i].Code);
                        }
                        if (!String.IsNullOrEmpty(emailTokenTypeWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, emailTokenTypeWhere[i].Desc);
                        }
                        if (!String.IsNullOrEmpty(emailTokenTypeWhere[i].Duration))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `duration` = @duration" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@duration" + i, emailTokenTypeWhere[i].Duration);
                        }
                        if (!String.IsNullOrEmpty(emailTokenTypeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailTokenTypeWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailTokenTypeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailTokenTypeWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailTokenTypeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailTokenTypeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailTokenTypeWhere.Count - 1)
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
                            EmailTokenTypeModel emailTokenType = new EmailTokenTypeModel(); if (!reader.IsDBNull(0))
                                emailTokenType.Id = reader.GetValue(0).ToString();
                            else
                                emailTokenType.Id = null;
                            if (!reader.IsDBNull(1))
                                emailTokenType.Code = reader.GetValue(1).ToString();
                            else
                                emailTokenType.Code = null;
                            if (!reader.IsDBNull(2))
                                emailTokenType.Desc = reader.GetValue(2).ToString();
                            else
                                emailTokenType.Desc = null;
                            if (!reader.IsDBNull(3))
                                emailTokenType.Duration = reader.GetValue(3).ToString();
                            else
                                emailTokenType.Duration = null;
                            if (!reader.IsDBNull(4))
                                emailTokenType.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                emailTokenType.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                emailTokenType.CreatedAt = reader.GetValue(5).ToString();
                            else
                                emailTokenType.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                emailTokenType.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                emailTokenType.UpdatedAt = null;
                            response.Add(emailTokenType);
                            emailTokenType = new EmailTokenTypeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EmailTokenTypeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailTokenTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailTokenTypeModel> Create(string connectionString, EmailTokenTypeModel emailTokenType)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailTokenTypeModel> response = new List<EmailTokenTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO ul_email_token_types (";
                string values = "(";
                if (!String.IsNullOrEmpty(emailTokenType.Id))
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
                    cmd.Parameters.AddWithValue("@id", emailTokenType.Id);
                }
                if (!String.IsNullOrEmpty(emailTokenType.Code))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `code` ");
                        values = String.Concat(values, ", @code");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `code` ");
                        values = String.Concat(values, " @code");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", emailTokenType.Code);
                }
                if (!String.IsNullOrEmpty(emailTokenType.Desc))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `desc` ");
                        values = String.Concat(values, ", @desc");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `desc` ");
                        values = String.Concat(values, " @desc");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", emailTokenType.Desc);
                }
                if (!String.IsNullOrEmpty(emailTokenType.Duration))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `duration` ");
                        values = String.Concat(values, ", @duration");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `duration` ");
                        values = String.Concat(values, " @duration");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@duration", emailTokenType.Duration);
                }
                if (!String.IsNullOrEmpty(emailTokenType.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", emailTokenType.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(emailTokenType.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", emailTokenType.CreatedAt);
                }
                if (!String.IsNullOrEmpty(emailTokenType.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", emailTokenType.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                emailTokenType.Id = cmd.LastInsertedId.ToString();
                response.Add(emailTokenType);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailTokenTypeModel>();
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
        public List<EmailTokenTypeModel> Create(string connectionString, List<EmailTokenTypeModel> emailTokenTypeList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailTokenTypeModel> response = new List<EmailTokenTypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < emailTokenTypeList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO ul_email_token_types (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(emailTokenTypeList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, emailTokenTypeList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(emailTokenTypeList[i].Code))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `code` ");
                                        values = String.Concat(values, ", @code" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `code` ");
                                        values = String.Concat(values, "@code" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, emailTokenTypeList[i].Code);
                                }
                                if (!String.IsNullOrEmpty(emailTokenTypeList[i].Desc))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `desc` ");
                                        values = String.Concat(values, ", @desc" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `desc` ");
                                        values = String.Concat(values, "@desc" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, emailTokenTypeList[i].Desc);
                                }
                                if (!String.IsNullOrEmpty(emailTokenTypeList[i].Duration))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `duration` ");
                                        values = String.Concat(values, ", @duration" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `duration` ");
                                        values = String.Concat(values, "@duration" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@duration" + i, emailTokenTypeList[i].Duration);
                                }
                                if (!String.IsNullOrEmpty(emailTokenTypeList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailTokenTypeList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(emailTokenTypeList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailTokenTypeList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(emailTokenTypeList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailTokenTypeList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(emailTokenTypeList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailTokenTypeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailTokenTypeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailTokenTypeModel>();
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

        public List<EmailTokenTypeModel> Update(string connectionString, EmailTokenTypeModel emailTokenType, EmailTokenTypeModel emailTokenTypeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailTokenTypeModel> response = new List<EmailTokenTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE ul_email_token_types SET ";
                if (!String.IsNullOrEmpty(emailTokenType.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", emailTokenType.Id);
                }
                if (!String.IsNullOrEmpty(emailTokenType.Code))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `code` = @code");
                    else
                        sql = String.Concat(sql, " `code` = @code");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", emailTokenType.Code);
                }
                if (!String.IsNullOrEmpty(emailTokenType.Desc))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `desc` = @desc");
                    else
                        sql = String.Concat(sql, " `desc` = @desc");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", emailTokenType.Desc);
                }
                if (!String.IsNullOrEmpty(emailTokenType.Duration))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `duration` = @duration");
                    else
                        sql = String.Concat(sql, " `duration` = @duration");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@duration", emailTokenType.Duration);
                }
                if (!String.IsNullOrEmpty(emailTokenType.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", emailTokenType.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(emailTokenType.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", emailTokenType.CreatedAt);
                }
                if (!String.IsNullOrEmpty(emailTokenType.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", emailTokenType.UpdatedAt);
                }
                if (emailTokenTypeWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(emailTokenTypeWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", emailTokenTypeWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(emailTokenTypeWhere.Code))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `code` = @codeWhere");
                        else
                            sql = String.Concat(sql, " `code` = @codeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@codeWhere", emailTokenTypeWhere.Code);
                    }
                    if (!String.IsNullOrEmpty(emailTokenTypeWhere.Desc))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `desc` = @descWhere");
                        else
                            sql = String.Concat(sql, " `desc` = @descWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descWhere", emailTokenTypeWhere.Desc);
                    }
                    if (!String.IsNullOrEmpty(emailTokenTypeWhere.Duration))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `duration` = @durationWhere");
                        else
                            sql = String.Concat(sql, " `duration` = @durationWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@durationWhere", emailTokenTypeWhere.Duration);
                    }
                    if (!String.IsNullOrEmpty(emailTokenTypeWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", emailTokenTypeWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(emailTokenTypeWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", emailTokenTypeWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(emailTokenTypeWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", emailTokenTypeWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(emailTokenType);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailTokenTypeModel>();
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

        public List<EmailTokenTypeModel> Update(string connectionString, List<EmailTokenTypeModel> emailTokenTypeList, List<EmailTokenTypeModel> emailTokenTypeWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailTokenTypeModel> response = new List<EmailTokenTypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < emailTokenTypeList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE ul_email_token_types SET ";
                                if (!String.IsNullOrEmpty(emailTokenTypeList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, emailTokenTypeList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(emailTokenTypeList[i].Code))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `code` = @code" + i);
                                    else
                                        sql = String.Concat(sql, " `code` = @code" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, emailTokenTypeList[i].Code);
                                }
                                if (!String.IsNullOrEmpty(emailTokenTypeList[i].Desc))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `desc` = @desc" + i);
                                    else
                                        sql = String.Concat(sql, " `desc` = @desc" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, emailTokenTypeList[i].Desc);
                                }
                                if (!String.IsNullOrEmpty(emailTokenTypeList[i].Duration))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `duration` = @duration" + i);
                                    else
                                        sql = String.Concat(sql, " `duration` = @duration" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@duration" + i, emailTokenTypeList[i].Duration);
                                }
                                if (!String.IsNullOrEmpty(emailTokenTypeList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailTokenTypeList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(emailTokenTypeList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailTokenTypeList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(emailTokenTypeList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailTokenTypeList[i].UpdatedAt);
                                }
                                if (emailTokenTypeWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(emailTokenTypeWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, emailTokenTypeWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(emailTokenTypeWhereList[i].Code))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `code` = @codeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `code` = @codeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@codeWhere" + i, emailTokenTypeWhereList[i].Code);
                                    }
                                    if (!String.IsNullOrEmpty(emailTokenTypeWhereList[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `desc` = @descWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `desc` = @descWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descWhere" + i, emailTokenTypeWhereList[i].Desc);
                                    }
                                    if (!String.IsNullOrEmpty(emailTokenTypeWhereList[i].Duration))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `duration` = @durationWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `duration` = @durationWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@durationWhere" + i, emailTokenTypeWhereList[i].Duration);
                                    }
                                    if (!String.IsNullOrEmpty(emailTokenTypeWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, emailTokenTypeWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(emailTokenTypeWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, emailTokenTypeWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(emailTokenTypeWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, emailTokenTypeWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailTokenTypeList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailTokenTypeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailTokenTypeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailTokenTypeModel>();
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

        public List<EmailTokenTypeModel> Delete(string connectionString, EmailTokenTypeModel emailTokenType)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailTokenTypeModel> response = new List<EmailTokenTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM ul_email_token_types  ";
                if (emailTokenType != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(emailTokenType.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", emailTokenType.Id);
                    }
                    if (!String.IsNullOrEmpty(emailTokenType.Code))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `code` = @code");
                        else
                            sql = String.Concat(sql, " `code` = @code");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@code", emailTokenType.Code);
                    }
                    if (!String.IsNullOrEmpty(emailTokenType.Desc))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `desc` = @desc");
                        else
                            sql = String.Concat(sql, " `desc` = @desc");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@desc", emailTokenType.Desc);
                    }
                    if (!String.IsNullOrEmpty(emailTokenType.Duration))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `duration` = @duration");
                        else
                            sql = String.Concat(sql, " `duration` = @duration");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@duration", emailTokenType.Duration);
                    }
                    if (!String.IsNullOrEmpty(emailTokenType.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", emailTokenType.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(emailTokenType.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", emailTokenType.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(emailTokenType.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", emailTokenType.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(emailTokenType);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailTokenTypeModel>();
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

        public List<EmailTokenTypeModel> Delete(string connectionString, List<EmailTokenTypeModel> emailTokenType)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailTokenTypeModel> response = new List<EmailTokenTypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < emailTokenType.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM ul_email_token_types  ";
                                if (emailTokenType[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(emailTokenType[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, emailTokenType[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(emailTokenType[i].Code))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `code` = @code" + i);
                                        else
                                            sql = String.Concat(sql, " `code` = @code" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@code" + i, emailTokenType[i].Code);
                                    }
                                    if (!String.IsNullOrEmpty(emailTokenType[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `desc` = @desc" + i);
                                        else
                                            sql = String.Concat(sql, " `desc` = @desc" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@desc" + i, emailTokenType[i].Desc);
                                    }
                                    if (!String.IsNullOrEmpty(emailTokenType[i].Duration))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `duration` = @duration" + i);
                                        else
                                            sql = String.Concat(sql, " `duration` = @duration" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@duration" + i, emailTokenType[i].Duration);
                                    }
                                    if (!String.IsNullOrEmpty(emailTokenType[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, emailTokenType[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(emailTokenType[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, emailTokenType[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(emailTokenType[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, emailTokenType[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailTokenType[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailTokenTypeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailTokenTypeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailTokenTypeModel>();
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
