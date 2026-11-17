using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Services.Generals;
using GSWebAPI.GSWebAPI.Domain.Models.General;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.Generals
{
    class GameVersionDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, GameVersionModel> ReadD(string connectionString, GameVersionModel gameVersion, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, GameVersionModel> response = new Dictionary<string, GameVersionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `version`, `platform`, `description`, `release_date`, `end_date`, `modified_by`, `created_at`, `updated_at`  FROM gwp_game_versions ";
                {
                    if (gameVersion != null)
                    {
                        if (!string.IsNullOrEmpty(gameVersion.Id))
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
                            cmd.Parameters.AddWithValue("@id", gameVersion.Id);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.Version))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `version` = @version ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `version` = @version ");
                            }
                            cmd.Parameters.AddWithValue("@version", gameVersion.Version);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.Platform))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `platform` = @platform ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `platform` = @platform ");
                            }
                            cmd.Parameters.AddWithValue("@platform", gameVersion.Platform);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", gameVersion.Description);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.ReleaseDate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `release_date` = @releaseDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `release_date` = @releaseDate ");
                            }
                            cmd.Parameters.AddWithValue("@releaseDate", gameVersion.ReleaseDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.EndDate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `end_date` = @endDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `end_date` = @endDate ");
                            }
                            cmd.Parameters.AddWithValue("@endDate", gameVersion.EndDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", gameVersion.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", gameVersion.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", gameVersion.UpdatedAt);
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
                                    gameVersion.Id = reader.GetValue(0).ToString();
                                else
                                    gameVersion.Id = null;
                                if (!reader.IsDBNull(1))
                                    gameVersion.Version = reader.GetValue(1).ToString();
                                else
                                    gameVersion.Version = null;
                                if (!reader.IsDBNull(2))
                                    gameVersion.Platform = reader.GetValue(2).ToString();
                                else
                                    gameVersion.Platform = null;
                                if (!reader.IsDBNull(3))
                                    gameVersion.Description = reader.GetValue(3).ToString();
                                else
                                    gameVersion.Description = null;
                                if (!reader.IsDBNull(4))
                                    gameVersion.ReleaseDate = reader.GetValue(4).ToString();
                                else
                                    gameVersion.ReleaseDate = null;
                                if (!reader.IsDBNull(5))
                                    gameVersion.EndDate = reader.GetValue(5).ToString();
                                else
                                    gameVersion.EndDate = null;
                                if (!reader.IsDBNull(6))
                                    gameVersion.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    gameVersion.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    gameVersion.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    gameVersion.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    gameVersion.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    gameVersion.UpdatedAt = null;
                                response.Add(gameVersion.Id, gameVersion);
                                gameVersion = new GameVersionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, GameVersionModel>();
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
                response = new Dictionary<string, GameVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, GameVersionModel> ReadD(string connectionString, GameVersionModel gameVersion, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, GameVersionModel> response = new Dictionary<string, GameVersionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `version`, `platform`, `description`, `release_date`, `end_date`, `modified_by`, `created_at`, `updated_at`  FROM gwp_game_versions ";
                {
                    if (gameVersion != null)
                    {
                        if (!string.IsNullOrEmpty(gameVersion.Id))
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
                            cmd.Parameters.AddWithValue("@id", gameVersion.Id);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.Version))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `version` = @version ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `version` = @version ");
                            }
                            cmd.Parameters.AddWithValue("@version", gameVersion.Version);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.Platform))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `platform` = @platform ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `platform` = @platform ");
                            }
                            cmd.Parameters.AddWithValue("@platform", gameVersion.Platform);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", gameVersion.Description);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.ReleaseDate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `release_date` = @releaseDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `release_date` = @releaseDate ");
                            }
                            cmd.Parameters.AddWithValue("@releaseDate", gameVersion.ReleaseDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.EndDate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `end_date` = @endDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `end_date` = @endDate ");
                            }
                            cmd.Parameters.AddWithValue("@endDate", gameVersion.EndDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", gameVersion.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", gameVersion.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", gameVersion.UpdatedAt);
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
                                    gameVersion.Id = reader.GetValue(0).ToString();
                                else
                                    gameVersion.Id = null;
                                if (!reader.IsDBNull(1))
                                    gameVersion.Version = reader.GetValue(1).ToString();
                                else
                                    gameVersion.Version = null;
                                if (!reader.IsDBNull(2))
                                    gameVersion.Platform = reader.GetValue(2).ToString();
                                else
                                    gameVersion.Platform = null;
                                if (!reader.IsDBNull(3))
                                    gameVersion.Description = reader.GetValue(3).ToString();
                                else
                                    gameVersion.Description = null;
                                if (!reader.IsDBNull(4))
                                    gameVersion.ReleaseDate = reader.GetValue(4).ToString();
                                else
                                    gameVersion.ReleaseDate = null;
                                if (!reader.IsDBNull(5))
                                    gameVersion.EndDate = reader.GetValue(5).ToString();
                                else
                                    gameVersion.EndDate = null;
                                if (!reader.IsDBNull(6))
                                    gameVersion.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    gameVersion.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    gameVersion.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    gameVersion.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    gameVersion.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    gameVersion.UpdatedAt = null;
                                response.Add(gameVersion.Id, gameVersion);
                                gameVersion = new GameVersionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, GameVersionModel>();
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
                response = new Dictionary<string, GameVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, GameVersionModel> ReadD(string connectionString, List<GameVersionModel> gameVersionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, GameVersionModel> response = new Dictionary<string, GameVersionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `version`, `platform`, `description`, `release_date`, `end_date`, `modified_by`, `created_at`, `updated_at`  FROM gwp_game_versions ";
                if (gameVersionWhere != null && gameVersionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < gameVersionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, gameVersionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Version))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `version` = @version" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@version" + i, gameVersionWhere[i].Version);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Platform))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform` = @platform" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platform" + i, gameVersionWhere[i].Platform);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, gameVersionWhere[i].Description);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].ReleaseDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `release_date` = @releaseDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@releaseDate" + i, gameVersionWhere[i].ReleaseDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].EndDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `end_date` = @endDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@endDate" + i, gameVersionWhere[i].EndDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, gameVersionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, gameVersionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, gameVersionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != gameVersionWhere.Count - 1)
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
                            GameVersionModel gameVersion = new GameVersionModel(); if (!reader.IsDBNull(0))
                                gameVersion.Id = reader.GetValue(0).ToString();
                            else
                                gameVersion.Id = null;
                            if (!reader.IsDBNull(1))
                                gameVersion.Version = reader.GetValue(1).ToString();
                            else
                                gameVersion.Version = null;
                            if (!reader.IsDBNull(2))
                                gameVersion.Platform = reader.GetValue(2).ToString();
                            else
                                gameVersion.Platform = null;
                            if (!reader.IsDBNull(3))
                                gameVersion.Description = reader.GetValue(3).ToString();
                            else
                                gameVersion.Description = null;
                            if (!reader.IsDBNull(4))
                                gameVersion.ReleaseDate = reader.GetValue(4).ToString();
                            else
                                gameVersion.ReleaseDate = null;
                            if (!reader.IsDBNull(5))
                                gameVersion.EndDate = reader.GetValue(5).ToString();
                            else
                                gameVersion.EndDate = null;
                            if (!reader.IsDBNull(6))
                                gameVersion.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                gameVersion.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                gameVersion.CreatedAt = reader.GetValue(7).ToString();
                            else
                                gameVersion.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                gameVersion.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                gameVersion.UpdatedAt = null;
                            response.Add(gameVersion.Id, gameVersion);
                            gameVersion = new GameVersionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, GameVersionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, GameVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, GameVersionModel> ReadD(string connectionString, List<GameVersionModel> gameVersionWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, GameVersionModel> response = new Dictionary<string, GameVersionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `version`, `platform`, `description`, `release_date`, `end_date`, `modified_by`, `created_at`, `updated_at`  FROM gwp_game_versions ";
                if (gameVersionWhere != null && gameVersionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < gameVersionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, gameVersionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Version))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `version` = @version" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@version" + i, gameVersionWhere[i].Version);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Platform))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform` = @platform" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platform" + i, gameVersionWhere[i].Platform);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, gameVersionWhere[i].Description);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].ReleaseDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `release_date` = @releaseDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@releaseDate" + i, gameVersionWhere[i].ReleaseDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].EndDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `end_date` = @endDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@endDate" + i, gameVersionWhere[i].EndDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, gameVersionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, gameVersionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, gameVersionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != gameVersionWhere.Count - 1)
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
                            GameVersionModel gameVersion = new GameVersionModel(); if (!reader.IsDBNull(0))
                                gameVersion.Id = reader.GetValue(0).ToString();
                            else
                                gameVersion.Id = null;
                            if (!reader.IsDBNull(1))
                                gameVersion.Version = reader.GetValue(1).ToString();
                            else
                                gameVersion.Version = null;
                            if (!reader.IsDBNull(2))
                                gameVersion.Platform = reader.GetValue(2).ToString();
                            else
                                gameVersion.Platform = null;
                            if (!reader.IsDBNull(3))
                                gameVersion.Description = reader.GetValue(3).ToString();
                            else
                                gameVersion.Description = null;
                            if (!reader.IsDBNull(4))
                                gameVersion.ReleaseDate = reader.GetValue(4).ToString();
                            else
                                gameVersion.ReleaseDate = null;
                            if (!reader.IsDBNull(5))
                                gameVersion.EndDate = reader.GetValue(5).ToString();
                            else
                                gameVersion.EndDate = null;
                            if (!reader.IsDBNull(6))
                                gameVersion.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                gameVersion.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                gameVersion.CreatedAt = reader.GetValue(7).ToString();
                            else
                                gameVersion.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                gameVersion.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                gameVersion.UpdatedAt = null;
                            response.Add(gameVersion.Id, gameVersion);
                            gameVersion = new GameVersionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, GameVersionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, GameVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<GameVersionModel> Read(string connectionString, GameVersionModel gameVersion, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameVersionModel> response = new List<GameVersionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `version`, `platform`, `description`, `release_date`, `end_date`, `modified_by`, `created_at`, `updated_at`  FROM gwp_game_versions ";
                {
                    if (gameVersion != null)
                    {
                        if (!string.IsNullOrEmpty(gameVersion.Id))
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
                            cmd.Parameters.AddWithValue("@id", gameVersion.Id);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.Version))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `version` = @version ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `version` = @version ");
                            }
                            cmd.Parameters.AddWithValue("@version", gameVersion.Version);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.Platform))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `platform` = @platform ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `platform` = @platform ");
                            }
                            cmd.Parameters.AddWithValue("@platform", gameVersion.Platform);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", gameVersion.Description);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.ReleaseDate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `release_date` = @releaseDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `release_date` = @releaseDate ");
                            }
                            cmd.Parameters.AddWithValue("@releaseDate", gameVersion.ReleaseDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.EndDate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `end_date` = @endDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `end_date` = @endDate ");
                            }
                            cmd.Parameters.AddWithValue("@endDate", gameVersion.EndDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", gameVersion.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", gameVersion.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", gameVersion.UpdatedAt);
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
                                    gameVersion.Id = reader.GetValue(0).ToString();
                                else
                                    gameVersion.Id = null;
                                if (!reader.IsDBNull(1))
                                    gameVersion.Version = reader.GetValue(1).ToString();
                                else
                                    gameVersion.Version = null;
                                if (!reader.IsDBNull(2))
                                    gameVersion.Platform = reader.GetValue(2).ToString();
                                else
                                    gameVersion.Platform = null;
                                if (!reader.IsDBNull(3))
                                    gameVersion.Description = reader.GetValue(3).ToString();
                                else
                                    gameVersion.Description = null;
                                if (!reader.IsDBNull(4))
                                    gameVersion.ReleaseDate = reader.GetValue(4).ToString();
                                else
                                    gameVersion.ReleaseDate = null;
                                if (!reader.IsDBNull(5))
                                    gameVersion.EndDate = reader.GetValue(5).ToString();
                                else
                                    gameVersion.EndDate = null;
                                if (!reader.IsDBNull(6))
                                    gameVersion.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    gameVersion.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    gameVersion.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    gameVersion.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    gameVersion.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    gameVersion.UpdatedAt = null;
                                response.Add(gameVersion);
                                gameVersion = new GameVersionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<GameVersionModel>();
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
                response = new List<GameVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<GameVersionModel> Read(string connectionString, GameVersionModel gameVersion, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameVersionModel> response = new List<GameVersionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `version`, `platform`, `description`, `release_date`, `end_date`, `modified_by`, `created_at`, `updated_at`  FROM gwp_game_versions ";
                {
                    if (gameVersion != null)
                    {
                        if (!string.IsNullOrEmpty(gameVersion.Id))
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
                            cmd.Parameters.AddWithValue("@id", gameVersion.Id);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.Version))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `version` = @version ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `version` = @version ");
                            }
                            cmd.Parameters.AddWithValue("@version", gameVersion.Version);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.Platform))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `platform` = @platform ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `platform` = @platform ");
                            }
                            cmd.Parameters.AddWithValue("@platform", gameVersion.Platform);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", gameVersion.Description);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.ReleaseDate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `release_date` = @releaseDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `release_date` = @releaseDate ");
                            }
                            cmd.Parameters.AddWithValue("@releaseDate", gameVersion.ReleaseDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.EndDate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `end_date` = @endDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `end_date` = @endDate ");
                            }
                            cmd.Parameters.AddWithValue("@endDate", gameVersion.EndDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", gameVersion.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", gameVersion.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(gameVersion.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", gameVersion.UpdatedAt);
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
                                    gameVersion.Id = reader.GetValue(0).ToString();
                                else
                                    gameVersion.Id = null;
                                if (!reader.IsDBNull(1))
                                    gameVersion.Version = reader.GetValue(1).ToString();
                                else
                                    gameVersion.Version = null;
                                if (!reader.IsDBNull(2))
                                    gameVersion.Platform = reader.GetValue(2).ToString();
                                else
                                    gameVersion.Platform = null;
                                if (!reader.IsDBNull(3))
                                    gameVersion.Description = reader.GetValue(3).ToString();
                                else
                                    gameVersion.Description = null;
                                if (!reader.IsDBNull(4))
                                    gameVersion.ReleaseDate = reader.GetValue(4).ToString();
                                else
                                    gameVersion.ReleaseDate = null;
                                if (!reader.IsDBNull(5))
                                    gameVersion.EndDate = reader.GetValue(5).ToString();
                                else
                                    gameVersion.EndDate = null;
                                if (!reader.IsDBNull(6))
                                    gameVersion.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    gameVersion.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    gameVersion.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    gameVersion.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    gameVersion.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    gameVersion.UpdatedAt = null;
                                response.Add(gameVersion);
                                gameVersion = new GameVersionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<GameVersionModel>();
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
                response = new List<GameVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<GameVersionModel> Read(string connectionString, List<GameVersionModel> gameVersionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameVersionModel> response = new List<GameVersionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `version`, `platform`, `description`, `release_date`, `end_date`, `modified_by`, `created_at`, `updated_at`  FROM gwp_game_versions ";
                if (gameVersionWhere != null && gameVersionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < gameVersionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, gameVersionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Version))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `version` = @version" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@version" + i, gameVersionWhere[i].Version);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Platform))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform` = @platform" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platform" + i, gameVersionWhere[i].Platform);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, gameVersionWhere[i].Description);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].ReleaseDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `release_date` = @releaseDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@releaseDate" + i, gameVersionWhere[i].ReleaseDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].EndDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `end_date` = @endDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@endDate" + i, gameVersionWhere[i].EndDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, gameVersionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, gameVersionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, gameVersionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != gameVersionWhere.Count - 1)
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
                            GameVersionModel gameVersion = new GameVersionModel(); if (!reader.IsDBNull(0))
                                gameVersion.Id = reader.GetValue(0).ToString();
                            else
                                gameVersion.Id = null;
                            if (!reader.IsDBNull(1))
                                gameVersion.Version = reader.GetValue(1).ToString();
                            else
                                gameVersion.Version = null;
                            if (!reader.IsDBNull(2))
                                gameVersion.Platform = reader.GetValue(2).ToString();
                            else
                                gameVersion.Platform = null;
                            if (!reader.IsDBNull(3))
                                gameVersion.Description = reader.GetValue(3).ToString();
                            else
                                gameVersion.Description = null;
                            if (!reader.IsDBNull(4))
                                gameVersion.ReleaseDate = reader.GetValue(4).ToString();
                            else
                                gameVersion.ReleaseDate = null;
                            if (!reader.IsDBNull(5))
                                gameVersion.EndDate = reader.GetValue(5).ToString();
                            else
                                gameVersion.EndDate = null;
                            if (!reader.IsDBNull(6))
                                gameVersion.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                gameVersion.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                gameVersion.CreatedAt = reader.GetValue(7).ToString();
                            else
                                gameVersion.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                gameVersion.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                gameVersion.UpdatedAt = null;
                            response.Add(gameVersion);
                            gameVersion = new GameVersionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<GameVersionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<GameVersionModel> Read(string connectionString, List<GameVersionModel> gameVersionWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameVersionModel> response = new List<GameVersionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `version`, `platform`, `description`, `release_date`, `end_date`, `modified_by`, `created_at`, `updated_at`  FROM gwp_game_versions ";
                if (gameVersionWhere != null && gameVersionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < gameVersionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, gameVersionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Version))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `version` = @version" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@version" + i, gameVersionWhere[i].Version);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Platform))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `platform` = @platform" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@platform" + i, gameVersionWhere[i].Platform);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, gameVersionWhere[i].Description);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].ReleaseDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `release_date` = @releaseDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@releaseDate" + i, gameVersionWhere[i].ReleaseDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].EndDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `end_date` = @endDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@endDate" + i, gameVersionWhere[i].EndDate);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, gameVersionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, gameVersionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(gameVersionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, gameVersionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != gameVersionWhere.Count - 1)
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
                            GameVersionModel gameVersion = new GameVersionModel(); if (!reader.IsDBNull(0))
                                gameVersion.Id = reader.GetValue(0).ToString();
                            else
                                gameVersion.Id = null;
                            if (!reader.IsDBNull(1))
                                gameVersion.Version = reader.GetValue(1).ToString();
                            else
                                gameVersion.Version = null;
                            if (!reader.IsDBNull(2))
                                gameVersion.Platform = reader.GetValue(2).ToString();
                            else
                                gameVersion.Platform = null;
                            if (!reader.IsDBNull(3))
                                gameVersion.Description = reader.GetValue(3).ToString();
                            else
                                gameVersion.Description = null;
                            if (!reader.IsDBNull(4))
                                gameVersion.ReleaseDate = reader.GetValue(4).ToString();
                            else
                                gameVersion.ReleaseDate = null;
                            if (!reader.IsDBNull(5))
                                gameVersion.EndDate = reader.GetValue(5).ToString();
                            else
                                gameVersion.EndDate = null;
                            if (!reader.IsDBNull(6))
                                gameVersion.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                gameVersion.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                gameVersion.CreatedAt = reader.GetValue(7).ToString();
                            else
                                gameVersion.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                gameVersion.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                gameVersion.UpdatedAt = null;
                            response.Add(gameVersion);
                            gameVersion = new GameVersionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<GameVersionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<GameVersionModel> Create(string connectionString, GameVersionModel gameVersion)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameVersionModel> response = new List<GameVersionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO gwp_game_versions (";
                string values = "(";
                if (!string.IsNullOrEmpty(gameVersion.Id))
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
                    cmd.Parameters.AddWithValue("@id", gameVersion.Id);
                }
                if (!string.IsNullOrEmpty(gameVersion.Version))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `version` ");
                        values = string.Concat(values, ", @version");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `version` ");
                        values = string.Concat(values, " @version");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@version", gameVersion.Version);
                }
                if (!string.IsNullOrEmpty(gameVersion.Platform))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `platform` ");
                        values = string.Concat(values, ", @platform");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `platform` ");
                        values = string.Concat(values, " @platform");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@platform", gameVersion.Platform);
                }
                if (!string.IsNullOrEmpty(gameVersion.Description))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `description` ");
                        values = string.Concat(values, ", @description");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `description` ");
                        values = string.Concat(values, " @description");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@description", gameVersion.Description);
                }
                if (!string.IsNullOrEmpty(gameVersion.ReleaseDate))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `release_date` ");
                        values = string.Concat(values, ", @releaseDate");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `release_date` ");
                        values = string.Concat(values, " @releaseDate");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@releaseDate", gameVersion.ReleaseDate);
                }
                if (!string.IsNullOrEmpty(gameVersion.EndDate))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `end_date` ");
                        values = string.Concat(values, ", @endDate");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `end_date` ");
                        values = string.Concat(values, " @endDate");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@endDate", gameVersion.EndDate);
                }
                if (!string.IsNullOrEmpty(gameVersion.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", gameVersion.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(gameVersion.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", gameVersion.CreatedAt);
                }
                if (!string.IsNullOrEmpty(gameVersion.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", gameVersion.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                gameVersion.Id = cmd.LastInsertedId.ToString();
                response.Add(gameVersion);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameVersionModel>();
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
        public List<GameVersionModel> Create(string connectionString, List<GameVersionModel> gameVersionList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameVersionModel> response = new List<GameVersionModel>();
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
                        for (int i = 0; i < gameVersionList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO gwp_game_versions (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(gameVersionList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, gameVersionList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].Version))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `version` ");
                                        values = string.Concat(values, ", @version" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `version` ");
                                        values = string.Concat(values, "@version" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@version" + i, gameVersionList[i].Version);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].Platform))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `platform` ");
                                        values = string.Concat(values, ", @platform" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `platform` ");
                                        values = string.Concat(values, "@platform" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@platform" + i, gameVersionList[i].Platform);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].Description))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `description` ");
                                        values = string.Concat(values, ", @description" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `description` ");
                                        values = string.Concat(values, "@description" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@description" + i, gameVersionList[i].Description);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].ReleaseDate))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `release_date` ");
                                        values = string.Concat(values, ", @releaseDate" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `release_date` ");
                                        values = string.Concat(values, "@releaseDate" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@releaseDate" + i, gameVersionList[i].ReleaseDate);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].EndDate))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `end_date` ");
                                        values = string.Concat(values, ", @endDate" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `end_date` ");
                                        values = string.Concat(values, "@endDate" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@endDate" + i, gameVersionList[i].EndDate);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, gameVersionList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, gameVersionList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, gameVersionList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(gameVersionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<GameVersionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<GameVersionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameVersionModel>();
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

        public List<GameVersionModel> Update(string connectionString, GameVersionModel gameVersion, GameVersionModel gameVersionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameVersionModel> response = new List<GameVersionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE gwp_game_versions SET ";
                if (!string.IsNullOrEmpty(gameVersion.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", gameVersion.Id);
                }
                if (!string.IsNullOrEmpty(gameVersion.Version))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `version` = @version");
                    else
                        sql = string.Concat(sql, " `version` = @version");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@version", gameVersion.Version);
                }
                if (!string.IsNullOrEmpty(gameVersion.Platform))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `platform` = @platform");
                    else
                        sql = string.Concat(sql, " `platform` = @platform");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@platform", gameVersion.Platform);
                }
                if (!string.IsNullOrEmpty(gameVersion.Description))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `description` = @description");
                    else
                        sql = string.Concat(sql, " `description` = @description");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@description", gameVersion.Description);
                }
                if (!string.IsNullOrEmpty(gameVersion.ReleaseDate))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `release_date` = @releaseDate");
                    else
                        sql = string.Concat(sql, " `release_date` = @releaseDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@releaseDate", gameVersion.ReleaseDate);
                }
                if (!string.IsNullOrEmpty(gameVersion.EndDate))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `end_date` = @endDate");
                    else
                        sql = string.Concat(sql, " `end_date` = @endDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@endDate", gameVersion.EndDate);
                }
                if (!string.IsNullOrEmpty(gameVersion.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", gameVersion.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(gameVersion.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", gameVersion.CreatedAt);
                }
                if (!string.IsNullOrEmpty(gameVersion.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", gameVersion.UpdatedAt);
                }
                if (gameVersionWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(gameVersionWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", gameVersionWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(gameVersionWhere.Version))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `version` = @versionWhere");
                        else
                            sql = string.Concat(sql, " `version` = @versionWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@versionWhere", gameVersionWhere.Version);
                    }
                    if (!string.IsNullOrEmpty(gameVersionWhere.Platform))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform` = @platformWhere");
                        else
                            sql = string.Concat(sql, " `platform` = @platformWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platformWhere", gameVersionWhere.Platform);
                    }
                    if (!string.IsNullOrEmpty(gameVersionWhere.Description))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `description` = @descriptionWhere");
                        else
                            sql = string.Concat(sql, " `description` = @descriptionWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descriptionWhere", gameVersionWhere.Description);
                    }
                    if (!string.IsNullOrEmpty(gameVersionWhere.ReleaseDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `release_date` = @releaseDateWhere");
                        else
                            sql = string.Concat(sql, " `release_date` = @releaseDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@releaseDateWhere", gameVersionWhere.ReleaseDate);
                    }
                    if (!string.IsNullOrEmpty(gameVersionWhere.EndDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `end_date` = @endDateWhere");
                        else
                            sql = string.Concat(sql, " `end_date` = @endDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@endDateWhere", gameVersionWhere.EndDate);
                    }
                    if (!string.IsNullOrEmpty(gameVersionWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", gameVersionWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(gameVersionWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", gameVersionWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(gameVersionWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", gameVersionWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(gameVersion);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<GameVersionModel>();
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

        public List<GameVersionModel> Update(string connectionString, List<GameVersionModel> gameVersionList, List<GameVersionModel> gameVersionWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameVersionModel> response = new List<GameVersionModel>();
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
                        for (int i = 0; i < gameVersionList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE gwp_game_versions SET ";
                                if (!string.IsNullOrEmpty(gameVersionList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, gameVersionList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].Version))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `version` = @version" + i);
                                    else
                                        sql = string.Concat(sql, " `version` = @version" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@version" + i, gameVersionList[i].Version);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].Platform))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `platform` = @platform" + i);
                                    else
                                        sql = string.Concat(sql, " `platform` = @platform" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@platform" + i, gameVersionList[i].Platform);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].Description))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `description` = @description" + i);
                                    else
                                        sql = string.Concat(sql, " `description` = @description" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@description" + i, gameVersionList[i].Description);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].ReleaseDate))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `release_date` = @releaseDate" + i);
                                    else
                                        sql = string.Concat(sql, " `release_date` = @releaseDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@releaseDate" + i, gameVersionList[i].ReleaseDate);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].EndDate))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `end_date` = @endDate" + i);
                                    else
                                        sql = string.Concat(sql, " `end_date` = @endDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@endDate" + i, gameVersionList[i].EndDate);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, gameVersionList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, gameVersionList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(gameVersionList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, gameVersionList[i].UpdatedAt);
                                }
                                if (gameVersionWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(gameVersionWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, gameVersionWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersionWhereList[i].Version))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `version` = @versionWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `version` = @versionWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@versionWhere" + i, gameVersionWhereList[i].Version);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersionWhereList[i].Platform))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform` = @platformWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `platform` = @platformWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platformWhere" + i, gameVersionWhereList[i].Platform);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersionWhereList[i].Description))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `description` = @descriptionWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `description` = @descriptionWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descriptionWhere" + i, gameVersionWhereList[i].Description);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersionWhereList[i].ReleaseDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `release_date` = @releaseDateWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `release_date` = @releaseDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@releaseDateWhere" + i, gameVersionWhereList[i].ReleaseDate);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersionWhereList[i].EndDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `end_date` = @endDateWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `end_date` = @endDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@endDateWhere" + i, gameVersionWhereList[i].EndDate);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersionWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, gameVersionWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersionWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, gameVersionWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersionWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, gameVersionWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(gameVersionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<GameVersionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<GameVersionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameVersionModel>();
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

        public List<GameVersionModel> Delete(string connectionString, GameVersionModel gameVersion)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameVersionModel> response = new List<GameVersionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM gwp_game_versions  ";
                if (gameVersion != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(gameVersion.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", gameVersion.Id);
                    }
                    if (!string.IsNullOrEmpty(gameVersion.Version))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `version` = @version");
                        else
                            sql = string.Concat(sql, " `version` = @version");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@version", gameVersion.Version);
                    }
                    if (!string.IsNullOrEmpty(gameVersion.Platform))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `platform` = @platform");
                        else
                            sql = string.Concat(sql, " `platform` = @platform");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@platform", gameVersion.Platform);
                    }
                    if (!string.IsNullOrEmpty(gameVersion.Description))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `description` = @description");
                        else
                            sql = string.Concat(sql, " `description` = @description");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@description", gameVersion.Description);
                    }
                    if (!string.IsNullOrEmpty(gameVersion.ReleaseDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `release_date` = @releaseDate");
                        else
                            sql = string.Concat(sql, " `release_date` = @releaseDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@releaseDate", gameVersion.ReleaseDate);
                    }
                    if (!string.IsNullOrEmpty(gameVersion.EndDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `end_date` = @endDate");
                        else
                            sql = string.Concat(sql, " `end_date` = @endDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@endDate", gameVersion.EndDate);
                    }
                    if (!string.IsNullOrEmpty(gameVersion.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", gameVersion.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(gameVersion.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", gameVersion.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(gameVersion.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", gameVersion.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(gameVersion);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<GameVersionModel>();
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

        public List<GameVersionModel> Delete(string connectionString, List<GameVersionModel> gameVersion)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<GameVersionModel> response = new List<GameVersionModel>();
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
                        for (int i = 0; i < gameVersion.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM gwp_game_versions  ";
                                if (gameVersion[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(gameVersion[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, gameVersion[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersion[i].Version))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `version` = @version" + i);
                                        else
                                            sql = string.Concat(sql, " `version` = @version" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@version" + i, gameVersion[i].Version);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersion[i].Platform))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `platform` = @platform" + i);
                                        else
                                            sql = string.Concat(sql, " `platform` = @platform" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@platform" + i, gameVersion[i].Platform);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersion[i].Description))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `description` = @description" + i);
                                        else
                                            sql = string.Concat(sql, " `description` = @description" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@description" + i, gameVersion[i].Description);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersion[i].ReleaseDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `release_date` = @releaseDate" + i);
                                        else
                                            sql = string.Concat(sql, " `release_date` = @releaseDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@releaseDate" + i, gameVersion[i].ReleaseDate);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersion[i].EndDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `end_date` = @endDate" + i);
                                        else
                                            sql = string.Concat(sql, " `end_date` = @endDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@endDate" + i, gameVersion[i].EndDate);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersion[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, gameVersion[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersion[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, gameVersion[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(gameVersion[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, gameVersion[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(gameVersion[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<GameVersionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<GameVersionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<GameVersionModel>();
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
