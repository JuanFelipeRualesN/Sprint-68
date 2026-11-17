using System;
using System.Collections.Generic;
using MySqlConnector;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;

namespace SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools
{
    class PlatformDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, PlatformModel> ReadD(string connectionString, PlatformModel platform, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlatformModel> response = new Dictionary<string, PlatformModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `code`, `name`, `description`, `modified_by`, `created_at`, `updated_at`  FROM sch_platforms ";
                {
                    if (platform != null)
                    {
                        if (!string.IsNullOrEmpty(platform.Id))
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
                            cmd.Parameters.AddWithValue("@id", platform.Id);
                        }
                        if (!string.IsNullOrEmpty(platform.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", platform.Code);
                        }
                        if (!string.IsNullOrEmpty(platform.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", platform.Name);
                        }
                        if (!string.IsNullOrEmpty(platform.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", platform.Description);
                        }
                        if (!string.IsNullOrEmpty(platform.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", platform.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(platform.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", platform.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(platform.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", platform.UpdatedAt);
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
                                    platform.Id = reader.GetValue(0).ToString();
                                else
                                    platform.Id = null;
                                if (!reader.IsDBNull(1))
                                    platform.Code = reader.GetValue(1).ToString();
                                else
                                    platform.Code = null;
                                if (!reader.IsDBNull(2))
                                    platform.Name = reader.GetValue(2).ToString();
                                else
                                    platform.Name = null;
                                if (!reader.IsDBNull(3))
                                    platform.Description = reader.GetValue(3).ToString();
                                else
                                    platform.Description = null;
                                if (!reader.IsDBNull(4))
                                    platform.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    platform.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    platform.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    platform.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    platform.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    platform.UpdatedAt = null;
                                response.Add(platform.Id, platform);
                                platform = new PlatformModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PlatformModel>();
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
                response = new Dictionary<string, PlatformModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlatformModel> ReadD(string connectionString, PlatformModel platform, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlatformModel> response = new Dictionary<string, PlatformModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `code`, `name`, `description`, `modified_by`, `created_at`, `updated_at`  FROM sch_platforms ";
                {
                    if (platform != null)
                    {
                        if (!string.IsNullOrEmpty(platform.Id))
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
                            cmd.Parameters.AddWithValue("@id", platform.Id);
                        }
                        if (!string.IsNullOrEmpty(platform.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", platform.Code);
                        }
                        if (!string.IsNullOrEmpty(platform.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", platform.Name);
                        }
                        if (!string.IsNullOrEmpty(platform.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", platform.Description);
                        }
                        if (!string.IsNullOrEmpty(platform.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", platform.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(platform.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", platform.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(platform.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", platform.UpdatedAt);
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
                                    platform.Id = reader.GetValue(0).ToString();
                                else
                                    platform.Id = null;
                                if (!reader.IsDBNull(1))
                                    platform.Code = reader.GetValue(1).ToString();
                                else
                                    platform.Code = null;
                                if (!reader.IsDBNull(2))
                                    platform.Name = reader.GetValue(2).ToString();
                                else
                                    platform.Name = null;
                                if (!reader.IsDBNull(3))
                                    platform.Description = reader.GetValue(3).ToString();
                                else
                                    platform.Description = null;
                                if (!reader.IsDBNull(4))
                                    platform.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    platform.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    platform.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    platform.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    platform.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    platform.UpdatedAt = null;
                                response.Add(platform.Id, platform);
                                platform = new PlatformModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PlatformModel>();
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
                response = new Dictionary<string, PlatformModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlatformModel> ReadD(string connectionString, List<PlatformModel> platformWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlatformModel> response = new Dictionary<string, PlatformModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `code`, `name`, `description`, `modified_by`, `created_at`, `updated_at`  FROM sch_platforms ";
                if (platformWhere != null && platformWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < platformWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(platformWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, platformWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, platformWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, platformWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, platformWhere[i].Description);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, platformWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, platformWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, platformWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != platformWhere.Count - 1)
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
                            PlatformModel platform = new PlatformModel(); if (!reader.IsDBNull(0))
                                platform.Id = reader.GetValue(0).ToString();
                            else
                                platform.Id = null;
                            if (!reader.IsDBNull(1))
                                platform.Code = reader.GetValue(1).ToString();
                            else
                                platform.Code = null;
                            if (!reader.IsDBNull(2))
                                platform.Name = reader.GetValue(2).ToString();
                            else
                                platform.Name = null;
                            if (!reader.IsDBNull(3))
                                platform.Description = reader.GetValue(3).ToString();
                            else
                                platform.Description = null;
                            if (!reader.IsDBNull(4))
                                platform.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                platform.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                platform.CreatedAt = reader.GetValue(5).ToString();
                            else
                                platform.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                platform.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                platform.UpdatedAt = null;
                            response.Add(platform.Id, platform);
                            platform = new PlatformModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PlatformModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlatformModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlatformModel> ReadD(string connectionString, List<PlatformModel> platformWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlatformModel> response = new Dictionary<string, PlatformModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `code`, `name`, `description`, `modified_by`, `created_at`, `updated_at`  FROM sch_platforms ";
                if (platformWhere != null && platformWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < platformWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(platformWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, platformWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, platformWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, platformWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, platformWhere[i].Description);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, platformWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, platformWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, platformWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != platformWhere.Count - 1)
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
                            PlatformModel platform = new PlatformModel(); if (!reader.IsDBNull(0))
                                platform.Id = reader.GetValue(0).ToString();
                            else
                                platform.Id = null;
                            if (!reader.IsDBNull(1))
                                platform.Code = reader.GetValue(1).ToString();
                            else
                                platform.Code = null;
                            if (!reader.IsDBNull(2))
                                platform.Name = reader.GetValue(2).ToString();
                            else
                                platform.Name = null;
                            if (!reader.IsDBNull(3))
                                platform.Description = reader.GetValue(3).ToString();
                            else
                                platform.Description = null;
                            if (!reader.IsDBNull(4))
                                platform.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                platform.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                platform.CreatedAt = reader.GetValue(5).ToString();
                            else
                                platform.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                platform.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                platform.UpdatedAt = null;
                            response.Add(platform.Id, platform);
                            platform = new PlatformModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PlatformModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlatformModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlatformModel> Read(string connectionString, PlatformModel platform, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformModel> response = new List<PlatformModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `code`, `name`, `description`, `modified_by`, `created_at`, `updated_at`  FROM sch_platforms ";
                {
                    if (platform != null)
                    {
                        if (!string.IsNullOrEmpty(platform.Id))
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
                            cmd.Parameters.AddWithValue("@id", platform.Id);
                        }
                        if (!string.IsNullOrEmpty(platform.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", platform.Code);
                        }
                        if (!string.IsNullOrEmpty(platform.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", platform.Name);
                        }
                        if (!string.IsNullOrEmpty(platform.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", platform.Description);
                        }
                        if (!string.IsNullOrEmpty(platform.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", platform.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(platform.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", platform.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(platform.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", platform.UpdatedAt);
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
                                    platform.Id = reader.GetValue(0).ToString();
                                else
                                    platform.Id = null;
                                if (!reader.IsDBNull(1))
                                    platform.Code = reader.GetValue(1).ToString();
                                else
                                    platform.Code = null;
                                if (!reader.IsDBNull(2))
                                    platform.Name = reader.GetValue(2).ToString();
                                else
                                    platform.Name = null;
                                if (!reader.IsDBNull(3))
                                    platform.Description = reader.GetValue(3).ToString();
                                else
                                    platform.Description = null;
                                if (!reader.IsDBNull(4))
                                    platform.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    platform.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    platform.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    platform.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    platform.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    platform.UpdatedAt = null;
                                response.Add(platform);
                                platform = new PlatformModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlatformModel>();
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
                response = new List<PlatformModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlatformModel> Read(string connectionString, PlatformModel platform, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformModel> response = new List<PlatformModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `code`, `name`, `description`, `modified_by`, `created_at`, `updated_at`  FROM sch_platforms ";
                {
                    if (platform != null)
                    {
                        if (!string.IsNullOrEmpty(platform.Id))
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
                            cmd.Parameters.AddWithValue("@id", platform.Id);
                        }
                        if (!string.IsNullOrEmpty(platform.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", platform.Code);
                        }
                        if (!string.IsNullOrEmpty(platform.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", platform.Name);
                        }
                        if (!string.IsNullOrEmpty(platform.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", platform.Description);
                        }
                        if (!string.IsNullOrEmpty(platform.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", platform.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(platform.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", platform.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(platform.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", platform.UpdatedAt);
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
                                    platform.Id = reader.GetValue(0).ToString();
                                else
                                    platform.Id = null;
                                if (!reader.IsDBNull(1))
                                    platform.Code = reader.GetValue(1).ToString();
                                else
                                    platform.Code = null;
                                if (!reader.IsDBNull(2))
                                    platform.Name = reader.GetValue(2).ToString();
                                else
                                    platform.Name = null;
                                if (!reader.IsDBNull(3))
                                    platform.Description = reader.GetValue(3).ToString();
                                else
                                    platform.Description = null;
                                if (!reader.IsDBNull(4))
                                    platform.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    platform.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    platform.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    platform.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    platform.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    platform.UpdatedAt = null;
                                response.Add(platform);
                                platform = new PlatformModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlatformModel>();
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
                response = new List<PlatformModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlatformModel> Read(string connectionString, List<PlatformModel> platformWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformModel> response = new List<PlatformModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `code`, `name`, `description`, `modified_by`, `created_at`, `updated_at`  FROM sch_platforms ";
                if (platformWhere != null && platformWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < platformWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(platformWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, platformWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, platformWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, platformWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, platformWhere[i].Description);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, platformWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, platformWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, platformWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != platformWhere.Count - 1)
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
                            PlatformModel platform = new PlatformModel(); if (!reader.IsDBNull(0))
                                platform.Id = reader.GetValue(0).ToString();
                            else
                                platform.Id = null;
                            if (!reader.IsDBNull(1))
                                platform.Code = reader.GetValue(1).ToString();
                            else
                                platform.Code = null;
                            if (!reader.IsDBNull(2))
                                platform.Name = reader.GetValue(2).ToString();
                            else
                                platform.Name = null;
                            if (!reader.IsDBNull(3))
                                platform.Description = reader.GetValue(3).ToString();
                            else
                                platform.Description = null;
                            if (!reader.IsDBNull(4))
                                platform.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                platform.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                platform.CreatedAt = reader.GetValue(5).ToString();
                            else
                                platform.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                platform.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                platform.UpdatedAt = null;
                            response.Add(platform);
                            platform = new PlatformModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlatformModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlatformModel> Read(string connectionString, List<PlatformModel> platformWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformModel> response = new List<PlatformModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `code`, `name`, `description`, `modified_by`, `created_at`, `updated_at`  FROM sch_platforms ";
                if (platformWhere != null && platformWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < platformWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(platformWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, platformWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, platformWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, platformWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, platformWhere[i].Description);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, platformWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, platformWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(platformWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, platformWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != platformWhere.Count - 1)
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
                            PlatformModel platform = new PlatformModel(); if (!reader.IsDBNull(0))
                                platform.Id = reader.GetValue(0).ToString();
                            else
                                platform.Id = null;
                            if (!reader.IsDBNull(1))
                                platform.Code = reader.GetValue(1).ToString();
                            else
                                platform.Code = null;
                            if (!reader.IsDBNull(2))
                                platform.Name = reader.GetValue(2).ToString();
                            else
                                platform.Name = null;
                            if (!reader.IsDBNull(3))
                                platform.Description = reader.GetValue(3).ToString();
                            else
                                platform.Description = null;
                            if (!reader.IsDBNull(4))
                                platform.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                platform.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                platform.CreatedAt = reader.GetValue(5).ToString();
                            else
                                platform.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                platform.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                platform.UpdatedAt = null;
                            response.Add(platform);
                            platform = new PlatformModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlatformModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlatformModel> Create(string connectionString, PlatformModel platform)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformModel> response = new List<PlatformModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO sch_platforms (";
                string values = "(";
                if (!string.IsNullOrEmpty(platform.Id))
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
                    cmd.Parameters.AddWithValue("@id", platform.Id);
                }
                if (!string.IsNullOrEmpty(platform.Code))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `code` ");
                        values = string.Concat(values, ", @code");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `code` ");
                        values = string.Concat(values, " @code");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", platform.Code);
                }
                if (!string.IsNullOrEmpty(platform.Name))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `name` ");
                        values = string.Concat(values, ", @name");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `name` ");
                        values = string.Concat(values, " @name");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", platform.Name);
                }
                if (!string.IsNullOrEmpty(platform.Description))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `description` ");
                        values = string.Concat(values, ", @description");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `description` ");
                        values = string.Concat(values, " @description");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@description", platform.Description);
                }
                if (!string.IsNullOrEmpty(platform.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", platform.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(platform.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", platform.CreatedAt);
                }
                if (!string.IsNullOrEmpty(platform.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", platform.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                platform.Id = cmd.LastInsertedId.ToString();
                response.Add(platform);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformModel>();
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
        public List<PlatformModel> Create(string connectionString, List<PlatformModel> platformList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformModel> response = new List<PlatformModel>();
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
                        for (int i = 0; i < platformList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO sch_platforms (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(platformList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, platformList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(platformList[i].Code))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `code` ");
                                        values = string.Concat(values, ", @code" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `code` ");
                                        values = string.Concat(values, "@code" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, platformList[i].Code);
                                }
                                if (!string.IsNullOrEmpty(platformList[i].Name))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `name` ");
                                        values = string.Concat(values, ", @name" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `name` ");
                                        values = string.Concat(values, "@name" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, platformList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(platformList[i].Description))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `description` ");
                                        values = string.Concat(values, ", @description" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `description` ");
                                        values = string.Concat(values, "@description" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@description" + i, platformList[i].Description);
                                }
                                if (!string.IsNullOrEmpty(platformList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, platformList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(platformList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, platformList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(platformList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, platformList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(platformList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<PlatformModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlatformModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformModel>();
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

        public List<PlatformModel> Update(string connectionString, PlatformModel platform, PlatformModel platformWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformModel> response = new List<PlatformModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE sch_platforms SET ";
                if (!string.IsNullOrEmpty(platform.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", platform.Id);
                }
                if (!string.IsNullOrEmpty(platform.Code))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `code` = @code");
                    else
                        sql = string.Concat(sql, " `code` = @code");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", platform.Code);
                }
                if (!string.IsNullOrEmpty(platform.Name))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `name` = @name");
                    else
                        sql = string.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", platform.Name);
                }
                if (!string.IsNullOrEmpty(platform.Description))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `description` = @description");
                    else
                        sql = string.Concat(sql, " `description` = @description");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@description", platform.Description);
                }
                if (!string.IsNullOrEmpty(platform.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", platform.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(platform.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", platform.CreatedAt);
                }
                if (!string.IsNullOrEmpty(platform.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", platform.UpdatedAt);
                }
                if (platformWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(platformWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", platformWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(platformWhere.Code))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `code` = @codeWhere");
                        else
                            sql = string.Concat(sql, " `code` = @codeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@codeWhere", platformWhere.Code);
                    }
                    if (!string.IsNullOrEmpty(platformWhere.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = string.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", platformWhere.Name);
                    }
                    if (!string.IsNullOrEmpty(platformWhere.Description))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `description` = @descriptionWhere");
                        else
                            sql = string.Concat(sql, " `description` = @descriptionWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descriptionWhere", platformWhere.Description);
                    }
                    if (!string.IsNullOrEmpty(platformWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", platformWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(platformWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", platformWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(platformWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", platformWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(platform);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformModel>();
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

        public List<PlatformModel> Update(string connectionString, List<PlatformModel> platformList, List<PlatformModel> platformWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformModel> response = new List<PlatformModel>();
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
                        for (int i = 0; i < platformList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE sch_platforms SET ";
                                if (!string.IsNullOrEmpty(platformList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, platformList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(platformList[i].Code))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `code` = @code" + i);
                                    else
                                        sql = string.Concat(sql, " `code` = @code" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, platformList[i].Code);
                                }
                                if (!string.IsNullOrEmpty(platformList[i].Name))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = string.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, platformList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(platformList[i].Description))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `description` = @description" + i);
                                    else
                                        sql = string.Concat(sql, " `description` = @description" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@description" + i, platformList[i].Description);
                                }
                                if (!string.IsNullOrEmpty(platformList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, platformList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(platformList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, platformList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(platformList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, platformList[i].UpdatedAt);
                                }
                                if (platformWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(platformWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, platformWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(platformWhereList[i].Code))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `code` = @codeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `code` = @codeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@codeWhere" + i, platformWhereList[i].Code);
                                    }
                                    if (!string.IsNullOrEmpty(platformWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, platformWhereList[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(platformWhereList[i].Description))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `description` = @descriptionWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `description` = @descriptionWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descriptionWhere" + i, platformWhereList[i].Description);
                                    }
                                    if (!string.IsNullOrEmpty(platformWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, platformWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(platformWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, platformWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(platformWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, platformWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(platformList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlatformModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlatformModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformModel>();
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

        public List<PlatformModel> Delete(string connectionString, PlatformModel platform)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformModel> response = new List<PlatformModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM sch_platforms  ";
                if (platform != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(platform.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", platform.Id);
                    }
                    if (!string.IsNullOrEmpty(platform.Code))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `code` = @code");
                        else
                            sql = string.Concat(sql, " `code` = @code");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@code", platform.Code);
                    }
                    if (!string.IsNullOrEmpty(platform.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @name");
                        else
                            sql = string.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", platform.Name);
                    }
                    if (!string.IsNullOrEmpty(platform.Description))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `description` = @description");
                        else
                            sql = string.Concat(sql, " `description` = @description");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@description", platform.Description);
                    }
                    if (!string.IsNullOrEmpty(platform.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", platform.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(platform.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", platform.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(platform.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", platform.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(platform);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformModel>();
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

        public List<PlatformModel> Delete(string connectionString, List<PlatformModel> platform)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlatformModel> response = new List<PlatformModel>();
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
                        for (int i = 0; i < platform.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM sch_platforms  ";
                                if (platform[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(platform[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, platform[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(platform[i].Code))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `code` = @code" + i);
                                        else
                                            sql = string.Concat(sql, " `code` = @code" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@code" + i, platform[i].Code);
                                    }
                                    if (!string.IsNullOrEmpty(platform[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, platform[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(platform[i].Description))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `description` = @description" + i);
                                        else
                                            sql = string.Concat(sql, " `description` = @description" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@description" + i, platform[i].Description);
                                    }
                                    if (!string.IsNullOrEmpty(platform[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, platform[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(platform[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, platform[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(platform[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, platform[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(platform[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlatformModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlatformModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlatformModel>();
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
