using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.CustomClasses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Generals
{
    class ExamTryMaxDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, ExamTryMaxModel> ReadD(string connectionString, ExamTryMaxModel examTryMax, bool firstOne, bool orderByLastDate, ExamTryMaxModel examTryMaxWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ExamTryMaxModel> response = new Dictionary<string, ExamTryMaxModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subscription_id`, `try_max`, `modified_by`, `created_at`, `updated_at`  FROM gwp_exam_try_maxs ";
                {
                    if (examTryMax != null)
                    {
                        if (!string.IsNullOrEmpty(examTryMax.Id))
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
                            cmd.Parameters.AddWithValue("@id", examTryMax.Id);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.SubscriptionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subscription_id` = @subscriptionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subscription_id` = @subscriptionId ");
                            }
                            cmd.Parameters.AddWithValue("@subscriptionId", examTryMax.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.TryMax))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `try_max` = @tryMax ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `try_max` = @tryMax ");
                            }
                            cmd.Parameters.AddWithValue("@tryMax", examTryMax.TryMax);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", examTryMax.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", examTryMax.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", examTryMax.UpdatedAt);
                        }
                    }
                    if (examTryMaxWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + examTryMaxWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.SubscriptionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + examTryMaxWhereLike.SubscriptionId + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.TryMax))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `try_max` LIKE @tryMaxWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `try_max` LIKE @tryMaxWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@tryMaxWhereLike", "%" + examTryMaxWhereLike.TryMax + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + examTryMaxWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + examTryMaxWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + examTryMaxWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = string.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                examTryMax = new ExamTryMaxModel();
                                if (!reader.IsDBNull(0))
                                    examTryMax.Id = reader.GetValue(0).ToString();
                                else
                                    examTryMax.Id = null;
                                if (!reader.IsDBNull(1))
                                    examTryMax.SubscriptionId = reader.GetValue(1).ToString();
                                else
                                    examTryMax.SubscriptionId = null;
                                if (!reader.IsDBNull(2))
                                    examTryMax.TryMax = reader.GetValue(2).ToString();
                                else
                                    examTryMax.TryMax = null;
                                if (!reader.IsDBNull(3))
                                    examTryMax.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    examTryMax.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    examTryMax.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    examTryMax.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    examTryMax.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    examTryMax.UpdatedAt = null;
                                response.Add(examTryMax.Id, examTryMax);
                                examTryMax = new ExamTryMaxModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, ExamTryMaxModel>();
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
                response = new Dictionary<string, ExamTryMaxModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ExamTryMaxModel> ReadD(string connectionString, ExamTryMaxModel examTryMax, bool orderByLastDate, int fromRow, int limit, ExamTryMaxModel examTryMaxWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ExamTryMaxModel> response = new Dictionary<string, ExamTryMaxModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subscription_id`, `try_max`, `modified_by`, `created_at`, `updated_at`  FROM gwp_exam_try_maxs ";
                {
                    if (examTryMax != null)
                    {
                        if (!string.IsNullOrEmpty(examTryMax.Id))
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
                            cmd.Parameters.AddWithValue("@id", examTryMax.Id);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.SubscriptionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subscription_id` = @subscriptionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subscription_id` = @subscriptionId ");
                            }
                            cmd.Parameters.AddWithValue("@subscriptionId", examTryMax.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.TryMax))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `try_max` = @tryMax ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `try_max` = @tryMax ");
                            }
                            cmd.Parameters.AddWithValue("@tryMax", examTryMax.TryMax);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", examTryMax.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", examTryMax.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", examTryMax.UpdatedAt);
                        }
                    }
                    if (examTryMaxWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + examTryMaxWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.SubscriptionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + examTryMaxWhereLike.SubscriptionId + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.TryMax))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `try_max` LIKE @tryMaxWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `try_max` LIKE @tryMaxWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@tryMaxWhereLike", "%" + examTryMaxWhereLike.TryMax + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + examTryMaxWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + examTryMaxWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + examTryMaxWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                examTryMax = new ExamTryMaxModel();
                                if (!reader.IsDBNull(0))
                                    examTryMax.Id = reader.GetValue(0).ToString();
                                else
                                    examTryMax.Id = null;
                                if (!reader.IsDBNull(1))
                                    examTryMax.SubscriptionId = reader.GetValue(1).ToString();
                                else
                                    examTryMax.SubscriptionId = null;
                                if (!reader.IsDBNull(2))
                                    examTryMax.TryMax = reader.GetValue(2).ToString();
                                else
                                    examTryMax.TryMax = null;
                                if (!reader.IsDBNull(3))
                                    examTryMax.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    examTryMax.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    examTryMax.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    examTryMax.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    examTryMax.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    examTryMax.UpdatedAt = null;
                                response.Add(examTryMax.Id, examTryMax);
                                examTryMax = new ExamTryMaxModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, ExamTryMaxModel>();
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
                response = new Dictionary<string, ExamTryMaxModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ExamTryMaxModel> ReadD(string connectionString, List<ExamTryMaxModel> examTryMaxWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ExamTryMaxModel> response = new Dictionary<string, ExamTryMaxModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subscription_id`, `try_max`, `modified_by`, `created_at`, `updated_at`  FROM gwp_exam_try_maxs ";
                if (examTryMaxWhere != null && examTryMaxWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < examTryMaxWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, examTryMaxWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, examTryMaxWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].TryMax))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `try_max` = @tryMax" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@tryMax" + i, examTryMaxWhere[i].TryMax);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, examTryMaxWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, examTryMaxWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, examTryMaxWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != examTryMaxWhere.Count - 1)
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
                            ExamTryMaxModel examTryMax = new ExamTryMaxModel(); if (!reader.IsDBNull(0))
                                examTryMax.Id = reader.GetValue(0).ToString();
                            else
                                examTryMax.Id = null;
                            if (!reader.IsDBNull(1))
                                examTryMax.SubscriptionId = reader.GetValue(1).ToString();
                            else
                                examTryMax.SubscriptionId = null;
                            if (!reader.IsDBNull(2))
                                examTryMax.TryMax = reader.GetValue(2).ToString();
                            else
                                examTryMax.TryMax = null;
                            if (!reader.IsDBNull(3))
                                examTryMax.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                examTryMax.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                examTryMax.CreatedAt = reader.GetValue(4).ToString();
                            else
                                examTryMax.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                examTryMax.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                examTryMax.UpdatedAt = null;
                            response.Add(examTryMax.Id, examTryMax);
                            examTryMax = new ExamTryMaxModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, ExamTryMaxModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, ExamTryMaxModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ExamTryMaxModel> ReadD(string connectionString, List<ExamTryMaxModel> examTryMaxWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ExamTryMaxModel> response = new Dictionary<string, ExamTryMaxModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subscription_id`, `try_max`, `modified_by`, `created_at`, `updated_at`  FROM gwp_exam_try_maxs ";
                if (examTryMaxWhere != null && examTryMaxWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < examTryMaxWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, examTryMaxWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, examTryMaxWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].TryMax))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `try_max` = @tryMax" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@tryMax" + i, examTryMaxWhere[i].TryMax);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, examTryMaxWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, examTryMaxWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, examTryMaxWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != examTryMaxWhere.Count - 1)
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
                            ExamTryMaxModel examTryMax = new ExamTryMaxModel(); if (!reader.IsDBNull(0))
                                examTryMax.Id = reader.GetValue(0).ToString();
                            else
                                examTryMax.Id = null;
                            if (!reader.IsDBNull(1))
                                examTryMax.SubscriptionId = reader.GetValue(1).ToString();
                            else
                                examTryMax.SubscriptionId = null;
                            if (!reader.IsDBNull(2))
                                examTryMax.TryMax = reader.GetValue(2).ToString();
                            else
                                examTryMax.TryMax = null;
                            if (!reader.IsDBNull(3))
                                examTryMax.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                examTryMax.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                examTryMax.CreatedAt = reader.GetValue(4).ToString();
                            else
                                examTryMax.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                examTryMax.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                examTryMax.UpdatedAt = null;
                            response.Add(examTryMax.Id, examTryMax);
                            examTryMax = new ExamTryMaxModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, ExamTryMaxModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, ExamTryMaxModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ExamTryMaxModel> Read(string connectionString, ExamTryMaxModel examTryMax, bool firstOne, bool orderByLastDate, ExamTryMaxModel examTryMaxWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ExamTryMaxModel> response = new List<ExamTryMaxModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subscription_id`, `try_max`, `modified_by`, `created_at`, `updated_at`  FROM gwp_exam_try_maxs ";
                {
                    if (examTryMax != null)
                    {
                        if (!string.IsNullOrEmpty(examTryMax.Id))
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
                            cmd.Parameters.AddWithValue("@id", examTryMax.Id);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.SubscriptionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subscription_id` = @subscriptionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subscription_id` = @subscriptionId ");
                            }
                            cmd.Parameters.AddWithValue("@subscriptionId", examTryMax.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.TryMax))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `try_max` = @tryMax ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `try_max` = @tryMax ");
                            }
                            cmd.Parameters.AddWithValue("@tryMax", examTryMax.TryMax);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", examTryMax.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", examTryMax.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", examTryMax.UpdatedAt);
                        }
                    }
                    if (examTryMaxWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + examTryMaxWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.SubscriptionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + examTryMaxWhereLike.SubscriptionId + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.TryMax))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `try_max` LIKE @tryMaxWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `try_max` LIKE @tryMaxWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@tryMaxWhereLike", "%" + examTryMaxWhereLike.TryMax + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + examTryMaxWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + examTryMaxWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + examTryMaxWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = string.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                examTryMax = new ExamTryMaxModel();
                                if (!reader.IsDBNull(0))
                                    examTryMax.Id = reader.GetValue(0).ToString();
                                else
                                    examTryMax.Id = null;
                                if (!reader.IsDBNull(1))
                                    examTryMax.SubscriptionId = reader.GetValue(1).ToString();
                                else
                                    examTryMax.SubscriptionId = null;
                                if (!reader.IsDBNull(2))
                                    examTryMax.TryMax = reader.GetValue(2).ToString();
                                else
                                    examTryMax.TryMax = null;
                                if (!reader.IsDBNull(3))
                                    examTryMax.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    examTryMax.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    examTryMax.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    examTryMax.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    examTryMax.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    examTryMax.UpdatedAt = null;
                                response.Add(examTryMax);
                                examTryMax = new ExamTryMaxModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ExamTryMaxModel>();
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
                response = new List<ExamTryMaxModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ExamTryMaxModel> Read(string connectionString, ExamTryMaxModel examTryMax, bool orderByLastDate, int fromRow, int limit, ExamTryMaxModel examTryMaxWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ExamTryMaxModel> response = new List<ExamTryMaxModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subscription_id`, `try_max`, `modified_by`, `created_at`, `updated_at`  FROM gwp_exam_try_maxs ";
                {
                    if (examTryMax != null)
                    {
                        if (!string.IsNullOrEmpty(examTryMax.Id))
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
                            cmd.Parameters.AddWithValue("@id", examTryMax.Id);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.SubscriptionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subscription_id` = @subscriptionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subscription_id` = @subscriptionId ");
                            }
                            cmd.Parameters.AddWithValue("@subscriptionId", examTryMax.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.TryMax))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `try_max` = @tryMax ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `try_max` = @tryMax ");
                            }
                            cmd.Parameters.AddWithValue("@tryMax", examTryMax.TryMax);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", examTryMax.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", examTryMax.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(examTryMax.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", examTryMax.UpdatedAt);
                        }
                    }
                    if (examTryMaxWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + examTryMaxWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.SubscriptionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + examTryMaxWhereLike.SubscriptionId + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.TryMax))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `try_max` LIKE @tryMaxWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `try_max` LIKE @tryMaxWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@tryMaxWhereLike", "%" + examTryMaxWhereLike.TryMax + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + examTryMaxWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + examTryMaxWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + examTryMaxWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                examTryMax = new ExamTryMaxModel();
                                if (!reader.IsDBNull(0))
                                    examTryMax.Id = reader.GetValue(0).ToString();
                                else
                                    examTryMax.Id = null;
                                if (!reader.IsDBNull(1))
                                    examTryMax.SubscriptionId = reader.GetValue(1).ToString();
                                else
                                    examTryMax.SubscriptionId = null;
                                if (!reader.IsDBNull(2))
                                    examTryMax.TryMax = reader.GetValue(2).ToString();
                                else
                                    examTryMax.TryMax = null;
                                if (!reader.IsDBNull(3))
                                    examTryMax.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    examTryMax.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    examTryMax.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    examTryMax.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    examTryMax.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    examTryMax.UpdatedAt = null;
                                response.Add(examTryMax);
                                examTryMax = new ExamTryMaxModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ExamTryMaxModel>();
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
                response = new List<ExamTryMaxModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ExamTryMaxModel> Read(string connectionString, List<ExamTryMaxModel> examTryMaxWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ExamTryMaxModel> response = new List<ExamTryMaxModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subscription_id`, `try_max`, `modified_by`, `created_at`, `updated_at`  FROM gwp_exam_try_maxs ";
                if (examTryMaxWhere != null && examTryMaxWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < examTryMaxWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, examTryMaxWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, examTryMaxWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].TryMax))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `try_max` = @tryMax" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@tryMax" + i, examTryMaxWhere[i].TryMax);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, examTryMaxWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, examTryMaxWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, examTryMaxWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != examTryMaxWhere.Count - 1)
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
                            ExamTryMaxModel examTryMax = new ExamTryMaxModel(); if (!reader.IsDBNull(0))
                                examTryMax.Id = reader.GetValue(0).ToString();
                            else
                                examTryMax.Id = null;
                            if (!reader.IsDBNull(1))
                                examTryMax.SubscriptionId = reader.GetValue(1).ToString();
                            else
                                examTryMax.SubscriptionId = null;
                            if (!reader.IsDBNull(2))
                                examTryMax.TryMax = reader.GetValue(2).ToString();
                            else
                                examTryMax.TryMax = null;
                            if (!reader.IsDBNull(3))
                                examTryMax.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                examTryMax.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                examTryMax.CreatedAt = reader.GetValue(4).ToString();
                            else
                                examTryMax.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                examTryMax.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                examTryMax.UpdatedAt = null;
                            response.Add(examTryMax);
                            examTryMax = new ExamTryMaxModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<ExamTryMaxModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ExamTryMaxModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ExamTryMaxModel> Read(string connectionString, List<ExamTryMaxModel> examTryMaxWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ExamTryMaxModel> response = new List<ExamTryMaxModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subscription_id`, `try_max`, `modified_by`, `created_at`, `updated_at`  FROM gwp_exam_try_maxs ";
                if (examTryMaxWhere != null && examTryMaxWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < examTryMaxWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, examTryMaxWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, examTryMaxWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].TryMax))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `try_max` = @tryMax" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@tryMax" + i, examTryMaxWhere[i].TryMax);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, examTryMaxWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, examTryMaxWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(examTryMaxWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, examTryMaxWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != examTryMaxWhere.Count - 1)
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
                            ExamTryMaxModel examTryMax = new ExamTryMaxModel(); if (!reader.IsDBNull(0))
                                examTryMax.Id = reader.GetValue(0).ToString();
                            else
                                examTryMax.Id = null;
                            if (!reader.IsDBNull(1))
                                examTryMax.SubscriptionId = reader.GetValue(1).ToString();
                            else
                                examTryMax.SubscriptionId = null;
                            if (!reader.IsDBNull(2))
                                examTryMax.TryMax = reader.GetValue(2).ToString();
                            else
                                examTryMax.TryMax = null;
                            if (!reader.IsDBNull(3))
                                examTryMax.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                examTryMax.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                examTryMax.CreatedAt = reader.GetValue(4).ToString();
                            else
                                examTryMax.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                examTryMax.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                examTryMax.UpdatedAt = null;
                            response.Add(examTryMax);
                            examTryMax = new ExamTryMaxModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<ExamTryMaxModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ExamTryMaxModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ExamTryMaxModel> Create(string connectionString, ExamTryMaxModel examTryMax)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ExamTryMaxModel> response = new List<ExamTryMaxModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO gwp_exam_try_maxs (";
                string values = "(";
                if (!string.IsNullOrEmpty(examTryMax.Id))
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
                    cmd.Parameters.AddWithValue("@id", examTryMax.Id);
                }
                if (!string.IsNullOrEmpty(examTryMax.SubscriptionId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `subscription_id` ");
                        values = string.Concat(values, ", @subscriptionId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `subscription_id` ");
                        values = string.Concat(values, " @subscriptionId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subscriptionId", examTryMax.SubscriptionId);
                }
                if (!string.IsNullOrEmpty(examTryMax.TryMax))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `try_max` ");
                        values = string.Concat(values, ", @tryMax");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `try_max` ");
                        values = string.Concat(values, " @tryMax");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@tryMax", examTryMax.TryMax);
                }
                if (!string.IsNullOrEmpty(examTryMax.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", examTryMax.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(examTryMax.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", examTryMax.CreatedAt);
                }
                if (!string.IsNullOrEmpty(examTryMax.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", examTryMax.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                examTryMax.Id = cmd.LastInsertedId.ToString();
                response.Add(examTryMax);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ExamTryMaxModel>();
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
        public List<ExamTryMaxModel> Create(string connectionString, List<ExamTryMaxModel> examTryMaxList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ExamTryMaxModel> response = new List<ExamTryMaxModel>();
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
                        for (int i = 0; i < examTryMaxList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO gwp_exam_try_maxs (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(examTryMaxList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, examTryMaxList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(examTryMaxList[i].SubscriptionId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `subscription_id` ");
                                        values = string.Concat(values, ", @subscriptionId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `subscription_id` ");
                                        values = string.Concat(values, "@subscriptionId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subscriptionId" + i, examTryMaxList[i].SubscriptionId);
                                }
                                if (!string.IsNullOrEmpty(examTryMaxList[i].TryMax))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `try_max` ");
                                        values = string.Concat(values, ", @tryMax" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `try_max` ");
                                        values = string.Concat(values, "@tryMax" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@tryMax" + i, examTryMaxList[i].TryMax);
                                }
                                if (!string.IsNullOrEmpty(examTryMaxList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, examTryMaxList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(examTryMaxList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, examTryMaxList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(examTryMaxList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, examTryMaxList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(examTryMaxList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<ExamTryMaxModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ExamTryMaxModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ExamTryMaxModel>();
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

        public List<ExamTryMaxModel> Update(string connectionString, ExamTryMaxModel examTryMax, ExamTryMaxModel examTryMaxWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ExamTryMaxModel> response = new List<ExamTryMaxModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE gwp_exam_try_maxs SET ";
                if (!string.IsNullOrEmpty(examTryMax.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", examTryMax.Id);
                }
                if (!string.IsNullOrEmpty(examTryMax.SubscriptionId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `subscription_id` = @subscriptionId");
                    else
                        sql = string.Concat(sql, " `subscription_id` = @subscriptionId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subscriptionId", examTryMax.SubscriptionId);
                }
                if (!string.IsNullOrEmpty(examTryMax.TryMax))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `try_max` = @tryMax");
                    else
                        sql = string.Concat(sql, " `try_max` = @tryMax");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@tryMax", examTryMax.TryMax);
                }
                if (!string.IsNullOrEmpty(examTryMax.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", examTryMax.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(examTryMax.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", examTryMax.CreatedAt);
                }
                if (!string.IsNullOrEmpty(examTryMax.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", examTryMax.UpdatedAt);
                }
                if (examTryMaxWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(examTryMaxWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", examTryMaxWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(examTryMaxWhere.SubscriptionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionIdWhere");
                        else
                            sql = string.Concat(sql, " `subscription_id` = @subscriptionIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionIdWhere", examTryMaxWhere.SubscriptionId);
                    }
                    if (!string.IsNullOrEmpty(examTryMaxWhere.TryMax))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `try_max` = @tryMaxWhere");
                        else
                            sql = string.Concat(sql, " `try_max` = @tryMaxWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@tryMaxWhere", examTryMaxWhere.TryMax);
                    }
                    if (!string.IsNullOrEmpty(examTryMaxWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", examTryMaxWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(examTryMaxWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", examTryMaxWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(examTryMaxWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", examTryMaxWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(examTryMax);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ExamTryMaxModel>();
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

        public List<ExamTryMaxModel> Update(string connectionString, List<ExamTryMaxModel> examTryMaxList, List<ExamTryMaxModel> examTryMaxWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ExamTryMaxModel> response = new List<ExamTryMaxModel>();
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
                        for (int i = 0; i < examTryMaxList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE gwp_exam_try_maxs SET ";
                                if (!string.IsNullOrEmpty(examTryMaxList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, examTryMaxList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(examTryMaxList[i].SubscriptionId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `subscription_id` = @subscriptionId" + i);
                                    else
                                        sql = string.Concat(sql, " `subscription_id` = @subscriptionId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subscriptionId" + i, examTryMaxList[i].SubscriptionId);
                                }
                                if (!string.IsNullOrEmpty(examTryMaxList[i].TryMax))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `try_max` = @tryMax" + i);
                                    else
                                        sql = string.Concat(sql, " `try_max` = @tryMax" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@tryMax" + i, examTryMaxList[i].TryMax);
                                }
                                if (!string.IsNullOrEmpty(examTryMaxList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, examTryMaxList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(examTryMaxList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, examTryMaxList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(examTryMaxList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, examTryMaxList[i].UpdatedAt);
                                }
                                if (examTryMaxWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(examTryMaxWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, examTryMaxWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(examTryMaxWhereList[i].SubscriptionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `subscription_id` = @subscriptionIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionIdWhere" + i, examTryMaxWhereList[i].SubscriptionId);
                                    }
                                    if (!string.IsNullOrEmpty(examTryMaxWhereList[i].TryMax))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `try_max` = @tryMaxWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `try_max` = @tryMaxWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@tryMaxWhere" + i, examTryMaxWhereList[i].TryMax);
                                    }
                                    if (!string.IsNullOrEmpty(examTryMaxWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, examTryMaxWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(examTryMaxWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, examTryMaxWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(examTryMaxWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, examTryMaxWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(examTryMaxList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ExamTryMaxModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ExamTryMaxModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ExamTryMaxModel>();
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

        public List<ExamTryMaxModel> Delete(string connectionString, ExamTryMaxModel examTryMax)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ExamTryMaxModel> response = new List<ExamTryMaxModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM gwp_exam_try_maxs  ";
                if (examTryMax != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(examTryMax.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", examTryMax.Id);
                    }
                    if (!string.IsNullOrEmpty(examTryMax.SubscriptionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionId");
                        else
                            sql = string.Concat(sql, " `subscription_id` = @subscriptionId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionId", examTryMax.SubscriptionId);
                    }
                    if (!string.IsNullOrEmpty(examTryMax.TryMax))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `try_max` = @tryMax");
                        else
                            sql = string.Concat(sql, " `try_max` = @tryMax");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@tryMax", examTryMax.TryMax);
                    }
                    if (!string.IsNullOrEmpty(examTryMax.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", examTryMax.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(examTryMax.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", examTryMax.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(examTryMax.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", examTryMax.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(examTryMax);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ExamTryMaxModel>();
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

        public List<ExamTryMaxModel> Delete(string connectionString, List<ExamTryMaxModel> examTryMax)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ExamTryMaxModel> response = new List<ExamTryMaxModel>();
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
                        for (int i = 0; i < examTryMax.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM gwp_exam_try_maxs  ";
                                if (examTryMax[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(examTryMax[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, examTryMax[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(examTryMax[i].SubscriptionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionId" + i);
                                        else
                                            sql = string.Concat(sql, " `subscription_id` = @subscriptionId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionId" + i, examTryMax[i].SubscriptionId);
                                    }
                                    if (!string.IsNullOrEmpty(examTryMax[i].TryMax))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `try_max` = @tryMax" + i);
                                        else
                                            sql = string.Concat(sql, " `try_max` = @tryMax" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@tryMax" + i, examTryMax[i].TryMax);
                                    }
                                    if (!string.IsNullOrEmpty(examTryMax[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, examTryMax[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(examTryMax[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, examTryMax[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(examTryMax[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, examTryMax[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(examTryMax[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ExamTryMaxModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ExamTryMaxModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ExamTryMaxModel>();
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
