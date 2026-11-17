using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.LearningGap;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.LearningGap
{
    class LearningGapLessonQuizResultDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, LearningGapLessonQuizResultModel> ReadD(string connectionString, LearningGapLessonQuizResultModel learningGapLessonQuizResult, bool firstOne, bool orderByLastDate, LearningGapLessonQuizResultModel learningGapLessonQuizResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapLessonQuizResultModel> response = new Dictionary<string, LearningGapLessonQuizResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `learning_gap_assigned_lesson_id`, `quiz_id`, `gsweb_usqui_results_id`, `score`, `percentage`, `actual_try`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_lesson_quiz_results ";
                {
                    if (learningGapLessonQuizResult != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapLessonQuizResult.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.LearningGapAssignedLessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId ");
                            }
                            cmd.Parameters.AddWithValue("@learningGapAssignedLessonId", learningGapLessonQuizResult.LearningGapAssignedLessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", learningGapLessonQuizResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.GswebUsquiResultsId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `gsweb_usqui_results_id` = @gswebUsquiResultsId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `gsweb_usqui_results_id` = @gswebUsquiResultsId ");
                            }
                            cmd.Parameters.AddWithValue("@gswebUsquiResultsId", learningGapLessonQuizResult.GswebUsquiResultsId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Score))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `score` = @score ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `score` = @score ");
                            }
                            cmd.Parameters.AddWithValue("@score", learningGapLessonQuizResult.Score);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Percentage))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `percentage` = @percentage ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `percentage` = @percentage ");
                            }
                            cmd.Parameters.AddWithValue("@percentage", learningGapLessonQuizResult.Percentage);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.ActualTry))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `actual_try` = @actualTry ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `actual_try` = @actualTry ");
                            }
                            cmd.Parameters.AddWithValue("@actualTry", learningGapLessonQuizResult.ActualTry);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapLessonQuizResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapLessonQuizResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapLessonQuizResult.UpdatedAt);
                        }
                    }
                    if (learningGapLessonQuizResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapLessonQuizResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.LearningGapAssignedLessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `learning_gap_assigned_lesson_id` LIKE @learningGapAssignedLessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `learning_gap_assigned_lesson_id` LIKE @learningGapAssignedLessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@learningGapAssignedLessonIdWhereLike", "%" + learningGapLessonQuizResultWhereLike.LearningGapAssignedLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + learningGapLessonQuizResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.GswebUsquiResultsId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `gsweb_usqui_results_id` LIKE @gswebUsquiResultsIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `gsweb_usqui_results_id` LIKE @gswebUsquiResultsIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@gswebUsquiResultsIdWhereLike", "%" + learningGapLessonQuizResultWhereLike.GswebUsquiResultsId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.Score))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `score` LIKE @scoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `score` LIKE @scoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + learningGapLessonQuizResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.Percentage))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `percentage` LIKE @percentageWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `percentage` LIKE @percentageWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@percentageWhereLike", "%" + learningGapLessonQuizResultWhereLike.Percentage + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.ActualTry))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `actual_try` LIKE @actualTryWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `actual_try` LIKE @actualTryWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@actualTryWhereLike", "%" + learningGapLessonQuizResultWhereLike.ActualTry + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapLessonQuizResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapLessonQuizResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapLessonQuizResultWhereLike.UpdatedAt + "%");
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
                                learningGapLessonQuizResult = new LearningGapLessonQuizResultModel();
                                if (!reader.IsDBNull(0))
                                    learningGapLessonQuizResult.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapLessonQuizResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapLessonQuizResult.LearningGapAssignedLessonId = reader.GetValue(1).ToString();
                                else
                                    learningGapLessonQuizResult.LearningGapAssignedLessonId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapLessonQuizResult.QuizId = reader.GetValue(2).ToString();
                                else
                                    learningGapLessonQuizResult.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapLessonQuizResult.GswebUsquiResultsId = reader.GetValue(3).ToString();
                                else
                                    learningGapLessonQuizResult.GswebUsquiResultsId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapLessonQuizResult.Score = reader.GetValue(4).ToString();
                                else
                                    learningGapLessonQuizResult.Score = null;
                                if (!reader.IsDBNull(5))
                                    learningGapLessonQuizResult.Percentage = reader.GetValue(5).ToString();
                                else
                                    learningGapLessonQuizResult.Percentage = null;
                                if (!reader.IsDBNull(6))
                                    learningGapLessonQuizResult.ActualTry = reader.GetValue(6).ToString();
                                else
                                    learningGapLessonQuizResult.ActualTry = null;
                                if (!reader.IsDBNull(7))
                                    learningGapLessonQuizResult.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    learningGapLessonQuizResult.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    learningGapLessonQuizResult.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    learningGapLessonQuizResult.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    learningGapLessonQuizResult.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    learningGapLessonQuizResult.UpdatedAt = null;
                                response.Add(learningGapLessonQuizResult.Id, learningGapLessonQuizResult);
                                learningGapLessonQuizResult = new LearningGapLessonQuizResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapLessonQuizResultModel>();
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
                response = new Dictionary<string, LearningGapLessonQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapLessonQuizResultModel> ReadD(string connectionString, LearningGapLessonQuizResultModel learningGapLessonQuizResult, bool orderByLastDate, int fromRow, int limit, LearningGapLessonQuizResultModel learningGapLessonQuizResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapLessonQuizResultModel> response = new Dictionary<string, LearningGapLessonQuizResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `learning_gap_assigned_lesson_id`, `quiz_id`, `gsweb_usqui_results_id`, `score`, `percentage`, `actual_try`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_lesson_quiz_results ";
                {
                    if (learningGapLessonQuizResult != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapLessonQuizResult.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.LearningGapAssignedLessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId ");
                            }
                            cmd.Parameters.AddWithValue("@learningGapAssignedLessonId", learningGapLessonQuizResult.LearningGapAssignedLessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", learningGapLessonQuizResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.GswebUsquiResultsId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `gsweb_usqui_results_id` = @gswebUsquiResultsId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `gsweb_usqui_results_id` = @gswebUsquiResultsId ");
                            }
                            cmd.Parameters.AddWithValue("@gswebUsquiResultsId", learningGapLessonQuizResult.GswebUsquiResultsId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Score))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `score` = @score ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `score` = @score ");
                            }
                            cmd.Parameters.AddWithValue("@score", learningGapLessonQuizResult.Score);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Percentage))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `percentage` = @percentage ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `percentage` = @percentage ");
                            }
                            cmd.Parameters.AddWithValue("@percentage", learningGapLessonQuizResult.Percentage);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.ActualTry))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `actual_try` = @actualTry ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `actual_try` = @actualTry ");
                            }
                            cmd.Parameters.AddWithValue("@actualTry", learningGapLessonQuizResult.ActualTry);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapLessonQuizResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapLessonQuizResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapLessonQuizResult.UpdatedAt);
                        }
                    }
                    if (learningGapLessonQuizResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapLessonQuizResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.LearningGapAssignedLessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `learning_gap_assigned_lesson_id` LIKE @learningGapAssignedLessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `learning_gap_assigned_lesson_id` LIKE @learningGapAssignedLessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@learningGapAssignedLessonIdWhereLike", "%" + learningGapLessonQuizResultWhereLike.LearningGapAssignedLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + learningGapLessonQuizResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.GswebUsquiResultsId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `gsweb_usqui_results_id` LIKE @gswebUsquiResultsIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `gsweb_usqui_results_id` LIKE @gswebUsquiResultsIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@gswebUsquiResultsIdWhereLike", "%" + learningGapLessonQuizResultWhereLike.GswebUsquiResultsId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.Score))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `score` LIKE @scoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `score` LIKE @scoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + learningGapLessonQuizResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.Percentage))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `percentage` LIKE @percentageWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `percentage` LIKE @percentageWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@percentageWhereLike", "%" + learningGapLessonQuizResultWhereLike.Percentage + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.ActualTry))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `actual_try` LIKE @actualTryWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `actual_try` LIKE @actualTryWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@actualTryWhereLike", "%" + learningGapLessonQuizResultWhereLike.ActualTry + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapLessonQuizResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapLessonQuizResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapLessonQuizResultWhereLike.UpdatedAt + "%");
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
                                learningGapLessonQuizResult = new LearningGapLessonQuizResultModel();
                                if (!reader.IsDBNull(0))
                                    learningGapLessonQuizResult.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapLessonQuizResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapLessonQuizResult.LearningGapAssignedLessonId = reader.GetValue(1).ToString();
                                else
                                    learningGapLessonQuizResult.LearningGapAssignedLessonId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapLessonQuizResult.QuizId = reader.GetValue(2).ToString();
                                else
                                    learningGapLessonQuizResult.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapLessonQuizResult.GswebUsquiResultsId = reader.GetValue(3).ToString();
                                else
                                    learningGapLessonQuizResult.GswebUsquiResultsId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapLessonQuizResult.Score = reader.GetValue(4).ToString();
                                else
                                    learningGapLessonQuizResult.Score = null;
                                if (!reader.IsDBNull(5))
                                    learningGapLessonQuizResult.Percentage = reader.GetValue(5).ToString();
                                else
                                    learningGapLessonQuizResult.Percentage = null;
                                if (!reader.IsDBNull(6))
                                    learningGapLessonQuizResult.ActualTry = reader.GetValue(6).ToString();
                                else
                                    learningGapLessonQuizResult.ActualTry = null;
                                if (!reader.IsDBNull(7))
                                    learningGapLessonQuizResult.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    learningGapLessonQuizResult.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    learningGapLessonQuizResult.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    learningGapLessonQuizResult.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    learningGapLessonQuizResult.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    learningGapLessonQuizResult.UpdatedAt = null;
                                response.Add(learningGapLessonQuizResult.Id, learningGapLessonQuizResult);
                                learningGapLessonQuizResult = new LearningGapLessonQuizResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapLessonQuizResultModel>();
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
                response = new Dictionary<string, LearningGapLessonQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapLessonQuizResultModel> ReadD(string connectionString, List<LearningGapLessonQuizResultModel> learningGapLessonQuizResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapLessonQuizResultModel> response = new Dictionary<string, LearningGapLessonQuizResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `learning_gap_assigned_lesson_id`, `quiz_id`, `gsweb_usqui_results_id`, `score`, `percentage`, `actual_try`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_lesson_quiz_results ";
                if (learningGapLessonQuizResultWhere != null && learningGapLessonQuizResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapLessonQuizResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapLessonQuizResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].LearningGapAssignedLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@learningGapAssignedLessonId" + i, learningGapLessonQuizResultWhere[i].LearningGapAssignedLessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, learningGapLessonQuizResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].GswebUsquiResultsId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gsweb_usqui_results_id` = @gswebUsquiResultsId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gswebUsquiResultsId" + i, learningGapLessonQuizResultWhere[i].GswebUsquiResultsId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, learningGapLessonQuizResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].Percentage))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `percentage` = @percentage" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@percentage" + i, learningGapLessonQuizResultWhere[i].Percentage);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].ActualTry))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `actual_try` = @actualTry" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@actualTry" + i, learningGapLessonQuizResultWhere[i].ActualTry);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapLessonQuizResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapLessonQuizResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapLessonQuizResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapLessonQuizResultWhere.Count - 1)
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
                            LearningGapLessonQuizResultModel learningGapLessonQuizResult = new LearningGapLessonQuizResultModel(); if (!reader.IsDBNull(0))
                                learningGapLessonQuizResult.Id = reader.GetValue(0).ToString();
                            else
                                learningGapLessonQuizResult.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapLessonQuizResult.LearningGapAssignedLessonId = reader.GetValue(1).ToString();
                            else
                                learningGapLessonQuizResult.LearningGapAssignedLessonId = null;
                            if (!reader.IsDBNull(2))
                                learningGapLessonQuizResult.QuizId = reader.GetValue(2).ToString();
                            else
                                learningGapLessonQuizResult.QuizId = null;
                            if (!reader.IsDBNull(3))
                                learningGapLessonQuizResult.GswebUsquiResultsId = reader.GetValue(3).ToString();
                            else
                                learningGapLessonQuizResult.GswebUsquiResultsId = null;
                            if (!reader.IsDBNull(4))
                                learningGapLessonQuizResult.Score = reader.GetValue(4).ToString();
                            else
                                learningGapLessonQuizResult.Score = null;
                            if (!reader.IsDBNull(5))
                                learningGapLessonQuizResult.Percentage = reader.GetValue(5).ToString();
                            else
                                learningGapLessonQuizResult.Percentage = null;
                            if (!reader.IsDBNull(6))
                                learningGapLessonQuizResult.ActualTry = reader.GetValue(6).ToString();
                            else
                                learningGapLessonQuizResult.ActualTry = null;
                            if (!reader.IsDBNull(7))
                                learningGapLessonQuizResult.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                learningGapLessonQuizResult.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                learningGapLessonQuizResult.CreatedAt = reader.GetValue(8).ToString();
                            else
                                learningGapLessonQuizResult.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                learningGapLessonQuizResult.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                learningGapLessonQuizResult.UpdatedAt = null;
                            response.Add(learningGapLessonQuizResult.Id, learningGapLessonQuizResult);
                            learningGapLessonQuizResult = new LearningGapLessonQuizResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapLessonQuizResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapLessonQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapLessonQuizResultModel> ReadD(string connectionString, List<LearningGapLessonQuizResultModel> learningGapLessonQuizResultWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapLessonQuizResultModel> response = new Dictionary<string, LearningGapLessonQuizResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `learning_gap_assigned_lesson_id`, `quiz_id`, `gsweb_usqui_results_id`, `score`, `percentage`, `actual_try`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_lesson_quiz_results ";
                if (learningGapLessonQuizResultWhere != null && learningGapLessonQuizResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapLessonQuizResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapLessonQuizResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].LearningGapAssignedLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@learningGapAssignedLessonId" + i, learningGapLessonQuizResultWhere[i].LearningGapAssignedLessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, learningGapLessonQuizResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].GswebUsquiResultsId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gsweb_usqui_results_id` = @gswebUsquiResultsId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gswebUsquiResultsId" + i, learningGapLessonQuizResultWhere[i].GswebUsquiResultsId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, learningGapLessonQuizResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].Percentage))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `percentage` = @percentage" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@percentage" + i, learningGapLessonQuizResultWhere[i].Percentage);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].ActualTry))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `actual_try` = @actualTry" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@actualTry" + i, learningGapLessonQuizResultWhere[i].ActualTry);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapLessonQuizResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapLessonQuizResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapLessonQuizResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapLessonQuizResultWhere.Count - 1)
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
                            LearningGapLessonQuizResultModel learningGapLessonQuizResult = new LearningGapLessonQuizResultModel(); if (!reader.IsDBNull(0))
                                learningGapLessonQuizResult.Id = reader.GetValue(0).ToString();
                            else
                                learningGapLessonQuizResult.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapLessonQuizResult.LearningGapAssignedLessonId = reader.GetValue(1).ToString();
                            else
                                learningGapLessonQuizResult.LearningGapAssignedLessonId = null;
                            if (!reader.IsDBNull(2))
                                learningGapLessonQuizResult.QuizId = reader.GetValue(2).ToString();
                            else
                                learningGapLessonQuizResult.QuizId = null;
                            if (!reader.IsDBNull(3))
                                learningGapLessonQuizResult.GswebUsquiResultsId = reader.GetValue(3).ToString();
                            else
                                learningGapLessonQuizResult.GswebUsquiResultsId = null;
                            if (!reader.IsDBNull(4))
                                learningGapLessonQuizResult.Score = reader.GetValue(4).ToString();
                            else
                                learningGapLessonQuizResult.Score = null;
                            if (!reader.IsDBNull(5))
                                learningGapLessonQuizResult.Percentage = reader.GetValue(5).ToString();
                            else
                                learningGapLessonQuizResult.Percentage = null;
                            if (!reader.IsDBNull(6))
                                learningGapLessonQuizResult.ActualTry = reader.GetValue(6).ToString();
                            else
                                learningGapLessonQuizResult.ActualTry = null;
                            if (!reader.IsDBNull(7))
                                learningGapLessonQuizResult.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                learningGapLessonQuizResult.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                learningGapLessonQuizResult.CreatedAt = reader.GetValue(8).ToString();
                            else
                                learningGapLessonQuizResult.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                learningGapLessonQuizResult.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                learningGapLessonQuizResult.UpdatedAt = null;
                            response.Add(learningGapLessonQuizResult.Id, learningGapLessonQuizResult);
                            learningGapLessonQuizResult = new LearningGapLessonQuizResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapLessonQuizResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapLessonQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapLessonQuizResultModel> Read(string connectionString, LearningGapLessonQuizResultModel learningGapLessonQuizResult, bool firstOne, bool orderByLastDate, LearningGapLessonQuizResultModel learningGapLessonQuizResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapLessonQuizResultModel> response = new List<LearningGapLessonQuizResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `learning_gap_assigned_lesson_id`, `quiz_id`, `gsweb_usqui_results_id`, `score`, `percentage`, `actual_try`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_lesson_quiz_results ";
                {
                    if (learningGapLessonQuizResult != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapLessonQuizResult.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.LearningGapAssignedLessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId ");
                            }
                            cmd.Parameters.AddWithValue("@learningGapAssignedLessonId", learningGapLessonQuizResult.LearningGapAssignedLessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", learningGapLessonQuizResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.GswebUsquiResultsId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `gsweb_usqui_results_id` = @gswebUsquiResultsId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `gsweb_usqui_results_id` = @gswebUsquiResultsId ");
                            }
                            cmd.Parameters.AddWithValue("@gswebUsquiResultsId", learningGapLessonQuizResult.GswebUsquiResultsId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Score))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `score` = @score ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `score` = @score ");
                            }
                            cmd.Parameters.AddWithValue("@score", learningGapLessonQuizResult.Score);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Percentage))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `percentage` = @percentage ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `percentage` = @percentage ");
                            }
                            cmd.Parameters.AddWithValue("@percentage", learningGapLessonQuizResult.Percentage);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.ActualTry))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `actual_try` = @actualTry ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `actual_try` = @actualTry ");
                            }
                            cmd.Parameters.AddWithValue("@actualTry", learningGapLessonQuizResult.ActualTry);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapLessonQuizResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapLessonQuizResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapLessonQuizResult.UpdatedAt);
                        }
                    }
                    if (learningGapLessonQuizResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapLessonQuizResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.LearningGapAssignedLessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `learning_gap_assigned_lesson_id` LIKE @learningGapAssignedLessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `learning_gap_assigned_lesson_id` LIKE @learningGapAssignedLessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@learningGapAssignedLessonIdWhereLike", "%" + learningGapLessonQuizResultWhereLike.LearningGapAssignedLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + learningGapLessonQuizResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.GswebUsquiResultsId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `gsweb_usqui_results_id` LIKE @gswebUsquiResultsIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `gsweb_usqui_results_id` LIKE @gswebUsquiResultsIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@gswebUsquiResultsIdWhereLike", "%" + learningGapLessonQuizResultWhereLike.GswebUsquiResultsId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.Score))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `score` LIKE @scoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `score` LIKE @scoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + learningGapLessonQuizResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.Percentage))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `percentage` LIKE @percentageWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `percentage` LIKE @percentageWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@percentageWhereLike", "%" + learningGapLessonQuizResultWhereLike.Percentage + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.ActualTry))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `actual_try` LIKE @actualTryWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `actual_try` LIKE @actualTryWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@actualTryWhereLike", "%" + learningGapLessonQuizResultWhereLike.ActualTry + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapLessonQuizResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapLessonQuizResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapLessonQuizResultWhereLike.UpdatedAt + "%");
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
                                learningGapLessonQuizResult = new LearningGapLessonQuizResultModel();
                                if (!reader.IsDBNull(0))
                                    learningGapLessonQuizResult.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapLessonQuizResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapLessonQuizResult.LearningGapAssignedLessonId = reader.GetValue(1).ToString();
                                else
                                    learningGapLessonQuizResult.LearningGapAssignedLessonId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapLessonQuizResult.QuizId = reader.GetValue(2).ToString();
                                else
                                    learningGapLessonQuizResult.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapLessonQuizResult.GswebUsquiResultsId = reader.GetValue(3).ToString();
                                else
                                    learningGapLessonQuizResult.GswebUsquiResultsId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapLessonQuizResult.Score = reader.GetValue(4).ToString();
                                else
                                    learningGapLessonQuizResult.Score = null;
                                if (!reader.IsDBNull(5))
                                    learningGapLessonQuizResult.Percentage = reader.GetValue(5).ToString();
                                else
                                    learningGapLessonQuizResult.Percentage = null;
                                if (!reader.IsDBNull(6))
                                    learningGapLessonQuizResult.ActualTry = reader.GetValue(6).ToString();
                                else
                                    learningGapLessonQuizResult.ActualTry = null;
                                if (!reader.IsDBNull(7))
                                    learningGapLessonQuizResult.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    learningGapLessonQuizResult.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    learningGapLessonQuizResult.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    learningGapLessonQuizResult.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    learningGapLessonQuizResult.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    learningGapLessonQuizResult.UpdatedAt = null;
                                response.Add(learningGapLessonQuizResult);
                                learningGapLessonQuizResult = new LearningGapLessonQuizResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapLessonQuizResultModel>();
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
                response = new List<LearningGapLessonQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapLessonQuizResultModel> Read(string connectionString, LearningGapLessonQuizResultModel learningGapLessonQuizResult, bool orderByLastDate, int fromRow, int limit, LearningGapLessonQuizResultModel learningGapLessonQuizResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapLessonQuizResultModel> response = new List<LearningGapLessonQuizResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `learning_gap_assigned_lesson_id`, `quiz_id`, `gsweb_usqui_results_id`, `score`, `percentage`, `actual_try`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_lesson_quiz_results ";
                {
                    if (learningGapLessonQuizResult != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapLessonQuizResult.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.LearningGapAssignedLessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId ");
                            }
                            cmd.Parameters.AddWithValue("@learningGapAssignedLessonId", learningGapLessonQuizResult.LearningGapAssignedLessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", learningGapLessonQuizResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.GswebUsquiResultsId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `gsweb_usqui_results_id` = @gswebUsquiResultsId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `gsweb_usqui_results_id` = @gswebUsquiResultsId ");
                            }
                            cmd.Parameters.AddWithValue("@gswebUsquiResultsId", learningGapLessonQuizResult.GswebUsquiResultsId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Score))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `score` = @score ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `score` = @score ");
                            }
                            cmd.Parameters.AddWithValue("@score", learningGapLessonQuizResult.Score);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Percentage))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `percentage` = @percentage ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `percentage` = @percentage ");
                            }
                            cmd.Parameters.AddWithValue("@percentage", learningGapLessonQuizResult.Percentage);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.ActualTry))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `actual_try` = @actualTry ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `actual_try` = @actualTry ");
                            }
                            cmd.Parameters.AddWithValue("@actualTry", learningGapLessonQuizResult.ActualTry);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapLessonQuizResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapLessonQuizResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapLessonQuizResult.UpdatedAt);
                        }
                    }
                    if (learningGapLessonQuizResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapLessonQuizResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.LearningGapAssignedLessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `learning_gap_assigned_lesson_id` LIKE @learningGapAssignedLessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `learning_gap_assigned_lesson_id` LIKE @learningGapAssignedLessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@learningGapAssignedLessonIdWhereLike", "%" + learningGapLessonQuizResultWhereLike.LearningGapAssignedLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + learningGapLessonQuizResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.GswebUsquiResultsId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `gsweb_usqui_results_id` LIKE @gswebUsquiResultsIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `gsweb_usqui_results_id` LIKE @gswebUsquiResultsIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@gswebUsquiResultsIdWhereLike", "%" + learningGapLessonQuizResultWhereLike.GswebUsquiResultsId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.Score))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `score` LIKE @scoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `score` LIKE @scoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + learningGapLessonQuizResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.Percentage))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `percentage` LIKE @percentageWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `percentage` LIKE @percentageWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@percentageWhereLike", "%" + learningGapLessonQuizResultWhereLike.Percentage + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.ActualTry))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `actual_try` LIKE @actualTryWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `actual_try` LIKE @actualTryWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@actualTryWhereLike", "%" + learningGapLessonQuizResultWhereLike.ActualTry + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapLessonQuizResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapLessonQuizResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapLessonQuizResultWhereLike.UpdatedAt + "%");
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
                                learningGapLessonQuizResult = new LearningGapLessonQuizResultModel();
                                if (!reader.IsDBNull(0))
                                    learningGapLessonQuizResult.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapLessonQuizResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapLessonQuizResult.LearningGapAssignedLessonId = reader.GetValue(1).ToString();
                                else
                                    learningGapLessonQuizResult.LearningGapAssignedLessonId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapLessonQuizResult.QuizId = reader.GetValue(2).ToString();
                                else
                                    learningGapLessonQuizResult.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapLessonQuizResult.GswebUsquiResultsId = reader.GetValue(3).ToString();
                                else
                                    learningGapLessonQuizResult.GswebUsquiResultsId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapLessonQuizResult.Score = reader.GetValue(4).ToString();
                                else
                                    learningGapLessonQuizResult.Score = null;
                                if (!reader.IsDBNull(5))
                                    learningGapLessonQuizResult.Percentage = reader.GetValue(5).ToString();
                                else
                                    learningGapLessonQuizResult.Percentage = null;
                                if (!reader.IsDBNull(6))
                                    learningGapLessonQuizResult.ActualTry = reader.GetValue(6).ToString();
                                else
                                    learningGapLessonQuizResult.ActualTry = null;
                                if (!reader.IsDBNull(7))
                                    learningGapLessonQuizResult.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    learningGapLessonQuizResult.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    learningGapLessonQuizResult.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    learningGapLessonQuizResult.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    learningGapLessonQuizResult.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    learningGapLessonQuizResult.UpdatedAt = null;
                                response.Add(learningGapLessonQuizResult);
                                learningGapLessonQuizResult = new LearningGapLessonQuizResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapLessonQuizResultModel>();
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
                response = new List<LearningGapLessonQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapLessonQuizResultModel> Read(string connectionString, List<LearningGapLessonQuizResultModel> learningGapLessonQuizResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapLessonQuizResultModel> response = new List<LearningGapLessonQuizResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `learning_gap_assigned_lesson_id`, `quiz_id`, `gsweb_usqui_results_id`, `score`, `percentage`, `actual_try`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_lesson_quiz_results ";
                if (learningGapLessonQuizResultWhere != null && learningGapLessonQuizResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapLessonQuizResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapLessonQuizResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].LearningGapAssignedLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@learningGapAssignedLessonId" + i, learningGapLessonQuizResultWhere[i].LearningGapAssignedLessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, learningGapLessonQuizResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].GswebUsquiResultsId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gsweb_usqui_results_id` = @gswebUsquiResultsId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gswebUsquiResultsId" + i, learningGapLessonQuizResultWhere[i].GswebUsquiResultsId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, learningGapLessonQuizResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].Percentage))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `percentage` = @percentage" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@percentage" + i, learningGapLessonQuizResultWhere[i].Percentage);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].ActualTry))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `actual_try` = @actualTry" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@actualTry" + i, learningGapLessonQuizResultWhere[i].ActualTry);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapLessonQuizResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapLessonQuizResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapLessonQuizResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapLessonQuizResultWhere.Count - 1)
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
                            LearningGapLessonQuizResultModel learningGapLessonQuizResult = new LearningGapLessonQuizResultModel(); if (!reader.IsDBNull(0))
                                learningGapLessonQuizResult.Id = reader.GetValue(0).ToString();
                            else
                                learningGapLessonQuizResult.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapLessonQuizResult.LearningGapAssignedLessonId = reader.GetValue(1).ToString();
                            else
                                learningGapLessonQuizResult.LearningGapAssignedLessonId = null;
                            if (!reader.IsDBNull(2))
                                learningGapLessonQuizResult.QuizId = reader.GetValue(2).ToString();
                            else
                                learningGapLessonQuizResult.QuizId = null;
                            if (!reader.IsDBNull(3))
                                learningGapLessonQuizResult.GswebUsquiResultsId = reader.GetValue(3).ToString();
                            else
                                learningGapLessonQuizResult.GswebUsquiResultsId = null;
                            if (!reader.IsDBNull(4))
                                learningGapLessonQuizResult.Score = reader.GetValue(4).ToString();
                            else
                                learningGapLessonQuizResult.Score = null;
                            if (!reader.IsDBNull(5))
                                learningGapLessonQuizResult.Percentage = reader.GetValue(5).ToString();
                            else
                                learningGapLessonQuizResult.Percentage = null;
                            if (!reader.IsDBNull(6))
                                learningGapLessonQuizResult.ActualTry = reader.GetValue(6).ToString();
                            else
                                learningGapLessonQuizResult.ActualTry = null;
                            if (!reader.IsDBNull(7))
                                learningGapLessonQuizResult.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                learningGapLessonQuizResult.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                learningGapLessonQuizResult.CreatedAt = reader.GetValue(8).ToString();
                            else
                                learningGapLessonQuizResult.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                learningGapLessonQuizResult.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                learningGapLessonQuizResult.UpdatedAt = null;
                            response.Add(learningGapLessonQuizResult);
                            learningGapLessonQuizResult = new LearningGapLessonQuizResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapLessonQuizResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapLessonQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapLessonQuizResultModel> Read(string connectionString, List<LearningGapLessonQuizResultModel> learningGapLessonQuizResultWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapLessonQuizResultModel> response = new List<LearningGapLessonQuizResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `learning_gap_assigned_lesson_id`, `quiz_id`, `gsweb_usqui_results_id`, `score`, `percentage`, `actual_try`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_lesson_quiz_results ";
                if (learningGapLessonQuizResultWhere != null && learningGapLessonQuizResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapLessonQuizResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapLessonQuizResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].LearningGapAssignedLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@learningGapAssignedLessonId" + i, learningGapLessonQuizResultWhere[i].LearningGapAssignedLessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, learningGapLessonQuizResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].GswebUsquiResultsId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gsweb_usqui_results_id` = @gswebUsquiResultsId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gswebUsquiResultsId" + i, learningGapLessonQuizResultWhere[i].GswebUsquiResultsId);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, learningGapLessonQuizResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].Percentage))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `percentage` = @percentage" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@percentage" + i, learningGapLessonQuizResultWhere[i].Percentage);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].ActualTry))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `actual_try` = @actualTry" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@actualTry" + i, learningGapLessonQuizResultWhere[i].ActualTry);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapLessonQuizResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapLessonQuizResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapLessonQuizResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapLessonQuizResultWhere.Count - 1)
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
                            LearningGapLessonQuizResultModel learningGapLessonQuizResult = new LearningGapLessonQuizResultModel(); if (!reader.IsDBNull(0))
                                learningGapLessonQuizResult.Id = reader.GetValue(0).ToString();
                            else
                                learningGapLessonQuizResult.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapLessonQuizResult.LearningGapAssignedLessonId = reader.GetValue(1).ToString();
                            else
                                learningGapLessonQuizResult.LearningGapAssignedLessonId = null;
                            if (!reader.IsDBNull(2))
                                learningGapLessonQuizResult.QuizId = reader.GetValue(2).ToString();
                            else
                                learningGapLessonQuizResult.QuizId = null;
                            if (!reader.IsDBNull(3))
                                learningGapLessonQuizResult.GswebUsquiResultsId = reader.GetValue(3).ToString();
                            else
                                learningGapLessonQuizResult.GswebUsquiResultsId = null;
                            if (!reader.IsDBNull(4))
                                learningGapLessonQuizResult.Score = reader.GetValue(4).ToString();
                            else
                                learningGapLessonQuizResult.Score = null;
                            if (!reader.IsDBNull(5))
                                learningGapLessonQuizResult.Percentage = reader.GetValue(5).ToString();
                            else
                                learningGapLessonQuizResult.Percentage = null;
                            if (!reader.IsDBNull(6))
                                learningGapLessonQuizResult.ActualTry = reader.GetValue(6).ToString();
                            else
                                learningGapLessonQuizResult.ActualTry = null;
                            if (!reader.IsDBNull(7))
                                learningGapLessonQuizResult.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                learningGapLessonQuizResult.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                learningGapLessonQuizResult.CreatedAt = reader.GetValue(8).ToString();
                            else
                                learningGapLessonQuizResult.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                learningGapLessonQuizResult.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                learningGapLessonQuizResult.UpdatedAt = null;
                            response.Add(learningGapLessonQuizResult);
                            learningGapLessonQuizResult = new LearningGapLessonQuizResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapLessonQuizResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapLessonQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapLessonQuizResultModel> Create(string connectionString, LearningGapLessonQuizResultModel learningGapLessonQuizResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapLessonQuizResultModel> response = new List<LearningGapLessonQuizResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO learning_gap_lesson_quiz_results (";
                string values = "(";
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Id))
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
                    cmd.Parameters.AddWithValue("@id", learningGapLessonQuizResult.Id);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.LearningGapAssignedLessonId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `learning_gap_assigned_lesson_id` ");
                        values = String.Concat(values, ", @learningGapAssignedLessonId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `learning_gap_assigned_lesson_id` ");
                        values = String.Concat(values, " @learningGapAssignedLessonId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@learningGapAssignedLessonId", learningGapLessonQuizResult.LearningGapAssignedLessonId);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.QuizId))
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
                    cmd.Parameters.AddWithValue("@quizId", learningGapLessonQuizResult.QuizId);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.GswebUsquiResultsId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `gsweb_usqui_results_id` ");
                        values = String.Concat(values, ", @gswebUsquiResultsId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `gsweb_usqui_results_id` ");
                        values = String.Concat(values, " @gswebUsquiResultsId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@gswebUsquiResultsId", learningGapLessonQuizResult.GswebUsquiResultsId);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Score))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `score` ");
                        values = String.Concat(values, ", @score");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `score` ");
                        values = String.Concat(values, " @score");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@score", learningGapLessonQuizResult.Score);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Percentage))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `percentage` ");
                        values = String.Concat(values, ", @percentage");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `percentage` ");
                        values = String.Concat(values, " @percentage");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@percentage", learningGapLessonQuizResult.Percentage);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.ActualTry))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `actual_try` ");
                        values = String.Concat(values, ", @actualTry");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `actual_try` ");
                        values = String.Concat(values, " @actualTry");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@actualTry", learningGapLessonQuizResult.ActualTry);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapLessonQuizResult.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", learningGapLessonQuizResult.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapLessonQuizResult.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                learningGapLessonQuizResult.Id = cmd.LastInsertedId.ToString();
                response.Add(learningGapLessonQuizResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapLessonQuizResultModel>();
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
        public List<LearningGapLessonQuizResultModel> Create(string connectionString, List<LearningGapLessonQuizResultModel> learningGapLessonQuizResultList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapLessonQuizResultModel> response = new List<LearningGapLessonQuizResultModel>();
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
                        for (int i = 0; i < learningGapLessonQuizResultList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO learning_gap_lesson_quiz_results (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapLessonQuizResultList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].LearningGapAssignedLessonId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `learning_gap_assigned_lesson_id` ");
                                        values = String.Concat(values, ", @learningGapAssignedLessonId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `learning_gap_assigned_lesson_id` ");
                                        values = String.Concat(values, "@learningGapAssignedLessonId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@learningGapAssignedLessonId" + i, learningGapLessonQuizResultList[i].LearningGapAssignedLessonId);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].QuizId))
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
                                    cmd.Parameters.AddWithValue("@quizId" + i, learningGapLessonQuizResultList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].GswebUsquiResultsId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `gsweb_usqui_results_id` ");
                                        values = String.Concat(values, ", @gswebUsquiResultsId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `gsweb_usqui_results_id` ");
                                        values = String.Concat(values, "@gswebUsquiResultsId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@gswebUsquiResultsId" + i, learningGapLessonQuizResultList[i].GswebUsquiResultsId);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].Score))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `score` ");
                                        values = String.Concat(values, ", @score" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `score` ");
                                        values = String.Concat(values, "@score" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@score" + i, learningGapLessonQuizResultList[i].Score);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].Percentage))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `percentage` ");
                                        values = String.Concat(values, ", @percentage" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `percentage` ");
                                        values = String.Concat(values, "@percentage" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@percentage" + i, learningGapLessonQuizResultList[i].Percentage);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].ActualTry))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `actual_try` ");
                                        values = String.Concat(values, ", @actualTry" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `actual_try` ");
                                        values = String.Concat(values, "@actualTry" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@actualTry" + i, learningGapLessonQuizResultList[i].ActualTry);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapLessonQuizResultList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapLessonQuizResultList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapLessonQuizResultList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(learningGapLessonQuizResultList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapLessonQuizResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapLessonQuizResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapLessonQuizResultModel>();
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

        public List<LearningGapLessonQuizResultModel> Update(string connectionString, LearningGapLessonQuizResultModel learningGapLessonQuizResult, LearningGapLessonQuizResultModel learningGapLessonQuizResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapLessonQuizResultModel> response = new List<LearningGapLessonQuizResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE learning_gap_lesson_quiz_results SET ";
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", learningGapLessonQuizResult.Id);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.LearningGapAssignedLessonId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId");
                    else
                        sql = String.Concat(sql, " `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@learningGapAssignedLessonId", learningGapLessonQuizResult.LearningGapAssignedLessonId);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.QuizId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `quiz_id` = @quizId");
                    else
                        sql = String.Concat(sql, " `quiz_id` = @quizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", learningGapLessonQuizResult.QuizId);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.GswebUsquiResultsId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `gsweb_usqui_results_id` = @gswebUsquiResultsId");
                    else
                        sql = String.Concat(sql, " `gsweb_usqui_results_id` = @gswebUsquiResultsId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@gswebUsquiResultsId", learningGapLessonQuizResult.GswebUsquiResultsId);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Score))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `score` = @score");
                    else
                        sql = String.Concat(sql, " `score` = @score");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@score", learningGapLessonQuizResult.Score);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Percentage))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `percentage` = @percentage");
                    else
                        sql = String.Concat(sql, " `percentage` = @percentage");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@percentage", learningGapLessonQuizResult.Percentage);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.ActualTry))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `actual_try` = @actualTry");
                    else
                        sql = String.Concat(sql, " `actual_try` = @actualTry");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@actualTry", learningGapLessonQuizResult.ActualTry);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapLessonQuizResult.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", learningGapLessonQuizResult.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapLessonQuizResult.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapLessonQuizResult.UpdatedAt);
                }
                if (learningGapLessonQuizResultWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", learningGapLessonQuizResultWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere.LearningGapAssignedLessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonIdWhere");
                        else
                            sql = String.Concat(sql, " `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@learningGapAssignedLessonIdWhere", learningGapLessonQuizResultWhere.LearningGapAssignedLessonId);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizIdWhere", learningGapLessonQuizResultWhere.QuizId);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere.GswebUsquiResultsId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `gsweb_usqui_results_id` = @gswebUsquiResultsIdWhere");
                        else
                            sql = String.Concat(sql, " `gsweb_usqui_results_id` = @gswebUsquiResultsIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@gswebUsquiResultsIdWhere", learningGapLessonQuizResultWhere.GswebUsquiResultsId);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere.Score))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `score` = @scoreWhere");
                        else
                            sql = String.Concat(sql, " `score` = @scoreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@scoreWhere", learningGapLessonQuizResultWhere.Score);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere.Percentage))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `percentage` = @percentageWhere");
                        else
                            sql = String.Concat(sql, " `percentage` = @percentageWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@percentageWhere", learningGapLessonQuizResultWhere.Percentage);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere.ActualTry))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `actual_try` = @actualTryWhere");
                        else
                            sql = String.Concat(sql, " `actual_try` = @actualTryWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@actualTryWhere", learningGapLessonQuizResultWhere.ActualTry);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", learningGapLessonQuizResultWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", learningGapLessonQuizResultWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", learningGapLessonQuizResultWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(learningGapLessonQuizResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapLessonQuizResultModel>();
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

        public List<LearningGapLessonQuizResultModel> Update(string connectionString, List<LearningGapLessonQuizResultModel> learningGapLessonQuizResultList, List<LearningGapLessonQuizResultModel> learningGapLessonQuizResultWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapLessonQuizResultModel> response = new List<LearningGapLessonQuizResultModel>();
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
                        for (int i = 0; i < learningGapLessonQuizResultList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE learning_gap_lesson_quiz_results SET ";
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapLessonQuizResultList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].LearningGapAssignedLessonId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId" + i);
                                    else
                                        sql = String.Concat(sql, " `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@learningGapAssignedLessonId" + i, learningGapLessonQuizResultList[i].LearningGapAssignedLessonId);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].QuizId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `quiz_id` = @quizId" + i);
                                    else
                                        sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, learningGapLessonQuizResultList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].GswebUsquiResultsId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `gsweb_usqui_results_id` = @gswebUsquiResultsId" + i);
                                    else
                                        sql = String.Concat(sql, " `gsweb_usqui_results_id` = @gswebUsquiResultsId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@gswebUsquiResultsId" + i, learningGapLessonQuizResultList[i].GswebUsquiResultsId);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].Score))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `score` = @score" + i);
                                    else
                                        sql = String.Concat(sql, " `score` = @score" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@score" + i, learningGapLessonQuizResultList[i].Score);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].Percentage))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `percentage` = @percentage" + i);
                                    else
                                        sql = String.Concat(sql, " `percentage` = @percentage" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@percentage" + i, learningGapLessonQuizResultList[i].Percentage);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].ActualTry))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `actual_try` = @actualTry" + i);
                                    else
                                        sql = String.Concat(sql, " `actual_try` = @actualTry" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@actualTry" + i, learningGapLessonQuizResultList[i].ActualTry);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapLessonQuizResultList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapLessonQuizResultList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapLessonQuizResultList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapLessonQuizResultList[i].UpdatedAt);
                                }
                                if (learningGapLessonQuizResultWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, learningGapLessonQuizResultWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereList[i].LearningGapAssignedLessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@learningGapAssignedLessonIdWhere" + i, learningGapLessonQuizResultWhereList[i].LearningGapAssignedLessonId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereList[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizIdWhere" + i, learningGapLessonQuizResultWhereList[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereList[i].GswebUsquiResultsId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `gsweb_usqui_results_id` = @gswebUsquiResultsIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `gsweb_usqui_results_id` = @gswebUsquiResultsIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@gswebUsquiResultsIdWhere" + i, learningGapLessonQuizResultWhereList[i].GswebUsquiResultsId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereList[i].Score))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `score` = @scoreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `score` = @scoreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@scoreWhere" + i, learningGapLessonQuizResultWhereList[i].Score);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereList[i].Percentage))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `percentage` = @percentageWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `percentage` = @percentageWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@percentageWhere" + i, learningGapLessonQuizResultWhereList[i].Percentage);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereList[i].ActualTry))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `actual_try` = @actualTryWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `actual_try` = @actualTryWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@actualTryWhere" + i, learningGapLessonQuizResultWhereList[i].ActualTry);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, learningGapLessonQuizResultWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, learningGapLessonQuizResultWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResultWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, learningGapLessonQuizResultWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapLessonQuizResultList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapLessonQuizResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapLessonQuizResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapLessonQuizResultModel>();
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

        public List<LearningGapLessonQuizResultModel> Delete(string connectionString, LearningGapLessonQuizResultModel learningGapLessonQuizResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapLessonQuizResultModel> response = new List<LearningGapLessonQuizResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM learning_gap_lesson_quiz_results  ";
                if (learningGapLessonQuizResult != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", learningGapLessonQuizResult.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult.LearningGapAssignedLessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId");
                        else
                            sql = String.Concat(sql, " `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@learningGapAssignedLessonId", learningGapLessonQuizResult.LearningGapAssignedLessonId);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizId");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizId", learningGapLessonQuizResult.QuizId);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult.GswebUsquiResultsId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `gsweb_usqui_results_id` = @gswebUsquiResultsId");
                        else
                            sql = String.Concat(sql, " `gsweb_usqui_results_id` = @gswebUsquiResultsId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@gswebUsquiResultsId", learningGapLessonQuizResult.GswebUsquiResultsId);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Score))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `score` = @score");
                        else
                            sql = String.Concat(sql, " `score` = @score");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@score", learningGapLessonQuizResult.Score);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult.Percentage))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `percentage` = @percentage");
                        else
                            sql = String.Concat(sql, " `percentage` = @percentage");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@percentage", learningGapLessonQuizResult.Percentage);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult.ActualTry))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `actual_try` = @actualTry");
                        else
                            sql = String.Concat(sql, " `actual_try` = @actualTry");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@actualTry", learningGapLessonQuizResult.ActualTry);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", learningGapLessonQuizResult.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", learningGapLessonQuizResult.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", learningGapLessonQuizResult.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(learningGapLessonQuizResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapLessonQuizResultModel>();
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

        public List<LearningGapLessonQuizResultModel> Delete(string connectionString, List<LearningGapLessonQuizResultModel> learningGapLessonQuizResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapLessonQuizResultModel> response = new List<LearningGapLessonQuizResultModel>();
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
                        for (int i = 0; i < learningGapLessonQuizResult.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM learning_gap_lesson_quiz_results  ";
                                if (learningGapLessonQuizResult[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, learningGapLessonQuizResult[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult[i].LearningGapAssignedLessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId" + i);
                                        else
                                            sql = String.Concat(sql, " `learning_gap_assigned_lesson_id` = @learningGapAssignedLessonId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@learningGapAssignedLessonId" + i, learningGapLessonQuizResult[i].LearningGapAssignedLessonId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizId" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizId" + i, learningGapLessonQuizResult[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult[i].GswebUsquiResultsId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `gsweb_usqui_results_id` = @gswebUsquiResultsId" + i);
                                        else
                                            sql = String.Concat(sql, " `gsweb_usqui_results_id` = @gswebUsquiResultsId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@gswebUsquiResultsId" + i, learningGapLessonQuizResult[i].GswebUsquiResultsId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult[i].Score))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `score` = @score" + i);
                                        else
                                            sql = String.Concat(sql, " `score` = @score" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@score" + i, learningGapLessonQuizResult[i].Score);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult[i].Percentage))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `percentage` = @percentage" + i);
                                        else
                                            sql = String.Concat(sql, " `percentage` = @percentage" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@percentage" + i, learningGapLessonQuizResult[i].Percentage);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult[i].ActualTry))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `actual_try` = @actualTry" + i);
                                        else
                                            sql = String.Concat(sql, " `actual_try` = @actualTry" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@actualTry" + i, learningGapLessonQuizResult[i].ActualTry);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapLessonQuizResult[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, learningGapLessonQuizResult[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapLessonQuizResult[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapLessonQuizResult[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapLessonQuizResult[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapLessonQuizResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapLessonQuizResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapLessonQuizResultModel>();
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
