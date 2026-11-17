using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Services.UserQuizzes.CustomClasses;
using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.UserQuizzes
{
    class UserQuizResultDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, UserQuizResultModel> ReadD(string connectionString, UserQuizResultModel userQuizResult, bool firstOne, bool orderByLastDate, UserQuizResultModel userQuizResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserQuizResultModel> response = new Dictionary<string, UserQuizResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `try_no`, `user_id`, `quiz_id`, `dok`, `is_exam`, `lesson_id`, `subject`, `level`, `total_correct`, `total_questions`, `total_percent`, `status`, `feedback`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_results ";
                {
                    if (userQuizResult != null)
                    {
                        if (!string.IsNullOrEmpty(userQuizResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", userQuizResult.Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TryNo))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `try_no` = @tryNo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `try_no` = @tryNo ");
                            }
                            cmd.Parameters.AddWithValue("@tryNo", userQuizResult.TryNo);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userQuizResult.UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", userQuizResult.QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Dok))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `dok` = @dok ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `dok` = @dok ");
                            }
                            cmd.Parameters.AddWithValue("@dok", userQuizResult.Dok);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.IsExam))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_exam` = @isExam ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_exam` = @isExam ");
                            }
                            cmd.Parameters.AddWithValue("@isExam", userQuizResult.IsExam);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", userQuizResult.LessonId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Subject))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject` = @subject ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject` = @subject ");
                            }
                            cmd.Parameters.AddWithValue("@subject", userQuizResult.Subject);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Level))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `level` = @level ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `level` = @level ");
                            }
                            cmd.Parameters.AddWithValue("@level", userQuizResult.Level);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TotalCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total_correct` = @totalCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total_correct` = @totalCorrect ");
                            }
                            cmd.Parameters.AddWithValue("@totalCorrect", userQuizResult.TotalCorrect);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TotalQuestions))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total_questions` = @totalQuestions ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total_questions` = @totalQuestions ");
                            }
                            cmd.Parameters.AddWithValue("@totalQuestions", userQuizResult.TotalQuestions);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TotalPercent))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total_percent` = @totalPercent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total_percent` = @totalPercent ");
                            }
                            cmd.Parameters.AddWithValue("@totalPercent", userQuizResult.TotalPercent);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Status))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `status` = @status ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `status` = @status ");
                            }
                            cmd.Parameters.AddWithValue("@status", userQuizResult.Status);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Feedback))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `feedback` = @feedback ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `feedback` = @feedback ");
                            }
                            cmd.Parameters.AddWithValue("@feedback", userQuizResult.Feedback);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userQuizResult.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userQuizResult.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userQuizResult.UpdatedAt);
                        }
                    }
                    if (userQuizResultWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userQuizResultWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TryNo))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `try_no` LIKE @tryNoWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `try_no` LIKE @tryNoWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@tryNoWhereLike", "%" + userQuizResultWhereLike.TryNo + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userQuizResultWhereLike.UserId + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + userQuizResultWhereLike.QuizId + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Dok))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `dok` LIKE @dokWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `dok` LIKE @dokWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dokWhereLike", "%" + userQuizResultWhereLike.Dok + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.IsExam))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `is_exam` LIKE @isExamWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `is_exam` LIKE @isExamWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isExamWhereLike", "%" + userQuizResultWhereLike.IsExam + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + userQuizResultWhereLike.LessonId + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Subject))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `subject` LIKE @subjectWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `subject` LIKE @subjectWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subjectWhereLike", "%" + userQuizResultWhereLike.Subject + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Level))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `level` LIKE @levelWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `level` LIKE @levelWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelWhereLike", "%" + userQuizResultWhereLike.Level + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TotalCorrect))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalCorrectWhereLike", "%" + userQuizResultWhereLike.TotalCorrect + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TotalQuestions))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalQuestionsWhereLike", "%" + userQuizResultWhereLike.TotalQuestions + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TotalPercent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalPercentWhereLike", "%" + userQuizResultWhereLike.TotalPercent + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Status))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `status` LIKE @statusWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `status` LIKE @statusWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@statusWhereLike", "%" + userQuizResultWhereLike.Status + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Feedback))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `feedback` LIKE @feedbackWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `feedback` LIKE @feedbackWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@feedbackWhereLike", "%" + userQuizResultWhereLike.Feedback + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userQuizResultWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userQuizResultWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userQuizResultWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = string.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                userQuizResult = new UserQuizResultModel();
                                if (!reader.IsDBNull(0))
                                    userQuizResult.Id = reader.GetValue(0).ToString();
                                else
                                    userQuizResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    userQuizResult.TryNo = reader.GetValue(1).ToString();
                                else
                                    userQuizResult.TryNo = null;
                                if (!reader.IsDBNull(2))
                                    userQuizResult.UserId = reader.GetValue(2).ToString();
                                else
                                    userQuizResult.UserId = null;
                                if (!reader.IsDBNull(3))
                                    userQuizResult.QuizId = reader.GetValue(3).ToString();
                                else
                                    userQuizResult.QuizId = null;
                                if (!reader.IsDBNull(4))
                                    userQuizResult.Dok = reader.GetValue(4).ToString();
                                else
                                    userQuizResult.Dok = null;
                                if (!reader.IsDBNull(5))
                                    userQuizResult.IsExam = reader.GetValue(5).ToString();
                                else
                                    userQuizResult.IsExam = null;
                                if (!reader.IsDBNull(6))
                                    userQuizResult.LessonId = reader.GetValue(6).ToString();
                                else
                                    userQuizResult.LessonId = null;
                                if (!reader.IsDBNull(7))
                                    userQuizResult.Subject = reader.GetValue(7).ToString();
                                else
                                    userQuizResult.Subject = null;
                                if (!reader.IsDBNull(8))
                                    userQuizResult.Level = reader.GetValue(8).ToString();
                                else
                                    userQuizResult.Level = null;
                                if (!reader.IsDBNull(9))
                                    userQuizResult.TotalCorrect = reader.GetValue(9).ToString();
                                else
                                    userQuizResult.TotalCorrect = null;
                                if (!reader.IsDBNull(10))
                                    userQuizResult.TotalQuestions = reader.GetValue(10).ToString();
                                else
                                    userQuizResult.TotalQuestions = null;
                                if (!reader.IsDBNull(11))
                                    userQuizResult.TotalPercent = reader.GetValue(11).ToString();
                                else
                                    userQuizResult.TotalPercent = null;
                                if (!reader.IsDBNull(12))
                                    userQuizResult.Status = reader.GetValue(12).ToString();
                                else
                                    userQuizResult.Status = null;
                                if (!reader.IsDBNull(13))
                                    userQuizResult.Feedback = reader.GetValue(13).ToString();
                                else
                                    userQuizResult.Feedback = null;
                                if (!reader.IsDBNull(14))
                                    userQuizResult.ModifiedBy = reader.GetValue(14).ToString();
                                else
                                    userQuizResult.ModifiedBy = null;
                                if (!reader.IsDBNull(15))
                                    userQuizResult.CreatedAt = reader.GetValue(15).ToString();
                                else
                                    userQuizResult.CreatedAt = null;
                                if (!reader.IsDBNull(16))
                                    userQuizResult.UpdatedAt = reader.GetValue(16).ToString();
                                else
                                    userQuizResult.UpdatedAt = null;
                                response.Add(userQuizResult.Id, userQuizResult);
                                userQuizResult = new UserQuizResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserQuizResultModel>();
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
                response = new Dictionary<string, UserQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserQuizResultModel> ReadD(string connectionString, UserQuizResultModel userQuizResult, bool orderByLastDate, int fromRow, int limit, UserQuizResultModel userQuizResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserQuizResultModel> response = new Dictionary<string, UserQuizResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `try_no`, `user_id`, `quiz_id`, `dok`, `is_exam`, `lesson_id`, `subject`, `level`, `total_correct`, `total_questions`, `total_percent`, `status`, `feedback`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_results ";
                {
                    if (userQuizResult != null)
                    {
                        if (!string.IsNullOrEmpty(userQuizResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", userQuizResult.Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TryNo))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `try_no` = @tryNo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `try_no` = @tryNo ");
                            }
                            cmd.Parameters.AddWithValue("@tryNo", userQuizResult.TryNo);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userQuizResult.UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", userQuizResult.QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Dok))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `dok` = @dok ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `dok` = @dok ");
                            }
                            cmd.Parameters.AddWithValue("@dok", userQuizResult.Dok);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.IsExam))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_exam` = @isExam ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_exam` = @isExam ");
                            }
                            cmd.Parameters.AddWithValue("@isExam", userQuizResult.IsExam);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", userQuizResult.LessonId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Subject))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject` = @subject ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject` = @subject ");
                            }
                            cmd.Parameters.AddWithValue("@subject", userQuizResult.Subject);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Level))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `level` = @level ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `level` = @level ");
                            }
                            cmd.Parameters.AddWithValue("@level", userQuizResult.Level);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TotalCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total_correct` = @totalCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total_correct` = @totalCorrect ");
                            }
                            cmd.Parameters.AddWithValue("@totalCorrect", userQuizResult.TotalCorrect);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TotalQuestions))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total_questions` = @totalQuestions ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total_questions` = @totalQuestions ");
                            }
                            cmd.Parameters.AddWithValue("@totalQuestions", userQuizResult.TotalQuestions);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TotalPercent))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total_percent` = @totalPercent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total_percent` = @totalPercent ");
                            }
                            cmd.Parameters.AddWithValue("@totalPercent", userQuizResult.TotalPercent);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Status))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `status` = @status ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `status` = @status ");
                            }
                            cmd.Parameters.AddWithValue("@status", userQuizResult.Status);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Feedback))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `feedback` = @feedback ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `feedback` = @feedback ");
                            }
                            cmd.Parameters.AddWithValue("@feedback", userQuizResult.Feedback);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userQuizResult.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userQuizResult.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userQuizResult.UpdatedAt);
                        }
                    }
                    if (userQuizResultWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userQuizResultWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TryNo))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `try_no` LIKE @tryNoWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `try_no` LIKE @tryNoWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@tryNoWhereLike", "%" + userQuizResultWhereLike.TryNo + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userQuizResultWhereLike.UserId + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + userQuizResultWhereLike.QuizId + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Dok))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `dok` LIKE @dokWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `dok` LIKE @dokWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dokWhereLike", "%" + userQuizResultWhereLike.Dok + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.IsExam))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `is_exam` LIKE @isExamWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `is_exam` LIKE @isExamWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isExamWhereLike", "%" + userQuizResultWhereLike.IsExam + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + userQuizResultWhereLike.LessonId + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Subject))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `subject` LIKE @subjectWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `subject` LIKE @subjectWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subjectWhereLike", "%" + userQuizResultWhereLike.Subject + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Level))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `level` LIKE @levelWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `level` LIKE @levelWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelWhereLike", "%" + userQuizResultWhereLike.Level + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TotalCorrect))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalCorrectWhereLike", "%" + userQuizResultWhereLike.TotalCorrect + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TotalQuestions))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalQuestionsWhereLike", "%" + userQuizResultWhereLike.TotalQuestions + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TotalPercent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalPercentWhereLike", "%" + userQuizResultWhereLike.TotalPercent + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Status))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `status` LIKE @statusWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `status` LIKE @statusWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@statusWhereLike", "%" + userQuizResultWhereLike.Status + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Feedback))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `feedback` LIKE @feedbackWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `feedback` LIKE @feedbackWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@feedbackWhereLike", "%" + userQuizResultWhereLike.Feedback + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userQuizResultWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userQuizResultWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userQuizResultWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                userQuizResult = new UserQuizResultModel();
                                if (!reader.IsDBNull(0))
                                    userQuizResult.Id = reader.GetValue(0).ToString();
                                else
                                    userQuizResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    userQuizResult.TryNo = reader.GetValue(1).ToString();
                                else
                                    userQuizResult.TryNo = null;
                                if (!reader.IsDBNull(2))
                                    userQuizResult.UserId = reader.GetValue(2).ToString();
                                else
                                    userQuizResult.UserId = null;
                                if (!reader.IsDBNull(3))
                                    userQuizResult.QuizId = reader.GetValue(3).ToString();
                                else
                                    userQuizResult.QuizId = null;
                                if (!reader.IsDBNull(4))
                                    userQuizResult.Dok = reader.GetValue(4).ToString();
                                else
                                    userQuizResult.Dok = null;
                                if (!reader.IsDBNull(5))
                                    userQuizResult.IsExam = reader.GetValue(5).ToString();
                                else
                                    userQuizResult.IsExam = null;
                                if (!reader.IsDBNull(6))
                                    userQuizResult.LessonId = reader.GetValue(6).ToString();
                                else
                                    userQuizResult.LessonId = null;
                                if (!reader.IsDBNull(7))
                                    userQuizResult.Subject = reader.GetValue(7).ToString();
                                else
                                    userQuizResult.Subject = null;
                                if (!reader.IsDBNull(8))
                                    userQuizResult.Level = reader.GetValue(8).ToString();
                                else
                                    userQuizResult.Level = null;
                                if (!reader.IsDBNull(9))
                                    userQuizResult.TotalCorrect = reader.GetValue(9).ToString();
                                else
                                    userQuizResult.TotalCorrect = null;
                                if (!reader.IsDBNull(10))
                                    userQuizResult.TotalQuestions = reader.GetValue(10).ToString();
                                else
                                    userQuizResult.TotalQuestions = null;
                                if (!reader.IsDBNull(11))
                                    userQuizResult.TotalPercent = reader.GetValue(11).ToString();
                                else
                                    userQuizResult.TotalPercent = null;
                                if (!reader.IsDBNull(12))
                                    userQuizResult.Status = reader.GetValue(12).ToString();
                                else
                                    userQuizResult.Status = null;
                                if (!reader.IsDBNull(13))
                                    userQuizResult.Feedback = reader.GetValue(13).ToString();
                                else
                                    userQuizResult.Feedback = null;
                                if (!reader.IsDBNull(14))
                                    userQuizResult.ModifiedBy = reader.GetValue(14).ToString();
                                else
                                    userQuizResult.ModifiedBy = null;
                                if (!reader.IsDBNull(15))
                                    userQuizResult.CreatedAt = reader.GetValue(15).ToString();
                                else
                                    userQuizResult.CreatedAt = null;
                                if (!reader.IsDBNull(16))
                                    userQuizResult.UpdatedAt = reader.GetValue(16).ToString();
                                else
                                    userQuizResult.UpdatedAt = null;
                                response.Add(userQuizResult.Id, userQuizResult);
                                userQuizResult = new UserQuizResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserQuizResultModel>();
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
                response = new Dictionary<string, UserQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserQuizResultModel> ReadD(string connectionString, List<UserQuizResultModel> userQuizResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserQuizResultModel> response = new Dictionary<string, UserQuizResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `try_no`, `user_id`, `quiz_id`, `dok`, `is_exam`, `lesson_id`, `subject`, `level`, `total_correct`, `total_questions`, `total_percent`, `status`, `feedback`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_results ";
                if (userQuizResultWhere != null && userQuizResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userQuizResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userQuizResultWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TryNo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `try_no` = @tryNo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@tryNo" + i, userQuizResultWhere[i].TryNo);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userQuizResultWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, userQuizResultWhere[i].QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Dok))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `dok` = @dok" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dok" + i, userQuizResultWhere[i].Dok);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].IsExam))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_exam` = @isExam" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isExam" + i, userQuizResultWhere[i].IsExam);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, userQuizResultWhere[i].LessonId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Subject))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject` = @subject" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subject" + i, userQuizResultWhere[i].Subject);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Level))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level` = @level" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@level" + i, userQuizResultWhere[i].Level);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TotalCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_correct` = @totalCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalCorrect" + i, userQuizResultWhere[i].TotalCorrect);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TotalQuestions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_questions` = @totalQuestions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalQuestions" + i, userQuizResultWhere[i].TotalQuestions);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TotalPercent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_percent` = @totalPercent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalPercent" + i, userQuizResultWhere[i].TotalPercent);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Status))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `status` = @status" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@status" + i, userQuizResultWhere[i].Status);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Feedback))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `feedback` = @feedback" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@feedback" + i, userQuizResultWhere[i].Feedback);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizResultWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userQuizResultWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userQuizResultWhere.Count - 1)
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
                            UserQuizResultModel userQuizResult = new UserQuizResultModel(); if (!reader.IsDBNull(0))
                                userQuizResult.Id = reader.GetValue(0).ToString();
                            else
                                userQuizResult.Id = null;
                            if (!reader.IsDBNull(1))
                                userQuizResult.TryNo = reader.GetValue(1).ToString();
                            else
                                userQuizResult.TryNo = null;
                            if (!reader.IsDBNull(2))
                                userQuizResult.UserId = reader.GetValue(2).ToString();
                            else
                                userQuizResult.UserId = null;
                            if (!reader.IsDBNull(3))
                                userQuizResult.QuizId = reader.GetValue(3).ToString();
                            else
                                userQuizResult.QuizId = null;
                            if (!reader.IsDBNull(4))
                                userQuizResult.Dok = reader.GetValue(4).ToString();
                            else
                                userQuizResult.Dok = null;
                            if (!reader.IsDBNull(5))
                                userQuizResult.IsExam = reader.GetValue(5).ToString();
                            else
                                userQuizResult.IsExam = null;
                            if (!reader.IsDBNull(6))
                                userQuizResult.LessonId = reader.GetValue(6).ToString();
                            else
                                userQuizResult.LessonId = null;
                            if (!reader.IsDBNull(7))
                                userQuizResult.Subject = reader.GetValue(7).ToString();
                            else
                                userQuizResult.Subject = null;
                            if (!reader.IsDBNull(8))
                                userQuizResult.Level = reader.GetValue(8).ToString();
                            else
                                userQuizResult.Level = null;
                            if (!reader.IsDBNull(9))
                                userQuizResult.TotalCorrect = reader.GetValue(9).ToString();
                            else
                                userQuizResult.TotalCorrect = null;
                            if (!reader.IsDBNull(10))
                                userQuizResult.TotalQuestions = reader.GetValue(10).ToString();
                            else
                                userQuizResult.TotalQuestions = null;
                            if (!reader.IsDBNull(11))
                                userQuizResult.TotalPercent = reader.GetValue(11).ToString();
                            else
                                userQuizResult.TotalPercent = null;
                            if (!reader.IsDBNull(12))
                                userQuizResult.Status = reader.GetValue(12).ToString();
                            else
                                userQuizResult.Status = null;
                            if (!reader.IsDBNull(13))
                                userQuizResult.Feedback = reader.GetValue(13).ToString();
                            else
                                userQuizResult.Feedback = null;
                            if (!reader.IsDBNull(14))
                                userQuizResult.ModifiedBy = reader.GetValue(14).ToString();
                            else
                                userQuizResult.ModifiedBy = null;
                            if (!reader.IsDBNull(15))
                                userQuizResult.CreatedAt = reader.GetValue(15).ToString();
                            else
                                userQuizResult.CreatedAt = null;
                            if (!reader.IsDBNull(16))
                                userQuizResult.UpdatedAt = reader.GetValue(16).ToString();
                            else
                                userQuizResult.UpdatedAt = null;
                            response.Add(userQuizResult.Id, userQuizResult);
                            userQuizResult = new UserQuizResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserQuizResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserQuizResultModel> ReadD(string connectionString, List<UserQuizResultModel> userQuizResultWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserQuizResultModel> response = new Dictionary<string, UserQuizResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `try_no`, `user_id`, `quiz_id`, `dok`, `is_exam`, `lesson_id`, `subject`, `level`, `total_correct`, `total_questions`, `total_percent`, `status`, `feedback`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_results ";
                if (userQuizResultWhere != null && userQuizResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userQuizResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userQuizResultWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TryNo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `try_no` = @tryNo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@tryNo" + i, userQuizResultWhere[i].TryNo);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userQuizResultWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, userQuizResultWhere[i].QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Dok))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `dok` = @dok" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dok" + i, userQuizResultWhere[i].Dok);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].IsExam))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_exam` = @isExam" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isExam" + i, userQuizResultWhere[i].IsExam);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, userQuizResultWhere[i].LessonId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Subject))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject` = @subject" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subject" + i, userQuizResultWhere[i].Subject);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Level))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level` = @level" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@level" + i, userQuizResultWhere[i].Level);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TotalCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_correct` = @totalCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalCorrect" + i, userQuizResultWhere[i].TotalCorrect);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TotalQuestions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_questions` = @totalQuestions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalQuestions" + i, userQuizResultWhere[i].TotalQuestions);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TotalPercent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_percent` = @totalPercent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalPercent" + i, userQuizResultWhere[i].TotalPercent);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Status))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `status` = @status" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@status" + i, userQuizResultWhere[i].Status);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Feedback))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `feedback` = @feedback" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@feedback" + i, userQuizResultWhere[i].Feedback);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizResultWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userQuizResultWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userQuizResultWhere.Count - 1)
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
                            UserQuizResultModel userQuizResult = new UserQuizResultModel(); if (!reader.IsDBNull(0))
                                userQuizResult.Id = reader.GetValue(0).ToString();
                            else
                                userQuizResult.Id = null;
                            if (!reader.IsDBNull(1))
                                userQuizResult.TryNo = reader.GetValue(1).ToString();
                            else
                                userQuizResult.TryNo = null;
                            if (!reader.IsDBNull(2))
                                userQuizResult.UserId = reader.GetValue(2).ToString();
                            else
                                userQuizResult.UserId = null;
                            if (!reader.IsDBNull(3))
                                userQuizResult.QuizId = reader.GetValue(3).ToString();
                            else
                                userQuizResult.QuizId = null;
                            if (!reader.IsDBNull(4))
                                userQuizResult.Dok = reader.GetValue(4).ToString();
                            else
                                userQuizResult.Dok = null;
                            if (!reader.IsDBNull(5))
                                userQuizResult.IsExam = reader.GetValue(5).ToString();
                            else
                                userQuizResult.IsExam = null;
                            if (!reader.IsDBNull(6))
                                userQuizResult.LessonId = reader.GetValue(6).ToString();
                            else
                                userQuizResult.LessonId = null;
                            if (!reader.IsDBNull(7))
                                userQuizResult.Subject = reader.GetValue(7).ToString();
                            else
                                userQuizResult.Subject = null;
                            if (!reader.IsDBNull(8))
                                userQuizResult.Level = reader.GetValue(8).ToString();
                            else
                                userQuizResult.Level = null;
                            if (!reader.IsDBNull(9))
                                userQuizResult.TotalCorrect = reader.GetValue(9).ToString();
                            else
                                userQuizResult.TotalCorrect = null;
                            if (!reader.IsDBNull(10))
                                userQuizResult.TotalQuestions = reader.GetValue(10).ToString();
                            else
                                userQuizResult.TotalQuestions = null;
                            if (!reader.IsDBNull(11))
                                userQuizResult.TotalPercent = reader.GetValue(11).ToString();
                            else
                                userQuizResult.TotalPercent = null;
                            if (!reader.IsDBNull(12))
                                userQuizResult.Status = reader.GetValue(12).ToString();
                            else
                                userQuizResult.Status = null;
                            if (!reader.IsDBNull(13))
                                userQuizResult.Feedback = reader.GetValue(13).ToString();
                            else
                                userQuizResult.Feedback = null;
                            if (!reader.IsDBNull(14))
                                userQuizResult.ModifiedBy = reader.GetValue(14).ToString();
                            else
                                userQuizResult.ModifiedBy = null;
                            if (!reader.IsDBNull(15))
                                userQuizResult.CreatedAt = reader.GetValue(15).ToString();
                            else
                                userQuizResult.CreatedAt = null;
                            if (!reader.IsDBNull(16))
                                userQuizResult.UpdatedAt = reader.GetValue(16).ToString();
                            else
                                userQuizResult.UpdatedAt = null;
                            response.Add(userQuizResult.Id, userQuizResult);
                            userQuizResult = new UserQuizResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserQuizResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserQuizResultModel> Read(string connectionString, UserQuizResultModel userQuizResult, bool firstOne, bool orderByLastDate, UserQuizResultModel userQuizResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizResultModel> response = new List<UserQuizResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };

                string sql = "SELECT `id`, `try_no`, `user_id`, `quiz_id`, `dok`, `is_exam`, `lesson_id`, `subject`, `level`, `total_correct`, `total_questions`, `total_percent`, `status`, `feedback`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_results ";
                {
                    if (userQuizResult != null)
                    {
                        if (!string.IsNullOrEmpty(userQuizResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", userQuizResult.Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TryNo))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `try_no` = @tryNo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `try_no` = @tryNo ");
                            }
                            cmd.Parameters.AddWithValue("@tryNo", userQuizResult.TryNo);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userQuizResult.UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", userQuizResult.QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Dok))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `dok` = @dok ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `dok` = @dok ");
                            }
                            cmd.Parameters.AddWithValue("@dok", userQuizResult.Dok);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.IsExam))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_exam` = @isExam ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_exam` = @isExam ");
                            }
                            cmd.Parameters.AddWithValue("@isExam", userQuizResult.IsExam);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", userQuizResult.LessonId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Subject))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject` = @subject ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject` = @subject ");
                            }
                            cmd.Parameters.AddWithValue("@subject", userQuizResult.Subject);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Level))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `level` = @level ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `level` = @level ");
                            }
                            cmd.Parameters.AddWithValue("@level", userQuizResult.Level);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TotalCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total_correct` = @totalCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total_correct` = @totalCorrect ");
                            }
                            cmd.Parameters.AddWithValue("@totalCorrect", userQuizResult.TotalCorrect);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TotalQuestions))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total_questions` = @totalQuestions ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total_questions` = @totalQuestions ");
                            }
                            cmd.Parameters.AddWithValue("@totalQuestions", userQuizResult.TotalQuestions);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TotalPercent))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total_percent` = @totalPercent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total_percent` = @totalPercent ");
                            }
                            cmd.Parameters.AddWithValue("@totalPercent", userQuizResult.TotalPercent);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Status))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `status` = @status ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `status` = @status ");
                            }
                            cmd.Parameters.AddWithValue("@status", userQuizResult.Status);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Feedback))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `feedback` = @feedback ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `feedback` = @feedback ");
                            }
                            cmd.Parameters.AddWithValue("@feedback", userQuizResult.Feedback);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userQuizResult.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userQuizResult.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userQuizResult.UpdatedAt);
                        }
                    }
                    if (userQuizResultWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userQuizResultWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TryNo))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `try_no` LIKE @tryNoWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `try_no` LIKE @tryNoWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@tryNoWhereLike", "%" + userQuizResultWhereLike.TryNo + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userQuizResultWhereLike.UserId + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + userQuizResultWhereLike.QuizId + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Dok))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `dok` LIKE @dokWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `dok` LIKE @dokWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dokWhereLike", "%" + userQuizResultWhereLike.Dok + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.IsExam))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `is_exam` LIKE @isExamWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `is_exam` LIKE @isExamWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isExamWhereLike", "%" + userQuizResultWhereLike.IsExam + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + userQuizResultWhereLike.LessonId + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Subject))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `subject` LIKE @subjectWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `subject` LIKE @subjectWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subjectWhereLike", "%" + userQuizResultWhereLike.Subject + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Level))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `level` LIKE @levelWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `level` LIKE @levelWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelWhereLike", "%" + userQuizResultWhereLike.Level + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TotalCorrect))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalCorrectWhereLike", "%" + userQuizResultWhereLike.TotalCorrect + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TotalQuestions))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalQuestionsWhereLike", "%" + userQuizResultWhereLike.TotalQuestions + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TotalPercent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalPercentWhereLike", "%" + userQuizResultWhereLike.TotalPercent + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Status))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `status` LIKE @statusWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `status` LIKE @statusWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@statusWhereLike", "%" + userQuizResultWhereLike.Status + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Feedback))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `feedback` LIKE @feedbackWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `feedback` LIKE @feedbackWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@feedbackWhereLike", "%" + userQuizResultWhereLike.Feedback + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userQuizResultWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userQuizResultWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userQuizResultWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = string.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                userQuizResult = new UserQuizResultModel();
                                if (!reader.IsDBNull(0))
                                    userQuizResult.Id = reader.GetValue(0).ToString();
                                else
                                    userQuizResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    userQuizResult.TryNo = reader.GetValue(1).ToString();
                                else
                                    userQuizResult.TryNo = null;
                                if (!reader.IsDBNull(2))
                                    userQuizResult.UserId = reader.GetValue(2).ToString();
                                else
                                    userQuizResult.UserId = null;
                                if (!reader.IsDBNull(3))
                                    userQuizResult.QuizId = reader.GetValue(3).ToString();
                                else
                                    userQuizResult.QuizId = null;
                                if (!reader.IsDBNull(4))
                                    userQuizResult.Dok = reader.GetValue(4).ToString();
                                else
                                    userQuizResult.Dok = null;
                                if (!reader.IsDBNull(5))
                                    userQuizResult.IsExam = reader.GetValue(5).ToString();
                                else
                                    userQuizResult.IsExam = null;
                                if (!reader.IsDBNull(6))
                                    userQuizResult.LessonId = reader.GetValue(6).ToString();
                                else
                                    userQuizResult.LessonId = null;
                                if (!reader.IsDBNull(7))
                                    userQuizResult.Subject = reader.GetValue(7).ToString();
                                else
                                    userQuizResult.Subject = null;
                                if (!reader.IsDBNull(8))
                                    userQuizResult.Level = reader.GetValue(8).ToString();
                                else
                                    userQuizResult.Level = null;
                                if (!reader.IsDBNull(9))
                                    userQuizResult.TotalCorrect = reader.GetValue(9).ToString();
                                else
                                    userQuizResult.TotalCorrect = null;
                                if (!reader.IsDBNull(10))
                                    userQuizResult.TotalQuestions = reader.GetValue(10).ToString();
                                else
                                    userQuizResult.TotalQuestions = null;
                                if (!reader.IsDBNull(11))
                                    userQuizResult.TotalPercent = reader.GetValue(11).ToString();
                                else
                                    userQuizResult.TotalPercent = null;
                                if (!reader.IsDBNull(12))
                                    userQuizResult.Status = reader.GetValue(12).ToString();
                                else
                                    userQuizResult.Status = null;
                                if (!reader.IsDBNull(13))
                                    userQuizResult.Feedback = reader.GetValue(13).ToString();
                                else
                                    userQuizResult.Feedback = null;
                                if (!reader.IsDBNull(14))
                                    userQuizResult.ModifiedBy = reader.GetValue(14).ToString();
                                else
                                    userQuizResult.ModifiedBy = null;
                                if (!reader.IsDBNull(15))
                                    userQuizResult.CreatedAt = reader.GetValue(15).ToString();
                                else
                                    userQuizResult.CreatedAt = null;
                                if (!reader.IsDBNull(16))
                                    userQuizResult.UpdatedAt = reader.GetValue(16).ToString();
                                else
                                    userQuizResult.UpdatedAt = null;
                                response.Add(userQuizResult);
                                userQuizResult = new UserQuizResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserQuizResultModel>();
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
                response = new List<UserQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserQuizResultModel> Read(string connectionString, UserQuizResultModel userQuizResult, bool orderByLastDate, int fromRow, int limit, UserQuizResultModel userQuizResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizResultModel> response = new List<UserQuizResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `try_no`, `user_id`, `quiz_id`, `dok`, `is_exam`, `lesson_id`, `subject`, `level`, `total_correct`, `total_questions`, `total_percent`, `status`, `feedback`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_results ";
                {
                    if (userQuizResult != null)
                    {
                        if (!string.IsNullOrEmpty(userQuizResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", userQuizResult.Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TryNo))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `try_no` = @tryNo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `try_no` = @tryNo ");
                            }
                            cmd.Parameters.AddWithValue("@tryNo", userQuizResult.TryNo);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userQuizResult.UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", userQuizResult.QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Dok))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `dok` = @dok ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `dok` = @dok ");
                            }
                            cmd.Parameters.AddWithValue("@dok", userQuizResult.Dok);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.IsExam))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_exam` = @isExam ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_exam` = @isExam ");
                            }
                            cmd.Parameters.AddWithValue("@isExam", userQuizResult.IsExam);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", userQuizResult.LessonId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Subject))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject` = @subject ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject` = @subject ");
                            }
                            cmd.Parameters.AddWithValue("@subject", userQuizResult.Subject);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Level))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `level` = @level ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `level` = @level ");
                            }
                            cmd.Parameters.AddWithValue("@level", userQuizResult.Level);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TotalCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total_correct` = @totalCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total_correct` = @totalCorrect ");
                            }
                            cmd.Parameters.AddWithValue("@totalCorrect", userQuizResult.TotalCorrect);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TotalQuestions))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total_questions` = @totalQuestions ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total_questions` = @totalQuestions ");
                            }
                            cmd.Parameters.AddWithValue("@totalQuestions", userQuizResult.TotalQuestions);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.TotalPercent))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `total_percent` = @totalPercent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `total_percent` = @totalPercent ");
                            }
                            cmd.Parameters.AddWithValue("@totalPercent", userQuizResult.TotalPercent);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Status))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `status` = @status ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `status` = @status ");
                            }
                            cmd.Parameters.AddWithValue("@status", userQuizResult.Status);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.Feedback))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `feedback` = @feedback ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `feedback` = @feedback ");
                            }
                            cmd.Parameters.AddWithValue("@feedback", userQuizResult.Feedback);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userQuizResult.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userQuizResult.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userQuizResult.UpdatedAt);
                        }
                    }
                    if (userQuizResultWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userQuizResultWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TryNo))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `try_no` LIKE @tryNoWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `try_no` LIKE @tryNoWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@tryNoWhereLike", "%" + userQuizResultWhereLike.TryNo + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userQuizResultWhereLike.UserId + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + userQuizResultWhereLike.QuizId + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Dok))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `dok` LIKE @dokWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `dok` LIKE @dokWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dokWhereLike", "%" + userQuizResultWhereLike.Dok + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.IsExam))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `is_exam` LIKE @isExamWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `is_exam` LIKE @isExamWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isExamWhereLike", "%" + userQuizResultWhereLike.IsExam + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + userQuizResultWhereLike.LessonId + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Subject))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `subject` LIKE @subjectWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `subject` LIKE @subjectWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subjectWhereLike", "%" + userQuizResultWhereLike.Subject + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Level))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `level` LIKE @levelWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `level` LIKE @levelWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelWhereLike", "%" + userQuizResultWhereLike.Level + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TotalCorrect))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `total_correct` LIKE @totalCorrectWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalCorrectWhereLike", "%" + userQuizResultWhereLike.TotalCorrect + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TotalQuestions))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `total_questions` LIKE @totalQuestionsWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalQuestionsWhereLike", "%" + userQuizResultWhereLike.TotalQuestions + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.TotalPercent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalPercentWhereLike", "%" + userQuizResultWhereLike.TotalPercent + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Status))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `status` LIKE @statusWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `status` LIKE @statusWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@statusWhereLike", "%" + userQuizResultWhereLike.Status + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.Feedback))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `feedback` LIKE @feedbackWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `feedback` LIKE @feedbackWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@feedbackWhereLike", "%" + userQuizResultWhereLike.Feedback + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userQuizResultWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userQuizResultWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userQuizResultWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                userQuizResult = new UserQuizResultModel();
                                if (!reader.IsDBNull(0))
                                    userQuizResult.Id = reader.GetValue(0).ToString();
                                else
                                    userQuizResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    userQuizResult.TryNo = reader.GetValue(1).ToString();
                                else
                                    userQuizResult.TryNo = null;
                                if (!reader.IsDBNull(2))
                                    userQuizResult.UserId = reader.GetValue(2).ToString();
                                else
                                    userQuizResult.UserId = null;
                                if (!reader.IsDBNull(3))
                                    userQuizResult.QuizId = reader.GetValue(3).ToString();
                                else
                                    userQuizResult.QuizId = null;
                                if (!reader.IsDBNull(4))
                                    userQuizResult.Dok = reader.GetValue(4).ToString();
                                else
                                    userQuizResult.Dok = null;
                                if (!reader.IsDBNull(5))
                                    userQuizResult.IsExam = reader.GetValue(5).ToString();
                                else
                                    userQuizResult.IsExam = null;
                                if (!reader.IsDBNull(6))
                                    userQuizResult.LessonId = reader.GetValue(6).ToString();
                                else
                                    userQuizResult.LessonId = null;
                                if (!reader.IsDBNull(7))
                                    userQuizResult.Subject = reader.GetValue(7).ToString();
                                else
                                    userQuizResult.Subject = null;
                                if (!reader.IsDBNull(8))
                                    userQuizResult.Level = reader.GetValue(8).ToString();
                                else
                                    userQuizResult.Level = null;
                                if (!reader.IsDBNull(9))
                                    userQuizResult.TotalCorrect = reader.GetValue(9).ToString();
                                else
                                    userQuizResult.TotalCorrect = null;
                                if (!reader.IsDBNull(10))
                                    userQuizResult.TotalQuestions = reader.GetValue(10).ToString();
                                else
                                    userQuizResult.TotalQuestions = null;
                                if (!reader.IsDBNull(11))
                                    userQuizResult.TotalPercent = reader.GetValue(11).ToString();
                                else
                                    userQuizResult.TotalPercent = null;
                                if (!reader.IsDBNull(12))
                                    userQuizResult.Status = reader.GetValue(12).ToString();
                                else
                                    userQuizResult.Status = null;
                                if (!reader.IsDBNull(13))
                                    userQuizResult.Feedback = reader.GetValue(13).ToString();
                                else
                                    userQuizResult.Feedback = null;
                                if (!reader.IsDBNull(14))
                                    userQuizResult.ModifiedBy = reader.GetValue(14).ToString();
                                else
                                    userQuizResult.ModifiedBy = null;
                                if (!reader.IsDBNull(15))
                                    userQuizResult.CreatedAt = reader.GetValue(15).ToString();
                                else
                                    userQuizResult.CreatedAt = null;
                                if (!reader.IsDBNull(16))
                                    userQuizResult.UpdatedAt = reader.GetValue(16).ToString();
                                else
                                    userQuizResult.UpdatedAt = null;
                                response.Add(userQuizResult);
                                userQuizResult = new UserQuizResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserQuizResultModel>();
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
                response = new List<UserQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserQuizResultModel> Read(string connectionString, List<UserQuizResultModel> userQuizResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizResultModel> response = new List<UserQuizResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `try_no`, `user_id`, `quiz_id`, `dok`, `is_exam`, `lesson_id`, `subject`, `level`, `total_correct`, `total_questions`, `total_percent`, `status`, `feedback`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_results ";
                if (userQuizResultWhere != null && userQuizResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userQuizResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userQuizResultWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TryNo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `try_no` = @tryNo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@tryNo" + i, userQuizResultWhere[i].TryNo);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userQuizResultWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, userQuizResultWhere[i].QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Dok))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `dok` = @dok" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dok" + i, userQuizResultWhere[i].Dok);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].IsExam))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_exam` = @isExam" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isExam" + i, userQuizResultWhere[i].IsExam);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, userQuizResultWhere[i].LessonId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Subject))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject` = @subject" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subject" + i, userQuizResultWhere[i].Subject);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Level))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level` = @level" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@level" + i, userQuizResultWhere[i].Level);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TotalCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_correct` = @totalCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalCorrect" + i, userQuizResultWhere[i].TotalCorrect);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TotalQuestions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_questions` = @totalQuestions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalQuestions" + i, userQuizResultWhere[i].TotalQuestions);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TotalPercent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_percent` = @totalPercent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalPercent" + i, userQuizResultWhere[i].TotalPercent);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Status))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `status` = @status" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@status" + i, userQuizResultWhere[i].Status);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Feedback))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `feedback` = @feedback" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@feedback" + i, userQuizResultWhere[i].Feedback);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizResultWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userQuizResultWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userQuizResultWhere.Count - 1)
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
                            UserQuizResultModel userQuizResult = new UserQuizResultModel(); if (!reader.IsDBNull(0))
                                userQuizResult.Id = reader.GetValue(0).ToString();
                            else
                                userQuizResult.Id = null;
                            if (!reader.IsDBNull(1))
                                userQuizResult.TryNo = reader.GetValue(1).ToString();
                            else
                                userQuizResult.TryNo = null;
                            if (!reader.IsDBNull(2))
                                userQuizResult.UserId = reader.GetValue(2).ToString();
                            else
                                userQuizResult.UserId = null;
                            if (!reader.IsDBNull(3))
                                userQuizResult.QuizId = reader.GetValue(3).ToString();
                            else
                                userQuizResult.QuizId = null;
                            if (!reader.IsDBNull(4))
                                userQuizResult.Dok = reader.GetValue(4).ToString();
                            else
                                userQuizResult.Dok = null;
                            if (!reader.IsDBNull(5))
                                userQuizResult.IsExam = reader.GetValue(5).ToString();
                            else
                                userQuizResult.IsExam = null;
                            if (!reader.IsDBNull(6))
                                userQuizResult.LessonId = reader.GetValue(6).ToString();
                            else
                                userQuizResult.LessonId = null;
                            if (!reader.IsDBNull(7))
                                userQuizResult.Subject = reader.GetValue(7).ToString();
                            else
                                userQuizResult.Subject = null;
                            if (!reader.IsDBNull(8))
                                userQuizResult.Level = reader.GetValue(8).ToString();
                            else
                                userQuizResult.Level = null;
                            if (!reader.IsDBNull(9))
                                userQuizResult.TotalCorrect = reader.GetValue(9).ToString();
                            else
                                userQuizResult.TotalCorrect = null;
                            if (!reader.IsDBNull(10))
                                userQuizResult.TotalQuestions = reader.GetValue(10).ToString();
                            else
                                userQuizResult.TotalQuestions = null;
                            if (!reader.IsDBNull(11))
                                userQuizResult.TotalPercent = reader.GetValue(11).ToString();
                            else
                                userQuizResult.TotalPercent = null;
                            if (!reader.IsDBNull(12))
                                userQuizResult.Status = reader.GetValue(12).ToString();
                            else
                                userQuizResult.Status = null;
                            if (!reader.IsDBNull(13))
                                userQuizResult.Feedback = reader.GetValue(13).ToString();
                            else
                                userQuizResult.Feedback = null;
                            if (!reader.IsDBNull(14))
                                userQuizResult.ModifiedBy = reader.GetValue(14).ToString();
                            else
                                userQuizResult.ModifiedBy = null;
                            if (!reader.IsDBNull(15))
                                userQuizResult.CreatedAt = reader.GetValue(15).ToString();
                            else
                                userQuizResult.CreatedAt = null;
                            if (!reader.IsDBNull(16))
                                userQuizResult.UpdatedAt = reader.GetValue(16).ToString();
                            else
                                userQuizResult.UpdatedAt = null;
                            response.Add(userQuizResult);
                            userQuizResult = new UserQuizResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserQuizResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserQuizResultModel> Read(string connectionString, List<UserQuizResultModel> userQuizResultWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizResultModel> response = new List<UserQuizResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `try_no`, `user_id`, `quiz_id`, `dok`, `is_exam`, `lesson_id`, `subject`, `level`, `total_correct`, `total_questions`, `total_percent`, `status`, `feedback`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_results ";
                if (userQuizResultWhere != null && userQuizResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userQuizResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userQuizResultWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TryNo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `try_no` = @tryNo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@tryNo" + i, userQuizResultWhere[i].TryNo);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userQuizResultWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, userQuizResultWhere[i].QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Dok))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `dok` = @dok" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dok" + i, userQuizResultWhere[i].Dok);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].IsExam))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_exam` = @isExam" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isExam" + i, userQuizResultWhere[i].IsExam);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, userQuizResultWhere[i].LessonId);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Subject))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject` = @subject" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subject" + i, userQuizResultWhere[i].Subject);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Level))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level` = @level" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@level" + i, userQuizResultWhere[i].Level);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TotalCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_correct` = @totalCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalCorrect" + i, userQuizResultWhere[i].TotalCorrect);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TotalQuestions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_questions` = @totalQuestions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalQuestions" + i, userQuizResultWhere[i].TotalQuestions);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].TotalPercent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_percent` = @totalPercent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalPercent" + i, userQuizResultWhere[i].TotalPercent);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Status))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `status` = @status" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@status" + i, userQuizResultWhere[i].Status);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].Feedback))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `feedback` = @feedback" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@feedback" + i, userQuizResultWhere[i].Feedback);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizResultWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userQuizResultWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userQuizResultWhere.Count - 1)
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
                            UserQuizResultModel userQuizResult = new UserQuizResultModel(); if (!reader.IsDBNull(0))
                                userQuizResult.Id = reader.GetValue(0).ToString();
                            else
                                userQuizResult.Id = null;
                            if (!reader.IsDBNull(1))
                                userQuizResult.TryNo = reader.GetValue(1).ToString();
                            else
                                userQuizResult.TryNo = null;
                            if (!reader.IsDBNull(2))
                                userQuizResult.UserId = reader.GetValue(2).ToString();
                            else
                                userQuizResult.UserId = null;
                            if (!reader.IsDBNull(3))
                                userQuizResult.QuizId = reader.GetValue(3).ToString();
                            else
                                userQuizResult.QuizId = null;
                            if (!reader.IsDBNull(4))
                                userQuizResult.Dok = reader.GetValue(4).ToString();
                            else
                                userQuizResult.Dok = null;
                            if (!reader.IsDBNull(5))
                                userQuizResult.IsExam = reader.GetValue(5).ToString();
                            else
                                userQuizResult.IsExam = null;
                            if (!reader.IsDBNull(6))
                                userQuizResult.LessonId = reader.GetValue(6).ToString();
                            else
                                userQuizResult.LessonId = null;
                            if (!reader.IsDBNull(7))
                                userQuizResult.Subject = reader.GetValue(7).ToString();
                            else
                                userQuizResult.Subject = null;
                            if (!reader.IsDBNull(8))
                                userQuizResult.Level = reader.GetValue(8).ToString();
                            else
                                userQuizResult.Level = null;
                            if (!reader.IsDBNull(9))
                                userQuizResult.TotalCorrect = reader.GetValue(9).ToString();
                            else
                                userQuizResult.TotalCorrect = null;
                            if (!reader.IsDBNull(10))
                                userQuizResult.TotalQuestions = reader.GetValue(10).ToString();
                            else
                                userQuizResult.TotalQuestions = null;
                            if (!reader.IsDBNull(11))
                                userQuizResult.TotalPercent = reader.GetValue(11).ToString();
                            else
                                userQuizResult.TotalPercent = null;
                            if (!reader.IsDBNull(12))
                                userQuizResult.Status = reader.GetValue(12).ToString();
                            else
                                userQuizResult.Status = null;
                            if (!reader.IsDBNull(13))
                                userQuizResult.Feedback = reader.GetValue(13).ToString();
                            else
                                userQuizResult.Feedback = null;
                            if (!reader.IsDBNull(14))
                                userQuizResult.ModifiedBy = reader.GetValue(14).ToString();
                            else
                                userQuizResult.ModifiedBy = null;
                            if (!reader.IsDBNull(15))
                                userQuizResult.CreatedAt = reader.GetValue(15).ToString();
                            else
                                userQuizResult.CreatedAt = null;
                            if (!reader.IsDBNull(16))
                                userQuizResult.UpdatedAt = reader.GetValue(16).ToString();
                            else
                                userQuizResult.UpdatedAt = null;
                            response.Add(userQuizResult);
                            userQuizResult = new UserQuizResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserQuizResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserQuizResultModel> Create(string connectionString, UserQuizResultModel userQuizResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizResultModel> response = new List<UserQuizResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO usqui_user_quiz_results (";
                string values = "(";
                if (!string.IsNullOrEmpty(userQuizResult.Id))
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
                    cmd.Parameters.AddWithValue("@id", userQuizResult.Id);
                }
                if (!string.IsNullOrEmpty(userQuizResult.TryNo))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `try_no` ");
                        values = string.Concat(values, ", @tryNo");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `try_no` ");
                        values = string.Concat(values, " @tryNo");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@tryNo", userQuizResult.TryNo);
                }
                if (!string.IsNullOrEmpty(userQuizResult.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", userQuizResult.UserId);
                }
                if (!string.IsNullOrEmpty(userQuizResult.QuizId))
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
                    cmd.Parameters.AddWithValue("@quizId", userQuizResult.QuizId);
                }
                if (!string.IsNullOrEmpty(userQuizResult.Dok))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `dok` ");
                        values = string.Concat(values, ", @dok");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `dok` ");
                        values = string.Concat(values, " @dok");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dok", userQuizResult.Dok);
                }
                if (!string.IsNullOrEmpty(userQuizResult.IsExam))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `is_exam` ");
                        values = string.Concat(values, ", @isExam");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `is_exam` ");
                        values = string.Concat(values, " @isExam");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isExam", userQuizResult.IsExam);
                }
                if (!string.IsNullOrEmpty(userQuizResult.LessonId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `lesson_id` ");
                        values = string.Concat(values, ", @lessonId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `lesson_id` ");
                        values = string.Concat(values, " @lessonId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@lessonId", userQuizResult.LessonId);
                }
                if (!string.IsNullOrEmpty(userQuizResult.Subject))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `subject` ");
                        values = string.Concat(values, ", @subject");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `subject` ");
                        values = string.Concat(values, " @subject");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subject", userQuizResult.Subject);
                }
                if (!string.IsNullOrEmpty(userQuizResult.Level))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `level` ");
                        values = string.Concat(values, ", @level");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `level` ");
                        values = string.Concat(values, " @level");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@level", userQuizResult.Level);
                }
                if (!string.IsNullOrEmpty(userQuizResult.TotalCorrect))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `total_correct` ");
                        values = string.Concat(values, ", @totalCorrect");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `total_correct` ");
                        values = string.Concat(values, " @totalCorrect");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalCorrect", userQuizResult.TotalCorrect);
                }
                if (!string.IsNullOrEmpty(userQuizResult.TotalQuestions))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `total_questions` ");
                        values = string.Concat(values, ", @totalQuestions");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `total_questions` ");
                        values = string.Concat(values, " @totalQuestions");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalQuestions", userQuizResult.TotalQuestions);
                }
                if (!string.IsNullOrEmpty(userQuizResult.TotalPercent))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `total_percent` ");
                        values = string.Concat(values, ", @totalPercent");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `total_percent` ");
                        values = string.Concat(values, " @totalPercent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalPercent", userQuizResult.TotalPercent);
                }
                if (!string.IsNullOrEmpty(userQuizResult.Status))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `status` ");
                        values = string.Concat(values, ", @status");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `status` ");
                        values = string.Concat(values, " @status");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@status", userQuizResult.Status);
                }
                if (!string.IsNullOrEmpty(userQuizResult.Feedback))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `feedback` ");
                        values = string.Concat(values, ", @feedback");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `feedback` ");
                        values = string.Concat(values, " @feedback");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@feedback", userQuizResult.Feedback);
                }
                if (!string.IsNullOrEmpty(userQuizResult.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userQuizResult.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userQuizResult.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userQuizResult.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userQuizResult.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userQuizResult.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userQuizResult.Id = cmd.LastInsertedId.ToString();
                response.Add(userQuizResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizResultModel>();
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
        public List<UserQuizResultModel> Create(string connectionString, List<UserQuizResultModel> userQuizResultList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizResultModel> response = new List<UserQuizResultModel>();
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
                        for (int i = 0; i < userQuizResultList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO usqui_user_quiz_results (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(userQuizResultList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userQuizResultList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].TryNo))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `try_no` ");
                                        values = string.Concat(values, ", @tryNo" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `try_no` ");
                                        values = string.Concat(values, "@tryNo" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@tryNo" + i, userQuizResultList[i].TryNo);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, userQuizResultList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].QuizId))
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
                                    cmd.Parameters.AddWithValue("@quizId" + i, userQuizResultList[i].QuizId);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].Dok))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `dok` ");
                                        values = string.Concat(values, ", @dok" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `dok` ");
                                        values = string.Concat(values, "@dok" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dok" + i, userQuizResultList[i].Dok);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].IsExam))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `is_exam` ");
                                        values = string.Concat(values, ", @isExam" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `is_exam` ");
                                        values = string.Concat(values, "@isExam" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isExam" + i, userQuizResultList[i].IsExam);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].LessonId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `lesson_id` ");
                                        values = string.Concat(values, ", @lessonId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `lesson_id` ");
                                        values = string.Concat(values, "@lessonId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@lessonId" + i, userQuizResultList[i].LessonId);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].Subject))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `subject` ");
                                        values = string.Concat(values, ", @subject" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `subject` ");
                                        values = string.Concat(values, "@subject" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subject" + i, userQuizResultList[i].Subject);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].Level))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `level` ");
                                        values = string.Concat(values, ", @level" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `level` ");
                                        values = string.Concat(values, "@level" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@level" + i, userQuizResultList[i].Level);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].TotalCorrect))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `total_correct` ");
                                        values = string.Concat(values, ", @totalCorrect" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `total_correct` ");
                                        values = string.Concat(values, "@totalCorrect" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalCorrect" + i, userQuizResultList[i].TotalCorrect);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].TotalQuestions))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `total_questions` ");
                                        values = string.Concat(values, ", @totalQuestions" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `total_questions` ");
                                        values = string.Concat(values, "@totalQuestions" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalQuestions" + i, userQuizResultList[i].TotalQuestions);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].TotalPercent))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `total_percent` ");
                                        values = string.Concat(values, ", @totalPercent" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `total_percent` ");
                                        values = string.Concat(values, "@totalPercent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalPercent" + i, userQuizResultList[i].TotalPercent);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].Status))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `status` ");
                                        values = string.Concat(values, ", @status" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `status` ");
                                        values = string.Concat(values, "@status" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@status" + i, userQuizResultList[i].Status);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].Feedback))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `feedback` ");
                                        values = string.Concat(values, ", @feedback" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `feedback` ");
                                        values = string.Concat(values, "@feedback" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@feedback" + i, userQuizResultList[i].Feedback);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizResultList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userQuizResultList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizResultList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userQuizResultList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserQuizResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserQuizResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizResultModel>();
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

        public List<UserQuizResultModel> Update(string connectionString, UserQuizResultModel userQuizResult, UserQuizResultModel userQuizResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizResultModel> response = new List<UserQuizResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE usqui_user_quiz_results SET ";
                if (!string.IsNullOrEmpty(userQuizResult.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userQuizResult.Id);
                }
                if (!string.IsNullOrEmpty(userQuizResult.TryNo))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `try_no` = @tryNo");
                    else
                        sql = string.Concat(sql, " `try_no` = @tryNo");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@tryNo", userQuizResult.TryNo);
                }
                if (!string.IsNullOrEmpty(userQuizResult.UserId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = string.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userQuizResult.UserId);
                }
                if (!string.IsNullOrEmpty(userQuizResult.QuizId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `quiz_id` = @quizId");
                    else
                        sql = string.Concat(sql, " `quiz_id` = @quizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", userQuizResult.QuizId);
                }
                if (!string.IsNullOrEmpty(userQuizResult.Dok))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `dok` = @dok");
                    else
                        sql = string.Concat(sql, " `dok` = @dok");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dok", userQuizResult.Dok);
                }
                if (!string.IsNullOrEmpty(userQuizResult.IsExam))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `is_exam` = @isExam");
                    else
                        sql = string.Concat(sql, " `is_exam` = @isExam");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isExam", userQuizResult.IsExam);
                }
                if (!string.IsNullOrEmpty(userQuizResult.LessonId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `lesson_id` = @lessonId");
                    else
                        sql = string.Concat(sql, " `lesson_id` = @lessonId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@lessonId", userQuizResult.LessonId);
                }
                if (!string.IsNullOrEmpty(userQuizResult.Subject))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `subject` = @subject");
                    else
                        sql = string.Concat(sql, " `subject` = @subject");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subject", userQuizResult.Subject);
                }
                if (!string.IsNullOrEmpty(userQuizResult.Level))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `level` = @level");
                    else
                        sql = string.Concat(sql, " `level` = @level");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@level", userQuizResult.Level);
                }
                if (!string.IsNullOrEmpty(userQuizResult.TotalCorrect))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `total_correct` = @totalCorrect");
                    else
                        sql = string.Concat(sql, " `total_correct` = @totalCorrect");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalCorrect", userQuizResult.TotalCorrect);
                }
                if (!string.IsNullOrEmpty(userQuizResult.TotalQuestions))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `total_questions` = @totalQuestions");
                    else
                        sql = string.Concat(sql, " `total_questions` = @totalQuestions");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalQuestions", userQuizResult.TotalQuestions);
                }
                if (!string.IsNullOrEmpty(userQuizResult.TotalPercent))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `total_percent` = @totalPercent");
                    else
                        sql = string.Concat(sql, " `total_percent` = @totalPercent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalPercent", userQuizResult.TotalPercent);
                }
                if (!string.IsNullOrEmpty(userQuizResult.Status))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `status` = @status");
                    else
                        sql = string.Concat(sql, " `status` = @status");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@status", userQuizResult.Status);
                }
                if (!string.IsNullOrEmpty(userQuizResult.Feedback))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `feedback` = @feedback");
                    else
                        sql = string.Concat(sql, " `feedback` = @feedback");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@feedback", userQuizResult.Feedback);
                }
                if (!string.IsNullOrEmpty(userQuizResult.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userQuizResult.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userQuizResult.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userQuizResult.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userQuizResult.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userQuizResult.UpdatedAt);
                }
                if (userQuizResultWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(userQuizResultWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userQuizResultWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.TryNo))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `try_no` = @tryNoWhere");
                        else
                            sql = string.Concat(sql, " `try_no` = @tryNoWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@tryNoWhere", userQuizResultWhere.TryNo);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = string.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userQuizResultWhere.UserId);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.QuizId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `quiz_id` = @quizIdWhere");
                        else
                            sql = string.Concat(sql, " `quiz_id` = @quizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizIdWhere", userQuizResultWhere.QuizId);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.Dok))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `dok` = @dokWhere");
                        else
                            sql = string.Concat(sql, " `dok` = @dokWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dokWhere", userQuizResultWhere.Dok);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.IsExam))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `is_exam` = @isExamWhere");
                        else
                            sql = string.Concat(sql, " `is_exam` = @isExamWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isExamWhere", userQuizResultWhere.IsExam);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.LessonId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `lesson_id` = @lessonIdWhere");
                        else
                            sql = string.Concat(sql, " `lesson_id` = @lessonIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonIdWhere", userQuizResultWhere.LessonId);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.Subject))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subject` = @subjectWhere");
                        else
                            sql = string.Concat(sql, " `subject` = @subjectWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectWhere", userQuizResultWhere.Subject);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.Level))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `level` = @levelWhere");
                        else
                            sql = string.Concat(sql, " `level` = @levelWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelWhere", userQuizResultWhere.Level);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.TotalCorrect))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `total_correct` = @totalCorrectWhere");
                        else
                            sql = string.Concat(sql, " `total_correct` = @totalCorrectWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalCorrectWhere", userQuizResultWhere.TotalCorrect);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.TotalQuestions))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `total_questions` = @totalQuestionsWhere");
                        else
                            sql = string.Concat(sql, " `total_questions` = @totalQuestionsWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalQuestionsWhere", userQuizResultWhere.TotalQuestions);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.TotalPercent))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `total_percent` = @totalPercentWhere");
                        else
                            sql = string.Concat(sql, " `total_percent` = @totalPercentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalPercentWhere", userQuizResultWhere.TotalPercent);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.Status))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `status` = @statusWhere");
                        else
                            sql = string.Concat(sql, " `status` = @statusWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@statusWhere", userQuizResultWhere.Status);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.Feedback))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `feedback` = @feedbackWhere");
                        else
                            sql = string.Concat(sql, " `feedback` = @feedbackWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@feedbackWhere", userQuizResultWhere.Feedback);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userQuizResultWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userQuizResultWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userQuizResultWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userQuizResultWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userQuizResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizResultModel>();
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

        public List<UserQuizResultModel> Update(string connectionString, List<UserQuizResultModel> userQuizResultList, List<UserQuizResultModel> userQuizResultWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizResultModel> response = new List<UserQuizResultModel>();
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
                        for (int i = 0; i < userQuizResultList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE usqui_user_quiz_results SET ";
                                if (!string.IsNullOrEmpty(userQuizResultList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userQuizResultList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].TryNo))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `try_no` = @tryNo" + i);
                                    else
                                        sql = string.Concat(sql, " `try_no` = @tryNo" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@tryNo" + i, userQuizResultList[i].TryNo);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = string.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userQuizResultList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].QuizId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `quiz_id` = @quizId" + i);
                                    else
                                        sql = string.Concat(sql, " `quiz_id` = @quizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, userQuizResultList[i].QuizId);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].Dok))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `dok` = @dok" + i);
                                    else
                                        sql = string.Concat(sql, " `dok` = @dok" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dok" + i, userQuizResultList[i].Dok);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].IsExam))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `is_exam` = @isExam" + i);
                                    else
                                        sql = string.Concat(sql, " `is_exam` = @isExam" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isExam" + i, userQuizResultList[i].IsExam);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].LessonId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `lesson_id` = @lessonId" + i);
                                    else
                                        sql = string.Concat(sql, " `lesson_id` = @lessonId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@lessonId" + i, userQuizResultList[i].LessonId);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].Subject))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `subject` = @subject" + i);
                                    else
                                        sql = string.Concat(sql, " `subject` = @subject" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subject" + i, userQuizResultList[i].Subject);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].Level))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `level` = @level" + i);
                                    else
                                        sql = string.Concat(sql, " `level` = @level" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@level" + i, userQuizResultList[i].Level);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].TotalCorrect))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `total_correct` = @totalCorrect" + i);
                                    else
                                        sql = string.Concat(sql, " `total_correct` = @totalCorrect" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalCorrect" + i, userQuizResultList[i].TotalCorrect);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].TotalQuestions))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `total_questions` = @totalQuestions" + i);
                                    else
                                        sql = string.Concat(sql, " `total_questions` = @totalQuestions" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalQuestions" + i, userQuizResultList[i].TotalQuestions);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].TotalPercent))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `total_percent` = @totalPercent" + i);
                                    else
                                        sql = string.Concat(sql, " `total_percent` = @totalPercent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalPercent" + i, userQuizResultList[i].TotalPercent);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].Status))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `status` = @status" + i);
                                    else
                                        sql = string.Concat(sql, " `status` = @status" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@status" + i, userQuizResultList[i].Status);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].Feedback))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `feedback` = @feedback" + i);
                                    else
                                        sql = string.Concat(sql, " `feedback` = @feedback" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@feedback" + i, userQuizResultList[i].Feedback);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizResultList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userQuizResultList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userQuizResultList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizResultList[i].UpdatedAt);
                                }
                                if (userQuizResultWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userQuizResultWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].TryNo))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `try_no` = @tryNoWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `try_no` = @tryNoWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@tryNoWhere" + i, userQuizResultWhereList[i].TryNo);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userQuizResultWhereList[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `quiz_id` = @quizIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `quiz_id` = @quizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizIdWhere" + i, userQuizResultWhereList[i].QuizId);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].Dok))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `dok` = @dokWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `dok` = @dokWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dokWhere" + i, userQuizResultWhereList[i].Dok);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].IsExam))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `is_exam` = @isExamWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `is_exam` = @isExamWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isExamWhere" + i, userQuizResultWhereList[i].IsExam);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].LessonId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `lesson_id` = @lessonIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `lesson_id` = @lessonIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonIdWhere" + i, userQuizResultWhereList[i].LessonId);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].Subject))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subject` = @subjectWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `subject` = @subjectWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectWhere" + i, userQuizResultWhereList[i].Subject);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].Level))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `level` = @levelWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `level` = @levelWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelWhere" + i, userQuizResultWhereList[i].Level);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].TotalCorrect))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `total_correct` = @totalCorrectWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `total_correct` = @totalCorrectWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalCorrectWhere" + i, userQuizResultWhereList[i].TotalCorrect);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].TotalQuestions))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `total_questions` = @totalQuestionsWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `total_questions` = @totalQuestionsWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalQuestionsWhere" + i, userQuizResultWhereList[i].TotalQuestions);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].TotalPercent))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `total_percent` = @totalPercentWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `total_percent` = @totalPercentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalPercentWhere" + i, userQuizResultWhereList[i].TotalPercent);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].Status))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `status` = @statusWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `status` = @statusWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@statusWhere" + i, userQuizResultWhereList[i].Status);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].Feedback))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `feedback` = @feedbackWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `feedback` = @feedbackWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@feedbackWhere" + i, userQuizResultWhereList[i].Feedback);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userQuizResultWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userQuizResultWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResultWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userQuizResultWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userQuizResultList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserQuizResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserQuizResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizResultModel>();
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

        public List<UserQuizResultModel> Delete(string connectionString, UserQuizResultModel userQuizResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizResultModel> response = new List<UserQuizResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM usqui_user_quiz_results  ";
                if (userQuizResult != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(userQuizResult.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userQuizResult.Id);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.TryNo))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `try_no` = @tryNo");
                        else
                            sql = string.Concat(sql, " `try_no` = @tryNo");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@tryNo", userQuizResult.TryNo);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = string.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userQuizResult.UserId);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.QuizId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `quiz_id` = @quizId");
                        else
                            sql = string.Concat(sql, " `quiz_id` = @quizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizId", userQuizResult.QuizId);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.Dok))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `dok` = @dok");
                        else
                            sql = string.Concat(sql, " `dok` = @dok");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dok", userQuizResult.Dok);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.IsExam))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `is_exam` = @isExam");
                        else
                            sql = string.Concat(sql, " `is_exam` = @isExam");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isExam", userQuizResult.IsExam);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.LessonId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `lesson_id` = @lessonId");
                        else
                            sql = string.Concat(sql, " `lesson_id` = @lessonId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonId", userQuizResult.LessonId);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.Subject))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subject` = @subject");
                        else
                            sql = string.Concat(sql, " `subject` = @subject");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subject", userQuizResult.Subject);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.Level))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `level` = @level");
                        else
                            sql = string.Concat(sql, " `level` = @level");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@level", userQuizResult.Level);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.TotalCorrect))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `total_correct` = @totalCorrect");
                        else
                            sql = string.Concat(sql, " `total_correct` = @totalCorrect");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalCorrect", userQuizResult.TotalCorrect);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.TotalQuestions))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `total_questions` = @totalQuestions");
                        else
                            sql = string.Concat(sql, " `total_questions` = @totalQuestions");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalQuestions", userQuizResult.TotalQuestions);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.TotalPercent))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `total_percent` = @totalPercent");
                        else
                            sql = string.Concat(sql, " `total_percent` = @totalPercent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalPercent", userQuizResult.TotalPercent);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.Status))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `status` = @status");
                        else
                            sql = string.Concat(sql, " `status` = @status");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@status", userQuizResult.Status);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.Feedback))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `feedback` = @feedback");
                        else
                            sql = string.Concat(sql, " `feedback` = @feedback");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@feedback", userQuizResult.Feedback);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userQuizResult.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userQuizResult.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userQuizResult.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userQuizResult.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userQuizResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizResultModel>();
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

        public List<UserQuizResultModel> Delete(string connectionString, List<UserQuizResultModel> userQuizResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizResultModel> response = new List<UserQuizResultModel>();
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
                        for (int i = 0; i < userQuizResult.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM usqui_user_quiz_results  ";
                                if (userQuizResult[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(userQuizResult[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userQuizResult[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].TryNo))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `try_no` = @tryNo" + i);
                                        else
                                            sql = string.Concat(sql, " `try_no` = @tryNo" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@tryNo" + i, userQuizResult[i].TryNo);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userQuizResult[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `quiz_id` = @quizId" + i);
                                        else
                                            sql = string.Concat(sql, " `quiz_id` = @quizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizId" + i, userQuizResult[i].QuizId);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].Dok))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `dok` = @dok" + i);
                                        else
                                            sql = string.Concat(sql, " `dok` = @dok" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dok" + i, userQuizResult[i].Dok);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].IsExam))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `is_exam` = @isExam" + i);
                                        else
                                            sql = string.Concat(sql, " `is_exam` = @isExam" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isExam" + i, userQuizResult[i].IsExam);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].LessonId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `lesson_id` = @lessonId" + i);
                                        else
                                            sql = string.Concat(sql, " `lesson_id` = @lessonId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonId" + i, userQuizResult[i].LessonId);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].Subject))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subject` = @subject" + i);
                                        else
                                            sql = string.Concat(sql, " `subject` = @subject" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subject" + i, userQuizResult[i].Subject);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].Level))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `level` = @level" + i);
                                        else
                                            sql = string.Concat(sql, " `level` = @level" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@level" + i, userQuizResult[i].Level);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].TotalCorrect))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `total_correct` = @totalCorrect" + i);
                                        else
                                            sql = string.Concat(sql, " `total_correct` = @totalCorrect" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalCorrect" + i, userQuizResult[i].TotalCorrect);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].TotalQuestions))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `total_questions` = @totalQuestions" + i);
                                        else
                                            sql = string.Concat(sql, " `total_questions` = @totalQuestions" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalQuestions" + i, userQuizResult[i].TotalQuestions);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].TotalPercent))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `total_percent` = @totalPercent" + i);
                                        else
                                            sql = string.Concat(sql, " `total_percent` = @totalPercent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalPercent" + i, userQuizResult[i].TotalPercent);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].Status))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `status` = @status" + i);
                                        else
                                            sql = string.Concat(sql, " `status` = @status" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@status" + i, userQuizResult[i].Status);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].Feedback))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `feedback` = @feedback" + i);
                                        else
                                            sql = string.Concat(sql, " `feedback` = @feedback" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@feedback" + i, userQuizResult[i].Feedback);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizResult[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userQuizResult[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizResult[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizResult[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userQuizResult[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserQuizResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserQuizResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizResultModel>();
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
