using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSWebAPI.GSWebAPI.Domain.Models.AvatarV2;
using MySqlConnector;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.AvatarV2
{
    class UserV2AvatarDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, UserV2AvatarModel> ReadD(string connectionString, UserV2AvatarModel userV2Avatar, bool firstOne, bool orderByLastDate, UserV2AvatarModel userV2AvatarWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserV2AvatarModel> response = new Dictionary<string, UserV2AvatarModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `avatar_3d_json`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatars ";
                {
                    if (userV2Avatar != null)
                    {
                        if (!String.IsNullOrEmpty(userV2Avatar.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", userV2Avatar.Id);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userV2Avatar.UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.Avatar3dJson))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `avatar_3d_json` = @avatar3dJson ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `avatar_3d_json` = @avatar3dJson ");
                            }
                            cmd.Parameters.AddWithValue("@avatar3dJson", userV2Avatar.Avatar3dJson);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", userV2Avatar.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", userV2Avatar.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", userV2Avatar.UpdatedAt);
                        }
                    }
                    if (userV2AvatarWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userV2AvatarWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userV2AvatarWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.Avatar3dJson))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `avatar_3d_json` LIKE @avatar3dJsonWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `avatar_3d_json` LIKE @avatar3dJsonWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@avatar3dJsonWhereLike", "%" + userV2AvatarWhereLike.Avatar3dJson + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userV2AvatarWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userV2AvatarWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userV2AvatarWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = String.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                userV2Avatar = new UserV2AvatarModel();
                                if (!reader.IsDBNull(0))
                                    userV2Avatar.Id = reader.GetValue(0).ToString();
                                else
                                    userV2Avatar.Id = null;
                                if (!reader.IsDBNull(1))
                                    userV2Avatar.UserId = reader.GetValue(1).ToString();
                                else
                                    userV2Avatar.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userV2Avatar.Avatar3dJson = reader.GetValue(2).ToString();
                                else
                                    userV2Avatar.Avatar3dJson = null;
                                if (!reader.IsDBNull(3))
                                    userV2Avatar.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    userV2Avatar.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    userV2Avatar.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    userV2Avatar.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    userV2Avatar.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    userV2Avatar.UpdatedAt = null;
                                response.Add(userV2Avatar.Id, userV2Avatar);
                                userV2Avatar = new UserV2AvatarModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserV2AvatarModel>();
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
                response = new Dictionary<string, UserV2AvatarModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserV2AvatarModel> ReadD(string connectionString, UserV2AvatarModel userV2Avatar, bool orderByLastDate, int fromRow, int limit, UserV2AvatarModel userV2AvatarWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserV2AvatarModel> response = new Dictionary<string, UserV2AvatarModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `avatar_3d_json`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatars ";
                {
                    if (userV2Avatar != null)
                    {
                        if (!String.IsNullOrEmpty(userV2Avatar.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", userV2Avatar.Id);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userV2Avatar.UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.Avatar3dJson))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `avatar_3d_json` = @avatar3dJson ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `avatar_3d_json` = @avatar3dJson ");
                            }
                            cmd.Parameters.AddWithValue("@avatar3dJson", userV2Avatar.Avatar3dJson);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", userV2Avatar.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", userV2Avatar.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", userV2Avatar.UpdatedAt);
                        }
                    }
                    if (userV2AvatarWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userV2AvatarWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userV2AvatarWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.Avatar3dJson))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `avatar_3d_json` LIKE @avatar3dJsonWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `avatar_3d_json` LIKE @avatar3dJsonWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@avatar3dJsonWhereLike", "%" + userV2AvatarWhereLike.Avatar3dJson + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userV2AvatarWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userV2AvatarWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userV2AvatarWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    sql = String.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                userV2Avatar = new UserV2AvatarModel();
                                if (!reader.IsDBNull(0))
                                    userV2Avatar.Id = reader.GetValue(0).ToString();
                                else
                                    userV2Avatar.Id = null;
                                if (!reader.IsDBNull(1))
                                    userV2Avatar.UserId = reader.GetValue(1).ToString();
                                else
                                    userV2Avatar.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userV2Avatar.Avatar3dJson = reader.GetValue(2).ToString();
                                else
                                    userV2Avatar.Avatar3dJson = null;
                                if (!reader.IsDBNull(3))
                                    userV2Avatar.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    userV2Avatar.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    userV2Avatar.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    userV2Avatar.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    userV2Avatar.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    userV2Avatar.UpdatedAt = null;
                                response.Add(userV2Avatar.Id, userV2Avatar);
                                userV2Avatar = new UserV2AvatarModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserV2AvatarModel>();
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
                response = new Dictionary<string, UserV2AvatarModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserV2AvatarModel> ReadD(string connectionString, List<UserV2AvatarModel> userV2AvatarWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserV2AvatarModel> response = new Dictionary<string, UserV2AvatarModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `avatar_3d_json`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatars ";
                if (userV2AvatarWhere != null && userV2AvatarWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userV2AvatarWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userV2AvatarWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userV2AvatarWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].Avatar3dJson))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `avatar_3d_json` = @avatar3dJson" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@avatar3dJson" + i, userV2AvatarWhere[i].Avatar3dJson);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userV2AvatarWhere.Count - 1)
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
                            UserV2AvatarModel userV2Avatar = new UserV2AvatarModel(); if (!reader.IsDBNull(0))
                                userV2Avatar.Id = reader.GetValue(0).ToString();
                            else
                                userV2Avatar.Id = null;
                            if (!reader.IsDBNull(1))
                                userV2Avatar.UserId = reader.GetValue(1).ToString();
                            else
                                userV2Avatar.UserId = null;
                            if (!reader.IsDBNull(2))
                                userV2Avatar.Avatar3dJson = reader.GetValue(2).ToString();
                            else
                                userV2Avatar.Avatar3dJson = null;
                            if (!reader.IsDBNull(3))
                                userV2Avatar.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                userV2Avatar.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                userV2Avatar.CreatedAt = reader.GetValue(4).ToString();
                            else
                                userV2Avatar.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                userV2Avatar.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                userV2Avatar.UpdatedAt = null;
                            response.Add(userV2Avatar.Id, userV2Avatar);
                            userV2Avatar = new UserV2AvatarModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserV2AvatarModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserV2AvatarModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserV2AvatarModel> ReadD(string connectionString, List<UserV2AvatarModel> userV2AvatarWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserV2AvatarModel> response = new Dictionary<string, UserV2AvatarModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `avatar_3d_json`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatars ";
                if (userV2AvatarWhere != null && userV2AvatarWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userV2AvatarWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userV2AvatarWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userV2AvatarWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].Avatar3dJson))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `avatar_3d_json` = @avatar3dJson" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@avatar3dJson" + i, userV2AvatarWhere[i].Avatar3dJson);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userV2AvatarWhere.Count - 1)
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
                            UserV2AvatarModel userV2Avatar = new UserV2AvatarModel(); if (!reader.IsDBNull(0))
                                userV2Avatar.Id = reader.GetValue(0).ToString();
                            else
                                userV2Avatar.Id = null;
                            if (!reader.IsDBNull(1))
                                userV2Avatar.UserId = reader.GetValue(1).ToString();
                            else
                                userV2Avatar.UserId = null;
                            if (!reader.IsDBNull(2))
                                userV2Avatar.Avatar3dJson = reader.GetValue(2).ToString();
                            else
                                userV2Avatar.Avatar3dJson = null;
                            if (!reader.IsDBNull(3))
                                userV2Avatar.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                userV2Avatar.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                userV2Avatar.CreatedAt = reader.GetValue(4).ToString();
                            else
                                userV2Avatar.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                userV2Avatar.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                userV2Avatar.UpdatedAt = null;
                            response.Add(userV2Avatar.Id, userV2Avatar);
                            userV2Avatar = new UserV2AvatarModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserV2AvatarModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserV2AvatarModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserV2AvatarModel> Read(string connectionString, UserV2AvatarModel userV2Avatar, bool firstOne, bool orderByLastDate, UserV2AvatarModel userV2AvatarWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarModel> response = new List<UserV2AvatarModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `avatar_3d_json`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatars ";
                {
                    if (userV2Avatar != null)
                    {
                        if (!String.IsNullOrEmpty(userV2Avatar.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", userV2Avatar.Id);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userV2Avatar.UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.Avatar3dJson))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `avatar_3d_json` = @avatar3dJson ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `avatar_3d_json` = @avatar3dJson ");
                            }
                            cmd.Parameters.AddWithValue("@avatar3dJson", userV2Avatar.Avatar3dJson);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", userV2Avatar.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", userV2Avatar.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", userV2Avatar.UpdatedAt);
                        }
                    }
                    if (userV2AvatarWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userV2AvatarWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userV2AvatarWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.Avatar3dJson))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `avatar_3d_json` LIKE @avatar3dJsonWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `avatar_3d_json` LIKE @avatar3dJsonWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@avatar3dJsonWhereLike", "%" + userV2AvatarWhereLike.Avatar3dJson + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userV2AvatarWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userV2AvatarWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userV2AvatarWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = String.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                userV2Avatar = new UserV2AvatarModel();
                                if (!reader.IsDBNull(0))
                                    userV2Avatar.Id = reader.GetValue(0).ToString();
                                else
                                    userV2Avatar.Id = null;
                                if (!reader.IsDBNull(1))
                                    userV2Avatar.UserId = reader.GetValue(1).ToString();
                                else
                                    userV2Avatar.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userV2Avatar.Avatar3dJson = reader.GetValue(2).ToString();
                                else
                                    userV2Avatar.Avatar3dJson = null;
                                if (!reader.IsDBNull(3))
                                    userV2Avatar.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    userV2Avatar.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    userV2Avatar.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    userV2Avatar.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    userV2Avatar.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    userV2Avatar.UpdatedAt = null;
                                response.Add(userV2Avatar);
                                userV2Avatar = new UserV2AvatarModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserV2AvatarModel>();
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
                response = new List<UserV2AvatarModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserV2AvatarModel> Read(string connectionString, UserV2AvatarModel userV2Avatar, bool orderByLastDate, int fromRow, int limit, UserV2AvatarModel userV2AvatarWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarModel> response = new List<UserV2AvatarModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `avatar_3d_json`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatars ";
                {
                    if (userV2Avatar != null)
                    {
                        if (!String.IsNullOrEmpty(userV2Avatar.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", userV2Avatar.Id);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userV2Avatar.UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.Avatar3dJson))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `avatar_3d_json` = @avatar3dJson ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `avatar_3d_json` = @avatar3dJson ");
                            }
                            cmd.Parameters.AddWithValue("@avatar3dJson", userV2Avatar.Avatar3dJson);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", userV2Avatar.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", userV2Avatar.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2Avatar.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", userV2Avatar.UpdatedAt);
                        }
                    }
                    if (userV2AvatarWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userV2AvatarWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userV2AvatarWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.Avatar3dJson))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `avatar_3d_json` LIKE @avatar3dJsonWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `avatar_3d_json` LIKE @avatar3dJsonWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@avatar3dJsonWhereLike", "%" + userV2AvatarWhereLike.Avatar3dJson + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userV2AvatarWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userV2AvatarWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userV2AvatarWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    sql = String.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                userV2Avatar = new UserV2AvatarModel();
                                if (!reader.IsDBNull(0))
                                    userV2Avatar.Id = reader.GetValue(0).ToString();
                                else
                                    userV2Avatar.Id = null;
                                if (!reader.IsDBNull(1))
                                    userV2Avatar.UserId = reader.GetValue(1).ToString();
                                else
                                    userV2Avatar.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userV2Avatar.Avatar3dJson = reader.GetValue(2).ToString();
                                else
                                    userV2Avatar.Avatar3dJson = null;
                                if (!reader.IsDBNull(3))
                                    userV2Avatar.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    userV2Avatar.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    userV2Avatar.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    userV2Avatar.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    userV2Avatar.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    userV2Avatar.UpdatedAt = null;
                                response.Add(userV2Avatar);
                                userV2Avatar = new UserV2AvatarModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserV2AvatarModel>();
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
                response = new List<UserV2AvatarModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserV2AvatarModel> Read(string connectionString, List<UserV2AvatarModel> userV2AvatarWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarModel> response = new List<UserV2AvatarModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `avatar_3d_json`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatars ";
                if (userV2AvatarWhere != null && userV2AvatarWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userV2AvatarWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userV2AvatarWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userV2AvatarWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].Avatar3dJson))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `avatar_3d_json` = @avatar3dJson" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@avatar3dJson" + i, userV2AvatarWhere[i].Avatar3dJson);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userV2AvatarWhere.Count - 1)
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
                            UserV2AvatarModel userV2Avatar = new UserV2AvatarModel(); if (!reader.IsDBNull(0))
                                userV2Avatar.Id = reader.GetValue(0).ToString();
                            else
                                userV2Avatar.Id = null;
                            if (!reader.IsDBNull(1))
                                userV2Avatar.UserId = reader.GetValue(1).ToString();
                            else
                                userV2Avatar.UserId = null;
                            if (!reader.IsDBNull(2))
                                userV2Avatar.Avatar3dJson = reader.GetValue(2).ToString();
                            else
                                userV2Avatar.Avatar3dJson = null;
                            if (!reader.IsDBNull(3))
                                userV2Avatar.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                userV2Avatar.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                userV2Avatar.CreatedAt = reader.GetValue(4).ToString();
                            else
                                userV2Avatar.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                userV2Avatar.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                userV2Avatar.UpdatedAt = null;
                            response.Add(userV2Avatar);
                            userV2Avatar = new UserV2AvatarModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserV2AvatarModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserV2AvatarModel> Read(string connectionString, List<UserV2AvatarModel> userV2AvatarWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarModel> response = new List<UserV2AvatarModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `avatar_3d_json`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatars ";
                if (userV2AvatarWhere != null && userV2AvatarWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userV2AvatarWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userV2AvatarWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userV2AvatarWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].Avatar3dJson))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `avatar_3d_json` = @avatar3dJson" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@avatar3dJson" + i, userV2AvatarWhere[i].Avatar3dJson);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userV2AvatarWhere.Count - 1)
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
                            UserV2AvatarModel userV2Avatar = new UserV2AvatarModel(); if (!reader.IsDBNull(0))
                                userV2Avatar.Id = reader.GetValue(0).ToString();
                            else
                                userV2Avatar.Id = null;
                            if (!reader.IsDBNull(1))
                                userV2Avatar.UserId = reader.GetValue(1).ToString();
                            else
                                userV2Avatar.UserId = null;
                            if (!reader.IsDBNull(2))
                                userV2Avatar.Avatar3dJson = reader.GetValue(2).ToString();
                            else
                                userV2Avatar.Avatar3dJson = null;
                            if (!reader.IsDBNull(3))
                                userV2Avatar.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                userV2Avatar.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                userV2Avatar.CreatedAt = reader.GetValue(4).ToString();
                            else
                                userV2Avatar.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                userV2Avatar.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                userV2Avatar.UpdatedAt = null;
                            response.Add(userV2Avatar);
                            userV2Avatar = new UserV2AvatarModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserV2AvatarModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserV2AvatarModel> Create(string connectionString, UserV2AvatarModel userV2Avatar)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarModel> response = new List<UserV2AvatarModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO user_v_2_avatars (";
                string values = "(";
                if (!String.IsNullOrEmpty(userV2Avatar.Id))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `id` ");
                        values = String.Concat(values, ", @id");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `id` ");
                        values = String.Concat(values, " @id");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userV2Avatar.Id);
                }
                if (!String.IsNullOrEmpty(userV2Avatar.UserId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `user_id` ");
                        values = String.Concat(values, ", @userId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `user_id` ");
                        values = String.Concat(values, " @userId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userV2Avatar.UserId);
                }
                if (!String.IsNullOrEmpty(userV2Avatar.Avatar3dJson))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `avatar_3d_json` ");
                        values = String.Concat(values, ", @avatar3dJson");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `avatar_3d_json` ");
                        values = String.Concat(values, " @avatar3dJson");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@avatar3dJson", userV2Avatar.Avatar3dJson);
                }
                if (!String.IsNullOrEmpty(userV2Avatar.ModifiedBy))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `modified_by` ");
                        values = String.Concat(values, ", @modifiedBy");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `modified_by` ");
                        values = String.Concat(values, " @modifiedBy");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userV2Avatar.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userV2Avatar.CreatedAt))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `created_at` ");
                        values = String.Concat(values, ", @createdAt");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `created_at` ");
                        values = String.Concat(values, " @createdAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userV2Avatar.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userV2Avatar.UpdatedAt))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `updated_at` ");
                        values = String.Concat(values, ", @updatedAt");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `updated_at` ");
                        values = String.Concat(values, " @updatedAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userV2Avatar.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userV2Avatar.Id = cmd.LastInsertedId.ToString();
                response.Add(userV2Avatar);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarModel>();
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
        public List<UserV2AvatarModel> Create(string connectionString, List<UserV2AvatarModel> userV2AvatarList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarModel> response = new List<UserV2AvatarModel>();
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
                        for (int i = 0; i < userV2AvatarList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO user_v_2_avatars (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(userV2AvatarList[i].Id))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `id` ");
                                        values = String.Concat(values, ", @id" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `id` ");
                                        values = String.Concat(values, "@id" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userV2AvatarList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarList[i].UserId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `user_id` ");
                                        values = String.Concat(values, ", @userId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `user_id` ");
                                        values = String.Concat(values, "@userId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userV2AvatarList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarList[i].Avatar3dJson))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `avatar_3d_json` ");
                                        values = String.Concat(values, ", @avatar3dJson" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `avatar_3d_json` ");
                                        values = String.Concat(values, "@avatar3dJson" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@avatar3dJson" + i, userV2AvatarList[i].Avatar3dJson);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `modified_by` ");
                                        values = String.Concat(values, ", @modifiedBy" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `modified_by` ");
                                        values = String.Concat(values, "@modifiedBy" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarList[i].CreatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `created_at` ");
                                        values = String.Concat(values, ", @createdAt" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `created_at` ");
                                        values = String.Concat(values, "@createdAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `updated_at` ");
                                        values = String.Concat(values, ", @updatedAt" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `updated_at` ");
                                        values = String.Concat(values, "@updatedAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userV2AvatarList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserV2AvatarModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserV2AvatarModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarModel>();
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

        public List<UserV2AvatarModel> Update(string connectionString, UserV2AvatarModel userV2Avatar, UserV2AvatarModel userV2AvatarWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarModel> response = new List<UserV2AvatarModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE user_v_2_avatars SET ";
                if (!String.IsNullOrEmpty(userV2Avatar.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userV2Avatar.Id);
                }
                if (!String.IsNullOrEmpty(userV2Avatar.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userV2Avatar.UserId);
                }
                if (!String.IsNullOrEmpty(userV2Avatar.Avatar3dJson))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `avatar_3d_json` = @avatar3dJson");
                    else
                        sql = String.Concat(sql, " `avatar_3d_json` = @avatar3dJson");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@avatar3dJson", userV2Avatar.Avatar3dJson);
                }
                if (!String.IsNullOrEmpty(userV2Avatar.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userV2Avatar.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userV2Avatar.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userV2Avatar.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userV2Avatar.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userV2Avatar.UpdatedAt);
                }
                if (userV2AvatarWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(userV2AvatarWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userV2AvatarWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userV2AvatarWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarWhere.Avatar3dJson))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `avatar_3d_json` = @avatar3dJsonWhere");
                        else
                            sql = String.Concat(sql, " `avatar_3d_json` = @avatar3dJsonWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@avatar3dJsonWhere", userV2AvatarWhere.Avatar3dJson);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userV2AvatarWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userV2AvatarWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userV2AvatarWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userV2Avatar);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarModel>();
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

        public List<UserV2AvatarModel> Update(string connectionString, List<UserV2AvatarModel> userV2AvatarList, List<UserV2AvatarModel> userV2AvatarWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarModel> response = new List<UserV2AvatarModel>();
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
                        for (int i = 0; i < userV2AvatarList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE user_v_2_avatars SET ";
                                if (!String.IsNullOrEmpty(userV2AvatarList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userV2AvatarList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userV2AvatarList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarList[i].Avatar3dJson))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `avatar_3d_json` = @avatar3dJson" + i);
                                    else
                                        sql = String.Concat(sql, " `avatar_3d_json` = @avatar3dJson" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@avatar3dJson" + i, userV2AvatarList[i].Avatar3dJson);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarList[i].UpdatedAt);
                                }
                                if (userV2AvatarWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(userV2AvatarWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userV2AvatarWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userV2AvatarWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarWhereList[i].Avatar3dJson))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `avatar_3d_json` = @avatar3dJsonWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `avatar_3d_json` = @avatar3dJsonWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@avatar3dJsonWhere" + i, userV2AvatarWhereList[i].Avatar3dJson);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userV2AvatarWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userV2AvatarWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userV2AvatarWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userV2AvatarList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserV2AvatarModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserV2AvatarModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarModel>();
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

        public List<UserV2AvatarModel> Delete(string connectionString, UserV2AvatarModel userV2Avatar)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarModel> response = new List<UserV2AvatarModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM user_v_2_avatars  ";
                if (userV2Avatar != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(userV2Avatar.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userV2Avatar.Id);
                    }
                    if (!String.IsNullOrEmpty(userV2Avatar.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userV2Avatar.UserId);
                    }
                    if (!String.IsNullOrEmpty(userV2Avatar.Avatar3dJson))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `avatar_3d_json` = @avatar3dJson");
                        else
                            sql = String.Concat(sql, " `avatar_3d_json` = @avatar3dJson");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@avatar3dJson", userV2Avatar.Avatar3dJson);
                    }
                    if (!String.IsNullOrEmpty(userV2Avatar.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userV2Avatar.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userV2Avatar.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userV2Avatar.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userV2Avatar.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userV2Avatar.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userV2Avatar);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarModel>();
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

        public List<UserV2AvatarModel> Delete(string connectionString, List<UserV2AvatarModel> userV2Avatar)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarModel> response = new List<UserV2AvatarModel>();
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
                        for (int i = 0; i < userV2Avatar.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM user_v_2_avatars  ";
                                if (userV2Avatar[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(userV2Avatar[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userV2Avatar[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userV2Avatar[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userV2Avatar[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userV2Avatar[i].Avatar3dJson))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `avatar_3d_json` = @avatar3dJson" + i);
                                        else
                                            sql = String.Concat(sql, " `avatar_3d_json` = @avatar3dJson" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@avatar3dJson" + i, userV2Avatar[i].Avatar3dJson);
                                    }
                                    if (!String.IsNullOrEmpty(userV2Avatar[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2Avatar[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userV2Avatar[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userV2Avatar[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userV2Avatar[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userV2Avatar[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userV2Avatar[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserV2AvatarModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserV2AvatarModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarModel>();
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