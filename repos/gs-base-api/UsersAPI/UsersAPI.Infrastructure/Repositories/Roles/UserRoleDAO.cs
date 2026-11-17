using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Roles;
using UsersAPI.UsersAPI.Services.Roles.CustomClasses;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Roles
{
    class UserRoleDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, UserRoleModel> ReadD(string connectionString, UserRoleModel userRole, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserRoleModel> response = new Dictionary<string, UserRoleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `role_id`, `user_id`, `modified_by`, `created_at`, `updated_at`  FROM ur_user_roles ";
                {
                    if (userRole != null)
                    {
                        if (!String.IsNullOrEmpty(userRole.Id))
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
                            cmd.Parameters.AddWithValue("@id", userRole.Id);
                        }
                        if (!String.IsNullOrEmpty(userRole.RoleId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `role_id` = @roleId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `role_id` = @roleId ");
                            }
                            cmd.Parameters.AddWithValue("@roleId", userRole.RoleId);
                        }
                        if (!String.IsNullOrEmpty(userRole.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userRole.UserId);
                        }
                        if (!String.IsNullOrEmpty(userRole.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userRole.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userRole.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userRole.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userRole.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userRole.UpdatedAt);
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
                                    userRole.Id = reader.GetValue(0).ToString();
                                else
                                    userRole.Id = null;
                                if (!reader.IsDBNull(1))
                                    userRole.RoleId = reader.GetValue(1).ToString();
                                else
                                    userRole.RoleId = null;
                                if (!reader.IsDBNull(2))
                                    userRole.UserId = reader.GetValue(2).ToString();
                                else
                                    userRole.UserId = null;
                                if (!reader.IsDBNull(3))
                                    userRole.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    userRole.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    userRole.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    userRole.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    userRole.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    userRole.UpdatedAt = null;
                                response.Add(userRole.Id, userRole);
                                userRole = new UserRoleModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserRoleModel>();
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
                response = new Dictionary<string, UserRoleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserRoleModel> ReadD(string connectionString, List<UserRoleModel> userRoleWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserRoleModel> response = new Dictionary<string, UserRoleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `role_id`, `user_id`, `modified_by`, `created_at`, `updated_at`  FROM ur_user_roles ";
                if (userRoleWhere != null && userRoleWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userRoleWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userRoleWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userRoleWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userRoleWhere[i].RoleId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `role_id` = @roleId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@roleId" + i, userRoleWhere[i].RoleId);
                        }
                        if (!String.IsNullOrEmpty(userRoleWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userRoleWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userRoleWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userRoleWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userRoleWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userRoleWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userRoleWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userRoleWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userRoleWhere.Count - 1)
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
                            UserRoleModel userRole = new UserRoleModel(); if (!reader.IsDBNull(0))
                                userRole.Id = reader.GetValue(0).ToString();
                            else
                                userRole.Id = null;
                            if (!reader.IsDBNull(1))
                                userRole.RoleId = reader.GetValue(1).ToString();
                            else
                                userRole.RoleId = null;
                            if (!reader.IsDBNull(2))
                                userRole.UserId = reader.GetValue(2).ToString();
                            else
                                userRole.UserId = null;
                            if (!reader.IsDBNull(3))
                                userRole.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                userRole.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                userRole.CreatedAt = reader.GetValue(4).ToString();
                            else
                                userRole.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                userRole.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                userRole.UpdatedAt = null;
                            response.Add(userRole.Id, userRole);
                            userRole = new UserRoleModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserRoleModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserRoleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserRoleModel> Read(string connectionString, UserRoleModel userRole, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRoleModel> response = new List<UserRoleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `role_id`, `user_id`, `modified_by`, `created_at`, `updated_at`  FROM ur_user_roles ";
                {
                    if (userRole != null)
                    {
                        if (!String.IsNullOrEmpty(userRole.Id))
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
                            cmd.Parameters.AddWithValue("@id", userRole.Id);
                        }
                        if (!String.IsNullOrEmpty(userRole.RoleId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `role_id` = @roleId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `role_id` = @roleId ");
                            }
                            cmd.Parameters.AddWithValue("@roleId", userRole.RoleId);
                        }
                        if (!String.IsNullOrEmpty(userRole.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userRole.UserId);
                        }
                        if (!String.IsNullOrEmpty(userRole.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userRole.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userRole.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userRole.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userRole.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userRole.UpdatedAt);
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
                                    userRole.Id = reader.GetValue(0).ToString();
                                else
                                    userRole.Id = null;
                                if (!reader.IsDBNull(1))
                                    userRole.RoleId = reader.GetValue(1).ToString();
                                else
                                    userRole.RoleId = null;
                                if (!reader.IsDBNull(2))
                                    userRole.UserId = reader.GetValue(2).ToString();
                                else
                                    userRole.UserId = null;
                                if (!reader.IsDBNull(3))
                                    userRole.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    userRole.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    userRole.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    userRole.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    userRole.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    userRole.UpdatedAt = null;
                                response.Add(userRole);
                                userRole = new UserRoleModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserRoleModel>();
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
                response = new List<UserRoleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserRoleModel> Read(string connectionString, List<UserRoleModel> userRoleWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRoleModel> response = new List<UserRoleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `role_id`, `user_id`, `modified_by`, `created_at`, `updated_at`  FROM ur_user_roles ";
                if (userRoleWhere != null && userRoleWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userRoleWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userRoleWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userRoleWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userRoleWhere[i].RoleId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `role_id` = @roleId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@roleId" + i, userRoleWhere[i].RoleId);
                        }
                        if (!String.IsNullOrEmpty(userRoleWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userRoleWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userRoleWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userRoleWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userRoleWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userRoleWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userRoleWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userRoleWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userRoleWhere.Count - 1)
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
                            UserRoleModel userRole = new UserRoleModel(); if (!reader.IsDBNull(0))
                                userRole.Id = reader.GetValue(0).ToString();
                            else
                                userRole.Id = null;
                            if (!reader.IsDBNull(1))
                                userRole.RoleId = reader.GetValue(1).ToString();
                            else
                                userRole.RoleId = null;
                            if (!reader.IsDBNull(2))
                                userRole.UserId = reader.GetValue(2).ToString();
                            else
                                userRole.UserId = null;
                            if (!reader.IsDBNull(3))
                                userRole.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                userRole.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                userRole.CreatedAt = reader.GetValue(4).ToString();
                            else
                                userRole.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                userRole.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                userRole.UpdatedAt = null;
                            response.Add(userRole);
                            userRole = new UserRoleModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserRoleModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserRoleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserRoleModel> Create(string connectionString, UserRoleModel userRole)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRoleModel> response = new List<UserRoleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO ur_user_roles (";
                string values = "(";
                if (!String.IsNullOrEmpty(userRole.Id))
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
                    cmd.Parameters.AddWithValue("@id", userRole.Id);
                }
                if (!String.IsNullOrEmpty(userRole.RoleId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `role_id` ");
                        values = String.Concat(values, ", @roleId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `role_id` ");
                        values = String.Concat(values, " @roleId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@roleId", userRole.RoleId);
                }
                if (!String.IsNullOrEmpty(userRole.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", userRole.UserId);
                }
                if (!String.IsNullOrEmpty(userRole.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userRole.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userRole.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userRole.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userRole.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userRole.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userRole.Id = cmd.LastInsertedId.ToString();
                response.Add(userRole);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserRoleModel>();
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
        public List<UserRoleModel> Create(string connectionString, List<UserRoleModel> userRoleList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRoleModel> response = new List<UserRoleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userRoleList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO ur_user_roles (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(userRoleList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userRoleList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userRoleList[i].RoleId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `role_id` ");
                                        values = String.Concat(values, ", @roleId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `role_id` ");
                                        values = String.Concat(values, "@roleId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@roleId" + i, userRoleList[i].RoleId);
                                }
                                if (!String.IsNullOrEmpty(userRoleList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, userRoleList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userRoleList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userRoleList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userRoleList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userRoleList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userRoleList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userRoleList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userRoleList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserRoleModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserRoleModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserRoleModel>();
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

        public List<UserRoleModel> Update(string connectionString, UserRoleModel userRole, UserRoleModel userRoleWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRoleModel> response = new List<UserRoleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE ur_user_roles SET ";
                if (!String.IsNullOrEmpty(userRole.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userRole.Id);
                }
                if (!String.IsNullOrEmpty(userRole.RoleId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `role_id` = @roleId");
                    else
                        sql = String.Concat(sql, " `role_id` = @roleId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@roleId", userRole.RoleId);
                }
                if (!String.IsNullOrEmpty(userRole.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userRole.UserId);
                }
                if (!String.IsNullOrEmpty(userRole.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userRole.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userRole.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userRole.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userRole.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userRole.UpdatedAt);
                }
                if (userRoleWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(userRoleWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userRoleWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(userRoleWhere.RoleId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `role_id` = @roleIdWhere");
                        else
                            sql = String.Concat(sql, " `role_id` = @roleIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@roleIdWhere", userRoleWhere.RoleId);
                    }
                    if (!String.IsNullOrEmpty(userRoleWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userRoleWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(userRoleWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userRoleWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userRoleWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userRoleWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userRoleWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userRoleWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userRole);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserRoleModel>();
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

        public List<UserRoleModel> Update(string connectionString, List<UserRoleModel> userRoleList, List<UserRoleModel> userRoleWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRoleModel> response = new List<UserRoleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userRoleList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE ur_user_roles SET ";
                                if (!String.IsNullOrEmpty(userRoleList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userRoleList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userRoleList[i].RoleId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `role_id` = @roleId" + i);
                                    else
                                        sql = String.Concat(sql, " `role_id` = @roleId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@roleId" + i, userRoleList[i].RoleId);
                                }
                                if (!String.IsNullOrEmpty(userRoleList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userRoleList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userRoleList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userRoleList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userRoleList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userRoleList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userRoleList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userRoleList[i].UpdatedAt);
                                }
                                if (userRoleWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(userRoleWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userRoleWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userRoleWhereList[i].RoleId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `role_id` = @roleIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `role_id` = @roleIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@roleIdWhere" + i, userRoleWhereList[i].RoleId);
                                    }
                                    if (!String.IsNullOrEmpty(userRoleWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userRoleWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userRoleWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userRoleWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userRoleWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userRoleWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userRoleWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userRoleWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userRoleList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserRoleModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserRoleModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserRoleModel>();
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

        public List<UserRoleModel> Delete(string connectionString, UserRoleModel userRole)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRoleModel> response = new List<UserRoleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM ur_user_roles  ";
                if (userRole != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(userRole.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userRole.Id);
                    }
                    if (!String.IsNullOrEmpty(userRole.RoleId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `role_id` = @roleId");
                        else
                            sql = String.Concat(sql, " `role_id` = @roleId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@roleId", userRole.RoleId);
                    }
                    if (!String.IsNullOrEmpty(userRole.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userRole.UserId);
                    }
                    if (!String.IsNullOrEmpty(userRole.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userRole.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userRole.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userRole.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userRole.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userRole.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userRole);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserRoleModel>();
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

        public List<UserRoleModel> Delete(string connectionString, List<UserRoleModel> userRole)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRoleModel> response = new List<UserRoleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userRole.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM ur_user_roles  ";
                                if (userRole[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(userRole[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userRole[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userRole[i].RoleId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `role_id` = @roleId" + i);
                                        else
                                            sql = String.Concat(sql, " `role_id` = @roleId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@roleId" + i, userRole[i].RoleId);
                                    }
                                    if (!String.IsNullOrEmpty(userRole[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userRole[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userRole[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userRole[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userRole[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userRole[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userRole[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userRole[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userRole[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserRoleModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserRoleModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserRoleModel>();
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
