using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Domain.Models.MiniGame;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.MiniGames
{
    class GameDAO
    {
        private MySqlConnection _connection;
        public List<GameModel> Read(string connectionString, GameModel game, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameModel> response = new List<GameModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `code`, `unlocked_value`, `unlocked_type`, `play_value`, `modified_by`, `created_at`, `updated_at`  FROM mg_games ";
                {
                    if (game != null)
                    {
                        if (!string.IsNullOrEmpty(game.Id))
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
                            cmd.Parameters.AddWithValue("@id", game.Id);
                        }
                        if (!string.IsNullOrEmpty(game.Code))
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
                            cmd.Parameters.AddWithValue("@code", game.Code);
                        }
                        if (!string.IsNullOrEmpty(game.UnlockedValue))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `unlocked_value` = @unlockedValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `unlocked_value` = @unlockedValue ");
                            }
                            cmd.Parameters.AddWithValue("@unlockedValue", game.UnlockedValue);
                        }
                        if (!string.IsNullOrEmpty(game.UnlockedType))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `unlocked_type` = @unlockedType ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `unlocked_type` = @unlockedType ");
                            }
                            cmd.Parameters.AddWithValue("@unlockedType", game.UnlockedType);
                        }
                        if (!string.IsNullOrEmpty(game.PlayValue))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `play_value` = @playValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `play_value` = @playValue ");
                            }
                            cmd.Parameters.AddWithValue("@playValue", game.PlayValue);
                        }
                        if (!string.IsNullOrEmpty(game.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", game.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(game.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", game.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(game.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", game.UpdatedAt);
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
                                    game.Id = reader.GetValue(0).ToString();
                                else
                                    game.Id = null;
                                if (!reader.IsDBNull(1))
                                    game.Code = reader.GetValue(1).ToString();
                                else
                                    game.Code = null;
                                if (!reader.IsDBNull(2))
                                    game.UnlockedValue = reader.GetValue(2).ToString();
                                else
                                    game.UnlockedValue = null;
                                if (!reader.IsDBNull(3))
                                    game.UnlockedType = reader.GetValue(3).ToString();
                                else
                                    game.UnlockedType = null;
                                if (!reader.IsDBNull(4))
                                    game.PlayValue = reader.GetValue(4).ToString();
                                else
                                    game.PlayValue = null;
                                if (!reader.IsDBNull(5))
                                    game.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    game.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    game.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    game.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    game.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    game.UpdatedAt = null;
                                response.Add(game);
                                game = new GameModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<GameModel>();
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
                response = new List<GameModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<GameModel> Read(string connectionString, List<GameModel> gameWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameModel> response = new List<GameModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `code`, `unlocked_value`, `unlocked_type`, `play_value`, `modified_by`, `created_at`, `updated_at`  FROM mg_games ";
                if (gameWhere != null && gameWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < gameWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(gameWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, gameWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(gameWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, gameWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(gameWhere[i].UnlockedValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `unlocked_value` = @unlockedValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@unlockedValue" + i, gameWhere[i].UnlockedValue);
                        }
                        if (!string.IsNullOrEmpty(gameWhere[i].UnlockedType))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `unlocked_type` = @unlockedType" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@unlockedType" + i, gameWhere[i].UnlockedType);
                        }
                        if (!string.IsNullOrEmpty(gameWhere[i].PlayValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `play_value` = @playValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@playValue" + i, gameWhere[i].PlayValue);
                        }
                        if (!string.IsNullOrEmpty(gameWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, gameWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(gameWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, gameWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(gameWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, gameWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != gameWhere.Count - 1)
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
                            GameModel game = new GameModel(); if (!reader.IsDBNull(0))
                                game.Id = reader.GetValue(0).ToString();
                            else
                                game.Id = null;
                            if (!reader.IsDBNull(1))
                                game.Code = reader.GetValue(1).ToString();
                            else
                                game.Code = null;
                            if (!reader.IsDBNull(2))
                                game.UnlockedValue = reader.GetValue(2).ToString();
                            else
                                game.UnlockedValue = null;
                            if (!reader.IsDBNull(3))
                                game.UnlockedType = reader.GetValue(3).ToString();
                            else
                                game.UnlockedType = null;
                            if (!reader.IsDBNull(4))
                                game.PlayValue = reader.GetValue(4).ToString();
                            else
                                game.PlayValue = null;
                            if (!reader.IsDBNull(5))
                                game.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                game.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                game.CreatedAt = reader.GetValue(6).ToString();
                            else
                                game.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                game.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                game.UpdatedAt = null;
                            response.Add(game);
                            game = new GameModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<GameModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<GameModel> Create(string connectionString, GameModel game)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameModel> response = new List<GameModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO mg_games (";
                string values = "(";
                if (!string.IsNullOrEmpty(game.Id))
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
                    cmd.Parameters.AddWithValue("@id", game.Id);
                }
                if (!string.IsNullOrEmpty(game.Code))
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
                    cmd.Parameters.AddWithValue("@code", game.Code);
                }
                if (!string.IsNullOrEmpty(game.UnlockedValue))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `unlocked_value` ");
                        values = string.Concat(values, ", @unlockedValue");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `unlocked_value` ");
                        values = string.Concat(values, " @unlockedValue");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@unlockedValue", game.UnlockedValue);
                }
                if (!string.IsNullOrEmpty(game.UnlockedType))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `unlocked_type` ");
                        values = string.Concat(values, ", @unlockedType");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `unlocked_type` ");
                        values = string.Concat(values, " @unlockedType");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@unlockedType", game.UnlockedType);
                }
                if (!string.IsNullOrEmpty(game.PlayValue))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `play_value` ");
                        values = string.Concat(values, ", @playValue");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `play_value` ");
                        values = string.Concat(values, " @playValue");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@playValue", game.PlayValue);
                }
                if (!string.IsNullOrEmpty(game.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", game.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(game.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", game.CreatedAt);
                }
                if (!string.IsNullOrEmpty(game.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", game.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                game.Id = cmd.LastInsertedId.ToString();
                response.Add(game);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameModel>();
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
        public List<GameModel> Create(string connectionString, List<GameModel> gameList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameModel> response = new List<GameModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < gameList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO mg_games (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(gameList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, gameList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(gameList[i].Code))
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
                                    cmd.Parameters.AddWithValue("@code" + i, gameList[i].Code);
                                }
                                if (!string.IsNullOrEmpty(gameList[i].UnlockedValue))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `unlocked_value` ");
                                        values = string.Concat(values, ", @unlockedValue" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `unlocked_value` ");
                                        values = string.Concat(values, "@unlockedValue" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@unlockedValue" + i, gameList[i].UnlockedValue);
                                }
                                if (!string.IsNullOrEmpty(gameList[i].UnlockedType))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `unlocked_type` ");
                                        values = string.Concat(values, ", @unlockedType" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `unlocked_type` ");
                                        values = string.Concat(values, "@unlockedType" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@unlockedType" + i, gameList[i].UnlockedType);
                                }
                                if (!string.IsNullOrEmpty(gameList[i].PlayValue))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `play_value` ");
                                        values = string.Concat(values, ", @playValue" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `play_value` ");
                                        values = string.Concat(values, "@playValue" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@playValue" + i, gameList[i].PlayValue);
                                }
                                if (!string.IsNullOrEmpty(gameList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, gameList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(gameList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, gameList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(gameList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, gameList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(gameList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<GameModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<GameModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameModel>();
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

        public List<GameModel> Update(string connectionString, GameModel game, GameModel gameWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameModel> response = new List<GameModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE mg_games SET ";
                if (!string.IsNullOrEmpty(game.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", game.Id);
                }
                if (!string.IsNullOrEmpty(game.Code))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `code` = @code");
                    else
                        sql = string.Concat(sql, " `code` = @code");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", game.Code);
                }
                if (!string.IsNullOrEmpty(game.UnlockedValue))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `unlocked_value` = @unlockedValue");
                    else
                        sql = string.Concat(sql, " `unlocked_value` = @unlockedValue");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@unlockedValue", game.UnlockedValue);
                }
                if (!string.IsNullOrEmpty(game.UnlockedType))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `unlocked_type` = @unlockedType");
                    else
                        sql = string.Concat(sql, " `unlocked_type` = @unlockedType");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@unlockedType", game.UnlockedType);
                }
                if (!string.IsNullOrEmpty(game.PlayValue))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `play_value` = @playValue");
                    else
                        sql = string.Concat(sql, " `play_value` = @playValue");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@playValue", game.PlayValue);
                }
                if (!string.IsNullOrEmpty(game.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", game.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(game.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", game.CreatedAt);
                }
                if (!string.IsNullOrEmpty(game.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", game.UpdatedAt);
                }
                if (gameWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(gameWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", gameWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(gameWhere.Code))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `code` = @codeWhere");
                        else
                            sql = string.Concat(sql, " `code` = @codeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@codeWhere", gameWhere.Code);
                    }
                    if (!string.IsNullOrEmpty(gameWhere.UnlockedValue))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `unlocked_value` = @unlockedValueWhere");
                        else
                            sql = string.Concat(sql, " `unlocked_value` = @unlockedValueWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@unlockedValueWhere", gameWhere.UnlockedValue);
                    }
                    if (!string.IsNullOrEmpty(gameWhere.UnlockedType))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `unlocked_type` = @unlockedTypeWhere");
                        else
                            sql = string.Concat(sql, " `unlocked_type` = @unlockedTypeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@unlockedTypeWhere", gameWhere.UnlockedType);
                    }
                    if (!string.IsNullOrEmpty(gameWhere.PlayValue))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `play_value` = @playValueWhere");
                        else
                            sql = string.Concat(sql, " `play_value` = @playValueWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@playValueWhere", gameWhere.PlayValue);
                    }
                    if (!string.IsNullOrEmpty(gameWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", gameWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(gameWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", gameWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(gameWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", gameWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(game);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<GameModel>();
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

        public List<GameModel> Update(string connectionString, List<GameModel> gameList, List<GameModel> gameWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameModel> response = new List<GameModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < gameList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE mg_games SET ";
                                if (!string.IsNullOrEmpty(gameList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, gameList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(gameList[i].Code))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `code` = @code" + i);
                                    else
                                        sql = string.Concat(sql, " `code` = @code" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, gameList[i].Code);
                                }
                                if (!string.IsNullOrEmpty(gameList[i].UnlockedValue))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `unlocked_value` = @unlockedValue" + i);
                                    else
                                        sql = string.Concat(sql, " `unlocked_value` = @unlockedValue" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@unlockedValue" + i, gameList[i].UnlockedValue);
                                }
                                if (!string.IsNullOrEmpty(gameList[i].UnlockedType))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `unlocked_type` = @unlockedType" + i);
                                    else
                                        sql = string.Concat(sql, " `unlocked_type` = @unlockedType" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@unlockedType" + i, gameList[i].UnlockedType);
                                }
                                if (!string.IsNullOrEmpty(gameList[i].PlayValue))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `play_value` = @playValue" + i);
                                    else
                                        sql = string.Concat(sql, " `play_value` = @playValue" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@playValue" + i, gameList[i].PlayValue);
                                }
                                if (!string.IsNullOrEmpty(gameList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, gameList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(gameList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, gameList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(gameList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, gameList[i].UpdatedAt);
                                }
                                if (gameWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(gameWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, gameWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(gameWhereList[i].Code))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `code` = @codeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `code` = @codeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@codeWhere" + i, gameWhereList[i].Code);
                                    }
                                    if (!string.IsNullOrEmpty(gameWhereList[i].UnlockedValue))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `unlocked_value` = @unlockedValueWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `unlocked_value` = @unlockedValueWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@unlockedValueWhere" + i, gameWhereList[i].UnlockedValue);
                                    }
                                    if (!string.IsNullOrEmpty(gameWhereList[i].UnlockedType))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `unlocked_type` = @unlockedTypeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `unlocked_type` = @unlockedTypeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@unlockedTypeWhere" + i, gameWhereList[i].UnlockedType);
                                    }
                                    if (!string.IsNullOrEmpty(gameWhereList[i].PlayValue))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `play_value` = @playValueWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `play_value` = @playValueWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@playValueWhere" + i, gameWhereList[i].PlayValue);
                                    }
                                    if (!string.IsNullOrEmpty(gameWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, gameWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(gameWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, gameWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(gameWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, gameWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(gameList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<GameModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<GameModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameModel>();
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

        public List<GameModel> Delete(string connectionString, GameModel game)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameModel> response = new List<GameModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM mg_games  ";
                if (game != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(game.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", game.Id);
                    }
                    if (!string.IsNullOrEmpty(game.Code))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `code` = @code");
                        else
                            sql = string.Concat(sql, " `code` = @code");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@code", game.Code);
                    }
                    if (!string.IsNullOrEmpty(game.UnlockedValue))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `unlocked_value` = @unlockedValue");
                        else
                            sql = string.Concat(sql, " `unlocked_value` = @unlockedValue");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@unlockedValue", game.UnlockedValue);
                    }
                    if (!string.IsNullOrEmpty(game.UnlockedType))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `unlocked_type` = @unlockedType");
                        else
                            sql = string.Concat(sql, " `unlocked_type` = @unlockedType");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@unlockedType", game.UnlockedType);
                    }
                    if (!string.IsNullOrEmpty(game.PlayValue))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `play_value` = @playValue");
                        else
                            sql = string.Concat(sql, " `play_value` = @playValue");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@playValue", game.PlayValue);
                    }
                    if (!string.IsNullOrEmpty(game.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", game.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(game.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", game.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(game.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", game.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(game);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<GameModel>();
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

        public List<GameModel> Delete(string connectionString, List<GameModel> game)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameModel> response = new List<GameModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < game.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM mg_games  ";
                                if (game[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(game[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, game[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(game[i].Code))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `code` = @code" + i);
                                        else
                                            sql = string.Concat(sql, " `code` = @code" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@code" + i, game[i].Code);
                                    }
                                    if (!string.IsNullOrEmpty(game[i].UnlockedValue))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `unlocked_value` = @unlockedValue" + i);
                                        else
                                            sql = string.Concat(sql, " `unlocked_value` = @unlockedValue" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@unlockedValue" + i, game[i].UnlockedValue);
                                    }
                                    if (!string.IsNullOrEmpty(game[i].UnlockedType))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `unlocked_type` = @unlockedType" + i);
                                        else
                                            sql = string.Concat(sql, " `unlocked_type` = @unlockedType" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@unlockedType" + i, game[i].UnlockedType);
                                    }
                                    if (!string.IsNullOrEmpty(game[i].PlayValue))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `play_value` = @playValue" + i);
                                        else
                                            sql = string.Concat(sql, " `play_value` = @playValue" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@playValue" + i, game[i].PlayValue);
                                    }
                                    if (!string.IsNullOrEmpty(game[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, game[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(game[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, game[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(game[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, game[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(game[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<GameModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<GameModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameModel>();
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
