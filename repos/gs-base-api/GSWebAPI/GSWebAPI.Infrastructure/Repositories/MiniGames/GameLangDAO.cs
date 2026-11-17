using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Domain.Models.MiniGame;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.MiniGames
{
    class GameLangDAO
    {
        private MySqlConnection _connection;
        public List<GameLangModel> Read(string connectionString, GameLangModel gameLang, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameLangModel> response = new List<GameLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `game_code`, `lang_code`, `title`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM mg_game_langs ";
                {
                    if (gameLang != null)
                    {
                        if (!string.IsNullOrEmpty(gameLang.Id))
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
                            cmd.Parameters.AddWithValue("@id", gameLang.Id);
                        }
                        if (!string.IsNullOrEmpty(gameLang.GameCode))
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
                            cmd.Parameters.AddWithValue("@gameCode", gameLang.GameCode);
                        }
                        if (!string.IsNullOrEmpty(gameLang.LangCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `lang_code` = @langCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `lang_code` = @langCode ");
                            }
                            cmd.Parameters.AddWithValue("@langCode", gameLang.LangCode);
                        }
                        if (!string.IsNullOrEmpty(gameLang.Title))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `title` = @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `title` = @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", gameLang.Title);
                        }
                        if (!string.IsNullOrEmpty(gameLang.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", gameLang.Desc);
                        }
                        if (!string.IsNullOrEmpty(gameLang.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", gameLang.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(gameLang.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", gameLang.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(gameLang.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", gameLang.UpdatedAt);
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
                                    gameLang.Id = Reader.GetValue(0).ToString();
                                else
                                    gameLang.Id = null;
                                if (!Reader.IsDBNull(1))
                                    gameLang.GameCode = Reader.GetValue(1).ToString();
                                else
                                    gameLang.GameCode = null;
                                if (!Reader.IsDBNull(2))
                                    gameLang.LangCode = Reader.GetValue(2).ToString();
                                else
                                    gameLang.LangCode = null;
                                if (!Reader.IsDBNull(3))
                                    gameLang.Title = Reader.GetValue(3).ToString();
                                else
                                    gameLang.Title = null;
                                if (!Reader.IsDBNull(4))
                                    gameLang.Desc = Reader.GetValue(4).ToString();
                                else
                                    gameLang.Desc = null;
                                if (!Reader.IsDBNull(5))
                                    gameLang.ModifiedBy = Reader.GetValue(5).ToString();
                                else
                                    gameLang.ModifiedBy = null;
                                if (!Reader.IsDBNull(6))
                                    gameLang.CreatedAt = Reader.GetValue(6).ToString();
                                else
                                    gameLang.CreatedAt = null;
                                if (!Reader.IsDBNull(7))
                                    gameLang.UpdatedAt = Reader.GetValue(7).ToString();
                                else
                                    gameLang.UpdatedAt = null;
                                response.Add(gameLang);
                                gameLang = new GameLangModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<GameLangModel>();
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
                response = new List<GameLangModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<GameLangModel> Read(string connectionString, List<GameLangModel> gameLangWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameLangModel> response = new List<GameLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `game_code`, `lang_code`, `title`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM mg_game_langs ";
                if (gameLangWhere != null && gameLangWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < gameLangWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(gameLangWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, gameLangWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(gameLangWhere[i].GameCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `game_code` = @gameCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gameCode" + i, gameLangWhere[i].GameCode);
                        }
                        if (!string.IsNullOrEmpty(gameLangWhere[i].LangCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lang_code` = @langCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@langCode" + i, gameLangWhere[i].LangCode);
                        }
                        if (!string.IsNullOrEmpty(gameLangWhere[i].Title))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `title` = @title" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@title" + i, gameLangWhere[i].Title);
                        }
                        if (!string.IsNullOrEmpty(gameLangWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, gameLangWhere[i].Desc);
                        }
                        if (!string.IsNullOrEmpty(gameLangWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, gameLangWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(gameLangWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, gameLangWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(gameLangWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, gameLangWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != gameLangWhere.Count - 1)
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
                            GameLangModel gameLang = new GameLangModel(); if (!Reader.IsDBNull(0))
                                gameLang.Id = Reader.GetValue(0).ToString();
                            else
                                gameLang.Id = null;
                            if (!Reader.IsDBNull(1))
                                gameLang.GameCode = Reader.GetValue(1).ToString();
                            else
                                gameLang.GameCode = null;
                            if (!Reader.IsDBNull(2))
                                gameLang.LangCode = Reader.GetValue(2).ToString();
                            else
                                gameLang.LangCode = null;
                            if (!Reader.IsDBNull(3))
                                gameLang.Title = Reader.GetValue(3).ToString();
                            else
                                gameLang.Title = null;
                            if (!Reader.IsDBNull(4))
                                gameLang.Desc = Reader.GetValue(4).ToString();
                            else
                                gameLang.Desc = null;
                            if (!Reader.IsDBNull(5))
                                gameLang.ModifiedBy = Reader.GetValue(5).ToString();
                            else
                                gameLang.ModifiedBy = null;
                            if (!Reader.IsDBNull(6))
                                gameLang.CreatedAt = Reader.GetValue(6).ToString();
                            else
                                gameLang.CreatedAt = null;
                            if (!Reader.IsDBNull(7))
                                gameLang.UpdatedAt = Reader.GetValue(7).ToString();
                            else
                                gameLang.UpdatedAt = null;
                            response.Add(gameLang);
                            gameLang = new GameLangModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<GameLangModel>();
                    Reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameLangModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<GameLangModel> Create(string connectionString, GameLangModel gameLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameLangModel> response = new List<GameLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO mg_game_langs (";
                string values = "(";
                if (!string.IsNullOrEmpty(gameLang.Id))
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
                    cmd.Parameters.AddWithValue("@id", gameLang.Id);
                }
                if (!string.IsNullOrEmpty(gameLang.GameCode))
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
                    cmd.Parameters.AddWithValue("@gameCode", gameLang.GameCode);
                }
                if (!string.IsNullOrEmpty(gameLang.LangCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `lang_code` ");
                        values = string.Concat(values, ", @langCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `lang_code` ");
                        values = string.Concat(values, " @langCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@langCode", gameLang.LangCode);
                }
                if (!string.IsNullOrEmpty(gameLang.Title))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `title` ");
                        values = string.Concat(values, ", @title");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `title` ");
                        values = string.Concat(values, " @title");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@title", gameLang.Title);
                }
                if (!string.IsNullOrEmpty(gameLang.Desc))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `desc` ");
                        values = string.Concat(values, ", @desc");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `desc` ");
                        values = string.Concat(values, " @desc");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", gameLang.Desc);
                }
                if (!string.IsNullOrEmpty(gameLang.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", gameLang.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(gameLang.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", gameLang.CreatedAt);
                }
                if (!string.IsNullOrEmpty(gameLang.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", gameLang.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                gameLang.Id = cmd.LastInsertedId.ToString();
                response.Add(gameLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameLangModel>();
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
        public List<GameLangModel> Create(string connectionString, List<GameLangModel> gameLangList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameLangModel> response = new List<GameLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < gameLangList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO mg_game_langs (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(gameLangList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, gameLangList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(gameLangList[i].GameCode))
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
                                    cmd.Parameters.AddWithValue("@gameCode" + i, gameLangList[i].GameCode);
                                }
                                if (!string.IsNullOrEmpty(gameLangList[i].LangCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `lang_code` ");
                                        values = string.Concat(values, ", @langCode" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `lang_code` ");
                                        values = string.Concat(values, "@langCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@langCode" + i, gameLangList[i].LangCode);
                                }
                                if (!string.IsNullOrEmpty(gameLangList[i].Title))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `title` ");
                                        values = string.Concat(values, ", @title" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `title` ");
                                        values = string.Concat(values, "@title" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@title" + i, gameLangList[i].Title);
                                }
                                if (!string.IsNullOrEmpty(gameLangList[i].Desc))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `desc` ");
                                        values = string.Concat(values, ", @desc" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `desc` ");
                                        values = string.Concat(values, "@desc" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, gameLangList[i].Desc);
                                }
                                if (!string.IsNullOrEmpty(gameLangList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, gameLangList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(gameLangList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, gameLangList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(gameLangList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, gameLangList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(gameLangList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<GameLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<GameLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameLangModel>();
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

        public List<GameLangModel> Update(string connectionString, GameLangModel gameLang, GameLangModel gameLangWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameLangModel> response = new List<GameLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE mg_game_langs SET ";
                if (!string.IsNullOrEmpty(gameLang.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", gameLang.Id);
                }
                if (!string.IsNullOrEmpty(gameLang.GameCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `game_code` = @gameCode");
                    else
                        sql = string.Concat(sql, " `game_code` = @gameCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@gameCode", gameLang.GameCode);
                }
                if (!string.IsNullOrEmpty(gameLang.LangCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `lang_code` = @langCode");
                    else
                        sql = string.Concat(sql, " `lang_code` = @langCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@langCode", gameLang.LangCode);
                }
                if (!string.IsNullOrEmpty(gameLang.Title))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `title` = @title");
                    else
                        sql = string.Concat(sql, " `title` = @title");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@title", gameLang.Title);
                }
                if (!string.IsNullOrEmpty(gameLang.Desc))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `desc` = @desc");
                    else
                        sql = string.Concat(sql, " `desc` = @desc");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", gameLang.Desc);
                }
                if (!string.IsNullOrEmpty(gameLang.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", gameLang.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(gameLang.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", gameLang.CreatedAt);
                }
                if (!string.IsNullOrEmpty(gameLang.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", gameLang.UpdatedAt);
                }
                if (gameLangWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(gameLangWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", gameLangWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(gameLangWhere.GameCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `game_code` = @gameCodeWhere");
                        else
                            sql = string.Concat(sql, " `game_code` = @gameCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@gameCodeWhere", gameLangWhere.GameCode);
                    }
                    if (!string.IsNullOrEmpty(gameLangWhere.LangCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `lang_code` = @langCodeWhere");
                        else
                            sql = string.Concat(sql, " `lang_code` = @langCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCodeWhere", gameLangWhere.LangCode);
                    }
                    if (!string.IsNullOrEmpty(gameLangWhere.Title))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `title` = @titleWhere");
                        else
                            sql = string.Concat(sql, " `title` = @titleWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@titleWhere", gameLangWhere.Title);
                    }
                    if (!string.IsNullOrEmpty(gameLangWhere.Desc))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `desc` = @descWhere");
                        else
                            sql = string.Concat(sql, " `desc` = @descWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descWhere", gameLangWhere.Desc);
                    }
                    if (!string.IsNullOrEmpty(gameLangWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", gameLangWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(gameLangWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", gameLangWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(gameLangWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", gameLangWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(gameLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<GameLangModel>();
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

        public List<GameLangModel> Update(string connectionString, List<GameLangModel> gameLangList, List<GameLangModel> gameLangWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameLangModel> response = new List<GameLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < gameLangList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE mg_game_langs SET ";
                                if (!string.IsNullOrEmpty(gameLangList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, gameLangList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(gameLangList[i].GameCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `game_code` = @gameCode" + i);
                                    else
                                        sql = string.Concat(sql, " `game_code` = @gameCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@gameCode" + i, gameLangList[i].GameCode);
                                }
                                if (!string.IsNullOrEmpty(gameLangList[i].LangCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `lang_code` = @langCode" + i);
                                    else
                                        sql = string.Concat(sql, " `lang_code` = @langCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@langCode" + i, gameLangList[i].LangCode);
                                }
                                if (!string.IsNullOrEmpty(gameLangList[i].Title))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `title` = @title" + i);
                                    else
                                        sql = string.Concat(sql, " `title` = @title" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@title" + i, gameLangList[i].Title);
                                }
                                if (!string.IsNullOrEmpty(gameLangList[i].Desc))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `desc` = @desc" + i);
                                    else
                                        sql = string.Concat(sql, " `desc` = @desc" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, gameLangList[i].Desc);
                                }
                                if (!string.IsNullOrEmpty(gameLangList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, gameLangList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(gameLangList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, gameLangList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(gameLangList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, gameLangList[i].UpdatedAt);
                                }
                                if (gameLangWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(gameLangWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, gameLangWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(gameLangWhereList[i].GameCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `game_code` = @gameCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `game_code` = @gameCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@gameCodeWhere" + i, gameLangWhereList[i].GameCode);
                                    }
                                    if (!string.IsNullOrEmpty(gameLangWhereList[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `lang_code` = @langCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `lang_code` = @langCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCodeWhere" + i, gameLangWhereList[i].LangCode);
                                    }
                                    if (!string.IsNullOrEmpty(gameLangWhereList[i].Title))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `title` = @titleWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `title` = @titleWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@titleWhere" + i, gameLangWhereList[i].Title);
                                    }
                                    if (!string.IsNullOrEmpty(gameLangWhereList[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `desc` = @descWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `desc` = @descWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descWhere" + i, gameLangWhereList[i].Desc);
                                    }
                                    if (!string.IsNullOrEmpty(gameLangWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, gameLangWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(gameLangWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, gameLangWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(gameLangWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, gameLangWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(gameLangList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<GameLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<GameLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameLangModel>();
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

        public List<GameLangModel> Delete(string connectionString, GameLangModel gameLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameLangModel> response = new List<GameLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM mg_game_langs  ";
                if (gameLang != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(gameLang.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", gameLang.Id);
                    }
                    if (!string.IsNullOrEmpty(gameLang.GameCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `game_code` = @gameCode");
                        else
                            sql = string.Concat(sql, " `game_code` = @gameCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@gameCode", gameLang.GameCode);
                    }
                    if (!string.IsNullOrEmpty(gameLang.LangCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `lang_code` = @langCode");
                        else
                            sql = string.Concat(sql, " `lang_code` = @langCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCode", gameLang.LangCode);
                    }
                    if (!string.IsNullOrEmpty(gameLang.Title))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `title` = @title");
                        else
                            sql = string.Concat(sql, " `title` = @title");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@title", gameLang.Title);
                    }
                    if (!string.IsNullOrEmpty(gameLang.Desc))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `desc` = @desc");
                        else
                            sql = string.Concat(sql, " `desc` = @desc");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@desc", gameLang.Desc);
                    }
                    if (!string.IsNullOrEmpty(gameLang.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", gameLang.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(gameLang.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", gameLang.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(gameLang.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", gameLang.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(gameLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<GameLangModel>();
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

        public List<GameLangModel> Delete(string connectionString, List<GameLangModel> gameLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameLangModel> response = new List<GameLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < gameLang.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM mg_game_langs  ";
                                if (gameLang[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(gameLang[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, gameLang[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(gameLang[i].GameCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `game_code` = @gameCode" + i);
                                        else
                                            sql = string.Concat(sql, " `game_code` = @gameCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@gameCode" + i, gameLang[i].GameCode);
                                    }
                                    if (!string.IsNullOrEmpty(gameLang[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `lang_code` = @langCode" + i);
                                        else
                                            sql = string.Concat(sql, " `lang_code` = @langCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCode" + i, gameLang[i].LangCode);
                                    }
                                    if (!string.IsNullOrEmpty(gameLang[i].Title))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `title` = @title" + i);
                                        else
                                            sql = string.Concat(sql, " `title` = @title" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@title" + i, gameLang[i].Title);
                                    }
                                    if (!string.IsNullOrEmpty(gameLang[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `desc` = @desc" + i);
                                        else
                                            sql = string.Concat(sql, " `desc` = @desc" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@desc" + i, gameLang[i].Desc);
                                    }
                                    if (!string.IsNullOrEmpty(gameLang[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, gameLang[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(gameLang[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, gameLang[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(gameLang[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, gameLang[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(gameLang[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<GameLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<GameLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameLangModel>();
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
