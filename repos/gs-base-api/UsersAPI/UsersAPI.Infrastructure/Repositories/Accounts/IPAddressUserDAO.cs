using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts
{
    class IpAddressUserDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, IpAddressUserModel> ReadD(string connectionString, IpAddressUserModel ipAddressUser, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, IpAddressUserModel> response = new Dictionary<string, IpAddressUserModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `ip_address`, `user_id`, `modified_by`, `created_at`, `updated_at`  FROM ul_ip_address_users ";
                {
                    if (ipAddressUser != null)
                    {
                        if (!String.IsNullOrEmpty(ipAddressUser.Id))
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
                            cmd.Parameters.AddWithValue("@id", ipAddressUser.Id);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUser.IpAddress))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `ip_address` = @ipAddress ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `ip_address` = @ipAddress ");
                            }
                            cmd.Parameters.AddWithValue("@ipAddress", ipAddressUser.IpAddress);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUser.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", ipAddressUser.UserId);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUser.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", ipAddressUser.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUser.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", ipAddressUser.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUser.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", ipAddressUser.UpdatedAt);
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
                                    ipAddressUser.Id = reader.GetValue(0).ToString();
                                else
                                    ipAddressUser.Id = null;
                                if (!reader.IsDBNull(1))
                                    ipAddressUser.IpAddress = reader.GetValue(1).ToString();
                                else
                                    ipAddressUser.IpAddress = null;
                                if (!reader.IsDBNull(2))
                                    ipAddressUser.UserId = reader.GetValue(2).ToString();
                                else
                                    ipAddressUser.UserId = null;
                                if (!reader.IsDBNull(3))
                                    ipAddressUser.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    ipAddressUser.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    ipAddressUser.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    ipAddressUser.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    ipAddressUser.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    ipAddressUser.UpdatedAt = null;
                                response.Add(ipAddressUser.Id, ipAddressUser);
                                ipAddressUser = new IpAddressUserModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, IpAddressUserModel>();
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
                response = new Dictionary<string, IpAddressUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, IpAddressUserModel> ReadD(string connectionString, List<IpAddressUserModel> ipAddressUserWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, IpAddressUserModel> response = new Dictionary<string, IpAddressUserModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `ip_address`, `user_id`, `modified_by`, `created_at`, `updated_at`  FROM ul_ip_address_users ";
                if (ipAddressUserWhere != null && ipAddressUserWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < ipAddressUserWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(ipAddressUserWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, ipAddressUserWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUserWhere[i].IpAddress))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `ip_address` = @ipAddress" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ipAddress" + i, ipAddressUserWhere[i].IpAddress);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUserWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, ipAddressUserWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUserWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, ipAddressUserWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUserWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, ipAddressUserWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUserWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, ipAddressUserWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != ipAddressUserWhere.Count - 1)
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
                            IpAddressUserModel ipAddressUser = new IpAddressUserModel(); if (!reader.IsDBNull(0))
                                ipAddressUser.Id = reader.GetValue(0).ToString();
                            else
                                ipAddressUser.Id = null;
                            if (!reader.IsDBNull(1))
                                ipAddressUser.IpAddress = reader.GetValue(1).ToString();
                            else
                                ipAddressUser.IpAddress = null;
                            if (!reader.IsDBNull(2))
                                ipAddressUser.UserId = reader.GetValue(2).ToString();
                            else
                                ipAddressUser.UserId = null;
                            if (!reader.IsDBNull(3))
                                ipAddressUser.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                ipAddressUser.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                ipAddressUser.CreatedAt = reader.GetValue(4).ToString();
                            else
                                ipAddressUser.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                ipAddressUser.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                ipAddressUser.UpdatedAt = null;
                            response.Add(ipAddressUser.Id, ipAddressUser);
                            ipAddressUser = new IpAddressUserModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, IpAddressUserModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, IpAddressUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<IpAddressUserModel> Read(string connectionString, IpAddressUserModel ipAddressUser, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<IpAddressUserModel> response = new List<IpAddressUserModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `ip_address`, `user_id`, `modified_by`, `created_at`, `updated_at`  FROM ul_ip_address_users ";
                {
                    if (ipAddressUser != null)
                    {
                        if (!String.IsNullOrEmpty(ipAddressUser.Id))
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
                            cmd.Parameters.AddWithValue("@id", ipAddressUser.Id);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUser.IpAddress))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `ip_address` = @ipAddress ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `ip_address` = @ipAddress ");
                            }
                            cmd.Parameters.AddWithValue("@ipAddress", ipAddressUser.IpAddress);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUser.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", ipAddressUser.UserId);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUser.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", ipAddressUser.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUser.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", ipAddressUser.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUser.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", ipAddressUser.UpdatedAt);
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
                                    ipAddressUser.Id = reader.GetValue(0).ToString();
                                else
                                    ipAddressUser.Id = null;
                                if (!reader.IsDBNull(1))
                                    ipAddressUser.IpAddress = reader.GetValue(1).ToString();
                                else
                                    ipAddressUser.IpAddress = null;
                                if (!reader.IsDBNull(2))
                                    ipAddressUser.UserId = reader.GetValue(2).ToString();
                                else
                                    ipAddressUser.UserId = null;
                                if (!reader.IsDBNull(3))
                                    ipAddressUser.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    ipAddressUser.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    ipAddressUser.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    ipAddressUser.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    ipAddressUser.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    ipAddressUser.UpdatedAt = null;
                                response.Add(ipAddressUser);
                                ipAddressUser = new IpAddressUserModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<IpAddressUserModel>();
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
                response = new List<IpAddressUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<IpAddressUserModel> Read(string connectionString, List<IpAddressUserModel> ipAddressUserWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<IpAddressUserModel> response = new List<IpAddressUserModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `ip_address`, `user_id`, `modified_by`, `created_at`, `updated_at`  FROM ul_ip_address_users ";
                if (ipAddressUserWhere != null && ipAddressUserWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < ipAddressUserWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(ipAddressUserWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, ipAddressUserWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUserWhere[i].IpAddress))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `ip_address` = @ipAddress" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ipAddress" + i, ipAddressUserWhere[i].IpAddress);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUserWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, ipAddressUserWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUserWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, ipAddressUserWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUserWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, ipAddressUserWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(ipAddressUserWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, ipAddressUserWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != ipAddressUserWhere.Count - 1)
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
                            IpAddressUserModel ipAddressUser = new IpAddressUserModel(); if (!reader.IsDBNull(0))
                                ipAddressUser.Id = reader.GetValue(0).ToString();
                            else
                                ipAddressUser.Id = null;
                            if (!reader.IsDBNull(1))
                                ipAddressUser.IpAddress = reader.GetValue(1).ToString();
                            else
                                ipAddressUser.IpAddress = null;
                            if (!reader.IsDBNull(2))
                                ipAddressUser.UserId = reader.GetValue(2).ToString();
                            else
                                ipAddressUser.UserId = null;
                            if (!reader.IsDBNull(3))
                                ipAddressUser.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                ipAddressUser.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                ipAddressUser.CreatedAt = reader.GetValue(4).ToString();
                            else
                                ipAddressUser.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                ipAddressUser.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                ipAddressUser.UpdatedAt = null;
                            response.Add(ipAddressUser);
                            ipAddressUser = new IpAddressUserModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<IpAddressUserModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<IpAddressUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<IpAddressUserModel> Create(string connectionString, IpAddressUserModel ipAddressUser)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<IpAddressUserModel> response = new List<IpAddressUserModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO ul_ip_address_users (";
                string values = "(";
                if (!String.IsNullOrEmpty(ipAddressUser.Id))
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
                    cmd.Parameters.AddWithValue("@id", ipAddressUser.Id);
                }
                if (!String.IsNullOrEmpty(ipAddressUser.IpAddress))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `ip_address` ");
                        values = String.Concat(values, ", @ipAddress");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `ip_address` ");
                        values = String.Concat(values, " @ipAddress");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ipAddress", ipAddressUser.IpAddress);
                }
                if (!String.IsNullOrEmpty(ipAddressUser.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", ipAddressUser.UserId);
                }
                if (!String.IsNullOrEmpty(ipAddressUser.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", ipAddressUser.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(ipAddressUser.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", ipAddressUser.CreatedAt);
                }
                if (!String.IsNullOrEmpty(ipAddressUser.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", ipAddressUser.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                ipAddressUser.Id = cmd.LastInsertedId.ToString();
                response.Add(ipAddressUser);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<IpAddressUserModel>();
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
        public List<IpAddressUserModel> Create(string connectionString, List<IpAddressUserModel> ipAddressUserList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<IpAddressUserModel> response = new List<IpAddressUserModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < ipAddressUserList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO ul_ip_address_users (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(ipAddressUserList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, ipAddressUserList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(ipAddressUserList[i].IpAddress))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `ip_address` ");
                                        values = String.Concat(values, ", @ipAddress" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `ip_address` ");
                                        values = String.Concat(values, "@ipAddress" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ipAddress" + i, ipAddressUserList[i].IpAddress);
                                }
                                if (!String.IsNullOrEmpty(ipAddressUserList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, ipAddressUserList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(ipAddressUserList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, ipAddressUserList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(ipAddressUserList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, ipAddressUserList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(ipAddressUserList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, ipAddressUserList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(ipAddressUserList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<IpAddressUserModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<IpAddressUserModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<IpAddressUserModel>();
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

        public List<IpAddressUserModel> Update(string connectionString, IpAddressUserModel ipAddressUser, IpAddressUserModel ipAddressUserWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<IpAddressUserModel> response = new List<IpAddressUserModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE ul_ip_address_users SET ";
                if (!String.IsNullOrEmpty(ipAddressUser.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", ipAddressUser.Id);
                }
                if (!String.IsNullOrEmpty(ipAddressUser.IpAddress))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `ip_address` = @ipAddress");
                    else
                        sql = String.Concat(sql, " `ip_address` = @ipAddress");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ipAddress", ipAddressUser.IpAddress);
                }
                if (!String.IsNullOrEmpty(ipAddressUser.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", ipAddressUser.UserId);
                }
                if (!String.IsNullOrEmpty(ipAddressUser.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", ipAddressUser.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(ipAddressUser.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", ipAddressUser.CreatedAt);
                }
                if (!String.IsNullOrEmpty(ipAddressUser.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", ipAddressUser.UpdatedAt);
                }
                if (ipAddressUserWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(ipAddressUserWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", ipAddressUserWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(ipAddressUserWhere.IpAddress))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `ip_address` = @ipAddressWhere");
                        else
                            sql = String.Concat(sql, " `ip_address` = @ipAddressWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ipAddressWhere", ipAddressUserWhere.IpAddress);
                    }
                    if (!String.IsNullOrEmpty(ipAddressUserWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", ipAddressUserWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(ipAddressUserWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", ipAddressUserWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(ipAddressUserWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", ipAddressUserWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(ipAddressUserWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", ipAddressUserWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(ipAddressUser);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<IpAddressUserModel>();
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

        public List<IpAddressUserModel> Update(string connectionString, List<IpAddressUserModel> ipAddressUserList, List<IpAddressUserModel> ipAddressUserWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<IpAddressUserModel> response = new List<IpAddressUserModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < ipAddressUserList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE ul_ip_address_users SET ";
                                if (!String.IsNullOrEmpty(ipAddressUserList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, ipAddressUserList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(ipAddressUserList[i].IpAddress))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `ip_address` = @ipAddress" + i);
                                    else
                                        sql = String.Concat(sql, " `ip_address` = @ipAddress" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ipAddress" + i, ipAddressUserList[i].IpAddress);
                                }
                                if (!String.IsNullOrEmpty(ipAddressUserList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, ipAddressUserList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(ipAddressUserList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, ipAddressUserList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(ipAddressUserList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, ipAddressUserList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(ipAddressUserList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, ipAddressUserList[i].UpdatedAt);
                                }
                                if (ipAddressUserWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(ipAddressUserWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, ipAddressUserWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(ipAddressUserWhereList[i].IpAddress))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `ip_address` = @ipAddressWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `ip_address` = @ipAddressWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ipAddressWhere" + i, ipAddressUserWhereList[i].IpAddress);
                                    }
                                    if (!String.IsNullOrEmpty(ipAddressUserWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, ipAddressUserWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(ipAddressUserWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, ipAddressUserWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(ipAddressUserWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, ipAddressUserWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(ipAddressUserWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, ipAddressUserWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(ipAddressUserList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<IpAddressUserModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<IpAddressUserModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<IpAddressUserModel>();
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

        public List<IpAddressUserModel> Delete(string connectionString, IpAddressUserModel ipAddressUser)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<IpAddressUserModel> response = new List<IpAddressUserModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM ul_ip_address_users  ";
                if (ipAddressUser != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(ipAddressUser.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", ipAddressUser.Id);
                    }
                    if (!String.IsNullOrEmpty(ipAddressUser.IpAddress))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `ip_address` = @ipAddress");
                        else
                            sql = String.Concat(sql, " `ip_address` = @ipAddress");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ipAddress", ipAddressUser.IpAddress);
                    }
                    if (!String.IsNullOrEmpty(ipAddressUser.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", ipAddressUser.UserId);
                    }
                    if (!String.IsNullOrEmpty(ipAddressUser.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", ipAddressUser.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(ipAddressUser.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", ipAddressUser.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(ipAddressUser.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", ipAddressUser.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(ipAddressUser);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<IpAddressUserModel>();
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

        public List<IpAddressUserModel> Delete(string connectionString, List<IpAddressUserModel> ipAddressUser)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<IpAddressUserModel> response = new List<IpAddressUserModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < ipAddressUser.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM ul_ip_address_users  ";
                                if (ipAddressUser[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(ipAddressUser[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, ipAddressUser[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(ipAddressUser[i].IpAddress))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `ip_address` = @ipAddress" + i);
                                        else
                                            sql = String.Concat(sql, " `ip_address` = @ipAddress" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ipAddress" + i, ipAddressUser[i].IpAddress);
                                    }
                                    if (!String.IsNullOrEmpty(ipAddressUser[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, ipAddressUser[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(ipAddressUser[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, ipAddressUser[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(ipAddressUser[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, ipAddressUser[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(ipAddressUser[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, ipAddressUser[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(ipAddressUser[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<IpAddressUserModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<IpAddressUserModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<IpAddressUserModel>();
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
