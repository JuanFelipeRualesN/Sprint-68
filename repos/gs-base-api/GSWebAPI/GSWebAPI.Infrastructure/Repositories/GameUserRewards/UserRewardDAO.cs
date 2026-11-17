using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Domain.Models.GameUserReward;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.GameUserRewards
{
    class UserRewardDAO
    {
        private MySqlConnection _connection;
        public List<UserRewardModel> Read(string connectionString, UserRewardModel userReward, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardModel> response = new List<UserRewardModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `coins_total`, `diamonds_total`, `modified_by`, `created_at`, `updated_at`  FROM gr_user_rewards ";
                {
                    if (userReward != null)
                    {
                        if (!string.IsNullOrEmpty(userReward.Id))
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
                            cmd.Parameters.AddWithValue("@id", userReward.Id);
                        }
                        if (!string.IsNullOrEmpty(userReward.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userReward.UserId);
                        }
                        if (!string.IsNullOrEmpty(userReward.CoinsTotal))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `coins_total` = @coinsTotal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `coins_total` = @coinsTotal ");
                            }
                            cmd.Parameters.AddWithValue("@coinsTotal", userReward.CoinsTotal);
                        }
                        if (!string.IsNullOrEmpty(userReward.DiamondsTotal))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `diamonds_total` = @diamondsTotal ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `diamonds_total` = @diamondsTotal ");
                            }
                            cmd.Parameters.AddWithValue("@diamondsTotal", userReward.DiamondsTotal);
                        }
                        if (!string.IsNullOrEmpty(userReward.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userReward.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userReward.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userReward.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userReward.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userReward.UpdatedAt);
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
                                    userReward.Id = Reader.GetValue(0).ToString();
                                else
                                    userReward.Id = null;
                                if (!Reader.IsDBNull(1))
                                    userReward.UserId = Reader.GetValue(1).ToString();
                                else
                                    userReward.UserId = null;
                                if (!Reader.IsDBNull(2))
                                    userReward.CoinsTotal = Reader.GetValue(2).ToString();
                                else
                                    userReward.CoinsTotal = null;
                                if (!Reader.IsDBNull(3))
                                    userReward.DiamondsTotal = Reader.GetValue(3).ToString();
                                else
                                    userReward.DiamondsTotal = null;
                                if (!Reader.IsDBNull(4))
                                    userReward.ModifiedBy = Reader.GetValue(4).ToString();
                                else
                                    userReward.ModifiedBy = null;
                                if (!Reader.IsDBNull(5))
                                    userReward.CreatedAt = Reader.GetValue(5).ToString();
                                else
                                    userReward.CreatedAt = null;
                                if (!Reader.IsDBNull(6))
                                    userReward.UpdatedAt = Reader.GetValue(6).ToString();
                                else
                                    userReward.UpdatedAt = null;
                                response.Add(userReward);
                                userReward = new UserRewardModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserRewardModel>();
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
                response = new List<UserRewardModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserRewardModel> Read(string connectionString, List<UserRewardModel> userRewardWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardModel> response = new List<UserRewardModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `coins_total`, `diamonds_total`, `modified_by`, `created_at`, `updated_at`  FROM gr_user_rewards ";
                if (userRewardWhere != null && userRewardWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userRewardWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userRewardWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userRewardWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userRewardWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userRewardWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(userRewardWhere[i].CoinsTotal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `coins_total` = @coinsTotal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@coinsTotal" + i, userRewardWhere[i].CoinsTotal);
                        }
                        if (!string.IsNullOrEmpty(userRewardWhere[i].DiamondsTotal))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `diamonds_total` = @diamondsTotal" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@diamondsTotal" + i, userRewardWhere[i].DiamondsTotal);
                        }
                        if (!string.IsNullOrEmpty(userRewardWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userRewardWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userRewardWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userRewardWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userRewardWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userRewardWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userRewardWhere.Count - 1)
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
                            UserRewardModel userReward = new UserRewardModel(); if (!Reader.IsDBNull(0))
                                userReward.Id = Reader.GetValue(0).ToString();
                            else
                                userReward.Id = null;
                            if (!Reader.IsDBNull(1))
                                userReward.UserId = Reader.GetValue(1).ToString();
                            else
                                userReward.UserId = null;
                            if (!Reader.IsDBNull(2))
                                userReward.CoinsTotal = Reader.GetValue(2).ToString();
                            else
                                userReward.CoinsTotal = null;
                            if (!Reader.IsDBNull(3))
                                userReward.DiamondsTotal = Reader.GetValue(3).ToString();
                            else
                                userReward.DiamondsTotal = null;
                            if (!Reader.IsDBNull(4))
                                userReward.ModifiedBy = Reader.GetValue(4).ToString();
                            else
                                userReward.ModifiedBy = null;
                            if (!Reader.IsDBNull(5))
                                userReward.CreatedAt = Reader.GetValue(5).ToString();
                            else
                                userReward.CreatedAt = null;
                            if (!Reader.IsDBNull(6))
                                userReward.UpdatedAt = Reader.GetValue(6).ToString();
                            else
                                userReward.UpdatedAt = null;
                            response.Add(userReward);
                            userReward = new UserRewardModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserRewardModel>();
                    Reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserRewardModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserRewardModel> Create(string connectionString, UserRewardModel userReward)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardModel> response = new List<UserRewardModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO gr_user_rewards (";
                string values = "(";
                if (!string.IsNullOrEmpty(userReward.Id))
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
                    cmd.Parameters.AddWithValue("@id", userReward.Id);
                }
                if (!string.IsNullOrEmpty(userReward.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", userReward.UserId);
                }
                if (!string.IsNullOrEmpty(userReward.CoinsTotal))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `coins_total` ");
                        values = string.Concat(values, ", @coinsTotal");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `coins_total` ");
                        values = string.Concat(values, " @coinsTotal");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@coinsTotal", userReward.CoinsTotal);
                }
                if (!string.IsNullOrEmpty(userReward.DiamondsTotal))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `diamonds_total` ");
                        values = string.Concat(values, ", @diamondsTotal");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `diamonds_total` ");
                        values = string.Concat(values, " @diamondsTotal");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@diamondsTotal", userReward.DiamondsTotal);
                }
                if (!string.IsNullOrEmpty(userReward.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userReward.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userReward.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userReward.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userReward.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userReward.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userReward.Id = cmd.LastInsertedId.ToString();
                response.Add(userReward);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserRewardModel>();
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
        public List<UserRewardModel> Create(string connectionString, List<UserRewardModel> userRewardList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardModel> response = new List<UserRewardModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userRewardList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO gr_user_rewards (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(userRewardList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userRewardList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userRewardList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, userRewardList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userRewardList[i].CoinsTotal))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `coins_total` ");
                                        values = string.Concat(values, ", @coinsTotal" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `coins_total` ");
                                        values = string.Concat(values, "@coinsTotal" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@coinsTotal" + i, userRewardList[i].CoinsTotal);
                                }
                                if (!string.IsNullOrEmpty(userRewardList[i].DiamondsTotal))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `diamonds_total` ");
                                        values = string.Concat(values, ", @diamondsTotal" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `diamonds_total` ");
                                        values = string.Concat(values, "@diamondsTotal" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@diamondsTotal" + i, userRewardList[i].DiamondsTotal);
                                }
                                if (!string.IsNullOrEmpty(userRewardList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userRewardList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userRewardList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userRewardList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userRewardList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userRewardList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userRewardList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserRewardModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserRewardModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserRewardModel>();
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

        public List<UserRewardModel> Update(string connectionString, UserRewardModel userReward, UserRewardModel userRewardWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardModel> response = new List<UserRewardModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE gr_user_rewards SET ";
                if (!string.IsNullOrEmpty(userReward.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userReward.Id);
                }
                if (!string.IsNullOrEmpty(userReward.UserId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = string.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userReward.UserId);
                }
                if (!string.IsNullOrEmpty(userReward.CoinsTotal))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `coins_total` = @coinsTotal");
                    else
                        sql = string.Concat(sql, " `coins_total` = @coinsTotal");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@coinsTotal", userReward.CoinsTotal);
                }
                if (!string.IsNullOrEmpty(userReward.DiamondsTotal))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `diamonds_total` = @diamondsTotal");
                    else
                        sql = string.Concat(sql, " `diamonds_total` = @diamondsTotal");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@diamondsTotal", userReward.DiamondsTotal);
                }
                if (!string.IsNullOrEmpty(userReward.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userReward.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userReward.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userReward.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userReward.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userReward.UpdatedAt);
                }
                if (userRewardWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(userRewardWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userRewardWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(userRewardWhere.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = string.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userRewardWhere.UserId);
                    }
                    if (!string.IsNullOrEmpty(userRewardWhere.CoinsTotal))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `coins_total` = @coinsTotalWhere");
                        else
                            sql = string.Concat(sql, " `coins_total` = @coinsTotalWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@coinsTotalWhere", userRewardWhere.CoinsTotal);
                    }
                    if (!string.IsNullOrEmpty(userRewardWhere.DiamondsTotal))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `diamonds_total` = @diamondsTotalWhere");
                        else
                            sql = string.Concat(sql, " `diamonds_total` = @diamondsTotalWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@diamondsTotalWhere", userRewardWhere.DiamondsTotal);
                    }
                    if (!string.IsNullOrEmpty(userRewardWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userRewardWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userRewardWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userRewardWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userRewardWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userRewardWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userReward);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserRewardModel>();
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

        public List<UserRewardModel> Update(string connectionString, List<UserRewardModel> userRewardList, List<UserRewardModel> userRewardWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardModel> response = new List<UserRewardModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userRewardList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE gr_user_rewards SET ";
                                if (!string.IsNullOrEmpty(userRewardList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userRewardList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userRewardList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = string.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userRewardList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userRewardList[i].CoinsTotal))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `coins_total` = @coinsTotal" + i);
                                    else
                                        sql = string.Concat(sql, " `coins_total` = @coinsTotal" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@coinsTotal" + i, userRewardList[i].CoinsTotal);
                                }
                                if (!string.IsNullOrEmpty(userRewardList[i].DiamondsTotal))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `diamonds_total` = @diamondsTotal" + i);
                                    else
                                        sql = string.Concat(sql, " `diamonds_total` = @diamondsTotal" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@diamondsTotal" + i, userRewardList[i].DiamondsTotal);
                                }
                                if (!string.IsNullOrEmpty(userRewardList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userRewardList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userRewardList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userRewardList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userRewardList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userRewardList[i].UpdatedAt);
                                }
                                if (userRewardWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(userRewardWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userRewardWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userRewardWhereList[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardWhereList[i].CoinsTotal))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `coins_total` = @coinsTotalWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `coins_total` = @coinsTotalWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@coinsTotalWhere" + i, userRewardWhereList[i].CoinsTotal);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardWhereList[i].DiamondsTotal))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `diamonds_total` = @diamondsTotalWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `diamonds_total` = @diamondsTotalWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@diamondsTotalWhere" + i, userRewardWhereList[i].DiamondsTotal);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userRewardWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userRewardWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userRewardWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userRewardList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserRewardModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserRewardModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserRewardModel>();
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

        public List<UserRewardModel> Delete(string connectionString, UserRewardModel userReward)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardModel> response = new List<UserRewardModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM gr_user_rewards  ";
                if (userReward != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(userReward.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userReward.Id);
                    }
                    if (!string.IsNullOrEmpty(userReward.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = string.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userReward.UserId);
                    }
                    if (!string.IsNullOrEmpty(userReward.CoinsTotal))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `coins_total` = @coinsTotal");
                        else
                            sql = string.Concat(sql, " `coins_total` = @coinsTotal");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@coinsTotal", userReward.CoinsTotal);
                    }
                    if (!string.IsNullOrEmpty(userReward.DiamondsTotal))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `diamonds_total` = @diamondsTotal");
                        else
                            sql = string.Concat(sql, " `diamonds_total` = @diamondsTotal");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@diamondsTotal", userReward.DiamondsTotal);
                    }
                    if (!string.IsNullOrEmpty(userReward.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userReward.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userReward.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userReward.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userReward.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userReward.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userReward);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserRewardModel>();
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

        public List<UserRewardModel> Delete(string connectionString, List<UserRewardModel> userReward)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardModel> response = new List<UserRewardModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userReward.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM gr_user_rewards  ";
                                if (userReward[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(userReward[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userReward[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userReward[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userReward[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userReward[i].CoinsTotal))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `coins_total` = @coinsTotal" + i);
                                        else
                                            sql = string.Concat(sql, " `coins_total` = @coinsTotal" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@coinsTotal" + i, userReward[i].CoinsTotal);
                                    }
                                    if (!string.IsNullOrEmpty(userReward[i].DiamondsTotal))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `diamonds_total` = @diamondsTotal" + i);
                                        else
                                            sql = string.Concat(sql, " `diamonds_total` = @diamondsTotal" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@diamondsTotal" + i, userReward[i].DiamondsTotal);
                                    }
                                    if (!string.IsNullOrEmpty(userReward[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userReward[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userReward[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userReward[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userReward[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userReward[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userReward[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserRewardModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserRewardModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserRewardModel>();
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
