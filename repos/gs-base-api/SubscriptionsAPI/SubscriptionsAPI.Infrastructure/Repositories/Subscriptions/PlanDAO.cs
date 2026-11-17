using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class PlanDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, PlanModel> ReadD(string connectionString, PlanModel plan, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanModel> response = new Dictionary<string, PlanModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `platform_id`, `product_id`, `name`, `interval`, `interval_count`, `currency`, `amount`, `is_for_school`, `is_private`, `profiles_quantity`, `is_special_promo`, `special_promo_limit`, `special_promo_days`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_plans ";
                {
                    if (plan != null)
                    {
                        if (!string.IsNullOrEmpty(plan.Id))
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
                            cmd.Parameters.AddWithValue("@id", plan.Id);
                        }
                        if (!string.IsNullOrEmpty(plan.PlatformId))
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
                            cmd.Parameters.AddWithValue("@platformId", plan.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(plan.ProductId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `product_id` = @productId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `product_id` = @productId ");
                            }
                            cmd.Parameters.AddWithValue("@productId", plan.ProductId);
                        }
                        if (!string.IsNullOrEmpty(plan.Name))
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
                            cmd.Parameters.AddWithValue("@name", plan.Name);
                        }
                        if (!string.IsNullOrEmpty(plan.Interval))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `interval` = @interval ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `interval` = @interval ");
                            }
                            cmd.Parameters.AddWithValue("@interval", plan.Interval);
                        }
                        if (!string.IsNullOrEmpty(plan.IntervalCount))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `interval_count` = @intervalCount ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `interval_count` = @intervalCount ");
                            }
                            cmd.Parameters.AddWithValue("@intervalCount", plan.IntervalCount);
                        }
                        if (!string.IsNullOrEmpty(plan.Currency))
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
                            cmd.Parameters.AddWithValue("@currency", plan.Currency);
                        }
                        if (!string.IsNullOrEmpty(plan.Amount))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `amount` = @amount ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `amount` = @amount ");
                            }
                            cmd.Parameters.AddWithValue("@amount", plan.Amount);
                        }
                        if (!string.IsNullOrEmpty(plan.IsForSchool))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_for_school` = @isForSchool ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_for_school` = @isForSchool ");
                            }
                            cmd.Parameters.AddWithValue("@isForSchool", plan.IsForSchool);
                        }
                        if (!string.IsNullOrEmpty(plan.IsPrivate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_private` = @isPrivate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_private` = @isPrivate ");
                            }
                            cmd.Parameters.AddWithValue("@isPrivate", plan.IsPrivate);
                        }
                        if (!string.IsNullOrEmpty(plan.ProfilesQuantity))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `profiles_quantity` = @profilesQuantity ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `profiles_quantity` = @profilesQuantity ");
                            }
                            cmd.Parameters.AddWithValue("@profilesQuantity", plan.ProfilesQuantity);
                        }
                        if (!string.IsNullOrEmpty(plan.IsSpecialPromo))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_special_promo` = @isSpecialPromo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_special_promo` = @isSpecialPromo ");
                            }
                            cmd.Parameters.AddWithValue("@isSpecialPromo", plan.IsSpecialPromo);
                        }
                        if (!string.IsNullOrEmpty(plan.SpecialPromoLimit))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `special_promo_limit` = @specialPromoLimit ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `special_promo_limit` = @specialPromoLimit ");
                            }
                            cmd.Parameters.AddWithValue("@specialPromoLimit", plan.SpecialPromoLimit);
                        }
                        if (!string.IsNullOrEmpty(plan.SpecialPromoDays))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `special_promo_days` = @specialPromoDays ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `special_promo_days` = @specialPromoDays ");
                            }
                            cmd.Parameters.AddWithValue("@specialPromoDays", plan.SpecialPromoDays);
                        }
                        if (!string.IsNullOrEmpty(plan.Available))
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
                            cmd.Parameters.AddWithValue("@available", plan.Available);
                        }
                        if (!string.IsNullOrEmpty(plan.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", plan.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(plan.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", plan.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(plan.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", plan.UpdatedAt);
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
                                    plan.Id = reader.GetValue(0).ToString();
                                else
                                    plan.Id = null;
                                if (!reader.IsDBNull(1))
                                    plan.PlatformId = reader.GetValue(1).ToString();
                                else
                                    plan.PlatformId = null;
                                if (!reader.IsDBNull(2))
                                    plan.ProductId = reader.GetValue(2).ToString();
                                else
                                    plan.ProductId = null;
                                if (!reader.IsDBNull(3))
                                    plan.Name = reader.GetValue(3).ToString();
                                else
                                    plan.Name = null;
                                if (!reader.IsDBNull(4))
                                    plan.Interval = reader.GetValue(4).ToString();
                                else
                                    plan.Interval = null;
                                if (!reader.IsDBNull(5))
                                    plan.IntervalCount = reader.GetValue(5).ToString();
                                else
                                    plan.IntervalCount = null;
                                if (!reader.IsDBNull(6))
                                    plan.Currency = reader.GetValue(6).ToString();
                                else
                                    plan.Currency = null;
                                if (!reader.IsDBNull(7))
                                    plan.Amount = reader.GetValue(7).ToString();
                                else
                                    plan.Amount = null;
                                if (!reader.IsDBNull(8))
                                    plan.IsForSchool = reader.GetValue(8).ToString();
                                else
                                    plan.IsForSchool = null;
                                if (!reader.IsDBNull(9))
                                    plan.IsPrivate = reader.GetValue(9).ToString();
                                else
                                    plan.IsPrivate = null;
                                if (!reader.IsDBNull(10))
                                    plan.ProfilesQuantity = reader.GetValue(10).ToString();
                                else
                                    plan.ProfilesQuantity = null;
                                if (!reader.IsDBNull(11))
                                    plan.IsSpecialPromo = reader.GetValue(11).ToString();
                                else
                                    plan.IsSpecialPromo = null;
                                if (!reader.IsDBNull(12))
                                    plan.SpecialPromoLimit = reader.GetValue(12).ToString();
                                else
                                    plan.SpecialPromoLimit = null;
                                if (!reader.IsDBNull(13))
                                    plan.SpecialPromoDays = reader.GetValue(13).ToString();
                                else
                                    plan.SpecialPromoDays = null;
                                if (!reader.IsDBNull(14))
                                    plan.Available = reader.GetValue(14).ToString();
                                else
                                    plan.Available = null;
                                if (!reader.IsDBNull(15))
                                    plan.ModifiedBy = reader.GetValue(15).ToString();
                                else
                                    plan.ModifiedBy = null;
                                if (!reader.IsDBNull(16))
                                    plan.CreatedAt = reader.GetValue(16).ToString();
                                else
                                    plan.CreatedAt = null;
                                if (!reader.IsDBNull(17))
                                    plan.UpdatedAt = reader.GetValue(17).ToString();
                                else
                                    plan.UpdatedAt = null;
                                response.Add(plan.Id, plan);
                                plan = new PlanModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PlanModel>();
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
                response = new Dictionary<string, PlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlanModel> ReadD(string connectionString, PlanModel plan, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanModel> response = new Dictionary<string, PlanModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `platform_id`, `product_id`, `name`, `interval`, `interval_count`, `currency`, `amount`, `is_for_school`, `is_private`, `profiles_quantity`, `is_special_promo`, `special_promo_limit`, `special_promo_days`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_plans ";
                {
                    if (plan != null)
                    {
                        if (!string.IsNullOrEmpty(plan.Id))
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
                            cmd.Parameters.AddWithValue("@id", plan.Id);
                        }
                        if (!string.IsNullOrEmpty(plan.PlatformId))
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
                            cmd.Parameters.AddWithValue("@platformId", plan.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(plan.ProductId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `product_id` = @productId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `product_id` = @productId ");
                            }
                            cmd.Parameters.AddWithValue("@productId", plan.ProductId);
                        }
                        if (!string.IsNullOrEmpty(plan.Name))
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
                            cmd.Parameters.AddWithValue("@name", plan.Name);
                        }
                        if (!string.IsNullOrEmpty(plan.Interval))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `interval` = @interval ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `interval` = @interval ");
                            }
                            cmd.Parameters.AddWithValue("@interval", plan.Interval);
                        }
                        if (!string.IsNullOrEmpty(plan.IntervalCount))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `interval_count` = @intervalCount ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `interval_count` = @intervalCount ");
                            }
                            cmd.Parameters.AddWithValue("@intervalCount", plan.IntervalCount);
                        }
                        if (!string.IsNullOrEmpty(plan.Currency))
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
                            cmd.Parameters.AddWithValue("@currency", plan.Currency);
                        }
                        if (!string.IsNullOrEmpty(plan.Amount))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `amount` = @amount ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `amount` = @amount ");
                            }
                            cmd.Parameters.AddWithValue("@amount", plan.Amount);
                        }
                        if (!string.IsNullOrEmpty(plan.IsForSchool))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_for_school` = @isForSchool ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_for_school` = @isForSchool ");
                            }
                            cmd.Parameters.AddWithValue("@isForSchool", plan.IsForSchool);
                        }
                        if (!string.IsNullOrEmpty(plan.IsPrivate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_private` = @isPrivate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_private` = @isPrivate ");
                            }
                            cmd.Parameters.AddWithValue("@isPrivate", plan.IsPrivate);
                        }
                        if (!string.IsNullOrEmpty(plan.ProfilesQuantity))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `profiles_quantity` = @profilesQuantity ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `profiles_quantity` = @profilesQuantity ");
                            }
                            cmd.Parameters.AddWithValue("@profilesQuantity", plan.ProfilesQuantity);
                        }
                        if (!string.IsNullOrEmpty(plan.IsSpecialPromo))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_special_promo` = @isSpecialPromo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_special_promo` = @isSpecialPromo ");
                            }
                            cmd.Parameters.AddWithValue("@isSpecialPromo", plan.IsSpecialPromo);
                        }
                        if (!string.IsNullOrEmpty(plan.SpecialPromoLimit))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `special_promo_limit` = @specialPromoLimit ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `special_promo_limit` = @specialPromoLimit ");
                            }
                            cmd.Parameters.AddWithValue("@specialPromoLimit", plan.SpecialPromoLimit);
                        }
                        if (!string.IsNullOrEmpty(plan.SpecialPromoDays))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `special_promo_days` = @specialPromoDays ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `special_promo_days` = @specialPromoDays ");
                            }
                            cmd.Parameters.AddWithValue("@specialPromoDays", plan.SpecialPromoDays);
                        }
                        if (!string.IsNullOrEmpty(plan.Available))
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
                            cmd.Parameters.AddWithValue("@available", plan.Available);
                        }
                        if (!string.IsNullOrEmpty(plan.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", plan.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(plan.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", plan.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(plan.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", plan.UpdatedAt);
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
                                    plan.Id = reader.GetValue(0).ToString();
                                else
                                    plan.Id = null;
                                if (!reader.IsDBNull(1))
                                    plan.PlatformId = reader.GetValue(1).ToString();
                                else
                                    plan.PlatformId = null;
                                if (!reader.IsDBNull(2))
                                    plan.ProductId = reader.GetValue(2).ToString();
                                else
                                    plan.ProductId = null;
                                if (!reader.IsDBNull(3))
                                    plan.Name = reader.GetValue(3).ToString();
                                else
                                    plan.Name = null;
                                if (!reader.IsDBNull(4))
                                    plan.Interval = reader.GetValue(4).ToString();
                                else
                                    plan.Interval = null;
                                if (!reader.IsDBNull(5))
                                    plan.IntervalCount = reader.GetValue(5).ToString();
                                else
                                    plan.IntervalCount = null;
                                if (!reader.IsDBNull(6))
                                    plan.Currency = reader.GetValue(6).ToString();
                                else
                                    plan.Currency = null;
                                if (!reader.IsDBNull(7))
                                    plan.Amount = reader.GetValue(7).ToString();
                                else
                                    plan.Amount = null;
                                if (!reader.IsDBNull(8))
                                    plan.IsForSchool = reader.GetValue(8).ToString();
                                else
                                    plan.IsForSchool = null;
                                if (!reader.IsDBNull(9))
                                    plan.IsPrivate = reader.GetValue(9).ToString();
                                else
                                    plan.IsPrivate = null;
                                if (!reader.IsDBNull(10))
                                    plan.ProfilesQuantity = reader.GetValue(10).ToString();
                                else
                                    plan.ProfilesQuantity = null;
                                if (!reader.IsDBNull(11))
                                    plan.IsSpecialPromo = reader.GetValue(11).ToString();
                                else
                                    plan.IsSpecialPromo = null;
                                if (!reader.IsDBNull(12))
                                    plan.SpecialPromoLimit = reader.GetValue(12).ToString();
                                else
                                    plan.SpecialPromoLimit = null;
                                if (!reader.IsDBNull(13))
                                    plan.SpecialPromoDays = reader.GetValue(13).ToString();
                                else
                                    plan.SpecialPromoDays = null;
                                if (!reader.IsDBNull(14))
                                    plan.Available = reader.GetValue(14).ToString();
                                else
                                    plan.Available = null;
                                if (!reader.IsDBNull(15))
                                    plan.ModifiedBy = reader.GetValue(15).ToString();
                                else
                                    plan.ModifiedBy = null;
                                if (!reader.IsDBNull(16))
                                    plan.CreatedAt = reader.GetValue(16).ToString();
                                else
                                    plan.CreatedAt = null;
                                if (!reader.IsDBNull(17))
                                    plan.UpdatedAt = reader.GetValue(17).ToString();
                                else
                                    plan.UpdatedAt = null;
                                response.Add(plan.Id, plan);
                                plan = new PlanModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PlanModel>();
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
                response = new Dictionary<string, PlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlanModel> ReadD(string connectionString, List<PlanModel> planWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanModel> response = new Dictionary<string, PlanModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `platform_id`, `product_id`, `name`, `interval`, `interval_count`, `currency`, `amount`, `is_for_school`, `is_private`, `profiles_quantity`, `is_special_promo`, `special_promo_limit`, `special_promo_days`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_plans ";
                if (planWhere != null && planWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(planWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, planWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].ProductId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `product_id` = @productId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@productId" + i, planWhere[i].ProductId);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, planWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Interval))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `interval` = @interval" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@interval" + i, planWhere[i].Interval);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IntervalCount))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `interval_count` = @intervalCount" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@intervalCount" + i, planWhere[i].IntervalCount);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Currency))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `currency` = @currency" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currency" + i, planWhere[i].Currency);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Amount))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `amount` = @amount" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@amount" + i, planWhere[i].Amount);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IsForSchool))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_for_school` = @isForSchool" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isForSchool" + i, planWhere[i].IsForSchool);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IsPrivate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_private` = @isPrivate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isPrivate" + i, planWhere[i].IsPrivate);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].ProfilesQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `profiles_quantity` = @profilesQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@profilesQuantity" + i, planWhere[i].ProfilesQuantity);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IsSpecialPromo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_special_promo` = @isSpecialPromo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isSpecialPromo" + i, planWhere[i].IsSpecialPromo);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].SpecialPromoLimit))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `special_promo_limit` = @specialPromoLimit" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@specialPromoLimit" + i, planWhere[i].SpecialPromoLimit);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].SpecialPromoDays))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `special_promo_days` = @specialPromoDays" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@specialPromoDays" + i, planWhere[i].SpecialPromoDays);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, planWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planWhere.Count - 1)
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
                            PlanModel plan = new PlanModel(); if (!reader.IsDBNull(0))
                                plan.Id = reader.GetValue(0).ToString();
                            else
                                plan.Id = null;
                            if (!reader.IsDBNull(1))
                                plan.PlatformId = reader.GetValue(1).ToString();
                            else
                                plan.PlatformId = null;
                            if (!reader.IsDBNull(2))
                                plan.ProductId = reader.GetValue(2).ToString();
                            else
                                plan.ProductId = null;
                            if (!reader.IsDBNull(3))
                                plan.Name = reader.GetValue(3).ToString();
                            else
                                plan.Name = null;
                            if (!reader.IsDBNull(4))
                                plan.Interval = reader.GetValue(4).ToString();
                            else
                                plan.Interval = null;
                            if (!reader.IsDBNull(5))
                                plan.IntervalCount = reader.GetValue(5).ToString();
                            else
                                plan.IntervalCount = null;
                            if (!reader.IsDBNull(6))
                                plan.Currency = reader.GetValue(6).ToString();
                            else
                                plan.Currency = null;
                            if (!reader.IsDBNull(7))
                                plan.Amount = reader.GetValue(7).ToString();
                            else
                                plan.Amount = null;
                            if (!reader.IsDBNull(8))
                                plan.IsForSchool = reader.GetValue(8).ToString();
                            else
                                plan.IsForSchool = null;
                            if (!reader.IsDBNull(9))
                                plan.IsPrivate = reader.GetValue(9).ToString();
                            else
                                plan.IsPrivate = null;
                            if (!reader.IsDBNull(10))
                                plan.ProfilesQuantity = reader.GetValue(10).ToString();
                            else
                                plan.ProfilesQuantity = null;
                            if (!reader.IsDBNull(11))
                                plan.IsSpecialPromo = reader.GetValue(11).ToString();
                            else
                                plan.IsSpecialPromo = null;
                            if (!reader.IsDBNull(12))
                                plan.SpecialPromoLimit = reader.GetValue(12).ToString();
                            else
                                plan.SpecialPromoLimit = null;
                            if (!reader.IsDBNull(13))
                                plan.SpecialPromoDays = reader.GetValue(13).ToString();
                            else
                                plan.SpecialPromoDays = null;
                            if (!reader.IsDBNull(14))
                                plan.Available = reader.GetValue(14).ToString();
                            else
                                plan.Available = null;
                            if (!reader.IsDBNull(15))
                                plan.ModifiedBy = reader.GetValue(15).ToString();
                            else
                                plan.ModifiedBy = null;
                            if (!reader.IsDBNull(16))
                                plan.CreatedAt = reader.GetValue(16).ToString();
                            else
                                plan.CreatedAt = null;
                            if (!reader.IsDBNull(17))
                                plan.UpdatedAt = reader.GetValue(17).ToString();
                            else
                                plan.UpdatedAt = null;
                            response.Add(plan.Id, plan);
                            plan = new PlanModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PlanModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlanModel> ReadD(string connectionString, List<PlanModel> planWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanModel> response = new Dictionary<string, PlanModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `platform_id`, `product_id`, `name`, `interval`, `interval_count`, `currency`, `amount`, `is_for_school`, `is_private`, `profiles_quantity`, `is_special_promo`, `special_promo_limit`, `special_promo_days`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_plans ";
                if (planWhere != null && planWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(planWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, planWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].ProductId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `product_id` = @productId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@productId" + i, planWhere[i].ProductId);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, planWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Interval))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `interval` = @interval" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@interval" + i, planWhere[i].Interval);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IntervalCount))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `interval_count` = @intervalCount" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@intervalCount" + i, planWhere[i].IntervalCount);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Currency))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `currency` = @currency" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currency" + i, planWhere[i].Currency);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Amount))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `amount` = @amount" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@amount" + i, planWhere[i].Amount);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IsForSchool))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_for_school` = @isForSchool" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isForSchool" + i, planWhere[i].IsForSchool);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IsPrivate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_private` = @isPrivate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isPrivate" + i, planWhere[i].IsPrivate);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].ProfilesQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `profiles_quantity` = @profilesQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@profilesQuantity" + i, planWhere[i].ProfilesQuantity);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IsSpecialPromo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_special_promo` = @isSpecialPromo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isSpecialPromo" + i, planWhere[i].IsSpecialPromo);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].SpecialPromoLimit))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `special_promo_limit` = @specialPromoLimit" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@specialPromoLimit" + i, planWhere[i].SpecialPromoLimit);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].SpecialPromoDays))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `special_promo_days` = @specialPromoDays" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@specialPromoDays" + i, planWhere[i].SpecialPromoDays);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, planWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planWhere.Count - 1)
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
                            PlanModel plan = new PlanModel(); if (!reader.IsDBNull(0))
                                plan.Id = reader.GetValue(0).ToString();
                            else
                                plan.Id = null;
                            if (!reader.IsDBNull(1))
                                plan.PlatformId = reader.GetValue(1).ToString();
                            else
                                plan.PlatformId = null;
                            if (!reader.IsDBNull(2))
                                plan.ProductId = reader.GetValue(2).ToString();
                            else
                                plan.ProductId = null;
                            if (!reader.IsDBNull(3))
                                plan.Name = reader.GetValue(3).ToString();
                            else
                                plan.Name = null;
                            if (!reader.IsDBNull(4))
                                plan.Interval = reader.GetValue(4).ToString();
                            else
                                plan.Interval = null;
                            if (!reader.IsDBNull(5))
                                plan.IntervalCount = reader.GetValue(5).ToString();
                            else
                                plan.IntervalCount = null;
                            if (!reader.IsDBNull(6))
                                plan.Currency = reader.GetValue(6).ToString();
                            else
                                plan.Currency = null;
                            if (!reader.IsDBNull(7))
                                plan.Amount = reader.GetValue(7).ToString();
                            else
                                plan.Amount = null;
                            if (!reader.IsDBNull(8))
                                plan.IsForSchool = reader.GetValue(8).ToString();
                            else
                                plan.IsForSchool = null;
                            if (!reader.IsDBNull(9))
                                plan.IsPrivate = reader.GetValue(9).ToString();
                            else
                                plan.IsPrivate = null;
                            if (!reader.IsDBNull(10))
                                plan.ProfilesQuantity = reader.GetValue(10).ToString();
                            else
                                plan.ProfilesQuantity = null;
                            if (!reader.IsDBNull(11))
                                plan.IsSpecialPromo = reader.GetValue(11).ToString();
                            else
                                plan.IsSpecialPromo = null;
                            if (!reader.IsDBNull(12))
                                plan.SpecialPromoLimit = reader.GetValue(12).ToString();
                            else
                                plan.SpecialPromoLimit = null;
                            if (!reader.IsDBNull(13))
                                plan.SpecialPromoDays = reader.GetValue(13).ToString();
                            else
                                plan.SpecialPromoDays = null;
                            if (!reader.IsDBNull(14))
                                plan.Available = reader.GetValue(14).ToString();
                            else
                                plan.Available = null;
                            if (!reader.IsDBNull(15))
                                plan.ModifiedBy = reader.GetValue(15).ToString();
                            else
                                plan.ModifiedBy = null;
                            if (!reader.IsDBNull(16))
                                plan.CreatedAt = reader.GetValue(16).ToString();
                            else
                                plan.CreatedAt = null;
                            if (!reader.IsDBNull(17))
                                plan.UpdatedAt = reader.GetValue(17).ToString();
                            else
                                plan.UpdatedAt = null;
                            response.Add(plan.Id, plan);
                            plan = new PlanModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PlanModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanModel> Read(string connectionString, PlanModel plan, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanModel> response = new List<PlanModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `platform_id`, `product_id`, `name`, `interval`, `interval_count`, `currency`, `amount`, `is_for_school`, `is_private`, `profiles_quantity`, `is_special_promo`, `special_promo_limit`, `special_promo_days`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_plans ";
                {
                    if (plan != null)
                    {
                        if (!string.IsNullOrEmpty(plan.Id))
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
                            cmd.Parameters.AddWithValue("@id", plan.Id);
                        }
                        if (!string.IsNullOrEmpty(plan.PlatformId))
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
                            cmd.Parameters.AddWithValue("@platformId", plan.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(plan.ProductId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `product_id` = @productId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `product_id` = @productId ");
                            }
                            cmd.Parameters.AddWithValue("@productId", plan.ProductId);
                        }
                        if (!string.IsNullOrEmpty(plan.Name))
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
                            cmd.Parameters.AddWithValue("@name", plan.Name);
                        }
                        if (!string.IsNullOrEmpty(plan.Interval))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `interval` = @interval ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `interval` = @interval ");
                            }
                            cmd.Parameters.AddWithValue("@interval", plan.Interval);
                        }
                        if (!string.IsNullOrEmpty(plan.IntervalCount))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `interval_count` = @intervalCount ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `interval_count` = @intervalCount ");
                            }
                            cmd.Parameters.AddWithValue("@intervalCount", plan.IntervalCount);
                        }
                        if (!string.IsNullOrEmpty(plan.Currency))
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
                            cmd.Parameters.AddWithValue("@currency", plan.Currency);
                        }
                        if (!string.IsNullOrEmpty(plan.Amount))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `amount` = @amount ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `amount` = @amount ");
                            }
                            cmd.Parameters.AddWithValue("@amount", plan.Amount);
                        }
                        if (!string.IsNullOrEmpty(plan.IsForSchool))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_for_school` = @isForSchool ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_for_school` = @isForSchool ");
                            }
                            cmd.Parameters.AddWithValue("@isForSchool", plan.IsForSchool);
                        }
                        if (!string.IsNullOrEmpty(plan.IsPrivate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_private` = @isPrivate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_private` = @isPrivate ");
                            }
                            cmd.Parameters.AddWithValue("@isPrivate", plan.IsPrivate);
                        }
                        if (!string.IsNullOrEmpty(plan.ProfilesQuantity))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `profiles_quantity` = @profilesQuantity ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `profiles_quantity` = @profilesQuantity ");
                            }
                            cmd.Parameters.AddWithValue("@profilesQuantity", plan.ProfilesQuantity);
                        }
                        if (!string.IsNullOrEmpty(plan.IsSpecialPromo))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_special_promo` = @isSpecialPromo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_special_promo` = @isSpecialPromo ");
                            }
                            cmd.Parameters.AddWithValue("@isSpecialPromo", plan.IsSpecialPromo);
                        }
                        if (!string.IsNullOrEmpty(plan.SpecialPromoLimit))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `special_promo_limit` = @specialPromoLimit ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `special_promo_limit` = @specialPromoLimit ");
                            }
                            cmd.Parameters.AddWithValue("@specialPromoLimit", plan.SpecialPromoLimit);
                        }
                        if (!string.IsNullOrEmpty(plan.SpecialPromoDays))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `special_promo_days` = @specialPromoDays ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `special_promo_days` = @specialPromoDays ");
                            }
                            cmd.Parameters.AddWithValue("@specialPromoDays", plan.SpecialPromoDays);
                        }
                        if (!string.IsNullOrEmpty(plan.Available))
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
                            cmd.Parameters.AddWithValue("@available", plan.Available);
                        }
                        if (!string.IsNullOrEmpty(plan.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", plan.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(plan.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", plan.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(plan.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", plan.UpdatedAt);
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
                                    plan.Id = reader.GetValue(0).ToString();
                                else
                                    plan.Id = null;
                                if (!reader.IsDBNull(1))
                                    plan.PlatformId = reader.GetValue(1).ToString();
                                else
                                    plan.PlatformId = null;
                                if (!reader.IsDBNull(2))
                                    plan.ProductId = reader.GetValue(2).ToString();
                                else
                                    plan.ProductId = null;
                                if (!reader.IsDBNull(3))
                                    plan.Name = reader.GetValue(3).ToString();
                                else
                                    plan.Name = null;
                                if (!reader.IsDBNull(4))
                                    plan.Interval = reader.GetValue(4).ToString();
                                else
                                    plan.Interval = null;
                                if (!reader.IsDBNull(5))
                                    plan.IntervalCount = reader.GetValue(5).ToString();
                                else
                                    plan.IntervalCount = null;
                                if (!reader.IsDBNull(6))
                                    plan.Currency = reader.GetValue(6).ToString();
                                else
                                    plan.Currency = null;
                                if (!reader.IsDBNull(7))
                                    plan.Amount = reader.GetValue(7).ToString();
                                else
                                    plan.Amount = null;
                                if (!reader.IsDBNull(8))
                                    plan.IsForSchool = reader.GetValue(8).ToString();
                                else
                                    plan.IsForSchool = null;
                                if (!reader.IsDBNull(9))
                                    plan.IsPrivate = reader.GetValue(9).ToString();
                                else
                                    plan.IsPrivate = null;
                                if (!reader.IsDBNull(10))
                                    plan.ProfilesQuantity = reader.GetValue(10).ToString();
                                else
                                    plan.ProfilesQuantity = null;
                                if (!reader.IsDBNull(11))
                                    plan.IsSpecialPromo = reader.GetValue(11).ToString();
                                else
                                    plan.IsSpecialPromo = null;
                                if (!reader.IsDBNull(12))
                                    plan.SpecialPromoLimit = reader.GetValue(12).ToString();
                                else
                                    plan.SpecialPromoLimit = null;
                                if (!reader.IsDBNull(13))
                                    plan.SpecialPromoDays = reader.GetValue(13).ToString();
                                else
                                    plan.SpecialPromoDays = null;
                                if (!reader.IsDBNull(14))
                                    plan.Available = reader.GetValue(14).ToString();
                                else
                                    plan.Available = null;
                                if (!reader.IsDBNull(15))
                                    plan.ModifiedBy = reader.GetValue(15).ToString();
                                else
                                    plan.ModifiedBy = null;
                                if (!reader.IsDBNull(16))
                                    plan.CreatedAt = reader.GetValue(16).ToString();
                                else
                                    plan.CreatedAt = null;
                                if (!reader.IsDBNull(17))
                                    plan.UpdatedAt = reader.GetValue(17).ToString();
                                else
                                    plan.UpdatedAt = null;
                                response.Add(plan);
                                plan = new PlanModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlanModel>();
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
                response = new List<PlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanModel> Read(string connectionString, PlanModel plan, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanModel> response = new List<PlanModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `platform_id`, `product_id`, `name`, `interval`, `interval_count`, `currency`, `amount`, `is_for_school`, `is_private`, `profiles_quantity`, `is_special_promo`, `special_promo_limit`, `special_promo_days`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_plans ";
                {
                    if (plan != null)
                    {
                        if (!string.IsNullOrEmpty(plan.Id))
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
                            cmd.Parameters.AddWithValue("@id", plan.Id);
                        }
                        if (!string.IsNullOrEmpty(plan.PlatformId))
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
                            cmd.Parameters.AddWithValue("@platformId", plan.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(plan.ProductId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `product_id` = @productId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `product_id` = @productId ");
                            }
                            cmd.Parameters.AddWithValue("@productId", plan.ProductId);
                        }
                        if (!string.IsNullOrEmpty(plan.Name))
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
                            cmd.Parameters.AddWithValue("@name", plan.Name);
                        }
                        if (!string.IsNullOrEmpty(plan.Interval))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `interval` = @interval ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `interval` = @interval ");
                            }
                            cmd.Parameters.AddWithValue("@interval", plan.Interval);
                        }
                        if (!string.IsNullOrEmpty(plan.IntervalCount))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `interval_count` = @intervalCount ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `interval_count` = @intervalCount ");
                            }
                            cmd.Parameters.AddWithValue("@intervalCount", plan.IntervalCount);
                        }
                        if (!string.IsNullOrEmpty(plan.Currency))
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
                            cmd.Parameters.AddWithValue("@currency", plan.Currency);
                        }
                        if (!string.IsNullOrEmpty(plan.Amount))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `amount` = @amount ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `amount` = @amount ");
                            }
                            cmd.Parameters.AddWithValue("@amount", plan.Amount);
                        }
                        if (!string.IsNullOrEmpty(plan.IsForSchool))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_for_school` = @isForSchool ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_for_school` = @isForSchool ");
                            }
                            cmd.Parameters.AddWithValue("@isForSchool", plan.IsForSchool);
                        }
                        if (!string.IsNullOrEmpty(plan.IsPrivate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_private` = @isPrivate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_private` = @isPrivate ");
                            }
                            cmd.Parameters.AddWithValue("@isPrivate", plan.IsPrivate);
                        }
                        if (!string.IsNullOrEmpty(plan.ProfilesQuantity))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `profiles_quantity` = @profilesQuantity ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `profiles_quantity` = @profilesQuantity ");
                            }
                            cmd.Parameters.AddWithValue("@profilesQuantity", plan.ProfilesQuantity);
                        }
                        if (!string.IsNullOrEmpty(plan.IsSpecialPromo))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_special_promo` = @isSpecialPromo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_special_promo` = @isSpecialPromo ");
                            }
                            cmd.Parameters.AddWithValue("@isSpecialPromo", plan.IsSpecialPromo);
                        }
                        if (!string.IsNullOrEmpty(plan.SpecialPromoLimit))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `special_promo_limit` = @specialPromoLimit ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `special_promo_limit` = @specialPromoLimit ");
                            }
                            cmd.Parameters.AddWithValue("@specialPromoLimit", plan.SpecialPromoLimit);
                        }
                        if (!string.IsNullOrEmpty(plan.SpecialPromoDays))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `special_promo_days` = @specialPromoDays ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `special_promo_days` = @specialPromoDays ");
                            }
                            cmd.Parameters.AddWithValue("@specialPromoDays", plan.SpecialPromoDays);
                        }
                        if (!string.IsNullOrEmpty(plan.Available))
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
                            cmd.Parameters.AddWithValue("@available", plan.Available);
                        }
                        if (!string.IsNullOrEmpty(plan.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", plan.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(plan.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", plan.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(plan.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", plan.UpdatedAt);
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
                                    plan.Id = reader.GetValue(0).ToString();
                                else
                                    plan.Id = null;
                                if (!reader.IsDBNull(1))
                                    plan.PlatformId = reader.GetValue(1).ToString();
                                else
                                    plan.PlatformId = null;
                                if (!reader.IsDBNull(2))
                                    plan.ProductId = reader.GetValue(2).ToString();
                                else
                                    plan.ProductId = null;
                                if (!reader.IsDBNull(3))
                                    plan.Name = reader.GetValue(3).ToString();
                                else
                                    plan.Name = null;
                                if (!reader.IsDBNull(4))
                                    plan.Interval = reader.GetValue(4).ToString();
                                else
                                    plan.Interval = null;
                                if (!reader.IsDBNull(5))
                                    plan.IntervalCount = reader.GetValue(5).ToString();
                                else
                                    plan.IntervalCount = null;
                                if (!reader.IsDBNull(6))
                                    plan.Currency = reader.GetValue(6).ToString();
                                else
                                    plan.Currency = null;
                                if (!reader.IsDBNull(7))
                                    plan.Amount = reader.GetValue(7).ToString();
                                else
                                    plan.Amount = null;
                                if (!reader.IsDBNull(8))
                                    plan.IsForSchool = reader.GetValue(8).ToString();
                                else
                                    plan.IsForSchool = null;
                                if (!reader.IsDBNull(9))
                                    plan.IsPrivate = reader.GetValue(9).ToString();
                                else
                                    plan.IsPrivate = null;
                                if (!reader.IsDBNull(10))
                                    plan.ProfilesQuantity = reader.GetValue(10).ToString();
                                else
                                    plan.ProfilesQuantity = null;
                                if (!reader.IsDBNull(11))
                                    plan.IsSpecialPromo = reader.GetValue(11).ToString();
                                else
                                    plan.IsSpecialPromo = null;
                                if (!reader.IsDBNull(12))
                                    plan.SpecialPromoLimit = reader.GetValue(12).ToString();
                                else
                                    plan.SpecialPromoLimit = null;
                                if (!reader.IsDBNull(13))
                                    plan.SpecialPromoDays = reader.GetValue(13).ToString();
                                else
                                    plan.SpecialPromoDays = null;
                                if (!reader.IsDBNull(14))
                                    plan.Available = reader.GetValue(14).ToString();
                                else
                                    plan.Available = null;
                                if (!reader.IsDBNull(15))
                                    plan.ModifiedBy = reader.GetValue(15).ToString();
                                else
                                    plan.ModifiedBy = null;
                                if (!reader.IsDBNull(16))
                                    plan.CreatedAt = reader.GetValue(16).ToString();
                                else
                                    plan.CreatedAt = null;
                                if (!reader.IsDBNull(17))
                                    plan.UpdatedAt = reader.GetValue(17).ToString();
                                else
                                    plan.UpdatedAt = null;
                                response.Add(plan);
                                plan = new PlanModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlanModel>();
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
                response = new List<PlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanModel> Read(string connectionString, List<PlanModel> planWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanModel> response = new List<PlanModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `platform_id`, `product_id`, `name`, `interval`, `interval_count`, `currency`, `amount`, `is_for_school`, `is_private`, `profiles_quantity`, `is_special_promo`, `special_promo_limit`, `special_promo_days`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_plans ";
                if (planWhere != null && planWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(planWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, planWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].ProductId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `product_id` = @productId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@productId" + i, planWhere[i].ProductId);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, planWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Interval))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `interval` = @interval" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@interval" + i, planWhere[i].Interval);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IntervalCount))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `interval_count` = @intervalCount" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@intervalCount" + i, planWhere[i].IntervalCount);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Currency))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `currency` = @currency" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currency" + i, planWhere[i].Currency);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Amount))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `amount` = @amount" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@amount" + i, planWhere[i].Amount);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IsForSchool))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_for_school` = @isForSchool" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isForSchool" + i, planWhere[i].IsForSchool);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IsPrivate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_private` = @isPrivate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isPrivate" + i, planWhere[i].IsPrivate);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].ProfilesQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `profiles_quantity` = @profilesQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@profilesQuantity" + i, planWhere[i].ProfilesQuantity);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IsSpecialPromo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_special_promo` = @isSpecialPromo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isSpecialPromo" + i, planWhere[i].IsSpecialPromo);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].SpecialPromoLimit))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `special_promo_limit` = @specialPromoLimit" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@specialPromoLimit" + i, planWhere[i].SpecialPromoLimit);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].SpecialPromoDays))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `special_promo_days` = @specialPromoDays" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@specialPromoDays" + i, planWhere[i].SpecialPromoDays);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, planWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planWhere.Count - 1)
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
                            PlanModel plan = new PlanModel(); if (!reader.IsDBNull(0))
                                plan.Id = reader.GetValue(0).ToString();
                            else
                                plan.Id = null;
                            if (!reader.IsDBNull(1))
                                plan.PlatformId = reader.GetValue(1).ToString();
                            else
                                plan.PlatformId = null;
                            if (!reader.IsDBNull(2))
                                plan.ProductId = reader.GetValue(2).ToString();
                            else
                                plan.ProductId = null;
                            if (!reader.IsDBNull(3))
                                plan.Name = reader.GetValue(3).ToString();
                            else
                                plan.Name = null;
                            if (!reader.IsDBNull(4))
                                plan.Interval = reader.GetValue(4).ToString();
                            else
                                plan.Interval = null;
                            if (!reader.IsDBNull(5))
                                plan.IntervalCount = reader.GetValue(5).ToString();
                            else
                                plan.IntervalCount = null;
                            if (!reader.IsDBNull(6))
                                plan.Currency = reader.GetValue(6).ToString();
                            else
                                plan.Currency = null;
                            if (!reader.IsDBNull(7))
                                plan.Amount = reader.GetValue(7).ToString();
                            else
                                plan.Amount = null;
                            if (!reader.IsDBNull(8))
                                plan.IsForSchool = reader.GetValue(8).ToString();
                            else
                                plan.IsForSchool = null;
                            if (!reader.IsDBNull(9))
                                plan.IsPrivate = reader.GetValue(9).ToString();
                            else
                                plan.IsPrivate = null;
                            if (!reader.IsDBNull(10))
                                plan.ProfilesQuantity = reader.GetValue(10).ToString();
                            else
                                plan.ProfilesQuantity = null;
                            if (!reader.IsDBNull(11))
                                plan.IsSpecialPromo = reader.GetValue(11).ToString();
                            else
                                plan.IsSpecialPromo = null;
                            if (!reader.IsDBNull(12))
                                plan.SpecialPromoLimit = reader.GetValue(12).ToString();
                            else
                                plan.SpecialPromoLimit = null;
                            if (!reader.IsDBNull(13))
                                plan.SpecialPromoDays = reader.GetValue(13).ToString();
                            else
                                plan.SpecialPromoDays = null;
                            if (!reader.IsDBNull(14))
                                plan.Available = reader.GetValue(14).ToString();
                            else
                                plan.Available = null;
                            if (!reader.IsDBNull(15))
                                plan.ModifiedBy = reader.GetValue(15).ToString();
                            else
                                plan.ModifiedBy = null;
                            if (!reader.IsDBNull(16))
                                plan.CreatedAt = reader.GetValue(16).ToString();
                            else
                                plan.CreatedAt = null;
                            if (!reader.IsDBNull(17))
                                plan.UpdatedAt = reader.GetValue(17).ToString();
                            else
                                plan.UpdatedAt = null;
                            response.Add(plan);
                            plan = new PlanModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlanModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanModel> Read(string connectionString, List<PlanModel> planWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanModel> response = new List<PlanModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `platform_id`, `product_id`, `name`, `interval`, `interval_count`, `currency`, `amount`, `is_for_school`, `is_private`, `profiles_quantity`, `is_special_promo`, `special_promo_limit`, `special_promo_days`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_plans ";
                if (planWhere != null && planWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(planWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, planWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].ProductId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `product_id` = @productId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@productId" + i, planWhere[i].ProductId);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, planWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Interval))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `interval` = @interval" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@interval" + i, planWhere[i].Interval);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IntervalCount))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `interval_count` = @intervalCount" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@intervalCount" + i, planWhere[i].IntervalCount);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Currency))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `currency` = @currency" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@currency" + i, planWhere[i].Currency);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Amount))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `amount` = @amount" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@amount" + i, planWhere[i].Amount);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IsForSchool))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_for_school` = @isForSchool" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isForSchool" + i, planWhere[i].IsForSchool);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IsPrivate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_private` = @isPrivate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isPrivate" + i, planWhere[i].IsPrivate);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].ProfilesQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `profiles_quantity` = @profilesQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@profilesQuantity" + i, planWhere[i].ProfilesQuantity);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].IsSpecialPromo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_special_promo` = @isSpecialPromo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isSpecialPromo" + i, planWhere[i].IsSpecialPromo);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].SpecialPromoLimit))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `special_promo_limit` = @specialPromoLimit" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@specialPromoLimit" + i, planWhere[i].SpecialPromoLimit);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].SpecialPromoDays))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `special_promo_days` = @specialPromoDays" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@specialPromoDays" + i, planWhere[i].SpecialPromoDays);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, planWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(planWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planWhere.Count - 1)
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
                            PlanModel plan = new PlanModel(); if (!reader.IsDBNull(0))
                                plan.Id = reader.GetValue(0).ToString();
                            else
                                plan.Id = null;
                            if (!reader.IsDBNull(1))
                                plan.PlatformId = reader.GetValue(1).ToString();
                            else
                                plan.PlatformId = null;
                            if (!reader.IsDBNull(2))
                                plan.ProductId = reader.GetValue(2).ToString();
                            else
                                plan.ProductId = null;
                            if (!reader.IsDBNull(3))
                                plan.Name = reader.GetValue(3).ToString();
                            else
                                plan.Name = null;
                            if (!reader.IsDBNull(4))
                                plan.Interval = reader.GetValue(4).ToString();
                            else
                                plan.Interval = null;
                            if (!reader.IsDBNull(5))
                                plan.IntervalCount = reader.GetValue(5).ToString();
                            else
                                plan.IntervalCount = null;
                            if (!reader.IsDBNull(6))
                                plan.Currency = reader.GetValue(6).ToString();
                            else
                                plan.Currency = null;
                            if (!reader.IsDBNull(7))
                                plan.Amount = reader.GetValue(7).ToString();
                            else
                                plan.Amount = null;
                            if (!reader.IsDBNull(8))
                                plan.IsForSchool = reader.GetValue(8).ToString();
                            else
                                plan.IsForSchool = null;
                            if (!reader.IsDBNull(9))
                                plan.IsPrivate = reader.GetValue(9).ToString();
                            else
                                plan.IsPrivate = null;
                            if (!reader.IsDBNull(10))
                                plan.ProfilesQuantity = reader.GetValue(10).ToString();
                            else
                                plan.ProfilesQuantity = null;
                            if (!reader.IsDBNull(11))
                                plan.IsSpecialPromo = reader.GetValue(11).ToString();
                            else
                                plan.IsSpecialPromo = null;
                            if (!reader.IsDBNull(12))
                                plan.SpecialPromoLimit = reader.GetValue(12).ToString();
                            else
                                plan.SpecialPromoLimit = null;
                            if (!reader.IsDBNull(13))
                                plan.SpecialPromoDays = reader.GetValue(13).ToString();
                            else
                                plan.SpecialPromoDays = null;
                            if (!reader.IsDBNull(14))
                                plan.Available = reader.GetValue(14).ToString();
                            else
                                plan.Available = null;
                            if (!reader.IsDBNull(15))
                                plan.ModifiedBy = reader.GetValue(15).ToString();
                            else
                                plan.ModifiedBy = null;
                            if (!reader.IsDBNull(16))
                                plan.CreatedAt = reader.GetValue(16).ToString();
                            else
                                plan.CreatedAt = null;
                            if (!reader.IsDBNull(17))
                                plan.UpdatedAt = reader.GetValue(17).ToString();
                            else
                                plan.UpdatedAt = null;
                            response.Add(plan);
                            plan = new PlanModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlanModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanModel> Create(string connectionString, PlanModel plan)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanModel> response = new List<PlanModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO subst_plans (";
                string values = "(";
                if (!string.IsNullOrEmpty(plan.Id))
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
                    cmd.Parameters.AddWithValue("@id", plan.Id);
                }
                if (!string.IsNullOrEmpty(plan.PlatformId))
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
                    cmd.Parameters.AddWithValue("@platformId", plan.PlatformId);
                }
                if (!string.IsNullOrEmpty(plan.ProductId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `product_id` ");
                        values = string.Concat(values, ", @productId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `product_id` ");
                        values = string.Concat(values, " @productId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@productId", plan.ProductId);
                }
                if (!string.IsNullOrEmpty(plan.Name))
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
                    cmd.Parameters.AddWithValue("@name", plan.Name);
                }
                if (!string.IsNullOrEmpty(plan.Interval))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `interval` ");
                        values = string.Concat(values, ", @interval");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `interval` ");
                        values = string.Concat(values, " @interval");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@interval", plan.Interval);
                }
                if (!string.IsNullOrEmpty(plan.IntervalCount))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `interval_count` ");
                        values = string.Concat(values, ", @intervalCount");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `interval_count` ");
                        values = string.Concat(values, " @intervalCount");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@intervalCount", plan.IntervalCount);
                }
                if (!string.IsNullOrEmpty(plan.Currency))
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
                    cmd.Parameters.AddWithValue("@currency", plan.Currency);
                }
                if (!string.IsNullOrEmpty(plan.Amount))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `amount` ");
                        values = string.Concat(values, ", @amount");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `amount` ");
                        values = string.Concat(values, " @amount");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@amount", plan.Amount);
                }
                if (!string.IsNullOrEmpty(plan.IsForSchool))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `is_for_school` ");
                        values = string.Concat(values, ", @isForSchool");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `is_for_school` ");
                        values = string.Concat(values, " @isForSchool");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isForSchool", plan.IsForSchool);
                }
                if (!string.IsNullOrEmpty(plan.IsPrivate))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `is_private` ");
                        values = string.Concat(values, ", @isPrivate");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `is_private` ");
                        values = string.Concat(values, " @isPrivate");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isPrivate", plan.IsPrivate);
                }
                if (!string.IsNullOrEmpty(plan.ProfilesQuantity))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `profiles_quantity` ");
                        values = string.Concat(values, ", @profilesQuantity");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `profiles_quantity` ");
                        values = string.Concat(values, " @profilesQuantity");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@profilesQuantity", plan.ProfilesQuantity);
                }
                if (!string.IsNullOrEmpty(plan.IsSpecialPromo))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `is_special_promo` ");
                        values = string.Concat(values, ", @isSpecialPromo");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `is_special_promo` ");
                        values = string.Concat(values, " @isSpecialPromo");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isSpecialPromo", plan.IsSpecialPromo);
                }
                if (!string.IsNullOrEmpty(plan.SpecialPromoLimit))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `special_promo_limit` ");
                        values = string.Concat(values, ", @specialPromoLimit");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `special_promo_limit` ");
                        values = string.Concat(values, " @specialPromoLimit");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@specialPromoLimit", plan.SpecialPromoLimit);
                }
                if (!string.IsNullOrEmpty(plan.SpecialPromoDays))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `special_promo_days` ");
                        values = string.Concat(values, ", @specialPromoDays");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `special_promo_days` ");
                        values = string.Concat(values, " @specialPromoDays");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@specialPromoDays", plan.SpecialPromoDays);
                }
                if (!string.IsNullOrEmpty(plan.Available))
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
                    cmd.Parameters.AddWithValue("@available", plan.Available);
                }
                if (!string.IsNullOrEmpty(plan.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", plan.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(plan.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", plan.CreatedAt);
                }
                if (!string.IsNullOrEmpty(plan.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", plan.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                plan.Id = cmd.LastInsertedId.ToString();
                response.Add(plan);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanModel>();
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
        public List<PlanModel> Create(string connectionString, List<PlanModel> planList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanModel> response = new List<PlanModel>();
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
                        for (int i = 0; i < planList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_plans (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(planList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, planList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(planList[i].PlatformId))
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
                                    cmd.Parameters.AddWithValue("@platformId" + i, planList[i].PlatformId);
                                }
                                if (!string.IsNullOrEmpty(planList[i].ProductId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `product_id` ");
                                        values = string.Concat(values, ", @productId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `product_id` ");
                                        values = string.Concat(values, "@productId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@productId" + i, planList[i].ProductId);
                                }
                                if (!string.IsNullOrEmpty(planList[i].Name))
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
                                    cmd.Parameters.AddWithValue("@name" + i, planList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(planList[i].Interval))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `interval` ");
                                        values = string.Concat(values, ", @interval" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `interval` ");
                                        values = string.Concat(values, "@interval" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@interval" + i, planList[i].Interval);
                                }
                                if (!string.IsNullOrEmpty(planList[i].IntervalCount))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `interval_count` ");
                                        values = string.Concat(values, ", @intervalCount" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `interval_count` ");
                                        values = string.Concat(values, "@intervalCount" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@intervalCount" + i, planList[i].IntervalCount);
                                }
                                if (!string.IsNullOrEmpty(planList[i].Currency))
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
                                    cmd.Parameters.AddWithValue("@currency" + i, planList[i].Currency);
                                }
                                if (!string.IsNullOrEmpty(planList[i].Amount))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `amount` ");
                                        values = string.Concat(values, ", @amount" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `amount` ");
                                        values = string.Concat(values, "@amount" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@amount" + i, planList[i].Amount);
                                }
                                if (!string.IsNullOrEmpty(planList[i].IsForSchool))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `is_for_school` ");
                                        values = string.Concat(values, ", @isForSchool" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `is_for_school` ");
                                        values = string.Concat(values, "@isForSchool" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isForSchool" + i, planList[i].IsForSchool);
                                }
                                if (!string.IsNullOrEmpty(planList[i].IsPrivate))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `is_private` ");
                                        values = string.Concat(values, ", @isPrivate" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `is_private` ");
                                        values = string.Concat(values, "@isPrivate" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isPrivate" + i, planList[i].IsPrivate);
                                }
                                if (!string.IsNullOrEmpty(planList[i].ProfilesQuantity))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `profiles_quantity` ");
                                        values = string.Concat(values, ", @profilesQuantity" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `profiles_quantity` ");
                                        values = string.Concat(values, "@profilesQuantity" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@profilesQuantity" + i, planList[i].ProfilesQuantity);
                                }
                                if (!string.IsNullOrEmpty(planList[i].IsSpecialPromo))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `is_special_promo` ");
                                        values = string.Concat(values, ", @isSpecialPromo" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `is_special_promo` ");
                                        values = string.Concat(values, "@isSpecialPromo" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isSpecialPromo" + i, planList[i].IsSpecialPromo);
                                }
                                if (!string.IsNullOrEmpty(planList[i].SpecialPromoLimit))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `special_promo_limit` ");
                                        values = string.Concat(values, ", @specialPromoLimit" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `special_promo_limit` ");
                                        values = string.Concat(values, "@specialPromoLimit" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@specialPromoLimit" + i, planList[i].SpecialPromoLimit);
                                }
                                if (!string.IsNullOrEmpty(planList[i].SpecialPromoDays))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `special_promo_days` ");
                                        values = string.Concat(values, ", @specialPromoDays" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `special_promo_days` ");
                                        values = string.Concat(values, "@specialPromoDays" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@specialPromoDays" + i, planList[i].SpecialPromoDays);
                                }
                                if (!string.IsNullOrEmpty(planList[i].Available))
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
                                    cmd.Parameters.AddWithValue("@available" + i, planList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(planList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(planList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(planList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(planList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanModel>();
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

        public List<PlanModel> Update(string connectionString, PlanModel plan, PlanModel planWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanModel> response = new List<PlanModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE subst_plans SET ";
                if (!string.IsNullOrEmpty(plan.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", plan.Id);
                }
                if (!string.IsNullOrEmpty(plan.PlatformId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `platform_id` = @platformId");
                    else
                        sql = string.Concat(sql, " `platform_id` = @platformId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@platformId", plan.PlatformId);
                }
                if (!string.IsNullOrEmpty(plan.ProductId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `product_id` = @productId");
                    else
                        sql = string.Concat(sql, " `product_id` = @productId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@productId", plan.ProductId);
                }
                if (!string.IsNullOrEmpty(plan.Name))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `name` = @name");
                    else
                        sql = string.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", plan.Name);
                }
                if (!string.IsNullOrEmpty(plan.Interval))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `interval` = @interval");
                    else
                        sql = string.Concat(sql, " `interval` = @interval");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@interval", plan.Interval);
                }
                if (!string.IsNullOrEmpty(plan.IntervalCount))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `interval_count` = @intervalCount");
                    else
                        sql = string.Concat(sql, " `interval_count` = @intervalCount");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@intervalCount", plan.IntervalCount);
                }
                if (!string.IsNullOrEmpty(plan.Currency))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `currency` = @currency");
                    else
                        sql = string.Concat(sql, " `currency` = @currency");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@currency", plan.Currency);
                }
                if (!string.IsNullOrEmpty(plan.Amount))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `amount` = @amount");
                    else
                        sql = string.Concat(sql, " `amount` = @amount");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@amount", plan.Amount);
                }
                if (!string.IsNullOrEmpty(plan.IsForSchool))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `is_for_school` = @isForSchool");
                    else
                        sql = string.Concat(sql, " `is_for_school` = @isForSchool");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isForSchool", plan.IsForSchool);
                }
                if (!string.IsNullOrEmpty(plan.IsPrivate))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `is_private` = @isPrivate");
                    else
                        sql = string.Concat(sql, " `is_private` = @isPrivate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isPrivate", plan.IsPrivate);
                }
                if (!string.IsNullOrEmpty(plan.ProfilesQuantity))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `profiles_quantity` = @profilesQuantity");
                    else
                        sql = string.Concat(sql, " `profiles_quantity` = @profilesQuantity");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@profilesQuantity", plan.ProfilesQuantity);
                }
                if (!string.IsNullOrEmpty(plan.IsSpecialPromo))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `is_special_promo` = @isSpecialPromo");
                    else
                        sql = string.Concat(sql, " `is_special_promo` = @isSpecialPromo");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isSpecialPromo", plan.IsSpecialPromo);
                }
                if (!string.IsNullOrEmpty(plan.SpecialPromoLimit))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `special_promo_limit` = @specialPromoLimit");
                    else
                        sql = string.Concat(sql, " `special_promo_limit` = @specialPromoLimit");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@specialPromoLimit", plan.SpecialPromoLimit);
                }
                if (!string.IsNullOrEmpty(plan.SpecialPromoDays))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `special_promo_days` = @specialPromoDays");
                    else
                        sql = string.Concat(sql, " `special_promo_days` = @specialPromoDays");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@specialPromoDays", plan.SpecialPromoDays);
                }
                if (!string.IsNullOrEmpty(plan.Available))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `available` = @available");
                    else
                        sql = string.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", plan.Available);
                }
                if (!string.IsNullOrEmpty(plan.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", plan.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(plan.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", plan.CreatedAt);
                }
                if (!string.IsNullOrEmpty(plan.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", plan.UpdatedAt);
                }
                if (planWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(planWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", planWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(planWhere.PlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform_id` = @platformIdWhere");
                        else
                            sql = string.Concat(sql, " `platform_id` = @platformIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformIdWhere", planWhere.PlatformId);
                    }
                    if (!string.IsNullOrEmpty(planWhere.ProductId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `product_id` = @productIdWhere");
                        else
                            sql = string.Concat(sql, " `product_id` = @productIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@productIdWhere", planWhere.ProductId);
                    }
                    if (!string.IsNullOrEmpty(planWhere.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = string.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", planWhere.Name);
                    }
                    if (!string.IsNullOrEmpty(planWhere.Interval))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `interval` = @intervalWhere");
                        else
                            sql = string.Concat(sql, " `interval` = @intervalWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@intervalWhere", planWhere.Interval);
                    }
                    if (!string.IsNullOrEmpty(planWhere.IntervalCount))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `interval_count` = @intervalCountWhere");
                        else
                            sql = string.Concat(sql, " `interval_count` = @intervalCountWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@intervalCountWhere", planWhere.IntervalCount);
                    }
                    if (!string.IsNullOrEmpty(planWhere.Currency))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `currency` = @currencyWhere");
                        else
                            sql = string.Concat(sql, " `currency` = @currencyWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@currencyWhere", planWhere.Currency);
                    }
                    if (!string.IsNullOrEmpty(planWhere.Amount))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `amount` = @amountWhere");
                        else
                            sql = string.Concat(sql, " `amount` = @amountWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@amountWhere", planWhere.Amount);
                    }
                    if (!string.IsNullOrEmpty(planWhere.IsForSchool))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `is_for_school` = @isForSchoolWhere");
                        else
                            sql = string.Concat(sql, " `is_for_school` = @isForSchoolWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isForSchoolWhere", planWhere.IsForSchool);
                    }
                    if (!string.IsNullOrEmpty(planWhere.IsPrivate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `is_private` = @isPrivateWhere");
                        else
                            sql = string.Concat(sql, " `is_private` = @isPrivateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isPrivateWhere", planWhere.IsPrivate);
                    }
                    if (!string.IsNullOrEmpty(planWhere.ProfilesQuantity))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `profiles_quantity` = @profilesQuantityWhere");
                        else
                            sql = string.Concat(sql, " `profiles_quantity` = @profilesQuantityWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@profilesQuantityWhere", planWhere.ProfilesQuantity);
                    }
                    if (!string.IsNullOrEmpty(planWhere.IsSpecialPromo))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `is_special_promo` = @isSpecialPromoWhere");
                        else
                            sql = string.Concat(sql, " `is_special_promo` = @isSpecialPromoWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isSpecialPromoWhere", planWhere.IsSpecialPromo);
                    }
                    if (!string.IsNullOrEmpty(planWhere.SpecialPromoLimit))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `special_promo_limit` = @specialPromoLimitWhere");
                        else
                            sql = string.Concat(sql, " `special_promo_limit` = @specialPromoLimitWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@specialPromoLimitWhere", planWhere.SpecialPromoLimit);
                    }
                    if (!string.IsNullOrEmpty(planWhere.SpecialPromoDays))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `special_promo_days` = @specialPromoDaysWhere");
                        else
                            sql = string.Concat(sql, " `special_promo_days` = @specialPromoDaysWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@specialPromoDaysWhere", planWhere.SpecialPromoDays);
                    }
                    if (!string.IsNullOrEmpty(planWhere.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = string.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", planWhere.Available);
                    }
                    if (!string.IsNullOrEmpty(planWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", planWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(planWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", planWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(planWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", planWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(plan);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanModel>();
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

        public List<PlanModel> Update(string connectionString, List<PlanModel> planList, List<PlanModel> planWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanModel> response = new List<PlanModel>();
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
                        for (int i = 0; i < planList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_plans SET ";
                                if (!string.IsNullOrEmpty(planList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, planList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(planList[i].PlatformId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `platform_id` = @platformId" + i);
                                    else
                                        sql = string.Concat(sql, " `platform_id` = @platformId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@platformId" + i, planList[i].PlatformId);
                                }
                                if (!string.IsNullOrEmpty(planList[i].ProductId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `product_id` = @productId" + i);
                                    else
                                        sql = string.Concat(sql, " `product_id` = @productId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@productId" + i, planList[i].ProductId);
                                }
                                if (!string.IsNullOrEmpty(planList[i].Name))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = string.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, planList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(planList[i].Interval))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `interval` = @interval" + i);
                                    else
                                        sql = string.Concat(sql, " `interval` = @interval" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@interval" + i, planList[i].Interval);
                                }
                                if (!string.IsNullOrEmpty(planList[i].IntervalCount))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `interval_count` = @intervalCount" + i);
                                    else
                                        sql = string.Concat(sql, " `interval_count` = @intervalCount" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@intervalCount" + i, planList[i].IntervalCount);
                                }
                                if (!string.IsNullOrEmpty(planList[i].Currency))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `currency` = @currency" + i);
                                    else
                                        sql = string.Concat(sql, " `currency` = @currency" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@currency" + i, planList[i].Currency);
                                }
                                if (!string.IsNullOrEmpty(planList[i].Amount))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `amount` = @amount" + i);
                                    else
                                        sql = string.Concat(sql, " `amount` = @amount" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@amount" + i, planList[i].Amount);
                                }
                                if (!string.IsNullOrEmpty(planList[i].IsForSchool))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `is_for_school` = @isForSchool" + i);
                                    else
                                        sql = string.Concat(sql, " `is_for_school` = @isForSchool" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isForSchool" + i, planList[i].IsForSchool);
                                }
                                if (!string.IsNullOrEmpty(planList[i].IsPrivate))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `is_private` = @isPrivate" + i);
                                    else
                                        sql = string.Concat(sql, " `is_private` = @isPrivate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isPrivate" + i, planList[i].IsPrivate);
                                }
                                if (!string.IsNullOrEmpty(planList[i].ProfilesQuantity))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `profiles_quantity` = @profilesQuantity" + i);
                                    else
                                        sql = string.Concat(sql, " `profiles_quantity` = @profilesQuantity" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@profilesQuantity" + i, planList[i].ProfilesQuantity);
                                }
                                if (!string.IsNullOrEmpty(planList[i].IsSpecialPromo))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `is_special_promo` = @isSpecialPromo" + i);
                                    else
                                        sql = string.Concat(sql, " `is_special_promo` = @isSpecialPromo" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isSpecialPromo" + i, planList[i].IsSpecialPromo);
                                }
                                if (!string.IsNullOrEmpty(planList[i].SpecialPromoLimit))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `special_promo_limit` = @specialPromoLimit" + i);
                                    else
                                        sql = string.Concat(sql, " `special_promo_limit` = @specialPromoLimit" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@specialPromoLimit" + i, planList[i].SpecialPromoLimit);
                                }
                                if (!string.IsNullOrEmpty(planList[i].SpecialPromoDays))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `special_promo_days` = @specialPromoDays" + i);
                                    else
                                        sql = string.Concat(sql, " `special_promo_days` = @specialPromoDays" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@specialPromoDays" + i, planList[i].SpecialPromoDays);
                                }
                                if (!string.IsNullOrEmpty(planList[i].Available))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = string.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, planList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(planList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(planList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(planList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planList[i].UpdatedAt);
                                }
                                if (planWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(planWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, planWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].PlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform_id` = @platformIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `platform_id` = @platformIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformIdWhere" + i, planWhereList[i].PlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].ProductId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `product_id` = @productIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `product_id` = @productIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@productIdWhere" + i, planWhereList[i].ProductId);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, planWhereList[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].Interval))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `interval` = @intervalWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `interval` = @intervalWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@intervalWhere" + i, planWhereList[i].Interval);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].IntervalCount))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `interval_count` = @intervalCountWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `interval_count` = @intervalCountWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@intervalCountWhere" + i, planWhereList[i].IntervalCount);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].Currency))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `currency` = @currencyWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `currency` = @currencyWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@currencyWhere" + i, planWhereList[i].Currency);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].Amount))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `amount` = @amountWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `amount` = @amountWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@amountWhere" + i, planWhereList[i].Amount);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].IsForSchool))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `is_for_school` = @isForSchoolWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `is_for_school` = @isForSchoolWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isForSchoolWhere" + i, planWhereList[i].IsForSchool);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].IsPrivate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `is_private` = @isPrivateWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `is_private` = @isPrivateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isPrivateWhere" + i, planWhereList[i].IsPrivate);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].ProfilesQuantity))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `profiles_quantity` = @profilesQuantityWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `profiles_quantity` = @profilesQuantityWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@profilesQuantityWhere" + i, planWhereList[i].ProfilesQuantity);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].IsSpecialPromo))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `is_special_promo` = @isSpecialPromoWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `is_special_promo` = @isSpecialPromoWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isSpecialPromoWhere" + i, planWhereList[i].IsSpecialPromo);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].SpecialPromoLimit))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `special_promo_limit` = @specialPromoLimitWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `special_promo_limit` = @specialPromoLimitWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@specialPromoLimitWhere" + i, planWhereList[i].SpecialPromoLimit);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].SpecialPromoDays))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `special_promo_days` = @specialPromoDaysWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `special_promo_days` = @specialPromoDaysWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@specialPromoDaysWhere" + i, planWhereList[i].SpecialPromoDays);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, planWhereList[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, planWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, planWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(planWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, planWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(planList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanModel>();
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

        public List<PlanModel> Delete(string connectionString, PlanModel plan)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanModel> response = new List<PlanModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM subst_plans  ";
                if (plan != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(plan.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", plan.Id);
                    }
                    if (!string.IsNullOrEmpty(plan.PlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform_id` = @platformId");
                        else
                            sql = string.Concat(sql, " `platform_id` = @platformId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformId", plan.PlatformId);
                    }
                    if (!string.IsNullOrEmpty(plan.ProductId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `product_id` = @productId");
                        else
                            sql = string.Concat(sql, " `product_id` = @productId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@productId", plan.ProductId);
                    }
                    if (!string.IsNullOrEmpty(plan.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @name");
                        else
                            sql = string.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", plan.Name);
                    }
                    if (!string.IsNullOrEmpty(plan.Interval))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `interval` = @interval");
                        else
                            sql = string.Concat(sql, " `interval` = @interval");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@interval", plan.Interval);
                    }
                    if (!string.IsNullOrEmpty(plan.IntervalCount))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `interval_count` = @intervalCount");
                        else
                            sql = string.Concat(sql, " `interval_count` = @intervalCount");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@intervalCount", plan.IntervalCount);
                    }
                    if (!string.IsNullOrEmpty(plan.Currency))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `currency` = @currency");
                        else
                            sql = string.Concat(sql, " `currency` = @currency");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@currency", plan.Currency);
                    }
                    if (!string.IsNullOrEmpty(plan.Amount))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `amount` = @amount");
                        else
                            sql = string.Concat(sql, " `amount` = @amount");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@amount", plan.Amount);
                    }
                    if (!string.IsNullOrEmpty(plan.IsForSchool))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `is_for_school` = @isForSchool");
                        else
                            sql = string.Concat(sql, " `is_for_school` = @isForSchool");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isForSchool", plan.IsForSchool);
                    }
                    if (!string.IsNullOrEmpty(plan.IsPrivate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `is_private` = @isPrivate");
                        else
                            sql = string.Concat(sql, " `is_private` = @isPrivate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isPrivate", plan.IsPrivate);
                    }
                    if (!string.IsNullOrEmpty(plan.ProfilesQuantity))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `profiles_quantity` = @profilesQuantity");
                        else
                            sql = string.Concat(sql, " `profiles_quantity` = @profilesQuantity");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@profilesQuantity", plan.ProfilesQuantity);
                    }
                    if (!string.IsNullOrEmpty(plan.IsSpecialPromo))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `is_special_promo` = @isSpecialPromo");
                        else
                            sql = string.Concat(sql, " `is_special_promo` = @isSpecialPromo");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isSpecialPromo", plan.IsSpecialPromo);
                    }
                    if (!string.IsNullOrEmpty(plan.SpecialPromoLimit))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `special_promo_limit` = @specialPromoLimit");
                        else
                            sql = string.Concat(sql, " `special_promo_limit` = @specialPromoLimit");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@specialPromoLimit", plan.SpecialPromoLimit);
                    }
                    if (!string.IsNullOrEmpty(plan.SpecialPromoDays))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `special_promo_days` = @specialPromoDays");
                        else
                            sql = string.Concat(sql, " `special_promo_days` = @specialPromoDays");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@specialPromoDays", plan.SpecialPromoDays);
                    }
                    if (!string.IsNullOrEmpty(plan.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @available");
                        else
                            sql = string.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", plan.Available);
                    }
                    if (!string.IsNullOrEmpty(plan.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", plan.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(plan.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", plan.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(plan.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", plan.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(plan);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanModel>();
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

        public List<PlanModel> Delete(string connectionString, List<PlanModel> plan)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanModel> response = new List<PlanModel>();
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
                        for (int i = 0; i < plan.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_plans  ";
                                if (plan[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(plan[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, plan[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].PlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform_id` = @platformId" + i);
                                        else
                                            sql = string.Concat(sql, " `platform_id` = @platformId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformId" + i, plan[i].PlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].ProductId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `product_id` = @productId" + i);
                                        else
                                            sql = string.Concat(sql, " `product_id` = @productId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@productId" + i, plan[i].ProductId);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, plan[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].Interval))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `interval` = @interval" + i);
                                        else
                                            sql = string.Concat(sql, " `interval` = @interval" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@interval" + i, plan[i].Interval);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].IntervalCount))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `interval_count` = @intervalCount" + i);
                                        else
                                            sql = string.Concat(sql, " `interval_count` = @intervalCount" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@intervalCount" + i, plan[i].IntervalCount);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].Currency))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `currency` = @currency" + i);
                                        else
                                            sql = string.Concat(sql, " `currency` = @currency" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@currency" + i, plan[i].Currency);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].Amount))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `amount` = @amount" + i);
                                        else
                                            sql = string.Concat(sql, " `amount` = @amount" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@amount" + i, plan[i].Amount);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].IsForSchool))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `is_for_school` = @isForSchool" + i);
                                        else
                                            sql = string.Concat(sql, " `is_for_school` = @isForSchool" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isForSchool" + i, plan[i].IsForSchool);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].IsPrivate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `is_private` = @isPrivate" + i);
                                        else
                                            sql = string.Concat(sql, " `is_private` = @isPrivate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isPrivate" + i, plan[i].IsPrivate);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].ProfilesQuantity))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `profiles_quantity` = @profilesQuantity" + i);
                                        else
                                            sql = string.Concat(sql, " `profiles_quantity` = @profilesQuantity" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@profilesQuantity" + i, plan[i].ProfilesQuantity);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].IsSpecialPromo))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `is_special_promo` = @isSpecialPromo" + i);
                                        else
                                            sql = string.Concat(sql, " `is_special_promo` = @isSpecialPromo" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isSpecialPromo" + i, plan[i].IsSpecialPromo);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].SpecialPromoLimit))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `special_promo_limit` = @specialPromoLimit" + i);
                                        else
                                            sql = string.Concat(sql, " `special_promo_limit` = @specialPromoLimit" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@specialPromoLimit" + i, plan[i].SpecialPromoLimit);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].SpecialPromoDays))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `special_promo_days` = @specialPromoDays" + i);
                                        else
                                            sql = string.Concat(sql, " `special_promo_days` = @specialPromoDays" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@specialPromoDays" + i, plan[i].SpecialPromoDays);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, plan[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, plan[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, plan[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(plan[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, plan[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(plan[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanModel>();
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
