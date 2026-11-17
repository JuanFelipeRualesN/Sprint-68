using System;
using System.Collections.Generic;
using GSWebAPI.GSWebAPI.Domain.Models.Avatar;
using MySqlConnector;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.Avatars
{
    class UserUnlockedItemDAO
    {
        private MySqlConnection _connection;
        public List<UserUnlockedItemModel> Read(string connectionString, UserUnlockedItemModel userUnlockedItem, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedItemModel> response = new List<UserUnlockedItemModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `avatar_item_code`, `avatar_part`, `modified_by`, `created_at`, `updated_at`  FROM av_user_unlocked_items ";
                {
                    if (userUnlockedItem != null)
                    {
                        if (!string.IsNullOrEmpty(userUnlockedItem.Id))
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
                            cmd.Parameters.AddWithValue("@id", userUnlockedItem.Id);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedItem.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userUnlockedItem.UserId);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedItem.AvatarItemCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `avatar_item_code` = @avatarItemCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `avatar_item_code` = @avatarItemCode ");
                            }
                            cmd.Parameters.AddWithValue("@avatarItemCode", userUnlockedItem.AvatarItemCode);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedItem.AvatarPart))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `avatar_part` = @avatarPart ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `avatar_part` = @avatarPart ");
                            }
                            cmd.Parameters.AddWithValue("@avatarPart", userUnlockedItem.AvatarPart);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedItem.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userUnlockedItem.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedItem.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userUnlockedItem.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedItem.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userUnlockedItem.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        if (firstOne)
                            sql = string.Concat(sql, " LIMIT 1 ");
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader Reader = cmd.ExecuteReader();
                    try
                    {
                        if (Reader.HasRows)
                            while (Reader.Read())
                            {
                                if (!Reader.IsDBNull(0))
                                    userUnlockedItem.Id = Reader.GetValue(0).ToString();
                                else
                                    userUnlockedItem.Id = null;
                                if (!Reader.IsDBNull(1))
                                    userUnlockedItem.UserId = Reader.GetValue(1).ToString();
                                else
                                    userUnlockedItem.UserId = null;
                                if (!Reader.IsDBNull(2))
                                    userUnlockedItem.AvatarItemCode = Reader.GetValue(2).ToString();
                                else
                                    userUnlockedItem.AvatarItemCode = null;
                                if (!Reader.IsDBNull(3))
                                    userUnlockedItem.AvatarPart = Reader.GetValue(3).ToString();
                                else
                                    userUnlockedItem.AvatarPart = null;
                                if (!Reader.IsDBNull(4))
                                    userUnlockedItem.ModifiedBy = Reader.GetValue(4).ToString();
                                else
                                    userUnlockedItem.ModifiedBy = null;
                                if (!Reader.IsDBNull(5))
                                    userUnlockedItem.CreatedAt = Reader.GetValue(5).ToString();
                                else
                                    userUnlockedItem.CreatedAt = null;
                                if (!Reader.IsDBNull(6))
                                    userUnlockedItem.UpdatedAt = Reader.GetValue(6).ToString();
                                else
                                    userUnlockedItem.UpdatedAt = null;
                                response.Add(userUnlockedItem);
                                userUnlockedItem = new UserUnlockedItemModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserUnlockedItemModel>();
                        Reader.Close();
                    }
                    finally
                    {
                        if (Reader != null)
                            Reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserUnlockedItemModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserUnlockedItemModel> Read(string connectionString, List<UserUnlockedItemModel> userUnlockedItemWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedItemModel> response = new List<UserUnlockedItemModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `avatar_item_code`, `avatar_part`, `modified_by`, `created_at`, `updated_at`  FROM av_user_unlocked_items ";
                if (userUnlockedItemWhere != null && userUnlockedItemWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userUnlockedItemWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userUnlockedItemWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userUnlockedItemWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedItemWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userUnlockedItemWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedItemWhere[i].AvatarItemCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `avatar_item_code` = @avatarItemCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@avatarItemCode" + i, userUnlockedItemWhere[i].AvatarItemCode);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedItemWhere[i].AvatarPart))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `avatar_part` = @avatarPart" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@avatarPart" + i, userUnlockedItemWhere[i].AvatarPart);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedItemWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userUnlockedItemWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedItemWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userUnlockedItemWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedItemWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userUnlockedItemWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userUnlockedItemWhere.Count - 1)
                            sqlIn += " OR ";
                    }
                }
                cmd.CommandText = sql + " " + sqlIn;
                MySqlDataReader Reader = cmd.ExecuteReader();
                try
                {
                    if (Reader.HasRows)
                        while (Reader.Read())
                        {
                            UserUnlockedItemModel userUnlockedItem = new UserUnlockedItemModel(); if (!Reader.IsDBNull(0))
                                userUnlockedItem.Id = Reader.GetValue(0).ToString();
                            else
                                userUnlockedItem.Id = null;
                            if (!Reader.IsDBNull(1))
                                userUnlockedItem.UserId = Reader.GetValue(1).ToString();
                            else
                                userUnlockedItem.UserId = null;
                            if (!Reader.IsDBNull(2))
                                userUnlockedItem.AvatarItemCode = Reader.GetValue(2).ToString();
                            else
                                userUnlockedItem.AvatarItemCode = null;
                            if (!Reader.IsDBNull(3))
                                userUnlockedItem.AvatarPart = Reader.GetValue(3).ToString();
                            else
                                userUnlockedItem.AvatarPart = null;
                            if (!Reader.IsDBNull(4))
                                userUnlockedItem.ModifiedBy = Reader.GetValue(4).ToString();
                            else
                                userUnlockedItem.ModifiedBy = null;
                            if (!Reader.IsDBNull(5))
                                userUnlockedItem.CreatedAt = Reader.GetValue(5).ToString();
                            else
                                userUnlockedItem.CreatedAt = null;
                            if (!Reader.IsDBNull(6))
                                userUnlockedItem.UpdatedAt = Reader.GetValue(6).ToString();
                            else
                                userUnlockedItem.UpdatedAt = null;
                            response.Add(userUnlockedItem);
                            userUnlockedItem = new UserUnlockedItemModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserUnlockedItemModel>();
                    Reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserUnlockedItemModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserUnlockedItemModel> Create(string connectionString, UserUnlockedItemModel userUnlockedItem)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedItemModel> response = new List<UserUnlockedItemModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO av_user_unlocked_items (";
                string values = "(";
                if (!string.IsNullOrEmpty(userUnlockedItem.Id))
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
                    cmd.Parameters.AddWithValue("@id", userUnlockedItem.Id);
                }
                if (!string.IsNullOrEmpty(userUnlockedItem.UserId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `user_id` ");
                        values = string.Concat(values, ", @userId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `user_id` ");
                        values = string.Concat(values, " @userId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userUnlockedItem.UserId);
                }
                if (!string.IsNullOrEmpty(userUnlockedItem.AvatarItemCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `avatar_item_code` ");
                        values = string.Concat(values, ", @avatarItemCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `avatar_item_code` ");
                        values = string.Concat(values, " @avatarItemCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@avatarItemCode", userUnlockedItem.AvatarItemCode);
                }
                if (!string.IsNullOrEmpty(userUnlockedItem.AvatarPart))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `avatar_part` ");
                        values = string.Concat(values, ", @avatarPart");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `avatar_part` ");
                        values = string.Concat(values, " @avatarPart");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@avatarPart", userUnlockedItem.AvatarPart);
                }
                if (!string.IsNullOrEmpty(userUnlockedItem.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userUnlockedItem.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userUnlockedItem.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userUnlockedItem.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userUnlockedItem.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userUnlockedItem.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userUnlockedItem.Id = cmd.LastInsertedId.ToString();
                response.Add(userUnlockedItem);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserUnlockedItemModel>();
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
        public List<UserUnlockedItemModel> Create(string connectionString, List<UserUnlockedItemModel> userUnlockedItemList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedItemModel> response = new List<UserUnlockedItemModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userUnlockedItemList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO av_user_unlocked_items (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(userUnlockedItemList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userUnlockedItemList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedItemList[i].UserId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `user_id` ");
                                        values = string.Concat(values, ", @userId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `user_id` ");
                                        values = string.Concat(values, "@userId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userUnlockedItemList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedItemList[i].AvatarItemCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `avatar_item_code` ");
                                        values = string.Concat(values, ", @avatarItemCode" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `avatar_item_code` ");
                                        values = string.Concat(values, "@avatarItemCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@avatarItemCode" + i, userUnlockedItemList[i].AvatarItemCode);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedItemList[i].AvatarPart))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `avatar_part` ");
                                        values = string.Concat(values, ", @avatarPart" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `avatar_part` ");
                                        values = string.Concat(values, "@avatarPart" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@avatarPart" + i, userUnlockedItemList[i].AvatarPart);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedItemList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userUnlockedItemList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedItemList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userUnlockedItemList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedItemList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userUnlockedItemList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userUnlockedItemList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserUnlockedItemModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserUnlockedItemModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserUnlockedItemModel>();
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

        public List<UserUnlockedItemModel> Update(string connectionString, UserUnlockedItemModel userUnlockedItem, UserUnlockedItemModel userUnlockedItemWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedItemModel> response = new List<UserUnlockedItemModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE av_user_unlocked_items SET ";
                if (!string.IsNullOrEmpty(userUnlockedItem.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userUnlockedItem.Id);
                }
                if (!string.IsNullOrEmpty(userUnlockedItem.UserId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = string.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userUnlockedItem.UserId);
                }
                if (!string.IsNullOrEmpty(userUnlockedItem.AvatarItemCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `avatar_item_code` = @avatarItemCode");
                    else
                        sql = string.Concat(sql, " `avatar_item_code` = @avatarItemCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@avatarItemCode", userUnlockedItem.AvatarItemCode);
                }
                if (!string.IsNullOrEmpty(userUnlockedItem.AvatarPart))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `avatar_part` = @avatarPart");
                    else
                        sql = string.Concat(sql, " `avatar_part` = @avatarPart");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@avatarPart", userUnlockedItem.AvatarPart);
                }
                if (!string.IsNullOrEmpty(userUnlockedItem.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userUnlockedItem.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userUnlockedItem.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userUnlockedItem.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userUnlockedItem.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userUnlockedItem.UpdatedAt);
                }
                if (userUnlockedItemWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(userUnlockedItemWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userUnlockedItemWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedItemWhere.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = string.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userUnlockedItemWhere.UserId);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedItemWhere.AvatarItemCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `avatar_item_code` = @avatarItemCodeWhere");
                        else
                            sql = string.Concat(sql, " `avatar_item_code` = @avatarItemCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@avatarItemCodeWhere", userUnlockedItemWhere.AvatarItemCode);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedItemWhere.AvatarPart))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `avatar_part` = @avatarPartWhere");
                        else
                            sql = string.Concat(sql, " `avatar_part` = @avatarPartWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@avatarPartWhere", userUnlockedItemWhere.AvatarPart);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedItemWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userUnlockedItemWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedItemWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userUnlockedItemWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedItemWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userUnlockedItemWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userUnlockedItem);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserUnlockedItemModel>();
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

        public List<UserUnlockedItemModel> Update(string connectionString, List<UserUnlockedItemModel> userUnlockedItemList, List<UserUnlockedItemModel> userUnlockedItemWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedItemModel> response = new List<UserUnlockedItemModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userUnlockedItemList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE av_user_unlocked_items SET ";
                                if (!string.IsNullOrEmpty(userUnlockedItemList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userUnlockedItemList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedItemList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = string.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userUnlockedItemList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedItemList[i].AvatarItemCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `avatar_item_code` = @avatarItemCode" + i);
                                    else
                                        sql = string.Concat(sql, " `avatar_item_code` = @avatarItemCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@avatarItemCode" + i, userUnlockedItemList[i].AvatarItemCode);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedItemList[i].AvatarPart))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `avatar_part` = @avatarPart" + i);
                                    else
                                        sql = string.Concat(sql, " `avatar_part` = @avatarPart" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@avatarPart" + i, userUnlockedItemList[i].AvatarPart);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedItemList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userUnlockedItemList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedItemList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userUnlockedItemList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedItemList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userUnlockedItemList[i].UpdatedAt);
                                }
                                if (userUnlockedItemWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(userUnlockedItemWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userUnlockedItemWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedItemWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userUnlockedItemWhereList[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedItemWhereList[i].AvatarItemCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `avatar_item_code` = @avatarItemCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `avatar_item_code` = @avatarItemCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@avatarItemCodeWhere" + i, userUnlockedItemWhereList[i].AvatarItemCode);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedItemWhereList[i].AvatarPart))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `avatar_part` = @avatarPartWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `avatar_part` = @avatarPartWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@avatarPartWhere" + i, userUnlockedItemWhereList[i].AvatarPart);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedItemWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userUnlockedItemWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedItemWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userUnlockedItemWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedItemWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userUnlockedItemWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userUnlockedItemList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserUnlockedItemModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserUnlockedItemModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserUnlockedItemModel>();
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

        public List<UserUnlockedItemModel> Delete(string connectionString, UserUnlockedItemModel userUnlockedItem)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedItemModel> response = new List<UserUnlockedItemModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM av_user_unlocked_items  ";
                if (userUnlockedItem != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(userUnlockedItem.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userUnlockedItem.Id);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedItem.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = string.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userUnlockedItem.UserId);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedItem.AvatarItemCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `avatar_item_code` = @avatarItemCode");
                        else
                            sql = string.Concat(sql, " `avatar_item_code` = @avatarItemCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@avatarItemCode", userUnlockedItem.AvatarItemCode);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedItem.AvatarPart))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `avatar_part` = @avatarPart");
                        else
                            sql = string.Concat(sql, " `avatar_part` = @avatarPart");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@avatarPart", userUnlockedItem.AvatarPart);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedItem.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userUnlockedItem.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedItem.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userUnlockedItem.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedItem.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userUnlockedItem.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userUnlockedItem);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserUnlockedItemModel>();
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

        public List<UserUnlockedItemModel> Delete(string connectionString, List<UserUnlockedItemModel> userUnlockedItem)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedItemModel> response = new List<UserUnlockedItemModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userUnlockedItem.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM av_user_unlocked_items  ";
                                if (userUnlockedItem[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(userUnlockedItem[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userUnlockedItem[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedItem[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userUnlockedItem[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedItem[i].AvatarItemCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `avatar_item_code` = @avatarItemCode" + i);
                                        else
                                            sql = string.Concat(sql, " `avatar_item_code` = @avatarItemCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@avatarItemCode" + i, userUnlockedItem[i].AvatarItemCode);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedItem[i].AvatarPart))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `avatar_part` = @avatarPart" + i);
                                        else
                                            sql = string.Concat(sql, " `avatar_part` = @avatarPart" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@avatarPart" + i, userUnlockedItem[i].AvatarPart);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedItem[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userUnlockedItem[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedItem[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userUnlockedItem[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedItem[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userUnlockedItem[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userUnlockedItem[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserUnlockedItemModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserUnlockedItemModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserUnlockedItemModel>();
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
