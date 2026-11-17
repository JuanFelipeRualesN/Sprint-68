using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class SubscriptionReceiptDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, SubscriptionReceiptModel> ReadD(string connectionString, SubscriptionReceiptModel subscriptionReceipt, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SubscriptionReceiptModel> response = new Dictionary<string, SubscriptionReceiptModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subscription_id`, `trial_days`, `trial_date_end`, `sub_total`, `tax_total`, `total`, `currency`, `coupon_id`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscription_receipts ";
                {
                    if (subscriptionReceipt != null)
                    {
                        if (!string.IsNullOrEmpty(subscriptionReceipt.Id))
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
                            cmd.Parameters.AddWithValue("@id", subscriptionReceipt.Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.SubscriptionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subscription_id` = @subscriptionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subscription_id` = @subscriptionId ");
                            }
                            cmd.Parameters.AddWithValue("@subscriptionId", subscriptionReceipt.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.TrialDays))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `trial_days` = @trialDays ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `trial_days` = @trialDays ");
                            }
                            cmd.Parameters.AddWithValue("@trialDays", subscriptionReceipt.TrialDays);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.TrialDateEnd))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `trial_date_end` = @trialDateEnd ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `trial_date_end` = @trialDateEnd ");
                            }
                            cmd.Parameters.AddWithValue("@trialDateEnd", subscriptionReceipt.TrialDateEnd);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.SubTotal))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `sub_total` = @subTotal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `sub_total` = @subTotal ");
                            }
                            cmd.Parameters.AddWithValue("@subTotal", subscriptionReceipt.SubTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.TaxTotal))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `tax_total` = @taxTotal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `tax_total` = @taxTotal ");
                            }
                            cmd.Parameters.AddWithValue("@taxTotal", subscriptionReceipt.TaxTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.Total))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total` = @total ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total` = @total ");
                            }
                            cmd.Parameters.AddWithValue("@total", subscriptionReceipt.Total);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.Currency))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `currency` = @currency ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `currency` = @currency ");
                            }
                            cmd.Parameters.AddWithValue("@currency", subscriptionReceipt.Currency);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.CouponId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `coupon_id` = @couponId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `coupon_id` = @couponId ");
                            }
                            cmd.Parameters.AddWithValue("@couponId", subscriptionReceipt.CouponId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", subscriptionReceipt.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", subscriptionReceipt.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", subscriptionReceipt.UpdatedAt);
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
                                    subscriptionReceipt.Id = reader.GetValue(0).ToString();
                                else
                                    subscriptionReceipt.Id = null;
                                if (!reader.IsDBNull(1))
                                    subscriptionReceipt.SubscriptionId = reader.GetValue(1).ToString();
                                else
                                    subscriptionReceipt.SubscriptionId = null;
                                if (!reader.IsDBNull(2))
                                    subscriptionReceipt.TrialDays = reader.GetValue(2).ToString();
                                else
                                    subscriptionReceipt.TrialDays = null;
                                if (!reader.IsDBNull(3))
                                    subscriptionReceipt.TrialDateEnd = reader.GetValue(3).ToString();
                                else
                                    subscriptionReceipt.TrialDateEnd = null;
                                if (!reader.IsDBNull(4))
                                    subscriptionReceipt.SubTotal = reader.GetValue(4).ToString();
                                else
                                    subscriptionReceipt.SubTotal = null;
                                if (!reader.IsDBNull(5))
                                    subscriptionReceipt.TaxTotal = reader.GetValue(5).ToString();
                                else
                                    subscriptionReceipt.TaxTotal = null;
                                if (!reader.IsDBNull(6))
                                    subscriptionReceipt.Total = reader.GetValue(6).ToString();
                                else
                                    subscriptionReceipt.Total = null;
                                if (!reader.IsDBNull(7))
                                    subscriptionReceipt.Currency = reader.GetValue(7).ToString();
                                else
                                    subscriptionReceipt.Currency = null;
                                if (!reader.IsDBNull(8))
                                    subscriptionReceipt.CouponId = reader.GetValue(8).ToString();
                                else
                                    subscriptionReceipt.CouponId = null;
                                if (!reader.IsDBNull(9))
                                    subscriptionReceipt.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    subscriptionReceipt.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    subscriptionReceipt.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    subscriptionReceipt.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    subscriptionReceipt.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    subscriptionReceipt.UpdatedAt = null;
                                response.Add(subscriptionReceipt.Id, subscriptionReceipt);
                                subscriptionReceipt = new SubscriptionReceiptModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, SubscriptionReceiptModel>();
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
                response = new Dictionary<string, SubscriptionReceiptModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, SubscriptionReceiptModel> ReadD(string connectionString, SubscriptionReceiptModel subscriptionReceipt, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SubscriptionReceiptModel> response = new Dictionary<string, SubscriptionReceiptModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subscription_id`, `trial_days`, `trial_date_end`, `sub_total`, `tax_total`, `total`, `currency`, `coupon_id`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscription_receipts ";
                {
                    if (subscriptionReceipt != null)
                    {
                        if (!string.IsNullOrEmpty(subscriptionReceipt.Id))
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
                            cmd.Parameters.AddWithValue("@id", subscriptionReceipt.Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.SubscriptionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subscription_id` = @subscriptionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subscription_id` = @subscriptionId ");
                            }
                            cmd.Parameters.AddWithValue("@subscriptionId", subscriptionReceipt.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.TrialDays))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `trial_days` = @trialDays ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `trial_days` = @trialDays ");
                            }
                            cmd.Parameters.AddWithValue("@trialDays", subscriptionReceipt.TrialDays);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.TrialDateEnd))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `trial_date_end` = @trialDateEnd ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `trial_date_end` = @trialDateEnd ");
                            }
                            cmd.Parameters.AddWithValue("@trialDateEnd", subscriptionReceipt.TrialDateEnd);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.SubTotal))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `sub_total` = @subTotal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `sub_total` = @subTotal ");
                            }
                            cmd.Parameters.AddWithValue("@subTotal", subscriptionReceipt.SubTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.TaxTotal))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `tax_total` = @taxTotal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `tax_total` = @taxTotal ");
                            }
                            cmd.Parameters.AddWithValue("@taxTotal", subscriptionReceipt.TaxTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.Total))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total` = @total ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total` = @total ");
                            }
                            cmd.Parameters.AddWithValue("@total", subscriptionReceipt.Total);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.Currency))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `currency` = @currency ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `currency` = @currency ");
                            }
                            cmd.Parameters.AddWithValue("@currency", subscriptionReceipt.Currency);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.CouponId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `coupon_id` = @couponId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `coupon_id` = @couponId ");
                            }
                            cmd.Parameters.AddWithValue("@couponId", subscriptionReceipt.CouponId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", subscriptionReceipt.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", subscriptionReceipt.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", subscriptionReceipt.UpdatedAt);
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
                                    subscriptionReceipt.Id = reader.GetValue(0).ToString();
                                else
                                    subscriptionReceipt.Id = null;
                                if (!reader.IsDBNull(1))
                                    subscriptionReceipt.SubscriptionId = reader.GetValue(1).ToString();
                                else
                                    subscriptionReceipt.SubscriptionId = null;
                                if (!reader.IsDBNull(2))
                                    subscriptionReceipt.TrialDays = reader.GetValue(2).ToString();
                                else
                                    subscriptionReceipt.TrialDays = null;
                                if (!reader.IsDBNull(3))
                                    subscriptionReceipt.TrialDateEnd = reader.GetValue(3).ToString();
                                else
                                    subscriptionReceipt.TrialDateEnd = null;
                                if (!reader.IsDBNull(4))
                                    subscriptionReceipt.SubTotal = reader.GetValue(4).ToString();
                                else
                                    subscriptionReceipt.SubTotal = null;
                                if (!reader.IsDBNull(5))
                                    subscriptionReceipt.TaxTotal = reader.GetValue(5).ToString();
                                else
                                    subscriptionReceipt.TaxTotal = null;
                                if (!reader.IsDBNull(6))
                                    subscriptionReceipt.Total = reader.GetValue(6).ToString();
                                else
                                    subscriptionReceipt.Total = null;
                                if (!reader.IsDBNull(7))
                                    subscriptionReceipt.Currency = reader.GetValue(7).ToString();
                                else
                                    subscriptionReceipt.Currency = null;
                                if (!reader.IsDBNull(8))
                                    subscriptionReceipt.CouponId = reader.GetValue(8).ToString();
                                else
                                    subscriptionReceipt.CouponId = null;
                                if (!reader.IsDBNull(9))
                                    subscriptionReceipt.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    subscriptionReceipt.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    subscriptionReceipt.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    subscriptionReceipt.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    subscriptionReceipt.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    subscriptionReceipt.UpdatedAt = null;
                                response.Add(subscriptionReceipt.Id, subscriptionReceipt);
                                subscriptionReceipt = new SubscriptionReceiptModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, SubscriptionReceiptModel>();
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
                response = new Dictionary<string, SubscriptionReceiptModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, SubscriptionReceiptModel> ReadD(string connectionString, List<SubscriptionReceiptModel> subscriptionReceiptWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SubscriptionReceiptModel> response = new Dictionary<string, SubscriptionReceiptModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subscription_id`, `trial_days`, `trial_date_end`, `sub_total`, `tax_total`, `total`, `currency`, `coupon_id`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscription_receipts ";
                if (subscriptionReceiptWhere != null && subscriptionReceiptWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < subscriptionReceiptWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, subscriptionReceiptWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, subscriptionReceiptWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].TrialDays))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `trial_days` = @trialDays" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@trialDays" + i, subscriptionReceiptWhere[i].TrialDays);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].TrialDateEnd))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `trial_date_end` = @trialDateEnd" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@trialDateEnd" + i, subscriptionReceiptWhere[i].TrialDateEnd);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].SubTotal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sub_total` = @subTotal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subTotal" + i, subscriptionReceiptWhere[i].SubTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].TaxTotal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tax_total` = @taxTotal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@taxTotal" + i, subscriptionReceiptWhere[i].TaxTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].Total))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total` = @total" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@total" + i, subscriptionReceiptWhere[i].Total);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].Currency))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `currency` = @currency" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currency" + i, subscriptionReceiptWhere[i].Currency);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].CouponId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `coupon_id` = @couponId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@couponId" + i, subscriptionReceiptWhere[i].CouponId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionReceiptWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionReceiptWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionReceiptWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != subscriptionReceiptWhere.Count - 1)
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
                            SubscriptionReceiptModel subscriptionReceipt = new SubscriptionReceiptModel(); if (!reader.IsDBNull(0))
                                subscriptionReceipt.Id = reader.GetValue(0).ToString();
                            else
                                subscriptionReceipt.Id = null;
                            if (!reader.IsDBNull(1))
                                subscriptionReceipt.SubscriptionId = reader.GetValue(1).ToString();
                            else
                                subscriptionReceipt.SubscriptionId = null;
                            if (!reader.IsDBNull(2))
                                subscriptionReceipt.TrialDays = reader.GetValue(2).ToString();
                            else
                                subscriptionReceipt.TrialDays = null;
                            if (!reader.IsDBNull(3))
                                subscriptionReceipt.TrialDateEnd = reader.GetValue(3).ToString();
                            else
                                subscriptionReceipt.TrialDateEnd = null;
                            if (!reader.IsDBNull(4))
                                subscriptionReceipt.SubTotal = reader.GetValue(4).ToString();
                            else
                                subscriptionReceipt.SubTotal = null;
                            if (!reader.IsDBNull(5))
                                subscriptionReceipt.TaxTotal = reader.GetValue(5).ToString();
                            else
                                subscriptionReceipt.TaxTotal = null;
                            if (!reader.IsDBNull(6))
                                subscriptionReceipt.Total = reader.GetValue(6).ToString();
                            else
                                subscriptionReceipt.Total = null;
                            if (!reader.IsDBNull(7))
                                subscriptionReceipt.Currency = reader.GetValue(7).ToString();
                            else
                                subscriptionReceipt.Currency = null;
                            if (!reader.IsDBNull(8))
                                subscriptionReceipt.CouponId = reader.GetValue(8).ToString();
                            else
                                subscriptionReceipt.CouponId = null;
                            if (!reader.IsDBNull(9))
                                subscriptionReceipt.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                subscriptionReceipt.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                subscriptionReceipt.CreatedAt = reader.GetValue(10).ToString();
                            else
                                subscriptionReceipt.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                subscriptionReceipt.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                subscriptionReceipt.UpdatedAt = null;
                            response.Add(subscriptionReceipt.Id, subscriptionReceipt);
                            subscriptionReceipt = new SubscriptionReceiptModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, SubscriptionReceiptModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, SubscriptionReceiptModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, SubscriptionReceiptModel> ReadD(string connectionString, List<SubscriptionReceiptModel> subscriptionReceiptWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SubscriptionReceiptModel> response = new Dictionary<string, SubscriptionReceiptModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subscription_id`, `trial_days`, `trial_date_end`, `sub_total`, `tax_total`, `total`, `currency`, `coupon_id`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscription_receipts ";
                if (subscriptionReceiptWhere != null && subscriptionReceiptWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < subscriptionReceiptWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, subscriptionReceiptWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, subscriptionReceiptWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].TrialDays))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `trial_days` = @trialDays" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@trialDays" + i, subscriptionReceiptWhere[i].TrialDays);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].TrialDateEnd))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `trial_date_end` = @trialDateEnd" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@trialDateEnd" + i, subscriptionReceiptWhere[i].TrialDateEnd);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].SubTotal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sub_total` = @subTotal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subTotal" + i, subscriptionReceiptWhere[i].SubTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].TaxTotal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tax_total` = @taxTotal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@taxTotal" + i, subscriptionReceiptWhere[i].TaxTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].Total))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total` = @total" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@total" + i, subscriptionReceiptWhere[i].Total);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].Currency))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `currency` = @currency" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currency" + i, subscriptionReceiptWhere[i].Currency);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].CouponId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `coupon_id` = @couponId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@couponId" + i, subscriptionReceiptWhere[i].CouponId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionReceiptWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionReceiptWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionReceiptWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != subscriptionReceiptWhere.Count - 1)
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
                            SubscriptionReceiptModel subscriptionReceipt = new SubscriptionReceiptModel(); if (!reader.IsDBNull(0))
                                subscriptionReceipt.Id = reader.GetValue(0).ToString();
                            else
                                subscriptionReceipt.Id = null;
                            if (!reader.IsDBNull(1))
                                subscriptionReceipt.SubscriptionId = reader.GetValue(1).ToString();
                            else
                                subscriptionReceipt.SubscriptionId = null;
                            if (!reader.IsDBNull(2))
                                subscriptionReceipt.TrialDays = reader.GetValue(2).ToString();
                            else
                                subscriptionReceipt.TrialDays = null;
                            if (!reader.IsDBNull(3))
                                subscriptionReceipt.TrialDateEnd = reader.GetValue(3).ToString();
                            else
                                subscriptionReceipt.TrialDateEnd = null;
                            if (!reader.IsDBNull(4))
                                subscriptionReceipt.SubTotal = reader.GetValue(4).ToString();
                            else
                                subscriptionReceipt.SubTotal = null;
                            if (!reader.IsDBNull(5))
                                subscriptionReceipt.TaxTotal = reader.GetValue(5).ToString();
                            else
                                subscriptionReceipt.TaxTotal = null;
                            if (!reader.IsDBNull(6))
                                subscriptionReceipt.Total = reader.GetValue(6).ToString();
                            else
                                subscriptionReceipt.Total = null;
                            if (!reader.IsDBNull(7))
                                subscriptionReceipt.Currency = reader.GetValue(7).ToString();
                            else
                                subscriptionReceipt.Currency = null;
                            if (!reader.IsDBNull(8))
                                subscriptionReceipt.CouponId = reader.GetValue(8).ToString();
                            else
                                subscriptionReceipt.CouponId = null;
                            if (!reader.IsDBNull(9))
                                subscriptionReceipt.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                subscriptionReceipt.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                subscriptionReceipt.CreatedAt = reader.GetValue(10).ToString();
                            else
                                subscriptionReceipt.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                subscriptionReceipt.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                subscriptionReceipt.UpdatedAt = null;
                            response.Add(subscriptionReceipt.Id, subscriptionReceipt);
                            subscriptionReceipt = new SubscriptionReceiptModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, SubscriptionReceiptModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, SubscriptionReceiptModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionReceiptModel> Read(string connectionString, SubscriptionReceiptModel subscriptionReceipt, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptModel> response = new List<SubscriptionReceiptModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subscription_id`, `trial_days`, `trial_date_end`, `sub_total`, `tax_total`, `total`, `currency`, `coupon_id`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscription_receipts ";
                {
                    if (subscriptionReceipt != null)
                    {
                        if (!string.IsNullOrEmpty(subscriptionReceipt.Id))
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
                            cmd.Parameters.AddWithValue("@id", subscriptionReceipt.Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.SubscriptionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subscription_id` = @subscriptionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subscription_id` = @subscriptionId ");
                            }
                            cmd.Parameters.AddWithValue("@subscriptionId", subscriptionReceipt.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.TrialDays))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `trial_days` = @trialDays ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `trial_days` = @trialDays ");
                            }
                            cmd.Parameters.AddWithValue("@trialDays", subscriptionReceipt.TrialDays);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.TrialDateEnd))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `trial_date_end` = @trialDateEnd ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `trial_date_end` = @trialDateEnd ");
                            }
                            cmd.Parameters.AddWithValue("@trialDateEnd", subscriptionReceipt.TrialDateEnd);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.SubTotal))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `sub_total` = @subTotal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `sub_total` = @subTotal ");
                            }
                            cmd.Parameters.AddWithValue("@subTotal", subscriptionReceipt.SubTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.TaxTotal))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `tax_total` = @taxTotal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `tax_total` = @taxTotal ");
                            }
                            cmd.Parameters.AddWithValue("@taxTotal", subscriptionReceipt.TaxTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.Total))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total` = @total ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total` = @total ");
                            }
                            cmd.Parameters.AddWithValue("@total", subscriptionReceipt.Total);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.Currency))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `currency` = @currency ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `currency` = @currency ");
                            }
                            cmd.Parameters.AddWithValue("@currency", subscriptionReceipt.Currency);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.CouponId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `coupon_id` = @couponId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `coupon_id` = @couponId ");
                            }
                            cmd.Parameters.AddWithValue("@couponId", subscriptionReceipt.CouponId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", subscriptionReceipt.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", subscriptionReceipt.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", subscriptionReceipt.UpdatedAt);
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
                                    subscriptionReceipt.Id = reader.GetValue(0).ToString();
                                else
                                    subscriptionReceipt.Id = null;
                                if (!reader.IsDBNull(1))
                                    subscriptionReceipt.SubscriptionId = reader.GetValue(1).ToString();
                                else
                                    subscriptionReceipt.SubscriptionId = null;
                                if (!reader.IsDBNull(2))
                                    subscriptionReceipt.TrialDays = reader.GetValue(2).ToString();
                                else
                                    subscriptionReceipt.TrialDays = null;
                                if (!reader.IsDBNull(3))
                                    subscriptionReceipt.TrialDateEnd = reader.GetValue(3).ToString();
                                else
                                    subscriptionReceipt.TrialDateEnd = null;
                                if (!reader.IsDBNull(4))
                                    subscriptionReceipt.SubTotal = reader.GetValue(4).ToString();
                                else
                                    subscriptionReceipt.SubTotal = null;
                                if (!reader.IsDBNull(5))
                                    subscriptionReceipt.TaxTotal = reader.GetValue(5).ToString();
                                else
                                    subscriptionReceipt.TaxTotal = null;
                                if (!reader.IsDBNull(6))
                                    subscriptionReceipt.Total = reader.GetValue(6).ToString();
                                else
                                    subscriptionReceipt.Total = null;
                                if (!reader.IsDBNull(7))
                                    subscriptionReceipt.Currency = reader.GetValue(7).ToString();
                                else
                                    subscriptionReceipt.Currency = null;
                                if (!reader.IsDBNull(8))
                                    subscriptionReceipt.CouponId = reader.GetValue(8).ToString();
                                else
                                    subscriptionReceipt.CouponId = null;
                                if (!reader.IsDBNull(9))
                                    subscriptionReceipt.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    subscriptionReceipt.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    subscriptionReceipt.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    subscriptionReceipt.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    subscriptionReceipt.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    subscriptionReceipt.UpdatedAt = null;
                                response.Add(subscriptionReceipt);
                                subscriptionReceipt = new SubscriptionReceiptModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<SubscriptionReceiptModel>();
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
                response = new List<SubscriptionReceiptModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionReceiptModel> Read(string connectionString, SubscriptionReceiptModel subscriptionReceipt, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptModel> response = new List<SubscriptionReceiptModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subscription_id`, `trial_days`, `trial_date_end`, `sub_total`, `tax_total`, `total`, `currency`, `coupon_id`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscription_receipts ";
                {
                    if (subscriptionReceipt != null)
                    {
                        if (!string.IsNullOrEmpty(subscriptionReceipt.Id))
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
                            cmd.Parameters.AddWithValue("@id", subscriptionReceipt.Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.SubscriptionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subscription_id` = @subscriptionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subscription_id` = @subscriptionId ");
                            }
                            cmd.Parameters.AddWithValue("@subscriptionId", subscriptionReceipt.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.TrialDays))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `trial_days` = @trialDays ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `trial_days` = @trialDays ");
                            }
                            cmd.Parameters.AddWithValue("@trialDays", subscriptionReceipt.TrialDays);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.TrialDateEnd))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `trial_date_end` = @trialDateEnd ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `trial_date_end` = @trialDateEnd ");
                            }
                            cmd.Parameters.AddWithValue("@trialDateEnd", subscriptionReceipt.TrialDateEnd);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.SubTotal))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `sub_total` = @subTotal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `sub_total` = @subTotal ");
                            }
                            cmd.Parameters.AddWithValue("@subTotal", subscriptionReceipt.SubTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.TaxTotal))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `tax_total` = @taxTotal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `tax_total` = @taxTotal ");
                            }
                            cmd.Parameters.AddWithValue("@taxTotal", subscriptionReceipt.TaxTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.Total))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total` = @total ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total` = @total ");
                            }
                            cmd.Parameters.AddWithValue("@total", subscriptionReceipt.Total);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.Currency))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `currency` = @currency ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `currency` = @currency ");
                            }
                            cmd.Parameters.AddWithValue("@currency", subscriptionReceipt.Currency);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.CouponId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `coupon_id` = @couponId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `coupon_id` = @couponId ");
                            }
                            cmd.Parameters.AddWithValue("@couponId", subscriptionReceipt.CouponId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", subscriptionReceipt.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", subscriptionReceipt.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceipt.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", subscriptionReceipt.UpdatedAt);
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
                                    subscriptionReceipt.Id = reader.GetValue(0).ToString();
                                else
                                    subscriptionReceipt.Id = null;
                                if (!reader.IsDBNull(1))
                                    subscriptionReceipt.SubscriptionId = reader.GetValue(1).ToString();
                                else
                                    subscriptionReceipt.SubscriptionId = null;
                                if (!reader.IsDBNull(2))
                                    subscriptionReceipt.TrialDays = reader.GetValue(2).ToString();
                                else
                                    subscriptionReceipt.TrialDays = null;
                                if (!reader.IsDBNull(3))
                                    subscriptionReceipt.TrialDateEnd = reader.GetValue(3).ToString();
                                else
                                    subscriptionReceipt.TrialDateEnd = null;
                                if (!reader.IsDBNull(4))
                                    subscriptionReceipt.SubTotal = reader.GetValue(4).ToString();
                                else
                                    subscriptionReceipt.SubTotal = null;
                                if (!reader.IsDBNull(5))
                                    subscriptionReceipt.TaxTotal = reader.GetValue(5).ToString();
                                else
                                    subscriptionReceipt.TaxTotal = null;
                                if (!reader.IsDBNull(6))
                                    subscriptionReceipt.Total = reader.GetValue(6).ToString();
                                else
                                    subscriptionReceipt.Total = null;
                                if (!reader.IsDBNull(7))
                                    subscriptionReceipt.Currency = reader.GetValue(7).ToString();
                                else
                                    subscriptionReceipt.Currency = null;
                                if (!reader.IsDBNull(8))
                                    subscriptionReceipt.CouponId = reader.GetValue(8).ToString();
                                else
                                    subscriptionReceipt.CouponId = null;
                                if (!reader.IsDBNull(9))
                                    subscriptionReceipt.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    subscriptionReceipt.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    subscriptionReceipt.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    subscriptionReceipt.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    subscriptionReceipt.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    subscriptionReceipt.UpdatedAt = null;
                                response.Add(subscriptionReceipt);
                                subscriptionReceipt = new SubscriptionReceiptModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<SubscriptionReceiptModel>();
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
                response = new List<SubscriptionReceiptModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionReceiptModel> Read(string connectionString, List<SubscriptionReceiptModel> subscriptionReceiptWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptModel> response = new List<SubscriptionReceiptModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subscription_id`, `trial_days`, `trial_date_end`, `sub_total`, `tax_total`, `total`, `currency`, `coupon_id`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscription_receipts ";
                if (subscriptionReceiptWhere != null && subscriptionReceiptWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < subscriptionReceiptWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, subscriptionReceiptWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, subscriptionReceiptWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].TrialDays))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `trial_days` = @trialDays" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@trialDays" + i, subscriptionReceiptWhere[i].TrialDays);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].TrialDateEnd))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `trial_date_end` = @trialDateEnd" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@trialDateEnd" + i, subscriptionReceiptWhere[i].TrialDateEnd);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].SubTotal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sub_total` = @subTotal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subTotal" + i, subscriptionReceiptWhere[i].SubTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].TaxTotal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tax_total` = @taxTotal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@taxTotal" + i, subscriptionReceiptWhere[i].TaxTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].Total))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total` = @total" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@total" + i, subscriptionReceiptWhere[i].Total);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].Currency))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `currency` = @currency" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currency" + i, subscriptionReceiptWhere[i].Currency);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].CouponId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `coupon_id` = @couponId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@couponId" + i, subscriptionReceiptWhere[i].CouponId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionReceiptWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionReceiptWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionReceiptWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != subscriptionReceiptWhere.Count - 1)
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
                            SubscriptionReceiptModel subscriptionReceipt = new SubscriptionReceiptModel(); if (!reader.IsDBNull(0))
                                subscriptionReceipt.Id = reader.GetValue(0).ToString();
                            else
                                subscriptionReceipt.Id = null;
                            if (!reader.IsDBNull(1))
                                subscriptionReceipt.SubscriptionId = reader.GetValue(1).ToString();
                            else
                                subscriptionReceipt.SubscriptionId = null;
                            if (!reader.IsDBNull(2))
                                subscriptionReceipt.TrialDays = reader.GetValue(2).ToString();
                            else
                                subscriptionReceipt.TrialDays = null;
                            if (!reader.IsDBNull(3))
                                subscriptionReceipt.TrialDateEnd = reader.GetValue(3).ToString();
                            else
                                subscriptionReceipt.TrialDateEnd = null;
                            if (!reader.IsDBNull(4))
                                subscriptionReceipt.SubTotal = reader.GetValue(4).ToString();
                            else
                                subscriptionReceipt.SubTotal = null;
                            if (!reader.IsDBNull(5))
                                subscriptionReceipt.TaxTotal = reader.GetValue(5).ToString();
                            else
                                subscriptionReceipt.TaxTotal = null;
                            if (!reader.IsDBNull(6))
                                subscriptionReceipt.Total = reader.GetValue(6).ToString();
                            else
                                subscriptionReceipt.Total = null;
                            if (!reader.IsDBNull(7))
                                subscriptionReceipt.Currency = reader.GetValue(7).ToString();
                            else
                                subscriptionReceipt.Currency = null;
                            if (!reader.IsDBNull(8))
                                subscriptionReceipt.CouponId = reader.GetValue(8).ToString();
                            else
                                subscriptionReceipt.CouponId = null;
                            if (!reader.IsDBNull(9))
                                subscriptionReceipt.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                subscriptionReceipt.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                subscriptionReceipt.CreatedAt = reader.GetValue(10).ToString();
                            else
                                subscriptionReceipt.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                subscriptionReceipt.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                subscriptionReceipt.UpdatedAt = null;
                            response.Add(subscriptionReceipt);
                            subscriptionReceipt = new SubscriptionReceiptModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<SubscriptionReceiptModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionReceiptModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionReceiptModel> Read(string connectionString, List<SubscriptionReceiptModel> subscriptionReceiptWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptModel> response = new List<SubscriptionReceiptModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subscription_id`, `trial_days`, `trial_date_end`, `sub_total`, `tax_total`, `total`, `currency`, `coupon_id`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscription_receipts ";
                if (subscriptionReceiptWhere != null && subscriptionReceiptWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < subscriptionReceiptWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, subscriptionReceiptWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, subscriptionReceiptWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].TrialDays))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `trial_days` = @trialDays" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@trialDays" + i, subscriptionReceiptWhere[i].TrialDays);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].TrialDateEnd))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `trial_date_end` = @trialDateEnd" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@trialDateEnd" + i, subscriptionReceiptWhere[i].TrialDateEnd);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].SubTotal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sub_total` = @subTotal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subTotal" + i, subscriptionReceiptWhere[i].SubTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].TaxTotal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tax_total` = @taxTotal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@taxTotal" + i, subscriptionReceiptWhere[i].TaxTotal);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].Total))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total` = @total" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@total" + i, subscriptionReceiptWhere[i].Total);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].Currency))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `currency` = @currency" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currency" + i, subscriptionReceiptWhere[i].Currency);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].CouponId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `coupon_id` = @couponId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@couponId" + i, subscriptionReceiptWhere[i].CouponId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionReceiptWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionReceiptWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionReceiptWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionReceiptWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != subscriptionReceiptWhere.Count - 1)
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
                            SubscriptionReceiptModel subscriptionReceipt = new SubscriptionReceiptModel(); if (!reader.IsDBNull(0))
                                subscriptionReceipt.Id = reader.GetValue(0).ToString();
                            else
                                subscriptionReceipt.Id = null;
                            if (!reader.IsDBNull(1))
                                subscriptionReceipt.SubscriptionId = reader.GetValue(1).ToString();
                            else
                                subscriptionReceipt.SubscriptionId = null;
                            if (!reader.IsDBNull(2))
                                subscriptionReceipt.TrialDays = reader.GetValue(2).ToString();
                            else
                                subscriptionReceipt.TrialDays = null;
                            if (!reader.IsDBNull(3))
                                subscriptionReceipt.TrialDateEnd = reader.GetValue(3).ToString();
                            else
                                subscriptionReceipt.TrialDateEnd = null;
                            if (!reader.IsDBNull(4))
                                subscriptionReceipt.SubTotal = reader.GetValue(4).ToString();
                            else
                                subscriptionReceipt.SubTotal = null;
                            if (!reader.IsDBNull(5))
                                subscriptionReceipt.TaxTotal = reader.GetValue(5).ToString();
                            else
                                subscriptionReceipt.TaxTotal = null;
                            if (!reader.IsDBNull(6))
                                subscriptionReceipt.Total = reader.GetValue(6).ToString();
                            else
                                subscriptionReceipt.Total = null;
                            if (!reader.IsDBNull(7))
                                subscriptionReceipt.Currency = reader.GetValue(7).ToString();
                            else
                                subscriptionReceipt.Currency = null;
                            if (!reader.IsDBNull(8))
                                subscriptionReceipt.CouponId = reader.GetValue(8).ToString();
                            else
                                subscriptionReceipt.CouponId = null;
                            if (!reader.IsDBNull(9))
                                subscriptionReceipt.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                subscriptionReceipt.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                subscriptionReceipt.CreatedAt = reader.GetValue(10).ToString();
                            else
                                subscriptionReceipt.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                subscriptionReceipt.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                subscriptionReceipt.UpdatedAt = null;
                            response.Add(subscriptionReceipt);
                            subscriptionReceipt = new SubscriptionReceiptModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<SubscriptionReceiptModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionReceiptModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionReceiptModel> Create(string connectionString, SubscriptionReceiptModel subscriptionReceipt)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptModel> response = new List<SubscriptionReceiptModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO subst_subscription_receipts (";
                string values = "(";
                if (!string.IsNullOrEmpty(subscriptionReceipt.Id))
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
                    cmd.Parameters.AddWithValue("@id", subscriptionReceipt.Id);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.SubscriptionId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `subscription_id` ");
                        values = string.Concat(values, ", @subscriptionId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `subscription_id` ");
                        values = string.Concat(values, " @subscriptionId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subscriptionId", subscriptionReceipt.SubscriptionId);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.TrialDays))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `trial_days` ");
                        values = string.Concat(values, ", @trialDays");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `trial_days` ");
                        values = string.Concat(values, " @trialDays");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@trialDays", subscriptionReceipt.TrialDays);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.TrialDateEnd))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `trial_date_end` ");
                        values = string.Concat(values, ", @trialDateEnd");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `trial_date_end` ");
                        values = string.Concat(values, " @trialDateEnd");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@trialDateEnd", subscriptionReceipt.TrialDateEnd);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.SubTotal))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `sub_total` ");
                        values = string.Concat(values, ", @subTotal");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `sub_total` ");
                        values = string.Concat(values, " @subTotal");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subTotal", subscriptionReceipt.SubTotal);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.TaxTotal))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `tax_total` ");
                        values = string.Concat(values, ", @taxTotal");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `tax_total` ");
                        values = string.Concat(values, " @taxTotal");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@taxTotal", subscriptionReceipt.TaxTotal);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.Total))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `total` ");
                        values = string.Concat(values, ", @total");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `total` ");
                        values = string.Concat(values, " @total");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@total", subscriptionReceipt.Total);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.Currency))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `currency` ");
                        values = string.Concat(values, ", @currency");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `currency` ");
                        values = string.Concat(values, " @currency");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@currency", subscriptionReceipt.Currency);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.CouponId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `coupon_id` ");
                        values = string.Concat(values, ", @couponId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `coupon_id` ");
                        values = string.Concat(values, " @couponId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@couponId", subscriptionReceipt.CouponId);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", subscriptionReceipt.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", subscriptionReceipt.CreatedAt);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", subscriptionReceipt.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                subscriptionReceipt.Id = cmd.LastInsertedId.ToString();
                response.Add(subscriptionReceipt);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionReceiptModel>();
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
        public List<SubscriptionReceiptModel> Create(string connectionString, List<SubscriptionReceiptModel> subscriptionReceiptList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptModel> response = new List<SubscriptionReceiptModel>();
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
                        for (int i = 0; i < subscriptionReceiptList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_subscription_receipts (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, subscriptionReceiptList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].SubscriptionId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `subscription_id` ");
                                        values = string.Concat(values, ", @subscriptionId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `subscription_id` ");
                                        values = string.Concat(values, "@subscriptionId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subscriptionId" + i, subscriptionReceiptList[i].SubscriptionId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].TrialDays))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `trial_days` ");
                                        values = string.Concat(values, ", @trialDays" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `trial_days` ");
                                        values = string.Concat(values, "@trialDays" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@trialDays" + i, subscriptionReceiptList[i].TrialDays);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].TrialDateEnd))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `trial_date_end` ");
                                        values = string.Concat(values, ", @trialDateEnd" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `trial_date_end` ");
                                        values = string.Concat(values, "@trialDateEnd" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@trialDateEnd" + i, subscriptionReceiptList[i].TrialDateEnd);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].SubTotal))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `sub_total` ");
                                        values = string.Concat(values, ", @subTotal" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `sub_total` ");
                                        values = string.Concat(values, "@subTotal" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subTotal" + i, subscriptionReceiptList[i].SubTotal);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].TaxTotal))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `tax_total` ");
                                        values = string.Concat(values, ", @taxTotal" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `tax_total` ");
                                        values = string.Concat(values, "@taxTotal" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@taxTotal" + i, subscriptionReceiptList[i].TaxTotal);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].Total))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `total` ");
                                        values = string.Concat(values, ", @total" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `total` ");
                                        values = string.Concat(values, "@total" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@total" + i, subscriptionReceiptList[i].Total);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].Currency))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `currency` ");
                                        values = string.Concat(values, ", @currency" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `currency` ");
                                        values = string.Concat(values, "@currency" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@currency" + i, subscriptionReceiptList[i].Currency);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].CouponId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `coupon_id` ");
                                        values = string.Concat(values, ", @couponId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `coupon_id` ");
                                        values = string.Concat(values, "@couponId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@couponId" + i, subscriptionReceiptList[i].CouponId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionReceiptList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionReceiptList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionReceiptList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(subscriptionReceiptList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<SubscriptionReceiptModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SubscriptionReceiptModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionReceiptModel>();
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

        public List<SubscriptionReceiptModel> Update(string connectionString, SubscriptionReceiptModel subscriptionReceipt, SubscriptionReceiptModel subscriptionReceiptWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptModel> response = new List<SubscriptionReceiptModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE subst_subscription_receipts SET ";
                if (!string.IsNullOrEmpty(subscriptionReceipt.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", subscriptionReceipt.Id);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.SubscriptionId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `subscription_id` = @subscriptionId");
                    else
                        sql = string.Concat(sql, " `subscription_id` = @subscriptionId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subscriptionId", subscriptionReceipt.SubscriptionId);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.TrialDays))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `trial_days` = @trialDays");
                    else
                        sql = string.Concat(sql, " `trial_days` = @trialDays");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@trialDays", subscriptionReceipt.TrialDays);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.TrialDateEnd))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `trial_date_end` = @trialDateEnd");
                    else
                        sql = string.Concat(sql, " `trial_date_end` = @trialDateEnd");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@trialDateEnd", subscriptionReceipt.TrialDateEnd);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.SubTotal))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `sub_total` = @subTotal");
                    else
                        sql = string.Concat(sql, " `sub_total` = @subTotal");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subTotal", subscriptionReceipt.SubTotal);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.TaxTotal))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `tax_total` = @taxTotal");
                    else
                        sql = string.Concat(sql, " `tax_total` = @taxTotal");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@taxTotal", subscriptionReceipt.TaxTotal);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.Total))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `total` = @total");
                    else
                        sql = string.Concat(sql, " `total` = @total");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@total", subscriptionReceipt.Total);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.Currency))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `currency` = @currency");
                    else
                        sql = string.Concat(sql, " `currency` = @currency");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@currency", subscriptionReceipt.Currency);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.CouponId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `coupon_id` = @couponId");
                    else
                        sql = string.Concat(sql, " `coupon_id` = @couponId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@couponId", subscriptionReceipt.CouponId);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", subscriptionReceipt.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", subscriptionReceipt.CreatedAt);
                }
                if (!string.IsNullOrEmpty(subscriptionReceipt.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", subscriptionReceipt.UpdatedAt);
                }
                if (subscriptionReceiptWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(subscriptionReceiptWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", subscriptionReceiptWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptWhere.SubscriptionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionIdWhere");
                        else
                            sql = string.Concat(sql, " `subscription_id` = @subscriptionIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionIdWhere", subscriptionReceiptWhere.SubscriptionId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptWhere.TrialDays))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `trial_days` = @trialDaysWhere");
                        else
                            sql = string.Concat(sql, " `trial_days` = @trialDaysWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@trialDaysWhere", subscriptionReceiptWhere.TrialDays);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptWhere.TrialDateEnd))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `trial_date_end` = @trialDateEndWhere");
                        else
                            sql = string.Concat(sql, " `trial_date_end` = @trialDateEndWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@trialDateEndWhere", subscriptionReceiptWhere.TrialDateEnd);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptWhere.SubTotal))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `sub_total` = @subTotalWhere");
                        else
                            sql = string.Concat(sql, " `sub_total` = @subTotalWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subTotalWhere", subscriptionReceiptWhere.SubTotal);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptWhere.TaxTotal))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `tax_total` = @taxTotalWhere");
                        else
                            sql = string.Concat(sql, " `tax_total` = @taxTotalWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@taxTotalWhere", subscriptionReceiptWhere.TaxTotal);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptWhere.Total))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `total` = @totalWhere");
                        else
                            sql = string.Concat(sql, " `total` = @totalWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalWhere", subscriptionReceiptWhere.Total);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptWhere.Currency))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `currency` = @currencyWhere");
                        else
                            sql = string.Concat(sql, " `currency` = @currencyWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@currencyWhere", subscriptionReceiptWhere.Currency);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptWhere.CouponId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `coupon_id` = @couponIdWhere");
                        else
                            sql = string.Concat(sql, " `coupon_id` = @couponIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@couponIdWhere", subscriptionReceiptWhere.CouponId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", subscriptionReceiptWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", subscriptionReceiptWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceiptWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", subscriptionReceiptWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(subscriptionReceipt);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionReceiptModel>();
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

        public List<SubscriptionReceiptModel> Update(string connectionString, List<SubscriptionReceiptModel> subscriptionReceiptList, List<SubscriptionReceiptModel> subscriptionReceiptWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptModel> response = new List<SubscriptionReceiptModel>();
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
                        for (int i = 0; i < subscriptionReceiptList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_subscription_receipts SET ";
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, subscriptionReceiptList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].SubscriptionId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `subscription_id` = @subscriptionId" + i);
                                    else
                                        sql = string.Concat(sql, " `subscription_id` = @subscriptionId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subscriptionId" + i, subscriptionReceiptList[i].SubscriptionId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].TrialDays))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `trial_days` = @trialDays" + i);
                                    else
                                        sql = string.Concat(sql, " `trial_days` = @trialDays" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@trialDays" + i, subscriptionReceiptList[i].TrialDays);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].TrialDateEnd))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `trial_date_end` = @trialDateEnd" + i);
                                    else
                                        sql = string.Concat(sql, " `trial_date_end` = @trialDateEnd" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@trialDateEnd" + i, subscriptionReceiptList[i].TrialDateEnd);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].SubTotal))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `sub_total` = @subTotal" + i);
                                    else
                                        sql = string.Concat(sql, " `sub_total` = @subTotal" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subTotal" + i, subscriptionReceiptList[i].SubTotal);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].TaxTotal))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `tax_total` = @taxTotal" + i);
                                    else
                                        sql = string.Concat(sql, " `tax_total` = @taxTotal" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@taxTotal" + i, subscriptionReceiptList[i].TaxTotal);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].Total))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `total` = @total" + i);
                                    else
                                        sql = string.Concat(sql, " `total` = @total" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@total" + i, subscriptionReceiptList[i].Total);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].Currency))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `currency` = @currency" + i);
                                    else
                                        sql = string.Concat(sql, " `currency` = @currency" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@currency" + i, subscriptionReceiptList[i].Currency);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].CouponId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `coupon_id` = @couponId" + i);
                                    else
                                        sql = string.Concat(sql, " `coupon_id` = @couponId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@couponId" + i, subscriptionReceiptList[i].CouponId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionReceiptList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionReceiptList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(subscriptionReceiptList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionReceiptList[i].UpdatedAt);
                                }
                                if (subscriptionReceiptWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(subscriptionReceiptWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, subscriptionReceiptWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptWhereList[i].SubscriptionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `subscription_id` = @subscriptionIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionIdWhere" + i, subscriptionReceiptWhereList[i].SubscriptionId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptWhereList[i].TrialDays))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `trial_days` = @trialDaysWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `trial_days` = @trialDaysWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@trialDaysWhere" + i, subscriptionReceiptWhereList[i].TrialDays);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptWhereList[i].TrialDateEnd))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `trial_date_end` = @trialDateEndWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `trial_date_end` = @trialDateEndWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@trialDateEndWhere" + i, subscriptionReceiptWhereList[i].TrialDateEnd);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptWhereList[i].SubTotal))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `sub_total` = @subTotalWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `sub_total` = @subTotalWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subTotalWhere" + i, subscriptionReceiptWhereList[i].SubTotal);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptWhereList[i].TaxTotal))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `tax_total` = @taxTotalWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `tax_total` = @taxTotalWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@taxTotalWhere" + i, subscriptionReceiptWhereList[i].TaxTotal);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptWhereList[i].Total))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `total` = @totalWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `total` = @totalWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalWhere" + i, subscriptionReceiptWhereList[i].Total);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptWhereList[i].Currency))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `currency` = @currencyWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `currency` = @currencyWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@currencyWhere" + i, subscriptionReceiptWhereList[i].Currency);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptWhereList[i].CouponId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `coupon_id` = @couponIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `coupon_id` = @couponIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@couponIdWhere" + i, subscriptionReceiptWhereList[i].CouponId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, subscriptionReceiptWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, subscriptionReceiptWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceiptWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, subscriptionReceiptWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(subscriptionReceiptList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<SubscriptionReceiptModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SubscriptionReceiptModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionReceiptModel>();
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

        public List<SubscriptionReceiptModel> Delete(string connectionString, SubscriptionReceiptModel subscriptionReceipt)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptModel> response = new List<SubscriptionReceiptModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM subst_subscription_receipts  ";
                if (subscriptionReceipt != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(subscriptionReceipt.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", subscriptionReceipt.Id);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceipt.SubscriptionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionId");
                        else
                            sql = string.Concat(sql, " `subscription_id` = @subscriptionId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionId", subscriptionReceipt.SubscriptionId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceipt.TrialDays))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `trial_days` = @trialDays");
                        else
                            sql = string.Concat(sql, " `trial_days` = @trialDays");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@trialDays", subscriptionReceipt.TrialDays);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceipt.TrialDateEnd))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `trial_date_end` = @trialDateEnd");
                        else
                            sql = string.Concat(sql, " `trial_date_end` = @trialDateEnd");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@trialDateEnd", subscriptionReceipt.TrialDateEnd);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceipt.SubTotal))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `sub_total` = @subTotal");
                        else
                            sql = string.Concat(sql, " `sub_total` = @subTotal");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subTotal", subscriptionReceipt.SubTotal);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceipt.TaxTotal))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `tax_total` = @taxTotal");
                        else
                            sql = string.Concat(sql, " `tax_total` = @taxTotal");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@taxTotal", subscriptionReceipt.TaxTotal);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceipt.Total))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `total` = @total");
                        else
                            sql = string.Concat(sql, " `total` = @total");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@total", subscriptionReceipt.Total);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceipt.Currency))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `currency` = @currency");
                        else
                            sql = string.Concat(sql, " `currency` = @currency");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@currency", subscriptionReceipt.Currency);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceipt.CouponId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `coupon_id` = @couponId");
                        else
                            sql = string.Concat(sql, " `coupon_id` = @couponId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@couponId", subscriptionReceipt.CouponId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceipt.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", subscriptionReceipt.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceipt.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", subscriptionReceipt.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(subscriptionReceipt.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", subscriptionReceipt.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(subscriptionReceipt);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionReceiptModel>();
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

        public List<SubscriptionReceiptModel> Delete(string connectionString, List<SubscriptionReceiptModel> subscriptionReceipt)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionReceiptModel> response = new List<SubscriptionReceiptModel>();
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
                        for (int i = 0; i < subscriptionReceipt.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_subscription_receipts  ";
                                if (subscriptionReceipt[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(subscriptionReceipt[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, subscriptionReceipt[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceipt[i].SubscriptionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionId" + i);
                                        else
                                            sql = string.Concat(sql, " `subscription_id` = @subscriptionId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionId" + i, subscriptionReceipt[i].SubscriptionId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceipt[i].TrialDays))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `trial_days` = @trialDays" + i);
                                        else
                                            sql = string.Concat(sql, " `trial_days` = @trialDays" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@trialDays" + i, subscriptionReceipt[i].TrialDays);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceipt[i].TrialDateEnd))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `trial_date_end` = @trialDateEnd" + i);
                                        else
                                            sql = string.Concat(sql, " `trial_date_end` = @trialDateEnd" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@trialDateEnd" + i, subscriptionReceipt[i].TrialDateEnd);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceipt[i].SubTotal))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `sub_total` = @subTotal" + i);
                                        else
                                            sql = string.Concat(sql, " `sub_total` = @subTotal" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subTotal" + i, subscriptionReceipt[i].SubTotal);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceipt[i].TaxTotal))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `tax_total` = @taxTotal" + i);
                                        else
                                            sql = string.Concat(sql, " `tax_total` = @taxTotal" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@taxTotal" + i, subscriptionReceipt[i].TaxTotal);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceipt[i].Total))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `total` = @total" + i);
                                        else
                                            sql = string.Concat(sql, " `total` = @total" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@total" + i, subscriptionReceipt[i].Total);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceipt[i].Currency))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `currency` = @currency" + i);
                                        else
                                            sql = string.Concat(sql, " `currency` = @currency" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@currency" + i, subscriptionReceipt[i].Currency);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceipt[i].CouponId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `coupon_id` = @couponId" + i);
                                        else
                                            sql = string.Concat(sql, " `coupon_id` = @couponId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@couponId" + i, subscriptionReceipt[i].CouponId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceipt[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionReceipt[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceipt[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionReceipt[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionReceipt[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionReceipt[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(subscriptionReceipt[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<SubscriptionReceiptModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SubscriptionReceiptModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionReceiptModel>();
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
