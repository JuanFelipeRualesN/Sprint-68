using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class TaxPlanDAO
    {
        private MySqlConnection _connection;
        public List<TaxPlanModel> Read(string connectionString, TaxPlanModel taxPlan, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxPlanModel> response = new List<TaxPlanModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `tax_id`, `plan_id`, `modified_by`, `created_at`, `updated_at`  FROM subst_tax_plans ";
                {
                    if (taxPlan != null)
                    {
                        if (!string.IsNullOrEmpty(taxPlan.Id))
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
                            cmd.Parameters.AddWithValue("@id", taxPlan.Id);
                        }
                        if (!string.IsNullOrEmpty(taxPlan.TaxId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `tax_id` = @taxId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `tax_id` = @taxId ");
                            }
                            cmd.Parameters.AddWithValue("@taxId", taxPlan.TaxId);
                        }
                        if (!string.IsNullOrEmpty(taxPlan.PlanId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `plan_id` = @planId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `plan_id` = @planId ");
                            }
                            cmd.Parameters.AddWithValue("@planId", taxPlan.PlanId);
                        }
                        if (!string.IsNullOrEmpty(taxPlan.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", taxPlan.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(taxPlan.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", taxPlan.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(taxPlan.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", taxPlan.UpdatedAt);
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
                                    taxPlan.Id = reader.GetValue(0).ToString();
                                else
                                    taxPlan.Id = null;
                                if (!reader.IsDBNull(1))
                                    taxPlan.TaxId = reader.GetValue(1).ToString();
                                else
                                    taxPlan.TaxId = null;
                                if (!reader.IsDBNull(2))
                                    taxPlan.PlanId = reader.GetValue(2).ToString();
                                else
                                    taxPlan.PlanId = null;
                                if (!reader.IsDBNull(3))
                                    taxPlan.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    taxPlan.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    taxPlan.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    taxPlan.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    taxPlan.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    taxPlan.UpdatedAt = null;
                                response.Add(taxPlan);
                                taxPlan = new TaxPlanModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TaxPlanModel>();
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
                response = new List<TaxPlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TaxPlanModel> Read(string connectionString, List<TaxPlanModel> taxPlanWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxPlanModel> response = new List<TaxPlanModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `tax_id`, `plan_id`, `modified_by`, `created_at`, `updated_at`  FROM subst_tax_plans ";
                if (taxPlanWhere != null && taxPlanWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < taxPlanWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(taxPlanWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, taxPlanWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(taxPlanWhere[i].TaxId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tax_id` = @taxId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@taxId" + i, taxPlanWhere[i].TaxId);
                        }
                        if (!string.IsNullOrEmpty(taxPlanWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, taxPlanWhere[i].PlanId);
                        }
                        if (!string.IsNullOrEmpty(taxPlanWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, taxPlanWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(taxPlanWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, taxPlanWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(taxPlanWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, taxPlanWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != taxPlanWhere.Count - 1)
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
                            TaxPlanModel taxPlan = new TaxPlanModel(); if (!reader.IsDBNull(0))
                                taxPlan.Id = reader.GetValue(0).ToString();
                            else
                                taxPlan.Id = null;
                            if (!reader.IsDBNull(1))
                                taxPlan.TaxId = reader.GetValue(1).ToString();
                            else
                                taxPlan.TaxId = null;
                            if (!reader.IsDBNull(2))
                                taxPlan.PlanId = reader.GetValue(2).ToString();
                            else
                                taxPlan.PlanId = null;
                            if (!reader.IsDBNull(3))
                                taxPlan.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                taxPlan.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                taxPlan.CreatedAt = reader.GetValue(4).ToString();
                            else
                                taxPlan.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                taxPlan.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                taxPlan.UpdatedAt = null;
                            response.Add(taxPlan);
                            taxPlan = new TaxPlanModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TaxPlanModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TaxPlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TaxPlanModel> Create(string connectionString, TaxPlanModel taxPlan)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxPlanModel> response = new List<TaxPlanModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO subst_tax_plans (";
                string values = "(";
                if (!string.IsNullOrEmpty(taxPlan.Id))
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
                    cmd.Parameters.AddWithValue("@id", taxPlan.Id);
                }
                if (!string.IsNullOrEmpty(taxPlan.TaxId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `tax_id` ");
                        values = string.Concat(values, ", @taxId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `tax_id` ");
                        values = string.Concat(values, " @taxId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@taxId", taxPlan.TaxId);
                }
                if (!string.IsNullOrEmpty(taxPlan.PlanId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `plan_id` ");
                        values = string.Concat(values, ", @planId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `plan_id` ");
                        values = string.Concat(values, " @planId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planId", taxPlan.PlanId);
                }
                if (!string.IsNullOrEmpty(taxPlan.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", taxPlan.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(taxPlan.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", taxPlan.CreatedAt);
                }
                if (!string.IsNullOrEmpty(taxPlan.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", taxPlan.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                taxPlan.Id = cmd.LastInsertedId.ToString();
                response.Add(taxPlan);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TaxPlanModel>();
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
        public List<TaxPlanModel> Create(string connectionString, List<TaxPlanModel> taxPlanList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxPlanModel> response = new List<TaxPlanModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < taxPlanList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_tax_plans (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(taxPlanList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, taxPlanList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(taxPlanList[i].TaxId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `tax_id` ");
                                        values = string.Concat(values, ", @taxId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `tax_id` ");
                                        values = string.Concat(values, "@taxId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@taxId" + i, taxPlanList[i].TaxId);
                                }
                                if (!string.IsNullOrEmpty(taxPlanList[i].PlanId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `plan_id` ");
                                        values = string.Concat(values, ", @planId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `plan_id` ");
                                        values = string.Concat(values, "@planId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planId" + i, taxPlanList[i].PlanId);
                                }
                                if (!string.IsNullOrEmpty(taxPlanList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, taxPlanList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(taxPlanList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, taxPlanList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(taxPlanList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, taxPlanList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(taxPlanList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<TaxPlanModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TaxPlanModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TaxPlanModel>();
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

        public List<TaxPlanModel> Update(string connectionString, TaxPlanModel taxPlan, TaxPlanModel taxPlanWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxPlanModel> response = new List<TaxPlanModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE subst_tax_plans SET ";
                if (!string.IsNullOrEmpty(taxPlan.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", taxPlan.Id);
                }
                if (!string.IsNullOrEmpty(taxPlan.TaxId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `tax_id` = @taxId");
                    else
                        sql = string.Concat(sql, " `tax_id` = @taxId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@taxId", taxPlan.TaxId);
                }
                if (!string.IsNullOrEmpty(taxPlan.PlanId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `plan_id` = @planId");
                    else
                        sql = string.Concat(sql, " `plan_id` = @planId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planId", taxPlan.PlanId);
                }
                if (!string.IsNullOrEmpty(taxPlan.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", taxPlan.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(taxPlan.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", taxPlan.CreatedAt);
                }
                if (!string.IsNullOrEmpty(taxPlan.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", taxPlan.UpdatedAt);
                }
                if (taxPlanWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(taxPlanWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", taxPlanWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(taxPlanWhere.TaxId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `tax_id` = @taxIdWhere");
                        else
                            sql = string.Concat(sql, " `tax_id` = @taxIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@taxIdWhere", taxPlanWhere.TaxId);
                    }
                    if (!string.IsNullOrEmpty(taxPlanWhere.PlanId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `plan_id` = @planIdWhere");
                        else
                            sql = string.Concat(sql, " `plan_id` = @planIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planIdWhere", taxPlanWhere.PlanId);
                    }
                    if (!string.IsNullOrEmpty(taxPlanWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", taxPlanWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(taxPlanWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", taxPlanWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(taxPlanWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", taxPlanWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(taxPlan);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TaxPlanModel>();
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

        public List<TaxPlanModel> Update(string connectionString, List<TaxPlanModel> taxPlanList, List<TaxPlanModel> taxPlanWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxPlanModel> response = new List<TaxPlanModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < taxPlanList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_tax_plans SET ";
                                if (!string.IsNullOrEmpty(taxPlanList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, taxPlanList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(taxPlanList[i].TaxId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `tax_id` = @taxId" + i);
                                    else
                                        sql = string.Concat(sql, " `tax_id` = @taxId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@taxId" + i, taxPlanList[i].TaxId);
                                }
                                if (!string.IsNullOrEmpty(taxPlanList[i].PlanId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `plan_id` = @planId" + i);
                                    else
                                        sql = string.Concat(sql, " `plan_id` = @planId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planId" + i, taxPlanList[i].PlanId);
                                }
                                if (!string.IsNullOrEmpty(taxPlanList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, taxPlanList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(taxPlanList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, taxPlanList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(taxPlanList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, taxPlanList[i].UpdatedAt);
                                }
                                if (taxPlanWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(taxPlanWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, taxPlanWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(taxPlanWhereList[i].TaxId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `tax_id` = @taxIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `tax_id` = @taxIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@taxIdWhere" + i, taxPlanWhereList[i].TaxId);
                                    }
                                    if (!string.IsNullOrEmpty(taxPlanWhereList[i].PlanId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `plan_id` = @planIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `plan_id` = @planIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planIdWhere" + i, taxPlanWhereList[i].PlanId);
                                    }
                                    if (!string.IsNullOrEmpty(taxPlanWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, taxPlanWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(taxPlanWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, taxPlanWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(taxPlanWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, taxPlanWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(taxPlanList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TaxPlanModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TaxPlanModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TaxPlanModel>();
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

        public List<TaxPlanModel> Delete(string connectionString, TaxPlanModel taxPlan)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxPlanModel> response = new List<TaxPlanModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM subst_tax_plans  ";
                if (taxPlan != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(taxPlan.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", taxPlan.Id);
                    }
                    if (!string.IsNullOrEmpty(taxPlan.TaxId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `tax_id` = @taxId");
                        else
                            sql = string.Concat(sql, " `tax_id` = @taxId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@taxId", taxPlan.TaxId);
                    }
                    if (!string.IsNullOrEmpty(taxPlan.PlanId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `plan_id` = @planId");
                        else
                            sql = string.Concat(sql, " `plan_id` = @planId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planId", taxPlan.PlanId);
                    }
                    if (!string.IsNullOrEmpty(taxPlan.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", taxPlan.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(taxPlan.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", taxPlan.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(taxPlan.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", taxPlan.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(taxPlan);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TaxPlanModel>();
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

        public List<TaxPlanModel> Delete(string connectionString, List<TaxPlanModel> taxPlan)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxPlanModel> response = new List<TaxPlanModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < taxPlan.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_tax_plans  ";
                                if (taxPlan[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(taxPlan[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, taxPlan[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(taxPlan[i].TaxId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `tax_id` = @taxId" + i);
                                        else
                                            sql = string.Concat(sql, " `tax_id` = @taxId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@taxId" + i, taxPlan[i].TaxId);
                                    }
                                    if (!string.IsNullOrEmpty(taxPlan[i].PlanId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `plan_id` = @planId" + i);
                                        else
                                            sql = string.Concat(sql, " `plan_id` = @planId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planId" + i, taxPlan[i].PlanId);
                                    }
                                    if (!string.IsNullOrEmpty(taxPlan[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, taxPlan[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(taxPlan[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, taxPlan[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(taxPlan[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, taxPlan[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(taxPlan[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TaxPlanModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TaxPlanModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TaxPlanModel>();
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
