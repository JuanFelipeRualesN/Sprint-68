using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class CancellationLogDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, CancellationLogModel> ReadD(string connectionString, CancellationLogModel cancellationLog, bool firstOne, bool orderByLastDate, CancellationLogModel cancellationLogWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, CancellationLogModel> response = new Dictionary<string, CancellationLogModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subscription_id`, `cancellation_now`, `cancellation_reason_id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_logs ";
                {
                    if (cancellationLog != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationLog.Id))
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
                            cmd.Parameters.AddWithValue("@id", cancellationLog.Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionId", cancellationLog.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.CancellationNow))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `cancellation_now` = @cancellationNow ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `cancellation_now` = @cancellationNow ");
                            }
                            cmd.Parameters.AddWithValue("@cancellationNow", cancellationLog.CancellationNow);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.CancellationReasonId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `cancellation_reason_id` = @cancellationReasonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `cancellation_reason_id` = @cancellationReasonId ");
                            }
                            cmd.Parameters.AddWithValue("@cancellationReasonId", cancellationLog.CancellationReasonId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.Reason))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `reason` = @reason ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `reason` = @reason ");
                            }
                            cmd.Parameters.AddWithValue("@reason", cancellationLog.Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", cancellationLog.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", cancellationLog.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", cancellationLog.UpdatedAt);
                        }
                    }
                    if (cancellationLogWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + cancellationLogWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + cancellationLogWhereLike.SubscriptionId + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.CancellationNow))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `cancellation_now` LIKE @cancellationNowWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `cancellation_now` LIKE @cancellationNowWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@cancellationNowWhereLike", "%" + cancellationLogWhereLike.CancellationNow + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.CancellationReasonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `cancellation_reason_id` LIKE @cancellationReasonIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `cancellation_reason_id` LIKE @cancellationReasonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@cancellationReasonIdWhereLike", "%" + cancellationLogWhereLike.CancellationReasonId + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.Reason))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `reason` LIKE @reasonWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `reason` LIKE @reasonWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@reasonWhereLike", "%" + cancellationLogWhereLike.Reason + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + cancellationLogWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + cancellationLogWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + cancellationLogWhereLike.UpdatedAt + "%");
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
                                cancellationLog = new CancellationLogModel();
                                if (!reader.IsDBNull(0))
                                    cancellationLog.Id = reader.GetValue(0).ToString();
                                else
                                    cancellationLog.Id = null;
                                if (!reader.IsDBNull(1))
                                    cancellationLog.SubscriptionId = reader.GetValue(1).ToString();
                                else
                                    cancellationLog.SubscriptionId = null;
                                if (!reader.IsDBNull(2))
                                    cancellationLog.CancellationNow = reader.GetValue(2).ToString();
                                else
                                    cancellationLog.CancellationNow = null;
                                if (!reader.IsDBNull(3))
                                    cancellationLog.CancellationReasonId = reader.GetValue(3).ToString();
                                else
                                    cancellationLog.CancellationReasonId = null;
                                if (!reader.IsDBNull(4))
                                    cancellationLog.Reason = reader.GetValue(4).ToString();
                                else
                                    cancellationLog.Reason = null;
                                if (!reader.IsDBNull(5))
                                    cancellationLog.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    cancellationLog.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    cancellationLog.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    cancellationLog.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    cancellationLog.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    cancellationLog.UpdatedAt = null;
                                response.Add(cancellationLog.Id, cancellationLog);
                                cancellationLog = new CancellationLogModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, CancellationLogModel>();
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
                response = new Dictionary<string, CancellationLogModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, CancellationLogModel> ReadD(string connectionString, CancellationLogModel cancellationLog, bool orderByLastDate, int fromRow, int limit, CancellationLogModel cancellationLogWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, CancellationLogModel> response = new Dictionary<string, CancellationLogModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subscription_id`, `cancellation_now`, `cancellation_reason_id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_logs ";
                {
                    if (cancellationLog != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationLog.Id))
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
                            cmd.Parameters.AddWithValue("@id", cancellationLog.Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionId", cancellationLog.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.CancellationNow))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `cancellation_now` = @cancellationNow ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `cancellation_now` = @cancellationNow ");
                            }
                            cmd.Parameters.AddWithValue("@cancellationNow", cancellationLog.CancellationNow);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.CancellationReasonId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `cancellation_reason_id` = @cancellationReasonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `cancellation_reason_id` = @cancellationReasonId ");
                            }
                            cmd.Parameters.AddWithValue("@cancellationReasonId", cancellationLog.CancellationReasonId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.Reason))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `reason` = @reason ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `reason` = @reason ");
                            }
                            cmd.Parameters.AddWithValue("@reason", cancellationLog.Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", cancellationLog.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", cancellationLog.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", cancellationLog.UpdatedAt);
                        }
                    }
                    if (cancellationLogWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + cancellationLogWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + cancellationLogWhereLike.SubscriptionId + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.CancellationNow))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `cancellation_now` LIKE @cancellationNowWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `cancellation_now` LIKE @cancellationNowWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@cancellationNowWhereLike", "%" + cancellationLogWhereLike.CancellationNow + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.CancellationReasonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `cancellation_reason_id` LIKE @cancellationReasonIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `cancellation_reason_id` LIKE @cancellationReasonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@cancellationReasonIdWhereLike", "%" + cancellationLogWhereLike.CancellationReasonId + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.Reason))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `reason` LIKE @reasonWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `reason` LIKE @reasonWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@reasonWhereLike", "%" + cancellationLogWhereLike.Reason + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + cancellationLogWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + cancellationLogWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + cancellationLogWhereLike.UpdatedAt + "%");
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
                                cancellationLog = new CancellationLogModel();
                                if (!reader.IsDBNull(0))
                                    cancellationLog.Id = reader.GetValue(0).ToString();
                                else
                                    cancellationLog.Id = null;
                                if (!reader.IsDBNull(1))
                                    cancellationLog.SubscriptionId = reader.GetValue(1).ToString();
                                else
                                    cancellationLog.SubscriptionId = null;
                                if (!reader.IsDBNull(2))
                                    cancellationLog.CancellationNow = reader.GetValue(2).ToString();
                                else
                                    cancellationLog.CancellationNow = null;
                                if (!reader.IsDBNull(3))
                                    cancellationLog.CancellationReasonId = reader.GetValue(3).ToString();
                                else
                                    cancellationLog.CancellationReasonId = null;
                                if (!reader.IsDBNull(4))
                                    cancellationLog.Reason = reader.GetValue(4).ToString();
                                else
                                    cancellationLog.Reason = null;
                                if (!reader.IsDBNull(5))
                                    cancellationLog.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    cancellationLog.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    cancellationLog.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    cancellationLog.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    cancellationLog.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    cancellationLog.UpdatedAt = null;
                                response.Add(cancellationLog.Id, cancellationLog);
                                cancellationLog = new CancellationLogModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, CancellationLogModel>();
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
                response = new Dictionary<string, CancellationLogModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, CancellationLogModel> ReadD(string connectionString, List<CancellationLogModel> cancellationLogWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, CancellationLogModel> response = new Dictionary<string, CancellationLogModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subscription_id`, `cancellation_now`, `cancellation_reason_id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_logs ";
                if (cancellationLogWhere != null && cancellationLogWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < cancellationLogWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, cancellationLogWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, cancellationLogWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].CancellationNow))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `cancellation_now` = @cancellationNow" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@cancellationNow" + i, cancellationLogWhere[i].CancellationNow);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].CancellationReasonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `cancellation_reason_id` = @cancellationReasonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@cancellationReasonId" + i, cancellationLogWhere[i].CancellationReasonId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].Reason))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `reason` = @reason" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@reason" + i, cancellationLogWhere[i].Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, cancellationLogWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, cancellationLogWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, cancellationLogWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != cancellationLogWhere.Count - 1)
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
                            CancellationLogModel cancellationLog = new CancellationLogModel(); if (!reader.IsDBNull(0))
                                cancellationLog.Id = reader.GetValue(0).ToString();
                            else
                                cancellationLog.Id = null;
                            if (!reader.IsDBNull(1))
                                cancellationLog.SubscriptionId = reader.GetValue(1).ToString();
                            else
                                cancellationLog.SubscriptionId = null;
                            if (!reader.IsDBNull(2))
                                cancellationLog.CancellationNow = reader.GetValue(2).ToString();
                            else
                                cancellationLog.CancellationNow = null;
                            if (!reader.IsDBNull(3))
                                cancellationLog.CancellationReasonId = reader.GetValue(3).ToString();
                            else
                                cancellationLog.CancellationReasonId = null;
                            if (!reader.IsDBNull(4))
                                cancellationLog.Reason = reader.GetValue(4).ToString();
                            else
                                cancellationLog.Reason = null;
                            if (!reader.IsDBNull(5))
                                cancellationLog.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                cancellationLog.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                cancellationLog.CreatedAt = reader.GetValue(6).ToString();
                            else
                                cancellationLog.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                cancellationLog.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                cancellationLog.UpdatedAt = null;
                            response.Add(cancellationLog.Id, cancellationLog);
                            cancellationLog = new CancellationLogModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, CancellationLogModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, CancellationLogModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, CancellationLogModel> ReadD(string connectionString, List<CancellationLogModel> cancellationLogWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, CancellationLogModel> response = new Dictionary<string, CancellationLogModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subscription_id`, `cancellation_now`, `cancellation_reason_id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_logs ";
                if (cancellationLogWhere != null && cancellationLogWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < cancellationLogWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, cancellationLogWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, cancellationLogWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].CancellationNow))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `cancellation_now` = @cancellationNow" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@cancellationNow" + i, cancellationLogWhere[i].CancellationNow);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].CancellationReasonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `cancellation_reason_id` = @cancellationReasonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@cancellationReasonId" + i, cancellationLogWhere[i].CancellationReasonId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].Reason))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `reason` = @reason" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@reason" + i, cancellationLogWhere[i].Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, cancellationLogWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, cancellationLogWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, cancellationLogWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != cancellationLogWhere.Count - 1)
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
                            CancellationLogModel cancellationLog = new CancellationLogModel(); if (!reader.IsDBNull(0))
                                cancellationLog.Id = reader.GetValue(0).ToString();
                            else
                                cancellationLog.Id = null;
                            if (!reader.IsDBNull(1))
                                cancellationLog.SubscriptionId = reader.GetValue(1).ToString();
                            else
                                cancellationLog.SubscriptionId = null;
                            if (!reader.IsDBNull(2))
                                cancellationLog.CancellationNow = reader.GetValue(2).ToString();
                            else
                                cancellationLog.CancellationNow = null;
                            if (!reader.IsDBNull(3))
                                cancellationLog.CancellationReasonId = reader.GetValue(3).ToString();
                            else
                                cancellationLog.CancellationReasonId = null;
                            if (!reader.IsDBNull(4))
                                cancellationLog.Reason = reader.GetValue(4).ToString();
                            else
                                cancellationLog.Reason = null;
                            if (!reader.IsDBNull(5))
                                cancellationLog.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                cancellationLog.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                cancellationLog.CreatedAt = reader.GetValue(6).ToString();
                            else
                                cancellationLog.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                cancellationLog.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                cancellationLog.UpdatedAt = null;
                            response.Add(cancellationLog.Id, cancellationLog);
                            cancellationLog = new CancellationLogModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, CancellationLogModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, CancellationLogModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CancellationLogModel> Read(string connectionString, CancellationLogModel cancellationLog, bool firstOne, bool orderByLastDate, CancellationLogModel cancellationLogWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationLogModel> response = new List<CancellationLogModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subscription_id`, `cancellation_now`, `cancellation_reason_id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_logs ";
                {
                    if (cancellationLog != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationLog.Id))
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
                            cmd.Parameters.AddWithValue("@id", cancellationLog.Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionId", cancellationLog.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.CancellationNow))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `cancellation_now` = @cancellationNow ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `cancellation_now` = @cancellationNow ");
                            }
                            cmd.Parameters.AddWithValue("@cancellationNow", cancellationLog.CancellationNow);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.CancellationReasonId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `cancellation_reason_id` = @cancellationReasonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `cancellation_reason_id` = @cancellationReasonId ");
                            }
                            cmd.Parameters.AddWithValue("@cancellationReasonId", cancellationLog.CancellationReasonId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.Reason))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `reason` = @reason ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `reason` = @reason ");
                            }
                            cmd.Parameters.AddWithValue("@reason", cancellationLog.Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", cancellationLog.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", cancellationLog.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", cancellationLog.UpdatedAt);
                        }
                    }
                    if (cancellationLogWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + cancellationLogWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + cancellationLogWhereLike.SubscriptionId + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.CancellationNow))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `cancellation_now` LIKE @cancellationNowWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `cancellation_now` LIKE @cancellationNowWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@cancellationNowWhereLike", "%" + cancellationLogWhereLike.CancellationNow + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.CancellationReasonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `cancellation_reason_id` LIKE @cancellationReasonIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `cancellation_reason_id` LIKE @cancellationReasonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@cancellationReasonIdWhereLike", "%" + cancellationLogWhereLike.CancellationReasonId + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.Reason))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `reason` LIKE @reasonWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `reason` LIKE @reasonWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@reasonWhereLike", "%" + cancellationLogWhereLike.Reason + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + cancellationLogWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + cancellationLogWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + cancellationLogWhereLike.UpdatedAt + "%");
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
                                cancellationLog = new CancellationLogModel();
                                if (!reader.IsDBNull(0))
                                    cancellationLog.Id = reader.GetValue(0).ToString();
                                else
                                    cancellationLog.Id = null;
                                if (!reader.IsDBNull(1))
                                    cancellationLog.SubscriptionId = reader.GetValue(1).ToString();
                                else
                                    cancellationLog.SubscriptionId = null;
                                if (!reader.IsDBNull(2))
                                    cancellationLog.CancellationNow = reader.GetValue(2).ToString();
                                else
                                    cancellationLog.CancellationNow = null;
                                if (!reader.IsDBNull(3))
                                    cancellationLog.CancellationReasonId = reader.GetValue(3).ToString();
                                else
                                    cancellationLog.CancellationReasonId = null;
                                if (!reader.IsDBNull(4))
                                    cancellationLog.Reason = reader.GetValue(4).ToString();
                                else
                                    cancellationLog.Reason = null;
                                if (!reader.IsDBNull(5))
                                    cancellationLog.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    cancellationLog.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    cancellationLog.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    cancellationLog.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    cancellationLog.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    cancellationLog.UpdatedAt = null;
                                response.Add(cancellationLog);
                                cancellationLog = new CancellationLogModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<CancellationLogModel>();
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
                response = new List<CancellationLogModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CancellationLogModel> Read(string connectionString, CancellationLogModel cancellationLog, bool orderByLastDate, int fromRow, int limit, CancellationLogModel cancellationLogWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationLogModel> response = new List<CancellationLogModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subscription_id`, `cancellation_now`, `cancellation_reason_id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_logs ";
                {
                    if (cancellationLog != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationLog.Id))
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
                            cmd.Parameters.AddWithValue("@id", cancellationLog.Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionId", cancellationLog.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.CancellationNow))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `cancellation_now` = @cancellationNow ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `cancellation_now` = @cancellationNow ");
                            }
                            cmd.Parameters.AddWithValue("@cancellationNow", cancellationLog.CancellationNow);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.CancellationReasonId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `cancellation_reason_id` = @cancellationReasonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `cancellation_reason_id` = @cancellationReasonId ");
                            }
                            cmd.Parameters.AddWithValue("@cancellationReasonId", cancellationLog.CancellationReasonId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.Reason))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `reason` = @reason ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `reason` = @reason ");
                            }
                            cmd.Parameters.AddWithValue("@reason", cancellationLog.Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", cancellationLog.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", cancellationLog.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationLog.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", cancellationLog.UpdatedAt);
                        }
                    }
                    if (cancellationLogWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + cancellationLogWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + cancellationLogWhereLike.SubscriptionId + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.CancellationNow))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `cancellation_now` LIKE @cancellationNowWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `cancellation_now` LIKE @cancellationNowWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@cancellationNowWhereLike", "%" + cancellationLogWhereLike.CancellationNow + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.CancellationReasonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `cancellation_reason_id` LIKE @cancellationReasonIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `cancellation_reason_id` LIKE @cancellationReasonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@cancellationReasonIdWhereLike", "%" + cancellationLogWhereLike.CancellationReasonId + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.Reason))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `reason` LIKE @reasonWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `reason` LIKE @reasonWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@reasonWhereLike", "%" + cancellationLogWhereLike.Reason + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + cancellationLogWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + cancellationLogWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + cancellationLogWhereLike.UpdatedAt + "%");
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
                                cancellationLog = new CancellationLogModel();
                                if (!reader.IsDBNull(0))
                                    cancellationLog.Id = reader.GetValue(0).ToString();
                                else
                                    cancellationLog.Id = null;
                                if (!reader.IsDBNull(1))
                                    cancellationLog.SubscriptionId = reader.GetValue(1).ToString();
                                else
                                    cancellationLog.SubscriptionId = null;
                                if (!reader.IsDBNull(2))
                                    cancellationLog.CancellationNow = reader.GetValue(2).ToString();
                                else
                                    cancellationLog.CancellationNow = null;
                                if (!reader.IsDBNull(3))
                                    cancellationLog.CancellationReasonId = reader.GetValue(3).ToString();
                                else
                                    cancellationLog.CancellationReasonId = null;
                                if (!reader.IsDBNull(4))
                                    cancellationLog.Reason = reader.GetValue(4).ToString();
                                else
                                    cancellationLog.Reason = null;
                                if (!reader.IsDBNull(5))
                                    cancellationLog.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    cancellationLog.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    cancellationLog.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    cancellationLog.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    cancellationLog.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    cancellationLog.UpdatedAt = null;
                                response.Add(cancellationLog);
                                cancellationLog = new CancellationLogModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<CancellationLogModel>();
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
                response = new List<CancellationLogModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CancellationLogModel> Read(string connectionString, List<CancellationLogModel> cancellationLogWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationLogModel> response = new List<CancellationLogModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subscription_id`, `cancellation_now`, `cancellation_reason_id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_logs ";
                if (cancellationLogWhere != null && cancellationLogWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < cancellationLogWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, cancellationLogWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, cancellationLogWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].CancellationNow))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `cancellation_now` = @cancellationNow" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@cancellationNow" + i, cancellationLogWhere[i].CancellationNow);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].CancellationReasonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `cancellation_reason_id` = @cancellationReasonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@cancellationReasonId" + i, cancellationLogWhere[i].CancellationReasonId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].Reason))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `reason` = @reason" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@reason" + i, cancellationLogWhere[i].Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, cancellationLogWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, cancellationLogWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, cancellationLogWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != cancellationLogWhere.Count - 1)
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
                            CancellationLogModel cancellationLog = new CancellationLogModel(); if (!reader.IsDBNull(0))
                                cancellationLog.Id = reader.GetValue(0).ToString();
                            else
                                cancellationLog.Id = null;
                            if (!reader.IsDBNull(1))
                                cancellationLog.SubscriptionId = reader.GetValue(1).ToString();
                            else
                                cancellationLog.SubscriptionId = null;
                            if (!reader.IsDBNull(2))
                                cancellationLog.CancellationNow = reader.GetValue(2).ToString();
                            else
                                cancellationLog.CancellationNow = null;
                            if (!reader.IsDBNull(3))
                                cancellationLog.CancellationReasonId = reader.GetValue(3).ToString();
                            else
                                cancellationLog.CancellationReasonId = null;
                            if (!reader.IsDBNull(4))
                                cancellationLog.Reason = reader.GetValue(4).ToString();
                            else
                                cancellationLog.Reason = null;
                            if (!reader.IsDBNull(5))
                                cancellationLog.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                cancellationLog.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                cancellationLog.CreatedAt = reader.GetValue(6).ToString();
                            else
                                cancellationLog.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                cancellationLog.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                cancellationLog.UpdatedAt = null;
                            response.Add(cancellationLog);
                            cancellationLog = new CancellationLogModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<CancellationLogModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationLogModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CancellationLogModel> Read(string connectionString, List<CancellationLogModel> cancellationLogWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationLogModel> response = new List<CancellationLogModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subscription_id`, `cancellation_now`, `cancellation_reason_id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_logs ";
                if (cancellationLogWhere != null && cancellationLogWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < cancellationLogWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, cancellationLogWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, cancellationLogWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].CancellationNow))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `cancellation_now` = @cancellationNow" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@cancellationNow" + i, cancellationLogWhere[i].CancellationNow);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].CancellationReasonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `cancellation_reason_id` = @cancellationReasonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@cancellationReasonId" + i, cancellationLogWhere[i].CancellationReasonId);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].Reason))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `reason` = @reason" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@reason" + i, cancellationLogWhere[i].Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, cancellationLogWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, cancellationLogWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationLogWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, cancellationLogWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != cancellationLogWhere.Count - 1)
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
                            CancellationLogModel cancellationLog = new CancellationLogModel(); if (!reader.IsDBNull(0))
                                cancellationLog.Id = reader.GetValue(0).ToString();
                            else
                                cancellationLog.Id = null;
                            if (!reader.IsDBNull(1))
                                cancellationLog.SubscriptionId = reader.GetValue(1).ToString();
                            else
                                cancellationLog.SubscriptionId = null;
                            if (!reader.IsDBNull(2))
                                cancellationLog.CancellationNow = reader.GetValue(2).ToString();
                            else
                                cancellationLog.CancellationNow = null;
                            if (!reader.IsDBNull(3))
                                cancellationLog.CancellationReasonId = reader.GetValue(3).ToString();
                            else
                                cancellationLog.CancellationReasonId = null;
                            if (!reader.IsDBNull(4))
                                cancellationLog.Reason = reader.GetValue(4).ToString();
                            else
                                cancellationLog.Reason = null;
                            if (!reader.IsDBNull(5))
                                cancellationLog.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                cancellationLog.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                cancellationLog.CreatedAt = reader.GetValue(6).ToString();
                            else
                                cancellationLog.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                cancellationLog.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                cancellationLog.UpdatedAt = null;
                            response.Add(cancellationLog);
                            cancellationLog = new CancellationLogModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<CancellationLogModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationLogModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CancellationLogModel> Create(string connectionString, CancellationLogModel cancellationLog)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationLogModel> response = new List<CancellationLogModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO subst_cancellation_logs (";
                string values = "(";
                if (!string.IsNullOrEmpty(cancellationLog.Id))
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
                    cmd.Parameters.AddWithValue("@id", cancellationLog.Id);
                }
                if (!string.IsNullOrEmpty(cancellationLog.SubscriptionId))
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
                    cmd.Parameters.AddWithValue("@subscriptionId", cancellationLog.SubscriptionId);
                }
                if (!string.IsNullOrEmpty(cancellationLog.CancellationNow))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `cancellation_now` ");
                        values = string.Concat(values, ", @cancellationNow");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `cancellation_now` ");
                        values = string.Concat(values, " @cancellationNow");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@cancellationNow", cancellationLog.CancellationNow);
                }
                if (!string.IsNullOrEmpty(cancellationLog.CancellationReasonId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `cancellation_reason_id` ");
                        values = string.Concat(values, ", @cancellationReasonId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `cancellation_reason_id` ");
                        values = string.Concat(values, " @cancellationReasonId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@cancellationReasonId", cancellationLog.CancellationReasonId);
                }
                if (!string.IsNullOrEmpty(cancellationLog.Reason))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `reason` ");
                        values = string.Concat(values, ", @reason");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `reason` ");
                        values = string.Concat(values, " @reason");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@reason", cancellationLog.Reason);
                }
                if (!string.IsNullOrEmpty(cancellationLog.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", cancellationLog.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(cancellationLog.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", cancellationLog.CreatedAt);
                }
                if (!string.IsNullOrEmpty(cancellationLog.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", cancellationLog.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                cancellationLog.Id = cmd.LastInsertedId.ToString();
                response.Add(cancellationLog);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationLogModel>();
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
        public List<CancellationLogModel> Create(string connectionString, List<CancellationLogModel> cancellationLogList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationLogModel> response = new List<CancellationLogModel>();
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
                        for (int i = 0; i < cancellationLogList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_cancellation_logs (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(cancellationLogList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, cancellationLogList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(cancellationLogList[i].SubscriptionId))
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
                                    cmd.Parameters.AddWithValue("@subscriptionId" + i, cancellationLogList[i].SubscriptionId);
                                }
                                if (!string.IsNullOrEmpty(cancellationLogList[i].CancellationNow))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `cancellation_now` ");
                                        values = string.Concat(values, ", @cancellationNow" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `cancellation_now` ");
                                        values = string.Concat(values, "@cancellationNow" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@cancellationNow" + i, cancellationLogList[i].CancellationNow);
                                }
                                if (!string.IsNullOrEmpty(cancellationLogList[i].CancellationReasonId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `cancellation_reason_id` ");
                                        values = string.Concat(values, ", @cancellationReasonId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `cancellation_reason_id` ");
                                        values = string.Concat(values, "@cancellationReasonId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@cancellationReasonId" + i, cancellationLogList[i].CancellationReasonId);
                                }
                                if (!string.IsNullOrEmpty(cancellationLogList[i].Reason))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `reason` ");
                                        values = string.Concat(values, ", @reason" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `reason` ");
                                        values = string.Concat(values, "@reason" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@reason" + i, cancellationLogList[i].Reason);
                                }
                                if (!string.IsNullOrEmpty(cancellationLogList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, cancellationLogList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(cancellationLogList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, cancellationLogList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(cancellationLogList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, cancellationLogList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(cancellationLogList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<CancellationLogModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CancellationLogModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationLogModel>();
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

        public List<CancellationLogModel> Update(string connectionString, CancellationLogModel cancellationLog, CancellationLogModel cancellationLogWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationLogModel> response = new List<CancellationLogModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE subst_cancellation_logs SET ";
                if (!string.IsNullOrEmpty(cancellationLog.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", cancellationLog.Id);
                }
                if (!string.IsNullOrEmpty(cancellationLog.SubscriptionId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `subscription_id` = @subscriptionId");
                    else
                        sql = string.Concat(sql, " `subscription_id` = @subscriptionId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subscriptionId", cancellationLog.SubscriptionId);
                }
                if (!string.IsNullOrEmpty(cancellationLog.CancellationNow))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `cancellation_now` = @cancellationNow");
                    else
                        sql = string.Concat(sql, " `cancellation_now` = @cancellationNow");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@cancellationNow", cancellationLog.CancellationNow);
                }
                if (!string.IsNullOrEmpty(cancellationLog.CancellationReasonId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `cancellation_reason_id` = @cancellationReasonId");
                    else
                        sql = string.Concat(sql, " `cancellation_reason_id` = @cancellationReasonId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@cancellationReasonId", cancellationLog.CancellationReasonId);
                }
                if (!string.IsNullOrEmpty(cancellationLog.Reason))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `reason` = @reason");
                    else
                        sql = string.Concat(sql, " `reason` = @reason");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@reason", cancellationLog.Reason);
                }
                if (!string.IsNullOrEmpty(cancellationLog.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", cancellationLog.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(cancellationLog.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", cancellationLog.CreatedAt);
                }
                if (!string.IsNullOrEmpty(cancellationLog.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", cancellationLog.UpdatedAt);
                }
                if (cancellationLogWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(cancellationLogWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", cancellationLogWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(cancellationLogWhere.SubscriptionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionIdWhere");
                        else
                            sql = string.Concat(sql, " `subscription_id` = @subscriptionIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionIdWhere", cancellationLogWhere.SubscriptionId);
                    }
                    if (!string.IsNullOrEmpty(cancellationLogWhere.CancellationNow))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `cancellation_now` = @cancellationNowWhere");
                        else
                            sql = string.Concat(sql, " `cancellation_now` = @cancellationNowWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@cancellationNowWhere", cancellationLogWhere.CancellationNow);
                    }
                    if (!string.IsNullOrEmpty(cancellationLogWhere.CancellationReasonId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `cancellation_reason_id` = @cancellationReasonIdWhere");
                        else
                            sql = string.Concat(sql, " `cancellation_reason_id` = @cancellationReasonIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@cancellationReasonIdWhere", cancellationLogWhere.CancellationReasonId);
                    }
                    if (!string.IsNullOrEmpty(cancellationLogWhere.Reason))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reason` = @reasonWhere");
                        else
                            sql = string.Concat(sql, " `reason` = @reasonWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@reasonWhere", cancellationLogWhere.Reason);
                    }
                    if (!string.IsNullOrEmpty(cancellationLogWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", cancellationLogWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(cancellationLogWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", cancellationLogWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(cancellationLogWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", cancellationLogWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(cancellationLog);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationLogModel>();
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

        public List<CancellationLogModel> Update(string connectionString, List<CancellationLogModel> cancellationLogList, List<CancellationLogModel> cancellationLogWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationLogModel> response = new List<CancellationLogModel>();
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
                        for (int i = 0; i < cancellationLogList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_cancellation_logs SET ";
                                if (!string.IsNullOrEmpty(cancellationLogList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, cancellationLogList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(cancellationLogList[i].SubscriptionId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `subscription_id` = @subscriptionId" + i);
                                    else
                                        sql = string.Concat(sql, " `subscription_id` = @subscriptionId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subscriptionId" + i, cancellationLogList[i].SubscriptionId);
                                }
                                if (!string.IsNullOrEmpty(cancellationLogList[i].CancellationNow))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `cancellation_now` = @cancellationNow" + i);
                                    else
                                        sql = string.Concat(sql, " `cancellation_now` = @cancellationNow" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@cancellationNow" + i, cancellationLogList[i].CancellationNow);
                                }
                                if (!string.IsNullOrEmpty(cancellationLogList[i].CancellationReasonId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `cancellation_reason_id` = @cancellationReasonId" + i);
                                    else
                                        sql = string.Concat(sql, " `cancellation_reason_id` = @cancellationReasonId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@cancellationReasonId" + i, cancellationLogList[i].CancellationReasonId);
                                }
                                if (!string.IsNullOrEmpty(cancellationLogList[i].Reason))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `reason` = @reason" + i);
                                    else
                                        sql = string.Concat(sql, " `reason` = @reason" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@reason" + i, cancellationLogList[i].Reason);
                                }
                                if (!string.IsNullOrEmpty(cancellationLogList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, cancellationLogList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(cancellationLogList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, cancellationLogList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(cancellationLogList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, cancellationLogList[i].UpdatedAt);
                                }
                                if (cancellationLogWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(cancellationLogWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, cancellationLogWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationLogWhereList[i].SubscriptionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `subscription_id` = @subscriptionIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionIdWhere" + i, cancellationLogWhereList[i].SubscriptionId);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationLogWhereList[i].CancellationNow))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `cancellation_now` = @cancellationNowWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `cancellation_now` = @cancellationNowWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@cancellationNowWhere" + i, cancellationLogWhereList[i].CancellationNow);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationLogWhereList[i].CancellationReasonId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `cancellation_reason_id` = @cancellationReasonIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `cancellation_reason_id` = @cancellationReasonIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@cancellationReasonIdWhere" + i, cancellationLogWhereList[i].CancellationReasonId);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationLogWhereList[i].Reason))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reason` = @reasonWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `reason` = @reasonWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@reasonWhere" + i, cancellationLogWhereList[i].Reason);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationLogWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, cancellationLogWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationLogWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, cancellationLogWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationLogWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, cancellationLogWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(cancellationLogList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CancellationLogModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CancellationLogModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationLogModel>();
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

        public List<CancellationLogModel> Delete(string connectionString, CancellationLogModel cancellationLog)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationLogModel> response = new List<CancellationLogModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM subst_cancellation_logs  ";
                if (cancellationLog != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(cancellationLog.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", cancellationLog.Id);
                    }
                    if (!string.IsNullOrEmpty(cancellationLog.SubscriptionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionId");
                        else
                            sql = string.Concat(sql, " `subscription_id` = @subscriptionId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionId", cancellationLog.SubscriptionId);
                    }
                    if (!string.IsNullOrEmpty(cancellationLog.CancellationNow))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `cancellation_now` = @cancellationNow");
                        else
                            sql = string.Concat(sql, " `cancellation_now` = @cancellationNow");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@cancellationNow", cancellationLog.CancellationNow);
                    }
                    if (!string.IsNullOrEmpty(cancellationLog.CancellationReasonId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `cancellation_reason_id` = @cancellationReasonId");
                        else
                            sql = string.Concat(sql, " `cancellation_reason_id` = @cancellationReasonId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@cancellationReasonId", cancellationLog.CancellationReasonId);
                    }
                    if (!string.IsNullOrEmpty(cancellationLog.Reason))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reason` = @reason");
                        else
                            sql = string.Concat(sql, " `reason` = @reason");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@reason", cancellationLog.Reason);
                    }
                    if (!string.IsNullOrEmpty(cancellationLog.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", cancellationLog.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(cancellationLog.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", cancellationLog.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(cancellationLog.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", cancellationLog.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(cancellationLog);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationLogModel>();
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

        public List<CancellationLogModel> Delete(string connectionString, List<CancellationLogModel> cancellationLog)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationLogModel> response = new List<CancellationLogModel>();
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
                        for (int i = 0; i < cancellationLog.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_cancellation_logs  ";
                                if (cancellationLog[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(cancellationLog[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, cancellationLog[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationLog[i].SubscriptionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionId" + i);
                                        else
                                            sql = string.Concat(sql, " `subscription_id` = @subscriptionId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionId" + i, cancellationLog[i].SubscriptionId);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationLog[i].CancellationNow))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `cancellation_now` = @cancellationNow" + i);
                                        else
                                            sql = string.Concat(sql, " `cancellation_now` = @cancellationNow" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@cancellationNow" + i, cancellationLog[i].CancellationNow);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationLog[i].CancellationReasonId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `cancellation_reason_id` = @cancellationReasonId" + i);
                                        else
                                            sql = string.Concat(sql, " `cancellation_reason_id` = @cancellationReasonId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@cancellationReasonId" + i, cancellationLog[i].CancellationReasonId);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationLog[i].Reason))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reason` = @reason" + i);
                                        else
                                            sql = string.Concat(sql, " `reason` = @reason" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@reason" + i, cancellationLog[i].Reason);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationLog[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, cancellationLog[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationLog[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, cancellationLog[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationLog[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, cancellationLog[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(cancellationLog[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CancellationLogModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CancellationLogModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationLogModel>();
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
