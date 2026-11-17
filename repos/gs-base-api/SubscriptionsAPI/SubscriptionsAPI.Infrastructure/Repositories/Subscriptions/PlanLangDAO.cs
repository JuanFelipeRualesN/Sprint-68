using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class PlanLangDAO
    {
        private MySqlConnection _connection;
        public List<PlanLangModel> Read(string connectionString, PlanLangModel planLang, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLangModel> response = new List<PlanLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `plan_id`, `lang_code`, `name`, `description`, `modified_by`, `created_at`, `updated_at`  FROM subst_plan_langs ";
                {
                    if (planLang != null)
                    {
                        if (!string.IsNullOrEmpty(planLang.Id))
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
                            cmd.Parameters.AddWithValue("@id", planLang.Id);
                        }
                        if (!string.IsNullOrEmpty(planLang.PlanId))
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
                            cmd.Parameters.AddWithValue("@planId", planLang.PlanId);
                        }
                        if (!string.IsNullOrEmpty(planLang.LangCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `lang_code` = @langCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `lang_code` = @langCode ");
                            }
                            cmd.Parameters.AddWithValue("@langCode", planLang.LangCode);
                        }
                        if (!string.IsNullOrEmpty(planLang.Name))
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
                            cmd.Parameters.AddWithValue("@name", planLang.Name);
                        }
                        if (!string.IsNullOrEmpty(planLang.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", planLang.Description);
                        }
                        if (!string.IsNullOrEmpty(planLang.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planLang.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(planLang.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planLang.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(planLang.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planLang.UpdatedAt);
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
                                    planLang.Id = reader.GetValue(0).ToString();
                                else
                                    planLang.Id = null;
                                if (!reader.IsDBNull(1))
                                    planLang.PlanId = reader.GetValue(1).ToString();
                                else
                                    planLang.PlanId = null;
                                if (!reader.IsDBNull(2))
                                    planLang.LangCode = reader.GetValue(2).ToString();
                                else
                                    planLang.LangCode = null;
                                if (!reader.IsDBNull(3))
                                    planLang.Name = reader.GetValue(3).ToString();
                                else
                                    planLang.Name = null;
                                if (!reader.IsDBNull(4))
                                    planLang.Description = reader.GetValue(4).ToString();
                                else
                                    planLang.Description = null;
                                if (!reader.IsDBNull(5))
                                    planLang.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    planLang.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    planLang.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    planLang.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    planLang.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    planLang.UpdatedAt = null;
                                response.Add(planLang);
                                planLang = new PlanLangModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlanLangModel>();
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
                response = new List<PlanLangModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanLangModel> Read(string connectionString, List<PlanLangModel> planLangWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLangModel> response = new List<PlanLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_id`, `lang_code`, `name`, `description`, `modified_by`, `created_at`, `updated_at`  FROM subst_plan_langs ";
                if (planLangWhere != null && planLangWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planLangWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(planLangWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planLangWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(planLangWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, planLangWhere[i].PlanId);
                        }
                        if (!string.IsNullOrEmpty(planLangWhere[i].LangCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lang_code` = @langCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@langCode" + i, planLangWhere[i].LangCode);
                        }
                        if (!string.IsNullOrEmpty(planLangWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, planLangWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(planLangWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, planLangWhere[i].Description);
                        }
                        if (!string.IsNullOrEmpty(planLangWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planLangWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(planLangWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planLangWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(planLangWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planLangWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planLangWhere.Count - 1)
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
                            PlanLangModel planLang = new PlanLangModel(); if (!reader.IsDBNull(0))
                                planLang.Id = reader.GetValue(0).ToString();
                            else
                                planLang.Id = null;
                            if (!reader.IsDBNull(1))
                                planLang.PlanId = reader.GetValue(1).ToString();
                            else
                                planLang.PlanId = null;
                            if (!reader.IsDBNull(2))
                                planLang.LangCode = reader.GetValue(2).ToString();
                            else
                                planLang.LangCode = null;
                            if (!reader.IsDBNull(3))
                                planLang.Name = reader.GetValue(3).ToString();
                            else
                                planLang.Name = null;
                            if (!reader.IsDBNull(4))
                                planLang.Description = reader.GetValue(4).ToString();
                            else
                                planLang.Description = null;
                            if (!reader.IsDBNull(5))
                                planLang.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                planLang.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                planLang.CreatedAt = reader.GetValue(6).ToString();
                            else
                                planLang.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                planLang.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                planLang.UpdatedAt = null;
                            response.Add(planLang);
                            planLang = new PlanLangModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlanLangModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLangModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanLangModel> Create(string connectionString, PlanLangModel planLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLangModel> response = new List<PlanLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO subst_plan_langs (";
                string values = "(";
                if (!string.IsNullOrEmpty(planLang.Id))
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
                    cmd.Parameters.AddWithValue("@id", planLang.Id);
                }
                if (!string.IsNullOrEmpty(planLang.PlanId))
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
                    cmd.Parameters.AddWithValue("@planId", planLang.PlanId);
                }
                if (!string.IsNullOrEmpty(planLang.LangCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `lang_code` ");
                        values = string.Concat(values, ", @langCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `lang_code` ");
                        values = string.Concat(values, " @langCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@langCode", planLang.LangCode);
                }
                if (!string.IsNullOrEmpty(planLang.Name))
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
                    cmd.Parameters.AddWithValue("@name", planLang.Name);
                }
                if (!string.IsNullOrEmpty(planLang.Description))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `description` ");
                        values = string.Concat(values, ", @description");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `description` ");
                        values = string.Concat(values, " @description");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@description", planLang.Description);
                }
                if (!string.IsNullOrEmpty(planLang.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", planLang.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(planLang.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", planLang.CreatedAt);
                }
                if (!string.IsNullOrEmpty(planLang.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", planLang.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                planLang.Id = cmd.LastInsertedId.ToString();
                response.Add(planLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLangModel>();
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
        public List<PlanLangModel> Create(string connectionString, List<PlanLangModel> planLangList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLangModel> response = new List<PlanLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < planLangList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_plan_langs (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(planLangList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, planLangList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(planLangList[i].PlanId))
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
                                    cmd.Parameters.AddWithValue("@planId" + i, planLangList[i].PlanId);
                                }
                                if (!string.IsNullOrEmpty(planLangList[i].LangCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `lang_code` ");
                                        values = string.Concat(values, ", @langCode" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `lang_code` ");
                                        values = string.Concat(values, "@langCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@langCode" + i, planLangList[i].LangCode);
                                }
                                if (!string.IsNullOrEmpty(planLangList[i].Name))
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
                                    cmd.Parameters.AddWithValue("@name" + i, planLangList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(planLangList[i].Description))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `description` ");
                                        values = string.Concat(values, ", @description" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `description` ");
                                        values = string.Concat(values, "@description" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@description" + i, planLangList[i].Description);
                                }
                                if (!string.IsNullOrEmpty(planLangList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planLangList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(planLangList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planLangList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(planLangList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planLangList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(planLangList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLangModel>();
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

        public List<PlanLangModel> Update(string connectionString, PlanLangModel planLang, PlanLangModel planLangWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLangModel> response = new List<PlanLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE subst_plan_langs SET ";
                if (!string.IsNullOrEmpty(planLang.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", planLang.Id);
                }
                if (!string.IsNullOrEmpty(planLang.PlanId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `plan_id` = @planId");
                    else
                        sql = string.Concat(sql, " `plan_id` = @planId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planId", planLang.PlanId);
                }
                if (!string.IsNullOrEmpty(planLang.LangCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `lang_code` = @langCode");
                    else
                        sql = string.Concat(sql, " `lang_code` = @langCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@langCode", planLang.LangCode);
                }
                if (!string.IsNullOrEmpty(planLang.Name))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `name` = @name");
                    else
                        sql = string.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", planLang.Name);
                }
                if (!string.IsNullOrEmpty(planLang.Description))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `description` = @description");
                    else
                        sql = string.Concat(sql, " `description` = @description");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@description", planLang.Description);
                }
                if (!string.IsNullOrEmpty(planLang.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", planLang.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(planLang.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", planLang.CreatedAt);
                }
                if (!string.IsNullOrEmpty(planLang.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", planLang.UpdatedAt);
                }
                if (planLangWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(planLangWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", planLangWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(planLangWhere.PlanId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `plan_id` = @planIdWhere");
                        else
                            sql = string.Concat(sql, " `plan_id` = @planIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planIdWhere", planLangWhere.PlanId);
                    }
                    if (!string.IsNullOrEmpty(planLangWhere.LangCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `lang_code` = @langCodeWhere");
                        else
                            sql = string.Concat(sql, " `lang_code` = @langCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCodeWhere", planLangWhere.LangCode);
                    }
                    if (!string.IsNullOrEmpty(planLangWhere.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = string.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", planLangWhere.Name);
                    }
                    if (!string.IsNullOrEmpty(planLangWhere.Description))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `description` = @descriptionWhere");
                        else
                            sql = string.Concat(sql, " `description` = @descriptionWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descriptionWhere", planLangWhere.Description);
                    }
                    if (!string.IsNullOrEmpty(planLangWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", planLangWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(planLangWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", planLangWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(planLangWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", planLangWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(planLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLangModel>();
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

        public List<PlanLangModel> Update(string connectionString, List<PlanLangModel> planLangList, List<PlanLangModel> planLangWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLangModel> response = new List<PlanLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < planLangList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_plan_langs SET ";
                                if (!string.IsNullOrEmpty(planLangList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, planLangList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(planLangList[i].PlanId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `plan_id` = @planId" + i);
                                    else
                                        sql = string.Concat(sql, " `plan_id` = @planId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planId" + i, planLangList[i].PlanId);
                                }
                                if (!string.IsNullOrEmpty(planLangList[i].LangCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `lang_code` = @langCode" + i);
                                    else
                                        sql = string.Concat(sql, " `lang_code` = @langCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@langCode" + i, planLangList[i].LangCode);
                                }
                                if (!string.IsNullOrEmpty(planLangList[i].Name))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = string.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, planLangList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(planLangList[i].Description))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `description` = @description" + i);
                                    else
                                        sql = string.Concat(sql, " `description` = @description" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@description" + i, planLangList[i].Description);
                                }
                                if (!string.IsNullOrEmpty(planLangList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planLangList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(planLangList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planLangList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(planLangList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planLangList[i].UpdatedAt);
                                }
                                if (planLangWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(planLangWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, planLangWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(planLangWhereList[i].PlanId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `plan_id` = @planIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `plan_id` = @planIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planIdWhere" + i, planLangWhereList[i].PlanId);
                                    }
                                    if (!string.IsNullOrEmpty(planLangWhereList[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `lang_code` = @langCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `lang_code` = @langCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCodeWhere" + i, planLangWhereList[i].LangCode);
                                    }
                                    if (!string.IsNullOrEmpty(planLangWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, planLangWhereList[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(planLangWhereList[i].Description))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `description` = @descriptionWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `description` = @descriptionWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descriptionWhere" + i, planLangWhereList[i].Description);
                                    }
                                    if (!string.IsNullOrEmpty(planLangWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, planLangWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(planLangWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, planLangWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(planLangWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, planLangWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(planLangList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLangModel>();
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

        public List<PlanLangModel> Delete(string connectionString, PlanLangModel planLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLangModel> response = new List<PlanLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM subst_plan_langs  ";
                if (planLang != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(planLang.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", planLang.Id);
                    }
                    if (!string.IsNullOrEmpty(planLang.PlanId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `plan_id` = @planId");
                        else
                            sql = string.Concat(sql, " `plan_id` = @planId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planId", planLang.PlanId);
                    }
                    if (!string.IsNullOrEmpty(planLang.LangCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `lang_code` = @langCode");
                        else
                            sql = string.Concat(sql, " `lang_code` = @langCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCode", planLang.LangCode);
                    }
                    if (!string.IsNullOrEmpty(planLang.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @name");
                        else
                            sql = string.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", planLang.Name);
                    }
                    if (!string.IsNullOrEmpty(planLang.Description))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `description` = @description");
                        else
                            sql = string.Concat(sql, " `description` = @description");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@description", planLang.Description);
                    }
                    if (!string.IsNullOrEmpty(planLang.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", planLang.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(planLang.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", planLang.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(planLang.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", planLang.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(planLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLangModel>();
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

        public List<PlanLangModel> Delete(string connectionString, List<PlanLangModel> planLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLangModel> response = new List<PlanLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < planLang.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_plan_langs  ";
                                if (planLang[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(planLang[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, planLang[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(planLang[i].PlanId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `plan_id` = @planId" + i);
                                        else
                                            sql = string.Concat(sql, " `plan_id` = @planId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planId" + i, planLang[i].PlanId);
                                    }
                                    if (!string.IsNullOrEmpty(planLang[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `lang_code` = @langCode" + i);
                                        else
                                            sql = string.Concat(sql, " `lang_code` = @langCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCode" + i, planLang[i].LangCode);
                                    }
                                    if (!string.IsNullOrEmpty(planLang[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, planLang[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(planLang[i].Description))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `description` = @description" + i);
                                        else
                                            sql = string.Concat(sql, " `description` = @description" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@description" + i, planLang[i].Description);
                                    }
                                    if (!string.IsNullOrEmpty(planLang[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, planLang[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(planLang[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, planLang[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(planLang[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, planLang[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(planLang[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLangModel>();
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
