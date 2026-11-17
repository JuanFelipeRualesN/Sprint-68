using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Services.Generals;
using GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.GameQuizCompetitions
{
    class HighScoreDAO
    {
        private MySqlConnection _connection;
        public List<HighScoreModel> Read(string connectionString, HighScoreModel highScore, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<HighScoreModel> response = new List<HighScoreModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `score`, `subject_code`, `level_code`, `modified_by`, `created_at`, `updated_at`  FROM gqz_high_scores ";
                {
                    if (highScore != null)
                    {
                        if (!string.IsNullOrEmpty(highScore.Id))
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
                            cmd.Parameters.AddWithValue("@id", highScore.Id);
                        }
                        if (!string.IsNullOrEmpty(highScore.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", highScore.UserId);
                        }
                        if (!string.IsNullOrEmpty(highScore.Score))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `score` = @score ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `score` = @score ");
                            }
                            cmd.Parameters.AddWithValue("@score", highScore.Score);
                        }
                        if (!string.IsNullOrEmpty(highScore.SubjectCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject_code` = @subjectCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject_code` = @subjectCode ");
                            }
                            cmd.Parameters.AddWithValue("@subjectCode", highScore.SubjectCode);
                        }
                        if (!string.IsNullOrEmpty(highScore.LevelCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `level_code` = @levelCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `level_code` = @levelCode ");
                            }
                            cmd.Parameters.AddWithValue("@levelCode", highScore.LevelCode);
                        }
                        if (!string.IsNullOrEmpty(highScore.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", highScore.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(highScore.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", highScore.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(highScore.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", highScore.UpdatedAt);
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
                                    highScore.Id = Reader.GetValue(0).ToString();
                                else
                                    highScore.Id = null;
                                if (!Reader.IsDBNull(1))
                                    highScore.UserId = Reader.GetValue(1).ToString();
                                else
                                    highScore.UserId = null;
                                if (!Reader.IsDBNull(2))
                                    highScore.Score = Reader.GetValue(2).ToString();
                                else
                                    highScore.Score = null;
                                if (!Reader.IsDBNull(3))
                                    highScore.SubjectCode = Reader.GetValue(3).ToString();
                                else
                                    highScore.SubjectCode = null;
                                if (!Reader.IsDBNull(4))
                                    highScore.LevelCode = Reader.GetValue(4).ToString();
                                else
                                    highScore.LevelCode = null;
                                if (!Reader.IsDBNull(5))
                                    highScore.ModifiedBy = Reader.GetValue(5).ToString();
                                else
                                    highScore.ModifiedBy = null;
                                if (!Reader.IsDBNull(6))
                                    highScore.CreatedAt = Reader.GetValue(6).ToString();
                                else
                                    highScore.CreatedAt = null;
                                if (!Reader.IsDBNull(7))
                                    highScore.UpdatedAt = Reader.GetValue(7).ToString();
                                else
                                    highScore.UpdatedAt = null;
                                response.Add(highScore);
                                highScore = new HighScoreModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<HighScoreModel>();
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
                response = new List<HighScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<HighScoreModel> Read(string connectionString, List<HighScoreModel> highScoreWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<HighScoreModel> response = new List<HighScoreModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `score`, `subject_code`, `level_code`, `modified_by`, `created_at`, `updated_at`  FROM gqz_high_scores ";
                if (highScoreWhere != null && highScoreWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < highScoreWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(highScoreWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, highScoreWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(highScoreWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, highScoreWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(highScoreWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, highScoreWhere[i].Score);
                        }
                        if (!string.IsNullOrEmpty(highScoreWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, highScoreWhere[i].SubjectCode);
                        }
                        if (!string.IsNullOrEmpty(highScoreWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, highScoreWhere[i].LevelCode);
                        }
                        if (!string.IsNullOrEmpty(highScoreWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, highScoreWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(highScoreWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, highScoreWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(highScoreWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, highScoreWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != highScoreWhere.Count - 1)
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
                            HighScoreModel highScore = new HighScoreModel(); if (!Reader.IsDBNull(0))
                                highScore.Id = Reader.GetValue(0).ToString();
                            else
                                highScore.Id = null;
                            if (!Reader.IsDBNull(1))
                                highScore.UserId = Reader.GetValue(1).ToString();
                            else
                                highScore.UserId = null;
                            if (!Reader.IsDBNull(2))
                                highScore.Score = Reader.GetValue(2).ToString();
                            else
                                highScore.Score = null;
                            if (!Reader.IsDBNull(3))
                                highScore.SubjectCode = Reader.GetValue(3).ToString();
                            else
                                highScore.SubjectCode = null;
                            if (!Reader.IsDBNull(4))
                                highScore.LevelCode = Reader.GetValue(4).ToString();
                            else
                                highScore.LevelCode = null;
                            if (!Reader.IsDBNull(5))
                                highScore.ModifiedBy = Reader.GetValue(5).ToString();
                            else
                                highScore.ModifiedBy = null;
                            if (!Reader.IsDBNull(6))
                                highScore.CreatedAt = Reader.GetValue(6).ToString();
                            else
                                highScore.CreatedAt = null;
                            if (!Reader.IsDBNull(7))
                                highScore.UpdatedAt = Reader.GetValue(7).ToString();
                            else
                                highScore.UpdatedAt = null;
                            response.Add(highScore);
                            highScore = new HighScoreModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<HighScoreModel>();
                    Reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<HighScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<HighScoreModel> Create(string connectionString, HighScoreModel highScore)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<HighScoreModel> response = new List<HighScoreModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO gqz_high_scores (";
                string values = "(";
                if (!string.IsNullOrEmpty(highScore.Id))
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
                    cmd.Parameters.AddWithValue("@id", highScore.Id);
                }
                if (!string.IsNullOrEmpty(highScore.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", highScore.UserId);
                }
                if (!string.IsNullOrEmpty(highScore.Score))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `score` ");
                        values = string.Concat(values, ", @score");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `score` ");
                        values = string.Concat(values, " @score");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@score", highScore.Score);
                }
                if (!string.IsNullOrEmpty(highScore.SubjectCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `subject_code` ");
                        values = string.Concat(values, ", @subjectCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `subject_code` ");
                        values = string.Concat(values, " @subjectCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectCode", highScore.SubjectCode);
                }
                if (!string.IsNullOrEmpty(highScore.LevelCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `level_code` ");
                        values = string.Concat(values, ", @levelCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `level_code` ");
                        values = string.Concat(values, " @levelCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@levelCode", highScore.LevelCode);
                }
                if (!string.IsNullOrEmpty(highScore.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", highScore.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(highScore.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", highScore.CreatedAt);
                }
                if (!string.IsNullOrEmpty(highScore.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", highScore.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                highScore.Id = cmd.LastInsertedId.ToString();
                response.Add(highScore);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<HighScoreModel>();
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
        public List<HighScoreModel> Create(string connectionString, List<HighScoreModel> highScoreList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<HighScoreModel> response = new List<HighScoreModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < highScoreList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO gqz_high_scores (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(highScoreList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, highScoreList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(highScoreList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, highScoreList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(highScoreList[i].Score))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `score` ");
                                        values = string.Concat(values, ", @score" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `score` ");
                                        values = string.Concat(values, "@score" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@score" + i, highScoreList[i].Score);
                                }
                                if (!string.IsNullOrEmpty(highScoreList[i].SubjectCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `subject_code` ");
                                        values = string.Concat(values, ", @subjectCode" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `subject_code` ");
                                        values = string.Concat(values, "@subjectCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, highScoreList[i].SubjectCode);
                                }
                                if (!string.IsNullOrEmpty(highScoreList[i].LevelCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `level_code` ");
                                        values = string.Concat(values, ", @levelCode" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `level_code` ");
                                        values = string.Concat(values, "@levelCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@levelCode" + i, highScoreList[i].LevelCode);
                                }
                                if (!string.IsNullOrEmpty(highScoreList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, highScoreList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(highScoreList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, highScoreList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(highScoreList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, highScoreList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(highScoreList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<HighScoreModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<HighScoreModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<HighScoreModel>();
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

        public List<HighScoreModel> Update(string connectionString, HighScoreModel highScore, HighScoreModel highScoreWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<HighScoreModel> response = new List<HighScoreModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE gqz_high_scores SET ";
                if (!string.IsNullOrEmpty(highScore.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", highScore.Id);
                }
                if (!string.IsNullOrEmpty(highScore.UserId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = string.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", highScore.UserId);
                }
                if (!string.IsNullOrEmpty(highScore.Score))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `score` = @score");
                    else
                        sql = string.Concat(sql, " `score` = @score");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@score", highScore.Score);
                }
                if (!string.IsNullOrEmpty(highScore.SubjectCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `subject_code` = @subjectCode");
                    else
                        sql = string.Concat(sql, " `subject_code` = @subjectCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectCode", highScore.SubjectCode);
                }
                if (!string.IsNullOrEmpty(highScore.LevelCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `level_code` = @levelCode");
                    else
                        sql = string.Concat(sql, " `level_code` = @levelCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@levelCode", highScore.LevelCode);
                }
                if (!string.IsNullOrEmpty(highScore.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", highScore.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(highScore.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", highScore.CreatedAt);
                }
                if (!string.IsNullOrEmpty(highScore.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", highScore.UpdatedAt);
                }
                if (highScoreWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(highScoreWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", highScoreWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(highScoreWhere.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = string.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", highScoreWhere.UserId);
                    }
                    if (!string.IsNullOrEmpty(highScoreWhere.Score))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `score` = @scoreWhere");
                        else
                            sql = string.Concat(sql, " `score` = @scoreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@scoreWhere", highScoreWhere.Score);
                    }
                    if (!string.IsNullOrEmpty(highScoreWhere.SubjectCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subject_code` = @subjectCodeWhere");
                        else
                            sql = string.Concat(sql, " `subject_code` = @subjectCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCodeWhere", highScoreWhere.SubjectCode);
                    }
                    if (!string.IsNullOrEmpty(highScoreWhere.LevelCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `level_code` = @levelCodeWhere");
                        else
                            sql = string.Concat(sql, " `level_code` = @levelCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelCodeWhere", highScoreWhere.LevelCode);
                    }
                    if (!string.IsNullOrEmpty(highScoreWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", highScoreWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(highScoreWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", highScoreWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(highScoreWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", highScoreWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(highScore);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<HighScoreModel>();
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

        public List<HighScoreModel> Update(string connectionString, List<HighScoreModel> highScoreList, List<HighScoreModel> highScoreWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<HighScoreModel> response = new List<HighScoreModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < highScoreList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE gqz_high_scores SET ";
                                if (!string.IsNullOrEmpty(highScoreList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, highScoreList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(highScoreList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = string.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, highScoreList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(highScoreList[i].Score))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `score` = @score" + i);
                                    else
                                        sql = string.Concat(sql, " `score` = @score" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@score" + i, highScoreList[i].Score);
                                }
                                if (!string.IsNullOrEmpty(highScoreList[i].SubjectCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `subject_code` = @subjectCode" + i);
                                    else
                                        sql = string.Concat(sql, " `subject_code` = @subjectCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, highScoreList[i].SubjectCode);
                                }
                                if (!string.IsNullOrEmpty(highScoreList[i].LevelCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `level_code` = @levelCode" + i);
                                    else
                                        sql = string.Concat(sql, " `level_code` = @levelCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@levelCode" + i, highScoreList[i].LevelCode);
                                }
                                if (!string.IsNullOrEmpty(highScoreList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, highScoreList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(highScoreList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, highScoreList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(highScoreList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, highScoreList[i].UpdatedAt);
                                }
                                if (highScoreWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(highScoreWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, highScoreWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(highScoreWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, highScoreWhereList[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(highScoreWhereList[i].Score))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `score` = @scoreWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `score` = @scoreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@scoreWhere" + i, highScoreWhereList[i].Score);
                                    }
                                    if (!string.IsNullOrEmpty(highScoreWhereList[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subject_code` = @subjectCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `subject_code` = @subjectCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCodeWhere" + i, highScoreWhereList[i].SubjectCode);
                                    }
                                    if (!string.IsNullOrEmpty(highScoreWhereList[i].LevelCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `level_code` = @levelCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `level_code` = @levelCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelCodeWhere" + i, highScoreWhereList[i].LevelCode);
                                    }
                                    if (!string.IsNullOrEmpty(highScoreWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, highScoreWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(highScoreWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, highScoreWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(highScoreWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, highScoreWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(highScoreList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<HighScoreModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<HighScoreModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<HighScoreModel>();
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

        public List<HighScoreModel> Delete(string connectionString, HighScoreModel highScore)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<HighScoreModel> response = new List<HighScoreModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM gqz_high_scores  ";
                if (highScore != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(highScore.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", highScore.Id);
                    }
                    if (!string.IsNullOrEmpty(highScore.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = string.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", highScore.UserId);
                    }
                    if (!string.IsNullOrEmpty(highScore.Score))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `score` = @score");
                        else
                            sql = string.Concat(sql, " `score` = @score");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@score", highScore.Score);
                    }
                    if (!string.IsNullOrEmpty(highScore.SubjectCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subject_code` = @subjectCode");
                        else
                            sql = string.Concat(sql, " `subject_code` = @subjectCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCode", highScore.SubjectCode);
                    }
                    if (!string.IsNullOrEmpty(highScore.LevelCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `level_code` = @levelCode");
                        else
                            sql = string.Concat(sql, " `level_code` = @levelCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelCode", highScore.LevelCode);
                    }
                    if (!string.IsNullOrEmpty(highScore.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", highScore.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(highScore.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", highScore.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(highScore.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", highScore.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(highScore);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<HighScoreModel>();
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

        public List<HighScoreModel> Delete(string connectionString, List<HighScoreModel> highScore)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<HighScoreModel> response = new List<HighScoreModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < highScore.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM gqz_high_scores  ";
                                if (highScore[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(highScore[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, highScore[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(highScore[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, highScore[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(highScore[i].Score))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `score` = @score" + i);
                                        else
                                            sql = string.Concat(sql, " `score` = @score" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@score" + i, highScore[i].Score);
                                    }
                                    if (!string.IsNullOrEmpty(highScore[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subject_code` = @subjectCode" + i);
                                        else
                                            sql = string.Concat(sql, " `subject_code` = @subjectCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCode" + i, highScore[i].SubjectCode);
                                    }
                                    if (!string.IsNullOrEmpty(highScore[i].LevelCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `level_code` = @levelCode" + i);
                                        else
                                            sql = string.Concat(sql, " `level_code` = @levelCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelCode" + i, highScore[i].LevelCode);
                                    }
                                    if (!string.IsNullOrEmpty(highScore[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, highScore[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(highScore[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, highScore[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(highScore[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, highScore[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(highScore[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<HighScoreModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<HighScoreModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<HighScoreModel>();
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
