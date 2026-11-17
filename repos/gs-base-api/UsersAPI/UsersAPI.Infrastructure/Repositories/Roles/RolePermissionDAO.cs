using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Roles;
using UsersAPI.UsersAPI.Services.Roles.CustomClasses;
namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Roles
{
    class RolePermissionDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, RolePermissionModel> ReadD(string connectionString, RolePermissionModel rolePermission, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, RolePermissionModel> response = new Dictionary<string, RolePermissionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `role_id`, `permission_id`, `modified_by`, `created_at`, `updated_at`  FROM ur_role_permissions ";
                {
                    if (rolePermission != null)
                    {
                        if (!String.IsNullOrEmpty(rolePermission.Id))
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
                            cmd.Parameters.AddWithValue("@id", rolePermission.Id);
                        }
                        if (!String.IsNullOrEmpty(rolePermission.RoleId))
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
                            cmd.Parameters.AddWithValue("@roleId", rolePermission.RoleId);
                        }
                        if (!String.IsNullOrEmpty(rolePermission.PermissionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `permission_id` = @permissionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `permission_id` = @permissionId ");
                            }
                            cmd.Parameters.AddWithValue("@permissionId", rolePermission.PermissionId);
                        }
                        if (!String.IsNullOrEmpty(rolePermission.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", rolePermission.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(rolePermission.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", rolePermission.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(rolePermission.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", rolePermission.UpdatedAt);
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
                                    rolePermission.Id = reader.GetValue(0).ToString();
                                else
                                    rolePermission.Id = null;
                                if (!reader.IsDBNull(1))
                                    rolePermission.RoleId = reader.GetValue(1).ToString();
                                else
                                    rolePermission.RoleId = null;
                                if (!reader.IsDBNull(2))
                                    rolePermission.PermissionId = reader.GetValue(2).ToString();
                                else
                                    rolePermission.PermissionId = null;
                                if (!reader.IsDBNull(3))
                                    rolePermission.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    rolePermission.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    rolePermission.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    rolePermission.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    rolePermission.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    rolePermission.UpdatedAt = null;
                                response.Add(rolePermission.Id, rolePermission);
                                rolePermission = new RolePermissionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, RolePermissionModel>();
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
                response = new Dictionary<string, RolePermissionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, RolePermissionModel> ReadD(string connectionString, List<RolePermissionModel> rolePermissionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, RolePermissionModel> response = new Dictionary<string, RolePermissionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `role_id`, `permission_id`, `modified_by`, `created_at`, `updated_at`  FROM ur_role_permissions ";
                if (rolePermissionWhere != null && rolePermissionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < rolePermissionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(rolePermissionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, rolePermissionWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(rolePermissionWhere[i].RoleId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `role_id` = @roleId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@roleId" + i, rolePermissionWhere[i].RoleId);
                        }
                        if (!String.IsNullOrEmpty(rolePermissionWhere[i].PermissionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `permission_id` = @permissionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@permissionId" + i, rolePermissionWhere[i].PermissionId);
                        }
                        if (!String.IsNullOrEmpty(rolePermissionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, rolePermissionWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(rolePermissionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, rolePermissionWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(rolePermissionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, rolePermissionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != rolePermissionWhere.Count - 1)
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
                            RolePermissionModel rolePermission = new RolePermissionModel(); if (!reader.IsDBNull(0))
                                rolePermission.Id = reader.GetValue(0).ToString();
                            else
                                rolePermission.Id = null;
                            if (!reader.IsDBNull(1))
                                rolePermission.RoleId = reader.GetValue(1).ToString();
                            else
                                rolePermission.RoleId = null;
                            if (!reader.IsDBNull(2))
                                rolePermission.PermissionId = reader.GetValue(2).ToString();
                            else
                                rolePermission.PermissionId = null;
                            if (!reader.IsDBNull(3))
                                rolePermission.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                rolePermission.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                rolePermission.CreatedAt = reader.GetValue(4).ToString();
                            else
                                rolePermission.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                rolePermission.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                rolePermission.UpdatedAt = null;
                            response.Add(rolePermission.Id, rolePermission);
                            rolePermission = new RolePermissionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, RolePermissionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, RolePermissionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RolePermissionModel> Read(string connectionString, RolePermissionModel rolePermission, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RolePermissionModel> response = new List<RolePermissionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `role_id`, `permission_id`, `modified_by`, `created_at`, `updated_at`  FROM ur_role_permissions ";
                {
                    if (rolePermission != null)
                    {
                        if (!String.IsNullOrEmpty(rolePermission.Id))
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
                            cmd.Parameters.AddWithValue("@id", rolePermission.Id);
                        }
                        if (!String.IsNullOrEmpty(rolePermission.RoleId))
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
                            cmd.Parameters.AddWithValue("@roleId", rolePermission.RoleId);
                        }
                        if (!String.IsNullOrEmpty(rolePermission.PermissionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `permission_id` = @permissionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `permission_id` = @permissionId ");
                            }
                            cmd.Parameters.AddWithValue("@permissionId", rolePermission.PermissionId);
                        }
                        if (!String.IsNullOrEmpty(rolePermission.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", rolePermission.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(rolePermission.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", rolePermission.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(rolePermission.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", rolePermission.UpdatedAt);
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
                                    rolePermission.Id = reader.GetValue(0).ToString();
                                else
                                    rolePermission.Id = null;
                                if (!reader.IsDBNull(1))
                                    rolePermission.RoleId = reader.GetValue(1).ToString();
                                else
                                    rolePermission.RoleId = null;
                                if (!reader.IsDBNull(2))
                                    rolePermission.PermissionId = reader.GetValue(2).ToString();
                                else
                                    rolePermission.PermissionId = null;
                                if (!reader.IsDBNull(3))
                                    rolePermission.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    rolePermission.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    rolePermission.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    rolePermission.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    rolePermission.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    rolePermission.UpdatedAt = null;
                                response.Add(rolePermission);
                                rolePermission = new RolePermissionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<RolePermissionModel>();
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
                response = new List<RolePermissionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RolePermissionModel> Read(string connectionString, List<RolePermissionModel> rolePermissionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RolePermissionModel> response = new List<RolePermissionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `role_id`, `permission_id`, `modified_by`, `created_at`, `updated_at`  FROM ur_role_permissions ";
                if (rolePermissionWhere != null && rolePermissionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < rolePermissionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(rolePermissionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, rolePermissionWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(rolePermissionWhere[i].RoleId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `role_id` = @roleId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@roleId" + i, rolePermissionWhere[i].RoleId);
                        }
                        if (!String.IsNullOrEmpty(rolePermissionWhere[i].PermissionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `permission_id` = @permissionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@permissionId" + i, rolePermissionWhere[i].PermissionId);
                        }
                        if (!String.IsNullOrEmpty(rolePermissionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, rolePermissionWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(rolePermissionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, rolePermissionWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(rolePermissionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, rolePermissionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != rolePermissionWhere.Count - 1)
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
                            RolePermissionModel rolePermission = new RolePermissionModel(); if (!reader.IsDBNull(0))
                                rolePermission.Id = reader.GetValue(0).ToString();
                            else
                                rolePermission.Id = null;
                            if (!reader.IsDBNull(1))
                                rolePermission.RoleId = reader.GetValue(1).ToString();
                            else
                                rolePermission.RoleId = null;
                            if (!reader.IsDBNull(2))
                                rolePermission.PermissionId = reader.GetValue(2).ToString();
                            else
                                rolePermission.PermissionId = null;
                            if (!reader.IsDBNull(3))
                                rolePermission.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                rolePermission.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                rolePermission.CreatedAt = reader.GetValue(4).ToString();
                            else
                                rolePermission.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                rolePermission.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                rolePermission.UpdatedAt = null;
                            response.Add(rolePermission);
                            rolePermission = new RolePermissionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<RolePermissionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RolePermissionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RolePermissionModel> Create(string connectionString, RolePermissionModel rolePermission)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RolePermissionModel> response = new List<RolePermissionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO ur_role_permissions (";
                string values = "(";
                if (!String.IsNullOrEmpty(rolePermission.Id))
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
                    cmd.Parameters.AddWithValue("@id", rolePermission.Id);
                }
                if (!String.IsNullOrEmpty(rolePermission.RoleId))
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
                    cmd.Parameters.AddWithValue("@roleId", rolePermission.RoleId);
                }
                if (!String.IsNullOrEmpty(rolePermission.PermissionId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `permission_id` ");
                        values = String.Concat(values, ", @permissionId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `permission_id` ");
                        values = String.Concat(values, " @permissionId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@permissionId", rolePermission.PermissionId);
                }
                if (!String.IsNullOrEmpty(rolePermission.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", rolePermission.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(rolePermission.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", rolePermission.CreatedAt);
                }
                if (!String.IsNullOrEmpty(rolePermission.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", rolePermission.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                rolePermission.Id = cmd.LastInsertedId.ToString();
                response.Add(rolePermission);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RolePermissionModel>();
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
        public List<RolePermissionModel> Create(string connectionString, List<RolePermissionModel> rolePermissionList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RolePermissionModel> response = new List<RolePermissionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < rolePermissionList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO ur_role_permissions (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(rolePermissionList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, rolePermissionList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(rolePermissionList[i].RoleId))
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
                                    cmd.Parameters.AddWithValue("@roleId" + i, rolePermissionList[i].RoleId);
                                }
                                if (!String.IsNullOrEmpty(rolePermissionList[i].PermissionId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `permission_id` ");
                                        values = String.Concat(values, ", @permissionId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `permission_id` ");
                                        values = String.Concat(values, "@permissionId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@permissionId" + i, rolePermissionList[i].PermissionId);
                                }
                                if (!String.IsNullOrEmpty(rolePermissionList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, rolePermissionList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(rolePermissionList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, rolePermissionList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(rolePermissionList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, rolePermissionList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(rolePermissionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<RolePermissionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RolePermissionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RolePermissionModel>();
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

        public List<RolePermissionModel> Update(string connectionString, RolePermissionModel rolePermission, RolePermissionModel rolePermissionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RolePermissionModel> response = new List<RolePermissionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE ur_role_permissions SET ";
                if (!String.IsNullOrEmpty(rolePermission.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", rolePermission.Id);
                }
                if (!String.IsNullOrEmpty(rolePermission.RoleId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `role_id` = @roleId");
                    else
                        sql = String.Concat(sql, " `role_id` = @roleId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@roleId", rolePermission.RoleId);
                }
                if (!String.IsNullOrEmpty(rolePermission.PermissionId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `permission_id` = @permissionId");
                    else
                        sql = String.Concat(sql, " `permission_id` = @permissionId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@permissionId", rolePermission.PermissionId);
                }
                if (!String.IsNullOrEmpty(rolePermission.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", rolePermission.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(rolePermission.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", rolePermission.CreatedAt);
                }
                if (!String.IsNullOrEmpty(rolePermission.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", rolePermission.UpdatedAt);
                }
                if (rolePermissionWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(rolePermissionWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", rolePermissionWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(rolePermissionWhere.RoleId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `role_id` = @roleIdWhere");
                        else
                            sql = String.Concat(sql, " `role_id` = @roleIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@roleIdWhere", rolePermissionWhere.RoleId);
                    }
                    if (!String.IsNullOrEmpty(rolePermissionWhere.PermissionId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `permission_id` = @permissionIdWhere");
                        else
                            sql = String.Concat(sql, " `permission_id` = @permissionIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@permissionIdWhere", rolePermissionWhere.PermissionId);
                    }
                    if (!String.IsNullOrEmpty(rolePermissionWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", rolePermissionWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(rolePermissionWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", rolePermissionWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(rolePermissionWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", rolePermissionWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(rolePermission);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<RolePermissionModel>();
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

        public List<RolePermissionModel> Update(string connectionString, List<RolePermissionModel> rolePermissionList, List<RolePermissionModel> rolePermissionWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RolePermissionModel> response = new List<RolePermissionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < rolePermissionList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE ur_role_permissions SET ";
                                if (!String.IsNullOrEmpty(rolePermissionList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, rolePermissionList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(rolePermissionList[i].RoleId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `role_id` = @roleId" + i);
                                    else
                                        sql = String.Concat(sql, " `role_id` = @roleId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@roleId" + i, rolePermissionList[i].RoleId);
                                }
                                if (!String.IsNullOrEmpty(rolePermissionList[i].PermissionId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `permission_id` = @permissionId" + i);
                                    else
                                        sql = String.Concat(sql, " `permission_id` = @permissionId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@permissionId" + i, rolePermissionList[i].PermissionId);
                                }
                                if (!String.IsNullOrEmpty(rolePermissionList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, rolePermissionList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(rolePermissionList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, rolePermissionList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(rolePermissionList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, rolePermissionList[i].UpdatedAt);
                                }
                                if (rolePermissionWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(rolePermissionWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, rolePermissionWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(rolePermissionWhereList[i].RoleId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `role_id` = @roleIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `role_id` = @roleIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@roleIdWhere" + i, rolePermissionWhereList[i].RoleId);
                                    }
                                    if (!String.IsNullOrEmpty(rolePermissionWhereList[i].PermissionId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `permission_id` = @permissionIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `permission_id` = @permissionIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@permissionIdWhere" + i, rolePermissionWhereList[i].PermissionId);
                                    }
                                    if (!String.IsNullOrEmpty(rolePermissionWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, rolePermissionWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(rolePermissionWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, rolePermissionWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(rolePermissionWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, rolePermissionWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(rolePermissionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<RolePermissionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RolePermissionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RolePermissionModel>();
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

        public List<RolePermissionModel> Delete(string connectionString, RolePermissionModel rolePermission)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RolePermissionModel> response = new List<RolePermissionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM ur_role_permissions  ";
                if (rolePermission != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(rolePermission.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", rolePermission.Id);
                    }
                    if (!String.IsNullOrEmpty(rolePermission.RoleId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `role_id` = @roleId");
                        else
                            sql = String.Concat(sql, " `role_id` = @roleId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@roleId", rolePermission.RoleId);
                    }
                    if (!String.IsNullOrEmpty(rolePermission.PermissionId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `permission_id` = @permissionId");
                        else
                            sql = String.Concat(sql, " `permission_id` = @permissionId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@permissionId", rolePermission.PermissionId);
                    }
                    if (!String.IsNullOrEmpty(rolePermission.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", rolePermission.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(rolePermission.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", rolePermission.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(rolePermission.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", rolePermission.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(rolePermission);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<RolePermissionModel>();
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

        public List<RolePermissionModel> Delete(string connectionString, List<RolePermissionModel> rolePermission)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RolePermissionModel> response = new List<RolePermissionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < rolePermission.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM ur_role_permissions  ";
                                if (rolePermission[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(rolePermission[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, rolePermission[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(rolePermission[i].RoleId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `role_id` = @roleId" + i);
                                        else
                                            sql = String.Concat(sql, " `role_id` = @roleId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@roleId" + i, rolePermission[i].RoleId);
                                    }
                                    if (!String.IsNullOrEmpty(rolePermission[i].PermissionId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `permission_id` = @permissionId" + i);
                                        else
                                            sql = String.Concat(sql, " `permission_id` = @permissionId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@permissionId" + i, rolePermission[i].PermissionId);
                                    }
                                    if (!String.IsNullOrEmpty(rolePermission[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, rolePermission[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(rolePermission[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, rolePermission[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(rolePermission[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, rolePermission[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(rolePermission[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<RolePermissionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RolePermissionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RolePermissionModel>();
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
