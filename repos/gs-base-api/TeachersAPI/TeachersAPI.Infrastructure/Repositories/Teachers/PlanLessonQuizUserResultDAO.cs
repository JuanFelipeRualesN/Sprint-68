using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Teachers;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    class PlanLessonQuizUserResultDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, PlanLessonQuizUserResultModel> ReadD(string connectionString, PlanLessonQuizUserResultModel planLessonQuizUserResult, bool firstOne, bool orderByLastDate, PlanLessonQuizUserResultModel planLessonQuizUserResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanLessonQuizUserResultModel> response = new Dictionary<string, PlanLessonQuizUserResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_lesson_id`, `manually_corrected_by_teacher`, `user_id`, `quiz_id`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_quiz_user_results ";
                {
                    if (planLessonQuizUserResult != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", planLessonQuizUserResult.Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonId", planLessonQuizUserResult.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher", planLessonQuizUserResult.ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", planLessonQuizUserResult.UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", planLessonQuizUserResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.Score))
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
                            cmd.Parameters.AddWithValue("@score", planLessonQuizUserResult.Score);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.UsquiUserResultId))
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
                            cmd.Parameters.AddWithValue("@usquiUserResultId", planLessonQuizUserResult.UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planLessonQuizUserResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planLessonQuizUserResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planLessonQuizUserResult.UpdatedAt);
                        }
                    }
                    if (planLessonQuizUserResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planLessonQuizUserResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + planLessonQuizUserResultWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacherWhereLike", "%" + planLessonQuizUserResultWhereLike.ManuallyCorrectedByTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planLessonQuizUserResultWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + planLessonQuizUserResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.Score))
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
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + planLessonQuizUserResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.UsquiUserResultId))
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
                            cmd.Parameters.AddWithValue("@usquiUserResultIdWhereLike", "%" + planLessonQuizUserResultWhereLike.UsquiUserResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planLessonQuizUserResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planLessonQuizUserResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planLessonQuizUserResultWhereLike.UpdatedAt + "%");
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
                                planLessonQuizUserResult = new PlanLessonQuizUserResultModel();
                                if (!reader.IsDBNull(0))
                                    planLessonQuizUserResult.Id = reader.GetValue(0).ToString();
                                else
                                    planLessonQuizUserResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    planLessonQuizUserResult.PlanLessonId = reader.GetValue(1).ToString();
                                else
                                    planLessonQuizUserResult.PlanLessonId = null;
                                if (!reader.IsDBNull(2))
                                    planLessonQuizUserResult.ManuallyCorrectedByTeacher = reader.GetValue(2).ToString();
                                else
                                    planLessonQuizUserResult.ManuallyCorrectedByTeacher = null;
                                if (!reader.IsDBNull(3))
                                    planLessonQuizUserResult.UserId = reader.GetValue(3).ToString();
                                else
                                    planLessonQuizUserResult.UserId = null;
                                if (!reader.IsDBNull(4))
                                    planLessonQuizUserResult.QuizId = reader.GetValue(4).ToString();
                                else
                                    planLessonQuizUserResult.QuizId = null;
                                if (!reader.IsDBNull(5))
                                    planLessonQuizUserResult.Score = reader.GetValue(5).ToString();
                                else
                                    planLessonQuizUserResult.Score = null;
                                if (!reader.IsDBNull(6))
                                    planLessonQuizUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                                else
                                    planLessonQuizUserResult.UsquiUserResultId = null;
                                if (!reader.IsDBNull(7))
                                    planLessonQuizUserResult.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    planLessonQuizUserResult.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    planLessonQuizUserResult.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    planLessonQuizUserResult.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    planLessonQuizUserResult.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    planLessonQuizUserResult.UpdatedAt = null;
                                response.Add(planLessonQuizUserResult.Id, planLessonQuizUserResult);
                                planLessonQuizUserResult = new PlanLessonQuizUserResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PlanLessonQuizUserResultModel>();
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
                response = new Dictionary<string, PlanLessonQuizUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlanLessonQuizUserResultModel> ReadD(string connectionString, PlanLessonQuizUserResultModel planLessonQuizUserResult, bool orderByLastDate, int fromRow, int limit, PlanLessonQuizUserResultModel planLessonQuizUserResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanLessonQuizUserResultModel> response = new Dictionary<string, PlanLessonQuizUserResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_lesson_id`, `manually_corrected_by_teacher`, `user_id`, `quiz_id`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_quiz_user_results ";
                {
                    if (planLessonQuizUserResult != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", planLessonQuizUserResult.Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonId", planLessonQuizUserResult.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher", planLessonQuizUserResult.ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", planLessonQuizUserResult.UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", planLessonQuizUserResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.Score))
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
                            cmd.Parameters.AddWithValue("@score", planLessonQuizUserResult.Score);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.UsquiUserResultId))
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
                            cmd.Parameters.AddWithValue("@usquiUserResultId", planLessonQuizUserResult.UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planLessonQuizUserResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planLessonQuizUserResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planLessonQuizUserResult.UpdatedAt);
                        }
                    }
                    if (planLessonQuizUserResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planLessonQuizUserResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + planLessonQuizUserResultWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacherWhereLike", "%" + planLessonQuizUserResultWhereLike.ManuallyCorrectedByTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planLessonQuizUserResultWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + planLessonQuizUserResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.Score))
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
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + planLessonQuizUserResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.UsquiUserResultId))
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
                            cmd.Parameters.AddWithValue("@usquiUserResultIdWhereLike", "%" + planLessonQuizUserResultWhereLike.UsquiUserResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planLessonQuizUserResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planLessonQuizUserResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planLessonQuizUserResultWhereLike.UpdatedAt + "%");
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
                                planLessonQuizUserResult = new PlanLessonQuizUserResultModel();
                                if (!reader.IsDBNull(0))
                                    planLessonQuizUserResult.Id = reader.GetValue(0).ToString();
                                else
                                    planLessonQuizUserResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    planLessonQuizUserResult.PlanLessonId = reader.GetValue(1).ToString();
                                else
                                    planLessonQuizUserResult.PlanLessonId = null;
                                if (!reader.IsDBNull(2))
                                    planLessonQuizUserResult.ManuallyCorrectedByTeacher = reader.GetValue(2).ToString();
                                else
                                    planLessonQuizUserResult.ManuallyCorrectedByTeacher = null;
                                if (!reader.IsDBNull(3))
                                    planLessonQuizUserResult.UserId = reader.GetValue(3).ToString();
                                else
                                    planLessonQuizUserResult.UserId = null;
                                if (!reader.IsDBNull(4))
                                    planLessonQuizUserResult.QuizId = reader.GetValue(4).ToString();
                                else
                                    planLessonQuizUserResult.QuizId = null;
                                if (!reader.IsDBNull(5))
                                    planLessonQuizUserResult.Score = reader.GetValue(5).ToString();
                                else
                                    planLessonQuizUserResult.Score = null;
                                if (!reader.IsDBNull(6))
                                    planLessonQuizUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                                else
                                    planLessonQuizUserResult.UsquiUserResultId = null;
                                if (!reader.IsDBNull(7))
                                    planLessonQuizUserResult.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    planLessonQuizUserResult.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    planLessonQuizUserResult.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    planLessonQuizUserResult.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    planLessonQuizUserResult.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    planLessonQuizUserResult.UpdatedAt = null;
                                response.Add(planLessonQuizUserResult.Id, planLessonQuizUserResult);
                                planLessonQuizUserResult = new PlanLessonQuizUserResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PlanLessonQuizUserResultModel>();
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
                response = new Dictionary<string, PlanLessonQuizUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlanLessonQuizUserResultModel> ReadD(string connectionString, List<PlanLessonQuizUserResultModel> planLessonQuizUserResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanLessonQuizUserResultModel> response = new Dictionary<string, PlanLessonQuizUserResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_lesson_id`, `manually_corrected_by_teacher`, `user_id`, `quiz_id`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_quiz_user_results ";
                if (planLessonQuizUserResultWhere != null && planLessonQuizUserResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planLessonQuizUserResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planLessonQuizUserResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, planLessonQuizUserResultWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].ManuallyCorrectedByTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher" + i, planLessonQuizUserResultWhere[i].ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planLessonQuizUserResultWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, planLessonQuizUserResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, planLessonQuizUserResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].UsquiUserResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `usqui_user_result_id` = @usquiUserResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planLessonQuizUserResultWhere[i].UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planLessonQuizUserResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planLessonQuizUserResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planLessonQuizUserResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planLessonQuizUserResultWhere.Count - 1)
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
                            PlanLessonQuizUserResultModel planLessonQuizUserResult = new PlanLessonQuizUserResultModel(); if (!reader.IsDBNull(0))
                                planLessonQuizUserResult.Id = reader.GetValue(0).ToString();
                            else
                                planLessonQuizUserResult.Id = null;
                            if (!reader.IsDBNull(1))
                                planLessonQuizUserResult.PlanLessonId = reader.GetValue(1).ToString();
                            else
                                planLessonQuizUserResult.PlanLessonId = null;
                            if (!reader.IsDBNull(2))
                                planLessonQuizUserResult.ManuallyCorrectedByTeacher = reader.GetValue(2).ToString();
                            else
                                planLessonQuizUserResult.ManuallyCorrectedByTeacher = null;
                            if (!reader.IsDBNull(3))
                                planLessonQuizUserResult.UserId = reader.GetValue(3).ToString();
                            else
                                planLessonQuizUserResult.UserId = null;
                            if (!reader.IsDBNull(4))
                                planLessonQuizUserResult.QuizId = reader.GetValue(4).ToString();
                            else
                                planLessonQuizUserResult.QuizId = null;
                            if (!reader.IsDBNull(5))
                                planLessonQuizUserResult.Score = reader.GetValue(5).ToString();
                            else
                                planLessonQuizUserResult.Score = null;
                            if (!reader.IsDBNull(6))
                                planLessonQuizUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                            else
                                planLessonQuizUserResult.UsquiUserResultId = null;
                            if (!reader.IsDBNull(7))
                                planLessonQuizUserResult.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                planLessonQuizUserResult.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                planLessonQuizUserResult.CreatedAt = reader.GetValue(8).ToString();
                            else
                                planLessonQuizUserResult.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                planLessonQuizUserResult.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                planLessonQuizUserResult.UpdatedAt = null;
                            response.Add(planLessonQuizUserResult.Id, planLessonQuizUserResult);
                            planLessonQuizUserResult = new PlanLessonQuizUserResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PlanLessonQuizUserResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlanLessonQuizUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlanLessonQuizUserResultModel> ReadD(string connectionString, List<PlanLessonQuizUserResultModel> planLessonQuizUserResultWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanLessonQuizUserResultModel> response = new Dictionary<string, PlanLessonQuizUserResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_lesson_id`, `manually_corrected_by_teacher`, `user_id`, `quiz_id`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_quiz_user_results ";
                if (planLessonQuizUserResultWhere != null && planLessonQuizUserResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planLessonQuizUserResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planLessonQuizUserResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, planLessonQuizUserResultWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].ManuallyCorrectedByTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher" + i, planLessonQuizUserResultWhere[i].ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planLessonQuizUserResultWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, planLessonQuizUserResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, planLessonQuizUserResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].UsquiUserResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `usqui_user_result_id` = @usquiUserResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planLessonQuizUserResultWhere[i].UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planLessonQuizUserResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planLessonQuizUserResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planLessonQuizUserResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planLessonQuizUserResultWhere.Count - 1)
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
                            PlanLessonQuizUserResultModel planLessonQuizUserResult = new PlanLessonQuizUserResultModel(); if (!reader.IsDBNull(0))
                                planLessonQuizUserResult.Id = reader.GetValue(0).ToString();
                            else
                                planLessonQuizUserResult.Id = null;
                            if (!reader.IsDBNull(1))
                                planLessonQuizUserResult.PlanLessonId = reader.GetValue(1).ToString();
                            else
                                planLessonQuizUserResult.PlanLessonId = null;
                            if (!reader.IsDBNull(2))
                                planLessonQuizUserResult.ManuallyCorrectedByTeacher = reader.GetValue(2).ToString();
                            else
                                planLessonQuizUserResult.ManuallyCorrectedByTeacher = null;
                            if (!reader.IsDBNull(3))
                                planLessonQuizUserResult.UserId = reader.GetValue(3).ToString();
                            else
                                planLessonQuizUserResult.UserId = null;
                            if (!reader.IsDBNull(4))
                                planLessonQuizUserResult.QuizId = reader.GetValue(4).ToString();
                            else
                                planLessonQuizUserResult.QuizId = null;
                            if (!reader.IsDBNull(5))
                                planLessonQuizUserResult.Score = reader.GetValue(5).ToString();
                            else
                                planLessonQuizUserResult.Score = null;
                            if (!reader.IsDBNull(6))
                                planLessonQuizUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                            else
                                planLessonQuizUserResult.UsquiUserResultId = null;
                            if (!reader.IsDBNull(7))
                                planLessonQuizUserResult.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                planLessonQuizUserResult.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                planLessonQuizUserResult.CreatedAt = reader.GetValue(8).ToString();
                            else
                                planLessonQuizUserResult.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                planLessonQuizUserResult.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                planLessonQuizUserResult.UpdatedAt = null;
                            response.Add(planLessonQuizUserResult.Id, planLessonQuizUserResult);
                            planLessonQuizUserResult = new PlanLessonQuizUserResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PlanLessonQuizUserResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlanLessonQuizUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanLessonQuizUserResultModel> Read(string connectionString, PlanLessonQuizUserResultModel planLessonQuizUserResult, bool firstOne, bool orderByLastDate, PlanLessonQuizUserResultModel planLessonQuizUserResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonQuizUserResultModel> response = new List<PlanLessonQuizUserResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_lesson_id`, `manually_corrected_by_teacher`, `user_id`, `quiz_id`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_quiz_user_results ";
                {
                    if (planLessonQuizUserResult != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", planLessonQuizUserResult.Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonId", planLessonQuizUserResult.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher", planLessonQuizUserResult.ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", planLessonQuizUserResult.UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", planLessonQuizUserResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.Score))
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
                            cmd.Parameters.AddWithValue("@score", planLessonQuizUserResult.Score);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.UsquiUserResultId))
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
                            cmd.Parameters.AddWithValue("@usquiUserResultId", planLessonQuizUserResult.UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planLessonQuizUserResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planLessonQuizUserResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planLessonQuizUserResult.UpdatedAt);
                        }
                    }
                    if (planLessonQuizUserResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planLessonQuizUserResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + planLessonQuizUserResultWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacherWhereLike", "%" + planLessonQuizUserResultWhereLike.ManuallyCorrectedByTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planLessonQuizUserResultWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + planLessonQuizUserResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.Score))
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
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + planLessonQuizUserResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.UsquiUserResultId))
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
                            cmd.Parameters.AddWithValue("@usquiUserResultIdWhereLike", "%" + planLessonQuizUserResultWhereLike.UsquiUserResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planLessonQuizUserResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planLessonQuizUserResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planLessonQuizUserResultWhereLike.UpdatedAt + "%");
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
                                planLessonQuizUserResult = new PlanLessonQuizUserResultModel();
                                if (!reader.IsDBNull(0))
                                    planLessonQuizUserResult.Id = reader.GetValue(0).ToString();
                                else
                                    planLessonQuizUserResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    planLessonQuizUserResult.PlanLessonId = reader.GetValue(1).ToString();
                                else
                                    planLessonQuizUserResult.PlanLessonId = null;
                                if (!reader.IsDBNull(2))
                                    planLessonQuizUserResult.ManuallyCorrectedByTeacher = reader.GetValue(2).ToString();
                                else
                                    planLessonQuizUserResult.ManuallyCorrectedByTeacher = null;
                                if (!reader.IsDBNull(3))
                                    planLessonQuizUserResult.UserId = reader.GetValue(3).ToString();
                                else
                                    planLessonQuizUserResult.UserId = null;
                                if (!reader.IsDBNull(4))
                                    planLessonQuizUserResult.QuizId = reader.GetValue(4).ToString();
                                else
                                    planLessonQuizUserResult.QuizId = null;
                                if (!reader.IsDBNull(5))
                                    planLessonQuizUserResult.Score = reader.GetValue(5).ToString();
                                else
                                    planLessonQuizUserResult.Score = null;
                                if (!reader.IsDBNull(6))
                                    planLessonQuizUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                                else
                                    planLessonQuizUserResult.UsquiUserResultId = null;
                                if (!reader.IsDBNull(7))
                                    planLessonQuizUserResult.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    planLessonQuizUserResult.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    planLessonQuizUserResult.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    planLessonQuizUserResult.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    planLessonQuizUserResult.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    planLessonQuizUserResult.UpdatedAt = null;
                                response.Add(planLessonQuizUserResult);
                                planLessonQuizUserResult = new PlanLessonQuizUserResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlanLessonQuizUserResultModel>();
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
                response = new List<PlanLessonQuizUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanLessonQuizUserResultModel> Read(string connectionString, PlanLessonQuizUserResultModel planLessonQuizUserResult, bool orderByLastDate, int fromRow, int limit, PlanLessonQuizUserResultModel planLessonQuizUserResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonQuizUserResultModel> response = new List<PlanLessonQuizUserResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_lesson_id`, `manually_corrected_by_teacher`, `user_id`, `quiz_id`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_quiz_user_results ";
                {
                    if (planLessonQuizUserResult != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", planLessonQuizUserResult.Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonId", planLessonQuizUserResult.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher", planLessonQuizUserResult.ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", planLessonQuizUserResult.UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", planLessonQuizUserResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.Score))
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
                            cmd.Parameters.AddWithValue("@score", planLessonQuizUserResult.Score);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.UsquiUserResultId))
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
                            cmd.Parameters.AddWithValue("@usquiUserResultId", planLessonQuizUserResult.UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planLessonQuizUserResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planLessonQuizUserResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planLessonQuizUserResult.UpdatedAt);
                        }
                    }
                    if (planLessonQuizUserResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planLessonQuizUserResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + planLessonQuizUserResultWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacherWhereLike", "%" + planLessonQuizUserResultWhereLike.ManuallyCorrectedByTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planLessonQuizUserResultWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + planLessonQuizUserResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.Score))
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
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + planLessonQuizUserResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.UsquiUserResultId))
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
                            cmd.Parameters.AddWithValue("@usquiUserResultIdWhereLike", "%" + planLessonQuizUserResultWhereLike.UsquiUserResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planLessonQuizUserResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planLessonQuizUserResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planLessonQuizUserResultWhereLike.UpdatedAt + "%");
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
                                planLessonQuizUserResult = new PlanLessonQuizUserResultModel();
                                if (!reader.IsDBNull(0))
                                    planLessonQuizUserResult.Id = reader.GetValue(0).ToString();
                                else
                                    planLessonQuizUserResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    planLessonQuizUserResult.PlanLessonId = reader.GetValue(1).ToString();
                                else
                                    planLessonQuizUserResult.PlanLessonId = null;
                                if (!reader.IsDBNull(2))
                                    planLessonQuizUserResult.ManuallyCorrectedByTeacher = reader.GetValue(2).ToString();
                                else
                                    planLessonQuizUserResult.ManuallyCorrectedByTeacher = null;
                                if (!reader.IsDBNull(3))
                                    planLessonQuizUserResult.UserId = reader.GetValue(3).ToString();
                                else
                                    planLessonQuizUserResult.UserId = null;
                                if (!reader.IsDBNull(4))
                                    planLessonQuizUserResult.QuizId = reader.GetValue(4).ToString();
                                else
                                    planLessonQuizUserResult.QuizId = null;
                                if (!reader.IsDBNull(5))
                                    planLessonQuizUserResult.Score = reader.GetValue(5).ToString();
                                else
                                    planLessonQuizUserResult.Score = null;
                                if (!reader.IsDBNull(6))
                                    planLessonQuizUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                                else
                                    planLessonQuizUserResult.UsquiUserResultId = null;
                                if (!reader.IsDBNull(7))
                                    planLessonQuizUserResult.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    planLessonQuizUserResult.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    planLessonQuizUserResult.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    planLessonQuizUserResult.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    planLessonQuizUserResult.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    planLessonQuizUserResult.UpdatedAt = null;
                                response.Add(planLessonQuizUserResult);
                                planLessonQuizUserResult = new PlanLessonQuizUserResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlanLessonQuizUserResultModel>();
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
                response = new List<PlanLessonQuizUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanLessonQuizUserResultModel> Read(string connectionString, List<PlanLessonQuizUserResultModel> planLessonQuizUserResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonQuizUserResultModel> response = new List<PlanLessonQuizUserResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_lesson_id`, `manually_corrected_by_teacher`, `user_id`, `quiz_id`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_quiz_user_results ";
                if (planLessonQuizUserResultWhere != null && planLessonQuizUserResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planLessonQuizUserResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planLessonQuizUserResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, planLessonQuizUserResultWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].ManuallyCorrectedByTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher" + i, planLessonQuizUserResultWhere[i].ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planLessonQuizUserResultWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, planLessonQuizUserResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, planLessonQuizUserResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].UsquiUserResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `usqui_user_result_id` = @usquiUserResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planLessonQuizUserResultWhere[i].UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planLessonQuizUserResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planLessonQuizUserResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planLessonQuizUserResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planLessonQuizUserResultWhere.Count - 1)
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
                            PlanLessonQuizUserResultModel planLessonQuizUserResult = new PlanLessonQuizUserResultModel(); if (!reader.IsDBNull(0))
                                planLessonQuizUserResult.Id = reader.GetValue(0).ToString();
                            else
                                planLessonQuizUserResult.Id = null;
                            if (!reader.IsDBNull(1))
                                planLessonQuizUserResult.PlanLessonId = reader.GetValue(1).ToString();
                            else
                                planLessonQuizUserResult.PlanLessonId = null;
                            if (!reader.IsDBNull(2))
                                planLessonQuizUserResult.ManuallyCorrectedByTeacher = reader.GetValue(2).ToString();
                            else
                                planLessonQuizUserResult.ManuallyCorrectedByTeacher = null;
                            if (!reader.IsDBNull(3))
                                planLessonQuizUserResult.UserId = reader.GetValue(3).ToString();
                            else
                                planLessonQuizUserResult.UserId = null;
                            if (!reader.IsDBNull(4))
                                planLessonQuizUserResult.QuizId = reader.GetValue(4).ToString();
                            else
                                planLessonQuizUserResult.QuizId = null;
                            if (!reader.IsDBNull(5))
                                planLessonQuizUserResult.Score = reader.GetValue(5).ToString();
                            else
                                planLessonQuizUserResult.Score = null;
                            if (!reader.IsDBNull(6))
                                planLessonQuizUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                            else
                                planLessonQuizUserResult.UsquiUserResultId = null;
                            if (!reader.IsDBNull(7))
                                planLessonQuizUserResult.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                planLessonQuizUserResult.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                planLessonQuizUserResult.CreatedAt = reader.GetValue(8).ToString();
                            else
                                planLessonQuizUserResult.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                planLessonQuizUserResult.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                planLessonQuizUserResult.UpdatedAt = null;
                            response.Add(planLessonQuizUserResult);
                            planLessonQuizUserResult = new PlanLessonQuizUserResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlanLessonQuizUserResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonQuizUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanLessonQuizUserResultModel> Read(string connectionString, List<PlanLessonQuizUserResultModel> planLessonQuizUserResultWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonQuizUserResultModel> response = new List<PlanLessonQuizUserResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_lesson_id`, `manually_corrected_by_teacher`, `user_id`, `quiz_id`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_quiz_user_results ";
                if (planLessonQuizUserResultWhere != null && planLessonQuizUserResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planLessonQuizUserResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planLessonQuizUserResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, planLessonQuizUserResultWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].ManuallyCorrectedByTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher" + i, planLessonQuizUserResultWhere[i].ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planLessonQuizUserResultWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, planLessonQuizUserResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, planLessonQuizUserResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].UsquiUserResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `usqui_user_result_id` = @usquiUserResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planLessonQuizUserResultWhere[i].UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planLessonQuizUserResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planLessonQuizUserResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planLessonQuizUserResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planLessonQuizUserResultWhere.Count - 1)
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
                            PlanLessonQuizUserResultModel planLessonQuizUserResult = new PlanLessonQuizUserResultModel(); if (!reader.IsDBNull(0))
                                planLessonQuizUserResult.Id = reader.GetValue(0).ToString();
                            else
                                planLessonQuizUserResult.Id = null;
                            if (!reader.IsDBNull(1))
                                planLessonQuizUserResult.PlanLessonId = reader.GetValue(1).ToString();
                            else
                                planLessonQuizUserResult.PlanLessonId = null;
                            if (!reader.IsDBNull(2))
                                planLessonQuizUserResult.ManuallyCorrectedByTeacher = reader.GetValue(2).ToString();
                            else
                                planLessonQuizUserResult.ManuallyCorrectedByTeacher = null;
                            if (!reader.IsDBNull(3))
                                planLessonQuizUserResult.UserId = reader.GetValue(3).ToString();
                            else
                                planLessonQuizUserResult.UserId = null;
                            if (!reader.IsDBNull(4))
                                planLessonQuizUserResult.QuizId = reader.GetValue(4).ToString();
                            else
                                planLessonQuizUserResult.QuizId = null;
                            if (!reader.IsDBNull(5))
                                planLessonQuizUserResult.Score = reader.GetValue(5).ToString();
                            else
                                planLessonQuizUserResult.Score = null;
                            if (!reader.IsDBNull(6))
                                planLessonQuizUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                            else
                                planLessonQuizUserResult.UsquiUserResultId = null;
                            if (!reader.IsDBNull(7))
                                planLessonQuizUserResult.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                planLessonQuizUserResult.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                planLessonQuizUserResult.CreatedAt = reader.GetValue(8).ToString();
                            else
                                planLessonQuizUserResult.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                planLessonQuizUserResult.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                planLessonQuizUserResult.UpdatedAt = null;
                            response.Add(planLessonQuizUserResult);
                            planLessonQuizUserResult = new PlanLessonQuizUserResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlanLessonQuizUserResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonQuizUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanLessonQuizUserResultModel> Create(string connectionString, PlanLessonQuizUserResultModel planLessonQuizUserResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonQuizUserResultModel> response = new List<PlanLessonQuizUserResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO plan_lesson_quiz_user_results (";
                string values = "(";
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.Id))
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
                    cmd.Parameters.AddWithValue("@id", planLessonQuizUserResult.Id);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.PlanLessonId))
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
                    cmd.Parameters.AddWithValue("@planLessonId", planLessonQuizUserResult.PlanLessonId);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.ManuallyCorrectedByTeacher))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `manually_corrected_by_teacher` ");
                        values = String.Concat(values, ", @manuallyCorrectedByTeacher");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `manually_corrected_by_teacher` ");
                        values = String.Concat(values, " @manuallyCorrectedByTeacher");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher", planLessonQuizUserResult.ManuallyCorrectedByTeacher);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", planLessonQuizUserResult.UserId);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.QuizId))
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
                    cmd.Parameters.AddWithValue("@quizId", planLessonQuizUserResult.QuizId);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.Score))
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
                    cmd.Parameters.AddWithValue("@score", planLessonQuizUserResult.Score);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.UsquiUserResultId))
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
                    cmd.Parameters.AddWithValue("@usquiUserResultId", planLessonQuizUserResult.UsquiUserResultId);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", planLessonQuizUserResult.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", planLessonQuizUserResult.CreatedAt);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", planLessonQuizUserResult.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                planLessonQuizUserResult.Id = cmd.LastInsertedId.ToString();
                response.Add(planLessonQuizUserResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonQuizUserResultModel>();
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
        public List<PlanLessonQuizUserResultModel> Create(string connectionString, List<PlanLessonQuizUserResultModel> planLessonQuizUserResultList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonQuizUserResultModel> response = new List<PlanLessonQuizUserResultModel>();
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
                        for (int i = 0; i < planLessonQuizUserResultList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO plan_lesson_quiz_user_results (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, planLessonQuizUserResultList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].PlanLessonId))
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
                                    cmd.Parameters.AddWithValue("@planLessonId" + i, planLessonQuizUserResultList[i].PlanLessonId);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].ManuallyCorrectedByTeacher))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `manually_corrected_by_teacher` ");
                                        values = String.Concat(values, ", @manuallyCorrectedByTeacher" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `manually_corrected_by_teacher` ");
                                        values = String.Concat(values, "@manuallyCorrectedByTeacher" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher" + i, planLessonQuizUserResultList[i].ManuallyCorrectedByTeacher);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, planLessonQuizUserResultList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].QuizId))
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
                                    cmd.Parameters.AddWithValue("@quizId" + i, planLessonQuizUserResultList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].Score))
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
                                    cmd.Parameters.AddWithValue("@score" + i, planLessonQuizUserResultList[i].Score);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].UsquiUserResultId))
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
                                    cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planLessonQuizUserResultList[i].UsquiUserResultId);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planLessonQuizUserResultList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planLessonQuizUserResultList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planLessonQuizUserResultList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(planLessonQuizUserResultList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanLessonQuizUserResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanLessonQuizUserResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonQuizUserResultModel>();
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

        public List<PlanLessonQuizUserResultModel> Update(string connectionString, PlanLessonQuizUserResultModel planLessonQuizUserResult, PlanLessonQuizUserResultModel planLessonQuizUserResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonQuizUserResultModel> response = new List<PlanLessonQuizUserResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE plan_lesson_quiz_user_results SET ";
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", planLessonQuizUserResult.Id);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.PlanLessonId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `plan_lesson_id` = @planLessonId");
                    else
                        sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planLessonId", planLessonQuizUserResult.PlanLessonId);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.ManuallyCorrectedByTeacher))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher");
                    else
                        sql = String.Concat(sql, " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher", planLessonQuizUserResult.ManuallyCorrectedByTeacher);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", planLessonQuizUserResult.UserId);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.QuizId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `quiz_id` = @quizId");
                    else
                        sql = String.Concat(sql, " `quiz_id` = @quizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", planLessonQuizUserResult.QuizId);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.Score))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `score` = @score");
                    else
                        sql = String.Concat(sql, " `score` = @score");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@score", planLessonQuizUserResult.Score);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.UsquiUserResultId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `usqui_user_result_id` = @usquiUserResultId");
                    else
                        sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@usquiUserResultId", planLessonQuizUserResult.UsquiUserResultId);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", planLessonQuizUserResult.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", planLessonQuizUserResult.CreatedAt);
                }
                if (!String.IsNullOrEmpty(planLessonQuizUserResult.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", planLessonQuizUserResult.UpdatedAt);
                }
                if (planLessonQuizUserResultWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", planLessonQuizUserResultWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere.PlanLessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonIdWhere");
                        else
                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planLessonIdWhere", planLessonQuizUserResultWhere.PlanLessonId);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere.ManuallyCorrectedByTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `manually_corrected_by_teacher` = @manuallyCorrectedByTeacherWhere");
                        else
                            sql = String.Concat(sql, " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacherWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacherWhere", planLessonQuizUserResultWhere.ManuallyCorrectedByTeacher);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", planLessonQuizUserResultWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizIdWhere", planLessonQuizUserResultWhere.QuizId);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere.Score))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `score` = @scoreWhere");
                        else
                            sql = String.Concat(sql, " `score` = @scoreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@scoreWhere", planLessonQuizUserResultWhere.Score);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere.UsquiUserResultId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `usqui_user_result_id` = @usquiUserResultIdWhere");
                        else
                            sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@usquiUserResultIdWhere", planLessonQuizUserResultWhere.UsquiUserResultId);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", planLessonQuizUserResultWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", planLessonQuizUserResultWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", planLessonQuizUserResultWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(planLessonQuizUserResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonQuizUserResultModel>();
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

        public List<PlanLessonQuizUserResultModel> Update(string connectionString, List<PlanLessonQuizUserResultModel> planLessonQuizUserResultList, List<PlanLessonQuizUserResultModel> planLessonQuizUserResultWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonQuizUserResultModel> response = new List<PlanLessonQuizUserResultModel>();
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
                        for (int i = 0; i < planLessonQuizUserResultList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE plan_lesson_quiz_user_results SET ";
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, planLessonQuizUserResultList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].PlanLessonId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `plan_lesson_id` = @planLessonId" + i);
                                    else
                                        sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planLessonId" + i, planLessonQuizUserResultList[i].PlanLessonId);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].ManuallyCorrectedByTeacher))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i);
                                    else
                                        sql = String.Concat(sql, " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher" + i, planLessonQuizUserResultList[i].ManuallyCorrectedByTeacher);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, planLessonQuizUserResultList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].QuizId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `quiz_id` = @quizId" + i);
                                    else
                                        sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, planLessonQuizUserResultList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].Score))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `score` = @score" + i);
                                    else
                                        sql = String.Concat(sql, " `score` = @score" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@score" + i, planLessonQuizUserResultList[i].Score);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].UsquiUserResultId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `usqui_user_result_id` = @usquiUserResultId" + i);
                                    else
                                        sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planLessonQuizUserResultList[i].UsquiUserResultId);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planLessonQuizUserResultList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planLessonQuizUserResultList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(planLessonQuizUserResultList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planLessonQuizUserResultList[i].UpdatedAt);
                                }
                                if (planLessonQuizUserResultWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, planLessonQuizUserResultWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereList[i].PlanLessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planLessonIdWhere" + i, planLessonQuizUserResultWhereList[i].PlanLessonId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereList[i].ManuallyCorrectedByTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `manually_corrected_by_teacher` = @manuallyCorrectedByTeacherWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacherWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacherWhere" + i, planLessonQuizUserResultWhereList[i].ManuallyCorrectedByTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, planLessonQuizUserResultWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereList[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizIdWhere" + i, planLessonQuizUserResultWhereList[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereList[i].Score))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `score` = @scoreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `score` = @scoreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@scoreWhere" + i, planLessonQuizUserResultWhereList[i].Score);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereList[i].UsquiUserResultId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `usqui_user_result_id` = @usquiUserResultIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@usquiUserResultIdWhere" + i, planLessonQuizUserResultWhereList[i].UsquiUserResultId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, planLessonQuizUserResultWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, planLessonQuizUserResultWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResultWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, planLessonQuizUserResultWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(planLessonQuizUserResultList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanLessonQuizUserResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanLessonQuizUserResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonQuizUserResultModel>();
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

        public List<PlanLessonQuizUserResultModel> Delete(string connectionString, PlanLessonQuizUserResultModel planLessonQuizUserResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonQuizUserResultModel> response = new List<PlanLessonQuizUserResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM plan_lesson_quiz_user_results  ";
                if (planLessonQuizUserResult != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(planLessonQuizUserResult.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", planLessonQuizUserResult.Id);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResult.PlanLessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonId");
                        else
                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planLessonId", planLessonQuizUserResult.PlanLessonId);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResult.ManuallyCorrectedByTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher");
                        else
                            sql = String.Concat(sql, " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher", planLessonQuizUserResult.ManuallyCorrectedByTeacher);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResult.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", planLessonQuizUserResult.UserId);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResult.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizId");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizId", planLessonQuizUserResult.QuizId);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResult.Score))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `score` = @score");
                        else
                            sql = String.Concat(sql, " `score` = @score");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@score", planLessonQuizUserResult.Score);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResult.UsquiUserResultId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `usqui_user_result_id` = @usquiUserResultId");
                        else
                            sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@usquiUserResultId", planLessonQuizUserResult.UsquiUserResultId);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResult.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", planLessonQuizUserResult.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResult.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", planLessonQuizUserResult.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(planLessonQuizUserResult.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", planLessonQuizUserResult.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(planLessonQuizUserResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonQuizUserResultModel>();
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

        public List<PlanLessonQuizUserResultModel> Delete(string connectionString, List<PlanLessonQuizUserResultModel> planLessonQuizUserResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonQuizUserResultModel> response = new List<PlanLessonQuizUserResultModel>();
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
                        for (int i = 0; i < planLessonQuizUserResult.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM plan_lesson_quiz_user_results  ";
                                if (planLessonQuizUserResult[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResult[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, planLessonQuizUserResult[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResult[i].PlanLessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonId" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planLessonId" + i, planLessonQuizUserResult[i].PlanLessonId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResult[i].ManuallyCorrectedByTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i);
                                        else
                                            sql = String.Concat(sql, " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher" + i, planLessonQuizUserResult[i].ManuallyCorrectedByTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResult[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, planLessonQuizUserResult[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResult[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizId" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizId" + i, planLessonQuizUserResult[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResult[i].Score))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `score` = @score" + i);
                                        else
                                            sql = String.Concat(sql, " `score` = @score" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@score" + i, planLessonQuizUserResult[i].Score);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResult[i].UsquiUserResultId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `usqui_user_result_id` = @usquiUserResultId" + i);
                                        else
                                            sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planLessonQuizUserResult[i].UsquiUserResultId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResult[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, planLessonQuizUserResult[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResult[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, planLessonQuizUserResult[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonQuizUserResult[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, planLessonQuizUserResult[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(planLessonQuizUserResult[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanLessonQuizUserResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanLessonQuizUserResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonQuizUserResultModel>();
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
