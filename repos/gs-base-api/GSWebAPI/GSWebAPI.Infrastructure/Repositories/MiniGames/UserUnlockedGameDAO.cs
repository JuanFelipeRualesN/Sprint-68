using System;
using System.Collections.Generic;
using MySqlConnector;

using GSWebAPI.GSWebAPI.Domain.Models.MiniGame;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.MiniGames
{
    class UserUnlockedGameDAO
    {
        private MySqlConnection _connection;
        public List<UserUnlockedGameModel> Read(string connectionString, UserUnlockedGameModel userUnlockedGame, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedGameModel> response = new List<UserUnlockedGameModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `game_code`, `modified_by`, `created_at`, `updated_at`  FROM mg_user_unlocked_games ";
                {
                    if (userUnlockedGame != null)
                    {
                        if (!string.IsNullOrEmpty(userUnlockedGame.Id))
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
                            cmd.Parameters.AddWithValue("@id", userUnlockedGame.Id);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedGame.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userUnlockedGame.UserId);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedGame.GameCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `game_code` = @gameCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `game_code` = @gameCode ");
                            }
                            cmd.Parameters.AddWithValue("@gameCode", userUnlockedGame.GameCode);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedGame.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userUnlockedGame.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedGame.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userUnlockedGame.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedGame.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userUnlockedGame.UpdatedAt);
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
                                    userUnlockedGame.Id = Reader.GetValue(0).ToString();
                                else
                                    userUnlockedGame.Id = null;
                                if (!Reader.IsDBNull(1))
                                    userUnlockedGame.UserId = Reader.GetValue(1).ToString();
                                else
                                    userUnlockedGame.UserId = null;
                                if (!Reader.IsDBNull(2))
                                    userUnlockedGame.GameCode = Reader.GetValue(2).ToString();
                                else
                                    userUnlockedGame.GameCode = null;
                                if (!Reader.IsDBNull(3))
                                    userUnlockedGame.ModifiedBy = Reader.GetValue(3).ToString();
                                else
                                    userUnlockedGame.ModifiedBy = null;
                                if (!Reader.IsDBNull(4))
                                    userUnlockedGame.CreatedAt = Reader.GetValue(4).ToString();
                                else
                                    userUnlockedGame.CreatedAt = null;
                                if (!Reader.IsDBNull(5))
                                    userUnlockedGame.UpdatedAt = Reader.GetValue(5).ToString();
                                else
                                    userUnlockedGame.UpdatedAt = null;
                                response.Add(userUnlockedGame);
                                userUnlockedGame = new UserUnlockedGameModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserUnlockedGameModel>();
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
                response = new List<UserUnlockedGameModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserUnlockedGameModel> Read(string connectionString, List<UserUnlockedGameModel> userUnlockedGameWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedGameModel> response = new List<UserUnlockedGameModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `game_code`, `modified_by`, `created_at`, `updated_at`  FROM mg_user_unlocked_games ";
                if (userUnlockedGameWhere != null && userUnlockedGameWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userUnlockedGameWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userUnlockedGameWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userUnlockedGameWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedGameWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userUnlockedGameWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedGameWhere[i].GameCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `game_code` = @gameCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gameCode" + i, userUnlockedGameWhere[i].GameCode);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedGameWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userUnlockedGameWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedGameWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userUnlockedGameWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userUnlockedGameWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userUnlockedGameWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userUnlockedGameWhere.Count - 1)
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
                            UserUnlockedGameModel userUnlockedGame = new UserUnlockedGameModel(); if (!Reader.IsDBNull(0))
                                userUnlockedGame.Id = Reader.GetValue(0).ToString();
                            else
                                userUnlockedGame.Id = null;
                            if (!Reader.IsDBNull(1))
                                userUnlockedGame.UserId = Reader.GetValue(1).ToString();
                            else
                                userUnlockedGame.UserId = null;
                            if (!Reader.IsDBNull(2))
                                userUnlockedGame.GameCode = Reader.GetValue(2).ToString();
                            else
                                userUnlockedGame.GameCode = null;
                            if (!Reader.IsDBNull(3))
                                userUnlockedGame.ModifiedBy = Reader.GetValue(3).ToString();
                            else
                                userUnlockedGame.ModifiedBy = null;
                            if (!Reader.IsDBNull(4))
                                userUnlockedGame.CreatedAt = Reader.GetValue(4).ToString();
                            else
                                userUnlockedGame.CreatedAt = null;
                            if (!Reader.IsDBNull(5))
                                userUnlockedGame.UpdatedAt = Reader.GetValue(5).ToString();
                            else
                                userUnlockedGame.UpdatedAt = null;
                            response.Add(userUnlockedGame);
                            userUnlockedGame = new UserUnlockedGameModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserUnlockedGameModel>();
                    Reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserUnlockedGameModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserUnlockedGameModel> Create(string connectionString, UserUnlockedGameModel userUnlockedGame)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedGameModel> response = new List<UserUnlockedGameModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO mg_user_unlocked_games (";
                string values = "(";
                if (!string.IsNullOrEmpty(userUnlockedGame.Id))
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
                    cmd.Parameters.AddWithValue("@id", userUnlockedGame.Id);
                }
                if (!string.IsNullOrEmpty(userUnlockedGame.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", userUnlockedGame.UserId);
                }
                if (!string.IsNullOrEmpty(userUnlockedGame.GameCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `game_code` ");
                        values = string.Concat(values, ", @gameCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `game_code` ");
                        values = string.Concat(values, " @gameCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@gameCode", userUnlockedGame.GameCode);
                }
                if (!string.IsNullOrEmpty(userUnlockedGame.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userUnlockedGame.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userUnlockedGame.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userUnlockedGame.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userUnlockedGame.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userUnlockedGame.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userUnlockedGame.Id = cmd.LastInsertedId.ToString();
                response.Add(userUnlockedGame);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserUnlockedGameModel>();
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
        public List<UserUnlockedGameModel> Create(string connectionString, List<UserUnlockedGameModel> userUnlockedGameList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedGameModel> response = new List<UserUnlockedGameModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userUnlockedGameList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO mg_user_unlocked_games (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(userUnlockedGameList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userUnlockedGameList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedGameList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, userUnlockedGameList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedGameList[i].GameCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `game_code` ");
                                        values = string.Concat(values, ", @gameCode" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `game_code` ");
                                        values = string.Concat(values, "@gameCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@gameCode" + i, userUnlockedGameList[i].GameCode);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedGameList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userUnlockedGameList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedGameList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userUnlockedGameList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedGameList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userUnlockedGameList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userUnlockedGameList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserUnlockedGameModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserUnlockedGameModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserUnlockedGameModel>();
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

        public List<UserUnlockedGameModel> Update(string connectionString, UserUnlockedGameModel userUnlockedGame, UserUnlockedGameModel userUnlockedGameWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedGameModel> response = new List<UserUnlockedGameModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE mg_user_unlocked_games SET ";
                if (!string.IsNullOrEmpty(userUnlockedGame.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userUnlockedGame.Id);
                }
                if (!string.IsNullOrEmpty(userUnlockedGame.UserId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = string.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userUnlockedGame.UserId);
                }
                if (!string.IsNullOrEmpty(userUnlockedGame.GameCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `game_code` = @gameCode");
                    else
                        sql = string.Concat(sql, " `game_code` = @gameCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@gameCode", userUnlockedGame.GameCode);
                }
                if (!string.IsNullOrEmpty(userUnlockedGame.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userUnlockedGame.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userUnlockedGame.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userUnlockedGame.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userUnlockedGame.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userUnlockedGame.UpdatedAt);
                }
                if (userUnlockedGameWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(userUnlockedGameWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userUnlockedGameWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedGameWhere.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = string.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userUnlockedGameWhere.UserId);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedGameWhere.GameCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `game_code` = @gameCodeWhere");
                        else
                            sql = string.Concat(sql, " `game_code` = @gameCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@gameCodeWhere", userUnlockedGameWhere.GameCode);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedGameWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userUnlockedGameWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedGameWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userUnlockedGameWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedGameWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userUnlockedGameWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userUnlockedGame);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserUnlockedGameModel>();
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

        public List<UserUnlockedGameModel> Update(string connectionString, List<UserUnlockedGameModel> userUnlockedGameList, List<UserUnlockedGameModel> userUnlockedGameWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedGameModel> response = new List<UserUnlockedGameModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userUnlockedGameList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE mg_user_unlocked_games SET ";
                                if (!string.IsNullOrEmpty(userUnlockedGameList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userUnlockedGameList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedGameList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = string.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userUnlockedGameList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedGameList[i].GameCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `game_code` = @gameCode" + i);
                                    else
                                        sql = string.Concat(sql, " `game_code` = @gameCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@gameCode" + i, userUnlockedGameList[i].GameCode);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedGameList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userUnlockedGameList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedGameList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userUnlockedGameList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userUnlockedGameList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userUnlockedGameList[i].UpdatedAt);
                                }
                                if (userUnlockedGameWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(userUnlockedGameWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userUnlockedGameWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedGameWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userUnlockedGameWhereList[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedGameWhereList[i].GameCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `game_code` = @gameCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `game_code` = @gameCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@gameCodeWhere" + i, userUnlockedGameWhereList[i].GameCode);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedGameWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userUnlockedGameWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedGameWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userUnlockedGameWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedGameWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userUnlockedGameWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userUnlockedGameList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserUnlockedGameModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserUnlockedGameModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserUnlockedGameModel>();
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

        public List<UserUnlockedGameModel> Delete(string connectionString, UserUnlockedGameModel userUnlockedGame)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedGameModel> response = new List<UserUnlockedGameModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM mg_user_unlocked_games  ";
                if (userUnlockedGame != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(userUnlockedGame.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userUnlockedGame.Id);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedGame.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = string.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userUnlockedGame.UserId);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedGame.GameCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `game_code` = @gameCode");
                        else
                            sql = string.Concat(sql, " `game_code` = @gameCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@gameCode", userUnlockedGame.GameCode);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedGame.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userUnlockedGame.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedGame.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userUnlockedGame.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userUnlockedGame.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userUnlockedGame.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userUnlockedGame);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserUnlockedGameModel>();
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

        public List<UserUnlockedGameModel> Delete(string connectionString, List<UserUnlockedGameModel> userUnlockedGame)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserUnlockedGameModel> response = new List<UserUnlockedGameModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userUnlockedGame.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM mg_user_unlocked_games  ";
                                if (userUnlockedGame[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(userUnlockedGame[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userUnlockedGame[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedGame[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userUnlockedGame[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedGame[i].GameCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `game_code` = @gameCode" + i);
                                        else
                                            sql = string.Concat(sql, " `game_code` = @gameCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@gameCode" + i, userUnlockedGame[i].GameCode);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedGame[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userUnlockedGame[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedGame[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userUnlockedGame[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userUnlockedGame[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userUnlockedGame[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userUnlockedGame[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserUnlockedGameModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserUnlockedGameModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserUnlockedGameModel>();
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
