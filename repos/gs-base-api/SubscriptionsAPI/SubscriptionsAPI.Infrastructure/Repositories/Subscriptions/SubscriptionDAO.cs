using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class SubscriptionDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, SubscriptionModel> ReadD(string connectionString, SubscriptionModel subscription, bool firstOne, bool orderByLastDate, SubscriptionModel subscriptionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SubscriptionModel> response = new Dictionary<string, SubscriptionModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `platform_id`, `customer_platform_id`, `plan_platform_id`, `quantity`, `status`, `current_period_end`, `platform_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscriptions ";
                {
                    if (subscription != null)
                    {
                        if (!string.IsNullOrEmpty(subscription.Id))
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
                            cmd.Parameters.AddWithValue("@id", subscription.Id);
                        }
                        if (!string.IsNullOrEmpty(subscription.PlatformId))
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
                            cmd.Parameters.AddWithValue("@platformId", subscription.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscription.CustomerPlatformId))
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
                            cmd.Parameters.AddWithValue("@customerPlatformId", subscription.CustomerPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscription.PlanPlatformId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `plan_platform_id` = @planPlatformId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `plan_platform_id` = @planPlatformId ");
                            }
                            cmd.Parameters.AddWithValue("@planPlatformId", subscription.PlanPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscription.Quantity))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quantity` = @quantity ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quantity` = @quantity ");
                            }
                            cmd.Parameters.AddWithValue("@quantity", subscription.Quantity);
                        }
                        if (!string.IsNullOrEmpty(subscription.Status))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `status` = @status ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `status` = @status ");
                            }
                            cmd.Parameters.AddWithValue("@status", subscription.Status);
                        }
                        if (!string.IsNullOrEmpty(subscription.CurrentPeriodEnd))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `current_period_end` = @currentPeriodEnd ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `current_period_end` = @currentPeriodEnd ");
                            }
                            cmd.Parameters.AddWithValue("@currentPeriodEnd", subscription.CurrentPeriodEnd);
                        }
                        if (!string.IsNullOrEmpty(subscription.PlatformCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `platform_code` = @platformCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `platform_code` = @platformCode ");
                            }
                            cmd.Parameters.AddWithValue("@platformCode", subscription.PlatformCode);
                        }
                        if (!string.IsNullOrEmpty(subscription.Type))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `type` = @type ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `type` = @type ");
                            }
                            cmd.Parameters.AddWithValue("@type", subscription.Type);
                        }
                        if (!string.IsNullOrEmpty(subscription.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", subscription.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscription.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", subscription.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscription.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", subscription.UpdatedAt);
                        }
                    }
                    if (subscriptionWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + subscriptionWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.PlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `platform_id` LIKE @platformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `platform_id` LIKE @platformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformIdWhereLike", "%" + subscriptionWhereLike.PlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.CustomerPlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `customer_platform_id` LIKE @customerPlatformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `customer_platform_id` LIKE @customerPlatformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customerPlatformIdWhereLike", "%" + subscriptionWhereLike.CustomerPlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.PlanPlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `plan_platform_id` LIKE @planPlatformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `plan_platform_id` LIKE @planPlatformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planPlatformIdWhereLike", "%" + subscriptionWhereLike.PlanPlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Quantity))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `quantity` LIKE @quantityWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `quantity` LIKE @quantityWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quantityWhereLike", "%" + subscriptionWhereLike.Quantity + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Status))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `status` LIKE @statusWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `status` LIKE @statusWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@statusWhereLike", "%" + subscriptionWhereLike.Status + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.CurrentPeriodEnd))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `current_period_end` LIKE @currentPeriodEndWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `current_period_end` LIKE @currentPeriodEndWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@currentPeriodEndWhereLike", "%" + subscriptionWhereLike.CurrentPeriodEnd + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.PlatformCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `platform_code` LIKE @platformCodeWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `platform_code` LIKE @platformCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformCodeWhereLike", "%" + subscriptionWhereLike.PlatformCode + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Type))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `type` LIKE @typeWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `type` LIKE @typeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@typeWhereLike", "%" + subscriptionWhereLike.Type + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + subscriptionWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + subscriptionWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + subscriptionWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = string.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                subscription = new SubscriptionModel();
                                if (!reader.IsDBNull(0))
                                    subscription.Id = reader.GetValue(0).ToString();
                                else
                                    subscription.Id = null;
                                if (!reader.IsDBNull(1))
                                    subscription.PlatformId = reader.GetValue(1).ToString();
                                else
                                    subscription.PlatformId = null;
                                if (!reader.IsDBNull(2))
                                    subscription.CustomerPlatformId = reader.GetValue(2).ToString();
                                else
                                    subscription.CustomerPlatformId = null;
                                if (!reader.IsDBNull(3))
                                    subscription.PlanPlatformId = reader.GetValue(3).ToString();
                                else
                                    subscription.PlanPlatformId = null;
                                if (!reader.IsDBNull(4))
                                    subscription.Quantity = reader.GetValue(4).ToString();
                                else
                                    subscription.Quantity = null;
                                if (!reader.IsDBNull(5))
                                    subscription.Status = reader.GetValue(5).ToString();
                                else
                                    subscription.Status = null;
                                if (!reader.IsDBNull(6))
                                    subscription.CurrentPeriodEnd = reader.GetValue(6).ToString();
                                else
                                    subscription.CurrentPeriodEnd = null;
                                if (!reader.IsDBNull(7))
                                    subscription.PlatformCode = reader.GetValue(7).ToString();
                                else
                                    subscription.PlatformCode = null;
                                if (!reader.IsDBNull(8))
                                    subscription.Type = reader.GetValue(8).ToString();
                                else
                                    subscription.Type = null;
                                if (!reader.IsDBNull(9))
                                    subscription.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    subscription.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    subscription.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    subscription.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    subscription.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    subscription.UpdatedAt = null;
                                response.Add(subscription.Id, subscription);
                                subscription = new SubscriptionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, SubscriptionModel>();
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
                response = new Dictionary<string, SubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, SubscriptionModel> ReadD(string connectionString, SubscriptionModel subscription, bool orderByLastDate, int fromRow, int limit, SubscriptionModel subscriptionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SubscriptionModel> response = new Dictionary<string, SubscriptionModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `platform_id`, `customer_platform_id`, `plan_platform_id`, `quantity`, `status`, `current_period_end`, `platform_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscriptions ";
                {
                    if (subscription != null)
                    {
                        if (!string.IsNullOrEmpty(subscription.Id))
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
                            cmd.Parameters.AddWithValue("@id", subscription.Id);
                        }
                        if (!string.IsNullOrEmpty(subscription.PlatformId))
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
                            cmd.Parameters.AddWithValue("@platformId", subscription.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscription.CustomerPlatformId))
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
                            cmd.Parameters.AddWithValue("@customerPlatformId", subscription.CustomerPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscription.PlanPlatformId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `plan_platform_id` = @planPlatformId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `plan_platform_id` = @planPlatformId ");
                            }
                            cmd.Parameters.AddWithValue("@planPlatformId", subscription.PlanPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscription.Quantity))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quantity` = @quantity ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quantity` = @quantity ");
                            }
                            cmd.Parameters.AddWithValue("@quantity", subscription.Quantity);
                        }
                        if (!string.IsNullOrEmpty(subscription.Status))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `status` = @status ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `status` = @status ");
                            }
                            cmd.Parameters.AddWithValue("@status", subscription.Status);
                        }
                        if (!string.IsNullOrEmpty(subscription.CurrentPeriodEnd))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `current_period_end` = @currentPeriodEnd ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `current_period_end` = @currentPeriodEnd ");
                            }
                            cmd.Parameters.AddWithValue("@currentPeriodEnd", subscription.CurrentPeriodEnd);
                        }
                        if (!string.IsNullOrEmpty(subscription.PlatformCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `platform_code` = @platformCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `platform_code` = @platformCode ");
                            }
                            cmd.Parameters.AddWithValue("@platformCode", subscription.PlatformCode);
                        }
                        if (!string.IsNullOrEmpty(subscription.Type))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `type` = @type ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `type` = @type ");
                            }
                            cmd.Parameters.AddWithValue("@type", subscription.Type);
                        }
                        if (!string.IsNullOrEmpty(subscription.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", subscription.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscription.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", subscription.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscription.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", subscription.UpdatedAt);
                        }
                    }
                    if (subscriptionWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + subscriptionWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.PlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `platform_id` LIKE @platformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `platform_id` LIKE @platformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformIdWhereLike", "%" + subscriptionWhereLike.PlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.CustomerPlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `customer_platform_id` LIKE @customerPlatformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `customer_platform_id` LIKE @customerPlatformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customerPlatformIdWhereLike", "%" + subscriptionWhereLike.CustomerPlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.PlanPlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `plan_platform_id` LIKE @planPlatformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `plan_platform_id` LIKE @planPlatformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planPlatformIdWhereLike", "%" + subscriptionWhereLike.PlanPlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Quantity))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `quantity` LIKE @quantityWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `quantity` LIKE @quantityWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quantityWhereLike", "%" + subscriptionWhereLike.Quantity + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Status))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `status` LIKE @statusWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `status` LIKE @statusWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@statusWhereLike", "%" + subscriptionWhereLike.Status + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.CurrentPeriodEnd))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `current_period_end` LIKE @currentPeriodEndWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `current_period_end` LIKE @currentPeriodEndWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@currentPeriodEndWhereLike", "%" + subscriptionWhereLike.CurrentPeriodEnd + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.PlatformCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `platform_code` LIKE @platformCodeWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `platform_code` LIKE @platformCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformCodeWhereLike", "%" + subscriptionWhereLike.PlatformCode + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Type))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `type` LIKE @typeWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `type` LIKE @typeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@typeWhereLike", "%" + subscriptionWhereLike.Type + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + subscriptionWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + subscriptionWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + subscriptionWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                subscription = new SubscriptionModel();
                                if (!reader.IsDBNull(0))
                                    subscription.Id = reader.GetValue(0).ToString();
                                else
                                    subscription.Id = null;
                                if (!reader.IsDBNull(1))
                                    subscription.PlatformId = reader.GetValue(1).ToString();
                                else
                                    subscription.PlatformId = null;
                                if (!reader.IsDBNull(2))
                                    subscription.CustomerPlatformId = reader.GetValue(2).ToString();
                                else
                                    subscription.CustomerPlatformId = null;
                                if (!reader.IsDBNull(3))
                                    subscription.PlanPlatformId = reader.GetValue(3).ToString();
                                else
                                    subscription.PlanPlatformId = null;
                                if (!reader.IsDBNull(4))
                                    subscription.Quantity = reader.GetValue(4).ToString();
                                else
                                    subscription.Quantity = null;
                                if (!reader.IsDBNull(5))
                                    subscription.Status = reader.GetValue(5).ToString();
                                else
                                    subscription.Status = null;
                                if (!reader.IsDBNull(6))
                                    subscription.CurrentPeriodEnd = reader.GetValue(6).ToString();
                                else
                                    subscription.CurrentPeriodEnd = null;
                                if (!reader.IsDBNull(7))
                                    subscription.PlatformCode = reader.GetValue(7).ToString();
                                else
                                    subscription.PlatformCode = null;
                                if (!reader.IsDBNull(8))
                                    subscription.Type = reader.GetValue(8).ToString();
                                else
                                    subscription.Type = null;
                                if (!reader.IsDBNull(9))
                                    subscription.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    subscription.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    subscription.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    subscription.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    subscription.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    subscription.UpdatedAt = null;
                                response.Add(subscription.Id, subscription);
                                subscription = new SubscriptionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, SubscriptionModel>();
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
                response = new Dictionary<string, SubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, SubscriptionModel> ReadD(string connectionString, List<SubscriptionModel> subscriptionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SubscriptionModel> response = new Dictionary<string, SubscriptionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `platform_id`, `customer_platform_id`, `plan_platform_id`, `quantity`, `status`, `current_period_end`, `platform_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscriptions ";
                if (subscriptionWhere != null && subscriptionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < subscriptionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, subscriptionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, subscriptionWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].CustomerPlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `customer_platform_id` = @customerPlatformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customerPlatformId" + i, subscriptionWhere[i].CustomerPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].PlanPlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_platform_id` = @planPlatformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planPlatformId" + i, subscriptionWhere[i].PlanPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Quantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quantity` = @quantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quantity" + i, subscriptionWhere[i].Quantity);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Status))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `status` = @status" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@status" + i, subscriptionWhere[i].Status);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].CurrentPeriodEnd))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `current_period_end` = @currentPeriodEnd" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currentPeriodEnd" + i, subscriptionWhere[i].CurrentPeriodEnd);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].PlatformCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_code` = @platformCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformCode" + i, subscriptionWhere[i].PlatformCode);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Type))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `type` = @type" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@type" + i, subscriptionWhere[i].Type);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != subscriptionWhere.Count - 1)
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
                            SubscriptionModel subscription = new SubscriptionModel(); if (!reader.IsDBNull(0))
                                subscription.Id = reader.GetValue(0).ToString();
                            else
                                subscription.Id = null;
                            if (!reader.IsDBNull(1))
                                subscription.PlatformId = reader.GetValue(1).ToString();
                            else
                                subscription.PlatformId = null;
                            if (!reader.IsDBNull(2))
                                subscription.CustomerPlatformId = reader.GetValue(2).ToString();
                            else
                                subscription.CustomerPlatformId = null;
                            if (!reader.IsDBNull(3))
                                subscription.PlanPlatformId = reader.GetValue(3).ToString();
                            else
                                subscription.PlanPlatformId = null;
                            if (!reader.IsDBNull(4))
                                subscription.Quantity = reader.GetValue(4).ToString();
                            else
                                subscription.Quantity = null;
                            if (!reader.IsDBNull(5))
                                subscription.Status = reader.GetValue(5).ToString();
                            else
                                subscription.Status = null;
                            if (!reader.IsDBNull(6))
                                subscription.CurrentPeriodEnd = reader.GetValue(6).ToString();
                            else
                                subscription.CurrentPeriodEnd = null;
                            if (!reader.IsDBNull(7))
                                subscription.PlatformCode = reader.GetValue(7).ToString();
                            else
                                subscription.PlatformCode = null;
                            if (!reader.IsDBNull(8))
                                subscription.Type = reader.GetValue(8).ToString();
                            else
                                subscription.Type = null;
                            if (!reader.IsDBNull(9))
                                subscription.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                subscription.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                subscription.CreatedAt = reader.GetValue(10).ToString();
                            else
                                subscription.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                subscription.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                subscription.UpdatedAt = null;
                            response.Add(subscription.Id, subscription);
                            subscription = new SubscriptionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, SubscriptionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, SubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, SubscriptionModel> ReadD(string connectionString, List<SubscriptionModel> subscriptionWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SubscriptionModel> response = new Dictionary<string, SubscriptionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `platform_id`, `customer_platform_id`, `plan_platform_id`, `quantity`, `status`, `current_period_end`, `platform_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscriptions ";
                if (subscriptionWhere != null && subscriptionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < subscriptionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, subscriptionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, subscriptionWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].CustomerPlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `customer_platform_id` = @customerPlatformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customerPlatformId" + i, subscriptionWhere[i].CustomerPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].PlanPlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_platform_id` = @planPlatformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planPlatformId" + i, subscriptionWhere[i].PlanPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Quantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quantity` = @quantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quantity" + i, subscriptionWhere[i].Quantity);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Status))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `status` = @status" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@status" + i, subscriptionWhere[i].Status);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].CurrentPeriodEnd))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `current_period_end` = @currentPeriodEnd" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currentPeriodEnd" + i, subscriptionWhere[i].CurrentPeriodEnd);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].PlatformCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_code` = @platformCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformCode" + i, subscriptionWhere[i].PlatformCode);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Type))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `type` = @type" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@type" + i, subscriptionWhere[i].Type);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != subscriptionWhere.Count - 1)
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
                            SubscriptionModel subscription = new SubscriptionModel(); if (!reader.IsDBNull(0))
                                subscription.Id = reader.GetValue(0).ToString();
                            else
                                subscription.Id = null;
                            if (!reader.IsDBNull(1))
                                subscription.PlatformId = reader.GetValue(1).ToString();
                            else
                                subscription.PlatformId = null;
                            if (!reader.IsDBNull(2))
                                subscription.CustomerPlatformId = reader.GetValue(2).ToString();
                            else
                                subscription.CustomerPlatformId = null;
                            if (!reader.IsDBNull(3))
                                subscription.PlanPlatformId = reader.GetValue(3).ToString();
                            else
                                subscription.PlanPlatformId = null;
                            if (!reader.IsDBNull(4))
                                subscription.Quantity = reader.GetValue(4).ToString();
                            else
                                subscription.Quantity = null;
                            if (!reader.IsDBNull(5))
                                subscription.Status = reader.GetValue(5).ToString();
                            else
                                subscription.Status = null;
                            if (!reader.IsDBNull(6))
                                subscription.CurrentPeriodEnd = reader.GetValue(6).ToString();
                            else
                                subscription.CurrentPeriodEnd = null;
                            if (!reader.IsDBNull(7))
                                subscription.PlatformCode = reader.GetValue(7).ToString();
                            else
                                subscription.PlatformCode = null;
                            if (!reader.IsDBNull(8))
                                subscription.Type = reader.GetValue(8).ToString();
                            else
                                subscription.Type = null;
                            if (!reader.IsDBNull(9))
                                subscription.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                subscription.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                subscription.CreatedAt = reader.GetValue(10).ToString();
                            else
                                subscription.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                subscription.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                subscription.UpdatedAt = null;
                            response.Add(subscription.Id, subscription);
                            subscription = new SubscriptionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, SubscriptionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, SubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionModel> Read(string connectionString, SubscriptionModel subscription, bool firstOne, bool orderByLastDate, SubscriptionModel subscriptionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionModel> response = new List<SubscriptionModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `platform_id`, `customer_platform_id`, `plan_platform_id`, `quantity`, `status`, `current_period_end`, `platform_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscriptions ";
                {
                    if (subscription != null)
                    {
                        if (!string.IsNullOrEmpty(subscription.Id))
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
                            cmd.Parameters.AddWithValue("@id", subscription.Id);
                        }
                        if (!string.IsNullOrEmpty(subscription.PlatformId))
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
                            cmd.Parameters.AddWithValue("@platformId", subscription.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscription.CustomerPlatformId))
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
                            cmd.Parameters.AddWithValue("@customerPlatformId", subscription.CustomerPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscription.PlanPlatformId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `plan_platform_id` = @planPlatformId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `plan_platform_id` = @planPlatformId ");
                            }
                            cmd.Parameters.AddWithValue("@planPlatformId", subscription.PlanPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscription.Quantity))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quantity` = @quantity ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quantity` = @quantity ");
                            }
                            cmd.Parameters.AddWithValue("@quantity", subscription.Quantity);
                        }
                        if (!string.IsNullOrEmpty(subscription.Status))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `status` = @status ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `status` = @status ");
                            }
                            cmd.Parameters.AddWithValue("@status", subscription.Status);
                        }
                        if (!string.IsNullOrEmpty(subscription.CurrentPeriodEnd))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `current_period_end` = @currentPeriodEnd ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `current_period_end` = @currentPeriodEnd ");
                            }
                            cmd.Parameters.AddWithValue("@currentPeriodEnd", subscription.CurrentPeriodEnd);
                        }
                        if (!string.IsNullOrEmpty(subscription.PlatformCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `platform_code` = @platformCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `platform_code` = @platformCode ");
                            }
                            cmd.Parameters.AddWithValue("@platformCode", subscription.PlatformCode);
                        }
                        if (!string.IsNullOrEmpty(subscription.Type))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `type` = @type ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `type` = @type ");
                            }
                            cmd.Parameters.AddWithValue("@type", subscription.Type);
                        }
                        if (!string.IsNullOrEmpty(subscription.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", subscription.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscription.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", subscription.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscription.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", subscription.UpdatedAt);
                        }
                    }
                    if (subscriptionWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + subscriptionWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.PlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `platform_id` LIKE @platformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `platform_id` LIKE @platformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformIdWhereLike", "%" + subscriptionWhereLike.PlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.CustomerPlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `customer_platform_id` LIKE @customerPlatformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `customer_platform_id` LIKE @customerPlatformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customerPlatformIdWhereLike", "%" + subscriptionWhereLike.CustomerPlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.PlanPlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `plan_platform_id` LIKE @planPlatformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `plan_platform_id` LIKE @planPlatformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planPlatformIdWhereLike", "%" + subscriptionWhereLike.PlanPlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Quantity))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `quantity` LIKE @quantityWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `quantity` LIKE @quantityWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quantityWhereLike", "%" + subscriptionWhereLike.Quantity + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Status))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `status` LIKE @statusWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `status` LIKE @statusWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@statusWhereLike", "%" + subscriptionWhereLike.Status + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.CurrentPeriodEnd))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `current_period_end` LIKE @currentPeriodEndWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `current_period_end` LIKE @currentPeriodEndWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@currentPeriodEndWhereLike", "%" + subscriptionWhereLike.CurrentPeriodEnd + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.PlatformCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `platform_code` LIKE @platformCodeWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `platform_code` LIKE @platformCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformCodeWhereLike", "%" + subscriptionWhereLike.PlatformCode + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Type))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `type` LIKE @typeWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `type` LIKE @typeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@typeWhereLike", "%" + subscriptionWhereLike.Type + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + subscriptionWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + subscriptionWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + subscriptionWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = string.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                subscription = new SubscriptionModel();
                                if (!reader.IsDBNull(0))
                                    subscription.Id = reader.GetValue(0).ToString();
                                else
                                    subscription.Id = null;
                                if (!reader.IsDBNull(1))
                                    subscription.PlatformId = reader.GetValue(1).ToString();
                                else
                                    subscription.PlatformId = null;
                                if (!reader.IsDBNull(2))
                                    subscription.CustomerPlatformId = reader.GetValue(2).ToString();
                                else
                                    subscription.CustomerPlatformId = null;
                                if (!reader.IsDBNull(3))
                                    subscription.PlanPlatformId = reader.GetValue(3).ToString();
                                else
                                    subscription.PlanPlatformId = null;
                                if (!reader.IsDBNull(4))
                                    subscription.Quantity = reader.GetValue(4).ToString();
                                else
                                    subscription.Quantity = null;
                                if (!reader.IsDBNull(5))
                                    subscription.Status = reader.GetValue(5).ToString();
                                else
                                    subscription.Status = null;
                                if (!reader.IsDBNull(6))
                                    subscription.CurrentPeriodEnd = reader.GetValue(6).ToString();
                                else
                                    subscription.CurrentPeriodEnd = null;
                                if (!reader.IsDBNull(7))
                                    subscription.PlatformCode = reader.GetValue(7).ToString();
                                else
                                    subscription.PlatformCode = null;
                                if (!reader.IsDBNull(8))
                                    subscription.Type = reader.GetValue(8).ToString();
                                else
                                    subscription.Type = null;
                                if (!reader.IsDBNull(9))
                                    subscription.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    subscription.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    subscription.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    subscription.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    subscription.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    subscription.UpdatedAt = null;
                                response.Add(subscription);
                                subscription = new SubscriptionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<SubscriptionModel>();
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
                response = new List<SubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionModel> Read(string connectionString, SubscriptionModel subscription, bool orderByLastDate, int fromRow, int limit, SubscriptionModel subscriptionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionModel> response = new List<SubscriptionModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `platform_id`, `customer_platform_id`, `plan_platform_id`, `quantity`, `status`, `current_period_end`, `platform_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscriptions ";
                {
                    if (subscription != null)
                    {
                        if (!string.IsNullOrEmpty(subscription.Id))
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
                            cmd.Parameters.AddWithValue("@id", subscription.Id);
                        }
                        if (!string.IsNullOrEmpty(subscription.PlatformId))
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
                            cmd.Parameters.AddWithValue("@platformId", subscription.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscription.CustomerPlatformId))
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
                            cmd.Parameters.AddWithValue("@customerPlatformId", subscription.CustomerPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscription.PlanPlatformId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `plan_platform_id` = @planPlatformId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `plan_platform_id` = @planPlatformId ");
                            }
                            cmd.Parameters.AddWithValue("@planPlatformId", subscription.PlanPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscription.Quantity))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quantity` = @quantity ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quantity` = @quantity ");
                            }
                            cmd.Parameters.AddWithValue("@quantity", subscription.Quantity);
                        }
                        if (!string.IsNullOrEmpty(subscription.Status))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `status` = @status ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `status` = @status ");
                            }
                            cmd.Parameters.AddWithValue("@status", subscription.Status);
                        }
                        if (!string.IsNullOrEmpty(subscription.CurrentPeriodEnd))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `current_period_end` = @currentPeriodEnd ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `current_period_end` = @currentPeriodEnd ");
                            }
                            cmd.Parameters.AddWithValue("@currentPeriodEnd", subscription.CurrentPeriodEnd);
                        }
                        if (!string.IsNullOrEmpty(subscription.PlatformCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `platform_code` = @platformCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `platform_code` = @platformCode ");
                            }
                            cmd.Parameters.AddWithValue("@platformCode", subscription.PlatformCode);
                        }
                        if (!string.IsNullOrEmpty(subscription.Type))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `type` = @type ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `type` = @type ");
                            }
                            cmd.Parameters.AddWithValue("@type", subscription.Type);
                        }
                        if (!string.IsNullOrEmpty(subscription.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", subscription.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscription.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", subscription.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscription.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", subscription.UpdatedAt);
                        }
                    }
                    if (subscriptionWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + subscriptionWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.PlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `platform_id` LIKE @platformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `platform_id` LIKE @platformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformIdWhereLike", "%" + subscriptionWhereLike.PlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.CustomerPlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `customer_platform_id` LIKE @customerPlatformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `customer_platform_id` LIKE @customerPlatformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customerPlatformIdWhereLike", "%" + subscriptionWhereLike.CustomerPlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.PlanPlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `plan_platform_id` LIKE @planPlatformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `plan_platform_id` LIKE @planPlatformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planPlatformIdWhereLike", "%" + subscriptionWhereLike.PlanPlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Quantity))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `quantity` LIKE @quantityWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `quantity` LIKE @quantityWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quantityWhereLike", "%" + subscriptionWhereLike.Quantity + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Status))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `status` LIKE @statusWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `status` LIKE @statusWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@statusWhereLike", "%" + subscriptionWhereLike.Status + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.CurrentPeriodEnd))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `current_period_end` LIKE @currentPeriodEndWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `current_period_end` LIKE @currentPeriodEndWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@currentPeriodEndWhereLike", "%" + subscriptionWhereLike.CurrentPeriodEnd + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.PlatformCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `platform_code` LIKE @platformCodeWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `platform_code` LIKE @platformCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformCodeWhereLike", "%" + subscriptionWhereLike.PlatformCode + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.Type))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `type` LIKE @typeWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `type` LIKE @typeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@typeWhereLike", "%" + subscriptionWhereLike.Type + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + subscriptionWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + subscriptionWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + subscriptionWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                subscription = new SubscriptionModel();
                                if (!reader.IsDBNull(0))
                                    subscription.Id = reader.GetValue(0).ToString();
                                else
                                    subscription.Id = null;
                                if (!reader.IsDBNull(1))
                                    subscription.PlatformId = reader.GetValue(1).ToString();
                                else
                                    subscription.PlatformId = null;
                                if (!reader.IsDBNull(2))
                                    subscription.CustomerPlatformId = reader.GetValue(2).ToString();
                                else
                                    subscription.CustomerPlatformId = null;
                                if (!reader.IsDBNull(3))
                                    subscription.PlanPlatformId = reader.GetValue(3).ToString();
                                else
                                    subscription.PlanPlatformId = null;
                                if (!reader.IsDBNull(4))
                                    subscription.Quantity = reader.GetValue(4).ToString();
                                else
                                    subscription.Quantity = null;
                                if (!reader.IsDBNull(5))
                                    subscription.Status = reader.GetValue(5).ToString();
                                else
                                    subscription.Status = null;
                                if (!reader.IsDBNull(6))
                                    subscription.CurrentPeriodEnd = reader.GetValue(6).ToString();
                                else
                                    subscription.CurrentPeriodEnd = null;
                                if (!reader.IsDBNull(7))
                                    subscription.PlatformCode = reader.GetValue(7).ToString();
                                else
                                    subscription.PlatformCode = null;
                                if (!reader.IsDBNull(8))
                                    subscription.Type = reader.GetValue(8).ToString();
                                else
                                    subscription.Type = null;
                                if (!reader.IsDBNull(9))
                                    subscription.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    subscription.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    subscription.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    subscription.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    subscription.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    subscription.UpdatedAt = null;
                                response.Add(subscription);
                                subscription = new SubscriptionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<SubscriptionModel>();
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
                response = new List<SubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionModel> Read(string connectionString, List<SubscriptionModel> subscriptionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionModel> response = new List<SubscriptionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `platform_id`, `customer_platform_id`, `plan_platform_id`, `quantity`, `status`, `current_period_end`, `platform_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscriptions ";
                if (subscriptionWhere != null && subscriptionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < subscriptionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, subscriptionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, subscriptionWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].CustomerPlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `customer_platform_id` = @customerPlatformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customerPlatformId" + i, subscriptionWhere[i].CustomerPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].PlanPlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_platform_id` = @planPlatformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planPlatformId" + i, subscriptionWhere[i].PlanPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Quantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quantity` = @quantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quantity" + i, subscriptionWhere[i].Quantity);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Status))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `status` = @status" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@status" + i, subscriptionWhere[i].Status);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].CurrentPeriodEnd))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `current_period_end` = @currentPeriodEnd" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currentPeriodEnd" + i, subscriptionWhere[i].CurrentPeriodEnd);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].PlatformCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_code` = @platformCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformCode" + i, subscriptionWhere[i].PlatformCode);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Type))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `type` = @type" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@type" + i, subscriptionWhere[i].Type);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != subscriptionWhere.Count - 1)
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
                            SubscriptionModel subscription = new SubscriptionModel(); if (!reader.IsDBNull(0))
                                subscription.Id = reader.GetValue(0).ToString();
                            else
                                subscription.Id = null;
                            if (!reader.IsDBNull(1))
                                subscription.PlatformId = reader.GetValue(1).ToString();
                            else
                                subscription.PlatformId = null;
                            if (!reader.IsDBNull(2))
                                subscription.CustomerPlatformId = reader.GetValue(2).ToString();
                            else
                                subscription.CustomerPlatformId = null;
                            if (!reader.IsDBNull(3))
                                subscription.PlanPlatformId = reader.GetValue(3).ToString();
                            else
                                subscription.PlanPlatformId = null;
                            if (!reader.IsDBNull(4))
                                subscription.Quantity = reader.GetValue(4).ToString();
                            else
                                subscription.Quantity = null;
                            if (!reader.IsDBNull(5))
                                subscription.Status = reader.GetValue(5).ToString();
                            else
                                subscription.Status = null;
                            if (!reader.IsDBNull(6))
                                subscription.CurrentPeriodEnd = reader.GetValue(6).ToString();
                            else
                                subscription.CurrentPeriodEnd = null;
                            if (!reader.IsDBNull(7))
                                subscription.PlatformCode = reader.GetValue(7).ToString();
                            else
                                subscription.PlatformCode = null;
                            if (!reader.IsDBNull(8))
                                subscription.Type = reader.GetValue(8).ToString();
                            else
                                subscription.Type = null;
                            if (!reader.IsDBNull(9))
                                subscription.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                subscription.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                subscription.CreatedAt = reader.GetValue(10).ToString();
                            else
                                subscription.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                subscription.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                subscription.UpdatedAt = null;
                            response.Add(subscription);
                            subscription = new SubscriptionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<SubscriptionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionModel> Read(string connectionString, List<SubscriptionModel> subscriptionWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionModel> response = new List<SubscriptionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `platform_id`, `customer_platform_id`, `plan_platform_id`, `quantity`, `status`, `current_period_end`, `platform_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscriptions ";
                if (subscriptionWhere != null && subscriptionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < subscriptionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, subscriptionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, subscriptionWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].CustomerPlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `customer_platform_id` = @customerPlatformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customerPlatformId" + i, subscriptionWhere[i].CustomerPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].PlanPlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_platform_id` = @planPlatformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planPlatformId" + i, subscriptionWhere[i].PlanPlatformId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Quantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quantity` = @quantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quantity" + i, subscriptionWhere[i].Quantity);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Status))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `status` = @status" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@status" + i, subscriptionWhere[i].Status);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].CurrentPeriodEnd))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `current_period_end` = @currentPeriodEnd" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currentPeriodEnd" + i, subscriptionWhere[i].CurrentPeriodEnd);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].PlatformCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_code` = @platformCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformCode" + i, subscriptionWhere[i].PlatformCode);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].Type))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `type` = @type" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@type" + i, subscriptionWhere[i].Type);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != subscriptionWhere.Count - 1)
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
                            SubscriptionModel subscription = new SubscriptionModel(); if (!reader.IsDBNull(0))
                                subscription.Id = reader.GetValue(0).ToString();
                            else
                                subscription.Id = null;
                            if (!reader.IsDBNull(1))
                                subscription.PlatformId = reader.GetValue(1).ToString();
                            else
                                subscription.PlatformId = null;
                            if (!reader.IsDBNull(2))
                                subscription.CustomerPlatformId = reader.GetValue(2).ToString();
                            else
                                subscription.CustomerPlatformId = null;
                            if (!reader.IsDBNull(3))
                                subscription.PlanPlatformId = reader.GetValue(3).ToString();
                            else
                                subscription.PlanPlatformId = null;
                            if (!reader.IsDBNull(4))
                                subscription.Quantity = reader.GetValue(4).ToString();
                            else
                                subscription.Quantity = null;
                            if (!reader.IsDBNull(5))
                                subscription.Status = reader.GetValue(5).ToString();
                            else
                                subscription.Status = null;
                            if (!reader.IsDBNull(6))
                                subscription.CurrentPeriodEnd = reader.GetValue(6).ToString();
                            else
                                subscription.CurrentPeriodEnd = null;
                            if (!reader.IsDBNull(7))
                                subscription.PlatformCode = reader.GetValue(7).ToString();
                            else
                                subscription.PlatformCode = null;
                            if (!reader.IsDBNull(8))
                                subscription.Type = reader.GetValue(8).ToString();
                            else
                                subscription.Type = null;
                            if (!reader.IsDBNull(9))
                                subscription.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                subscription.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                subscription.CreatedAt = reader.GetValue(10).ToString();
                            else
                                subscription.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                subscription.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                subscription.UpdatedAt = null;
                            response.Add(subscription);
                            subscription = new SubscriptionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<SubscriptionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionModel> Create(string connectionString, SubscriptionModel subscription)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionModel> response = new List<SubscriptionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO subst_subscriptions (";
                string values = "(";
                if (!string.IsNullOrEmpty(subscription.Id))
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
                    cmd.Parameters.AddWithValue("@id", subscription.Id);
                }
                if (!string.IsNullOrEmpty(subscription.PlatformId))
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
                    cmd.Parameters.AddWithValue("@platformId", subscription.PlatformId);
                }
                if (!string.IsNullOrEmpty(subscription.CustomerPlatformId))
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
                    cmd.Parameters.AddWithValue("@customerPlatformId", subscription.CustomerPlatformId);
                }
                if (!string.IsNullOrEmpty(subscription.PlanPlatformId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `plan_platform_id` ");
                        values = string.Concat(values, ", @planPlatformId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `plan_platform_id` ");
                        values = string.Concat(values, " @planPlatformId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planPlatformId", subscription.PlanPlatformId);
                }
                if (!string.IsNullOrEmpty(subscription.Quantity))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `quantity` ");
                        values = string.Concat(values, ", @quantity");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `quantity` ");
                        values = string.Concat(values, " @quantity");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quantity", subscription.Quantity);
                }
                if (!string.IsNullOrEmpty(subscription.Status))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `status` ");
                        values = string.Concat(values, ", @status");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `status` ");
                        values = string.Concat(values, " @status");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@status", subscription.Status);
                }
                if (!string.IsNullOrEmpty(subscription.CurrentPeriodEnd))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `current_period_end` ");
                        values = string.Concat(values, ", @currentPeriodEnd");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `current_period_end` ");
                        values = string.Concat(values, " @currentPeriodEnd");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@currentPeriodEnd", subscription.CurrentPeriodEnd);
                }
                if (!string.IsNullOrEmpty(subscription.PlatformCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `platform_code` ");
                        values = string.Concat(values, ", @platformCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `platform_code` ");
                        values = string.Concat(values, " @platformCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@platformCode", subscription.PlatformCode);
                }
                if (!string.IsNullOrEmpty(subscription.Type))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `type` ");
                        values = string.Concat(values, ", @type");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `type` ");
                        values = string.Concat(values, " @type");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@type", subscription.Type);
                }
                if (!string.IsNullOrEmpty(subscription.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", subscription.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(subscription.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", subscription.CreatedAt);
                }
                if (!string.IsNullOrEmpty(subscription.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", subscription.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                subscription.Id = cmd.LastInsertedId.ToString();
                response.Add(subscription);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionModel>();
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
        public List<SubscriptionModel> Create(string connectionString, List<SubscriptionModel> subscriptionList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionModel> response = new List<SubscriptionModel>();
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
                        for (int i = 0; i < subscriptionList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_subscriptions (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(subscriptionList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, subscriptionList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].PlatformId))
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
                                    cmd.Parameters.AddWithValue("@platformId" + i, subscriptionList[i].PlatformId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].CustomerPlatformId))
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
                                    cmd.Parameters.AddWithValue("@customerPlatformId" + i, subscriptionList[i].CustomerPlatformId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].PlanPlatformId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `plan_platform_id` ");
                                        values = string.Concat(values, ", @planPlatformId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `plan_platform_id` ");
                                        values = string.Concat(values, "@planPlatformId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planPlatformId" + i, subscriptionList[i].PlanPlatformId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].Quantity))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `quantity` ");
                                        values = string.Concat(values, ", @quantity" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `quantity` ");
                                        values = string.Concat(values, "@quantity" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quantity" + i, subscriptionList[i].Quantity);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].Status))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `status` ");
                                        values = string.Concat(values, ", @status" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `status` ");
                                        values = string.Concat(values, "@status" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@status" + i, subscriptionList[i].Status);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].CurrentPeriodEnd))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `current_period_end` ");
                                        values = string.Concat(values, ", @currentPeriodEnd" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `current_period_end` ");
                                        values = string.Concat(values, "@currentPeriodEnd" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@currentPeriodEnd" + i, subscriptionList[i].CurrentPeriodEnd);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].PlatformCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `platform_code` ");
                                        values = string.Concat(values, ", @platformCode" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `platform_code` ");
                                        values = string.Concat(values, "@platformCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@platformCode" + i, subscriptionList[i].PlatformCode);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].Type))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `type` ");
                                        values = string.Concat(values, ", @type" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `type` ");
                                        values = string.Concat(values, "@type" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@type" + i, subscriptionList[i].Type);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(subscriptionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<SubscriptionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SubscriptionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionModel>();
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

        public List<SubscriptionModel> Update(string connectionString, SubscriptionModel subscription, SubscriptionModel subscriptionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionModel> response = new List<SubscriptionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE subst_subscriptions SET ";
                if (!string.IsNullOrEmpty(subscription.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", subscription.Id);
                }
                if (!string.IsNullOrEmpty(subscription.PlatformId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `platform_id` = @platformId");
                    else
                        sql = string.Concat(sql, " `platform_id` = @platformId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@platformId", subscription.PlatformId);
                }
                if (!string.IsNullOrEmpty(subscription.CustomerPlatformId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `customer_platform_id` = @customerPlatformId");
                    else
                        sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@customerPlatformId", subscription.CustomerPlatformId);
                }
                if (!string.IsNullOrEmpty(subscription.PlanPlatformId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `plan_platform_id` = @planPlatformId");
                    else
                        sql = string.Concat(sql, " `plan_platform_id` = @planPlatformId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planPlatformId", subscription.PlanPlatformId);
                }
                if (!string.IsNullOrEmpty(subscription.Quantity))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `quantity` = @quantity");
                    else
                        sql = string.Concat(sql, " `quantity` = @quantity");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quantity", subscription.Quantity);
                }
                if (!string.IsNullOrEmpty(subscription.Status))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `status` = @status");
                    else
                        sql = string.Concat(sql, " `status` = @status");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@status", subscription.Status);
                }
                if (!string.IsNullOrEmpty(subscription.CurrentPeriodEnd))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `current_period_end` = @currentPeriodEnd");
                    else
                        sql = string.Concat(sql, " `current_period_end` = @currentPeriodEnd");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@currentPeriodEnd", subscription.CurrentPeriodEnd);
                }
                if (!string.IsNullOrEmpty(subscription.PlatformCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `platform_code` = @platformCode");
                    else
                        sql = string.Concat(sql, " `platform_code` = @platformCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@platformCode", subscription.PlatformCode);
                }
                if (!string.IsNullOrEmpty(subscription.Type))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `type` = @type");
                    else
                        sql = string.Concat(sql, " `type` = @type");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@type", subscription.Type);
                }
                if (!string.IsNullOrEmpty(subscription.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", subscription.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(subscription.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", subscription.CreatedAt);
                }
                if (!string.IsNullOrEmpty(subscription.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", subscription.UpdatedAt);
                }
                if (subscriptionWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(subscriptionWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", subscriptionWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(subscriptionWhere.PlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform_id` = @platformIdWhere");
                        else
                            sql = string.Concat(sql, " `platform_id` = @platformIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformIdWhere", subscriptionWhere.PlatformId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionWhere.CustomerPlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `customer_platform_id` = @customerPlatformIdWhere");
                        else
                            sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@customerPlatformIdWhere", subscriptionWhere.CustomerPlatformId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionWhere.PlanPlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `plan_platform_id` = @planPlatformIdWhere");
                        else
                            sql = string.Concat(sql, " `plan_platform_id` = @planPlatformIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planPlatformIdWhere", subscriptionWhere.PlanPlatformId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionWhere.Quantity))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `quantity` = @quantityWhere");
                        else
                            sql = string.Concat(sql, " `quantity` = @quantityWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quantityWhere", subscriptionWhere.Quantity);
                    }
                    if (!string.IsNullOrEmpty(subscriptionWhere.Status))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `status` = @statusWhere");
                        else
                            sql = string.Concat(sql, " `status` = @statusWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@statusWhere", subscriptionWhere.Status);
                    }
                    if (!string.IsNullOrEmpty(subscriptionWhere.CurrentPeriodEnd))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `current_period_end` = @currentPeriodEndWhere");
                        else
                            sql = string.Concat(sql, " `current_period_end` = @currentPeriodEndWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@currentPeriodEndWhere", subscriptionWhere.CurrentPeriodEnd);
                    }
                    if (!string.IsNullOrEmpty(subscriptionWhere.PlatformCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform_code` = @platformCodeWhere");
                        else
                            sql = string.Concat(sql, " `platform_code` = @platformCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformCodeWhere", subscriptionWhere.PlatformCode);
                    }
                    if (!string.IsNullOrEmpty(subscriptionWhere.Type))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `type` = @typeWhere");
                        else
                            sql = string.Concat(sql, " `type` = @typeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@typeWhere", subscriptionWhere.Type);
                    }
                    if (!string.IsNullOrEmpty(subscriptionWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", subscriptionWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(subscriptionWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", subscriptionWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(subscriptionWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", subscriptionWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(subscription);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionModel>();
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

        public List<SubscriptionModel> Update(string connectionString, List<SubscriptionModel> subscriptionList, List<SubscriptionModel> subscriptionWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionModel> response = new List<SubscriptionModel>();
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
                        for (int i = 0; i < subscriptionList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_subscriptions SET ";
                                if (!string.IsNullOrEmpty(subscriptionList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, subscriptionList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].PlatformId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `platform_id` = @platformId" + i);
                                    else
                                        sql = string.Concat(sql, " `platform_id` = @platformId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@platformId" + i, subscriptionList[i].PlatformId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].CustomerPlatformId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `customer_platform_id` = @customerPlatformId" + i);
                                    else
                                        sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@customerPlatformId" + i, subscriptionList[i].CustomerPlatformId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].PlanPlatformId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `plan_platform_id` = @planPlatformId" + i);
                                    else
                                        sql = string.Concat(sql, " `plan_platform_id` = @planPlatformId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planPlatformId" + i, subscriptionList[i].PlanPlatformId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].Quantity))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `quantity` = @quantity" + i);
                                    else
                                        sql = string.Concat(sql, " `quantity` = @quantity" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quantity" + i, subscriptionList[i].Quantity);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].Status))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `status` = @status" + i);
                                    else
                                        sql = string.Concat(sql, " `status` = @status" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@status" + i, subscriptionList[i].Status);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].CurrentPeriodEnd))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `current_period_end` = @currentPeriodEnd" + i);
                                    else
                                        sql = string.Concat(sql, " `current_period_end` = @currentPeriodEnd" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@currentPeriodEnd" + i, subscriptionList[i].CurrentPeriodEnd);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].PlatformCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `platform_code` = @platformCode" + i);
                                    else
                                        sql = string.Concat(sql, " `platform_code` = @platformCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@platformCode" + i, subscriptionList[i].PlatformCode);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].Type))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `type` = @type" + i);
                                    else
                                        sql = string.Concat(sql, " `type` = @type" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@type" + i, subscriptionList[i].Type);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(subscriptionList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionList[i].UpdatedAt);
                                }
                                if (subscriptionWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(subscriptionWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, subscriptionWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionWhereList[i].PlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform_id` = @platformIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `platform_id` = @platformIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformIdWhere" + i, subscriptionWhereList[i].PlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionWhereList[i].CustomerPlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `customer_platform_id` = @customerPlatformIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@customerPlatformIdWhere" + i, subscriptionWhereList[i].CustomerPlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionWhereList[i].PlanPlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `plan_platform_id` = @planPlatformIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `plan_platform_id` = @planPlatformIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planPlatformIdWhere" + i, subscriptionWhereList[i].PlanPlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionWhereList[i].Quantity))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `quantity` = @quantityWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `quantity` = @quantityWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quantityWhere" + i, subscriptionWhereList[i].Quantity);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionWhereList[i].Status))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `status` = @statusWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `status` = @statusWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@statusWhere" + i, subscriptionWhereList[i].Status);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionWhereList[i].CurrentPeriodEnd))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `current_period_end` = @currentPeriodEndWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `current_period_end` = @currentPeriodEndWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@currentPeriodEndWhere" + i, subscriptionWhereList[i].CurrentPeriodEnd);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionWhereList[i].PlatformCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform_code` = @platformCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `platform_code` = @platformCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformCodeWhere" + i, subscriptionWhereList[i].PlatformCode);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionWhereList[i].Type))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `type` = @typeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `type` = @typeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@typeWhere" + i, subscriptionWhereList[i].Type);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, subscriptionWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, subscriptionWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, subscriptionWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(subscriptionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<SubscriptionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SubscriptionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionModel>();
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

        public List<SubscriptionModel> Delete(string connectionString, SubscriptionModel subscription)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionModel> response = new List<SubscriptionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM subst_subscriptions  ";
                if (subscription != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(subscription.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", subscription.Id);
                    }
                    if (!string.IsNullOrEmpty(subscription.PlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform_id` = @platformId");
                        else
                            sql = string.Concat(sql, " `platform_id` = @platformId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformId", subscription.PlatformId);
                    }
                    if (!string.IsNullOrEmpty(subscription.CustomerPlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `customer_platform_id` = @customerPlatformId");
                        else
                            sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@customerPlatformId", subscription.CustomerPlatformId);
                    }
                    if (!string.IsNullOrEmpty(subscription.PlanPlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `plan_platform_id` = @planPlatformId");
                        else
                            sql = string.Concat(sql, " `plan_platform_id` = @planPlatformId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planPlatformId", subscription.PlanPlatformId);
                    }
                    if (!string.IsNullOrEmpty(subscription.Quantity))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `quantity` = @quantity");
                        else
                            sql = string.Concat(sql, " `quantity` = @quantity");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quantity", subscription.Quantity);
                    }
                    if (!string.IsNullOrEmpty(subscription.Status))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `status` = @status");
                        else
                            sql = string.Concat(sql, " `status` = @status");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@status", subscription.Status);
                    }
                    if (!string.IsNullOrEmpty(subscription.CurrentPeriodEnd))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `current_period_end` = @currentPeriodEnd");
                        else
                            sql = string.Concat(sql, " `current_period_end` = @currentPeriodEnd");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@currentPeriodEnd", subscription.CurrentPeriodEnd);
                    }
                    if (!string.IsNullOrEmpty(subscription.PlatformCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform_code` = @platformCode");
                        else
                            sql = string.Concat(sql, " `platform_code` = @platformCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformCode", subscription.PlatformCode);
                    }
                    if (!string.IsNullOrEmpty(subscription.Type))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `type` = @type");
                        else
                            sql = string.Concat(sql, " `type` = @type");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@type", subscription.Type);
                    }
                    if (!string.IsNullOrEmpty(subscription.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", subscription.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(subscription.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", subscription.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(subscription.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", subscription.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(subscription);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionModel>();
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

        public List<SubscriptionModel> Delete(string connectionString, List<SubscriptionModel> subscription)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionModel> response = new List<SubscriptionModel>();
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
                        for (int i = 0; i < subscription.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_subscriptions  ";
                                if (subscription[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(subscription[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, subscription[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(subscription[i].PlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform_id` = @platformId" + i);
                                        else
                                            sql = string.Concat(sql, " `platform_id` = @platformId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformId" + i, subscription[i].PlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(subscription[i].CustomerPlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `customer_platform_id` = @customerPlatformId" + i);
                                        else
                                            sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@customerPlatformId" + i, subscription[i].CustomerPlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(subscription[i].PlanPlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `plan_platform_id` = @planPlatformId" + i);
                                        else
                                            sql = string.Concat(sql, " `plan_platform_id` = @planPlatformId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planPlatformId" + i, subscription[i].PlanPlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(subscription[i].Quantity))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `quantity` = @quantity" + i);
                                        else
                                            sql = string.Concat(sql, " `quantity` = @quantity" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quantity" + i, subscription[i].Quantity);
                                    }
                                    if (!string.IsNullOrEmpty(subscription[i].Status))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `status` = @status" + i);
                                        else
                                            sql = string.Concat(sql, " `status` = @status" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@status" + i, subscription[i].Status);
                                    }
                                    if (!string.IsNullOrEmpty(subscription[i].CurrentPeriodEnd))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `current_period_end` = @currentPeriodEnd" + i);
                                        else
                                            sql = string.Concat(sql, " `current_period_end` = @currentPeriodEnd" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@currentPeriodEnd" + i, subscription[i].CurrentPeriodEnd);
                                    }
                                    if (!string.IsNullOrEmpty(subscription[i].PlatformCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform_code` = @platformCode" + i);
                                        else
                                            sql = string.Concat(sql, " `platform_code` = @platformCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformCode" + i, subscription[i].PlatformCode);
                                    }
                                    if (!string.IsNullOrEmpty(subscription[i].Type))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `type` = @type" + i);
                                        else
                                            sql = string.Concat(sql, " `type` = @type" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@type" + i, subscription[i].Type);
                                    }
                                    if (!string.IsNullOrEmpty(subscription[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, subscription[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(subscription[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, subscription[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(subscription[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, subscription[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(subscription[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<SubscriptionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SubscriptionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionModel>();
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
