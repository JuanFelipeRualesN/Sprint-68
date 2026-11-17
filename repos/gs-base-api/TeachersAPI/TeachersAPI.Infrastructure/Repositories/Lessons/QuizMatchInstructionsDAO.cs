using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Lessons;

namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons
{
    class QuizMatchInstructionsDAO
    {
        private MySqlConnection _connection;

        public Dictionary<string, QuizMatchInstructionsModel> ReadD(string connectionString,
            QuizMatchInstructionsModel quizMatchInstructions, bool firstOne, bool orderByLastDate,
            QuizMatchInstructionsModel quizMatchInstructionsWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizMatchInstructionsModel> response =
                new Dictionary<string, QuizMatchInstructionsModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql =
                    "SELECT `id`, `quiz_id`, `match_group_number`, `instruction`, `modified_by`, `created_at`, `updated_at`  FROM quiz_match_instructions ";
                {
                    if (quizMatchInstructions != null)
                    {
                        if (!String.IsNullOrEmpty(quizMatchInstructions.Id))
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

                            cmd.Parameters.AddWithValue("@id", quizMatchInstructions.Id);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_id` = @quizId ");
                            }

                            cmd.Parameters.AddWithValue("@quizId", quizMatchInstructions.QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.MatchGroupNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `match_group_number` = @matchGroupNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `match_group_number` = @matchGroupNumber ");
                            }

                            cmd.Parameters.AddWithValue("@matchGroupNumber", quizMatchInstructions.MatchGroupNumber);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.Instruction))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `instruction` = @instruction ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `instruction` = @instruction ");
                            }

                            cmd.Parameters.AddWithValue("@instruction", quizMatchInstructions.Instruction);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.ModifiedBy))
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

                            cmd.Parameters.AddWithValue("@modifiedBy", quizMatchInstructions.ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.CreatedAt))
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

                            cmd.Parameters.AddWithValue("@createdAt", quizMatchInstructions.CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt     ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt     ");
                            }

                            cmd.Parameters.AddWithValue("@updated_at", quizMatchInstructions.UpdatedAt);
                        }
                    }

                    if (quizMatchInstructionsWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizMatchInstructionsWhereLike.Id + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike",
                                "%" + quizMatchInstructionsWhereLike.QuizId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.MatchGroupNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `match_group_number` LIKE @matchGroupNumberWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql,
                                    " where `match_group_number` LIKE @matchGroupNumberWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@matchGroupNumberWhereLike",
                                "%" + quizMatchInstructionsWhereLike.MatchGroupNumber + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.Instruction))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `instruction` LIKE @instructionWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `instruction` LIKE @instructionWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@instructionWhereLike",
                                "%" + quizMatchInstructionsWhereLike.Instruction + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike",
                                "%" + quizMatchInstructionsWhereLike.ModifiedBy + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike",
                                "%" + quizMatchInstructionsWhereLike.CreatedAt + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAt    WhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAt    WhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAt    WhereLike",
                                "%" + quizMatchInstructionsWhereLike.UpdatedAt + "%");
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
                                quizMatchInstructions = new QuizMatchInstructionsModel();
                                if (!reader.IsDBNull(0))
                                    quizMatchInstructions.Id = reader.GetValue(0).ToString();
                                else
                                    quizMatchInstructions.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizMatchInstructions.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizMatchInstructions.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizMatchInstructions.MatchGroupNumber = reader.GetValue(2).ToString();
                                else
                                    quizMatchInstructions.MatchGroupNumber = null;
                                if (!reader.IsDBNull(3))
                                    quizMatchInstructions.Instruction = reader.GetValue(3).ToString();
                                else
                                    quizMatchInstructions.Instruction = null;
                                if (!reader.IsDBNull(4))
                                    quizMatchInstructions.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    quizMatchInstructions.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    quizMatchInstructions.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    quizMatchInstructions.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    quizMatchInstructions.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    quizMatchInstructions.UpdatedAt = null;
                                response.Add(quizMatchInstructions.Id, quizMatchInstructions);
                                quizMatchInstructions = new QuizMatchInstructionsModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizMatchInstructionsModel>();
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
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizMatchInstructionsModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public Dictionary<string, QuizMatchInstructionsModel> ReadD(string connectionString,
            QuizMatchInstructionsModel quizMatchInstructions, bool orderByLastDate, int fromRow, int limit,
            QuizMatchInstructionsModel quizMatchInstructionsWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizMatchInstructionsModel> response =
                new Dictionary<string, QuizMatchInstructionsModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql =
                    "SELECT `id`, `quiz_id`, `match_group_number`, `instruction`, `modified_by`, `created_at`, `updated_at`  FROM quiz_match_instructions ";
                {
                    if (quizMatchInstructions != null)
                    {
                        if (!String.IsNullOrEmpty(quizMatchInstructions.Id))
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

                            cmd.Parameters.AddWithValue("@id", quizMatchInstructions.Id);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_id` = @quizId ");
                            }

                            cmd.Parameters.AddWithValue("@quizId", quizMatchInstructions.QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.MatchGroupNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `match_group_number` = @matchGroupNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `match_group_number` = @matchGroupNumber ");
                            }

                            cmd.Parameters.AddWithValue("@matchGroupNumber", quizMatchInstructions.MatchGroupNumber);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.Instruction))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `instruction` = @instruction ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `instruction` = @instruction ");
                            }

                            cmd.Parameters.AddWithValue("@instruction", quizMatchInstructions.Instruction);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.ModifiedBy))
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

                            cmd.Parameters.AddWithValue("@modifiedBy", quizMatchInstructions.ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.CreatedAt))
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

                            cmd.Parameters.AddWithValue("@createdAt", quizMatchInstructions.CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt     ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt     ");
                            }

                            cmd.Parameters.AddWithValue("@updated_at", quizMatchInstructions.UpdatedAt);
                        }
                    }

                    if (quizMatchInstructionsWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizMatchInstructionsWhereLike.Id + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike",
                                "%" + quizMatchInstructionsWhereLike.QuizId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.MatchGroupNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `match_group_number` LIKE @matchGroupNumberWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql,
                                    " where `match_group_number` LIKE @matchGroupNumberWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@matchGroupNumberWhereLike",
                                "%" + quizMatchInstructionsWhereLike.MatchGroupNumber + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.Instruction))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `instruction` LIKE @instructionWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `instruction` LIKE @instructionWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@instructionWhereLike",
                                "%" + quizMatchInstructionsWhereLike.Instruction + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike",
                                "%" + quizMatchInstructionsWhereLike.ModifiedBy + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike",
                                "%" + quizMatchInstructionsWhereLike.CreatedAt + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAt    WhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAt    WhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAt    WhereLike",
                                "%" + quizMatchInstructionsWhereLike.UpdatedAt + "%");
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
                                quizMatchInstructions = new QuizMatchInstructionsModel();
                                if (!reader.IsDBNull(0))
                                    quizMatchInstructions.Id = reader.GetValue(0).ToString();
                                else
                                    quizMatchInstructions.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizMatchInstructions.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizMatchInstructions.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizMatchInstructions.MatchGroupNumber = reader.GetValue(2).ToString();
                                else
                                    quizMatchInstructions.MatchGroupNumber = null;
                                if (!reader.IsDBNull(3))
                                    quizMatchInstructions.Instruction = reader.GetValue(3).ToString();
                                else
                                    quizMatchInstructions.Instruction = null;
                                if (!reader.IsDBNull(4))
                                    quizMatchInstructions.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    quizMatchInstructions.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    quizMatchInstructions.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    quizMatchInstructions.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    quizMatchInstructions.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    quizMatchInstructions.UpdatedAt = null;
                                response.Add(quizMatchInstructions.Id, quizMatchInstructions);
                                quizMatchInstructions = new QuizMatchInstructionsModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizMatchInstructionsModel>();
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
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizMatchInstructionsModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public Dictionary<string, QuizMatchInstructionsModel> ReadD(string connectionString,
            List<QuizMatchInstructionsModel> quizMatchInstructionsWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizMatchInstructionsModel> response =
                new Dictionary<string, QuizMatchInstructionsModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql =
                    "SELECT `id`, `quiz_id`, `match_group_number`, `instruction`, `modified_by`, `created_at`, `updated_at`  FROM quiz_match_instructions ";
                if (quizMatchInstructionsWhere != null && quizMatchInstructionsWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizMatchInstructionsWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizMatchInstructionsWhere[i].Id);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizMatchInstructionsWhere[i].QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].MatchGroupNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `match_group_number` = @matchGroupNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@matchGroupNumber" + i,
                                quizMatchInstructionsWhere[i].MatchGroupNumber);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].Instruction))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `instruction` = @instruction" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@instruction" + i, quizMatchInstructionsWhere[i].Instruction);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizMatchInstructionsWhere[i].ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizMatchInstructionsWhere[i].CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updated_at" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updated_at" + i, quizMatchInstructionsWhere[i].UpdatedAt);
                        }

                        sqlIn += ")";
                        if (i != quizMatchInstructionsWhere.Count - 1)
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
                            QuizMatchInstructionsModel quizMatchInstructions = new QuizMatchInstructionsModel();
                            if (!reader.IsDBNull(0))
                                quizMatchInstructions.Id = reader.GetValue(0).ToString();
                            else
                                quizMatchInstructions.Id = null;
                            if (!reader.IsDBNull(1))
                                quizMatchInstructions.QuizId = reader.GetValue(1).ToString();
                            else
                                quizMatchInstructions.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizMatchInstructions.MatchGroupNumber = reader.GetValue(2).ToString();
                            else
                                quizMatchInstructions.MatchGroupNumber = null;
                            if (!reader.IsDBNull(3))
                                quizMatchInstructions.Instruction = reader.GetValue(3).ToString();
                            else
                                quizMatchInstructions.Instruction = null;
                            if (!reader.IsDBNull(4))
                                quizMatchInstructions.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                quizMatchInstructions.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                quizMatchInstructions.CreatedAt = reader.GetValue(5).ToString();
                            else
                                quizMatchInstructions.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                quizMatchInstructions.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                quizMatchInstructions.UpdatedAt = null;
                            response.Add(quizMatchInstructions.Id, quizMatchInstructions);
                            quizMatchInstructions = new QuizMatchInstructionsModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizMatchInstructionsModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizMatchInstructionsModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public Dictionary<string, QuizMatchInstructionsModel> ReadD(string connectionString,
            List<QuizMatchInstructionsModel> quizMatchInstructionsWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizMatchInstructionsModel> response =
                new Dictionary<string, QuizMatchInstructionsModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql =
                    "SELECT `id`, `quiz_id`, `match_group_number`, `instruction`, `modified_by`, `created_at`, `updated_at`  FROM quiz_match_instructions ";
                if (quizMatchInstructionsWhere != null && quizMatchInstructionsWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizMatchInstructionsWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizMatchInstructionsWhere[i].Id);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizMatchInstructionsWhere[i].QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].MatchGroupNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `match_group_number` = @matchGroupNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@matchGroupNumber" + i,
                                quizMatchInstructionsWhere[i].MatchGroupNumber);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].Instruction))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `instruction` = @instruction" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@instruction" + i, quizMatchInstructionsWhere[i].Instruction);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizMatchInstructionsWhere[i].ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizMatchInstructionsWhere[i].CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updated_at" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updated_at" + i, quizMatchInstructionsWhere[i].UpdatedAt);
                        }

                        sqlIn += ")";
                        if (i != quizMatchInstructionsWhere.Count - 1)
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
                            QuizMatchInstructionsModel quizMatchInstructions = new QuizMatchInstructionsModel();
                            if (!reader.IsDBNull(0))
                                quizMatchInstructions.Id = reader.GetValue(0).ToString();
                            else
                                quizMatchInstructions.Id = null;
                            if (!reader.IsDBNull(1))
                                quizMatchInstructions.QuizId = reader.GetValue(1).ToString();
                            else
                                quizMatchInstructions.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizMatchInstructions.MatchGroupNumber = reader.GetValue(2).ToString();
                            else
                                quizMatchInstructions.MatchGroupNumber = null;
                            if (!reader.IsDBNull(3))
                                quizMatchInstructions.Instruction = reader.GetValue(3).ToString();
                            else
                                quizMatchInstructions.Instruction = null;
                            if (!reader.IsDBNull(4))
                                quizMatchInstructions.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                quizMatchInstructions.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                quizMatchInstructions.CreatedAt = reader.GetValue(5).ToString();
                            else
                                quizMatchInstructions.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                quizMatchInstructions.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                quizMatchInstructions.UpdatedAt = null;
                            response.Add(quizMatchInstructions.Id, quizMatchInstructions);
                            quizMatchInstructions = new QuizMatchInstructionsModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizMatchInstructionsModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizMatchInstructionsModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public List<QuizMatchInstructionsModel> Read(string connectionString,
            QuizMatchInstructionsModel quizMatchInstructions, bool firstOne, bool orderByLastDate,
            QuizMatchInstructionsModel quizMatchInstructionsWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizMatchInstructionsModel> response = new List<QuizMatchInstructionsModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql =
                    "SELECT `id`, `quiz_id`, `match_group_number`, `instruction`, `modified_by`, `created_at`, `updated_at`  FROM quiz_match_instructions ";
                {
                    if (quizMatchInstructions != null)
                    {
                        if (!String.IsNullOrEmpty(quizMatchInstructions.Id))
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

                            cmd.Parameters.AddWithValue("@id", quizMatchInstructions.Id);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_id` = @quizId ");
                            }

                            cmd.Parameters.AddWithValue("@quizId", quizMatchInstructions.QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.MatchGroupNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `match_group_number` = @matchGroupNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `match_group_number` = @matchGroupNumber ");
                            }

                            cmd.Parameters.AddWithValue("@matchGroupNumber", quizMatchInstructions.MatchGroupNumber);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.Instruction))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `instruction` = @instruction ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `instruction` = @instruction ");
                            }

                            cmd.Parameters.AddWithValue("@instruction", quizMatchInstructions.Instruction);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.ModifiedBy))
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

                            cmd.Parameters.AddWithValue("@modifiedBy", quizMatchInstructions.ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.CreatedAt))
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

                            cmd.Parameters.AddWithValue("@createdAt", quizMatchInstructions.CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt     ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt     ");
                            }

                            cmd.Parameters.AddWithValue("@updated_at", quizMatchInstructions.UpdatedAt);
                        }
                    }

                    if (quizMatchInstructionsWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizMatchInstructionsWhereLike.Id + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike",
                                "%" + quizMatchInstructionsWhereLike.QuizId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.MatchGroupNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `match_group_number` LIKE @matchGroupNumberWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql,
                                    " where `match_group_number` LIKE @matchGroupNumberWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@matchGroupNumberWhereLike",
                                "%" + quizMatchInstructionsWhereLike.MatchGroupNumber + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.Instruction))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `instruction` LIKE @instructionWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `instruction` LIKE @instructionWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@instructionWhereLike",
                                "%" + quizMatchInstructionsWhereLike.Instruction + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike",
                                "%" + quizMatchInstructionsWhereLike.ModifiedBy + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike",
                                "%" + quizMatchInstructionsWhereLike.CreatedAt + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAt    WhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAt    WhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAt    WhereLike",
                                "%" + quizMatchInstructionsWhereLike.UpdatedAt + "%");
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
                                quizMatchInstructions = new QuizMatchInstructionsModel();
                                if (!reader.IsDBNull(0))
                                    quizMatchInstructions.Id = reader.GetValue(0).ToString();
                                else
                                    quizMatchInstructions.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizMatchInstructions.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizMatchInstructions.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizMatchInstructions.MatchGroupNumber = reader.GetValue(2).ToString();
                                else
                                    quizMatchInstructions.MatchGroupNumber = null;
                                if (!reader.IsDBNull(3))
                                    quizMatchInstructions.Instruction = reader.GetValue(3).ToString();
                                else
                                    quizMatchInstructions.Instruction = null;
                                if (!reader.IsDBNull(4))
                                    quizMatchInstructions.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    quizMatchInstructions.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    quizMatchInstructions.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    quizMatchInstructions.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    quizMatchInstructions.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    quizMatchInstructions.UpdatedAt = null;
                                response.Add(quizMatchInstructions);
                                quizMatchInstructions = new QuizMatchInstructionsModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizMatchInstructionsModel>();
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
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizMatchInstructionsModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public List<QuizMatchInstructionsModel> Read(string connectionString,
            QuizMatchInstructionsModel quizMatchInstructions, bool orderByLastDate, int fromRow, int limit,
            QuizMatchInstructionsModel quizMatchInstructionsWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizMatchInstructionsModel> response = new List<QuizMatchInstructionsModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql =
                    "SELECT `id`, `quiz_id`, `match_group_number`, `instruction`, `modified_by`, `created_at`, `updated_at`  FROM quiz_match_instructions ";
                {
                    if (quizMatchInstructions != null)
                    {
                        if (!String.IsNullOrEmpty(quizMatchInstructions.Id))
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

                            cmd.Parameters.AddWithValue("@id", quizMatchInstructions.Id);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_id` = @quizId ");
                            }

                            cmd.Parameters.AddWithValue("@quizId", quizMatchInstructions.QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.MatchGroupNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `match_group_number` = @matchGroupNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `match_group_number` = @matchGroupNumber ");
                            }

                            cmd.Parameters.AddWithValue("@matchGroupNumber", quizMatchInstructions.MatchGroupNumber);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.Instruction))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `instruction` = @instruction ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `instruction` = @instruction ");
                            }

                            cmd.Parameters.AddWithValue("@instruction", quizMatchInstructions.Instruction);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.ModifiedBy))
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

                            cmd.Parameters.AddWithValue("@modifiedBy", quizMatchInstructions.ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.CreatedAt))
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

                            cmd.Parameters.AddWithValue("@createdAt", quizMatchInstructions.CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructions.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt     ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt     ");
                            }

                            cmd.Parameters.AddWithValue("@updated_at", quizMatchInstructions.UpdatedAt);
                        }
                    }

                    if (quizMatchInstructionsWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizMatchInstructionsWhereLike.Id + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike",
                                "%" + quizMatchInstructionsWhereLike.QuizId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.MatchGroupNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `match_group_number` LIKE @matchGroupNumberWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql,
                                    " where `match_group_number` LIKE @matchGroupNumberWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@matchGroupNumberWhereLike",
                                "%" + quizMatchInstructionsWhereLike.MatchGroupNumber + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.Instruction))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `instruction` LIKE @instructionWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `instruction` LIKE @instructionWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@instructionWhereLike",
                                "%" + quizMatchInstructionsWhereLike.Instruction + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike",
                                "%" + quizMatchInstructionsWhereLike.ModifiedBy + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike",
                                "%" + quizMatchInstructionsWhereLike.CreatedAt + "%");
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAt    WhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAt    WhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAt    WhereLike",
                                "%" + quizMatchInstructionsWhereLike.UpdatedAt + "%");
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
                                quizMatchInstructions = new QuizMatchInstructionsModel();
                                if (!reader.IsDBNull(0))
                                    quizMatchInstructions.Id = reader.GetValue(0).ToString();
                                else
                                    quizMatchInstructions.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizMatchInstructions.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizMatchInstructions.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizMatchInstructions.MatchGroupNumber = reader.GetValue(2).ToString();
                                else
                                    quizMatchInstructions.MatchGroupNumber = null;
                                if (!reader.IsDBNull(3))
                                    quizMatchInstructions.Instruction = reader.GetValue(3).ToString();
                                else
                                    quizMatchInstructions.Instruction = null;
                                if (!reader.IsDBNull(4))
                                    quizMatchInstructions.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    quizMatchInstructions.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    quizMatchInstructions.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    quizMatchInstructions.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    quizMatchInstructions.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    quizMatchInstructions.UpdatedAt = null;
                                response.Add(quizMatchInstructions);
                                quizMatchInstructions = new QuizMatchInstructionsModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizMatchInstructionsModel>();
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
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizMatchInstructionsModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public List<QuizMatchInstructionsModel> Read(string connectionString,
            List<QuizMatchInstructionsModel> quizMatchInstructionsWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizMatchInstructionsModel> response = new List<QuizMatchInstructionsModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql =
                    "SELECT `id`, `quiz_id`, `match_group_number`, `instruction`, `modified_by`, `created_at`, `updated_at`  FROM quiz_match_instructions ";
                if (quizMatchInstructionsWhere != null && quizMatchInstructionsWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizMatchInstructionsWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizMatchInstructionsWhere[i].Id);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizMatchInstructionsWhere[i].QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].MatchGroupNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `match_group_number` = @matchGroupNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@matchGroupNumber" + i,
                                quizMatchInstructionsWhere[i].MatchGroupNumber);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].Instruction))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `instruction` = @instruction" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@instruction" + i, quizMatchInstructionsWhere[i].Instruction);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizMatchInstructionsWhere[i].ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizMatchInstructionsWhere[i].CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updated_at" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updated_at" + i, quizMatchInstructionsWhere[i].UpdatedAt);
                        }

                        sqlIn += ")";
                        if (i != quizMatchInstructionsWhere.Count - 1)
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
                            QuizMatchInstructionsModel quizMatchInstructions = new QuizMatchInstructionsModel();
                            if (!reader.IsDBNull(0))
                                quizMatchInstructions.Id = reader.GetValue(0).ToString();
                            else
                                quizMatchInstructions.Id = null;
                            if (!reader.IsDBNull(1))
                                quizMatchInstructions.QuizId = reader.GetValue(1).ToString();
                            else
                                quizMatchInstructions.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizMatchInstructions.MatchGroupNumber = reader.GetValue(2).ToString();
                            else
                                quizMatchInstructions.MatchGroupNumber = null;
                            if (!reader.IsDBNull(3))
                                quizMatchInstructions.Instruction = reader.GetValue(3).ToString();
                            else
                                quizMatchInstructions.Instruction = null;
                            if (!reader.IsDBNull(4))
                                quizMatchInstructions.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                quizMatchInstructions.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                quizMatchInstructions.CreatedAt = reader.GetValue(5).ToString();
                            else
                                quizMatchInstructions.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                quizMatchInstructions.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                quizMatchInstructions.UpdatedAt = null;
                            response.Add(quizMatchInstructions);
                            quizMatchInstructions = new QuizMatchInstructionsModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizMatchInstructionsModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizMatchInstructionsModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public List<QuizMatchInstructionsModel> Read(string connectionString,
            List<QuizMatchInstructionsModel> quizMatchInstructionsWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizMatchInstructionsModel> response = new List<QuizMatchInstructionsModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql =
                    "SELECT `id`, `quiz_id`, `match_group_number`, `instruction`, `modified_by`, `created_at`, `updated_at`  FROM quiz_match_instructions ";
                if (quizMatchInstructionsWhere != null && quizMatchInstructionsWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizMatchInstructionsWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizMatchInstructionsWhere[i].Id);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizMatchInstructionsWhere[i].QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].MatchGroupNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `match_group_number` = @matchGroupNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@matchGroupNumber" + i,
                                quizMatchInstructionsWhere[i].MatchGroupNumber);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].Instruction))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `instruction` = @instruction" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@instruction" + i, quizMatchInstructionsWhere[i].Instruction);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizMatchInstructionsWhere[i].ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizMatchInstructionsWhere[i].CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizMatchInstructionsWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updated_at" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updated_at" + i, quizMatchInstructionsWhere[i].UpdatedAt);
                        }

                        sqlIn += ")";
                        if (i != quizMatchInstructionsWhere.Count - 1)
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
                            QuizMatchInstructionsModel quizMatchInstructions = new QuizMatchInstructionsModel();
                            if (!reader.IsDBNull(0))
                                quizMatchInstructions.Id = reader.GetValue(0).ToString();
                            else
                                quizMatchInstructions.Id = null;
                            if (!reader.IsDBNull(1))
                                quizMatchInstructions.QuizId = reader.GetValue(1).ToString();
                            else
                                quizMatchInstructions.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizMatchInstructions.MatchGroupNumber = reader.GetValue(2).ToString();
                            else
                                quizMatchInstructions.MatchGroupNumber = null;
                            if (!reader.IsDBNull(3))
                                quizMatchInstructions.Instruction = reader.GetValue(3).ToString();
                            else
                                quizMatchInstructions.Instruction = null;
                            if (!reader.IsDBNull(4))
                                quizMatchInstructions.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                quizMatchInstructions.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                quizMatchInstructions.CreatedAt = reader.GetValue(5).ToString();
                            else
                                quizMatchInstructions.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                quizMatchInstructions.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                quizMatchInstructions.UpdatedAt = null;
                            response.Add(quizMatchInstructions);
                            quizMatchInstructions = new QuizMatchInstructionsModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizMatchInstructionsModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizMatchInstructionsModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public List<QuizMatchInstructionsModel> Create(string connectionString,
            QuizMatchInstructionsModel quizMatchInstructions)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizMatchInstructionsModel> response = new List<QuizMatchInstructionsModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO quiz_match_instructions (";
                string values = "(";
                if (!String.IsNullOrEmpty(quizMatchInstructions.Id))
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
                    cmd.Parameters.AddWithValue("@id", quizMatchInstructions.Id);
                }

                if (!String.IsNullOrEmpty(quizMatchInstructions.QuizId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `quiz_id` ");
                        values = String.Concat(values, ", @quizId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `quiz_id` ");
                        values = String.Concat(values, " @quizId");
                    }

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", quizMatchInstructions.QuizId);
                }

                if (!String.IsNullOrEmpty(quizMatchInstructions.MatchGroupNumber))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `match_group_number` ");
                        values = String.Concat(values, ", @matchGroupNumber");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `match_group_number` ");
                        values = String.Concat(values, " @matchGroupNumber");
                    }

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@matchGroupNumber", quizMatchInstructions.MatchGroupNumber);
                }

                if (!String.IsNullOrEmpty(quizMatchInstructions.Instruction))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `instruction` ");
                        values = String.Concat(values, ", @instruction");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `instruction` ");
                        values = String.Concat(values, " @instruction");
                    }

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@instruction", quizMatchInstructions.Instruction);
                }

                if (!String.IsNullOrEmpty(quizMatchInstructions.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", quizMatchInstructions.ModifiedBy);
                }

                if (!String.IsNullOrEmpty(quizMatchInstructions.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", quizMatchInstructions.CreatedAt);
                }

                if (!String.IsNullOrEmpty(quizMatchInstructions.UpdatedAt))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `updated_at` ");
                        values = String.Concat(values, ", @updated_at");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `updated_at` ");
                        values = String.Concat(values, " @updated_at");
                    }

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updated_at", quizMatchInstructions.UpdatedAt);
                }

                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                quizMatchInstructions.Id = cmd.LastInsertedId.ToString();
                response.Add(quizMatchInstructions);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " +
                                       e.Message;
                response = new List<QuizMatchInstructionsModel>();
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

        public List<QuizMatchInstructionsModel> Create(string connectionString,
            List<QuizMatchInstructionsModel> quizMatchInstructionsList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizMatchInstructionsModel> response = new List<QuizMatchInstructionsModel>();
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
                        for (int i = 0; i < quizMatchInstructionsList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO quiz_match_instructions (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(quizMatchInstructionsList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, quizMatchInstructionsList[i].Id);
                                }

                                if (!String.IsNullOrEmpty(quizMatchInstructionsList[i].QuizId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `quiz_id` ");
                                        values = String.Concat(values, ", @quizId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `quiz_id` ");
                                        values = String.Concat(values, "@quizId" + i);
                                    }

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, quizMatchInstructionsList[i].QuizId);
                                }

                                if (!String.IsNullOrEmpty(quizMatchInstructionsList[i].MatchGroupNumber))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `match_group_number` ");
                                        values = String.Concat(values, ", @matchGroupNumber" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `match_group_number` ");
                                        values = String.Concat(values, "@matchGroupNumber" + i);
                                    }

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@matchGroupNumber" + i,
                                        quizMatchInstructionsList[i].MatchGroupNumber);
                                }

                                if (!String.IsNullOrEmpty(quizMatchInstructionsList[i].Instruction))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `instruction` ");
                                        values = String.Concat(values, ", @instruction" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `instruction` ");
                                        values = String.Concat(values, "@instruction" + i);
                                    }

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@instruction" + i,
                                        quizMatchInstructionsList[i].Instruction);
                                }

                                if (!String.IsNullOrEmpty(quizMatchInstructionsList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i,
                                        quizMatchInstructionsList[i].ModifiedBy);
                                }

                                if (!String.IsNullOrEmpty(quizMatchInstructionsList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i,
                                        quizMatchInstructionsList[i].CreatedAt);
                                }

                                if (!String.IsNullOrEmpty(quizMatchInstructionsList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `updated_at` ");
                                        values = String.Concat(values, ", @updated_at" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `updated_at` ");
                                        values = String.Concat(values, "@updated_at" + i);
                                    }

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updated_at" + i,
                                        quizMatchInstructionsList[i].UpdatedAt);
                                }

                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(quizMatchInstructionsList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " +
                                                       e.ErrorCode + " - " + e.Message;
                                response = new List<QuizMatchInstructionsModel>();
                            }
                        }

                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " +
                                               e.Message;
                        trans.Rollback();
                        response = new List<QuizMatchInstructionsModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString =
                    "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizMatchInstructionsModel>();
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

        public List<QuizMatchInstructionsModel> Update(string connectionString,
            QuizMatchInstructionsModel quizMatchInstructions, QuizMatchInstructionsModel quizMatchInstructionsWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizMatchInstructionsModel> response = new List<QuizMatchInstructionsModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE quiz_match_instructions SET ";
                if (!String.IsNullOrEmpty(quizMatchInstructions.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", quizMatchInstructions.Id);
                }

                if (!String.IsNullOrEmpty(quizMatchInstructions.QuizId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `quiz_id` = @quizId");
                    else
                        sql = String.Concat(sql, " `quiz_id` = @quizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", quizMatchInstructions.QuizId);
                }

                if (!String.IsNullOrEmpty(quizMatchInstructions.MatchGroupNumber))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `match_group_number` = @matchGroupNumber");
                    else
                        sql = String.Concat(sql, " `match_group_number` = @matchGroupNumber");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@matchGroupNumber", quizMatchInstructions.MatchGroupNumber);
                }

                if (!String.IsNullOrEmpty(quizMatchInstructions.Instruction))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `instruction` = @instruction");
                    else
                        sql = String.Concat(sql, " `instruction` = @instruction");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@instruction", quizMatchInstructions.Instruction);
                }

                if (!String.IsNullOrEmpty(quizMatchInstructions.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", quizMatchInstructions.ModifiedBy);
                }

                if (!String.IsNullOrEmpty(quizMatchInstructions.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", quizMatchInstructions.CreatedAt);
                }

                if (!String.IsNullOrEmpty(quizMatchInstructions.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updated_at");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updated_at");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updated_at", quizMatchInstructions.UpdatedAt);
                }

                if (quizMatchInstructionsWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(quizMatchInstructionsWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", quizMatchInstructionsWhere.Id);
                    }

                    if (!String.IsNullOrEmpty(quizMatchInstructionsWhere.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizIdWhere", quizMatchInstructionsWhere.QuizId);
                    }

                    if (!String.IsNullOrEmpty(quizMatchInstructionsWhere.MatchGroupNumber))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `match_group_number` = @matchGroupNumberWhere");
                        else
                            sql = String.Concat(sql, " `match_group_number` = @matchGroupNumberWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@matchGroupNumberWhere",
                            quizMatchInstructionsWhere.MatchGroupNumber);
                    }

                    if (!String.IsNullOrEmpty(quizMatchInstructionsWhere.Instruction))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `instruction` = @instructionWhere");
                        else
                            sql = String.Concat(sql, " `instruction` = @instructionWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@instructionWhere", quizMatchInstructionsWhere.Instruction);
                    }

                    if (!String.IsNullOrEmpty(quizMatchInstructionsWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", quizMatchInstructionsWhere.ModifiedBy);
                    }

                    if (!String.IsNullOrEmpty(quizMatchInstructionsWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", quizMatchInstructionsWhere.CreatedAt);
                    }

                    if (!String.IsNullOrEmpty(quizMatchInstructionsWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt    Where");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt    Where");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt    Where", quizMatchInstructionsWhere.UpdatedAt);
                    }
                }

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(quizMatchInstructions);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " +
                                       e.Message;
                response = new List<QuizMatchInstructionsModel>();
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

        public List<QuizMatchInstructionsModel> Update(string connectionString,
            List<QuizMatchInstructionsModel> quizMatchInstructionsList,
            List<QuizMatchInstructionsModel> quizMatchInstructionsWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizMatchInstructionsModel> response = new List<QuizMatchInstructionsModel>();
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
                        for (int i = 0; i < quizMatchInstructionsList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE quiz_match_instructions SET ";
                                if (!String.IsNullOrEmpty(quizMatchInstructionsList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, quizMatchInstructionsList[i].Id);
                                }

                                if (!String.IsNullOrEmpty(quizMatchInstructionsList[i].QuizId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `quiz_id` = @quizId" + i);
                                    else
                                        sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, quizMatchInstructionsList[i].QuizId);
                                }

                                if (!String.IsNullOrEmpty(quizMatchInstructionsList[i].MatchGroupNumber))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `match_group_number` = @matchGroupNumber" + i);
                                    else
                                        sql = String.Concat(sql, " `match_group_number` = @matchGroupNumber" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@matchGroupNumber" + i,
                                        quizMatchInstructionsList[i].MatchGroupNumber);
                                }

                                if (!String.IsNullOrEmpty(quizMatchInstructionsList[i].Instruction))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `instruction` = @instruction" + i);
                                    else
                                        sql = String.Concat(sql, " `instruction` = @instruction" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@instruction" + i,
                                        quizMatchInstructionsList[i].Instruction);
                                }

                                if (!String.IsNullOrEmpty(quizMatchInstructionsList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i,
                                        quizMatchInstructionsList[i].ModifiedBy);
                                }

                                if (!String.IsNullOrEmpty(quizMatchInstructionsList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i,
                                        quizMatchInstructionsList[i].CreatedAt);
                                }

                                if (!String.IsNullOrEmpty(quizMatchInstructionsList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updated_at" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updated_at" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updated_at" + i,
                                        quizMatchInstructionsList[i].UpdatedAt);
                                }

                                if (quizMatchInstructionsWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(quizMatchInstructionsWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i,
                                            quizMatchInstructionsWhereList[i].Id);
                                    }

                                    if (!String.IsNullOrEmpty(quizMatchInstructionsWhereList[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizIdWhere" + i,
                                            quizMatchInstructionsWhereList[i].QuizId);
                                    }

                                    if (!String.IsNullOrEmpty(quizMatchInstructionsWhereList[i].MatchGroupNumber))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql,
                                                " AND `match_group_number` = @matchGroupNumberWhere" + i);
                                        else
                                            sql = String.Concat(sql,
                                                " `match_group_number` = @matchGroupNumberWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@matchGroupNumberWhere" + i,
                                            quizMatchInstructionsWhereList[i].MatchGroupNumber);
                                    }

                                    if (!String.IsNullOrEmpty(quizMatchInstructionsWhereList[i].Instruction))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `instruction` = @instructionWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `instruction` = @instructionWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@instructionWhere" + i,
                                            quizMatchInstructionsWhereList[i].Instruction);
                                    }

                                    if (!String.IsNullOrEmpty(quizMatchInstructionsWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i,
                                            quizMatchInstructionsWhereList[i].ModifiedBy);
                                    }

                                    if (!String.IsNullOrEmpty(quizMatchInstructionsWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i,
                                            quizMatchInstructionsWhereList[i].CreatedAt);
                                    }

                                    if (!String.IsNullOrEmpty(quizMatchInstructionsWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql,
                                                " AND `updated_at` = @updatedAt    Where" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt    Where" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt    Where" + i,
                                            quizMatchInstructionsWhereList[i].UpdatedAt);
                                    }
                                }

                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizMatchInstructionsList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" +
                                                       e.ErrorCode + " - " + e.Message;
                                response = new List<QuizMatchInstructionsModel>();
                            }
                        }

                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode +
                                               " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizMatchInstructionsModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " +
                                       e.Message;
                response = new List<QuizMatchInstructionsModel>();
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

        public List<QuizMatchInstructionsModel> Delete(string connectionString,
            QuizMatchInstructionsModel quizMatchInstructions)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizMatchInstructionsModel> response = new List<QuizMatchInstructionsModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM quiz_match_instructions  ";
                if (quizMatchInstructions != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(quizMatchInstructions.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", quizMatchInstructions.Id);
                    }

                    if (!String.IsNullOrEmpty(quizMatchInstructions.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizId");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizId", quizMatchInstructions.QuizId);
                    }

                    if (!String.IsNullOrEmpty(quizMatchInstructions.MatchGroupNumber))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `match_group_number` = @matchGroupNumber");
                        else
                            sql = String.Concat(sql, " `match_group_number` = @matchGroupNumber");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@matchGroupNumber", quizMatchInstructions.MatchGroupNumber);
                    }

                    if (!String.IsNullOrEmpty(quizMatchInstructions.Instruction))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `instruction` = @instruction");
                        else
                            sql = String.Concat(sql, " `instruction` = @instruction");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@instruction", quizMatchInstructions.Instruction);
                    }

                    if (!String.IsNullOrEmpty(quizMatchInstructions.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", quizMatchInstructions.ModifiedBy);
                    }

                    if (!String.IsNullOrEmpty(quizMatchInstructions.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", quizMatchInstructions.CreatedAt);
                    }

                    if (!String.IsNullOrEmpty(quizMatchInstructions.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updated_at");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updated_at");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updated_at", quizMatchInstructions.UpdatedAt);
                    }
                }

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(quizMatchInstructions);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " +
                                       e.Message;
                response = new List<QuizMatchInstructionsModel>();
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

        public List<QuizMatchInstructionsModel> Delete(string connectionString,
            List<QuizMatchInstructionsModel> quizMatchInstructions)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizMatchInstructionsModel> response = new List<QuizMatchInstructionsModel>();
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
                        for (int i = 0; i < quizMatchInstructions.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM quiz_match_instructions  ";
                                if (quizMatchInstructions[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(quizMatchInstructions[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, quizMatchInstructions[i].Id);
                                    }

                                    if (!String.IsNullOrEmpty(quizMatchInstructions[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizId" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizId" + i, quizMatchInstructions[i].QuizId);
                                    }

                                    if (!String.IsNullOrEmpty(quizMatchInstructions[i].MatchGroupNumber))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql,
                                                " AND `match_group_number` = @matchGroupNumber" + i);
                                        else
                                            sql = String.Concat(sql, " `match_group_number` = @matchGroupNumber" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@matchGroupNumber" + i,
                                            quizMatchInstructions[i].MatchGroupNumber);
                                    }

                                    if (!String.IsNullOrEmpty(quizMatchInstructions[i].Instruction))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `instruction` = @instruction" + i);
                                        else
                                            sql = String.Concat(sql, " `instruction` = @instruction" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@instruction" + i,
                                            quizMatchInstructions[i].Instruction);
                                    }

                                    if (!String.IsNullOrEmpty(quizMatchInstructions[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i,
                                            quizMatchInstructions[i].ModifiedBy);
                                    }

                                    if (!String.IsNullOrEmpty(quizMatchInstructions[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i,
                                            quizMatchInstructions[i].CreatedAt);
                                    }

                                    if (!String.IsNullOrEmpty(quizMatchInstructions[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updated_at" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updated_at" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updated_at" + i,
                                            quizMatchInstructions[i].UpdatedAt);
                                    }
                                }

                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizMatchInstructions[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" +
                                                       e.ErrorCode + " - " + e.Message;
                                response = new List<QuizMatchInstructionsModel>();
                            }
                        }

                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode +
                                               " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizMatchInstructionsModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " +
                                       e.Message;
                response = new List<QuizMatchInstructionsModel>();
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