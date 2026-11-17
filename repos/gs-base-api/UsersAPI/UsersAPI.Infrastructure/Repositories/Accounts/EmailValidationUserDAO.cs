using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts
{
    class EmailValidationUserDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, EmailValidationUserModel> ReadD(string connectionString, EmailValidationUserModel emailValidationUser, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailValidationUserModel> response = new Dictionary<string, EmailValidationUserModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `email`, `user_id`, `email_type_id`, `token`, `used`, `modified_by`, `created_at`, `updated_at`  FROM ul_email_validation_users ";
                {
                    if (emailValidationUser != null)
                    {
                        if (!String.IsNullOrEmpty(emailValidationUser.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailValidationUser.Id);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.Email))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `email` = @email ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `email` = @email ");
                            }
                            cmd.Parameters.AddWithValue("@email", emailValidationUser.Email);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", emailValidationUser.UserId);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.EmailTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `email_type_id` = @emailTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `email_type_id` = @emailTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@emailTypeId", emailValidationUser.EmailTypeId);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.Token))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `token` = @token ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `token` = @token ");
                            }
                            cmd.Parameters.AddWithValue("@token", emailValidationUser.Token);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.Used))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `used` = @used ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `used` = @used ");
                            }
                            cmd.Parameters.AddWithValue("@used", emailValidationUser.Used);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailValidationUser.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailValidationUser.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailValidationUser.UpdatedAt);
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
                                    emailValidationUser.Id = reader.GetValue(0).ToString();
                                else
                                    emailValidationUser.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailValidationUser.Email = reader.GetValue(1).ToString();
                                else
                                    emailValidationUser.Email = null;
                                if (!reader.IsDBNull(2))
                                    emailValidationUser.UserId = reader.GetValue(2).ToString();
                                else
                                    emailValidationUser.UserId = null;
                                if (!reader.IsDBNull(3))
                                    emailValidationUser.EmailTypeId = reader.GetValue(3).ToString();
                                else
                                    emailValidationUser.EmailTypeId = null;
                                if (!reader.IsDBNull(4))
                                    emailValidationUser.Token = reader.GetValue(4).ToString();
                                else
                                    emailValidationUser.Token = null;
                                if (!reader.IsDBNull(5))
                                    emailValidationUser.Used = reader.GetValue(5).ToString();
                                else
                                    emailValidationUser.Used = null;
                                if (!reader.IsDBNull(6))
                                    emailValidationUser.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    emailValidationUser.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    emailValidationUser.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    emailValidationUser.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    emailValidationUser.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    emailValidationUser.UpdatedAt = null;
                                response.Add(emailValidationUser.Id, emailValidationUser);
                                emailValidationUser = new EmailValidationUserModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EmailValidationUserModel>();
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
                response = new Dictionary<string, EmailValidationUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EmailValidationUserModel> ReadD(string connectionString, List<EmailValidationUserModel> emailValidationUserWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailValidationUserModel> response = new Dictionary<string, EmailValidationUserModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `email`, `user_id`, `email_type_id`, `token`, `used`, `modified_by`, `created_at`, `updated_at`  FROM ul_email_validation_users ";
                if (emailValidationUserWhere != null && emailValidationUserWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailValidationUserWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailValidationUserWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].Email))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email` = @email" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@email" + i, emailValidationUserWhere[i].Email);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, emailValidationUserWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].EmailTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email_type_id` = @emailTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@emailTypeId" + i, emailValidationUserWhere[i].EmailTypeId);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].Token))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `token` = @token" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@token" + i, emailValidationUserWhere[i].Token);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].Used))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `used` = @used" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@used" + i, emailValidationUserWhere[i].Used);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailValidationUserWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailValidationUserWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailValidationUserWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailValidationUserWhere.Count - 1)
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
                            EmailValidationUserModel emailValidationUser = new EmailValidationUserModel(); if (!reader.IsDBNull(0))
                                emailValidationUser.Id = reader.GetValue(0).ToString();
                            else
                                emailValidationUser.Id = null;
                            if (!reader.IsDBNull(1))
                                emailValidationUser.Email = reader.GetValue(1).ToString();
                            else
                                emailValidationUser.Email = null;
                            if (!reader.IsDBNull(2))
                                emailValidationUser.UserId = reader.GetValue(2).ToString();
                            else
                                emailValidationUser.UserId = null;
                            if (!reader.IsDBNull(3))
                                emailValidationUser.EmailTypeId = reader.GetValue(3).ToString();
                            else
                                emailValidationUser.EmailTypeId = null;
                            if (!reader.IsDBNull(4))
                                emailValidationUser.Token = reader.GetValue(4).ToString();
                            else
                                emailValidationUser.Token = null;
                            if (!reader.IsDBNull(5))
                                emailValidationUser.Used = reader.GetValue(5).ToString();
                            else
                                emailValidationUser.Used = null;
                            if (!reader.IsDBNull(6))
                                emailValidationUser.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                emailValidationUser.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                emailValidationUser.CreatedAt = reader.GetValue(7).ToString();
                            else
                                emailValidationUser.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                emailValidationUser.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                emailValidationUser.UpdatedAt = null;
                            response.Add(emailValidationUser.Id, emailValidationUser);
                            emailValidationUser = new EmailValidationUserModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EmailValidationUserModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EmailValidationUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailValidationUserModel> Read(string connectionString, EmailValidationUserModel emailValidationUser, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationUserModel> response = new List<EmailValidationUserModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `email`, `user_id`, `email_type_id`, `token`, `used`, `modified_by`, `created_at`, `updated_at`  FROM ul_email_validation_users ";
                {
                    if (emailValidationUser != null)
                    {
                        if (!String.IsNullOrEmpty(emailValidationUser.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailValidationUser.Id);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.Email))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `email` = @email ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `email` = @email ");
                            }
                            cmd.Parameters.AddWithValue("@email", emailValidationUser.Email);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", emailValidationUser.UserId);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.EmailTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `email_type_id` = @emailTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `email_type_id` = @emailTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@emailTypeId", emailValidationUser.EmailTypeId);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.Token))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `token` = @token ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `token` = @token ");
                            }
                            cmd.Parameters.AddWithValue("@token", emailValidationUser.Token);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.Used))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `used` = @used ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `used` = @used ");
                            }
                            cmd.Parameters.AddWithValue("@used", emailValidationUser.Used);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailValidationUser.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailValidationUser.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUser.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailValidationUser.UpdatedAt);
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
                                    emailValidationUser.Id = reader.GetValue(0).ToString();
                                else
                                    emailValidationUser.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailValidationUser.Email = reader.GetValue(1).ToString();
                                else
                                    emailValidationUser.Email = null;
                                if (!reader.IsDBNull(2))
                                    emailValidationUser.UserId = reader.GetValue(2).ToString();
                                else
                                    emailValidationUser.UserId = null;
                                if (!reader.IsDBNull(3))
                                    emailValidationUser.EmailTypeId = reader.GetValue(3).ToString();
                                else
                                    emailValidationUser.EmailTypeId = null;
                                if (!reader.IsDBNull(4))
                                    emailValidationUser.Token = reader.GetValue(4).ToString();
                                else
                                    emailValidationUser.Token = null;
                                if (!reader.IsDBNull(5))
                                    emailValidationUser.Used = reader.GetValue(5).ToString();
                                else
                                    emailValidationUser.Used = null;
                                if (!reader.IsDBNull(6))
                                    emailValidationUser.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    emailValidationUser.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    emailValidationUser.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    emailValidationUser.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    emailValidationUser.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    emailValidationUser.UpdatedAt = null;
                                response.Add(emailValidationUser);
                                emailValidationUser = new EmailValidationUserModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EmailValidationUserModel>();
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
                response = new List<EmailValidationUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailValidationUserModel> Read(string connectionString, List<EmailValidationUserModel> emailValidationUserWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationUserModel> response = new List<EmailValidationUserModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `email`, `user_id`, `email_type_id`, `token`, `used`, `modified_by`, `created_at`, `updated_at`  FROM ul_email_validation_users ";
                if (emailValidationUserWhere != null && emailValidationUserWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailValidationUserWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailValidationUserWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].Email))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email` = @email" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@email" + i, emailValidationUserWhere[i].Email);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, emailValidationUserWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].EmailTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email_type_id` = @emailTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@emailTypeId" + i, emailValidationUserWhere[i].EmailTypeId);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].Token))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `token` = @token" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@token" + i, emailValidationUserWhere[i].Token);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].Used))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `used` = @used" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@used" + i, emailValidationUserWhere[i].Used);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailValidationUserWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailValidationUserWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailValidationUserWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailValidationUserWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailValidationUserWhere.Count - 1)
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
                            EmailValidationUserModel emailValidationUser = new EmailValidationUserModel(); if (!reader.IsDBNull(0))
                                emailValidationUser.Id = reader.GetValue(0).ToString();
                            else
                                emailValidationUser.Id = null;
                            if (!reader.IsDBNull(1))
                                emailValidationUser.Email = reader.GetValue(1).ToString();
                            else
                                emailValidationUser.Email = null;
                            if (!reader.IsDBNull(2))
                                emailValidationUser.UserId = reader.GetValue(2).ToString();
                            else
                                emailValidationUser.UserId = null;
                            if (!reader.IsDBNull(3))
                                emailValidationUser.EmailTypeId = reader.GetValue(3).ToString();
                            else
                                emailValidationUser.EmailTypeId = null;
                            if (!reader.IsDBNull(4))
                                emailValidationUser.Token = reader.GetValue(4).ToString();
                            else
                                emailValidationUser.Token = null;
                            if (!reader.IsDBNull(5))
                                emailValidationUser.Used = reader.GetValue(5).ToString();
                            else
                                emailValidationUser.Used = null;
                            if (!reader.IsDBNull(6))
                                emailValidationUser.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                emailValidationUser.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                emailValidationUser.CreatedAt = reader.GetValue(7).ToString();
                            else
                                emailValidationUser.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                emailValidationUser.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                emailValidationUser.UpdatedAt = null;
                            response.Add(emailValidationUser);
                            emailValidationUser = new EmailValidationUserModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EmailValidationUserModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailValidationUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailValidationUserModel> Create(string connectionString, EmailValidationUserModel emailValidationUser)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationUserModel> response = new List<EmailValidationUserModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO ul_email_validation_users (";
                string values = "(";
                if (!String.IsNullOrEmpty(emailValidationUser.Id))
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
                    cmd.Parameters.AddWithValue("@id", emailValidationUser.Id);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.Email))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `email` ");
                        values = String.Concat(values, ", @email");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `email` ");
                        values = String.Concat(values, " @email");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@email", emailValidationUser.Email);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.UserId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `user_id` ");
                        values = String.Concat(values, ", @userId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `user_id` ");
                        values = String.Concat(values, " @userId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", emailValidationUser.UserId);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.EmailTypeId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `email_type_id` ");
                        values = String.Concat(values, ", @emailTypeId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `email_type_id` ");
                        values = String.Concat(values, " @emailTypeId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@emailTypeId", emailValidationUser.EmailTypeId);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.Token))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `token` ");
                        values = String.Concat(values, ", @token");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `token` ");
                        values = String.Concat(values, " @token");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@token", emailValidationUser.Token);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.Used))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `used` ");
                        values = String.Concat(values, ", @used");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `used` ");
                        values = String.Concat(values, " @used");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@used", emailValidationUser.Used);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", emailValidationUser.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", emailValidationUser.CreatedAt);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", emailValidationUser.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                emailValidationUser.Id = cmd.LastInsertedId.ToString();
                response.Add(emailValidationUser);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailValidationUserModel>();
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
        public List<EmailValidationUserModel> Create(string connectionString, List<EmailValidationUserModel> emailValidationUserList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationUserModel> response = new List<EmailValidationUserModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < emailValidationUserList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO ul_email_validation_users (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, emailValidationUserList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].Email))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `email` ");
                                        values = String.Concat(values, ", @email" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `email` ");
                                        values = String.Concat(values, "@email" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@email" + i, emailValidationUserList[i].Email);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].UserId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `user_id` ");
                                        values = String.Concat(values, ", @userId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `user_id` ");
                                        values = String.Concat(values, "@userId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, emailValidationUserList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].EmailTypeId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `email_type_id` ");
                                        values = String.Concat(values, ", @emailTypeId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `email_type_id` ");
                                        values = String.Concat(values, "@emailTypeId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@emailTypeId" + i, emailValidationUserList[i].EmailTypeId);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].Token))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `token` ");
                                        values = String.Concat(values, ", @token" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `token` ");
                                        values = String.Concat(values, "@token" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@token" + i, emailValidationUserList[i].Token);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].Used))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `used` ");
                                        values = String.Concat(values, ", @used" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `used` ");
                                        values = String.Concat(values, "@used" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@used" + i, emailValidationUserList[i].Used);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailValidationUserList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailValidationUserList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailValidationUserList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(emailValidationUserList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailValidationUserModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailValidationUserModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailValidationUserModel>();
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

        public List<EmailValidationUserModel> Update(string connectionString, EmailValidationUserModel emailValidationUser, EmailValidationUserModel emailValidationUserWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationUserModel> response = new List<EmailValidationUserModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE ul_email_validation_users SET ";
                if (!String.IsNullOrEmpty(emailValidationUser.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", emailValidationUser.Id);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.Email))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `email` = @email");
                    else
                        sql = String.Concat(sql, " `email` = @email");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@email", emailValidationUser.Email);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", emailValidationUser.UserId);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.EmailTypeId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `email_type_id` = @emailTypeId");
                    else
                        sql = String.Concat(sql, " `email_type_id` = @emailTypeId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@emailTypeId", emailValidationUser.EmailTypeId);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.Token))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `token` = @token");
                    else
                        sql = String.Concat(sql, " `token` = @token");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@token", emailValidationUser.Token);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.Used))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `used` = @used");
                    else
                        sql = String.Concat(sql, " `used` = @used");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@used", emailValidationUser.Used);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", emailValidationUser.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", emailValidationUser.CreatedAt);
                }
                if (!String.IsNullOrEmpty(emailValidationUser.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", emailValidationUser.UpdatedAt);
                }
                if (emailValidationUserWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(emailValidationUserWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", emailValidationUserWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUserWhere.Email))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `email` = @emailWhere");
                        else
                            sql = String.Concat(sql, " `email` = @emailWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@emailWhere", emailValidationUserWhere.Email);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUserWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", emailValidationUserWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUserWhere.EmailTypeId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `email_type_id` = @emailTypeIdWhere");
                        else
                            sql = String.Concat(sql, " `email_type_id` = @emailTypeIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@emailTypeIdWhere", emailValidationUserWhere.EmailTypeId);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUserWhere.Token))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `token` = @tokenWhere");
                        else
                            sql = String.Concat(sql, " `token` = @tokenWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@tokenWhere", emailValidationUserWhere.Token);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUserWhere.Used))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `used` = @usedWhere");
                        else
                            sql = String.Concat(sql, " `used` = @usedWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@usedWhere", emailValidationUserWhere.Used);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUserWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", emailValidationUserWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUserWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", emailValidationUserWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUserWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", emailValidationUserWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(emailValidationUser);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailValidationUserModel>();
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

        public List<EmailValidationUserModel> Update(string connectionString, List<EmailValidationUserModel> emailValidationUserList, List<EmailValidationUserModel> emailValidationUserWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationUserModel> response = new List<EmailValidationUserModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < emailValidationUserList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE ul_email_validation_users SET ";
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, emailValidationUserList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].Email))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `email` = @email" + i);
                                    else
                                        sql = String.Concat(sql, " `email` = @email" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@email" + i, emailValidationUserList[i].Email);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, emailValidationUserList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].EmailTypeId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `email_type_id` = @emailTypeId" + i);
                                    else
                                        sql = String.Concat(sql, " `email_type_id` = @emailTypeId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@emailTypeId" + i, emailValidationUserList[i].EmailTypeId);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].Token))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `token` = @token" + i);
                                    else
                                        sql = String.Concat(sql, " `token` = @token" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@token" + i, emailValidationUserList[i].Token);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].Used))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `used` = @used" + i);
                                    else
                                        sql = String.Concat(sql, " `used` = @used" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@used" + i, emailValidationUserList[i].Used);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailValidationUserList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailValidationUserList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(emailValidationUserList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailValidationUserList[i].UpdatedAt);
                                }
                                if (emailValidationUserWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(emailValidationUserWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, emailValidationUserWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUserWhereList[i].Email))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `email` = @emailWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `email` = @emailWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@emailWhere" + i, emailValidationUserWhereList[i].Email);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUserWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, emailValidationUserWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUserWhereList[i].EmailTypeId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `email_type_id` = @emailTypeIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `email_type_id` = @emailTypeIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@emailTypeIdWhere" + i, emailValidationUserWhereList[i].EmailTypeId);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUserWhereList[i].Token))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `token` = @tokenWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `token` = @tokenWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@tokenWhere" + i, emailValidationUserWhereList[i].Token);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUserWhereList[i].Used))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `used` = @usedWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `used` = @usedWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@usedWhere" + i, emailValidationUserWhereList[i].Used);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUserWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, emailValidationUserWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUserWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, emailValidationUserWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUserWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, emailValidationUserWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailValidationUserList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailValidationUserModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailValidationUserModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailValidationUserModel>();
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

        public List<EmailValidationUserModel> Delete(string connectionString, EmailValidationUserModel emailValidationUser)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationUserModel> response = new List<EmailValidationUserModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM ul_email_validation_users  ";
                if (emailValidationUser != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(emailValidationUser.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", emailValidationUser.Id);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUser.Email))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `email` = @email");
                        else
                            sql = String.Concat(sql, " `email` = @email");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@email", emailValidationUser.Email);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUser.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", emailValidationUser.UserId);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUser.EmailTypeId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `email_type_id` = @emailTypeId");
                        else
                            sql = String.Concat(sql, " `email_type_id` = @emailTypeId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@emailTypeId", emailValidationUser.EmailTypeId);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUser.Token))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `token` = @token");
                        else
                            sql = String.Concat(sql, " `token` = @token");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@token", emailValidationUser.Token);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUser.Used))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `used` = @used");
                        else
                            sql = String.Concat(sql, " `used` = @used");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@used", emailValidationUser.Used);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUser.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", emailValidationUser.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUser.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", emailValidationUser.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(emailValidationUser.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", emailValidationUser.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(emailValidationUser);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailValidationUserModel>();
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

        public List<EmailValidationUserModel> Delete(string connectionString, List<EmailValidationUserModel> emailValidationUser)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationUserModel> response = new List<EmailValidationUserModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < emailValidationUser.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM ul_email_validation_users  ";
                                if (emailValidationUser[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(emailValidationUser[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, emailValidationUser[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUser[i].Email))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `email` = @email" + i);
                                        else
                                            sql = String.Concat(sql, " `email` = @email" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@email" + i, emailValidationUser[i].Email);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUser[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, emailValidationUser[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUser[i].EmailTypeId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `email_type_id` = @emailTypeId" + i);
                                        else
                                            sql = String.Concat(sql, " `email_type_id` = @emailTypeId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@emailTypeId" + i, emailValidationUser[i].EmailTypeId);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUser[i].Token))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `token` = @token" + i);
                                        else
                                            sql = String.Concat(sql, " `token` = @token" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@token" + i, emailValidationUser[i].Token);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUser[i].Used))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `used` = @used" + i);
                                        else
                                            sql = String.Concat(sql, " `used` = @used" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@used" + i, emailValidationUser[i].Used);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUser[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, emailValidationUser[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUser[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, emailValidationUser[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationUser[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, emailValidationUser[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailValidationUser[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailValidationUserModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailValidationUserModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailValidationUserModel>();
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
