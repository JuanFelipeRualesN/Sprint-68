using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts
{
    class StatusAccountDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, StatusAccountModel> ReadD(string connectionString, StatusAccountModel statusAccount, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, StatusAccountModel> response = new Dictionary<string, StatusAccountModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `desc`, `code`, `allow_access`, `modified_by`, `created_at`, `updated_at`  FROM ul_status_accounts ";
                {
                    if (statusAccount != null)
                    {
                        if (!String.IsNullOrEmpty(statusAccount.Id))
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
                            cmd.Parameters.AddWithValue("@id", statusAccount.Id);
                        }
                        if (!String.IsNullOrEmpty(statusAccount.Desc))
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
                            cmd.Parameters.AddWithValue("@desc", statusAccount.Desc);
                        }
                        if (!String.IsNullOrEmpty(statusAccount.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", statusAccount.Code);
                        }
                        if (!String.IsNullOrEmpty(statusAccount.AllowAccess))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `allow_access` = @allowAccess ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `allow_access` = @allowAccess ");
                            }
                            cmd.Parameters.AddWithValue("@allowAccess", statusAccount.AllowAccess);
                        }
                        if (!String.IsNullOrEmpty(statusAccount.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", statusAccount.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(statusAccount.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", statusAccount.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(statusAccount.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", statusAccount.UpdatedAt);
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
                                    statusAccount.Id = reader.GetValue(0).ToString();
                                else
                                    statusAccount.Id = null;
                                if (!reader.IsDBNull(1))
                                    statusAccount.Desc = reader.GetValue(1).ToString();
                                else
                                    statusAccount.Desc = null;
                                if (!reader.IsDBNull(2))
                                    statusAccount.Code = reader.GetValue(2).ToString();
                                else
                                    statusAccount.Code = null;
                                if (!reader.IsDBNull(3))
                                    statusAccount.AllowAccess = reader.GetValue(3).ToString();
                                else
                                    statusAccount.AllowAccess = null;
                                if (!reader.IsDBNull(4))
                                    statusAccount.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    statusAccount.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    statusAccount.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    statusAccount.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    statusAccount.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    statusAccount.UpdatedAt = null;
                                response.Add(statusAccount.Id, statusAccount);
                                statusAccount = new StatusAccountModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, StatusAccountModel>();
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
                response = new Dictionary<string, StatusAccountModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, StatusAccountModel> ReadD(string connectionString, List<StatusAccountModel> statusAccountWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, StatusAccountModel> response = new Dictionary<string, StatusAccountModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `desc`, `code`, `allow_access`, `modified_by`, `created_at`, `updated_at`  FROM ul_status_accounts ";
                if (statusAccountWhere != null && statusAccountWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < statusAccountWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(statusAccountWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, statusAccountWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(statusAccountWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, statusAccountWhere[i].Desc);
                        }
                        if (!String.IsNullOrEmpty(statusAccountWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, statusAccountWhere[i].Code);
                        }
                        if (!String.IsNullOrEmpty(statusAccountWhere[i].AllowAccess))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `allow_access` = @allowAccess" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@allowAccess" + i, statusAccountWhere[i].AllowAccess);
                        }
                        if (!String.IsNullOrEmpty(statusAccountWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, statusAccountWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(statusAccountWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, statusAccountWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(statusAccountWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, statusAccountWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != statusAccountWhere.Count - 1)
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
                            StatusAccountModel statusAccount = new StatusAccountModel(); if (!reader.IsDBNull(0))
                                statusAccount.Id = reader.GetValue(0).ToString();
                            else
                                statusAccount.Id = null;
                            if (!reader.IsDBNull(1))
                                statusAccount.Desc = reader.GetValue(1).ToString();
                            else
                                statusAccount.Desc = null;
                            if (!reader.IsDBNull(2))
                                statusAccount.Code = reader.GetValue(2).ToString();
                            else
                                statusAccount.Code = null;
                            if (!reader.IsDBNull(3))
                                statusAccount.AllowAccess = reader.GetValue(3).ToString();
                            else
                                statusAccount.AllowAccess = null;
                            if (!reader.IsDBNull(4))
                                statusAccount.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                statusAccount.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                statusAccount.CreatedAt = reader.GetValue(5).ToString();
                            else
                                statusAccount.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                statusAccount.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                statusAccount.UpdatedAt = null;
                            response.Add(statusAccount.Id, statusAccount);
                            statusAccount = new StatusAccountModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, StatusAccountModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, StatusAccountModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<StatusAccountModel> Read(string connectionString, StatusAccountModel statusAccount, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountModel> response = new List<StatusAccountModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `desc`, `code`, `allow_access`, `modified_by`, `created_at`, `updated_at`  FROM ul_status_accounts ";
                {
                    if (statusAccount != null)
                    {
                        if (!String.IsNullOrEmpty(statusAccount.Id))
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
                            cmd.Parameters.AddWithValue("@id", statusAccount.Id);
                        }
                        if (!String.IsNullOrEmpty(statusAccount.Desc))
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
                            cmd.Parameters.AddWithValue("@desc", statusAccount.Desc);
                        }
                        if (!String.IsNullOrEmpty(statusAccount.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", statusAccount.Code);
                        }
                        if (!String.IsNullOrEmpty(statusAccount.AllowAccess))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `allow_access` = @allowAccess ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `allow_access` = @allowAccess ");
                            }
                            cmd.Parameters.AddWithValue("@allowAccess", statusAccount.AllowAccess);
                        }
                        if (!String.IsNullOrEmpty(statusAccount.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", statusAccount.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(statusAccount.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", statusAccount.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(statusAccount.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", statusAccount.UpdatedAt);
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
                                    statusAccount.Id = reader.GetValue(0).ToString();
                                else
                                    statusAccount.Id = null;
                                if (!reader.IsDBNull(1))
                                    statusAccount.Desc = reader.GetValue(1).ToString();
                                else
                                    statusAccount.Desc = null;
                                if (!reader.IsDBNull(2))
                                    statusAccount.Code = reader.GetValue(2).ToString();
                                else
                                    statusAccount.Code = null;
                                if (!reader.IsDBNull(3))
                                    statusAccount.AllowAccess = reader.GetValue(3).ToString();
                                else
                                    statusAccount.AllowAccess = null;
                                if (!reader.IsDBNull(4))
                                    statusAccount.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    statusAccount.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    statusAccount.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    statusAccount.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    statusAccount.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    statusAccount.UpdatedAt = null;
                                response.Add(statusAccount);
                                statusAccount = new StatusAccountModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<StatusAccountModel>();
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
                response = new List<StatusAccountModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<StatusAccountModel> Read(string connectionString, List<StatusAccountModel> statusAccountWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountModel> response = new List<StatusAccountModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `desc`, `code`, `allow_access`, `modified_by`, `created_at`, `updated_at`  FROM ul_status_accounts ";
                if (statusAccountWhere != null && statusAccountWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < statusAccountWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(statusAccountWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, statusAccountWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(statusAccountWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, statusAccountWhere[i].Desc);
                        }
                        if (!String.IsNullOrEmpty(statusAccountWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, statusAccountWhere[i].Code);
                        }
                        if (!String.IsNullOrEmpty(statusAccountWhere[i].AllowAccess))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `allow_access` = @allowAccess" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@allowAccess" + i, statusAccountWhere[i].AllowAccess);
                        }
                        if (!String.IsNullOrEmpty(statusAccountWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, statusAccountWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(statusAccountWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, statusAccountWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(statusAccountWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, statusAccountWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != statusAccountWhere.Count - 1)
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
                            StatusAccountModel statusAccount = new StatusAccountModel(); if (!reader.IsDBNull(0))
                                statusAccount.Id = reader.GetValue(0).ToString();
                            else
                                statusAccount.Id = null;
                            if (!reader.IsDBNull(1))
                                statusAccount.Desc = reader.GetValue(1).ToString();
                            else
                                statusAccount.Desc = null;
                            if (!reader.IsDBNull(2))
                                statusAccount.Code = reader.GetValue(2).ToString();
                            else
                                statusAccount.Code = null;
                            if (!reader.IsDBNull(3))
                                statusAccount.AllowAccess = reader.GetValue(3).ToString();
                            else
                                statusAccount.AllowAccess = null;
                            if (!reader.IsDBNull(4))
                                statusAccount.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                statusAccount.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                statusAccount.CreatedAt = reader.GetValue(5).ToString();
                            else
                                statusAccount.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                statusAccount.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                statusAccount.UpdatedAt = null;
                            response.Add(statusAccount);
                            statusAccount = new StatusAccountModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<StatusAccountModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<StatusAccountModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<StatusAccountModel> Create(string connectionString, StatusAccountModel statusAccount)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountModel> response = new List<StatusAccountModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO ul_status_accounts (";
                string values = "(";
                if (!String.IsNullOrEmpty(statusAccount.Id))
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
                    cmd.Parameters.AddWithValue("@id", statusAccount.Id);
                }
                if (!String.IsNullOrEmpty(statusAccount.Desc))
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
                    cmd.Parameters.AddWithValue("@desc", statusAccount.Desc);
                }
                if (!String.IsNullOrEmpty(statusAccount.Code))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `code` ");
                        values = String.Concat(values, ", @code");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `code` ");
                        values = String.Concat(values, " @code");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", statusAccount.Code);
                }
                if (!String.IsNullOrEmpty(statusAccount.AllowAccess))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `allow_access` ");
                        values = String.Concat(values, ", @allowAccess");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `allow_access` ");
                        values = String.Concat(values, " @allowAccess");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@allowAccess", statusAccount.AllowAccess);
                }
                if (!String.IsNullOrEmpty(statusAccount.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", statusAccount.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(statusAccount.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", statusAccount.CreatedAt);
                }
                if (!String.IsNullOrEmpty(statusAccount.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", statusAccount.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                statusAccount.Id = cmd.LastInsertedId.ToString();
                response.Add(statusAccount);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<StatusAccountModel>();
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
        public List<StatusAccountModel> Create(string connectionString, List<StatusAccountModel> statusAccountList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountModel> response = new List<StatusAccountModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < statusAccountList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO ul_status_accounts (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(statusAccountList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, statusAccountList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(statusAccountList[i].Desc))
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
                                    cmd.Parameters.AddWithValue("@desc" + i, statusAccountList[i].Desc);
                                }
                                if (!String.IsNullOrEmpty(statusAccountList[i].Code))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `code` ");
                                        values = String.Concat(values, ", @code" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `code` ");
                                        values = String.Concat(values, "@code" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, statusAccountList[i].Code);
                                }
                                if (!String.IsNullOrEmpty(statusAccountList[i].AllowAccess))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `allow_access` ");
                                        values = String.Concat(values, ", @allowAccess" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `allow_access` ");
                                        values = String.Concat(values, "@allowAccess" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@allowAccess" + i, statusAccountList[i].AllowAccess);
                                }
                                if (!String.IsNullOrEmpty(statusAccountList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, statusAccountList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(statusAccountList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, statusAccountList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(statusAccountList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, statusAccountList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(statusAccountList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<StatusAccountModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<StatusAccountModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<StatusAccountModel>();
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

        public List<StatusAccountModel> Update(string connectionString, StatusAccountModel statusAccount, StatusAccountModel statusAccountWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountModel> response = new List<StatusAccountModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE ul_status_accounts SET ";
                if (!String.IsNullOrEmpty(statusAccount.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", statusAccount.Id);
                }
                if (!String.IsNullOrEmpty(statusAccount.Desc))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `desc` = @desc");
                    else
                        sql = String.Concat(sql, " `desc` = @desc");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", statusAccount.Desc);
                }
                if (!String.IsNullOrEmpty(statusAccount.Code))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `code` = @code");
                    else
                        sql = String.Concat(sql, " `code` = @code");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", statusAccount.Code);
                }
                if (!String.IsNullOrEmpty(statusAccount.AllowAccess))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `allow_access` = @allowAccess");
                    else
                        sql = String.Concat(sql, " `allow_access` = @allowAccess");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@allowAccess", statusAccount.AllowAccess);
                }
                if (!String.IsNullOrEmpty(statusAccount.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", statusAccount.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(statusAccount.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", statusAccount.CreatedAt);
                }
                if (!String.IsNullOrEmpty(statusAccount.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", statusAccount.UpdatedAt);
                }
                if (statusAccountWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(statusAccountWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", statusAccountWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(statusAccountWhere.Desc))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `desc` = @descWhere");
                        else
                            sql = String.Concat(sql, " `desc` = @descWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descWhere", statusAccountWhere.Desc);
                    }
                    if (!String.IsNullOrEmpty(statusAccountWhere.Code))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `code` = @codeWhere");
                        else
                            sql = String.Concat(sql, " `code` = @codeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@codeWhere", statusAccountWhere.Code);
                    }
                    if (!String.IsNullOrEmpty(statusAccountWhere.AllowAccess))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `allow_access` = @allowAccessWhere");
                        else
                            sql = String.Concat(sql, " `allow_access` = @allowAccessWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@allowAccessWhere", statusAccountWhere.AllowAccess);
                    }
                    if (!String.IsNullOrEmpty(statusAccountWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", statusAccountWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(statusAccountWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", statusAccountWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(statusAccountWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", statusAccountWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(statusAccount);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<StatusAccountModel>();
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

        public List<StatusAccountModel> Update(string connectionString, List<StatusAccountModel> statusAccountList, List<StatusAccountModel> statusAccountWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountModel> response = new List<StatusAccountModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < statusAccountList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE ul_status_accounts SET ";
                                if (!String.IsNullOrEmpty(statusAccountList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, statusAccountList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(statusAccountList[i].Desc))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `desc` = @desc" + i);
                                    else
                                        sql = String.Concat(sql, " `desc` = @desc" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, statusAccountList[i].Desc);
                                }
                                if (!String.IsNullOrEmpty(statusAccountList[i].Code))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `code` = @code" + i);
                                    else
                                        sql = String.Concat(sql, " `code` = @code" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, statusAccountList[i].Code);
                                }
                                if (!String.IsNullOrEmpty(statusAccountList[i].AllowAccess))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `allow_access` = @allowAccess" + i);
                                    else
                                        sql = String.Concat(sql, " `allow_access` = @allowAccess" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@allowAccess" + i, statusAccountList[i].AllowAccess);
                                }
                                if (!String.IsNullOrEmpty(statusAccountList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, statusAccountList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(statusAccountList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, statusAccountList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(statusAccountList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, statusAccountList[i].UpdatedAt);
                                }
                                if (statusAccountWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(statusAccountWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, statusAccountWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountWhereList[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `desc` = @descWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `desc` = @descWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descWhere" + i, statusAccountWhereList[i].Desc);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountWhereList[i].Code))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `code` = @codeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `code` = @codeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@codeWhere" + i, statusAccountWhereList[i].Code);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountWhereList[i].AllowAccess))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `allow_access` = @allowAccessWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `allow_access` = @allowAccessWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@allowAccessWhere" + i, statusAccountWhereList[i].AllowAccess);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, statusAccountWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, statusAccountWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, statusAccountWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(statusAccountList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<StatusAccountModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<StatusAccountModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<StatusAccountModel>();
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

        public List<StatusAccountModel> Delete(string connectionString, StatusAccountModel statusAccount)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountModel> response = new List<StatusAccountModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM ul_status_accounts  ";
                if (statusAccount != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(statusAccount.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", statusAccount.Id);
                    }
                    if (!String.IsNullOrEmpty(statusAccount.Desc))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `desc` = @desc");
                        else
                            sql = String.Concat(sql, " `desc` = @desc");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@desc", statusAccount.Desc);
                    }
                    if (!String.IsNullOrEmpty(statusAccount.Code))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `code` = @code");
                        else
                            sql = String.Concat(sql, " `code` = @code");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@code", statusAccount.Code);
                    }
                    if (!String.IsNullOrEmpty(statusAccount.AllowAccess))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `allow_access` = @allowAccess");
                        else
                            sql = String.Concat(sql, " `allow_access` = @allowAccess");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@allowAccess", statusAccount.AllowAccess);
                    }
                    if (!String.IsNullOrEmpty(statusAccount.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", statusAccount.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(statusAccount.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", statusAccount.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(statusAccount.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", statusAccount.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(statusAccount);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<StatusAccountModel>();
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

        public List<StatusAccountModel> Delete(string connectionString, List<StatusAccountModel> statusAccount)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountModel> response = new List<StatusAccountModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < statusAccount.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM ul_status_accounts  ";
                                if (statusAccount[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(statusAccount[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, statusAccount[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccount[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `desc` = @desc" + i);
                                        else
                                            sql = String.Concat(sql, " `desc` = @desc" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@desc" + i, statusAccount[i].Desc);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccount[i].Code))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `code` = @code" + i);
                                        else
                                            sql = String.Concat(sql, " `code` = @code" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@code" + i, statusAccount[i].Code);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccount[i].AllowAccess))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `allow_access` = @allowAccess" + i);
                                        else
                                            sql = String.Concat(sql, " `allow_access` = @allowAccess" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@allowAccess" + i, statusAccount[i].AllowAccess);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccount[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, statusAccount[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccount[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, statusAccount[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccount[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, statusAccount[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(statusAccount[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<StatusAccountModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<StatusAccountModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<StatusAccountModel>();
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
