using System;
using System.Collections.Generic;
using MySqlConnector;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;

namespace SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools
{
    class EntityDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, EntityModel> ReadD(string connectionString, EntityModel entity, bool firstOne, bool orderByLastDate, EntityModel entityWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EntityModel> response = new Dictionary<string, EntityModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `token`, `entity_type_id`, `private`, `modified_by`, `created_at`, `updated_at`  FROM sch_entities ";
                {
                    if (entity != null)
                    {
                        if (!string.IsNullOrEmpty(entity.Id))
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
                            cmd.Parameters.AddWithValue("@id", entity.Id);
                        }
                        if (!string.IsNullOrEmpty(entity.Name))
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
                            cmd.Parameters.AddWithValue("@name", entity.Name);
                        }
                        if (!string.IsNullOrEmpty(entity.Token))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `token` = @token ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `token` = @token ");
                            }
                            cmd.Parameters.AddWithValue("@token", entity.Token);
                        }
                        if (!string.IsNullOrEmpty(entity.EntityTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_type_id` = @entityTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_type_id` = @entityTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@entityTypeId", entity.EntityTypeId);
                        }
                        if (!string.IsNullOrEmpty(entity.Private))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `private` = @private ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `private` = @private ");
                            }
                            cmd.Parameters.AddWithValue("@private", entity.Private);
                        }
                        if (!string.IsNullOrEmpty(entity.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", entity.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entity.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", entity.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entity.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", entity.UpdatedAt);
                        }
                    }
                    if (entityWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(entityWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + entityWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + entityWhereLike.Name + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.Token))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `token` LIKE @tokenWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `token` LIKE @tokenWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@tokenWhereLike", "%" + entityWhereLike.Token + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.EntityTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_type_id` LIKE @entityTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_type_id` LIKE @entityTypeIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityTypeIdWhereLike", "%" + entityWhereLike.EntityTypeId + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.Private))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `private` LIKE @privateWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `private` LIKE @privateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@privateWhereLike", "%" + entityWhereLike.Private + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + entityWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + entityWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + entityWhereLike.UpdatedAt + "%");
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
                                entity = new EntityModel();
                                if (!reader.IsDBNull(0))
                                    entity.Id = reader.GetValue(0).ToString();
                                else
                                    entity.Id = null;
                                if (!reader.IsDBNull(1))
                                    entity.Name = reader.GetValue(1).ToString();
                                else
                                    entity.Name = null;
                                if (!reader.IsDBNull(2))
                                    entity.Token = reader.GetValue(2).ToString();
                                else
                                    entity.Token = null;
                                if (!reader.IsDBNull(3))
                                    entity.EntityTypeId = reader.GetValue(3).ToString();
                                else
                                    entity.EntityTypeId = null;
                                if (!reader.IsDBNull(4))
                                    entity.Private = reader.GetValue(4).ToString();
                                else
                                    entity.Private = null;
                                if (!reader.IsDBNull(5))
                                    entity.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    entity.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    entity.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    entity.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    entity.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    entity.UpdatedAt = null;
                                response.Add(entity.Id, entity);
                                entity = new EntityModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EntityModel>();
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
                response = new Dictionary<string, EntityModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EntityModel> ReadD(string connectionString, EntityModel entity, bool orderByLastDate, int fromRow, int limit, EntityModel entityWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EntityModel> response = new Dictionary<string, EntityModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `token`, `entity_type_id`, `private`, `modified_by`, `created_at`, `updated_at`  FROM sch_entities ";
                {
                    if (entity != null)
                    {
                        if (!string.IsNullOrEmpty(entity.Id))
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
                            cmd.Parameters.AddWithValue("@id", entity.Id);
                        }
                        if (!string.IsNullOrEmpty(entity.Name))
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
                            cmd.Parameters.AddWithValue("@name", entity.Name);
                        }
                        if (!string.IsNullOrEmpty(entity.Token))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `token` = @token ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `token` = @token ");
                            }
                            cmd.Parameters.AddWithValue("@token", entity.Token);
                        }
                        if (!string.IsNullOrEmpty(entity.EntityTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_type_id` = @entityTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_type_id` = @entityTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@entityTypeId", entity.EntityTypeId);
                        }
                        if (!string.IsNullOrEmpty(entity.Private))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `private` = @private ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `private` = @private ");
                            }
                            cmd.Parameters.AddWithValue("@private", entity.Private);
                        }
                        if (!string.IsNullOrEmpty(entity.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", entity.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entity.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", entity.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entity.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", entity.UpdatedAt);
                        }
                    }
                    if (entityWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(entityWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + entityWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + entityWhereLike.Name + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.Token))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `token` LIKE @tokenWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `token` LIKE @tokenWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@tokenWhereLike", "%" + entityWhereLike.Token + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.EntityTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_type_id` LIKE @entityTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_type_id` LIKE @entityTypeIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityTypeIdWhereLike", "%" + entityWhereLike.EntityTypeId + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.Private))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `private` LIKE @privateWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `private` LIKE @privateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@privateWhereLike", "%" + entityWhereLike.Private + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + entityWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + entityWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + entityWhereLike.UpdatedAt + "%");
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
                                entity = new EntityModel();
                                if (!reader.IsDBNull(0))
                                    entity.Id = reader.GetValue(0).ToString();
                                else
                                    entity.Id = null;
                                if (!reader.IsDBNull(1))
                                    entity.Name = reader.GetValue(1).ToString();
                                else
                                    entity.Name = null;
                                if (!reader.IsDBNull(2))
                                    entity.Token = reader.GetValue(2).ToString();
                                else
                                    entity.Token = null;
                                if (!reader.IsDBNull(3))
                                    entity.EntityTypeId = reader.GetValue(3).ToString();
                                else
                                    entity.EntityTypeId = null;
                                if (!reader.IsDBNull(4))
                                    entity.Private = reader.GetValue(4).ToString();
                                else
                                    entity.Private = null;
                                if (!reader.IsDBNull(5))
                                    entity.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    entity.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    entity.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    entity.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    entity.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    entity.UpdatedAt = null;
                                response.Add(entity.Id, entity);
                                entity = new EntityModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EntityModel>();
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
                response = new Dictionary<string, EntityModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EntityModel> ReadD(string connectionString, List<EntityModel> entityWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EntityModel> response = new Dictionary<string, EntityModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `token`, `entity_type_id`, `private`, `modified_by`, `created_at`, `updated_at`  FROM sch_entities ";
                if (entityWhere != null && entityWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < entityWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(entityWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, entityWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, entityWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].Token))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `token` = @token" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@token" + i, entityWhere[i].Token);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].EntityTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_type_id` = @entityTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityTypeId" + i, entityWhere[i].EntityTypeId);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].Private))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `private` = @private" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@private" + i, entityWhere[i].Private);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, entityWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, entityWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, entityWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != entityWhere.Count - 1)
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
                            EntityModel entity = new EntityModel(); if (!reader.IsDBNull(0))
                                entity.Id = reader.GetValue(0).ToString();
                            else
                                entity.Id = null;
                            if (!reader.IsDBNull(1))
                                entity.Name = reader.GetValue(1).ToString();
                            else
                                entity.Name = null;
                            if (!reader.IsDBNull(2))
                                entity.Token = reader.GetValue(2).ToString();
                            else
                                entity.Token = null;
                            if (!reader.IsDBNull(3))
                                entity.EntityTypeId = reader.GetValue(3).ToString();
                            else
                                entity.EntityTypeId = null;
                            if (!reader.IsDBNull(4))
                                entity.Private = reader.GetValue(4).ToString();
                            else
                                entity.Private = null;
                            if (!reader.IsDBNull(5))
                                entity.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                entity.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                entity.CreatedAt = reader.GetValue(6).ToString();
                            else
                                entity.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                entity.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                entity.UpdatedAt = null;
                            response.Add(entity.Id, entity);
                            entity = new EntityModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EntityModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EntityModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EntityModel> ReadD(string connectionString, List<EntityModel> entityWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EntityModel> response = new Dictionary<string, EntityModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `token`, `entity_type_id`, `private`, `modified_by`, `created_at`, `updated_at`  FROM sch_entities ";
                if (entityWhere != null && entityWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < entityWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(entityWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, entityWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, entityWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].Token))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `token` = @token" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@token" + i, entityWhere[i].Token);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].EntityTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_type_id` = @entityTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityTypeId" + i, entityWhere[i].EntityTypeId);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].Private))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `private` = @private" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@private" + i, entityWhere[i].Private);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, entityWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, entityWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, entityWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != entityWhere.Count - 1)
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
                            EntityModel entity = new EntityModel(); if (!reader.IsDBNull(0))
                                entity.Id = reader.GetValue(0).ToString();
                            else
                                entity.Id = null;
                            if (!reader.IsDBNull(1))
                                entity.Name = reader.GetValue(1).ToString();
                            else
                                entity.Name = null;
                            if (!reader.IsDBNull(2))
                                entity.Token = reader.GetValue(2).ToString();
                            else
                                entity.Token = null;
                            if (!reader.IsDBNull(3))
                                entity.EntityTypeId = reader.GetValue(3).ToString();
                            else
                                entity.EntityTypeId = null;
                            if (!reader.IsDBNull(4))
                                entity.Private = reader.GetValue(4).ToString();
                            else
                                entity.Private = null;
                            if (!reader.IsDBNull(5))
                                entity.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                entity.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                entity.CreatedAt = reader.GetValue(6).ToString();
                            else
                                entity.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                entity.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                entity.UpdatedAt = null;
                            response.Add(entity.Id, entity);
                            entity = new EntityModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EntityModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EntityModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EntityModel> Read(string connectionString, EntityModel entity, bool firstOne, bool orderByLastDate, EntityModel entityWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntityModel> response = new List<EntityModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `token`, `entity_type_id`, `private`, `modified_by`, `created_at`, `updated_at`  FROM sch_entities ";
                {
                    if (entity != null)
                    {
                        if (!string.IsNullOrEmpty(entity.Id))
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
                            cmd.Parameters.AddWithValue("@id", entity.Id);
                        }
                        if (!string.IsNullOrEmpty(entity.Name))
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
                            cmd.Parameters.AddWithValue("@name", entity.Name);
                        }
                        if (!string.IsNullOrEmpty(entity.Token))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `token` = @token ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `token` = @token ");
                            }
                            cmd.Parameters.AddWithValue("@token", entity.Token);
                        }
                        if (!string.IsNullOrEmpty(entity.EntityTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_type_id` = @entityTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_type_id` = @entityTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@entityTypeId", entity.EntityTypeId);
                        }
                        if (!string.IsNullOrEmpty(entity.Private))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `private` = @private ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `private` = @private ");
                            }
                            cmd.Parameters.AddWithValue("@private", entity.Private);
                        }
                        if (!string.IsNullOrEmpty(entity.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", entity.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entity.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", entity.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entity.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", entity.UpdatedAt);
                        }
                    }
                    if (entityWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(entityWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + entityWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + entityWhereLike.Name + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.Token))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `token` LIKE @tokenWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `token` LIKE @tokenWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@tokenWhereLike", "%" + entityWhereLike.Token + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.EntityTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_type_id` LIKE @entityTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_type_id` LIKE @entityTypeIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityTypeIdWhereLike", "%" + entityWhereLike.EntityTypeId + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.Private))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `private` LIKE @privateWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `private` LIKE @privateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@privateWhereLike", "%" + entityWhereLike.Private + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + entityWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + entityWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + entityWhereLike.UpdatedAt + "%");
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
                                entity = new EntityModel();
                                if (!reader.IsDBNull(0))
                                    entity.Id = reader.GetValue(0).ToString();
                                else
                                    entity.Id = null;
                                if (!reader.IsDBNull(1))
                                    entity.Name = reader.GetValue(1).ToString();
                                else
                                    entity.Name = null;
                                if (!reader.IsDBNull(2))
                                    entity.Token = reader.GetValue(2).ToString();
                                else
                                    entity.Token = null;
                                if (!reader.IsDBNull(3))
                                    entity.EntityTypeId = reader.GetValue(3).ToString();
                                else
                                    entity.EntityTypeId = null;
                                if (!reader.IsDBNull(4))
                                    entity.Private = reader.GetValue(4).ToString();
                                else
                                    entity.Private = null;
                                if (!reader.IsDBNull(5))
                                    entity.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    entity.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    entity.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    entity.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    entity.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    entity.UpdatedAt = null;
                                response.Add(entity);
                                entity = new EntityModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EntityModel>();
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
                response = new List<EntityModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EntityModel> Read(string connectionString, EntityModel entity, bool orderByLastDate, int fromRow, int limit, EntityModel entityWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntityModel> response = new List<EntityModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `token`, `entity_type_id`, `private`, `modified_by`, `created_at`, `updated_at`  FROM sch_entities ";
                {
                    if (entity != null)
                    {
                        if (!string.IsNullOrEmpty(entity.Id))
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
                            cmd.Parameters.AddWithValue("@id", entity.Id);
                        }
                        if (!string.IsNullOrEmpty(entity.Name))
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
                            cmd.Parameters.AddWithValue("@name", entity.Name);
                        }
                        if (!string.IsNullOrEmpty(entity.Token))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `token` = @token ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `token` = @token ");
                            }
                            cmd.Parameters.AddWithValue("@token", entity.Token);
                        }
                        if (!string.IsNullOrEmpty(entity.EntityTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_type_id` = @entityTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_type_id` = @entityTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@entityTypeId", entity.EntityTypeId);
                        }
                        if (!string.IsNullOrEmpty(entity.Private))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `private` = @private ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `private` = @private ");
                            }
                            cmd.Parameters.AddWithValue("@private", entity.Private);
                        }
                        if (!string.IsNullOrEmpty(entity.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", entity.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entity.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", entity.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entity.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", entity.UpdatedAt);
                        }
                    }
                    if (entityWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(entityWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + entityWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + entityWhereLike.Name + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.Token))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `token` LIKE @tokenWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `token` LIKE @tokenWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@tokenWhereLike", "%" + entityWhereLike.Token + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.EntityTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_type_id` LIKE @entityTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_type_id` LIKE @entityTypeIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityTypeIdWhereLike", "%" + entityWhereLike.EntityTypeId + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.Private))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `private` LIKE @privateWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `private` LIKE @privateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@privateWhereLike", "%" + entityWhereLike.Private + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + entityWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + entityWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(entityWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + entityWhereLike.UpdatedAt + "%");
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
                                entity = new EntityModel();
                                if (!reader.IsDBNull(0))
                                    entity.Id = reader.GetValue(0).ToString();
                                else
                                    entity.Id = null;
                                if (!reader.IsDBNull(1))
                                    entity.Name = reader.GetValue(1).ToString();
                                else
                                    entity.Name = null;
                                if (!reader.IsDBNull(2))
                                    entity.Token = reader.GetValue(2).ToString();
                                else
                                    entity.Token = null;
                                if (!reader.IsDBNull(3))
                                    entity.EntityTypeId = reader.GetValue(3).ToString();
                                else
                                    entity.EntityTypeId = null;
                                if (!reader.IsDBNull(4))
                                    entity.Private = reader.GetValue(4).ToString();
                                else
                                    entity.Private = null;
                                if (!reader.IsDBNull(5))
                                    entity.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    entity.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    entity.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    entity.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    entity.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    entity.UpdatedAt = null;
                                response.Add(entity);
                                entity = new EntityModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EntityModel>();
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
                response = new List<EntityModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EntityModel> Read(string connectionString, List<EntityModel> entityWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntityModel> response = new List<EntityModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `token`, `entity_type_id`, `private`, `modified_by`, `created_at`, `updated_at`  FROM sch_entities ";
                if (entityWhere != null && entityWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < entityWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(entityWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, entityWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, entityWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].Token))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `token` = @token" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@token" + i, entityWhere[i].Token);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].EntityTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_type_id` = @entityTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityTypeId" + i, entityWhere[i].EntityTypeId);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].Private))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `private` = @private" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@private" + i, entityWhere[i].Private);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, entityWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, entityWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, entityWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != entityWhere.Count - 1)
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
                            EntityModel entity = new EntityModel(); if (!reader.IsDBNull(0))
                                entity.Id = reader.GetValue(0).ToString();
                            else
                                entity.Id = null;
                            if (!reader.IsDBNull(1))
                                entity.Name = reader.GetValue(1).ToString();
                            else
                                entity.Name = null;
                            if (!reader.IsDBNull(2))
                                entity.Token = reader.GetValue(2).ToString();
                            else
                                entity.Token = null;
                            if (!reader.IsDBNull(3))
                                entity.EntityTypeId = reader.GetValue(3).ToString();
                            else
                                entity.EntityTypeId = null;
                            if (!reader.IsDBNull(4))
                                entity.Private = reader.GetValue(4).ToString();
                            else
                                entity.Private = null;
                            if (!reader.IsDBNull(5))
                                entity.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                entity.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                entity.CreatedAt = reader.GetValue(6).ToString();
                            else
                                entity.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                entity.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                entity.UpdatedAt = null;
                            response.Add(entity);
                            entity = new EntityModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EntityModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EntityModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EntityModel> Read(string connectionString, List<EntityModel> entityWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntityModel> response = new List<EntityModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `token`, `entity_type_id`, `private`, `modified_by`, `created_at`, `updated_at`  FROM sch_entities ";
                if (entityWhere != null && entityWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < entityWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(entityWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, entityWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, entityWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].Token))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `token` = @token" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@token" + i, entityWhere[i].Token);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].EntityTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_type_id` = @entityTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityTypeId" + i, entityWhere[i].EntityTypeId);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].Private))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `private` = @private" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@private" + i, entityWhere[i].Private);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, entityWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, entityWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(entityWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, entityWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != entityWhere.Count - 1)
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
                            EntityModel entity = new EntityModel(); if (!reader.IsDBNull(0))
                                entity.Id = reader.GetValue(0).ToString();
                            else
                                entity.Id = null;
                            if (!reader.IsDBNull(1))
                                entity.Name = reader.GetValue(1).ToString();
                            else
                                entity.Name = null;
                            if (!reader.IsDBNull(2))
                                entity.Token = reader.GetValue(2).ToString();
                            else
                                entity.Token = null;
                            if (!reader.IsDBNull(3))
                                entity.EntityTypeId = reader.GetValue(3).ToString();
                            else
                                entity.EntityTypeId = null;
                            if (!reader.IsDBNull(4))
                                entity.Private = reader.GetValue(4).ToString();
                            else
                                entity.Private = null;
                            if (!reader.IsDBNull(5))
                                entity.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                entity.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                entity.CreatedAt = reader.GetValue(6).ToString();
                            else
                                entity.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                entity.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                entity.UpdatedAt = null;
                            response.Add(entity);
                            entity = new EntityModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EntityModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EntityModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EntityModel> Create(string connectionString, EntityModel entity)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntityModel> response = new List<EntityModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO sch_entities (";
                string values = "(";
                if (!string.IsNullOrEmpty(entity.Id))
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
                    cmd.Parameters.AddWithValue("@id", entity.Id);
                }
                if (!string.IsNullOrEmpty(entity.Name))
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
                    cmd.Parameters.AddWithValue("@name", entity.Name);
                }
                if (!string.IsNullOrEmpty(entity.Token))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `token` ");
                        values = string.Concat(values, ", @token");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `token` ");
                        values = string.Concat(values, " @token");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@token", entity.Token);
                }
                if (!string.IsNullOrEmpty(entity.EntityTypeId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `entity_type_id` ");
                        values = string.Concat(values, ", @entityTypeId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `entity_type_id` ");
                        values = string.Concat(values, " @entityTypeId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@entityTypeId", entity.EntityTypeId);
                }
                if (!string.IsNullOrEmpty(entity.Private))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `private` ");
                        values = string.Concat(values, ", @private");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `private` ");
                        values = string.Concat(values, " @private");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@private", entity.Private);
                }
                if (!string.IsNullOrEmpty(entity.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", entity.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(entity.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", entity.CreatedAt);
                }
                if (!string.IsNullOrEmpty(entity.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", entity.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                entity.Id = cmd.LastInsertedId.ToString();
                response.Add(entity);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EntityModel>();
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
        public List<EntityModel> Create(string connectionString, List<EntityModel> entityList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntityModel> response = new List<EntityModel>();
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
                        for (int i = 0; i < entityList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO sch_entities (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(entityList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, entityList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(entityList[i].Name))
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
                                    cmd.Parameters.AddWithValue("@name" + i, entityList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(entityList[i].Token))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `token` ");
                                        values = string.Concat(values, ", @token" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `token` ");
                                        values = string.Concat(values, "@token" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@token" + i, entityList[i].Token);
                                }
                                if (!string.IsNullOrEmpty(entityList[i].EntityTypeId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `entity_type_id` ");
                                        values = string.Concat(values, ", @entityTypeId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `entity_type_id` ");
                                        values = string.Concat(values, "@entityTypeId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@entityTypeId" + i, entityList[i].EntityTypeId);
                                }
                                if (!string.IsNullOrEmpty(entityList[i].Private))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `private` ");
                                        values = string.Concat(values, ", @private" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `private` ");
                                        values = string.Concat(values, "@private" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@private" + i, entityList[i].Private);
                                }
                                if (!string.IsNullOrEmpty(entityList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, entityList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(entityList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, entityList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(entityList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, entityList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(entityList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<EntityModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EntityModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EntityModel>();
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

        public List<EntityModel> Update(string connectionString, EntityModel entity, EntityModel entityWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntityModel> response = new List<EntityModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE sch_entities SET ";
                if (!string.IsNullOrEmpty(entity.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", entity.Id);
                }
                if (!string.IsNullOrEmpty(entity.Name))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `name` = @name");
                    else
                        sql = string.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", entity.Name);
                }
                if (!string.IsNullOrEmpty(entity.Token))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `token` = @token");
                    else
                        sql = string.Concat(sql, " `token` = @token");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@token", entity.Token);
                }
                if (!string.IsNullOrEmpty(entity.EntityTypeId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `entity_type_id` = @entityTypeId");
                    else
                        sql = string.Concat(sql, " `entity_type_id` = @entityTypeId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@entityTypeId", entity.EntityTypeId);
                }
                if (!string.IsNullOrEmpty(entity.Private))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `private` = @private");
                    else
                        sql = string.Concat(sql, " `private` = @private");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@private", entity.Private);
                }
                if (!string.IsNullOrEmpty(entity.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", entity.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(entity.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", entity.CreatedAt);
                }
                if (!string.IsNullOrEmpty(entity.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", entity.UpdatedAt);
                }
                if (entityWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(entityWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", entityWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(entityWhere.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = string.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", entityWhere.Name);
                    }
                    if (!string.IsNullOrEmpty(entityWhere.Token))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `token` = @tokenWhere");
                        else
                            sql = string.Concat(sql, " `token` = @tokenWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@tokenWhere", entityWhere.Token);
                    }
                    if (!string.IsNullOrEmpty(entityWhere.EntityTypeId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `entity_type_id` = @entityTypeIdWhere");
                        else
                            sql = string.Concat(sql, " `entity_type_id` = @entityTypeIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@entityTypeIdWhere", entityWhere.EntityTypeId);
                    }
                    if (!string.IsNullOrEmpty(entityWhere.Private))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `private` = @privateWhere");
                        else
                            sql = string.Concat(sql, " `private` = @privateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@privateWhere", entityWhere.Private);
                    }
                    if (!string.IsNullOrEmpty(entityWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", entityWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(entityWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", entityWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(entityWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", entityWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(entity);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EntityModel>();
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

        public List<EntityModel> Update(string connectionString, List<EntityModel> entityList, List<EntityModel> entityWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntityModel> response = new List<EntityModel>();
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
                        for (int i = 0; i < entityList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE sch_entities SET ";
                                if (!string.IsNullOrEmpty(entityList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, entityList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(entityList[i].Name))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = string.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, entityList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(entityList[i].Token))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `token` = @token" + i);
                                    else
                                        sql = string.Concat(sql, " `token` = @token" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@token" + i, entityList[i].Token);
                                }
                                if (!string.IsNullOrEmpty(entityList[i].EntityTypeId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `entity_type_id` = @entityTypeId" + i);
                                    else
                                        sql = string.Concat(sql, " `entity_type_id` = @entityTypeId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@entityTypeId" + i, entityList[i].EntityTypeId);
                                }
                                if (!string.IsNullOrEmpty(entityList[i].Private))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `private` = @private" + i);
                                    else
                                        sql = string.Concat(sql, " `private` = @private" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@private" + i, entityList[i].Private);
                                }
                                if (!string.IsNullOrEmpty(entityList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, entityList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(entityList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, entityList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(entityList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, entityList[i].UpdatedAt);
                                }
                                if (entityWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(entityWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, entityWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(entityWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, entityWhereList[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(entityWhereList[i].Token))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `token` = @tokenWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `token` = @tokenWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@tokenWhere" + i, entityWhereList[i].Token);
                                    }
                                    if (!string.IsNullOrEmpty(entityWhereList[i].EntityTypeId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `entity_type_id` = @entityTypeIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `entity_type_id` = @entityTypeIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@entityTypeIdWhere" + i, entityWhereList[i].EntityTypeId);
                                    }
                                    if (!string.IsNullOrEmpty(entityWhereList[i].Private))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `private` = @privateWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `private` = @privateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@privateWhere" + i, entityWhereList[i].Private);
                                    }
                                    if (!string.IsNullOrEmpty(entityWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, entityWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(entityWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, entityWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(entityWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, entityWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(entityList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EntityModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EntityModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EntityModel>();
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

        public List<EntityModel> Delete(string connectionString, EntityModel entity)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntityModel> response = new List<EntityModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM sch_entities  ";
                if (entity != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(entity.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", entity.Id);
                    }
                    if (!string.IsNullOrEmpty(entity.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @name");
                        else
                            sql = string.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", entity.Name);
                    }
                    if (!string.IsNullOrEmpty(entity.Token))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `token` = @token");
                        else
                            sql = string.Concat(sql, " `token` = @token");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@token", entity.Token);
                    }
                    if (!string.IsNullOrEmpty(entity.EntityTypeId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `entity_type_id` = @entityTypeId");
                        else
                            sql = string.Concat(sql, " `entity_type_id` = @entityTypeId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@entityTypeId", entity.EntityTypeId);
                    }
                    if (!string.IsNullOrEmpty(entity.Private))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `private` = @private");
                        else
                            sql = string.Concat(sql, " `private` = @private");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@private", entity.Private);
                    }
                    if (!string.IsNullOrEmpty(entity.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", entity.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(entity.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", entity.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(entity.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", entity.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(entity);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EntityModel>();
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

        public List<EntityModel> Delete(string connectionString, List<EntityModel> entity)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EntityModel> response = new List<EntityModel>();
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
                        for (int i = 0; i < entity.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM sch_entities  ";
                                if (entity[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(entity[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, entity[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(entity[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, entity[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(entity[i].Token))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `token` = @token" + i);
                                        else
                                            sql = string.Concat(sql, " `token` = @token" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@token" + i, entity[i].Token);
                                    }
                                    if (!string.IsNullOrEmpty(entity[i].EntityTypeId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `entity_type_id` = @entityTypeId" + i);
                                        else
                                            sql = string.Concat(sql, " `entity_type_id` = @entityTypeId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@entityTypeId" + i, entity[i].EntityTypeId);
                                    }
                                    if (!string.IsNullOrEmpty(entity[i].Private))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `private` = @private" + i);
                                        else
                                            sql = string.Concat(sql, " `private` = @private" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@private" + i, entity[i].Private);
                                    }
                                    if (!string.IsNullOrEmpty(entity[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, entity[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(entity[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, entity[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(entity[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, entity[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(entity[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EntityModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EntityModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EntityModel>();
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
