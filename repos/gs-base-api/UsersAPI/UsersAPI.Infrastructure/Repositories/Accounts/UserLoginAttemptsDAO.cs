using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts
{
    class UserLoginAttemptDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, UserLoginAttemptModel> ReadD(string connectionString, UserLoginAttemptModel userLoginAttempt, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserLoginAttemptModel> response = new Dictionary<string, UserLoginAttemptModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `attempt`, `last_login`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_login_attempts ";
                {
                    if (userLoginAttempt != null)
                    {
                        if (!String.IsNullOrEmpty(userLoginAttempt.Id))
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
                            cmd.Parameters.AddWithValue("@id", userLoginAttempt.Id);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttempt.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userLoginAttempt.UserId);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttempt.Attempt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `attempt` = @attempt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `attempt` = @attempt ");
                            }
                            cmd.Parameters.AddWithValue("@attempt", userLoginAttempt.Attempt);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttempt.LastLogin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `last_login` = @lastLogin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `last_login` = @lastLogin ");
                            }
                            cmd.Parameters.AddWithValue("@lastLogin", userLoginAttempt.LastLogin);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttempt.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userLoginAttempt.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttempt.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userLoginAttempt.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttempt.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userLoginAttempt.UpdatedAt);
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
                                    userLoginAttempt.Id = reader.GetValue(0).ToString();
                                else
                                    userLoginAttempt.Id = null;
                                if (!reader.IsDBNull(1))
                                    userLoginAttempt.UserId = reader.GetValue(1).ToString();
                                else
                                    userLoginAttempt.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userLoginAttempt.Attempt = reader.GetValue(2).ToString();
                                else
                                    userLoginAttempt.Attempt = null;
                                if (!reader.IsDBNull(3))
                                    userLoginAttempt.LastLogin = reader.GetValue(3).ToString();
                                else
                                    userLoginAttempt.LastLogin = null;
                                if (!reader.IsDBNull(4))
                                    userLoginAttempt.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    userLoginAttempt.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    userLoginAttempt.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    userLoginAttempt.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    userLoginAttempt.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    userLoginAttempt.UpdatedAt = null;
                                response.Add(userLoginAttempt.Id, userLoginAttempt);
                                userLoginAttempt = new UserLoginAttemptModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserLoginAttemptModel>();
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
                response = new Dictionary<string, UserLoginAttemptModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserLoginAttemptModel> ReadD(string connectionString, List<UserLoginAttemptModel> userLoginAttemptWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserLoginAttemptModel> response = new Dictionary<string, UserLoginAttemptModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `attempt`, `last_login`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_login_attempts ";
                if (userLoginAttemptWhere != null && userLoginAttemptWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userLoginAttemptWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userLoginAttemptWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userLoginAttemptWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttemptWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userLoginAttemptWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttemptWhere[i].Attempt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `attempt` = @attempt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@attempt" + i, userLoginAttemptWhere[i].Attempt);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttemptWhere[i].LastLogin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `last_login` = @lastLogin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lastLogin" + i, userLoginAttemptWhere[i].LastLogin);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttemptWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userLoginAttemptWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttemptWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userLoginAttemptWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttemptWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userLoginAttemptWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userLoginAttemptWhere.Count - 1)
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
                            UserLoginAttemptModel userLoginAttempt = new UserLoginAttemptModel(); if (!reader.IsDBNull(0))
                                userLoginAttempt.Id = reader.GetValue(0).ToString();
                            else
                                userLoginAttempt.Id = null;
                            if (!reader.IsDBNull(1))
                                userLoginAttempt.UserId = reader.GetValue(1).ToString();
                            else
                                userLoginAttempt.UserId = null;
                            if (!reader.IsDBNull(2))
                                userLoginAttempt.Attempt = reader.GetValue(2).ToString();
                            else
                                userLoginAttempt.Attempt = null;
                            if (!reader.IsDBNull(3))
                                userLoginAttempt.LastLogin = reader.GetValue(3).ToString();
                            else
                                userLoginAttempt.LastLogin = null;
                            if (!reader.IsDBNull(4))
                                userLoginAttempt.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                userLoginAttempt.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                userLoginAttempt.CreatedAt = reader.GetValue(5).ToString();
                            else
                                userLoginAttempt.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                userLoginAttempt.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                userLoginAttempt.UpdatedAt = null;
                            response.Add(userLoginAttempt.Id, userLoginAttempt);
                            userLoginAttempt = new UserLoginAttemptModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserLoginAttemptModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserLoginAttemptModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserLoginAttemptModel> Read(string connectionString, UserLoginAttemptModel userLoginAttempt, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLoginAttemptModel> response = new List<UserLoginAttemptModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `attempt`, `last_login`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_login_attempts ";
                {
                    if (userLoginAttempt != null)
                    {
                        if (!String.IsNullOrEmpty(userLoginAttempt.Id))
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
                            cmd.Parameters.AddWithValue("@id", userLoginAttempt.Id);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttempt.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userLoginAttempt.UserId);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttempt.Attempt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `attempt` = @attempt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `attempt` = @attempt ");
                            }
                            cmd.Parameters.AddWithValue("@attempt", userLoginAttempt.Attempt);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttempt.LastLogin))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `last_login` = @lastLogin ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `last_login` = @lastLogin ");
                            }
                            cmd.Parameters.AddWithValue("@lastLogin", userLoginAttempt.LastLogin);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttempt.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userLoginAttempt.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttempt.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userLoginAttempt.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttempt.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userLoginAttempt.UpdatedAt);
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
                                    userLoginAttempt.Id = reader.GetValue(0).ToString();
                                else
                                    userLoginAttempt.Id = null;
                                if (!reader.IsDBNull(1))
                                    userLoginAttempt.UserId = reader.GetValue(1).ToString();
                                else
                                    userLoginAttempt.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userLoginAttempt.Attempt = reader.GetValue(2).ToString();
                                else
                                    userLoginAttempt.Attempt = null;
                                if (!reader.IsDBNull(3))
                                    userLoginAttempt.LastLogin = reader.GetValue(3).ToString();
                                else
                                    userLoginAttempt.LastLogin = null;
                                if (!reader.IsDBNull(4))
                                    userLoginAttempt.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    userLoginAttempt.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    userLoginAttempt.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    userLoginAttempt.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    userLoginAttempt.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    userLoginAttempt.UpdatedAt = null;
                                response.Add(userLoginAttempt);
                                userLoginAttempt = new UserLoginAttemptModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserLoginAttemptModel>();
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
                response = new List<UserLoginAttemptModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserLoginAttemptModel> Read(string connectionString, List<UserLoginAttemptModel> userLoginAttemptWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLoginAttemptModel> response = new List<UserLoginAttemptModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `attempt`, `last_login`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_login_attempts ";
                if (userLoginAttemptWhere != null && userLoginAttemptWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userLoginAttemptWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userLoginAttemptWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userLoginAttemptWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttemptWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userLoginAttemptWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttemptWhere[i].Attempt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `attempt` = @attempt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@attempt" + i, userLoginAttemptWhere[i].Attempt);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttemptWhere[i].LastLogin))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `last_login` = @lastLogin" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lastLogin" + i, userLoginAttemptWhere[i].LastLogin);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttemptWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userLoginAttemptWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttemptWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userLoginAttemptWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userLoginAttemptWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userLoginAttemptWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userLoginAttemptWhere.Count - 1)
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
                            UserLoginAttemptModel userLoginAttempt = new UserLoginAttemptModel(); if (!reader.IsDBNull(0))
                                userLoginAttempt.Id = reader.GetValue(0).ToString();
                            else
                                userLoginAttempt.Id = null;
                            if (!reader.IsDBNull(1))
                                userLoginAttempt.UserId = reader.GetValue(1).ToString();
                            else
                                userLoginAttempt.UserId = null;
                            if (!reader.IsDBNull(2))
                                userLoginAttempt.Attempt = reader.GetValue(2).ToString();
                            else
                                userLoginAttempt.Attempt = null;
                            if (!reader.IsDBNull(3))
                                userLoginAttempt.LastLogin = reader.GetValue(3).ToString();
                            else
                                userLoginAttempt.LastLogin = null;
                            if (!reader.IsDBNull(4))
                                userLoginAttempt.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                userLoginAttempt.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                userLoginAttempt.CreatedAt = reader.GetValue(5).ToString();
                            else
                                userLoginAttempt.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                userLoginAttempt.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                userLoginAttempt.UpdatedAt = null;
                            response.Add(userLoginAttempt);
                            userLoginAttempt = new UserLoginAttemptModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserLoginAttemptModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserLoginAttemptModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserLoginAttemptModel> Create(string connectionString, UserLoginAttemptModel userLoginAttempt)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLoginAttemptModel> response = new List<UserLoginAttemptModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO ul_user_login_attempts (";
                string values = "(";
                if (!String.IsNullOrEmpty(userLoginAttempt.Id))
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
                    cmd.Parameters.AddWithValue("@id", userLoginAttempt.Id);
                }
                if (!String.IsNullOrEmpty(userLoginAttempt.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", userLoginAttempt.UserId);
                }
                if (!String.IsNullOrEmpty(userLoginAttempt.Attempt))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `attempt` ");
                        values = String.Concat(values, ", @attempt");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `attempt` ");
                        values = String.Concat(values, " @attempt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@attempt", userLoginAttempt.Attempt);
                }
                if (!String.IsNullOrEmpty(userLoginAttempt.LastLogin))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `last_login` ");
                        values = String.Concat(values, ", @lastLogin");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `last_login` ");
                        values = String.Concat(values, " @lastLogin");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@lastLogin", userLoginAttempt.LastLogin);
                }
                if (!String.IsNullOrEmpty(userLoginAttempt.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userLoginAttempt.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userLoginAttempt.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userLoginAttempt.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userLoginAttempt.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userLoginAttempt.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userLoginAttempt.Id = cmd.LastInsertedId.ToString();
                response.Add(userLoginAttempt);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserLoginAttemptModel>();
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
        public List<UserLoginAttemptModel> Create(string connectionString, List<UserLoginAttemptModel> userLoginAttemptList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLoginAttemptModel> response = new List<UserLoginAttemptModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userLoginAttemptList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO ul_user_login_attempts (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(userLoginAttemptList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userLoginAttemptList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userLoginAttemptList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, userLoginAttemptList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userLoginAttemptList[i].Attempt))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `attempt` ");
                                        values = String.Concat(values, ", @attempt" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `attempt` ");
                                        values = String.Concat(values, "@attempt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@attempt" + i, userLoginAttemptList[i].Attempt);
                                }
                                if (!String.IsNullOrEmpty(userLoginAttemptList[i].LastLogin))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `last_login` ");
                                        values = String.Concat(values, ", @lastLogin" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `last_login` ");
                                        values = String.Concat(values, "@lastLogin" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@lastLogin" + i, userLoginAttemptList[i].LastLogin);
                                }
                                if (!String.IsNullOrEmpty(userLoginAttemptList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userLoginAttemptList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userLoginAttemptList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userLoginAttemptList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userLoginAttemptList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userLoginAttemptList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userLoginAttemptList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserLoginAttemptModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserLoginAttemptModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserLoginAttemptModel>();
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

        public List<UserLoginAttemptModel> Update(string connectionString, UserLoginAttemptModel userLoginAttempt, UserLoginAttemptModel userLoginAttemptWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLoginAttemptModel> response = new List<UserLoginAttemptModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE ul_user_login_attempts SET ";
                if (!String.IsNullOrEmpty(userLoginAttempt.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userLoginAttempt.Id);
                }
                if (!String.IsNullOrEmpty(userLoginAttempt.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userLoginAttempt.UserId);
                }
                if (!String.IsNullOrEmpty(userLoginAttempt.Attempt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `attempt` = @attempt");
                    else
                        sql = String.Concat(sql, " `attempt` = @attempt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@attempt", userLoginAttempt.Attempt);
                }
                if (!String.IsNullOrEmpty(userLoginAttempt.LastLogin))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `last_login` = @lastLogin");
                    else
                        sql = String.Concat(sql, " `last_login` = @lastLogin");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@lastLogin", userLoginAttempt.LastLogin);
                }
                if (!String.IsNullOrEmpty(userLoginAttempt.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userLoginAttempt.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userLoginAttempt.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userLoginAttempt.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userLoginAttempt.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userLoginAttempt.UpdatedAt);
                }
                if (userLoginAttemptWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(userLoginAttemptWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userLoginAttemptWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(userLoginAttemptWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userLoginAttemptWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(userLoginAttemptWhere.Attempt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `attempt` = @attemptWhere");
                        else
                            sql = String.Concat(sql, " `attempt` = @attemptWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@attemptWhere", userLoginAttemptWhere.Attempt);
                    }
                    if (!String.IsNullOrEmpty(userLoginAttemptWhere.LastLogin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `last_login` = @lastLoginWhere");
                        else
                            sql = String.Concat(sql, " `last_login` = @lastLoginWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lastLoginWhere", userLoginAttemptWhere.LastLogin);
                    }
                    if (!String.IsNullOrEmpty(userLoginAttemptWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userLoginAttemptWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userLoginAttemptWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userLoginAttemptWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userLoginAttemptWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userLoginAttemptWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userLoginAttempt);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserLoginAttemptModel>();
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

        public List<UserLoginAttemptModel> Update(string connectionString, List<UserLoginAttemptModel> userLoginAttemptList, List<UserLoginAttemptModel> userLoginAttemptWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLoginAttemptModel> response = new List<UserLoginAttemptModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userLoginAttemptList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE ul_user_login_attempts SET ";
                                if (!String.IsNullOrEmpty(userLoginAttemptList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userLoginAttemptList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userLoginAttemptList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userLoginAttemptList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userLoginAttemptList[i].Attempt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `attempt` = @attempt" + i);
                                    else
                                        sql = String.Concat(sql, " `attempt` = @attempt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@attempt" + i, userLoginAttemptList[i].Attempt);
                                }
                                if (!String.IsNullOrEmpty(userLoginAttemptList[i].LastLogin))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `last_login` = @lastLogin" + i);
                                    else
                                        sql = String.Concat(sql, " `last_login` = @lastLogin" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@lastLogin" + i, userLoginAttemptList[i].LastLogin);
                                }
                                if (!String.IsNullOrEmpty(userLoginAttemptList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userLoginAttemptList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userLoginAttemptList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userLoginAttemptList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userLoginAttemptList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userLoginAttemptList[i].UpdatedAt);
                                }
                                if (userLoginAttemptWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(userLoginAttemptWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userLoginAttemptWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userLoginAttemptWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userLoginAttemptWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userLoginAttemptWhereList[i].Attempt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `attempt` = @attemptWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `attempt` = @attemptWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@attemptWhere" + i, userLoginAttemptWhereList[i].Attempt);
                                    }
                                    if (!String.IsNullOrEmpty(userLoginAttemptWhereList[i].LastLogin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `last_login` = @lastLoginWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `last_login` = @lastLoginWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lastLoginWhere" + i, userLoginAttemptWhereList[i].LastLogin);
                                    }
                                    if (!String.IsNullOrEmpty(userLoginAttemptWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userLoginAttemptWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userLoginAttemptWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userLoginAttemptWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userLoginAttemptWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userLoginAttemptWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userLoginAttemptList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserLoginAttemptModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserLoginAttemptModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserLoginAttemptModel>();
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

        public List<UserLoginAttemptModel> Delete(string connectionString, UserLoginAttemptModel userLoginAttempt)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLoginAttemptModel> response = new List<UserLoginAttemptModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM ul_user_login_attempts  ";
                if (userLoginAttempt != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(userLoginAttempt.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userLoginAttempt.Id);
                    }
                    if (!String.IsNullOrEmpty(userLoginAttempt.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userLoginAttempt.UserId);
                    }
                    if (!String.IsNullOrEmpty(userLoginAttempt.Attempt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `attempt` = @attempt");
                        else
                            sql = String.Concat(sql, " `attempt` = @attempt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@attempt", userLoginAttempt.Attempt);
                    }
                    if (!String.IsNullOrEmpty(userLoginAttempt.LastLogin))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `last_login` = @lastLogin");
                        else
                            sql = String.Concat(sql, " `last_login` = @lastLogin");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lastLogin", userLoginAttempt.LastLogin);
                    }
                    if (!String.IsNullOrEmpty(userLoginAttempt.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userLoginAttempt.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userLoginAttempt.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userLoginAttempt.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userLoginAttempt.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userLoginAttempt.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userLoginAttempt);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserLoginAttemptModel>();
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

        public List<UserLoginAttemptModel> Delete(string connectionString, List<UserLoginAttemptModel> userLoginAttempt)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLoginAttemptModel> response = new List<UserLoginAttemptModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userLoginAttempt.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM ul_user_login_attempts  ";
                                if (userLoginAttempt[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(userLoginAttempt[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userLoginAttempt[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userLoginAttempt[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userLoginAttempt[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userLoginAttempt[i].Attempt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `attempt` = @attempt" + i);
                                        else
                                            sql = String.Concat(sql, " `attempt` = @attempt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@attempt" + i, userLoginAttempt[i].Attempt);
                                    }
                                    if (!String.IsNullOrEmpty(userLoginAttempt[i].LastLogin))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `last_login` = @lastLogin" + i);
                                        else
                                            sql = String.Concat(sql, " `last_login` = @lastLogin" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lastLogin" + i, userLoginAttempt[i].LastLogin);
                                    }
                                    if (!String.IsNullOrEmpty(userLoginAttempt[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userLoginAttempt[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userLoginAttempt[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userLoginAttempt[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userLoginAttempt[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userLoginAttempt[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userLoginAttempt[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserLoginAttemptModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserLoginAttemptModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserLoginAttemptModel>();
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
