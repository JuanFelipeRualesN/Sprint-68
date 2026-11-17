using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class CouponDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, CouponModel> ReadD(string connectionString, CouponModel coupon, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, CouponModel> response = new Dictionary<string, CouponModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `code`, `platform_id`, `currency`, `percent_off`, `amount_off`, `duration`, `duration_in_months`, `max_redemptions`, `max_redemption_by_single_user`, `redeem_by`, `available`, `hidden`, `interval_intended`, `modified_by`, `created_at`, `updated_at`  FROM subst_coupons ";
                {
                    if (coupon != null)
                    {
                        if (!string.IsNullOrEmpty(coupon.Id))
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
                            cmd.Parameters.AddWithValue("@id", coupon.Id);
                        }
                        if (!string.IsNullOrEmpty(coupon.Name))
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
                            cmd.Parameters.AddWithValue("@name", coupon.Name);
                        }
                        if (!string.IsNullOrEmpty(coupon.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", coupon.Code);
                        }
                        if (!string.IsNullOrEmpty(coupon.PlatformId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `platform_id` = @platformId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `platform_id` = @platformId ");
                            }
                            cmd.Parameters.AddWithValue("@platformId", coupon.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(coupon.Currency))
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
                            cmd.Parameters.AddWithValue("@currency", coupon.Currency);
                        }
                        if (!string.IsNullOrEmpty(coupon.PercentOff))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `percent_off` = @percentOff ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `percent_off` = @percentOff ");
                            }
                            cmd.Parameters.AddWithValue("@percentOff", coupon.PercentOff);
                        }
                        if (!string.IsNullOrEmpty(coupon.AmountOff))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `amount_off` = @amountOff ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `amount_off` = @amountOff ");
                            }
                            cmd.Parameters.AddWithValue("@amountOff", coupon.AmountOff);
                        }
                        if (!string.IsNullOrEmpty(coupon.Duration))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `duration` = @duration ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `duration` = @duration ");
                            }
                            cmd.Parameters.AddWithValue("@duration", coupon.Duration);
                        }
                        if (!string.IsNullOrEmpty(coupon.DurationInMonths))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `duration_in_months` = @durationInMonths ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `duration_in_months` = @durationInMonths ");
                            }
                            cmd.Parameters.AddWithValue("@durationInMonths", coupon.DurationInMonths);
                        }
                        if (!string.IsNullOrEmpty(coupon.MaxRedemptions))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_redemptions` = @maxRedemptions ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_redemptions` = @maxRedemptions ");
                            }
                            cmd.Parameters.AddWithValue("@maxRedemptions", coupon.MaxRedemptions);
                        }
                        if (!string.IsNullOrEmpty(coupon.MaxRedemptionBySingleUser))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_redemption_by_single_user` = @maxRedemptionBySingleUser ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_redemption_by_single_user` = @maxRedemptionBySingleUser ");
                            }
                            cmd.Parameters.AddWithValue("@maxRedemptionBySingleUser", coupon.MaxRedemptionBySingleUser);
                        }
                        if (!string.IsNullOrEmpty(coupon.RedeemBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `redeem_by` = @redeemBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `redeem_by` = @redeemBy ");
                            }
                            cmd.Parameters.AddWithValue("@redeemBy", coupon.RedeemBy);
                        }
                        if (!string.IsNullOrEmpty(coupon.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", coupon.Available);
                        }
                        if (!string.IsNullOrEmpty(coupon.Hidden))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `hidden` = @hidden ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `hidden` = @hidden ");
                            }
                            cmd.Parameters.AddWithValue("@hidden", coupon.Hidden);
                        }
                        if (!string.IsNullOrEmpty(coupon.IntervalIntended))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `interval_intended` = @intervalIntended ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `interval_intended` = @intervalIntended ");
                            }
                            cmd.Parameters.AddWithValue("@intervalIntended", coupon.IntervalIntended);
                        }
                        if (!string.IsNullOrEmpty(coupon.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", coupon.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(coupon.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", coupon.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(coupon.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", coupon.UpdatedAt);
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
                                    coupon.Id = reader.GetValue(0).ToString();
                                else
                                    coupon.Id = null;
                                if (!reader.IsDBNull(1))
                                    coupon.Name = reader.GetValue(1).ToString();
                                else
                                    coupon.Name = null;
                                if (!reader.IsDBNull(2))
                                    coupon.Code = reader.GetValue(2).ToString();
                                else
                                    coupon.Code = null;
                                if (!reader.IsDBNull(3))
                                    coupon.PlatformId = reader.GetValue(3).ToString();
                                else
                                    coupon.PlatformId = null;
                                if (!reader.IsDBNull(4))
                                    coupon.Currency = reader.GetValue(4).ToString();
                                else
                                    coupon.Currency = null;
                                if (!reader.IsDBNull(5))
                                    coupon.PercentOff = reader.GetValue(5).ToString();
                                else
                                    coupon.PercentOff = null;
                                if (!reader.IsDBNull(6))
                                    coupon.AmountOff = reader.GetValue(6).ToString();
                                else
                                    coupon.AmountOff = null;
                                if (!reader.IsDBNull(7))
                                    coupon.Duration = reader.GetValue(7).ToString();
                                else
                                    coupon.Duration = null;
                                if (!reader.IsDBNull(8))
                                    coupon.DurationInMonths = reader.GetValue(8).ToString();
                                else
                                    coupon.DurationInMonths = null;
                                if (!reader.IsDBNull(9))
                                    coupon.MaxRedemptions = reader.GetValue(9).ToString();
                                else
                                    coupon.MaxRedemptions = null;
                                if (!reader.IsDBNull(10))
                                    coupon.MaxRedemptionBySingleUser = reader.GetValue(10).ToString();
                                else
                                    coupon.MaxRedemptionBySingleUser = null;
                                if (!reader.IsDBNull(11))
                                    coupon.RedeemBy = reader.GetValue(11).ToString();
                                else
                                    coupon.RedeemBy = null;
                                if (!reader.IsDBNull(12))
                                    coupon.Available = reader.GetValue(12).ToString();
                                else
                                    coupon.Available = null;
                                if (!reader.IsDBNull(13))
                                    coupon.Hidden = reader.GetValue(13).ToString();
                                else
                                    coupon.Hidden = null;
                                if (!reader.IsDBNull(14))
                                    coupon.IntervalIntended = reader.GetValue(14).ToString();
                                else
                                    coupon.IntervalIntended = null;
                                if (!reader.IsDBNull(15))
                                    coupon.ModifiedBy = reader.GetValue(15).ToString();
                                else
                                    coupon.ModifiedBy = null;
                                if (!reader.IsDBNull(16))
                                    coupon.CreatedAt = reader.GetValue(16).ToString();
                                else
                                    coupon.CreatedAt = null;
                                if (!reader.IsDBNull(17))
                                    coupon.UpdatedAt = reader.GetValue(17).ToString();
                                else
                                    coupon.UpdatedAt = null;
                                response.Add(coupon.Id, coupon);
                                coupon = new CouponModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, CouponModel>();
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
                response = new Dictionary<string, CouponModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, CouponModel> ReadD(string connectionString, CouponModel coupon, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, CouponModel> response = new Dictionary<string, CouponModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `code`, `platform_id`, `currency`, `percent_off`, `amount_off`, `duration`, `duration_in_months`, `max_redemptions`, `max_redemption_by_single_user`, `redeem_by`, `available`, `hidden`, `interval_intended`, `modified_by`, `created_at`, `updated_at`  FROM subst_coupons ";
                {
                    if (coupon != null)
                    {
                        if (!string.IsNullOrEmpty(coupon.Id))
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
                            cmd.Parameters.AddWithValue("@id", coupon.Id);
                        }
                        if (!string.IsNullOrEmpty(coupon.Name))
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
                            cmd.Parameters.AddWithValue("@name", coupon.Name);
                        }
                        if (!string.IsNullOrEmpty(coupon.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", coupon.Code);
                        }
                        if (!string.IsNullOrEmpty(coupon.PlatformId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `platform_id` = @platformId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `platform_id` = @platformId ");
                            }
                            cmd.Parameters.AddWithValue("@platformId", coupon.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(coupon.Currency))
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
                            cmd.Parameters.AddWithValue("@currency", coupon.Currency);
                        }
                        if (!string.IsNullOrEmpty(coupon.PercentOff))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `percent_off` = @percentOff ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `percent_off` = @percentOff ");
                            }
                            cmd.Parameters.AddWithValue("@percentOff", coupon.PercentOff);
                        }
                        if (!string.IsNullOrEmpty(coupon.AmountOff))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `amount_off` = @amountOff ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `amount_off` = @amountOff ");
                            }
                            cmd.Parameters.AddWithValue("@amountOff", coupon.AmountOff);
                        }
                        if (!string.IsNullOrEmpty(coupon.Duration))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `duration` = @duration ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `duration` = @duration ");
                            }
                            cmd.Parameters.AddWithValue("@duration", coupon.Duration);
                        }
                        if (!string.IsNullOrEmpty(coupon.DurationInMonths))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `duration_in_months` = @durationInMonths ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `duration_in_months` = @durationInMonths ");
                            }
                            cmd.Parameters.AddWithValue("@durationInMonths", coupon.DurationInMonths);
                        }
                        if (!string.IsNullOrEmpty(coupon.MaxRedemptions))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_redemptions` = @maxRedemptions ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_redemptions` = @maxRedemptions ");
                            }
                            cmd.Parameters.AddWithValue("@maxRedemptions", coupon.MaxRedemptions);
                        }
                        if (!string.IsNullOrEmpty(coupon.MaxRedemptionBySingleUser))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_redemption_by_single_user` = @maxRedemptionBySingleUser ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_redemption_by_single_user` = @maxRedemptionBySingleUser ");
                            }
                            cmd.Parameters.AddWithValue("@maxRedemptionBySingleUser", coupon.MaxRedemptionBySingleUser);
                        }
                        if (!string.IsNullOrEmpty(coupon.RedeemBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `redeem_by` = @redeemBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `redeem_by` = @redeemBy ");
                            }
                            cmd.Parameters.AddWithValue("@redeemBy", coupon.RedeemBy);
                        }
                        if (!string.IsNullOrEmpty(coupon.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", coupon.Available);
                        }
                        if (!string.IsNullOrEmpty(coupon.Hidden))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `hidden` = @hidden ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `hidden` = @hidden ");
                            }
                            cmd.Parameters.AddWithValue("@hidden", coupon.Hidden);
                        }
                        if (!string.IsNullOrEmpty(coupon.IntervalIntended))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `interval_intended` = @intervalIntended ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `interval_intended` = @intervalIntended ");
                            }
                            cmd.Parameters.AddWithValue("@intervalIntended", coupon.IntervalIntended);
                        }
                        if (!string.IsNullOrEmpty(coupon.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", coupon.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(coupon.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", coupon.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(coupon.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", coupon.UpdatedAt);
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
                                    coupon.Id = reader.GetValue(0).ToString();
                                else
                                    coupon.Id = null;
                                if (!reader.IsDBNull(1))
                                    coupon.Name = reader.GetValue(1).ToString();
                                else
                                    coupon.Name = null;
                                if (!reader.IsDBNull(2))
                                    coupon.Code = reader.GetValue(2).ToString();
                                else
                                    coupon.Code = null;
                                if (!reader.IsDBNull(3))
                                    coupon.PlatformId = reader.GetValue(3).ToString();
                                else
                                    coupon.PlatformId = null;
                                if (!reader.IsDBNull(4))
                                    coupon.Currency = reader.GetValue(4).ToString();
                                else
                                    coupon.Currency = null;
                                if (!reader.IsDBNull(5))
                                    coupon.PercentOff = reader.GetValue(5).ToString();
                                else
                                    coupon.PercentOff = null;
                                if (!reader.IsDBNull(6))
                                    coupon.AmountOff = reader.GetValue(6).ToString();
                                else
                                    coupon.AmountOff = null;
                                if (!reader.IsDBNull(7))
                                    coupon.Duration = reader.GetValue(7).ToString();
                                else
                                    coupon.Duration = null;
                                if (!reader.IsDBNull(8))
                                    coupon.DurationInMonths = reader.GetValue(8).ToString();
                                else
                                    coupon.DurationInMonths = null;
                                if (!reader.IsDBNull(9))
                                    coupon.MaxRedemptions = reader.GetValue(9).ToString();
                                else
                                    coupon.MaxRedemptions = null;
                                if (!reader.IsDBNull(10))
                                    coupon.MaxRedemptionBySingleUser = reader.GetValue(10).ToString();
                                else
                                    coupon.MaxRedemptionBySingleUser = null;
                                if (!reader.IsDBNull(11))
                                    coupon.RedeemBy = reader.GetValue(11).ToString();
                                else
                                    coupon.RedeemBy = null;
                                if (!reader.IsDBNull(12))
                                    coupon.Available = reader.GetValue(12).ToString();
                                else
                                    coupon.Available = null;
                                if (!reader.IsDBNull(13))
                                    coupon.Hidden = reader.GetValue(13).ToString();
                                else
                                    coupon.Hidden = null;
                                if (!reader.IsDBNull(14))
                                    coupon.IntervalIntended = reader.GetValue(14).ToString();
                                else
                                    coupon.IntervalIntended = null;
                                if (!reader.IsDBNull(15))
                                    coupon.ModifiedBy = reader.GetValue(15).ToString();
                                else
                                    coupon.ModifiedBy = null;
                                if (!reader.IsDBNull(16))
                                    coupon.CreatedAt = reader.GetValue(16).ToString();
                                else
                                    coupon.CreatedAt = null;
                                if (!reader.IsDBNull(17))
                                    coupon.UpdatedAt = reader.GetValue(17).ToString();
                                else
                                    coupon.UpdatedAt = null;
                                response.Add(coupon.Id, coupon);
                                coupon = new CouponModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, CouponModel>();
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
                response = new Dictionary<string, CouponModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, CouponModel> ReadD(string connectionString, List<CouponModel> couponWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, CouponModel> response = new Dictionary<string, CouponModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `code`, `platform_id`, `currency`, `percent_off`, `amount_off`, `duration`, `duration_in_months`, `max_redemptions`, `max_redemption_by_single_user`, `redeem_by`, `available`, `hidden`, `interval_intended`, `modified_by`, `created_at`, `updated_at`  FROM subst_coupons ";
                if (couponWhere != null && couponWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < couponWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(couponWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, couponWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, couponWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, couponWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, couponWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Currency))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `currency` = @currency" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currency" + i, couponWhere[i].Currency);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].PercentOff))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `percent_off` = @percentOff" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@percentOff" + i, couponWhere[i].PercentOff);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].AmountOff))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `amount_off` = @amountOff" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@amountOff" + i, couponWhere[i].AmountOff);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Duration))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `duration` = @duration" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@duration" + i, couponWhere[i].Duration);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].DurationInMonths))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `duration_in_months` = @durationInMonths" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@durationInMonths" + i, couponWhere[i].DurationInMonths);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].MaxRedemptions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_redemptions` = @maxRedemptions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxRedemptions" + i, couponWhere[i].MaxRedemptions);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].MaxRedemptionBySingleUser))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_redemption_by_single_user` = @maxRedemptionBySingleUser" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxRedemptionBySingleUser" + i, couponWhere[i].MaxRedemptionBySingleUser);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].RedeemBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `redeem_by` = @redeemBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@redeemBy" + i, couponWhere[i].RedeemBy);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, couponWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Hidden))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `hidden` = @hidden" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@hidden" + i, couponWhere[i].Hidden);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].IntervalIntended))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `interval_intended` = @intervalIntended" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@intervalIntended" + i, couponWhere[i].IntervalIntended);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, couponWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, couponWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, couponWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != couponWhere.Count - 1)
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
                            CouponModel coupon = new CouponModel(); if (!reader.IsDBNull(0))
                                coupon.Id = reader.GetValue(0).ToString();
                            else
                                coupon.Id = null;
                            if (!reader.IsDBNull(1))
                                coupon.Name = reader.GetValue(1).ToString();
                            else
                                coupon.Name = null;
                            if (!reader.IsDBNull(2))
                                coupon.Code = reader.GetValue(2).ToString();
                            else
                                coupon.Code = null;
                            if (!reader.IsDBNull(3))
                                coupon.PlatformId = reader.GetValue(3).ToString();
                            else
                                coupon.PlatformId = null;
                            if (!reader.IsDBNull(4))
                                coupon.Currency = reader.GetValue(4).ToString();
                            else
                                coupon.Currency = null;
                            if (!reader.IsDBNull(5))
                                coupon.PercentOff = reader.GetValue(5).ToString();
                            else
                                coupon.PercentOff = null;
                            if (!reader.IsDBNull(6))
                                coupon.AmountOff = reader.GetValue(6).ToString();
                            else
                                coupon.AmountOff = null;
                            if (!reader.IsDBNull(7))
                                coupon.Duration = reader.GetValue(7).ToString();
                            else
                                coupon.Duration = null;
                            if (!reader.IsDBNull(8))
                                coupon.DurationInMonths = reader.GetValue(8).ToString();
                            else
                                coupon.DurationInMonths = null;
                            if (!reader.IsDBNull(9))
                                coupon.MaxRedemptions = reader.GetValue(9).ToString();
                            else
                                coupon.MaxRedemptions = null;
                            if (!reader.IsDBNull(10))
                                coupon.MaxRedemptionBySingleUser = reader.GetValue(10).ToString();
                            else
                                coupon.MaxRedemptionBySingleUser = null;
                            if (!reader.IsDBNull(11))
                                coupon.RedeemBy = reader.GetValue(11).ToString();
                            else
                                coupon.RedeemBy = null;
                            if (!reader.IsDBNull(12))
                                coupon.Available = reader.GetValue(12).ToString();
                            else
                                coupon.Available = null;
                            if (!reader.IsDBNull(13))
                                coupon.Hidden = reader.GetValue(13).ToString();
                            else
                                coupon.Hidden = null;
                            if (!reader.IsDBNull(14))
                                coupon.IntervalIntended = reader.GetValue(14).ToString();
                            else
                                coupon.IntervalIntended = null;
                            if (!reader.IsDBNull(15))
                                coupon.ModifiedBy = reader.GetValue(15).ToString();
                            else
                                coupon.ModifiedBy = null;
                            if (!reader.IsDBNull(16))
                                coupon.CreatedAt = reader.GetValue(16).ToString();
                            else
                                coupon.CreatedAt = null;
                            if (!reader.IsDBNull(17))
                                coupon.UpdatedAt = reader.GetValue(17).ToString();
                            else
                                coupon.UpdatedAt = null;
                            response.Add(coupon.Id, coupon);
                            coupon = new CouponModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, CouponModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, CouponModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, CouponModel> ReadD(string connectionString, List<CouponModel> couponWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, CouponModel> response = new Dictionary<string, CouponModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `code`, `platform_id`, `currency`, `percent_off`, `amount_off`, `duration`, `duration_in_months`, `max_redemptions`, `max_redemption_by_single_user`, `redeem_by`, `available`, `hidden`, `interval_intended`, `modified_by`, `created_at`, `updated_at`  FROM subst_coupons ";
                if (couponWhere != null && couponWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < couponWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(couponWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, couponWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, couponWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, couponWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, couponWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Currency))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `currency` = @currency" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currency" + i, couponWhere[i].Currency);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].PercentOff))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `percent_off` = @percentOff" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@percentOff" + i, couponWhere[i].PercentOff);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].AmountOff))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `amount_off` = @amountOff" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@amountOff" + i, couponWhere[i].AmountOff);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Duration))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `duration` = @duration" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@duration" + i, couponWhere[i].Duration);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].DurationInMonths))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `duration_in_months` = @durationInMonths" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@durationInMonths" + i, couponWhere[i].DurationInMonths);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].MaxRedemptions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_redemptions` = @maxRedemptions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxRedemptions" + i, couponWhere[i].MaxRedemptions);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].MaxRedemptionBySingleUser))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_redemption_by_single_user` = @maxRedemptionBySingleUser" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxRedemptionBySingleUser" + i, couponWhere[i].MaxRedemptionBySingleUser);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].RedeemBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `redeem_by` = @redeemBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@redeemBy" + i, couponWhere[i].RedeemBy);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, couponWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Hidden))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `hidden` = @hidden" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@hidden" + i, couponWhere[i].Hidden);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].IntervalIntended))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `interval_intended` = @intervalIntended" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@intervalIntended" + i, couponWhere[i].IntervalIntended);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, couponWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, couponWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, couponWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != couponWhere.Count - 1)
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
                            CouponModel coupon = new CouponModel(); if (!reader.IsDBNull(0))
                                coupon.Id = reader.GetValue(0).ToString();
                            else
                                coupon.Id = null;
                            if (!reader.IsDBNull(1))
                                coupon.Name = reader.GetValue(1).ToString();
                            else
                                coupon.Name = null;
                            if (!reader.IsDBNull(2))
                                coupon.Code = reader.GetValue(2).ToString();
                            else
                                coupon.Code = null;
                            if (!reader.IsDBNull(3))
                                coupon.PlatformId = reader.GetValue(3).ToString();
                            else
                                coupon.PlatformId = null;
                            if (!reader.IsDBNull(4))
                                coupon.Currency = reader.GetValue(4).ToString();
                            else
                                coupon.Currency = null;
                            if (!reader.IsDBNull(5))
                                coupon.PercentOff = reader.GetValue(5).ToString();
                            else
                                coupon.PercentOff = null;
                            if (!reader.IsDBNull(6))
                                coupon.AmountOff = reader.GetValue(6).ToString();
                            else
                                coupon.AmountOff = null;
                            if (!reader.IsDBNull(7))
                                coupon.Duration = reader.GetValue(7).ToString();
                            else
                                coupon.Duration = null;
                            if (!reader.IsDBNull(8))
                                coupon.DurationInMonths = reader.GetValue(8).ToString();
                            else
                                coupon.DurationInMonths = null;
                            if (!reader.IsDBNull(9))
                                coupon.MaxRedemptions = reader.GetValue(9).ToString();
                            else
                                coupon.MaxRedemptions = null;
                            if (!reader.IsDBNull(10))
                                coupon.MaxRedemptionBySingleUser = reader.GetValue(10).ToString();
                            else
                                coupon.MaxRedemptionBySingleUser = null;
                            if (!reader.IsDBNull(11))
                                coupon.RedeemBy = reader.GetValue(11).ToString();
                            else
                                coupon.RedeemBy = null;
                            if (!reader.IsDBNull(12))
                                coupon.Available = reader.GetValue(12).ToString();
                            else
                                coupon.Available = null;
                            if (!reader.IsDBNull(13))
                                coupon.Hidden = reader.GetValue(13).ToString();
                            else
                                coupon.Hidden = null;
                            if (!reader.IsDBNull(14))
                                coupon.IntervalIntended = reader.GetValue(14).ToString();
                            else
                                coupon.IntervalIntended = null;
                            if (!reader.IsDBNull(15))
                                coupon.ModifiedBy = reader.GetValue(15).ToString();
                            else
                                coupon.ModifiedBy = null;
                            if (!reader.IsDBNull(16))
                                coupon.CreatedAt = reader.GetValue(16).ToString();
                            else
                                coupon.CreatedAt = null;
                            if (!reader.IsDBNull(17))
                                coupon.UpdatedAt = reader.GetValue(17).ToString();
                            else
                                coupon.UpdatedAt = null;
                            response.Add(coupon.Id, coupon);
                            coupon = new CouponModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, CouponModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, CouponModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CouponModel> Read(string connectionString, CouponModel coupon, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CouponModel> response = new List<CouponModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `code`, `platform_id`, `currency`, `percent_off`, `amount_off`, `duration`, `duration_in_months`, `max_redemptions`, `max_redemption_by_single_user`, `redeem_by`, `available`, `hidden`, `interval_intended`, `modified_by`, `created_at`, `updated_at`  FROM subst_coupons ";
                {
                    if (coupon != null)
                    {
                        if (!string.IsNullOrEmpty(coupon.Id))
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
                            cmd.Parameters.AddWithValue("@id", coupon.Id);
                        }
                        if (!string.IsNullOrEmpty(coupon.Name))
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
                            cmd.Parameters.AddWithValue("@name", coupon.Name);
                        }
                        if (!string.IsNullOrEmpty(coupon.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", coupon.Code);
                        }
                        if (!string.IsNullOrEmpty(coupon.PlatformId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `platform_id` = @platformId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `platform_id` = @platformId ");
                            }
                            cmd.Parameters.AddWithValue("@platformId", coupon.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(coupon.Currency))
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
                            cmd.Parameters.AddWithValue("@currency", coupon.Currency);
                        }
                        if (!string.IsNullOrEmpty(coupon.PercentOff))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `percent_off` = @percentOff ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `percent_off` = @percentOff ");
                            }
                            cmd.Parameters.AddWithValue("@percentOff", coupon.PercentOff);
                        }
                        if (!string.IsNullOrEmpty(coupon.AmountOff))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `amount_off` = @amountOff ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `amount_off` = @amountOff ");
                            }
                            cmd.Parameters.AddWithValue("@amountOff", coupon.AmountOff);
                        }
                        if (!string.IsNullOrEmpty(coupon.Duration))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `duration` = @duration ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `duration` = @duration ");
                            }
                            cmd.Parameters.AddWithValue("@duration", coupon.Duration);
                        }
                        if (!string.IsNullOrEmpty(coupon.DurationInMonths))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `duration_in_months` = @durationInMonths ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `duration_in_months` = @durationInMonths ");
                            }
                            cmd.Parameters.AddWithValue("@durationInMonths", coupon.DurationInMonths);
                        }
                        if (!string.IsNullOrEmpty(coupon.MaxRedemptions))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_redemptions` = @maxRedemptions ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_redemptions` = @maxRedemptions ");
                            }
                            cmd.Parameters.AddWithValue("@maxRedemptions", coupon.MaxRedemptions);
                        }
                        if (!string.IsNullOrEmpty(coupon.MaxRedemptionBySingleUser))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_redemption_by_single_user` = @maxRedemptionBySingleUser ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_redemption_by_single_user` = @maxRedemptionBySingleUser ");
                            }
                            cmd.Parameters.AddWithValue("@maxRedemptionBySingleUser", coupon.MaxRedemptionBySingleUser);
                        }
                        if (!string.IsNullOrEmpty(coupon.RedeemBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `redeem_by` = @redeemBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `redeem_by` = @redeemBy ");
                            }
                            cmd.Parameters.AddWithValue("@redeemBy", coupon.RedeemBy);
                        }
                        if (!string.IsNullOrEmpty(coupon.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", coupon.Available);
                        }
                        if (!string.IsNullOrEmpty(coupon.Hidden))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `hidden` = @hidden ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `hidden` = @hidden ");
                            }
                            cmd.Parameters.AddWithValue("@hidden", coupon.Hidden);
                        }
                        if (!string.IsNullOrEmpty(coupon.IntervalIntended))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `interval_intended` = @intervalIntended ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `interval_intended` = @intervalIntended ");
                            }
                            cmd.Parameters.AddWithValue("@intervalIntended", coupon.IntervalIntended);
                        }
                        if (!string.IsNullOrEmpty(coupon.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", coupon.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(coupon.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", coupon.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(coupon.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", coupon.UpdatedAt);
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
                                    coupon.Id = reader.GetValue(0).ToString();
                                else
                                    coupon.Id = null;
                                if (!reader.IsDBNull(1))
                                    coupon.Name = reader.GetValue(1).ToString();
                                else
                                    coupon.Name = null;
                                if (!reader.IsDBNull(2))
                                    coupon.Code = reader.GetValue(2).ToString();
                                else
                                    coupon.Code = null;
                                if (!reader.IsDBNull(3))
                                    coupon.PlatformId = reader.GetValue(3).ToString();
                                else
                                    coupon.PlatformId = null;
                                if (!reader.IsDBNull(4))
                                    coupon.Currency = reader.GetValue(4).ToString();
                                else
                                    coupon.Currency = null;
                                if (!reader.IsDBNull(5))
                                    coupon.PercentOff = reader.GetValue(5).ToString();
                                else
                                    coupon.PercentOff = null;
                                if (!reader.IsDBNull(6))
                                    coupon.AmountOff = reader.GetValue(6).ToString();
                                else
                                    coupon.AmountOff = null;
                                if (!reader.IsDBNull(7))
                                    coupon.Duration = reader.GetValue(7).ToString();
                                else
                                    coupon.Duration = null;
                                if (!reader.IsDBNull(8))
                                    coupon.DurationInMonths = reader.GetValue(8).ToString();
                                else
                                    coupon.DurationInMonths = null;
                                if (!reader.IsDBNull(9))
                                    coupon.MaxRedemptions = reader.GetValue(9).ToString();
                                else
                                    coupon.MaxRedemptions = null;
                                if (!reader.IsDBNull(10))
                                    coupon.MaxRedemptionBySingleUser = reader.GetValue(10).ToString();
                                else
                                    coupon.MaxRedemptionBySingleUser = null;
                                if (!reader.IsDBNull(11))
                                    coupon.RedeemBy = reader.GetValue(11).ToString();
                                else
                                    coupon.RedeemBy = null;
                                if (!reader.IsDBNull(12))
                                    coupon.Available = reader.GetValue(12).ToString();
                                else
                                    coupon.Available = null;
                                if (!reader.IsDBNull(13))
                                    coupon.Hidden = reader.GetValue(13).ToString();
                                else
                                    coupon.Hidden = null;
                                if (!reader.IsDBNull(14))
                                    coupon.IntervalIntended = reader.GetValue(14).ToString();
                                else
                                    coupon.IntervalIntended = null;
                                if (!reader.IsDBNull(15))
                                    coupon.ModifiedBy = reader.GetValue(15).ToString();
                                else
                                    coupon.ModifiedBy = null;
                                if (!reader.IsDBNull(16))
                                    coupon.CreatedAt = reader.GetValue(16).ToString();
                                else
                                    coupon.CreatedAt = null;
                                if (!reader.IsDBNull(17))
                                    coupon.UpdatedAt = reader.GetValue(17).ToString();
                                else
                                    coupon.UpdatedAt = null;
                                response.Add(coupon);
                                coupon = new CouponModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<CouponModel>();
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
                response = new List<CouponModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CouponModel> Read(string connectionString, CouponModel coupon, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CouponModel> response = new List<CouponModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `code`, `platform_id`, `currency`, `percent_off`, `amount_off`, `duration`, `duration_in_months`, `max_redemptions`, `max_redemption_by_single_user`, `redeem_by`, `available`, `hidden`, `interval_intended`, `modified_by`, `created_at`, `updated_at`  FROM subst_coupons ";
                {
                    if (coupon != null)
                    {
                        if (!string.IsNullOrEmpty(coupon.Id))
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
                            cmd.Parameters.AddWithValue("@id", coupon.Id);
                        }
                        if (!string.IsNullOrEmpty(coupon.Name))
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
                            cmd.Parameters.AddWithValue("@name", coupon.Name);
                        }
                        if (!string.IsNullOrEmpty(coupon.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", coupon.Code);
                        }
                        if (!string.IsNullOrEmpty(coupon.PlatformId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `platform_id` = @platformId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `platform_id` = @platformId ");
                            }
                            cmd.Parameters.AddWithValue("@platformId", coupon.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(coupon.Currency))
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
                            cmd.Parameters.AddWithValue("@currency", coupon.Currency);
                        }
                        if (!string.IsNullOrEmpty(coupon.PercentOff))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `percent_off` = @percentOff ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `percent_off` = @percentOff ");
                            }
                            cmd.Parameters.AddWithValue("@percentOff", coupon.PercentOff);
                        }
                        if (!string.IsNullOrEmpty(coupon.AmountOff))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `amount_off` = @amountOff ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `amount_off` = @amountOff ");
                            }
                            cmd.Parameters.AddWithValue("@amountOff", coupon.AmountOff);
                        }
                        if (!string.IsNullOrEmpty(coupon.Duration))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `duration` = @duration ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `duration` = @duration ");
                            }
                            cmd.Parameters.AddWithValue("@duration", coupon.Duration);
                        }
                        if (!string.IsNullOrEmpty(coupon.DurationInMonths))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `duration_in_months` = @durationInMonths ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `duration_in_months` = @durationInMonths ");
                            }
                            cmd.Parameters.AddWithValue("@durationInMonths", coupon.DurationInMonths);
                        }
                        if (!string.IsNullOrEmpty(coupon.MaxRedemptions))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_redemptions` = @maxRedemptions ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_redemptions` = @maxRedemptions ");
                            }
                            cmd.Parameters.AddWithValue("@maxRedemptions", coupon.MaxRedemptions);
                        }
                        if (!string.IsNullOrEmpty(coupon.MaxRedemptionBySingleUser))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `max_redemption_by_single_user` = @maxRedemptionBySingleUser ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `max_redemption_by_single_user` = @maxRedemptionBySingleUser ");
                            }
                            cmd.Parameters.AddWithValue("@maxRedemptionBySingleUser", coupon.MaxRedemptionBySingleUser);
                        }
                        if (!string.IsNullOrEmpty(coupon.RedeemBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `redeem_by` = @redeemBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `redeem_by` = @redeemBy ");
                            }
                            cmd.Parameters.AddWithValue("@redeemBy", coupon.RedeemBy);
                        }
                        if (!string.IsNullOrEmpty(coupon.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", coupon.Available);
                        }
                        if (!string.IsNullOrEmpty(coupon.Hidden))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `hidden` = @hidden ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `hidden` = @hidden ");
                            }
                            cmd.Parameters.AddWithValue("@hidden", coupon.Hidden);
                        }
                        if (!string.IsNullOrEmpty(coupon.IntervalIntended))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `interval_intended` = @intervalIntended ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `interval_intended` = @intervalIntended ");
                            }
                            cmd.Parameters.AddWithValue("@intervalIntended", coupon.IntervalIntended);
                        }
                        if (!string.IsNullOrEmpty(coupon.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", coupon.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(coupon.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", coupon.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(coupon.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", coupon.UpdatedAt);
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
                                    coupon.Id = reader.GetValue(0).ToString();
                                else
                                    coupon.Id = null;
                                if (!reader.IsDBNull(1))
                                    coupon.Name = reader.GetValue(1).ToString();
                                else
                                    coupon.Name = null;
                                if (!reader.IsDBNull(2))
                                    coupon.Code = reader.GetValue(2).ToString();
                                else
                                    coupon.Code = null;
                                if (!reader.IsDBNull(3))
                                    coupon.PlatformId = reader.GetValue(3).ToString();
                                else
                                    coupon.PlatformId = null;
                                if (!reader.IsDBNull(4))
                                    coupon.Currency = reader.GetValue(4).ToString();
                                else
                                    coupon.Currency = null;
                                if (!reader.IsDBNull(5))
                                    coupon.PercentOff = reader.GetValue(5).ToString();
                                else
                                    coupon.PercentOff = null;
                                if (!reader.IsDBNull(6))
                                    coupon.AmountOff = reader.GetValue(6).ToString();
                                else
                                    coupon.AmountOff = null;
                                if (!reader.IsDBNull(7))
                                    coupon.Duration = reader.GetValue(7).ToString();
                                else
                                    coupon.Duration = null;
                                if (!reader.IsDBNull(8))
                                    coupon.DurationInMonths = reader.GetValue(8).ToString();
                                else
                                    coupon.DurationInMonths = null;
                                if (!reader.IsDBNull(9))
                                    coupon.MaxRedemptions = reader.GetValue(9).ToString();
                                else
                                    coupon.MaxRedemptions = null;
                                if (!reader.IsDBNull(10))
                                    coupon.MaxRedemptionBySingleUser = reader.GetValue(10).ToString();
                                else
                                    coupon.MaxRedemptionBySingleUser = null;
                                if (!reader.IsDBNull(11))
                                    coupon.RedeemBy = reader.GetValue(11).ToString();
                                else
                                    coupon.RedeemBy = null;
                                if (!reader.IsDBNull(12))
                                    coupon.Available = reader.GetValue(12).ToString();
                                else
                                    coupon.Available = null;
                                if (!reader.IsDBNull(13))
                                    coupon.Hidden = reader.GetValue(13).ToString();
                                else
                                    coupon.Hidden = null;
                                if (!reader.IsDBNull(14))
                                    coupon.IntervalIntended = reader.GetValue(14).ToString();
                                else
                                    coupon.IntervalIntended = null;
                                if (!reader.IsDBNull(15))
                                    coupon.ModifiedBy = reader.GetValue(15).ToString();
                                else
                                    coupon.ModifiedBy = null;
                                if (!reader.IsDBNull(16))
                                    coupon.CreatedAt = reader.GetValue(16).ToString();
                                else
                                    coupon.CreatedAt = null;
                                if (!reader.IsDBNull(17))
                                    coupon.UpdatedAt = reader.GetValue(17).ToString();
                                else
                                    coupon.UpdatedAt = null;
                                response.Add(coupon);
                                coupon = new CouponModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<CouponModel>();
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
                response = new List<CouponModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CouponModel> Read(string connectionString, List<CouponModel> couponWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CouponModel> response = new List<CouponModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `code`, `platform_id`, `currency`, `percent_off`, `amount_off`, `duration`, `duration_in_months`, `max_redemptions`, `max_redemption_by_single_user`, `redeem_by`, `available`, `hidden`, `interval_intended`, `modified_by`, `created_at`, `updated_at`  FROM subst_coupons ";
                if (couponWhere != null && couponWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < couponWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(couponWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, couponWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, couponWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, couponWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, couponWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Currency))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `currency` = @currency" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currency" + i, couponWhere[i].Currency);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].PercentOff))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `percent_off` = @percentOff" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@percentOff" + i, couponWhere[i].PercentOff);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].AmountOff))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `amount_off` = @amountOff" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@amountOff" + i, couponWhere[i].AmountOff);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Duration))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `duration` = @duration" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@duration" + i, couponWhere[i].Duration);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].DurationInMonths))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `duration_in_months` = @durationInMonths" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@durationInMonths" + i, couponWhere[i].DurationInMonths);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].MaxRedemptions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_redemptions` = @maxRedemptions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxRedemptions" + i, couponWhere[i].MaxRedemptions);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].MaxRedemptionBySingleUser))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_redemption_by_single_user` = @maxRedemptionBySingleUser" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxRedemptionBySingleUser" + i, couponWhere[i].MaxRedemptionBySingleUser);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].RedeemBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `redeem_by` = @redeemBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@redeemBy" + i, couponWhere[i].RedeemBy);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, couponWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Hidden))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `hidden` = @hidden" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@hidden" + i, couponWhere[i].Hidden);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].IntervalIntended))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `interval_intended` = @intervalIntended" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@intervalIntended" + i, couponWhere[i].IntervalIntended);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, couponWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, couponWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, couponWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != couponWhere.Count - 1)
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
                            CouponModel coupon = new CouponModel(); if (!reader.IsDBNull(0))
                                coupon.Id = reader.GetValue(0).ToString();
                            else
                                coupon.Id = null;
                            if (!reader.IsDBNull(1))
                                coupon.Name = reader.GetValue(1).ToString();
                            else
                                coupon.Name = null;
                            if (!reader.IsDBNull(2))
                                coupon.Code = reader.GetValue(2).ToString();
                            else
                                coupon.Code = null;
                            if (!reader.IsDBNull(3))
                                coupon.PlatformId = reader.GetValue(3).ToString();
                            else
                                coupon.PlatformId = null;
                            if (!reader.IsDBNull(4))
                                coupon.Currency = reader.GetValue(4).ToString();
                            else
                                coupon.Currency = null;
                            if (!reader.IsDBNull(5))
                                coupon.PercentOff = reader.GetValue(5).ToString();
                            else
                                coupon.PercentOff = null;
                            if (!reader.IsDBNull(6))
                                coupon.AmountOff = reader.GetValue(6).ToString();
                            else
                                coupon.AmountOff = null;
                            if (!reader.IsDBNull(7))
                                coupon.Duration = reader.GetValue(7).ToString();
                            else
                                coupon.Duration = null;
                            if (!reader.IsDBNull(8))
                                coupon.DurationInMonths = reader.GetValue(8).ToString();
                            else
                                coupon.DurationInMonths = null;
                            if (!reader.IsDBNull(9))
                                coupon.MaxRedemptions = reader.GetValue(9).ToString();
                            else
                                coupon.MaxRedemptions = null;
                            if (!reader.IsDBNull(10))
                                coupon.MaxRedemptionBySingleUser = reader.GetValue(10).ToString();
                            else
                                coupon.MaxRedemptionBySingleUser = null;
                            if (!reader.IsDBNull(11))
                                coupon.RedeemBy = reader.GetValue(11).ToString();
                            else
                                coupon.RedeemBy = null;
                            if (!reader.IsDBNull(12))
                                coupon.Available = reader.GetValue(12).ToString();
                            else
                                coupon.Available = null;
                            if (!reader.IsDBNull(13))
                                coupon.Hidden = reader.GetValue(13).ToString();
                            else
                                coupon.Hidden = null;
                            if (!reader.IsDBNull(14))
                                coupon.IntervalIntended = reader.GetValue(14).ToString();
                            else
                                coupon.IntervalIntended = null;
                            if (!reader.IsDBNull(15))
                                coupon.ModifiedBy = reader.GetValue(15).ToString();
                            else
                                coupon.ModifiedBy = null;
                            if (!reader.IsDBNull(16))
                                coupon.CreatedAt = reader.GetValue(16).ToString();
                            else
                                coupon.CreatedAt = null;
                            if (!reader.IsDBNull(17))
                                coupon.UpdatedAt = reader.GetValue(17).ToString();
                            else
                                coupon.UpdatedAt = null;
                            response.Add(coupon);
                            coupon = new CouponModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<CouponModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CouponModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CouponModel> Read(string connectionString, List<CouponModel> couponWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CouponModel> response = new List<CouponModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `code`, `platform_id`, `currency`, `percent_off`, `amount_off`, `duration`, `duration_in_months`, `max_redemptions`, `max_redemption_by_single_user`, `redeem_by`, `available`, `hidden`, `interval_intended`, `modified_by`, `created_at`, `updated_at`  FROM subst_coupons ";
                if (couponWhere != null && couponWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < couponWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(couponWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, couponWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, couponWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, couponWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, couponWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Currency))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `currency` = @currency" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currency" + i, couponWhere[i].Currency);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].PercentOff))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `percent_off` = @percentOff" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@percentOff" + i, couponWhere[i].PercentOff);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].AmountOff))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `amount_off` = @amountOff" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@amountOff" + i, couponWhere[i].AmountOff);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Duration))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `duration` = @duration" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@duration" + i, couponWhere[i].Duration);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].DurationInMonths))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `duration_in_months` = @durationInMonths" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@durationInMonths" + i, couponWhere[i].DurationInMonths);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].MaxRedemptions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_redemptions` = @maxRedemptions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxRedemptions" + i, couponWhere[i].MaxRedemptions);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].MaxRedemptionBySingleUser))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_redemption_by_single_user` = @maxRedemptionBySingleUser" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxRedemptionBySingleUser" + i, couponWhere[i].MaxRedemptionBySingleUser);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].RedeemBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `redeem_by` = @redeemBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@redeemBy" + i, couponWhere[i].RedeemBy);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, couponWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].Hidden))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `hidden` = @hidden" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@hidden" + i, couponWhere[i].Hidden);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].IntervalIntended))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `interval_intended` = @intervalIntended" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@intervalIntended" + i, couponWhere[i].IntervalIntended);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, couponWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, couponWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(couponWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, couponWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != couponWhere.Count - 1)
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
                            CouponModel coupon = new CouponModel(); if (!reader.IsDBNull(0))
                                coupon.Id = reader.GetValue(0).ToString();
                            else
                                coupon.Id = null;
                            if (!reader.IsDBNull(1))
                                coupon.Name = reader.GetValue(1).ToString();
                            else
                                coupon.Name = null;
                            if (!reader.IsDBNull(2))
                                coupon.Code = reader.GetValue(2).ToString();
                            else
                                coupon.Code = null;
                            if (!reader.IsDBNull(3))
                                coupon.PlatformId = reader.GetValue(3).ToString();
                            else
                                coupon.PlatformId = null;
                            if (!reader.IsDBNull(4))
                                coupon.Currency = reader.GetValue(4).ToString();
                            else
                                coupon.Currency = null;
                            if (!reader.IsDBNull(5))
                                coupon.PercentOff = reader.GetValue(5).ToString();
                            else
                                coupon.PercentOff = null;
                            if (!reader.IsDBNull(6))
                                coupon.AmountOff = reader.GetValue(6).ToString();
                            else
                                coupon.AmountOff = null;
                            if (!reader.IsDBNull(7))
                                coupon.Duration = reader.GetValue(7).ToString();
                            else
                                coupon.Duration = null;
                            if (!reader.IsDBNull(8))
                                coupon.DurationInMonths = reader.GetValue(8).ToString();
                            else
                                coupon.DurationInMonths = null;
                            if (!reader.IsDBNull(9))
                                coupon.MaxRedemptions = reader.GetValue(9).ToString();
                            else
                                coupon.MaxRedemptions = null;
                            if (!reader.IsDBNull(10))
                                coupon.MaxRedemptionBySingleUser = reader.GetValue(10).ToString();
                            else
                                coupon.MaxRedemptionBySingleUser = null;
                            if (!reader.IsDBNull(11))
                                coupon.RedeemBy = reader.GetValue(11).ToString();
                            else
                                coupon.RedeemBy = null;
                            if (!reader.IsDBNull(12))
                                coupon.Available = reader.GetValue(12).ToString();
                            else
                                coupon.Available = null;
                            if (!reader.IsDBNull(13))
                                coupon.Hidden = reader.GetValue(13).ToString();
                            else
                                coupon.Hidden = null;
                            if (!reader.IsDBNull(14))
                                coupon.IntervalIntended = reader.GetValue(14).ToString();
                            else
                                coupon.IntervalIntended = null;
                            if (!reader.IsDBNull(15))
                                coupon.ModifiedBy = reader.GetValue(15).ToString();
                            else
                                coupon.ModifiedBy = null;
                            if (!reader.IsDBNull(16))
                                coupon.CreatedAt = reader.GetValue(16).ToString();
                            else
                                coupon.CreatedAt = null;
                            if (!reader.IsDBNull(17))
                                coupon.UpdatedAt = reader.GetValue(17).ToString();
                            else
                                coupon.UpdatedAt = null;
                            response.Add(coupon);
                            coupon = new CouponModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<CouponModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CouponModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CouponModel> Create(string connectionString, CouponModel coupon)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CouponModel> response = new List<CouponModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO subst_coupons (";
                string values = "(";
                if (!string.IsNullOrEmpty(coupon.Id))
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
                    cmd.Parameters.AddWithValue("@id", coupon.Id);
                }
                if (!string.IsNullOrEmpty(coupon.Name))
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
                    cmd.Parameters.AddWithValue("@name", coupon.Name);
                }
                if (!string.IsNullOrEmpty(coupon.Code))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `code` ");
                        values = string.Concat(values, ", @code");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `code` ");
                        values = string.Concat(values, " @code");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", coupon.Code);
                }
                if (!string.IsNullOrEmpty(coupon.PlatformId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `platform_id` ");
                        values = string.Concat(values, ", @platformId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `platform_id` ");
                        values = string.Concat(values, " @platformId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@platformId", coupon.PlatformId);
                }
                if (!string.IsNullOrEmpty(coupon.Currency))
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
                    cmd.Parameters.AddWithValue("@currency", coupon.Currency);
                }
                if (!string.IsNullOrEmpty(coupon.PercentOff))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `percent_off` ");
                        values = string.Concat(values, ", @percentOff");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `percent_off` ");
                        values = string.Concat(values, " @percentOff");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@percentOff", coupon.PercentOff);
                }
                if (!string.IsNullOrEmpty(coupon.AmountOff))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `amount_off` ");
                        values = string.Concat(values, ", @amountOff");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `amount_off` ");
                        values = string.Concat(values, " @amountOff");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@amountOff", coupon.AmountOff);
                }
                if (!string.IsNullOrEmpty(coupon.Duration))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `duration` ");
                        values = string.Concat(values, ", @duration");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `duration` ");
                        values = string.Concat(values, " @duration");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@duration", coupon.Duration);
                }
                if (!string.IsNullOrEmpty(coupon.DurationInMonths))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `duration_in_months` ");
                        values = string.Concat(values, ", @durationInMonths");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `duration_in_months` ");
                        values = string.Concat(values, " @durationInMonths");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@durationInMonths", coupon.DurationInMonths);
                }
                if (!string.IsNullOrEmpty(coupon.MaxRedemptions))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `max_redemptions` ");
                        values = string.Concat(values, ", @maxRedemptions");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `max_redemptions` ");
                        values = string.Concat(values, " @maxRedemptions");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@maxRedemptions", coupon.MaxRedemptions);
                }
                if (!string.IsNullOrEmpty(coupon.MaxRedemptionBySingleUser))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `max_redemption_by_single_user` ");
                        values = string.Concat(values, ", @maxRedemptionBySingleUser");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `max_redemption_by_single_user` ");
                        values = string.Concat(values, " @maxRedemptionBySingleUser");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@maxRedemptionBySingleUser", coupon.MaxRedemptionBySingleUser);
                }
                if (!string.IsNullOrEmpty(coupon.RedeemBy))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `redeem_by` ");
                        values = string.Concat(values, ", @redeemBy");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `redeem_by` ");
                        values = string.Concat(values, " @redeemBy");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@redeemBy", coupon.RedeemBy);
                }
                if (!string.IsNullOrEmpty(coupon.Available))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `available` ");
                        values = string.Concat(values, ", @available");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `available` ");
                        values = string.Concat(values, " @available");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", coupon.Available);
                }
                if (!string.IsNullOrEmpty(coupon.Hidden))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `hidden` ");
                        values = string.Concat(values, ", @hidden");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `hidden` ");
                        values = string.Concat(values, " @hidden");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@hidden", coupon.Hidden);
                }
                if (!string.IsNullOrEmpty(coupon.IntervalIntended))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `interval_intended` ");
                        values = string.Concat(values, ", @intervalIntended");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `interval_intended` ");
                        values = string.Concat(values, " @intervalIntended");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@intervalIntended", coupon.IntervalIntended);
                }
                if (!string.IsNullOrEmpty(coupon.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", coupon.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(coupon.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", coupon.CreatedAt);
                }
                if (!string.IsNullOrEmpty(coupon.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", coupon.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                coupon.Id = cmd.LastInsertedId.ToString();
                response.Add(coupon);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CouponModel>();
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
        public List<CouponModel> Create(string connectionString, List<CouponModel> couponList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CouponModel> response = new List<CouponModel>();
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
                        for (int i = 0; i < couponList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_coupons (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(couponList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, couponList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].Name))
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
                                    cmd.Parameters.AddWithValue("@name" + i, couponList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].Code))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `code` ");
                                        values = string.Concat(values, ", @code" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `code` ");
                                        values = string.Concat(values, "@code" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, couponList[i].Code);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].PlatformId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `platform_id` ");
                                        values = string.Concat(values, ", @platformId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `platform_id` ");
                                        values = string.Concat(values, "@platformId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@platformId" + i, couponList[i].PlatformId);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].Currency))
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
                                    cmd.Parameters.AddWithValue("@currency" + i, couponList[i].Currency);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].PercentOff))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `percent_off` ");
                                        values = string.Concat(values, ", @percentOff" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `percent_off` ");
                                        values = string.Concat(values, "@percentOff" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@percentOff" + i, couponList[i].PercentOff);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].AmountOff))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `amount_off` ");
                                        values = string.Concat(values, ", @amountOff" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `amount_off` ");
                                        values = string.Concat(values, "@amountOff" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@amountOff" + i, couponList[i].AmountOff);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].Duration))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `duration` ");
                                        values = string.Concat(values, ", @duration" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `duration` ");
                                        values = string.Concat(values, "@duration" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@duration" + i, couponList[i].Duration);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].DurationInMonths))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `duration_in_months` ");
                                        values = string.Concat(values, ", @durationInMonths" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `duration_in_months` ");
                                        values = string.Concat(values, "@durationInMonths" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@durationInMonths" + i, couponList[i].DurationInMonths);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].MaxRedemptions))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `max_redemptions` ");
                                        values = string.Concat(values, ", @maxRedemptions" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `max_redemptions` ");
                                        values = string.Concat(values, "@maxRedemptions" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@maxRedemptions" + i, couponList[i].MaxRedemptions);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].MaxRedemptionBySingleUser))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `max_redemption_by_single_user` ");
                                        values = string.Concat(values, ", @maxRedemptionBySingleUser" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `max_redemption_by_single_user` ");
                                        values = string.Concat(values, "@maxRedemptionBySingleUser" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@maxRedemptionBySingleUser" + i, couponList[i].MaxRedemptionBySingleUser);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].RedeemBy))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `redeem_by` ");
                                        values = string.Concat(values, ", @redeemBy" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `redeem_by` ");
                                        values = string.Concat(values, "@redeemBy" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@redeemBy" + i, couponList[i].RedeemBy);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].Available))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `available` ");
                                        values = string.Concat(values, ", @available" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `available` ");
                                        values = string.Concat(values, "@available" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, couponList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].Hidden))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `hidden` ");
                                        values = string.Concat(values, ", @hidden" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `hidden` ");
                                        values = string.Concat(values, "@hidden" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@hidden" + i, couponList[i].Hidden);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].IntervalIntended))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `interval_intended` ");
                                        values = string.Concat(values, ", @intervalIntended" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `interval_intended` ");
                                        values = string.Concat(values, "@intervalIntended" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@intervalIntended" + i, couponList[i].IntervalIntended);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, couponList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, couponList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, couponList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(couponList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<CouponModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CouponModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CouponModel>();
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

        public List<CouponModel> Update(string connectionString, CouponModel coupon, CouponModel couponWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CouponModel> response = new List<CouponModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE subst_coupons SET ";
                if (!string.IsNullOrEmpty(coupon.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", coupon.Id);
                }
                if (!string.IsNullOrEmpty(coupon.Name))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `name` = @name");
                    else
                        sql = string.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", coupon.Name);
                }
                if (!string.IsNullOrEmpty(coupon.Code))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `code` = @code");
                    else
                        sql = string.Concat(sql, " `code` = @code");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", coupon.Code);
                }
                if (!string.IsNullOrEmpty(coupon.PlatformId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `platform_id` = @platformId");
                    else
                        sql = string.Concat(sql, " `platform_id` = @platformId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@platformId", coupon.PlatformId);
                }
                if (!string.IsNullOrEmpty(coupon.Currency))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `currency` = @currency");
                    else
                        sql = string.Concat(sql, " `currency` = @currency");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@currency", coupon.Currency);
                }
                if (!string.IsNullOrEmpty(coupon.PercentOff))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `percent_off` = @percentOff");
                    else
                        sql = string.Concat(sql, " `percent_off` = @percentOff");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@percentOff", coupon.PercentOff);
                }
                if (!string.IsNullOrEmpty(coupon.AmountOff))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `amount_off` = @amountOff");
                    else
                        sql = string.Concat(sql, " `amount_off` = @amountOff");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@amountOff", coupon.AmountOff);
                }
                if (!string.IsNullOrEmpty(coupon.Duration))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `duration` = @duration");
                    else
                        sql = string.Concat(sql, " `duration` = @duration");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@duration", coupon.Duration);
                }
                if (!string.IsNullOrEmpty(coupon.DurationInMonths))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `duration_in_months` = @durationInMonths");
                    else
                        sql = string.Concat(sql, " `duration_in_months` = @durationInMonths");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@durationInMonths", coupon.DurationInMonths);
                }
                if (!string.IsNullOrEmpty(coupon.MaxRedemptions))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `max_redemptions` = @maxRedemptions");
                    else
                        sql = string.Concat(sql, " `max_redemptions` = @maxRedemptions");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@maxRedemptions", coupon.MaxRedemptions);
                }
                if (!string.IsNullOrEmpty(coupon.MaxRedemptionBySingleUser))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `max_redemption_by_single_user` = @maxRedemptionBySingleUser");
                    else
                        sql = string.Concat(sql, " `max_redemption_by_single_user` = @maxRedemptionBySingleUser");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@maxRedemptionBySingleUser", coupon.MaxRedemptionBySingleUser);
                }
                if (!string.IsNullOrEmpty(coupon.RedeemBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `redeem_by` = @redeemBy");
                    else
                        sql = string.Concat(sql, " `redeem_by` = @redeemBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@redeemBy", coupon.RedeemBy);
                }
                if (!string.IsNullOrEmpty(coupon.Available))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `available` = @available");
                    else
                        sql = string.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", coupon.Available);
                }
                if (!string.IsNullOrEmpty(coupon.Hidden))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `hidden` = @hidden");
                    else
                        sql = string.Concat(sql, " `hidden` = @hidden");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@hidden", coupon.Hidden);
                }
                if (!string.IsNullOrEmpty(coupon.IntervalIntended))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `interval_intended` = @intervalIntended");
                    else
                        sql = string.Concat(sql, " `interval_intended` = @intervalIntended");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@intervalIntended", coupon.IntervalIntended);
                }
                if (!string.IsNullOrEmpty(coupon.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", coupon.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(coupon.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", coupon.CreatedAt);
                }
                if (!string.IsNullOrEmpty(coupon.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", coupon.UpdatedAt);
                }
                if (couponWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(couponWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", couponWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = string.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", couponWhere.Name);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.Code))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `code` = @codeWhere");
                        else
                            sql = string.Concat(sql, " `code` = @codeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@codeWhere", couponWhere.Code);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.PlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform_id` = @platformIdWhere");
                        else
                            sql = string.Concat(sql, " `platform_id` = @platformIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformIdWhere", couponWhere.PlatformId);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.Currency))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `currency` = @currencyWhere");
                        else
                            sql = string.Concat(sql, " `currency` = @currencyWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@currencyWhere", couponWhere.Currency);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.PercentOff))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `percent_off` = @percentOffWhere");
                        else
                            sql = string.Concat(sql, " `percent_off` = @percentOffWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@percentOffWhere", couponWhere.PercentOff);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.AmountOff))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `amount_off` = @amountOffWhere");
                        else
                            sql = string.Concat(sql, " `amount_off` = @amountOffWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@amountOffWhere", couponWhere.AmountOff);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.Duration))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `duration` = @durationWhere");
                        else
                            sql = string.Concat(sql, " `duration` = @durationWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@durationWhere", couponWhere.Duration);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.DurationInMonths))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `duration_in_months` = @durationInMonthsWhere");
                        else
                            sql = string.Concat(sql, " `duration_in_months` = @durationInMonthsWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@durationInMonthsWhere", couponWhere.DurationInMonths);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.MaxRedemptions))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `max_redemptions` = @maxRedemptionsWhere");
                        else
                            sql = string.Concat(sql, " `max_redemptions` = @maxRedemptionsWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxRedemptionsWhere", couponWhere.MaxRedemptions);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.MaxRedemptionBySingleUser))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `max_redemption_by_single_user` = @maxRedemptionBySingleUserWhere");
                        else
                            sql = string.Concat(sql, " `max_redemption_by_single_user` = @maxRedemptionBySingleUserWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxRedemptionBySingleUserWhere", couponWhere.MaxRedemptionBySingleUser);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.RedeemBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `redeem_by` = @redeemByWhere");
                        else
                            sql = string.Concat(sql, " `redeem_by` = @redeemByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@redeemByWhere", couponWhere.RedeemBy);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = string.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", couponWhere.Available);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.Hidden))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `hidden` = @hiddenWhere");
                        else
                            sql = string.Concat(sql, " `hidden` = @hiddenWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@hiddenWhere", couponWhere.Hidden);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.IntervalIntended))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `interval_intended` = @intervalIntendedWhere");
                        else
                            sql = string.Concat(sql, " `interval_intended` = @intervalIntendedWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@intervalIntendedWhere", couponWhere.IntervalIntended);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", couponWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", couponWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(couponWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", couponWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(coupon);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CouponModel>();
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

        public List<CouponModel> Update(string connectionString, List<CouponModel> couponList, List<CouponModel> couponWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CouponModel> response = new List<CouponModel>();
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
                        for (int i = 0; i < couponList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_coupons SET ";
                                if (!string.IsNullOrEmpty(couponList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, couponList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].Name))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = string.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, couponList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].Code))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `code` = @code" + i);
                                    else
                                        sql = string.Concat(sql, " `code` = @code" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, couponList[i].Code);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].PlatformId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `platform_id` = @platformId" + i);
                                    else
                                        sql = string.Concat(sql, " `platform_id` = @platformId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@platformId" + i, couponList[i].PlatformId);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].Currency))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `currency` = @currency" + i);
                                    else
                                        sql = string.Concat(sql, " `currency` = @currency" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@currency" + i, couponList[i].Currency);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].PercentOff))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `percent_off` = @percentOff" + i);
                                    else
                                        sql = string.Concat(sql, " `percent_off` = @percentOff" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@percentOff" + i, couponList[i].PercentOff);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].AmountOff))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `amount_off` = @amountOff" + i);
                                    else
                                        sql = string.Concat(sql, " `amount_off` = @amountOff" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@amountOff" + i, couponList[i].AmountOff);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].Duration))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `duration` = @duration" + i);
                                    else
                                        sql = string.Concat(sql, " `duration` = @duration" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@duration" + i, couponList[i].Duration);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].DurationInMonths))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `duration_in_months` = @durationInMonths" + i);
                                    else
                                        sql = string.Concat(sql, " `duration_in_months` = @durationInMonths" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@durationInMonths" + i, couponList[i].DurationInMonths);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].MaxRedemptions))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `max_redemptions` = @maxRedemptions" + i);
                                    else
                                        sql = string.Concat(sql, " `max_redemptions` = @maxRedemptions" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@maxRedemptions" + i, couponList[i].MaxRedemptions);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].MaxRedemptionBySingleUser))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `max_redemption_by_single_user` = @maxRedemptionBySingleUser" + i);
                                    else
                                        sql = string.Concat(sql, " `max_redemption_by_single_user` = @maxRedemptionBySingleUser" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@maxRedemptionBySingleUser" + i, couponList[i].MaxRedemptionBySingleUser);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].RedeemBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `redeem_by` = @redeemBy" + i);
                                    else
                                        sql = string.Concat(sql, " `redeem_by` = @redeemBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@redeemBy" + i, couponList[i].RedeemBy);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].Available))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = string.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, couponList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].Hidden))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `hidden` = @hidden" + i);
                                    else
                                        sql = string.Concat(sql, " `hidden` = @hidden" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@hidden" + i, couponList[i].Hidden);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].IntervalIntended))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `interval_intended` = @intervalIntended" + i);
                                    else
                                        sql = string.Concat(sql, " `interval_intended` = @intervalIntended" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@intervalIntended" + i, couponList[i].IntervalIntended);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, couponList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, couponList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(couponList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, couponList[i].UpdatedAt);
                                }
                                if (couponWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(couponWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, couponWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, couponWhereList[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].Code))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `code` = @codeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `code` = @codeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@codeWhere" + i, couponWhereList[i].Code);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].PlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform_id` = @platformIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `platform_id` = @platformIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformIdWhere" + i, couponWhereList[i].PlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].Currency))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `currency` = @currencyWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `currency` = @currencyWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@currencyWhere" + i, couponWhereList[i].Currency);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].PercentOff))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `percent_off` = @percentOffWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `percent_off` = @percentOffWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@percentOffWhere" + i, couponWhereList[i].PercentOff);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].AmountOff))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `amount_off` = @amountOffWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `amount_off` = @amountOffWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@amountOffWhere" + i, couponWhereList[i].AmountOff);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].Duration))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `duration` = @durationWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `duration` = @durationWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@durationWhere" + i, couponWhereList[i].Duration);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].DurationInMonths))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `duration_in_months` = @durationInMonthsWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `duration_in_months` = @durationInMonthsWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@durationInMonthsWhere" + i, couponWhereList[i].DurationInMonths);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].MaxRedemptions))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `max_redemptions` = @maxRedemptionsWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `max_redemptions` = @maxRedemptionsWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxRedemptionsWhere" + i, couponWhereList[i].MaxRedemptions);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].MaxRedemptionBySingleUser))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `max_redemption_by_single_user` = @maxRedemptionBySingleUserWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `max_redemption_by_single_user` = @maxRedemptionBySingleUserWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxRedemptionBySingleUserWhere" + i, couponWhereList[i].MaxRedemptionBySingleUser);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].RedeemBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `redeem_by` = @redeemByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `redeem_by` = @redeemByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@redeemByWhere" + i, couponWhereList[i].RedeemBy);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, couponWhereList[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].Hidden))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `hidden` = @hiddenWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `hidden` = @hiddenWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@hiddenWhere" + i, couponWhereList[i].Hidden);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].IntervalIntended))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `interval_intended` = @intervalIntendedWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `interval_intended` = @intervalIntendedWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@intervalIntendedWhere" + i, couponWhereList[i].IntervalIntended);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, couponWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, couponWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(couponWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, couponWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(couponList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CouponModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CouponModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CouponModel>();
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

        public List<CouponModel> Delete(string connectionString, CouponModel coupon)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CouponModel> response = new List<CouponModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM subst_coupons  ";
                if (coupon != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(coupon.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", coupon.Id);
                    }
                    if (!string.IsNullOrEmpty(coupon.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @name");
                        else
                            sql = string.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", coupon.Name);
                    }
                    if (!string.IsNullOrEmpty(coupon.Code))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `code` = @code");
                        else
                            sql = string.Concat(sql, " `code` = @code");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@code", coupon.Code);
                    }
                    if (!string.IsNullOrEmpty(coupon.PlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform_id` = @platformId");
                        else
                            sql = string.Concat(sql, " `platform_id` = @platformId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformId", coupon.PlatformId);
                    }
                    if (!string.IsNullOrEmpty(coupon.Currency))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `currency` = @currency");
                        else
                            sql = string.Concat(sql, " `currency` = @currency");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@currency", coupon.Currency);
                    }
                    if (!string.IsNullOrEmpty(coupon.PercentOff))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `percent_off` = @percentOff");
                        else
                            sql = string.Concat(sql, " `percent_off` = @percentOff");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@percentOff", coupon.PercentOff);
                    }
                    if (!string.IsNullOrEmpty(coupon.AmountOff))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `amount_off` = @amountOff");
                        else
                            sql = string.Concat(sql, " `amount_off` = @amountOff");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@amountOff", coupon.AmountOff);
                    }
                    if (!string.IsNullOrEmpty(coupon.Duration))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `duration` = @duration");
                        else
                            sql = string.Concat(sql, " `duration` = @duration");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@duration", coupon.Duration);
                    }
                    if (!string.IsNullOrEmpty(coupon.DurationInMonths))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `duration_in_months` = @durationInMonths");
                        else
                            sql = string.Concat(sql, " `duration_in_months` = @durationInMonths");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@durationInMonths", coupon.DurationInMonths);
                    }
                    if (!string.IsNullOrEmpty(coupon.MaxRedemptions))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `max_redemptions` = @maxRedemptions");
                        else
                            sql = string.Concat(sql, " `max_redemptions` = @maxRedemptions");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxRedemptions", coupon.MaxRedemptions);
                    }
                    if (!string.IsNullOrEmpty(coupon.MaxRedemptionBySingleUser))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `max_redemption_by_single_user` = @maxRedemptionBySingleUser");
                        else
                            sql = string.Concat(sql, " `max_redemption_by_single_user` = @maxRedemptionBySingleUser");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxRedemptionBySingleUser", coupon.MaxRedemptionBySingleUser);
                    }
                    if (!string.IsNullOrEmpty(coupon.RedeemBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `redeem_by` = @redeemBy");
                        else
                            sql = string.Concat(sql, " `redeem_by` = @redeemBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@redeemBy", coupon.RedeemBy);
                    }
                    if (!string.IsNullOrEmpty(coupon.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @available");
                        else
                            sql = string.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", coupon.Available);
                    }
                    if (!string.IsNullOrEmpty(coupon.Hidden))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `hidden` = @hidden");
                        else
                            sql = string.Concat(sql, " `hidden` = @hidden");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@hidden", coupon.Hidden);
                    }
                    if (!string.IsNullOrEmpty(coupon.IntervalIntended))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `interval_intended` = @intervalIntended");
                        else
                            sql = string.Concat(sql, " `interval_intended` = @intervalIntended");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@intervalIntended", coupon.IntervalIntended);
                    }
                    if (!string.IsNullOrEmpty(coupon.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", coupon.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(coupon.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", coupon.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(coupon.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", coupon.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(coupon);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CouponModel>();
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

        public List<CouponModel> Delete(string connectionString, List<CouponModel> coupon)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CouponModel> response = new List<CouponModel>();
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
                        for (int i = 0; i < coupon.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_coupons  ";
                                if (coupon[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(coupon[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, coupon[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, coupon[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].Code))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `code` = @code" + i);
                                        else
                                            sql = string.Concat(sql, " `code` = @code" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@code" + i, coupon[i].Code);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].PlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform_id` = @platformId" + i);
                                        else
                                            sql = string.Concat(sql, " `platform_id` = @platformId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformId" + i, coupon[i].PlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].Currency))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `currency` = @currency" + i);
                                        else
                                            sql = string.Concat(sql, " `currency` = @currency" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@currency" + i, coupon[i].Currency);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].PercentOff))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `percent_off` = @percentOff" + i);
                                        else
                                            sql = string.Concat(sql, " `percent_off` = @percentOff" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@percentOff" + i, coupon[i].PercentOff);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].AmountOff))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `amount_off` = @amountOff" + i);
                                        else
                                            sql = string.Concat(sql, " `amount_off` = @amountOff" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@amountOff" + i, coupon[i].AmountOff);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].Duration))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `duration` = @duration" + i);
                                        else
                                            sql = string.Concat(sql, " `duration` = @duration" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@duration" + i, coupon[i].Duration);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].DurationInMonths))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `duration_in_months` = @durationInMonths" + i);
                                        else
                                            sql = string.Concat(sql, " `duration_in_months` = @durationInMonths" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@durationInMonths" + i, coupon[i].DurationInMonths);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].MaxRedemptions))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `max_redemptions` = @maxRedemptions" + i);
                                        else
                                            sql = string.Concat(sql, " `max_redemptions` = @maxRedemptions" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxRedemptions" + i, coupon[i].MaxRedemptions);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].MaxRedemptionBySingleUser))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `max_redemption_by_single_user` = @maxRedemptionBySingleUser" + i);
                                        else
                                            sql = string.Concat(sql, " `max_redemption_by_single_user` = @maxRedemptionBySingleUser" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxRedemptionBySingleUser" + i, coupon[i].MaxRedemptionBySingleUser);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].RedeemBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `redeem_by` = @redeemBy" + i);
                                        else
                                            sql = string.Concat(sql, " `redeem_by` = @redeemBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@redeemBy" + i, coupon[i].RedeemBy);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, coupon[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].Hidden))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `hidden` = @hidden" + i);
                                        else
                                            sql = string.Concat(sql, " `hidden` = @hidden" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@hidden" + i, coupon[i].Hidden);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].IntervalIntended))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `interval_intended` = @intervalIntended" + i);
                                        else
                                            sql = string.Concat(sql, " `interval_intended` = @intervalIntended" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@intervalIntended" + i, coupon[i].IntervalIntended);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, coupon[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, coupon[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(coupon[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, coupon[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(coupon[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CouponModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CouponModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CouponModel>();
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
