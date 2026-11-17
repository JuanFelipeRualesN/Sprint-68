using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class ProductItemProductDAO
    {
        private MySqlConnection _connection;
        public List<ProductItemProductModel> Read(string connectionString, ProductItemProductModel productItemProduct, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductItemProductModel> response = new List<ProductItemProductModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `item_id`, `product_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_product_item_products ";
                {
                    if (productItemProduct != null)
                    {
                        if (!string.IsNullOrEmpty(productItemProduct.Id))
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
                            cmd.Parameters.AddWithValue("@id", productItemProduct.Id);
                        }
                        if (!string.IsNullOrEmpty(productItemProduct.ItemId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `item_id` = @itemId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `item_id` = @itemId ");
                            }
                            cmd.Parameters.AddWithValue("@itemId", productItemProduct.ItemId);
                        }
                        if (!string.IsNullOrEmpty(productItemProduct.ProductId))
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
                            cmd.Parameters.AddWithValue("@productId", productItemProduct.ProductId);
                        }
                        if (!string.IsNullOrEmpty(productItemProduct.Available))
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
                            cmd.Parameters.AddWithValue("@available", productItemProduct.Available);
                        }
                        if (!string.IsNullOrEmpty(productItemProduct.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", productItemProduct.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(productItemProduct.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", productItemProduct.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(productItemProduct.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", productItemProduct.UpdatedAt);
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
                                    productItemProduct.Id = reader.GetValue(0).ToString();
                                else
                                    productItemProduct.Id = null;
                                if (!reader.IsDBNull(1))
                                    productItemProduct.ItemId = reader.GetValue(1).ToString();
                                else
                                    productItemProduct.ItemId = null;
                                if (!reader.IsDBNull(2))
                                    productItemProduct.ProductId = reader.GetValue(2).ToString();
                                else
                                    productItemProduct.ProductId = null;
                                if (!reader.IsDBNull(3))
                                    productItemProduct.Available = reader.GetValue(3).ToString();
                                else
                                    productItemProduct.Available = null;
                                if (!reader.IsDBNull(4))
                                    productItemProduct.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    productItemProduct.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    productItemProduct.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    productItemProduct.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    productItemProduct.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    productItemProduct.UpdatedAt = null;
                                response.Add(productItemProduct);
                                productItemProduct = new ProductItemProductModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ProductItemProductModel>();
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
                response = new List<ProductItemProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ProductItemProductModel> Read(string connectionString, List<ProductItemProductModel> productItemProductWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductItemProductModel> response = new List<ProductItemProductModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `item_id`, `product_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_product_item_products ";
                if (productItemProductWhere != null && productItemProductWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < productItemProductWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(productItemProductWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, productItemProductWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(productItemProductWhere[i].ItemId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `item_id` = @itemId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@itemId" + i, productItemProductWhere[i].ItemId);
                        }
                        if (!string.IsNullOrEmpty(productItemProductWhere[i].ProductId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `product_id` = @productId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@productId" + i, productItemProductWhere[i].ProductId);
                        }
                        if (!string.IsNullOrEmpty(productItemProductWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, productItemProductWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(productItemProductWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, productItemProductWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(productItemProductWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, productItemProductWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(productItemProductWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, productItemProductWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != productItemProductWhere.Count - 1)
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
                            ProductItemProductModel productItemProduct = new ProductItemProductModel(); if (!reader.IsDBNull(0))
                                productItemProduct.Id = reader.GetValue(0).ToString();
                            else
                                productItemProduct.Id = null;
                            if (!reader.IsDBNull(1))
                                productItemProduct.ItemId = reader.GetValue(1).ToString();
                            else
                                productItemProduct.ItemId = null;
                            if (!reader.IsDBNull(2))
                                productItemProduct.ProductId = reader.GetValue(2).ToString();
                            else
                                productItemProduct.ProductId = null;
                            if (!reader.IsDBNull(3))
                                productItemProduct.Available = reader.GetValue(3).ToString();
                            else
                                productItemProduct.Available = null;
                            if (!reader.IsDBNull(4))
                                productItemProduct.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                productItemProduct.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                productItemProduct.CreatedAt = reader.GetValue(5).ToString();
                            else
                                productItemProduct.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                productItemProduct.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                productItemProduct.UpdatedAt = null;
                            response.Add(productItemProduct);
                            productItemProduct = new ProductItemProductModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<ProductItemProductModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ProductItemProductModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ProductItemProductModel> Create(string connectionString, ProductItemProductModel productItemProduct)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductItemProductModel> response = new List<ProductItemProductModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO subst_product_item_products (";
                string values = "(";
                if (!string.IsNullOrEmpty(productItemProduct.Id))
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
                    cmd.Parameters.AddWithValue("@id", productItemProduct.Id);
                }
                if (!string.IsNullOrEmpty(productItemProduct.ItemId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `item_id` ");
                        values = string.Concat(values, ", @itemId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `item_id` ");
                        values = string.Concat(values, " @itemId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@itemId", productItemProduct.ItemId);
                }
                if (!string.IsNullOrEmpty(productItemProduct.ProductId))
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
                    cmd.Parameters.AddWithValue("@productId", productItemProduct.ProductId);
                }
                if (!string.IsNullOrEmpty(productItemProduct.Available))
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
                    cmd.Parameters.AddWithValue("@available", productItemProduct.Available);
                }
                if (!string.IsNullOrEmpty(productItemProduct.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", productItemProduct.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(productItemProduct.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", productItemProduct.CreatedAt);
                }
                if (!string.IsNullOrEmpty(productItemProduct.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", productItemProduct.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                productItemProduct.Id = cmd.LastInsertedId.ToString();
                response.Add(productItemProduct);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ProductItemProductModel>();
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
        public List<ProductItemProductModel> Create(string connectionString, List<ProductItemProductModel> productItemProductList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductItemProductModel> response = new List<ProductItemProductModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < productItemProductList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_product_item_products (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(productItemProductList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, productItemProductList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(productItemProductList[i].ItemId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `item_id` ");
                                        values = string.Concat(values, ", @itemId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `item_id` ");
                                        values = string.Concat(values, "@itemId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@itemId" + i, productItemProductList[i].ItemId);
                                }
                                if (!string.IsNullOrEmpty(productItemProductList[i].ProductId))
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
                                    cmd.Parameters.AddWithValue("@productId" + i, productItemProductList[i].ProductId);
                                }
                                if (!string.IsNullOrEmpty(productItemProductList[i].Available))
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
                                    cmd.Parameters.AddWithValue("@available" + i, productItemProductList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(productItemProductList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, productItemProductList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(productItemProductList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, productItemProductList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(productItemProductList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, productItemProductList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(productItemProductList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<ProductItemProductModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ProductItemProductModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ProductItemProductModel>();
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

        public List<ProductItemProductModel> Update(string connectionString, ProductItemProductModel productItemProduct, ProductItemProductModel productItemProductWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductItemProductModel> response = new List<ProductItemProductModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE subst_product_item_products SET ";
                if (!string.IsNullOrEmpty(productItemProduct.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", productItemProduct.Id);
                }
                if (!string.IsNullOrEmpty(productItemProduct.ItemId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `item_id` = @itemId");
                    else
                        sql = string.Concat(sql, " `item_id` = @itemId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@itemId", productItemProduct.ItemId);
                }
                if (!string.IsNullOrEmpty(productItemProduct.ProductId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `product_id` = @productId");
                    else
                        sql = string.Concat(sql, " `product_id` = @productId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@productId", productItemProduct.ProductId);
                }
                if (!string.IsNullOrEmpty(productItemProduct.Available))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `available` = @available");
                    else
                        sql = string.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", productItemProduct.Available);
                }
                if (!string.IsNullOrEmpty(productItemProduct.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", productItemProduct.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(productItemProduct.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", productItemProduct.CreatedAt);
                }
                if (!string.IsNullOrEmpty(productItemProduct.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", productItemProduct.UpdatedAt);
                }
                if (productItemProductWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(productItemProductWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", productItemProductWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(productItemProductWhere.ItemId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `item_id` = @itemIdWhere");
                        else
                            sql = string.Concat(sql, " `item_id` = @itemIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@itemIdWhere", productItemProductWhere.ItemId);
                    }
                    if (!string.IsNullOrEmpty(productItemProductWhere.ProductId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `product_id` = @productIdWhere");
                        else
                            sql = string.Concat(sql, " `product_id` = @productIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@productIdWhere", productItemProductWhere.ProductId);
                    }
                    if (!string.IsNullOrEmpty(productItemProductWhere.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = string.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", productItemProductWhere.Available);
                    }
                    if (!string.IsNullOrEmpty(productItemProductWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", productItemProductWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(productItemProductWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", productItemProductWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(productItemProductWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", productItemProductWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(productItemProduct);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ProductItemProductModel>();
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

        public List<ProductItemProductModel> Update(string connectionString, List<ProductItemProductModel> productItemProductList, List<ProductItemProductModel> productItemProductWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductItemProductModel> response = new List<ProductItemProductModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < productItemProductList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_product_item_products SET ";
                                if (!string.IsNullOrEmpty(productItemProductList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, productItemProductList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(productItemProductList[i].ItemId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `item_id` = @itemId" + i);
                                    else
                                        sql = string.Concat(sql, " `item_id` = @itemId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@itemId" + i, productItemProductList[i].ItemId);
                                }
                                if (!string.IsNullOrEmpty(productItemProductList[i].ProductId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `product_id` = @productId" + i);
                                    else
                                        sql = string.Concat(sql, " `product_id` = @productId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@productId" + i, productItemProductList[i].ProductId);
                                }
                                if (!string.IsNullOrEmpty(productItemProductList[i].Available))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = string.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, productItemProductList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(productItemProductList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, productItemProductList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(productItemProductList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, productItemProductList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(productItemProductList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, productItemProductList[i].UpdatedAt);
                                }
                                if (productItemProductWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(productItemProductWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, productItemProductWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(productItemProductWhereList[i].ItemId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `item_id` = @itemIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `item_id` = @itemIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@itemIdWhere" + i, productItemProductWhereList[i].ItemId);
                                    }
                                    if (!string.IsNullOrEmpty(productItemProductWhereList[i].ProductId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `product_id` = @productIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `product_id` = @productIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@productIdWhere" + i, productItemProductWhereList[i].ProductId);
                                    }
                                    if (!string.IsNullOrEmpty(productItemProductWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, productItemProductWhereList[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(productItemProductWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, productItemProductWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(productItemProductWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, productItemProductWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(productItemProductWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, productItemProductWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(productItemProductList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ProductItemProductModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ProductItemProductModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ProductItemProductModel>();
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

        public List<ProductItemProductModel> Delete(string connectionString, ProductItemProductModel productItemProduct)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductItemProductModel> response = new List<ProductItemProductModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM subst_product_item_products  ";
                if (productItemProduct != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(productItemProduct.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", productItemProduct.Id);
                    }
                    if (!string.IsNullOrEmpty(productItemProduct.ItemId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `item_id` = @itemId");
                        else
                            sql = string.Concat(sql, " `item_id` = @itemId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@itemId", productItemProduct.ItemId);
                    }
                    if (!string.IsNullOrEmpty(productItemProduct.ProductId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `product_id` = @productId");
                        else
                            sql = string.Concat(sql, " `product_id` = @productId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@productId", productItemProduct.ProductId);
                    }
                    if (!string.IsNullOrEmpty(productItemProduct.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @available");
                        else
                            sql = string.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", productItemProduct.Available);
                    }
                    if (!string.IsNullOrEmpty(productItemProduct.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", productItemProduct.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(productItemProduct.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", productItemProduct.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(productItemProduct.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", productItemProduct.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(productItemProduct);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ProductItemProductModel>();
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

        public List<ProductItemProductModel> Delete(string connectionString, List<ProductItemProductModel> productItemProduct)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ProductItemProductModel> response = new List<ProductItemProductModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < productItemProduct.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_product_item_products  ";
                                if (productItemProduct[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(productItemProduct[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, productItemProduct[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(productItemProduct[i].ItemId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `item_id` = @itemId" + i);
                                        else
                                            sql = string.Concat(sql, " `item_id` = @itemId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@itemId" + i, productItemProduct[i].ItemId);
                                    }
                                    if (!string.IsNullOrEmpty(productItemProduct[i].ProductId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `product_id` = @productId" + i);
                                        else
                                            sql = string.Concat(sql, " `product_id` = @productId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@productId" + i, productItemProduct[i].ProductId);
                                    }
                                    if (!string.IsNullOrEmpty(productItemProduct[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, productItemProduct[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(productItemProduct[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, productItemProduct[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(productItemProduct[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, productItemProduct[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(productItemProduct[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, productItemProduct[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(productItemProduct[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ProductItemProductModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ProductItemProductModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ProductItemProductModel>();
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
