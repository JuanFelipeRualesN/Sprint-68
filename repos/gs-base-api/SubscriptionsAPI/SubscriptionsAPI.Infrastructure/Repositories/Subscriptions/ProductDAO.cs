using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class ProductDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, ProductModel> ReadD(string connectionString, ProductModel product, bool firstOne, bool orderByLastDate, ProductModel productWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ProductModel> response = new Dictionary<string, ProductModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `platform_id`, `name`, `is_commercial`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_products ";
                {
                    if (product != null)
                    {
                        if (!String.IsNullOrEmpty(product.Id))
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
                            cmd.Parameters.AddWithValue("@id", product.Id);
                        }
                        if (!String.IsNullOrEmpty(product.PlatformId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `platform_id` = @platformId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `platform_id` = @platformId ");
                            }
                            cmd.Parameters.AddWithValue("@platformId", product.PlatformId);
                        }
                        if (!String.IsNullOrEmpty(product.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", product.Name);
                        }
                        if (!String.IsNullOrEmpty(product.IsCommercial))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `is_commercial` = @isCommercial ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `is_commercial` = @isCommercial ");
                            }
                            cmd.Parameters.AddWithValue("@isCommercial", product.IsCommercial);
                        }
                        if (!String.IsNullOrEmpty(product.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", product.Available);
                        }
                        if (!String.IsNullOrEmpty(product.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", product.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(product.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", product.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(product.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", product.UpdatedAt);
                        }
                    }
                    if (productWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(productWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + productWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.PlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `platform_id` LIKE @platformIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `platform_id` LIKE @platformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformIdWhereLike", "%" + productWhereLike.PlatformId + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + productWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.IsCommercial))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `is_commercial` LIKE @isCommercialWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `is_commercial` LIKE @isCommercialWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isCommercialWhereLike", "%" + productWhereLike.IsCommercial + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.Available))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `available` LIKE @availableWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `available` LIKE @availableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + productWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + productWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + productWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + productWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = String.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                product = new ProductModel();
                                if (!reader.IsDBNull(0))
                                    product.Id = reader.GetValue(0).ToString();
                                else
                                    product.Id = null;
                                if (!reader.IsDBNull(1))
                                    product.PlatformId = reader.GetValue(1).ToString();
                                else
                                    product.PlatformId = null;
                                if (!reader.IsDBNull(2))
                                    product.Name = reader.GetValue(2).ToString();
                                else
                                    product.Name = null;
                                if (!reader.IsDBNull(3))
                                    product.IsCommercial = reader.GetValue(3).ToString();
                                else
                                    product.IsCommercial = null;
                                if (!reader.IsDBNull(4))
                                    product.Available = reader.GetValue(4).ToString();
                                else
                                    product.Available = null;
                                if (!reader.IsDBNull(5))
                                    product.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    product.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    product.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    product.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    product.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    product.UpdatedAt = null;
                                response.Add(product.Id, product);
                                product = new ProductModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, ProductModel>();
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
                response = new Dictionary<string, ProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ProductModel> ReadD(string connectionString, ProductModel product, bool orderByLastDate, int fromRow, int limit, ProductModel productWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ProductModel> response = new Dictionary<string, ProductModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `platform_id`, `name`, `is_commercial`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_products ";
                {
                    if (product != null)
                    {
                        if (!String.IsNullOrEmpty(product.Id))
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
                            cmd.Parameters.AddWithValue("@id", product.Id);
                        }
                        if (!String.IsNullOrEmpty(product.PlatformId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `platform_id` = @platformId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `platform_id` = @platformId ");
                            }
                            cmd.Parameters.AddWithValue("@platformId", product.PlatformId);
                        }
                        if (!String.IsNullOrEmpty(product.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", product.Name);
                        }
                        if (!String.IsNullOrEmpty(product.IsCommercial))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `is_commercial` = @isCommercial ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `is_commercial` = @isCommercial ");
                            }
                            cmd.Parameters.AddWithValue("@isCommercial", product.IsCommercial);
                        }
                        if (!String.IsNullOrEmpty(product.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", product.Available);
                        }
                        if (!String.IsNullOrEmpty(product.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", product.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(product.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", product.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(product.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", product.UpdatedAt);
                        }
                    }
                    if (productWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(productWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + productWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.PlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `platform_id` LIKE @platformIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `platform_id` LIKE @platformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformIdWhereLike", "%" + productWhereLike.PlatformId + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + productWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.IsCommercial))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `is_commercial` LIKE @isCommercialWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `is_commercial` LIKE @isCommercialWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isCommercialWhereLike", "%" + productWhereLike.IsCommercial + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.Available))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `available` LIKE @availableWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `available` LIKE @availableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + productWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + productWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + productWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + productWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    sql = String.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                product = new ProductModel();
                                if (!reader.IsDBNull(0))
                                    product.Id = reader.GetValue(0).ToString();
                                else
                                    product.Id = null;
                                if (!reader.IsDBNull(1))
                                    product.PlatformId = reader.GetValue(1).ToString();
                                else
                                    product.PlatformId = null;
                                if (!reader.IsDBNull(2))
                                    product.Name = reader.GetValue(2).ToString();
                                else
                                    product.Name = null;
                                if (!reader.IsDBNull(3))
                                    product.IsCommercial = reader.GetValue(3).ToString();
                                else
                                    product.IsCommercial = null;
                                if (!reader.IsDBNull(4))
                                    product.Available = reader.GetValue(4).ToString();
                                else
                                    product.Available = null;
                                if (!reader.IsDBNull(5))
                                    product.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    product.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    product.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    product.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    product.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    product.UpdatedAt = null;
                                response.Add(product.Id, product);
                                product = new ProductModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, ProductModel>();
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
                response = new Dictionary<string, ProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ProductModel> ReadD(string connectionString, List<ProductModel> productWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ProductModel> response = new Dictionary<string, ProductModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `platform_id`, `name`, `is_commercial`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_products ";
                if (productWhere != null && productWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < productWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(productWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, productWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, productWhere[i].PlatformId);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, productWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].IsCommercial))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_commercial` = @isCommercial" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isCommercial" + i, productWhere[i].IsCommercial);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, productWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, productWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, productWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, productWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != productWhere.Count - 1)
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
                            ProductModel product = new ProductModel(); if (!reader.IsDBNull(0))
                                product.Id = reader.GetValue(0).ToString();
                            else
                                product.Id = null;
                            if (!reader.IsDBNull(1))
                                product.PlatformId = reader.GetValue(1).ToString();
                            else
                                product.PlatformId = null;
                            if (!reader.IsDBNull(2))
                                product.Name = reader.GetValue(2).ToString();
                            else
                                product.Name = null;
                            if (!reader.IsDBNull(3))
                                product.IsCommercial = reader.GetValue(3).ToString();
                            else
                                product.IsCommercial = null;
                            if (!reader.IsDBNull(4))
                                product.Available = reader.GetValue(4).ToString();
                            else
                                product.Available = null;
                            if (!reader.IsDBNull(5))
                                product.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                product.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                product.CreatedAt = reader.GetValue(6).ToString();
                            else
                                product.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                product.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                product.UpdatedAt = null;
                            response.Add(product.Id, product);
                            product = new ProductModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, ProductModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, ProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ProductModel> ReadD(string connectionString, List<ProductModel> productWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ProductModel> response = new Dictionary<string, ProductModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `platform_id`, `name`, `is_commercial`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_products ";
                if (productWhere != null && productWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < productWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(productWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, productWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, productWhere[i].PlatformId);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, productWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].IsCommercial))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_commercial` = @isCommercial" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isCommercial" + i, productWhere[i].IsCommercial);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, productWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, productWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, productWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, productWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != productWhere.Count - 1)
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
                            ProductModel product = new ProductModel(); if (!reader.IsDBNull(0))
                                product.Id = reader.GetValue(0).ToString();
                            else
                                product.Id = null;
                            if (!reader.IsDBNull(1))
                                product.PlatformId = reader.GetValue(1).ToString();
                            else
                                product.PlatformId = null;
                            if (!reader.IsDBNull(2))
                                product.Name = reader.GetValue(2).ToString();
                            else
                                product.Name = null;
                            if (!reader.IsDBNull(3))
                                product.IsCommercial = reader.GetValue(3).ToString();
                            else
                                product.IsCommercial = null;
                            if (!reader.IsDBNull(4))
                                product.Available = reader.GetValue(4).ToString();
                            else
                                product.Available = null;
                            if (!reader.IsDBNull(5))
                                product.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                product.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                product.CreatedAt = reader.GetValue(6).ToString();
                            else
                                product.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                product.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                product.UpdatedAt = null;
                            response.Add(product.Id, product);
                            product = new ProductModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, ProductModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, ProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ProductModel> Read(string connectionString, ProductModel product, bool firstOne, bool orderByLastDate, ProductModel productWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductModel> response = new List<ProductModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `platform_id`, `name`, `is_commercial`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_products ";
                {
                    if (product != null)
                    {
                        if (!String.IsNullOrEmpty(product.Id))
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
                            cmd.Parameters.AddWithValue("@id", product.Id);
                        }
                        if (!String.IsNullOrEmpty(product.PlatformId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `platform_id` = @platformId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `platform_id` = @platformId ");
                            }
                            cmd.Parameters.AddWithValue("@platformId", product.PlatformId);
                        }
                        if (!String.IsNullOrEmpty(product.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", product.Name);
                        }
                        if (!String.IsNullOrEmpty(product.IsCommercial))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `is_commercial` = @isCommercial ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `is_commercial` = @isCommercial ");
                            }
                            cmd.Parameters.AddWithValue("@isCommercial", product.IsCommercial);
                        }
                        if (!String.IsNullOrEmpty(product.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", product.Available);
                        }
                        if (!String.IsNullOrEmpty(product.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", product.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(product.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", product.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(product.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", product.UpdatedAt);
                        }
                    }
                    if (productWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(productWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + productWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.PlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `platform_id` LIKE @platformIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `platform_id` LIKE @platformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformIdWhereLike", "%" + productWhereLike.PlatformId + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + productWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.IsCommercial))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `is_commercial` LIKE @isCommercialWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `is_commercial` LIKE @isCommercialWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isCommercialWhereLike", "%" + productWhereLike.IsCommercial + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.Available))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `available` LIKE @availableWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `available` LIKE @availableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + productWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + productWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + productWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + productWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = String.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                product = new ProductModel();
                                if (!reader.IsDBNull(0))
                                    product.Id = reader.GetValue(0).ToString();
                                else
                                    product.Id = null;
                                if (!reader.IsDBNull(1))
                                    product.PlatformId = reader.GetValue(1).ToString();
                                else
                                    product.PlatformId = null;
                                if (!reader.IsDBNull(2))
                                    product.Name = reader.GetValue(2).ToString();
                                else
                                    product.Name = null;
                                if (!reader.IsDBNull(3))
                                    product.IsCommercial = reader.GetValue(3).ToString();
                                else
                                    product.IsCommercial = null;
                                if (!reader.IsDBNull(4))
                                    product.Available = reader.GetValue(4).ToString();
                                else
                                    product.Available = null;
                                if (!reader.IsDBNull(5))
                                    product.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    product.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    product.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    product.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    product.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    product.UpdatedAt = null;
                                response.Add(product);
                                product = new ProductModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ProductModel>();
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
                response = new List<ProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ProductModel> Read(string connectionString, ProductModel product, bool orderByLastDate, int fromRow, int limit, ProductModel productWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductModel> response = new List<ProductModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `platform_id`, `name`, `is_commercial`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_products ";
                {
                    if (product != null)
                    {
                        if (!String.IsNullOrEmpty(product.Id))
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
                            cmd.Parameters.AddWithValue("@id", product.Id);
                        }
                        if (!String.IsNullOrEmpty(product.PlatformId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `platform_id` = @platformId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `platform_id` = @platformId ");
                            }
                            cmd.Parameters.AddWithValue("@platformId", product.PlatformId);
                        }
                        if (!String.IsNullOrEmpty(product.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", product.Name);
                        }
                        if (!String.IsNullOrEmpty(product.IsCommercial))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `is_commercial` = @isCommercial ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `is_commercial` = @isCommercial ");
                            }
                            cmd.Parameters.AddWithValue("@isCommercial", product.IsCommercial);
                        }
                        if (!String.IsNullOrEmpty(product.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", product.Available);
                        }
                        if (!String.IsNullOrEmpty(product.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", product.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(product.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", product.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(product.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", product.UpdatedAt);
                        }
                    }
                    if (productWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(productWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + productWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.PlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `platform_id` LIKE @platformIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `platform_id` LIKE @platformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformIdWhereLike", "%" + productWhereLike.PlatformId + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + productWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.IsCommercial))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `is_commercial` LIKE @isCommercialWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `is_commercial` LIKE @isCommercialWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isCommercialWhereLike", "%" + productWhereLike.IsCommercial + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.Available))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `available` LIKE @availableWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `available` LIKE @availableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + productWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + productWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + productWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(productWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + productWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    sql = String.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                product = new ProductModel();
                                if (!reader.IsDBNull(0))
                                    product.Id = reader.GetValue(0).ToString();
                                else
                                    product.Id = null;
                                if (!reader.IsDBNull(1))
                                    product.PlatformId = reader.GetValue(1).ToString();
                                else
                                    product.PlatformId = null;
                                if (!reader.IsDBNull(2))
                                    product.Name = reader.GetValue(2).ToString();
                                else
                                    product.Name = null;
                                if (!reader.IsDBNull(3))
                                    product.IsCommercial = reader.GetValue(3).ToString();
                                else
                                    product.IsCommercial = null;
                                if (!reader.IsDBNull(4))
                                    product.Available = reader.GetValue(4).ToString();
                                else
                                    product.Available = null;
                                if (!reader.IsDBNull(5))
                                    product.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    product.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    product.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    product.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    product.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    product.UpdatedAt = null;
                                response.Add(product);
                                product = new ProductModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ProductModel>();
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
                response = new List<ProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ProductModel> Read(string connectionString, List<ProductModel> productWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductModel> response = new List<ProductModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `platform_id`, `name`, `is_commercial`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_products ";
                if (productWhere != null && productWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < productWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(productWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, productWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, productWhere[i].PlatformId);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, productWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].IsCommercial))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_commercial` = @isCommercial" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isCommercial" + i, productWhere[i].IsCommercial);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, productWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, productWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, productWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, productWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != productWhere.Count - 1)
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
                            ProductModel product = new ProductModel(); if (!reader.IsDBNull(0))
                                product.Id = reader.GetValue(0).ToString();
                            else
                                product.Id = null;
                            if (!reader.IsDBNull(1))
                                product.PlatformId = reader.GetValue(1).ToString();
                            else
                                product.PlatformId = null;
                            if (!reader.IsDBNull(2))
                                product.Name = reader.GetValue(2).ToString();
                            else
                                product.Name = null;
                            if (!reader.IsDBNull(3))
                                product.IsCommercial = reader.GetValue(3).ToString();
                            else
                                product.IsCommercial = null;
                            if (!reader.IsDBNull(4))
                                product.Available = reader.GetValue(4).ToString();
                            else
                                product.Available = null;
                            if (!reader.IsDBNull(5))
                                product.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                product.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                product.CreatedAt = reader.GetValue(6).ToString();
                            else
                                product.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                product.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                product.UpdatedAt = null;
                            response.Add(product);
                            product = new ProductModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<ProductModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ProductModel> Read(string connectionString, List<ProductModel> productWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductModel> response = new List<ProductModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `platform_id`, `name`, `is_commercial`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_products ";
                if (productWhere != null && productWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < productWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(productWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, productWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, productWhere[i].PlatformId);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, productWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].IsCommercial))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_commercial` = @isCommercial" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isCommercial" + i, productWhere[i].IsCommercial);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, productWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, productWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, productWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(productWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, productWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != productWhere.Count - 1)
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
                            ProductModel product = new ProductModel(); if (!reader.IsDBNull(0))
                                product.Id = reader.GetValue(0).ToString();
                            else
                                product.Id = null;
                            if (!reader.IsDBNull(1))
                                product.PlatformId = reader.GetValue(1).ToString();
                            else
                                product.PlatformId = null;
                            if (!reader.IsDBNull(2))
                                product.Name = reader.GetValue(2).ToString();
                            else
                                product.Name = null;
                            if (!reader.IsDBNull(3))
                                product.IsCommercial = reader.GetValue(3).ToString();
                            else
                                product.IsCommercial = null;
                            if (!reader.IsDBNull(4))
                                product.Available = reader.GetValue(4).ToString();
                            else
                                product.Available = null;
                            if (!reader.IsDBNull(5))
                                product.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                product.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                product.CreatedAt = reader.GetValue(6).ToString();
                            else
                                product.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                product.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                product.UpdatedAt = null;
                            response.Add(product);
                            product = new ProductModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<ProductModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ProductModel> Create(string connectionString, ProductModel product)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductModel> response = new List<ProductModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO subst_products (";
                string values = "(";
                if (!String.IsNullOrEmpty(product.Id))
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
                    cmd.Parameters.AddWithValue("@id", product.Id);
                }
                if (!String.IsNullOrEmpty(product.PlatformId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `platform_id` ");
                        values = String.Concat(values, ", @platformId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `platform_id` ");
                        values = String.Concat(values, " @platformId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@platformId", product.PlatformId);
                }
                if (!String.IsNullOrEmpty(product.Name))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `name` ");
                        values = String.Concat(values, ", @name");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `name` ");
                        values = String.Concat(values, " @name");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", product.Name);
                }
                if (!String.IsNullOrEmpty(product.IsCommercial))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `is_commercial` ");
                        values = String.Concat(values, ", @isCommercial");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `is_commercial` ");
                        values = String.Concat(values, " @isCommercial");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isCommercial", product.IsCommercial);
                }
                if (!String.IsNullOrEmpty(product.Available))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `available` ");
                        values = String.Concat(values, ", @available");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `available` ");
                        values = String.Concat(values, " @available");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", product.Available);
                }
                if (!String.IsNullOrEmpty(product.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", product.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(product.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", product.CreatedAt);
                }
                if (!String.IsNullOrEmpty(product.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", product.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                product.Id = cmd.LastInsertedId.ToString();
                response.Add(product);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ProductModel>();
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
        public List<ProductModel> Create(string connectionString, List<ProductModel> productList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductModel> response = new List<ProductModel>();
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
                        for (int i = 0; i < productList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_products (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(productList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, productList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(productList[i].PlatformId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `platform_id` ");
                                        values = String.Concat(values, ", @platformId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `platform_id` ");
                                        values = String.Concat(values, "@platformId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@platformId" + i, productList[i].PlatformId);
                                }
                                if (!String.IsNullOrEmpty(productList[i].Name))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `name` ");
                                        values = String.Concat(values, ", @name" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `name` ");
                                        values = String.Concat(values, "@name" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, productList[i].Name);
                                }
                                if (!String.IsNullOrEmpty(productList[i].IsCommercial))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `is_commercial` ");
                                        values = String.Concat(values, ", @isCommercial" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `is_commercial` ");
                                        values = String.Concat(values, "@isCommercial" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isCommercial" + i, productList[i].IsCommercial);
                                }
                                if (!String.IsNullOrEmpty(productList[i].Available))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `available` ");
                                        values = String.Concat(values, ", @available" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `available` ");
                                        values = String.Concat(values, "@available" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, productList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(productList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, productList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(productList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, productList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(productList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, productList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(productList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<ProductModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ProductModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ProductModel>();
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

        public List<ProductModel> Update(string connectionString, ProductModel product, ProductModel productWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductModel> response = new List<ProductModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE subst_products SET ";
                if (!String.IsNullOrEmpty(product.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", product.Id);
                }
                if (!String.IsNullOrEmpty(product.PlatformId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `platform_id` = @platformId");
                    else
                        sql = String.Concat(sql, " `platform_id` = @platformId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@platformId", product.PlatformId);
                }
                if (!String.IsNullOrEmpty(product.Name))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `name` = @name");
                    else
                        sql = String.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", product.Name);
                }
                if (!String.IsNullOrEmpty(product.IsCommercial))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `is_commercial` = @isCommercial");
                    else
                        sql = String.Concat(sql, " `is_commercial` = @isCommercial");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isCommercial", product.IsCommercial);
                }
                if (!String.IsNullOrEmpty(product.Available))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `available` = @available");
                    else
                        sql = String.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", product.Available);
                }
                if (!String.IsNullOrEmpty(product.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", product.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(product.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", product.CreatedAt);
                }
                if (!String.IsNullOrEmpty(product.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", product.UpdatedAt);
                }
                if (productWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(productWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", productWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(productWhere.PlatformId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `platform_id` = @platformIdWhere");
                        else
                            sql = String.Concat(sql, " `platform_id` = @platformIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformIdWhere", productWhere.PlatformId);
                    }
                    if (!String.IsNullOrEmpty(productWhere.Name))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = String.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", productWhere.Name);
                    }
                    if (!String.IsNullOrEmpty(productWhere.IsCommercial))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `is_commercial` = @isCommercialWhere");
                        else
                            sql = String.Concat(sql, " `is_commercial` = @isCommercialWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isCommercialWhere", productWhere.IsCommercial);
                    }
                    if (!String.IsNullOrEmpty(productWhere.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = String.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", productWhere.Available);
                    }
                    if (!String.IsNullOrEmpty(productWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", productWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(productWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", productWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(productWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", productWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(product);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ProductModel>();
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

        public List<ProductModel> Update(string connectionString, List<ProductModel> productList, List<ProductModel> productWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductModel> response = new List<ProductModel>();
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
                        for (int i = 0; i < productList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_products SET ";
                                if (!String.IsNullOrEmpty(productList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, productList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(productList[i].PlatformId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `platform_id` = @platformId" + i);
                                    else
                                        sql = String.Concat(sql, " `platform_id` = @platformId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@platformId" + i, productList[i].PlatformId);
                                }
                                if (!String.IsNullOrEmpty(productList[i].Name))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = String.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, productList[i].Name);
                                }
                                if (!String.IsNullOrEmpty(productList[i].IsCommercial))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `is_commercial` = @isCommercial" + i);
                                    else
                                        sql = String.Concat(sql, " `is_commercial` = @isCommercial" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isCommercial" + i, productList[i].IsCommercial);
                                }
                                if (!String.IsNullOrEmpty(productList[i].Available))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = String.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, productList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(productList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, productList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(productList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, productList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(productList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, productList[i].UpdatedAt);
                                }
                                if (productWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(productWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, productWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(productWhereList[i].PlatformId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `platform_id` = @platformIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `platform_id` = @platformIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformIdWhere" + i, productWhereList[i].PlatformId);
                                    }
                                    if (!String.IsNullOrEmpty(productWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, productWhereList[i].Name);
                                    }
                                    if (!String.IsNullOrEmpty(productWhereList[i].IsCommercial))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `is_commercial` = @isCommercialWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `is_commercial` = @isCommercialWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isCommercialWhere" + i, productWhereList[i].IsCommercial);
                                    }
                                    if (!String.IsNullOrEmpty(productWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, productWhereList[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(productWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, productWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(productWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, productWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(productWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, productWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(productList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ProductModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ProductModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ProductModel>();
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

        public List<ProductModel> Delete(string connectionString, ProductModel product)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductModel> response = new List<ProductModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM subst_products  ";
                if (product != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(product.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", product.Id);
                    }
                    if (!String.IsNullOrEmpty(product.PlatformId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `platform_id` = @platformId");
                        else
                            sql = String.Concat(sql, " `platform_id` = @platformId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformId", product.PlatformId);
                    }
                    if (!String.IsNullOrEmpty(product.Name))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `name` = @name");
                        else
                            sql = String.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", product.Name);
                    }
                    if (!String.IsNullOrEmpty(product.IsCommercial))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `is_commercial` = @isCommercial");
                        else
                            sql = String.Concat(sql, " `is_commercial` = @isCommercial");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isCommercial", product.IsCommercial);
                    }
                    if (!String.IsNullOrEmpty(product.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @available");
                        else
                            sql = String.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", product.Available);
                    }
                    if (!String.IsNullOrEmpty(product.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", product.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(product.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", product.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(product.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", product.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(product);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ProductModel>();
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

        public List<ProductModel> Delete(string connectionString, List<ProductModel> product)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductModel> response = new List<ProductModel>();
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
                        for (int i = 0; i < product.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_products  ";
                                if (product[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(product[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, product[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(product[i].PlatformId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `platform_id` = @platformId" + i);
                                        else
                                            sql = String.Concat(sql, " `platform_id` = @platformId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformId" + i, product[i].PlatformId);
                                    }
                                    if (!String.IsNullOrEmpty(product[i].Name))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = String.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, product[i].Name);
                                    }
                                    if (!String.IsNullOrEmpty(product[i].IsCommercial))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `is_commercial` = @isCommercial" + i);
                                        else
                                            sql = String.Concat(sql, " `is_commercial` = @isCommercial" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isCommercial" + i, product[i].IsCommercial);
                                    }
                                    if (!String.IsNullOrEmpty(product[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, product[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(product[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, product[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(product[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, product[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(product[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, product[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(product[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ProductModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ProductModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ProductModel>();
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
