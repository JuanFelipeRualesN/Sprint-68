using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Services.Generals;
using GSWebAPI.GSWebAPI.Domain.Models.General;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.Generals
{
    class UserRewardMaxPricePerQuizDAO
    {
        private MySqlConnection _connection;
        public List<UserRewardMaxPricePerQuizModel> Read(string connectionString, UserRewardMaxPricePerQuizModel userRewardMaxPricePerQuiz, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardMaxPricePerQuizModel> response = new List<UserRewardMaxPricePerQuizModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `quiz_id`, `modified_by`, `created_at`, `updated_at`  FROM gwp_user_reward_max_price_per_quizs ";
                {
                    if (userRewardMaxPricePerQuiz != null)
                    {
                        if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.Id))
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
                            cmd.Parameters.AddWithValue("@id", userRewardMaxPricePerQuiz.Id);
                        }
                        if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userRewardMaxPricePerQuiz.UserId);
                        }
                        if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quiz_id` = @quizId ");
                            }
                            cmd.Parameters.AddWithValue("@quizId", userRewardMaxPricePerQuiz.QuizId);
                        }
                        if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userRewardMaxPricePerQuiz.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userRewardMaxPricePerQuiz.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userRewardMaxPricePerQuiz.UpdatedAt);
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
                                    userRewardMaxPricePerQuiz.Id = reader.GetValue(0).ToString();
                                else
                                    userRewardMaxPricePerQuiz.Id = null;
                                if (!reader.IsDBNull(1))
                                    userRewardMaxPricePerQuiz.UserId = reader.GetValue(1).ToString();
                                else
                                    userRewardMaxPricePerQuiz.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userRewardMaxPricePerQuiz.QuizId = reader.GetValue(2).ToString();
                                else
                                    userRewardMaxPricePerQuiz.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    userRewardMaxPricePerQuiz.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    userRewardMaxPricePerQuiz.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    userRewardMaxPricePerQuiz.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    userRewardMaxPricePerQuiz.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    userRewardMaxPricePerQuiz.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    userRewardMaxPricePerQuiz.UpdatedAt = null;
                                response.Add(userRewardMaxPricePerQuiz);
                                userRewardMaxPricePerQuiz = new UserRewardMaxPricePerQuizModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserRewardMaxPricePerQuizModel>();
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
                response = new List<UserRewardMaxPricePerQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserRewardMaxPricePerQuizModel> Read(string connectionString, List<UserRewardMaxPricePerQuizModel> userRewardMaxPricePerQuizWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardMaxPricePerQuizModel> response = new List<UserRewardMaxPricePerQuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `quiz_id`, `modified_by`, `created_at`, `updated_at`  FROM gwp_user_reward_max_price_per_quizs ";
                if (userRewardMaxPricePerQuizWhere != null && userRewardMaxPricePerQuizWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userRewardMaxPricePerQuizWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userRewardMaxPricePerQuizWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userRewardMaxPricePerQuizWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, userRewardMaxPricePerQuizWhere[i].QuizId);
                        }
                        if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userRewardMaxPricePerQuizWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userRewardMaxPricePerQuizWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userRewardMaxPricePerQuizWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userRewardMaxPricePerQuizWhere.Count - 1)
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
                            UserRewardMaxPricePerQuizModel userRewardMaxPricePerQuiz = new UserRewardMaxPricePerQuizModel(); if (!reader.IsDBNull(0))
                                userRewardMaxPricePerQuiz.Id = reader.GetValue(0).ToString();
                            else
                                userRewardMaxPricePerQuiz.Id = null;
                            if (!reader.IsDBNull(1))
                                userRewardMaxPricePerQuiz.UserId = reader.GetValue(1).ToString();
                            else
                                userRewardMaxPricePerQuiz.UserId = null;
                            if (!reader.IsDBNull(2))
                                userRewardMaxPricePerQuiz.QuizId = reader.GetValue(2).ToString();
                            else
                                userRewardMaxPricePerQuiz.QuizId = null;
                            if (!reader.IsDBNull(3))
                                userRewardMaxPricePerQuiz.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                userRewardMaxPricePerQuiz.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                userRewardMaxPricePerQuiz.CreatedAt = reader.GetValue(4).ToString();
                            else
                                userRewardMaxPricePerQuiz.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                userRewardMaxPricePerQuiz.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                userRewardMaxPricePerQuiz.UpdatedAt = null;
                            response.Add(userRewardMaxPricePerQuiz);
                            userRewardMaxPricePerQuiz = new UserRewardMaxPricePerQuizModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserRewardMaxPricePerQuizModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserRewardMaxPricePerQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserRewardMaxPricePerQuizModel> Create(string connectionString, UserRewardMaxPricePerQuizModel userRewardMaxPricePerQuiz)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardMaxPricePerQuizModel> response = new List<UserRewardMaxPricePerQuizModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO gwp_user_reward_max_price_per_quizs (";
                string values = "(";
                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.Id))
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
                    cmd.Parameters.AddWithValue("@id", userRewardMaxPricePerQuiz.Id);
                }
                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", userRewardMaxPricePerQuiz.UserId);
                }
                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.QuizId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `quiz_id` ");
                        values = string.Concat(values, ", @quizId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `quiz_id` ");
                        values = string.Concat(values, " @quizId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", userRewardMaxPricePerQuiz.QuizId);
                }
                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userRewardMaxPricePerQuiz.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userRewardMaxPricePerQuiz.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userRewardMaxPricePerQuiz.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userRewardMaxPricePerQuiz.Id = cmd.LastInsertedId.ToString();
                response.Add(userRewardMaxPricePerQuiz);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserRewardMaxPricePerQuizModel>();
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
        public List<UserRewardMaxPricePerQuizModel> Create(string connectionString, List<UserRewardMaxPricePerQuizModel> userRewardMaxPricePerQuizList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardMaxPricePerQuizModel> response = new List<UserRewardMaxPricePerQuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userRewardMaxPricePerQuizList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO gwp_user_reward_max_price_per_quizs (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userRewardMaxPricePerQuizList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, userRewardMaxPricePerQuizList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizList[i].QuizId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `quiz_id` ");
                                        values = string.Concat(values, ", @quizId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `quiz_id` ");
                                        values = string.Concat(values, "@quizId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, userRewardMaxPricePerQuizList[i].QuizId);
                                }
                                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userRewardMaxPricePerQuizList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userRewardMaxPricePerQuizList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userRewardMaxPricePerQuizList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userRewardMaxPricePerQuizList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserRewardMaxPricePerQuizModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserRewardMaxPricePerQuizModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserRewardMaxPricePerQuizModel>();
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

        public List<UserRewardMaxPricePerQuizModel> Update(string connectionString, UserRewardMaxPricePerQuizModel userRewardMaxPricePerQuiz, UserRewardMaxPricePerQuizModel userRewardMaxPricePerQuizWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardMaxPricePerQuizModel> response = new List<UserRewardMaxPricePerQuizModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE gwp_user_reward_max_price_per_quizs SET ";
                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userRewardMaxPricePerQuiz.Id);
                }
                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.UserId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = string.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userRewardMaxPricePerQuiz.UserId);
                }
                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.QuizId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `quiz_id` = @quizId");
                    else
                        sql = string.Concat(sql, " `quiz_id` = @quizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", userRewardMaxPricePerQuiz.QuizId);
                }
                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userRewardMaxPricePerQuiz.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userRewardMaxPricePerQuiz.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userRewardMaxPricePerQuiz.UpdatedAt);
                }
                if (userRewardMaxPricePerQuizWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userRewardMaxPricePerQuizWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhere.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = string.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userRewardMaxPricePerQuizWhere.UserId);
                    }
                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhere.QuizId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `quiz_id` = @quizIdWhere");
                        else
                            sql = string.Concat(sql, " `quiz_id` = @quizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizIdWhere", userRewardMaxPricePerQuizWhere.QuizId);
                    }
                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userRewardMaxPricePerQuizWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userRewardMaxPricePerQuizWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userRewardMaxPricePerQuizWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userRewardMaxPricePerQuiz);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserRewardMaxPricePerQuizModel>();
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

        public List<UserRewardMaxPricePerQuizModel> Update(string connectionString, List<UserRewardMaxPricePerQuizModel> userRewardMaxPricePerQuizList, List<UserRewardMaxPricePerQuizModel> userRewardMaxPricePerQuizWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardMaxPricePerQuizModel> response = new List<UserRewardMaxPricePerQuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userRewardMaxPricePerQuizList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE gwp_user_reward_max_price_per_quizs SET ";
                                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userRewardMaxPricePerQuizList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = string.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userRewardMaxPricePerQuizList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizList[i].QuizId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `quiz_id` = @quizId" + i);
                                    else
                                        sql = string.Concat(sql, " `quiz_id` = @quizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, userRewardMaxPricePerQuizList[i].QuizId);
                                }
                                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userRewardMaxPricePerQuizList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userRewardMaxPricePerQuizList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userRewardMaxPricePerQuizList[i].UpdatedAt);
                                }
                                if (userRewardMaxPricePerQuizWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userRewardMaxPricePerQuizWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userRewardMaxPricePerQuizWhereList[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhereList[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `quiz_id` = @quizIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `quiz_id` = @quizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizIdWhere" + i, userRewardMaxPricePerQuizWhereList[i].QuizId);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userRewardMaxPricePerQuizWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userRewardMaxPricePerQuizWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuizWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userRewardMaxPricePerQuizWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userRewardMaxPricePerQuizList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserRewardMaxPricePerQuizModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserRewardMaxPricePerQuizModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserRewardMaxPricePerQuizModel>();
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

        public List<UserRewardMaxPricePerQuizModel> Delete(string connectionString, UserRewardMaxPricePerQuizModel userRewardMaxPricePerQuiz)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardMaxPricePerQuizModel> response = new List<UserRewardMaxPricePerQuizModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM gwp_user_reward_max_price_per_quizs  ";
                if (userRewardMaxPricePerQuiz != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userRewardMaxPricePerQuiz.Id);
                    }
                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = string.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userRewardMaxPricePerQuiz.UserId);
                    }
                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.QuizId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `quiz_id` = @quizId");
                        else
                            sql = string.Concat(sql, " `quiz_id` = @quizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizId", userRewardMaxPricePerQuiz.QuizId);
                    }
                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userRewardMaxPricePerQuiz.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userRewardMaxPricePerQuiz.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userRewardMaxPricePerQuiz.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userRewardMaxPricePerQuiz);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserRewardMaxPricePerQuizModel>();
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

        public List<UserRewardMaxPricePerQuizModel> Delete(string connectionString, List<UserRewardMaxPricePerQuizModel> userRewardMaxPricePerQuiz)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserRewardMaxPricePerQuizModel> response = new List<UserRewardMaxPricePerQuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userRewardMaxPricePerQuiz.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM gwp_user_reward_max_price_per_quizs  ";
                                if (userRewardMaxPricePerQuiz[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userRewardMaxPricePerQuiz[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userRewardMaxPricePerQuiz[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `quiz_id` = @quizId" + i);
                                        else
                                            sql = string.Concat(sql, " `quiz_id` = @quizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizId" + i, userRewardMaxPricePerQuiz[i].QuizId);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userRewardMaxPricePerQuiz[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userRewardMaxPricePerQuiz[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userRewardMaxPricePerQuiz[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userRewardMaxPricePerQuiz[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userRewardMaxPricePerQuiz[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserRewardMaxPricePerQuizModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserRewardMaxPricePerQuizModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserRewardMaxPricePerQuizModel>();
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
