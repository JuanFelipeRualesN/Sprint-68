using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class CancellationReasonDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, CancellationReasonModel> ReadD(string connectionString, CancellationReasonModel cancellationReason, bool firstOne, bool orderByLastDate, CancellationReasonModel cancellationReasonWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, CancellationReasonModel> response = new Dictionary<string, CancellationReasonModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_reasons ";
                {
                    if (cancellationReason != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationReason.Id))
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
                            cmd.Parameters.AddWithValue("@id", cancellationReason.Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.Reason))
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
                            cmd.Parameters.AddWithValue("@reason", cancellationReason.Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", cancellationReason.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", cancellationReason.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", cancellationReason.UpdatedAt);
                        }
                    }
                    if (cancellationReasonWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + cancellationReasonWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.Reason))
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
                            cmd.Parameters.AddWithValue("@reasonWhereLike", "%" + cancellationReasonWhereLike.Reason + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + cancellationReasonWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + cancellationReasonWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + cancellationReasonWhereLike.UpdatedAt + "%");
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
                                cancellationReason = new CancellationReasonModel();
                                if (!reader.IsDBNull(0))
                                    cancellationReason.Id = reader.GetValue(0).ToString();
                                else
                                    cancellationReason.Id = null;
                                if (!reader.IsDBNull(1))
                                    cancellationReason.Reason = reader.GetValue(1).ToString();
                                else
                                    cancellationReason.Reason = null;
                                if (!reader.IsDBNull(2))
                                    cancellationReason.ModifiedBy = reader.GetValue(2).ToString();
                                else
                                    cancellationReason.ModifiedBy = null;
                                if (!reader.IsDBNull(3))
                                    cancellationReason.CreatedAt = reader.GetValue(3).ToString();
                                else
                                    cancellationReason.CreatedAt = null;
                                if (!reader.IsDBNull(4))
                                    cancellationReason.UpdatedAt = reader.GetValue(4).ToString();
                                else
                                    cancellationReason.UpdatedAt = null;
                                response.Add(cancellationReason.Id, cancellationReason);
                                cancellationReason = new CancellationReasonModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, CancellationReasonModel>();
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
                response = new Dictionary<string, CancellationReasonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, CancellationReasonModel> ReadD(string connectionString, CancellationReasonModel cancellationReason, bool orderByLastDate, int fromRow, int limit, CancellationReasonModel cancellationReasonWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, CancellationReasonModel> response = new Dictionary<string, CancellationReasonModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_reasons ";
                {
                    if (cancellationReason != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationReason.Id))
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
                            cmd.Parameters.AddWithValue("@id", cancellationReason.Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.Reason))
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
                            cmd.Parameters.AddWithValue("@reason", cancellationReason.Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", cancellationReason.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", cancellationReason.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", cancellationReason.UpdatedAt);
                        }
                    }
                    if (cancellationReasonWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + cancellationReasonWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.Reason))
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
                            cmd.Parameters.AddWithValue("@reasonWhereLike", "%" + cancellationReasonWhereLike.Reason + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + cancellationReasonWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + cancellationReasonWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + cancellationReasonWhereLike.UpdatedAt + "%");
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
                                cancellationReason = new CancellationReasonModel();
                                if (!reader.IsDBNull(0))
                                    cancellationReason.Id = reader.GetValue(0).ToString();
                                else
                                    cancellationReason.Id = null;
                                if (!reader.IsDBNull(1))
                                    cancellationReason.Reason = reader.GetValue(1).ToString();
                                else
                                    cancellationReason.Reason = null;
                                if (!reader.IsDBNull(2))
                                    cancellationReason.ModifiedBy = reader.GetValue(2).ToString();
                                else
                                    cancellationReason.ModifiedBy = null;
                                if (!reader.IsDBNull(3))
                                    cancellationReason.CreatedAt = reader.GetValue(3).ToString();
                                else
                                    cancellationReason.CreatedAt = null;
                                if (!reader.IsDBNull(4))
                                    cancellationReason.UpdatedAt = reader.GetValue(4).ToString();
                                else
                                    cancellationReason.UpdatedAt = null;
                                response.Add(cancellationReason.Id, cancellationReason);
                                cancellationReason = new CancellationReasonModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, CancellationReasonModel>();
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
                response = new Dictionary<string, CancellationReasonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, CancellationReasonModel> ReadD(string connectionString, List<CancellationReasonModel> cancellationReasonWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, CancellationReasonModel> response = new Dictionary<string, CancellationReasonModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_reasons ";
                if (cancellationReasonWhere != null && cancellationReasonWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < cancellationReasonWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, cancellationReasonWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].Reason))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `reason` = @reason" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@reason" + i, cancellationReasonWhere[i].Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, cancellationReasonWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, cancellationReasonWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, cancellationReasonWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != cancellationReasonWhere.Count - 1)
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
                            CancellationReasonModel cancellationReason = new CancellationReasonModel(); if (!reader.IsDBNull(0))
                                cancellationReason.Id = reader.GetValue(0).ToString();
                            else
                                cancellationReason.Id = null;
                            if (!reader.IsDBNull(1))
                                cancellationReason.Reason = reader.GetValue(1).ToString();
                            else
                                cancellationReason.Reason = null;
                            if (!reader.IsDBNull(2))
                                cancellationReason.ModifiedBy = reader.GetValue(2).ToString();
                            else
                                cancellationReason.ModifiedBy = null;
                            if (!reader.IsDBNull(3))
                                cancellationReason.CreatedAt = reader.GetValue(3).ToString();
                            else
                                cancellationReason.CreatedAt = null;
                            if (!reader.IsDBNull(4))
                                cancellationReason.UpdatedAt = reader.GetValue(4).ToString();
                            else
                                cancellationReason.UpdatedAt = null;
                            response.Add(cancellationReason.Id, cancellationReason);
                            cancellationReason = new CancellationReasonModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, CancellationReasonModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, CancellationReasonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, CancellationReasonModel> ReadD(string connectionString, List<CancellationReasonModel> cancellationReasonWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, CancellationReasonModel> response = new Dictionary<string, CancellationReasonModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_reasons ";
                if (cancellationReasonWhere != null && cancellationReasonWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < cancellationReasonWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, cancellationReasonWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].Reason))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `reason` = @reason" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@reason" + i, cancellationReasonWhere[i].Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, cancellationReasonWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, cancellationReasonWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, cancellationReasonWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != cancellationReasonWhere.Count - 1)
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
                            CancellationReasonModel cancellationReason = new CancellationReasonModel(); if (!reader.IsDBNull(0))
                                cancellationReason.Id = reader.GetValue(0).ToString();
                            else
                                cancellationReason.Id = null;
                            if (!reader.IsDBNull(1))
                                cancellationReason.Reason = reader.GetValue(1).ToString();
                            else
                                cancellationReason.Reason = null;
                            if (!reader.IsDBNull(2))
                                cancellationReason.ModifiedBy = reader.GetValue(2).ToString();
                            else
                                cancellationReason.ModifiedBy = null;
                            if (!reader.IsDBNull(3))
                                cancellationReason.CreatedAt = reader.GetValue(3).ToString();
                            else
                                cancellationReason.CreatedAt = null;
                            if (!reader.IsDBNull(4))
                                cancellationReason.UpdatedAt = reader.GetValue(4).ToString();
                            else
                                cancellationReason.UpdatedAt = null;
                            response.Add(cancellationReason.Id, cancellationReason);
                            cancellationReason = new CancellationReasonModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, CancellationReasonModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, CancellationReasonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CancellationReasonModel> Read(string connectionString, CancellationReasonModel cancellationReason, bool firstOne, bool orderByLastDate, CancellationReasonModel cancellationReasonWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationReasonModel> response = new List<CancellationReasonModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_reasons ";
                {
                    if (cancellationReason != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationReason.Id))
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
                            cmd.Parameters.AddWithValue("@id", cancellationReason.Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.Reason))
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
                            cmd.Parameters.AddWithValue("@reason", cancellationReason.Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", cancellationReason.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", cancellationReason.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", cancellationReason.UpdatedAt);
                        }
                    }
                    if (cancellationReasonWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + cancellationReasonWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.Reason))
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
                            cmd.Parameters.AddWithValue("@reasonWhereLike", "%" + cancellationReasonWhereLike.Reason + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + cancellationReasonWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + cancellationReasonWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + cancellationReasonWhereLike.UpdatedAt + "%");
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
                                cancellationReason = new CancellationReasonModel();
                                if (!reader.IsDBNull(0))
                                    cancellationReason.Id = reader.GetValue(0).ToString();
                                else
                                    cancellationReason.Id = null;
                                if (!reader.IsDBNull(1))
                                    cancellationReason.Reason = reader.GetValue(1).ToString();
                                else
                                    cancellationReason.Reason = null;
                                if (!reader.IsDBNull(2))
                                    cancellationReason.ModifiedBy = reader.GetValue(2).ToString();
                                else
                                    cancellationReason.ModifiedBy = null;
                                if (!reader.IsDBNull(3))
                                    cancellationReason.CreatedAt = reader.GetValue(3).ToString();
                                else
                                    cancellationReason.CreatedAt = null;
                                if (!reader.IsDBNull(4))
                                    cancellationReason.UpdatedAt = reader.GetValue(4).ToString();
                                else
                                    cancellationReason.UpdatedAt = null;
                                response.Add(cancellationReason);
                                cancellationReason = new CancellationReasonModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<CancellationReasonModel>();
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
                response = new List<CancellationReasonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CancellationReasonModel> Read(string connectionString, CancellationReasonModel cancellationReason, bool orderByLastDate, int fromRow, int limit, CancellationReasonModel cancellationReasonWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationReasonModel> response = new List<CancellationReasonModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_reasons ";
                {
                    if (cancellationReason != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationReason.Id))
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
                            cmd.Parameters.AddWithValue("@id", cancellationReason.Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.Reason))
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
                            cmd.Parameters.AddWithValue("@reason", cancellationReason.Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", cancellationReason.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", cancellationReason.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationReason.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", cancellationReason.UpdatedAt);
                        }
                    }
                    if (cancellationReasonWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + cancellationReasonWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.Reason))
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
                            cmd.Parameters.AddWithValue("@reasonWhereLike", "%" + cancellationReasonWhereLike.Reason + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + cancellationReasonWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + cancellationReasonWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + cancellationReasonWhereLike.UpdatedAt + "%");
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
                                cancellationReason = new CancellationReasonModel();
                                if (!reader.IsDBNull(0))
                                    cancellationReason.Id = reader.GetValue(0).ToString();
                                else
                                    cancellationReason.Id = null;
                                if (!reader.IsDBNull(1))
                                    cancellationReason.Reason = reader.GetValue(1).ToString();
                                else
                                    cancellationReason.Reason = null;
                                if (!reader.IsDBNull(2))
                                    cancellationReason.ModifiedBy = reader.GetValue(2).ToString();
                                else
                                    cancellationReason.ModifiedBy = null;
                                if (!reader.IsDBNull(3))
                                    cancellationReason.CreatedAt = reader.GetValue(3).ToString();
                                else
                                    cancellationReason.CreatedAt = null;
                                if (!reader.IsDBNull(4))
                                    cancellationReason.UpdatedAt = reader.GetValue(4).ToString();
                                else
                                    cancellationReason.UpdatedAt = null;
                                response.Add(cancellationReason);
                                cancellationReason = new CancellationReasonModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<CancellationReasonModel>();
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
                response = new List<CancellationReasonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CancellationReasonModel> Read(string connectionString, List<CancellationReasonModel> cancellationReasonWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationReasonModel> response = new List<CancellationReasonModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_reasons ";
                if (cancellationReasonWhere != null && cancellationReasonWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < cancellationReasonWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, cancellationReasonWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].Reason))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `reason` = @reason" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@reason" + i, cancellationReasonWhere[i].Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, cancellationReasonWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, cancellationReasonWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, cancellationReasonWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != cancellationReasonWhere.Count - 1)
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
                            CancellationReasonModel cancellationReason = new CancellationReasonModel(); if (!reader.IsDBNull(0))
                                cancellationReason.Id = reader.GetValue(0).ToString();
                            else
                                cancellationReason.Id = null;
                            if (!reader.IsDBNull(1))
                                cancellationReason.Reason = reader.GetValue(1).ToString();
                            else
                                cancellationReason.Reason = null;
                            if (!reader.IsDBNull(2))
                                cancellationReason.ModifiedBy = reader.GetValue(2).ToString();
                            else
                                cancellationReason.ModifiedBy = null;
                            if (!reader.IsDBNull(3))
                                cancellationReason.CreatedAt = reader.GetValue(3).ToString();
                            else
                                cancellationReason.CreatedAt = null;
                            if (!reader.IsDBNull(4))
                                cancellationReason.UpdatedAt = reader.GetValue(4).ToString();
                            else
                                cancellationReason.UpdatedAt = null;
                            response.Add(cancellationReason);
                            cancellationReason = new CancellationReasonModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<CancellationReasonModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationReasonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CancellationReasonModel> Read(string connectionString, List<CancellationReasonModel> cancellationReasonWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationReasonModel> response = new List<CancellationReasonModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `reason`, `modified_by`, `created_at`, `updated_at`  FROM subst_cancellation_reasons ";
                if (cancellationReasonWhere != null && cancellationReasonWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < cancellationReasonWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, cancellationReasonWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].Reason))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `reason` = @reason" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@reason" + i, cancellationReasonWhere[i].Reason);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, cancellationReasonWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, cancellationReasonWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(cancellationReasonWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, cancellationReasonWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != cancellationReasonWhere.Count - 1)
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
                            CancellationReasonModel cancellationReason = new CancellationReasonModel(); if (!reader.IsDBNull(0))
                                cancellationReason.Id = reader.GetValue(0).ToString();
                            else
                                cancellationReason.Id = null;
                            if (!reader.IsDBNull(1))
                                cancellationReason.Reason = reader.GetValue(1).ToString();
                            else
                                cancellationReason.Reason = null;
                            if (!reader.IsDBNull(2))
                                cancellationReason.ModifiedBy = reader.GetValue(2).ToString();
                            else
                                cancellationReason.ModifiedBy = null;
                            if (!reader.IsDBNull(3))
                                cancellationReason.CreatedAt = reader.GetValue(3).ToString();
                            else
                                cancellationReason.CreatedAt = null;
                            if (!reader.IsDBNull(4))
                                cancellationReason.UpdatedAt = reader.GetValue(4).ToString();
                            else
                                cancellationReason.UpdatedAt = null;
                            response.Add(cancellationReason);
                            cancellationReason = new CancellationReasonModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<CancellationReasonModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationReasonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CancellationReasonModel> Create(string connectionString, CancellationReasonModel cancellationReason)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationReasonModel> response = new List<CancellationReasonModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO subst_cancellation_reasons (";
                string values = "(";
                if (!string.IsNullOrEmpty(cancellationReason.Id))
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
                    cmd.Parameters.AddWithValue("@id", cancellationReason.Id);
                }
                if (!string.IsNullOrEmpty(cancellationReason.Reason))
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
                    cmd.Parameters.AddWithValue("@reason", cancellationReason.Reason);
                }
                if (!string.IsNullOrEmpty(cancellationReason.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", cancellationReason.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(cancellationReason.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", cancellationReason.CreatedAt);
                }
                if (!string.IsNullOrEmpty(cancellationReason.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", cancellationReason.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                cancellationReason.Id = cmd.LastInsertedId.ToString();
                response.Add(cancellationReason);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationReasonModel>();
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
        public List<CancellationReasonModel> Create(string connectionString, List<CancellationReasonModel> cancellationReasonList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationReasonModel> response = new List<CancellationReasonModel>();
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
                        for (int i = 0; i < cancellationReasonList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_cancellation_reasons (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(cancellationReasonList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, cancellationReasonList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(cancellationReasonList[i].Reason))
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
                                    cmd.Parameters.AddWithValue("@reason" + i, cancellationReasonList[i].Reason);
                                }
                                if (!string.IsNullOrEmpty(cancellationReasonList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, cancellationReasonList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(cancellationReasonList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, cancellationReasonList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(cancellationReasonList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, cancellationReasonList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(cancellationReasonList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<CancellationReasonModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CancellationReasonModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationReasonModel>();
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

        public List<CancellationReasonModel> Update(string connectionString, CancellationReasonModel cancellationReason, CancellationReasonModel cancellationReasonWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationReasonModel> response = new List<CancellationReasonModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE subst_cancellation_reasons SET ";
                if (!string.IsNullOrEmpty(cancellationReason.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", cancellationReason.Id);
                }
                if (!string.IsNullOrEmpty(cancellationReason.Reason))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `reason` = @reason");
                    else
                        sql = string.Concat(sql, " `reason` = @reason");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@reason", cancellationReason.Reason);
                }
                if (!string.IsNullOrEmpty(cancellationReason.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", cancellationReason.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(cancellationReason.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", cancellationReason.CreatedAt);
                }
                if (!string.IsNullOrEmpty(cancellationReason.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", cancellationReason.UpdatedAt);
                }
                if (cancellationReasonWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(cancellationReasonWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", cancellationReasonWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(cancellationReasonWhere.Reason))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reason` = @reasonWhere");
                        else
                            sql = string.Concat(sql, " `reason` = @reasonWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@reasonWhere", cancellationReasonWhere.Reason);
                    }
                    if (!string.IsNullOrEmpty(cancellationReasonWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", cancellationReasonWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(cancellationReasonWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", cancellationReasonWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(cancellationReasonWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", cancellationReasonWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(cancellationReason);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationReasonModel>();
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

        public List<CancellationReasonModel> Update(string connectionString, List<CancellationReasonModel> cancellationReasonList, List<CancellationReasonModel> cancellationReasonWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationReasonModel> response = new List<CancellationReasonModel>();
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
                        for (int i = 0; i < cancellationReasonList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_cancellation_reasons SET ";
                                if (!string.IsNullOrEmpty(cancellationReasonList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, cancellationReasonList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(cancellationReasonList[i].Reason))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `reason` = @reason" + i);
                                    else
                                        sql = string.Concat(sql, " `reason` = @reason" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@reason" + i, cancellationReasonList[i].Reason);
                                }
                                if (!string.IsNullOrEmpty(cancellationReasonList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, cancellationReasonList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(cancellationReasonList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, cancellationReasonList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(cancellationReasonList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, cancellationReasonList[i].UpdatedAt);
                                }
                                if (cancellationReasonWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(cancellationReasonWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, cancellationReasonWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationReasonWhereList[i].Reason))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reason` = @reasonWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `reason` = @reasonWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@reasonWhere" + i, cancellationReasonWhereList[i].Reason);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationReasonWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, cancellationReasonWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationReasonWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, cancellationReasonWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationReasonWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, cancellationReasonWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(cancellationReasonList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CancellationReasonModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CancellationReasonModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationReasonModel>();
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

        public List<CancellationReasonModel> Delete(string connectionString, CancellationReasonModel cancellationReason)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationReasonModel> response = new List<CancellationReasonModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM subst_cancellation_reasons  ";
                if (cancellationReason != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(cancellationReason.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", cancellationReason.Id);
                    }
                    if (!string.IsNullOrEmpty(cancellationReason.Reason))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reason` = @reason");
                        else
                            sql = string.Concat(sql, " `reason` = @reason");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@reason", cancellationReason.Reason);
                    }
                    if (!string.IsNullOrEmpty(cancellationReason.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", cancellationReason.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(cancellationReason.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", cancellationReason.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(cancellationReason.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", cancellationReason.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(cancellationReason);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationReasonModel>();
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

        public List<CancellationReasonModel> Delete(string connectionString, List<CancellationReasonModel> cancellationReason)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CancellationReasonModel> response = new List<CancellationReasonModel>();
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
                        for (int i = 0; i < cancellationReason.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_cancellation_reasons  ";
                                if (cancellationReason[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(cancellationReason[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, cancellationReason[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationReason[i].Reason))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reason` = @reason" + i);
                                        else
                                            sql = string.Concat(sql, " `reason` = @reason" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@reason" + i, cancellationReason[i].Reason);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationReason[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, cancellationReason[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationReason[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, cancellationReason[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(cancellationReason[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, cancellationReason[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(cancellationReason[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CancellationReasonModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CancellationReasonModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CancellationReasonModel>();
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
