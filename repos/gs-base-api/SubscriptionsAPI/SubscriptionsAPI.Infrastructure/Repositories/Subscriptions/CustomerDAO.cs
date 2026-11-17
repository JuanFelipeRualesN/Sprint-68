using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class CustomerDAO
    {
        private MySqlConnection _connection;
        public List<CustomerModel> Read(string connectionString, CustomerModel customer, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerModel> response = new List<CustomerModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `platform_id`, `modified_by`, `created_at`, `updated_at`  FROM subst_customers ";
                {
                    if (customer != null)
                    {
                        if (!string.IsNullOrEmpty(customer.Id))
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
                            cmd.Parameters.AddWithValue("@id", customer.Id);
                        }
                        if (!string.IsNullOrEmpty(customer.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", customer.UserId);
                        }
                        if (!string.IsNullOrEmpty(customer.PlatformId))
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
                            cmd.Parameters.AddWithValue("@platformId", customer.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(customer.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", customer.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(customer.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", customer.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(customer.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", customer.UpdatedAt);
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
                                    customer.Id = reader.GetValue(0).ToString();
                                else
                                    customer.Id = null;
                                if (!reader.IsDBNull(1))
                                    customer.UserId = reader.GetValue(1).ToString();
                                else
                                    customer.UserId = null;
                                if (!reader.IsDBNull(2))
                                    customer.PlatformId = reader.GetValue(2).ToString();
                                else
                                    customer.PlatformId = null;
                                if (!reader.IsDBNull(3))
                                    customer.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    customer.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    customer.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    customer.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    customer.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    customer.UpdatedAt = null;
                                response.Add(customer);
                                customer = new CustomerModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<CustomerModel>();
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
                response = new List<CustomerModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CustomerModel> Read(string connectionString, List<CustomerModel> customerWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerModel> response = new List<CustomerModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `platform_id`, `modified_by`, `created_at`, `updated_at`  FROM subst_customers ";
                if (customerWhere != null && customerWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < customerWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(customerWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, customerWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(customerWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, customerWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(customerWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, customerWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(customerWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, customerWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(customerWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, customerWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(customerWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, customerWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != customerWhere.Count - 1)
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
                            CustomerModel customer = new CustomerModel(); if (!reader.IsDBNull(0))
                                customer.Id = reader.GetValue(0).ToString();
                            else
                                customer.Id = null;
                            if (!reader.IsDBNull(1))
                                customer.UserId = reader.GetValue(1).ToString();
                            else
                                customer.UserId = null;
                            if (!reader.IsDBNull(2))
                                customer.PlatformId = reader.GetValue(2).ToString();
                            else
                                customer.PlatformId = null;
                            if (!reader.IsDBNull(3))
                                customer.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                customer.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                customer.CreatedAt = reader.GetValue(4).ToString();
                            else
                                customer.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                customer.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                customer.UpdatedAt = null;
                            response.Add(customer);
                            customer = new CustomerModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<CustomerModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CustomerModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CustomerModel> Create(string connectionString, CustomerModel customer)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerModel> response = new List<CustomerModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO subst_customers (";
                string values = "(";
                if (!string.IsNullOrEmpty(customer.Id))
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
                    cmd.Parameters.AddWithValue("@id", customer.Id);
                }
                if (!string.IsNullOrEmpty(customer.UserId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `user_id` ");
                        values = string.Concat(values, ", @userId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `user_id` ");
                        values = string.Concat(values, " @userId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", customer.UserId);
                }
                if (!string.IsNullOrEmpty(customer.PlatformId))
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
                    cmd.Parameters.AddWithValue("@platformId", customer.PlatformId);
                }
                if (!string.IsNullOrEmpty(customer.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", customer.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(customer.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", customer.CreatedAt);
                }
                if (!string.IsNullOrEmpty(customer.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", customer.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                customer.Id = cmd.LastInsertedId.ToString();
                response.Add(customer);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CustomerModel>();
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
        public List<CustomerModel> Create(string connectionString, List<CustomerModel> customerList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerModel> response = new List<CustomerModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < customerList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_customers (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(customerList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, customerList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(customerList[i].UserId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `user_id` ");
                                        values = string.Concat(values, ", @userId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `user_id` ");
                                        values = string.Concat(values, "@userId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, customerList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(customerList[i].PlatformId))
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
                                    cmd.Parameters.AddWithValue("@platformId" + i, customerList[i].PlatformId);
                                }
                                if (!string.IsNullOrEmpty(customerList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, customerList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(customerList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, customerList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(customerList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, customerList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(customerList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<CustomerModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CustomerModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CustomerModel>();
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

        public List<CustomerModel> Update(string connectionString, CustomerModel customer, CustomerModel customerWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerModel> response = new List<CustomerModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE subst_customers SET ";
                if (!string.IsNullOrEmpty(customer.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", customer.Id);
                }
                if (!string.IsNullOrEmpty(customer.UserId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = string.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", customer.UserId);
                }
                if (!string.IsNullOrEmpty(customer.PlatformId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `platform_id` = @platformId");
                    else
                        sql = string.Concat(sql, " `platform_id` = @platformId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@platformId", customer.PlatformId);
                }
                if (!string.IsNullOrEmpty(customer.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", customer.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(customer.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", customer.CreatedAt);
                }
                if (!string.IsNullOrEmpty(customer.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", customer.UpdatedAt);
                }
                if (customerWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(customerWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", customerWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(customerWhere.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = string.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", customerWhere.UserId);
                    }
                    if (!string.IsNullOrEmpty(customerWhere.PlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform_id` = @platformIdWhere");
                        else
                            sql = string.Concat(sql, " `platform_id` = @platformIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformIdWhere", customerWhere.PlatformId);
                    }
                    if (!string.IsNullOrEmpty(customerWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", customerWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(customerWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", customerWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(customerWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", customerWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(customer);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CustomerModel>();
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

        public List<CustomerModel> Update(string connectionString, List<CustomerModel> customerList, List<CustomerModel> customerWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerModel> response = new List<CustomerModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < customerList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_customers SET ";
                                if (!string.IsNullOrEmpty(customerList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, customerList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(customerList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = string.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, customerList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(customerList[i].PlatformId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `platform_id` = @platformId" + i);
                                    else
                                        sql = string.Concat(sql, " `platform_id` = @platformId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@platformId" + i, customerList[i].PlatformId);
                                }
                                if (!string.IsNullOrEmpty(customerList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, customerList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(customerList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, customerList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(customerList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, customerList[i].UpdatedAt);
                                }
                                if (customerWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(customerWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, customerWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(customerWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, customerWhereList[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(customerWhereList[i].PlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform_id` = @platformIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `platform_id` = @platformIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformIdWhere" + i, customerWhereList[i].PlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(customerWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, customerWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(customerWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, customerWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(customerWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, customerWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(customerList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CustomerModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CustomerModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CustomerModel>();
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

        public List<CustomerModel> Delete(string connectionString, CustomerModel customer)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerModel> response = new List<CustomerModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM subst_customers  ";
                if (customer != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(customer.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", customer.Id);
                    }
                    if (!string.IsNullOrEmpty(customer.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = string.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", customer.UserId);
                    }
                    if (!string.IsNullOrEmpty(customer.PlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform_id` = @platformId");
                        else
                            sql = string.Concat(sql, " `platform_id` = @platformId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformId", customer.PlatformId);
                    }
                    if (!string.IsNullOrEmpty(customer.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", customer.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(customer.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", customer.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(customer.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", customer.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(customer);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CustomerModel>();
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

        public List<CustomerModel> Delete(string connectionString, List<CustomerModel> customer)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CustomerModel> response = new List<CustomerModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < customer.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_customers  ";
                                if (customer[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(customer[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, customer[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(customer[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, customer[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(customer[i].PlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform_id` = @platformId" + i);
                                        else
                                            sql = string.Concat(sql, " `platform_id` = @platformId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformId" + i, customer[i].PlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(customer[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, customer[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(customer[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, customer[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(customer[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, customer[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(customer[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CustomerModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CustomerModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CustomerModel>();
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
