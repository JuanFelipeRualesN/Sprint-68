using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts
{
    class UserAccountStatusDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, UserAccountStatusModel> ReadD(string connectionString, UserAccountStatusModel userAccountStatus, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserAccountStatusModel> response = new Dictionary<string, UserAccountStatusModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `status_id`, `detail`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_account_statuses ";
                {
                    if (userAccountStatus != null)
                    {
                        if (!String.IsNullOrEmpty(userAccountStatus.Id))
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
                            cmd.Parameters.AddWithValue("@id", userAccountStatus.Id);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatus.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userAccountStatus.UserId);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatus.StatusId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `status_id` = @statusId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `status_id` = @statusId ");
                            }
                            cmd.Parameters.AddWithValue("@statusId", userAccountStatus.StatusId);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatus.Detail))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `detail` = @detail ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `detail` = @detail ");
                            }
                            cmd.Parameters.AddWithValue("@detail", userAccountStatus.Detail);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatus.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userAccountStatus.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatus.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userAccountStatus.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatus.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userAccountStatus.UpdatedAt);
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
                                    userAccountStatus.Id = reader.GetValue(0).ToString();
                                else
                                    userAccountStatus.Id = null;
                                if (!reader.IsDBNull(1))
                                    userAccountStatus.UserId = reader.GetValue(1).ToString();
                                else
                                    userAccountStatus.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userAccountStatus.StatusId = reader.GetValue(2).ToString();
                                else
                                    userAccountStatus.StatusId = null;
                                if (!reader.IsDBNull(3))
                                    userAccountStatus.Detail = reader.GetValue(3).ToString();
                                else
                                    userAccountStatus.Detail = null;
                                if (!reader.IsDBNull(4))
                                    userAccountStatus.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    userAccountStatus.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    userAccountStatus.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    userAccountStatus.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    userAccountStatus.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    userAccountStatus.UpdatedAt = null;
                                response.Add(userAccountStatus.Id, userAccountStatus);
                                userAccountStatus = new UserAccountStatusModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserAccountStatusModel>();
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
                response = new Dictionary<string, UserAccountStatusModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserAccountStatusModel> ReadD(string connectionString, List<UserAccountStatusModel> userAccountStatusWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserAccountStatusModel> response = new Dictionary<string, UserAccountStatusModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `status_id`, `detail`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_account_statuses ";
                if (userAccountStatusWhere != null && userAccountStatusWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userAccountStatusWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userAccountStatusWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userAccountStatusWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatusWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userAccountStatusWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatusWhere[i].StatusId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `status_id` = @statusId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@statusId" + i, userAccountStatusWhere[i].StatusId);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatusWhere[i].Detail))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `detail` = @detail" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@detail" + i, userAccountStatusWhere[i].Detail);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatusWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userAccountStatusWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatusWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userAccountStatusWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatusWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userAccountStatusWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userAccountStatusWhere.Count - 1)
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
                            UserAccountStatusModel userAccountStatus = new UserAccountStatusModel(); if (!reader.IsDBNull(0))
                                userAccountStatus.Id = reader.GetValue(0).ToString();
                            else
                                userAccountStatus.Id = null;
                            if (!reader.IsDBNull(1))
                                userAccountStatus.UserId = reader.GetValue(1).ToString();
                            else
                                userAccountStatus.UserId = null;
                            if (!reader.IsDBNull(2))
                                userAccountStatus.StatusId = reader.GetValue(2).ToString();
                            else
                                userAccountStatus.StatusId = null;
                            if (!reader.IsDBNull(3))
                                userAccountStatus.Detail = reader.GetValue(3).ToString();
                            else
                                userAccountStatus.Detail = null;
                            if (!reader.IsDBNull(4))
                                userAccountStatus.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                userAccountStatus.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                userAccountStatus.CreatedAt = reader.GetValue(5).ToString();
                            else
                                userAccountStatus.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                userAccountStatus.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                userAccountStatus.UpdatedAt = null;
                            response.Add(userAccountStatus.Id, userAccountStatus);
                            userAccountStatus = new UserAccountStatusModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserAccountStatusModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserAccountStatusModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserAccountStatusModel> Read(string connectionString, UserAccountStatusModel userAccountStatus, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAccountStatusModel> response = new List<UserAccountStatusModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `status_id`, `detail`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_account_statuses ";
                {
                    if (userAccountStatus != null)
                    {
                        if (!String.IsNullOrEmpty(userAccountStatus.Id))
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
                            cmd.Parameters.AddWithValue("@id", userAccountStatus.Id);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatus.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userAccountStatus.UserId);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatus.StatusId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `status_id` = @statusId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `status_id` = @statusId ");
                            }
                            cmd.Parameters.AddWithValue("@statusId", userAccountStatus.StatusId);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatus.Detail))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `detail` = @detail ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `detail` = @detail ");
                            }
                            cmd.Parameters.AddWithValue("@detail", userAccountStatus.Detail);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatus.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userAccountStatus.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatus.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userAccountStatus.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatus.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userAccountStatus.UpdatedAt);
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
                                    userAccountStatus.Id = reader.GetValue(0).ToString();
                                else
                                    userAccountStatus.Id = null;
                                if (!reader.IsDBNull(1))
                                    userAccountStatus.UserId = reader.GetValue(1).ToString();
                                else
                                    userAccountStatus.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userAccountStatus.StatusId = reader.GetValue(2).ToString();
                                else
                                    userAccountStatus.StatusId = null;
                                if (!reader.IsDBNull(3))
                                    userAccountStatus.Detail = reader.GetValue(3).ToString();
                                else
                                    userAccountStatus.Detail = null;
                                if (!reader.IsDBNull(4))
                                    userAccountStatus.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    userAccountStatus.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    userAccountStatus.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    userAccountStatus.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    userAccountStatus.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    userAccountStatus.UpdatedAt = null;
                                response.Add(userAccountStatus);
                                userAccountStatus = new UserAccountStatusModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserAccountStatusModel>();
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
                response = new List<UserAccountStatusModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserAccountStatusModel> Read(string connectionString, List<UserAccountStatusModel> userAccountStatusWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAccountStatusModel> response = new List<UserAccountStatusModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `status_id`, `detail`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_account_statuses ";
                if (userAccountStatusWhere != null && userAccountStatusWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userAccountStatusWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userAccountStatusWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userAccountStatusWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatusWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userAccountStatusWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatusWhere[i].StatusId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `status_id` = @statusId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@statusId" + i, userAccountStatusWhere[i].StatusId);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatusWhere[i].Detail))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `detail` = @detail" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@detail" + i, userAccountStatusWhere[i].Detail);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatusWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userAccountStatusWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatusWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userAccountStatusWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userAccountStatusWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userAccountStatusWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userAccountStatusWhere.Count - 1)
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
                            UserAccountStatusModel userAccountStatus = new UserAccountStatusModel(); if (!reader.IsDBNull(0))
                                userAccountStatus.Id = reader.GetValue(0).ToString();
                            else
                                userAccountStatus.Id = null;
                            if (!reader.IsDBNull(1))
                                userAccountStatus.UserId = reader.GetValue(1).ToString();
                            else
                                userAccountStatus.UserId = null;
                            if (!reader.IsDBNull(2))
                                userAccountStatus.StatusId = reader.GetValue(2).ToString();
                            else
                                userAccountStatus.StatusId = null;
                            if (!reader.IsDBNull(3))
                                userAccountStatus.Detail = reader.GetValue(3).ToString();
                            else
                                userAccountStatus.Detail = null;
                            if (!reader.IsDBNull(4))
                                userAccountStatus.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                userAccountStatus.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                userAccountStatus.CreatedAt = reader.GetValue(5).ToString();
                            else
                                userAccountStatus.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                userAccountStatus.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                userAccountStatus.UpdatedAt = null;
                            response.Add(userAccountStatus);
                            userAccountStatus = new UserAccountStatusModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserAccountStatusModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserAccountStatusModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserAccountStatusModel> Create(string connectionString, UserAccountStatusModel userAccountStatus)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAccountStatusModel> response = new List<UserAccountStatusModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO ul_user_account_statuses (";
                string values = "(";
                if (!String.IsNullOrEmpty(userAccountStatus.Id))
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
                    cmd.Parameters.AddWithValue("@id", userAccountStatus.Id);
                }
                if (!String.IsNullOrEmpty(userAccountStatus.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", userAccountStatus.UserId);
                }
                if (!String.IsNullOrEmpty(userAccountStatus.StatusId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `status_id` ");
                        values = String.Concat(values, ", @statusId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `status_id` ");
                        values = String.Concat(values, " @statusId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@statusId", userAccountStatus.StatusId);
                }
                if (!String.IsNullOrEmpty(userAccountStatus.Detail))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `detail` ");
                        values = String.Concat(values, ", @detail");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `detail` ");
                        values = String.Concat(values, " @detail");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@detail", userAccountStatus.Detail);
                }
                if (!String.IsNullOrEmpty(userAccountStatus.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userAccountStatus.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userAccountStatus.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userAccountStatus.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userAccountStatus.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userAccountStatus.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userAccountStatus.Id = cmd.LastInsertedId.ToString();
                response.Add(userAccountStatus);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserAccountStatusModel>();
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
        public List<UserAccountStatusModel> Create(string connectionString, List<UserAccountStatusModel> userAccountStatusList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAccountStatusModel> response = new List<UserAccountStatusModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userAccountStatusList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO ul_user_account_statuses (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(userAccountStatusList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userAccountStatusList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userAccountStatusList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, userAccountStatusList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userAccountStatusList[i].StatusId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `status_id` ");
                                        values = String.Concat(values, ", @statusId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `status_id` ");
                                        values = String.Concat(values, "@statusId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@statusId" + i, userAccountStatusList[i].StatusId);
                                }
                                if (!String.IsNullOrEmpty(userAccountStatusList[i].Detail))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `detail` ");
                                        values = String.Concat(values, ", @detail" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `detail` ");
                                        values = String.Concat(values, "@detail" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@detail" + i, userAccountStatusList[i].Detail);
                                }
                                if (!String.IsNullOrEmpty(userAccountStatusList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userAccountStatusList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userAccountStatusList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userAccountStatusList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userAccountStatusList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userAccountStatusList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userAccountStatusList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserAccountStatusModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserAccountStatusModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserAccountStatusModel>();
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

        public List<UserAccountStatusModel> Update(string connectionString, UserAccountStatusModel userAccountStatus, UserAccountStatusModel userAccountStatusWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAccountStatusModel> response = new List<UserAccountStatusModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE ul_user_account_statuses SET ";
                if (!String.IsNullOrEmpty(userAccountStatus.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userAccountStatus.Id);
                }
                if (!String.IsNullOrEmpty(userAccountStatus.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userAccountStatus.UserId);
                }
                if (!String.IsNullOrEmpty(userAccountStatus.StatusId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `status_id` = @statusId");
                    else
                        sql = String.Concat(sql, " `status_id` = @statusId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@statusId", userAccountStatus.StatusId);
                }
                if (!String.IsNullOrEmpty(userAccountStatus.Detail))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `detail` = @detail");
                    else
                        sql = String.Concat(sql, " `detail` = @detail");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@detail", userAccountStatus.Detail);
                }
                if (!String.IsNullOrEmpty(userAccountStatus.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userAccountStatus.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userAccountStatus.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userAccountStatus.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userAccountStatus.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userAccountStatus.UpdatedAt);
                }
                if (userAccountStatusWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(userAccountStatusWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userAccountStatusWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(userAccountStatusWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userAccountStatusWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(userAccountStatusWhere.StatusId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `status_id` = @statusIdWhere");
                        else
                            sql = String.Concat(sql, " `status_id` = @statusIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@statusIdWhere", userAccountStatusWhere.StatusId);
                    }
                    if (!String.IsNullOrEmpty(userAccountStatusWhere.Detail))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `detail` = @detailWhere");
                        else
                            sql = String.Concat(sql, " `detail` = @detailWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@detailWhere", userAccountStatusWhere.Detail);
                    }
                    if (!String.IsNullOrEmpty(userAccountStatusWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userAccountStatusWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userAccountStatusWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userAccountStatusWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userAccountStatusWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userAccountStatusWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userAccountStatus);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserAccountStatusModel>();
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

        public List<UserAccountStatusModel> Update(string connectionString, List<UserAccountStatusModel> userAccountStatusList, List<UserAccountStatusModel> userAccountStatusWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAccountStatusModel> response = new List<UserAccountStatusModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userAccountStatusList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE ul_user_account_statuses SET ";
                                if (!String.IsNullOrEmpty(userAccountStatusList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userAccountStatusList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userAccountStatusList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userAccountStatusList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userAccountStatusList[i].StatusId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `status_id` = @statusId" + i);
                                    else
                                        sql = String.Concat(sql, " `status_id` = @statusId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@statusId" + i, userAccountStatusList[i].StatusId);
                                }
                                if (!String.IsNullOrEmpty(userAccountStatusList[i].Detail))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `detail` = @detail" + i);
                                    else
                                        sql = String.Concat(sql, " `detail` = @detail" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@detail" + i, userAccountStatusList[i].Detail);
                                }
                                if (!String.IsNullOrEmpty(userAccountStatusList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userAccountStatusList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userAccountStatusList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userAccountStatusList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userAccountStatusList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userAccountStatusList[i].UpdatedAt);
                                }
                                if (userAccountStatusWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(userAccountStatusWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userAccountStatusWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userAccountStatusWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userAccountStatusWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userAccountStatusWhereList[i].StatusId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `status_id` = @statusIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `status_id` = @statusIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@statusIdWhere" + i, userAccountStatusWhereList[i].StatusId);
                                    }
                                    if (!String.IsNullOrEmpty(userAccountStatusWhereList[i].Detail))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `detail` = @detailWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `detail` = @detailWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@detailWhere" + i, userAccountStatusWhereList[i].Detail);
                                    }
                                    if (!String.IsNullOrEmpty(userAccountStatusWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userAccountStatusWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userAccountStatusWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userAccountStatusWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userAccountStatusWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userAccountStatusWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userAccountStatusList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserAccountStatusModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserAccountStatusModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserAccountStatusModel>();
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

        public List<UserAccountStatusModel> Delete(string connectionString, UserAccountStatusModel userAccountStatus)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAccountStatusModel> response = new List<UserAccountStatusModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM ul_user_account_statuses  ";
                if (userAccountStatus != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(userAccountStatus.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userAccountStatus.Id);
                    }
                    if (!String.IsNullOrEmpty(userAccountStatus.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userAccountStatus.UserId);
                    }
                    if (!String.IsNullOrEmpty(userAccountStatus.StatusId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `status_id` = @statusId");
                        else
                            sql = String.Concat(sql, " `status_id` = @statusId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@statusId", userAccountStatus.StatusId);
                    }
                    if (!String.IsNullOrEmpty(userAccountStatus.Detail))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `detail` = @detail");
                        else
                            sql = String.Concat(sql, " `detail` = @detail");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@detail", userAccountStatus.Detail);
                    }
                    if (!String.IsNullOrEmpty(userAccountStatus.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userAccountStatus.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userAccountStatus.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userAccountStatus.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userAccountStatus.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userAccountStatus.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userAccountStatus);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserAccountStatusModel>();
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

        public List<UserAccountStatusModel> Delete(string connectionString, List<UserAccountStatusModel> userAccountStatus)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAccountStatusModel> response = new List<UserAccountStatusModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userAccountStatus.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM ul_user_account_statuses  ";
                                if (userAccountStatus[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(userAccountStatus[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userAccountStatus[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userAccountStatus[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userAccountStatus[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userAccountStatus[i].StatusId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `status_id` = @statusId" + i);
                                        else
                                            sql = String.Concat(sql, " `status_id` = @statusId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@statusId" + i, userAccountStatus[i].StatusId);
                                    }
                                    if (!String.IsNullOrEmpty(userAccountStatus[i].Detail))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `detail` = @detail" + i);
                                        else
                                            sql = String.Concat(sql, " `detail` = @detail" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@detail" + i, userAccountStatus[i].Detail);
                                    }
                                    if (!String.IsNullOrEmpty(userAccountStatus[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userAccountStatus[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userAccountStatus[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userAccountStatus[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userAccountStatus[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userAccountStatus[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userAccountStatus[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserAccountStatusModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserAccountStatusModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserAccountStatusModel>();
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
