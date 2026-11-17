using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Teachers;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    class PlanPrePostUserResultDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, PlanPrePostUserResultModel> ReadD(string connectionString, PlanPrePostUserResultModel planPrePostUserResult, bool firstOne, bool orderByLastDate, PlanPrePostUserResultModel planPrePostUserResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanPrePostUserResultModel> response = new Dictionary<string, PlanPrePostUserResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_lesson_id`, `user_id`, `quiz_id`, `pre_post`, `total_correct`, `total_questions`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_pre_post_user_results ";
                {
                    if (planPrePostUserResult != null)
                    {
                        if (!String.IsNullOrEmpty(planPrePostUserResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", planPrePostUserResult.Id);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.PlanLessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` = @planLessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` = @planLessonId ");
                            }
                            cmd.Parameters.AddWithValue("@planLessonId", planPrePostUserResult.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", planPrePostUserResult.UserId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", planPrePostUserResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.PrePost))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `pre_post` = @prePost ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `pre_post` = @prePost ");
                            }
                            cmd.Parameters.AddWithValue("@prePost", planPrePostUserResult.PrePost);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.TotalCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_correct` = @totalCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_correct` = @totalCorrect ");
                            }
                            cmd.Parameters.AddWithValue("@totalCorrect", planPrePostUserResult.TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.TotalQuestions))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_questions` = @totalQuestions ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_questions` = @totalQuestions ");
                            }
                            cmd.Parameters.AddWithValue("@totalQuestions", planPrePostUserResult.TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.Score))
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
                            cmd.Parameters.AddWithValue("@score", planPrePostUserResult.Score);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.UsquiUserResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` = @usquiUserResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` = @usquiUserResultId ");
                            }
                            cmd.Parameters.AddWithValue("@usquiUserResultId", planPrePostUserResult.UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planPrePostUserResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planPrePostUserResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planPrePostUserResult.UpdatedAt);
                        }
                    }
                    if (planPrePostUserResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planPrePostUserResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.PlanLessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + planPrePostUserResultWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planPrePostUserResultWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + planPrePostUserResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.PrePost))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `pre_post` LIKE @prePostWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `pre_post` LIKE @prePostWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@prePostWhereLike", "%" + planPrePostUserResultWhereLike.PrePost + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.TotalCorrect))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalCorrectWhereLike", "%" + planPrePostUserResultWhereLike.TotalCorrect + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.TotalQuestions))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalQuestionsWhereLike", "%" + planPrePostUserResultWhereLike.TotalQuestions + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.Score))
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
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + planPrePostUserResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.UsquiUserResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultIdWhereLike", "%" + planPrePostUserResultWhereLike.UsquiUserResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planPrePostUserResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planPrePostUserResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planPrePostUserResultWhereLike.UpdatedAt + "%");
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
                                planPrePostUserResult = new PlanPrePostUserResultModel();
                                if (!reader.IsDBNull(0))
                                    planPrePostUserResult.Id = reader.GetValue(0).ToString();
                                else
                                    planPrePostUserResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    planPrePostUserResult.PlanLessonId = reader.GetValue(1).ToString();
                                else
                                    planPrePostUserResult.PlanLessonId = null;
                                if (!reader.IsDBNull(2))
                                    planPrePostUserResult.UserId = reader.GetValue(2).ToString();
                                else
                                    planPrePostUserResult.UserId = null;
                                if (!reader.IsDBNull(3))
                                    planPrePostUserResult.QuizId = reader.GetValue(3).ToString();
                                else
                                    planPrePostUserResult.QuizId = null;
                                if (!reader.IsDBNull(4))
                                    planPrePostUserResult.PrePost = reader.GetValue(4).ToString();
                                else
                                    planPrePostUserResult.PrePost = null;
                                if (!reader.IsDBNull(5))
                                    planPrePostUserResult.TotalCorrect = reader.GetValue(5).ToString();
                                else
                                    planPrePostUserResult.TotalCorrect = null;
                                if (!reader.IsDBNull(6))
                                    planPrePostUserResult.TotalQuestions = reader.GetValue(6).ToString();
                                else
                                    planPrePostUserResult.TotalQuestions = null;
                                if (!reader.IsDBNull(7))
                                    planPrePostUserResult.Score = reader.GetValue(7).ToString();
                                else
                                    planPrePostUserResult.Score = null;
                                if (!reader.IsDBNull(8))
                                    planPrePostUserResult.UsquiUserResultId = reader.GetValue(8).ToString();
                                else
                                    planPrePostUserResult.UsquiUserResultId = null;
                                if (!reader.IsDBNull(9))
                                    planPrePostUserResult.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    planPrePostUserResult.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    planPrePostUserResult.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    planPrePostUserResult.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    planPrePostUserResult.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    planPrePostUserResult.UpdatedAt = null;
                                response.Add(planPrePostUserResult.Id, planPrePostUserResult);
                                planPrePostUserResult = new PlanPrePostUserResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PlanPrePostUserResultModel>();
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
                response = new Dictionary<string, PlanPrePostUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlanPrePostUserResultModel> ReadD(string connectionString, PlanPrePostUserResultModel planPrePostUserResult, bool orderByLastDate, int fromRow, int limit, PlanPrePostUserResultModel planPrePostUserResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanPrePostUserResultModel> response = new Dictionary<string, PlanPrePostUserResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_lesson_id`, `user_id`, `quiz_id`, `pre_post`, `total_correct`, `total_questions`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_pre_post_user_results ";
                {
                    if (planPrePostUserResult != null)
                    {
                        if (!String.IsNullOrEmpty(planPrePostUserResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", planPrePostUserResult.Id);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.PlanLessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` = @planLessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` = @planLessonId ");
                            }
                            cmd.Parameters.AddWithValue("@planLessonId", planPrePostUserResult.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", planPrePostUserResult.UserId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", planPrePostUserResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.PrePost))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `pre_post` = @prePost ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `pre_post` = @prePost ");
                            }
                            cmd.Parameters.AddWithValue("@prePost", planPrePostUserResult.PrePost);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.TotalCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_correct` = @totalCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_correct` = @totalCorrect ");
                            }
                            cmd.Parameters.AddWithValue("@totalCorrect", planPrePostUserResult.TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.TotalQuestions))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_questions` = @totalQuestions ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_questions` = @totalQuestions ");
                            }
                            cmd.Parameters.AddWithValue("@totalQuestions", planPrePostUserResult.TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.Score))
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
                            cmd.Parameters.AddWithValue("@score", planPrePostUserResult.Score);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.UsquiUserResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` = @usquiUserResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` = @usquiUserResultId ");
                            }
                            cmd.Parameters.AddWithValue("@usquiUserResultId", planPrePostUserResult.UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planPrePostUserResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planPrePostUserResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planPrePostUserResult.UpdatedAt);
                        }
                    }
                    if (planPrePostUserResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planPrePostUserResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.PlanLessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + planPrePostUserResultWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planPrePostUserResultWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + planPrePostUserResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.PrePost))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `pre_post` LIKE @prePostWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `pre_post` LIKE @prePostWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@prePostWhereLike", "%" + planPrePostUserResultWhereLike.PrePost + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.TotalCorrect))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalCorrectWhereLike", "%" + planPrePostUserResultWhereLike.TotalCorrect + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.TotalQuestions))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalQuestionsWhereLike", "%" + planPrePostUserResultWhereLike.TotalQuestions + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.Score))
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
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + planPrePostUserResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.UsquiUserResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultIdWhereLike", "%" + planPrePostUserResultWhereLike.UsquiUserResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planPrePostUserResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planPrePostUserResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planPrePostUserResultWhereLike.UpdatedAt + "%");
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
                                planPrePostUserResult = new PlanPrePostUserResultModel();
                                if (!reader.IsDBNull(0))
                                    planPrePostUserResult.Id = reader.GetValue(0).ToString();
                                else
                                    planPrePostUserResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    planPrePostUserResult.PlanLessonId = reader.GetValue(1).ToString();
                                else
                                    planPrePostUserResult.PlanLessonId = null;
                                if (!reader.IsDBNull(2))
                                    planPrePostUserResult.UserId = reader.GetValue(2).ToString();
                                else
                                    planPrePostUserResult.UserId = null;
                                if (!reader.IsDBNull(3))
                                    planPrePostUserResult.QuizId = reader.GetValue(3).ToString();
                                else
                                    planPrePostUserResult.QuizId = null;
                                if (!reader.IsDBNull(4))
                                    planPrePostUserResult.PrePost = reader.GetValue(4).ToString();
                                else
                                    planPrePostUserResult.PrePost = null;
                                if (!reader.IsDBNull(5))
                                    planPrePostUserResult.TotalCorrect = reader.GetValue(5).ToString();
                                else
                                    planPrePostUserResult.TotalCorrect = null;
                                if (!reader.IsDBNull(6))
                                    planPrePostUserResult.TotalQuestions = reader.GetValue(6).ToString();
                                else
                                    planPrePostUserResult.TotalQuestions = null;
                                if (!reader.IsDBNull(7))
                                    planPrePostUserResult.Score = reader.GetValue(7).ToString();
                                else
                                    planPrePostUserResult.Score = null;
                                if (!reader.IsDBNull(8))
                                    planPrePostUserResult.UsquiUserResultId = reader.GetValue(8).ToString();
                                else
                                    planPrePostUserResult.UsquiUserResultId = null;
                                if (!reader.IsDBNull(9))
                                    planPrePostUserResult.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    planPrePostUserResult.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    planPrePostUserResult.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    planPrePostUserResult.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    planPrePostUserResult.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    planPrePostUserResult.UpdatedAt = null;
                                response.Add(planPrePostUserResult.Id, planPrePostUserResult);
                                planPrePostUserResult = new PlanPrePostUserResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PlanPrePostUserResultModel>();
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
                response = new Dictionary<string, PlanPrePostUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlanPrePostUserResultModel> ReadD(string connectionString, List<PlanPrePostUserResultModel> planPrePostUserResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanPrePostUserResultModel> response = new Dictionary<string, PlanPrePostUserResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_lesson_id`, `user_id`, `quiz_id`, `pre_post`, `total_correct`, `total_questions`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_pre_post_user_results ";
                if (planPrePostUserResultWhere != null && planPrePostUserResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planPrePostUserResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planPrePostUserResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, planPrePostUserResultWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planPrePostUserResultWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, planPrePostUserResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].PrePost))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `pre_post` = @prePost" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@prePost" + i, planPrePostUserResultWhere[i].PrePost);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].TotalCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_correct` = @totalCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalCorrect" + i, planPrePostUserResultWhere[i].TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].TotalQuestions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_questions` = @totalQuestions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalQuestions" + i, planPrePostUserResultWhere[i].TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, planPrePostUserResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].UsquiUserResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `usqui_user_result_id` = @usquiUserResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planPrePostUserResultWhere[i].UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planPrePostUserResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planPrePostUserResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planPrePostUserResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planPrePostUserResultWhere.Count - 1)
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
                            PlanPrePostUserResultModel planPrePostUserResult = new PlanPrePostUserResultModel(); if (!reader.IsDBNull(0))
                                planPrePostUserResult.Id = reader.GetValue(0).ToString();
                            else
                                planPrePostUserResult.Id = null;
                            if (!reader.IsDBNull(1))
                                planPrePostUserResult.PlanLessonId = reader.GetValue(1).ToString();
                            else
                                planPrePostUserResult.PlanLessonId = null;
                            if (!reader.IsDBNull(2))
                                planPrePostUserResult.UserId = reader.GetValue(2).ToString();
                            else
                                planPrePostUserResult.UserId = null;
                            if (!reader.IsDBNull(3))
                                planPrePostUserResult.QuizId = reader.GetValue(3).ToString();
                            else
                                planPrePostUserResult.QuizId = null;
                            if (!reader.IsDBNull(4))
                                planPrePostUserResult.PrePost = reader.GetValue(4).ToString();
                            else
                                planPrePostUserResult.PrePost = null;
                            if (!reader.IsDBNull(5))
                                planPrePostUserResult.TotalCorrect = reader.GetValue(5).ToString();
                            else
                                planPrePostUserResult.TotalCorrect = null;
                            if (!reader.IsDBNull(6))
                                planPrePostUserResult.TotalQuestions = reader.GetValue(6).ToString();
                            else
                                planPrePostUserResult.TotalQuestions = null;
                            if (!reader.IsDBNull(7))
                                planPrePostUserResult.Score = reader.GetValue(7).ToString();
                            else
                                planPrePostUserResult.Score = null;
                            if (!reader.IsDBNull(8))
                                planPrePostUserResult.UsquiUserResultId = reader.GetValue(8).ToString();
                            else
                                planPrePostUserResult.UsquiUserResultId = null;
                            if (!reader.IsDBNull(9))
                                planPrePostUserResult.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                planPrePostUserResult.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                planPrePostUserResult.CreatedAt = reader.GetValue(10).ToString();
                            else
                                planPrePostUserResult.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                planPrePostUserResult.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                planPrePostUserResult.UpdatedAt = null;
                            response.Add(planPrePostUserResult.Id, planPrePostUserResult);
                            planPrePostUserResult = new PlanPrePostUserResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PlanPrePostUserResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlanPrePostUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlanPrePostUserResultModel> ReadD(string connectionString, List<PlanPrePostUserResultModel> planPrePostUserResultWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanPrePostUserResultModel> response = new Dictionary<string, PlanPrePostUserResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_lesson_id`, `user_id`, `quiz_id`, `pre_post`, `total_correct`, `total_questions`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_pre_post_user_results ";
                if (planPrePostUserResultWhere != null && planPrePostUserResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planPrePostUserResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planPrePostUserResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, planPrePostUserResultWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planPrePostUserResultWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, planPrePostUserResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].PrePost))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `pre_post` = @prePost" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@prePost" + i, planPrePostUserResultWhere[i].PrePost);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].TotalCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_correct` = @totalCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalCorrect" + i, planPrePostUserResultWhere[i].TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].TotalQuestions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_questions` = @totalQuestions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalQuestions" + i, planPrePostUserResultWhere[i].TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, planPrePostUserResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].UsquiUserResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `usqui_user_result_id` = @usquiUserResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planPrePostUserResultWhere[i].UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planPrePostUserResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planPrePostUserResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planPrePostUserResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planPrePostUserResultWhere.Count - 1)
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
                            PlanPrePostUserResultModel planPrePostUserResult = new PlanPrePostUserResultModel(); if (!reader.IsDBNull(0))
                                planPrePostUserResult.Id = reader.GetValue(0).ToString();
                            else
                                planPrePostUserResult.Id = null;
                            if (!reader.IsDBNull(1))
                                planPrePostUserResult.PlanLessonId = reader.GetValue(1).ToString();
                            else
                                planPrePostUserResult.PlanLessonId = null;
                            if (!reader.IsDBNull(2))
                                planPrePostUserResult.UserId = reader.GetValue(2).ToString();
                            else
                                planPrePostUserResult.UserId = null;
                            if (!reader.IsDBNull(3))
                                planPrePostUserResult.QuizId = reader.GetValue(3).ToString();
                            else
                                planPrePostUserResult.QuizId = null;
                            if (!reader.IsDBNull(4))
                                planPrePostUserResult.PrePost = reader.GetValue(4).ToString();
                            else
                                planPrePostUserResult.PrePost = null;
                            if (!reader.IsDBNull(5))
                                planPrePostUserResult.TotalCorrect = reader.GetValue(5).ToString();
                            else
                                planPrePostUserResult.TotalCorrect = null;
                            if (!reader.IsDBNull(6))
                                planPrePostUserResult.TotalQuestions = reader.GetValue(6).ToString();
                            else
                                planPrePostUserResult.TotalQuestions = null;
                            if (!reader.IsDBNull(7))
                                planPrePostUserResult.Score = reader.GetValue(7).ToString();
                            else
                                planPrePostUserResult.Score = null;
                            if (!reader.IsDBNull(8))
                                planPrePostUserResult.UsquiUserResultId = reader.GetValue(8).ToString();
                            else
                                planPrePostUserResult.UsquiUserResultId = null;
                            if (!reader.IsDBNull(9))
                                planPrePostUserResult.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                planPrePostUserResult.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                planPrePostUserResult.CreatedAt = reader.GetValue(10).ToString();
                            else
                                planPrePostUserResult.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                planPrePostUserResult.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                planPrePostUserResult.UpdatedAt = null;
                            response.Add(planPrePostUserResult.Id, planPrePostUserResult);
                            planPrePostUserResult = new PlanPrePostUserResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PlanPrePostUserResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlanPrePostUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanPrePostUserResultModel> Read(string connectionString, PlanPrePostUserResultModel planPrePostUserResult, bool firstOne, bool orderByLastDate, PlanPrePostUserResultModel planPrePostUserResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanPrePostUserResultModel> response = new List<PlanPrePostUserResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_lesson_id`, `user_id`, `quiz_id`, `pre_post`, `total_correct`, `total_questions`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_pre_post_user_results ";
                {
                    if (planPrePostUserResult != null)
                    {
                        if (!String.IsNullOrEmpty(planPrePostUserResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", planPrePostUserResult.Id);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.PlanLessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` = @planLessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` = @planLessonId ");
                            }
                            cmd.Parameters.AddWithValue("@planLessonId", planPrePostUserResult.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", planPrePostUserResult.UserId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", planPrePostUserResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.PrePost))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `pre_post` = @prePost ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `pre_post` = @prePost ");
                            }
                            cmd.Parameters.AddWithValue("@prePost", planPrePostUserResult.PrePost);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.TotalCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_correct` = @totalCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_correct` = @totalCorrect ");
                            }
                            cmd.Parameters.AddWithValue("@totalCorrect", planPrePostUserResult.TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.TotalQuestions))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_questions` = @totalQuestions ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_questions` = @totalQuestions ");
                            }
                            cmd.Parameters.AddWithValue("@totalQuestions", planPrePostUserResult.TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.Score))
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
                            cmd.Parameters.AddWithValue("@score", planPrePostUserResult.Score);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.UsquiUserResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` = @usquiUserResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` = @usquiUserResultId ");
                            }
                            cmd.Parameters.AddWithValue("@usquiUserResultId", planPrePostUserResult.UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planPrePostUserResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planPrePostUserResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planPrePostUserResult.UpdatedAt);
                        }
                    }
                    if (planPrePostUserResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planPrePostUserResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.PlanLessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + planPrePostUserResultWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planPrePostUserResultWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + planPrePostUserResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.PrePost))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `pre_post` LIKE @prePostWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `pre_post` LIKE @prePostWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@prePostWhereLike", "%" + planPrePostUserResultWhereLike.PrePost + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.TotalCorrect))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalCorrectWhereLike", "%" + planPrePostUserResultWhereLike.TotalCorrect + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.TotalQuestions))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalQuestionsWhereLike", "%" + planPrePostUserResultWhereLike.TotalQuestions + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.Score))
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
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + planPrePostUserResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.UsquiUserResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultIdWhereLike", "%" + planPrePostUserResultWhereLike.UsquiUserResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planPrePostUserResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planPrePostUserResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planPrePostUserResultWhereLike.UpdatedAt + "%");
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
                                planPrePostUserResult = new PlanPrePostUserResultModel();
                                if (!reader.IsDBNull(0))
                                    planPrePostUserResult.Id = reader.GetValue(0).ToString();
                                else
                                    planPrePostUserResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    planPrePostUserResult.PlanLessonId = reader.GetValue(1).ToString();
                                else
                                    planPrePostUserResult.PlanLessonId = null;
                                if (!reader.IsDBNull(2))
                                    planPrePostUserResult.UserId = reader.GetValue(2).ToString();
                                else
                                    planPrePostUserResult.UserId = null;
                                if (!reader.IsDBNull(3))
                                    planPrePostUserResult.QuizId = reader.GetValue(3).ToString();
                                else
                                    planPrePostUserResult.QuizId = null;
                                if (!reader.IsDBNull(4))
                                    planPrePostUserResult.PrePost = reader.GetValue(4).ToString();
                                else
                                    planPrePostUserResult.PrePost = null;
                                if (!reader.IsDBNull(5))
                                    planPrePostUserResult.TotalCorrect = reader.GetValue(5).ToString();
                                else
                                    planPrePostUserResult.TotalCorrect = null;
                                if (!reader.IsDBNull(6))
                                    planPrePostUserResult.TotalQuestions = reader.GetValue(6).ToString();
                                else
                                    planPrePostUserResult.TotalQuestions = null;
                                if (!reader.IsDBNull(7))
                                    planPrePostUserResult.Score = reader.GetValue(7).ToString();
                                else
                                    planPrePostUserResult.Score = null;
                                if (!reader.IsDBNull(8))
                                    planPrePostUserResult.UsquiUserResultId = reader.GetValue(8).ToString();
                                else
                                    planPrePostUserResult.UsquiUserResultId = null;
                                if (!reader.IsDBNull(9))
                                    planPrePostUserResult.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    planPrePostUserResult.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    planPrePostUserResult.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    planPrePostUserResult.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    planPrePostUserResult.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    planPrePostUserResult.UpdatedAt = null;
                                response.Add(planPrePostUserResult);
                                planPrePostUserResult = new PlanPrePostUserResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlanPrePostUserResultModel>();
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
                response = new List<PlanPrePostUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanPrePostUserResultModel> Read(string connectionString, PlanPrePostUserResultModel planPrePostUserResult, bool orderByLastDate, int fromRow, int limit, PlanPrePostUserResultModel planPrePostUserResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanPrePostUserResultModel> response = new List<PlanPrePostUserResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_lesson_id`, `user_id`, `quiz_id`, `pre_post`, `total_correct`, `total_questions`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_pre_post_user_results ";
                {
                    if (planPrePostUserResult != null)
                    {
                        if (!String.IsNullOrEmpty(planPrePostUserResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", planPrePostUserResult.Id);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.PlanLessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` = @planLessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` = @planLessonId ");
                            }
                            cmd.Parameters.AddWithValue("@planLessonId", planPrePostUserResult.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", planPrePostUserResult.UserId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", planPrePostUserResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.PrePost))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `pre_post` = @prePost ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `pre_post` = @prePost ");
                            }
                            cmd.Parameters.AddWithValue("@prePost", planPrePostUserResult.PrePost);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.TotalCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_correct` = @totalCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_correct` = @totalCorrect ");
                            }
                            cmd.Parameters.AddWithValue("@totalCorrect", planPrePostUserResult.TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.TotalQuestions))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_questions` = @totalQuestions ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_questions` = @totalQuestions ");
                            }
                            cmd.Parameters.AddWithValue("@totalQuestions", planPrePostUserResult.TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.Score))
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
                            cmd.Parameters.AddWithValue("@score", planPrePostUserResult.Score);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.UsquiUserResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` = @usquiUserResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` = @usquiUserResultId ");
                            }
                            cmd.Parameters.AddWithValue("@usquiUserResultId", planPrePostUserResult.UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planPrePostUserResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planPrePostUserResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planPrePostUserResult.UpdatedAt);
                        }
                    }
                    if (planPrePostUserResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planPrePostUserResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.PlanLessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + planPrePostUserResultWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planPrePostUserResultWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + planPrePostUserResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.PrePost))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `pre_post` LIKE @prePostWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `pre_post` LIKE @prePostWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@prePostWhereLike", "%" + planPrePostUserResultWhereLike.PrePost + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.TotalCorrect))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalCorrectWhereLike", "%" + planPrePostUserResultWhereLike.TotalCorrect + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.TotalQuestions))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalQuestionsWhereLike", "%" + planPrePostUserResultWhereLike.TotalQuestions + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.Score))
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
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + planPrePostUserResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.UsquiUserResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultIdWhereLike", "%" + planPrePostUserResultWhereLike.UsquiUserResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planPrePostUserResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planPrePostUserResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planPrePostUserResultWhereLike.UpdatedAt + "%");
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
                                planPrePostUserResult = new PlanPrePostUserResultModel();
                                if (!reader.IsDBNull(0))
                                    planPrePostUserResult.Id = reader.GetValue(0).ToString();
                                else
                                    planPrePostUserResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    planPrePostUserResult.PlanLessonId = reader.GetValue(1).ToString();
                                else
                                    planPrePostUserResult.PlanLessonId = null;
                                if (!reader.IsDBNull(2))
                                    planPrePostUserResult.UserId = reader.GetValue(2).ToString();
                                else
                                    planPrePostUserResult.UserId = null;
                                if (!reader.IsDBNull(3))
                                    planPrePostUserResult.QuizId = reader.GetValue(3).ToString();
                                else
                                    planPrePostUserResult.QuizId = null;
                                if (!reader.IsDBNull(4))
                                    planPrePostUserResult.PrePost = reader.GetValue(4).ToString();
                                else
                                    planPrePostUserResult.PrePost = null;
                                if (!reader.IsDBNull(5))
                                    planPrePostUserResult.TotalCorrect = reader.GetValue(5).ToString();
                                else
                                    planPrePostUserResult.TotalCorrect = null;
                                if (!reader.IsDBNull(6))
                                    planPrePostUserResult.TotalQuestions = reader.GetValue(6).ToString();
                                else
                                    planPrePostUserResult.TotalQuestions = null;
                                if (!reader.IsDBNull(7))
                                    planPrePostUserResult.Score = reader.GetValue(7).ToString();
                                else
                                    planPrePostUserResult.Score = null;
                                if (!reader.IsDBNull(8))
                                    planPrePostUserResult.UsquiUserResultId = reader.GetValue(8).ToString();
                                else
                                    planPrePostUserResult.UsquiUserResultId = null;
                                if (!reader.IsDBNull(9))
                                    planPrePostUserResult.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    planPrePostUserResult.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    planPrePostUserResult.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    planPrePostUserResult.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    planPrePostUserResult.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    planPrePostUserResult.UpdatedAt = null;
                                response.Add(planPrePostUserResult);
                                planPrePostUserResult = new PlanPrePostUserResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlanPrePostUserResultModel>();
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
                response = new List<PlanPrePostUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanPrePostUserResultModel> Read(string connectionString, List<PlanPrePostUserResultModel> planPrePostUserResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanPrePostUserResultModel> response = new List<PlanPrePostUserResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_lesson_id`, `user_id`, `quiz_id`, `pre_post`, `total_correct`, `total_questions`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_pre_post_user_results ";
                if (planPrePostUserResultWhere != null && planPrePostUserResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planPrePostUserResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planPrePostUserResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, planPrePostUserResultWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planPrePostUserResultWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, planPrePostUserResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].PrePost))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `pre_post` = @prePost" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@prePost" + i, planPrePostUserResultWhere[i].PrePost);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].TotalCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_correct` = @totalCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalCorrect" + i, planPrePostUserResultWhere[i].TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].TotalQuestions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_questions` = @totalQuestions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalQuestions" + i, planPrePostUserResultWhere[i].TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, planPrePostUserResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].UsquiUserResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `usqui_user_result_id` = @usquiUserResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planPrePostUserResultWhere[i].UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planPrePostUserResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planPrePostUserResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planPrePostUserResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planPrePostUserResultWhere.Count - 1)
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
                            PlanPrePostUserResultModel planPrePostUserResult = new PlanPrePostUserResultModel(); if (!reader.IsDBNull(0))
                                planPrePostUserResult.Id = reader.GetValue(0).ToString();
                            else
                                planPrePostUserResult.Id = null;
                            if (!reader.IsDBNull(1))
                                planPrePostUserResult.PlanLessonId = reader.GetValue(1).ToString();
                            else
                                planPrePostUserResult.PlanLessonId = null;
                            if (!reader.IsDBNull(2))
                                planPrePostUserResult.UserId = reader.GetValue(2).ToString();
                            else
                                planPrePostUserResult.UserId = null;
                            if (!reader.IsDBNull(3))
                                planPrePostUserResult.QuizId = reader.GetValue(3).ToString();
                            else
                                planPrePostUserResult.QuizId = null;
                            if (!reader.IsDBNull(4))
                                planPrePostUserResult.PrePost = reader.GetValue(4).ToString();
                            else
                                planPrePostUserResult.PrePost = null;
                            if (!reader.IsDBNull(5))
                                planPrePostUserResult.TotalCorrect = reader.GetValue(5).ToString();
                            else
                                planPrePostUserResult.TotalCorrect = null;
                            if (!reader.IsDBNull(6))
                                planPrePostUserResult.TotalQuestions = reader.GetValue(6).ToString();
                            else
                                planPrePostUserResult.TotalQuestions = null;
                            if (!reader.IsDBNull(7))
                                planPrePostUserResult.Score = reader.GetValue(7).ToString();
                            else
                                planPrePostUserResult.Score = null;
                            if (!reader.IsDBNull(8))
                                planPrePostUserResult.UsquiUserResultId = reader.GetValue(8).ToString();
                            else
                                planPrePostUserResult.UsquiUserResultId = null;
                            if (!reader.IsDBNull(9))
                                planPrePostUserResult.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                planPrePostUserResult.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                planPrePostUserResult.CreatedAt = reader.GetValue(10).ToString();
                            else
                                planPrePostUserResult.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                planPrePostUserResult.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                planPrePostUserResult.UpdatedAt = null;
                            response.Add(planPrePostUserResult);
                            planPrePostUserResult = new PlanPrePostUserResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlanPrePostUserResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanPrePostUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanPrePostUserResultModel> Read(string connectionString, List<PlanPrePostUserResultModel> planPrePostUserResultWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanPrePostUserResultModel> response = new List<PlanPrePostUserResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_lesson_id`, `user_id`, `quiz_id`, `pre_post`, `total_correct`, `total_questions`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_pre_post_user_results ";
                if (planPrePostUserResultWhere != null && planPrePostUserResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planPrePostUserResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planPrePostUserResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, planPrePostUserResultWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planPrePostUserResultWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, planPrePostUserResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].PrePost))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `pre_post` = @prePost" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@prePost" + i, planPrePostUserResultWhere[i].PrePost);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].TotalCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_correct` = @totalCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalCorrect" + i, planPrePostUserResultWhere[i].TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].TotalQuestions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_questions` = @totalQuestions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalQuestions" + i, planPrePostUserResultWhere[i].TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, planPrePostUserResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].UsquiUserResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `usqui_user_result_id` = @usquiUserResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planPrePostUserResultWhere[i].UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planPrePostUserResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planPrePostUserResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planPrePostUserResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planPrePostUserResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planPrePostUserResultWhere.Count - 1)
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
                            PlanPrePostUserResultModel planPrePostUserResult = new PlanPrePostUserResultModel(); if (!reader.IsDBNull(0))
                                planPrePostUserResult.Id = reader.GetValue(0).ToString();
                            else
                                planPrePostUserResult.Id = null;
                            if (!reader.IsDBNull(1))
                                planPrePostUserResult.PlanLessonId = reader.GetValue(1).ToString();
                            else
                                planPrePostUserResult.PlanLessonId = null;
                            if (!reader.IsDBNull(2))
                                planPrePostUserResult.UserId = reader.GetValue(2).ToString();
                            else
                                planPrePostUserResult.UserId = null;
                            if (!reader.IsDBNull(3))
                                planPrePostUserResult.QuizId = reader.GetValue(3).ToString();
                            else
                                planPrePostUserResult.QuizId = null;
                            if (!reader.IsDBNull(4))
                                planPrePostUserResult.PrePost = reader.GetValue(4).ToString();
                            else
                                planPrePostUserResult.PrePost = null;
                            if (!reader.IsDBNull(5))
                                planPrePostUserResult.TotalCorrect = reader.GetValue(5).ToString();
                            else
                                planPrePostUserResult.TotalCorrect = null;
                            if (!reader.IsDBNull(6))
                                planPrePostUserResult.TotalQuestions = reader.GetValue(6).ToString();
                            else
                                planPrePostUserResult.TotalQuestions = null;
                            if (!reader.IsDBNull(7))
                                planPrePostUserResult.Score = reader.GetValue(7).ToString();
                            else
                                planPrePostUserResult.Score = null;
                            if (!reader.IsDBNull(8))
                                planPrePostUserResult.UsquiUserResultId = reader.GetValue(8).ToString();
                            else
                                planPrePostUserResult.UsquiUserResultId = null;
                            if (!reader.IsDBNull(9))
                                planPrePostUserResult.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                planPrePostUserResult.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                planPrePostUserResult.CreatedAt = reader.GetValue(10).ToString();
                            else
                                planPrePostUserResult.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                planPrePostUserResult.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                planPrePostUserResult.UpdatedAt = null;
                            response.Add(planPrePostUserResult);
                            planPrePostUserResult = new PlanPrePostUserResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlanPrePostUserResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanPrePostUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanPrePostUserResultModel> Create(string connectionString, PlanPrePostUserResultModel planPrePostUserResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanPrePostUserResultModel> response = new List<PlanPrePostUserResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO plan_pre_post_user_results (";
                string values = "(";
                if (!String.IsNullOrEmpty(planPrePostUserResult.Id))
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
                    cmd.Parameters.AddWithValue("@id", planPrePostUserResult.Id);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.PlanLessonId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `plan_lesson_id` ");
                        values = String.Concat(values, ", @planLessonId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `plan_lesson_id` ");
                        values = String.Concat(values, " @planLessonId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planLessonId", planPrePostUserResult.PlanLessonId);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.UserId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `user_id` ");
                        values = String.Concat(values, ", @userId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `user_id` ");
                        values = String.Concat(values, " @userId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", planPrePostUserResult.UserId);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.QuizId))
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
                    cmd.Parameters.AddWithValue("@quizId", planPrePostUserResult.QuizId);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.PrePost))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `pre_post` ");
                        values = String.Concat(values, ", @prePost");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `pre_post` ");
                        values = String.Concat(values, " @prePost");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@prePost", planPrePostUserResult.PrePost);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.TotalCorrect))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `total_correct` ");
                        values = String.Concat(values, ", @totalCorrect");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `total_correct` ");
                        values = String.Concat(values, " @totalCorrect");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalCorrect", planPrePostUserResult.TotalCorrect);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.TotalQuestions))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `total_questions` ");
                        values = String.Concat(values, ", @totalQuestions");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `total_questions` ");
                        values = String.Concat(values, " @totalQuestions");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalQuestions", planPrePostUserResult.TotalQuestions);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.Score))
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
                    cmd.Parameters.AddWithValue("@score", planPrePostUserResult.Score);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.UsquiUserResultId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `usqui_user_result_id` ");
                        values = String.Concat(values, ", @usquiUserResultId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `usqui_user_result_id` ");
                        values = String.Concat(values, " @usquiUserResultId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@usquiUserResultId", planPrePostUserResult.UsquiUserResultId);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", planPrePostUserResult.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", planPrePostUserResult.CreatedAt);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", planPrePostUserResult.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                planPrePostUserResult.Id = cmd.LastInsertedId.ToString();
                response.Add(planPrePostUserResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanPrePostUserResultModel>();
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
        public List<PlanPrePostUserResultModel> Create(string connectionString, List<PlanPrePostUserResultModel> planPrePostUserResultList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanPrePostUserResultModel> response = new List<PlanPrePostUserResultModel>();
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
                        for (int i = 0; i < planPrePostUserResultList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO plan_pre_post_user_results (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, planPrePostUserResultList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].PlanLessonId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `plan_lesson_id` ");
                                        values = String.Concat(values, ", @planLessonId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `plan_lesson_id` ");
                                        values = String.Concat(values, "@planLessonId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planLessonId" + i, planPrePostUserResultList[i].PlanLessonId);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].UserId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `user_id` ");
                                        values = String.Concat(values, ", @userId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `user_id` ");
                                        values = String.Concat(values, "@userId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, planPrePostUserResultList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].QuizId))
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
                                    cmd.Parameters.AddWithValue("@quizId" + i, planPrePostUserResultList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].PrePost))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `pre_post` ");
                                        values = String.Concat(values, ", @prePost" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `pre_post` ");
                                        values = String.Concat(values, "@prePost" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@prePost" + i, planPrePostUserResultList[i].PrePost);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].TotalCorrect))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `total_correct` ");
                                        values = String.Concat(values, ", @totalCorrect" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `total_correct` ");
                                        values = String.Concat(values, "@totalCorrect" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalCorrect" + i, planPrePostUserResultList[i].TotalCorrect);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].TotalQuestions))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `total_questions` ");
                                        values = String.Concat(values, ", @totalQuestions" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `total_questions` ");
                                        values = String.Concat(values, "@totalQuestions" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalQuestions" + i, planPrePostUserResultList[i].TotalQuestions);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].Score))
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
                                    cmd.Parameters.AddWithValue("@score" + i, planPrePostUserResultList[i].Score);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].UsquiUserResultId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `usqui_user_result_id` ");
                                        values = String.Concat(values, ", @usquiUserResultId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `usqui_user_result_id` ");
                                        values = String.Concat(values, "@usquiUserResultId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planPrePostUserResultList[i].UsquiUserResultId);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planPrePostUserResultList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planPrePostUserResultList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planPrePostUserResultList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(planPrePostUserResultList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanPrePostUserResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanPrePostUserResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanPrePostUserResultModel>();
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

        public List<PlanPrePostUserResultModel> Update(string connectionString, PlanPrePostUserResultModel planPrePostUserResult, PlanPrePostUserResultModel planPrePostUserResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanPrePostUserResultModel> response = new List<PlanPrePostUserResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE plan_pre_post_user_results SET ";
                if (!String.IsNullOrEmpty(planPrePostUserResult.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", planPrePostUserResult.Id);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.PlanLessonId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `plan_lesson_id` = @planLessonId");
                    else
                        sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planLessonId", planPrePostUserResult.PlanLessonId);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", planPrePostUserResult.UserId);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.QuizId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `quiz_id` = @quizId");
                    else
                        sql = String.Concat(sql, " `quiz_id` = @quizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", planPrePostUserResult.QuizId);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.PrePost))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `pre_post` = @prePost");
                    else
                        sql = String.Concat(sql, " `pre_post` = @prePost");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@prePost", planPrePostUserResult.PrePost);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.TotalCorrect))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `total_correct` = @totalCorrect");
                    else
                        sql = String.Concat(sql, " `total_correct` = @totalCorrect");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalCorrect", planPrePostUserResult.TotalCorrect);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.TotalQuestions))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `total_questions` = @totalQuestions");
                    else
                        sql = String.Concat(sql, " `total_questions` = @totalQuestions");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalQuestions", planPrePostUserResult.TotalQuestions);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.Score))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `score` = @score");
                    else
                        sql = String.Concat(sql, " `score` = @score");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@score", planPrePostUserResult.Score);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.UsquiUserResultId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `usqui_user_result_id` = @usquiUserResultId");
                    else
                        sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@usquiUserResultId", planPrePostUserResult.UsquiUserResultId);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", planPrePostUserResult.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", planPrePostUserResult.CreatedAt);
                }
                if (!String.IsNullOrEmpty(planPrePostUserResult.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", planPrePostUserResult.UpdatedAt);
                }
                if (planPrePostUserResultWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(planPrePostUserResultWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", planPrePostUserResultWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResultWhere.PlanLessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonIdWhere");
                        else
                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planLessonIdWhere", planPrePostUserResultWhere.PlanLessonId);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResultWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", planPrePostUserResultWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResultWhere.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizIdWhere", planPrePostUserResultWhere.QuizId);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResultWhere.PrePost))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `pre_post` = @prePostWhere");
                        else
                            sql = String.Concat(sql, " `pre_post` = @prePostWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@prePostWhere", planPrePostUserResultWhere.PrePost);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResultWhere.TotalCorrect))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `total_correct` = @totalCorrectWhere");
                        else
                            sql = String.Concat(sql, " `total_correct` = @totalCorrectWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalCorrectWhere", planPrePostUserResultWhere.TotalCorrect);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResultWhere.TotalQuestions))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `total_questions` = @totalQuestionsWhere");
                        else
                            sql = String.Concat(sql, " `total_questions` = @totalQuestionsWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalQuestionsWhere", planPrePostUserResultWhere.TotalQuestions);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResultWhere.Score))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `score` = @scoreWhere");
                        else
                            sql = String.Concat(sql, " `score` = @scoreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@scoreWhere", planPrePostUserResultWhere.Score);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResultWhere.UsquiUserResultId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `usqui_user_result_id` = @usquiUserResultIdWhere");
                        else
                            sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@usquiUserResultIdWhere", planPrePostUserResultWhere.UsquiUserResultId);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResultWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", planPrePostUserResultWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResultWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", planPrePostUserResultWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResultWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", planPrePostUserResultWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(planPrePostUserResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanPrePostUserResultModel>();
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

        public List<PlanPrePostUserResultModel> Update(string connectionString, List<PlanPrePostUserResultModel> planPrePostUserResultList, List<PlanPrePostUserResultModel> planPrePostUserResultWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanPrePostUserResultModel> response = new List<PlanPrePostUserResultModel>();
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
                        for (int i = 0; i < planPrePostUserResultList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE plan_pre_post_user_results SET ";
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, planPrePostUserResultList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].PlanLessonId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `plan_lesson_id` = @planLessonId" + i);
                                    else
                                        sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planLessonId" + i, planPrePostUserResultList[i].PlanLessonId);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, planPrePostUserResultList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].QuizId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `quiz_id` = @quizId" + i);
                                    else
                                        sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, planPrePostUserResultList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].PrePost))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `pre_post` = @prePost" + i);
                                    else
                                        sql = String.Concat(sql, " `pre_post` = @prePost" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@prePost" + i, planPrePostUserResultList[i].PrePost);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].TotalCorrect))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `total_correct` = @totalCorrect" + i);
                                    else
                                        sql = String.Concat(sql, " `total_correct` = @totalCorrect" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalCorrect" + i, planPrePostUserResultList[i].TotalCorrect);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].TotalQuestions))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `total_questions` = @totalQuestions" + i);
                                    else
                                        sql = String.Concat(sql, " `total_questions` = @totalQuestions" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalQuestions" + i, planPrePostUserResultList[i].TotalQuestions);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].Score))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `score` = @score" + i);
                                    else
                                        sql = String.Concat(sql, " `score` = @score" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@score" + i, planPrePostUserResultList[i].Score);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].UsquiUserResultId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `usqui_user_result_id` = @usquiUserResultId" + i);
                                    else
                                        sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planPrePostUserResultList[i].UsquiUserResultId);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planPrePostUserResultList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planPrePostUserResultList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(planPrePostUserResultList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planPrePostUserResultList[i].UpdatedAt);
                                }
                                if (planPrePostUserResultWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(planPrePostUserResultWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, planPrePostUserResultWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResultWhereList[i].PlanLessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planLessonIdWhere" + i, planPrePostUserResultWhereList[i].PlanLessonId);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResultWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, planPrePostUserResultWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResultWhereList[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizIdWhere" + i, planPrePostUserResultWhereList[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResultWhereList[i].PrePost))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `pre_post` = @prePostWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `pre_post` = @prePostWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@prePostWhere" + i, planPrePostUserResultWhereList[i].PrePost);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResultWhereList[i].TotalCorrect))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `total_correct` = @totalCorrectWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `total_correct` = @totalCorrectWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalCorrectWhere" + i, planPrePostUserResultWhereList[i].TotalCorrect);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResultWhereList[i].TotalQuestions))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `total_questions` = @totalQuestionsWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `total_questions` = @totalQuestionsWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalQuestionsWhere" + i, planPrePostUserResultWhereList[i].TotalQuestions);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResultWhereList[i].Score))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `score` = @scoreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `score` = @scoreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@scoreWhere" + i, planPrePostUserResultWhereList[i].Score);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResultWhereList[i].UsquiUserResultId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `usqui_user_result_id` = @usquiUserResultIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@usquiUserResultIdWhere" + i, planPrePostUserResultWhereList[i].UsquiUserResultId);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResultWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, planPrePostUserResultWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResultWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, planPrePostUserResultWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResultWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, planPrePostUserResultWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(planPrePostUserResultList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanPrePostUserResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanPrePostUserResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanPrePostUserResultModel>();
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

        public List<PlanPrePostUserResultModel> Delete(string connectionString, PlanPrePostUserResultModel planPrePostUserResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanPrePostUserResultModel> response = new List<PlanPrePostUserResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM plan_pre_post_user_results  ";
                if (planPrePostUserResult != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(planPrePostUserResult.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", planPrePostUserResult.Id);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResult.PlanLessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonId");
                        else
                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planLessonId", planPrePostUserResult.PlanLessonId);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResult.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", planPrePostUserResult.UserId);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResult.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizId");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizId", planPrePostUserResult.QuizId);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResult.PrePost))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `pre_post` = @prePost");
                        else
                            sql = String.Concat(sql, " `pre_post` = @prePost");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@prePost", planPrePostUserResult.PrePost);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResult.TotalCorrect))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `total_correct` = @totalCorrect");
                        else
                            sql = String.Concat(sql, " `total_correct` = @totalCorrect");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalCorrect", planPrePostUserResult.TotalCorrect);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResult.TotalQuestions))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `total_questions` = @totalQuestions");
                        else
                            sql = String.Concat(sql, " `total_questions` = @totalQuestions");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalQuestions", planPrePostUserResult.TotalQuestions);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResult.Score))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `score` = @score");
                        else
                            sql = String.Concat(sql, " `score` = @score");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@score", planPrePostUserResult.Score);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResult.UsquiUserResultId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `usqui_user_result_id` = @usquiUserResultId");
                        else
                            sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@usquiUserResultId", planPrePostUserResult.UsquiUserResultId);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResult.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", planPrePostUserResult.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResult.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", planPrePostUserResult.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(planPrePostUserResult.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", planPrePostUserResult.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(planPrePostUserResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanPrePostUserResultModel>();
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

        public List<PlanPrePostUserResultModel> Delete(string connectionString, List<PlanPrePostUserResultModel> planPrePostUserResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanPrePostUserResultModel> response = new List<PlanPrePostUserResultModel>();
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
                        for (int i = 0; i < planPrePostUserResult.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM plan_pre_post_user_results  ";
                                if (planPrePostUserResult[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(planPrePostUserResult[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, planPrePostUserResult[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResult[i].PlanLessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonId" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planLessonId" + i, planPrePostUserResult[i].PlanLessonId);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResult[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, planPrePostUserResult[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResult[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizId" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizId" + i, planPrePostUserResult[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResult[i].PrePost))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `pre_post` = @prePost" + i);
                                        else
                                            sql = String.Concat(sql, " `pre_post` = @prePost" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@prePost" + i, planPrePostUserResult[i].PrePost);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResult[i].TotalCorrect))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `total_correct` = @totalCorrect" + i);
                                        else
                                            sql = String.Concat(sql, " `total_correct` = @totalCorrect" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalCorrect" + i, planPrePostUserResult[i].TotalCorrect);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResult[i].TotalQuestions))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `total_questions` = @totalQuestions" + i);
                                        else
                                            sql = String.Concat(sql, " `total_questions` = @totalQuestions" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalQuestions" + i, planPrePostUserResult[i].TotalQuestions);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResult[i].Score))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `score` = @score" + i);
                                        else
                                            sql = String.Concat(sql, " `score` = @score" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@score" + i, planPrePostUserResult[i].Score);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResult[i].UsquiUserResultId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `usqui_user_result_id` = @usquiUserResultId" + i);
                                        else
                                            sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planPrePostUserResult[i].UsquiUserResultId);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResult[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, planPrePostUserResult[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResult[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, planPrePostUserResult[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(planPrePostUserResult[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, planPrePostUserResult[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(planPrePostUserResult[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanPrePostUserResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanPrePostUserResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanPrePostUserResultModel>();
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
