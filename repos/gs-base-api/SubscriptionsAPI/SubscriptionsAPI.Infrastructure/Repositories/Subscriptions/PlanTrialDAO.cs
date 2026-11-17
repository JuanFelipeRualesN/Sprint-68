using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class PlanTrialDAO
    {
        private MySqlConnection _connection;
        public List<PlanTrialModel> Read(string connectionString, PlanTrialModel planTrial, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanTrialModel> response = new List<PlanTrialModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `plan_id`, `trial_days`, `modified_by`, `created_at`, `updated_at`  FROM subst_plan_trials ";
                {
                    if (planTrial != null)
                    {
                        if (!string.IsNullOrEmpty(planTrial.Id))
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
                            cmd.Parameters.AddWithValue("@id", planTrial.Id);
                        }
                        if (!string.IsNullOrEmpty(planTrial.PlanId))
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
                            cmd.Parameters.AddWithValue("@planId", planTrial.PlanId);
                        }
                        if (!string.IsNullOrEmpty(planTrial.TrialDays))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `trial_days` = @trialDays ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `trial_days` = @trialDays ");
                            }
                            cmd.Parameters.AddWithValue("@trialDays", planTrial.TrialDays);
                        }
                        if (!string.IsNullOrEmpty(planTrial.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planTrial.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(planTrial.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planTrial.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(planTrial.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planTrial.UpdatedAt);
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
                                    planTrial.Id = reader.GetValue(0).ToString();
                                else
                                    planTrial.Id = null;
                                if (!reader.IsDBNull(1))
                                    planTrial.PlanId = reader.GetValue(1).ToString();
                                else
                                    planTrial.PlanId = null;
                                if (!reader.IsDBNull(2))
                                    planTrial.TrialDays = reader.GetValue(2).ToString();
                                else
                                    planTrial.TrialDays = null;
                                if (!reader.IsDBNull(3))
                                    planTrial.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    planTrial.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    planTrial.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    planTrial.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    planTrial.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    planTrial.UpdatedAt = null;
                                response.Add(planTrial);
                                planTrial = new PlanTrialModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlanTrialModel>();
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
                response = new List<PlanTrialModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanTrialModel> Read(string connectionString, List<PlanTrialModel> planTrialWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanTrialModel> response = new List<PlanTrialModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_id`, `trial_days`, `modified_by`, `created_at`, `updated_at`  FROM subst_plan_trials ";
                if (planTrialWhere != null && planTrialWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planTrialWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(planTrialWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planTrialWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(planTrialWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, planTrialWhere[i].PlanId);
                        }
                        if (!string.IsNullOrEmpty(planTrialWhere[i].TrialDays))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `trial_days` = @trialDays" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@trialDays" + i, planTrialWhere[i].TrialDays);
                        }
                        if (!string.IsNullOrEmpty(planTrialWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planTrialWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(planTrialWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planTrialWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(planTrialWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planTrialWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planTrialWhere.Count - 1)
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
                            PlanTrialModel planTrial = new PlanTrialModel(); if (!reader.IsDBNull(0))
                                planTrial.Id = reader.GetValue(0).ToString();
                            else
                                planTrial.Id = null;
                            if (!reader.IsDBNull(1))
                                planTrial.PlanId = reader.GetValue(1).ToString();
                            else
                                planTrial.PlanId = null;
                            if (!reader.IsDBNull(2))
                                planTrial.TrialDays = reader.GetValue(2).ToString();
                            else
                                planTrial.TrialDays = null;
                            if (!reader.IsDBNull(3))
                                planTrial.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                planTrial.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                planTrial.CreatedAt = reader.GetValue(4).ToString();
                            else
                                planTrial.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                planTrial.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                planTrial.UpdatedAt = null;
                            response.Add(planTrial);
                            planTrial = new PlanTrialModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlanTrialModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanTrialModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanTrialModel> Create(string connectionString, PlanTrialModel planTrial)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanTrialModel> response = new List<PlanTrialModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO subst_plan_trials (";
                string values = "(";
                if (!string.IsNullOrEmpty(planTrial.Id))
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
                    cmd.Parameters.AddWithValue("@id", planTrial.Id);
                }
                if (!string.IsNullOrEmpty(planTrial.PlanId))
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
                    cmd.Parameters.AddWithValue("@planId", planTrial.PlanId);
                }
                if (!string.IsNullOrEmpty(planTrial.TrialDays))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `trial_days` ");
                        values = string.Concat(values, ", @trialDays");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `trial_days` ");
                        values = string.Concat(values, " @trialDays");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@trialDays", planTrial.TrialDays);
                }
                if (!string.IsNullOrEmpty(planTrial.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", planTrial.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(planTrial.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", planTrial.CreatedAt);
                }
                if (!string.IsNullOrEmpty(planTrial.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", planTrial.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                planTrial.Id = cmd.LastInsertedId.ToString();
                response.Add(planTrial);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanTrialModel>();
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
        public List<PlanTrialModel> Create(string connectionString, List<PlanTrialModel> planTrialList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanTrialModel> response = new List<PlanTrialModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < planTrialList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_plan_trials (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(planTrialList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, planTrialList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(planTrialList[i].PlanId))
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
                                    cmd.Parameters.AddWithValue("@planId" + i, planTrialList[i].PlanId);
                                }
                                if (!string.IsNullOrEmpty(planTrialList[i].TrialDays))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `trial_days` ");
                                        values = string.Concat(values, ", @trialDays" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `trial_days` ");
                                        values = string.Concat(values, "@trialDays" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@trialDays" + i, planTrialList[i].TrialDays);
                                }
                                if (!string.IsNullOrEmpty(planTrialList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planTrialList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(planTrialList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planTrialList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(planTrialList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planTrialList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(planTrialList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanTrialModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanTrialModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanTrialModel>();
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

        public List<PlanTrialModel> Update(string connectionString, PlanTrialModel planTrial, PlanTrialModel planTrialWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanTrialModel> response = new List<PlanTrialModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE subst_plan_trials SET ";
                if (!string.IsNullOrEmpty(planTrial.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", planTrial.Id);
                }
                if (!string.IsNullOrEmpty(planTrial.PlanId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `plan_id` = @planId");
                    else
                        sql = string.Concat(sql, " `plan_id` = @planId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planId", planTrial.PlanId);
                }
                if (!string.IsNullOrEmpty(planTrial.TrialDays))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `trial_days` = @trialDays");
                    else
                        sql = string.Concat(sql, " `trial_days` = @trialDays");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@trialDays", planTrial.TrialDays);
                }
                if (!string.IsNullOrEmpty(planTrial.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", planTrial.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(planTrial.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", planTrial.CreatedAt);
                }
                if (!string.IsNullOrEmpty(planTrial.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", planTrial.UpdatedAt);
                }
                if (planTrialWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(planTrialWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", planTrialWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(planTrialWhere.PlanId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `plan_id` = @planIdWhere");
                        else
                            sql = string.Concat(sql, " `plan_id` = @planIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planIdWhere", planTrialWhere.PlanId);
                    }
                    if (!string.IsNullOrEmpty(planTrialWhere.TrialDays))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `trial_days` = @trialDaysWhere");
                        else
                            sql = string.Concat(sql, " `trial_days` = @trialDaysWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@trialDaysWhere", planTrialWhere.TrialDays);
                    }
                    if (!string.IsNullOrEmpty(planTrialWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", planTrialWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(planTrialWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", planTrialWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(planTrialWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", planTrialWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(planTrial);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanTrialModel>();
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

        public List<PlanTrialModel> Update(string connectionString, List<PlanTrialModel> planTrialList, List<PlanTrialModel> planTrialWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanTrialModel> response = new List<PlanTrialModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < planTrialList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_plan_trials SET ";
                                if (!string.IsNullOrEmpty(planTrialList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, planTrialList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(planTrialList[i].PlanId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `plan_id` = @planId" + i);
                                    else
                                        sql = string.Concat(sql, " `plan_id` = @planId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planId" + i, planTrialList[i].PlanId);
                                }
                                if (!string.IsNullOrEmpty(planTrialList[i].TrialDays))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `trial_days` = @trialDays" + i);
                                    else
                                        sql = string.Concat(sql, " `trial_days` = @trialDays" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@trialDays" + i, planTrialList[i].TrialDays);
                                }
                                if (!string.IsNullOrEmpty(planTrialList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planTrialList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(planTrialList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planTrialList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(planTrialList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planTrialList[i].UpdatedAt);
                                }
                                if (planTrialWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(planTrialWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, planTrialWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(planTrialWhereList[i].PlanId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `plan_id` = @planIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `plan_id` = @planIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planIdWhere" + i, planTrialWhereList[i].PlanId);
                                    }
                                    if (!string.IsNullOrEmpty(planTrialWhereList[i].TrialDays))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `trial_days` = @trialDaysWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `trial_days` = @trialDaysWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@trialDaysWhere" + i, planTrialWhereList[i].TrialDays);
                                    }
                                    if (!string.IsNullOrEmpty(planTrialWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, planTrialWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(planTrialWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, planTrialWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(planTrialWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, planTrialWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(planTrialList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanTrialModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanTrialModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanTrialModel>();
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

        public List<PlanTrialModel> Delete(string connectionString, PlanTrialModel planTrial)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanTrialModel> response = new List<PlanTrialModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM subst_plan_trials  ";
                if (planTrial != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(planTrial.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", planTrial.Id);
                    }
                    if (!string.IsNullOrEmpty(planTrial.PlanId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `plan_id` = @planId");
                        else
                            sql = string.Concat(sql, " `plan_id` = @planId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planId", planTrial.PlanId);
                    }
                    if (!string.IsNullOrEmpty(planTrial.TrialDays))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `trial_days` = @trialDays");
                        else
                            sql = string.Concat(sql, " `trial_days` = @trialDays");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@trialDays", planTrial.TrialDays);
                    }
                    if (!string.IsNullOrEmpty(planTrial.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", planTrial.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(planTrial.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", planTrial.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(planTrial.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", planTrial.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(planTrial);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanTrialModel>();
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

        public List<PlanTrialModel> Delete(string connectionString, List<PlanTrialModel> planTrial)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanTrialModel> response = new List<PlanTrialModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < planTrial.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_plan_trials  ";
                                if (planTrial[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(planTrial[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, planTrial[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(planTrial[i].PlanId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `plan_id` = @planId" + i);
                                        else
                                            sql = string.Concat(sql, " `plan_id` = @planId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planId" + i, planTrial[i].PlanId);
                                    }
                                    if (!string.IsNullOrEmpty(planTrial[i].TrialDays))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `trial_days` = @trialDays" + i);
                                        else
                                            sql = string.Concat(sql, " `trial_days` = @trialDays" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@trialDays" + i, planTrial[i].TrialDays);
                                    }
                                    if (!string.IsNullOrEmpty(planTrial[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, planTrial[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(planTrial[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, planTrial[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(planTrial[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, planTrial[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(planTrial[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanTrialModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanTrialModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanTrialModel>();
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
