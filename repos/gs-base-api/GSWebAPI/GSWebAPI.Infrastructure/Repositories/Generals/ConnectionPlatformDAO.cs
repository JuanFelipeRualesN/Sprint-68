using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Services.Generals;
using GSWebAPI.GSWebAPI.Domain.Models.General;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.Generals
{
    class ConnectionPlatformDAO
    {
        private MySqlConnection _connection;
        public List<ConnectionPlatformModel> Read(string connectionString, ConnectionPlatformModel connectionPlatform, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ConnectionPlatformModel> response = new List<ConnectionPlatformModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `user_id`, `platform`, `os_version`, `ip_address`, `modified_by`, `created_at`, `updated_at`  FROM gwp_connection_platforms ";
                {
                    if (connectionPlatform != null)
                    {
                        if (!string.IsNullOrEmpty(connectionPlatform.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", connectionPlatform.UserId);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatform.Platform))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `platform` = @platform ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `platform` = @platform ");
                            }
                            cmd.Parameters.AddWithValue("@platform", connectionPlatform.Platform);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatform.OsVersion))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `os_version` = @osVersion ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `os_version` = @osVersion ");
                            }
                            cmd.Parameters.AddWithValue("@osVersion", connectionPlatform.OsVersion);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatform.IpAddress))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `ip_address` = @ipAddress ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `ip_address` = @ipAddress ");
                            }
                            cmd.Parameters.AddWithValue("@ipAddress", connectionPlatform.IpAddress);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatform.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", connectionPlatform.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatform.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", connectionPlatform.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatform.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", connectionPlatform.UpdatedAt);
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
                                    connectionPlatform.UserId = reader.GetValue(0).ToString();
                                else
                                    connectionPlatform.UserId = null;
                                if (!reader.IsDBNull(1))
                                    connectionPlatform.Platform = reader.GetValue(1).ToString();
                                else
                                    connectionPlatform.Platform = null;
                                if (!reader.IsDBNull(2))
                                    connectionPlatform.OsVersion = reader.GetValue(2).ToString();
                                else
                                    connectionPlatform.OsVersion = null;
                                if (!reader.IsDBNull(3))
                                    connectionPlatform.IpAddress = reader.GetValue(3).ToString();
                                else
                                    connectionPlatform.IpAddress = null;
                                if (!reader.IsDBNull(4))
                                    connectionPlatform.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    connectionPlatform.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    connectionPlatform.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    connectionPlatform.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    connectionPlatform.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    connectionPlatform.UpdatedAt = null;
                                response.Add(connectionPlatform);
                                connectionPlatform = new ConnectionPlatformModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ConnectionPlatformModel>();
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
                response = new List<ConnectionPlatformModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ConnectionPlatformModel> Read(string connectionString, ConnectionPlatformModel connectionPlatform, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ConnectionPlatformModel> response = new List<ConnectionPlatformModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `user_id`, `platform`, `os_version`, `ip_address`, `modified_by`, `created_at`, `updated_at`  FROM gwp_connection_platforms ";
                {
                    if (connectionPlatform != null)
                    {
                        if (!string.IsNullOrEmpty(connectionPlatform.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", connectionPlatform.UserId);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatform.Platform))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `platform` = @platform ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `platform` = @platform ");
                            }
                            cmd.Parameters.AddWithValue("@platform", connectionPlatform.Platform);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatform.OsVersion))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `os_version` = @osVersion ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `os_version` = @osVersion ");
                            }
                            cmd.Parameters.AddWithValue("@osVersion", connectionPlatform.OsVersion);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatform.IpAddress))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `ip_address` = @ipAddress ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `ip_address` = @ipAddress ");
                            }
                            cmd.Parameters.AddWithValue("@ipAddress", connectionPlatform.IpAddress);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatform.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", connectionPlatform.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatform.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", connectionPlatform.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatform.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", connectionPlatform.UpdatedAt);
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
                                    connectionPlatform.UserId = reader.GetValue(0).ToString();
                                else
                                    connectionPlatform.UserId = null;
                                if (!reader.IsDBNull(1))
                                    connectionPlatform.Platform = reader.GetValue(1).ToString();
                                else
                                    connectionPlatform.Platform = null;
                                if (!reader.IsDBNull(2))
                                    connectionPlatform.OsVersion = reader.GetValue(2).ToString();
                                else
                                    connectionPlatform.OsVersion = null;
                                if (!reader.IsDBNull(3))
                                    connectionPlatform.IpAddress = reader.GetValue(3).ToString();
                                else
                                    connectionPlatform.IpAddress = null;
                                if (!reader.IsDBNull(4))
                                    connectionPlatform.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    connectionPlatform.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    connectionPlatform.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    connectionPlatform.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    connectionPlatform.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    connectionPlatform.UpdatedAt = null;
                                response.Add(connectionPlatform);
                                connectionPlatform = new ConnectionPlatformModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ConnectionPlatformModel>();
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
                response = new List<ConnectionPlatformModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ConnectionPlatformModel> Read(string connectionString, List<ConnectionPlatformModel> connectionPlatformWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ConnectionPlatformModel> response = new List<ConnectionPlatformModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `user_id`, `platform`, `os_version`, `ip_address`, `modified_by`, `created_at`, `updated_at`  FROM gwp_connection_platforms ";
                if (connectionPlatformWhere != null && connectionPlatformWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < connectionPlatformWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(connectionPlatformWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, connectionPlatformWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatformWhere[i].Platform))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform` = @platform" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platform" + i, connectionPlatformWhere[i].Platform);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatformWhere[i].OsVersion))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `os_version` = @osVersion" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@osVersion" + i, connectionPlatformWhere[i].OsVersion);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatformWhere[i].IpAddress))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `ip_address` = @ipAddress" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ipAddress" + i, connectionPlatformWhere[i].IpAddress);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatformWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, connectionPlatformWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatformWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, connectionPlatformWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatformWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, connectionPlatformWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != connectionPlatformWhere.Count - 1)
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
                            ConnectionPlatformModel connectionPlatform = new ConnectionPlatformModel(); if (!reader.IsDBNull(0))
                                connectionPlatform.UserId = reader.GetValue(0).ToString();
                            else
                                connectionPlatform.UserId = null;
                            if (!reader.IsDBNull(1))
                                connectionPlatform.Platform = reader.GetValue(1).ToString();
                            else
                                connectionPlatform.Platform = null;
                            if (!reader.IsDBNull(2))
                                connectionPlatform.OsVersion = reader.GetValue(2).ToString();
                            else
                                connectionPlatform.OsVersion = null;
                            if (!reader.IsDBNull(3))
                                connectionPlatform.IpAddress = reader.GetValue(3).ToString();
                            else
                                connectionPlatform.IpAddress = null;
                            if (!reader.IsDBNull(4))
                                connectionPlatform.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                connectionPlatform.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                connectionPlatform.CreatedAt = reader.GetValue(5).ToString();
                            else
                                connectionPlatform.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                connectionPlatform.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                connectionPlatform.UpdatedAt = null;
                            response.Add(connectionPlatform);
                            connectionPlatform = new ConnectionPlatformModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<ConnectionPlatformModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ConnectionPlatformModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ConnectionPlatformModel> Read(string connectionString, List<ConnectionPlatformModel> connectionPlatformWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ConnectionPlatformModel> response = new List<ConnectionPlatformModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `user_id`, `platform`, `os_version`, `ip_address`, `modified_by`, `created_at`, `updated_at`  FROM gwp_connection_platforms ";
                if (connectionPlatformWhere != null && connectionPlatformWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < connectionPlatformWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(connectionPlatformWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, connectionPlatformWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatformWhere[i].Platform))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform` = @platform" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platform" + i, connectionPlatformWhere[i].Platform);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatformWhere[i].OsVersion))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `os_version` = @osVersion" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@osVersion" + i, connectionPlatformWhere[i].OsVersion);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatformWhere[i].IpAddress))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `ip_address` = @ipAddress" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ipAddress" + i, connectionPlatformWhere[i].IpAddress);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatformWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, connectionPlatformWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatformWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, connectionPlatformWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(connectionPlatformWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, connectionPlatformWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != connectionPlatformWhere.Count - 1)
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
                            ConnectionPlatformModel connectionPlatform = new ConnectionPlatformModel(); if (!reader.IsDBNull(0))
                                connectionPlatform.UserId = reader.GetValue(0).ToString();
                            else
                                connectionPlatform.UserId = null;
                            if (!reader.IsDBNull(1))
                                connectionPlatform.Platform = reader.GetValue(1).ToString();
                            else
                                connectionPlatform.Platform = null;
                            if (!reader.IsDBNull(2))
                                connectionPlatform.OsVersion = reader.GetValue(2).ToString();
                            else
                                connectionPlatform.OsVersion = null;
                            if (!reader.IsDBNull(3))
                                connectionPlatform.IpAddress = reader.GetValue(3).ToString();
                            else
                                connectionPlatform.IpAddress = null;
                            if (!reader.IsDBNull(4))
                                connectionPlatform.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                connectionPlatform.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                connectionPlatform.CreatedAt = reader.GetValue(5).ToString();
                            else
                                connectionPlatform.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                connectionPlatform.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                connectionPlatform.UpdatedAt = null;
                            response.Add(connectionPlatform);
                            connectionPlatform = new ConnectionPlatformModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<ConnectionPlatformModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ConnectionPlatformModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ConnectionPlatformModel> Create(string connectionString, ConnectionPlatformModel connectionPlatform)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ConnectionPlatformModel> response = new List<ConnectionPlatformModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO gwp_connection_platforms (";
                string values = "(";
                if (!string.IsNullOrEmpty(connectionPlatform.UserId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `user_id` ");
                        values = string.Concat(values, ", @userId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `user_id` ");
                        values = string.Concat(values, " @userId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", connectionPlatform.UserId);
                }
                if (!string.IsNullOrEmpty(connectionPlatform.Platform))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `platform` ");
                        values = string.Concat(values, ", @platform");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `platform` ");
                        values = string.Concat(values, " @platform");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@platform", connectionPlatform.Platform);
                }
                if (!string.IsNullOrEmpty(connectionPlatform.OsVersion))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `os_version` ");
                        values = string.Concat(values, ", @osVersion");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `os_version` ");
                        values = string.Concat(values, " @osVersion");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@osVersion", connectionPlatform.OsVersion);
                }
                if (!string.IsNullOrEmpty(connectionPlatform.IpAddress))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `ip_address` ");
                        values = string.Concat(values, ", @ipAddress");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `ip_address` ");
                        values = string.Concat(values, " @ipAddress");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ipAddress", connectionPlatform.IpAddress);
                }
                if (!string.IsNullOrEmpty(connectionPlatform.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", connectionPlatform.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(connectionPlatform.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", connectionPlatform.CreatedAt);
                }
                if (!string.IsNullOrEmpty(connectionPlatform.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", connectionPlatform.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                response.Add(connectionPlatform);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ConnectionPlatformModel>();
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
        public List<ConnectionPlatformModel> Create(string connectionString, List<ConnectionPlatformModel> connectionPlatformList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ConnectionPlatformModel> response = new List<ConnectionPlatformModel>();
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
                        for (int i = 0; i < connectionPlatformList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO gwp_connection_platforms (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(connectionPlatformList[i].UserId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `user_id` ");
                                        values = string.Concat(values, ", @userId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `user_id` ");
                                        values = string.Concat(values, "@userId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, connectionPlatformList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(connectionPlatformList[i].Platform))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `platform` ");
                                        values = string.Concat(values, ", @platform" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `platform` ");
                                        values = string.Concat(values, "@platform" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@platform" + i, connectionPlatformList[i].Platform);
                                }
                                if (!string.IsNullOrEmpty(connectionPlatformList[i].OsVersion))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `os_version` ");
                                        values = string.Concat(values, ", @osVersion" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `os_version` ");
                                        values = string.Concat(values, "@osVersion" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@osVersion" + i, connectionPlatformList[i].OsVersion);
                                }
                                if (!string.IsNullOrEmpty(connectionPlatformList[i].IpAddress))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `ip_address` ");
                                        values = string.Concat(values, ", @ipAddress" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `ip_address` ");
                                        values = string.Concat(values, "@ipAddress" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ipAddress" + i, connectionPlatformList[i].IpAddress);
                                }
                                if (!string.IsNullOrEmpty(connectionPlatformList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, connectionPlatformList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(connectionPlatformList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, connectionPlatformList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(connectionPlatformList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, connectionPlatformList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(connectionPlatformList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<ConnectionPlatformModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ConnectionPlatformModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ConnectionPlatformModel>();
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

        public List<ConnectionPlatformModel> Update(string connectionString, ConnectionPlatformModel connectionPlatform, ConnectionPlatformModel connectionPlatformWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ConnectionPlatformModel> response = new List<ConnectionPlatformModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE gwp_connection_platforms SET ";
                if (!string.IsNullOrEmpty(connectionPlatform.UserId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = string.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", connectionPlatform.UserId);
                }
                if (!string.IsNullOrEmpty(connectionPlatform.Platform))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `platform` = @platform");
                    else
                        sql = string.Concat(sql, " `platform` = @platform");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@platform", connectionPlatform.Platform);
                }
                if (!string.IsNullOrEmpty(connectionPlatform.OsVersion))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `os_version` = @osVersion");
                    else
                        sql = string.Concat(sql, " `os_version` = @osVersion");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@osVersion", connectionPlatform.OsVersion);
                }
                if (!string.IsNullOrEmpty(connectionPlatform.IpAddress))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `ip_address` = @ipAddress");
                    else
                        sql = string.Concat(sql, " `ip_address` = @ipAddress");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ipAddress", connectionPlatform.IpAddress);
                }
                if (!string.IsNullOrEmpty(connectionPlatform.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", connectionPlatform.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(connectionPlatform.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", connectionPlatform.CreatedAt);
                }
                if (!string.IsNullOrEmpty(connectionPlatform.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", connectionPlatform.UpdatedAt);
                }
                if (connectionPlatformWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(connectionPlatformWhere.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = string.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", connectionPlatformWhere.UserId);
                    }
                    if (!string.IsNullOrEmpty(connectionPlatformWhere.Platform))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform` = @platformWhere");
                        else
                            sql = string.Concat(sql, " `platform` = @platformWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformWhere", connectionPlatformWhere.Platform);
                    }
                    if (!string.IsNullOrEmpty(connectionPlatformWhere.OsVersion))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `os_version` = @osVersionWhere");
                        else
                            sql = string.Concat(sql, " `os_version` = @osVersionWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@osVersionWhere", connectionPlatformWhere.OsVersion);
                    }
                    if (!string.IsNullOrEmpty(connectionPlatformWhere.IpAddress))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `ip_address` = @ipAddressWhere");
                        else
                            sql = string.Concat(sql, " `ip_address` = @ipAddressWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ipAddressWhere", connectionPlatformWhere.IpAddress);
                    }
                    if (!string.IsNullOrEmpty(connectionPlatformWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", connectionPlatformWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(connectionPlatformWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", connectionPlatformWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(connectionPlatformWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", connectionPlatformWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(connectionPlatform);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ConnectionPlatformModel>();
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

        public List<ConnectionPlatformModel> Update(string connectionString, List<ConnectionPlatformModel> connectionPlatformList, List<ConnectionPlatformModel> connectionPlatformWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ConnectionPlatformModel> response = new List<ConnectionPlatformModel>();
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
                        for (int i = 0; i < connectionPlatformList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE gwp_connection_platforms SET ";
                                if (!string.IsNullOrEmpty(connectionPlatformList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = string.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, connectionPlatformList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(connectionPlatformList[i].Platform))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `platform` = @platform" + i);
                                    else
                                        sql = string.Concat(sql, " `platform` = @platform" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@platform" + i, connectionPlatformList[i].Platform);
                                }
                                if (!string.IsNullOrEmpty(connectionPlatformList[i].OsVersion))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `os_version` = @osVersion" + i);
                                    else
                                        sql = string.Concat(sql, " `os_version` = @osVersion" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@osVersion" + i, connectionPlatformList[i].OsVersion);
                                }
                                if (!string.IsNullOrEmpty(connectionPlatformList[i].IpAddress))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `ip_address` = @ipAddress" + i);
                                    else
                                        sql = string.Concat(sql, " `ip_address` = @ipAddress" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ipAddress" + i, connectionPlatformList[i].IpAddress);
                                }
                                if (!string.IsNullOrEmpty(connectionPlatformList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, connectionPlatformList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(connectionPlatformList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, connectionPlatformList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(connectionPlatformList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, connectionPlatformList[i].UpdatedAt);
                                }
                                if (connectionPlatformWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(connectionPlatformWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, connectionPlatformWhereList[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(connectionPlatformWhereList[i].Platform))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform` = @platformWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `platform` = @platformWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformWhere" + i, connectionPlatformWhereList[i].Platform);
                                    }
                                    if (!string.IsNullOrEmpty(connectionPlatformWhereList[i].OsVersion))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `os_version` = @osVersionWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `os_version` = @osVersionWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@osVersionWhere" + i, connectionPlatformWhereList[i].OsVersion);
                                    }
                                    if (!string.IsNullOrEmpty(connectionPlatformWhereList[i].IpAddress))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `ip_address` = @ipAddressWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `ip_address` = @ipAddressWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ipAddressWhere" + i, connectionPlatformWhereList[i].IpAddress);
                                    }
                                    if (!string.IsNullOrEmpty(connectionPlatformWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, connectionPlatformWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(connectionPlatformWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, connectionPlatformWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(connectionPlatformWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, connectionPlatformWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(connectionPlatformList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ConnectionPlatformModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ConnectionPlatformModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ConnectionPlatformModel>();
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

        public List<ConnectionPlatformModel> Delete(string connectionString, ConnectionPlatformModel connectionPlatform)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ConnectionPlatformModel> response = new List<ConnectionPlatformModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM gwp_connection_platforms  ";
                if (connectionPlatform != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(connectionPlatform.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = string.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", connectionPlatform.UserId);
                    }
                    if (!string.IsNullOrEmpty(connectionPlatform.Platform))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform` = @platform");
                        else
                            sql = string.Concat(sql, " `platform` = @platform");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platform", connectionPlatform.Platform);
                    }
                    if (!string.IsNullOrEmpty(connectionPlatform.OsVersion))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `os_version` = @osVersion");
                        else
                            sql = string.Concat(sql, " `os_version` = @osVersion");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@osVersion", connectionPlatform.OsVersion);
                    }
                    if (!string.IsNullOrEmpty(connectionPlatform.IpAddress))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `ip_address` = @ipAddress");
                        else
                            sql = string.Concat(sql, " `ip_address` = @ipAddress");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ipAddress", connectionPlatform.IpAddress);
                    }
                    if (!string.IsNullOrEmpty(connectionPlatform.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", connectionPlatform.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(connectionPlatform.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", connectionPlatform.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(connectionPlatform.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", connectionPlatform.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(connectionPlatform);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ConnectionPlatformModel>();
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

        public List<ConnectionPlatformModel> Delete(string connectionString, List<ConnectionPlatformModel> connectionPlatform)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ConnectionPlatformModel> response = new List<ConnectionPlatformModel>();
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
                        for (int i = 0; i < connectionPlatform.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM gwp_connection_platforms  ";
                                if (connectionPlatform[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(connectionPlatform[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, connectionPlatform[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(connectionPlatform[i].Platform))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform` = @platform" + i);
                                        else
                                            sql = string.Concat(sql, " `platform` = @platform" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platform" + i, connectionPlatform[i].Platform);
                                    }
                                    if (!string.IsNullOrEmpty(connectionPlatform[i].OsVersion))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `os_version` = @osVersion" + i);
                                        else
                                            sql = string.Concat(sql, " `os_version` = @osVersion" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@osVersion" + i, connectionPlatform[i].OsVersion);
                                    }
                                    if (!string.IsNullOrEmpty(connectionPlatform[i].IpAddress))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `ip_address` = @ipAddress" + i);
                                        else
                                            sql = string.Concat(sql, " `ip_address` = @ipAddress" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ipAddress" + i, connectionPlatform[i].IpAddress);
                                    }
                                    if (!string.IsNullOrEmpty(connectionPlatform[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, connectionPlatform[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(connectionPlatform[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, connectionPlatform[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(connectionPlatform[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, connectionPlatform[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(connectionPlatform[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ConnectionPlatformModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ConnectionPlatformModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ConnectionPlatformModel>();
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
