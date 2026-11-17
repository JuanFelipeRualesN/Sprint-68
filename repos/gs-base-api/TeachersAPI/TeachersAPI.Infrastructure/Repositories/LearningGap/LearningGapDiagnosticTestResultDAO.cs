using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.LearningGap;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.LearningGap
{
    class LearningGapDiagnosticTestResultDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, LearningGapDiagnosticTestResultModel> ReadD(string connectionString, LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResult, bool firstOne, bool orderByLastDate, LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapDiagnosticTestResultModel> response = new Dictionary<string, LearningGapDiagnosticTestResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `student_id`, `quiz_id`, `subject_code`, `level_code`, `total_correct`, `total_questions`, `total_percent`, `total_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_results ";
                {
                    if (learningGapDiagnosticTestResult != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapDiagnosticTestResult.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.StudentId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `student_id` = @studentId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `student_id` = @studentId ");
                            }
                            cmd.Parameters.AddWithValue("@studentId", learningGapDiagnosticTestResult.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", learningGapDiagnosticTestResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.SubjectCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `subject_code` = @subjectCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `subject_code` = @subjectCode ");
                            }
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapDiagnosticTestResult.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.LevelCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `level_code` = @levelCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `level_code` = @levelCode ");
                            }
                            cmd.Parameters.AddWithValue("@levelCode", learningGapDiagnosticTestResult.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalCorrect))
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
                            cmd.Parameters.AddWithValue("@totalCorrect", learningGapDiagnosticTestResult.TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalQuestions))
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
                            cmd.Parameters.AddWithValue("@totalQuestions", learningGapDiagnosticTestResult.TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalPercent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_percent` = @totalPercent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_percent` = @totalPercent ");
                            }
                            cmd.Parameters.AddWithValue("@totalPercent", learningGapDiagnosticTestResult.TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_score` = @totalScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_score` = @totalScore ");
                            }
                            cmd.Parameters.AddWithValue("@totalScore", learningGapDiagnosticTestResult.TotalScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticTestResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticTestResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticTestResult.UpdatedAt);
                        }
                    }
                    if (learningGapDiagnosticTestResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.StudentId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `student_id` LIKE @studentIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `student_id` LIKE @studentIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.SubjectCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.LevelCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `level_code` LIKE @levelCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `level_code` LIKE @levelCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalCorrect))
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
                            cmd.Parameters.AddWithValue("@totalCorrectWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalCorrect + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalQuestions))
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
                            cmd.Parameters.AddWithValue("@totalQuestionsWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalQuestions + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalPercent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalPercentWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalPercent + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_score` LIKE @totalScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_score` LIKE @totalScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalScoreWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.UpdatedAt + "%");
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
                                learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel();
                                if (!reader.IsDBNull(0))
                                    learningGapDiagnosticTestResult.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapDiagnosticTestResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapDiagnosticTestResult.StudentId = reader.GetValue(1).ToString();
                                else
                                    learningGapDiagnosticTestResult.StudentId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapDiagnosticTestResult.QuizId = reader.GetValue(2).ToString();
                                else
                                    learningGapDiagnosticTestResult.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapDiagnosticTestResult.SubjectCode = reader.GetValue(3).ToString();
                                else
                                    learningGapDiagnosticTestResult.SubjectCode = null;
                                if (!reader.IsDBNull(4))
                                    learningGapDiagnosticTestResult.LevelCode = reader.GetValue(4).ToString();
                                else
                                    learningGapDiagnosticTestResult.LevelCode = null;
                                if (!reader.IsDBNull(5))
                                    learningGapDiagnosticTestResult.TotalCorrect = reader.GetValue(5).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalCorrect = null;
                                if (!reader.IsDBNull(6))
                                    learningGapDiagnosticTestResult.TotalQuestions = reader.GetValue(6).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalQuestions = null;
                                if (!reader.IsDBNull(7))
                                    learningGapDiagnosticTestResult.TotalPercent = reader.GetValue(7).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalPercent = null;
                                if (!reader.IsDBNull(8))
                                    learningGapDiagnosticTestResult.TotalScore = reader.GetValue(8).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalScore = null;
                                if (!reader.IsDBNull(9))
                                    learningGapDiagnosticTestResult.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    learningGapDiagnosticTestResult.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    learningGapDiagnosticTestResult.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    learningGapDiagnosticTestResult.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    learningGapDiagnosticTestResult.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    learningGapDiagnosticTestResult.UpdatedAt = null;
                                response.Add(learningGapDiagnosticTestResult.Id, learningGapDiagnosticTestResult);
                                learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapDiagnosticTestResultModel>();
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
                response = new Dictionary<string, LearningGapDiagnosticTestResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapDiagnosticTestResultModel> ReadD(string connectionString, LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResult, bool orderByLastDate, int fromRow, int limit, LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapDiagnosticTestResultModel> response = new Dictionary<string, LearningGapDiagnosticTestResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `student_id`, `quiz_id`, `subject_code`, `level_code`, `total_correct`, `total_questions`, `total_percent`, `total_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_results ";
                {
                    if (learningGapDiagnosticTestResult != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapDiagnosticTestResult.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.StudentId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `student_id` = @studentId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `student_id` = @studentId ");
                            }
                            cmd.Parameters.AddWithValue("@studentId", learningGapDiagnosticTestResult.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", learningGapDiagnosticTestResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.SubjectCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `subject_code` = @subjectCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `subject_code` = @subjectCode ");
                            }
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapDiagnosticTestResult.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.LevelCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `level_code` = @levelCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `level_code` = @levelCode ");
                            }
                            cmd.Parameters.AddWithValue("@levelCode", learningGapDiagnosticTestResult.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalCorrect))
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
                            cmd.Parameters.AddWithValue("@totalCorrect", learningGapDiagnosticTestResult.TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalQuestions))
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
                            cmd.Parameters.AddWithValue("@totalQuestions", learningGapDiagnosticTestResult.TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalPercent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_percent` = @totalPercent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_percent` = @totalPercent ");
                            }
                            cmd.Parameters.AddWithValue("@totalPercent", learningGapDiagnosticTestResult.TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_score` = @totalScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_score` = @totalScore ");
                            }
                            cmd.Parameters.AddWithValue("@totalScore", learningGapDiagnosticTestResult.TotalScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticTestResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticTestResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticTestResult.UpdatedAt);
                        }
                    }
                    if (learningGapDiagnosticTestResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.StudentId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `student_id` LIKE @studentIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `student_id` LIKE @studentIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.SubjectCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.LevelCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `level_code` LIKE @levelCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `level_code` LIKE @levelCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalCorrect))
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
                            cmd.Parameters.AddWithValue("@totalCorrectWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalCorrect + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalQuestions))
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
                            cmd.Parameters.AddWithValue("@totalQuestionsWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalQuestions + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalPercent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalPercentWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalPercent + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_score` LIKE @totalScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_score` LIKE @totalScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalScoreWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.UpdatedAt + "%");
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
                                learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel();
                                if (!reader.IsDBNull(0))
                                    learningGapDiagnosticTestResult.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapDiagnosticTestResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapDiagnosticTestResult.StudentId = reader.GetValue(1).ToString();
                                else
                                    learningGapDiagnosticTestResult.StudentId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapDiagnosticTestResult.QuizId = reader.GetValue(2).ToString();
                                else
                                    learningGapDiagnosticTestResult.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapDiagnosticTestResult.SubjectCode = reader.GetValue(3).ToString();
                                else
                                    learningGapDiagnosticTestResult.SubjectCode = null;
                                if (!reader.IsDBNull(4))
                                    learningGapDiagnosticTestResult.LevelCode = reader.GetValue(4).ToString();
                                else
                                    learningGapDiagnosticTestResult.LevelCode = null;
                                if (!reader.IsDBNull(5))
                                    learningGapDiagnosticTestResult.TotalCorrect = reader.GetValue(5).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalCorrect = null;
                                if (!reader.IsDBNull(6))
                                    learningGapDiagnosticTestResult.TotalQuestions = reader.GetValue(6).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalQuestions = null;
                                if (!reader.IsDBNull(7))
                                    learningGapDiagnosticTestResult.TotalPercent = reader.GetValue(7).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalPercent = null;
                                if (!reader.IsDBNull(8))
                                    learningGapDiagnosticTestResult.TotalScore = reader.GetValue(8).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalScore = null;
                                if (!reader.IsDBNull(9))
                                    learningGapDiagnosticTestResult.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    learningGapDiagnosticTestResult.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    learningGapDiagnosticTestResult.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    learningGapDiagnosticTestResult.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    learningGapDiagnosticTestResult.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    learningGapDiagnosticTestResult.UpdatedAt = null;
                                response.Add(learningGapDiagnosticTestResult.Id, learningGapDiagnosticTestResult);
                                learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapDiagnosticTestResultModel>();
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
                response = new Dictionary<string, LearningGapDiagnosticTestResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapDiagnosticTestResultModel> ReadD(string connectionString, List<LearningGapDiagnosticTestResultModel> learningGapDiagnosticTestResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapDiagnosticTestResultModel> response = new Dictionary<string, LearningGapDiagnosticTestResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `student_id`, `quiz_id`, `subject_code`, `level_code`, `total_correct`, `total_questions`, `total_percent`, `total_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_results ";
                if (learningGapDiagnosticTestResultWhere != null && learningGapDiagnosticTestResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapDiagnosticTestResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticTestResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapDiagnosticTestResultWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, learningGapDiagnosticTestResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapDiagnosticTestResultWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, learningGapDiagnosticTestResultWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_correct` = @totalCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalCorrect" + i, learningGapDiagnosticTestResultWhere[i].TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalQuestions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_questions` = @totalQuestions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalQuestions" + i, learningGapDiagnosticTestResultWhere[i].TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalPercent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_percent` = @totalPercent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalPercent" + i, learningGapDiagnosticTestResultWhere[i].TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_score` = @totalScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalScore" + i, learningGapDiagnosticTestResultWhere[i].TotalScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticTestResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticTestResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticTestResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapDiagnosticTestResultWhere.Count - 1)
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
                            LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel(); if (!reader.IsDBNull(0))
                                learningGapDiagnosticTestResult.Id = reader.GetValue(0).ToString();
                            else
                                learningGapDiagnosticTestResult.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapDiagnosticTestResult.StudentId = reader.GetValue(1).ToString();
                            else
                                learningGapDiagnosticTestResult.StudentId = null;
                            if (!reader.IsDBNull(2))
                                learningGapDiagnosticTestResult.QuizId = reader.GetValue(2).ToString();
                            else
                                learningGapDiagnosticTestResult.QuizId = null;
                            if (!reader.IsDBNull(3))
                                learningGapDiagnosticTestResult.SubjectCode = reader.GetValue(3).ToString();
                            else
                                learningGapDiagnosticTestResult.SubjectCode = null;
                            if (!reader.IsDBNull(4))
                                learningGapDiagnosticTestResult.LevelCode = reader.GetValue(4).ToString();
                            else
                                learningGapDiagnosticTestResult.LevelCode = null;
                            if (!reader.IsDBNull(5))
                                learningGapDiagnosticTestResult.TotalCorrect = reader.GetValue(5).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalCorrect = null;
                            if (!reader.IsDBNull(6))
                                learningGapDiagnosticTestResult.TotalQuestions = reader.GetValue(6).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalQuestions = null;
                            if (!reader.IsDBNull(7))
                                learningGapDiagnosticTestResult.TotalPercent = reader.GetValue(7).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalPercent = null;
                            if (!reader.IsDBNull(8))
                                learningGapDiagnosticTestResult.TotalScore = reader.GetValue(8).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalScore = null;
                            if (!reader.IsDBNull(9))
                                learningGapDiagnosticTestResult.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                learningGapDiagnosticTestResult.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                learningGapDiagnosticTestResult.CreatedAt = reader.GetValue(10).ToString();
                            else
                                learningGapDiagnosticTestResult.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                learningGapDiagnosticTestResult.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                learningGapDiagnosticTestResult.UpdatedAt = null;
                            response.Add(learningGapDiagnosticTestResult.Id, learningGapDiagnosticTestResult);
                            learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapDiagnosticTestResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapDiagnosticTestResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapDiagnosticTestResultModel> ReadD(string connectionString, List<LearningGapDiagnosticTestResultModel> learningGapDiagnosticTestResultWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapDiagnosticTestResultModel> response = new Dictionary<string, LearningGapDiagnosticTestResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `student_id`, `quiz_id`, `subject_code`, `level_code`, `total_correct`, `total_questions`, `total_percent`, `total_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_results ";
                if (learningGapDiagnosticTestResultWhere != null && learningGapDiagnosticTestResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapDiagnosticTestResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticTestResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapDiagnosticTestResultWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, learningGapDiagnosticTestResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapDiagnosticTestResultWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, learningGapDiagnosticTestResultWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_correct` = @totalCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalCorrect" + i, learningGapDiagnosticTestResultWhere[i].TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalQuestions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_questions` = @totalQuestions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalQuestions" + i, learningGapDiagnosticTestResultWhere[i].TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalPercent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_percent` = @totalPercent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalPercent" + i, learningGapDiagnosticTestResultWhere[i].TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_score` = @totalScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalScore" + i, learningGapDiagnosticTestResultWhere[i].TotalScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticTestResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticTestResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticTestResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapDiagnosticTestResultWhere.Count - 1)
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
                            LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel(); if (!reader.IsDBNull(0))
                                learningGapDiagnosticTestResult.Id = reader.GetValue(0).ToString();
                            else
                                learningGapDiagnosticTestResult.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapDiagnosticTestResult.StudentId = reader.GetValue(1).ToString();
                            else
                                learningGapDiagnosticTestResult.StudentId = null;
                            if (!reader.IsDBNull(2))
                                learningGapDiagnosticTestResult.QuizId = reader.GetValue(2).ToString();
                            else
                                learningGapDiagnosticTestResult.QuizId = null;
                            if (!reader.IsDBNull(3))
                                learningGapDiagnosticTestResult.SubjectCode = reader.GetValue(3).ToString();
                            else
                                learningGapDiagnosticTestResult.SubjectCode = null;
                            if (!reader.IsDBNull(4))
                                learningGapDiagnosticTestResult.LevelCode = reader.GetValue(4).ToString();
                            else
                                learningGapDiagnosticTestResult.LevelCode = null;
                            if (!reader.IsDBNull(5))
                                learningGapDiagnosticTestResult.TotalCorrect = reader.GetValue(5).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalCorrect = null;
                            if (!reader.IsDBNull(6))
                                learningGapDiagnosticTestResult.TotalQuestions = reader.GetValue(6).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalQuestions = null;
                            if (!reader.IsDBNull(7))
                                learningGapDiagnosticTestResult.TotalPercent = reader.GetValue(7).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalPercent = null;
                            if (!reader.IsDBNull(8))
                                learningGapDiagnosticTestResult.TotalScore = reader.GetValue(8).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalScore = null;
                            if (!reader.IsDBNull(9))
                                learningGapDiagnosticTestResult.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                learningGapDiagnosticTestResult.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                learningGapDiagnosticTestResult.CreatedAt = reader.GetValue(10).ToString();
                            else
                                learningGapDiagnosticTestResult.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                learningGapDiagnosticTestResult.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                learningGapDiagnosticTestResult.UpdatedAt = null;
                            response.Add(learningGapDiagnosticTestResult.Id, learningGapDiagnosticTestResult);
                            learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapDiagnosticTestResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapDiagnosticTestResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapDiagnosticTestResultModel> Read(string connectionString, LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResult, bool firstOne, bool orderByLastDate, LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultModel> response = new List<LearningGapDiagnosticTestResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `student_id`, `quiz_id`, `subject_code`, `level_code`, `total_correct`, `total_questions`, `total_percent`, `total_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_results ";
                {
                    if (learningGapDiagnosticTestResult != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapDiagnosticTestResult.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.StudentId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `student_id` = @studentId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `student_id` = @studentId ");
                            }
                            cmd.Parameters.AddWithValue("@studentId", learningGapDiagnosticTestResult.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", learningGapDiagnosticTestResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.SubjectCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `subject_code` = @subjectCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `subject_code` = @subjectCode ");
                            }
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapDiagnosticTestResult.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.LevelCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `level_code` = @levelCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `level_code` = @levelCode ");
                            }
                            cmd.Parameters.AddWithValue("@levelCode", learningGapDiagnosticTestResult.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalCorrect))
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
                            cmd.Parameters.AddWithValue("@totalCorrect", learningGapDiagnosticTestResult.TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalQuestions))
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
                            cmd.Parameters.AddWithValue("@totalQuestions", learningGapDiagnosticTestResult.TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalPercent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_percent` = @totalPercent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_percent` = @totalPercent ");
                            }
                            cmd.Parameters.AddWithValue("@totalPercent", learningGapDiagnosticTestResult.TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_score` = @totalScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_score` = @totalScore ");
                            }
                            cmd.Parameters.AddWithValue("@totalScore", learningGapDiagnosticTestResult.TotalScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticTestResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticTestResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticTestResult.UpdatedAt);
                        }
                    }
                    if (learningGapDiagnosticTestResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.StudentId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `student_id` LIKE @studentIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `student_id` LIKE @studentIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.SubjectCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.LevelCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `level_code` LIKE @levelCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `level_code` LIKE @levelCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalCorrect))
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
                            cmd.Parameters.AddWithValue("@totalCorrectWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalCorrect + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalQuestions))
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
                            cmd.Parameters.AddWithValue("@totalQuestionsWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalQuestions + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalPercent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalPercentWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalPercent + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_score` LIKE @totalScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_score` LIKE @totalScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalScoreWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.UpdatedAt + "%");
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
                                learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel();
                                if (!reader.IsDBNull(0))
                                    learningGapDiagnosticTestResult.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapDiagnosticTestResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapDiagnosticTestResult.StudentId = reader.GetValue(1).ToString();
                                else
                                    learningGapDiagnosticTestResult.StudentId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapDiagnosticTestResult.QuizId = reader.GetValue(2).ToString();
                                else
                                    learningGapDiagnosticTestResult.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapDiagnosticTestResult.SubjectCode = reader.GetValue(3).ToString();
                                else
                                    learningGapDiagnosticTestResult.SubjectCode = null;
                                if (!reader.IsDBNull(4))
                                    learningGapDiagnosticTestResult.LevelCode = reader.GetValue(4).ToString();
                                else
                                    learningGapDiagnosticTestResult.LevelCode = null;
                                if (!reader.IsDBNull(5))
                                    learningGapDiagnosticTestResult.TotalCorrect = reader.GetValue(5).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalCorrect = null;
                                if (!reader.IsDBNull(6))
                                    learningGapDiagnosticTestResult.TotalQuestions = reader.GetValue(6).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalQuestions = null;
                                if (!reader.IsDBNull(7))
                                    learningGapDiagnosticTestResult.TotalPercent = reader.GetValue(7).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalPercent = null;
                                if (!reader.IsDBNull(8))
                                    learningGapDiagnosticTestResult.TotalScore = reader.GetValue(8).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalScore = null;
                                if (!reader.IsDBNull(9))
                                    learningGapDiagnosticTestResult.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    learningGapDiagnosticTestResult.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    learningGapDiagnosticTestResult.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    learningGapDiagnosticTestResult.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    learningGapDiagnosticTestResult.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    learningGapDiagnosticTestResult.UpdatedAt = null;
                                response.Add(learningGapDiagnosticTestResult);
                                learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapDiagnosticTestResultModel>();
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
                response = new List<LearningGapDiagnosticTestResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapDiagnosticTestResultModel> Read(string connectionString, LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResult, bool orderByLastDate, int fromRow, int limit, LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultModel> response = new List<LearningGapDiagnosticTestResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `student_id`, `quiz_id`, `subject_code`, `level_code`, `total_correct`, `total_questions`, `total_percent`, `total_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_results ";
                {
                    if (learningGapDiagnosticTestResult != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapDiagnosticTestResult.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.StudentId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `student_id` = @studentId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `student_id` = @studentId ");
                            }
                            cmd.Parameters.AddWithValue("@studentId", learningGapDiagnosticTestResult.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", learningGapDiagnosticTestResult.QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.SubjectCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `subject_code` = @subjectCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `subject_code` = @subjectCode ");
                            }
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapDiagnosticTestResult.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.LevelCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `level_code` = @levelCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `level_code` = @levelCode ");
                            }
                            cmd.Parameters.AddWithValue("@levelCode", learningGapDiagnosticTestResult.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalCorrect))
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
                            cmd.Parameters.AddWithValue("@totalCorrect", learningGapDiagnosticTestResult.TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalQuestions))
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
                            cmd.Parameters.AddWithValue("@totalQuestions", learningGapDiagnosticTestResult.TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalPercent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_percent` = @totalPercent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_percent` = @totalPercent ");
                            }
                            cmd.Parameters.AddWithValue("@totalPercent", learningGapDiagnosticTestResult.TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_score` = @totalScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_score` = @totalScore ");
                            }
                            cmd.Parameters.AddWithValue("@totalScore", learningGapDiagnosticTestResult.TotalScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticTestResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticTestResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticTestResult.UpdatedAt);
                        }
                    }
                    if (learningGapDiagnosticTestResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.StudentId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `student_id` LIKE @studentIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `student_id` LIKE @studentIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.SubjectCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.LevelCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `level_code` LIKE @levelCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `level_code` LIKE @levelCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalCorrect))
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
                            cmd.Parameters.AddWithValue("@totalCorrectWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalCorrect + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalQuestions))
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
                            cmd.Parameters.AddWithValue("@totalQuestionsWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalQuestions + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalPercent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalPercentWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalPercent + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.TotalScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_score` LIKE @totalScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_score` LIKE @totalScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalScoreWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.TotalScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapDiagnosticTestResultWhereLike.UpdatedAt + "%");
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
                                learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel();
                                if (!reader.IsDBNull(0))
                                    learningGapDiagnosticTestResult.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapDiagnosticTestResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapDiagnosticTestResult.StudentId = reader.GetValue(1).ToString();
                                else
                                    learningGapDiagnosticTestResult.StudentId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapDiagnosticTestResult.QuizId = reader.GetValue(2).ToString();
                                else
                                    learningGapDiagnosticTestResult.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapDiagnosticTestResult.SubjectCode = reader.GetValue(3).ToString();
                                else
                                    learningGapDiagnosticTestResult.SubjectCode = null;
                                if (!reader.IsDBNull(4))
                                    learningGapDiagnosticTestResult.LevelCode = reader.GetValue(4).ToString();
                                else
                                    learningGapDiagnosticTestResult.LevelCode = null;
                                if (!reader.IsDBNull(5))
                                    learningGapDiagnosticTestResult.TotalCorrect = reader.GetValue(5).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalCorrect = null;
                                if (!reader.IsDBNull(6))
                                    learningGapDiagnosticTestResult.TotalQuestions = reader.GetValue(6).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalQuestions = null;
                                if (!reader.IsDBNull(7))
                                    learningGapDiagnosticTestResult.TotalPercent = reader.GetValue(7).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalPercent = null;
                                if (!reader.IsDBNull(8))
                                    learningGapDiagnosticTestResult.TotalScore = reader.GetValue(8).ToString();
                                else
                                    learningGapDiagnosticTestResult.TotalScore = null;
                                if (!reader.IsDBNull(9))
                                    learningGapDiagnosticTestResult.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    learningGapDiagnosticTestResult.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    learningGapDiagnosticTestResult.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    learningGapDiagnosticTestResult.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    learningGapDiagnosticTestResult.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    learningGapDiagnosticTestResult.UpdatedAt = null;
                                response.Add(learningGapDiagnosticTestResult);
                                learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapDiagnosticTestResultModel>();
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
                response = new List<LearningGapDiagnosticTestResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapDiagnosticTestResultModel> Read(string connectionString, List<LearningGapDiagnosticTestResultModel> learningGapDiagnosticTestResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultModel> response = new List<LearningGapDiagnosticTestResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `student_id`, `quiz_id`, `subject_code`, `level_code`, `total_correct`, `total_questions`, `total_percent`, `total_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_results ";
                if (learningGapDiagnosticTestResultWhere != null && learningGapDiagnosticTestResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapDiagnosticTestResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticTestResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapDiagnosticTestResultWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, learningGapDiagnosticTestResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapDiagnosticTestResultWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, learningGapDiagnosticTestResultWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_correct` = @totalCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalCorrect" + i, learningGapDiagnosticTestResultWhere[i].TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalQuestions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_questions` = @totalQuestions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalQuestions" + i, learningGapDiagnosticTestResultWhere[i].TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalPercent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_percent` = @totalPercent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalPercent" + i, learningGapDiagnosticTestResultWhere[i].TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_score` = @totalScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalScore" + i, learningGapDiagnosticTestResultWhere[i].TotalScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticTestResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticTestResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticTestResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapDiagnosticTestResultWhere.Count - 1)
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
                            LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel(); if (!reader.IsDBNull(0))
                                learningGapDiagnosticTestResult.Id = reader.GetValue(0).ToString();
                            else
                                learningGapDiagnosticTestResult.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapDiagnosticTestResult.StudentId = reader.GetValue(1).ToString();
                            else
                                learningGapDiagnosticTestResult.StudentId = null;
                            if (!reader.IsDBNull(2))
                                learningGapDiagnosticTestResult.QuizId = reader.GetValue(2).ToString();
                            else
                                learningGapDiagnosticTestResult.QuizId = null;
                            if (!reader.IsDBNull(3))
                                learningGapDiagnosticTestResult.SubjectCode = reader.GetValue(3).ToString();
                            else
                                learningGapDiagnosticTestResult.SubjectCode = null;
                            if (!reader.IsDBNull(4))
                                learningGapDiagnosticTestResult.LevelCode = reader.GetValue(4).ToString();
                            else
                                learningGapDiagnosticTestResult.LevelCode = null;
                            if (!reader.IsDBNull(5))
                                learningGapDiagnosticTestResult.TotalCorrect = reader.GetValue(5).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalCorrect = null;
                            if (!reader.IsDBNull(6))
                                learningGapDiagnosticTestResult.TotalQuestions = reader.GetValue(6).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalQuestions = null;
                            if (!reader.IsDBNull(7))
                                learningGapDiagnosticTestResult.TotalPercent = reader.GetValue(7).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalPercent = null;
                            if (!reader.IsDBNull(8))
                                learningGapDiagnosticTestResult.TotalScore = reader.GetValue(8).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalScore = null;
                            if (!reader.IsDBNull(9))
                                learningGapDiagnosticTestResult.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                learningGapDiagnosticTestResult.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                learningGapDiagnosticTestResult.CreatedAt = reader.GetValue(10).ToString();
                            else
                                learningGapDiagnosticTestResult.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                learningGapDiagnosticTestResult.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                learningGapDiagnosticTestResult.UpdatedAt = null;
                            response.Add(learningGapDiagnosticTestResult);
                            learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapDiagnosticTestResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapDiagnosticTestResultModel> Read(string connectionString, List<LearningGapDiagnosticTestResultModel> learningGapDiagnosticTestResultWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultModel> response = new List<LearningGapDiagnosticTestResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `student_id`, `quiz_id`, `subject_code`, `level_code`, `total_correct`, `total_questions`, `total_percent`, `total_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_test_results ";
                if (learningGapDiagnosticTestResultWhere != null && learningGapDiagnosticTestResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapDiagnosticTestResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticTestResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapDiagnosticTestResultWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, learningGapDiagnosticTestResultWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapDiagnosticTestResultWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, learningGapDiagnosticTestResultWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_correct` = @totalCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalCorrect" + i, learningGapDiagnosticTestResultWhere[i].TotalCorrect);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalQuestions))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_questions` = @totalQuestions" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalQuestions" + i, learningGapDiagnosticTestResultWhere[i].TotalQuestions);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalPercent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_percent` = @totalPercent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalPercent" + i, learningGapDiagnosticTestResultWhere[i].TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].TotalScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_score` = @totalScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalScore" + i, learningGapDiagnosticTestResultWhere[i].TotalScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticTestResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticTestResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticTestResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapDiagnosticTestResultWhere.Count - 1)
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
                            LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel(); if (!reader.IsDBNull(0))
                                learningGapDiagnosticTestResult.Id = reader.GetValue(0).ToString();
                            else
                                learningGapDiagnosticTestResult.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapDiagnosticTestResult.StudentId = reader.GetValue(1).ToString();
                            else
                                learningGapDiagnosticTestResult.StudentId = null;
                            if (!reader.IsDBNull(2))
                                learningGapDiagnosticTestResult.QuizId = reader.GetValue(2).ToString();
                            else
                                learningGapDiagnosticTestResult.QuizId = null;
                            if (!reader.IsDBNull(3))
                                learningGapDiagnosticTestResult.SubjectCode = reader.GetValue(3).ToString();
                            else
                                learningGapDiagnosticTestResult.SubjectCode = null;
                            if (!reader.IsDBNull(4))
                                learningGapDiagnosticTestResult.LevelCode = reader.GetValue(4).ToString();
                            else
                                learningGapDiagnosticTestResult.LevelCode = null;
                            if (!reader.IsDBNull(5))
                                learningGapDiagnosticTestResult.TotalCorrect = reader.GetValue(5).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalCorrect = null;
                            if (!reader.IsDBNull(6))
                                learningGapDiagnosticTestResult.TotalQuestions = reader.GetValue(6).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalQuestions = null;
                            if (!reader.IsDBNull(7))
                                learningGapDiagnosticTestResult.TotalPercent = reader.GetValue(7).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalPercent = null;
                            if (!reader.IsDBNull(8))
                                learningGapDiagnosticTestResult.TotalScore = reader.GetValue(8).ToString();
                            else
                                learningGapDiagnosticTestResult.TotalScore = null;
                            if (!reader.IsDBNull(9))
                                learningGapDiagnosticTestResult.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                learningGapDiagnosticTestResult.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                learningGapDiagnosticTestResult.CreatedAt = reader.GetValue(10).ToString();
                            else
                                learningGapDiagnosticTestResult.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                learningGapDiagnosticTestResult.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                learningGapDiagnosticTestResult.UpdatedAt = null;
                            response.Add(learningGapDiagnosticTestResult);
                            learningGapDiagnosticTestResult = new LearningGapDiagnosticTestResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapDiagnosticTestResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapDiagnosticTestResultModel> Create(string connectionString, LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultModel> response = new List<LearningGapDiagnosticTestResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO learning_gap_diagnostic_test_results (";
                string values = "(";
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.Id))
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
                    cmd.Parameters.AddWithValue("@id", learningGapDiagnosticTestResult.Id);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.StudentId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `student_id` ");
                        values = String.Concat(values, ", @studentId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `student_id` ");
                        values = String.Concat(values, " @studentId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@studentId", learningGapDiagnosticTestResult.StudentId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.QuizId))
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
                    cmd.Parameters.AddWithValue("@quizId", learningGapDiagnosticTestResult.QuizId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.SubjectCode))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `subject_code` ");
                        values = String.Concat(values, ", @subjectCode");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `subject_code` ");
                        values = String.Concat(values, " @subjectCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectCode", learningGapDiagnosticTestResult.SubjectCode);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.LevelCode))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `level_code` ");
                        values = String.Concat(values, ", @levelCode");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `level_code` ");
                        values = String.Concat(values, " @levelCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@levelCode", learningGapDiagnosticTestResult.LevelCode);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalCorrect))
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
                    cmd.Parameters.AddWithValue("@totalCorrect", learningGapDiagnosticTestResult.TotalCorrect);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalQuestions))
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
                    cmd.Parameters.AddWithValue("@totalQuestions", learningGapDiagnosticTestResult.TotalQuestions);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalPercent))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `total_percent` ");
                        values = String.Concat(values, ", @totalPercent");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `total_percent` ");
                        values = String.Concat(values, " @totalPercent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalPercent", learningGapDiagnosticTestResult.TotalPercent);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalScore))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `total_score` ");
                        values = String.Concat(values, ", @totalScore");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `total_score` ");
                        values = String.Concat(values, " @totalScore");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalScore", learningGapDiagnosticTestResult.TotalScore);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticTestResult.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticTestResult.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticTestResult.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                learningGapDiagnosticTestResult.Id = cmd.LastInsertedId.ToString();
                response.Add(learningGapDiagnosticTestResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultModel>();
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
        public List<LearningGapDiagnosticTestResultModel> Create(string connectionString, List<LearningGapDiagnosticTestResultModel> learningGapDiagnosticTestResultList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultModel> response = new List<LearningGapDiagnosticTestResultModel>();
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
                        for (int i = 0; i < learningGapDiagnosticTestResultList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO learning_gap_diagnostic_test_results (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticTestResultList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].StudentId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `student_id` ");
                                        values = String.Concat(values, ", @studentId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `student_id` ");
                                        values = String.Concat(values, "@studentId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@studentId" + i, learningGapDiagnosticTestResultList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].QuizId))
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
                                    cmd.Parameters.AddWithValue("@quizId" + i, learningGapDiagnosticTestResultList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].SubjectCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `subject_code` ");
                                        values = String.Concat(values, ", @subjectCode" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `subject_code` ");
                                        values = String.Concat(values, "@subjectCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapDiagnosticTestResultList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].LevelCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `level_code` ");
                                        values = String.Concat(values, ", @levelCode" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `level_code` ");
                                        values = String.Concat(values, "@levelCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@levelCode" + i, learningGapDiagnosticTestResultList[i].LevelCode);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].TotalCorrect))
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
                                    cmd.Parameters.AddWithValue("@totalCorrect" + i, learningGapDiagnosticTestResultList[i].TotalCorrect);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].TotalQuestions))
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
                                    cmd.Parameters.AddWithValue("@totalQuestions" + i, learningGapDiagnosticTestResultList[i].TotalQuestions);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].TotalPercent))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `total_percent` ");
                                        values = String.Concat(values, ", @totalPercent" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `total_percent` ");
                                        values = String.Concat(values, "@totalPercent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalPercent" + i, learningGapDiagnosticTestResultList[i].TotalPercent);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].TotalScore))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `total_score` ");
                                        values = String.Concat(values, ", @totalScore" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `total_score` ");
                                        values = String.Concat(values, "@totalScore" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalScore" + i, learningGapDiagnosticTestResultList[i].TotalScore);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticTestResultList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticTestResultList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticTestResultList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(learningGapDiagnosticTestResultList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapDiagnosticTestResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapDiagnosticTestResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultModel>();
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

        public List<LearningGapDiagnosticTestResultModel> Update(string connectionString, LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResult, LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultModel> response = new List<LearningGapDiagnosticTestResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE learning_gap_diagnostic_test_results SET ";
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", learningGapDiagnosticTestResult.Id);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.StudentId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `student_id` = @studentId");
                    else
                        sql = String.Concat(sql, " `student_id` = @studentId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@studentId", learningGapDiagnosticTestResult.StudentId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.QuizId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `quiz_id` = @quizId");
                    else
                        sql = String.Concat(sql, " `quiz_id` = @quizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", learningGapDiagnosticTestResult.QuizId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.SubjectCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `subject_code` = @subjectCode");
                    else
                        sql = String.Concat(sql, " `subject_code` = @subjectCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectCode", learningGapDiagnosticTestResult.SubjectCode);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.LevelCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `level_code` = @levelCode");
                    else
                        sql = String.Concat(sql, " `level_code` = @levelCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@levelCode", learningGapDiagnosticTestResult.LevelCode);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalCorrect))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `total_correct` = @totalCorrect");
                    else
                        sql = String.Concat(sql, " `total_correct` = @totalCorrect");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalCorrect", learningGapDiagnosticTestResult.TotalCorrect);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalQuestions))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `total_questions` = @totalQuestions");
                    else
                        sql = String.Concat(sql, " `total_questions` = @totalQuestions");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalQuestions", learningGapDiagnosticTestResult.TotalQuestions);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalPercent))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `total_percent` = @totalPercent");
                    else
                        sql = String.Concat(sql, " `total_percent` = @totalPercent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalPercent", learningGapDiagnosticTestResult.TotalPercent);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalScore))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `total_score` = @totalScore");
                    else
                        sql = String.Concat(sql, " `total_score` = @totalScore");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalScore", learningGapDiagnosticTestResult.TotalScore);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticTestResult.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticTestResult.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticTestResult.UpdatedAt);
                }
                if (learningGapDiagnosticTestResultWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", learningGapDiagnosticTestResultWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentIdWhere", learningGapDiagnosticTestResultWhere.StudentId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizIdWhere", learningGapDiagnosticTestResultWhere.QuizId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCodeWhere", learningGapDiagnosticTestResultWhere.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere.LevelCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `level_code` = @levelCodeWhere");
                        else
                            sql = String.Concat(sql, " `level_code` = @levelCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelCodeWhere", learningGapDiagnosticTestResultWhere.LevelCode);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere.TotalCorrect))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `total_correct` = @totalCorrectWhere");
                        else
                            sql = String.Concat(sql, " `total_correct` = @totalCorrectWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalCorrectWhere", learningGapDiagnosticTestResultWhere.TotalCorrect);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere.TotalQuestions))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `total_questions` = @totalQuestionsWhere");
                        else
                            sql = String.Concat(sql, " `total_questions` = @totalQuestionsWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalQuestionsWhere", learningGapDiagnosticTestResultWhere.TotalQuestions);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere.TotalPercent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `total_percent` = @totalPercentWhere");
                        else
                            sql = String.Concat(sql, " `total_percent` = @totalPercentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalPercentWhere", learningGapDiagnosticTestResultWhere.TotalPercent);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere.TotalScore))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `total_score` = @totalScoreWhere");
                        else
                            sql = String.Concat(sql, " `total_score` = @totalScoreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalScoreWhere", learningGapDiagnosticTestResultWhere.TotalScore);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", learningGapDiagnosticTestResultWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", learningGapDiagnosticTestResultWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", learningGapDiagnosticTestResultWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(learningGapDiagnosticTestResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultModel>();
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

        public List<LearningGapDiagnosticTestResultModel> Update(string connectionString, List<LearningGapDiagnosticTestResultModel> learningGapDiagnosticTestResultList, List<LearningGapDiagnosticTestResultModel> learningGapDiagnosticTestResultWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultModel> response = new List<LearningGapDiagnosticTestResultModel>();
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
                        for (int i = 0; i < learningGapDiagnosticTestResultList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE learning_gap_diagnostic_test_results SET ";
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticTestResultList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].StudentId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `student_id` = @studentId" + i);
                                    else
                                        sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@studentId" + i, learningGapDiagnosticTestResultList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].QuizId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `quiz_id` = @quizId" + i);
                                    else
                                        sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, learningGapDiagnosticTestResultList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].SubjectCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `subject_code` = @subjectCode" + i);
                                    else
                                        sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapDiagnosticTestResultList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].LevelCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `level_code` = @levelCode" + i);
                                    else
                                        sql = String.Concat(sql, " `level_code` = @levelCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@levelCode" + i, learningGapDiagnosticTestResultList[i].LevelCode);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].TotalCorrect))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `total_correct` = @totalCorrect" + i);
                                    else
                                        sql = String.Concat(sql, " `total_correct` = @totalCorrect" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalCorrect" + i, learningGapDiagnosticTestResultList[i].TotalCorrect);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].TotalQuestions))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `total_questions` = @totalQuestions" + i);
                                    else
                                        sql = String.Concat(sql, " `total_questions` = @totalQuestions" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalQuestions" + i, learningGapDiagnosticTestResultList[i].TotalQuestions);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].TotalPercent))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `total_percent` = @totalPercent" + i);
                                    else
                                        sql = String.Concat(sql, " `total_percent` = @totalPercent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalPercent" + i, learningGapDiagnosticTestResultList[i].TotalPercent);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].TotalScore))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `total_score` = @totalScore" + i);
                                    else
                                        sql = String.Concat(sql, " `total_score` = @totalScore" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalScore" + i, learningGapDiagnosticTestResultList[i].TotalScore);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticTestResultList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticTestResultList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticTestResultList[i].UpdatedAt);
                                }
                                if (learningGapDiagnosticTestResultWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, learningGapDiagnosticTestResultWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereList[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentIdWhere" + i, learningGapDiagnosticTestResultWhereList[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereList[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizIdWhere" + i, learningGapDiagnosticTestResultWhereList[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereList[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCodeWhere" + i, learningGapDiagnosticTestResultWhereList[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereList[i].LevelCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `level_code` = @levelCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `level_code` = @levelCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelCodeWhere" + i, learningGapDiagnosticTestResultWhereList[i].LevelCode);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereList[i].TotalCorrect))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `total_correct` = @totalCorrectWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `total_correct` = @totalCorrectWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalCorrectWhere" + i, learningGapDiagnosticTestResultWhereList[i].TotalCorrect);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereList[i].TotalQuestions))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `total_questions` = @totalQuestionsWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `total_questions` = @totalQuestionsWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalQuestionsWhere" + i, learningGapDiagnosticTestResultWhereList[i].TotalQuestions);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereList[i].TotalPercent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `total_percent` = @totalPercentWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `total_percent` = @totalPercentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalPercentWhere" + i, learningGapDiagnosticTestResultWhereList[i].TotalPercent);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereList[i].TotalScore))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `total_score` = @totalScoreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `total_score` = @totalScoreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalScoreWhere" + i, learningGapDiagnosticTestResultWhereList[i].TotalScore);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, learningGapDiagnosticTestResultWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, learningGapDiagnosticTestResultWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResultWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, learningGapDiagnosticTestResultWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapDiagnosticTestResultList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapDiagnosticTestResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapDiagnosticTestResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultModel>();
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

        public List<LearningGapDiagnosticTestResultModel> Delete(string connectionString, LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultModel> response = new List<LearningGapDiagnosticTestResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM learning_gap_diagnostic_test_results  ";
                if (learningGapDiagnosticTestResult != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", learningGapDiagnosticTestResult.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentId");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentId", learningGapDiagnosticTestResult.StudentId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizId");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizId", learningGapDiagnosticTestResult.QuizId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCode", learningGapDiagnosticTestResult.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.LevelCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `level_code` = @levelCode");
                        else
                            sql = String.Concat(sql, " `level_code` = @levelCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelCode", learningGapDiagnosticTestResult.LevelCode);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalCorrect))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `total_correct` = @totalCorrect");
                        else
                            sql = String.Concat(sql, " `total_correct` = @totalCorrect");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalCorrect", learningGapDiagnosticTestResult.TotalCorrect);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalQuestions))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `total_questions` = @totalQuestions");
                        else
                            sql = String.Concat(sql, " `total_questions` = @totalQuestions");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalQuestions", learningGapDiagnosticTestResult.TotalQuestions);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalPercent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `total_percent` = @totalPercent");
                        else
                            sql = String.Concat(sql, " `total_percent` = @totalPercent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalPercent", learningGapDiagnosticTestResult.TotalPercent);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.TotalScore))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `total_score` = @totalScore");
                        else
                            sql = String.Concat(sql, " `total_score` = @totalScore");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalScore", learningGapDiagnosticTestResult.TotalScore);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticTestResult.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticTestResult.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticTestResult.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(learningGapDiagnosticTestResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultModel>();
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

        public List<LearningGapDiagnosticTestResultModel> Delete(string connectionString, List<LearningGapDiagnosticTestResultModel> learningGapDiagnosticTestResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticTestResultModel> response = new List<LearningGapDiagnosticTestResultModel>();
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
                        for (int i = 0; i < learningGapDiagnosticTestResult.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM learning_gap_diagnostic_test_results  ";
                                if (learningGapDiagnosticTestResult[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticTestResult[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentId" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentId" + i, learningGapDiagnosticTestResult[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizId" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizId" + i, learningGapDiagnosticTestResult[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapDiagnosticTestResult[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult[i].LevelCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `level_code` = @levelCode" + i);
                                        else
                                            sql = String.Concat(sql, " `level_code` = @levelCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelCode" + i, learningGapDiagnosticTestResult[i].LevelCode);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult[i].TotalCorrect))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `total_correct` = @totalCorrect" + i);
                                        else
                                            sql = String.Concat(sql, " `total_correct` = @totalCorrect" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalCorrect" + i, learningGapDiagnosticTestResult[i].TotalCorrect);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult[i].TotalQuestions))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `total_questions` = @totalQuestions" + i);
                                        else
                                            sql = String.Concat(sql, " `total_questions` = @totalQuestions" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalQuestions" + i, learningGapDiagnosticTestResult[i].TotalQuestions);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult[i].TotalPercent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `total_percent` = @totalPercent" + i);
                                        else
                                            sql = String.Concat(sql, " `total_percent` = @totalPercent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalPercent" + i, learningGapDiagnosticTestResult[i].TotalPercent);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult[i].TotalScore))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `total_score` = @totalScore" + i);
                                        else
                                            sql = String.Concat(sql, " `total_score` = @totalScore" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalScore" + i, learningGapDiagnosticTestResult[i].TotalScore);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticTestResult[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticTestResult[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticTestResult[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticTestResult[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapDiagnosticTestResult[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapDiagnosticTestResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapDiagnosticTestResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticTestResultModel>();
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
