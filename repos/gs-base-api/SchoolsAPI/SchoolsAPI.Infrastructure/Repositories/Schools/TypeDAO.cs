using System;
using System.Collections.Generic;
using MySqlConnector;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;

namespace SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools
{
    class TypeDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, TypeModel> ReadD(string connectionString, TypeModel type, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TypeModel> response = new Dictionary<string, TypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_types ";
                {
                    if (type != null)
                    {
                        if (!string.IsNullOrEmpty(type.Id))
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
                            cmd.Parameters.AddWithValue("@id", type.Id);
                        }
                        if (!string.IsNullOrEmpty(type.Name))
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
                            cmd.Parameters.AddWithValue("@name", type.Name);
                        }
                        if (!string.IsNullOrEmpty(type.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", type.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(type.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", type.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(type.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", type.UpdatedAt);
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
                                    type.Id = reader.GetValue(0).ToString();
                                else
                                    type.Id = null;
                                if (!reader.IsDBNull(1))
                                    type.Name = reader.GetValue(1).ToString();
                                else
                                    type.Name = null;
                                if (!reader.IsDBNull(2))
                                    type.ModifiedBy = reader.GetValue(2).ToString();
                                else
                                    type.ModifiedBy = null;
                                if (!reader.IsDBNull(3))
                                    type.CreatedAt = reader.GetValue(3).ToString();
                                else
                                    type.CreatedAt = null;
                                if (!reader.IsDBNull(4))
                                    type.UpdatedAt = reader.GetValue(4).ToString();
                                else
                                    type.UpdatedAt = null;
                                response.Add(type.Id, type);
                                type = new TypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TypeModel>();
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
                response = new Dictionary<string, TypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TypeModel> ReadD(string connectionString, TypeModel type, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TypeModel> response = new Dictionary<string, TypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_types ";
                {
                    if (type != null)
                    {
                        if (!string.IsNullOrEmpty(type.Id))
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
                            cmd.Parameters.AddWithValue("@id", type.Id);
                        }
                        if (!string.IsNullOrEmpty(type.Name))
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
                            cmd.Parameters.AddWithValue("@name", type.Name);
                        }
                        if (!string.IsNullOrEmpty(type.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", type.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(type.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", type.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(type.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", type.UpdatedAt);
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
                                    type.Id = reader.GetValue(0).ToString();
                                else
                                    type.Id = null;
                                if (!reader.IsDBNull(1))
                                    type.Name = reader.GetValue(1).ToString();
                                else
                                    type.Name = null;
                                if (!reader.IsDBNull(2))
                                    type.ModifiedBy = reader.GetValue(2).ToString();
                                else
                                    type.ModifiedBy = null;
                                if (!reader.IsDBNull(3))
                                    type.CreatedAt = reader.GetValue(3).ToString();
                                else
                                    type.CreatedAt = null;
                                if (!reader.IsDBNull(4))
                                    type.UpdatedAt = reader.GetValue(4).ToString();
                                else
                                    type.UpdatedAt = null;
                                response.Add(type.Id, type);
                                type = new TypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TypeModel>();
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
                response = new Dictionary<string, TypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TypeModel> ReadD(string connectionString, List<TypeModel> typeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TypeModel> response = new Dictionary<string, TypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_types ";
                if (typeWhere != null && typeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < typeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(typeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, typeWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, typeWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, typeWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, typeWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, typeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != typeWhere.Count - 1)
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
                            TypeModel type = new TypeModel(); if (!reader.IsDBNull(0))
                                type.Id = reader.GetValue(0).ToString();
                            else
                                type.Id = null;
                            if (!reader.IsDBNull(1))
                                type.Name = reader.GetValue(1).ToString();
                            else
                                type.Name = null;
                            if (!reader.IsDBNull(2))
                                type.ModifiedBy = reader.GetValue(2).ToString();
                            else
                                type.ModifiedBy = null;
                            if (!reader.IsDBNull(3))
                                type.CreatedAt = reader.GetValue(3).ToString();
                            else
                                type.CreatedAt = null;
                            if (!reader.IsDBNull(4))
                                type.UpdatedAt = reader.GetValue(4).ToString();
                            else
                                type.UpdatedAt = null;
                            response.Add(type.Id, type);
                            type = new TypeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TypeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TypeModel> ReadD(string connectionString, List<TypeModel> typeWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TypeModel> response = new Dictionary<string, TypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_types ";
                if (typeWhere != null && typeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < typeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(typeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, typeWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, typeWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, typeWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, typeWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, typeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != typeWhere.Count - 1)
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
                            TypeModel type = new TypeModel(); if (!reader.IsDBNull(0))
                                type.Id = reader.GetValue(0).ToString();
                            else
                                type.Id = null;
                            if (!reader.IsDBNull(1))
                                type.Name = reader.GetValue(1).ToString();
                            else
                                type.Name = null;
                            if (!reader.IsDBNull(2))
                                type.ModifiedBy = reader.GetValue(2).ToString();
                            else
                                type.ModifiedBy = null;
                            if (!reader.IsDBNull(3))
                                type.CreatedAt = reader.GetValue(3).ToString();
                            else
                                type.CreatedAt = null;
                            if (!reader.IsDBNull(4))
                                type.UpdatedAt = reader.GetValue(4).ToString();
                            else
                                type.UpdatedAt = null;
                            response.Add(type.Id, type);
                            type = new TypeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TypeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TypeModel> Read(string connectionString, TypeModel type, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TypeModel> response = new List<TypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_types ";
                {
                    if (type != null)
                    {
                        if (!string.IsNullOrEmpty(type.Id))
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
                            cmd.Parameters.AddWithValue("@id", type.Id);
                        }
                        if (!string.IsNullOrEmpty(type.Name))
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
                            cmd.Parameters.AddWithValue("@name", type.Name);
                        }
                        if (!string.IsNullOrEmpty(type.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", type.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(type.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", type.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(type.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", type.UpdatedAt);
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
                                    type.Id = reader.GetValue(0).ToString();
                                else
                                    type.Id = null;
                                if (!reader.IsDBNull(1))
                                    type.Name = reader.GetValue(1).ToString();
                                else
                                    type.Name = null;
                                if (!reader.IsDBNull(2))
                                    type.ModifiedBy = reader.GetValue(2).ToString();
                                else
                                    type.ModifiedBy = null;
                                if (!reader.IsDBNull(3))
                                    type.CreatedAt = reader.GetValue(3).ToString();
                                else
                                    type.CreatedAt = null;
                                if (!reader.IsDBNull(4))
                                    type.UpdatedAt = reader.GetValue(4).ToString();
                                else
                                    type.UpdatedAt = null;
                                response.Add(type);
                                type = new TypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TypeModel>();
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
                response = new List<TypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TypeModel> Read(string connectionString, TypeModel type, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TypeModel> response = new List<TypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_types ";
                {
                    if (type != null)
                    {
                        if (!string.IsNullOrEmpty(type.Id))
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
                            cmd.Parameters.AddWithValue("@id", type.Id);
                        }
                        if (!string.IsNullOrEmpty(type.Name))
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
                            cmd.Parameters.AddWithValue("@name", type.Name);
                        }
                        if (!string.IsNullOrEmpty(type.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", type.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(type.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", type.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(type.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", type.UpdatedAt);
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
                                    type.Id = reader.GetValue(0).ToString();
                                else
                                    type.Id = null;
                                if (!reader.IsDBNull(1))
                                    type.Name = reader.GetValue(1).ToString();
                                else
                                    type.Name = null;
                                if (!reader.IsDBNull(2))
                                    type.ModifiedBy = reader.GetValue(2).ToString();
                                else
                                    type.ModifiedBy = null;
                                if (!reader.IsDBNull(3))
                                    type.CreatedAt = reader.GetValue(3).ToString();
                                else
                                    type.CreatedAt = null;
                                if (!reader.IsDBNull(4))
                                    type.UpdatedAt = reader.GetValue(4).ToString();
                                else
                                    type.UpdatedAt = null;
                                response.Add(type);
                                type = new TypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TypeModel>();
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
                response = new List<TypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TypeModel> Read(string connectionString, List<TypeModel> typeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TypeModel> response = new List<TypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_types ";
                if (typeWhere != null && typeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < typeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(typeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, typeWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, typeWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, typeWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, typeWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, typeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != typeWhere.Count - 1)
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
                            TypeModel type = new TypeModel(); if (!reader.IsDBNull(0))
                                type.Id = reader.GetValue(0).ToString();
                            else
                                type.Id = null;
                            if (!reader.IsDBNull(1))
                                type.Name = reader.GetValue(1).ToString();
                            else
                                type.Name = null;
                            if (!reader.IsDBNull(2))
                                type.ModifiedBy = reader.GetValue(2).ToString();
                            else
                                type.ModifiedBy = null;
                            if (!reader.IsDBNull(3))
                                type.CreatedAt = reader.GetValue(3).ToString();
                            else
                                type.CreatedAt = null;
                            if (!reader.IsDBNull(4))
                                type.UpdatedAt = reader.GetValue(4).ToString();
                            else
                                type.UpdatedAt = null;
                            response.Add(type);
                            type = new TypeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TypeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TypeModel> Read(string connectionString, List<TypeModel> typeWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TypeModel> response = new List<TypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_types ";
                if (typeWhere != null && typeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < typeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(typeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, typeWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, typeWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, typeWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, typeWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(typeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, typeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != typeWhere.Count - 1)
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
                            TypeModel type = new TypeModel(); if (!reader.IsDBNull(0))
                                type.Id = reader.GetValue(0).ToString();
                            else
                                type.Id = null;
                            if (!reader.IsDBNull(1))
                                type.Name = reader.GetValue(1).ToString();
                            else
                                type.Name = null;
                            if (!reader.IsDBNull(2))
                                type.ModifiedBy = reader.GetValue(2).ToString();
                            else
                                type.ModifiedBy = null;
                            if (!reader.IsDBNull(3))
                                type.CreatedAt = reader.GetValue(3).ToString();
                            else
                                type.CreatedAt = null;
                            if (!reader.IsDBNull(4))
                                type.UpdatedAt = reader.GetValue(4).ToString();
                            else
                                type.UpdatedAt = null;
                            response.Add(type);
                            type = new TypeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TypeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TypeModel> Create(string connectionString, TypeModel type)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TypeModel> response = new List<TypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO sch_types (";
                string values = "(";
                if (!string.IsNullOrEmpty(type.Id))
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
                    cmd.Parameters.AddWithValue("@id", type.Id);
                }
                if (!string.IsNullOrEmpty(type.Name))
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
                    cmd.Parameters.AddWithValue("@name", type.Name);
                }
                if (!string.IsNullOrEmpty(type.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", type.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(type.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", type.CreatedAt);
                }
                if (!string.IsNullOrEmpty(type.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", type.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                type.Id = cmd.LastInsertedId.ToString();
                response.Add(type);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TypeModel>();
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
        public List<TypeModel> Create(string connectionString, List<TypeModel> typeList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TypeModel> response = new List<TypeModel>();
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
                        for (int i = 0; i < typeList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO sch_types (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(typeList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, typeList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(typeList[i].Name))
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
                                    cmd.Parameters.AddWithValue("@name" + i, typeList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(typeList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, typeList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(typeList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, typeList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(typeList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, typeList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(typeList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<TypeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TypeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TypeModel>();
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

        public List<TypeModel> Update(string connectionString, TypeModel type, TypeModel typeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TypeModel> response = new List<TypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE sch_types SET ";
                if (!string.IsNullOrEmpty(type.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", type.Id);
                }
                if (!string.IsNullOrEmpty(type.Name))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `name` = @name");
                    else
                        sql = string.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", type.Name);
                }
                if (!string.IsNullOrEmpty(type.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", type.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(type.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", type.CreatedAt);
                }
                if (!string.IsNullOrEmpty(type.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", type.UpdatedAt);
                }
                if (typeWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(typeWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", typeWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(typeWhere.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = string.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", typeWhere.Name);
                    }
                    if (!string.IsNullOrEmpty(typeWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", typeWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(typeWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", typeWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(typeWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", typeWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(type);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TypeModel>();
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

        public List<TypeModel> Update(string connectionString, List<TypeModel> typeList, List<TypeModel> typeWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TypeModel> response = new List<TypeModel>();
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
                        for (int i = 0; i < typeList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE sch_types SET ";
                                if (!string.IsNullOrEmpty(typeList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, typeList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(typeList[i].Name))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = string.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, typeList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(typeList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, typeList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(typeList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, typeList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(typeList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, typeList[i].UpdatedAt);
                                }
                                if (typeWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(typeWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, typeWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(typeWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, typeWhereList[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(typeWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, typeWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(typeWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, typeWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(typeWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, typeWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(typeList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TypeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TypeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TypeModel>();
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

        public List<TypeModel> Delete(string connectionString, TypeModel type)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TypeModel> response = new List<TypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM sch_types  ";
                if (type != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(type.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", type.Id);
                    }
                    if (!string.IsNullOrEmpty(type.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @name");
                        else
                            sql = string.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", type.Name);
                    }
                    if (!string.IsNullOrEmpty(type.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", type.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(type.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", type.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(type.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", type.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(type);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TypeModel>();
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

        public List<TypeModel> Delete(string connectionString, List<TypeModel> type)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TypeModel> response = new List<TypeModel>();
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
                        for (int i = 0; i < type.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM sch_types  ";
                                if (type[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(type[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, type[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(type[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, type[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(type[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, type[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(type[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, type[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(type[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, type[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(type[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TypeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TypeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TypeModel>();
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
