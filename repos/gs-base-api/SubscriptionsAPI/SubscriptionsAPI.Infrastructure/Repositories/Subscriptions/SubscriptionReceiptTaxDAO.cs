using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class SubscriptionReceiptTaxDAO
    {
        private MySqlConnection _connection;
        public List<SubscriptionReceiptTaxModel> Read(string connectionString, SubscriptionReceiptTaxModel subscriptionReceiptTax, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptTaxModel> response = new List<SubscriptionReceiptTaxModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `subscription_receipt_id`, `tax_id`, `tax_name`, `tax_percent`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscription_receipt_taxs ";
                {
                    if (subscriptionReceiptTax != null)
                    {
                        if (!string.IsNullOrEmpty(subscriptionReceiptTax.Id))
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
                            cmd.Parameters.AddWithValue("@id", subscriptionReceiptTax.Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptTax.SubscriptionReceiptId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subscription_receipt_id` = @subscriptionReceiptId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subscription_receipt_id` = @subscriptionReceiptId ");
                            }
                            cmd.Parameters.AddWithValue("@subscriptionReceiptId", subscriptionReceiptTax.SubscriptionReceiptId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptTax.TaxId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `tax_id` = @taxId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `tax_id` = @taxId ");
                            }
                            cmd.Parameters.AddWithValue("@taxId", subscriptionReceiptTax.TaxId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptTax.TaxName))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `tax_name` = @taxName ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `tax_name` = @taxName ");
                            }
                            cmd.Parameters.AddWithValue("@taxName", subscriptionReceiptTax.TaxName);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptTax.TaxPercent))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `tax_percent` = @taxPercent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `tax_percent` = @taxPercent ");
                            }
                            cmd.Parameters.AddWithValue("@taxPercent", subscriptionReceiptTax.TaxPercent);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptTax.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", subscriptionReceiptTax.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptTax.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", subscriptionReceiptTax.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptTax.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", subscriptionReceiptTax.UpdatedAt);
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
                                    subscriptionReceiptTax.Id = reader.GetValue(0).ToString();
                                else
                                    subscriptionReceiptTax.Id = null;
                                if (!reader.IsDBNull(1))
                                    subscriptionReceiptTax.SubscriptionReceiptId = reader.GetValue(1).ToString();
                                else
                                    subscriptionReceiptTax.SubscriptionReceiptId = null;
                                if (!reader.IsDBNull(2))
                                    subscriptionReceiptTax.TaxId = reader.GetValue(2).ToString();
                                else
                                    subscriptionReceiptTax.TaxId = null;
                                if (!reader.IsDBNull(3))
                                    subscriptionReceiptTax.TaxName = reader.GetValue(3).ToString();
                                else
                                    subscriptionReceiptTax.TaxName = null;
                                if (!reader.IsDBNull(4))
                                    subscriptionReceiptTax.TaxPercent = reader.GetValue(4).ToString();
                                else
                                    subscriptionReceiptTax.TaxPercent = null;
                                if (!reader.IsDBNull(5))
                                    subscriptionReceiptTax.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    subscriptionReceiptTax.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    subscriptionReceiptTax.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    subscriptionReceiptTax.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    subscriptionReceiptTax.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    subscriptionReceiptTax.UpdatedAt = null;
                                response.Add(subscriptionReceiptTax);
                                subscriptionReceiptTax = new SubscriptionReceiptTaxModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<SubscriptionReceiptTaxModel>();
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
                response = new List<SubscriptionReceiptTaxModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionReceiptTaxModel> Read(string connectionString, List<SubscriptionReceiptTaxModel> subscriptionReceiptTaxWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptTaxModel> response = new List<SubscriptionReceiptTaxModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `subscription_receipt_id`, `tax_id`, `tax_name`, `tax_percent`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscription_receipt_taxs ";
                if (subscriptionReceiptTaxWhere != null && subscriptionReceiptTaxWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < subscriptionReceiptTaxWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, subscriptionReceiptTaxWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere[i].SubscriptionReceiptId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_receipt_id` = @subscriptionReceiptId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionReceiptId" + i, subscriptionReceiptTaxWhere[i].SubscriptionReceiptId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere[i].TaxId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tax_id` = @taxId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@taxId" + i, subscriptionReceiptTaxWhere[i].TaxId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere[i].TaxName))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tax_name` = @taxName" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@taxName" + i, subscriptionReceiptTaxWhere[i].TaxName);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere[i].TaxPercent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tax_percent` = @taxPercent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@taxPercent" + i, subscriptionReceiptTaxWhere[i].TaxPercent);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionReceiptTaxWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionReceiptTaxWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionReceiptTaxWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != subscriptionReceiptTaxWhere.Count - 1)
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
                            SubscriptionReceiptTaxModel subscriptionReceiptTax = new SubscriptionReceiptTaxModel(); if (!reader.IsDBNull(0))
                                subscriptionReceiptTax.Id = reader.GetValue(0).ToString();
                            else
                                subscriptionReceiptTax.Id = null;
                            if (!reader.IsDBNull(1))
                                subscriptionReceiptTax.SubscriptionReceiptId = reader.GetValue(1).ToString();
                            else
                                subscriptionReceiptTax.SubscriptionReceiptId = null;
                            if (!reader.IsDBNull(2))
                                subscriptionReceiptTax.TaxId = reader.GetValue(2).ToString();
                            else
                                subscriptionReceiptTax.TaxId = null;
                            if (!reader.IsDBNull(3))
                                subscriptionReceiptTax.TaxName = reader.GetValue(3).ToString();
                            else
                                subscriptionReceiptTax.TaxName = null;
                            if (!reader.IsDBNull(4))
                                subscriptionReceiptTax.TaxPercent = reader.GetValue(4).ToString();
                            else
                                subscriptionReceiptTax.TaxPercent = null;
                            if (!reader.IsDBNull(5))
                                subscriptionReceiptTax.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                subscriptionReceiptTax.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                subscriptionReceiptTax.CreatedAt = reader.GetValue(6).ToString();
                            else
                                subscriptionReceiptTax.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                subscriptionReceiptTax.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                subscriptionReceiptTax.UpdatedAt = null;
                            response.Add(subscriptionReceiptTax);
                            subscriptionReceiptTax = new SubscriptionReceiptTaxModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<SubscriptionReceiptTaxModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionReceiptTaxModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionReceiptTaxModel> Create(string connectionString, SubscriptionReceiptTaxModel subscriptionReceiptTax)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptTaxModel> response = new List<SubscriptionReceiptTaxModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO subst_subscription_receipt_taxs (";
                string values = "(";
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.Id))
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
                    cmd.Parameters.AddWithValue("@id", subscriptionReceiptTax.Id);
                }
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.SubscriptionReceiptId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `subscription_receipt_id` ");
                        values = string.Concat(values, ", @subscriptionReceiptId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `subscription_receipt_id` ");
                        values = string.Concat(values, " @subscriptionReceiptId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subscriptionReceiptId", subscriptionReceiptTax.SubscriptionReceiptId);
                }
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.TaxId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `tax_id` ");
                        values = string.Concat(values, ", @taxId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `tax_id` ");
                        values = string.Concat(values, " @taxId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@taxId", subscriptionReceiptTax.TaxId);
                }
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.TaxName))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `tax_name` ");
                        values = string.Concat(values, ", @taxName");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `tax_name` ");
                        values = string.Concat(values, " @taxName");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@taxName", subscriptionReceiptTax.TaxName);
                }
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.TaxPercent))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `tax_percent` ");
                        values = string.Concat(values, ", @taxPercent");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `tax_percent` ");
                        values = string.Concat(values, " @taxPercent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@taxPercent", subscriptionReceiptTax.TaxPercent);
                }
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", subscriptionReceiptTax.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", subscriptionReceiptTax.CreatedAt);
                }
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", subscriptionReceiptTax.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                subscriptionReceiptTax.Id = cmd.LastInsertedId.ToString();
                response.Add(subscriptionReceiptTax);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionReceiptTaxModel>();
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
        public List<SubscriptionReceiptTaxModel> Create(string connectionString, List<SubscriptionReceiptTaxModel> subscriptionReceiptTaxList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptTaxModel> response = new List<SubscriptionReceiptTaxModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < subscriptionReceiptTaxList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_subscription_receipt_taxs (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, subscriptionReceiptTaxList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].SubscriptionReceiptId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `subscription_receipt_id` ");
                                        values = string.Concat(values, ", @subscriptionReceiptId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `subscription_receipt_id` ");
                                        values = string.Concat(values, "@subscriptionReceiptId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subscriptionReceiptId" + i, subscriptionReceiptTaxList[i].SubscriptionReceiptId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].TaxId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `tax_id` ");
                                        values = string.Concat(values, ", @taxId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `tax_id` ");
                                        values = string.Concat(values, "@taxId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@taxId" + i, subscriptionReceiptTaxList[i].TaxId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].TaxName))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `tax_name` ");
                                        values = string.Concat(values, ", @taxName" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `tax_name` ");
                                        values = string.Concat(values, "@taxName" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@taxName" + i, subscriptionReceiptTaxList[i].TaxName);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].TaxPercent))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `tax_percent` ");
                                        values = string.Concat(values, ", @taxPercent" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `tax_percent` ");
                                        values = string.Concat(values, "@taxPercent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@taxPercent" + i, subscriptionReceiptTaxList[i].TaxPercent);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionReceiptTaxList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionReceiptTaxList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionReceiptTaxList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(subscriptionReceiptTaxList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<SubscriptionReceiptTaxModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SubscriptionReceiptTaxModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionReceiptTaxModel>();
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

        public List<SubscriptionReceiptTaxModel> Update(string connectionString, SubscriptionReceiptTaxModel subscriptionReceiptTax, SubscriptionReceiptTaxModel subscriptionReceiptTaxWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptTaxModel> response = new List<SubscriptionReceiptTaxModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE subst_subscription_receipt_taxs SET ";
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", subscriptionReceiptTax.Id);
                }
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.SubscriptionReceiptId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `subscription_receipt_id` = @subscriptionReceiptId");
                    else
                        sql = string.Concat(sql, " `subscription_receipt_id` = @subscriptionReceiptId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subscriptionReceiptId", subscriptionReceiptTax.SubscriptionReceiptId);
                }
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.TaxId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `tax_id` = @taxId");
                    else
                        sql = string.Concat(sql, " `tax_id` = @taxId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@taxId", subscriptionReceiptTax.TaxId);
                }
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.TaxName))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `tax_name` = @taxName");
                    else
                        sql = string.Concat(sql, " `tax_name` = @taxName");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@taxName", subscriptionReceiptTax.TaxName);
                }
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.TaxPercent))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `tax_percent` = @taxPercent");
                    else
                        sql = string.Concat(sql, " `tax_percent` = @taxPercent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@taxPercent", subscriptionReceiptTax.TaxPercent);
                }
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", subscriptionReceiptTax.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", subscriptionReceiptTax.CreatedAt);
                }
                if (!string.IsNullOrEmpty(subscriptionReceiptTax.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", subscriptionReceiptTax.UpdatedAt);
                }
                if (subscriptionReceiptTaxWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", subscriptionReceiptTaxWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere.SubscriptionReceiptId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subscription_receipt_id` = @subscriptionReceiptIdWhere");
                        else
                            sql = string.Concat(sql, " `subscription_receipt_id` = @subscriptionReceiptIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionReceiptIdWhere", subscriptionReceiptTaxWhere.SubscriptionReceiptId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere.TaxId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `tax_id` = @taxIdWhere");
                        else
                            sql = string.Concat(sql, " `tax_id` = @taxIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@taxIdWhere", subscriptionReceiptTaxWhere.TaxId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere.TaxName))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `tax_name` = @taxNameWhere");
                        else
                            sql = string.Concat(sql, " `tax_name` = @taxNameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@taxNameWhere", subscriptionReceiptTaxWhere.TaxName);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere.TaxPercent))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `tax_percent` = @taxPercentWhere");
                        else
                            sql = string.Concat(sql, " `tax_percent` = @taxPercentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@taxPercentWhere", subscriptionReceiptTaxWhere.TaxPercent);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", subscriptionReceiptTaxWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", subscriptionReceiptTaxWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", subscriptionReceiptTaxWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(subscriptionReceiptTax);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionReceiptTaxModel>();
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

        public List<SubscriptionReceiptTaxModel> Update(string connectionString, List<SubscriptionReceiptTaxModel> subscriptionReceiptTaxList, List<SubscriptionReceiptTaxModel> subscriptionReceiptTaxWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptTaxModel> response = new List<SubscriptionReceiptTaxModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < subscriptionReceiptTaxList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_subscription_receipt_taxs SET ";
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, subscriptionReceiptTaxList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].SubscriptionReceiptId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `subscription_receipt_id` = @subscriptionReceiptId" + i);
                                    else
                                        sql = string.Concat(sql, " `subscription_receipt_id` = @subscriptionReceiptId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subscriptionReceiptId" + i, subscriptionReceiptTaxList[i].SubscriptionReceiptId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].TaxId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `tax_id` = @taxId" + i);
                                    else
                                        sql = string.Concat(sql, " `tax_id` = @taxId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@taxId" + i, subscriptionReceiptTaxList[i].TaxId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].TaxName))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `tax_name` = @taxName" + i);
                                    else
                                        sql = string.Concat(sql, " `tax_name` = @taxName" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@taxName" + i, subscriptionReceiptTaxList[i].TaxName);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].TaxPercent))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `tax_percent` = @taxPercent" + i);
                                    else
                                        sql = string.Concat(sql, " `tax_percent` = @taxPercent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@taxPercent" + i, subscriptionReceiptTaxList[i].TaxPercent);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionReceiptTaxList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionReceiptTaxList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptTaxList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionReceiptTaxList[i].UpdatedAt);
                                }
                                if (subscriptionReceiptTaxWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, subscriptionReceiptTaxWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhereList[i].SubscriptionReceiptId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subscription_receipt_id` = @subscriptionReceiptIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `subscription_receipt_id` = @subscriptionReceiptIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionReceiptIdWhere" + i, subscriptionReceiptTaxWhereList[i].SubscriptionReceiptId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhereList[i].TaxId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `tax_id` = @taxIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `tax_id` = @taxIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@taxIdWhere" + i, subscriptionReceiptTaxWhereList[i].TaxId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhereList[i].TaxName))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `tax_name` = @taxNameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `tax_name` = @taxNameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@taxNameWhere" + i, subscriptionReceiptTaxWhereList[i].TaxName);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhereList[i].TaxPercent))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `tax_percent` = @taxPercentWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `tax_percent` = @taxPercentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@taxPercentWhere" + i, subscriptionReceiptTaxWhereList[i].TaxPercent);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, subscriptionReceiptTaxWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, subscriptionReceiptTaxWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTaxWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, subscriptionReceiptTaxWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(subscriptionReceiptTaxList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<SubscriptionReceiptTaxModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SubscriptionReceiptTaxModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionReceiptTaxModel>();
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

        public List<SubscriptionReceiptTaxModel> Delete(string connectionString, SubscriptionReceiptTaxModel subscriptionReceiptTax)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptTaxModel> response = new List<SubscriptionReceiptTaxModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM subst_subscription_receipt_taxs  ";
                if (subscriptionReceiptTax != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(subscriptionReceiptTax.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", subscriptionReceiptTax.Id);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptTax.SubscriptionReceiptId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subscription_receipt_id` = @subscriptionReceiptId");
                        else
                            sql = string.Concat(sql, " `subscription_receipt_id` = @subscriptionReceiptId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionReceiptId", subscriptionReceiptTax.SubscriptionReceiptId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptTax.TaxId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `tax_id` = @taxId");
                        else
                            sql = string.Concat(sql, " `tax_id` = @taxId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@taxId", subscriptionReceiptTax.TaxId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptTax.TaxName))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `tax_name` = @taxName");
                        else
                            sql = string.Concat(sql, " `tax_name` = @taxName");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@taxName", subscriptionReceiptTax.TaxName);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptTax.TaxPercent))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `tax_percent` = @taxPercent");
                        else
                            sql = string.Concat(sql, " `tax_percent` = @taxPercent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@taxPercent", subscriptionReceiptTax.TaxPercent);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptTax.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", subscriptionReceiptTax.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptTax.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", subscriptionReceiptTax.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptTax.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", subscriptionReceiptTax.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(subscriptionReceiptTax);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionReceiptTaxModel>();
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

        public List<SubscriptionReceiptTaxModel> Delete(string connectionString, List<SubscriptionReceiptTaxModel> subscriptionReceiptTax)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptTaxModel> response = new List<SubscriptionReceiptTaxModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < subscriptionReceiptTax.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_subscription_receipt_taxs  ";
                                if (subscriptionReceiptTax[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTax[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, subscriptionReceiptTax[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTax[i].SubscriptionReceiptId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subscription_receipt_id` = @subscriptionReceiptId" + i);
                                        else
                                            sql = string.Concat(sql, " `subscription_receipt_id` = @subscriptionReceiptId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionReceiptId" + i, subscriptionReceiptTax[i].SubscriptionReceiptId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTax[i].TaxId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `tax_id` = @taxId" + i);
                                        else
                                            sql = string.Concat(sql, " `tax_id` = @taxId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@taxId" + i, subscriptionReceiptTax[i].TaxId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTax[i].TaxName))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `tax_name` = @taxName" + i);
                                        else
                                            sql = string.Concat(sql, " `tax_name` = @taxName" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@taxName" + i, subscriptionReceiptTax[i].TaxName);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTax[i].TaxPercent))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `tax_percent` = @taxPercent" + i);
                                        else
                                            sql = string.Concat(sql, " `tax_percent` = @taxPercent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@taxPercent" + i, subscriptionReceiptTax[i].TaxPercent);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTax[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionReceiptTax[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTax[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionReceiptTax[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptTax[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionReceiptTax[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(subscriptionReceiptTax[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<SubscriptionReceiptTaxModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SubscriptionReceiptTaxModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionReceiptTaxModel>();
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
