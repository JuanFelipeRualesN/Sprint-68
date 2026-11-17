using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class CustomerSourceTokenDAO
    {
        private MySqlConnection _connection;
        public List<CustomerSourceTokenModel> Read(string connectionString, CustomerSourceTokenModel customerSourceToken, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerSourceTokenModel> response = new List<CustomerSourceTokenModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `customer_platform_id`, `source_token`, `active`, `modified_by`, `created_at`, `updated_at`  FROM subst_customer_source_tokens ";
                {
                    if (customerSourceToken != null)
                    {
                        if (!string.IsNullOrEmpty(customerSourceToken.Id))
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
                            cmd.Parameters.AddWithValue("@id", customerSourceToken.Id);
                        }
                        if (!string.IsNullOrEmpty(customerSourceToken.CustomerPlatformId))
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
                            cmd.Parameters.AddWithValue("@customerPlatformId", customerSourceToken.CustomerPlatformId);
                        }
                        if (!string.IsNullOrEmpty(customerSourceToken.SourceToken))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `source_token` = @sourceToken ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `source_token` = @sourceToken ");
                            }
                            cmd.Parameters.AddWithValue("@sourceToken", customerSourceToken.SourceToken);
                        }
                        if (!string.IsNullOrEmpty(customerSourceToken.Active))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `active` = @active ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `active` = @active ");
                            }
                            cmd.Parameters.AddWithValue("@active", customerSourceToken.Active);
                        }
                        if (!string.IsNullOrEmpty(customerSourceToken.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", customerSourceToken.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(customerSourceToken.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", customerSourceToken.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(customerSourceToken.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", customerSourceToken.UpdatedAt);
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
                                    customerSourceToken.Id = reader.GetValue(0).ToString();
                                else
                                    customerSourceToken.Id = null;
                                if (!reader.IsDBNull(1))
                                    customerSourceToken.CustomerPlatformId = reader.GetValue(1).ToString();
                                else
                                    customerSourceToken.CustomerPlatformId = null;
                                if (!reader.IsDBNull(2))
                                    customerSourceToken.SourceToken = reader.GetValue(2).ToString();
                                else
                                    customerSourceToken.SourceToken = null;
                                if (!reader.IsDBNull(3))
                                    customerSourceToken.Active = reader.GetValue(3).ToString();
                                else
                                    customerSourceToken.Active = null;
                                if (!reader.IsDBNull(4))
                                    customerSourceToken.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    customerSourceToken.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    customerSourceToken.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    customerSourceToken.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    customerSourceToken.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    customerSourceToken.UpdatedAt = null;
                                response.Add(customerSourceToken);
                                customerSourceToken = new CustomerSourceTokenModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<CustomerSourceTokenModel>();
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
                response = new List<CustomerSourceTokenModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CustomerSourceTokenModel> Read(string connectionString, List<CustomerSourceTokenModel> customerSourceTokenWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerSourceTokenModel> response = new List<CustomerSourceTokenModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `customer_platform_id`, `source_token`, `active`, `modified_by`, `created_at`, `updated_at`  FROM subst_customer_source_tokens ";
                if (customerSourceTokenWhere != null && customerSourceTokenWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < customerSourceTokenWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(customerSourceTokenWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, customerSourceTokenWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(customerSourceTokenWhere[i].CustomerPlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `customer_platform_id` = @customerPlatformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customerPlatformId" + i, customerSourceTokenWhere[i].CustomerPlatformId);
                        }
                        if (!string.IsNullOrEmpty(customerSourceTokenWhere[i].SourceToken))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `source_token` = @sourceToken" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@sourceToken" + i, customerSourceTokenWhere[i].SourceToken);
                        }
                        if (!string.IsNullOrEmpty(customerSourceTokenWhere[i].Active))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `active` = @active" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@active" + i, customerSourceTokenWhere[i].Active);
                        }
                        if (!string.IsNullOrEmpty(customerSourceTokenWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, customerSourceTokenWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(customerSourceTokenWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, customerSourceTokenWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(customerSourceTokenWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, customerSourceTokenWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != customerSourceTokenWhere.Count - 1)
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
                            CustomerSourceTokenModel customerSourceToken = new CustomerSourceTokenModel(); if (!reader.IsDBNull(0))
                                customerSourceToken.Id = reader.GetValue(0).ToString();
                            else
                                customerSourceToken.Id = null;
                            if (!reader.IsDBNull(1))
                                customerSourceToken.CustomerPlatformId = reader.GetValue(1).ToString();
                            else
                                customerSourceToken.CustomerPlatformId = null;
                            if (!reader.IsDBNull(2))
                                customerSourceToken.SourceToken = reader.GetValue(2).ToString();
                            else
                                customerSourceToken.SourceToken = null;
                            if (!reader.IsDBNull(3))
                                customerSourceToken.Active = reader.GetValue(3).ToString();
                            else
                                customerSourceToken.Active = null;
                            if (!reader.IsDBNull(4))
                                customerSourceToken.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                customerSourceToken.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                customerSourceToken.CreatedAt = reader.GetValue(5).ToString();
                            else
                                customerSourceToken.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                customerSourceToken.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                customerSourceToken.UpdatedAt = null;
                            response.Add(customerSourceToken);
                            customerSourceToken = new CustomerSourceTokenModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<CustomerSourceTokenModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CustomerSourceTokenModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CustomerSourceTokenModel> Create(string connectionString, CustomerSourceTokenModel customerSourceToken)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerSourceTokenModel> response = new List<CustomerSourceTokenModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO subst_customer_source_tokens (";
                string values = "(";
                if (!string.IsNullOrEmpty(customerSourceToken.Id))
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
                    cmd.Parameters.AddWithValue("@id", customerSourceToken.Id);
                }
                if (!string.IsNullOrEmpty(customerSourceToken.CustomerPlatformId))
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
                    cmd.Parameters.AddWithValue("@customerPlatformId", customerSourceToken.CustomerPlatformId);
                }
                if (!string.IsNullOrEmpty(customerSourceToken.SourceToken))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `source_token` ");
                        values = string.Concat(values, ", @sourceToken");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `source_token` ");
                        values = string.Concat(values, " @sourceToken");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@sourceToken", customerSourceToken.SourceToken);
                }
                if (!string.IsNullOrEmpty(customerSourceToken.Active))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `active` ");
                        values = string.Concat(values, ", @active");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `active` ");
                        values = string.Concat(values, " @active");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@active", customerSourceToken.Active);
                }
                if (!string.IsNullOrEmpty(customerSourceToken.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", customerSourceToken.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(customerSourceToken.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", customerSourceToken.CreatedAt);
                }
                if (!string.IsNullOrEmpty(customerSourceToken.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", customerSourceToken.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                customerSourceToken.Id = cmd.LastInsertedId.ToString();
                response.Add(customerSourceToken);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CustomerSourceTokenModel>();
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
        public List<CustomerSourceTokenModel> Create(string connectionString, List<CustomerSourceTokenModel> customerSourceTokenList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerSourceTokenModel> response = new List<CustomerSourceTokenModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < customerSourceTokenList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_customer_source_tokens (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(customerSourceTokenList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, customerSourceTokenList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(customerSourceTokenList[i].CustomerPlatformId))
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
                                    cmd.Parameters.AddWithValue("@customerPlatformId" + i, customerSourceTokenList[i].CustomerPlatformId);
                                }
                                if (!string.IsNullOrEmpty(customerSourceTokenList[i].SourceToken))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `source_token` ");
                                        values = string.Concat(values, ", @sourceToken" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `source_token` ");
                                        values = string.Concat(values, "@sourceToken" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@sourceToken" + i, customerSourceTokenList[i].SourceToken);
                                }
                                if (!string.IsNullOrEmpty(customerSourceTokenList[i].Active))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `active` ");
                                        values = string.Concat(values, ", @active" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `active` ");
                                        values = string.Concat(values, "@active" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@active" + i, customerSourceTokenList[i].Active);
                                }
                                if (!string.IsNullOrEmpty(customerSourceTokenList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, customerSourceTokenList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(customerSourceTokenList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, customerSourceTokenList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(customerSourceTokenList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, customerSourceTokenList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(customerSourceTokenList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<CustomerSourceTokenModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CustomerSourceTokenModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CustomerSourceTokenModel>();
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

        public List<CustomerSourceTokenModel> Update(string connectionString, CustomerSourceTokenModel customerSourceToken, CustomerSourceTokenModel customerSourceTokenWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerSourceTokenModel> response = new List<CustomerSourceTokenModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE subst_customer_source_tokens SET ";
                if (!string.IsNullOrEmpty(customerSourceToken.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", customerSourceToken.Id);
                }
                if (!string.IsNullOrEmpty(customerSourceToken.CustomerPlatformId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `customer_platform_id` = @customerPlatformId");
                    else
                        sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@customerPlatformId", customerSourceToken.CustomerPlatformId);
                }
                if (!string.IsNullOrEmpty(customerSourceToken.SourceToken))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `source_token` = @sourceToken");
                    else
                        sql = string.Concat(sql, " `source_token` = @sourceToken");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@sourceToken", customerSourceToken.SourceToken);
                }
                if (!string.IsNullOrEmpty(customerSourceToken.Active))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `active` = @active");
                    else
                        sql = string.Concat(sql, " `active` = @active");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@active", customerSourceToken.Active);
                }
                if (!string.IsNullOrEmpty(customerSourceToken.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", customerSourceToken.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(customerSourceToken.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", customerSourceToken.CreatedAt);
                }
                if (!string.IsNullOrEmpty(customerSourceToken.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", customerSourceToken.UpdatedAt);
                }
                if (customerSourceTokenWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(customerSourceTokenWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", customerSourceTokenWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(customerSourceTokenWhere.CustomerPlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `customer_platform_id` = @customerPlatformIdWhere");
                        else
                            sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@customerPlatformIdWhere", customerSourceTokenWhere.CustomerPlatformId);
                    }
                    if (!string.IsNullOrEmpty(customerSourceTokenWhere.SourceToken))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `source_token` = @sourceTokenWhere");
                        else
                            sql = string.Concat(sql, " `source_token` = @sourceTokenWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@sourceTokenWhere", customerSourceTokenWhere.SourceToken);
                    }
                    if (!string.IsNullOrEmpty(customerSourceTokenWhere.Active))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `active` = @activeWhere");
                        else
                            sql = string.Concat(sql, " `active` = @activeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@activeWhere", customerSourceTokenWhere.Active);
                    }
                    if (!string.IsNullOrEmpty(customerSourceTokenWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", customerSourceTokenWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(customerSourceTokenWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", customerSourceTokenWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(customerSourceTokenWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", customerSourceTokenWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(customerSourceToken);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CustomerSourceTokenModel>();
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

        public List<CustomerSourceTokenModel> Update(string connectionString, List<CustomerSourceTokenModel> customerSourceTokenList, List<CustomerSourceTokenModel> customerSourceTokenWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerSourceTokenModel> response = new List<CustomerSourceTokenModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < customerSourceTokenList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_customer_source_tokens SET ";
                                if (!string.IsNullOrEmpty(customerSourceTokenList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, customerSourceTokenList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(customerSourceTokenList[i].CustomerPlatformId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `customer_platform_id` = @customerPlatformId" + i);
                                    else
                                        sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@customerPlatformId" + i, customerSourceTokenList[i].CustomerPlatformId);
                                }
                                if (!string.IsNullOrEmpty(customerSourceTokenList[i].SourceToken))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `source_token` = @sourceToken" + i);
                                    else
                                        sql = string.Concat(sql, " `source_token` = @sourceToken" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@sourceToken" + i, customerSourceTokenList[i].SourceToken);
                                }
                                if (!string.IsNullOrEmpty(customerSourceTokenList[i].Active))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `active` = @active" + i);
                                    else
                                        sql = string.Concat(sql, " `active` = @active" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@active" + i, customerSourceTokenList[i].Active);
                                }
                                if (!string.IsNullOrEmpty(customerSourceTokenList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, customerSourceTokenList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(customerSourceTokenList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, customerSourceTokenList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(customerSourceTokenList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, customerSourceTokenList[i].UpdatedAt);
                                }
                                if (customerSourceTokenWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(customerSourceTokenWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, customerSourceTokenWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(customerSourceTokenWhereList[i].CustomerPlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `customer_platform_id` = @customerPlatformIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@customerPlatformIdWhere" + i, customerSourceTokenWhereList[i].CustomerPlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(customerSourceTokenWhereList[i].SourceToken))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `source_token` = @sourceTokenWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `source_token` = @sourceTokenWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@sourceTokenWhere" + i, customerSourceTokenWhereList[i].SourceToken);
                                    }
                                    if (!string.IsNullOrEmpty(customerSourceTokenWhereList[i].Active))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `active` = @activeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `active` = @activeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@activeWhere" + i, customerSourceTokenWhereList[i].Active);
                                    }
                                    if (!string.IsNullOrEmpty(customerSourceTokenWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, customerSourceTokenWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(customerSourceTokenWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, customerSourceTokenWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(customerSourceTokenWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, customerSourceTokenWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(customerSourceTokenList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CustomerSourceTokenModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CustomerSourceTokenModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CustomerSourceTokenModel>();
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

        public List<CustomerSourceTokenModel> Delete(string connectionString, CustomerSourceTokenModel customerSourceToken)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerSourceTokenModel> response = new List<CustomerSourceTokenModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM subst_customer_source_tokens  ";
                if (customerSourceToken != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(customerSourceToken.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", customerSourceToken.Id);
                    }
                    if (!string.IsNullOrEmpty(customerSourceToken.CustomerPlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `customer_platform_id` = @customerPlatformId");
                        else
                            sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@customerPlatformId", customerSourceToken.CustomerPlatformId);
                    }
                    if (!string.IsNullOrEmpty(customerSourceToken.SourceToken))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `source_token` = @sourceToken");
                        else
                            sql = string.Concat(sql, " `source_token` = @sourceToken");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@sourceToken", customerSourceToken.SourceToken);
                    }
                    if (!string.IsNullOrEmpty(customerSourceToken.Active))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `active` = @active");
                        else
                            sql = string.Concat(sql, " `active` = @active");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@active", customerSourceToken.Active);
                    }
                    if (!string.IsNullOrEmpty(customerSourceToken.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", customerSourceToken.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(customerSourceToken.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", customerSourceToken.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(customerSourceToken.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", customerSourceToken.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(customerSourceToken);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CustomerSourceTokenModel>();
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

        public List<CustomerSourceTokenModel> Delete(string connectionString, List<CustomerSourceTokenModel> customerSourceToken)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerSourceTokenModel> response = new List<CustomerSourceTokenModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < customerSourceToken.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_customer_source_tokens  ";
                                if (customerSourceToken[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(customerSourceToken[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, customerSourceToken[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(customerSourceToken[i].CustomerPlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `customer_platform_id` = @customerPlatformId" + i);
                                        else
                                            sql = string.Concat(sql, " `customer_platform_id` = @customerPlatformId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@customerPlatformId" + i, customerSourceToken[i].CustomerPlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(customerSourceToken[i].SourceToken))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `source_token` = @sourceToken" + i);
                                        else
                                            sql = string.Concat(sql, " `source_token` = @sourceToken" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@sourceToken" + i, customerSourceToken[i].SourceToken);
                                    }
                                    if (!string.IsNullOrEmpty(customerSourceToken[i].Active))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `active` = @active" + i);
                                        else
                                            sql = string.Concat(sql, " `active` = @active" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@active" + i, customerSourceToken[i].Active);
                                    }
                                    if (!string.IsNullOrEmpty(customerSourceToken[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, customerSourceToken[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(customerSourceToken[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, customerSourceToken[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(customerSourceToken[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, customerSourceToken[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(customerSourceToken[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CustomerSourceTokenModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CustomerSourceTokenModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CustomerSourceTokenModel>();
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
