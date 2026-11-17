using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts
{
    class StatusAccountLangDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, StatusAccountLangModel> ReadD(string connectionString, StatusAccountLangModel statusAccountLang, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, StatusAccountLangModel> response = new Dictionary<string, StatusAccountLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `status_id`, `lang_code`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ul_status_account_langs ";
                {
                    if (statusAccountLang != null)
                    {
                        if (!String.IsNullOrEmpty(statusAccountLang.Id))
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
                            cmd.Parameters.AddWithValue("@id", statusAccountLang.Id);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLang.StatusId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `status_id` = @statusId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `status_id` = @statusId ");
                            }
                            cmd.Parameters.AddWithValue("@statusId", statusAccountLang.StatusId);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLang.LangCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lang_code` = @langCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lang_code` = @langCode ");
                            }
                            cmd.Parameters.AddWithValue("@langCode", statusAccountLang.LangCode);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLang.Desc))
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
                            cmd.Parameters.AddWithValue("@desc", statusAccountLang.Desc);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLang.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", statusAccountLang.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLang.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", statusAccountLang.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLang.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", statusAccountLang.UpdatedAt);
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
                                    statusAccountLang.Id = reader.GetValue(0).ToString();
                                else
                                    statusAccountLang.Id = null;
                                if (!reader.IsDBNull(1))
                                    statusAccountLang.StatusId = reader.GetValue(1).ToString();
                                else
                                    statusAccountLang.StatusId = null;
                                if (!reader.IsDBNull(2))
                                    statusAccountLang.LangCode = reader.GetValue(2).ToString();
                                else
                                    statusAccountLang.LangCode = null;
                                if (!reader.IsDBNull(3))
                                    statusAccountLang.Desc = reader.GetValue(3).ToString();
                                else
                                    statusAccountLang.Desc = null;
                                if (!reader.IsDBNull(4))
                                    statusAccountLang.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    statusAccountLang.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    statusAccountLang.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    statusAccountLang.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    statusAccountLang.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    statusAccountLang.UpdatedAt = null;
                                response.Add(statusAccountLang.Id, statusAccountLang);
                                statusAccountLang = new StatusAccountLangModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, StatusAccountLangModel>();
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
                response = new Dictionary<string, StatusAccountLangModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, StatusAccountLangModel> ReadD(string connectionString, List<StatusAccountLangModel> statusAccountLangWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, StatusAccountLangModel> response = new Dictionary<string, StatusAccountLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `status_id`, `lang_code`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ul_status_account_langs ";
                if (statusAccountLangWhere != null && statusAccountLangWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < statusAccountLangWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(statusAccountLangWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, statusAccountLangWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLangWhere[i].StatusId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `status_id` = @statusId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@statusId" + i, statusAccountLangWhere[i].StatusId);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLangWhere[i].LangCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lang_code` = @langCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@langCode" + i, statusAccountLangWhere[i].LangCode);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLangWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, statusAccountLangWhere[i].Desc);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLangWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, statusAccountLangWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLangWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, statusAccountLangWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLangWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, statusAccountLangWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != statusAccountLangWhere.Count - 1)
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
                            StatusAccountLangModel statusAccountLang = new StatusAccountLangModel(); if (!reader.IsDBNull(0))
                                statusAccountLang.Id = reader.GetValue(0).ToString();
                            else
                                statusAccountLang.Id = null;
                            if (!reader.IsDBNull(1))
                                statusAccountLang.StatusId = reader.GetValue(1).ToString();
                            else
                                statusAccountLang.StatusId = null;
                            if (!reader.IsDBNull(2))
                                statusAccountLang.LangCode = reader.GetValue(2).ToString();
                            else
                                statusAccountLang.LangCode = null;
                            if (!reader.IsDBNull(3))
                                statusAccountLang.Desc = reader.GetValue(3).ToString();
                            else
                                statusAccountLang.Desc = null;
                            if (!reader.IsDBNull(4))
                                statusAccountLang.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                statusAccountLang.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                statusAccountLang.CreatedAt = reader.GetValue(5).ToString();
                            else
                                statusAccountLang.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                statusAccountLang.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                statusAccountLang.UpdatedAt = null;
                            response.Add(statusAccountLang.Id, statusAccountLang);
                            statusAccountLang = new StatusAccountLangModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, StatusAccountLangModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, StatusAccountLangModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<StatusAccountLangModel> Read(string connectionString, StatusAccountLangModel statusAccountLang, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountLangModel> response = new List<StatusAccountLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `status_id`, `lang_code`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ul_status_account_langs ";
                {
                    if (statusAccountLang != null)
                    {
                        if (!String.IsNullOrEmpty(statusAccountLang.Id))
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
                            cmd.Parameters.AddWithValue("@id", statusAccountLang.Id);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLang.StatusId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `status_id` = @statusId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `status_id` = @statusId ");
                            }
                            cmd.Parameters.AddWithValue("@statusId", statusAccountLang.StatusId);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLang.LangCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lang_code` = @langCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lang_code` = @langCode ");
                            }
                            cmd.Parameters.AddWithValue("@langCode", statusAccountLang.LangCode);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLang.Desc))
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
                            cmd.Parameters.AddWithValue("@desc", statusAccountLang.Desc);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLang.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", statusAccountLang.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLang.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", statusAccountLang.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLang.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", statusAccountLang.UpdatedAt);
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
                                    statusAccountLang.Id = reader.GetValue(0).ToString();
                                else
                                    statusAccountLang.Id = null;
                                if (!reader.IsDBNull(1))
                                    statusAccountLang.StatusId = reader.GetValue(1).ToString();
                                else
                                    statusAccountLang.StatusId = null;
                                if (!reader.IsDBNull(2))
                                    statusAccountLang.LangCode = reader.GetValue(2).ToString();
                                else
                                    statusAccountLang.LangCode = null;
                                if (!reader.IsDBNull(3))
                                    statusAccountLang.Desc = reader.GetValue(3).ToString();
                                else
                                    statusAccountLang.Desc = null;
                                if (!reader.IsDBNull(4))
                                    statusAccountLang.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    statusAccountLang.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    statusAccountLang.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    statusAccountLang.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    statusAccountLang.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    statusAccountLang.UpdatedAt = null;
                                response.Add(statusAccountLang);
                                statusAccountLang = new StatusAccountLangModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<StatusAccountLangModel>();
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
                response = new List<StatusAccountLangModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<StatusAccountLangModel> Read(string connectionString, List<StatusAccountLangModel> statusAccountLangWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountLangModel> response = new List<StatusAccountLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `status_id`, `lang_code`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ul_status_account_langs ";
                if (statusAccountLangWhere != null && statusAccountLangWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < statusAccountLangWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(statusAccountLangWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, statusAccountLangWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLangWhere[i].StatusId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `status_id` = @statusId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@statusId" + i, statusAccountLangWhere[i].StatusId);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLangWhere[i].LangCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lang_code` = @langCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@langCode" + i, statusAccountLangWhere[i].LangCode);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLangWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, statusAccountLangWhere[i].Desc);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLangWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, statusAccountLangWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLangWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, statusAccountLangWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(statusAccountLangWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, statusAccountLangWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != statusAccountLangWhere.Count - 1)
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
                            StatusAccountLangModel statusAccountLang = new StatusAccountLangModel(); if (!reader.IsDBNull(0))
                                statusAccountLang.Id = reader.GetValue(0).ToString();
                            else
                                statusAccountLang.Id = null;
                            if (!reader.IsDBNull(1))
                                statusAccountLang.StatusId = reader.GetValue(1).ToString();
                            else
                                statusAccountLang.StatusId = null;
                            if (!reader.IsDBNull(2))
                                statusAccountLang.LangCode = reader.GetValue(2).ToString();
                            else
                                statusAccountLang.LangCode = null;
                            if (!reader.IsDBNull(3))
                                statusAccountLang.Desc = reader.GetValue(3).ToString();
                            else
                                statusAccountLang.Desc = null;
                            if (!reader.IsDBNull(4))
                                statusAccountLang.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                statusAccountLang.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                statusAccountLang.CreatedAt = reader.GetValue(5).ToString();
                            else
                                statusAccountLang.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                statusAccountLang.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                statusAccountLang.UpdatedAt = null;
                            response.Add(statusAccountLang);
                            statusAccountLang = new StatusAccountLangModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<StatusAccountLangModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<StatusAccountLangModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<StatusAccountLangModel> Create(string connectionString, StatusAccountLangModel statusAccountLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountLangModel> response = new List<StatusAccountLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO ul_status_account_langs (";
                string values = "(";
                if (!String.IsNullOrEmpty(statusAccountLang.Id))
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
                    cmd.Parameters.AddWithValue("@id", statusAccountLang.Id);
                }
                if (!String.IsNullOrEmpty(statusAccountLang.StatusId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `status_id` ");
                        values = String.Concat(values, ", @statusId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `status_id` ");
                        values = String.Concat(values, " @statusId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@statusId", statusAccountLang.StatusId);
                }
                if (!String.IsNullOrEmpty(statusAccountLang.LangCode))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `lang_code` ");
                        values = String.Concat(values, ", @langCode");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `lang_code` ");
                        values = String.Concat(values, " @langCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@langCode", statusAccountLang.LangCode);
                }
                if (!String.IsNullOrEmpty(statusAccountLang.Desc))
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
                    cmd.Parameters.AddWithValue("@desc", statusAccountLang.Desc);
                }
                if (!String.IsNullOrEmpty(statusAccountLang.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", statusAccountLang.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(statusAccountLang.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", statusAccountLang.CreatedAt);
                }
                if (!String.IsNullOrEmpty(statusAccountLang.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", statusAccountLang.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                statusAccountLang.Id = cmd.LastInsertedId.ToString();
                response.Add(statusAccountLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<StatusAccountLangModel>();
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
        public List<StatusAccountLangModel> Create(string connectionString, List<StatusAccountLangModel> statusAccountLangList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountLangModel> response = new List<StatusAccountLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < statusAccountLangList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO ul_status_account_langs (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(statusAccountLangList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, statusAccountLangList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(statusAccountLangList[i].StatusId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `status_id` ");
                                        values = String.Concat(values, ", @statusId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `status_id` ");
                                        values = String.Concat(values, "@statusId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@statusId" + i, statusAccountLangList[i].StatusId);
                                }
                                if (!String.IsNullOrEmpty(statusAccountLangList[i].LangCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `lang_code` ");
                                        values = String.Concat(values, ", @langCode" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `lang_code` ");
                                        values = String.Concat(values, "@langCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@langCode" + i, statusAccountLangList[i].LangCode);
                                }
                                if (!String.IsNullOrEmpty(statusAccountLangList[i].Desc))
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
                                    cmd.Parameters.AddWithValue("@desc" + i, statusAccountLangList[i].Desc);
                                }
                                if (!String.IsNullOrEmpty(statusAccountLangList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, statusAccountLangList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(statusAccountLangList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, statusAccountLangList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(statusAccountLangList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, statusAccountLangList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(statusAccountLangList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<StatusAccountLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<StatusAccountLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<StatusAccountLangModel>();
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

        public List<StatusAccountLangModel> Update(string connectionString, StatusAccountLangModel statusAccountLang, StatusAccountLangModel statusAccountLangWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountLangModel> response = new List<StatusAccountLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE ul_status_account_langs SET ";
                if (!String.IsNullOrEmpty(statusAccountLang.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", statusAccountLang.Id);
                }
                if (!String.IsNullOrEmpty(statusAccountLang.StatusId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `status_id` = @statusId");
                    else
                        sql = String.Concat(sql, " `status_id` = @statusId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@statusId", statusAccountLang.StatusId);
                }
                if (!String.IsNullOrEmpty(statusAccountLang.LangCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `lang_code` = @langCode");
                    else
                        sql = String.Concat(sql, " `lang_code` = @langCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@langCode", statusAccountLang.LangCode);
                }
                if (!String.IsNullOrEmpty(statusAccountLang.Desc))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `desc` = @desc");
                    else
                        sql = String.Concat(sql, " `desc` = @desc");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", statusAccountLang.Desc);
                }
                if (!String.IsNullOrEmpty(statusAccountLang.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", statusAccountLang.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(statusAccountLang.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", statusAccountLang.CreatedAt);
                }
                if (!String.IsNullOrEmpty(statusAccountLang.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", statusAccountLang.UpdatedAt);
                }
                if (statusAccountLangWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(statusAccountLangWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", statusAccountLangWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(statusAccountLangWhere.StatusId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `status_id` = @statusIdWhere");
                        else
                            sql = String.Concat(sql, " `status_id` = @statusIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@statusIdWhere", statusAccountLangWhere.StatusId);
                    }
                    if (!String.IsNullOrEmpty(statusAccountLangWhere.LangCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lang_code` = @langCodeWhere");
                        else
                            sql = String.Concat(sql, " `lang_code` = @langCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCodeWhere", statusAccountLangWhere.LangCode);
                    }
                    if (!String.IsNullOrEmpty(statusAccountLangWhere.Desc))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `desc` = @descWhere");
                        else
                            sql = String.Concat(sql, " `desc` = @descWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descWhere", statusAccountLangWhere.Desc);
                    }
                    if (!String.IsNullOrEmpty(statusAccountLangWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", statusAccountLangWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(statusAccountLangWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", statusAccountLangWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(statusAccountLangWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", statusAccountLangWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(statusAccountLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<StatusAccountLangModel>();
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

        public List<StatusAccountLangModel> Update(string connectionString, List<StatusAccountLangModel> statusAccountLangList, List<StatusAccountLangModel> statusAccountLangWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountLangModel> response = new List<StatusAccountLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < statusAccountLangList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE ul_status_account_langs SET ";
                                if (!String.IsNullOrEmpty(statusAccountLangList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, statusAccountLangList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(statusAccountLangList[i].StatusId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `status_id` = @statusId" + i);
                                    else
                                        sql = String.Concat(sql, " `status_id` = @statusId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@statusId" + i, statusAccountLangList[i].StatusId);
                                }
                                if (!String.IsNullOrEmpty(statusAccountLangList[i].LangCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `lang_code` = @langCode" + i);
                                    else
                                        sql = String.Concat(sql, " `lang_code` = @langCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@langCode" + i, statusAccountLangList[i].LangCode);
                                }
                                if (!String.IsNullOrEmpty(statusAccountLangList[i].Desc))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `desc` = @desc" + i);
                                    else
                                        sql = String.Concat(sql, " `desc` = @desc" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, statusAccountLangList[i].Desc);
                                }
                                if (!String.IsNullOrEmpty(statusAccountLangList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, statusAccountLangList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(statusAccountLangList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, statusAccountLangList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(statusAccountLangList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, statusAccountLangList[i].UpdatedAt);
                                }
                                if (statusAccountLangWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(statusAccountLangWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, statusAccountLangWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountLangWhereList[i].StatusId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `status_id` = @statusIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `status_id` = @statusIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@statusIdWhere" + i, statusAccountLangWhereList[i].StatusId);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountLangWhereList[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lang_code` = @langCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `lang_code` = @langCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCodeWhere" + i, statusAccountLangWhereList[i].LangCode);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountLangWhereList[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `desc` = @descWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `desc` = @descWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descWhere" + i, statusAccountLangWhereList[i].Desc);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountLangWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, statusAccountLangWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountLangWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, statusAccountLangWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountLangWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, statusAccountLangWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(statusAccountLangList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<StatusAccountLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<StatusAccountLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<StatusAccountLangModel>();
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

        public List<StatusAccountLangModel> Delete(string connectionString, StatusAccountLangModel statusAccountLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountLangModel> response = new List<StatusAccountLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM ul_status_account_langs  ";
                if (statusAccountLang != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(statusAccountLang.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", statusAccountLang.Id);
                    }
                    if (!String.IsNullOrEmpty(statusAccountLang.StatusId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `status_id` = @statusId");
                        else
                            sql = String.Concat(sql, " `status_id` = @statusId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@statusId", statusAccountLang.StatusId);
                    }
                    if (!String.IsNullOrEmpty(statusAccountLang.LangCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lang_code` = @langCode");
                        else
                            sql = String.Concat(sql, " `lang_code` = @langCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCode", statusAccountLang.LangCode);
                    }
                    if (!String.IsNullOrEmpty(statusAccountLang.Desc))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `desc` = @desc");
                        else
                            sql = String.Concat(sql, " `desc` = @desc");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@desc", statusAccountLang.Desc);
                    }
                    if (!String.IsNullOrEmpty(statusAccountLang.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", statusAccountLang.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(statusAccountLang.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", statusAccountLang.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(statusAccountLang.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", statusAccountLang.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(statusAccountLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<StatusAccountLangModel>();
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

        public List<StatusAccountLangModel> Delete(string connectionString, List<StatusAccountLangModel> statusAccountLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatusAccountLangModel> response = new List<StatusAccountLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < statusAccountLang.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM ul_status_account_langs  ";
                                if (statusAccountLang[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(statusAccountLang[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, statusAccountLang[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountLang[i].StatusId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `status_id` = @statusId" + i);
                                        else
                                            sql = String.Concat(sql, " `status_id` = @statusId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@statusId" + i, statusAccountLang[i].StatusId);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountLang[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lang_code` = @langCode" + i);
                                        else
                                            sql = String.Concat(sql, " `lang_code` = @langCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCode" + i, statusAccountLang[i].LangCode);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountLang[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `desc` = @desc" + i);
                                        else
                                            sql = String.Concat(sql, " `desc` = @desc" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@desc" + i, statusAccountLang[i].Desc);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountLang[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, statusAccountLang[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountLang[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, statusAccountLang[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(statusAccountLang[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, statusAccountLang[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(statusAccountLang[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<StatusAccountLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<StatusAccountLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<StatusAccountLangModel>();
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
