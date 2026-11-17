using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts
{
    class SessionDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, SessionModel> ReadD(string connectionString, SessionModel session, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SessionModel> response = new Dictionary<string, SessionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `token`, `ip_address`, `modified_by`, `created_at`, `updated_at`  FROM ul_sessions ";
                {
                    if (session != null)
                    {
                        if (!String.IsNullOrEmpty(session.Id))
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
                            cmd.Parameters.AddWithValue("@id", session.Id);
                        }
                        if (!String.IsNullOrEmpty(session.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", session.UserId);
                        }
                        if (!String.IsNullOrEmpty(session.Token))
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
                            cmd.Parameters.AddWithValue("@token", session.Token);
                        }
                        if (!String.IsNullOrEmpty(session.IpAddress))
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
                            cmd.Parameters.AddWithValue("@ipAddress", session.IpAddress);
                        }
                        if (!String.IsNullOrEmpty(session.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", session.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(session.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", session.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(session.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", session.UpdatedAt);
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
                                    session.Id = reader.GetValue(0).ToString();
                                else
                                    session.Id = null;
                                if (!reader.IsDBNull(1))
                                    session.UserId = reader.GetValue(1).ToString();
                                else
                                    session.UserId = null;
                                if (!reader.IsDBNull(2))
                                    session.Token = reader.GetValue(2).ToString();
                                else
                                    session.Token = null;
                                if (!reader.IsDBNull(3))
                                    session.IpAddress = reader.GetValue(3).ToString();
                                else
                                    session.IpAddress = null;
                                if (!reader.IsDBNull(4))
                                    session.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    session.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    session.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    session.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    session.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    session.UpdatedAt = null;
                                response.Add(session.Id, session);
                                session = new SessionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, SessionModel>();
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
                response = new Dictionary<string, SessionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, SessionModel> ReadD(string connectionString, List<SessionModel> sessionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SessionModel> response = new Dictionary<string, SessionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `token`, `ip_address`, `modified_by`, `created_at`, `updated_at`  FROM ul_sessions ";
                if (sessionWhere != null && sessionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < sessionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(sessionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, sessionWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(sessionWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, sessionWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(sessionWhere[i].Token))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `token` = @token" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@token" + i, sessionWhere[i].Token);
                        }
                        if (!String.IsNullOrEmpty(sessionWhere[i].IpAddress))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `ip_address` = @ipAddress" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ipAddress" + i, sessionWhere[i].IpAddress);
                        }
                        if (!String.IsNullOrEmpty(sessionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, sessionWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(sessionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, sessionWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(sessionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, sessionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != sessionWhere.Count - 1)
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
                            SessionModel session = new SessionModel(); if (!reader.IsDBNull(0))
                                session.Id = reader.GetValue(0).ToString();
                            else
                                session.Id = null;
                            if (!reader.IsDBNull(1))
                                session.UserId = reader.GetValue(1).ToString();
                            else
                                session.UserId = null;
                            if (!reader.IsDBNull(2))
                                session.Token = reader.GetValue(2).ToString();
                            else
                                session.Token = null;
                            if (!reader.IsDBNull(3))
                                session.IpAddress = reader.GetValue(3).ToString();
                            else
                                session.IpAddress = null;
                            if (!reader.IsDBNull(4))
                                session.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                session.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                session.CreatedAt = reader.GetValue(5).ToString();
                            else
                                session.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                session.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                session.UpdatedAt = null;
                            response.Add(session.Id, session);
                            session = new SessionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, SessionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, SessionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SessionModel> Read(string connectionString, SessionModel session, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SessionModel> response = new List<SessionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `token`, `ip_address`, `modified_by`, `created_at`, `updated_at`  FROM ul_sessions ";
                {
                    if (session != null)
                    {
                        if (!String.IsNullOrEmpty(session.Id))
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
                            cmd.Parameters.AddWithValue("@id", session.Id);
                        }
                        if (!String.IsNullOrEmpty(session.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", session.UserId);
                        }
                        if (!String.IsNullOrEmpty(session.Token))
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
                            cmd.Parameters.AddWithValue("@token", session.Token);
                        }
                        if (!String.IsNullOrEmpty(session.IpAddress))
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
                            cmd.Parameters.AddWithValue("@ipAddress", session.IpAddress);
                        }
                        if (!String.IsNullOrEmpty(session.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", session.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(session.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", session.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(session.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", session.UpdatedAt);
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
                                    session.Id = reader.GetValue(0).ToString();
                                else
                                    session.Id = null;
                                if (!reader.IsDBNull(1))
                                    session.UserId = reader.GetValue(1).ToString();
                                else
                                    session.UserId = null;
                                if (!reader.IsDBNull(2))
                                    session.Token = reader.GetValue(2).ToString();
                                else
                                    session.Token = null;
                                if (!reader.IsDBNull(3))
                                    session.IpAddress = reader.GetValue(3).ToString();
                                else
                                    session.IpAddress = null;
                                if (!reader.IsDBNull(4))
                                    session.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    session.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    session.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    session.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    session.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    session.UpdatedAt = null;
                                response.Add(session);
                                session = new SessionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<SessionModel>();
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
                response = new List<SessionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SessionModel> Read(string connectionString, List<SessionModel> sessionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SessionModel> response = new List<SessionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `token`, `ip_address`, `modified_by`, `created_at`, `updated_at`  FROM ul_sessions ";
                if (sessionWhere != null && sessionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < sessionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(sessionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, sessionWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(sessionWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, sessionWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(sessionWhere[i].Token))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `token` = @token" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@token" + i, sessionWhere[i].Token);
                        }
                        if (!String.IsNullOrEmpty(sessionWhere[i].IpAddress))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `ip_address` = @ipAddress" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ipAddress" + i, sessionWhere[i].IpAddress);
                        }
                        if (!String.IsNullOrEmpty(sessionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, sessionWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(sessionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, sessionWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(sessionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, sessionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != sessionWhere.Count - 1)
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
                            SessionModel session = new SessionModel(); if (!reader.IsDBNull(0))
                                session.Id = reader.GetValue(0).ToString();
                            else
                                session.Id = null;
                            if (!reader.IsDBNull(1))
                                session.UserId = reader.GetValue(1).ToString();
                            else
                                session.UserId = null;
                            if (!reader.IsDBNull(2))
                                session.Token = reader.GetValue(2).ToString();
                            else
                                session.Token = null;
                            if (!reader.IsDBNull(3))
                                session.IpAddress = reader.GetValue(3).ToString();
                            else
                                session.IpAddress = null;
                            if (!reader.IsDBNull(4))
                                session.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                session.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                session.CreatedAt = reader.GetValue(5).ToString();
                            else
                                session.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                session.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                session.UpdatedAt = null;
                            response.Add(session);
                            session = new SessionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<SessionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SessionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SessionModel> Create(string connectionString, SessionModel session)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SessionModel> response = new List<SessionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO ul_sessions (";
                string values = "(";
                if (!String.IsNullOrEmpty(session.Id))
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
                    cmd.Parameters.AddWithValue("@id", session.Id);
                }
                if (!String.IsNullOrEmpty(session.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", session.UserId);
                }
                if (!String.IsNullOrEmpty(session.Token))
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
                    cmd.Parameters.AddWithValue("@token", session.Token);
                }
                if (!String.IsNullOrEmpty(session.IpAddress))
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
                    cmd.Parameters.AddWithValue("@ipAddress", session.IpAddress);
                }
                if (!String.IsNullOrEmpty(session.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", session.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(session.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", session.CreatedAt);
                }
                if (!String.IsNullOrEmpty(session.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", session.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                session.Id = cmd.LastInsertedId.ToString();
                response.Add(session);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SessionModel>();
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
        public List<SessionModel> Create(string connectionString, List<SessionModel> sessionList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SessionModel> response = new List<SessionModel>();
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
                        for (int i = 0; i < sessionList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO ul_sessions (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(sessionList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, sessionList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(sessionList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, sessionList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(sessionList[i].Token))
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
                                    cmd.Parameters.AddWithValue("@token" + i, sessionList[i].Token);
                                }
                                if (!String.IsNullOrEmpty(sessionList[i].IpAddress))
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
                                    cmd.Parameters.AddWithValue("@ipAddress" + i, sessionList[i].IpAddress);
                                }
                                if (!String.IsNullOrEmpty(sessionList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, sessionList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(sessionList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, sessionList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(sessionList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, sessionList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(sessionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<SessionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SessionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SessionModel>();
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

        public List<SessionModel> Update(string connectionString, SessionModel session, SessionModel sessionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SessionModel> response = new List<SessionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE ul_sessions SET ";
                if (!String.IsNullOrEmpty(session.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", session.Id);
                }
                if (!String.IsNullOrEmpty(session.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", session.UserId);
                }
                if (!String.IsNullOrEmpty(session.Token))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `token` = @token");
                    else
                        sql = String.Concat(sql, " `token` = @token");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@token", session.Token);
                }
                if (!String.IsNullOrEmpty(session.IpAddress))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `ip_address` = @ipAddress");
                    else
                        sql = String.Concat(sql, " `ip_address` = @ipAddress");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ipAddress", session.IpAddress);
                }
                if (!String.IsNullOrEmpty(session.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", session.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(session.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", session.CreatedAt);
                }
                if (!String.IsNullOrEmpty(session.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", session.UpdatedAt);
                }
                if (sessionWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(sessionWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", sessionWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(sessionWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", sessionWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(sessionWhere.Token))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `token` = @tokenWhere");
                        else
                            sql = String.Concat(sql, " `token` = @tokenWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@tokenWhere", sessionWhere.Token);
                    }
                    if (!String.IsNullOrEmpty(sessionWhere.IpAddress))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `ip_address` = @ipAddressWhere");
                        else
                            sql = String.Concat(sql, " `ip_address` = @ipAddressWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ipAddressWhere", sessionWhere.IpAddress);
                    }
                    if (!String.IsNullOrEmpty(sessionWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", sessionWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(sessionWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", sessionWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(sessionWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", sessionWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(session);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<SessionModel>();
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

        public List<SessionModel> Update(string connectionString, List<SessionModel> sessionList, List<SessionModel> sessionWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SessionModel> response = new List<SessionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < sessionList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE ul_sessions SET ";
                                if (!String.IsNullOrEmpty(sessionList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, sessionList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(sessionList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, sessionList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(sessionList[i].Token))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `token` = @token" + i);
                                    else
                                        sql = String.Concat(sql, " `token` = @token" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@token" + i, sessionList[i].Token);
                                }
                                if (!String.IsNullOrEmpty(sessionList[i].IpAddress))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `ip_address` = @ipAddress" + i);
                                    else
                                        sql = String.Concat(sql, " `ip_address` = @ipAddress" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ipAddress" + i, sessionList[i].IpAddress);
                                }
                                if (!String.IsNullOrEmpty(sessionList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, sessionList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(sessionList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, sessionList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(sessionList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, sessionList[i].UpdatedAt);
                                }
                                if (sessionWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(sessionWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, sessionWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(sessionWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, sessionWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(sessionWhereList[i].Token))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `token` = @tokenWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `token` = @tokenWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@tokenWhere" + i, sessionWhereList[i].Token);
                                    }
                                    if (!String.IsNullOrEmpty(sessionWhereList[i].IpAddress))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `ip_address` = @ipAddressWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `ip_address` = @ipAddressWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ipAddressWhere" + i, sessionWhereList[i].IpAddress);
                                    }
                                    if (!String.IsNullOrEmpty(sessionWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, sessionWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(sessionWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, sessionWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(sessionWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, sessionWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(sessionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<SessionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SessionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SessionModel>();
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

        public List<SessionModel> Delete(string connectionString, SessionModel session)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SessionModel> response = new List<SessionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM ul_sessions  ";
                if (session != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(session.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", session.Id);
                    }
                    if (!String.IsNullOrEmpty(session.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", session.UserId);
                    }
                    if (!String.IsNullOrEmpty(session.Token))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `token` = @token");
                        else
                            sql = String.Concat(sql, " `token` = @token");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@token", session.Token);
                    }
                    if (!String.IsNullOrEmpty(session.IpAddress))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `ip_address` = @ipAddress");
                        else
                            sql = String.Concat(sql, " `ip_address` = @ipAddress");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ipAddress", session.IpAddress);
                    }
                    if (!String.IsNullOrEmpty(session.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", session.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(session.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", session.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(session.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", session.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(session);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<SessionModel>();
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

        public List<SessionModel> Delete(string connectionString, List<SessionModel> session)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SessionModel> response = new List<SessionModel>();
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
                        for (int i = 0; i < session.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM ul_sessions  ";
                                if (session[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(session[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, session[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(session[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, session[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(session[i].Token))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `token` = @token" + i);
                                        else
                                            sql = String.Concat(sql, " `token` = @token" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@token" + i, session[i].Token);
                                    }
                                    if (!String.IsNullOrEmpty(session[i].IpAddress))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `ip_address` = @ipAddress" + i);
                                        else
                                            sql = String.Concat(sql, " `ip_address` = @ipAddress" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ipAddress" + i, session[i].IpAddress);
                                    }
                                    if (!String.IsNullOrEmpty(session[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, session[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(session[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, session[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(session[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, session[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(session[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<SessionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SessionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SessionModel>();
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
