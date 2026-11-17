using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubscriptionsAPI.CustomClasses;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Generals
{
    public class SubscriptionProductDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, SubscriptionProductModel> ReadD(string connectionString, SubscriptionProductModel subscriptionProduct, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SubscriptionProductModel> response = new Dictionary<string, SubscriptionProductModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `product_id`, `subject`, `subject_name`, `level`, `modified_by`, `created_at`, `updated_at`  FROM gwp_subscription_products ";
                {
                    if (subscriptionProduct != null)
                    {
                        if (!string.IsNullOrEmpty(subscriptionProduct.Id))
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
                            cmd.Parameters.AddWithValue("@id", subscriptionProduct.Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.ProductId))
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
                            cmd.Parameters.AddWithValue("@productId", subscriptionProduct.ProductId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.Subject))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject` = @subject ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject` = @subject ");
                            }
                            cmd.Parameters.AddWithValue("@subject", subscriptionProduct.Subject);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.SubjectName))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject_name` = @subjectName ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject_name` = @subjectName ");
                            }
                            cmd.Parameters.AddWithValue("@subjectName", subscriptionProduct.SubjectName);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.Level))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `level` = @level ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `level` = @level ");
                            }
                            cmd.Parameters.AddWithValue("@level", subscriptionProduct.Level);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", subscriptionProduct.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", subscriptionProduct.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", subscriptionProduct.UpdatedAt);
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
                                    subscriptionProduct.Id = reader.GetValue(0).ToString();
                                else
                                    subscriptionProduct.Id = null;
                                if (!reader.IsDBNull(1))
                                    subscriptionProduct.ProductId = reader.GetValue(1).ToString();
                                else
                                    subscriptionProduct.ProductId = null;
                                if (!reader.IsDBNull(2))
                                    subscriptionProduct.Subject = reader.GetValue(2).ToString();
                                else
                                    subscriptionProduct.Subject = null;
                                if (!reader.IsDBNull(3))
                                    subscriptionProduct.SubjectName = reader.GetValue(3).ToString();
                                else
                                    subscriptionProduct.SubjectName = null;
                                if (!reader.IsDBNull(4))
                                    subscriptionProduct.Level = reader.GetValue(4).ToString();
                                else
                                    subscriptionProduct.Level = null;
                                if (!reader.IsDBNull(5))
                                    subscriptionProduct.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    subscriptionProduct.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    subscriptionProduct.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    subscriptionProduct.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    subscriptionProduct.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    subscriptionProduct.UpdatedAt = null;
                                response.Add(subscriptionProduct.Id, subscriptionProduct);
                                subscriptionProduct = new SubscriptionProductModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, SubscriptionProductModel>();
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
                response = new Dictionary<string, SubscriptionProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, SubscriptionProductModel> ReadD(string connectionString, SubscriptionProductModel subscriptionProduct, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SubscriptionProductModel> response = new Dictionary<string, SubscriptionProductModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `product_id`, `subject`, `subject_name`, `level`, `modified_by`, `created_at`, `updated_at`  FROM gwp_subscription_products ";
                {
                    if (subscriptionProduct != null)
                    {
                        if (!string.IsNullOrEmpty(subscriptionProduct.Id))
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
                            cmd.Parameters.AddWithValue("@id", subscriptionProduct.Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.ProductId))
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
                            cmd.Parameters.AddWithValue("@productId", subscriptionProduct.ProductId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.Subject))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject` = @subject ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject` = @subject ");
                            }
                            cmd.Parameters.AddWithValue("@subject", subscriptionProduct.Subject);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.SubjectName))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject_name` = @subjectName ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject_name` = @subjectName ");
                            }
                            cmd.Parameters.AddWithValue("@subjectName", subscriptionProduct.SubjectName);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.Level))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `level` = @level ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `level` = @level ");
                            }
                            cmd.Parameters.AddWithValue("@level", subscriptionProduct.Level);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", subscriptionProduct.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", subscriptionProduct.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", subscriptionProduct.UpdatedAt);
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
                                    subscriptionProduct.Id = reader.GetValue(0).ToString();
                                else
                                    subscriptionProduct.Id = null;
                                if (!reader.IsDBNull(1))
                                    subscriptionProduct.ProductId = reader.GetValue(1).ToString();
                                else
                                    subscriptionProduct.ProductId = null;
                                if (!reader.IsDBNull(2))
                                    subscriptionProduct.Subject = reader.GetValue(2).ToString();
                                else
                                    subscriptionProduct.Subject = null;
                                if (!reader.IsDBNull(3))
                                    subscriptionProduct.SubjectName = reader.GetValue(3).ToString();
                                else
                                    subscriptionProduct.SubjectName = null;
                                if (!reader.IsDBNull(4))
                                    subscriptionProduct.Level = reader.GetValue(4).ToString();
                                else
                                    subscriptionProduct.Level = null;
                                if (!reader.IsDBNull(5))
                                    subscriptionProduct.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    subscriptionProduct.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    subscriptionProduct.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    subscriptionProduct.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    subscriptionProduct.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    subscriptionProduct.UpdatedAt = null;
                                response.Add(subscriptionProduct.Id, subscriptionProduct);
                                subscriptionProduct = new SubscriptionProductModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, SubscriptionProductModel>();
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
                response = new Dictionary<string, SubscriptionProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, SubscriptionProductModel> ReadD(string connectionString, List<SubscriptionProductModel> subscriptionProductWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SubscriptionProductModel> response = new Dictionary<string, SubscriptionProductModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `product_id`, `subject`, `subject_name`, `level`, `modified_by`, `created_at`, `updated_at`  FROM gwp_subscription_products ";
                if (subscriptionProductWhere != null && subscriptionProductWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < subscriptionProductWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, subscriptionProductWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].ProductId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `product_id` = @productId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@productId" + i, subscriptionProductWhere[i].ProductId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].Subject))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject` = @subject" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subject" + i, subscriptionProductWhere[i].Subject);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].SubjectName))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_name` = @subjectName" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectName" + i, subscriptionProductWhere[i].SubjectName);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].Level))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level` = @level" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@level" + i, subscriptionProductWhere[i].Level);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionProductWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionProductWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionProductWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != subscriptionProductWhere.Count - 1)
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
                            SubscriptionProductModel subscriptionProduct = new SubscriptionProductModel(); if (!reader.IsDBNull(0))
                                subscriptionProduct.Id = reader.GetValue(0).ToString();
                            else
                                subscriptionProduct.Id = null;
                            if (!reader.IsDBNull(1))
                                subscriptionProduct.ProductId = reader.GetValue(1).ToString();
                            else
                                subscriptionProduct.ProductId = null;
                            if (!reader.IsDBNull(2))
                                subscriptionProduct.Subject = reader.GetValue(2).ToString();
                            else
                                subscriptionProduct.Subject = null;
                            if (!reader.IsDBNull(3))
                                subscriptionProduct.SubjectName = reader.GetValue(3).ToString();
                            else
                                subscriptionProduct.SubjectName = null;
                            if (!reader.IsDBNull(4))
                                subscriptionProduct.Level = reader.GetValue(4).ToString();
                            else
                                subscriptionProduct.Level = null;
                            if (!reader.IsDBNull(5))
                                subscriptionProduct.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                subscriptionProduct.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                subscriptionProduct.CreatedAt = reader.GetValue(6).ToString();
                            else
                                subscriptionProduct.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                subscriptionProduct.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                subscriptionProduct.UpdatedAt = null;
                            response.Add(subscriptionProduct.Id, subscriptionProduct);
                            subscriptionProduct = new SubscriptionProductModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, SubscriptionProductModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, SubscriptionProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, SubscriptionProductModel> ReadD(string connectionString, List<SubscriptionProductModel> subscriptionProductWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, SubscriptionProductModel> response = new Dictionary<string, SubscriptionProductModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `product_id`, `subject`, `subject_name`, `level`, `modified_by`, `created_at`, `updated_at`  FROM gwp_subscription_products ";
                if (subscriptionProductWhere != null && subscriptionProductWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < subscriptionProductWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, subscriptionProductWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].ProductId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `product_id` = @productId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@productId" + i, subscriptionProductWhere[i].ProductId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].Subject))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject` = @subject" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subject" + i, subscriptionProductWhere[i].Subject);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].SubjectName))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_name` = @subjectName" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectName" + i, subscriptionProductWhere[i].SubjectName);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].Level))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level` = @level" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@level" + i, subscriptionProductWhere[i].Level);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionProductWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionProductWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionProductWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != subscriptionProductWhere.Count - 1)
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
                            SubscriptionProductModel subscriptionProduct = new SubscriptionProductModel(); if (!reader.IsDBNull(0))
                                subscriptionProduct.Id = reader.GetValue(0).ToString();
                            else
                                subscriptionProduct.Id = null;
                            if (!reader.IsDBNull(1))
                                subscriptionProduct.ProductId = reader.GetValue(1).ToString();
                            else
                                subscriptionProduct.ProductId = null;
                            if (!reader.IsDBNull(2))
                                subscriptionProduct.Subject = reader.GetValue(2).ToString();
                            else
                                subscriptionProduct.Subject = null;
                            if (!reader.IsDBNull(3))
                                subscriptionProduct.SubjectName = reader.GetValue(3).ToString();
                            else
                                subscriptionProduct.SubjectName = null;
                            if (!reader.IsDBNull(4))
                                subscriptionProduct.Level = reader.GetValue(4).ToString();
                            else
                                subscriptionProduct.Level = null;
                            if (!reader.IsDBNull(5))
                                subscriptionProduct.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                subscriptionProduct.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                subscriptionProduct.CreatedAt = reader.GetValue(6).ToString();
                            else
                                subscriptionProduct.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                subscriptionProduct.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                subscriptionProduct.UpdatedAt = null;
                            response.Add(subscriptionProduct.Id, subscriptionProduct);
                            subscriptionProduct = new SubscriptionProductModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, SubscriptionProductModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, SubscriptionProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionProductModel> Read(string connectionString, SubscriptionProductModel subscriptionProduct, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProductModel> response = new List<SubscriptionProductModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `product_id`, `subject`, `subject_name`, `level`, `modified_by`, `created_at`, `updated_at`  FROM gwp_subscription_products ";
                {
                    if (subscriptionProduct != null)
                    {
                        if (!string.IsNullOrEmpty(subscriptionProduct.Id))
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
                            cmd.Parameters.AddWithValue("@id", subscriptionProduct.Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.ProductId))
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
                            cmd.Parameters.AddWithValue("@productId", subscriptionProduct.ProductId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.Subject))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject` = @subject ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject` = @subject ");
                            }
                            cmd.Parameters.AddWithValue("@subject", subscriptionProduct.Subject);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.SubjectName))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject_name` = @subjectName ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject_name` = @subjectName ");
                            }
                            cmd.Parameters.AddWithValue("@subjectName", subscriptionProduct.SubjectName);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.Level))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `level` = @level ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `level` = @level ");
                            }
                            cmd.Parameters.AddWithValue("@level", subscriptionProduct.Level);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", subscriptionProduct.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", subscriptionProduct.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", subscriptionProduct.UpdatedAt);
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
                                    subscriptionProduct.Id = reader.GetValue(0).ToString();
                                else
                                    subscriptionProduct.Id = null;
                                if (!reader.IsDBNull(1))
                                    subscriptionProduct.ProductId = reader.GetValue(1).ToString();
                                else
                                    subscriptionProduct.ProductId = null;
                                if (!reader.IsDBNull(2))
                                    subscriptionProduct.Subject = reader.GetValue(2).ToString();
                                else
                                    subscriptionProduct.Subject = null;
                                if (!reader.IsDBNull(3))
                                    subscriptionProduct.SubjectName = reader.GetValue(3).ToString();
                                else
                                    subscriptionProduct.SubjectName = null;
                                if (!reader.IsDBNull(4))
                                    subscriptionProduct.Level = reader.GetValue(4).ToString();
                                else
                                    subscriptionProduct.Level = null;
                                if (!reader.IsDBNull(5))
                                    subscriptionProduct.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    subscriptionProduct.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    subscriptionProduct.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    subscriptionProduct.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    subscriptionProduct.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    subscriptionProduct.UpdatedAt = null;
                                response.Add(subscriptionProduct);
                                subscriptionProduct = new SubscriptionProductModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<SubscriptionProductModel>();
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
                response = new List<SubscriptionProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionProductModel> Read(string connectionString, SubscriptionProductModel subscriptionProduct, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProductModel> response = new List<SubscriptionProductModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `product_id`, `subject`, `subject_name`, `level`, `modified_by`, `created_at`, `updated_at`  FROM gwp_subscription_products ";
                {
                    if (subscriptionProduct != null)
                    {
                        if (!string.IsNullOrEmpty(subscriptionProduct.Id))
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
                            cmd.Parameters.AddWithValue("@id", subscriptionProduct.Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.ProductId))
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
                            cmd.Parameters.AddWithValue("@productId", subscriptionProduct.ProductId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.Subject))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject` = @subject ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject` = @subject ");
                            }
                            cmd.Parameters.AddWithValue("@subject", subscriptionProduct.Subject);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.SubjectName))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject_name` = @subjectName ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject_name` = @subjectName ");
                            }
                            cmd.Parameters.AddWithValue("@subjectName", subscriptionProduct.SubjectName);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.Level))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `level` = @level ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `level` = @level ");
                            }
                            cmd.Parameters.AddWithValue("@level", subscriptionProduct.Level);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", subscriptionProduct.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", subscriptionProduct.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProduct.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", subscriptionProduct.UpdatedAt);
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
                                    subscriptionProduct.Id = reader.GetValue(0).ToString();
                                else
                                    subscriptionProduct.Id = null;
                                if (!reader.IsDBNull(1))
                                    subscriptionProduct.ProductId = reader.GetValue(1).ToString();
                                else
                                    subscriptionProduct.ProductId = null;
                                if (!reader.IsDBNull(2))
                                    subscriptionProduct.Subject = reader.GetValue(2).ToString();
                                else
                                    subscriptionProduct.Subject = null;
                                if (!reader.IsDBNull(3))
                                    subscriptionProduct.SubjectName = reader.GetValue(3).ToString();
                                else
                                    subscriptionProduct.SubjectName = null;
                                if (!reader.IsDBNull(4))
                                    subscriptionProduct.Level = reader.GetValue(4).ToString();
                                else
                                    subscriptionProduct.Level = null;
                                if (!reader.IsDBNull(5))
                                    subscriptionProduct.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    subscriptionProduct.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    subscriptionProduct.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    subscriptionProduct.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    subscriptionProduct.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    subscriptionProduct.UpdatedAt = null;
                                response.Add(subscriptionProduct);
                                subscriptionProduct = new SubscriptionProductModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<SubscriptionProductModel>();
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
                response = new List<SubscriptionProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionProductModel> Read(string connectionString, List<SubscriptionProductModel> subscriptionProductWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProductModel> response = new List<SubscriptionProductModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `product_id`, `subject`, `subject_name`, `level`, `modified_by`, `created_at`, `updated_at`  FROM gwp_subscription_products ";
                if (subscriptionProductWhere != null && subscriptionProductWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < subscriptionProductWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, subscriptionProductWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].ProductId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `product_id` = @productId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@productId" + i, subscriptionProductWhere[i].ProductId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].Subject))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject` = @subject" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subject" + i, subscriptionProductWhere[i].Subject);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].SubjectName))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_name` = @subjectName" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectName" + i, subscriptionProductWhere[i].SubjectName);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].Level))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level` = @level" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@level" + i, subscriptionProductWhere[i].Level);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionProductWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionProductWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionProductWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != subscriptionProductWhere.Count - 1)
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
                            SubscriptionProductModel subscriptionProduct = new SubscriptionProductModel(); if (!reader.IsDBNull(0))
                                subscriptionProduct.Id = reader.GetValue(0).ToString();
                            else
                                subscriptionProduct.Id = null;
                            if (!reader.IsDBNull(1))
                                subscriptionProduct.ProductId = reader.GetValue(1).ToString();
                            else
                                subscriptionProduct.ProductId = null;
                            if (!reader.IsDBNull(2))
                                subscriptionProduct.Subject = reader.GetValue(2).ToString();
                            else
                                subscriptionProduct.Subject = null;
                            if (!reader.IsDBNull(3))
                                subscriptionProduct.SubjectName = reader.GetValue(3).ToString();
                            else
                                subscriptionProduct.SubjectName = null;
                            if (!reader.IsDBNull(4))
                                subscriptionProduct.Level = reader.GetValue(4).ToString();
                            else
                                subscriptionProduct.Level = null;
                            if (!reader.IsDBNull(5))
                                subscriptionProduct.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                subscriptionProduct.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                subscriptionProduct.CreatedAt = reader.GetValue(6).ToString();
                            else
                                subscriptionProduct.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                subscriptionProduct.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                subscriptionProduct.UpdatedAt = null;
                            response.Add(subscriptionProduct);
                            subscriptionProduct = new SubscriptionProductModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<SubscriptionProductModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionProductModel> Read(string connectionString, List<SubscriptionProductModel> subscriptionProductWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProductModel> response = new List<SubscriptionProductModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `product_id`, `subject`, `subject_name`, `level`, `modified_by`, `created_at`, `updated_at`  FROM gwp_subscription_products ";
                if (subscriptionProductWhere != null && subscriptionProductWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < subscriptionProductWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, subscriptionProductWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].ProductId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `product_id` = @productId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@productId" + i, subscriptionProductWhere[i].ProductId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].Subject))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject` = @subject" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subject" + i, subscriptionProductWhere[i].Subject);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].SubjectName))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_name` = @subjectName" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectName" + i, subscriptionProductWhere[i].SubjectName);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].Level))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level` = @level" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@level" + i, subscriptionProductWhere[i].Level);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionProductWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionProductWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProductWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionProductWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != subscriptionProductWhere.Count - 1)
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
                            SubscriptionProductModel subscriptionProduct = new SubscriptionProductModel(); if (!reader.IsDBNull(0))
                                subscriptionProduct.Id = reader.GetValue(0).ToString();
                            else
                                subscriptionProduct.Id = null;
                            if (!reader.IsDBNull(1))
                                subscriptionProduct.ProductId = reader.GetValue(1).ToString();
                            else
                                subscriptionProduct.ProductId = null;
                            if (!reader.IsDBNull(2))
                                subscriptionProduct.Subject = reader.GetValue(2).ToString();
                            else
                                subscriptionProduct.Subject = null;
                            if (!reader.IsDBNull(3))
                                subscriptionProduct.SubjectName = reader.GetValue(3).ToString();
                            else
                                subscriptionProduct.SubjectName = null;
                            if (!reader.IsDBNull(4))
                                subscriptionProduct.Level = reader.GetValue(4).ToString();
                            else
                                subscriptionProduct.Level = null;
                            if (!reader.IsDBNull(5))
                                subscriptionProduct.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                subscriptionProduct.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                subscriptionProduct.CreatedAt = reader.GetValue(6).ToString();
                            else
                                subscriptionProduct.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                subscriptionProduct.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                subscriptionProduct.UpdatedAt = null;
                            response.Add(subscriptionProduct);
                            subscriptionProduct = new SubscriptionProductModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<SubscriptionProductModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionProductModel> Create(string connectionString, SubscriptionProductModel subscriptionProduct)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProductModel> response = new List<SubscriptionProductModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO gwp_subscription_products (";
                string values = "(";
                if (!string.IsNullOrEmpty(subscriptionProduct.Id))
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
                    cmd.Parameters.AddWithValue("@id", subscriptionProduct.Id);
                }
                if (!string.IsNullOrEmpty(subscriptionProduct.ProductId))
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
                    cmd.Parameters.AddWithValue("@productId", subscriptionProduct.ProductId);
                }
                if (!string.IsNullOrEmpty(subscriptionProduct.Subject))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `subject` ");
                        values = string.Concat(values, ", @subject");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `subject` ");
                        values = string.Concat(values, " @subject");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subject", subscriptionProduct.Subject);
                }
                if (!string.IsNullOrEmpty(subscriptionProduct.SubjectName))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `subject_name` ");
                        values = string.Concat(values, ", @subjectName");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `subject_name` ");
                        values = string.Concat(values, " @subjectName");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectName", subscriptionProduct.SubjectName);
                }
                if (!string.IsNullOrEmpty(subscriptionProduct.Level))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `level` ");
                        values = string.Concat(values, ", @level");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `level` ");
                        values = string.Concat(values, " @level");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@level", subscriptionProduct.Level);
                }
                if (!string.IsNullOrEmpty(subscriptionProduct.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", subscriptionProduct.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(subscriptionProduct.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", subscriptionProduct.CreatedAt);
                }
                if (!string.IsNullOrEmpty(subscriptionProduct.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", subscriptionProduct.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                subscriptionProduct.Id = cmd.LastInsertedId.ToString();
                response.Add(subscriptionProduct);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProductModel>();
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
        public List<SubscriptionProductModel> Create(string connectionString, List<SubscriptionProductModel> subscriptionProductList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProductModel> response = new List<SubscriptionProductModel>();
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
                        for (int i = 0; i < subscriptionProductList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO gwp_subscription_products (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, subscriptionProductList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].ProductId))
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
                                    cmd.Parameters.AddWithValue("@productId" + i, subscriptionProductList[i].ProductId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].Subject))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `subject` ");
                                        values = string.Concat(values, ", @subject" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `subject` ");
                                        values = string.Concat(values, "@subject" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subject" + i, subscriptionProductList[i].Subject);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].SubjectName))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `subject_name` ");
                                        values = string.Concat(values, ", @subjectName" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `subject_name` ");
                                        values = string.Concat(values, "@subjectName" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectName" + i, subscriptionProductList[i].SubjectName);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].Level))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `level` ");
                                        values = string.Concat(values, ", @level" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `level` ");
                                        values = string.Concat(values, "@level" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@level" + i, subscriptionProductList[i].Level);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionProductList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionProductList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionProductList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(subscriptionProductList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<SubscriptionProductModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SubscriptionProductModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProductModel>();
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

        public List<SubscriptionProductModel> Update(string connectionString, SubscriptionProductModel subscriptionProduct, SubscriptionProductModel subscriptionProductWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProductModel> response = new List<SubscriptionProductModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE gwp_subscription_products SET ";
                if (!string.IsNullOrEmpty(subscriptionProduct.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", subscriptionProduct.Id);
                }
                if (!string.IsNullOrEmpty(subscriptionProduct.ProductId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `product_id` = @productId");
                    else
                        sql = string.Concat(sql, " `product_id` = @productId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@productId", subscriptionProduct.ProductId);
                }
                if (!string.IsNullOrEmpty(subscriptionProduct.Subject))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `subject` = @subject");
                    else
                        sql = string.Concat(sql, " `subject` = @subject");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subject", subscriptionProduct.Subject);
                }
                if (!string.IsNullOrEmpty(subscriptionProduct.SubjectName))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `subject_name` = @subjectName");
                    else
                        sql = string.Concat(sql, " `subject_name` = @subjectName");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectName", subscriptionProduct.SubjectName);
                }
                if (!string.IsNullOrEmpty(subscriptionProduct.Level))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `level` = @level");
                    else
                        sql = string.Concat(sql, " `level` = @level");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@level", subscriptionProduct.Level);
                }
                if (!string.IsNullOrEmpty(subscriptionProduct.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", subscriptionProduct.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(subscriptionProduct.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", subscriptionProduct.CreatedAt);
                }
                if (!string.IsNullOrEmpty(subscriptionProduct.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", subscriptionProduct.UpdatedAt);
                }
                if (subscriptionProductWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(subscriptionProductWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", subscriptionProductWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProductWhere.ProductId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `product_id` = @productIdWhere");
                        else
                            sql = string.Concat(sql, " `product_id` = @productIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@productIdWhere", subscriptionProductWhere.ProductId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProductWhere.Subject))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subject` = @subjectWhere");
                        else
                            sql = string.Concat(sql, " `subject` = @subjectWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectWhere", subscriptionProductWhere.Subject);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProductWhere.SubjectName))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subject_name` = @subjectNameWhere");
                        else
                            sql = string.Concat(sql, " `subject_name` = @subjectNameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectNameWhere", subscriptionProductWhere.SubjectName);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProductWhere.Level))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `level` = @levelWhere");
                        else
                            sql = string.Concat(sql, " `level` = @levelWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelWhere", subscriptionProductWhere.Level);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProductWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", subscriptionProductWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProductWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", subscriptionProductWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProductWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", subscriptionProductWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(subscriptionProduct);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProductModel>();
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

        public List<SubscriptionProductModel> Update(string connectionString, List<SubscriptionProductModel> subscriptionProductList, List<SubscriptionProductModel> subscriptionProductWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProductModel> response = new List<SubscriptionProductModel>();
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
                        for (int i = 0; i < subscriptionProductList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE gwp_subscription_products SET ";
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, subscriptionProductList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].ProductId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `product_id` = @productId" + i);
                                    else
                                        sql = string.Concat(sql, " `product_id` = @productId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@productId" + i, subscriptionProductList[i].ProductId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].Subject))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `subject` = @subject" + i);
                                    else
                                        sql = string.Concat(sql, " `subject` = @subject" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subject" + i, subscriptionProductList[i].Subject);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].SubjectName))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `subject_name` = @subjectName" + i);
                                    else
                                        sql = string.Concat(sql, " `subject_name` = @subjectName" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectName" + i, subscriptionProductList[i].SubjectName);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].Level))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `level` = @level" + i);
                                    else
                                        sql = string.Concat(sql, " `level` = @level" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@level" + i, subscriptionProductList[i].Level);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionProductList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionProductList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProductList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionProductList[i].UpdatedAt);
                                }
                                if (subscriptionProductWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(subscriptionProductWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, subscriptionProductWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProductWhereList[i].ProductId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `product_id` = @productIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `product_id` = @productIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@productIdWhere" + i, subscriptionProductWhereList[i].ProductId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProductWhereList[i].Subject))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subject` = @subjectWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `subject` = @subjectWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectWhere" + i, subscriptionProductWhereList[i].Subject);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProductWhereList[i].SubjectName))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subject_name` = @subjectNameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `subject_name` = @subjectNameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectNameWhere" + i, subscriptionProductWhereList[i].SubjectName);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProductWhereList[i].Level))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `level` = @levelWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `level` = @levelWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelWhere" + i, subscriptionProductWhereList[i].Level);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProductWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, subscriptionProductWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProductWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, subscriptionProductWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProductWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, subscriptionProductWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(subscriptionProductList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<SubscriptionProductModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SubscriptionProductModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProductModel>();
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

        public List<SubscriptionProductModel> Delete(string connectionString, SubscriptionProductModel subscriptionProduct)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProductModel> response = new List<SubscriptionProductModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM gwp_subscription_products  ";
                if (subscriptionProduct != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(subscriptionProduct.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", subscriptionProduct.Id);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProduct.ProductId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `product_id` = @productId");
                        else
                            sql = string.Concat(sql, " `product_id` = @productId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@productId", subscriptionProduct.ProductId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProduct.Subject))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subject` = @subject");
                        else
                            sql = string.Concat(sql, " `subject` = @subject");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subject", subscriptionProduct.Subject);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProduct.SubjectName))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subject_name` = @subjectName");
                        else
                            sql = string.Concat(sql, " `subject_name` = @subjectName");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectName", subscriptionProduct.SubjectName);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProduct.Level))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `level` = @level");
                        else
                            sql = string.Concat(sql, " `level` = @level");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@level", subscriptionProduct.Level);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProduct.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", subscriptionProduct.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProduct.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", subscriptionProduct.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProduct.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", subscriptionProduct.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(subscriptionProduct);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProductModel>();
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

        public List<SubscriptionProductModel> Delete(string connectionString, List<SubscriptionProductModel> subscriptionProduct)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProductModel> response = new List<SubscriptionProductModel>();
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
                        for (int i = 0; i < subscriptionProduct.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM gwp_subscription_products  ";
                                if (subscriptionProduct[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(subscriptionProduct[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, subscriptionProduct[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProduct[i].ProductId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `product_id` = @productId" + i);
                                        else
                                            sql = string.Concat(sql, " `product_id` = @productId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@productId" + i, subscriptionProduct[i].ProductId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProduct[i].Subject))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subject` = @subject" + i);
                                        else
                                            sql = string.Concat(sql, " `subject` = @subject" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subject" + i, subscriptionProduct[i].Subject);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProduct[i].SubjectName))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subject_name` = @subjectName" + i);
                                        else
                                            sql = string.Concat(sql, " `subject_name` = @subjectName" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectName" + i, subscriptionProduct[i].SubjectName);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProduct[i].Level))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `level` = @level" + i);
                                        else
                                            sql = string.Concat(sql, " `level` = @level" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@level" + i, subscriptionProduct[i].Level);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProduct[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionProduct[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProduct[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionProduct[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProduct[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionProduct[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(subscriptionProduct[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<SubscriptionProductModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SubscriptionProductModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProductModel>();
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
