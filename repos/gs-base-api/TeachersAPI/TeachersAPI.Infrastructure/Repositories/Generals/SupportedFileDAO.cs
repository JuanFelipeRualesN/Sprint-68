using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Generals;

namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Generals
{
    class SupportedFileDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, SupportedFileModel> ReadD(string connectionString, SupportedFileModel supportedFile, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SupportedFileModel> response = new Dictionary<string, SupportedFileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `classification`, `format`, `max_width`, `max_height`, `max_size`, `path`, `modified_by`, `created_at`, `updated_at`  FROM supported_files ";
                {
                    if (supportedFile != null)
                    {
                        if (!string.IsNullOrEmpty(supportedFile.Id))
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
                            cmd.Parameters.AddWithValue("@id", supportedFile.Id);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.Classification))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `classification` = @classification ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `classification` = @classification ");
                            }
                            cmd.Parameters.AddWithValue("@classification", supportedFile.Classification);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.Format))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `format` = @format ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `format` = @format ");
                            }
                            cmd.Parameters.AddWithValue("@format", supportedFile.Format);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.MaxWidth))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_width` = @maxWidth ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_width` = @maxWidth ");
                            }
                            cmd.Parameters.AddWithValue("@maxWidth", supportedFile.MaxWidth);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.MaxHeight))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_height` = @maxHeight ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_height` = @maxHeight ");
                            }
                            cmd.Parameters.AddWithValue("@maxHeight", supportedFile.MaxHeight);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.MaxSize))
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
                            cmd.Parameters.AddWithValue("@maxSize", supportedFile.MaxSize);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.Path))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `path` = @path ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `path` = @path ");
                            }
                            cmd.Parameters.AddWithValue("@path", supportedFile.Path);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", supportedFile.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", supportedFile.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", supportedFile.UpdatedAt);
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
                                    supportedFile.Id = reader.GetValue(0).ToString();
                                else
                                    supportedFile.Id = null;
                                if (!reader.IsDBNull(1))
                                    supportedFile.Classification = reader.GetValue(1).ToString();
                                else
                                    supportedFile.Classification = null;
                                if (!reader.IsDBNull(2))
                                    supportedFile.Format = reader.GetValue(2).ToString();
                                else
                                    supportedFile.Format = null;
                                if (!reader.IsDBNull(3))
                                    supportedFile.MaxWidth = reader.GetValue(3).ToString();
                                else
                                    supportedFile.MaxWidth = null;
                                if (!reader.IsDBNull(4))
                                    supportedFile.MaxHeight = reader.GetValue(4).ToString();
                                else
                                    supportedFile.MaxHeight = null;
                                if (!reader.IsDBNull(5))
                                    supportedFile.MaxSize = reader.GetValue(5).ToString();
                                else
                                    supportedFile.MaxSize = null;
                                if (!reader.IsDBNull(6))
                                    supportedFile.Path = reader.GetValue(6).ToString();
                                else
                                    supportedFile.Path = null;
                                if (!reader.IsDBNull(7))
                                    supportedFile.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    supportedFile.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    supportedFile.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    supportedFile.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    supportedFile.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    supportedFile.UpdatedAt = null;
                                response.Add(supportedFile.Id, supportedFile);
                                supportedFile = new SupportedFileModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, SupportedFileModel>();
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
                response = new Dictionary<string, SupportedFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, SupportedFileModel> ReadD(string connectionString, SupportedFileModel supportedFile, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SupportedFileModel> response = new Dictionary<string, SupportedFileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `classification`, `format`, `max_width`, `max_height`, `max_size`, `path`, `modified_by`, `created_at`, `updated_at`  FROM supported_files ";
                {
                    if (supportedFile != null)
                    {
                        if (!string.IsNullOrEmpty(supportedFile.Id))
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
                            cmd.Parameters.AddWithValue("@id", supportedFile.Id);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.Classification))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `classification` = @classification ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `classification` = @classification ");
                            }
                            cmd.Parameters.AddWithValue("@classification", supportedFile.Classification);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.Format))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `format` = @format ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `format` = @format ");
                            }
                            cmd.Parameters.AddWithValue("@format", supportedFile.Format);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.MaxWidth))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_width` = @maxWidth ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_width` = @maxWidth ");
                            }
                            cmd.Parameters.AddWithValue("@maxWidth", supportedFile.MaxWidth);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.MaxHeight))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_height` = @maxHeight ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_height` = @maxHeight ");
                            }
                            cmd.Parameters.AddWithValue("@maxHeight", supportedFile.MaxHeight);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.MaxSize))
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
                            cmd.Parameters.AddWithValue("@maxSize", supportedFile.MaxSize);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.Path))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `path` = @path ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `path` = @path ");
                            }
                            cmd.Parameters.AddWithValue("@path", supportedFile.Path);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", supportedFile.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", supportedFile.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", supportedFile.UpdatedAt);
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
                                    supportedFile.Id = reader.GetValue(0).ToString();
                                else
                                    supportedFile.Id = null;
                                if (!reader.IsDBNull(1))
                                    supportedFile.Classification = reader.GetValue(1).ToString();
                                else
                                    supportedFile.Classification = null;
                                if (!reader.IsDBNull(2))
                                    supportedFile.Format = reader.GetValue(2).ToString();
                                else
                                    supportedFile.Format = null;
                                if (!reader.IsDBNull(3))
                                    supportedFile.MaxWidth = reader.GetValue(3).ToString();
                                else
                                    supportedFile.MaxWidth = null;
                                if (!reader.IsDBNull(4))
                                    supportedFile.MaxHeight = reader.GetValue(4).ToString();
                                else
                                    supportedFile.MaxHeight = null;
                                if (!reader.IsDBNull(5))
                                    supportedFile.MaxSize = reader.GetValue(5).ToString();
                                else
                                    supportedFile.MaxSize = null;
                                if (!reader.IsDBNull(6))
                                    supportedFile.Path = reader.GetValue(6).ToString();
                                else
                                    supportedFile.Path = null;
                                if (!reader.IsDBNull(7))
                                    supportedFile.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    supportedFile.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    supportedFile.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    supportedFile.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    supportedFile.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    supportedFile.UpdatedAt = null;
                                response.Add(supportedFile.Id, supportedFile);
                                supportedFile = new SupportedFileModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, SupportedFileModel>();
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
                response = new Dictionary<string, SupportedFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, SupportedFileModel> ReadD(string connectionString, List<SupportedFileModel> supportedFileWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SupportedFileModel> response = new Dictionary<string, SupportedFileModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `classification`, `format`, `max_width`, `max_height`, `max_size`, `path`, `modified_by`, `created_at`, `updated_at`  FROM supported_files ";
                if (supportedFileWhere != null && supportedFileWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < supportedFileWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, supportedFileWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Classification))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `classification` = @classification" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@classification" + i, supportedFileWhere[i].Classification);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Format))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `format` = @format" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@format" + i, supportedFileWhere[i].Format);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].MaxWidth))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_width` = @maxWidth" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxWidth" + i, supportedFileWhere[i].MaxWidth);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].MaxHeight))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_height` = @maxHeight" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxHeight" + i, supportedFileWhere[i].MaxHeight);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].MaxSize))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_size` = @maxSize" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxSize" + i, supportedFileWhere[i].MaxSize);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Path))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `path` = @path" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@path" + i, supportedFileWhere[i].Path);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, supportedFileWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, supportedFileWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, supportedFileWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != supportedFileWhere.Count - 1)
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
                            SupportedFileModel supportedFile = new SupportedFileModel(); if (!reader.IsDBNull(0))
                                supportedFile.Id = reader.GetValue(0).ToString();
                            else
                                supportedFile.Id = null;
                            if (!reader.IsDBNull(1))
                                supportedFile.Classification = reader.GetValue(1).ToString();
                            else
                                supportedFile.Classification = null;
                            if (!reader.IsDBNull(2))
                                supportedFile.Format = reader.GetValue(2).ToString();
                            else
                                supportedFile.Format = null;
                            if (!reader.IsDBNull(3))
                                supportedFile.MaxWidth = reader.GetValue(3).ToString();
                            else
                                supportedFile.MaxWidth = null;
                            if (!reader.IsDBNull(4))
                                supportedFile.MaxHeight = reader.GetValue(4).ToString();
                            else
                                supportedFile.MaxHeight = null;
                            if (!reader.IsDBNull(5))
                                supportedFile.MaxSize = reader.GetValue(5).ToString();
                            else
                                supportedFile.MaxSize = null;
                            if (!reader.IsDBNull(6))
                                supportedFile.Path = reader.GetValue(6).ToString();
                            else
                                supportedFile.Path = null;
                            if (!reader.IsDBNull(7))
                                supportedFile.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                supportedFile.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                supportedFile.CreatedAt = reader.GetValue(8).ToString();
                            else
                                supportedFile.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                supportedFile.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                supportedFile.UpdatedAt = null;
                            response.Add(supportedFile.Id, supportedFile);
                            supportedFile = new SupportedFileModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, SupportedFileModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, SupportedFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, SupportedFileModel> ReadD(string connectionString, List<SupportedFileModel> supportedFileWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SupportedFileModel> response = new Dictionary<string, SupportedFileModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `classification`, `format`, `max_width`, `max_height`, `max_size`, `path`, `modified_by`, `created_at`, `updated_at`  FROM supported_files ";
                if (supportedFileWhere != null && supportedFileWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < supportedFileWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, supportedFileWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Classification))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `classification` = @classification" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@classification" + i, supportedFileWhere[i].Classification);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Format))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `format` = @format" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@format" + i, supportedFileWhere[i].Format);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].MaxWidth))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_width` = @maxWidth" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxWidth" + i, supportedFileWhere[i].MaxWidth);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].MaxHeight))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_height` = @maxHeight" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxHeight" + i, supportedFileWhere[i].MaxHeight);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].MaxSize))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_size` = @maxSize" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxSize" + i, supportedFileWhere[i].MaxSize);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Path))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `path` = @path" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@path" + i, supportedFileWhere[i].Path);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, supportedFileWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, supportedFileWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, supportedFileWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != supportedFileWhere.Count - 1)
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
                            SupportedFileModel supportedFile = new SupportedFileModel(); if (!reader.IsDBNull(0))
                                supportedFile.Id = reader.GetValue(0).ToString();
                            else
                                supportedFile.Id = null;
                            if (!reader.IsDBNull(1))
                                supportedFile.Classification = reader.GetValue(1).ToString();
                            else
                                supportedFile.Classification = null;
                            if (!reader.IsDBNull(2))
                                supportedFile.Format = reader.GetValue(2).ToString();
                            else
                                supportedFile.Format = null;
                            if (!reader.IsDBNull(3))
                                supportedFile.MaxWidth = reader.GetValue(3).ToString();
                            else
                                supportedFile.MaxWidth = null;
                            if (!reader.IsDBNull(4))
                                supportedFile.MaxHeight = reader.GetValue(4).ToString();
                            else
                                supportedFile.MaxHeight = null;
                            if (!reader.IsDBNull(5))
                                supportedFile.MaxSize = reader.GetValue(5).ToString();
                            else
                                supportedFile.MaxSize = null;
                            if (!reader.IsDBNull(6))
                                supportedFile.Path = reader.GetValue(6).ToString();
                            else
                                supportedFile.Path = null;
                            if (!reader.IsDBNull(7))
                                supportedFile.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                supportedFile.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                supportedFile.CreatedAt = reader.GetValue(8).ToString();
                            else
                                supportedFile.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                supportedFile.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                supportedFile.UpdatedAt = null;
                            response.Add(supportedFile.Id, supportedFile);
                            supportedFile = new SupportedFileModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, SupportedFileModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, SupportedFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SupportedFileModel> Read(string connectionString, SupportedFileModel supportedFile, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SupportedFileModel> response = new List<SupportedFileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `classification`, `format`, `max_width`, `max_height`, `max_size`, `path`, `modified_by`, `created_at`, `updated_at`  FROM supported_files ";
                {
                    if (supportedFile != null)
                    {
                        if (!string.IsNullOrEmpty(supportedFile.Id))
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
                            cmd.Parameters.AddWithValue("@id", supportedFile.Id);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.Classification))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `classification` = @classification ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `classification` = @classification ");
                            }
                            cmd.Parameters.AddWithValue("@classification", supportedFile.Classification);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.Format))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `format` = @format ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `format` = @format ");
                            }
                            cmd.Parameters.AddWithValue("@format", supportedFile.Format);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.MaxWidth))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_width` = @maxWidth ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_width` = @maxWidth ");
                            }
                            cmd.Parameters.AddWithValue("@maxWidth", supportedFile.MaxWidth);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.MaxHeight))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_height` = @maxHeight ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_height` = @maxHeight ");
                            }
                            cmd.Parameters.AddWithValue("@maxHeight", supportedFile.MaxHeight);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.MaxSize))
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
                            cmd.Parameters.AddWithValue("@maxSize", supportedFile.MaxSize);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.Path))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `path` = @path ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `path` = @path ");
                            }
                            cmd.Parameters.AddWithValue("@path", supportedFile.Path);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", supportedFile.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", supportedFile.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", supportedFile.UpdatedAt);
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
                                    supportedFile.Id = reader.GetValue(0).ToString();
                                else
                                    supportedFile.Id = null;
                                if (!reader.IsDBNull(1))
                                    supportedFile.Classification = reader.GetValue(1).ToString();
                                else
                                    supportedFile.Classification = null;
                                if (!reader.IsDBNull(2))
                                    supportedFile.Format = reader.GetValue(2).ToString();
                                else
                                    supportedFile.Format = null;
                                if (!reader.IsDBNull(3))
                                    supportedFile.MaxWidth = reader.GetValue(3).ToString();
                                else
                                    supportedFile.MaxWidth = null;
                                if (!reader.IsDBNull(4))
                                    supportedFile.MaxHeight = reader.GetValue(4).ToString();
                                else
                                    supportedFile.MaxHeight = null;
                                if (!reader.IsDBNull(5))
                                    supportedFile.MaxSize = reader.GetValue(5).ToString();
                                else
                                    supportedFile.MaxSize = null;
                                if (!reader.IsDBNull(6))
                                    supportedFile.Path = reader.GetValue(6).ToString();
                                else
                                    supportedFile.Path = null;
                                if (!reader.IsDBNull(7))
                                    supportedFile.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    supportedFile.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    supportedFile.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    supportedFile.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    supportedFile.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    supportedFile.UpdatedAt = null;
                                response.Add(supportedFile);
                                supportedFile = new SupportedFileModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<SupportedFileModel>();
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
                response = new List<SupportedFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SupportedFileModel> Read(string connectionString, SupportedFileModel supportedFile, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SupportedFileModel> response = new List<SupportedFileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `classification`, `format`, `max_width`, `max_height`, `max_size`, `path`, `modified_by`, `created_at`, `updated_at`  FROM supported_files ";
                {
                    if (supportedFile != null)
                    {
                        if (!string.IsNullOrEmpty(supportedFile.Id))
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
                            cmd.Parameters.AddWithValue("@id", supportedFile.Id);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.Classification))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `classification` = @classification ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `classification` = @classification ");
                            }
                            cmd.Parameters.AddWithValue("@classification", supportedFile.Classification);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.Format))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `format` = @format ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `format` = @format ");
                            }
                            cmd.Parameters.AddWithValue("@format", supportedFile.Format);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.MaxWidth))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_width` = @maxWidth ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_width` = @maxWidth ");
                            }
                            cmd.Parameters.AddWithValue("@maxWidth", supportedFile.MaxWidth);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.MaxHeight))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_height` = @maxHeight ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_height` = @maxHeight ");
                            }
                            cmd.Parameters.AddWithValue("@maxHeight", supportedFile.MaxHeight);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.MaxSize))
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
                            cmd.Parameters.AddWithValue("@maxSize", supportedFile.MaxSize);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.Path))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `path` = @path ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `path` = @path ");
                            }
                            cmd.Parameters.AddWithValue("@path", supportedFile.Path);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", supportedFile.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", supportedFile.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(supportedFile.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", supportedFile.UpdatedAt);
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
                                    supportedFile.Id = reader.GetValue(0).ToString();
                                else
                                    supportedFile.Id = null;
                                if (!reader.IsDBNull(1))
                                    supportedFile.Classification = reader.GetValue(1).ToString();
                                else
                                    supportedFile.Classification = null;
                                if (!reader.IsDBNull(2))
                                    supportedFile.Format = reader.GetValue(2).ToString();
                                else
                                    supportedFile.Format = null;
                                if (!reader.IsDBNull(3))
                                    supportedFile.MaxWidth = reader.GetValue(3).ToString();
                                else
                                    supportedFile.MaxWidth = null;
                                if (!reader.IsDBNull(4))
                                    supportedFile.MaxHeight = reader.GetValue(4).ToString();
                                else
                                    supportedFile.MaxHeight = null;
                                if (!reader.IsDBNull(5))
                                    supportedFile.MaxSize = reader.GetValue(5).ToString();
                                else
                                    supportedFile.MaxSize = null;
                                if (!reader.IsDBNull(6))
                                    supportedFile.Path = reader.GetValue(6).ToString();
                                else
                                    supportedFile.Path = null;
                                if (!reader.IsDBNull(7))
                                    supportedFile.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    supportedFile.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    supportedFile.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    supportedFile.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    supportedFile.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    supportedFile.UpdatedAt = null;
                                response.Add(supportedFile);
                                supportedFile = new SupportedFileModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<SupportedFileModel>();
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
                response = new List<SupportedFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SupportedFileModel> Read(string connectionString, List<SupportedFileModel> supportedFileWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SupportedFileModel> response = new List<SupportedFileModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `classification`, `format`, `max_width`, `max_height`, `max_size`, `path`, `modified_by`, `created_at`, `updated_at`  FROM supported_files ";
                if (supportedFileWhere != null && supportedFileWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < supportedFileWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, supportedFileWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Classification))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `classification` = @classification" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@classification" + i, supportedFileWhere[i].Classification);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Format))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `format` = @format" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@format" + i, supportedFileWhere[i].Format);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].MaxWidth))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_width` = @maxWidth" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxWidth" + i, supportedFileWhere[i].MaxWidth);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].MaxHeight))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_height` = @maxHeight" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxHeight" + i, supportedFileWhere[i].MaxHeight);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].MaxSize))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_size` = @maxSize" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxSize" + i, supportedFileWhere[i].MaxSize);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Path))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `path` = @path" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@path" + i, supportedFileWhere[i].Path);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, supportedFileWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, supportedFileWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, supportedFileWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != supportedFileWhere.Count - 1)
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
                            SupportedFileModel supportedFile = new SupportedFileModel(); if (!reader.IsDBNull(0))
                                supportedFile.Id = reader.GetValue(0).ToString();
                            else
                                supportedFile.Id = null;
                            if (!reader.IsDBNull(1))
                                supportedFile.Classification = reader.GetValue(1).ToString();
                            else
                                supportedFile.Classification = null;
                            if (!reader.IsDBNull(2))
                                supportedFile.Format = reader.GetValue(2).ToString();
                            else
                                supportedFile.Format = null;
                            if (!reader.IsDBNull(3))
                                supportedFile.MaxWidth = reader.GetValue(3).ToString();
                            else
                                supportedFile.MaxWidth = null;
                            if (!reader.IsDBNull(4))
                                supportedFile.MaxHeight = reader.GetValue(4).ToString();
                            else
                                supportedFile.MaxHeight = null;
                            if (!reader.IsDBNull(5))
                                supportedFile.MaxSize = reader.GetValue(5).ToString();
                            else
                                supportedFile.MaxSize = null;
                            if (!reader.IsDBNull(6))
                                supportedFile.Path = reader.GetValue(6).ToString();
                            else
                                supportedFile.Path = null;
                            if (!reader.IsDBNull(7))
                                supportedFile.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                supportedFile.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                supportedFile.CreatedAt = reader.GetValue(8).ToString();
                            else
                                supportedFile.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                supportedFile.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                supportedFile.UpdatedAt = null;
                            response.Add(supportedFile);
                            supportedFile = new SupportedFileModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<SupportedFileModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SupportedFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SupportedFileModel> Read(string connectionString, List<SupportedFileModel> supportedFileWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SupportedFileModel> response = new List<SupportedFileModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `classification`, `format`, `max_width`, `max_height`, `max_size`, `path`, `modified_by`, `created_at`, `updated_at`  FROM supported_files ";
                if (supportedFileWhere != null && supportedFileWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < supportedFileWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, supportedFileWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Classification))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `classification` = @classification" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@classification" + i, supportedFileWhere[i].Classification);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Format))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `format` = @format" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@format" + i, supportedFileWhere[i].Format);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].MaxWidth))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_width` = @maxWidth" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxWidth" + i, supportedFileWhere[i].MaxWidth);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].MaxHeight))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_height` = @maxHeight" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxHeight" + i, supportedFileWhere[i].MaxHeight);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].MaxSize))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_size` = @maxSize" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxSize" + i, supportedFileWhere[i].MaxSize);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].Path))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `path` = @path" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@path" + i, supportedFileWhere[i].Path);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, supportedFileWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, supportedFileWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(supportedFileWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, supportedFileWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != supportedFileWhere.Count - 1)
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
                            SupportedFileModel supportedFile = new SupportedFileModel(); if (!reader.IsDBNull(0))
                                supportedFile.Id = reader.GetValue(0).ToString();
                            else
                                supportedFile.Id = null;
                            if (!reader.IsDBNull(1))
                                supportedFile.Classification = reader.GetValue(1).ToString();
                            else
                                supportedFile.Classification = null;
                            if (!reader.IsDBNull(2))
                                supportedFile.Format = reader.GetValue(2).ToString();
                            else
                                supportedFile.Format = null;
                            if (!reader.IsDBNull(3))
                                supportedFile.MaxWidth = reader.GetValue(3).ToString();
                            else
                                supportedFile.MaxWidth = null;
                            if (!reader.IsDBNull(4))
                                supportedFile.MaxHeight = reader.GetValue(4).ToString();
                            else
                                supportedFile.MaxHeight = null;
                            if (!reader.IsDBNull(5))
                                supportedFile.MaxSize = reader.GetValue(5).ToString();
                            else
                                supportedFile.MaxSize = null;
                            if (!reader.IsDBNull(6))
                                supportedFile.Path = reader.GetValue(6).ToString();
                            else
                                supportedFile.Path = null;
                            if (!reader.IsDBNull(7))
                                supportedFile.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                supportedFile.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                supportedFile.CreatedAt = reader.GetValue(8).ToString();
                            else
                                supportedFile.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                supportedFile.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                supportedFile.UpdatedAt = null;
                            response.Add(supportedFile);
                            supportedFile = new SupportedFileModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<SupportedFileModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SupportedFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SupportedFileModel> Create(string connectionString, SupportedFileModel supportedFile)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SupportedFileModel> response = new List<SupportedFileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO supported_files (";
                string values = "(";
                if (!string.IsNullOrEmpty(supportedFile.Id))
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
                    cmd.Parameters.AddWithValue("@id", supportedFile.Id);
                }
                if (!string.IsNullOrEmpty(supportedFile.Classification))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `classification` ");
                        values = string.Concat(values, ", @classification");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `classification` ");
                        values = string.Concat(values, " @classification");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@classification", supportedFile.Classification);
                }
                if (!string.IsNullOrEmpty(supportedFile.Format))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `format` ");
                        values = string.Concat(values, ", @format");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `format` ");
                        values = string.Concat(values, " @format");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@format", supportedFile.Format);
                }
                if (!string.IsNullOrEmpty(supportedFile.MaxWidth))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `max_width` ");
                        values = string.Concat(values, ", @maxWidth");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `max_width` ");
                        values = string.Concat(values, " @maxWidth");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@maxWidth", supportedFile.MaxWidth);
                }
                if (!string.IsNullOrEmpty(supportedFile.MaxHeight))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `max_height` ");
                        values = string.Concat(values, ", @maxHeight");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `max_height` ");
                        values = string.Concat(values, " @maxHeight");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@maxHeight", supportedFile.MaxHeight);
                }
                if (!string.IsNullOrEmpty(supportedFile.MaxSize))
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
                    cmd.Parameters.AddWithValue("@maxSize", supportedFile.MaxSize);
                }
                if (!string.IsNullOrEmpty(supportedFile.Path))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `path` ");
                        values = string.Concat(values, ", @path");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `path` ");
                        values = string.Concat(values, " @path");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@path", supportedFile.Path);
                }
                if (!string.IsNullOrEmpty(supportedFile.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", supportedFile.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(supportedFile.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", supportedFile.CreatedAt);
                }
                if (!string.IsNullOrEmpty(supportedFile.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", supportedFile.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                supportedFile.Id = cmd.LastInsertedId.ToString();
                response.Add(supportedFile);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SupportedFileModel>();
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
        public List<SupportedFileModel> Create(string connectionString, List<SupportedFileModel> supportedFileList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SupportedFileModel> response = new List<SupportedFileModel>();
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
                        for (int i = 0; i < supportedFileList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO supported_files (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(supportedFileList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, supportedFileList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].Classification))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `classification` ");
                                        values = string.Concat(values, ", @classification" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `classification` ");
                                        values = string.Concat(values, "@classification" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@classification" + i, supportedFileList[i].Classification);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].Format))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `format` ");
                                        values = string.Concat(values, ", @format" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `format` ");
                                        values = string.Concat(values, "@format" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@format" + i, supportedFileList[i].Format);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].MaxWidth))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `max_width` ");
                                        values = string.Concat(values, ", @maxWidth" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `max_width` ");
                                        values = string.Concat(values, "@maxWidth" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@maxWidth" + i, supportedFileList[i].MaxWidth);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].MaxHeight))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `max_height` ");
                                        values = string.Concat(values, ", @maxHeight" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `max_height` ");
                                        values = string.Concat(values, "@maxHeight" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@maxHeight" + i, supportedFileList[i].MaxHeight);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].MaxSize))
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
                                    cmd.Parameters.AddWithValue("@maxSize" + i, supportedFileList[i].MaxSize);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].Path))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `path` ");
                                        values = string.Concat(values, ", @path" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `path` ");
                                        values = string.Concat(values, "@path" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@path" + i, supportedFileList[i].Path);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, supportedFileList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, supportedFileList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, supportedFileList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(supportedFileList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<SupportedFileModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SupportedFileModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SupportedFileModel>();
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

        public List<SupportedFileModel> Update(string connectionString, SupportedFileModel supportedFile, SupportedFileModel supportedFileWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SupportedFileModel> response = new List<SupportedFileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE supported_files SET ";
                if (!string.IsNullOrEmpty(supportedFile.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", supportedFile.Id);
                }
                if (!string.IsNullOrEmpty(supportedFile.Classification))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `classification` = @classification");
                    else
                        sql = string.Concat(sql, " `classification` = @classification");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@classification", supportedFile.Classification);
                }
                if (!string.IsNullOrEmpty(supportedFile.Format))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `format` = @format");
                    else
                        sql = string.Concat(sql, " `format` = @format");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@format", supportedFile.Format);
                }
                if (!string.IsNullOrEmpty(supportedFile.MaxWidth))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `max_width` = @maxWidth");
                    else
                        sql = string.Concat(sql, " `max_width` = @maxWidth");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@maxWidth", supportedFile.MaxWidth);
                }
                if (!string.IsNullOrEmpty(supportedFile.MaxHeight))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `max_height` = @maxHeight");
                    else
                        sql = string.Concat(sql, " `max_height` = @maxHeight");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@maxHeight", supportedFile.MaxHeight);
                }
                if (!string.IsNullOrEmpty(supportedFile.MaxSize))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `max_size` = @maxSize");
                    else
                        sql = string.Concat(sql, " `max_size` = @maxSize");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@maxSize", supportedFile.MaxSize);
                }
                if (!string.IsNullOrEmpty(supportedFile.Path))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `path` = @path");
                    else
                        sql = string.Concat(sql, " `path` = @path");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@path", supportedFile.Path);
                }
                if (!string.IsNullOrEmpty(supportedFile.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", supportedFile.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(supportedFile.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", supportedFile.CreatedAt);
                }
                if (!string.IsNullOrEmpty(supportedFile.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", supportedFile.UpdatedAt);
                }
                if (supportedFileWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(supportedFileWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", supportedFileWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(supportedFileWhere.Classification))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `classification` = @classificationWhere");
                        else
                            sql = string.Concat(sql, " `classification` = @classificationWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@classificationWhere", supportedFileWhere.Classification);
                    }
                    if (!string.IsNullOrEmpty(supportedFileWhere.Format))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `format` = @formatWhere");
                        else
                            sql = string.Concat(sql, " `format` = @formatWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@formatWhere", supportedFileWhere.Format);
                    }
                    if (!string.IsNullOrEmpty(supportedFileWhere.MaxWidth))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `max_width` = @maxWidthWhere");
                        else
                            sql = string.Concat(sql, " `max_width` = @maxWidthWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxWidthWhere", supportedFileWhere.MaxWidth);
                    }
                    if (!string.IsNullOrEmpty(supportedFileWhere.MaxHeight))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `max_height` = @maxHeightWhere");
                        else
                            sql = string.Concat(sql, " `max_height` = @maxHeightWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxHeightWhere", supportedFileWhere.MaxHeight);
                    }
                    if (!string.IsNullOrEmpty(supportedFileWhere.MaxSize))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `max_size` = @maxSizeWhere");
                        else
                            sql = string.Concat(sql, " `max_size` = @maxSizeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxSizeWhere", supportedFileWhere.MaxSize);
                    }
                    if (!string.IsNullOrEmpty(supportedFileWhere.Path))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `path` = @pathWhere");
                        else
                            sql = string.Concat(sql, " `path` = @pathWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@pathWhere", supportedFileWhere.Path);
                    }
                    if (!string.IsNullOrEmpty(supportedFileWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", supportedFileWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(supportedFileWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", supportedFileWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(supportedFileWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", supportedFileWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(supportedFile);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<SupportedFileModel>();
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

        public List<SupportedFileModel> Update(string connectionString, List<SupportedFileModel> supportedFileList, List<SupportedFileModel> supportedFileWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SupportedFileModel> response = new List<SupportedFileModel>();
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
                        for (int i = 0; i < supportedFileList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE supported_files SET ";
                                if (!string.IsNullOrEmpty(supportedFileList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, supportedFileList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].Classification))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `classification` = @classification" + i);
                                    else
                                        sql = string.Concat(sql, " `classification` = @classification" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@classification" + i, supportedFileList[i].Classification);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].Format))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `format` = @format" + i);
                                    else
                                        sql = string.Concat(sql, " `format` = @format" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@format" + i, supportedFileList[i].Format);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].MaxWidth))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `max_width` = @maxWidth" + i);
                                    else
                                        sql = string.Concat(sql, " `max_width` = @maxWidth" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@maxWidth" + i, supportedFileList[i].MaxWidth);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].MaxHeight))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `max_height` = @maxHeight" + i);
                                    else
                                        sql = string.Concat(sql, " `max_height` = @maxHeight" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@maxHeight" + i, supportedFileList[i].MaxHeight);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].MaxSize))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `max_size` = @maxSize" + i);
                                    else
                                        sql = string.Concat(sql, " `max_size` = @maxSize" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@maxSize" + i, supportedFileList[i].MaxSize);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].Path))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `path` = @path" + i);
                                    else
                                        sql = string.Concat(sql, " `path` = @path" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@path" + i, supportedFileList[i].Path);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, supportedFileList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, supportedFileList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(supportedFileList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, supportedFileList[i].UpdatedAt);
                                }
                                if (supportedFileWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(supportedFileWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, supportedFileWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFileWhereList[i].Classification))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `classification` = @classificationWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `classification` = @classificationWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@classificationWhere" + i, supportedFileWhereList[i].Classification);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFileWhereList[i].Format))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `format` = @formatWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `format` = @formatWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@formatWhere" + i, supportedFileWhereList[i].Format);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFileWhereList[i].MaxWidth))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `max_width` = @maxWidthWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `max_width` = @maxWidthWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxWidthWhere" + i, supportedFileWhereList[i].MaxWidth);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFileWhereList[i].MaxHeight))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `max_height` = @maxHeightWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `max_height` = @maxHeightWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxHeightWhere" + i, supportedFileWhereList[i].MaxHeight);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFileWhereList[i].MaxSize))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `max_size` = @maxSizeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `max_size` = @maxSizeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxSizeWhere" + i, supportedFileWhereList[i].MaxSize);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFileWhereList[i].Path))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `path` = @pathWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `path` = @pathWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@pathWhere" + i, supportedFileWhereList[i].Path);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFileWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, supportedFileWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFileWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, supportedFileWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFileWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, supportedFileWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(supportedFileList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<SupportedFileModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SupportedFileModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SupportedFileModel>();
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

        public List<SupportedFileModel> Delete(string connectionString, SupportedFileModel supportedFile)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SupportedFileModel> response = new List<SupportedFileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM supported_files  ";
                if (supportedFile != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(supportedFile.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", supportedFile.Id);
                    }
                    if (!string.IsNullOrEmpty(supportedFile.Classification))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `classification` = @classification");
                        else
                            sql = string.Concat(sql, " `classification` = @classification");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@classification", supportedFile.Classification);
                    }
                    if (!string.IsNullOrEmpty(supportedFile.Format))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `format` = @format");
                        else
                            sql = string.Concat(sql, " `format` = @format");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@format", supportedFile.Format);
                    }
                    if (!string.IsNullOrEmpty(supportedFile.MaxWidth))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `max_width` = @maxWidth");
                        else
                            sql = string.Concat(sql, " `max_width` = @maxWidth");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxWidth", supportedFile.MaxWidth);
                    }
                    if (!string.IsNullOrEmpty(supportedFile.MaxHeight))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `max_height` = @maxHeight");
                        else
                            sql = string.Concat(sql, " `max_height` = @maxHeight");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxHeight", supportedFile.MaxHeight);
                    }
                    if (!string.IsNullOrEmpty(supportedFile.MaxSize))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `max_size` = @maxSize");
                        else
                            sql = string.Concat(sql, " `max_size` = @maxSize");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxSize", supportedFile.MaxSize);
                    }
                    if (!string.IsNullOrEmpty(supportedFile.Path))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `path` = @path");
                        else
                            sql = string.Concat(sql, " `path` = @path");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@path", supportedFile.Path);
                    }
                    if (!string.IsNullOrEmpty(supportedFile.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", supportedFile.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(supportedFile.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", supportedFile.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(supportedFile.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", supportedFile.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(supportedFile);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<SupportedFileModel>();
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

        public List<SupportedFileModel> Delete(string connectionString, List<SupportedFileModel> supportedFile)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SupportedFileModel> response = new List<SupportedFileModel>();
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
                        for (int i = 0; i < supportedFile.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM supported_files  ";
                                if (supportedFile[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(supportedFile[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, supportedFile[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFile[i].Classification))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `classification` = @classification" + i);
                                        else
                                            sql = string.Concat(sql, " `classification` = @classification" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@classification" + i, supportedFile[i].Classification);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFile[i].Format))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `format` = @format" + i);
                                        else
                                            sql = string.Concat(sql, " `format` = @format" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@format" + i, supportedFile[i].Format);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFile[i].MaxWidth))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `max_width` = @maxWidth" + i);
                                        else
                                            sql = string.Concat(sql, " `max_width` = @maxWidth" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxWidth" + i, supportedFile[i].MaxWidth);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFile[i].MaxHeight))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `max_height` = @maxHeight" + i);
                                        else
                                            sql = string.Concat(sql, " `max_height` = @maxHeight" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxHeight" + i, supportedFile[i].MaxHeight);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFile[i].MaxSize))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `max_size` = @maxSize" + i);
                                        else
                                            sql = string.Concat(sql, " `max_size` = @maxSize" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxSize" + i, supportedFile[i].MaxSize);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFile[i].Path))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `path` = @path" + i);
                                        else
                                            sql = string.Concat(sql, " `path` = @path" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@path" + i, supportedFile[i].Path);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFile[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, supportedFile[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFile[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, supportedFile[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(supportedFile[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, supportedFile[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(supportedFile[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<SupportedFileModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SupportedFileModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SupportedFileModel>();
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
