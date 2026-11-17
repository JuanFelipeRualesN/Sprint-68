using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class BillingIntervalDAO
    {
        private MySqlConnection _connection;
        public List<BillingIntervalModel> Read(string connectionString, BillingIntervalModel billingInterval, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalModel> response = new List<BillingIntervalModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `value`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_billing_intervals ";
                {
                    if (billingInterval != null)
                    {
                        if (!string.IsNullOrEmpty(billingInterval.Id))
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
                            cmd.Parameters.AddWithValue("@id", billingInterval.Id);
                        }
                        if (!string.IsNullOrEmpty(billingInterval.Value))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `value` = @value ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `value` = @value ");
                            }
                            cmd.Parameters.AddWithValue("@value", billingInterval.Value);
                        }
                        if (!string.IsNullOrEmpty(billingInterval.Available))
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
                            cmd.Parameters.AddWithValue("@available", billingInterval.Available);
                        }
                        if (!string.IsNullOrEmpty(billingInterval.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", billingInterval.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(billingInterval.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", billingInterval.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(billingInterval.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", billingInterval.UpdatedAt);
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
                                    billingInterval.Id = reader.GetValue(0).ToString();
                                else
                                    billingInterval.Id = null;
                                if (!reader.IsDBNull(1))
                                    billingInterval.Value = reader.GetValue(1).ToString();
                                else
                                    billingInterval.Value = null;
                                if (!reader.IsDBNull(2))
                                    billingInterval.Available = reader.GetValue(2).ToString();
                                else
                                    billingInterval.Available = null;
                                if (!reader.IsDBNull(3))
                                    billingInterval.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    billingInterval.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    billingInterval.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    billingInterval.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    billingInterval.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    billingInterval.UpdatedAt = null;
                                response.Add(billingInterval);
                                billingInterval = new BillingIntervalModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<BillingIntervalModel>();
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
                response = new List<BillingIntervalModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<BillingIntervalModel> Read(string connectionString, List<BillingIntervalModel> billingIntervalWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalModel> response = new List<BillingIntervalModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `value`, `available`, `modified_by`, `created_at`, `updated_at`  FROM subst_billing_intervals ";
                if (billingIntervalWhere != null && billingIntervalWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < billingIntervalWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(billingIntervalWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, billingIntervalWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalWhere[i].Value))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `value` = @value" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@value" + i, billingIntervalWhere[i].Value);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, billingIntervalWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, billingIntervalWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, billingIntervalWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(billingIntervalWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, billingIntervalWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != billingIntervalWhere.Count - 1)
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
                            BillingIntervalModel billingInterval = new BillingIntervalModel(); if (!reader.IsDBNull(0))
                                billingInterval.Id = reader.GetValue(0).ToString();
                            else
                                billingInterval.Id = null;
                            if (!reader.IsDBNull(1))
                                billingInterval.Value = reader.GetValue(1).ToString();
                            else
                                billingInterval.Value = null;
                            if (!reader.IsDBNull(2))
                                billingInterval.Available = reader.GetValue(2).ToString();
                            else
                                billingInterval.Available = null;
                            if (!reader.IsDBNull(3))
                                billingInterval.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                billingInterval.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                billingInterval.CreatedAt = reader.GetValue(4).ToString();
                            else
                                billingInterval.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                billingInterval.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                billingInterval.UpdatedAt = null;
                            response.Add(billingInterval);
                            billingInterval = new BillingIntervalModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<BillingIntervalModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<BillingIntervalModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<BillingIntervalModel> Create(string connectionString, BillingIntervalModel billingInterval)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalModel> response = new List<BillingIntervalModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO subst_billing_intervals (";
                string values = "(";
                if (!string.IsNullOrEmpty(billingInterval.Id))
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
                    cmd.Parameters.AddWithValue("@id", billingInterval.Id);
                }
                if (!string.IsNullOrEmpty(billingInterval.Value))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `value` ");
                        values = string.Concat(values, ", @value");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `value` ");
                        values = string.Concat(values, " @value");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@value", billingInterval.Value);
                }
                if (!string.IsNullOrEmpty(billingInterval.Available))
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
                    cmd.Parameters.AddWithValue("@available", billingInterval.Available);
                }
                if (!string.IsNullOrEmpty(billingInterval.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", billingInterval.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(billingInterval.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", billingInterval.CreatedAt);
                }
                if (!string.IsNullOrEmpty(billingInterval.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", billingInterval.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                billingInterval.Id = cmd.LastInsertedId.ToString();
                response.Add(billingInterval);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<BillingIntervalModel>();
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
        public List<BillingIntervalModel> Create(string connectionString, List<BillingIntervalModel> billingIntervalList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalModel> response = new List<BillingIntervalModel>();
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
                        for (int i = 0; i < billingIntervalList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_billing_intervals (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(billingIntervalList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, billingIntervalList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalList[i].Value))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `value` ");
                                        values = string.Concat(values, ", @value" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `value` ");
                                        values = string.Concat(values, "@value" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@value" + i, billingIntervalList[i].Value);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalList[i].Available))
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
                                    cmd.Parameters.AddWithValue("@available" + i, billingIntervalList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, billingIntervalList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, billingIntervalList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, billingIntervalList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(billingIntervalList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<BillingIntervalModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<BillingIntervalModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<BillingIntervalModel>();
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

        public List<BillingIntervalModel> Update(string connectionString, BillingIntervalModel billingInterval, BillingIntervalModel billingIntervalWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalModel> response = new List<BillingIntervalModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE subst_billing_intervals SET ";
                if (!string.IsNullOrEmpty(billingInterval.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", billingInterval.Id);
                }
                if (!string.IsNullOrEmpty(billingInterval.Value))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `value` = @value");
                    else
                        sql = string.Concat(sql, " `value` = @value");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@value", billingInterval.Value);
                }
                if (!string.IsNullOrEmpty(billingInterval.Available))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `available` = @available");
                    else
                        sql = string.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", billingInterval.Available);
                }
                if (!string.IsNullOrEmpty(billingInterval.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", billingInterval.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(billingInterval.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", billingInterval.CreatedAt);
                }
                if (!string.IsNullOrEmpty(billingInterval.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", billingInterval.UpdatedAt);
                }
                if (billingIntervalWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(billingIntervalWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", billingIntervalWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalWhere.Value))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `value` = @valueWhere");
                        else
                            sql = string.Concat(sql, " `value` = @valueWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@valueWhere", billingIntervalWhere.Value);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalWhere.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = string.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", billingIntervalWhere.Available);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", billingIntervalWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", billingIntervalWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(billingIntervalWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", billingIntervalWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(billingInterval);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<BillingIntervalModel>();
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

        public List<BillingIntervalModel> Update(string connectionString, List<BillingIntervalModel> billingIntervalList, List<BillingIntervalModel> billingIntervalWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalModel> response = new List<BillingIntervalModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < billingIntervalList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_billing_intervals SET ";
                                if (!string.IsNullOrEmpty(billingIntervalList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, billingIntervalList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalList[i].Value))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `value` = @value" + i);
                                    else
                                        sql = string.Concat(sql, " `value` = @value" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@value" + i, billingIntervalList[i].Value);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalList[i].Available))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = string.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, billingIntervalList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, billingIntervalList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, billingIntervalList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(billingIntervalList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, billingIntervalList[i].UpdatedAt);
                                }
                                if (billingIntervalWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(billingIntervalWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, billingIntervalWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalWhereList[i].Value))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `value` = @valueWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `value` = @valueWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@valueWhere" + i, billingIntervalWhereList[i].Value);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, billingIntervalWhereList[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, billingIntervalWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, billingIntervalWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(billingIntervalWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, billingIntervalWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(billingIntervalList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<BillingIntervalModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<BillingIntervalModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<BillingIntervalModel>();
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

        public List<BillingIntervalModel> Delete(string connectionString, BillingIntervalModel billingInterval)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalModel> response = new List<BillingIntervalModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM subst_billing_intervals  ";
                if (billingInterval != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(billingInterval.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", billingInterval.Id);
                    }
                    if (!string.IsNullOrEmpty(billingInterval.Value))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `value` = @value");
                        else
                            sql = string.Concat(sql, " `value` = @value");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@value", billingInterval.Value);
                    }
                    if (!string.IsNullOrEmpty(billingInterval.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @available");
                        else
                            sql = string.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", billingInterval.Available);
                    }
                    if (!string.IsNullOrEmpty(billingInterval.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", billingInterval.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(billingInterval.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", billingInterval.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(billingInterval.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", billingInterval.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(billingInterval);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<BillingIntervalModel>();
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

        public List<BillingIntervalModel> Delete(string connectionString, List<BillingIntervalModel> billingInterval)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<BillingIntervalModel> response = new List<BillingIntervalModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < billingInterval.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_billing_intervals  ";
                                if (billingInterval[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(billingInterval[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, billingInterval[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(billingInterval[i].Value))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `value` = @value" + i);
                                        else
                                            sql = string.Concat(sql, " `value` = @value" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@value" + i, billingInterval[i].Value);
                                    }
                                    if (!string.IsNullOrEmpty(billingInterval[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, billingInterval[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(billingInterval[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, billingInterval[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(billingInterval[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, billingInterval[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(billingInterval[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, billingInterval[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(billingInterval[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<BillingIntervalModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<BillingIntervalModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<BillingIntervalModel>();
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
