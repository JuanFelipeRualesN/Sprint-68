using System;
using System.Collections.Generic;
using MySqlConnector;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;

namespace SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools
{
    class EntitySubscriptionDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, EntitySubscriptionModel> ReadD(string connectionString, EntitySubscriptionModel entitySubscription, bool firstOne, bool orderByLastDate, EntitySubscriptionModel entitySubscriptionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EntitySubscriptionModel> response = new Dictionary<string, EntitySubscriptionModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `subscription_id`, `platform_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_subscriptions ";
                {
                    if (entitySubscription != null)
                    {
                        if (!string.IsNullOrEmpty(entitySubscription.Id))
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
                            cmd.Parameters.AddWithValue("@id", entitySubscription.Id);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.EntityId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_id` = @entityId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_id` = @entityId ");
                            }
                            cmd.Parameters.AddWithValue("@entityId", entitySubscription.EntityId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionId", entitySubscription.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.PlatformId))
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
                            cmd.Parameters.AddWithValue("@platformId", entitySubscription.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", entitySubscription.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", entitySubscription.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", entitySubscription.UpdatedAt);
                        }
                    }
                    if (entitySubscriptionWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + entitySubscriptionWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.EntityId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_id` LIKE @entityIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_id` LIKE @entityIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + entitySubscriptionWhereLike.EntityId + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + entitySubscriptionWhereLike.SubscriptionId + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.PlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `platform_id` LIKE @platformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `platform_id` LIKE @platformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformIdWhereLike", "%" + entitySubscriptionWhereLike.PlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + entitySubscriptionWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + entitySubscriptionWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + entitySubscriptionWhereLike.UpdatedAt + "%");
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
                                entitySubscription = new EntitySubscriptionModel();
                                if (!reader.IsDBNull(0))
                                    entitySubscription.Id = reader.GetValue(0).ToString();
                                else
                                    entitySubscription.Id = null;
                                if (!reader.IsDBNull(1))
                                    entitySubscription.EntityId = reader.GetValue(1).ToString();
                                else
                                    entitySubscription.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    entitySubscription.SubscriptionId = reader.GetValue(2).ToString();
                                else
                                    entitySubscription.SubscriptionId = null;
                                if (!reader.IsDBNull(3))
                                    entitySubscription.PlatformId = reader.GetValue(3).ToString();
                                else
                                    entitySubscription.PlatformId = null;
                                if (!reader.IsDBNull(4))
                                    entitySubscription.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    entitySubscription.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    entitySubscription.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    entitySubscription.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    entitySubscription.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    entitySubscription.UpdatedAt = null;
                                response.Add(entitySubscription.Id, entitySubscription);
                                entitySubscription = new EntitySubscriptionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EntitySubscriptionModel>();
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
                response = new Dictionary<string, EntitySubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EntitySubscriptionModel> ReadD(string connectionString, EntitySubscriptionModel entitySubscription, bool orderByLastDate, int fromRow, int limit, EntitySubscriptionModel entitySubscriptionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EntitySubscriptionModel> response = new Dictionary<string, EntitySubscriptionModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `subscription_id`, `platform_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_subscriptions ";
                {
                    if (entitySubscription != null)
                    {
                        if (!string.IsNullOrEmpty(entitySubscription.Id))
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
                            cmd.Parameters.AddWithValue("@id", entitySubscription.Id);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.EntityId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_id` = @entityId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_id` = @entityId ");
                            }
                            cmd.Parameters.AddWithValue("@entityId", entitySubscription.EntityId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionId", entitySubscription.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.PlatformId))
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
                            cmd.Parameters.AddWithValue("@platformId", entitySubscription.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", entitySubscription.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", entitySubscription.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", entitySubscription.UpdatedAt);
                        }
                    }
                    if (entitySubscriptionWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + entitySubscriptionWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.EntityId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_id` LIKE @entityIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_id` LIKE @entityIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + entitySubscriptionWhereLike.EntityId + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + entitySubscriptionWhereLike.SubscriptionId + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.PlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `platform_id` LIKE @platformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `platform_id` LIKE @platformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformIdWhereLike", "%" + entitySubscriptionWhereLike.PlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + entitySubscriptionWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + entitySubscriptionWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + entitySubscriptionWhereLike.UpdatedAt + "%");
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
                                entitySubscription = new EntitySubscriptionModel();
                                if (!reader.IsDBNull(0))
                                    entitySubscription.Id = reader.GetValue(0).ToString();
                                else
                                    entitySubscription.Id = null;
                                if (!reader.IsDBNull(1))
                                    entitySubscription.EntityId = reader.GetValue(1).ToString();
                                else
                                    entitySubscription.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    entitySubscription.SubscriptionId = reader.GetValue(2).ToString();
                                else
                                    entitySubscription.SubscriptionId = null;
                                if (!reader.IsDBNull(3))
                                    entitySubscription.PlatformId = reader.GetValue(3).ToString();
                                else
                                    entitySubscription.PlatformId = null;
                                if (!reader.IsDBNull(4))
                                    entitySubscription.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    entitySubscription.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    entitySubscription.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    entitySubscription.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    entitySubscription.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    entitySubscription.UpdatedAt = null;
                                response.Add(entitySubscription.Id, entitySubscription);
                                entitySubscription = new EntitySubscriptionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EntitySubscriptionModel>();
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
                response = new Dictionary<string, EntitySubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EntitySubscriptionModel> ReadD(string connectionString, List<EntitySubscriptionModel> entitySubscriptionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EntitySubscriptionModel> response = new Dictionary<string, EntitySubscriptionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `subscription_id`, `platform_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_subscriptions ";
                if (entitySubscriptionWhere != null && entitySubscriptionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < entitySubscriptionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, entitySubscriptionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, entitySubscriptionWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, entitySubscriptionWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, entitySubscriptionWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, entitySubscriptionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, entitySubscriptionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, entitySubscriptionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != entitySubscriptionWhere.Count - 1)
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
                            EntitySubscriptionModel entitySubscription = new EntitySubscriptionModel(); if (!reader.IsDBNull(0))
                                entitySubscription.Id = reader.GetValue(0).ToString();
                            else
                                entitySubscription.Id = null;
                            if (!reader.IsDBNull(1))
                                entitySubscription.EntityId = reader.GetValue(1).ToString();
                            else
                                entitySubscription.EntityId = null;
                            if (!reader.IsDBNull(2))
                                entitySubscription.SubscriptionId = reader.GetValue(2).ToString();
                            else
                                entitySubscription.SubscriptionId = null;
                            if (!reader.IsDBNull(3))
                                entitySubscription.PlatformId = reader.GetValue(3).ToString();
                            else
                                entitySubscription.PlatformId = null;
                            if (!reader.IsDBNull(4))
                                entitySubscription.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                entitySubscription.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                entitySubscription.CreatedAt = reader.GetValue(5).ToString();
                            else
                                entitySubscription.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                entitySubscription.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                entitySubscription.UpdatedAt = null;
                            response.Add(entitySubscription.Id, entitySubscription);
                            entitySubscription = new EntitySubscriptionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EntitySubscriptionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EntitySubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EntitySubscriptionModel> ReadD(string connectionString, List<EntitySubscriptionModel> entitySubscriptionWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EntitySubscriptionModel> response = new Dictionary<string, EntitySubscriptionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `subscription_id`, `platform_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_subscriptions ";
                if (entitySubscriptionWhere != null && entitySubscriptionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < entitySubscriptionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, entitySubscriptionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, entitySubscriptionWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, entitySubscriptionWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, entitySubscriptionWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, entitySubscriptionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, entitySubscriptionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, entitySubscriptionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != entitySubscriptionWhere.Count - 1)
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
                            EntitySubscriptionModel entitySubscription = new EntitySubscriptionModel(); if (!reader.IsDBNull(0))
                                entitySubscription.Id = reader.GetValue(0).ToString();
                            else
                                entitySubscription.Id = null;
                            if (!reader.IsDBNull(1))
                                entitySubscription.EntityId = reader.GetValue(1).ToString();
                            else
                                entitySubscription.EntityId = null;
                            if (!reader.IsDBNull(2))
                                entitySubscription.SubscriptionId = reader.GetValue(2).ToString();
                            else
                                entitySubscription.SubscriptionId = null;
                            if (!reader.IsDBNull(3))
                                entitySubscription.PlatformId = reader.GetValue(3).ToString();
                            else
                                entitySubscription.PlatformId = null;
                            if (!reader.IsDBNull(4))
                                entitySubscription.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                entitySubscription.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                entitySubscription.CreatedAt = reader.GetValue(5).ToString();
                            else
                                entitySubscription.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                entitySubscription.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                entitySubscription.UpdatedAt = null;
                            response.Add(entitySubscription.Id, entitySubscription);
                            entitySubscription = new EntitySubscriptionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EntitySubscriptionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EntitySubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EntitySubscriptionModel> Read(string connectionString, EntitySubscriptionModel entitySubscription, bool firstOne, bool orderByLastDate, EntitySubscriptionModel entitySubscriptionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntitySubscriptionModel> response = new List<EntitySubscriptionModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `subscription_id`, `platform_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_subscriptions ";
                {
                    if (entitySubscription != null)
                    {
                        if (!string.IsNullOrEmpty(entitySubscription.Id))
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
                            cmd.Parameters.AddWithValue("@id", entitySubscription.Id);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.EntityId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_id` = @entityId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_id` = @entityId ");
                            }
                            cmd.Parameters.AddWithValue("@entityId", entitySubscription.EntityId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionId", entitySubscription.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.PlatformId))
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
                            cmd.Parameters.AddWithValue("@platformId", entitySubscription.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", entitySubscription.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", entitySubscription.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", entitySubscription.UpdatedAt);
                        }
                    }
                    if (entitySubscriptionWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + entitySubscriptionWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.EntityId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_id` LIKE @entityIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_id` LIKE @entityIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + entitySubscriptionWhereLike.EntityId + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + entitySubscriptionWhereLike.SubscriptionId + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.PlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `platform_id` LIKE @platformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `platform_id` LIKE @platformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformIdWhereLike", "%" + entitySubscriptionWhereLike.PlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + entitySubscriptionWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + entitySubscriptionWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + entitySubscriptionWhereLike.UpdatedAt + "%");
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
                                entitySubscription = new EntitySubscriptionModel();
                                if (!reader.IsDBNull(0))
                                    entitySubscription.Id = reader.GetValue(0).ToString();
                                else
                                    entitySubscription.Id = null;
                                if (!reader.IsDBNull(1))
                                    entitySubscription.EntityId = reader.GetValue(1).ToString();
                                else
                                    entitySubscription.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    entitySubscription.SubscriptionId = reader.GetValue(2).ToString();
                                else
                                    entitySubscription.SubscriptionId = null;
                                if (!reader.IsDBNull(3))
                                    entitySubscription.PlatformId = reader.GetValue(3).ToString();
                                else
                                    entitySubscription.PlatformId = null;
                                if (!reader.IsDBNull(4))
                                    entitySubscription.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    entitySubscription.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    entitySubscription.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    entitySubscription.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    entitySubscription.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    entitySubscription.UpdatedAt = null;
                                response.Add(entitySubscription);
                                entitySubscription = new EntitySubscriptionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EntitySubscriptionModel>();
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
                response = new List<EntitySubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EntitySubscriptionModel> Read(string connectionString, EntitySubscriptionModel entitySubscription, bool orderByLastDate, int fromRow, int limit, EntitySubscriptionModel entitySubscriptionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntitySubscriptionModel> response = new List<EntitySubscriptionModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `subscription_id`, `platform_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_subscriptions ";
                {
                    if (entitySubscription != null)
                    {
                        if (!string.IsNullOrEmpty(entitySubscription.Id))
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
                            cmd.Parameters.AddWithValue("@id", entitySubscription.Id);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.EntityId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_id` = @entityId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_id` = @entityId ");
                            }
                            cmd.Parameters.AddWithValue("@entityId", entitySubscription.EntityId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionId", entitySubscription.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.PlatformId))
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
                            cmd.Parameters.AddWithValue("@platformId", entitySubscription.PlatformId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", entitySubscription.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", entitySubscription.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entitySubscription.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", entitySubscription.UpdatedAt);
                        }
                    }
                    if (entitySubscriptionWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + entitySubscriptionWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.EntityId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_id` LIKE @entityIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_id` LIKE @entityIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + entitySubscriptionWhereLike.EntityId + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.SubscriptionId))
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
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + entitySubscriptionWhereLike.SubscriptionId + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.PlatformId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `platform_id` LIKE @platformIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `platform_id` LIKE @platformIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@platformIdWhereLike", "%" + entitySubscriptionWhereLike.PlatformId + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + entitySubscriptionWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + entitySubscriptionWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + entitySubscriptionWhereLike.UpdatedAt + "%");
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
                                entitySubscription = new EntitySubscriptionModel();
                                if (!reader.IsDBNull(0))
                                    entitySubscription.Id = reader.GetValue(0).ToString();
                                else
                                    entitySubscription.Id = null;
                                if (!reader.IsDBNull(1))
                                    entitySubscription.EntityId = reader.GetValue(1).ToString();
                                else
                                    entitySubscription.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    entitySubscription.SubscriptionId = reader.GetValue(2).ToString();
                                else
                                    entitySubscription.SubscriptionId = null;
                                if (!reader.IsDBNull(3))
                                    entitySubscription.PlatformId = reader.GetValue(3).ToString();
                                else
                                    entitySubscription.PlatformId = null;
                                if (!reader.IsDBNull(4))
                                    entitySubscription.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    entitySubscription.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    entitySubscription.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    entitySubscription.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    entitySubscription.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    entitySubscription.UpdatedAt = null;
                                response.Add(entitySubscription);
                                entitySubscription = new EntitySubscriptionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EntitySubscriptionModel>();
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
                response = new List<EntitySubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EntitySubscriptionModel> Read(string connectionString, List<EntitySubscriptionModel> entitySubscriptionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntitySubscriptionModel> response = new List<EntitySubscriptionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `subscription_id`, `platform_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_subscriptions ";
                if (entitySubscriptionWhere != null && entitySubscriptionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < entitySubscriptionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, entitySubscriptionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, entitySubscriptionWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, entitySubscriptionWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, entitySubscriptionWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, entitySubscriptionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, entitySubscriptionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, entitySubscriptionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != entitySubscriptionWhere.Count - 1)
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
                            EntitySubscriptionModel entitySubscription = new EntitySubscriptionModel(); if (!reader.IsDBNull(0))
                                entitySubscription.Id = reader.GetValue(0).ToString();
                            else
                                entitySubscription.Id = null;
                            if (!reader.IsDBNull(1))
                                entitySubscription.EntityId = reader.GetValue(1).ToString();
                            else
                                entitySubscription.EntityId = null;
                            if (!reader.IsDBNull(2))
                                entitySubscription.SubscriptionId = reader.GetValue(2).ToString();
                            else
                                entitySubscription.SubscriptionId = null;
                            if (!reader.IsDBNull(3))
                                entitySubscription.PlatformId = reader.GetValue(3).ToString();
                            else
                                entitySubscription.PlatformId = null;
                            if (!reader.IsDBNull(4))
                                entitySubscription.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                entitySubscription.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                entitySubscription.CreatedAt = reader.GetValue(5).ToString();
                            else
                                entitySubscription.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                entitySubscription.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                entitySubscription.UpdatedAt = null;
                            response.Add(entitySubscription);
                            entitySubscription = new EntitySubscriptionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EntitySubscriptionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EntitySubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EntitySubscriptionModel> Read(string connectionString, List<EntitySubscriptionModel> entitySubscriptionWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntitySubscriptionModel> response = new List<EntitySubscriptionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `subscription_id`, `platform_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_subscriptions ";
                if (entitySubscriptionWhere != null && entitySubscriptionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < entitySubscriptionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, entitySubscriptionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, entitySubscriptionWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, entitySubscriptionWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].PlatformId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform_id` = @platformId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platformId" + i, entitySubscriptionWhere[i].PlatformId);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, entitySubscriptionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, entitySubscriptionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entitySubscriptionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, entitySubscriptionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != entitySubscriptionWhere.Count - 1)
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
                            EntitySubscriptionModel entitySubscription = new EntitySubscriptionModel(); if (!reader.IsDBNull(0))
                                entitySubscription.Id = reader.GetValue(0).ToString();
                            else
                                entitySubscription.Id = null;
                            if (!reader.IsDBNull(1))
                                entitySubscription.EntityId = reader.GetValue(1).ToString();
                            else
                                entitySubscription.EntityId = null;
                            if (!reader.IsDBNull(2))
                                entitySubscription.SubscriptionId = reader.GetValue(2).ToString();
                            else
                                entitySubscription.SubscriptionId = null;
                            if (!reader.IsDBNull(3))
                                entitySubscription.PlatformId = reader.GetValue(3).ToString();
                            else
                                entitySubscription.PlatformId = null;
                            if (!reader.IsDBNull(4))
                                entitySubscription.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                entitySubscription.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                entitySubscription.CreatedAt = reader.GetValue(5).ToString();
                            else
                                entitySubscription.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                entitySubscription.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                entitySubscription.UpdatedAt = null;
                            response.Add(entitySubscription);
                            entitySubscription = new EntitySubscriptionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EntitySubscriptionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EntitySubscriptionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EntitySubscriptionModel> Create(string connectionString, EntitySubscriptionModel entitySubscription)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntitySubscriptionModel> response = new List<EntitySubscriptionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO sch_entity_subscriptions (";
                string values = "(";
                if (!string.IsNullOrEmpty(entitySubscription.Id))
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
                    cmd.Parameters.AddWithValue("@id", entitySubscription.Id);
                }
                if (!string.IsNullOrEmpty(entitySubscription.EntityId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `entity_id` ");
                        values = string.Concat(values, ", @entityId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `entity_id` ");
                        values = string.Concat(values, " @entityId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@entityId", entitySubscription.EntityId);
                }
                if (!string.IsNullOrEmpty(entitySubscription.SubscriptionId))
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
                    cmd.Parameters.AddWithValue("@subscriptionId", entitySubscription.SubscriptionId);
                }
                if (!string.IsNullOrEmpty(entitySubscription.PlatformId))
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
                    cmd.Parameters.AddWithValue("@platformId", entitySubscription.PlatformId);
                }
                if (!string.IsNullOrEmpty(entitySubscription.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", entitySubscription.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(entitySubscription.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", entitySubscription.CreatedAt);
                }
                if (!string.IsNullOrEmpty(entitySubscription.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", entitySubscription.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                entitySubscription.Id = cmd.LastInsertedId.ToString();
                response.Add(entitySubscription);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EntitySubscriptionModel>();
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
        public List<EntitySubscriptionModel> Create(string connectionString, List<EntitySubscriptionModel> entitySubscriptionList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntitySubscriptionModel> response = new List<EntitySubscriptionModel>();
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
                        for (int i = 0; i < entitySubscriptionList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO sch_entity_subscriptions (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(entitySubscriptionList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, entitySubscriptionList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(entitySubscriptionList[i].EntityId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `entity_id` ");
                                        values = string.Concat(values, ", @entityId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `entity_id` ");
                                        values = string.Concat(values, "@entityId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@entityId" + i, entitySubscriptionList[i].EntityId);
                                }
                                if (!string.IsNullOrEmpty(entitySubscriptionList[i].SubscriptionId))
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
                                    cmd.Parameters.AddWithValue("@subscriptionId" + i, entitySubscriptionList[i].SubscriptionId);
                                }
                                if (!string.IsNullOrEmpty(entitySubscriptionList[i].PlatformId))
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
                                    cmd.Parameters.AddWithValue("@platformId" + i, entitySubscriptionList[i].PlatformId);
                                }
                                if (!string.IsNullOrEmpty(entitySubscriptionList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, entitySubscriptionList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(entitySubscriptionList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, entitySubscriptionList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(entitySubscriptionList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, entitySubscriptionList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(entitySubscriptionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<EntitySubscriptionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EntitySubscriptionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EntitySubscriptionModel>();
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

        public List<EntitySubscriptionModel> Update(string connectionString, EntitySubscriptionModel entitySubscription, EntitySubscriptionModel entitySubscriptionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntitySubscriptionModel> response = new List<EntitySubscriptionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE sch_entity_subscriptions SET ";
                if (!string.IsNullOrEmpty(entitySubscription.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", entitySubscription.Id);
                }
                if (!string.IsNullOrEmpty(entitySubscription.EntityId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `entity_id` = @entityId");
                    else
                        sql = string.Concat(sql, " `entity_id` = @entityId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@entityId", entitySubscription.EntityId);
                }
                if (!string.IsNullOrEmpty(entitySubscription.SubscriptionId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `subscription_id` = @subscriptionId");
                    else
                        sql = string.Concat(sql, " `subscription_id` = @subscriptionId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subscriptionId", entitySubscription.SubscriptionId);
                }
                if (!string.IsNullOrEmpty(entitySubscription.PlatformId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `platform_id` = @platformId");
                    else
                        sql = string.Concat(sql, " `platform_id` = @platformId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@platformId", entitySubscription.PlatformId);
                }
                if (!string.IsNullOrEmpty(entitySubscription.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", entitySubscription.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(entitySubscription.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", entitySubscription.CreatedAt);
                }
                if (!string.IsNullOrEmpty(entitySubscription.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", entitySubscription.UpdatedAt);
                }
                if (entitySubscriptionWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(entitySubscriptionWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", entitySubscriptionWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(entitySubscriptionWhere.EntityId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `entity_id` = @entityIdWhere");
                        else
                            sql = string.Concat(sql, " `entity_id` = @entityIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@entityIdWhere", entitySubscriptionWhere.EntityId);
                    }
                    if (!string.IsNullOrEmpty(entitySubscriptionWhere.SubscriptionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionIdWhere");
                        else
                            sql = string.Concat(sql, " `subscription_id` = @subscriptionIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionIdWhere", entitySubscriptionWhere.SubscriptionId);
                    }
                    if (!string.IsNullOrEmpty(entitySubscriptionWhere.PlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform_id` = @platformIdWhere");
                        else
                            sql = string.Concat(sql, " `platform_id` = @platformIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformIdWhere", entitySubscriptionWhere.PlatformId);
                    }
                    if (!string.IsNullOrEmpty(entitySubscriptionWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", entitySubscriptionWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(entitySubscriptionWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", entitySubscriptionWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(entitySubscriptionWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", entitySubscriptionWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(entitySubscription);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EntitySubscriptionModel>();
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

        public List<EntitySubscriptionModel> Update(string connectionString, List<EntitySubscriptionModel> entitySubscriptionList, List<EntitySubscriptionModel> entitySubscriptionWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntitySubscriptionModel> response = new List<EntitySubscriptionModel>();
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
                        for (int i = 0; i < entitySubscriptionList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE sch_entity_subscriptions SET ";
                                if (!string.IsNullOrEmpty(entitySubscriptionList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, entitySubscriptionList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(entitySubscriptionList[i].EntityId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `entity_id` = @entityId" + i);
                                    else
                                        sql = string.Concat(sql, " `entity_id` = @entityId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@entityId" + i, entitySubscriptionList[i].EntityId);
                                }
                                if (!string.IsNullOrEmpty(entitySubscriptionList[i].SubscriptionId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `subscription_id` = @subscriptionId" + i);
                                    else
                                        sql = string.Concat(sql, " `subscription_id` = @subscriptionId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subscriptionId" + i, entitySubscriptionList[i].SubscriptionId);
                                }
                                if (!string.IsNullOrEmpty(entitySubscriptionList[i].PlatformId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `platform_id` = @platformId" + i);
                                    else
                                        sql = string.Concat(sql, " `platform_id` = @platformId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@platformId" + i, entitySubscriptionList[i].PlatformId);
                                }
                                if (!string.IsNullOrEmpty(entitySubscriptionList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, entitySubscriptionList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(entitySubscriptionList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, entitySubscriptionList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(entitySubscriptionList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, entitySubscriptionList[i].UpdatedAt);
                                }
                                if (entitySubscriptionWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(entitySubscriptionWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, entitySubscriptionWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(entitySubscriptionWhereList[i].EntityId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `entity_id` = @entityIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `entity_id` = @entityIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@entityIdWhere" + i, entitySubscriptionWhereList[i].EntityId);
                                    }
                                    if (!string.IsNullOrEmpty(entitySubscriptionWhereList[i].SubscriptionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `subscription_id` = @subscriptionIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionIdWhere" + i, entitySubscriptionWhereList[i].SubscriptionId);
                                    }
                                    if (!string.IsNullOrEmpty(entitySubscriptionWhereList[i].PlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform_id` = @platformIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `platform_id` = @platformIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformIdWhere" + i, entitySubscriptionWhereList[i].PlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(entitySubscriptionWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, entitySubscriptionWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(entitySubscriptionWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, entitySubscriptionWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(entitySubscriptionWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, entitySubscriptionWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(entitySubscriptionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EntitySubscriptionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EntitySubscriptionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EntitySubscriptionModel>();
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

        public List<EntitySubscriptionModel> Delete(string connectionString, EntitySubscriptionModel entitySubscription)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntitySubscriptionModel> response = new List<EntitySubscriptionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM sch_entity_subscriptions  ";
                if (entitySubscription != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(entitySubscription.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", entitySubscription.Id);
                    }
                    if (!string.IsNullOrEmpty(entitySubscription.EntityId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `entity_id` = @entityId");
                        else
                            sql = string.Concat(sql, " `entity_id` = @entityId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@entityId", entitySubscription.EntityId);
                    }
                    if (!string.IsNullOrEmpty(entitySubscription.SubscriptionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionId");
                        else
                            sql = string.Concat(sql, " `subscription_id` = @subscriptionId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionId", entitySubscription.SubscriptionId);
                    }
                    if (!string.IsNullOrEmpty(entitySubscription.PlatformId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform_id` = @platformId");
                        else
                            sql = string.Concat(sql, " `platform_id` = @platformId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformId", entitySubscription.PlatformId);
                    }
                    if (!string.IsNullOrEmpty(entitySubscription.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", entitySubscription.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(entitySubscription.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", entitySubscription.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(entitySubscription.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", entitySubscription.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(entitySubscription);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EntitySubscriptionModel>();
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

        public List<EntitySubscriptionModel> Delete(string connectionString, List<EntitySubscriptionModel> entitySubscription)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntitySubscriptionModel> response = new List<EntitySubscriptionModel>();
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
                        for (int i = 0; i < entitySubscription.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM sch_entity_subscriptions  ";
                                if (entitySubscription[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(entitySubscription[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, entitySubscription[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(entitySubscription[i].EntityId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `entity_id` = @entityId" + i);
                                        else
                                            sql = string.Concat(sql, " `entity_id` = @entityId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@entityId" + i, entitySubscription[i].EntityId);
                                    }
                                    if (!string.IsNullOrEmpty(entitySubscription[i].SubscriptionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionId" + i);
                                        else
                                            sql = string.Concat(sql, " `subscription_id` = @subscriptionId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionId" + i, entitySubscription[i].SubscriptionId);
                                    }
                                    if (!string.IsNullOrEmpty(entitySubscription[i].PlatformId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform_id` = @platformId" + i);
                                        else
                                            sql = string.Concat(sql, " `platform_id` = @platformId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformId" + i, entitySubscription[i].PlatformId);
                                    }
                                    if (!string.IsNullOrEmpty(entitySubscription[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, entitySubscription[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(entitySubscription[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, entitySubscription[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(entitySubscription[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, entitySubscription[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(entitySubscription[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EntitySubscriptionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EntitySubscriptionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EntitySubscriptionModel>();
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
