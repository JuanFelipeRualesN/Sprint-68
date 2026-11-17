using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.LearningGap;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.LearningGap
{
    class LearningGapDiagnosticTestResultResponseDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, LearningGapDiagnosticTestResultResponseModel> ReadD(string connectionString, LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponse, bool firstOne, bool orderByLastDate, LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponseWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapDiagnosticTestResultResponseModel> response = new Dictionary<string, LearningGapDiagnosticTestResultResponseModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `learning_gap_diagnostic_test_result_id`, `question_id`, `response_id`, `correct`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_result_responses ";
                {
                    if (learningGapDiagnosticTestResultResponse != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapDiagnosticTestResultResponse.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId ");
                            }
                            cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultId", learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.QuestionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `question_id` = @questionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `question_id` = @questionId ");
                            }
                            cmd.Parameters.AddWithValue("@questionId", learningGapDiagnosticTestResultResponse.QuestionId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.ResponseId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `response_id` = @responseId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `response_id` = @responseId ");
                            }
                            cmd.Parameters.AddWithValue("@responseId", learningGapDiagnosticTestResultResponse.ResponseId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.Correct))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `correct` = @correct ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `correct` = @correct ");
                            }
                            cmd.Parameters.AddWithValue("@correct", learningGapDiagnosticTestResultResponse.Correct);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticTestResultResponse.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticTestResultResponse.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticTestResultResponse.UpdatedAt);
                        }
                    }
                    if (learningGapDiagnosticTestResultResponseWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.LearningGapDiagnosticTestResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `learning_gap_diagnostic_test_result_id` LIKE @learningGapDiagnosticTestResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `learning_gap_diagnostic_test_result_id` LIKE @learningGapDiagnosticTestResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultIdWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.LearningGapDiagnosticTestResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.QuestionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `question_id` LIKE @questionIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `question_id` LIKE @questionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@questionIdWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.QuestionId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.ResponseId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `response_id` LIKE @responseIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `response_id` LIKE @responseIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@responseIdWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.ResponseId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.Correct))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `correct` LIKE @correctWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `correct` LIKE @correctWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@correctWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.Correct + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.UpdatedAt + "%");
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
                                learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel();
                                if (!reader.IsDBNull(0))
                                    learningGapDiagnosticTestResultResponse.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = reader.GetValue(1).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapDiagnosticTestResultResponse.QuestionId = reader.GetValue(2).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.QuestionId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapDiagnosticTestResultResponse.ResponseId = reader.GetValue(3).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.ResponseId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapDiagnosticTestResultResponse.Correct = reader.GetValue(4).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.Correct = null;
                                if (!reader.IsDBNull(5))
                                    learningGapDiagnosticTestResultResponse.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    learningGapDiagnosticTestResultResponse.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    learningGapDiagnosticTestResultResponse.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.UpdatedAt = null;
                                response.Add(learningGapDiagnosticTestResultResponse.Id, learningGapDiagnosticTestResultResponse);
                                learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapDiagnosticTestResultResponseModel>();
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
                response = new Dictionary<string, LearningGapDiagnosticTestResultResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapDiagnosticTestResultResponseModel> ReadD(string connectionString, LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponse, bool orderByLastDate, int fromRow, int limit, LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponseWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapDiagnosticTestResultResponseModel> response = new Dictionary<string, LearningGapDiagnosticTestResultResponseModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `learning_gap_diagnostic_test_result_id`, `question_id`, `response_id`, `correct`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_result_responses ";
                {
                    if (learningGapDiagnosticTestResultResponse != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapDiagnosticTestResultResponse.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId ");
                            }
                            cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultId", learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.QuestionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `question_id` = @questionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `question_id` = @questionId ");
                            }
                            cmd.Parameters.AddWithValue("@questionId", learningGapDiagnosticTestResultResponse.QuestionId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.ResponseId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `response_id` = @responseId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `response_id` = @responseId ");
                            }
                            cmd.Parameters.AddWithValue("@responseId", learningGapDiagnosticTestResultResponse.ResponseId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.Correct))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `correct` = @correct ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `correct` = @correct ");
                            }
                            cmd.Parameters.AddWithValue("@correct", learningGapDiagnosticTestResultResponse.Correct);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticTestResultResponse.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticTestResultResponse.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticTestResultResponse.UpdatedAt);
                        }
                    }
                    if (learningGapDiagnosticTestResultResponseWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.LearningGapDiagnosticTestResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `learning_gap_diagnostic_test_result_id` LIKE @learningGapDiagnosticTestResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `learning_gap_diagnostic_test_result_id` LIKE @learningGapDiagnosticTestResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultIdWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.LearningGapDiagnosticTestResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.QuestionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `question_id` LIKE @questionIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `question_id` LIKE @questionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@questionIdWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.QuestionId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.ResponseId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `response_id` LIKE @responseIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `response_id` LIKE @responseIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@responseIdWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.ResponseId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.Correct))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `correct` LIKE @correctWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `correct` LIKE @correctWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@correctWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.Correct + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.UpdatedAt + "%");
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
                                learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel();
                                if (!reader.IsDBNull(0))
                                    learningGapDiagnosticTestResultResponse.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = reader.GetValue(1).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapDiagnosticTestResultResponse.QuestionId = reader.GetValue(2).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.QuestionId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapDiagnosticTestResultResponse.ResponseId = reader.GetValue(3).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.ResponseId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapDiagnosticTestResultResponse.Correct = reader.GetValue(4).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.Correct = null;
                                if (!reader.IsDBNull(5))
                                    learningGapDiagnosticTestResultResponse.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    learningGapDiagnosticTestResultResponse.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    learningGapDiagnosticTestResultResponse.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.UpdatedAt = null;
                                response.Add(learningGapDiagnosticTestResultResponse.Id, learningGapDiagnosticTestResultResponse);
                                learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapDiagnosticTestResultResponseModel>();
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
                response = new Dictionary<string, LearningGapDiagnosticTestResultResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapDiagnosticTestResultResponseModel> ReadD(string connectionString, List<LearningGapDiagnosticTestResultResponseModel> learningGapDiagnosticTestResultResponseWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapDiagnosticTestResultResponseModel> response = new Dictionary<string, LearningGapDiagnosticTestResultResponseModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `learning_gap_diagnostic_test_result_id`, `question_id`, `response_id`, `correct`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_result_responses ";
                if (learningGapDiagnosticTestResultResponseWhere != null && learningGapDiagnosticTestResultResponseWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapDiagnosticTestResultResponseWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticTestResultResponseWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].LearningGapDiagnosticTestResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultId" + i, learningGapDiagnosticTestResultResponseWhere[i].LearningGapDiagnosticTestResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].QuestionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_id` = @questionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionId" + i, learningGapDiagnosticTestResultResponseWhere[i].QuestionId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].ResponseId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_id` = @responseId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseId" + i, learningGapDiagnosticTestResultResponseWhere[i].ResponseId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].Correct))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `correct` = @correct" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@correct" + i, learningGapDiagnosticTestResultResponseWhere[i].Correct);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticTestResultResponseWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticTestResultResponseWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticTestResultResponseWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapDiagnosticTestResultResponseWhere.Count - 1)
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
                            LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel(); if (!reader.IsDBNull(0))
                                learningGapDiagnosticTestResultResponse.Id = reader.GetValue(0).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = reader.GetValue(1).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = null;
                            if (!reader.IsDBNull(2))
                                learningGapDiagnosticTestResultResponse.QuestionId = reader.GetValue(2).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.QuestionId = null;
                            if (!reader.IsDBNull(3))
                                learningGapDiagnosticTestResultResponse.ResponseId = reader.GetValue(3).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.ResponseId = null;
                            if (!reader.IsDBNull(4))
                                learningGapDiagnosticTestResultResponse.Correct = reader.GetValue(4).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.Correct = null;
                            if (!reader.IsDBNull(5))
                                learningGapDiagnosticTestResultResponse.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                learningGapDiagnosticTestResultResponse.CreatedAt = reader.GetValue(6).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                learningGapDiagnosticTestResultResponse.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.UpdatedAt = null;
                            response.Add(learningGapDiagnosticTestResultResponse.Id, learningGapDiagnosticTestResultResponse);
                            learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapDiagnosticTestResultResponseModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapDiagnosticTestResultResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapDiagnosticTestResultResponseModel> ReadD(string connectionString, List<LearningGapDiagnosticTestResultResponseModel> learningGapDiagnosticTestResultResponseWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapDiagnosticTestResultResponseModel> response = new Dictionary<string, LearningGapDiagnosticTestResultResponseModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `learning_gap_diagnostic_test_result_id`, `question_id`, `response_id`, `correct`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_result_responses ";
                if (learningGapDiagnosticTestResultResponseWhere != null && learningGapDiagnosticTestResultResponseWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapDiagnosticTestResultResponseWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticTestResultResponseWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].LearningGapDiagnosticTestResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultId" + i, learningGapDiagnosticTestResultResponseWhere[i].LearningGapDiagnosticTestResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].QuestionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_id` = @questionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionId" + i, learningGapDiagnosticTestResultResponseWhere[i].QuestionId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].ResponseId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_id` = @responseId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseId" + i, learningGapDiagnosticTestResultResponseWhere[i].ResponseId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].Correct))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `correct` = @correct" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@correct" + i, learningGapDiagnosticTestResultResponseWhere[i].Correct);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticTestResultResponseWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticTestResultResponseWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticTestResultResponseWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapDiagnosticTestResultResponseWhere.Count - 1)
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
                            LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel(); if (!reader.IsDBNull(0))
                                learningGapDiagnosticTestResultResponse.Id = reader.GetValue(0).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = reader.GetValue(1).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = null;
                            if (!reader.IsDBNull(2))
                                learningGapDiagnosticTestResultResponse.QuestionId = reader.GetValue(2).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.QuestionId = null;
                            if (!reader.IsDBNull(3))
                                learningGapDiagnosticTestResultResponse.ResponseId = reader.GetValue(3).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.ResponseId = null;
                            if (!reader.IsDBNull(4))
                                learningGapDiagnosticTestResultResponse.Correct = reader.GetValue(4).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.Correct = null;
                            if (!reader.IsDBNull(5))
                                learningGapDiagnosticTestResultResponse.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                learningGapDiagnosticTestResultResponse.CreatedAt = reader.GetValue(6).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                learningGapDiagnosticTestResultResponse.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.UpdatedAt = null;
                            response.Add(learningGapDiagnosticTestResultResponse.Id, learningGapDiagnosticTestResultResponse);
                            learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapDiagnosticTestResultResponseModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapDiagnosticTestResultResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapDiagnosticTestResultResponseModel> Read(string connectionString, LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponse, bool firstOne, bool orderByLastDate, LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponseWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultResponseModel> response = new List<LearningGapDiagnosticTestResultResponseModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `learning_gap_diagnostic_test_result_id`, `question_id`, `response_id`, `correct`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_result_responses ";
                {
                    if (learningGapDiagnosticTestResultResponse != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapDiagnosticTestResultResponse.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId ");
                            }
                            cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultId", learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.QuestionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `question_id` = @questionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `question_id` = @questionId ");
                            }
                            cmd.Parameters.AddWithValue("@questionId", learningGapDiagnosticTestResultResponse.QuestionId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.ResponseId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `response_id` = @responseId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `response_id` = @responseId ");
                            }
                            cmd.Parameters.AddWithValue("@responseId", learningGapDiagnosticTestResultResponse.ResponseId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.Correct))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `correct` = @correct ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `correct` = @correct ");
                            }
                            cmd.Parameters.AddWithValue("@correct", learningGapDiagnosticTestResultResponse.Correct);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticTestResultResponse.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticTestResultResponse.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticTestResultResponse.UpdatedAt);
                        }
                    }
                    if (learningGapDiagnosticTestResultResponseWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.LearningGapDiagnosticTestResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `learning_gap_diagnostic_test_result_id` LIKE @learningGapDiagnosticTestResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `learning_gap_diagnostic_test_result_id` LIKE @learningGapDiagnosticTestResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultIdWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.LearningGapDiagnosticTestResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.QuestionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `question_id` LIKE @questionIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `question_id` LIKE @questionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@questionIdWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.QuestionId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.ResponseId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `response_id` LIKE @responseIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `response_id` LIKE @responseIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@responseIdWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.ResponseId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.Correct))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `correct` LIKE @correctWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `correct` LIKE @correctWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@correctWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.Correct + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.UpdatedAt + "%");
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
                                learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel();
                                if (!reader.IsDBNull(0))
                                    learningGapDiagnosticTestResultResponse.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = reader.GetValue(1).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapDiagnosticTestResultResponse.QuestionId = reader.GetValue(2).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.QuestionId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapDiagnosticTestResultResponse.ResponseId = reader.GetValue(3).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.ResponseId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapDiagnosticTestResultResponse.Correct = reader.GetValue(4).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.Correct = null;
                                if (!reader.IsDBNull(5))
                                    learningGapDiagnosticTestResultResponse.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    learningGapDiagnosticTestResultResponse.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    learningGapDiagnosticTestResultResponse.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.UpdatedAt = null;
                                response.Add(learningGapDiagnosticTestResultResponse);
                                learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapDiagnosticTestResultResponseModel>();
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
                response = new List<LearningGapDiagnosticTestResultResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapDiagnosticTestResultResponseModel> Read(string connectionString, LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponse, bool orderByLastDate, int fromRow, int limit, LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponseWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultResponseModel> response = new List<LearningGapDiagnosticTestResultResponseModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `learning_gap_diagnostic_test_result_id`, `question_id`, `response_id`, `correct`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_result_responses ";
                {
                    if (learningGapDiagnosticTestResultResponse != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapDiagnosticTestResultResponse.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId ");
                            }
                            cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultId", learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.QuestionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `question_id` = @questionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `question_id` = @questionId ");
                            }
                            cmd.Parameters.AddWithValue("@questionId", learningGapDiagnosticTestResultResponse.QuestionId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.ResponseId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `response_id` = @responseId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `response_id` = @responseId ");
                            }
                            cmd.Parameters.AddWithValue("@responseId", learningGapDiagnosticTestResultResponse.ResponseId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.Correct))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `correct` = @correct ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `correct` = @correct ");
                            }
                            cmd.Parameters.AddWithValue("@correct", learningGapDiagnosticTestResultResponse.Correct);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticTestResultResponse.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticTestResultResponse.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticTestResultResponse.UpdatedAt);
                        }
                    }
                    if (learningGapDiagnosticTestResultResponseWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.LearningGapDiagnosticTestResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `learning_gap_diagnostic_test_result_id` LIKE @learningGapDiagnosticTestResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `learning_gap_diagnostic_test_result_id` LIKE @learningGapDiagnosticTestResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultIdWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.LearningGapDiagnosticTestResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.QuestionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `question_id` LIKE @questionIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `question_id` LIKE @questionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@questionIdWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.QuestionId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.ResponseId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `response_id` LIKE @responseIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `response_id` LIKE @responseIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@responseIdWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.ResponseId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.Correct))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `correct` LIKE @correctWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `correct` LIKE @correctWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@correctWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.Correct + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapDiagnosticTestResultResponseWhereLike.UpdatedAt + "%");
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
                                learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel();
                                if (!reader.IsDBNull(0))
                                    learningGapDiagnosticTestResultResponse.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = reader.GetValue(1).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapDiagnosticTestResultResponse.QuestionId = reader.GetValue(2).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.QuestionId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapDiagnosticTestResultResponse.ResponseId = reader.GetValue(3).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.ResponseId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapDiagnosticTestResultResponse.Correct = reader.GetValue(4).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.Correct = null;
                                if (!reader.IsDBNull(5))
                                    learningGapDiagnosticTestResultResponse.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    learningGapDiagnosticTestResultResponse.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    learningGapDiagnosticTestResultResponse.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapDiagnosticTestResultResponse.UpdatedAt = null;
                                response.Add(learningGapDiagnosticTestResultResponse);
                                learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapDiagnosticTestResultResponseModel>();
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
                response = new List<LearningGapDiagnosticTestResultResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapDiagnosticTestResultResponseModel> Read(string connectionString, List<LearningGapDiagnosticTestResultResponseModel> learningGapDiagnosticTestResultResponseWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultResponseModel> response = new List<LearningGapDiagnosticTestResultResponseModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `learning_gap_diagnostic_test_result_id`, `question_id`, `response_id`, `correct`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_result_responses ";
                if (learningGapDiagnosticTestResultResponseWhere != null && learningGapDiagnosticTestResultResponseWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapDiagnosticTestResultResponseWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticTestResultResponseWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].LearningGapDiagnosticTestResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultId" + i, learningGapDiagnosticTestResultResponseWhere[i].LearningGapDiagnosticTestResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].QuestionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_id` = @questionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionId" + i, learningGapDiagnosticTestResultResponseWhere[i].QuestionId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].ResponseId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_id` = @responseId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseId" + i, learningGapDiagnosticTestResultResponseWhere[i].ResponseId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].Correct))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `correct` = @correct" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@correct" + i, learningGapDiagnosticTestResultResponseWhere[i].Correct);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticTestResultResponseWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticTestResultResponseWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticTestResultResponseWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapDiagnosticTestResultResponseWhere.Count - 1)
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
                            LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel(); if (!reader.IsDBNull(0))
                                learningGapDiagnosticTestResultResponse.Id = reader.GetValue(0).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = reader.GetValue(1).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = null;
                            if (!reader.IsDBNull(2))
                                learningGapDiagnosticTestResultResponse.QuestionId = reader.GetValue(2).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.QuestionId = null;
                            if (!reader.IsDBNull(3))
                                learningGapDiagnosticTestResultResponse.ResponseId = reader.GetValue(3).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.ResponseId = null;
                            if (!reader.IsDBNull(4))
                                learningGapDiagnosticTestResultResponse.Correct = reader.GetValue(4).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.Correct = null;
                            if (!reader.IsDBNull(5))
                                learningGapDiagnosticTestResultResponse.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                learningGapDiagnosticTestResultResponse.CreatedAt = reader.GetValue(6).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                learningGapDiagnosticTestResultResponse.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.UpdatedAt = null;
                            response.Add(learningGapDiagnosticTestResultResponse);
                            learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapDiagnosticTestResultResponseModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapDiagnosticTestResultResponseModel> Read(string connectionString, List<LearningGapDiagnosticTestResultResponseModel> learningGapDiagnosticTestResultResponseWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultResponseModel> response = new List<LearningGapDiagnosticTestResultResponseModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `learning_gap_diagnostic_test_result_id`, `question_id`, `response_id`, `correct`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_result_responses ";
                if (learningGapDiagnosticTestResultResponseWhere != null && learningGapDiagnosticTestResultResponseWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapDiagnosticTestResultResponseWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticTestResultResponseWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].LearningGapDiagnosticTestResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultId" + i, learningGapDiagnosticTestResultResponseWhere[i].LearningGapDiagnosticTestResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].QuestionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_id` = @questionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionId" + i, learningGapDiagnosticTestResultResponseWhere[i].QuestionId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].ResponseId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_id` = @responseId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseId" + i, learningGapDiagnosticTestResultResponseWhere[i].ResponseId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].Correct))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `correct` = @correct" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@correct" + i, learningGapDiagnosticTestResultResponseWhere[i].Correct);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticTestResultResponseWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticTestResultResponseWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticTestResultResponseWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapDiagnosticTestResultResponseWhere.Count - 1)
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
                            LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel(); if (!reader.IsDBNull(0))
                                learningGapDiagnosticTestResultResponse.Id = reader.GetValue(0).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = reader.GetValue(1).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId = null;
                            if (!reader.IsDBNull(2))
                                learningGapDiagnosticTestResultResponse.QuestionId = reader.GetValue(2).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.QuestionId = null;
                            if (!reader.IsDBNull(3))
                                learningGapDiagnosticTestResultResponse.ResponseId = reader.GetValue(3).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.ResponseId = null;
                            if (!reader.IsDBNull(4))
                                learningGapDiagnosticTestResultResponse.Correct = reader.GetValue(4).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.Correct = null;
                            if (!reader.IsDBNull(5))
                                learningGapDiagnosticTestResultResponse.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                learningGapDiagnosticTestResultResponse.CreatedAt = reader.GetValue(6).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                learningGapDiagnosticTestResultResponse.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapDiagnosticTestResultResponse.UpdatedAt = null;
                            response.Add(learningGapDiagnosticTestResultResponse);
                            learningGapDiagnosticTestResultResponse = new LearningGapDiagnosticTestResultResponseModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapDiagnosticTestResultResponseModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapDiagnosticTestResultResponseModel> Create(string connectionString, LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponse)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultResponseModel> response = new List<LearningGapDiagnosticTestResultResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO learning_gap_diagnostic_test_result_responses (";
                string values = "(";
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.Id))
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
                    cmd.Parameters.AddWithValue("@id", learningGapDiagnosticTestResultResponse.Id);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `learning_gap_diagnostic_test_result_id` ");
                        values = String.Concat(values, ", @learningGapDiagnosticTestResultId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `learning_gap_diagnostic_test_result_id` ");
                        values = String.Concat(values, " @learningGapDiagnosticTestResultId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultId", learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.QuestionId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `question_id` ");
                        values = String.Concat(values, ", @questionId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `question_id` ");
                        values = String.Concat(values, " @questionId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@questionId", learningGapDiagnosticTestResultResponse.QuestionId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.ResponseId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `response_id` ");
                        values = String.Concat(values, ", @responseId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `response_id` ");
                        values = String.Concat(values, " @responseId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@responseId", learningGapDiagnosticTestResultResponse.ResponseId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.Correct))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `correct` ");
                        values = String.Concat(values, ", @correct");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `correct` ");
                        values = String.Concat(values, " @correct");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@correct", learningGapDiagnosticTestResultResponse.Correct);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticTestResultResponse.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticTestResultResponse.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticTestResultResponse.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                learningGapDiagnosticTestResultResponse.Id = cmd.LastInsertedId.ToString();
                response.Add(learningGapDiagnosticTestResultResponse);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultResponseModel>();
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
        public List<LearningGapDiagnosticTestResultResponseModel> Create(string connectionString, List<LearningGapDiagnosticTestResultResponseModel> learningGapDiagnosticTestResultResponseList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultResponseModel> response = new List<LearningGapDiagnosticTestResultResponseModel>();
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
                        for (int i = 0; i < learningGapDiagnosticTestResultResponseList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO learning_gap_diagnostic_test_result_responses (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticTestResultResponseList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].LearningGapDiagnosticTestResultId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `learning_gap_diagnostic_test_result_id` ");
                                        values = String.Concat(values, ", @learningGapDiagnosticTestResultId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `learning_gap_diagnostic_test_result_id` ");
                                        values = String.Concat(values, "@learningGapDiagnosticTestResultId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultId" + i, learningGapDiagnosticTestResultResponseList[i].LearningGapDiagnosticTestResultId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].QuestionId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `question_id` ");
                                        values = String.Concat(values, ", @questionId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `question_id` ");
                                        values = String.Concat(values, "@questionId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@questionId" + i, learningGapDiagnosticTestResultResponseList[i].QuestionId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].ResponseId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `response_id` ");
                                        values = String.Concat(values, ", @responseId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `response_id` ");
                                        values = String.Concat(values, "@responseId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@responseId" + i, learningGapDiagnosticTestResultResponseList[i].ResponseId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].Correct))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `correct` ");
                                        values = String.Concat(values, ", @correct" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `correct` ");
                                        values = String.Concat(values, "@correct" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@correct" + i, learningGapDiagnosticTestResultResponseList[i].Correct);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticTestResultResponseList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticTestResultResponseList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticTestResultResponseList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(learningGapDiagnosticTestResultResponseList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapDiagnosticTestResultResponseModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapDiagnosticTestResultResponseModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultResponseModel>();
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

        public List<LearningGapDiagnosticTestResultResponseModel> Update(string connectionString, LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponse, LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponseWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultResponseModel> response = new List<LearningGapDiagnosticTestResultResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE learning_gap_diagnostic_test_result_responses SET ";
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", learningGapDiagnosticTestResultResponse.Id);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId");
                    else
                        sql = String.Concat(sql, " `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultId", learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.QuestionId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `question_id` = @questionId");
                    else
                        sql = String.Concat(sql, " `question_id` = @questionId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@questionId", learningGapDiagnosticTestResultResponse.QuestionId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.ResponseId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `response_id` = @responseId");
                    else
                        sql = String.Concat(sql, " `response_id` = @responseId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@responseId", learningGapDiagnosticTestResultResponse.ResponseId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.Correct))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `correct` = @correct");
                    else
                        sql = String.Concat(sql, " `correct` = @correct");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@correct", learningGapDiagnosticTestResultResponse.Correct);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticTestResultResponse.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticTestResultResponse.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticTestResultResponse.UpdatedAt);
                }
                if (learningGapDiagnosticTestResultResponseWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", learningGapDiagnosticTestResultResponseWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere.LearningGapDiagnosticTestResultId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultIdWhere");
                        else
                            sql = String.Concat(sql, " `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultIdWhere", learningGapDiagnosticTestResultResponseWhere.LearningGapDiagnosticTestResultId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere.QuestionId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `question_id` = @questionIdWhere");
                        else
                            sql = String.Concat(sql, " `question_id` = @questionIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@questionIdWhere", learningGapDiagnosticTestResultResponseWhere.QuestionId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere.ResponseId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `response_id` = @responseIdWhere");
                        else
                            sql = String.Concat(sql, " `response_id` = @responseIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@responseIdWhere", learningGapDiagnosticTestResultResponseWhere.ResponseId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere.Correct))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `correct` = @correctWhere");
                        else
                            sql = String.Concat(sql, " `correct` = @correctWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@correctWhere", learningGapDiagnosticTestResultResponseWhere.Correct);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", learningGapDiagnosticTestResultResponseWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", learningGapDiagnosticTestResultResponseWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", learningGapDiagnosticTestResultResponseWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(learningGapDiagnosticTestResultResponse);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultResponseModel>();
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

        public List<LearningGapDiagnosticTestResultResponseModel> Update(string connectionString, List<LearningGapDiagnosticTestResultResponseModel> learningGapDiagnosticTestResultResponseList, List<LearningGapDiagnosticTestResultResponseModel> learningGapDiagnosticTestResultResponseWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultResponseModel> response = new List<LearningGapDiagnosticTestResultResponseModel>();
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
                        for (int i = 0; i < learningGapDiagnosticTestResultResponseList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE learning_gap_diagnostic_test_result_responses SET ";
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticTestResultResponseList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].LearningGapDiagnosticTestResultId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId" + i);
                                    else
                                        sql = String.Concat(sql, " `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultId" + i, learningGapDiagnosticTestResultResponseList[i].LearningGapDiagnosticTestResultId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].QuestionId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `question_id` = @questionId" + i);
                                    else
                                        sql = String.Concat(sql, " `question_id` = @questionId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@questionId" + i, learningGapDiagnosticTestResultResponseList[i].QuestionId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].ResponseId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `response_id` = @responseId" + i);
                                    else
                                        sql = String.Concat(sql, " `response_id` = @responseId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@responseId" + i, learningGapDiagnosticTestResultResponseList[i].ResponseId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].Correct))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `correct` = @correct" + i);
                                    else
                                        sql = String.Concat(sql, " `correct` = @correct" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@correct" + i, learningGapDiagnosticTestResultResponseList[i].Correct);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticTestResultResponseList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticTestResultResponseList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticTestResultResponseList[i].UpdatedAt);
                                }
                                if (learningGapDiagnosticTestResultResponseWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, learningGapDiagnosticTestResultResponseWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereList[i].LearningGapDiagnosticTestResultId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultIdWhere" + i, learningGapDiagnosticTestResultResponseWhereList[i].LearningGapDiagnosticTestResultId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereList[i].QuestionId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `question_id` = @questionIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `question_id` = @questionIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@questionIdWhere" + i, learningGapDiagnosticTestResultResponseWhereList[i].QuestionId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereList[i].ResponseId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `response_id` = @responseIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `response_id` = @responseIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@responseIdWhere" + i, learningGapDiagnosticTestResultResponseWhereList[i].ResponseId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereList[i].Correct))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `correct` = @correctWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `correct` = @correctWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@correctWhere" + i, learningGapDiagnosticTestResultResponseWhereList[i].Correct);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, learningGapDiagnosticTestResultResponseWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, learningGapDiagnosticTestResultResponseWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponseWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, learningGapDiagnosticTestResultResponseWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapDiagnosticTestResultResponseList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapDiagnosticTestResultResponseModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapDiagnosticTestResultResponseModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultResponseModel>();
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

        public List<LearningGapDiagnosticTestResultResponseModel> Delete(string connectionString, LearningGapDiagnosticTestResultResponseModel learningGapDiagnosticTestResultResponse)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultResponseModel> response = new List<LearningGapDiagnosticTestResultResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM learning_gap_diagnostic_test_result_responses  ";
                if (learningGapDiagnosticTestResultResponse != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", learningGapDiagnosticTestResultResponse.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId");
                        else
                            sql = String.Concat(sql, " `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultId", learningGapDiagnosticTestResultResponse.LearningGapDiagnosticTestResultId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.QuestionId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `question_id` = @questionId");
                        else
                            sql = String.Concat(sql, " `question_id` = @questionId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@questionId", learningGapDiagnosticTestResultResponse.QuestionId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.ResponseId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `response_id` = @responseId");
                        else
                            sql = String.Concat(sql, " `response_id` = @responseId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@responseId", learningGapDiagnosticTestResultResponse.ResponseId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.Correct))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `correct` = @correct");
                        else
                            sql = String.Concat(sql, " `correct` = @correct");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@correct", learningGapDiagnosticTestResultResponse.Correct);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticTestResultResponse.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticTestResultResponse.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticTestResultResponse.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(learningGapDiagnosticTestResultResponse);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultResponseModel>();
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

        public List<LearningGapDiagnosticTestResultResponseModel> Delete(string connectionString, List<LearningGapDiagnosticTestResultResponseModel> learningGapDiagnosticTestResultResponse)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultResponseModel> response = new List<LearningGapDiagnosticTestResultResponseModel>();
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
                        for (int i = 0; i < learningGapDiagnosticTestResultResponse.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM learning_gap_diagnostic_test_result_responses  ";
                                if (learningGapDiagnosticTestResultResponse[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticTestResultResponse[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse[i].LearningGapDiagnosticTestResultId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId" + i);
                                        else
                                            sql = String.Concat(sql, " `learning_gap_diagnostic_test_result_id` = @learningGapDiagnosticTestResultId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@learningGapDiagnosticTestResultId" + i, learningGapDiagnosticTestResultResponse[i].LearningGapDiagnosticTestResultId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse[i].QuestionId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `question_id` = @questionId" + i);
                                        else
                                            sql = String.Concat(sql, " `question_id` = @questionId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@questionId" + i, learningGapDiagnosticTestResultResponse[i].QuestionId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse[i].ResponseId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `response_id` = @responseId" + i);
                                        else
                                            sql = String.Concat(sql, " `response_id` = @responseId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@responseId" + i, learningGapDiagnosticTestResultResponse[i].ResponseId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse[i].Correct))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `correct` = @correct" + i);
                                        else
                                            sql = String.Concat(sql, " `correct` = @correct" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@correct" + i, learningGapDiagnosticTestResultResponse[i].Correct);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticTestResultResponse[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticTestResultResponse[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultResponse[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticTestResultResponse[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapDiagnosticTestResultResponse[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapDiagnosticTestResultResponseModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapDiagnosticTestResultResponseModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultResponseModel>();
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
