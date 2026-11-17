using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSWebAPI.GSWebAPI.Domain.Models.AvatarV2;
using MySqlConnector;


namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.AvatarV2
{
    class UserV2AvatarUnlockDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, UserV2AvatarUnlockModel> ReadD(string connectionString, UserV2AvatarUnlockModel userV2AvatarUnlock, bool firstOne, bool orderByLastDate, UserV2AvatarUnlockModel userV2AvatarUnlockWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserV2AvatarUnlockModel> response = new Dictionary<string, UserV2AvatarUnlockModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `gender`, `part`, `part_index`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_unlocks ";
                {
                    if (userV2AvatarUnlock != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.Id))
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
                            cmd.Parameters.AddWithValue("@id", userV2AvatarUnlock.Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userV2AvatarUnlock.UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.Gender))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `gender` = @gender ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `gender` = @gender ");
                            }
                            cmd.Parameters.AddWithValue("@gender", userV2AvatarUnlock.Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.Part))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `part` = @part ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `part` = @part ");
                            }
                            cmd.Parameters.AddWithValue("@part", userV2AvatarUnlock.Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.PartIndex))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `part_index` = @partIndex ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `part_index` = @partIndex ");
                            }
                            cmd.Parameters.AddWithValue("@partIndex", userV2AvatarUnlock.PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userV2AvatarUnlock.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userV2AvatarUnlock.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userV2AvatarUnlock.UpdatedAt);
                        }
                    }
                    if (userV2AvatarUnlockWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userV2AvatarUnlockWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userV2AvatarUnlockWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.Gender))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `gender` LIKE @genderWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `gender` LIKE @genderWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@genderWhereLike", "%" + userV2AvatarUnlockWhereLike.Gender + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.Part))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `part` LIKE @partWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `part` LIKE @partWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@partWhereLike", "%" + userV2AvatarUnlockWhereLike.Part + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.PartIndex))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `part_index` LIKE @partIndexWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `part_index` LIKE @partIndexWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@partIndexWhereLike", "%" + userV2AvatarUnlockWhereLike.PartIndex + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userV2AvatarUnlockWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userV2AvatarUnlockWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userV2AvatarUnlockWhereLike.UpdatedAt + "%");
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
                                userV2AvatarUnlock = new UserV2AvatarUnlockModel();
                                if (!reader.IsDBNull(0))
                                    userV2AvatarUnlock.Id = reader.GetValue(0).ToString();
                                else
                                    userV2AvatarUnlock.Id = null;
                                if (!reader.IsDBNull(1))
                                    userV2AvatarUnlock.UserId = reader.GetValue(1).ToString();
                                else
                                    userV2AvatarUnlock.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userV2AvatarUnlock.Gender = reader.GetValue(2).ToString();
                                else
                                    userV2AvatarUnlock.Gender = null;
                                if (!reader.IsDBNull(3))
                                    userV2AvatarUnlock.Part = reader.GetValue(3).ToString();
                                else
                                    userV2AvatarUnlock.Part = null;
                                if (!reader.IsDBNull(4))
                                    userV2AvatarUnlock.PartIndex = reader.GetValue(4).ToString();
                                else
                                    userV2AvatarUnlock.PartIndex = null;
                                if (!reader.IsDBNull(5))
                                    userV2AvatarUnlock.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userV2AvatarUnlock.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userV2AvatarUnlock.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userV2AvatarUnlock.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userV2AvatarUnlock.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userV2AvatarUnlock.UpdatedAt = null;
                                response.Add(userV2AvatarUnlock.Id, userV2AvatarUnlock);
                                userV2AvatarUnlock = new UserV2AvatarUnlockModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserV2AvatarUnlockModel>();
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
                response = new Dictionary<string, UserV2AvatarUnlockModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserV2AvatarUnlockModel> ReadD(string connectionString, UserV2AvatarUnlockModel userV2AvatarUnlock, bool orderByLastDate, int fromRow, int limit, UserV2AvatarUnlockModel userV2AvatarUnlockWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserV2AvatarUnlockModel> response = new Dictionary<string, UserV2AvatarUnlockModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `gender`, `part`, `part_index`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_unlocks ";
                {
                    if (userV2AvatarUnlock != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.Id))
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
                            cmd.Parameters.AddWithValue("@id", userV2AvatarUnlock.Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userV2AvatarUnlock.UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.Gender))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `gender` = @gender ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `gender` = @gender ");
                            }
                            cmd.Parameters.AddWithValue("@gender", userV2AvatarUnlock.Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.Part))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `part` = @part ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `part` = @part ");
                            }
                            cmd.Parameters.AddWithValue("@part", userV2AvatarUnlock.Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.PartIndex))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `part_index` = @partIndex ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `part_index` = @partIndex ");
                            }
                            cmd.Parameters.AddWithValue("@partIndex", userV2AvatarUnlock.PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userV2AvatarUnlock.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userV2AvatarUnlock.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userV2AvatarUnlock.UpdatedAt);
                        }
                    }
                    if (userV2AvatarUnlockWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userV2AvatarUnlockWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userV2AvatarUnlockWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.Gender))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `gender` LIKE @genderWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `gender` LIKE @genderWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@genderWhereLike", "%" + userV2AvatarUnlockWhereLike.Gender + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.Part))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `part` LIKE @partWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `part` LIKE @partWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@partWhereLike", "%" + userV2AvatarUnlockWhereLike.Part + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.PartIndex))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `part_index` LIKE @partIndexWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `part_index` LIKE @partIndexWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@partIndexWhereLike", "%" + userV2AvatarUnlockWhereLike.PartIndex + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userV2AvatarUnlockWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userV2AvatarUnlockWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userV2AvatarUnlockWhereLike.UpdatedAt + "%");
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
                                userV2AvatarUnlock = new UserV2AvatarUnlockModel();
                                if (!reader.IsDBNull(0))
                                    userV2AvatarUnlock.Id = reader.GetValue(0).ToString();
                                else
                                    userV2AvatarUnlock.Id = null;
                                if (!reader.IsDBNull(1))
                                    userV2AvatarUnlock.UserId = reader.GetValue(1).ToString();
                                else
                                    userV2AvatarUnlock.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userV2AvatarUnlock.Gender = reader.GetValue(2).ToString();
                                else
                                    userV2AvatarUnlock.Gender = null;
                                if (!reader.IsDBNull(3))
                                    userV2AvatarUnlock.Part = reader.GetValue(3).ToString();
                                else
                                    userV2AvatarUnlock.Part = null;
                                if (!reader.IsDBNull(4))
                                    userV2AvatarUnlock.PartIndex = reader.GetValue(4).ToString();
                                else
                                    userV2AvatarUnlock.PartIndex = null;
                                if (!reader.IsDBNull(5))
                                    userV2AvatarUnlock.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userV2AvatarUnlock.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userV2AvatarUnlock.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userV2AvatarUnlock.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userV2AvatarUnlock.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userV2AvatarUnlock.UpdatedAt = null;
                                response.Add(userV2AvatarUnlock.Id, userV2AvatarUnlock);
                                userV2AvatarUnlock = new UserV2AvatarUnlockModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserV2AvatarUnlockModel>();
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
                response = new Dictionary<string, UserV2AvatarUnlockModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserV2AvatarUnlockModel> ReadD(string connectionString, List<UserV2AvatarUnlockModel> userV2AvatarUnlockWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserV2AvatarUnlockModel> response = new Dictionary<string, UserV2AvatarUnlockModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `gender`, `part`, `part_index`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_unlocks ";
                if (userV2AvatarUnlockWhere != null && userV2AvatarUnlockWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userV2AvatarUnlockWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userV2AvatarUnlockWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userV2AvatarUnlockWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].Gender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gender` = @gender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gender" + i, userV2AvatarUnlockWhere[i].Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].Part))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part` = @part" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@part" + i, userV2AvatarUnlockWhere[i].Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].PartIndex))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part_index` = @partIndex" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@partIndex" + i, userV2AvatarUnlockWhere[i].PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarUnlockWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarUnlockWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarUnlockWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userV2AvatarUnlockWhere.Count - 1)
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
                            UserV2AvatarUnlockModel userV2AvatarUnlock = new UserV2AvatarUnlockModel(); if (!reader.IsDBNull(0))
                                userV2AvatarUnlock.Id = reader.GetValue(0).ToString();
                            else
                                userV2AvatarUnlock.Id = null;
                            if (!reader.IsDBNull(1))
                                userV2AvatarUnlock.UserId = reader.GetValue(1).ToString();
                            else
                                userV2AvatarUnlock.UserId = null;
                            if (!reader.IsDBNull(2))
                                userV2AvatarUnlock.Gender = reader.GetValue(2).ToString();
                            else
                                userV2AvatarUnlock.Gender = null;
                            if (!reader.IsDBNull(3))
                                userV2AvatarUnlock.Part = reader.GetValue(3).ToString();
                            else
                                userV2AvatarUnlock.Part = null;
                            if (!reader.IsDBNull(4))
                                userV2AvatarUnlock.PartIndex = reader.GetValue(4).ToString();
                            else
                                userV2AvatarUnlock.PartIndex = null;
                            if (!reader.IsDBNull(5))
                                userV2AvatarUnlock.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                userV2AvatarUnlock.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                userV2AvatarUnlock.CreatedAt = reader.GetValue(6).ToString();
                            else
                                userV2AvatarUnlock.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                userV2AvatarUnlock.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                userV2AvatarUnlock.UpdatedAt = null;
                            response.Add(userV2AvatarUnlock.Id, userV2AvatarUnlock);
                            userV2AvatarUnlock = new UserV2AvatarUnlockModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserV2AvatarUnlockModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserV2AvatarUnlockModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserV2AvatarUnlockModel> ReadD(string connectionString, List<UserV2AvatarUnlockModel> userV2AvatarUnlockWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserV2AvatarUnlockModel> response = new Dictionary<string, UserV2AvatarUnlockModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `gender`, `part`, `part_index`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_unlocks ";
                if (userV2AvatarUnlockWhere != null && userV2AvatarUnlockWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userV2AvatarUnlockWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userV2AvatarUnlockWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userV2AvatarUnlockWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].Gender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gender` = @gender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gender" + i, userV2AvatarUnlockWhere[i].Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].Part))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part` = @part" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@part" + i, userV2AvatarUnlockWhere[i].Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].PartIndex))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part_index` = @partIndex" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@partIndex" + i, userV2AvatarUnlockWhere[i].PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarUnlockWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarUnlockWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarUnlockWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userV2AvatarUnlockWhere.Count - 1)
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
                            UserV2AvatarUnlockModel userV2AvatarUnlock = new UserV2AvatarUnlockModel(); if (!reader.IsDBNull(0))
                                userV2AvatarUnlock.Id = reader.GetValue(0).ToString();
                            else
                                userV2AvatarUnlock.Id = null;
                            if (!reader.IsDBNull(1))
                                userV2AvatarUnlock.UserId = reader.GetValue(1).ToString();
                            else
                                userV2AvatarUnlock.UserId = null;
                            if (!reader.IsDBNull(2))
                                userV2AvatarUnlock.Gender = reader.GetValue(2).ToString();
                            else
                                userV2AvatarUnlock.Gender = null;
                            if (!reader.IsDBNull(3))
                                userV2AvatarUnlock.Part = reader.GetValue(3).ToString();
                            else
                                userV2AvatarUnlock.Part = null;
                            if (!reader.IsDBNull(4))
                                userV2AvatarUnlock.PartIndex = reader.GetValue(4).ToString();
                            else
                                userV2AvatarUnlock.PartIndex = null;
                            if (!reader.IsDBNull(5))
                                userV2AvatarUnlock.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                userV2AvatarUnlock.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                userV2AvatarUnlock.CreatedAt = reader.GetValue(6).ToString();
                            else
                                userV2AvatarUnlock.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                userV2AvatarUnlock.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                userV2AvatarUnlock.UpdatedAt = null;
                            response.Add(userV2AvatarUnlock.Id, userV2AvatarUnlock);
                            userV2AvatarUnlock = new UserV2AvatarUnlockModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserV2AvatarUnlockModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserV2AvatarUnlockModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserV2AvatarUnlockModel> Read(string connectionString, UserV2AvatarUnlockModel userV2AvatarUnlock, bool firstOne, bool orderByLastDate, UserV2AvatarUnlockModel userV2AvatarUnlockWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarUnlockModel> response = new List<UserV2AvatarUnlockModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `gender`, `part`, `part_index`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_unlocks ";
                {
                    if (userV2AvatarUnlock != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.Id))
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
                            cmd.Parameters.AddWithValue("@id", userV2AvatarUnlock.Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userV2AvatarUnlock.UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.Gender))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `gender` = @gender ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `gender` = @gender ");
                            }
                            cmd.Parameters.AddWithValue("@gender", userV2AvatarUnlock.Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.Part))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `part` = @part ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `part` = @part ");
                            }
                            cmd.Parameters.AddWithValue("@part", userV2AvatarUnlock.Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.PartIndex))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `part_index` = @partIndex ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `part_index` = @partIndex ");
                            }
                            cmd.Parameters.AddWithValue("@partIndex", userV2AvatarUnlock.PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userV2AvatarUnlock.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userV2AvatarUnlock.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userV2AvatarUnlock.UpdatedAt);
                        }
                    }
                    if (userV2AvatarUnlockWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userV2AvatarUnlockWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userV2AvatarUnlockWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.Gender))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `gender` LIKE @genderWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `gender` LIKE @genderWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@genderWhereLike", "%" + userV2AvatarUnlockWhereLike.Gender + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.Part))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `part` LIKE @partWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `part` LIKE @partWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@partWhereLike", "%" + userV2AvatarUnlockWhereLike.Part + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.PartIndex))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `part_index` LIKE @partIndexWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `part_index` LIKE @partIndexWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@partIndexWhereLike", "%" + userV2AvatarUnlockWhereLike.PartIndex + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userV2AvatarUnlockWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userV2AvatarUnlockWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userV2AvatarUnlockWhereLike.UpdatedAt + "%");
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
                                userV2AvatarUnlock = new UserV2AvatarUnlockModel();
                                if (!reader.IsDBNull(0))
                                    userV2AvatarUnlock.Id = reader.GetValue(0).ToString();
                                else
                                    userV2AvatarUnlock.Id = null;
                                if (!reader.IsDBNull(1))
                                    userV2AvatarUnlock.UserId = reader.GetValue(1).ToString();
                                else
                                    userV2AvatarUnlock.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userV2AvatarUnlock.Gender = reader.GetValue(2).ToString();
                                else
                                    userV2AvatarUnlock.Gender = null;
                                if (!reader.IsDBNull(3))
                                    userV2AvatarUnlock.Part = reader.GetValue(3).ToString();
                                else
                                    userV2AvatarUnlock.Part = null;
                                if (!reader.IsDBNull(4))
                                    userV2AvatarUnlock.PartIndex = reader.GetValue(4).ToString();
                                else
                                    userV2AvatarUnlock.PartIndex = null;
                                if (!reader.IsDBNull(5))
                                    userV2AvatarUnlock.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userV2AvatarUnlock.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userV2AvatarUnlock.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userV2AvatarUnlock.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userV2AvatarUnlock.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userV2AvatarUnlock.UpdatedAt = null;
                                response.Add(userV2AvatarUnlock);
                                userV2AvatarUnlock = new UserV2AvatarUnlockModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserV2AvatarUnlockModel>();
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
                response = new List<UserV2AvatarUnlockModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserV2AvatarUnlockModel> Read(string connectionString, UserV2AvatarUnlockModel userV2AvatarUnlock, bool orderByLastDate, int fromRow, int limit, UserV2AvatarUnlockModel userV2AvatarUnlockWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarUnlockModel> response = new List<UserV2AvatarUnlockModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `gender`, `part`, `part_index`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_unlocks ";
                {
                    if (userV2AvatarUnlock != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.Id))
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
                            cmd.Parameters.AddWithValue("@id", userV2AvatarUnlock.Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userV2AvatarUnlock.UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.Gender))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `gender` = @gender ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `gender` = @gender ");
                            }
                            cmd.Parameters.AddWithValue("@gender", userV2AvatarUnlock.Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.Part))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `part` = @part ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `part` = @part ");
                            }
                            cmd.Parameters.AddWithValue("@part", userV2AvatarUnlock.Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.PartIndex))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `part_index` = @partIndex ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `part_index` = @partIndex ");
                            }
                            cmd.Parameters.AddWithValue("@partIndex", userV2AvatarUnlock.PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userV2AvatarUnlock.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userV2AvatarUnlock.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlock.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userV2AvatarUnlock.UpdatedAt);
                        }
                    }
                    if (userV2AvatarUnlockWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userV2AvatarUnlockWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userV2AvatarUnlockWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.Gender))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `gender` LIKE @genderWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `gender` LIKE @genderWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@genderWhereLike", "%" + userV2AvatarUnlockWhereLike.Gender + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.Part))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `part` LIKE @partWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `part` LIKE @partWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@partWhereLike", "%" + userV2AvatarUnlockWhereLike.Part + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.PartIndex))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `part_index` LIKE @partIndexWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `part_index` LIKE @partIndexWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@partIndexWhereLike", "%" + userV2AvatarUnlockWhereLike.PartIndex + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userV2AvatarUnlockWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userV2AvatarUnlockWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userV2AvatarUnlockWhereLike.UpdatedAt + "%");
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
                                userV2AvatarUnlock = new UserV2AvatarUnlockModel();
                                if (!reader.IsDBNull(0))
                                    userV2AvatarUnlock.Id = reader.GetValue(0).ToString();
                                else
                                    userV2AvatarUnlock.Id = null;
                                if (!reader.IsDBNull(1))
                                    userV2AvatarUnlock.UserId = reader.GetValue(1).ToString();
                                else
                                    userV2AvatarUnlock.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userV2AvatarUnlock.Gender = reader.GetValue(2).ToString();
                                else
                                    userV2AvatarUnlock.Gender = null;
                                if (!reader.IsDBNull(3))
                                    userV2AvatarUnlock.Part = reader.GetValue(3).ToString();
                                else
                                    userV2AvatarUnlock.Part = null;
                                if (!reader.IsDBNull(4))
                                    userV2AvatarUnlock.PartIndex = reader.GetValue(4).ToString();
                                else
                                    userV2AvatarUnlock.PartIndex = null;
                                if (!reader.IsDBNull(5))
                                    userV2AvatarUnlock.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userV2AvatarUnlock.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userV2AvatarUnlock.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userV2AvatarUnlock.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userV2AvatarUnlock.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userV2AvatarUnlock.UpdatedAt = null;
                                response.Add(userV2AvatarUnlock);
                                userV2AvatarUnlock = new UserV2AvatarUnlockModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserV2AvatarUnlockModel>();
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
                response = new List<UserV2AvatarUnlockModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserV2AvatarUnlockModel> Read(string connectionString, List<UserV2AvatarUnlockModel> userV2AvatarUnlockWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarUnlockModel> response = new List<UserV2AvatarUnlockModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `gender`, `part`, `part_index`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_unlocks ";
                if (userV2AvatarUnlockWhere != null && userV2AvatarUnlockWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userV2AvatarUnlockWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userV2AvatarUnlockWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userV2AvatarUnlockWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].Gender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gender` = @gender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gender" + i, userV2AvatarUnlockWhere[i].Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].Part))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part` = @part" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@part" + i, userV2AvatarUnlockWhere[i].Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].PartIndex))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part_index` = @partIndex" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@partIndex" + i, userV2AvatarUnlockWhere[i].PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarUnlockWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarUnlockWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarUnlockWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userV2AvatarUnlockWhere.Count - 1)
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
                            UserV2AvatarUnlockModel userV2AvatarUnlock = new UserV2AvatarUnlockModel(); if (!reader.IsDBNull(0))
                                userV2AvatarUnlock.Id = reader.GetValue(0).ToString();
                            else
                                userV2AvatarUnlock.Id = null;
                            if (!reader.IsDBNull(1))
                                userV2AvatarUnlock.UserId = reader.GetValue(1).ToString();
                            else
                                userV2AvatarUnlock.UserId = null;
                            if (!reader.IsDBNull(2))
                                userV2AvatarUnlock.Gender = reader.GetValue(2).ToString();
                            else
                                userV2AvatarUnlock.Gender = null;
                            if (!reader.IsDBNull(3))
                                userV2AvatarUnlock.Part = reader.GetValue(3).ToString();
                            else
                                userV2AvatarUnlock.Part = null;
                            if (!reader.IsDBNull(4))
                                userV2AvatarUnlock.PartIndex = reader.GetValue(4).ToString();
                            else
                                userV2AvatarUnlock.PartIndex = null;
                            if (!reader.IsDBNull(5))
                                userV2AvatarUnlock.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                userV2AvatarUnlock.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                userV2AvatarUnlock.CreatedAt = reader.GetValue(6).ToString();
                            else
                                userV2AvatarUnlock.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                userV2AvatarUnlock.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                userV2AvatarUnlock.UpdatedAt = null;
                            response.Add(userV2AvatarUnlock);
                            userV2AvatarUnlock = new UserV2AvatarUnlockModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserV2AvatarUnlockModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarUnlockModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserV2AvatarUnlockModel> Read(string connectionString, List<UserV2AvatarUnlockModel> userV2AvatarUnlockWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarUnlockModel> response = new List<UserV2AvatarUnlockModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `gender`, `part`, `part_index`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_unlocks ";
                if (userV2AvatarUnlockWhere != null && userV2AvatarUnlockWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userV2AvatarUnlockWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userV2AvatarUnlockWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userV2AvatarUnlockWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].Gender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gender` = @gender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gender" + i, userV2AvatarUnlockWhere[i].Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].Part))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part` = @part" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@part" + i, userV2AvatarUnlockWhere[i].Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].PartIndex))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part_index` = @partIndex" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@partIndex" + i, userV2AvatarUnlockWhere[i].PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarUnlockWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarUnlockWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarUnlockWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userV2AvatarUnlockWhere.Count - 1)
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
                            UserV2AvatarUnlockModel userV2AvatarUnlock = new UserV2AvatarUnlockModel(); if (!reader.IsDBNull(0))
                                userV2AvatarUnlock.Id = reader.GetValue(0).ToString();
                            else
                                userV2AvatarUnlock.Id = null;
                            if (!reader.IsDBNull(1))
                                userV2AvatarUnlock.UserId = reader.GetValue(1).ToString();
                            else
                                userV2AvatarUnlock.UserId = null;
                            if (!reader.IsDBNull(2))
                                userV2AvatarUnlock.Gender = reader.GetValue(2).ToString();
                            else
                                userV2AvatarUnlock.Gender = null;
                            if (!reader.IsDBNull(3))
                                userV2AvatarUnlock.Part = reader.GetValue(3).ToString();
                            else
                                userV2AvatarUnlock.Part = null;
                            if (!reader.IsDBNull(4))
                                userV2AvatarUnlock.PartIndex = reader.GetValue(4).ToString();
                            else
                                userV2AvatarUnlock.PartIndex = null;
                            if (!reader.IsDBNull(5))
                                userV2AvatarUnlock.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                userV2AvatarUnlock.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                userV2AvatarUnlock.CreatedAt = reader.GetValue(6).ToString();
                            else
                                userV2AvatarUnlock.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                userV2AvatarUnlock.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                userV2AvatarUnlock.UpdatedAt = null;
                            response.Add(userV2AvatarUnlock);
                            userV2AvatarUnlock = new UserV2AvatarUnlockModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserV2AvatarUnlockModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarUnlockModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserV2AvatarUnlockModel> Create(string connectionString, UserV2AvatarUnlockModel userV2AvatarUnlock)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarUnlockModel> response = new List<UserV2AvatarUnlockModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO user_v_2_avatar_unlocks (";
                string values = "(";
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.Id))
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
                    cmd.Parameters.AddWithValue("@id", userV2AvatarUnlock.Id);
                }
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", userV2AvatarUnlock.UserId);
                }
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.Gender))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `gender` ");
                        values = String.Concat(values, ", @gender");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `gender` ");
                        values = String.Concat(values, " @gender");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@gender", userV2AvatarUnlock.Gender);
                }
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.Part))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `part` ");
                        values = String.Concat(values, ", @part");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `part` ");
                        values = String.Concat(values, " @part");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@part", userV2AvatarUnlock.Part);
                }
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.PartIndex))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `part_index` ");
                        values = String.Concat(values, ", @partIndex");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `part_index` ");
                        values = String.Concat(values, " @partIndex");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@partIndex", userV2AvatarUnlock.PartIndex);
                }
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userV2AvatarUnlock.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userV2AvatarUnlock.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userV2AvatarUnlock.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userV2AvatarUnlock.Id = cmd.LastInsertedId.ToString();
                response.Add(userV2AvatarUnlock);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarUnlockModel>();
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
        public List<UserV2AvatarUnlockModel> Create(string connectionString, List<UserV2AvatarUnlockModel> userV2AvatarUnlockList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarUnlockModel> response = new List<UserV2AvatarUnlockModel>();
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
                        for (int i = 0; i < userV2AvatarUnlockList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO user_v_2_avatar_unlocks (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userV2AvatarUnlockList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, userV2AvatarUnlockList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].Gender))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `gender` ");
                                        values = String.Concat(values, ", @gender" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `gender` ");
                                        values = String.Concat(values, "@gender" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@gender" + i, userV2AvatarUnlockList[i].Gender);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].Part))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `part` ");
                                        values = String.Concat(values, ", @part" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `part` ");
                                        values = String.Concat(values, "@part" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@part" + i, userV2AvatarUnlockList[i].Part);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].PartIndex))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `part_index` ");
                                        values = String.Concat(values, ", @partIndex" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `part_index` ");
                                        values = String.Concat(values, "@partIndex" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@partIndex" + i, userV2AvatarUnlockList[i].PartIndex);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarUnlockList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarUnlockList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarUnlockList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userV2AvatarUnlockList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserV2AvatarUnlockModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserV2AvatarUnlockModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarUnlockModel>();
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

        public List<UserV2AvatarUnlockModel> Update(string connectionString, UserV2AvatarUnlockModel userV2AvatarUnlock, UserV2AvatarUnlockModel userV2AvatarUnlockWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarUnlockModel> response = new List<UserV2AvatarUnlockModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE user_v_2_avatar_unlocks SET ";
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userV2AvatarUnlock.Id);
                }
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userV2AvatarUnlock.UserId);
                }
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.Gender))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `gender` = @gender");
                    else
                        sql = String.Concat(sql, " `gender` = @gender");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@gender", userV2AvatarUnlock.Gender);
                }
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.Part))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `part` = @part");
                    else
                        sql = String.Concat(sql, " `part` = @part");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@part", userV2AvatarUnlock.Part);
                }
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.PartIndex))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `part_index` = @partIndex");
                    else
                        sql = String.Concat(sql, " `part_index` = @partIndex");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@partIndex", userV2AvatarUnlock.PartIndex);
                }
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userV2AvatarUnlock.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userV2AvatarUnlock.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userV2AvatarUnlock.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userV2AvatarUnlock.UpdatedAt);
                }
                if (userV2AvatarUnlockWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userV2AvatarUnlockWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userV2AvatarUnlockWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere.Gender))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `gender` = @genderWhere");
                        else
                            sql = String.Concat(sql, " `gender` = @genderWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@genderWhere", userV2AvatarUnlockWhere.Gender);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere.Part))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `part` = @partWhere");
                        else
                            sql = String.Concat(sql, " `part` = @partWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@partWhere", userV2AvatarUnlockWhere.Part);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere.PartIndex))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `part_index` = @partIndexWhere");
                        else
                            sql = String.Concat(sql, " `part_index` = @partIndexWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@partIndexWhere", userV2AvatarUnlockWhere.PartIndex);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userV2AvatarUnlockWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userV2AvatarUnlockWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userV2AvatarUnlockWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userV2AvatarUnlock);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarUnlockModel>();
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

        public List<UserV2AvatarUnlockModel> Update(string connectionString, List<UserV2AvatarUnlockModel> userV2AvatarUnlockList, List<UserV2AvatarUnlockModel> userV2AvatarUnlockWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarUnlockModel> response = new List<UserV2AvatarUnlockModel>();
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
                        for (int i = 0; i < userV2AvatarUnlockList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE user_v_2_avatar_unlocks SET ";
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userV2AvatarUnlockList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userV2AvatarUnlockList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].Gender))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `gender` = @gender" + i);
                                    else
                                        sql = String.Concat(sql, " `gender` = @gender" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@gender" + i, userV2AvatarUnlockList[i].Gender);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].Part))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `part` = @part" + i);
                                    else
                                        sql = String.Concat(sql, " `part` = @part" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@part" + i, userV2AvatarUnlockList[i].Part);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].PartIndex))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `part_index` = @partIndex" + i);
                                    else
                                        sql = String.Concat(sql, " `part_index` = @partIndex" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@partIndex" + i, userV2AvatarUnlockList[i].PartIndex);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarUnlockList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarUnlockList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarUnlockList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarUnlockList[i].UpdatedAt);
                                }
                                if (userV2AvatarUnlockWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userV2AvatarUnlockWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userV2AvatarUnlockWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereList[i].Gender))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `gender` = @genderWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `gender` = @genderWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@genderWhere" + i, userV2AvatarUnlockWhereList[i].Gender);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereList[i].Part))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `part` = @partWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `part` = @partWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@partWhere" + i, userV2AvatarUnlockWhereList[i].Part);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereList[i].PartIndex))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `part_index` = @partIndexWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `part_index` = @partIndexWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@partIndexWhere" + i, userV2AvatarUnlockWhereList[i].PartIndex);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userV2AvatarUnlockWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userV2AvatarUnlockWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlockWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userV2AvatarUnlockWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userV2AvatarUnlockList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserV2AvatarUnlockModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserV2AvatarUnlockModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarUnlockModel>();
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

        public List<UserV2AvatarUnlockModel> Delete(string connectionString, UserV2AvatarUnlockModel userV2AvatarUnlock)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarUnlockModel> response = new List<UserV2AvatarUnlockModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM user_v_2_avatar_unlocks  ";
                if (userV2AvatarUnlock != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(userV2AvatarUnlock.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userV2AvatarUnlock.Id);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarUnlock.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userV2AvatarUnlock.UserId);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarUnlock.Gender))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `gender` = @gender");
                        else
                            sql = String.Concat(sql, " `gender` = @gender");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@gender", userV2AvatarUnlock.Gender);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarUnlock.Part))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `part` = @part");
                        else
                            sql = String.Concat(sql, " `part` = @part");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@part", userV2AvatarUnlock.Part);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarUnlock.PartIndex))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `part_index` = @partIndex");
                        else
                            sql = String.Concat(sql, " `part_index` = @partIndex");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@partIndex", userV2AvatarUnlock.PartIndex);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarUnlock.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userV2AvatarUnlock.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarUnlock.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userV2AvatarUnlock.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarUnlock.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userV2AvatarUnlock.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userV2AvatarUnlock);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarUnlockModel>();
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

        public List<UserV2AvatarUnlockModel> Delete(string connectionString, List<UserV2AvatarUnlockModel> userV2AvatarUnlock)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarUnlockModel> response = new List<UserV2AvatarUnlockModel>();
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
                        for (int i = 0; i < userV2AvatarUnlock.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM user_v_2_avatar_unlocks  ";
                                if (userV2AvatarUnlock[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlock[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userV2AvatarUnlock[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlock[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userV2AvatarUnlock[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlock[i].Gender))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `gender` = @gender" + i);
                                        else
                                            sql = String.Concat(sql, " `gender` = @gender" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@gender" + i, userV2AvatarUnlock[i].Gender);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlock[i].Part))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `part` = @part" + i);
                                        else
                                            sql = String.Concat(sql, " `part` = @part" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@part" + i, userV2AvatarUnlock[i].Part);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlock[i].PartIndex))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `part_index` = @partIndex" + i);
                                        else
                                            sql = String.Concat(sql, " `part_index` = @partIndex" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@partIndex" + i, userV2AvatarUnlock[i].PartIndex);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlock[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarUnlock[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlock[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarUnlock[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarUnlock[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarUnlock[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userV2AvatarUnlock[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserV2AvatarUnlockModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserV2AvatarUnlockModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarUnlockModel>();
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