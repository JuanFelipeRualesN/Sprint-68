using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Services.Generals;
using GSWebAPI.GSWebAPI.Domain.Models.General;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.Generals
{
    class ParentLockedGameDAO
    {
        private MySqlConnection _connection;
        public List<ParentLockedGameModel> Read(string connectionString, ParentLockedGameModel parentLockedGame, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ParentLockedGameModel> response = new List<ParentLockedGameModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `locked`, `modified_by`, `created_at`, `updated_at`  FROM gwp_parent_locked_games ";
                {
                    if (parentLockedGame != null)
                    {
                        if (!string.IsNullOrEmpty(parentLockedGame.Id))
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
                            cmd.Parameters.AddWithValue("@id", parentLockedGame.Id);
                        }
                        if (!string.IsNullOrEmpty(parentLockedGame.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", parentLockedGame.UserId);
                        }
                        if (!string.IsNullOrEmpty(parentLockedGame.Locked))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `locked` = @locked ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `locked` = @locked ");
                            }
                            cmd.Parameters.AddWithValue("@locked", parentLockedGame.Locked);
                        }
                        if (!string.IsNullOrEmpty(parentLockedGame.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", parentLockedGame.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(parentLockedGame.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", parentLockedGame.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(parentLockedGame.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", parentLockedGame.UpdatedAt);
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
                                    parentLockedGame.Id = reader.GetValue(0).ToString();
                                else
                                    parentLockedGame.Id = null;
                                if (!reader.IsDBNull(1))
                                    parentLockedGame.UserId = reader.GetValue(1).ToString();
                                else
                                    parentLockedGame.UserId = null;
                                if (!reader.IsDBNull(2))
                                    parentLockedGame.Locked = reader.GetValue(2).ToString();
                                else
                                    parentLockedGame.Locked = null;
                                if (!reader.IsDBNull(3))
                                    parentLockedGame.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    parentLockedGame.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    parentLockedGame.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    parentLockedGame.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    parentLockedGame.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    parentLockedGame.UpdatedAt = null;
                                response.Add(parentLockedGame);
                                parentLockedGame = new ParentLockedGameModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ParentLockedGameModel>();
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
                response = new List<ParentLockedGameModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ParentLockedGameModel> Read(string connectionString, List<ParentLockedGameModel> parentLockedGameWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ParentLockedGameModel> response = new List<ParentLockedGameModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `locked`, `modified_by`, `created_at`, `updated_at`  FROM gwp_parent_locked_games ";
                if (parentLockedGameWhere != null && parentLockedGameWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < parentLockedGameWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(parentLockedGameWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, parentLockedGameWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(parentLockedGameWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, parentLockedGameWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(parentLockedGameWhere[i].Locked))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `locked` = @locked" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@locked" + i, parentLockedGameWhere[i].Locked);
                        }
                        if (!string.IsNullOrEmpty(parentLockedGameWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, parentLockedGameWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(parentLockedGameWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, parentLockedGameWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(parentLockedGameWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, parentLockedGameWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != parentLockedGameWhere.Count - 1)
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
                            ParentLockedGameModel parentLockedGame = new ParentLockedGameModel(); if (!reader.IsDBNull(0))
                                parentLockedGame.Id = reader.GetValue(0).ToString();
                            else
                                parentLockedGame.Id = null;
                            if (!reader.IsDBNull(1))
                                parentLockedGame.UserId = reader.GetValue(1).ToString();
                            else
                                parentLockedGame.UserId = null;
                            if (!reader.IsDBNull(2))
                                parentLockedGame.Locked = reader.GetValue(2).ToString();
                            else
                                parentLockedGame.Locked = null;
                            if (!reader.IsDBNull(3))
                                parentLockedGame.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                parentLockedGame.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                parentLockedGame.CreatedAt = reader.GetValue(4).ToString();
                            else
                                parentLockedGame.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                parentLockedGame.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                parentLockedGame.UpdatedAt = null;
                            response.Add(parentLockedGame);
                            parentLockedGame = new ParentLockedGameModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<ParentLockedGameModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ParentLockedGameModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ParentLockedGameModel> Create(string connectionString, ParentLockedGameModel parentLockedGame)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ParentLockedGameModel> response = new List<ParentLockedGameModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO gwp_parent_locked_games (";
                string values = "(";
                if (!string.IsNullOrEmpty(parentLockedGame.Id))
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
                    cmd.Parameters.AddWithValue("@id", parentLockedGame.Id);
                }
                if (!string.IsNullOrEmpty(parentLockedGame.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", parentLockedGame.UserId);
                }
                if (!string.IsNullOrEmpty(parentLockedGame.Locked))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `locked` ");
                        values = string.Concat(values, ", @locked");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `locked` ");
                        values = string.Concat(values, " @locked");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@locked", parentLockedGame.Locked);
                }
                if (!string.IsNullOrEmpty(parentLockedGame.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", parentLockedGame.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(parentLockedGame.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", parentLockedGame.CreatedAt);
                }
                if (!string.IsNullOrEmpty(parentLockedGame.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", parentLockedGame.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                parentLockedGame.Id = cmd.LastInsertedId.ToString();
                response.Add(parentLockedGame);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ParentLockedGameModel>();
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
        public List<ParentLockedGameModel> Create(string connectionString, List<ParentLockedGameModel> parentLockedGameList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ParentLockedGameModel> response = new List<ParentLockedGameModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < parentLockedGameList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO gwp_parent_locked_games (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(parentLockedGameList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, parentLockedGameList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(parentLockedGameList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, parentLockedGameList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(parentLockedGameList[i].Locked))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `locked` ");
                                        values = string.Concat(values, ", @locked" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `locked` ");
                                        values = string.Concat(values, "@locked" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@locked" + i, parentLockedGameList[i].Locked);
                                }
                                if (!string.IsNullOrEmpty(parentLockedGameList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, parentLockedGameList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(parentLockedGameList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, parentLockedGameList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(parentLockedGameList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, parentLockedGameList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(parentLockedGameList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<ParentLockedGameModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ParentLockedGameModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ParentLockedGameModel>();
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

        public List<ParentLockedGameModel> Update(string connectionString, ParentLockedGameModel parentLockedGame, ParentLockedGameModel parentLockedGameWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ParentLockedGameModel> response = new List<ParentLockedGameModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE gwp_parent_locked_games SET ";
                if (!string.IsNullOrEmpty(parentLockedGame.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", parentLockedGame.Id);
                }
                if (!string.IsNullOrEmpty(parentLockedGame.UserId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = string.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", parentLockedGame.UserId);
                }
                if (!string.IsNullOrEmpty(parentLockedGame.Locked))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `locked` = @locked");
                    else
                        sql = string.Concat(sql, " `locked` = @locked");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@locked", parentLockedGame.Locked);
                }
                if (!string.IsNullOrEmpty(parentLockedGame.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", parentLockedGame.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(parentLockedGame.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", parentLockedGame.CreatedAt);
                }
                if (!string.IsNullOrEmpty(parentLockedGame.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", parentLockedGame.UpdatedAt);
                }
                if (parentLockedGameWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(parentLockedGameWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", parentLockedGameWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(parentLockedGameWhere.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = string.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", parentLockedGameWhere.UserId);
                    }
                    if (!string.IsNullOrEmpty(parentLockedGameWhere.Locked))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `locked` = @lockedWhere");
                        else
                            sql = string.Concat(sql, " `locked` = @lockedWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lockedWhere", parentLockedGameWhere.Locked);
                    }
                    if (!string.IsNullOrEmpty(parentLockedGameWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", parentLockedGameWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(parentLockedGameWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", parentLockedGameWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(parentLockedGameWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", parentLockedGameWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(parentLockedGame);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ParentLockedGameModel>();
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

        public List<ParentLockedGameModel> Update(string connectionString, List<ParentLockedGameModel> parentLockedGameList, List<ParentLockedGameModel> parentLockedGameWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ParentLockedGameModel> response = new List<ParentLockedGameModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < parentLockedGameList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE gwp_parent_locked_games SET ";
                                if (!string.IsNullOrEmpty(parentLockedGameList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, parentLockedGameList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(parentLockedGameList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = string.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, parentLockedGameList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(parentLockedGameList[i].Locked))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `locked` = @locked" + i);
                                    else
                                        sql = string.Concat(sql, " `locked` = @locked" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@locked" + i, parentLockedGameList[i].Locked);
                                }
                                if (!string.IsNullOrEmpty(parentLockedGameList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, parentLockedGameList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(parentLockedGameList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, parentLockedGameList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(parentLockedGameList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, parentLockedGameList[i].UpdatedAt);
                                }
                                if (parentLockedGameWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(parentLockedGameWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, parentLockedGameWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(parentLockedGameWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, parentLockedGameWhereList[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(parentLockedGameWhereList[i].Locked))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `locked` = @lockedWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `locked` = @lockedWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lockedWhere" + i, parentLockedGameWhereList[i].Locked);
                                    }
                                    if (!string.IsNullOrEmpty(parentLockedGameWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, parentLockedGameWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(parentLockedGameWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, parentLockedGameWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(parentLockedGameWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, parentLockedGameWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(parentLockedGameList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ParentLockedGameModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ParentLockedGameModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ParentLockedGameModel>();
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

        public List<ParentLockedGameModel> Delete(string connectionString, ParentLockedGameModel parentLockedGame)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ParentLockedGameModel> response = new List<ParentLockedGameModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM gwp_parent_locked_games  ";
                if (parentLockedGame != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(parentLockedGame.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", parentLockedGame.Id);
                    }
                    if (!string.IsNullOrEmpty(parentLockedGame.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = string.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", parentLockedGame.UserId);
                    }
                    if (!string.IsNullOrEmpty(parentLockedGame.Locked))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `locked` = @locked");
                        else
                            sql = string.Concat(sql, " `locked` = @locked");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@locked", parentLockedGame.Locked);
                    }
                    if (!string.IsNullOrEmpty(parentLockedGame.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", parentLockedGame.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(parentLockedGame.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", parentLockedGame.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(parentLockedGame.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", parentLockedGame.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(parentLockedGame);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ParentLockedGameModel>();
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

        public List<ParentLockedGameModel> Delete(string connectionString, List<ParentLockedGameModel> parentLockedGame)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ParentLockedGameModel> response = new List<ParentLockedGameModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < parentLockedGame.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM gwp_parent_locked_games  ";
                                if (parentLockedGame[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(parentLockedGame[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, parentLockedGame[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(parentLockedGame[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, parentLockedGame[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(parentLockedGame[i].Locked))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `locked` = @locked" + i);
                                        else
                                            sql = string.Concat(sql, " `locked` = @locked" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@locked" + i, parentLockedGame[i].Locked);
                                    }
                                    if (!string.IsNullOrEmpty(parentLockedGame[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, parentLockedGame[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(parentLockedGame[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, parentLockedGame[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(parentLockedGame[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, parentLockedGame[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(parentLockedGame[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ParentLockedGameModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ParentLockedGameModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ParentLockedGameModel>();
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
