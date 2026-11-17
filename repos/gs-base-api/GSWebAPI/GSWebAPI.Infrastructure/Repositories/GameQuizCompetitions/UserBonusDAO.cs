using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Services.GameQuizCompetitions.CustomClasses;
using GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.GameQuizCompetitions
{
    class UserBonusLogDAO
    {
        private MySqlConnection _connection;
        public List<UserBonusLogModel> Read(string connectionString, UserBonusLogModel userBonusLog, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserBonusLogModel> response = new List<UserBonusLogModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `reward_type`, `reward_quantity`, `modified_by`, `created_at`, `updated_at`  FROM gqz_user_bonus_logs ";
                {
                    if (userBonusLog != null)
                    {
                        if (!string.IsNullOrEmpty(userBonusLog.Id))
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
                            cmd.Parameters.AddWithValue("@id", userBonusLog.Id);
                        }
                        if (!string.IsNullOrEmpty(userBonusLog.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userBonusLog.UserId);
                        }
                        if (!string.IsNullOrEmpty(userBonusLog.RewardType))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `reward_type` = @rewardType ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `reward_type` = @rewardType ");
                            }
                            cmd.Parameters.AddWithValue("@rewardType", userBonusLog.RewardType);
                        }
                        if (!string.IsNullOrEmpty(userBonusLog.RewardQuantity))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `reward_quantity` = @rewardQuantity ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `reward_quantity` = @rewardQuantity ");
                            }
                            cmd.Parameters.AddWithValue("@rewardQuantity", userBonusLog.RewardQuantity);
                        }
                        if (!string.IsNullOrEmpty(userBonusLog.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userBonusLog.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userBonusLog.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userBonusLog.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userBonusLog.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userBonusLog.UpdatedAt);
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
                                    userBonusLog.Id = Reader.GetValue(0).ToString();
                                else
                                    userBonusLog.Id = null;
                                if (!Reader.IsDBNull(1))
                                    userBonusLog.UserId = Reader.GetValue(1).ToString();
                                else
                                    userBonusLog.UserId = null;
                                if (!Reader.IsDBNull(2))
                                    userBonusLog.RewardType = Reader.GetValue(2).ToString();
                                else
                                    userBonusLog.RewardType = null;
                                if (!Reader.IsDBNull(3))
                                    userBonusLog.RewardQuantity = Reader.GetValue(3).ToString();
                                else
                                    userBonusLog.RewardQuantity = null;
                                if (!Reader.IsDBNull(4))
                                    userBonusLog.ModifiedBy = Reader.GetValue(4).ToString();
                                else
                                    userBonusLog.ModifiedBy = null;
                                if (!Reader.IsDBNull(5))
                                    userBonusLog.CreatedAt = Reader.GetValue(5).ToString();
                                else
                                    userBonusLog.CreatedAt = null;
                                if (!Reader.IsDBNull(6))
                                    userBonusLog.UpdatedAt = Reader.GetValue(6).ToString();
                                else
                                    userBonusLog.UpdatedAt = null;
                                response.Add(userBonusLog);
                                userBonusLog = new UserBonusLogModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserBonusLogModel>();
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
                response = new List<UserBonusLogModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserBonusLogModel> Read(string connectionString, List<UserBonusLogModel> userBonusLogWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserBonusLogModel> response = new List<UserBonusLogModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `reward_type`, `reward_quantity`, `modified_by`, `created_at`, `updated_at`  FROM gqz_user_bonus_logs ";
                if (userBonusLogWhere != null && userBonusLogWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userBonusLogWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userBonusLogWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userBonusLogWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userBonusLogWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userBonusLogWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(userBonusLogWhere[i].RewardType))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `reward_type` = @rewardType" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@rewardType" + i, userBonusLogWhere[i].RewardType);
                        }
                        if (!string.IsNullOrEmpty(userBonusLogWhere[i].RewardQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `reward_quantity` = @rewardQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@rewardQuantity" + i, userBonusLogWhere[i].RewardQuantity);
                        }
                        if (!string.IsNullOrEmpty(userBonusLogWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userBonusLogWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userBonusLogWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userBonusLogWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userBonusLogWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userBonusLogWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userBonusLogWhere.Count - 1)
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
                            UserBonusLogModel userBonusLog = new UserBonusLogModel(); if (!Reader.IsDBNull(0))
                                userBonusLog.Id = Reader.GetValue(0).ToString();
                            else
                                userBonusLog.Id = null;
                            if (!Reader.IsDBNull(1))
                                userBonusLog.UserId = Reader.GetValue(1).ToString();
                            else
                                userBonusLog.UserId = null;
                            if (!Reader.IsDBNull(2))
                                userBonusLog.RewardType = Reader.GetValue(2).ToString();
                            else
                                userBonusLog.RewardType = null;
                            if (!Reader.IsDBNull(3))
                                userBonusLog.RewardQuantity = Reader.GetValue(3).ToString();
                            else
                                userBonusLog.RewardQuantity = null;
                            if (!Reader.IsDBNull(4))
                                userBonusLog.ModifiedBy = Reader.GetValue(4).ToString();
                            else
                                userBonusLog.ModifiedBy = null;
                            if (!Reader.IsDBNull(5))
                                userBonusLog.CreatedAt = Reader.GetValue(5).ToString();
                            else
                                userBonusLog.CreatedAt = null;
                            if (!Reader.IsDBNull(6))
                                userBonusLog.UpdatedAt = Reader.GetValue(6).ToString();
                            else
                                userBonusLog.UpdatedAt = null;
                            response.Add(userBonusLog);
                            userBonusLog = new UserBonusLogModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserBonusLogModel>();
                    Reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserBonusLogModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserBonusLogModel> Create(string connectionString, UserBonusLogModel userBonusLog)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserBonusLogModel> response = new List<UserBonusLogModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO gqz_user_bonus_logs (";
                string values = "(";
                if (!string.IsNullOrEmpty(userBonusLog.Id))
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
                    cmd.Parameters.AddWithValue("@id", userBonusLog.Id);
                }
                if (!string.IsNullOrEmpty(userBonusLog.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", userBonusLog.UserId);
                }
                if (!string.IsNullOrEmpty(userBonusLog.RewardType))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `reward_type` ");
                        values = string.Concat(values, ", @rewardType");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `reward_type` ");
                        values = string.Concat(values, " @rewardType");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@rewardType", userBonusLog.RewardType);
                }
                if (!string.IsNullOrEmpty(userBonusLog.RewardQuantity))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `reward_quantity` ");
                        values = string.Concat(values, ", @rewardQuantity");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `reward_quantity` ");
                        values = string.Concat(values, " @rewardQuantity");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@rewardQuantity", userBonusLog.RewardQuantity);
                }
                if (!string.IsNullOrEmpty(userBonusLog.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userBonusLog.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userBonusLog.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userBonusLog.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userBonusLog.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userBonusLog.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userBonusLog.Id = cmd.LastInsertedId.ToString();
                response.Add(userBonusLog);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserBonusLogModel>();
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
        public List<UserBonusLogModel> Create(string connectionString, List<UserBonusLogModel> userBonusLogList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserBonusLogModel> response = new List<UserBonusLogModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userBonusLogList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO gqz_user_bonus_logs (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(userBonusLogList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userBonusLogList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userBonusLogList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, userBonusLogList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userBonusLogList[i].RewardType))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `reward_type` ");
                                        values = string.Concat(values, ", @rewardType" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `reward_type` ");
                                        values = string.Concat(values, "@rewardType" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@rewardType" + i, userBonusLogList[i].RewardType);
                                }
                                if (!string.IsNullOrEmpty(userBonusLogList[i].RewardQuantity))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `reward_quantity` ");
                                        values = string.Concat(values, ", @rewardQuantity" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `reward_quantity` ");
                                        values = string.Concat(values, "@rewardQuantity" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@rewardQuantity" + i, userBonusLogList[i].RewardQuantity);
                                }
                                if (!string.IsNullOrEmpty(userBonusLogList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userBonusLogList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userBonusLogList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userBonusLogList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userBonusLogList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userBonusLogList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userBonusLogList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserBonusLogModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserBonusLogModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserBonusLogModel>();
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

        public List<UserBonusLogModel> Update(string connectionString, UserBonusLogModel userBonusLog, UserBonusLogModel userBonusLogWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserBonusLogModel> response = new List<UserBonusLogModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE gqz_user_bonus_logs SET ";
                if (!string.IsNullOrEmpty(userBonusLog.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userBonusLog.Id);
                }
                if (!string.IsNullOrEmpty(userBonusLog.UserId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = string.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userBonusLog.UserId);
                }
                if (!string.IsNullOrEmpty(userBonusLog.RewardType))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `reward_type` = @rewardType");
                    else
                        sql = string.Concat(sql, " `reward_type` = @rewardType");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@rewardType", userBonusLog.RewardType);
                }
                if (!string.IsNullOrEmpty(userBonusLog.RewardQuantity))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `reward_quantity` = @rewardQuantity");
                    else
                        sql = string.Concat(sql, " `reward_quantity` = @rewardQuantity");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@rewardQuantity", userBonusLog.RewardQuantity);
                }
                if (!string.IsNullOrEmpty(userBonusLog.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userBonusLog.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userBonusLog.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userBonusLog.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userBonusLog.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userBonusLog.UpdatedAt);
                }
                if (userBonusLogWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(userBonusLogWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userBonusLogWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(userBonusLogWhere.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = string.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userBonusLogWhere.UserId);
                    }
                    if (!string.IsNullOrEmpty(userBonusLogWhere.RewardType))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reward_type` = @rewardTypeWhere");
                        else
                            sql = string.Concat(sql, " `reward_type` = @rewardTypeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rewardTypeWhere", userBonusLogWhere.RewardType);
                    }
                    if (!string.IsNullOrEmpty(userBonusLogWhere.RewardQuantity))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reward_quantity` = @rewardQuantityWhere");
                        else
                            sql = string.Concat(sql, " `reward_quantity` = @rewardQuantityWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rewardQuantityWhere", userBonusLogWhere.RewardQuantity);
                    }
                    if (!string.IsNullOrEmpty(userBonusLogWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userBonusLogWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userBonusLogWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userBonusLogWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userBonusLogWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userBonusLogWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userBonusLog);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserBonusLogModel>();
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

        public List<UserBonusLogModel> Update(string connectionString, List<UserBonusLogModel> userBonusLogList, List<UserBonusLogModel> userBonusLogWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserBonusLogModel> response = new List<UserBonusLogModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userBonusLogList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE gqz_user_bonus_logs SET ";
                                if (!string.IsNullOrEmpty(userBonusLogList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userBonusLogList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userBonusLogList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = string.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userBonusLogList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userBonusLogList[i].RewardType))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `reward_type` = @rewardType" + i);
                                    else
                                        sql = string.Concat(sql, " `reward_type` = @rewardType" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@rewardType" + i, userBonusLogList[i].RewardType);
                                }
                                if (!string.IsNullOrEmpty(userBonusLogList[i].RewardQuantity))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `reward_quantity` = @rewardQuantity" + i);
                                    else
                                        sql = string.Concat(sql, " `reward_quantity` = @rewardQuantity" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@rewardQuantity" + i, userBonusLogList[i].RewardQuantity);
                                }
                                if (!string.IsNullOrEmpty(userBonusLogList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userBonusLogList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userBonusLogList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userBonusLogList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userBonusLogList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userBonusLogList[i].UpdatedAt);
                                }
                                if (userBonusLogWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(userBonusLogWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userBonusLogWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userBonusLogWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userBonusLogWhereList[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userBonusLogWhereList[i].RewardType))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reward_type` = @rewardTypeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `reward_type` = @rewardTypeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rewardTypeWhere" + i, userBonusLogWhereList[i].RewardType);
                                    }
                                    if (!string.IsNullOrEmpty(userBonusLogWhereList[i].RewardQuantity))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reward_quantity` = @rewardQuantityWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `reward_quantity` = @rewardQuantityWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rewardQuantityWhere" + i, userBonusLogWhereList[i].RewardQuantity);
                                    }
                                    if (!string.IsNullOrEmpty(userBonusLogWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userBonusLogWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userBonusLogWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userBonusLogWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userBonusLogWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userBonusLogWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userBonusLogList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserBonusLogModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserBonusLogModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserBonusLogModel>();
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

        public List<UserBonusLogModel> Delete(string connectionString, UserBonusLogModel userBonusLog)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserBonusLogModel> response = new List<UserBonusLogModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM gqz_user_bonus_logs  ";
                if (userBonusLog != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(userBonusLog.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userBonusLog.Id);
                    }
                    if (!string.IsNullOrEmpty(userBonusLog.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = string.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userBonusLog.UserId);
                    }
                    if (!string.IsNullOrEmpty(userBonusLog.RewardType))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reward_type` = @rewardType");
                        else
                            sql = string.Concat(sql, " `reward_type` = @rewardType");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rewardType", userBonusLog.RewardType);
                    }
                    if (!string.IsNullOrEmpty(userBonusLog.RewardQuantity))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reward_quantity` = @rewardQuantity");
                        else
                            sql = string.Concat(sql, " `reward_quantity` = @rewardQuantity");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rewardQuantity", userBonusLog.RewardQuantity);
                    }
                    if (!string.IsNullOrEmpty(userBonusLog.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userBonusLog.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userBonusLog.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userBonusLog.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userBonusLog.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userBonusLog.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userBonusLog);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserBonusLogModel>();
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

        public List<UserBonusLogModel> Delete(string connectionString, List<UserBonusLogModel> userBonusLog)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserBonusLogModel> response = new List<UserBonusLogModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userBonusLog.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM gqz_user_bonus_logs  ";
                                if (userBonusLog[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(userBonusLog[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userBonusLog[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userBonusLog[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userBonusLog[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userBonusLog[i].RewardType))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reward_type` = @rewardType" + i);
                                        else
                                            sql = string.Concat(sql, " `reward_type` = @rewardType" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rewardType" + i, userBonusLog[i].RewardType);
                                    }
                                    if (!string.IsNullOrEmpty(userBonusLog[i].RewardQuantity))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reward_quantity` = @rewardQuantity" + i);
                                        else
                                            sql = string.Concat(sql, " `reward_quantity` = @rewardQuantity" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rewardQuantity" + i, userBonusLog[i].RewardQuantity);
                                    }
                                    if (!string.IsNullOrEmpty(userBonusLog[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userBonusLog[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userBonusLog[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userBonusLog[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userBonusLog[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userBonusLog[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userBonusLog[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserBonusLogModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserBonusLogModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserBonusLogModel>();
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
