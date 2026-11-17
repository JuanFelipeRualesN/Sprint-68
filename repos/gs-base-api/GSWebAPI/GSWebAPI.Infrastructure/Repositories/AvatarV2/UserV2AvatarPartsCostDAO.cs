using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSWebAPI.GSWebAPI.Domain.Models.AvatarV2;
using MySqlConnector;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.AvatarV2
{
    class UserV2AvatarPartsCostDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, UserV2AvatarPartsCostModel> ReadD(string connectionString, UserV2AvatarPartsCostModel userV2AvatarPartsCost, bool firstOne, bool orderByLastDate, UserV2AvatarPartsCostModel userV2AvatarPartsCostWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserV2AvatarPartsCostModel> response = new Dictionary<string, UserV2AvatarPartsCostModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `gender`, `part`, `part_index`, `cost`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_parts_costs ";
                {
                    if (userV2AvatarPartsCost != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Id))
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
                            cmd.Parameters.AddWithValue("@id", userV2AvatarPartsCost.Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Gender))
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
                            cmd.Parameters.AddWithValue("@gender", userV2AvatarPartsCost.Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Part))
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
                            cmd.Parameters.AddWithValue("@part", userV2AvatarPartsCost.Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.PartIndex))
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
                            cmd.Parameters.AddWithValue("@partIndex", userV2AvatarPartsCost.PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Cost))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `cost` = @cost ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `cost` = @cost ");
                            }
                            cmd.Parameters.AddWithValue("@cost", userV2AvatarPartsCost.Cost);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userV2AvatarPartsCost.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userV2AvatarPartsCost.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userV2AvatarPartsCost.UpdatedAt);
                        }
                    }
                    if (userV2AvatarPartsCostWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userV2AvatarPartsCostWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Gender))
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
                            cmd.Parameters.AddWithValue("@genderWhereLike", "%" + userV2AvatarPartsCostWhereLike.Gender + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Part))
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
                            cmd.Parameters.AddWithValue("@partWhereLike", "%" + userV2AvatarPartsCostWhereLike.Part + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.PartIndex))
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
                            cmd.Parameters.AddWithValue("@partIndexWhereLike", "%" + userV2AvatarPartsCostWhereLike.PartIndex + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Cost))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `cost` LIKE @costWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `cost` LIKE @costWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@costWhereLike", "%" + userV2AvatarPartsCostWhereLike.Cost + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userV2AvatarPartsCostWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userV2AvatarPartsCostWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userV2AvatarPartsCostWhereLike.UpdatedAt + "%");
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
                                userV2AvatarPartsCost = new UserV2AvatarPartsCostModel();
                                if (!reader.IsDBNull(0))
                                    userV2AvatarPartsCost.Id = reader.GetValue(0).ToString();
                                else
                                    userV2AvatarPartsCost.Id = null;
                                if (!reader.IsDBNull(1))
                                    userV2AvatarPartsCost.Gender = reader.GetValue(1).ToString();
                                else
                                    userV2AvatarPartsCost.Gender = null;
                                if (!reader.IsDBNull(2))
                                    userV2AvatarPartsCost.Part = reader.GetValue(2).ToString();
                                else
                                    userV2AvatarPartsCost.Part = null;
                                if (!reader.IsDBNull(3))
                                    userV2AvatarPartsCost.PartIndex = reader.GetValue(3).ToString();
                                else
                                    userV2AvatarPartsCost.PartIndex = null;
                                if (!reader.IsDBNull(4))
                                    userV2AvatarPartsCost.Cost = reader.GetValue(4).ToString();
                                else
                                    userV2AvatarPartsCost.Cost = null;
                                if (!reader.IsDBNull(5))
                                    userV2AvatarPartsCost.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userV2AvatarPartsCost.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userV2AvatarPartsCost.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userV2AvatarPartsCost.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userV2AvatarPartsCost.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userV2AvatarPartsCost.UpdatedAt = null;
                                response.Add(userV2AvatarPartsCost.Id, userV2AvatarPartsCost);
                                userV2AvatarPartsCost = new UserV2AvatarPartsCostModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserV2AvatarPartsCostModel>();
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
                response = new Dictionary<string, UserV2AvatarPartsCostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserV2AvatarPartsCostModel> ReadD(string connectionString, UserV2AvatarPartsCostModel userV2AvatarPartsCost, bool orderByLastDate, int fromRow, int limit, UserV2AvatarPartsCostModel userV2AvatarPartsCostWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserV2AvatarPartsCostModel> response = new Dictionary<string, UserV2AvatarPartsCostModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `gender`, `part`, `part_index`, `cost`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_parts_costs ";
                {
                    if (userV2AvatarPartsCost != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Id))
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
                            cmd.Parameters.AddWithValue("@id", userV2AvatarPartsCost.Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Gender))
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
                            cmd.Parameters.AddWithValue("@gender", userV2AvatarPartsCost.Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Part))
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
                            cmd.Parameters.AddWithValue("@part", userV2AvatarPartsCost.Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.PartIndex))
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
                            cmd.Parameters.AddWithValue("@partIndex", userV2AvatarPartsCost.PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Cost))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `cost` = @cost ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `cost` = @cost ");
                            }
                            cmd.Parameters.AddWithValue("@cost", userV2AvatarPartsCost.Cost);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userV2AvatarPartsCost.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userV2AvatarPartsCost.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userV2AvatarPartsCost.UpdatedAt);
                        }
                    }
                    if (userV2AvatarPartsCostWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userV2AvatarPartsCostWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Gender))
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
                            cmd.Parameters.AddWithValue("@genderWhereLike", "%" + userV2AvatarPartsCostWhereLike.Gender + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Part))
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
                            cmd.Parameters.AddWithValue("@partWhereLike", "%" + userV2AvatarPartsCostWhereLike.Part + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.PartIndex))
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
                            cmd.Parameters.AddWithValue("@partIndexWhereLike", "%" + userV2AvatarPartsCostWhereLike.PartIndex + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Cost))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `cost` LIKE @costWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `cost` LIKE @costWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@costWhereLike", "%" + userV2AvatarPartsCostWhereLike.Cost + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userV2AvatarPartsCostWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userV2AvatarPartsCostWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userV2AvatarPartsCostWhereLike.UpdatedAt + "%");
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
                                userV2AvatarPartsCost = new UserV2AvatarPartsCostModel();
                                if (!reader.IsDBNull(0))
                                    userV2AvatarPartsCost.Id = reader.GetValue(0).ToString();
                                else
                                    userV2AvatarPartsCost.Id = null;
                                if (!reader.IsDBNull(1))
                                    userV2AvatarPartsCost.Gender = reader.GetValue(1).ToString();
                                else
                                    userV2AvatarPartsCost.Gender = null;
                                if (!reader.IsDBNull(2))
                                    userV2AvatarPartsCost.Part = reader.GetValue(2).ToString();
                                else
                                    userV2AvatarPartsCost.Part = null;
                                if (!reader.IsDBNull(3))
                                    userV2AvatarPartsCost.PartIndex = reader.GetValue(3).ToString();
                                else
                                    userV2AvatarPartsCost.PartIndex = null;
                                if (!reader.IsDBNull(4))
                                    userV2AvatarPartsCost.Cost = reader.GetValue(4).ToString();
                                else
                                    userV2AvatarPartsCost.Cost = null;
                                if (!reader.IsDBNull(5))
                                    userV2AvatarPartsCost.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userV2AvatarPartsCost.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userV2AvatarPartsCost.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userV2AvatarPartsCost.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userV2AvatarPartsCost.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userV2AvatarPartsCost.UpdatedAt = null;
                                response.Add(userV2AvatarPartsCost.Id, userV2AvatarPartsCost);
                                userV2AvatarPartsCost = new UserV2AvatarPartsCostModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserV2AvatarPartsCostModel>();
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
                response = new Dictionary<string, UserV2AvatarPartsCostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserV2AvatarPartsCostModel> ReadD(string connectionString, List<UserV2AvatarPartsCostModel> userV2AvatarPartsCostWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserV2AvatarPartsCostModel> response = new Dictionary<string, UserV2AvatarPartsCostModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `gender`, `part`, `part_index`, `cost`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_parts_costs ";
                if (userV2AvatarPartsCostWhere != null && userV2AvatarPartsCostWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userV2AvatarPartsCostWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userV2AvatarPartsCostWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Gender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gender` = @gender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gender" + i, userV2AvatarPartsCostWhere[i].Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Part))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part` = @part" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@part" + i, userV2AvatarPartsCostWhere[i].Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].PartIndex))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part_index` = @partIndex" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@partIndex" + i, userV2AvatarPartsCostWhere[i].PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Cost))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `cost` = @cost" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@cost" + i, userV2AvatarPartsCostWhere[i].Cost);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarPartsCostWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarPartsCostWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarPartsCostWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userV2AvatarPartsCostWhere.Count - 1)
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
                            UserV2AvatarPartsCostModel userV2AvatarPartsCost = new UserV2AvatarPartsCostModel(); if (!reader.IsDBNull(0))
                                userV2AvatarPartsCost.Id = reader.GetValue(0).ToString();
                            else
                                userV2AvatarPartsCost.Id = null;
                            if (!reader.IsDBNull(1))
                                userV2AvatarPartsCost.Gender = reader.GetValue(1).ToString();
                            else
                                userV2AvatarPartsCost.Gender = null;
                            if (!reader.IsDBNull(2))
                                userV2AvatarPartsCost.Part = reader.GetValue(2).ToString();
                            else
                                userV2AvatarPartsCost.Part = null;
                            if (!reader.IsDBNull(3))
                                userV2AvatarPartsCost.PartIndex = reader.GetValue(3).ToString();
                            else
                                userV2AvatarPartsCost.PartIndex = null;
                            if (!reader.IsDBNull(4))
                                userV2AvatarPartsCost.Cost = reader.GetValue(4).ToString();
                            else
                                userV2AvatarPartsCost.Cost = null;
                            if (!reader.IsDBNull(5))
                                userV2AvatarPartsCost.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                userV2AvatarPartsCost.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                userV2AvatarPartsCost.CreatedAt = reader.GetValue(6).ToString();
                            else
                                userV2AvatarPartsCost.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                userV2AvatarPartsCost.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                userV2AvatarPartsCost.UpdatedAt = null;
                            response.Add(userV2AvatarPartsCost.Id, userV2AvatarPartsCost);
                            userV2AvatarPartsCost = new UserV2AvatarPartsCostModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserV2AvatarPartsCostModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserV2AvatarPartsCostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserV2AvatarPartsCostModel> ReadD(string connectionString, List<UserV2AvatarPartsCostModel> userV2AvatarPartsCostWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserV2AvatarPartsCostModel> response = new Dictionary<string, UserV2AvatarPartsCostModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `gender`, `part`, `part_index`, `cost`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_parts_costs ";
                if (userV2AvatarPartsCostWhere != null && userV2AvatarPartsCostWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userV2AvatarPartsCostWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userV2AvatarPartsCostWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Gender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gender` = @gender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gender" + i, userV2AvatarPartsCostWhere[i].Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Part))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part` = @part" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@part" + i, userV2AvatarPartsCostWhere[i].Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].PartIndex))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part_index` = @partIndex" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@partIndex" + i, userV2AvatarPartsCostWhere[i].PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Cost))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `cost` = @cost" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@cost" + i, userV2AvatarPartsCostWhere[i].Cost);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarPartsCostWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarPartsCostWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarPartsCostWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userV2AvatarPartsCostWhere.Count - 1)
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
                            UserV2AvatarPartsCostModel userV2AvatarPartsCost = new UserV2AvatarPartsCostModel(); if (!reader.IsDBNull(0))
                                userV2AvatarPartsCost.Id = reader.GetValue(0).ToString();
                            else
                                userV2AvatarPartsCost.Id = null;
                            if (!reader.IsDBNull(1))
                                userV2AvatarPartsCost.Gender = reader.GetValue(1).ToString();
                            else
                                userV2AvatarPartsCost.Gender = null;
                            if (!reader.IsDBNull(2))
                                userV2AvatarPartsCost.Part = reader.GetValue(2).ToString();
                            else
                                userV2AvatarPartsCost.Part = null;
                            if (!reader.IsDBNull(3))
                                userV2AvatarPartsCost.PartIndex = reader.GetValue(3).ToString();
                            else
                                userV2AvatarPartsCost.PartIndex = null;
                            if (!reader.IsDBNull(4))
                                userV2AvatarPartsCost.Cost = reader.GetValue(4).ToString();
                            else
                                userV2AvatarPartsCost.Cost = null;
                            if (!reader.IsDBNull(5))
                                userV2AvatarPartsCost.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                userV2AvatarPartsCost.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                userV2AvatarPartsCost.CreatedAt = reader.GetValue(6).ToString();
                            else
                                userV2AvatarPartsCost.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                userV2AvatarPartsCost.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                userV2AvatarPartsCost.UpdatedAt = null;
                            response.Add(userV2AvatarPartsCost.Id, userV2AvatarPartsCost);
                            userV2AvatarPartsCost = new UserV2AvatarPartsCostModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserV2AvatarPartsCostModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserV2AvatarPartsCostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserV2AvatarPartsCostModel> Read(string connectionString, UserV2AvatarPartsCostModel userV2AvatarPartsCost, bool firstOne, bool orderByLastDate, UserV2AvatarPartsCostModel userV2AvatarPartsCostWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarPartsCostModel> response = new List<UserV2AvatarPartsCostModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `gender`, `part`, `part_index`, `cost`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_parts_costs ";
                {
                    if (userV2AvatarPartsCost != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Id))
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
                            cmd.Parameters.AddWithValue("@id", userV2AvatarPartsCost.Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Gender))
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
                            cmd.Parameters.AddWithValue("@gender", userV2AvatarPartsCost.Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Part))
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
                            cmd.Parameters.AddWithValue("@part", userV2AvatarPartsCost.Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.PartIndex))
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
                            cmd.Parameters.AddWithValue("@partIndex", userV2AvatarPartsCost.PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Cost))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `cost` = @cost ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `cost` = @cost ");
                            }
                            cmd.Parameters.AddWithValue("@cost", userV2AvatarPartsCost.Cost);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userV2AvatarPartsCost.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userV2AvatarPartsCost.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userV2AvatarPartsCost.UpdatedAt);
                        }
                    }
                    if (userV2AvatarPartsCostWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userV2AvatarPartsCostWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Gender))
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
                            cmd.Parameters.AddWithValue("@genderWhereLike", "%" + userV2AvatarPartsCostWhereLike.Gender + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Part))
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
                            cmd.Parameters.AddWithValue("@partWhereLike", "%" + userV2AvatarPartsCostWhereLike.Part + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.PartIndex))
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
                            cmd.Parameters.AddWithValue("@partIndexWhereLike", "%" + userV2AvatarPartsCostWhereLike.PartIndex + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Cost))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `cost` LIKE @costWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `cost` LIKE @costWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@costWhereLike", "%" + userV2AvatarPartsCostWhereLike.Cost + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userV2AvatarPartsCostWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userV2AvatarPartsCostWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userV2AvatarPartsCostWhereLike.UpdatedAt + "%");
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
                                userV2AvatarPartsCost = new UserV2AvatarPartsCostModel();
                                if (!reader.IsDBNull(0))
                                    userV2AvatarPartsCost.Id = reader.GetValue(0).ToString();
                                else
                                    userV2AvatarPartsCost.Id = null;
                                if (!reader.IsDBNull(1))
                                    userV2AvatarPartsCost.Gender = reader.GetValue(1).ToString();
                                else
                                    userV2AvatarPartsCost.Gender = null;
                                if (!reader.IsDBNull(2))
                                    userV2AvatarPartsCost.Part = reader.GetValue(2).ToString();
                                else
                                    userV2AvatarPartsCost.Part = null;
                                if (!reader.IsDBNull(3))
                                    userV2AvatarPartsCost.PartIndex = reader.GetValue(3).ToString();
                                else
                                    userV2AvatarPartsCost.PartIndex = null;
                                if (!reader.IsDBNull(4))
                                    userV2AvatarPartsCost.Cost = reader.GetValue(4).ToString();
                                else
                                    userV2AvatarPartsCost.Cost = null;
                                if (!reader.IsDBNull(5))
                                    userV2AvatarPartsCost.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userV2AvatarPartsCost.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userV2AvatarPartsCost.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userV2AvatarPartsCost.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userV2AvatarPartsCost.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userV2AvatarPartsCost.UpdatedAt = null;
                                response.Add(userV2AvatarPartsCost);
                                userV2AvatarPartsCost = new UserV2AvatarPartsCostModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserV2AvatarPartsCostModel>();
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
                response = new List<UserV2AvatarPartsCostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserV2AvatarPartsCostModel> Read(string connectionString, UserV2AvatarPartsCostModel userV2AvatarPartsCost, bool orderByLastDate, int fromRow, int limit, UserV2AvatarPartsCostModel userV2AvatarPartsCostWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarPartsCostModel> response = new List<UserV2AvatarPartsCostModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `gender`, `part`, `part_index`, `cost`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_parts_costs ";
                {
                    if (userV2AvatarPartsCost != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Id))
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
                            cmd.Parameters.AddWithValue("@id", userV2AvatarPartsCost.Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Gender))
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
                            cmd.Parameters.AddWithValue("@gender", userV2AvatarPartsCost.Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Part))
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
                            cmd.Parameters.AddWithValue("@part", userV2AvatarPartsCost.Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.PartIndex))
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
                            cmd.Parameters.AddWithValue("@partIndex", userV2AvatarPartsCost.PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Cost))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `cost` = @cost ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `cost` = @cost ");
                            }
                            cmd.Parameters.AddWithValue("@cost", userV2AvatarPartsCost.Cost);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userV2AvatarPartsCost.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userV2AvatarPartsCost.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCost.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userV2AvatarPartsCost.UpdatedAt);
                        }
                    }
                    if (userV2AvatarPartsCostWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userV2AvatarPartsCostWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Gender))
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
                            cmd.Parameters.AddWithValue("@genderWhereLike", "%" + userV2AvatarPartsCostWhereLike.Gender + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Part))
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
                            cmd.Parameters.AddWithValue("@partWhereLike", "%" + userV2AvatarPartsCostWhereLike.Part + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.PartIndex))
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
                            cmd.Parameters.AddWithValue("@partIndexWhereLike", "%" + userV2AvatarPartsCostWhereLike.PartIndex + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.Cost))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `cost` LIKE @costWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `cost` LIKE @costWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@costWhereLike", "%" + userV2AvatarPartsCostWhereLike.Cost + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userV2AvatarPartsCostWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userV2AvatarPartsCostWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userV2AvatarPartsCostWhereLike.UpdatedAt + "%");
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
                                userV2AvatarPartsCost = new UserV2AvatarPartsCostModel();
                                if (!reader.IsDBNull(0))
                                    userV2AvatarPartsCost.Id = reader.GetValue(0).ToString();
                                else
                                    userV2AvatarPartsCost.Id = null;
                                if (!reader.IsDBNull(1))
                                    userV2AvatarPartsCost.Gender = reader.GetValue(1).ToString();
                                else
                                    userV2AvatarPartsCost.Gender = null;
                                if (!reader.IsDBNull(2))
                                    userV2AvatarPartsCost.Part = reader.GetValue(2).ToString();
                                else
                                    userV2AvatarPartsCost.Part = null;
                                if (!reader.IsDBNull(3))
                                    userV2AvatarPartsCost.PartIndex = reader.GetValue(3).ToString();
                                else
                                    userV2AvatarPartsCost.PartIndex = null;
                                if (!reader.IsDBNull(4))
                                    userV2AvatarPartsCost.Cost = reader.GetValue(4).ToString();
                                else
                                    userV2AvatarPartsCost.Cost = null;
                                if (!reader.IsDBNull(5))
                                    userV2AvatarPartsCost.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userV2AvatarPartsCost.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userV2AvatarPartsCost.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userV2AvatarPartsCost.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userV2AvatarPartsCost.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userV2AvatarPartsCost.UpdatedAt = null;
                                response.Add(userV2AvatarPartsCost);
                                userV2AvatarPartsCost = new UserV2AvatarPartsCostModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserV2AvatarPartsCostModel>();
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
                response = new List<UserV2AvatarPartsCostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserV2AvatarPartsCostModel> Read(string connectionString, List<UserV2AvatarPartsCostModel> userV2AvatarPartsCostWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarPartsCostModel> response = new List<UserV2AvatarPartsCostModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `gender`, `part`, `part_index`, `cost`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_parts_costs ";
                if (userV2AvatarPartsCostWhere != null && userV2AvatarPartsCostWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userV2AvatarPartsCostWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userV2AvatarPartsCostWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Gender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gender` = @gender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gender" + i, userV2AvatarPartsCostWhere[i].Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Part))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part` = @part" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@part" + i, userV2AvatarPartsCostWhere[i].Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].PartIndex))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part_index` = @partIndex" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@partIndex" + i, userV2AvatarPartsCostWhere[i].PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Cost))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `cost` = @cost" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@cost" + i, userV2AvatarPartsCostWhere[i].Cost);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarPartsCostWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarPartsCostWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarPartsCostWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userV2AvatarPartsCostWhere.Count - 1)
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
                            UserV2AvatarPartsCostModel userV2AvatarPartsCost = new UserV2AvatarPartsCostModel(); if (!reader.IsDBNull(0))
                                userV2AvatarPartsCost.Id = reader.GetValue(0).ToString();
                            else
                                userV2AvatarPartsCost.Id = null;
                            if (!reader.IsDBNull(1))
                                userV2AvatarPartsCost.Gender = reader.GetValue(1).ToString();
                            else
                                userV2AvatarPartsCost.Gender = null;
                            if (!reader.IsDBNull(2))
                                userV2AvatarPartsCost.Part = reader.GetValue(2).ToString();
                            else
                                userV2AvatarPartsCost.Part = null;
                            if (!reader.IsDBNull(3))
                                userV2AvatarPartsCost.PartIndex = reader.GetValue(3).ToString();
                            else
                                userV2AvatarPartsCost.PartIndex = null;
                            if (!reader.IsDBNull(4))
                                userV2AvatarPartsCost.Cost = reader.GetValue(4).ToString();
                            else
                                userV2AvatarPartsCost.Cost = null;
                            if (!reader.IsDBNull(5))
                                userV2AvatarPartsCost.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                userV2AvatarPartsCost.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                userV2AvatarPartsCost.CreatedAt = reader.GetValue(6).ToString();
                            else
                                userV2AvatarPartsCost.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                userV2AvatarPartsCost.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                userV2AvatarPartsCost.UpdatedAt = null;
                            response.Add(userV2AvatarPartsCost);
                            userV2AvatarPartsCost = new UserV2AvatarPartsCostModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserV2AvatarPartsCostModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarPartsCostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserV2AvatarPartsCostModel> Read(string connectionString, List<UserV2AvatarPartsCostModel> userV2AvatarPartsCostWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarPartsCostModel> response = new List<UserV2AvatarPartsCostModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `gender`, `part`, `part_index`, `cost`, `modified_by`, `created_at`, `updated_at`  FROM user_v_2_avatar_parts_costs ";
                if (userV2AvatarPartsCostWhere != null && userV2AvatarPartsCostWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userV2AvatarPartsCostWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userV2AvatarPartsCostWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Gender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gender` = @gender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gender" + i, userV2AvatarPartsCostWhere[i].Gender);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Part))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part` = @part" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@part" + i, userV2AvatarPartsCostWhere[i].Part);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].PartIndex))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `part_index` = @partIndex" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@partIndex" + i, userV2AvatarPartsCostWhere[i].PartIndex);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].Cost))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `cost` = @cost" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@cost" + i, userV2AvatarPartsCostWhere[i].Cost);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarPartsCostWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarPartsCostWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarPartsCostWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userV2AvatarPartsCostWhere.Count - 1)
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
                            UserV2AvatarPartsCostModel userV2AvatarPartsCost = new UserV2AvatarPartsCostModel(); if (!reader.IsDBNull(0))
                                userV2AvatarPartsCost.Id = reader.GetValue(0).ToString();
                            else
                                userV2AvatarPartsCost.Id = null;
                            if (!reader.IsDBNull(1))
                                userV2AvatarPartsCost.Gender = reader.GetValue(1).ToString();
                            else
                                userV2AvatarPartsCost.Gender = null;
                            if (!reader.IsDBNull(2))
                                userV2AvatarPartsCost.Part = reader.GetValue(2).ToString();
                            else
                                userV2AvatarPartsCost.Part = null;
                            if (!reader.IsDBNull(3))
                                userV2AvatarPartsCost.PartIndex = reader.GetValue(3).ToString();
                            else
                                userV2AvatarPartsCost.PartIndex = null;
                            if (!reader.IsDBNull(4))
                                userV2AvatarPartsCost.Cost = reader.GetValue(4).ToString();
                            else
                                userV2AvatarPartsCost.Cost = null;
                            if (!reader.IsDBNull(5))
                                userV2AvatarPartsCost.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                userV2AvatarPartsCost.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                userV2AvatarPartsCost.CreatedAt = reader.GetValue(6).ToString();
                            else
                                userV2AvatarPartsCost.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                userV2AvatarPartsCost.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                userV2AvatarPartsCost.UpdatedAt = null;
                            response.Add(userV2AvatarPartsCost);
                            userV2AvatarPartsCost = new UserV2AvatarPartsCostModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserV2AvatarPartsCostModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarPartsCostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserV2AvatarPartsCostModel> Create(string connectionString, UserV2AvatarPartsCostModel userV2AvatarPartsCost)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarPartsCostModel> response = new List<UserV2AvatarPartsCostModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO user_v_2_avatar_parts_costs (";
                string values = "(";
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Id))
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
                    cmd.Parameters.AddWithValue("@id", userV2AvatarPartsCost.Id);
                }
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Gender))
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
                    cmd.Parameters.AddWithValue("@gender", userV2AvatarPartsCost.Gender);
                }
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Part))
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
                    cmd.Parameters.AddWithValue("@part", userV2AvatarPartsCost.Part);
                }
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.PartIndex))
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
                    cmd.Parameters.AddWithValue("@partIndex", userV2AvatarPartsCost.PartIndex);
                }
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Cost))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `cost` ");
                        values = String.Concat(values, ", @cost");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `cost` ");
                        values = String.Concat(values, " @cost");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@cost", userV2AvatarPartsCost.Cost);
                }
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userV2AvatarPartsCost.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userV2AvatarPartsCost.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userV2AvatarPartsCost.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userV2AvatarPartsCost.Id = cmd.LastInsertedId.ToString();
                response.Add(userV2AvatarPartsCost);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarPartsCostModel>();
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
        public List<UserV2AvatarPartsCostModel> Create(string connectionString, List<UserV2AvatarPartsCostModel> userV2AvatarPartsCostList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarPartsCostModel> response = new List<UserV2AvatarPartsCostModel>();
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
                        for (int i = 0; i < userV2AvatarPartsCostList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO user_v_2_avatar_parts_costs (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userV2AvatarPartsCostList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].Gender))
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
                                    cmd.Parameters.AddWithValue("@gender" + i, userV2AvatarPartsCostList[i].Gender);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].Part))
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
                                    cmd.Parameters.AddWithValue("@part" + i, userV2AvatarPartsCostList[i].Part);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].PartIndex))
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
                                    cmd.Parameters.AddWithValue("@partIndex" + i, userV2AvatarPartsCostList[i].PartIndex);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].Cost))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `cost` ");
                                        values = String.Concat(values, ", @cost" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `cost` ");
                                        values = String.Concat(values, "@cost" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@cost" + i, userV2AvatarPartsCostList[i].Cost);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarPartsCostList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarPartsCostList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarPartsCostList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userV2AvatarPartsCostList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserV2AvatarPartsCostModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserV2AvatarPartsCostModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarPartsCostModel>();
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

        public List<UserV2AvatarPartsCostModel> Update(string connectionString, UserV2AvatarPartsCostModel userV2AvatarPartsCost, UserV2AvatarPartsCostModel userV2AvatarPartsCostWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarPartsCostModel> response = new List<UserV2AvatarPartsCostModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE user_v_2_avatar_parts_costs SET ";
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userV2AvatarPartsCost.Id);
                }
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Gender))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `gender` = @gender");
                    else
                        sql = String.Concat(sql, " `gender` = @gender");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@gender", userV2AvatarPartsCost.Gender);
                }
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Part))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `part` = @part");
                    else
                        sql = String.Concat(sql, " `part` = @part");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@part", userV2AvatarPartsCost.Part);
                }
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.PartIndex))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `part_index` = @partIndex");
                    else
                        sql = String.Concat(sql, " `part_index` = @partIndex");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@partIndex", userV2AvatarPartsCost.PartIndex);
                }
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Cost))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `cost` = @cost");
                    else
                        sql = String.Concat(sql, " `cost` = @cost");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@cost", userV2AvatarPartsCost.Cost);
                }
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userV2AvatarPartsCost.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userV2AvatarPartsCost.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userV2AvatarPartsCost.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userV2AvatarPartsCost.UpdatedAt);
                }
                if (userV2AvatarPartsCostWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userV2AvatarPartsCostWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere.Gender))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `gender` = @genderWhere");
                        else
                            sql = String.Concat(sql, " `gender` = @genderWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@genderWhere", userV2AvatarPartsCostWhere.Gender);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere.Part))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `part` = @partWhere");
                        else
                            sql = String.Concat(sql, " `part` = @partWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@partWhere", userV2AvatarPartsCostWhere.Part);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere.PartIndex))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `part_index` = @partIndexWhere");
                        else
                            sql = String.Concat(sql, " `part_index` = @partIndexWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@partIndexWhere", userV2AvatarPartsCostWhere.PartIndex);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere.Cost))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `cost` = @costWhere");
                        else
                            sql = String.Concat(sql, " `cost` = @costWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@costWhere", userV2AvatarPartsCostWhere.Cost);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userV2AvatarPartsCostWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userV2AvatarPartsCostWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userV2AvatarPartsCostWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userV2AvatarPartsCost);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarPartsCostModel>();
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

        public List<UserV2AvatarPartsCostModel> Update(string connectionString, List<UserV2AvatarPartsCostModel> userV2AvatarPartsCostList, List<UserV2AvatarPartsCostModel> userV2AvatarPartsCostWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarPartsCostModel> response = new List<UserV2AvatarPartsCostModel>();
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
                        for (int i = 0; i < userV2AvatarPartsCostList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE user_v_2_avatar_parts_costs SET ";
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userV2AvatarPartsCostList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].Gender))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `gender` = @gender" + i);
                                    else
                                        sql = String.Concat(sql, " `gender` = @gender" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@gender" + i, userV2AvatarPartsCostList[i].Gender);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].Part))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `part` = @part" + i);
                                    else
                                        sql = String.Concat(sql, " `part` = @part" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@part" + i, userV2AvatarPartsCostList[i].Part);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].PartIndex))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `part_index` = @partIndex" + i);
                                    else
                                        sql = String.Concat(sql, " `part_index` = @partIndex" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@partIndex" + i, userV2AvatarPartsCostList[i].PartIndex);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].Cost))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `cost` = @cost" + i);
                                    else
                                        sql = String.Concat(sql, " `cost` = @cost" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@cost" + i, userV2AvatarPartsCostList[i].Cost);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarPartsCostList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarPartsCostList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userV2AvatarPartsCostList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarPartsCostList[i].UpdatedAt);
                                }
                                if (userV2AvatarPartsCostWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userV2AvatarPartsCostWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereList[i].Gender))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `gender` = @genderWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `gender` = @genderWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@genderWhere" + i, userV2AvatarPartsCostWhereList[i].Gender);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereList[i].Part))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `part` = @partWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `part` = @partWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@partWhere" + i, userV2AvatarPartsCostWhereList[i].Part);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereList[i].PartIndex))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `part_index` = @partIndexWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `part_index` = @partIndexWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@partIndexWhere" + i, userV2AvatarPartsCostWhereList[i].PartIndex);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereList[i].Cost))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `cost` = @costWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `cost` = @costWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@costWhere" + i, userV2AvatarPartsCostWhereList[i].Cost);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userV2AvatarPartsCostWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userV2AvatarPartsCostWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCostWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userV2AvatarPartsCostWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userV2AvatarPartsCostList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserV2AvatarPartsCostModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserV2AvatarPartsCostModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarPartsCostModel>();
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

        public List<UserV2AvatarPartsCostModel> Delete(string connectionString, UserV2AvatarPartsCostModel userV2AvatarPartsCost)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarPartsCostModel> response = new List<UserV2AvatarPartsCostModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM user_v_2_avatar_parts_costs  ";
                if (userV2AvatarPartsCost != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userV2AvatarPartsCost.Id);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Gender))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `gender` = @gender");
                        else
                            sql = String.Concat(sql, " `gender` = @gender");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@gender", userV2AvatarPartsCost.Gender);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Part))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `part` = @part");
                        else
                            sql = String.Concat(sql, " `part` = @part");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@part", userV2AvatarPartsCost.Part);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost.PartIndex))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `part_index` = @partIndex");
                        else
                            sql = String.Concat(sql, " `part_index` = @partIndex");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@partIndex", userV2AvatarPartsCost.PartIndex);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost.Cost))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `cost` = @cost");
                        else
                            sql = String.Concat(sql, " `cost` = @cost");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@cost", userV2AvatarPartsCost.Cost);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userV2AvatarPartsCost.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userV2AvatarPartsCost.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userV2AvatarPartsCost.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userV2AvatarPartsCost);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarPartsCostModel>();
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

        public List<UserV2AvatarPartsCostModel> Delete(string connectionString, List<UserV2AvatarPartsCostModel> userV2AvatarPartsCost)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserV2AvatarPartsCostModel> response = new List<UserV2AvatarPartsCostModel>();
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
                        for (int i = 0; i < userV2AvatarPartsCost.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM user_v_2_avatar_parts_costs  ";
                                if (userV2AvatarPartsCost[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userV2AvatarPartsCost[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost[i].Gender))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `gender` = @gender" + i);
                                        else
                                            sql = String.Concat(sql, " `gender` = @gender" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@gender" + i, userV2AvatarPartsCost[i].Gender);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost[i].Part))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `part` = @part" + i);
                                        else
                                            sql = String.Concat(sql, " `part` = @part" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@part" + i, userV2AvatarPartsCost[i].Part);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost[i].PartIndex))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `part_index` = @partIndex" + i);
                                        else
                                            sql = String.Concat(sql, " `part_index` = @partIndex" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@partIndex" + i, userV2AvatarPartsCost[i].PartIndex);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost[i].Cost))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `cost` = @cost" + i);
                                        else
                                            sql = String.Concat(sql, " `cost` = @cost" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@cost" + i, userV2AvatarPartsCost[i].Cost);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userV2AvatarPartsCost[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userV2AvatarPartsCost[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userV2AvatarPartsCost[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userV2AvatarPartsCost[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userV2AvatarPartsCost[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserV2AvatarPartsCostModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserV2AvatarPartsCostModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserV2AvatarPartsCostModel>();
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