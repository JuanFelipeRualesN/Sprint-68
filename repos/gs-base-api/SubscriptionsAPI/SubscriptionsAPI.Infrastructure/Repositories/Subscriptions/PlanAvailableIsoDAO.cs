using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class PlanAvailableIsoDAO
    {
        private MySqlConnection _connection;
        public List<PlanAvailableIsoModel> Read(string connectionString, PlanAvailableIsoModel planAvailableIso, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAvailableIsoModel> response = new List<PlanAvailableIsoModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `plan_id`, `state_iso`, `country_iso`, `modified_by`, `created_at`, `updated_at`  FROM subst_plan_available_isos ";
                {
                    if (planAvailableIso != null)
                    {
                        if (!string.IsNullOrEmpty(planAvailableIso.Id))
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
                            cmd.Parameters.AddWithValue("@id", planAvailableIso.Id);
                        }
                        if (!string.IsNullOrEmpty(planAvailableIso.PlanId))
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
                            cmd.Parameters.AddWithValue("@planId", planAvailableIso.PlanId);
                        }
                        if (!string.IsNullOrEmpty(planAvailableIso.StateIso))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `state_iso` = @stateIso ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `state_iso` = @stateIso ");
                            }
                            cmd.Parameters.AddWithValue("@stateIso", planAvailableIso.StateIso);
                        }
                        if (!string.IsNullOrEmpty(planAvailableIso.CountryIso))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `country_iso` = @countryIso ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `country_iso` = @countryIso ");
                            }
                            cmd.Parameters.AddWithValue("@countryIso", planAvailableIso.CountryIso);
                        }
                        if (!string.IsNullOrEmpty(planAvailableIso.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planAvailableIso.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(planAvailableIso.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planAvailableIso.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(planAvailableIso.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planAvailableIso.UpdatedAt);
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
                                    planAvailableIso.Id = reader.GetValue(0).ToString();
                                else
                                    planAvailableIso.Id = null;
                                if (!reader.IsDBNull(1))
                                    planAvailableIso.PlanId = reader.GetValue(1).ToString();
                                else
                                    planAvailableIso.PlanId = null;
                                if (!reader.IsDBNull(2))
                                    planAvailableIso.StateIso = reader.GetValue(2).ToString();
                                else
                                    planAvailableIso.StateIso = null;
                                if (!reader.IsDBNull(3))
                                    planAvailableIso.CountryIso = reader.GetValue(3).ToString();
                                else
                                    planAvailableIso.CountryIso = null;
                                if (!reader.IsDBNull(4))
                                    planAvailableIso.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    planAvailableIso.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    planAvailableIso.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    planAvailableIso.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    planAvailableIso.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    planAvailableIso.UpdatedAt = null;
                                response.Add(planAvailableIso);
                                planAvailableIso = new PlanAvailableIsoModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlanAvailableIsoModel>();
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
                response = new List<PlanAvailableIsoModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanAvailableIsoModel> Read(string connectionString, List<PlanAvailableIsoModel> planAvailableIsoWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAvailableIsoModel> response = new List<PlanAvailableIsoModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_id`, `state_iso`, `country_iso`, `modified_by`, `created_at`, `updated_at`  FROM subst_plan_available_isos ";
                if (planAvailableIsoWhere != null && planAvailableIsoWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planAvailableIsoWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(planAvailableIsoWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planAvailableIsoWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(planAvailableIsoWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, planAvailableIsoWhere[i].PlanId);
                        }
                        if (!string.IsNullOrEmpty(planAvailableIsoWhere[i].StateIso))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `state_iso` = @stateIso" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@stateIso" + i, planAvailableIsoWhere[i].StateIso);
                        }
                        if (!string.IsNullOrEmpty(planAvailableIsoWhere[i].CountryIso))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country_iso` = @countryIso" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@countryIso" + i, planAvailableIsoWhere[i].CountryIso);
                        }
                        if (!string.IsNullOrEmpty(planAvailableIsoWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planAvailableIsoWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(planAvailableIsoWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planAvailableIsoWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(planAvailableIsoWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planAvailableIsoWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planAvailableIsoWhere.Count - 1)
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
                            PlanAvailableIsoModel planAvailableIso = new PlanAvailableIsoModel(); if (!reader.IsDBNull(0))
                                planAvailableIso.Id = reader.GetValue(0).ToString();
                            else
                                planAvailableIso.Id = null;
                            if (!reader.IsDBNull(1))
                                planAvailableIso.PlanId = reader.GetValue(1).ToString();
                            else
                                planAvailableIso.PlanId = null;
                            if (!reader.IsDBNull(2))
                                planAvailableIso.StateIso = reader.GetValue(2).ToString();
                            else
                                planAvailableIso.StateIso = null;
                            if (!reader.IsDBNull(3))
                                planAvailableIso.CountryIso = reader.GetValue(3).ToString();
                            else
                                planAvailableIso.CountryIso = null;
                            if (!reader.IsDBNull(4))
                                planAvailableIso.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                planAvailableIso.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                planAvailableIso.CreatedAt = reader.GetValue(5).ToString();
                            else
                                planAvailableIso.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                planAvailableIso.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                planAvailableIso.UpdatedAt = null;
                            response.Add(planAvailableIso);
                            planAvailableIso = new PlanAvailableIsoModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlanAvailableIsoModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanAvailableIsoModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanAvailableIsoModel> Create(string connectionString, PlanAvailableIsoModel planAvailableIso)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAvailableIsoModel> response = new List<PlanAvailableIsoModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO subst_plan_available_isos (";
                string values = "(";
                if (!string.IsNullOrEmpty(planAvailableIso.Id))
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
                    cmd.Parameters.AddWithValue("@id", planAvailableIso.Id);
                }
                if (!string.IsNullOrEmpty(planAvailableIso.PlanId))
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
                    cmd.Parameters.AddWithValue("@planId", planAvailableIso.PlanId);
                }
                if (!string.IsNullOrEmpty(planAvailableIso.StateIso))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `state_iso` ");
                        values = string.Concat(values, ", @stateIso");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `state_iso` ");
                        values = string.Concat(values, " @stateIso");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@stateIso", planAvailableIso.StateIso);
                }
                if (!string.IsNullOrEmpty(planAvailableIso.CountryIso))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `country_iso` ");
                        values = string.Concat(values, ", @countryIso");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `country_iso` ");
                        values = string.Concat(values, " @countryIso");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@countryIso", planAvailableIso.CountryIso);
                }
                if (!string.IsNullOrEmpty(planAvailableIso.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", planAvailableIso.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(planAvailableIso.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", planAvailableIso.CreatedAt);
                }
                if (!string.IsNullOrEmpty(planAvailableIso.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", planAvailableIso.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                planAvailableIso.Id = cmd.LastInsertedId.ToString();
                response.Add(planAvailableIso);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanAvailableIsoModel>();
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
        public List<PlanAvailableIsoModel> Create(string connectionString, List<PlanAvailableIsoModel> planAvailableIsoList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAvailableIsoModel> response = new List<PlanAvailableIsoModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < planAvailableIsoList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_plan_available_isos (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(planAvailableIsoList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, planAvailableIsoList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(planAvailableIsoList[i].PlanId))
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
                                    cmd.Parameters.AddWithValue("@planId" + i, planAvailableIsoList[i].PlanId);
                                }
                                if (!string.IsNullOrEmpty(planAvailableIsoList[i].StateIso))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `state_iso` ");
                                        values = string.Concat(values, ", @stateIso" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `state_iso` ");
                                        values = string.Concat(values, "@stateIso" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@stateIso" + i, planAvailableIsoList[i].StateIso);
                                }
                                if (!string.IsNullOrEmpty(planAvailableIsoList[i].CountryIso))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `country_iso` ");
                                        values = string.Concat(values, ", @countryIso" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `country_iso` ");
                                        values = string.Concat(values, "@countryIso" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@countryIso" + i, planAvailableIsoList[i].CountryIso);
                                }
                                if (!string.IsNullOrEmpty(planAvailableIsoList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planAvailableIsoList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(planAvailableIsoList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planAvailableIsoList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(planAvailableIsoList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planAvailableIsoList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(planAvailableIsoList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanAvailableIsoModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanAvailableIsoModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanAvailableIsoModel>();
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

        public List<PlanAvailableIsoModel> Update(string connectionString, PlanAvailableIsoModel planAvailableIso, PlanAvailableIsoModel planAvailableIsoWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAvailableIsoModel> response = new List<PlanAvailableIsoModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE subst_plan_available_isos SET ";
                if (!string.IsNullOrEmpty(planAvailableIso.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", planAvailableIso.Id);
                }
                if (!string.IsNullOrEmpty(planAvailableIso.PlanId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `plan_id` = @planId");
                    else
                        sql = string.Concat(sql, " `plan_id` = @planId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planId", planAvailableIso.PlanId);
                }
                if (!string.IsNullOrEmpty(planAvailableIso.StateIso))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `state_iso` = @stateIso");
                    else
                        sql = string.Concat(sql, " `state_iso` = @stateIso");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@stateIso", planAvailableIso.StateIso);
                }
                if (!string.IsNullOrEmpty(planAvailableIso.CountryIso))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `country_iso` = @countryIso");
                    else
                        sql = string.Concat(sql, " `country_iso` = @countryIso");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@countryIso", planAvailableIso.CountryIso);
                }
                if (!string.IsNullOrEmpty(planAvailableIso.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", planAvailableIso.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(planAvailableIso.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", planAvailableIso.CreatedAt);
                }
                if (!string.IsNullOrEmpty(planAvailableIso.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", planAvailableIso.UpdatedAt);
                }
                if (planAvailableIsoWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(planAvailableIsoWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", planAvailableIsoWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(planAvailableIsoWhere.PlanId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `plan_id` = @planIdWhere");
                        else
                            sql = string.Concat(sql, " `plan_id` = @planIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planIdWhere", planAvailableIsoWhere.PlanId);
                    }
                    if (!string.IsNullOrEmpty(planAvailableIsoWhere.StateIso))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `state_iso` = @stateIsoWhere");
                        else
                            sql = string.Concat(sql, " `state_iso` = @stateIsoWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@stateIsoWhere", planAvailableIsoWhere.StateIso);
                    }
                    if (!string.IsNullOrEmpty(planAvailableIsoWhere.CountryIso))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_iso` = @countryIsoWhere");
                        else
                            sql = string.Concat(sql, " `country_iso` = @countryIsoWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIsoWhere", planAvailableIsoWhere.CountryIso);
                    }
                    if (!string.IsNullOrEmpty(planAvailableIsoWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", planAvailableIsoWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(planAvailableIsoWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", planAvailableIsoWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(planAvailableIsoWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", planAvailableIsoWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(planAvailableIso);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanAvailableIsoModel>();
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

        public List<PlanAvailableIsoModel> Update(string connectionString, List<PlanAvailableIsoModel> planAvailableIsoList, List<PlanAvailableIsoModel> planAvailableIsoWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAvailableIsoModel> response = new List<PlanAvailableIsoModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < planAvailableIsoList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_plan_available_isos SET ";
                                if (!string.IsNullOrEmpty(planAvailableIsoList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, planAvailableIsoList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(planAvailableIsoList[i].PlanId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `plan_id` = @planId" + i);
                                    else
                                        sql = string.Concat(sql, " `plan_id` = @planId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planId" + i, planAvailableIsoList[i].PlanId);
                                }
                                if (!string.IsNullOrEmpty(planAvailableIsoList[i].StateIso))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `state_iso` = @stateIso" + i);
                                    else
                                        sql = string.Concat(sql, " `state_iso` = @stateIso" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@stateIso" + i, planAvailableIsoList[i].StateIso);
                                }
                                if (!string.IsNullOrEmpty(planAvailableIsoList[i].CountryIso))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `country_iso` = @countryIso" + i);
                                    else
                                        sql = string.Concat(sql, " `country_iso` = @countryIso" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@countryIso" + i, planAvailableIsoList[i].CountryIso);
                                }
                                if (!string.IsNullOrEmpty(planAvailableIsoList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planAvailableIsoList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(planAvailableIsoList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planAvailableIsoList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(planAvailableIsoList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planAvailableIsoList[i].UpdatedAt);
                                }
                                if (planAvailableIsoWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(planAvailableIsoWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, planAvailableIsoWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(planAvailableIsoWhereList[i].PlanId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `plan_id` = @planIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `plan_id` = @planIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planIdWhere" + i, planAvailableIsoWhereList[i].PlanId);
                                    }
                                    if (!string.IsNullOrEmpty(planAvailableIsoWhereList[i].StateIso))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `state_iso` = @stateIsoWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `state_iso` = @stateIsoWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@stateIsoWhere" + i, planAvailableIsoWhereList[i].StateIso);
                                    }
                                    if (!string.IsNullOrEmpty(planAvailableIsoWhereList[i].CountryIso))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_iso` = @countryIsoWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `country_iso` = @countryIsoWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIsoWhere" + i, planAvailableIsoWhereList[i].CountryIso);
                                    }
                                    if (!string.IsNullOrEmpty(planAvailableIsoWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, planAvailableIsoWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(planAvailableIsoWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, planAvailableIsoWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(planAvailableIsoWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, planAvailableIsoWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(planAvailableIsoList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanAvailableIsoModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanAvailableIsoModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanAvailableIsoModel>();
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

        public List<PlanAvailableIsoModel> Delete(string connectionString, PlanAvailableIsoModel planAvailableIso)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAvailableIsoModel> response = new List<PlanAvailableIsoModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM subst_plan_available_isos  ";
                if (planAvailableIso != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(planAvailableIso.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", planAvailableIso.Id);
                    }
                    if (!string.IsNullOrEmpty(planAvailableIso.PlanId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `plan_id` = @planId");
                        else
                            sql = string.Concat(sql, " `plan_id` = @planId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planId", planAvailableIso.PlanId);
                    }
                    if (!string.IsNullOrEmpty(planAvailableIso.StateIso))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `state_iso` = @stateIso");
                        else
                            sql = string.Concat(sql, " `state_iso` = @stateIso");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@stateIso", planAvailableIso.StateIso);
                    }
                    if (!string.IsNullOrEmpty(planAvailableIso.CountryIso))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_iso` = @countryIso");
                        else
                            sql = string.Concat(sql, " `country_iso` = @countryIso");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIso", planAvailableIso.CountryIso);
                    }
                    if (!string.IsNullOrEmpty(planAvailableIso.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", planAvailableIso.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(planAvailableIso.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", planAvailableIso.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(planAvailableIso.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", planAvailableIso.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(planAvailableIso);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanAvailableIsoModel>();
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

        public List<PlanAvailableIsoModel> Delete(string connectionString, List<PlanAvailableIsoModel> planAvailableIso)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAvailableIsoModel> response = new List<PlanAvailableIsoModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < planAvailableIso.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_plan_available_isos  ";
                                if (planAvailableIso[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(planAvailableIso[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, planAvailableIso[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(planAvailableIso[i].PlanId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `plan_id` = @planId" + i);
                                        else
                                            sql = string.Concat(sql, " `plan_id` = @planId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planId" + i, planAvailableIso[i].PlanId);
                                    }
                                    if (!string.IsNullOrEmpty(planAvailableIso[i].StateIso))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `state_iso` = @stateIso" + i);
                                        else
                                            sql = string.Concat(sql, " `state_iso` = @stateIso" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@stateIso" + i, planAvailableIso[i].StateIso);
                                    }
                                    if (!string.IsNullOrEmpty(planAvailableIso[i].CountryIso))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_iso` = @countryIso" + i);
                                        else
                                            sql = string.Concat(sql, " `country_iso` = @countryIso" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIso" + i, planAvailableIso[i].CountryIso);
                                    }
                                    if (!string.IsNullOrEmpty(planAvailableIso[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, planAvailableIso[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(planAvailableIso[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, planAvailableIso[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(planAvailableIso[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, planAvailableIso[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(planAvailableIso[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanAvailableIsoModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanAvailableIsoModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanAvailableIsoModel>();
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
