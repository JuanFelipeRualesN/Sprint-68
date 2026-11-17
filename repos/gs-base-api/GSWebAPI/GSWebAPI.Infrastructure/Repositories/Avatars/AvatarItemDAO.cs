using System;
using System.Collections.Generic;
using GSWebAPI.GSWebAPI.Domain.Models.Avatar;
using MySqlConnector;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.Avatars
{
    class AvatarItemDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, AvatarItemModel> ReadD(string connectionString, AvatarItemModel avatarItem, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AvatarItemModel> response = new Dictionary<string, AvatarItemModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `code`, `name`, `gender`, `url`, `part`, `unlock_value`, `unlock_type`, `is_bundle`, `modified_by`, `created_at`, `updated_at`  FROM av_avatar_items ";
                {
                    if (avatarItem != null)
                    {
                        if (!string.IsNullOrEmpty(avatarItem.Id))
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
                            cmd.Parameters.AddWithValue("@id", avatarItem.Id);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", avatarItem.Code);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Name))
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
                            cmd.Parameters.AddWithValue("@name", avatarItem.Name);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Gender))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `gender` = @gender ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `gender` = @gender ");
                            }
                            cmd.Parameters.AddWithValue("@gender", avatarItem.Gender);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Url))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `url` = @url ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `url` = @url ");
                            }
                            cmd.Parameters.AddWithValue("@url", avatarItem.Url);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Part))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `part` = @part ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `part` = @part ");
                            }
                            cmd.Parameters.AddWithValue("@part", avatarItem.Part);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.UnlockValue))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `unlock_value` = @unlockValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `unlock_value` = @unlockValue ");
                            }
                            cmd.Parameters.AddWithValue("@unlockValue", avatarItem.UnlockValue);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.UnlockType))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `unlock_type` = @unlockType ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `unlock_type` = @unlockType ");
                            }
                            cmd.Parameters.AddWithValue("@unlockType", avatarItem.UnlockType);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.IsBundle))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_bundle` = @isBundle ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_bundle` = @isBundle ");
                            }
                            cmd.Parameters.AddWithValue("@isBundle", avatarItem.IsBundle);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `modified_by` = @ModifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `modified_by` = @ModifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@ModifiedBy", avatarItem.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", avatarItem.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", avatarItem.UpdatedAt);
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
                                    avatarItem.Id = reader.GetValue(0).ToString();
                                else
                                    avatarItem.Id = null;
                                if (!reader.IsDBNull(1))
                                    avatarItem.Code = reader.GetValue(1).ToString();
                                else
                                    avatarItem.Code = null;
                                if (!reader.IsDBNull(2))
                                    avatarItem.Name = reader.GetValue(2).ToString();
                                else
                                    avatarItem.Name = null;
                                if (!reader.IsDBNull(3))
                                    avatarItem.Gender = reader.GetValue(3).ToString();
                                else
                                    avatarItem.Gender = null;
                                if (!reader.IsDBNull(4))
                                    avatarItem.Url = reader.GetValue(4).ToString();
                                else
                                    avatarItem.Url = null;
                                if (!reader.IsDBNull(5))
                                    avatarItem.Part = reader.GetValue(5).ToString();
                                else
                                    avatarItem.Part = null;
                                if (!reader.IsDBNull(6))
                                    avatarItem.UnlockValue = reader.GetValue(6).ToString();
                                else
                                    avatarItem.UnlockValue = null;
                                if (!reader.IsDBNull(7))
                                    avatarItem.UnlockType = reader.GetValue(7).ToString();
                                else
                                    avatarItem.UnlockType = null;
                                if (!reader.IsDBNull(8))
                                    avatarItem.IsBundle = reader.GetValue(8).ToString();
                                else
                                    avatarItem.IsBundle = null;
                                if (!reader.IsDBNull(9))
                                    avatarItem.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    avatarItem.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    avatarItem.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    avatarItem.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    avatarItem.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    avatarItem.UpdatedAt = null;
                                response.Add(avatarItem.Id, avatarItem);
                                avatarItem = new AvatarItemModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, AvatarItemModel>();
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
                response = new Dictionary<string, AvatarItemModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, AvatarItemModel> ReadD(string connectionString, AvatarItemModel avatarItem, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AvatarItemModel> response = new Dictionary<string, AvatarItemModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `code`, `name`, `gender`, `url`, `part`, `unlock_value`, `unlock_type`, `is_bundle`, `modified_by`, `created_at`, `updated_at`  FROM av_avatar_items ";
                {
                    if (avatarItem != null)
                    {
                        if (!string.IsNullOrEmpty(avatarItem.Id))
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
                            cmd.Parameters.AddWithValue("@id", avatarItem.Id);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", avatarItem.Code);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Name))
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
                            cmd.Parameters.AddWithValue("@name", avatarItem.Name);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Gender))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `gender` = @gender ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `gender` = @gender ");
                            }
                            cmd.Parameters.AddWithValue("@gender", avatarItem.Gender);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Url))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `url` = @url ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `url` = @url ");
                            }
                            cmd.Parameters.AddWithValue("@url", avatarItem.Url);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Part))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `part` = @part ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `part` = @part ");
                            }
                            cmd.Parameters.AddWithValue("@part", avatarItem.Part);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.UnlockValue))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `unlock_value` = @unlockValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `unlock_value` = @unlockValue ");
                            }
                            cmd.Parameters.AddWithValue("@unlockValue", avatarItem.UnlockValue);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.UnlockType))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `unlock_type` = @unlockType ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `unlock_type` = @unlockType ");
                            }
                            cmd.Parameters.AddWithValue("@unlockType", avatarItem.UnlockType);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.IsBundle))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_bundle` = @isBundle ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_bundle` = @isBundle ");
                            }
                            cmd.Parameters.AddWithValue("@isBundle", avatarItem.IsBundle);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `modified_by` = @ModifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `modified_by` = @ModifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@ModifiedBy", avatarItem.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", avatarItem.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", avatarItem.UpdatedAt);
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
                                    avatarItem.Id = reader.GetValue(0).ToString();
                                else
                                    avatarItem.Id = null;
                                if (!reader.IsDBNull(1))
                                    avatarItem.Code = reader.GetValue(1).ToString();
                                else
                                    avatarItem.Code = null;
                                if (!reader.IsDBNull(2))
                                    avatarItem.Name = reader.GetValue(2).ToString();
                                else
                                    avatarItem.Name = null;
                                if (!reader.IsDBNull(3))
                                    avatarItem.Gender = reader.GetValue(3).ToString();
                                else
                                    avatarItem.Gender = null;
                                if (!reader.IsDBNull(4))
                                    avatarItem.Url = reader.GetValue(4).ToString();
                                else
                                    avatarItem.Url = null;
                                if (!reader.IsDBNull(5))
                                    avatarItem.Part = reader.GetValue(5).ToString();
                                else
                                    avatarItem.Part = null;
                                if (!reader.IsDBNull(6))
                                    avatarItem.UnlockValue = reader.GetValue(6).ToString();
                                else
                                    avatarItem.UnlockValue = null;
                                if (!reader.IsDBNull(7))
                                    avatarItem.UnlockType = reader.GetValue(7).ToString();
                                else
                                    avatarItem.UnlockType = null;
                                if (!reader.IsDBNull(8))
                                    avatarItem.IsBundle = reader.GetValue(8).ToString();
                                else
                                    avatarItem.IsBundle = null;
                                if (!reader.IsDBNull(9))
                                    avatarItem.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    avatarItem.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    avatarItem.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    avatarItem.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    avatarItem.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    avatarItem.UpdatedAt = null;
                                response.Add(avatarItem.Id, avatarItem);
                                avatarItem = new AvatarItemModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, AvatarItemModel>();
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
                response = new Dictionary<string, AvatarItemModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, AvatarItemModel> ReadD(string connectionString, List<AvatarItemModel> avatarItemWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AvatarItemModel> response = new Dictionary<string, AvatarItemModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `code`, `name`, `gender`, `url`, `part`, `unlock_value`, `unlock_type`, `is_bundle`, `modified_by`, `created_at`, `updated_at`  FROM av_avatar_items ";
                if (avatarItemWhere != null && avatarItemWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < avatarItemWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, avatarItemWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, avatarItemWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, avatarItemWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Gender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gender` = @gender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gender" + i, avatarItemWhere[i].Gender);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Url))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `url` = @url" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@url" + i, avatarItemWhere[i].Url);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Part))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part` = @part" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@part" + i, avatarItemWhere[i].Part);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].UnlockValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `unlock_value` = @unlockValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@unlockValue" + i, avatarItemWhere[i].UnlockValue);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].UnlockType))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `unlock_type` = @unlockType" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@unlockType" + i, avatarItemWhere[i].UnlockType);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].IsBundle))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_bundle` = @isBundle" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isBundle" + i, avatarItemWhere[i].IsBundle);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @ModifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ModifiedBy" + i, avatarItemWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, avatarItemWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, avatarItemWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != avatarItemWhere.Count - 1)
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
                            AvatarItemModel avatarItem = new AvatarItemModel(); if (!reader.IsDBNull(0))
                                avatarItem.Id = reader.GetValue(0).ToString();
                            else
                                avatarItem.Id = null;
                            if (!reader.IsDBNull(1))
                                avatarItem.Code = reader.GetValue(1).ToString();
                            else
                                avatarItem.Code = null;
                            if (!reader.IsDBNull(2))
                                avatarItem.Name = reader.GetValue(2).ToString();
                            else
                                avatarItem.Name = null;
                            if (!reader.IsDBNull(3))
                                avatarItem.Gender = reader.GetValue(3).ToString();
                            else
                                avatarItem.Gender = null;
                            if (!reader.IsDBNull(4))
                                avatarItem.Url = reader.GetValue(4).ToString();
                            else
                                avatarItem.Url = null;
                            if (!reader.IsDBNull(5))
                                avatarItem.Part = reader.GetValue(5).ToString();
                            else
                                avatarItem.Part = null;
                            if (!reader.IsDBNull(6))
                                avatarItem.UnlockValue = reader.GetValue(6).ToString();
                            else
                                avatarItem.UnlockValue = null;
                            if (!reader.IsDBNull(7))
                                avatarItem.UnlockType = reader.GetValue(7).ToString();
                            else
                                avatarItem.UnlockType = null;
                            if (!reader.IsDBNull(8))
                                avatarItem.IsBundle = reader.GetValue(8).ToString();
                            else
                                avatarItem.IsBundle = null;
                            if (!reader.IsDBNull(9))
                                avatarItem.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                avatarItem.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                avatarItem.CreatedAt = reader.GetValue(10).ToString();
                            else
                                avatarItem.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                avatarItem.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                avatarItem.UpdatedAt = null;
                            response.Add(avatarItem.Id, avatarItem);
                            avatarItem = new AvatarItemModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, AvatarItemModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, AvatarItemModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, AvatarItemModel> ReadD(string connectionString, List<AvatarItemModel> avatarItemWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AvatarItemModel> response = new Dictionary<string, AvatarItemModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `code`, `name`, `gender`, `url`, `part`, `unlock_value`, `unlock_type`, `is_bundle`, `modified_by`, `created_at`, `updated_at`  FROM av_avatar_items ";
                if (avatarItemWhere != null && avatarItemWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < avatarItemWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, avatarItemWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, avatarItemWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, avatarItemWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Gender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gender` = @gender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gender" + i, avatarItemWhere[i].Gender);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Url))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `url` = @url" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@url" + i, avatarItemWhere[i].Url);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Part))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part` = @part" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@part" + i, avatarItemWhere[i].Part);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].UnlockValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `unlock_value` = @unlockValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@unlockValue" + i, avatarItemWhere[i].UnlockValue);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].UnlockType))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `unlock_type` = @unlockType" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@unlockType" + i, avatarItemWhere[i].UnlockType);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].IsBundle))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_bundle` = @isBundle" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isBundle" + i, avatarItemWhere[i].IsBundle);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @ModifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ModifiedBy" + i, avatarItemWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, avatarItemWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, avatarItemWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != avatarItemWhere.Count - 1)
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
                            AvatarItemModel avatarItem = new AvatarItemModel(); if (!reader.IsDBNull(0))
                                avatarItem.Id = reader.GetValue(0).ToString();
                            else
                                avatarItem.Id = null;
                            if (!reader.IsDBNull(1))
                                avatarItem.Code = reader.GetValue(1).ToString();
                            else
                                avatarItem.Code = null;
                            if (!reader.IsDBNull(2))
                                avatarItem.Name = reader.GetValue(2).ToString();
                            else
                                avatarItem.Name = null;
                            if (!reader.IsDBNull(3))
                                avatarItem.Gender = reader.GetValue(3).ToString();
                            else
                                avatarItem.Gender = null;
                            if (!reader.IsDBNull(4))
                                avatarItem.Url = reader.GetValue(4).ToString();
                            else
                                avatarItem.Url = null;
                            if (!reader.IsDBNull(5))
                                avatarItem.Part = reader.GetValue(5).ToString();
                            else
                                avatarItem.Part = null;
                            if (!reader.IsDBNull(6))
                                avatarItem.UnlockValue = reader.GetValue(6).ToString();
                            else
                                avatarItem.UnlockValue = null;
                            if (!reader.IsDBNull(7))
                                avatarItem.UnlockType = reader.GetValue(7).ToString();
                            else
                                avatarItem.UnlockType = null;
                            if (!reader.IsDBNull(8))
                                avatarItem.IsBundle = reader.GetValue(8).ToString();
                            else
                                avatarItem.IsBundle = null;
                            if (!reader.IsDBNull(9))
                                avatarItem.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                avatarItem.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                avatarItem.CreatedAt = reader.GetValue(10).ToString();
                            else
                                avatarItem.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                avatarItem.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                avatarItem.UpdatedAt = null;
                            response.Add(avatarItem.Id, avatarItem);
                            avatarItem = new AvatarItemModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, AvatarItemModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, AvatarItemModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AvatarItemModel> Read(string connectionString, AvatarItemModel avatarItem, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AvatarItemModel> response = new List<AvatarItemModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `code`, `name`, `gender`, `url`, `part`, `unlock_value`, `unlock_type`, `is_bundle`, `modified_by`, `created_at`, `updated_at`  FROM av_avatar_items ";
                {
                    if (avatarItem != null)
                    {
                        if (!string.IsNullOrEmpty(avatarItem.Id))
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
                            cmd.Parameters.AddWithValue("@id", avatarItem.Id);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", avatarItem.Code);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Name))
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
                            cmd.Parameters.AddWithValue("@name", avatarItem.Name);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Gender))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `gender` = @gender ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `gender` = @gender ");
                            }
                            cmd.Parameters.AddWithValue("@gender", avatarItem.Gender);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Url))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `url` = @url ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `url` = @url ");
                            }
                            cmd.Parameters.AddWithValue("@url", avatarItem.Url);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Part))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `part` = @part ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `part` = @part ");
                            }
                            cmd.Parameters.AddWithValue("@part", avatarItem.Part);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.UnlockValue))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `unlock_value` = @unlockValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `unlock_value` = @unlockValue ");
                            }
                            cmd.Parameters.AddWithValue("@unlockValue", avatarItem.UnlockValue);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.UnlockType))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `unlock_type` = @unlockType ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `unlock_type` = @unlockType ");
                            }
                            cmd.Parameters.AddWithValue("@unlockType", avatarItem.UnlockType);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.IsBundle))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_bundle` = @isBundle ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_bundle` = @isBundle ");
                            }
                            cmd.Parameters.AddWithValue("@isBundle", avatarItem.IsBundle);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `modified_by` = @ModifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `modified_by` = @ModifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@ModifiedBy", avatarItem.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", avatarItem.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", avatarItem.UpdatedAt);
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
                                    avatarItem.Id = reader.GetValue(0).ToString();
                                else
                                    avatarItem.Id = null;
                                if (!reader.IsDBNull(1))
                                    avatarItem.Code = reader.GetValue(1).ToString();
                                else
                                    avatarItem.Code = null;
                                if (!reader.IsDBNull(2))
                                    avatarItem.Name = reader.GetValue(2).ToString();
                                else
                                    avatarItem.Name = null;
                                if (!reader.IsDBNull(3))
                                    avatarItem.Gender = reader.GetValue(3).ToString();
                                else
                                    avatarItem.Gender = null;
                                if (!reader.IsDBNull(4))
                                    avatarItem.Url = reader.GetValue(4).ToString();
                                else
                                    avatarItem.Url = null;
                                if (!reader.IsDBNull(5))
                                    avatarItem.Part = reader.GetValue(5).ToString();
                                else
                                    avatarItem.Part = null;
                                if (!reader.IsDBNull(6))
                                    avatarItem.UnlockValue = reader.GetValue(6).ToString();
                                else
                                    avatarItem.UnlockValue = null;
                                if (!reader.IsDBNull(7))
                                    avatarItem.UnlockType = reader.GetValue(7).ToString();
                                else
                                    avatarItem.UnlockType = null;
                                if (!reader.IsDBNull(8))
                                    avatarItem.IsBundle = reader.GetValue(8).ToString();
                                else
                                    avatarItem.IsBundle = null;
                                if (!reader.IsDBNull(9))
                                    avatarItem.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    avatarItem.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    avatarItem.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    avatarItem.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    avatarItem.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    avatarItem.UpdatedAt = null;
                                response.Add(avatarItem);
                                avatarItem = new AvatarItemModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<AvatarItemModel>();
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
                response = new List<AvatarItemModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AvatarItemModel> Read(string connectionString, AvatarItemModel avatarItem, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AvatarItemModel> response = new List<AvatarItemModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `code`, `name`, `gender`, `url`, `part`, `unlock_value`, `unlock_type`, `is_bundle`, `modified_by`, `created_at`, `updated_at`  FROM av_avatar_items ";
                {
                    if (avatarItem != null)
                    {
                        if (!string.IsNullOrEmpty(avatarItem.Id))
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
                            cmd.Parameters.AddWithValue("@id", avatarItem.Id);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", avatarItem.Code);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Name))
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
                            cmd.Parameters.AddWithValue("@name", avatarItem.Name);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Gender))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `gender` = @gender ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `gender` = @gender ");
                            }
                            cmd.Parameters.AddWithValue("@gender", avatarItem.Gender);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Url))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `url` = @url ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `url` = @url ");
                            }
                            cmd.Parameters.AddWithValue("@url", avatarItem.Url);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.Part))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `part` = @part ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `part` = @part ");
                            }
                            cmd.Parameters.AddWithValue("@part", avatarItem.Part);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.UnlockValue))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `unlock_value` = @unlockValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `unlock_value` = @unlockValue ");
                            }
                            cmd.Parameters.AddWithValue("@unlockValue", avatarItem.UnlockValue);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.UnlockType))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `unlock_type` = @unlockType ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `unlock_type` = @unlockType ");
                            }
                            cmd.Parameters.AddWithValue("@unlockType", avatarItem.UnlockType);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.IsBundle))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_bundle` = @isBundle ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_bundle` = @isBundle ");
                            }
                            cmd.Parameters.AddWithValue("@isBundle", avatarItem.IsBundle);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `modified_by` = @ModifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `modified_by` = @ModifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@ModifiedBy", avatarItem.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", avatarItem.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(avatarItem.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", avatarItem.UpdatedAt);
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
                                    avatarItem.Id = reader.GetValue(0).ToString();
                                else
                                    avatarItem.Id = null;
                                if (!reader.IsDBNull(1))
                                    avatarItem.Code = reader.GetValue(1).ToString();
                                else
                                    avatarItem.Code = null;
                                if (!reader.IsDBNull(2))
                                    avatarItem.Name = reader.GetValue(2).ToString();
                                else
                                    avatarItem.Name = null;
                                if (!reader.IsDBNull(3))
                                    avatarItem.Gender = reader.GetValue(3).ToString();
                                else
                                    avatarItem.Gender = null;
                                if (!reader.IsDBNull(4))
                                    avatarItem.Url = reader.GetValue(4).ToString();
                                else
                                    avatarItem.Url = null;
                                if (!reader.IsDBNull(5))
                                    avatarItem.Part = reader.GetValue(5).ToString();
                                else
                                    avatarItem.Part = null;
                                if (!reader.IsDBNull(6))
                                    avatarItem.UnlockValue = reader.GetValue(6).ToString();
                                else
                                    avatarItem.UnlockValue = null;
                                if (!reader.IsDBNull(7))
                                    avatarItem.UnlockType = reader.GetValue(7).ToString();
                                else
                                    avatarItem.UnlockType = null;
                                if (!reader.IsDBNull(8))
                                    avatarItem.IsBundle = reader.GetValue(8).ToString();
                                else
                                    avatarItem.IsBundle = null;
                                if (!reader.IsDBNull(9))
                                    avatarItem.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    avatarItem.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    avatarItem.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    avatarItem.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    avatarItem.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    avatarItem.UpdatedAt = null;
                                response.Add(avatarItem);
                                avatarItem = new AvatarItemModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<AvatarItemModel>();
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
                response = new List<AvatarItemModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AvatarItemModel> Read(string connectionString, List<AvatarItemModel> avatarItemWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AvatarItemModel> response = new List<AvatarItemModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `code`, `name`, `gender`, `url`, `part`, `unlock_value`, `unlock_type`, `is_bundle`, `modified_by`, `created_at`, `updated_at`  FROM av_avatar_items ";
                if (avatarItemWhere != null && avatarItemWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < avatarItemWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, avatarItemWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, avatarItemWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, avatarItemWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Gender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gender` = @gender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gender" + i, avatarItemWhere[i].Gender);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Url))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `url` = @url" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@url" + i, avatarItemWhere[i].Url);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Part))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part` = @part" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@part" + i, avatarItemWhere[i].Part);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].UnlockValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `unlock_value` = @unlockValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@unlockValue" + i, avatarItemWhere[i].UnlockValue);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].UnlockType))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `unlock_type` = @unlockType" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@unlockType" + i, avatarItemWhere[i].UnlockType);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].IsBundle))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_bundle` = @isBundle" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isBundle" + i, avatarItemWhere[i].IsBundle);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @ModifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ModifiedBy" + i, avatarItemWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, avatarItemWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, avatarItemWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != avatarItemWhere.Count - 1)
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
                            AvatarItemModel avatarItem = new AvatarItemModel(); if (!reader.IsDBNull(0))
                                avatarItem.Id = reader.GetValue(0).ToString();
                            else
                                avatarItem.Id = null;
                            if (!reader.IsDBNull(1))
                                avatarItem.Code = reader.GetValue(1).ToString();
                            else
                                avatarItem.Code = null;
                            if (!reader.IsDBNull(2))
                                avatarItem.Name = reader.GetValue(2).ToString();
                            else
                                avatarItem.Name = null;
                            if (!reader.IsDBNull(3))
                                avatarItem.Gender = reader.GetValue(3).ToString();
                            else
                                avatarItem.Gender = null;
                            if (!reader.IsDBNull(4))
                                avatarItem.Url = reader.GetValue(4).ToString();
                            else
                                avatarItem.Url = null;
                            if (!reader.IsDBNull(5))
                                avatarItem.Part = reader.GetValue(5).ToString();
                            else
                                avatarItem.Part = null;
                            if (!reader.IsDBNull(6))
                                avatarItem.UnlockValue = reader.GetValue(6).ToString();
                            else
                                avatarItem.UnlockValue = null;
                            if (!reader.IsDBNull(7))
                                avatarItem.UnlockType = reader.GetValue(7).ToString();
                            else
                                avatarItem.UnlockType = null;
                            if (!reader.IsDBNull(8))
                                avatarItem.IsBundle = reader.GetValue(8).ToString();
                            else
                                avatarItem.IsBundle = null;
                            if (!reader.IsDBNull(9))
                                avatarItem.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                avatarItem.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                avatarItem.CreatedAt = reader.GetValue(10).ToString();
                            else
                                avatarItem.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                avatarItem.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                avatarItem.UpdatedAt = null;
                            response.Add(avatarItem);
                            avatarItem = new AvatarItemModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<AvatarItemModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AvatarItemModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AvatarItemModel> Read(string connectionString, List<AvatarItemModel> avatarItemWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AvatarItemModel> response = new List<AvatarItemModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `code`, `name`, `gender`, `url`, `part`, `unlock_value`, `unlock_type`, `is_bundle`, `modified_by`, `created_at`, `updated_at`  FROM av_avatar_items ";
                if (avatarItemWhere != null && avatarItemWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < avatarItemWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, avatarItemWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, avatarItemWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, avatarItemWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Gender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gender` = @gender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gender" + i, avatarItemWhere[i].Gender);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Url))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `url` = @url" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@url" + i, avatarItemWhere[i].Url);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].Part))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part` = @part" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@part" + i, avatarItemWhere[i].Part);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].UnlockValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `unlock_value` = @unlockValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@unlockValue" + i, avatarItemWhere[i].UnlockValue);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].UnlockType))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `unlock_type` = @unlockType" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@unlockType" + i, avatarItemWhere[i].UnlockType);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].IsBundle))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_bundle` = @isBundle" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isBundle" + i, avatarItemWhere[i].IsBundle);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @ModifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@ModifiedBy" + i, avatarItemWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, avatarItemWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(avatarItemWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, avatarItemWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != avatarItemWhere.Count - 1)
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
                            AvatarItemModel avatarItem = new AvatarItemModel(); if (!reader.IsDBNull(0))
                                avatarItem.Id = reader.GetValue(0).ToString();
                            else
                                avatarItem.Id = null;
                            if (!reader.IsDBNull(1))
                                avatarItem.Code = reader.GetValue(1).ToString();
                            else
                                avatarItem.Code = null;
                            if (!reader.IsDBNull(2))
                                avatarItem.Name = reader.GetValue(2).ToString();
                            else
                                avatarItem.Name = null;
                            if (!reader.IsDBNull(3))
                                avatarItem.Gender = reader.GetValue(3).ToString();
                            else
                                avatarItem.Gender = null;
                            if (!reader.IsDBNull(4))
                                avatarItem.Url = reader.GetValue(4).ToString();
                            else
                                avatarItem.Url = null;
                            if (!reader.IsDBNull(5))
                                avatarItem.Part = reader.GetValue(5).ToString();
                            else
                                avatarItem.Part = null;
                            if (!reader.IsDBNull(6))
                                avatarItem.UnlockValue = reader.GetValue(6).ToString();
                            else
                                avatarItem.UnlockValue = null;
                            if (!reader.IsDBNull(7))
                                avatarItem.UnlockType = reader.GetValue(7).ToString();
                            else
                                avatarItem.UnlockType = null;
                            if (!reader.IsDBNull(8))
                                avatarItem.IsBundle = reader.GetValue(8).ToString();
                            else
                                avatarItem.IsBundle = null;
                            if (!reader.IsDBNull(9))
                                avatarItem.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                avatarItem.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                avatarItem.CreatedAt = reader.GetValue(10).ToString();
                            else
                                avatarItem.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                avatarItem.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                avatarItem.UpdatedAt = null;
                            response.Add(avatarItem);
                            avatarItem = new AvatarItemModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<AvatarItemModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AvatarItemModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AvatarItemModel> Create(string connectionString, AvatarItemModel avatarItem)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AvatarItemModel> response = new List<AvatarItemModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO av_avatar_items (";
                string values = "(";
                if (!string.IsNullOrEmpty(avatarItem.Id))
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
                    cmd.Parameters.AddWithValue("@id", avatarItem.Id);
                }
                if (!string.IsNullOrEmpty(avatarItem.Code))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `code` ");
                        values = string.Concat(values, ", @code");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `code` ");
                        values = string.Concat(values, " @code");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", avatarItem.Code);
                }
                if (!string.IsNullOrEmpty(avatarItem.Name))
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
                    cmd.Parameters.AddWithValue("@name", avatarItem.Name);
                }
                if (!string.IsNullOrEmpty(avatarItem.Gender))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `gender` ");
                        values = string.Concat(values, ", @gender");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `gender` ");
                        values = string.Concat(values, " @gender");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@gender", avatarItem.Gender);
                }
                if (!string.IsNullOrEmpty(avatarItem.Url))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `url` ");
                        values = string.Concat(values, ", @url");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `url` ");
                        values = string.Concat(values, " @url");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@url", avatarItem.Url);
                }
                if (!string.IsNullOrEmpty(avatarItem.Part))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `part` ");
                        values = string.Concat(values, ", @part");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `part` ");
                        values = string.Concat(values, " @part");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@part", avatarItem.Part);
                }
                if (!string.IsNullOrEmpty(avatarItem.UnlockValue))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `unlock_value` ");
                        values = string.Concat(values, ", @unlockValue");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `unlock_value` ");
                        values = string.Concat(values, " @unlockValue");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@unlockValue", avatarItem.UnlockValue);
                }
                if (!string.IsNullOrEmpty(avatarItem.UnlockType))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `unlock_type` ");
                        values = string.Concat(values, ", @unlockType");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `unlock_type` ");
                        values = string.Concat(values, " @unlockType");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@unlockType", avatarItem.UnlockType);
                }
                if (!string.IsNullOrEmpty(avatarItem.IsBundle))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `is_bundle` ");
                        values = string.Concat(values, ", @isBundle");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `is_bundle` ");
                        values = string.Concat(values, " @isBundle");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isBundle", avatarItem.IsBundle);
                }
                if (!string.IsNullOrEmpty(avatarItem.ModifiedBy))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `modified_by` ");
                        values = string.Concat(values, ", @ModifiedBy");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `modified_by` ");
                        values = string.Concat(values, " @ModifiedBy");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ModifiedBy", avatarItem.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(avatarItem.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", avatarItem.CreatedAt);
                }
                if (!string.IsNullOrEmpty(avatarItem.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", avatarItem.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                avatarItem.Id = cmd.LastInsertedId.ToString();
                response.Add(avatarItem);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AvatarItemModel>();
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
        public List<AvatarItemModel> Create(string connectionString, List<AvatarItemModel> avatarItemList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AvatarItemModel> response = new List<AvatarItemModel>();
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
                        for (int i = 0; i < avatarItemList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO av_avatar_items (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(avatarItemList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, avatarItemList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].Code))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `code` ");
                                        values = string.Concat(values, ", @code" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `code` ");
                                        values = string.Concat(values, "@code" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, avatarItemList[i].Code);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].Name))
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
                                    cmd.Parameters.AddWithValue("@name" + i, avatarItemList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].Gender))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `gender` ");
                                        values = string.Concat(values, ", @gender" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `gender` ");
                                        values = string.Concat(values, "@gender" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@gender" + i, avatarItemList[i].Gender);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].Url))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `url` ");
                                        values = string.Concat(values, ", @url" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `url` ");
                                        values = string.Concat(values, "@url" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@url" + i, avatarItemList[i].Url);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].Part))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `part` ");
                                        values = string.Concat(values, ", @part" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `part` ");
                                        values = string.Concat(values, "@part" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@part" + i, avatarItemList[i].Part);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].UnlockValue))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `unlock_value` ");
                                        values = string.Concat(values, ", @unlockValue" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `unlock_value` ");
                                        values = string.Concat(values, "@unlockValue" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@unlockValue" + i, avatarItemList[i].UnlockValue);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].UnlockType))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `unlock_type` ");
                                        values = string.Concat(values, ", @unlockType" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `unlock_type` ");
                                        values = string.Concat(values, "@unlockType" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@unlockType" + i, avatarItemList[i].UnlockType);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].IsBundle))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `is_bundle` ");
                                        values = string.Concat(values, ", @isBundle" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `is_bundle` ");
                                        values = string.Concat(values, "@isBundle" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isBundle" + i, avatarItemList[i].IsBundle);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `modified_by` ");
                                        values = string.Concat(values, ", @ModifiedBy" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `modified_by` ");
                                        values = string.Concat(values, "@ModifiedBy" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ModifiedBy" + i, avatarItemList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, avatarItemList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, avatarItemList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(avatarItemList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<AvatarItemModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AvatarItemModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AvatarItemModel>();
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

        public List<AvatarItemModel> Update(string connectionString, AvatarItemModel avatarItem, AvatarItemModel avatarItemWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AvatarItemModel> response = new List<AvatarItemModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE av_avatar_items SET ";
                if (!string.IsNullOrEmpty(avatarItem.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", avatarItem.Id);
                }
                if (!string.IsNullOrEmpty(avatarItem.Code))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `code` = @code");
                    else
                        sql = string.Concat(sql, " `code` = @code");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", avatarItem.Code);
                }
                if (!string.IsNullOrEmpty(avatarItem.Name))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `name` = @name");
                    else
                        sql = string.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", avatarItem.Name);
                }
                if (!string.IsNullOrEmpty(avatarItem.Gender))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `gender` = @gender");
                    else
                        sql = string.Concat(sql, " `gender` = @gender");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@gender", avatarItem.Gender);
                }
                if (!string.IsNullOrEmpty(avatarItem.Url))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `url` = @url");
                    else
                        sql = string.Concat(sql, " `url` = @url");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@url", avatarItem.Url);
                }
                if (!string.IsNullOrEmpty(avatarItem.Part))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `part` = @part");
                    else
                        sql = string.Concat(sql, " `part` = @part");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@part", avatarItem.Part);
                }
                if (!string.IsNullOrEmpty(avatarItem.UnlockValue))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `unlock_value` = @unlockValue");
                    else
                        sql = string.Concat(sql, " `unlock_value` = @unlockValue");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@unlockValue", avatarItem.UnlockValue);
                }
                if (!string.IsNullOrEmpty(avatarItem.UnlockType))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `unlock_type` = @unlockType");
                    else
                        sql = string.Concat(sql, " `unlock_type` = @unlockType");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@unlockType", avatarItem.UnlockType);
                }
                if (!string.IsNullOrEmpty(avatarItem.IsBundle))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `is_bundle` = @isBundle");
                    else
                        sql = string.Concat(sql, " `is_bundle` = @isBundle");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isBundle", avatarItem.IsBundle);
                }
                if (!string.IsNullOrEmpty(avatarItem.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @ModifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @ModifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@ModifiedBy", avatarItem.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(avatarItem.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", avatarItem.CreatedAt);
                }
                if (!string.IsNullOrEmpty(avatarItem.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", avatarItem.UpdatedAt);
                }
                if (avatarItemWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(avatarItemWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", avatarItemWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(avatarItemWhere.Code))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `code` = @codeWhere");
                        else
                            sql = string.Concat(sql, " `code` = @codeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@codeWhere", avatarItemWhere.Code);
                    }
                    if (!string.IsNullOrEmpty(avatarItemWhere.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = string.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", avatarItemWhere.Name);
                    }
                    if (!string.IsNullOrEmpty(avatarItemWhere.Gender))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `gender` = @genderWhere");
                        else
                            sql = string.Concat(sql, " `gender` = @genderWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@genderWhere", avatarItemWhere.Gender);
                    }
                    if (!string.IsNullOrEmpty(avatarItemWhere.Url))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `url` = @urlWhere");
                        else
                            sql = string.Concat(sql, " `url` = @urlWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@urlWhere", avatarItemWhere.Url);
                    }
                    if (!string.IsNullOrEmpty(avatarItemWhere.Part))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `part` = @partWhere");
                        else
                            sql = string.Concat(sql, " `part` = @partWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@partWhere", avatarItemWhere.Part);
                    }
                    if (!string.IsNullOrEmpty(avatarItemWhere.UnlockValue))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `unlock_value` = @unlockValueWhere");
                        else
                            sql = string.Concat(sql, " `unlock_value` = @unlockValueWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@unlockValueWhere", avatarItemWhere.UnlockValue);
                    }
                    if (!string.IsNullOrEmpty(avatarItemWhere.UnlockType))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `unlock_type` = @unlockTypeWhere");
                        else
                            sql = string.Concat(sql, " `unlock_type` = @unlockTypeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@unlockTypeWhere", avatarItemWhere.UnlockType);
                    }
                    if (!string.IsNullOrEmpty(avatarItemWhere.IsBundle))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `is_bundle` = @isBundleWhere");
                        else
                            sql = string.Concat(sql, " `is_bundle` = @isBundleWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isBundleWhere", avatarItemWhere.IsBundle);
                    }
                    if (!string.IsNullOrEmpty(avatarItemWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @ModifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @ModifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ModifiedByWhere", avatarItemWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(avatarItemWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", avatarItemWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(avatarItemWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", avatarItemWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(avatarItem);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<AvatarItemModel>();
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

        public List<AvatarItemModel> Update(string connectionString, List<AvatarItemModel> avatarItemList, List<AvatarItemModel> avatarItemWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AvatarItemModel> response = new List<AvatarItemModel>();
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
                        for (int i = 0; i < avatarItemList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE av_avatar_items SET ";
                                if (!string.IsNullOrEmpty(avatarItemList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, avatarItemList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].Code))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `code` = @code" + i);
                                    else
                                        sql = string.Concat(sql, " `code` = @code" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, avatarItemList[i].Code);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].Name))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = string.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, avatarItemList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].Gender))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `gender` = @gender" + i);
                                    else
                                        sql = string.Concat(sql, " `gender` = @gender" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@gender" + i, avatarItemList[i].Gender);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].Url))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `url` = @url" + i);
                                    else
                                        sql = string.Concat(sql, " `url` = @url" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@url" + i, avatarItemList[i].Url);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].Part))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `part` = @part" + i);
                                    else
                                        sql = string.Concat(sql, " `part` = @part" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@part" + i, avatarItemList[i].Part);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].UnlockValue))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `unlock_value` = @unlockValue" + i);
                                    else
                                        sql = string.Concat(sql, " `unlock_value` = @unlockValue" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@unlockValue" + i, avatarItemList[i].UnlockValue);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].UnlockType))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `unlock_type` = @unlockType" + i);
                                    else
                                        sql = string.Concat(sql, " `unlock_type` = @unlockType" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@unlockType" + i, avatarItemList[i].UnlockType);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].IsBundle))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `is_bundle` = @isBundle" + i);
                                    else
                                        sql = string.Concat(sql, " `is_bundle` = @isBundle" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isBundle" + i, avatarItemList[i].IsBundle);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @ModifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @ModifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@ModifiedBy" + i, avatarItemList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, avatarItemList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(avatarItemList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, avatarItemList[i].UpdatedAt);
                                }
                                if (avatarItemWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(avatarItemWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, avatarItemWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItemWhereList[i].Code))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `code` = @codeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `code` = @codeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@codeWhere" + i, avatarItemWhereList[i].Code);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItemWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, avatarItemWhereList[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItemWhereList[i].Gender))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `gender` = @genderWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `gender` = @genderWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@genderWhere" + i, avatarItemWhereList[i].Gender);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItemWhereList[i].Url))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `url` = @urlWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `url` = @urlWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@urlWhere" + i, avatarItemWhereList[i].Url);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItemWhereList[i].Part))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `part` = @partWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `part` = @partWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@partWhere" + i, avatarItemWhereList[i].Part);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItemWhereList[i].UnlockValue))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `unlock_value` = @unlockValueWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `unlock_value` = @unlockValueWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@unlockValueWhere" + i, avatarItemWhereList[i].UnlockValue);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItemWhereList[i].UnlockType))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `unlock_type` = @unlockTypeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `unlock_type` = @unlockTypeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@unlockTypeWhere" + i, avatarItemWhereList[i].UnlockType);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItemWhereList[i].IsBundle))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `is_bundle` = @isBundleWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `is_bundle` = @isBundleWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isBundleWhere" + i, avatarItemWhereList[i].IsBundle);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItemWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @ModifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @ModifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ModifiedByWhere" + i, avatarItemWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItemWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, avatarItemWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItemWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, avatarItemWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(avatarItemList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<AvatarItemModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AvatarItemModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AvatarItemModel>();
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

        public List<AvatarItemModel> Delete(string connectionString, AvatarItemModel avatarItem)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AvatarItemModel> response = new List<AvatarItemModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM av_avatar_items  ";
                if (avatarItem != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(avatarItem.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", avatarItem.Id);
                    }
                    if (!string.IsNullOrEmpty(avatarItem.Code))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `code` = @code");
                        else
                            sql = string.Concat(sql, " `code` = @code");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@code", avatarItem.Code);
                    }
                    if (!string.IsNullOrEmpty(avatarItem.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @name");
                        else
                            sql = string.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", avatarItem.Name);
                    }
                    if (!string.IsNullOrEmpty(avatarItem.Gender))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `gender` = @gender");
                        else
                            sql = string.Concat(sql, " `gender` = @gender");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@gender", avatarItem.Gender);
                    }
                    if (!string.IsNullOrEmpty(avatarItem.Url))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `url` = @url");
                        else
                            sql = string.Concat(sql, " `url` = @url");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@url", avatarItem.Url);
                    }
                    if (!string.IsNullOrEmpty(avatarItem.Part))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `part` = @part");
                        else
                            sql = string.Concat(sql, " `part` = @part");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@part", avatarItem.Part);
                    }
                    if (!string.IsNullOrEmpty(avatarItem.UnlockValue))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `unlock_value` = @unlockValue");
                        else
                            sql = string.Concat(sql, " `unlock_value` = @unlockValue");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@unlockValue", avatarItem.UnlockValue);
                    }
                    if (!string.IsNullOrEmpty(avatarItem.UnlockType))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `unlock_type` = @unlockType");
                        else
                            sql = string.Concat(sql, " `unlock_type` = @unlockType");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@unlockType", avatarItem.UnlockType);
                    }
                    if (!string.IsNullOrEmpty(avatarItem.IsBundle))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `is_bundle` = @isBundle");
                        else
                            sql = string.Concat(sql, " `is_bundle` = @isBundle");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isBundle", avatarItem.IsBundle);
                    }
                    if (!string.IsNullOrEmpty(avatarItem.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @ModifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @ModifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@ModifiedBy", avatarItem.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(avatarItem.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", avatarItem.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(avatarItem.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", avatarItem.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(avatarItem);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<AvatarItemModel>();
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

        public List<AvatarItemModel> Delete(string connectionString, List<AvatarItemModel> avatarItem)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AvatarItemModel> response = new List<AvatarItemModel>();
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
                        for (int i = 0; i < avatarItem.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM av_avatar_items  ";
                                if (avatarItem[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(avatarItem[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, avatarItem[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItem[i].Code))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `code` = @code" + i);
                                        else
                                            sql = string.Concat(sql, " `code` = @code" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@code" + i, avatarItem[i].Code);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItem[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, avatarItem[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItem[i].Gender))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `gender` = @gender" + i);
                                        else
                                            sql = string.Concat(sql, " `gender` = @gender" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@gender" + i, avatarItem[i].Gender);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItem[i].Url))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `url` = @url" + i);
                                        else
                                            sql = string.Concat(sql, " `url` = @url" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@url" + i, avatarItem[i].Url);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItem[i].Part))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `part` = @part" + i);
                                        else
                                            sql = string.Concat(sql, " `part` = @part" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@part" + i, avatarItem[i].Part);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItem[i].UnlockValue))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `unlock_value` = @unlockValue" + i);
                                        else
                                            sql = string.Concat(sql, " `unlock_value` = @unlockValue" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@unlockValue" + i, avatarItem[i].UnlockValue);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItem[i].UnlockType))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `unlock_type` = @unlockType" + i);
                                        else
                                            sql = string.Concat(sql, " `unlock_type` = @unlockType" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@unlockType" + i, avatarItem[i].UnlockType);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItem[i].IsBundle))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `is_bundle` = @isBundle" + i);
                                        else
                                            sql = string.Concat(sql, " `is_bundle` = @isBundle" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isBundle" + i, avatarItem[i].IsBundle);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItem[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @ModifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @ModifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@ModifiedBy" + i, avatarItem[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItem[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, avatarItem[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(avatarItem[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, avatarItem[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(avatarItem[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<AvatarItemModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AvatarItemModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AvatarItemModel>();
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
