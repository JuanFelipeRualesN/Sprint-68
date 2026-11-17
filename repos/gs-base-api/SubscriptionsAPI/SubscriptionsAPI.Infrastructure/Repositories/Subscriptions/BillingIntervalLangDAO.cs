using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class BillingIntervalLangDAO
    {
        private MySqlConnection _connection;
        public List<BillingIntervalLangModel> Read(string connectionString, BillingIntervalLangModel billingIntervalLang, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalLangModel> response = new List<BillingIntervalLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `billing_interval_id`, `lang_code`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM subst_billing_interval_langs ";
                {
                    if (billingIntervalLang != null)
                    {
                        if (!string.IsNullOrEmpty(billingIntervalLang.Id))
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
                            cmd.Parameters.AddWithValue("@id", billingIntervalLang.Id);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalLang.BillingIntervalId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `billing_interval_id` = @billingIntervalId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `billing_interval_id` = @billingIntervalId ");
                            }
                            cmd.Parameters.AddWithValue("@billingIntervalId", billingIntervalLang.BillingIntervalId);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalLang.LangCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `lang_code` = @langCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `lang_code` = @langCode ");
                            }
                            cmd.Parameters.AddWithValue("@langCode", billingIntervalLang.LangCode);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalLang.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", billingIntervalLang.Desc);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalLang.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", billingIntervalLang.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalLang.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", billingIntervalLang.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalLang.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", billingIntervalLang.UpdatedAt);
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
                                    billingIntervalLang.Id = reader.GetValue(0).ToString();
                                else
                                    billingIntervalLang.Id = null;
                                if (!reader.IsDBNull(1))
                                    billingIntervalLang.BillingIntervalId = reader.GetValue(1).ToString();
                                else
                                    billingIntervalLang.BillingIntervalId = null;
                                if (!reader.IsDBNull(2))
                                    billingIntervalLang.LangCode = reader.GetValue(2).ToString();
                                else
                                    billingIntervalLang.LangCode = null;
                                if (!reader.IsDBNull(3))
                                    billingIntervalLang.Desc = reader.GetValue(3).ToString();
                                else
                                    billingIntervalLang.Desc = null;
                                if (!reader.IsDBNull(4))
                                    billingIntervalLang.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    billingIntervalLang.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    billingIntervalLang.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    billingIntervalLang.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    billingIntervalLang.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    billingIntervalLang.UpdatedAt = null;
                                response.Add(billingIntervalLang);
                                billingIntervalLang = new BillingIntervalLangModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<BillingIntervalLangModel>();
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
                response = new List<BillingIntervalLangModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<BillingIntervalLangModel> Read(string connectionString, List<BillingIntervalLangModel> billingIntervalLangWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalLangModel> response = new List<BillingIntervalLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `billing_interval_id`, `lang_code`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM subst_billing_interval_langs ";
                if (billingIntervalLangWhere != null && billingIntervalLangWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < billingIntervalLangWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(billingIntervalLangWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, billingIntervalLangWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalLangWhere[i].BillingIntervalId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `billing_interval_id` = @billingIntervalId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@billingIntervalId" + i, billingIntervalLangWhere[i].BillingIntervalId);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalLangWhere[i].LangCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lang_code` = @langCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@langCode" + i, billingIntervalLangWhere[i].LangCode);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalLangWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, billingIntervalLangWhere[i].Desc);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalLangWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, billingIntervalLangWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalLangWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, billingIntervalLangWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalLangWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, billingIntervalLangWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != billingIntervalLangWhere.Count - 1)
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
                            BillingIntervalLangModel billingIntervalLang = new BillingIntervalLangModel(); if (!reader.IsDBNull(0))
                                billingIntervalLang.Id = reader.GetValue(0).ToString();
                            else
                                billingIntervalLang.Id = null;
                            if (!reader.IsDBNull(1))
                                billingIntervalLang.BillingIntervalId = reader.GetValue(1).ToString();
                            else
                                billingIntervalLang.BillingIntervalId = null;
                            if (!reader.IsDBNull(2))
                                billingIntervalLang.LangCode = reader.GetValue(2).ToString();
                            else
                                billingIntervalLang.LangCode = null;
                            if (!reader.IsDBNull(3))
                                billingIntervalLang.Desc = reader.GetValue(3).ToString();
                            else
                                billingIntervalLang.Desc = null;
                            if (!reader.IsDBNull(4))
                                billingIntervalLang.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                billingIntervalLang.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                billingIntervalLang.CreatedAt = reader.GetValue(5).ToString();
                            else
                                billingIntervalLang.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                billingIntervalLang.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                billingIntervalLang.UpdatedAt = null;
                            response.Add(billingIntervalLang);
                            billingIntervalLang = new BillingIntervalLangModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<BillingIntervalLangModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<BillingIntervalLangModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<BillingIntervalLangModel> Create(string connectionString, BillingIntervalLangModel billingIntervalLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalLangModel> response = new List<BillingIntervalLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO subst_billing_interval_langs (";
                string values = "(";
                if (!string.IsNullOrEmpty(billingIntervalLang.Id))
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
                    cmd.Parameters.AddWithValue("@id", billingIntervalLang.Id);
                }
                if (!string.IsNullOrEmpty(billingIntervalLang.BillingIntervalId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `billing_interval_id` ");
                        values = string.Concat(values, ", @billingIntervalId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `billing_interval_id` ");
                        values = string.Concat(values, " @billingIntervalId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@billingIntervalId", billingIntervalLang.BillingIntervalId);
                }
                if (!string.IsNullOrEmpty(billingIntervalLang.LangCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `lang_code` ");
                        values = string.Concat(values, ", @langCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `lang_code` ");
                        values = string.Concat(values, " @langCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@langCode", billingIntervalLang.LangCode);
                }
                if (!string.IsNullOrEmpty(billingIntervalLang.Desc))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `desc` ");
                        values = string.Concat(values, ", @desc");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `desc` ");
                        values = string.Concat(values, " @desc");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", billingIntervalLang.Desc);
                }
                if (!string.IsNullOrEmpty(billingIntervalLang.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", billingIntervalLang.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(billingIntervalLang.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", billingIntervalLang.CreatedAt);
                }
                if (!string.IsNullOrEmpty(billingIntervalLang.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", billingIntervalLang.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                billingIntervalLang.Id = cmd.LastInsertedId.ToString();
                response.Add(billingIntervalLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<BillingIntervalLangModel>();
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
        public List<BillingIntervalLangModel> Create(string connectionString, List<BillingIntervalLangModel> billingIntervalLangList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalLangModel> response = new List<BillingIntervalLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < billingIntervalLangList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_billing_interval_langs (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(billingIntervalLangList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, billingIntervalLangList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalLangList[i].BillingIntervalId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `billing_interval_id` ");
                                        values = string.Concat(values, ", @billingIntervalId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `billing_interval_id` ");
                                        values = string.Concat(values, "@billingIntervalId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@billingIntervalId" + i, billingIntervalLangList[i].BillingIntervalId);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalLangList[i].LangCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `lang_code` ");
                                        values = string.Concat(values, ", @langCode" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `lang_code` ");
                                        values = string.Concat(values, "@langCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@langCode" + i, billingIntervalLangList[i].LangCode);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalLangList[i].Desc))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `desc` ");
                                        values = string.Concat(values, ", @desc" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `desc` ");
                                        values = string.Concat(values, "@desc" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, billingIntervalLangList[i].Desc);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalLangList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, billingIntervalLangList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalLangList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, billingIntervalLangList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalLangList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, billingIntervalLangList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(billingIntervalLangList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<BillingIntervalLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<BillingIntervalLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<BillingIntervalLangModel>();
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

        public List<BillingIntervalLangModel> Update(string connectionString, BillingIntervalLangModel billingIntervalLang, BillingIntervalLangModel billingIntervalLangWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalLangModel> response = new List<BillingIntervalLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE subst_billing_interval_langs SET ";
                if (!string.IsNullOrEmpty(billingIntervalLang.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", billingIntervalLang.Id);
                }
                if (!string.IsNullOrEmpty(billingIntervalLang.BillingIntervalId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `billing_interval_id` = @billingIntervalId");
                    else
                        sql = string.Concat(sql, " `billing_interval_id` = @billingIntervalId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@billingIntervalId", billingIntervalLang.BillingIntervalId);
                }
                if (!string.IsNullOrEmpty(billingIntervalLang.LangCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `lang_code` = @langCode");
                    else
                        sql = string.Concat(sql, " `lang_code` = @langCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@langCode", billingIntervalLang.LangCode);
                }
                if (!string.IsNullOrEmpty(billingIntervalLang.Desc))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `desc` = @desc");
                    else
                        sql = string.Concat(sql, " `desc` = @desc");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", billingIntervalLang.Desc);
                }
                if (!string.IsNullOrEmpty(billingIntervalLang.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", billingIntervalLang.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(billingIntervalLang.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", billingIntervalLang.CreatedAt);
                }
                if (!string.IsNullOrEmpty(billingIntervalLang.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", billingIntervalLang.UpdatedAt);
                }
                if (billingIntervalLangWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(billingIntervalLangWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", billingIntervalLangWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalLangWhere.BillingIntervalId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `billing_interval_id` = @billingIntervalIdWhere");
                        else
                            sql = string.Concat(sql, " `billing_interval_id` = @billingIntervalIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@billingIntervalIdWhere", billingIntervalLangWhere.BillingIntervalId);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalLangWhere.LangCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `lang_code` = @langCodeWhere");
                        else
                            sql = string.Concat(sql, " `lang_code` = @langCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCodeWhere", billingIntervalLangWhere.LangCode);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalLangWhere.Desc))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `desc` = @descWhere");
                        else
                            sql = string.Concat(sql, " `desc` = @descWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descWhere", billingIntervalLangWhere.Desc);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalLangWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", billingIntervalLangWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalLangWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", billingIntervalLangWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalLangWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", billingIntervalLangWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(billingIntervalLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<BillingIntervalLangModel>();
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

        public List<BillingIntervalLangModel> Update(string connectionString, List<BillingIntervalLangModel> billingIntervalLangList, List<BillingIntervalLangModel> billingIntervalLangWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalLangModel> response = new List<BillingIntervalLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < billingIntervalLangList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_billing_interval_langs SET ";
                                if (!string.IsNullOrEmpty(billingIntervalLangList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, billingIntervalLangList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalLangList[i].BillingIntervalId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `billing_interval_id` = @billingIntervalId" + i);
                                    else
                                        sql = string.Concat(sql, " `billing_interval_id` = @billingIntervalId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@billingIntervalId" + i, billingIntervalLangList[i].BillingIntervalId);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalLangList[i].LangCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `lang_code` = @langCode" + i);
                                    else
                                        sql = string.Concat(sql, " `lang_code` = @langCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@langCode" + i, billingIntervalLangList[i].LangCode);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalLangList[i].Desc))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `desc` = @desc" + i);
                                    else
                                        sql = string.Concat(sql, " `desc` = @desc" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, billingIntervalLangList[i].Desc);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalLangList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, billingIntervalLangList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalLangList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, billingIntervalLangList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalLangList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, billingIntervalLangList[i].UpdatedAt);
                                }
                                if (billingIntervalLangWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(billingIntervalLangWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, billingIntervalLangWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalLangWhereList[i].BillingIntervalId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `billing_interval_id` = @billingIntervalIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `billing_interval_id` = @billingIntervalIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@billingIntervalIdWhere" + i, billingIntervalLangWhereList[i].BillingIntervalId);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalLangWhereList[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `lang_code` = @langCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `lang_code` = @langCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCodeWhere" + i, billingIntervalLangWhereList[i].LangCode);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalLangWhereList[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `desc` = @descWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `desc` = @descWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descWhere" + i, billingIntervalLangWhereList[i].Desc);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalLangWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, billingIntervalLangWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalLangWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, billingIntervalLangWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalLangWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, billingIntervalLangWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(billingIntervalLangList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<BillingIntervalLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<BillingIntervalLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<BillingIntervalLangModel>();
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

        public List<BillingIntervalLangModel> Delete(string connectionString, BillingIntervalLangModel billingIntervalLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalLangModel> response = new List<BillingIntervalLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM subst_billing_interval_langs  ";
                if (billingIntervalLang != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(billingIntervalLang.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", billingIntervalLang.Id);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalLang.BillingIntervalId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `billing_interval_id` = @billingIntervalId");
                        else
                            sql = string.Concat(sql, " `billing_interval_id` = @billingIntervalId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@billingIntervalId", billingIntervalLang.BillingIntervalId);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalLang.LangCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `lang_code` = @langCode");
                        else
                            sql = string.Concat(sql, " `lang_code` = @langCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCode", billingIntervalLang.LangCode);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalLang.Desc))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `desc` = @desc");
                        else
                            sql = string.Concat(sql, " `desc` = @desc");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@desc", billingIntervalLang.Desc);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalLang.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", billingIntervalLang.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalLang.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", billingIntervalLang.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalLang.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", billingIntervalLang.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(billingIntervalLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<BillingIntervalLangModel>();
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

        public List<BillingIntervalLangModel> Delete(string connectionString, List<BillingIntervalLangModel> billingIntervalLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalLangModel> response = new List<BillingIntervalLangModel>();
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
                        for (int i = 0; i < billingIntervalLang.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_billing_interval_langs  ";
                                if (billingIntervalLang[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(billingIntervalLang[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, billingIntervalLang[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalLang[i].BillingIntervalId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `billing_interval_id` = @billingIntervalId" + i);
                                        else
                                            sql = string.Concat(sql, " `billing_interval_id` = @billingIntervalId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@billingIntervalId" + i, billingIntervalLang[i].BillingIntervalId);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalLang[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `lang_code` = @langCode" + i);
                                        else
                                            sql = string.Concat(sql, " `lang_code` = @langCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCode" + i, billingIntervalLang[i].LangCode);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalLang[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `desc` = @desc" + i);
                                        else
                                            sql = string.Concat(sql, " `desc` = @desc" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@desc" + i, billingIntervalLang[i].Desc);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalLang[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, billingIntervalLang[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalLang[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, billingIntervalLang[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalLang[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, billingIntervalLang[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(billingIntervalLang[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<BillingIntervalLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<BillingIntervalLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<BillingIntervalLangModel>();
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
