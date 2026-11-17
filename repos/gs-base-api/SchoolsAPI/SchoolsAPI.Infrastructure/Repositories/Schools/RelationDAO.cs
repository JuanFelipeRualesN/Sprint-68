using System;
using System.Collections.Generic;
using MySqlConnector;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;

namespace SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools
{
    class RelationDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, RelationModel> ReadD(string connectionString, RelationModel relation, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, RelationModel> response = new Dictionary<string, RelationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `relation_entity_id`, `type_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_relations ";
                {
                    if (relation != null)
                    {
                        if (!string.IsNullOrEmpty(relation.Id))
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
                            cmd.Parameters.AddWithValue("@id", relation.Id);
                        }
                        if (!string.IsNullOrEmpty(relation.EntityId))
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
                            cmd.Parameters.AddWithValue("@entityId", relation.EntityId);
                        }
                        if (!string.IsNullOrEmpty(relation.RelationEntityId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `relation_entity_id` = @relationEntityId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `relation_entity_id` = @relationEntityId ");
                            }
                            cmd.Parameters.AddWithValue("@relationEntityId", relation.RelationEntityId);
                        }
                        if (!string.IsNullOrEmpty(relation.TypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `type_id` = @typeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `type_id` = @typeId ");
                            }
                            cmd.Parameters.AddWithValue("@typeId", relation.TypeId);
                        }
                        if (!string.IsNullOrEmpty(relation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", relation.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(relation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", relation.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(relation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", relation.UpdatedAt);
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
                                    relation.Id = reader.GetValue(0).ToString();
                                else
                                    relation.Id = null;
                                if (!reader.IsDBNull(1))
                                    relation.EntityId = reader.GetValue(1).ToString();
                                else
                                    relation.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    relation.RelationEntityId = reader.GetValue(2).ToString();
                                else
                                    relation.RelationEntityId = null;
                                if (!reader.IsDBNull(3))
                                    relation.TypeId = reader.GetValue(3).ToString();
                                else
                                    relation.TypeId = null;
                                if (!reader.IsDBNull(4))
                                    relation.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    relation.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    relation.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    relation.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    relation.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    relation.UpdatedAt = null;
                                response.Add(relation.Id, relation);
                                relation = new RelationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, RelationModel>();
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
                response = new Dictionary<string, RelationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, RelationModel> ReadD(string connectionString, RelationModel relation, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, RelationModel> response = new Dictionary<string, RelationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `relation_entity_id`, `type_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_relations ";
                {
                    if (relation != null)
                    {
                        if (!string.IsNullOrEmpty(relation.Id))
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
                            cmd.Parameters.AddWithValue("@id", relation.Id);
                        }
                        if (!string.IsNullOrEmpty(relation.EntityId))
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
                            cmd.Parameters.AddWithValue("@entityId", relation.EntityId);
                        }
                        if (!string.IsNullOrEmpty(relation.RelationEntityId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `relation_entity_id` = @relationEntityId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `relation_entity_id` = @relationEntityId ");
                            }
                            cmd.Parameters.AddWithValue("@relationEntityId", relation.RelationEntityId);
                        }
                        if (!string.IsNullOrEmpty(relation.TypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `type_id` = @typeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `type_id` = @typeId ");
                            }
                            cmd.Parameters.AddWithValue("@typeId", relation.TypeId);
                        }
                        if (!string.IsNullOrEmpty(relation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", relation.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(relation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", relation.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(relation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", relation.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                        cmd.Parameters.AddWithValue("@fromRow", fromRow);
                        cmd.Parameters.AddWithValue("@limit", limit);
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    relation.Id = reader.GetValue(0).ToString();
                                else
                                    relation.Id = null;
                                if (!reader.IsDBNull(1))
                                    relation.EntityId = reader.GetValue(1).ToString();
                                else
                                    relation.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    relation.RelationEntityId = reader.GetValue(2).ToString();
                                else
                                    relation.RelationEntityId = null;
                                if (!reader.IsDBNull(3))
                                    relation.TypeId = reader.GetValue(3).ToString();
                                else
                                    relation.TypeId = null;
                                if (!reader.IsDBNull(4))
                                    relation.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    relation.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    relation.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    relation.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    relation.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    relation.UpdatedAt = null;
                                response.Add(relation.Id, relation);
                                relation = new RelationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, RelationModel>();
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
                response = new Dictionary<string, RelationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, RelationModel> ReadD(string connectionString, List<RelationModel> relationWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, RelationModel> response = new Dictionary<string, RelationModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `relation_entity_id`, `type_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_relations ";
                if (relationWhere != null && relationWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < relationWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(relationWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, relationWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, relationWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].RelationEntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `relation_entity_id` = @relationEntityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@relationEntityId" + i, relationWhere[i].RelationEntityId);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].TypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `type_id` = @typeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@typeId" + i, relationWhere[i].TypeId);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, relationWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, relationWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, relationWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != relationWhere.Count - 1)
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
                            RelationModel relation = new RelationModel(); if (!reader.IsDBNull(0))
                                relation.Id = reader.GetValue(0).ToString();
                            else
                                relation.Id = null;
                            if (!reader.IsDBNull(1))
                                relation.EntityId = reader.GetValue(1).ToString();
                            else
                                relation.EntityId = null;
                            if (!reader.IsDBNull(2))
                                relation.RelationEntityId = reader.GetValue(2).ToString();
                            else
                                relation.RelationEntityId = null;
                            if (!reader.IsDBNull(3))
                                relation.TypeId = reader.GetValue(3).ToString();
                            else
                                relation.TypeId = null;
                            if (!reader.IsDBNull(4))
                                relation.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                relation.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                relation.CreatedAt = reader.GetValue(5).ToString();
                            else
                                relation.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                relation.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                relation.UpdatedAt = null;
                            response.Add(relation.Id, relation);
                            relation = new RelationModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, RelationModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, RelationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, RelationModel> ReadD(string connectionString, List<RelationModel> relationWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, RelationModel> response = new Dictionary<string, RelationModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `relation_entity_id`, `type_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_relations ";
                if (relationWhere != null && relationWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < relationWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(relationWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, relationWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, relationWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].RelationEntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `relation_entity_id` = @relationEntityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@relationEntityId" + i, relationWhere[i].RelationEntityId);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].TypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `type_id` = @typeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@typeId" + i, relationWhere[i].TypeId);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, relationWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, relationWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, relationWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != relationWhere.Count - 1)
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
                            RelationModel relation = new RelationModel(); if (!reader.IsDBNull(0))
                                relation.Id = reader.GetValue(0).ToString();
                            else
                                relation.Id = null;
                            if (!reader.IsDBNull(1))
                                relation.EntityId = reader.GetValue(1).ToString();
                            else
                                relation.EntityId = null;
                            if (!reader.IsDBNull(2))
                                relation.RelationEntityId = reader.GetValue(2).ToString();
                            else
                                relation.RelationEntityId = null;
                            if (!reader.IsDBNull(3))
                                relation.TypeId = reader.GetValue(3).ToString();
                            else
                                relation.TypeId = null;
                            if (!reader.IsDBNull(4))
                                relation.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                relation.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                relation.CreatedAt = reader.GetValue(5).ToString();
                            else
                                relation.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                relation.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                relation.UpdatedAt = null;
                            response.Add(relation.Id, relation);
                            relation = new RelationModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, RelationModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, RelationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RelationModel> Read(string connectionString, RelationModel relation, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RelationModel> response = new List<RelationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `relation_entity_id`, `type_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_relations ";
                {
                    if (relation != null)
                    {
                        if (!string.IsNullOrEmpty(relation.Id))
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
                            cmd.Parameters.AddWithValue("@id", relation.Id);
                        }
                        if (!string.IsNullOrEmpty(relation.EntityId))
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
                            cmd.Parameters.AddWithValue("@entityId", relation.EntityId);
                        }
                        if (!string.IsNullOrEmpty(relation.RelationEntityId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `relation_entity_id` = @relationEntityId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `relation_entity_id` = @relationEntityId ");
                            }
                            cmd.Parameters.AddWithValue("@relationEntityId", relation.RelationEntityId);
                        }
                        if (!string.IsNullOrEmpty(relation.TypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `type_id` = @typeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `type_id` = @typeId ");
                            }
                            cmd.Parameters.AddWithValue("@typeId", relation.TypeId);
                        }
                        if (!string.IsNullOrEmpty(relation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", relation.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(relation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", relation.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(relation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", relation.UpdatedAt);
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
                                    relation.Id = reader.GetValue(0).ToString();
                                else
                                    relation.Id = null;
                                if (!reader.IsDBNull(1))
                                    relation.EntityId = reader.GetValue(1).ToString();
                                else
                                    relation.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    relation.RelationEntityId = reader.GetValue(2).ToString();
                                else
                                    relation.RelationEntityId = null;
                                if (!reader.IsDBNull(3))
                                    relation.TypeId = reader.GetValue(3).ToString();
                                else
                                    relation.TypeId = null;
                                if (!reader.IsDBNull(4))
                                    relation.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    relation.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    relation.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    relation.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    relation.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    relation.UpdatedAt = null;
                                response.Add(relation);
                                relation = new RelationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<RelationModel>();
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
                response = new List<RelationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RelationModel> Read(string connectionString, RelationModel relation, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RelationModel> response = new List<RelationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `relation_entity_id`, `type_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_relations ";
                {
                    if (relation != null)
                    {
                        if (!string.IsNullOrEmpty(relation.Id))
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
                            cmd.Parameters.AddWithValue("@id", relation.Id);
                        }
                        if (!string.IsNullOrEmpty(relation.EntityId))
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
                            cmd.Parameters.AddWithValue("@entityId", relation.EntityId);
                        }
                        if (!string.IsNullOrEmpty(relation.RelationEntityId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `relation_entity_id` = @relationEntityId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `relation_entity_id` = @relationEntityId ");
                            }
                            cmd.Parameters.AddWithValue("@relationEntityId", relation.RelationEntityId);
                        }
                        if (!string.IsNullOrEmpty(relation.TypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `type_id` = @typeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `type_id` = @typeId ");
                            }
                            cmd.Parameters.AddWithValue("@typeId", relation.TypeId);
                        }
                        if (!string.IsNullOrEmpty(relation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", relation.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(relation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", relation.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(relation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", relation.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                        cmd.Parameters.AddWithValue("@fromRow", fromRow);
                        cmd.Parameters.AddWithValue("@limit", limit);
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    relation.Id = reader.GetValue(0).ToString();
                                else
                                    relation.Id = null;
                                if (!reader.IsDBNull(1))
                                    relation.EntityId = reader.GetValue(1).ToString();
                                else
                                    relation.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    relation.RelationEntityId = reader.GetValue(2).ToString();
                                else
                                    relation.RelationEntityId = null;
                                if (!reader.IsDBNull(3))
                                    relation.TypeId = reader.GetValue(3).ToString();
                                else
                                    relation.TypeId = null;
                                if (!reader.IsDBNull(4))
                                    relation.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    relation.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    relation.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    relation.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    relation.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    relation.UpdatedAt = null;
                                response.Add(relation);
                                relation = new RelationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<RelationModel>();
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
                response = new List<RelationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RelationModel> Read(string connectionString, List<RelationModel> relationWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RelationModel> response = new List<RelationModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `relation_entity_id`, `type_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_relations ";
                if (relationWhere != null && relationWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < relationWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(relationWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, relationWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, relationWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].RelationEntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `relation_entity_id` = @relationEntityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@relationEntityId" + i, relationWhere[i].RelationEntityId);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].TypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `type_id` = @typeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@typeId" + i, relationWhere[i].TypeId);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, relationWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, relationWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, relationWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != relationWhere.Count - 1)
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
                            RelationModel relation = new RelationModel(); if (!reader.IsDBNull(0))
                                relation.Id = reader.GetValue(0).ToString();
                            else
                                relation.Id = null;
                            if (!reader.IsDBNull(1))
                                relation.EntityId = reader.GetValue(1).ToString();
                            else
                                relation.EntityId = null;
                            if (!reader.IsDBNull(2))
                                relation.RelationEntityId = reader.GetValue(2).ToString();
                            else
                                relation.RelationEntityId = null;
                            if (!reader.IsDBNull(3))
                                relation.TypeId = reader.GetValue(3).ToString();
                            else
                                relation.TypeId = null;
                            if (!reader.IsDBNull(4))
                                relation.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                relation.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                relation.CreatedAt = reader.GetValue(5).ToString();
                            else
                                relation.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                relation.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                relation.UpdatedAt = null;
                            response.Add(relation);
                            relation = new RelationModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<RelationModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RelationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RelationModel> Read(string connectionString, List<RelationModel> relationWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RelationModel> response = new List<RelationModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `relation_entity_id`, `type_id`, `modified_by`, `created_at`, `updated_at`  FROM sch_relations ";
                if (relationWhere != null && relationWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < relationWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(relationWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, relationWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, relationWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].RelationEntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `relation_entity_id` = @relationEntityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@relationEntityId" + i, relationWhere[i].RelationEntityId);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].TypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `type_id` = @typeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@typeId" + i, relationWhere[i].TypeId);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, relationWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, relationWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(relationWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, relationWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != relationWhere.Count - 1)
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
                            RelationModel relation = new RelationModel(); if (!reader.IsDBNull(0))
                                relation.Id = reader.GetValue(0).ToString();
                            else
                                relation.Id = null;
                            if (!reader.IsDBNull(1))
                                relation.EntityId = reader.GetValue(1).ToString();
                            else
                                relation.EntityId = null;
                            if (!reader.IsDBNull(2))
                                relation.RelationEntityId = reader.GetValue(2).ToString();
                            else
                                relation.RelationEntityId = null;
                            if (!reader.IsDBNull(3))
                                relation.TypeId = reader.GetValue(3).ToString();
                            else
                                relation.TypeId = null;
                            if (!reader.IsDBNull(4))
                                relation.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                relation.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                relation.CreatedAt = reader.GetValue(5).ToString();
                            else
                                relation.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                relation.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                relation.UpdatedAt = null;
                            response.Add(relation);
                            relation = new RelationModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<RelationModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RelationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RelationModel> Create(string connectionString, RelationModel relation)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RelationModel> response = new List<RelationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO sch_relations (";
                string values = "(";
                if (!string.IsNullOrEmpty(relation.Id))
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
                    cmd.Parameters.AddWithValue("@id", relation.Id);
                }
                if (!string.IsNullOrEmpty(relation.EntityId))
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
                    cmd.Parameters.AddWithValue("@entityId", relation.EntityId);
                }
                if (!string.IsNullOrEmpty(relation.RelationEntityId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `relation_entity_id` ");
                        values = string.Concat(values, ", @relationEntityId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `relation_entity_id` ");
                        values = string.Concat(values, " @relationEntityId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@relationEntityId", relation.RelationEntityId);
                }
                if (!string.IsNullOrEmpty(relation.TypeId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `type_id` ");
                        values = string.Concat(values, ", @typeId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `type_id` ");
                        values = string.Concat(values, " @typeId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@typeId", relation.TypeId);
                }
                if (!string.IsNullOrEmpty(relation.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", relation.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(relation.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", relation.CreatedAt);
                }
                if (!string.IsNullOrEmpty(relation.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", relation.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                relation.Id = cmd.LastInsertedId.ToString();
                response.Add(relation);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RelationModel>();
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
        public List<RelationModel> Create(string connectionString, List<RelationModel> relationList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RelationModel> response = new List<RelationModel>();
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
                        for (int i = 0; i < relationList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO sch_relations (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(relationList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, relationList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(relationList[i].EntityId))
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
                                    cmd.Parameters.AddWithValue("@entityId" + i, relationList[i].EntityId);
                                }
                                if (!string.IsNullOrEmpty(relationList[i].RelationEntityId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `relation_entity_id` ");
                                        values = string.Concat(values, ", @relationEntityId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `relation_entity_id` ");
                                        values = string.Concat(values, "@relationEntityId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@relationEntityId" + i, relationList[i].RelationEntityId);
                                }
                                if (!string.IsNullOrEmpty(relationList[i].TypeId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `type_id` ");
                                        values = string.Concat(values, ", @typeId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `type_id` ");
                                        values = string.Concat(values, "@typeId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@typeId" + i, relationList[i].TypeId);
                                }
                                if (!string.IsNullOrEmpty(relationList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, relationList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(relationList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, relationList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(relationList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, relationList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(relationList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<RelationModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RelationModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RelationModel>();
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

        public List<RelationModel> Update(string connectionString, RelationModel relation, RelationModel relationWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RelationModel> response = new List<RelationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE sch_relations SET ";
                if (!string.IsNullOrEmpty(relation.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", relation.Id);
                }
                if (!string.IsNullOrEmpty(relation.EntityId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `entity_id` = @entityId");
                    else
                        sql = string.Concat(sql, " `entity_id` = @entityId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@entityId", relation.EntityId);
                }
                if (!string.IsNullOrEmpty(relation.RelationEntityId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `relation_entity_id` = @relationEntityId");
                    else
                        sql = string.Concat(sql, " `relation_entity_id` = @relationEntityId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@relationEntityId", relation.RelationEntityId);
                }
                if (!string.IsNullOrEmpty(relation.TypeId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `type_id` = @typeId");
                    else
                        sql = string.Concat(sql, " `type_id` = @typeId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@typeId", relation.TypeId);
                }
                if (!string.IsNullOrEmpty(relation.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", relation.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(relation.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", relation.CreatedAt);
                }
                if (!string.IsNullOrEmpty(relation.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", relation.UpdatedAt);
                }
                if (relationWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(relationWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", relationWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(relationWhere.EntityId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `entity_id` = @entityIdWhere");
                        else
                            sql = string.Concat(sql, " `entity_id` = @entityIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@entityIdWhere", relationWhere.EntityId);
                    }
                    if (!string.IsNullOrEmpty(relationWhere.RelationEntityId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `relation_entity_id` = @relationEntityIdWhere");
                        else
                            sql = string.Concat(sql, " `relation_entity_id` = @relationEntityIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@relationEntityIdWhere", relationWhere.RelationEntityId);
                    }
                    if (!string.IsNullOrEmpty(relationWhere.TypeId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `type_id` = @typeIdWhere");
                        else
                            sql = string.Concat(sql, " `type_id` = @typeIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@typeIdWhere", relationWhere.TypeId);
                    }
                    if (!string.IsNullOrEmpty(relationWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", relationWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(relationWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", relationWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(relationWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", relationWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(relation);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<RelationModel>();
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

        public List<RelationModel> Update(string connectionString, List<RelationModel> relationList, List<RelationModel> relationWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RelationModel> response = new List<RelationModel>();
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
                        for (int i = 0; i < relationList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE sch_relations SET ";
                                if (!string.IsNullOrEmpty(relationList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, relationList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(relationList[i].EntityId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `entity_id` = @entityId" + i);
                                    else
                                        sql = string.Concat(sql, " `entity_id` = @entityId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@entityId" + i, relationList[i].EntityId);
                                }
                                if (!string.IsNullOrEmpty(relationList[i].RelationEntityId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `relation_entity_id` = @relationEntityId" + i);
                                    else
                                        sql = string.Concat(sql, " `relation_entity_id` = @relationEntityId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@relationEntityId" + i, relationList[i].RelationEntityId);
                                }
                                if (!string.IsNullOrEmpty(relationList[i].TypeId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `type_id` = @typeId" + i);
                                    else
                                        sql = string.Concat(sql, " `type_id` = @typeId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@typeId" + i, relationList[i].TypeId);
                                }
                                if (!string.IsNullOrEmpty(relationList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, relationList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(relationList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, relationList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(relationList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, relationList[i].UpdatedAt);
                                }
                                if (relationWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(relationWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, relationWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(relationWhereList[i].EntityId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `entity_id` = @entityIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `entity_id` = @entityIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@entityIdWhere" + i, relationWhereList[i].EntityId);
                                    }
                                    if (!string.IsNullOrEmpty(relationWhereList[i].RelationEntityId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `relation_entity_id` = @relationEntityIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `relation_entity_id` = @relationEntityIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@relationEntityIdWhere" + i, relationWhereList[i].RelationEntityId);
                                    }
                                    if (!string.IsNullOrEmpty(relationWhereList[i].TypeId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `type_id` = @typeIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `type_id` = @typeIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@typeIdWhere" + i, relationWhereList[i].TypeId);
                                    }
                                    if (!string.IsNullOrEmpty(relationWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, relationWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(relationWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, relationWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(relationWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, relationWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(relationList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<RelationModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RelationModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RelationModel>();
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

        public List<RelationModel> Delete(string connectionString, RelationModel relation)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RelationModel> response = new List<RelationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM sch_relations  ";
                if (relation != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(relation.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", relation.Id);
                    }
                    if (!string.IsNullOrEmpty(relation.EntityId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `entity_id` = @entityId");
                        else
                            sql = string.Concat(sql, " `entity_id` = @entityId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@entityId", relation.EntityId);
                    }
                    if (!string.IsNullOrEmpty(relation.RelationEntityId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `relation_entity_id` = @relationEntityId");
                        else
                            sql = string.Concat(sql, " `relation_entity_id` = @relationEntityId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@relationEntityId", relation.RelationEntityId);
                    }
                    if (!string.IsNullOrEmpty(relation.TypeId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `type_id` = @typeId");
                        else
                            sql = string.Concat(sql, " `type_id` = @typeId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@typeId", relation.TypeId);
                    }
                    if (!string.IsNullOrEmpty(relation.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", relation.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(relation.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", relation.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(relation.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", relation.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(relation);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<RelationModel>();
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

        public List<RelationModel> Delete(string connectionString, List<RelationModel> relation)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RelationModel> response = new List<RelationModel>();
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
                        for (int i = 0; i < relation.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM sch_relations  ";
                                if (relation[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(relation[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, relation[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(relation[i].EntityId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `entity_id` = @entityId" + i);
                                        else
                                            sql = string.Concat(sql, " `entity_id` = @entityId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@entityId" + i, relation[i].EntityId);
                                    }
                                    if (!string.IsNullOrEmpty(relation[i].RelationEntityId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `relation_entity_id` = @relationEntityId" + i);
                                        else
                                            sql = string.Concat(sql, " `relation_entity_id` = @relationEntityId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@relationEntityId" + i, relation[i].RelationEntityId);
                                    }
                                    if (!string.IsNullOrEmpty(relation[i].TypeId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `type_id` = @typeId" + i);
                                        else
                                            sql = string.Concat(sql, " `type_id` = @typeId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@typeId" + i, relation[i].TypeId);
                                    }
                                    if (!string.IsNullOrEmpty(relation[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, relation[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(relation[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, relation[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(relation[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, relation[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(relation[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<RelationModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RelationModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RelationModel>();
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
