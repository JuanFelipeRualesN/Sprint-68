using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Roles;
using UsersAPI.UsersAPI.Services.Roles.CustomClasses;
namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Roles
{
    class RoleTypeDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, RoleTypeModel> ReadD(string connectionString, RoleTypeModel roleType, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, RoleTypeModel> response = new Dictionary<string, RoleTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `name`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ur_role_types ";
                {
                    if (roleType != null)
                    {
                        if (!String.IsNullOrEmpty(roleType.Id))
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
                            cmd.Parameters.AddWithValue("@id", roleType.Id);
                        }
                        if (!String.IsNullOrEmpty(roleType.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", roleType.Name);
                        }
                        if (!String.IsNullOrEmpty(roleType.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", roleType.Desc);
                        }
                        if (!String.IsNullOrEmpty(roleType.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", roleType.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(roleType.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", roleType.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(roleType.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", roleType.UpdatedAt);
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
                                    roleType.Id = reader.GetValue(0).ToString();
                                else
                                    roleType.Id = null;
                                if (!reader.IsDBNull(1))
                                    roleType.Name = reader.GetValue(1).ToString();
                                else
                                    roleType.Name = null;
                                if (!reader.IsDBNull(2))
                                    roleType.Desc = reader.GetValue(2).ToString();
                                else
                                    roleType.Desc = null;
                                if (!reader.IsDBNull(3))
                                    roleType.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    roleType.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    roleType.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    roleType.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    roleType.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    roleType.UpdatedAt = null;
                                response.Add(roleType.Id, roleType);
                                roleType = new RoleTypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, RoleTypeModel>();
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
                response = new Dictionary<string, RoleTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, RoleTypeModel> ReadD(string connectionString, List<RoleTypeModel> roleTypeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, RoleTypeModel> response = new Dictionary<string, RoleTypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ur_role_types ";
                if (roleTypeWhere != null && roleTypeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < roleTypeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(roleTypeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, roleTypeWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(roleTypeWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, roleTypeWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(roleTypeWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, roleTypeWhere[i].Desc);
                        }
                        if (!String.IsNullOrEmpty(roleTypeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, roleTypeWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(roleTypeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, roleTypeWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(roleTypeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, roleTypeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != roleTypeWhere.Count - 1)
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
                            RoleTypeModel roleType = new RoleTypeModel(); if (!reader.IsDBNull(0))
                                roleType.Id = reader.GetValue(0).ToString();
                            else
                                roleType.Id = null;
                            if (!reader.IsDBNull(1))
                                roleType.Name = reader.GetValue(1).ToString();
                            else
                                roleType.Name = null;
                            if (!reader.IsDBNull(2))
                                roleType.Desc = reader.GetValue(2).ToString();
                            else
                                roleType.Desc = null;
                            if (!reader.IsDBNull(3))
                                roleType.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                roleType.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                roleType.CreatedAt = reader.GetValue(4).ToString();
                            else
                                roleType.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                roleType.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                roleType.UpdatedAt = null;
                            response.Add(roleType.Id, roleType);
                            roleType = new RoleTypeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, RoleTypeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, RoleTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RoleTypeModel> Read(string connectionString, RoleTypeModel roleType, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RoleTypeModel> response = new List<RoleTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `name`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ur_role_types ";
                {
                    if (roleType != null)
                    {
                        if (!String.IsNullOrEmpty(roleType.Id))
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
                            cmd.Parameters.AddWithValue("@id", roleType.Id);
                        }
                        if (!String.IsNullOrEmpty(roleType.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", roleType.Name);
                        }
                        if (!String.IsNullOrEmpty(roleType.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", roleType.Desc);
                        }
                        if (!String.IsNullOrEmpty(roleType.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", roleType.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(roleType.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", roleType.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(roleType.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", roleType.UpdatedAt);
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
                                    roleType.Id = reader.GetValue(0).ToString();
                                else
                                    roleType.Id = null;
                                if (!reader.IsDBNull(1))
                                    roleType.Name = reader.GetValue(1).ToString();
                                else
                                    roleType.Name = null;
                                if (!reader.IsDBNull(2))
                                    roleType.Desc = reader.GetValue(2).ToString();
                                else
                                    roleType.Desc = null;
                                if (!reader.IsDBNull(3))
                                    roleType.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    roleType.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    roleType.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    roleType.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    roleType.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    roleType.UpdatedAt = null;
                                response.Add(roleType);
                                roleType = new RoleTypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<RoleTypeModel>();
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
                response = new List<RoleTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RoleTypeModel> Read(string connectionString, List<RoleTypeModel> roleTypeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RoleTypeModel> response = new List<RoleTypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ur_role_types ";
                if (roleTypeWhere != null && roleTypeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < roleTypeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(roleTypeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, roleTypeWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(roleTypeWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, roleTypeWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(roleTypeWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, roleTypeWhere[i].Desc);
                        }
                        if (!String.IsNullOrEmpty(roleTypeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, roleTypeWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(roleTypeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, roleTypeWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(roleTypeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, roleTypeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != roleTypeWhere.Count - 1)
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
                            RoleTypeModel roleType = new RoleTypeModel(); if (!reader.IsDBNull(0))
                                roleType.Id = reader.GetValue(0).ToString();
                            else
                                roleType.Id = null;
                            if (!reader.IsDBNull(1))
                                roleType.Name = reader.GetValue(1).ToString();
                            else
                                roleType.Name = null;
                            if (!reader.IsDBNull(2))
                                roleType.Desc = reader.GetValue(2).ToString();
                            else
                                roleType.Desc = null;
                            if (!reader.IsDBNull(3))
                                roleType.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                roleType.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                roleType.CreatedAt = reader.GetValue(4).ToString();
                            else
                                roleType.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                roleType.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                roleType.UpdatedAt = null;
                            response.Add(roleType);
                            roleType = new RoleTypeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<RoleTypeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RoleTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RoleTypeModel> Create(string connectionString, RoleTypeModel roleType)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RoleTypeModel> response = new List<RoleTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO ur_role_types (";
                string values = "(";
                if (!String.IsNullOrEmpty(roleType.Id))
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
                    cmd.Parameters.AddWithValue("@id", roleType.Id);
                }
                if (!String.IsNullOrEmpty(roleType.Name))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `name` ");
                        values = String.Concat(values, ", @name");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `name` ");
                        values = String.Concat(values, " @name");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", roleType.Name);
                }
                if (!String.IsNullOrEmpty(roleType.Desc))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `desc` ");
                        values = String.Concat(values, ", @desc");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `desc` ");
                        values = String.Concat(values, " @desc");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", roleType.Desc);
                }
                if (!String.IsNullOrEmpty(roleType.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", roleType.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(roleType.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", roleType.CreatedAt);
                }
                if (!String.IsNullOrEmpty(roleType.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", roleType.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                roleType.Id = cmd.LastInsertedId.ToString();
                response.Add(roleType);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RoleTypeModel>();
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
        public List<RoleTypeModel> Create(string connectionString, List<RoleTypeModel> roleTypeList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RoleTypeModel> response = new List<RoleTypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < roleTypeList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO ur_role_types (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(roleTypeList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, roleTypeList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(roleTypeList[i].Name))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `name` ");
                                        values = String.Concat(values, ", @name" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `name` ");
                                        values = String.Concat(values, "@name" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, roleTypeList[i].Name);
                                }
                                if (!String.IsNullOrEmpty(roleTypeList[i].Desc))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `desc` ");
                                        values = String.Concat(values, ", @desc" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `desc` ");
                                        values = String.Concat(values, "@desc" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, roleTypeList[i].Desc);
                                }
                                if (!String.IsNullOrEmpty(roleTypeList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, roleTypeList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(roleTypeList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, roleTypeList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(roleTypeList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, roleTypeList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(roleTypeList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<RoleTypeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RoleTypeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RoleTypeModel>();
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

        public List<RoleTypeModel> Update(string connectionString, RoleTypeModel roleType, RoleTypeModel roleTypeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RoleTypeModel> response = new List<RoleTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE ur_role_types SET ";
                if (!String.IsNullOrEmpty(roleType.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", roleType.Id);
                }
                if (!String.IsNullOrEmpty(roleType.Name))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `name` = @name");
                    else
                        sql = String.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", roleType.Name);
                }
                if (!String.IsNullOrEmpty(roleType.Desc))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `desc` = @desc");
                    else
                        sql = String.Concat(sql, " `desc` = @desc");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", roleType.Desc);
                }
                if (!String.IsNullOrEmpty(roleType.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", roleType.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(roleType.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", roleType.CreatedAt);
                }
                if (!String.IsNullOrEmpty(roleType.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", roleType.UpdatedAt);
                }
                if (roleTypeWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(roleTypeWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", roleTypeWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(roleTypeWhere.Name))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = String.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", roleTypeWhere.Name);
                    }
                    if (!String.IsNullOrEmpty(roleTypeWhere.Desc))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `desc` = @descWhere");
                        else
                            sql = String.Concat(sql, " `desc` = @descWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descWhere", roleTypeWhere.Desc);
                    }
                    if (!String.IsNullOrEmpty(roleTypeWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", roleTypeWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(roleTypeWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", roleTypeWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(roleTypeWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", roleTypeWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(roleType);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<RoleTypeModel>();
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

        public List<RoleTypeModel> Update(string connectionString, List<RoleTypeModel> roleTypeList, List<RoleTypeModel> roleTypeWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RoleTypeModel> response = new List<RoleTypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < roleTypeList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE ur_role_types SET ";
                                if (!String.IsNullOrEmpty(roleTypeList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, roleTypeList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(roleTypeList[i].Name))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = String.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, roleTypeList[i].Name);
                                }
                                if (!String.IsNullOrEmpty(roleTypeList[i].Desc))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `desc` = @desc" + i);
                                    else
                                        sql = String.Concat(sql, " `desc` = @desc" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, roleTypeList[i].Desc);
                                }
                                if (!String.IsNullOrEmpty(roleTypeList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, roleTypeList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(roleTypeList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, roleTypeList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(roleTypeList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, roleTypeList[i].UpdatedAt);
                                }
                                if (roleTypeWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(roleTypeWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, roleTypeWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(roleTypeWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, roleTypeWhereList[i].Name);
                                    }
                                    if (!String.IsNullOrEmpty(roleTypeWhereList[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `desc` = @descWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `desc` = @descWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descWhere" + i, roleTypeWhereList[i].Desc);
                                    }
                                    if (!String.IsNullOrEmpty(roleTypeWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, roleTypeWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(roleTypeWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, roleTypeWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(roleTypeWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, roleTypeWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(roleTypeList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<RoleTypeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RoleTypeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RoleTypeModel>();
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

        public List<RoleTypeModel> Delete(string connectionString, RoleTypeModel roleType)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RoleTypeModel> response = new List<RoleTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM ur_role_types  ";
                if (roleType != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(roleType.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", roleType.Id);
                    }
                    if (!String.IsNullOrEmpty(roleType.Name))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `name` = @name");
                        else
                            sql = String.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", roleType.Name);
                    }
                    if (!String.IsNullOrEmpty(roleType.Desc))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `desc` = @desc");
                        else
                            sql = String.Concat(sql, " `desc` = @desc");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@desc", roleType.Desc);
                    }
                    if (!String.IsNullOrEmpty(roleType.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", roleType.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(roleType.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", roleType.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(roleType.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", roleType.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(roleType);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<RoleTypeModel>();
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

        public List<RoleTypeModel> Delete(string connectionString, List<RoleTypeModel> roleType)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RoleTypeModel> response = new List<RoleTypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < roleType.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM ur_role_types  ";
                                if (roleType[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(roleType[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, roleType[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(roleType[i].Name))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = String.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, roleType[i].Name);
                                    }
                                    if (!String.IsNullOrEmpty(roleType[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `desc` = @desc" + i);
                                        else
                                            sql = String.Concat(sql, " `desc` = @desc" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@desc" + i, roleType[i].Desc);
                                    }
                                    if (!String.IsNullOrEmpty(roleType[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, roleType[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(roleType[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, roleType[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(roleType[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, roleType[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(roleType[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<RoleTypeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RoleTypeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RoleTypeModel>();
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
