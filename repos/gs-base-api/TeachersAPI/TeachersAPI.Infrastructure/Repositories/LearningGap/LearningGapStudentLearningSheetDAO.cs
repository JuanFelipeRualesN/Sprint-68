using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.LearningGap;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.LearningGap
{
    class LearningGapStudentLearningSheetDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, LearningGapStudentLearningSheetModel> ReadD(string connectionString, LearningGapStudentLearningSheetModel learningGapStudentLearningSheet, bool firstOne, bool orderByLastDate, LearningGapStudentLearningSheetModel learningGapStudentLearningSheetWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapStudentLearningSheetModel> response = new Dictionary<string, LearningGapStudentLearningSheetModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `student_id`, `academic_year_id`, `learning_sheet_id`, `quiz_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_student_learning_sheets ";
                {
                    if (learningGapStudentLearningSheet != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapStudentLearningSheet.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapStudentLearningSheet.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.AcademicYearId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `academic_year_id` = @academicYearId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `academic_year_id` = @academicYearId ");
                            }
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapStudentLearningSheet.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.LearningSheetId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `learning_sheet_id` = @learningSheetId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `learning_sheet_id` = @learningSheetId ");
                            }
                            cmd.Parameters.AddWithValue("@learningSheetId", learningGapStudentLearningSheet.LearningSheetId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", learningGapStudentLearningSheet.QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", learningGapStudentLearningSheet.Available);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapStudentLearningSheet.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapStudentLearningSheet.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapStudentLearningSheet.UpdatedAt);
                        }
                    }
                    if (learningGapStudentLearningSheetWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapStudentLearningSheetWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.AcademicYearId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `academic_year_id` LIKE @academicYearIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `academic_year_id` LIKE @academicYearIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.LearningSheetId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `learning_sheet_id` LIKE @learningSheetIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `learning_sheet_id` LIKE @learningSheetIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@learningSheetIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.LearningSheetId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.Available))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `available` LIKE @availableWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `available` LIKE @availableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + learningGapStudentLearningSheetWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapStudentLearningSheetWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapStudentLearningSheetWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapStudentLearningSheetWhereLike.UpdatedAt + "%");
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
                                learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel();
                                if (!reader.IsDBNull(0))
                                    learningGapStudentLearningSheet.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapStudentLearningSheet.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapStudentLearningSheet.StudentId = reader.GetValue(1).ToString();
                                else
                                    learningGapStudentLearningSheet.StudentId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapStudentLearningSheet.AcademicYearId = reader.GetValue(2).ToString();
                                else
                                    learningGapStudentLearningSheet.AcademicYearId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapStudentLearningSheet.LearningSheetId = reader.GetValue(3).ToString();
                                else
                                    learningGapStudentLearningSheet.LearningSheetId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapStudentLearningSheet.QuizId = reader.GetValue(4).ToString();
                                else
                                    learningGapStudentLearningSheet.QuizId = null;
                                if (!reader.IsDBNull(5))
                                    learningGapStudentLearningSheet.Available = reader.GetValue(5).ToString();
                                else
                                    learningGapStudentLearningSheet.Available = null;
                                if (!reader.IsDBNull(6))
                                    learningGapStudentLearningSheet.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    learningGapStudentLearningSheet.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    learningGapStudentLearningSheet.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapStudentLearningSheet.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    learningGapStudentLearningSheet.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    learningGapStudentLearningSheet.UpdatedAt = null;
                                response.Add(learningGapStudentLearningSheet.Id, learningGapStudentLearningSheet);
                                learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapStudentLearningSheetModel>();
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
                response = new Dictionary<string, LearningGapStudentLearningSheetModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapStudentLearningSheetModel> ReadD(string connectionString, LearningGapStudentLearningSheetModel learningGapStudentLearningSheet, bool orderByLastDate, int fromRow, int limit, LearningGapStudentLearningSheetModel learningGapStudentLearningSheetWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapStudentLearningSheetModel> response = new Dictionary<string, LearningGapStudentLearningSheetModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `student_id`, `academic_year_id`, `learning_sheet_id`, `quiz_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_student_learning_sheets ";
                {
                    if (learningGapStudentLearningSheet != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapStudentLearningSheet.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapStudentLearningSheet.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.AcademicYearId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `academic_year_id` = @academicYearId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `academic_year_id` = @academicYearId ");
                            }
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapStudentLearningSheet.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.LearningSheetId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `learning_sheet_id` = @learningSheetId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `learning_sheet_id` = @learningSheetId ");
                            }
                            cmd.Parameters.AddWithValue("@learningSheetId", learningGapStudentLearningSheet.LearningSheetId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", learningGapStudentLearningSheet.QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", learningGapStudentLearningSheet.Available);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapStudentLearningSheet.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapStudentLearningSheet.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapStudentLearningSheet.UpdatedAt);
                        }
                    }
                    if (learningGapStudentLearningSheetWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapStudentLearningSheetWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.AcademicYearId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `academic_year_id` LIKE @academicYearIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `academic_year_id` LIKE @academicYearIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.LearningSheetId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `learning_sheet_id` LIKE @learningSheetIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `learning_sheet_id` LIKE @learningSheetIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@learningSheetIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.LearningSheetId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.Available))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `available` LIKE @availableWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `available` LIKE @availableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + learningGapStudentLearningSheetWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapStudentLearningSheetWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapStudentLearningSheetWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapStudentLearningSheetWhereLike.UpdatedAt + "%");
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
                                learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel();
                                if (!reader.IsDBNull(0))
                                    learningGapStudentLearningSheet.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapStudentLearningSheet.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapStudentLearningSheet.StudentId = reader.GetValue(1).ToString();
                                else
                                    learningGapStudentLearningSheet.StudentId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapStudentLearningSheet.AcademicYearId = reader.GetValue(2).ToString();
                                else
                                    learningGapStudentLearningSheet.AcademicYearId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapStudentLearningSheet.LearningSheetId = reader.GetValue(3).ToString();
                                else
                                    learningGapStudentLearningSheet.LearningSheetId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapStudentLearningSheet.QuizId = reader.GetValue(4).ToString();
                                else
                                    learningGapStudentLearningSheet.QuizId = null;
                                if (!reader.IsDBNull(5))
                                    learningGapStudentLearningSheet.Available = reader.GetValue(5).ToString();
                                else
                                    learningGapStudentLearningSheet.Available = null;
                                if (!reader.IsDBNull(6))
                                    learningGapStudentLearningSheet.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    learningGapStudentLearningSheet.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    learningGapStudentLearningSheet.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapStudentLearningSheet.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    learningGapStudentLearningSheet.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    learningGapStudentLearningSheet.UpdatedAt = null;
                                response.Add(learningGapStudentLearningSheet.Id, learningGapStudentLearningSheet);
                                learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapStudentLearningSheetModel>();
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
                response = new Dictionary<string, LearningGapStudentLearningSheetModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapStudentLearningSheetModel> ReadD(string connectionString, List<LearningGapStudentLearningSheetModel> learningGapStudentLearningSheetWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapStudentLearningSheetModel> response = new Dictionary<string, LearningGapStudentLearningSheetModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `student_id`, `academic_year_id`, `learning_sheet_id`, `quiz_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_student_learning_sheets ";
                if (learningGapStudentLearningSheetWhere != null && learningGapStudentLearningSheetWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapStudentLearningSheetWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapStudentLearningSheetWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapStudentLearningSheetWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapStudentLearningSheetWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].LearningSheetId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `learning_sheet_id` = @learningSheetId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@learningSheetId" + i, learningGapStudentLearningSheetWhere[i].LearningSheetId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, learningGapStudentLearningSheetWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, learningGapStudentLearningSheetWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapStudentLearningSheetWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapStudentLearningSheetWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapStudentLearningSheetWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapStudentLearningSheetWhere.Count - 1)
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
                            LearningGapStudentLearningSheetModel learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel(); if (!reader.IsDBNull(0))
                                learningGapStudentLearningSheet.Id = reader.GetValue(0).ToString();
                            else
                                learningGapStudentLearningSheet.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapStudentLearningSheet.StudentId = reader.GetValue(1).ToString();
                            else
                                learningGapStudentLearningSheet.StudentId = null;
                            if (!reader.IsDBNull(2))
                                learningGapStudentLearningSheet.AcademicYearId = reader.GetValue(2).ToString();
                            else
                                learningGapStudentLearningSheet.AcademicYearId = null;
                            if (!reader.IsDBNull(3))
                                learningGapStudentLearningSheet.LearningSheetId = reader.GetValue(3).ToString();
                            else
                                learningGapStudentLearningSheet.LearningSheetId = null;
                            if (!reader.IsDBNull(4))
                                learningGapStudentLearningSheet.QuizId = reader.GetValue(4).ToString();
                            else
                                learningGapStudentLearningSheet.QuizId = null;
                            if (!reader.IsDBNull(5))
                                learningGapStudentLearningSheet.Available = reader.GetValue(5).ToString();
                            else
                                learningGapStudentLearningSheet.Available = null;
                            if (!reader.IsDBNull(6))
                                learningGapStudentLearningSheet.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                learningGapStudentLearningSheet.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                learningGapStudentLearningSheet.CreatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapStudentLearningSheet.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                learningGapStudentLearningSheet.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                learningGapStudentLearningSheet.UpdatedAt = null;
                            response.Add(learningGapStudentLearningSheet.Id, learningGapStudentLearningSheet);
                            learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapStudentLearningSheetModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapStudentLearningSheetModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapStudentLearningSheetModel> ReadD(string connectionString, List<LearningGapStudentLearningSheetModel> learningGapStudentLearningSheetWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapStudentLearningSheetModel> response = new Dictionary<string, LearningGapStudentLearningSheetModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `student_id`, `academic_year_id`, `learning_sheet_id`, `quiz_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_student_learning_sheets ";
                if (learningGapStudentLearningSheetWhere != null && learningGapStudentLearningSheetWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapStudentLearningSheetWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapStudentLearningSheetWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapStudentLearningSheetWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapStudentLearningSheetWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].LearningSheetId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `learning_sheet_id` = @learningSheetId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@learningSheetId" + i, learningGapStudentLearningSheetWhere[i].LearningSheetId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, learningGapStudentLearningSheetWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, learningGapStudentLearningSheetWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapStudentLearningSheetWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapStudentLearningSheetWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapStudentLearningSheetWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapStudentLearningSheetWhere.Count - 1)
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
                            LearningGapStudentLearningSheetModel learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel(); if (!reader.IsDBNull(0))
                                learningGapStudentLearningSheet.Id = reader.GetValue(0).ToString();
                            else
                                learningGapStudentLearningSheet.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapStudentLearningSheet.StudentId = reader.GetValue(1).ToString();
                            else
                                learningGapStudentLearningSheet.StudentId = null;
                            if (!reader.IsDBNull(2))
                                learningGapStudentLearningSheet.AcademicYearId = reader.GetValue(2).ToString();
                            else
                                learningGapStudentLearningSheet.AcademicYearId = null;
                            if (!reader.IsDBNull(3))
                                learningGapStudentLearningSheet.LearningSheetId = reader.GetValue(3).ToString();
                            else
                                learningGapStudentLearningSheet.LearningSheetId = null;
                            if (!reader.IsDBNull(4))
                                learningGapStudentLearningSheet.QuizId = reader.GetValue(4).ToString();
                            else
                                learningGapStudentLearningSheet.QuizId = null;
                            if (!reader.IsDBNull(5))
                                learningGapStudentLearningSheet.Available = reader.GetValue(5).ToString();
                            else
                                learningGapStudentLearningSheet.Available = null;
                            if (!reader.IsDBNull(6))
                                learningGapStudentLearningSheet.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                learningGapStudentLearningSheet.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                learningGapStudentLearningSheet.CreatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapStudentLearningSheet.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                learningGapStudentLearningSheet.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                learningGapStudentLearningSheet.UpdatedAt = null;
                            response.Add(learningGapStudentLearningSheet.Id, learningGapStudentLearningSheet);
                            learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapStudentLearningSheetModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapStudentLearningSheetModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapStudentLearningSheetModel> Read(string connectionString, LearningGapStudentLearningSheetModel learningGapStudentLearningSheet, bool firstOne, bool orderByLastDate, LearningGapStudentLearningSheetModel learningGapStudentLearningSheetWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapStudentLearningSheetModel> response = new List<LearningGapStudentLearningSheetModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `student_id`, `academic_year_id`, `learning_sheet_id`, `quiz_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_student_learning_sheets ";
                {
                    if (learningGapStudentLearningSheet != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapStudentLearningSheet.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapStudentLearningSheet.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.AcademicYearId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `academic_year_id` = @academicYearId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `academic_year_id` = @academicYearId ");
                            }
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapStudentLearningSheet.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.LearningSheetId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `learning_sheet_id` = @learningSheetId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `learning_sheet_id` = @learningSheetId ");
                            }
                            cmd.Parameters.AddWithValue("@learningSheetId", learningGapStudentLearningSheet.LearningSheetId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", learningGapStudentLearningSheet.QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", learningGapStudentLearningSheet.Available);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapStudentLearningSheet.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapStudentLearningSheet.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapStudentLearningSheet.UpdatedAt);
                        }
                    }
                    if (learningGapStudentLearningSheetWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapStudentLearningSheetWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.AcademicYearId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `academic_year_id` LIKE @academicYearIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `academic_year_id` LIKE @academicYearIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.LearningSheetId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `learning_sheet_id` LIKE @learningSheetIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `learning_sheet_id` LIKE @learningSheetIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@learningSheetIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.LearningSheetId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.Available))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `available` LIKE @availableWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `available` LIKE @availableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + learningGapStudentLearningSheetWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapStudentLearningSheetWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapStudentLearningSheetWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapStudentLearningSheetWhereLike.UpdatedAt + "%");
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
                                learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel();
                                if (!reader.IsDBNull(0))
                                    learningGapStudentLearningSheet.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapStudentLearningSheet.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapStudentLearningSheet.StudentId = reader.GetValue(1).ToString();
                                else
                                    learningGapStudentLearningSheet.StudentId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapStudentLearningSheet.AcademicYearId = reader.GetValue(2).ToString();
                                else
                                    learningGapStudentLearningSheet.AcademicYearId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapStudentLearningSheet.LearningSheetId = reader.GetValue(3).ToString();
                                else
                                    learningGapStudentLearningSheet.LearningSheetId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapStudentLearningSheet.QuizId = reader.GetValue(4).ToString();
                                else
                                    learningGapStudentLearningSheet.QuizId = null;
                                if (!reader.IsDBNull(5))
                                    learningGapStudentLearningSheet.Available = reader.GetValue(5).ToString();
                                else
                                    learningGapStudentLearningSheet.Available = null;
                                if (!reader.IsDBNull(6))
                                    learningGapStudentLearningSheet.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    learningGapStudentLearningSheet.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    learningGapStudentLearningSheet.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapStudentLearningSheet.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    learningGapStudentLearningSheet.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    learningGapStudentLearningSheet.UpdatedAt = null;
                                response.Add(learningGapStudentLearningSheet);
                                learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapStudentLearningSheetModel>();
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
                response = new List<LearningGapStudentLearningSheetModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapStudentLearningSheetModel> Read(string connectionString, LearningGapStudentLearningSheetModel learningGapStudentLearningSheet, bool orderByLastDate, int fromRow, int limit, LearningGapStudentLearningSheetModel learningGapStudentLearningSheetWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapStudentLearningSheetModel> response = new List<LearningGapStudentLearningSheetModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `student_id`, `academic_year_id`, `learning_sheet_id`, `quiz_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_student_learning_sheets ";
                {
                    if (learningGapStudentLearningSheet != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapStudentLearningSheet.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapStudentLearningSheet.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.AcademicYearId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `academic_year_id` = @academicYearId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `academic_year_id` = @academicYearId ");
                            }
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapStudentLearningSheet.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.LearningSheetId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `learning_sheet_id` = @learningSheetId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `learning_sheet_id` = @learningSheetId ");
                            }
                            cmd.Parameters.AddWithValue("@learningSheetId", learningGapStudentLearningSheet.LearningSheetId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", learningGapStudentLearningSheet.QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", learningGapStudentLearningSheet.Available);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapStudentLearningSheet.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapStudentLearningSheet.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapStudentLearningSheet.UpdatedAt);
                        }
                    }
                    if (learningGapStudentLearningSheetWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapStudentLearningSheetWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.AcademicYearId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `academic_year_id` LIKE @academicYearIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `academic_year_id` LIKE @academicYearIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.LearningSheetId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `learning_sheet_id` LIKE @learningSheetIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `learning_sheet_id` LIKE @learningSheetIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@learningSheetIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.LearningSheetId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + learningGapStudentLearningSheetWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.Available))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `available` LIKE @availableWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `available` LIKE @availableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + learningGapStudentLearningSheetWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapStudentLearningSheetWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapStudentLearningSheetWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapStudentLearningSheetWhereLike.UpdatedAt + "%");
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
                                learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel();
                                if (!reader.IsDBNull(0))
                                    learningGapStudentLearningSheet.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapStudentLearningSheet.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapStudentLearningSheet.StudentId = reader.GetValue(1).ToString();
                                else
                                    learningGapStudentLearningSheet.StudentId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapStudentLearningSheet.AcademicYearId = reader.GetValue(2).ToString();
                                else
                                    learningGapStudentLearningSheet.AcademicYearId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapStudentLearningSheet.LearningSheetId = reader.GetValue(3).ToString();
                                else
                                    learningGapStudentLearningSheet.LearningSheetId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapStudentLearningSheet.QuizId = reader.GetValue(4).ToString();
                                else
                                    learningGapStudentLearningSheet.QuizId = null;
                                if (!reader.IsDBNull(5))
                                    learningGapStudentLearningSheet.Available = reader.GetValue(5).ToString();
                                else
                                    learningGapStudentLearningSheet.Available = null;
                                if (!reader.IsDBNull(6))
                                    learningGapStudentLearningSheet.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    learningGapStudentLearningSheet.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    learningGapStudentLearningSheet.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapStudentLearningSheet.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    learningGapStudentLearningSheet.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    learningGapStudentLearningSheet.UpdatedAt = null;
                                response.Add(learningGapStudentLearningSheet);
                                learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapStudentLearningSheetModel>();
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
                response = new List<LearningGapStudentLearningSheetModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapStudentLearningSheetModel> Read(string connectionString, List<LearningGapStudentLearningSheetModel> learningGapStudentLearningSheetWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapStudentLearningSheetModel> response = new List<LearningGapStudentLearningSheetModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `student_id`, `academic_year_id`, `learning_sheet_id`, `quiz_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_student_learning_sheets ";
                if (learningGapStudentLearningSheetWhere != null && learningGapStudentLearningSheetWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapStudentLearningSheetWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapStudentLearningSheetWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapStudentLearningSheetWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapStudentLearningSheetWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].LearningSheetId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `learning_sheet_id` = @learningSheetId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@learningSheetId" + i, learningGapStudentLearningSheetWhere[i].LearningSheetId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, learningGapStudentLearningSheetWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, learningGapStudentLearningSheetWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapStudentLearningSheetWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapStudentLearningSheetWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapStudentLearningSheetWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapStudentLearningSheetWhere.Count - 1)
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
                            LearningGapStudentLearningSheetModel learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel(); if (!reader.IsDBNull(0))
                                learningGapStudentLearningSheet.Id = reader.GetValue(0).ToString();
                            else
                                learningGapStudentLearningSheet.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapStudentLearningSheet.StudentId = reader.GetValue(1).ToString();
                            else
                                learningGapStudentLearningSheet.StudentId = null;
                            if (!reader.IsDBNull(2))
                                learningGapStudentLearningSheet.AcademicYearId = reader.GetValue(2).ToString();
                            else
                                learningGapStudentLearningSheet.AcademicYearId = null;
                            if (!reader.IsDBNull(3))
                                learningGapStudentLearningSheet.LearningSheetId = reader.GetValue(3).ToString();
                            else
                                learningGapStudentLearningSheet.LearningSheetId = null;
                            if (!reader.IsDBNull(4))
                                learningGapStudentLearningSheet.QuizId = reader.GetValue(4).ToString();
                            else
                                learningGapStudentLearningSheet.QuizId = null;
                            if (!reader.IsDBNull(5))
                                learningGapStudentLearningSheet.Available = reader.GetValue(5).ToString();
                            else
                                learningGapStudentLearningSheet.Available = null;
                            if (!reader.IsDBNull(6))
                                learningGapStudentLearningSheet.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                learningGapStudentLearningSheet.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                learningGapStudentLearningSheet.CreatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapStudentLearningSheet.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                learningGapStudentLearningSheet.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                learningGapStudentLearningSheet.UpdatedAt = null;
                            response.Add(learningGapStudentLearningSheet);
                            learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapStudentLearningSheetModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapStudentLearningSheetModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapStudentLearningSheetModel> Read(string connectionString, List<LearningGapStudentLearningSheetModel> learningGapStudentLearningSheetWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapStudentLearningSheetModel> response = new List<LearningGapStudentLearningSheetModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `student_id`, `academic_year_id`, `learning_sheet_id`, `quiz_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_student_learning_sheets ";
                if (learningGapStudentLearningSheetWhere != null && learningGapStudentLearningSheetWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapStudentLearningSheetWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapStudentLearningSheetWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapStudentLearningSheetWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapStudentLearningSheetWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].LearningSheetId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `learning_sheet_id` = @learningSheetId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@learningSheetId" + i, learningGapStudentLearningSheetWhere[i].LearningSheetId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, learningGapStudentLearningSheetWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, learningGapStudentLearningSheetWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapStudentLearningSheetWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapStudentLearningSheetWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapStudentLearningSheetWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapStudentLearningSheetWhere.Count - 1)
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
                            LearningGapStudentLearningSheetModel learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel(); if (!reader.IsDBNull(0))
                                learningGapStudentLearningSheet.Id = reader.GetValue(0).ToString();
                            else
                                learningGapStudentLearningSheet.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapStudentLearningSheet.StudentId = reader.GetValue(1).ToString();
                            else
                                learningGapStudentLearningSheet.StudentId = null;
                            if (!reader.IsDBNull(2))
                                learningGapStudentLearningSheet.AcademicYearId = reader.GetValue(2).ToString();
                            else
                                learningGapStudentLearningSheet.AcademicYearId = null;
                            if (!reader.IsDBNull(3))
                                learningGapStudentLearningSheet.LearningSheetId = reader.GetValue(3).ToString();
                            else
                                learningGapStudentLearningSheet.LearningSheetId = null;
                            if (!reader.IsDBNull(4))
                                learningGapStudentLearningSheet.QuizId = reader.GetValue(4).ToString();
                            else
                                learningGapStudentLearningSheet.QuizId = null;
                            if (!reader.IsDBNull(5))
                                learningGapStudentLearningSheet.Available = reader.GetValue(5).ToString();
                            else
                                learningGapStudentLearningSheet.Available = null;
                            if (!reader.IsDBNull(6))
                                learningGapStudentLearningSheet.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                learningGapStudentLearningSheet.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                learningGapStudentLearningSheet.CreatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapStudentLearningSheet.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                learningGapStudentLearningSheet.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                learningGapStudentLearningSheet.UpdatedAt = null;
                            response.Add(learningGapStudentLearningSheet);
                            learningGapStudentLearningSheet = new LearningGapStudentLearningSheetModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapStudentLearningSheetModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapStudentLearningSheetModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapStudentLearningSheetModel> Create(string connectionString, LearningGapStudentLearningSheetModel learningGapStudentLearningSheet)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapStudentLearningSheetModel> response = new List<LearningGapStudentLearningSheetModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO learning_gap_student_learning_sheets (";
                string values = "(";
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.Id))
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
                    cmd.Parameters.AddWithValue("@id", learningGapStudentLearningSheet.Id);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.StudentId))
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
                    cmd.Parameters.AddWithValue("@studentId", learningGapStudentLearningSheet.StudentId);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.AcademicYearId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `academic_year_id` ");
                        values = String.Concat(values, ", @academicYearId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `academic_year_id` ");
                        values = String.Concat(values, " @academicYearId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@academicYearId", learningGapStudentLearningSheet.AcademicYearId);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.LearningSheetId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `learning_sheet_id` ");
                        values = String.Concat(values, ", @learningSheetId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `learning_sheet_id` ");
                        values = String.Concat(values, " @learningSheetId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@learningSheetId", learningGapStudentLearningSheet.LearningSheetId);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.QuizId))
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
                    cmd.Parameters.AddWithValue("@quizId", learningGapStudentLearningSheet.QuizId);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.Available))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `available` ");
                        values = String.Concat(values, ", @available");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `available` ");
                        values = String.Concat(values, " @available");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", learningGapStudentLearningSheet.Available);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapStudentLearningSheet.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", learningGapStudentLearningSheet.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapStudentLearningSheet.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                learningGapStudentLearningSheet.Id = cmd.LastInsertedId.ToString();
                response.Add(learningGapStudentLearningSheet);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapStudentLearningSheetModel>();
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
        public List<LearningGapStudentLearningSheetModel> Create(string connectionString, List<LearningGapStudentLearningSheetModel> learningGapStudentLearningSheetList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapStudentLearningSheetModel> response = new List<LearningGapStudentLearningSheetModel>();
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
                        for (int i = 0; i < learningGapStudentLearningSheetList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO learning_gap_student_learning_sheets (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapStudentLearningSheetList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].StudentId))
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
                                    cmd.Parameters.AddWithValue("@studentId" + i, learningGapStudentLearningSheetList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].AcademicYearId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `academic_year_id` ");
                                        values = String.Concat(values, ", @academicYearId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `academic_year_id` ");
                                        values = String.Concat(values, "@academicYearId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapStudentLearningSheetList[i].AcademicYearId);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].LearningSheetId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `learning_sheet_id` ");
                                        values = String.Concat(values, ", @learningSheetId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `learning_sheet_id` ");
                                        values = String.Concat(values, "@learningSheetId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@learningSheetId" + i, learningGapStudentLearningSheetList[i].LearningSheetId);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].QuizId))
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
                                    cmd.Parameters.AddWithValue("@quizId" + i, learningGapStudentLearningSheetList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].Available))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `available` ");
                                        values = String.Concat(values, ", @available" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `available` ");
                                        values = String.Concat(values, "@available" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, learningGapStudentLearningSheetList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapStudentLearningSheetList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapStudentLearningSheetList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapStudentLearningSheetList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(learningGapStudentLearningSheetList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapStudentLearningSheetModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapStudentLearningSheetModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapStudentLearningSheetModel>();
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

        public List<LearningGapStudentLearningSheetModel> Update(string connectionString, LearningGapStudentLearningSheetModel learningGapStudentLearningSheet, LearningGapStudentLearningSheetModel learningGapStudentLearningSheetWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapStudentLearningSheetModel> response = new List<LearningGapStudentLearningSheetModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE learning_gap_student_learning_sheets SET ";
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", learningGapStudentLearningSheet.Id);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.StudentId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `student_id` = @studentId");
                    else
                        sql = String.Concat(sql, " `student_id` = @studentId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@studentId", learningGapStudentLearningSheet.StudentId);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.AcademicYearId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `academic_year_id` = @academicYearId");
                    else
                        sql = String.Concat(sql, " `academic_year_id` = @academicYearId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@academicYearId", learningGapStudentLearningSheet.AcademicYearId);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.LearningSheetId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `learning_sheet_id` = @learningSheetId");
                    else
                        sql = String.Concat(sql, " `learning_sheet_id` = @learningSheetId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@learningSheetId", learningGapStudentLearningSheet.LearningSheetId);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.QuizId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `quiz_id` = @quizId");
                    else
                        sql = String.Concat(sql, " `quiz_id` = @quizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", learningGapStudentLearningSheet.QuizId);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.Available))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `available` = @available");
                    else
                        sql = String.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", learningGapStudentLearningSheet.Available);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapStudentLearningSheet.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", learningGapStudentLearningSheet.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapStudentLearningSheet.UpdatedAt);
                }
                if (learningGapStudentLearningSheetWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", learningGapStudentLearningSheetWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentIdWhere", learningGapStudentLearningSheetWhere.StudentId);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere.AcademicYearId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearIdWhere");
                        else
                            sql = String.Concat(sql, " `academic_year_id` = @academicYearIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@academicYearIdWhere", learningGapStudentLearningSheetWhere.AcademicYearId);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere.LearningSheetId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `learning_sheet_id` = @learningSheetIdWhere");
                        else
                            sql = String.Concat(sql, " `learning_sheet_id` = @learningSheetIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@learningSheetIdWhere", learningGapStudentLearningSheetWhere.LearningSheetId);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizIdWhere", learningGapStudentLearningSheetWhere.QuizId);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = String.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", learningGapStudentLearningSheetWhere.Available);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", learningGapStudentLearningSheetWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", learningGapStudentLearningSheetWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", learningGapStudentLearningSheetWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(learningGapStudentLearningSheet);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapStudentLearningSheetModel>();
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

        public List<LearningGapStudentLearningSheetModel> Update(string connectionString, List<LearningGapStudentLearningSheetModel> learningGapStudentLearningSheetList, List<LearningGapStudentLearningSheetModel> learningGapStudentLearningSheetWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapStudentLearningSheetModel> response = new List<LearningGapStudentLearningSheetModel>();
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
                        for (int i = 0; i < learningGapStudentLearningSheetList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE learning_gap_student_learning_sheets SET ";
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapStudentLearningSheetList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].StudentId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `student_id` = @studentId" + i);
                                    else
                                        sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@studentId" + i, learningGapStudentLearningSheetList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].AcademicYearId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `academic_year_id` = @academicYearId" + i);
                                    else
                                        sql = String.Concat(sql, " `academic_year_id` = @academicYearId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapStudentLearningSheetList[i].AcademicYearId);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].LearningSheetId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `learning_sheet_id` = @learningSheetId" + i);
                                    else
                                        sql = String.Concat(sql, " `learning_sheet_id` = @learningSheetId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@learningSheetId" + i, learningGapStudentLearningSheetList[i].LearningSheetId);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].QuizId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `quiz_id` = @quizId" + i);
                                    else
                                        sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, learningGapStudentLearningSheetList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].Available))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = String.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, learningGapStudentLearningSheetList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapStudentLearningSheetList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapStudentLearningSheetList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapStudentLearningSheetList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapStudentLearningSheetList[i].UpdatedAt);
                                }
                                if (learningGapStudentLearningSheetWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, learningGapStudentLearningSheetWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereList[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentIdWhere" + i, learningGapStudentLearningSheetWhereList[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereList[i].AcademicYearId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `academic_year_id` = @academicYearIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@academicYearIdWhere" + i, learningGapStudentLearningSheetWhereList[i].AcademicYearId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereList[i].LearningSheetId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `learning_sheet_id` = @learningSheetIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `learning_sheet_id` = @learningSheetIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@learningSheetIdWhere" + i, learningGapStudentLearningSheetWhereList[i].LearningSheetId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereList[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizIdWhere" + i, learningGapStudentLearningSheetWhereList[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, learningGapStudentLearningSheetWhereList[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, learningGapStudentLearningSheetWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, learningGapStudentLearningSheetWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheetWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, learningGapStudentLearningSheetWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapStudentLearningSheetList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapStudentLearningSheetModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapStudentLearningSheetModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapStudentLearningSheetModel>();
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

        public List<LearningGapStudentLearningSheetModel> Delete(string connectionString, LearningGapStudentLearningSheetModel learningGapStudentLearningSheet)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapStudentLearningSheetModel> response = new List<LearningGapStudentLearningSheetModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM learning_gap_student_learning_sheets  ";
                if (learningGapStudentLearningSheet != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", learningGapStudentLearningSheet.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentId");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentId", learningGapStudentLearningSheet.StudentId);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.AcademicYearId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearId");
                        else
                            sql = String.Concat(sql, " `academic_year_id` = @academicYearId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@academicYearId", learningGapStudentLearningSheet.AcademicYearId);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.LearningSheetId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `learning_sheet_id` = @learningSheetId");
                        else
                            sql = String.Concat(sql, " `learning_sheet_id` = @learningSheetId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@learningSheetId", learningGapStudentLearningSheet.LearningSheetId);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizId");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizId", learningGapStudentLearningSheet.QuizId);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @available");
                        else
                            sql = String.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", learningGapStudentLearningSheet.Available);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", learningGapStudentLearningSheet.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", learningGapStudentLearningSheet.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", learningGapStudentLearningSheet.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(learningGapStudentLearningSheet);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapStudentLearningSheetModel>();
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

        public List<LearningGapStudentLearningSheetModel> Delete(string connectionString, List<LearningGapStudentLearningSheetModel> learningGapStudentLearningSheet)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapStudentLearningSheetModel> response = new List<LearningGapStudentLearningSheetModel>();
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
                        for (int i = 0; i < learningGapStudentLearningSheet.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM learning_gap_student_learning_sheets  ";
                                if (learningGapStudentLearningSheet[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, learningGapStudentLearningSheet[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentId" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentId" + i, learningGapStudentLearningSheet[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet[i].AcademicYearId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearId" + i);
                                        else
                                            sql = String.Concat(sql, " `academic_year_id` = @academicYearId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapStudentLearningSheet[i].AcademicYearId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet[i].LearningSheetId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `learning_sheet_id` = @learningSheetId" + i);
                                        else
                                            sql = String.Concat(sql, " `learning_sheet_id` = @learningSheetId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@learningSheetId" + i, learningGapStudentLearningSheet[i].LearningSheetId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizId" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizId" + i, learningGapStudentLearningSheet[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, learningGapStudentLearningSheet[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapStudentLearningSheet[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, learningGapStudentLearningSheet[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapStudentLearningSheet[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapStudentLearningSheet[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapStudentLearningSheet[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapStudentLearningSheetModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapStudentLearningSheetModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapStudentLearningSheetModel>();
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
