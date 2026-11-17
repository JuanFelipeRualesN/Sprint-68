using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class InvoiceLogDAO
    {
        private MySqlConnection _connection;
        public List<InvoiceLogModel> Read(string connectionString, InvoiceLogModel invoiceLog, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<InvoiceLogModel> response = new List<InvoiceLogModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `invoice_platform_id`, `customer_platform_id`, `subscription_platform_id`, `amt_due`, `modified_by`, `created_at`, `updated_at`  FROM subst_invoice_logs ";
                {
                    if (invoiceLog != null)
                    {
                        if (!string.IsNullOrEmpty(invoiceLog.Id))
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
                            cmd.Parameters.AddWithValue("@id", invoiceLog.Id);
                        }
                        if (!string.IsNullOrEmpty(invoiceLog.InvoicePlatformId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `invoice_platform_id` = @invoicePlatformId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `invoice_platform_id` = @invoicePlatformId ");
                            }
                            cmd.Parameters.AddWithValue("@invoicePlatformId", invoiceLog.InvoicePlatformId);
                        }
                        if (!string.IsNullOrEmpty(invoiceLog.CustomerPlatformId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `customer_platform_id` = @customerPlatformId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `customer_platform_id` = @customerPlatformId ");
                            }
                            cmd.Parameters.AddWithValue("@customerPlatformId", invoiceLog.CustomerPlatformId);
                        }
                        if (!string.IsNullOrEmpty(invoiceLog.SubscriptionPlatformId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subscription_platform_id` = @subscriptionPlatformId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subscription_platform_id` = @subscriptionPlatformId ");
                            }
                            cmd.Parameters.AddWithValue("@subscriptionPlatformId", invoiceLog.SubscriptionPlatformId);
                        }
                        if (!string.IsNullOrEmpty(invoiceLog.AmtDue))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `amt_due` = @amtDue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `amt_due` = @amtDue ");
                            }
                            cmd.Parameters.AddWithValue("@amtDue", invoiceLog.AmtDue);
                        }
                        if (!string.IsNullOrEmpty(invoiceLog.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", invoiceLog.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(invoiceLog.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", invoiceLog.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(invoiceLog.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", invoiceLog.UpdatedAt);
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
                                    invoiceLog.Id = reader.GetValue(0).ToString();
                                else
                                    invoiceLog.Id = null;
                                if (!reader.IsDBNull(1))
                                    invoiceLog.InvoicePlatformId = reader.GetValue(1).ToString();
                                else
                                    invoiceLog.InvoicePlatformId = null;
                                if (!reader.IsDBNull(2))
                                    invoiceLog.CustomerPlatformId = reader.GetValue(2).ToString();
                                else
                                    invoiceLog.CustomerPlatformId = null;
                                if (!reader.IsDBNull(3))
                                    invoiceLog.SubscriptionPlatformId = reader.GetValue(3).ToString();
                                else
                                    invoiceLog.SubscriptionPlatformId = null;
                                if (!reader.IsDBNull(4))
                                    invoiceLog.AmtDue = reader.GetValue(4).ToString();
                                else
                                    invoiceLog.AmtDue = null;
                                if (!reader.IsDBNull(5))
                                    invoiceLog.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    invoiceLog.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    invoiceLog.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    invoiceLog.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    invoiceLog.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    invoiceLog.UpdatedAt = null;
                                response.Add(invoiceLog);
                                invoiceLog = new InvoiceLogModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<InvoiceLogModel>();
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
                response = new List<InvoiceLogModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<InvoiceLogModel> Read(string connectionString, List<InvoiceLogModel> invoiceLogWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<InvoiceLogModel> response = new List<InvoiceLogModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `invoice_platform_id`, `customer_platform_id`, `subscription_platform_id`, `amt_due`, `modified_by`, `created_at`, `updated_at`  FROM subst_invoice_logs ";
                if (invoiceLogWhere != null && invoiceLogWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < invoiceLogWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(invoiceLogWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, invoiceLogWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(invoiceLogWhere[i].InvoicePlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `invoice_platform_id` = @invoicePlatformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@invoicePlatformId" + i, invoiceLogWhere[i].InvoicePlatformId);
                        }
                        if (!string.IsNullOrEmpty(invoiceLogWhere[i].CustomerPlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `customer_platform_id` = @customerPlatformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customerPlatformId" + i, invoiceLogWhere[i].CustomerPlatformId);
                        }
                        if (!string.IsNullOrEmpty(invoiceLogWhere[i].SubscriptionPlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_platform_id` = @subscriptionPlatformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionPlatformId" + i, invoiceLogWhere[i].SubscriptionPlatformId);
                        }
                        if (!string.IsNullOrEmpty(invoiceLogWhere[i].AmtDue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `amt_due` = @amtDue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@amtDue" + i, invoiceLogWhere[i].AmtDue);
                        }
                        if (!string.IsNullOrEmpty(invoiceLogWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, invoiceLogWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(invoiceLogWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, invoiceLogWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(invoiceLogWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, invoiceLogWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != invoiceLogWhere.Count - 1)
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
                            InvoiceLogModel invoiceLog = new InvoiceLogModel(); if (!reader.IsDBNull(0))
                                invoiceLog.Id = reader.GetValue(0).ToString();
                            else
                                invoiceLog.Id = null;
                            if (!reader.IsDBNull(1))
                                invoiceLog.InvoicePlatformId = reader.GetValue(1).ToString();
                            else
                                invoiceLog.InvoicePlatformId = null;
                            if (!reader.IsDBNull(2))
                                invoiceLog.CustomerPlatformId = reader.GetValue(2).ToString();
                            else
                                invoiceLog.CustomerPlatformId = null;
                            if (!reader.IsDBNull(3))
                                invoiceLog.SubscriptionPlatformId = reader.GetValue(3).ToString();
                            else
                                invoiceLog.SubscriptionPlatformId = null;
                            if (!reader.IsDBNull(4))
                                invoiceLog.AmtDue = reader.GetValue(4).ToString();
                            else
                                invoiceLog.AmtDue = null;
                            if (!reader.IsDBNull(5))
                                invoiceLog.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                invoiceLog.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                invoiceLog.CreatedAt = reader.GetValue(6).ToString();
                            else
                                invoiceLog.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                invoiceLog.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                invoiceLog.UpdatedAt = null;
                            response.Add(invoiceLog);
                            invoiceLog = new InvoiceLogModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<InvoiceLogModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<InvoiceLogModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<InvoiceLogModel> Create(string connectionString, InvoiceLogModel invoiceLog)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<InvoiceLogModel> response = new List<InvoiceLogModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO subst_invoice_logs (";
                string values = "(";
                if (!string.IsNullOrEmpty(invoiceLog.Id))
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
                    cmd.Parameters.AddWithValue("@id", invoiceLog.Id);
                }
                if (!string.IsNullOrEmpty(invoiceLog.InvoicePlatformId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `invoice_platform_id` ");
                        values = string.Concat(values, ", @invoicePlatformId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `invoice_platform_id` ");
                        values = string.Concat(values, " @invoicePlatformId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@invoicePlatformId", invoiceLog.InvoicePlatformId);
                }
                if (!string.IsNullOrEmpty(invoiceLog.CustomerPlatformId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `customer_platform_id` ");
                        values = string.Concat(values, ", @customerPlatformId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `customer_platform_id` ");
                        values = string.Concat(values, " @customerPlatformId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@customerPlatformId", invoiceLog.CustomerPlatformId);
                }
                if (!string.IsNullOrEmpty(invoiceLog.SubscriptionPlatformId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `subscription_platform_id` ");
                        values = string.Concat(values, ", @subscriptionPlatformId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `subscription_platform_id` ");
                        values = string.Concat(values, " @subscriptionPlatformId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subscriptionPlatformId", invoiceLog.SubscriptionPlatformId);
                }
                if (!string.IsNullOrEmpty(invoiceLog.AmtDue))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `amt_due` ");
                        values = string.Concat(values, ", @amtDue");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `amt_due` ");
                        values = string.Concat(values, " @amtDue");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@amtDue", invoiceLog.AmtDue);
                }
                if (!string.IsNullOrEmpty(invoiceLog.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", invoiceLog.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(invoiceLog.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", invoiceLog.CreatedAt);
                }
                if (!string.IsNullOrEmpty(invoiceLog.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", invoiceLog.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                invoiceLog.Id = cmd.LastInsertedId.ToString();
                response.Add(invoiceLog);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<InvoiceLogModel>();
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
        public List<InvoiceLogModel> Create(string connectionString, List<InvoiceLogModel> invoiceLogList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<InvoiceLogModel> response = new List<InvoiceLogModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < invoiceLogList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_invoice_logs (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(invoiceLogList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, invoiceLogList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(invoiceLogList[i].InvoicePlatformId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `invoice_platform_id` ");
                                        values = string.Concat(values, ", @invoicePlatformId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `invoice_platform_id` ");
                                        values = string.Concat(values, "@invoicePlatformId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@invoicePlatformId" + i, invoiceLogList[i].InvoicePlatformId);
                                }
                                if (!string.IsNullOrEmpty(invoiceLogList[i].CustomerPlatformId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `customer_platform_id` ");
                                        values = string.Concat(values, ", @customerPlatformId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `customer_platform_id` ");
                                        values = string.Concat(values, "@customerPlatformId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@customerPlatformId" + i, invoiceLogList[i].CustomerPlatformId);
                                }
                                if (!string.IsNullOrEmpty(invoiceLogList[i].SubscriptionPlatformId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `subscription_platform_id` ");
                                        values = string.Concat(values, ", @subscriptionPlatformId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `subscription_platform_id` ");
                                        values = string.Concat(values, "@subscriptionPlatformId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subscriptionPlatformId" + i, invoiceLogList[i].SubscriptionPlatformId);
                                }
                                if (!string.IsNullOrEmpty(invoiceLogList[i].AmtDue))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `amt_due` ");
                                        values = string.Concat(values, ", @amtDue" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `amt_due` ");
                                        values = string.Concat(values, "@amtDue" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@amtDue" + i, invoiceLogList[i].AmtDue);
                                }
                                if (!string.IsNullOrEmpty(invoiceLogList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, invoiceLogList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(invoiceLogList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, invoiceLogList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(invoiceLogList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, invoiceLogList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(invoiceLogList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<InvoiceLogModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<InvoiceLogModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<InvoiceLogModel>();
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

        public List<InvoiceLogModel> Update(string connectionString, InvoiceLogModel invoiceLog, InvoiceLogModel invoiceLogWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<InvoiceLogModel> response = new List<InvoiceLogModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE subst_invoice_logs SET ";
                if (!string.IsNullOrEmpty(invoiceLog.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", invoiceLog.Id);
                }
                if (!string.IsNullOrEmpty(invoiceLog.InvoicePlatformId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `invoice_platform_id` = @invoicePlatformId");
                    else
                        sql = string.Concat(sql, " `invoice_platform_id` = @invoicePlatformId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@invoicePlatformId", invoiceLog.InvoicePlatformId);
                }
                if (!string.IsNullOrEmpty(invoiceLog.CustomerPlatformId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `customer_platform_id` = @customerPlatformId");
                    else
                        sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@customerPlatformId", invoiceLog.CustomerPlatformId);
                }
                if (!string.IsNullOrEmpty(invoiceLog.SubscriptionPlatformId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `subscription_platform_id` = @subscriptionPlatformId");
                    else
                        sql = string.Concat(sql, " `subscription_platform_id` = @subscriptionPlatformId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subscriptionPlatformId", invoiceLog.SubscriptionPlatformId);
                }
                if (!string.IsNullOrEmpty(invoiceLog.AmtDue))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `amt_due` = @amtDue");
                    else
                        sql = string.Concat(sql, " `amt_due` = @amtDue");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@amtDue", invoiceLog.AmtDue);
                }
                if (!string.IsNullOrEmpty(invoiceLog.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", invoiceLog.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(invoiceLog.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", invoiceLog.CreatedAt);
                }
                if (!string.IsNullOrEmpty(invoiceLog.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", invoiceLog.UpdatedAt);
                }
                if (invoiceLogWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(invoiceLogWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", invoiceLogWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(invoiceLogWhere.InvoicePlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `invoice_platform_id` = @invoicePlatformIdWhere");
                        else
                            sql = string.Concat(sql, " `invoice_platform_id` = @invoicePlatformIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@invoicePlatformIdWhere", invoiceLogWhere.InvoicePlatformId);
                    }
                    if (!string.IsNullOrEmpty(invoiceLogWhere.CustomerPlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `customer_platform_id` = @customerPlatformIdWhere");
                        else
                            sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@customerPlatformIdWhere", invoiceLogWhere.CustomerPlatformId);
                    }
                    if (!string.IsNullOrEmpty(invoiceLogWhere.SubscriptionPlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subscription_platform_id` = @subscriptionPlatformIdWhere");
                        else
                            sql = string.Concat(sql, " `subscription_platform_id` = @subscriptionPlatformIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionPlatformIdWhere", invoiceLogWhere.SubscriptionPlatformId);
                    }
                    if (!string.IsNullOrEmpty(invoiceLogWhere.AmtDue))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `amt_due` = @amtDueWhere");
                        else
                            sql = string.Concat(sql, " `amt_due` = @amtDueWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@amtDueWhere", invoiceLogWhere.AmtDue);
                    }
                    if (!string.IsNullOrEmpty(invoiceLogWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", invoiceLogWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(invoiceLogWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", invoiceLogWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(invoiceLogWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", invoiceLogWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(invoiceLog);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<InvoiceLogModel>();
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

        public List<InvoiceLogModel> Update(string connectionString, List<InvoiceLogModel> invoiceLogList, List<InvoiceLogModel> invoiceLogWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<InvoiceLogModel> response = new List<InvoiceLogModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < invoiceLogList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_invoice_logs SET ";
                                if (!string.IsNullOrEmpty(invoiceLogList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, invoiceLogList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(invoiceLogList[i].InvoicePlatformId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `invoice_platform_id` = @invoicePlatformId" + i);
                                    else
                                        sql = string.Concat(sql, " `invoice_platform_id` = @invoicePlatformId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@invoicePlatformId" + i, invoiceLogList[i].InvoicePlatformId);
                                }
                                if (!string.IsNullOrEmpty(invoiceLogList[i].CustomerPlatformId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `customer_platform_id` = @customerPlatformId" + i);
                                    else
                                        sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@customerPlatformId" + i, invoiceLogList[i].CustomerPlatformId);
                                }
                                if (!string.IsNullOrEmpty(invoiceLogList[i].SubscriptionPlatformId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `subscription_platform_id` = @subscriptionPlatformId" + i);
                                    else
                                        sql = string.Concat(sql, " `subscription_platform_id` = @subscriptionPlatformId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subscriptionPlatformId" + i, invoiceLogList[i].SubscriptionPlatformId);
                                }
                                if (!string.IsNullOrEmpty(invoiceLogList[i].AmtDue))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `amt_due` = @amtDue" + i);
                                    else
                                        sql = string.Concat(sql, " `amt_due` = @amtDue" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@amtDue" + i, invoiceLogList[i].AmtDue);
                                }
                                if (!string.IsNullOrEmpty(invoiceLogList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, invoiceLogList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(invoiceLogList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, invoiceLogList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(invoiceLogList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, invoiceLogList[i].UpdatedAt);
                                }
                                if (invoiceLogWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(invoiceLogWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, invoiceLogWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(invoiceLogWhereList[i].InvoicePlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `invoice_platform_id` = @invoicePlatformIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `invoice_platform_id` = @invoicePlatformIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@invoicePlatformIdWhere" + i, invoiceLogWhereList[i].InvoicePlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(invoiceLogWhereList[i].CustomerPlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `customer_platform_id` = @customerPlatformIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@customerPlatformIdWhere" + i, invoiceLogWhereList[i].CustomerPlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(invoiceLogWhereList[i].SubscriptionPlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subscription_platform_id` = @subscriptionPlatformIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `subscription_platform_id` = @subscriptionPlatformIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionPlatformIdWhere" + i, invoiceLogWhereList[i].SubscriptionPlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(invoiceLogWhereList[i].AmtDue))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `amt_due` = @amtDueWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `amt_due` = @amtDueWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@amtDueWhere" + i, invoiceLogWhereList[i].AmtDue);
                                    }
                                    if (!string.IsNullOrEmpty(invoiceLogWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, invoiceLogWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(invoiceLogWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, invoiceLogWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(invoiceLogWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, invoiceLogWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(invoiceLogList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<InvoiceLogModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<InvoiceLogModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<InvoiceLogModel>();
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

        public List<InvoiceLogModel> Delete(string connectionString, InvoiceLogModel invoiceLog)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<InvoiceLogModel> response = new List<InvoiceLogModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM subst_invoice_logs  ";
                if (invoiceLog != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(invoiceLog.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", invoiceLog.Id);
                    }
                    if (!string.IsNullOrEmpty(invoiceLog.InvoicePlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `invoice_platform_id` = @invoicePlatformId");
                        else
                            sql = string.Concat(sql, " `invoice_platform_id` = @invoicePlatformId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@invoicePlatformId", invoiceLog.InvoicePlatformId);
                    }
                    if (!string.IsNullOrEmpty(invoiceLog.CustomerPlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `customer_platform_id` = @customerPlatformId");
                        else
                            sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@customerPlatformId", invoiceLog.CustomerPlatformId);
                    }
                    if (!string.IsNullOrEmpty(invoiceLog.SubscriptionPlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subscription_platform_id` = @subscriptionPlatformId");
                        else
                            sql = string.Concat(sql, " `subscription_platform_id` = @subscriptionPlatformId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionPlatformId", invoiceLog.SubscriptionPlatformId);
                    }
                    if (!string.IsNullOrEmpty(invoiceLog.AmtDue))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `amt_due` = @amtDue");
                        else
                            sql = string.Concat(sql, " `amt_due` = @amtDue");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@amtDue", invoiceLog.AmtDue);
                    }
                    if (!string.IsNullOrEmpty(invoiceLog.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", invoiceLog.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(invoiceLog.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", invoiceLog.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(invoiceLog.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", invoiceLog.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(invoiceLog);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<InvoiceLogModel>();
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

        public List<InvoiceLogModel> Delete(string connectionString, List<InvoiceLogModel> invoiceLog)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<InvoiceLogModel> response = new List<InvoiceLogModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < invoiceLog.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_invoice_logs  ";
                                if (invoiceLog[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(invoiceLog[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, invoiceLog[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(invoiceLog[i].InvoicePlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `invoice_platform_id` = @invoicePlatformId" + i);
                                        else
                                            sql = string.Concat(sql, " `invoice_platform_id` = @invoicePlatformId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@invoicePlatformId" + i, invoiceLog[i].InvoicePlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(invoiceLog[i].CustomerPlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `customer_platform_id` = @customerPlatformId" + i);
                                        else
                                            sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@customerPlatformId" + i, invoiceLog[i].CustomerPlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(invoiceLog[i].SubscriptionPlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subscription_platform_id` = @subscriptionPlatformId" + i);
                                        else
                                            sql = string.Concat(sql, " `subscription_platform_id` = @subscriptionPlatformId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionPlatformId" + i, invoiceLog[i].SubscriptionPlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(invoiceLog[i].AmtDue))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `amt_due` = @amtDue" + i);
                                        else
                                            sql = string.Concat(sql, " `amt_due` = @amtDue" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@amtDue" + i, invoiceLog[i].AmtDue);
                                    }
                                    if (!string.IsNullOrEmpty(invoiceLog[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, invoiceLog[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(invoiceLog[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, invoiceLog[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(invoiceLog[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, invoiceLog[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(invoiceLog[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<InvoiceLogModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<InvoiceLogModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<InvoiceLogModel>();
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
