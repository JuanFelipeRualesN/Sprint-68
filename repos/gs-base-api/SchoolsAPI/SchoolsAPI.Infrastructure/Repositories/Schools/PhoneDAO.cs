using System;
using System.Collections.Generic;
using MySqlConnector;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;

namespace SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools
{
    class PhoneDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, PhoneModel> ReadD(string connectionString, PhoneModel phone, bool firstOne, bool orderByLastDate, PhoneModel phoneWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PhoneModel> response = new Dictionary<string, PhoneModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `phone`, `modified_by`, `created_at`, `updated_at`  FROM sch_phones ";
                {
                    if (phone != null)
                    {
                        if (!string.IsNullOrEmpty(phone.Id))
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
                            cmd.Parameters.AddWithValue("@id", phone.Id);
                        }
                        if (!string.IsNullOrEmpty(phone.EntityId))
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
                            cmd.Parameters.AddWithValue("@entityId", phone.EntityId);
                        }
                        if (!string.IsNullOrEmpty(phone.Phone))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `phone` = @phone ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `phone` = @phone ");
                            }
                            cmd.Parameters.AddWithValue("@phone", phone.Phone);
                        }
                        if (!string.IsNullOrEmpty(phone.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", phone.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(phone.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", phone.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(phone.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", phone.UpdatedAt);
                        }
                    }
                    if (phoneWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(phoneWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + phoneWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.EntityId))
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
                            cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + phoneWhereLike.EntityId + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.Phone))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `phone` LIKE @phoneWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `phone` LIKE @phoneWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@phoneWhereLike", "%" + phoneWhereLike.Phone + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + phoneWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + phoneWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + phoneWhereLike.UpdatedAt + "%");
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
                                phone = new PhoneModel();
                                if (!reader.IsDBNull(0))
                                    phone.Id = reader.GetValue(0).ToString();
                                else
                                    phone.Id = null;
                                if (!reader.IsDBNull(1))
                                    phone.EntityId = reader.GetValue(1).ToString();
                                else
                                    phone.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    phone.Phone = reader.GetValue(2).ToString();
                                else
                                    phone.Phone = null;
                                if (!reader.IsDBNull(3))
                                    phone.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    phone.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    phone.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    phone.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    phone.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    phone.UpdatedAt = null;
                                response.Add(phone.Id, phone);
                                phone = new PhoneModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PhoneModel>();
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
                response = new Dictionary<string, PhoneModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PhoneModel> ReadD(string connectionString, PhoneModel phone, bool orderByLastDate, int fromRow, int limit, PhoneModel phoneWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PhoneModel> response = new Dictionary<string, PhoneModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `phone`, `modified_by`, `created_at`, `updated_at`  FROM sch_phones ";
                {
                    if (phone != null)
                    {
                        if (!string.IsNullOrEmpty(phone.Id))
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
                            cmd.Parameters.AddWithValue("@id", phone.Id);
                        }
                        if (!string.IsNullOrEmpty(phone.EntityId))
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
                            cmd.Parameters.AddWithValue("@entityId", phone.EntityId);
                        }
                        if (!string.IsNullOrEmpty(phone.Phone))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `phone` = @phone ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `phone` = @phone ");
                            }
                            cmd.Parameters.AddWithValue("@phone", phone.Phone);
                        }
                        if (!string.IsNullOrEmpty(phone.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", phone.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(phone.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", phone.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(phone.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", phone.UpdatedAt);
                        }
                    }
                    if (phoneWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(phoneWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + phoneWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.EntityId))
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
                            cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + phoneWhereLike.EntityId + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.Phone))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `phone` LIKE @phoneWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `phone` LIKE @phoneWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@phoneWhereLike", "%" + phoneWhereLike.Phone + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + phoneWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + phoneWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + phoneWhereLike.UpdatedAt + "%");
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
                                phone = new PhoneModel();
                                if (!reader.IsDBNull(0))
                                    phone.Id = reader.GetValue(0).ToString();
                                else
                                    phone.Id = null;
                                if (!reader.IsDBNull(1))
                                    phone.EntityId = reader.GetValue(1).ToString();
                                else
                                    phone.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    phone.Phone = reader.GetValue(2).ToString();
                                else
                                    phone.Phone = null;
                                if (!reader.IsDBNull(3))
                                    phone.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    phone.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    phone.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    phone.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    phone.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    phone.UpdatedAt = null;
                                response.Add(phone.Id, phone);
                                phone = new PhoneModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PhoneModel>();
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
                response = new Dictionary<string, PhoneModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PhoneModel> ReadD(string connectionString, List<PhoneModel> phoneWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PhoneModel> response = new Dictionary<string, PhoneModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `phone`, `modified_by`, `created_at`, `updated_at`  FROM sch_phones ";
                if (phoneWhere != null && phoneWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < phoneWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(phoneWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, phoneWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, phoneWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].Phone))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `phone` = @phone" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@phone" + i, phoneWhere[i].Phone);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, phoneWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, phoneWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, phoneWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != phoneWhere.Count - 1)
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
                            PhoneModel phone = new PhoneModel(); if (!reader.IsDBNull(0))
                                phone.Id = reader.GetValue(0).ToString();
                            else
                                phone.Id = null;
                            if (!reader.IsDBNull(1))
                                phone.EntityId = reader.GetValue(1).ToString();
                            else
                                phone.EntityId = null;
                            if (!reader.IsDBNull(2))
                                phone.Phone = reader.GetValue(2).ToString();
                            else
                                phone.Phone = null;
                            if (!reader.IsDBNull(3))
                                phone.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                phone.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                phone.CreatedAt = reader.GetValue(4).ToString();
                            else
                                phone.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                phone.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                phone.UpdatedAt = null;
                            response.Add(phone.Id, phone);
                            phone = new PhoneModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PhoneModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PhoneModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PhoneModel> ReadD(string connectionString, List<PhoneModel> phoneWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PhoneModel> response = new Dictionary<string, PhoneModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `phone`, `modified_by`, `created_at`, `updated_at`  FROM sch_phones ";
                if (phoneWhere != null && phoneWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < phoneWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(phoneWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, phoneWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, phoneWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].Phone))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `phone` = @phone" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@phone" + i, phoneWhere[i].Phone);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, phoneWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, phoneWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, phoneWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != phoneWhere.Count - 1)
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
                            PhoneModel phone = new PhoneModel(); if (!reader.IsDBNull(0))
                                phone.Id = reader.GetValue(0).ToString();
                            else
                                phone.Id = null;
                            if (!reader.IsDBNull(1))
                                phone.EntityId = reader.GetValue(1).ToString();
                            else
                                phone.EntityId = null;
                            if (!reader.IsDBNull(2))
                                phone.Phone = reader.GetValue(2).ToString();
                            else
                                phone.Phone = null;
                            if (!reader.IsDBNull(3))
                                phone.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                phone.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                phone.CreatedAt = reader.GetValue(4).ToString();
                            else
                                phone.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                phone.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                phone.UpdatedAt = null;
                            response.Add(phone.Id, phone);
                            phone = new PhoneModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PhoneModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PhoneModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PhoneModel> Read(string connectionString, PhoneModel phone, bool firstOne, bool orderByLastDate, PhoneModel phoneWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PhoneModel> response = new List<PhoneModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `phone`, `modified_by`, `created_at`, `updated_at`  FROM sch_phones ";
                {
                    if (phone != null)
                    {
                        if (!string.IsNullOrEmpty(phone.Id))
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
                            cmd.Parameters.AddWithValue("@id", phone.Id);
                        }
                        if (!string.IsNullOrEmpty(phone.EntityId))
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
                            cmd.Parameters.AddWithValue("@entityId", phone.EntityId);
                        }
                        if (!string.IsNullOrEmpty(phone.Phone))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `phone` = @phone ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `phone` = @phone ");
                            }
                            cmd.Parameters.AddWithValue("@phone", phone.Phone);
                        }
                        if (!string.IsNullOrEmpty(phone.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", phone.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(phone.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", phone.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(phone.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", phone.UpdatedAt);
                        }
                    }
                    if (phoneWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(phoneWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + phoneWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.EntityId))
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
                            cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + phoneWhereLike.EntityId + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.Phone))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `phone` LIKE @phoneWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `phone` LIKE @phoneWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@phoneWhereLike", "%" + phoneWhereLike.Phone + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + phoneWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + phoneWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + phoneWhereLike.UpdatedAt + "%");
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
                                phone = new PhoneModel();
                                if (!reader.IsDBNull(0))
                                    phone.Id = reader.GetValue(0).ToString();
                                else
                                    phone.Id = null;
                                if (!reader.IsDBNull(1))
                                    phone.EntityId = reader.GetValue(1).ToString();
                                else
                                    phone.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    phone.Phone = reader.GetValue(2).ToString();
                                else
                                    phone.Phone = null;
                                if (!reader.IsDBNull(3))
                                    phone.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    phone.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    phone.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    phone.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    phone.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    phone.UpdatedAt = null;
                                response.Add(phone);
                                phone = new PhoneModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PhoneModel>();
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
                response = new List<PhoneModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PhoneModel> Read(string connectionString, PhoneModel phone, bool orderByLastDate, int fromRow, int limit, PhoneModel phoneWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PhoneModel> response = new List<PhoneModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `phone`, `modified_by`, `created_at`, `updated_at`  FROM sch_phones ";
                {
                    if (phone != null)
                    {
                        if (!string.IsNullOrEmpty(phone.Id))
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
                            cmd.Parameters.AddWithValue("@id", phone.Id);
                        }
                        if (!string.IsNullOrEmpty(phone.EntityId))
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
                            cmd.Parameters.AddWithValue("@entityId", phone.EntityId);
                        }
                        if (!string.IsNullOrEmpty(phone.Phone))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `phone` = @phone ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `phone` = @phone ");
                            }
                            cmd.Parameters.AddWithValue("@phone", phone.Phone);
                        }
                        if (!string.IsNullOrEmpty(phone.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", phone.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(phone.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", phone.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(phone.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", phone.UpdatedAt);
                        }
                    }
                    if (phoneWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(phoneWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + phoneWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.EntityId))
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
                            cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + phoneWhereLike.EntityId + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.Phone))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `phone` LIKE @phoneWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `phone` LIKE @phoneWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@phoneWhereLike", "%" + phoneWhereLike.Phone + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + phoneWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + phoneWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(phoneWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + phoneWhereLike.UpdatedAt + "%");
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
                                phone = new PhoneModel();
                                if (!reader.IsDBNull(0))
                                    phone.Id = reader.GetValue(0).ToString();
                                else
                                    phone.Id = null;
                                if (!reader.IsDBNull(1))
                                    phone.EntityId = reader.GetValue(1).ToString();
                                else
                                    phone.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    phone.Phone = reader.GetValue(2).ToString();
                                else
                                    phone.Phone = null;
                                if (!reader.IsDBNull(3))
                                    phone.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    phone.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    phone.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    phone.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    phone.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    phone.UpdatedAt = null;
                                response.Add(phone);
                                phone = new PhoneModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PhoneModel>();
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
                response = new List<PhoneModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PhoneModel> Read(string connectionString, List<PhoneModel> phoneWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PhoneModel> response = new List<PhoneModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `phone`, `modified_by`, `created_at`, `updated_at`  FROM sch_phones ";
                if (phoneWhere != null && phoneWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < phoneWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(phoneWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, phoneWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, phoneWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].Phone))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `phone` = @phone" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@phone" + i, phoneWhere[i].Phone);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, phoneWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, phoneWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, phoneWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != phoneWhere.Count - 1)
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
                            PhoneModel phone = new PhoneModel(); if (!reader.IsDBNull(0))
                                phone.Id = reader.GetValue(0).ToString();
                            else
                                phone.Id = null;
                            if (!reader.IsDBNull(1))
                                phone.EntityId = reader.GetValue(1).ToString();
                            else
                                phone.EntityId = null;
                            if (!reader.IsDBNull(2))
                                phone.Phone = reader.GetValue(2).ToString();
                            else
                                phone.Phone = null;
                            if (!reader.IsDBNull(3))
                                phone.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                phone.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                phone.CreatedAt = reader.GetValue(4).ToString();
                            else
                                phone.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                phone.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                phone.UpdatedAt = null;
                            response.Add(phone);
                            phone = new PhoneModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PhoneModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PhoneModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PhoneModel> Read(string connectionString, List<PhoneModel> phoneWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PhoneModel> response = new List<PhoneModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `phone`, `modified_by`, `created_at`, `updated_at`  FROM sch_phones ";
                if (phoneWhere != null && phoneWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < phoneWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(phoneWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, phoneWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, phoneWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].Phone))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `phone` = @phone" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@phone" + i, phoneWhere[i].Phone);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, phoneWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, phoneWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(phoneWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, phoneWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != phoneWhere.Count - 1)
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
                            PhoneModel phone = new PhoneModel(); if (!reader.IsDBNull(0))
                                phone.Id = reader.GetValue(0).ToString();
                            else
                                phone.Id = null;
                            if (!reader.IsDBNull(1))
                                phone.EntityId = reader.GetValue(1).ToString();
                            else
                                phone.EntityId = null;
                            if (!reader.IsDBNull(2))
                                phone.Phone = reader.GetValue(2).ToString();
                            else
                                phone.Phone = null;
                            if (!reader.IsDBNull(3))
                                phone.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                phone.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                phone.CreatedAt = reader.GetValue(4).ToString();
                            else
                                phone.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                phone.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                phone.UpdatedAt = null;
                            response.Add(phone);
                            phone = new PhoneModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PhoneModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PhoneModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PhoneModel> Create(string connectionString, PhoneModel phone)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PhoneModel> response = new List<PhoneModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO sch_phones (";
                string values = "(";
                if (!string.IsNullOrEmpty(phone.Id))
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
                    cmd.Parameters.AddWithValue("@id", phone.Id);
                }
                if (!string.IsNullOrEmpty(phone.EntityId))
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
                    cmd.Parameters.AddWithValue("@entityId", phone.EntityId);
                }
                if (!string.IsNullOrEmpty(phone.Phone))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `phone` ");
                        values = string.Concat(values, ", @phone");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `phone` ");
                        values = string.Concat(values, " @phone");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@phone", phone.Phone);
                }
                if (!string.IsNullOrEmpty(phone.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", phone.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(phone.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", phone.CreatedAt);
                }
                if (!string.IsNullOrEmpty(phone.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", phone.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                phone.Id = cmd.LastInsertedId.ToString();
                response.Add(phone);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PhoneModel>();
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
        public List<PhoneModel> Create(string connectionString, List<PhoneModel> phoneList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PhoneModel> response = new List<PhoneModel>();
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
                        for (int i = 0; i < phoneList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO sch_phones (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(phoneList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, phoneList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(phoneList[i].EntityId))
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
                                    cmd.Parameters.AddWithValue("@entityId" + i, phoneList[i].EntityId);
                                }
                                if (!string.IsNullOrEmpty(phoneList[i].Phone))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `phone` ");
                                        values = string.Concat(values, ", @phone" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `phone` ");
                                        values = string.Concat(values, "@phone" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@phone" + i, phoneList[i].Phone);
                                }
                                if (!string.IsNullOrEmpty(phoneList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, phoneList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(phoneList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, phoneList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(phoneList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, phoneList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(phoneList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<PhoneModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PhoneModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PhoneModel>();
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

        public List<PhoneModel> Update(string connectionString, PhoneModel phone, PhoneModel phoneWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PhoneModel> response = new List<PhoneModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE sch_phones SET ";
                if (!string.IsNullOrEmpty(phone.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", phone.Id);
                }
                if (!string.IsNullOrEmpty(phone.EntityId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `entity_id` = @entityId");
                    else
                        sql = string.Concat(sql, " `entity_id` = @entityId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@entityId", phone.EntityId);
                }
                if (!string.IsNullOrEmpty(phone.Phone))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `phone` = @phone");
                    else
                        sql = string.Concat(sql, " `phone` = @phone");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@phone", phone.Phone);
                }
                if (!string.IsNullOrEmpty(phone.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", phone.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(phone.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", phone.CreatedAt);
                }
                if (!string.IsNullOrEmpty(phone.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", phone.UpdatedAt);
                }
                if (phoneWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(phoneWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", phoneWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(phoneWhere.EntityId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `entity_id` = @entityIdWhere");
                        else
                            sql = string.Concat(sql, " `entity_id` = @entityIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@entityIdWhere", phoneWhere.EntityId);
                    }
                    if (!string.IsNullOrEmpty(phoneWhere.Phone))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `phone` = @phoneWhere");
                        else
                            sql = string.Concat(sql, " `phone` = @phoneWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@phoneWhere", phoneWhere.Phone);
                    }
                    if (!string.IsNullOrEmpty(phoneWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", phoneWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(phoneWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", phoneWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(phoneWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", phoneWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(phone);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PhoneModel>();
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

        public List<PhoneModel> Update(string connectionString, List<PhoneModel> phoneList, List<PhoneModel> phoneWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PhoneModel> response = new List<PhoneModel>();
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
                        for (int i = 0; i < phoneList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE sch_phones SET ";
                                if (!string.IsNullOrEmpty(phoneList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, phoneList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(phoneList[i].EntityId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `entity_id` = @entityId" + i);
                                    else
                                        sql = string.Concat(sql, " `entity_id` = @entityId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@entityId" + i, phoneList[i].EntityId);
                                }
                                if (!string.IsNullOrEmpty(phoneList[i].Phone))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `phone` = @phone" + i);
                                    else
                                        sql = string.Concat(sql, " `phone` = @phone" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@phone" + i, phoneList[i].Phone);
                                }
                                if (!string.IsNullOrEmpty(phoneList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, phoneList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(phoneList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, phoneList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(phoneList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, phoneList[i].UpdatedAt);
                                }
                                if (phoneWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(phoneWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, phoneWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(phoneWhereList[i].EntityId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `entity_id` = @entityIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `entity_id` = @entityIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@entityIdWhere" + i, phoneWhereList[i].EntityId);
                                    }
                                    if (!string.IsNullOrEmpty(phoneWhereList[i].Phone))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `phone` = @phoneWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `phone` = @phoneWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@phoneWhere" + i, phoneWhereList[i].Phone);
                                    }
                                    if (!string.IsNullOrEmpty(phoneWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, phoneWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(phoneWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, phoneWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(phoneWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, phoneWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(phoneList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PhoneModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PhoneModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PhoneModel>();
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

        public List<PhoneModel> Delete(string connectionString, PhoneModel phone)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PhoneModel> response = new List<PhoneModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM sch_phones  ";
                if (phone != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(phone.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", phone.Id);
                    }
                    if (!string.IsNullOrEmpty(phone.EntityId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `entity_id` = @entityId");
                        else
                            sql = string.Concat(sql, " `entity_id` = @entityId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@entityId", phone.EntityId);
                    }
                    if (!string.IsNullOrEmpty(phone.Phone))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `phone` = @phone");
                        else
                            sql = string.Concat(sql, " `phone` = @phone");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@phone", phone.Phone);
                    }
                    if (!string.IsNullOrEmpty(phone.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", phone.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(phone.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", phone.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(phone.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", phone.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(phone);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PhoneModel>();
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

        public List<PhoneModel> Delete(string connectionString, List<PhoneModel> phone)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PhoneModel> response = new List<PhoneModel>();
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
                        for (int i = 0; i < phone.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM sch_phones  ";
                                if (phone[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(phone[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, phone[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(phone[i].EntityId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `entity_id` = @entityId" + i);
                                        else
                                            sql = string.Concat(sql, " `entity_id` = @entityId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@entityId" + i, phone[i].EntityId);
                                    }
                                    if (!string.IsNullOrEmpty(phone[i].Phone))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `phone` = @phone" + i);
                                        else
                                            sql = string.Concat(sql, " `phone` = @phone" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@phone" + i, phone[i].Phone);
                                    }
                                    if (!string.IsNullOrEmpty(phone[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, phone[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(phone[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, phone[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(phone[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, phone[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(phone[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PhoneModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PhoneModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PhoneModel>();
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
