using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Lessons;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons
{
    class QuizQuestionDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, QuizQuestionModel> ReadD(string connectionString, QuizQuestionModel quizQuestion, bool firstOne, bool orderByLastDate, QuizQuestionModel quizQuestionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionModel> response = new Dictionary<string, QuizQuestionModel>();
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
                    "SELECT `id`, `quiz_id`, `question_type_id`, `desc`, `quiz_match_instruction_id`, `custom_value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_questions ";
                {
                    if (quizQuestion != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestion.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestion.Id);
                        }
                        if (!String.IsNullOrEmpty(quizQuestion.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", quizQuestion.QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.QuestionTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `question_type_id` = @questionTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `question_type_id` = @questionTypeId ");
                            }

                            cmd.Parameters.AddWithValue("@questionTypeId", quizQuestion.QuestionTypeId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", quizQuestion.Desc);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.QuizMatchInstructionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql,
                                    " where `quiz_match_instruction_id` = @quizMatchInstructionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_match_instruction_id` = @quizMatchInstructionId ");
                            }

                            cmd.Parameters.AddWithValue("@quizMatchInstructionId", quizQuestion.QuizMatchInstructionId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.CustomValue))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `custom_value` = @customValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `custom_value` = @customValue ");
                            }

                            cmd.Parameters.AddWithValue("@customValue", quizQuestion.CustomValue);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestion.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizQuestion.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestion.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizQuestion.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestion.UpdatedAt);
                        }
                    }
                    if (quizQuestionWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizQuestionWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + quizQuestionWhereLike.QuizId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.QuestionTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `question_type_id` LIKE @questionTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `question_type_id` LIKE @questionTypeIdWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@questionTypeIdWhereLike",
                                "%" + quizQuestionWhereLike.QuestionTypeId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.Desc))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `desc` LIKE @descWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `desc` LIKE @descWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@descWhereLike", "%" + quizQuestionWhereLike.Desc + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.QuizMatchInstructionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql,
                                    " and `quiz_match_instruction_id` LIKE @quizMatchInstructionIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql,
                                    " where `quiz_match_instruction_id` LIKE @quizMatchInstructionIdWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizMatchInstructionIdWhereLike",
                                "%" + quizQuestionWhereLike.QuizMatchInstructionId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.CustomValue))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `custom_value` LIKE @customValueWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `custom_value` LIKE @customValueWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customValueWhereLike",
                                "%" + quizQuestionWhereLike.CustomValue + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizQuestionWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizQuestionWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizQuestionWhereLike.UpdatedAt + "%");
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
                                quizQuestion = new QuizQuestionModel();
                                if (!reader.IsDBNull(0))
                                    quizQuestion.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestion.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestion.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizQuestion.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestion.QuestionTypeId = reader.GetValue(2).ToString();
                                else
                                    quizQuestion.QuestionTypeId = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestion.Desc = reader.GetValue(3).ToString();
                                else
                                    quizQuestion.Desc = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestion.QuizMatchInstructionId = reader.GetValue(4).ToString();
                                else
                                    quizQuestion.QuizMatchInstructionId = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestion.CustomValue = reader.GetValue(5).ToString();
                                else
                                    quizQuestion.CustomValue = null;
                                if (!reader.IsDBNull(6))
                                    quizQuestion.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    quizQuestion.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    quizQuestion.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    quizQuestion.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    quizQuestion.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    quizQuestion.UpdatedAt = null;
                                response.Add(quizQuestion.Id, quizQuestion);
                                quizQuestion = new QuizQuestionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizQuestionModel>();
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
                response = new Dictionary<string, QuizQuestionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizQuestionModel> ReadD(string connectionString, QuizQuestionModel quizQuestion, bool orderByLastDate, int fromRow, int limit, QuizQuestionModel quizQuestionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionModel> response = new Dictionary<string, QuizQuestionModel>();
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
                    "SELECT `id`, `quiz_id`, `question_type_id`, `desc`, `quiz_match_instruction_id`, `custom_value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_questions ";
                {
                    if (quizQuestion != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestion.Id))
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

                            cmd.Parameters.AddWithValue("@id", quizQuestion.Id);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.QuizId))
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

                            cmd.Parameters.AddWithValue("@quizId", quizQuestion.QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.QuestionTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `question_type_id` = @questionTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `question_type_id` = @questionTypeId ");
                            }

                            cmd.Parameters.AddWithValue("@questionTypeId", quizQuestion.QuestionTypeId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", quizQuestion.Desc);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.QuizMatchInstructionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql,
                                    " where `quiz_match_instruction_id` = @quizMatchInstructionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_match_instruction_id` = @quizMatchInstructionId ");
                            }

                            cmd.Parameters.AddWithValue("@quizMatchInstructionId", quizQuestion.QuizMatchInstructionId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.CustomValue))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `custom_value` = @customValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `custom_value` = @customValue ");
                            }

                            cmd.Parameters.AddWithValue("@customValue", quizQuestion.CustomValue);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestion.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizQuestion.CreatedAt))
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

                            cmd.Parameters.AddWithValue("@createdAt", quizQuestion.CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }

                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestion.UpdatedAt);
                        }
                    }
                    if (quizQuestionWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizQuestionWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + quizQuestionWhereLike.QuizId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.QuestionTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `question_type_id` LIKE @questionTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `question_type_id` LIKE @questionTypeIdWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@questionTypeIdWhereLike",
                                "%" + quizQuestionWhereLike.QuestionTypeId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.Desc))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `desc` LIKE @descWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `desc` LIKE @descWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@descWhereLike", "%" + quizQuestionWhereLike.Desc + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.QuizMatchInstructionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql,
                                    " and `quiz_match_instruction_id` LIKE @quizMatchInstructionIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql,
                                    " where `quiz_match_instruction_id` LIKE @quizMatchInstructionIdWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizMatchInstructionIdWhereLike",
                                "%" + quizQuestionWhereLike.QuizMatchInstructionId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.CustomValue))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `custom_value` LIKE @customValueWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `custom_value` LIKE @customValueWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customValueWhereLike",
                                "%" + quizQuestionWhereLike.CustomValue + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizQuestionWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizQuestionWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizQuestionWhereLike.UpdatedAt + "%");
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
                                quizQuestion = new QuizQuestionModel();
                                if (!reader.IsDBNull(0))
                                    quizQuestion.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestion.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestion.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizQuestion.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestion.QuestionTypeId = reader.GetValue(2).ToString();
                                else
                                    quizQuestion.QuestionTypeId = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestion.Desc = reader.GetValue(3).ToString();
                                else
                                    quizQuestion.Desc = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestion.QuizMatchInstructionId = reader.GetValue(4).ToString();
                                else
                                    quizQuestion.QuizMatchInstructionId = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestion.CustomValue = reader.GetValue(5).ToString();
                                else
                                    quizQuestion.CustomValue = null;
                                if (!reader.IsDBNull(6))
                                    quizQuestion.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    quizQuestion.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    quizQuestion.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    quizQuestion.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    quizQuestion.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    quizQuestion.UpdatedAt = null;
                                response.Add(quizQuestion.Id, quizQuestion);
                                quizQuestion = new QuizQuestionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizQuestionModel>();
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
                response = new Dictionary<string, QuizQuestionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizQuestionModel> ReadD(string connectionString, List<QuizQuestionModel> quizQuestionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionModel> response = new Dictionary<string, QuizQuestionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql =
                    "SELECT `id`, `quiz_id`, `question_type_id`, `desc`, `quiz_match_instruction_id`, `custom_value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_questions ";
                if (quizQuestionWhere != null && quizQuestionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizQuestionWhere[i].QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].QuestionTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_type_id` = @questionTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionTypeId" + i, quizQuestionWhere[i].QuestionTypeId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, quizQuestionWhere[i].Desc);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].QuizMatchInstructionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_match_instruction_id` = @quizMatchInstructionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizMatchInstructionId" + i,
                                quizQuestionWhere[i].QuizMatchInstructionId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].CustomValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `custom_value` = @customValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customValue" + i, quizQuestionWhere[i].CustomValue);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionWhere.Count - 1)
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
                            QuizQuestionModel quizQuestion = new QuizQuestionModel();
                            if (!reader.IsDBNull(0))
                                quizQuestion.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestion.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestion.QuizId = reader.GetValue(1).ToString();
                            else
                                quizQuestion.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestion.QuestionTypeId = reader.GetValue(2).ToString();
                            else
                                quizQuestion.QuestionTypeId = null;
                            if (!reader.IsDBNull(3))
                                quizQuestion.Desc = reader.GetValue(3).ToString();
                            else
                                quizQuestion.Desc = null;
                            if (!reader.IsDBNull(4))
                                quizQuestion.QuizMatchInstructionId = reader.GetValue(4).ToString();
                            else
                                quizQuestion.QuizMatchInstructionId = null;
                            if (!reader.IsDBNull(5))
                                quizQuestion.CustomValue = reader.GetValue(5).ToString();
                            else
                                quizQuestion.CustomValue = null;
                            if (!reader.IsDBNull(6))
                                quizQuestion.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                quizQuestion.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                quizQuestion.CreatedAt = reader.GetValue(7).ToString();
                            else
                                quizQuestion.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                quizQuestion.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                quizQuestion.UpdatedAt = null;
                            response.Add(quizQuestion.Id, quizQuestion);
                            quizQuestion = new QuizQuestionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizQuestionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizQuestionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizQuestionModel> ReadD(string connectionString, List<QuizQuestionModel> quizQuestionWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionModel> response = new Dictionary<string, QuizQuestionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql =
                    "SELECT `id`, `quiz_id`, `question_type_id`, `desc`, `quiz_match_instruction_id`, `custom_value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_questions ";
                if (quizQuestionWhere != null && quizQuestionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionWhere[i].Id);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizQuestionWhere[i].QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].QuestionTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_type_id` = @questionTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionTypeId" + i, quizQuestionWhere[i].QuestionTypeId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, quizQuestionWhere[i].Desc);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].QuizMatchInstructionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_match_instruction_id` = @quizMatchInstructionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizMatchInstructionId" + i,
                                quizQuestionWhere[i].QuizMatchInstructionId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].CustomValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `custom_value` = @customValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customValue" + i, quizQuestionWhere[i].CustomValue);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionWhere.Count - 1)
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
                            QuizQuestionModel quizQuestion = new QuizQuestionModel();
                            if (!reader.IsDBNull(0))
                                quizQuestion.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestion.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestion.QuizId = reader.GetValue(1).ToString();
                            else
                                quizQuestion.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestion.QuestionTypeId = reader.GetValue(2).ToString();
                            else
                                quizQuestion.QuestionTypeId = null;
                            if (!reader.IsDBNull(3))
                                quizQuestion.Desc = reader.GetValue(3).ToString();
                            else
                                quizQuestion.Desc = null;
                            if (!reader.IsDBNull(4))
                                quizQuestion.QuizMatchInstructionId = reader.GetValue(4).ToString();
                            else
                                quizQuestion.QuizMatchInstructionId = null;
                            if (!reader.IsDBNull(5))
                                quizQuestion.CustomValue = reader.GetValue(5).ToString();
                            else
                                quizQuestion.CustomValue = null;
                            if (!reader.IsDBNull(6))
                                quizQuestion.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                quizQuestion.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                quizQuestion.CreatedAt = reader.GetValue(7).ToString();
                            else
                                quizQuestion.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                quizQuestion.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                quizQuestion.UpdatedAt = null;
                            response.Add(quizQuestion.Id, quizQuestion);
                            quizQuestion = new QuizQuestionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizQuestionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizQuestionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionModel> Read(string connectionString, QuizQuestionModel quizQuestion, bool firstOne, bool orderByLastDate, QuizQuestionModel quizQuestionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionModel> response = new List<QuizQuestionModel>();
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
                    "SELECT `id`, `quiz_id`, `question_type_id`, `desc`, `quiz_match_instruction_id`, `custom_value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_questions ";
                {
                    if (quizQuestion != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestion.Id))
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

                            cmd.Parameters.AddWithValue("@id", quizQuestion.Id);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.QuizId))
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

                            cmd.Parameters.AddWithValue("@quizId", quizQuestion.QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.QuestionTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `question_type_id` = @questionTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `question_type_id` = @questionTypeId ");
                            }

                            cmd.Parameters.AddWithValue("@questionTypeId", quizQuestion.QuestionTypeId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", quizQuestion.Desc);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.QuizMatchInstructionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql,
                                    " where `quiz_match_instruction_id` = @quizMatchInstructionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_match_instruction_id` = @quizMatchInstructionId ");
                            }

                            cmd.Parameters.AddWithValue("@quizMatchInstructionId", quizQuestion.QuizMatchInstructionId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.CustomValue))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `custom_value` = @customValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `custom_value` = @customValue ");
                            }

                            cmd.Parameters.AddWithValue("@customValue", quizQuestion.CustomValue);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestion.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizQuestion.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestion.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizQuestion.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestion.UpdatedAt);
                        }
                    }
                    if (quizQuestionWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizQuestionWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + quizQuestionWhereLike.QuizId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.QuestionTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `question_type_id` LIKE @questionTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `question_type_id` LIKE @questionTypeIdWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@questionTypeIdWhereLike",
                                "%" + quizQuestionWhereLike.QuestionTypeId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.Desc))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `desc` LIKE @descWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `desc` LIKE @descWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@descWhereLike", "%" + quizQuestionWhereLike.Desc + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.QuizMatchInstructionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql,
                                    " and `quiz_match_instruction_id` LIKE @quizMatchInstructionIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql,
                                    " where `quiz_match_instruction_id` LIKE @quizMatchInstructionIdWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizMatchInstructionIdWhereLike",
                                "%" + quizQuestionWhereLike.QuizMatchInstructionId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.CustomValue))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `custom_value` LIKE @customValueWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `custom_value` LIKE @customValueWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customValueWhereLike",
                                "%" + quizQuestionWhereLike.CustomValue + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizQuestionWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizQuestionWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizQuestionWhereLike.UpdatedAt + "%");
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
                                quizQuestion = new QuizQuestionModel();
                                if (!reader.IsDBNull(0))
                                    quizQuestion.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestion.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestion.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizQuestion.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestion.QuestionTypeId = reader.GetValue(2).ToString();
                                else
                                    quizQuestion.QuestionTypeId = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestion.Desc = reader.GetValue(3).ToString();
                                else
                                    quizQuestion.Desc = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestion.QuizMatchInstructionId = reader.GetValue(4).ToString();
                                else
                                    quizQuestion.QuizMatchInstructionId = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestion.CustomValue = reader.GetValue(5).ToString();
                                else
                                    quizQuestion.CustomValue = null;
                                if (!reader.IsDBNull(6))
                                    quizQuestion.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    quizQuestion.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    quizQuestion.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    quizQuestion.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    quizQuestion.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    quizQuestion.UpdatedAt = null;
                                response.Add(quizQuestion);
                                quizQuestion = new QuizQuestionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizQuestionModel>();
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
                response = new List<QuizQuestionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionModel> Read(string connectionString, QuizQuestionModel quizQuestion, bool orderByLastDate, int fromRow, int limit, QuizQuestionModel quizQuestionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionModel> response = new List<QuizQuestionModel>();
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
                    "SELECT `id`, `quiz_id`, `question_type_id`, `desc`, `quiz_match_instruction_id`, `custom_value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_questions ";
                {
                    if (quizQuestion != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestion.Id))
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

                            cmd.Parameters.AddWithValue("@id", quizQuestion.Id);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.QuizId))
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

                            cmd.Parameters.AddWithValue("@quizId", quizQuestion.QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.QuestionTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `question_type_id` = @questionTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `question_type_id` = @questionTypeId ");
                            }

                            cmd.Parameters.AddWithValue("@questionTypeId", quizQuestion.QuestionTypeId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", quizQuestion.Desc);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.QuizMatchInstructionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql,
                                    " where `quiz_match_instruction_id` = @quizMatchInstructionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_match_instruction_id` = @quizMatchInstructionId ");
                            }

                            cmd.Parameters.AddWithValue("@quizMatchInstructionId", quizQuestion.QuizMatchInstructionId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.CustomValue))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `custom_value` = @customValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `custom_value` = @customValue ");
                            }

                            cmd.Parameters.AddWithValue("@customValue", quizQuestion.CustomValue);
                        }

                        if (!String.IsNullOrEmpty(quizQuestion.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestion.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizQuestion.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestion.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizQuestion.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestion.UpdatedAt);
                        }
                    }
                    if (quizQuestionWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizQuestionWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + quizQuestionWhereLike.QuizId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.QuestionTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `question_type_id` LIKE @questionTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `question_type_id` LIKE @questionTypeIdWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@questionTypeIdWhereLike",
                                "%" + quizQuestionWhereLike.QuestionTypeId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.Desc))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `desc` LIKE @descWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `desc` LIKE @descWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@descWhereLike", "%" + quizQuestionWhereLike.Desc + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.QuizMatchInstructionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql,
                                    " and `quiz_match_instruction_id` LIKE @quizMatchInstructionIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql,
                                    " where `quiz_match_instruction_id` LIKE @quizMatchInstructionIdWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizMatchInstructionIdWhereLike",
                                "%" + quizQuestionWhereLike.QuizMatchInstructionId + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.CustomValue))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `custom_value` LIKE @customValueWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `custom_value` LIKE @customValueWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customValueWhereLike",
                                "%" + quizQuestionWhereLike.CustomValue + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizQuestionWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizQuestionWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizQuestionWhereLike.UpdatedAt + "%");
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
                                quizQuestion = new QuizQuestionModel();
                                if (!reader.IsDBNull(0))
                                    quizQuestion.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestion.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestion.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizQuestion.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestion.QuestionTypeId = reader.GetValue(2).ToString();
                                else
                                    quizQuestion.QuestionTypeId = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestion.Desc = reader.GetValue(3).ToString();
                                else
                                    quizQuestion.Desc = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestion.QuizMatchInstructionId = reader.GetValue(4).ToString();
                                else
                                    quizQuestion.QuizMatchInstructionId = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestion.CustomValue = reader.GetValue(5).ToString();
                                else
                                    quizQuestion.CustomValue = null;
                                if (!reader.IsDBNull(6))
                                    quizQuestion.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    quizQuestion.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    quizQuestion.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    quizQuestion.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    quizQuestion.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    quizQuestion.UpdatedAt = null;
                                response.Add(quizQuestion);
                                quizQuestion = new QuizQuestionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizQuestionModel>();
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
                response = new List<QuizQuestionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionModel> Read(string connectionString, List<QuizQuestionModel> quizQuestionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionModel> response = new List<QuizQuestionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql =
                    "SELECT `id`, `quiz_id`, `question_type_id`, `desc`, `quiz_match_instruction_id`, `custom_value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_questions ";
                if (quizQuestionWhere != null && quizQuestionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionWhere[i].Id);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizQuestionWhere[i].QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].QuestionTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_type_id` = @questionTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionTypeId" + i, quizQuestionWhere[i].QuestionTypeId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, quizQuestionWhere[i].Desc);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].QuizMatchInstructionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_match_instruction_id` = @quizMatchInstructionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizMatchInstructionId" + i,
                                quizQuestionWhere[i].QuizMatchInstructionId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].CustomValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `custom_value` = @customValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customValue" + i, quizQuestionWhere[i].CustomValue);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionWhere.Count - 1)
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
                            QuizQuestionModel quizQuestion = new QuizQuestionModel();
                            if (!reader.IsDBNull(0))
                                quizQuestion.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestion.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestion.QuizId = reader.GetValue(1).ToString();
                            else
                                quizQuestion.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestion.QuestionTypeId = reader.GetValue(2).ToString();
                            else
                                quizQuestion.QuestionTypeId = null;
                            if (!reader.IsDBNull(3))
                                quizQuestion.Desc = reader.GetValue(3).ToString();
                            else
                                quizQuestion.Desc = null;
                            if (!reader.IsDBNull(4))
                                quizQuestion.QuizMatchInstructionId = reader.GetValue(4).ToString();
                            else
                                quizQuestion.QuizMatchInstructionId = null;
                            if (!reader.IsDBNull(5))
                                quizQuestion.CustomValue = reader.GetValue(5).ToString();
                            else
                                quizQuestion.CustomValue = null;
                            if (!reader.IsDBNull(6))
                                quizQuestion.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                quizQuestion.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                quizQuestion.CreatedAt = reader.GetValue(7).ToString();
                            else
                                quizQuestion.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                quizQuestion.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                quizQuestion.UpdatedAt = null;
                            response.Add(quizQuestion);
                            quizQuestion = new QuizQuestionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizQuestionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }
        public List<QuizQuestionModel> Read(string connectionString, List<QuizQuestionModel> quizQuestionWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionModel> response = new List<QuizQuestionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql =
                    "SELECT `id`, `quiz_id`, `question_type_id`, `desc`, `quiz_match_instruction_id`, `custom_value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_questions ";
                if (quizQuestionWhere != null && quizQuestionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionWhere[i].Id);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizQuestionWhere[i].QuizId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].QuestionTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_type_id` = @questionTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionTypeId" + i, quizQuestionWhere[i].QuestionTypeId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, quizQuestionWhere[i].Desc);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].QuizMatchInstructionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_match_instruction_id` = @quizMatchInstructionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizMatchInstructionId" + i,
                                quizQuestionWhere[i].QuizMatchInstructionId);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].CustomValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `custom_value` = @customValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customValue" + i, quizQuestionWhere[i].CustomValue);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionWhere.Count - 1)
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
                            QuizQuestionModel quizQuestion = new QuizQuestionModel();
                            if (!reader.IsDBNull(0))
                                quizQuestion.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestion.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestion.QuizId = reader.GetValue(1).ToString();
                            else
                                quizQuestion.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestion.QuestionTypeId = reader.GetValue(2).ToString();
                            else
                                quizQuestion.QuestionTypeId = null;
                            if (!reader.IsDBNull(3))
                                quizQuestion.Desc = reader.GetValue(3).ToString();
                            else
                                quizQuestion.Desc = null;
                            if (!reader.IsDBNull(4))
                                quizQuestion.QuizMatchInstructionId = reader.GetValue(4).ToString();
                            else
                                quizQuestion.QuizMatchInstructionId = null;
                            if (!reader.IsDBNull(5))
                                quizQuestion.CustomValue = reader.GetValue(5).ToString();
                            else
                                quizQuestion.CustomValue = null;
                            if (!reader.IsDBNull(6))
                                quizQuestion.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                quizQuestion.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                quizQuestion.CreatedAt = reader.GetValue(7).ToString();
                            else
                                quizQuestion.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                quizQuestion.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                quizQuestion.UpdatedAt = null;
                            response.Add(quizQuestion);
                            quizQuestion = new QuizQuestionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizQuestionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public List<QuizQuestionModel> Create(string connectionString, QuizQuestionModel quizQuestion)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionModel> response = new List<QuizQuestionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO quiz_questions (";
                string values = "(";
                if (!String.IsNullOrEmpty(quizQuestion.Id))
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
                    cmd.Parameters.AddWithValue("@id", quizQuestion.Id);
                }

                if (!String.IsNullOrEmpty(quizQuestion.QuizId))
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
                    cmd.Parameters.AddWithValue("@quizId", quizQuestion.QuizId);
                }

                if (!String.IsNullOrEmpty(quizQuestion.QuestionTypeId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `question_type_id` ");
                        values = String.Concat(values, ", @questionTypeId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `question_type_id` ");
                        values = String.Concat(values, " @questionTypeId");
                    }

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@questionTypeId", quizQuestion.QuestionTypeId);
                }

                if (!String.IsNullOrEmpty(quizQuestion.Desc))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `desc` ");
                        values = String.Concat(values, ", @desc");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `desc` ");
                        values = String.Concat(values, " @desc");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", quizQuestion.Desc);
                }

                if (!String.IsNullOrEmpty(quizQuestion.QuizMatchInstructionId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `quiz_match_instruction_id` ");
                        values = String.Concat(values, ", @quizMatchInstructionId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `quiz_match_instruction_id` ");
                        values = String.Concat(values, " @quizMatchInstructionId");
                    }

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizMatchInstructionId", quizQuestion.QuizMatchInstructionId);
                }

                if (!String.IsNullOrEmpty(quizQuestion.CustomValue))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `custom_value` ");
                        values = String.Concat(values, ", @customValue");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `custom_value` ");
                        values = String.Concat(values, " @customValue");
                    }

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@customValue", quizQuestion.CustomValue);
                }

                if (!String.IsNullOrEmpty(quizQuestion.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", quizQuestion.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(quizQuestion.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", quizQuestion.CreatedAt);
                }
                if (!String.IsNullOrEmpty(quizQuestion.UpdatedAt))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `updated_at` ");
                        values = String.Concat(values, ", @updatedAt");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `updated_at` ");
                        values = String.Concat(values, " @updatedAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", quizQuestion.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                quizQuestion.Id = cmd.LastInsertedId.ToString();
                response.Add(quizQuestion);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionModel>();
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
        public List<QuizQuestionModel> Create(string connectionString, List<QuizQuestionModel> quizQuestionList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionModel> response = new List<QuizQuestionModel>();
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
                        for (int i = 0; i < quizQuestionList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO quiz_questions (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(quizQuestionList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, quizQuestionList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionList[i].QuizId))
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
                                    cmd.Parameters.AddWithValue("@quizId" + i, quizQuestionList[i].QuizId);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionList[i].QuestionTypeId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `question_type_id` ");
                                        values = String.Concat(values, ", @questionTypeId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `question_type_id` ");
                                        values = String.Concat(values, "@questionTypeId" + i);
                                    }

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@questionTypeId" + i,
                                        quizQuestionList[i].QuestionTypeId);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionList[i].Desc))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `desc` ");
                                        values = String.Concat(values, ", @desc" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `desc` ");
                                        values = String.Concat(values, "@desc" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, quizQuestionList[i].Desc);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionList[i].QuizMatchInstructionId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `quiz_match_instruction_id` ");
                                        values = String.Concat(values, ", @quizMatchInstructionId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `quiz_match_instruction_id` ");
                                        values = String.Concat(values, "@quizMatchInstructionId" + i);
                                    }

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizMatchInstructionId" + i,
                                        quizQuestionList[i].QuizMatchInstructionId);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionList[i].CustomValue))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `custom_value` ");
                                        values = String.Concat(values, ", @customValue" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `custom_value` ");
                                        values = String.Concat(values, "@customValue" + i);
                                    }

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@customValue" + i, quizQuestionList[i].CustomValue);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `updated_at` ");
                                        values = String.Concat(values, ", @updatedAt" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `updated_at` ");
                                        values = String.Concat(values, "@updatedAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(quizQuestionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionModel>();
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

        public List<QuizQuestionModel> Update(string connectionString, QuizQuestionModel quizQuestion, QuizQuestionModel quizQuestionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionModel> response = new List<QuizQuestionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE quiz_questions SET ";
                if (!String.IsNullOrEmpty(quizQuestion.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", quizQuestion.Id);
                }
                if (!String.IsNullOrEmpty(quizQuestion.QuizId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `quiz_id` = @quizId");
                    else
                        sql = String.Concat(sql, " `quiz_id` = @quizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", quizQuestion.QuizId);
                }

                if (!String.IsNullOrEmpty(quizQuestion.QuestionTypeId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `question_type_id` = @questionTypeId");
                    else
                        sql = String.Concat(sql, " `question_type_id` = @questionTypeId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@questionTypeId", quizQuestion.QuestionTypeId);
                }

                if (!String.IsNullOrEmpty(quizQuestion.Desc))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `desc` = @desc");
                    else
                        sql = String.Concat(sql, " `desc` = @desc");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", quizQuestion.Desc);
                }

                if (!String.IsNullOrEmpty(quizQuestion.QuizMatchInstructionId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `quiz_match_instruction_id` = @quizMatchInstructionId");
                    else
                        sql = String.Concat(sql, " `quiz_match_instruction_id` = @quizMatchInstructionId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizMatchInstructionId", quizQuestion.QuizMatchInstructionId);
                }

                if (!String.IsNullOrEmpty(quizQuestion.CustomValue))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `custom_value` = @customValue");
                    else
                        sql = String.Concat(sql, " `custom_value` = @customValue");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@customValue", quizQuestion.CustomValue);
                }

                if (!String.IsNullOrEmpty(quizQuestion.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", quizQuestion.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(quizQuestion.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", quizQuestion.CreatedAt);
                }
                if (!String.IsNullOrEmpty(quizQuestion.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", quizQuestion.UpdatedAt);
                }
                if (quizQuestionWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(quizQuestionWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", quizQuestionWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionWhere.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizIdWhere", quizQuestionWhere.QuizId);
                    }

                    if (!String.IsNullOrEmpty(quizQuestionWhere.QuestionTypeId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `question_type_id` = @questionTypeIdWhere");
                        else
                            sql = String.Concat(sql, " `question_type_id` = @questionTypeIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@questionTypeIdWhere", quizQuestionWhere.QuestionTypeId);
                    }

                    if (!String.IsNullOrEmpty(quizQuestionWhere.Desc))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `desc` = @descWhere");
                        else
                            sql = String.Concat(sql, " `desc` = @descWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descWhere", quizQuestionWhere.Desc);
                    }

                    if (!String.IsNullOrEmpty(quizQuestionWhere.QuizMatchInstructionId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_match_instruction_id` = @quizMatchInstructionIdWhere");
                        else
                            sql = String.Concat(sql, " `quiz_match_instruction_id` = @quizMatchInstructionIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizMatchInstructionIdWhere",
                            quizQuestionWhere.QuizMatchInstructionId);
                    }

                    if (!String.IsNullOrEmpty(quizQuestionWhere.CustomValue))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `custom_value` = @customValueWhere");
                        else
                            sql = String.Concat(sql, " `custom_value` = @customValueWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@customValueWhere", quizQuestionWhere.CustomValue);
                    }

                    if (!String.IsNullOrEmpty(quizQuestionWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", quizQuestionWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", quizQuestionWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", quizQuestionWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(quizQuestion);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionModel>();
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

        public List<QuizQuestionModel> Update(string connectionString, List<QuizQuestionModel> quizQuestionList, List<QuizQuestionModel> quizQuestionWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionModel> response = new List<QuizQuestionModel>();
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
                        for (int i = 0; i < quizQuestionList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE quiz_questions SET ";
                                if (!String.IsNullOrEmpty(quizQuestionList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, quizQuestionList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionList[i].QuizId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `quiz_id` = @quizId" + i);
                                    else
                                        sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, quizQuestionList[i].QuizId);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionList[i].QuestionTypeId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `question_type_id` = @questionTypeId" + i);
                                    else
                                        sql = String.Concat(sql, " `question_type_id` = @questionTypeId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@questionTypeId" + i,
                                        quizQuestionList[i].QuestionTypeId);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionList[i].Desc))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `desc` = @desc" + i);
                                    else
                                        sql = String.Concat(sql, " `desc` = @desc" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, quizQuestionList[i].Desc);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionList[i].QuizMatchInstructionId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql,
                                            ", `quiz_match_instruction_id` = @quizMatchInstructionId" + i);
                                    else
                                        sql = String.Concat(sql,
                                            " `quiz_match_instruction_id` = @quizMatchInstructionId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizMatchInstructionId" + i,
                                        quizQuestionList[i].QuizMatchInstructionId);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionList[i].CustomValue))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `custom_value` = @customValue" + i);
                                    else
                                        sql = String.Concat(sql, " `custom_value` = @customValue" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@customValue" + i, quizQuestionList[i].CustomValue);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionList[i].UpdatedAt);
                                }
                                if (quizQuestionWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(quizQuestionWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, quizQuestionWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionWhereList[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizIdWhere" + i, quizQuestionWhereList[i].QuizId);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestionWhereList[i].QuestionTypeId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql,
                                                " AND `question_type_id` = @questionTypeIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `question_type_id` = @questionTypeIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@questionTypeIdWhere" + i,
                                            quizQuestionWhereList[i].QuestionTypeId);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestionWhereList[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `desc` = @descWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `desc` = @descWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descWhere" + i, quizQuestionWhereList[i].Desc);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestionWhereList[i].QuizMatchInstructionId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql,
                                                " AND `quiz_match_instruction_id` = @quizMatchInstructionIdWhere" + i);
                                        else
                                            sql = String.Concat(sql,
                                                " `quiz_match_instruction_id` = @quizMatchInstructionIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizMatchInstructionIdWhere" + i,
                                            quizQuestionWhereList[i].QuizMatchInstructionId);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestionWhereList[i].CustomValue))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `custom_value` = @customValueWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `custom_value` = @customValueWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@customValueWhere" + i,
                                            quizQuestionWhereList[i].CustomValue);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestionWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, quizQuestionWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, quizQuestionWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, quizQuestionWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizQuestionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionModel>();
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

        public List<QuizQuestionModel> Delete(string connectionString, QuizQuestionModel quizQuestion)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionModel> response = new List<QuizQuestionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM quiz_questions  ";
                if (quizQuestion != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(quizQuestion.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", quizQuestion.Id);
                    }
                    if (!String.IsNullOrEmpty(quizQuestion.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizId");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizId", quizQuestion.QuizId);
                    }

                    if (!String.IsNullOrEmpty(quizQuestion.QuestionTypeId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `question_type_id` = @questionTypeId");
                        else
                            sql = String.Concat(sql, " `question_type_id` = @questionTypeId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@questionTypeId", quizQuestion.QuestionTypeId);
                    }

                    if (!String.IsNullOrEmpty(quizQuestion.Desc))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `desc` = @desc");
                        else
                            sql = String.Concat(sql, " `desc` = @desc");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@desc", quizQuestion.Desc);
                    }

                    if (!String.IsNullOrEmpty(quizQuestion.QuizMatchInstructionId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_match_instruction_id` = @quizMatchInstructionId");
                        else
                            sql = String.Concat(sql, " `quiz_match_instruction_id` = @quizMatchInstructionId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizMatchInstructionId", quizQuestion.QuizMatchInstructionId);
                    }

                    if (!String.IsNullOrEmpty(quizQuestion.CustomValue))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `custom_value` = @customValue");
                        else
                            sql = String.Concat(sql, " `custom_value` = @customValue");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@customValue", quizQuestion.CustomValue);
                    }

                    if (!String.IsNullOrEmpty(quizQuestion.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", quizQuestion.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(quizQuestion.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", quizQuestion.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(quizQuestion.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", quizQuestion.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(quizQuestion);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionModel>();
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

        public List<QuizQuestionModel> Delete(string connectionString, List<QuizQuestionModel> quizQuestion)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionModel> response = new List<QuizQuestionModel>();
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
                        for (int i = 0; i < quizQuestion.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM quiz_questions  ";
                                if (quizQuestion[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(quizQuestion[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, quizQuestion[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestion[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizId" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizId" + i, quizQuestion[i].QuizId);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestion[i].QuestionTypeId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `question_type_id` = @questionTypeId" + i);
                                        else
                                            sql = String.Concat(sql, " `question_type_id` = @questionTypeId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@questionTypeId" + i,
                                            quizQuestion[i].QuestionTypeId);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestion[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `desc` = @desc" + i);
                                        else
                                            sql = String.Concat(sql, " `desc` = @desc" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@desc" + i, quizQuestion[i].Desc);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestion[i].QuizMatchInstructionId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql,
                                                " AND `quiz_match_instruction_id` = @quizMatchInstructionId" + i);
                                        else
                                            sql = String.Concat(sql,
                                                " `quiz_match_instruction_id` = @quizMatchInstructionId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizMatchInstructionId" + i,
                                            quizQuestion[i].QuizMatchInstructionId);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestion[i].CustomValue))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `custom_value` = @customValue" + i);
                                        else
                                            sql = String.Concat(sql, " `custom_value` = @customValue" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@customValue" + i, quizQuestion[i].CustomValue);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestion[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestion[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestion[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestion[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestion[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestion[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizQuestion[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionModel>();
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
