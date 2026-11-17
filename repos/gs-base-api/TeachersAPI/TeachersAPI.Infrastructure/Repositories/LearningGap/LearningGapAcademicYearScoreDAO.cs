using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.LearningGap;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.LearningGap
{
    class LearningGapAcademicYearScoreDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, LearningGapAcademicYearScoreModel> ReadD(string connectionString, LearningGapAcademicYearScoreModel learningGapAcademicYearScore, bool firstOne, bool orderByLastDate, LearningGapAcademicYearScoreModel learningGapAcademicYearScoreWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapAcademicYearScoreModel> response = new Dictionary<string, LearningGapAcademicYearScoreModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `initial_grade_score`, `final_grade_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_academic_year_scores ";
                {
                    if (learningGapAcademicYearScore != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapAcademicYearScore.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapAcademicYearScore.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapAcademicYearScore.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapAcademicYearScore.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.InitialGradeScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_grade_score` = @initialGradeScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_grade_score` = @initialGradeScore ");
                            }
                            cmd.Parameters.AddWithValue("@initialGradeScore", learningGapAcademicYearScore.InitialGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.FinalGradeScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_grade_score` = @finalGradeScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_grade_score` = @finalGradeScore ");
                            }
                            cmd.Parameters.AddWithValue("@finalGradeScore", learningGapAcademicYearScore.FinalGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapAcademicYearScore.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapAcademicYearScore.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapAcademicYearScore.UpdatedAt);
                        }
                    }
                    if (learningGapAcademicYearScoreWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapAcademicYearScoreWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapAcademicYearScoreWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapAcademicYearScoreWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapAcademicYearScoreWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.InitialGradeScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_grade_score` LIKE @initialGradeScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_grade_score` LIKE @initialGradeScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialGradeScoreWhereLike", "%" + learningGapAcademicYearScoreWhereLike.InitialGradeScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.FinalGradeScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_grade_score` LIKE @finalGradeScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_grade_score` LIKE @finalGradeScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalGradeScoreWhereLike", "%" + learningGapAcademicYearScoreWhereLike.FinalGradeScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapAcademicYearScoreWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapAcademicYearScoreWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapAcademicYearScoreWhereLike.UpdatedAt + "%");
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
                                learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel();
                                if (!reader.IsDBNull(0))
                                    learningGapAcademicYearScore.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapAcademicYearScore.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapAcademicYearScore.AcademicYearId = reader.GetValue(1).ToString();
                                else
                                    learningGapAcademicYearScore.AcademicYearId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapAcademicYearScore.StudentId = reader.GetValue(2).ToString();
                                else
                                    learningGapAcademicYearScore.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapAcademicYearScore.SubjectCode = reader.GetValue(3).ToString();
                                else
                                    learningGapAcademicYearScore.SubjectCode = null;
                                if (!reader.IsDBNull(4))
                                    learningGapAcademicYearScore.InitialGradeScore = reader.GetValue(4).ToString();
                                else
                                    learningGapAcademicYearScore.InitialGradeScore = null;
                                if (!reader.IsDBNull(5))
                                    learningGapAcademicYearScore.FinalGradeScore = reader.GetValue(5).ToString();
                                else
                                    learningGapAcademicYearScore.FinalGradeScore = null;
                                if (!reader.IsDBNull(6))
                                    learningGapAcademicYearScore.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    learningGapAcademicYearScore.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    learningGapAcademicYearScore.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapAcademicYearScore.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    learningGapAcademicYearScore.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    learningGapAcademicYearScore.UpdatedAt = null;
                                response.Add(learningGapAcademicYearScore.Id, learningGapAcademicYearScore);
                                learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapAcademicYearScoreModel>();
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
                response = new Dictionary<string, LearningGapAcademicYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapAcademicYearScoreModel> ReadD(string connectionString, LearningGapAcademicYearScoreModel learningGapAcademicYearScore, bool orderByLastDate, int fromRow, int limit, LearningGapAcademicYearScoreModel learningGapAcademicYearScoreWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapAcademicYearScoreModel> response = new Dictionary<string, LearningGapAcademicYearScoreModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `initial_grade_score`, `final_grade_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_academic_year_scores ";
                {
                    if (learningGapAcademicYearScore != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapAcademicYearScore.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapAcademicYearScore.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapAcademicYearScore.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapAcademicYearScore.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.InitialGradeScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_grade_score` = @initialGradeScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_grade_score` = @initialGradeScore ");
                            }
                            cmd.Parameters.AddWithValue("@initialGradeScore", learningGapAcademicYearScore.InitialGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.FinalGradeScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_grade_score` = @finalGradeScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_grade_score` = @finalGradeScore ");
                            }
                            cmd.Parameters.AddWithValue("@finalGradeScore", learningGapAcademicYearScore.FinalGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapAcademicYearScore.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapAcademicYearScore.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapAcademicYearScore.UpdatedAt);
                        }
                    }
                    if (learningGapAcademicYearScoreWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapAcademicYearScoreWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapAcademicYearScoreWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapAcademicYearScoreWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapAcademicYearScoreWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.InitialGradeScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_grade_score` LIKE @initialGradeScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_grade_score` LIKE @initialGradeScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialGradeScoreWhereLike", "%" + learningGapAcademicYearScoreWhereLike.InitialGradeScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.FinalGradeScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_grade_score` LIKE @finalGradeScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_grade_score` LIKE @finalGradeScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalGradeScoreWhereLike", "%" + learningGapAcademicYearScoreWhereLike.FinalGradeScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapAcademicYearScoreWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapAcademicYearScoreWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapAcademicYearScoreWhereLike.UpdatedAt + "%");
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
                                learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel();
                                if (!reader.IsDBNull(0))
                                    learningGapAcademicYearScore.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapAcademicYearScore.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapAcademicYearScore.AcademicYearId = reader.GetValue(1).ToString();
                                else
                                    learningGapAcademicYearScore.AcademicYearId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapAcademicYearScore.StudentId = reader.GetValue(2).ToString();
                                else
                                    learningGapAcademicYearScore.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapAcademicYearScore.SubjectCode = reader.GetValue(3).ToString();
                                else
                                    learningGapAcademicYearScore.SubjectCode = null;
                                if (!reader.IsDBNull(4))
                                    learningGapAcademicYearScore.InitialGradeScore = reader.GetValue(4).ToString();
                                else
                                    learningGapAcademicYearScore.InitialGradeScore = null;
                                if (!reader.IsDBNull(5))
                                    learningGapAcademicYearScore.FinalGradeScore = reader.GetValue(5).ToString();
                                else
                                    learningGapAcademicYearScore.FinalGradeScore = null;
                                if (!reader.IsDBNull(6))
                                    learningGapAcademicYearScore.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    learningGapAcademicYearScore.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    learningGapAcademicYearScore.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapAcademicYearScore.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    learningGapAcademicYearScore.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    learningGapAcademicYearScore.UpdatedAt = null;
                                response.Add(learningGapAcademicYearScore.Id, learningGapAcademicYearScore);
                                learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapAcademicYearScoreModel>();
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
                response = new Dictionary<string, LearningGapAcademicYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapAcademicYearScoreModel> ReadD(string connectionString, List<LearningGapAcademicYearScoreModel> learningGapAcademicYearScoreWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapAcademicYearScoreModel> response = new Dictionary<string, LearningGapAcademicYearScoreModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `initial_grade_score`, `final_grade_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_academic_year_scores ";
                if (learningGapAcademicYearScoreWhere != null && learningGapAcademicYearScoreWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapAcademicYearScoreWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapAcademicYearScoreWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAcademicYearScoreWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapAcademicYearScoreWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapAcademicYearScoreWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].InitialGradeScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_grade_score` = @initialGradeScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialGradeScore" + i, learningGapAcademicYearScoreWhere[i].InitialGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].FinalGradeScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_grade_score` = @finalGradeScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalGradeScore" + i, learningGapAcademicYearScoreWhere[i].FinalGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAcademicYearScoreWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAcademicYearScoreWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAcademicYearScoreWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapAcademicYearScoreWhere.Count - 1)
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
                            LearningGapAcademicYearScoreModel learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel(); if (!reader.IsDBNull(0))
                                learningGapAcademicYearScore.Id = reader.GetValue(0).ToString();
                            else
                                learningGapAcademicYearScore.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapAcademicYearScore.AcademicYearId = reader.GetValue(1).ToString();
                            else
                                learningGapAcademicYearScore.AcademicYearId = null;
                            if (!reader.IsDBNull(2))
                                learningGapAcademicYearScore.StudentId = reader.GetValue(2).ToString();
                            else
                                learningGapAcademicYearScore.StudentId = null;
                            if (!reader.IsDBNull(3))
                                learningGapAcademicYearScore.SubjectCode = reader.GetValue(3).ToString();
                            else
                                learningGapAcademicYearScore.SubjectCode = null;
                            if (!reader.IsDBNull(4))
                                learningGapAcademicYearScore.InitialGradeScore = reader.GetValue(4).ToString();
                            else
                                learningGapAcademicYearScore.InitialGradeScore = null;
                            if (!reader.IsDBNull(5))
                                learningGapAcademicYearScore.FinalGradeScore = reader.GetValue(5).ToString();
                            else
                                learningGapAcademicYearScore.FinalGradeScore = null;
                            if (!reader.IsDBNull(6))
                                learningGapAcademicYearScore.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                learningGapAcademicYearScore.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                learningGapAcademicYearScore.CreatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapAcademicYearScore.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                learningGapAcademicYearScore.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                learningGapAcademicYearScore.UpdatedAt = null;
                            response.Add(learningGapAcademicYearScore.Id, learningGapAcademicYearScore);
                            learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapAcademicYearScoreModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapAcademicYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapAcademicYearScoreModel> ReadD(string connectionString, List<LearningGapAcademicYearScoreModel> learningGapAcademicYearScoreWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapAcademicYearScoreModel> response = new Dictionary<string, LearningGapAcademicYearScoreModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `initial_grade_score`, `final_grade_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_academic_year_scores ";
                if (learningGapAcademicYearScoreWhere != null && learningGapAcademicYearScoreWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapAcademicYearScoreWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapAcademicYearScoreWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAcademicYearScoreWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapAcademicYearScoreWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapAcademicYearScoreWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].InitialGradeScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_grade_score` = @initialGradeScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialGradeScore" + i, learningGapAcademicYearScoreWhere[i].InitialGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].FinalGradeScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_grade_score` = @finalGradeScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalGradeScore" + i, learningGapAcademicYearScoreWhere[i].FinalGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAcademicYearScoreWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAcademicYearScoreWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAcademicYearScoreWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapAcademicYearScoreWhere.Count - 1)
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
                            LearningGapAcademicYearScoreModel learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel(); if (!reader.IsDBNull(0))
                                learningGapAcademicYearScore.Id = reader.GetValue(0).ToString();
                            else
                                learningGapAcademicYearScore.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapAcademicYearScore.AcademicYearId = reader.GetValue(1).ToString();
                            else
                                learningGapAcademicYearScore.AcademicYearId = null;
                            if (!reader.IsDBNull(2))
                                learningGapAcademicYearScore.StudentId = reader.GetValue(2).ToString();
                            else
                                learningGapAcademicYearScore.StudentId = null;
                            if (!reader.IsDBNull(3))
                                learningGapAcademicYearScore.SubjectCode = reader.GetValue(3).ToString();
                            else
                                learningGapAcademicYearScore.SubjectCode = null;
                            if (!reader.IsDBNull(4))
                                learningGapAcademicYearScore.InitialGradeScore = reader.GetValue(4).ToString();
                            else
                                learningGapAcademicYearScore.InitialGradeScore = null;
                            if (!reader.IsDBNull(5))
                                learningGapAcademicYearScore.FinalGradeScore = reader.GetValue(5).ToString();
                            else
                                learningGapAcademicYearScore.FinalGradeScore = null;
                            if (!reader.IsDBNull(6))
                                learningGapAcademicYearScore.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                learningGapAcademicYearScore.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                learningGapAcademicYearScore.CreatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapAcademicYearScore.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                learningGapAcademicYearScore.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                learningGapAcademicYearScore.UpdatedAt = null;
                            response.Add(learningGapAcademicYearScore.Id, learningGapAcademicYearScore);
                            learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapAcademicYearScoreModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapAcademicYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapAcademicYearScoreModel> Read(string connectionString, LearningGapAcademicYearScoreModel learningGapAcademicYearScore, bool firstOne, bool orderByLastDate, LearningGapAcademicYearScoreModel learningGapAcademicYearScoreWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAcademicYearScoreModel> response = new List<LearningGapAcademicYearScoreModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `initial_grade_score`, `final_grade_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_academic_year_scores ";
                {
                    if (learningGapAcademicYearScore != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapAcademicYearScore.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapAcademicYearScore.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapAcademicYearScore.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapAcademicYearScore.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.InitialGradeScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_grade_score` = @initialGradeScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_grade_score` = @initialGradeScore ");
                            }
                            cmd.Parameters.AddWithValue("@initialGradeScore", learningGapAcademicYearScore.InitialGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.FinalGradeScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_grade_score` = @finalGradeScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_grade_score` = @finalGradeScore ");
                            }
                            cmd.Parameters.AddWithValue("@finalGradeScore", learningGapAcademicYearScore.FinalGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapAcademicYearScore.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapAcademicYearScore.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapAcademicYearScore.UpdatedAt);
                        }
                    }
                    if (learningGapAcademicYearScoreWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapAcademicYearScoreWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapAcademicYearScoreWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapAcademicYearScoreWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapAcademicYearScoreWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.InitialGradeScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_grade_score` LIKE @initialGradeScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_grade_score` LIKE @initialGradeScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialGradeScoreWhereLike", "%" + learningGapAcademicYearScoreWhereLike.InitialGradeScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.FinalGradeScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_grade_score` LIKE @finalGradeScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_grade_score` LIKE @finalGradeScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalGradeScoreWhereLike", "%" + learningGapAcademicYearScoreWhereLike.FinalGradeScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapAcademicYearScoreWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapAcademicYearScoreWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapAcademicYearScoreWhereLike.UpdatedAt + "%");
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
                                learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel();
                                if (!reader.IsDBNull(0))
                                    learningGapAcademicYearScore.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapAcademicYearScore.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapAcademicYearScore.AcademicYearId = reader.GetValue(1).ToString();
                                else
                                    learningGapAcademicYearScore.AcademicYearId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapAcademicYearScore.StudentId = reader.GetValue(2).ToString();
                                else
                                    learningGapAcademicYearScore.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapAcademicYearScore.SubjectCode = reader.GetValue(3).ToString();
                                else
                                    learningGapAcademicYearScore.SubjectCode = null;
                                if (!reader.IsDBNull(4))
                                    learningGapAcademicYearScore.InitialGradeScore = reader.GetValue(4).ToString();
                                else
                                    learningGapAcademicYearScore.InitialGradeScore = null;
                                if (!reader.IsDBNull(5))
                                    learningGapAcademicYearScore.FinalGradeScore = reader.GetValue(5).ToString();
                                else
                                    learningGapAcademicYearScore.FinalGradeScore = null;
                                if (!reader.IsDBNull(6))
                                    learningGapAcademicYearScore.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    learningGapAcademicYearScore.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    learningGapAcademicYearScore.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapAcademicYearScore.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    learningGapAcademicYearScore.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    learningGapAcademicYearScore.UpdatedAt = null;
                                response.Add(learningGapAcademicYearScore);
                                learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapAcademicYearScoreModel>();
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
                response = new List<LearningGapAcademicYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapAcademicYearScoreModel> Read(string connectionString, LearningGapAcademicYearScoreModel learningGapAcademicYearScore, bool orderByLastDate, int fromRow, int limit, LearningGapAcademicYearScoreModel learningGapAcademicYearScoreWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAcademicYearScoreModel> response = new List<LearningGapAcademicYearScoreModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `initial_grade_score`, `final_grade_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_academic_year_scores ";
                {
                    if (learningGapAcademicYearScore != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapAcademicYearScore.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapAcademicYearScore.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapAcademicYearScore.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapAcademicYearScore.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.InitialGradeScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_grade_score` = @initialGradeScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_grade_score` = @initialGradeScore ");
                            }
                            cmd.Parameters.AddWithValue("@initialGradeScore", learningGapAcademicYearScore.InitialGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.FinalGradeScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_grade_score` = @finalGradeScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_grade_score` = @finalGradeScore ");
                            }
                            cmd.Parameters.AddWithValue("@finalGradeScore", learningGapAcademicYearScore.FinalGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapAcademicYearScore.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapAcademicYearScore.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScore.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapAcademicYearScore.UpdatedAt);
                        }
                    }
                    if (learningGapAcademicYearScoreWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapAcademicYearScoreWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapAcademicYearScoreWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapAcademicYearScoreWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapAcademicYearScoreWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.InitialGradeScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_grade_score` LIKE @initialGradeScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_grade_score` LIKE @initialGradeScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialGradeScoreWhereLike", "%" + learningGapAcademicYearScoreWhereLike.InitialGradeScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.FinalGradeScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_grade_score` LIKE @finalGradeScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_grade_score` LIKE @finalGradeScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalGradeScoreWhereLike", "%" + learningGapAcademicYearScoreWhereLike.FinalGradeScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapAcademicYearScoreWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapAcademicYearScoreWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapAcademicYearScoreWhereLike.UpdatedAt + "%");
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
                                learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel();
                                if (!reader.IsDBNull(0))
                                    learningGapAcademicYearScore.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapAcademicYearScore.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapAcademicYearScore.AcademicYearId = reader.GetValue(1).ToString();
                                else
                                    learningGapAcademicYearScore.AcademicYearId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapAcademicYearScore.StudentId = reader.GetValue(2).ToString();
                                else
                                    learningGapAcademicYearScore.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapAcademicYearScore.SubjectCode = reader.GetValue(3).ToString();
                                else
                                    learningGapAcademicYearScore.SubjectCode = null;
                                if (!reader.IsDBNull(4))
                                    learningGapAcademicYearScore.InitialGradeScore = reader.GetValue(4).ToString();
                                else
                                    learningGapAcademicYearScore.InitialGradeScore = null;
                                if (!reader.IsDBNull(5))
                                    learningGapAcademicYearScore.FinalGradeScore = reader.GetValue(5).ToString();
                                else
                                    learningGapAcademicYearScore.FinalGradeScore = null;
                                if (!reader.IsDBNull(6))
                                    learningGapAcademicYearScore.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    learningGapAcademicYearScore.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    learningGapAcademicYearScore.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapAcademicYearScore.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    learningGapAcademicYearScore.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    learningGapAcademicYearScore.UpdatedAt = null;
                                response.Add(learningGapAcademicYearScore);
                                learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapAcademicYearScoreModel>();
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
                response = new List<LearningGapAcademicYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapAcademicYearScoreModel> Read(string connectionString, List<LearningGapAcademicYearScoreModel> learningGapAcademicYearScoreWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAcademicYearScoreModel> response = new List<LearningGapAcademicYearScoreModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `initial_grade_score`, `final_grade_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_academic_year_scores ";
                if (learningGapAcademicYearScoreWhere != null && learningGapAcademicYearScoreWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapAcademicYearScoreWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapAcademicYearScoreWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAcademicYearScoreWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapAcademicYearScoreWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapAcademicYearScoreWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].InitialGradeScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_grade_score` = @initialGradeScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialGradeScore" + i, learningGapAcademicYearScoreWhere[i].InitialGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].FinalGradeScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_grade_score` = @finalGradeScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalGradeScore" + i, learningGapAcademicYearScoreWhere[i].FinalGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAcademicYearScoreWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAcademicYearScoreWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAcademicYearScoreWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapAcademicYearScoreWhere.Count - 1)
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
                            LearningGapAcademicYearScoreModel learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel(); if (!reader.IsDBNull(0))
                                learningGapAcademicYearScore.Id = reader.GetValue(0).ToString();
                            else
                                learningGapAcademicYearScore.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapAcademicYearScore.AcademicYearId = reader.GetValue(1).ToString();
                            else
                                learningGapAcademicYearScore.AcademicYearId = null;
                            if (!reader.IsDBNull(2))
                                learningGapAcademicYearScore.StudentId = reader.GetValue(2).ToString();
                            else
                                learningGapAcademicYearScore.StudentId = null;
                            if (!reader.IsDBNull(3))
                                learningGapAcademicYearScore.SubjectCode = reader.GetValue(3).ToString();
                            else
                                learningGapAcademicYearScore.SubjectCode = null;
                            if (!reader.IsDBNull(4))
                                learningGapAcademicYearScore.InitialGradeScore = reader.GetValue(4).ToString();
                            else
                                learningGapAcademicYearScore.InitialGradeScore = null;
                            if (!reader.IsDBNull(5))
                                learningGapAcademicYearScore.FinalGradeScore = reader.GetValue(5).ToString();
                            else
                                learningGapAcademicYearScore.FinalGradeScore = null;
                            if (!reader.IsDBNull(6))
                                learningGapAcademicYearScore.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                learningGapAcademicYearScore.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                learningGapAcademicYearScore.CreatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapAcademicYearScore.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                learningGapAcademicYearScore.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                learningGapAcademicYearScore.UpdatedAt = null;
                            response.Add(learningGapAcademicYearScore);
                            learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapAcademicYearScoreModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAcademicYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapAcademicYearScoreModel> Read(string connectionString, List<LearningGapAcademicYearScoreModel> learningGapAcademicYearScoreWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAcademicYearScoreModel> response = new List<LearningGapAcademicYearScoreModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `initial_grade_score`, `final_grade_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_academic_year_scores ";
                if (learningGapAcademicYearScoreWhere != null && learningGapAcademicYearScoreWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapAcademicYearScoreWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapAcademicYearScoreWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAcademicYearScoreWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapAcademicYearScoreWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapAcademicYearScoreWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].InitialGradeScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_grade_score` = @initialGradeScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialGradeScore" + i, learningGapAcademicYearScoreWhere[i].InitialGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].FinalGradeScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_grade_score` = @finalGradeScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalGradeScore" + i, learningGapAcademicYearScoreWhere[i].FinalGradeScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAcademicYearScoreWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAcademicYearScoreWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAcademicYearScoreWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapAcademicYearScoreWhere.Count - 1)
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
                            LearningGapAcademicYearScoreModel learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel(); if (!reader.IsDBNull(0))
                                learningGapAcademicYearScore.Id = reader.GetValue(0).ToString();
                            else
                                learningGapAcademicYearScore.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapAcademicYearScore.AcademicYearId = reader.GetValue(1).ToString();
                            else
                                learningGapAcademicYearScore.AcademicYearId = null;
                            if (!reader.IsDBNull(2))
                                learningGapAcademicYearScore.StudentId = reader.GetValue(2).ToString();
                            else
                                learningGapAcademicYearScore.StudentId = null;
                            if (!reader.IsDBNull(3))
                                learningGapAcademicYearScore.SubjectCode = reader.GetValue(3).ToString();
                            else
                                learningGapAcademicYearScore.SubjectCode = null;
                            if (!reader.IsDBNull(4))
                                learningGapAcademicYearScore.InitialGradeScore = reader.GetValue(4).ToString();
                            else
                                learningGapAcademicYearScore.InitialGradeScore = null;
                            if (!reader.IsDBNull(5))
                                learningGapAcademicYearScore.FinalGradeScore = reader.GetValue(5).ToString();
                            else
                                learningGapAcademicYearScore.FinalGradeScore = null;
                            if (!reader.IsDBNull(6))
                                learningGapAcademicYearScore.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                learningGapAcademicYearScore.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                learningGapAcademicYearScore.CreatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapAcademicYearScore.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                learningGapAcademicYearScore.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                learningGapAcademicYearScore.UpdatedAt = null;
                            response.Add(learningGapAcademicYearScore);
                            learningGapAcademicYearScore = new LearningGapAcademicYearScoreModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapAcademicYearScoreModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAcademicYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapAcademicYearScoreModel> Create(string connectionString, LearningGapAcademicYearScoreModel learningGapAcademicYearScore)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAcademicYearScoreModel> response = new List<LearningGapAcademicYearScoreModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO learning_gap_academic_year_scores (";
                string values = "(";
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.Id))
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
                    cmd.Parameters.AddWithValue("@id", learningGapAcademicYearScore.Id);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.AcademicYearId))
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
                    cmd.Parameters.AddWithValue("@academicYearId", learningGapAcademicYearScore.AcademicYearId);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.StudentId))
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
                    cmd.Parameters.AddWithValue("@studentId", learningGapAcademicYearScore.StudentId);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.SubjectCode))
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
                    cmd.Parameters.AddWithValue("@subjectCode", learningGapAcademicYearScore.SubjectCode);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.InitialGradeScore))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `initial_grade_score` ");
                        values = String.Concat(values, ", @initialGradeScore");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `initial_grade_score` ");
                        values = String.Concat(values, " @initialGradeScore");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@initialGradeScore", learningGapAcademicYearScore.InitialGradeScore);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.FinalGradeScore))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `final_grade_score` ");
                        values = String.Concat(values, ", @finalGradeScore");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `final_grade_score` ");
                        values = String.Concat(values, " @finalGradeScore");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@finalGradeScore", learningGapAcademicYearScore.FinalGradeScore);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapAcademicYearScore.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", learningGapAcademicYearScore.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapAcademicYearScore.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                learningGapAcademicYearScore.Id = cmd.LastInsertedId.ToString();
                response.Add(learningGapAcademicYearScore);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAcademicYearScoreModel>();
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
        public List<LearningGapAcademicYearScoreModel> Create(string connectionString, List<LearningGapAcademicYearScoreModel> learningGapAcademicYearScoreList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAcademicYearScoreModel> response = new List<LearningGapAcademicYearScoreModel>();
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
                        for (int i = 0; i < learningGapAcademicYearScoreList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO learning_gap_academic_year_scores (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapAcademicYearScoreList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].AcademicYearId))
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
                                    cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAcademicYearScoreList[i].AcademicYearId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].StudentId))
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
                                    cmd.Parameters.AddWithValue("@studentId" + i, learningGapAcademicYearScoreList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].SubjectCode))
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
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapAcademicYearScoreList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].InitialGradeScore))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `initial_grade_score` ");
                                        values = String.Concat(values, ", @initialGradeScore" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `initial_grade_score` ");
                                        values = String.Concat(values, "@initialGradeScore" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@initialGradeScore" + i, learningGapAcademicYearScoreList[i].InitialGradeScore);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].FinalGradeScore))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `final_grade_score` ");
                                        values = String.Concat(values, ", @finalGradeScore" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `final_grade_score` ");
                                        values = String.Concat(values, "@finalGradeScore" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@finalGradeScore" + i, learningGapAcademicYearScoreList[i].FinalGradeScore);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAcademicYearScoreList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAcademicYearScoreList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAcademicYearScoreList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(learningGapAcademicYearScoreList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapAcademicYearScoreModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapAcademicYearScoreModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAcademicYearScoreModel>();
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

        public List<LearningGapAcademicYearScoreModel> Update(string connectionString, LearningGapAcademicYearScoreModel learningGapAcademicYearScore, LearningGapAcademicYearScoreModel learningGapAcademicYearScoreWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAcademicYearScoreModel> response = new List<LearningGapAcademicYearScoreModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE learning_gap_academic_year_scores SET ";
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", learningGapAcademicYearScore.Id);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.AcademicYearId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `academic_year_id` = @academicYearId");
                    else
                        sql = String.Concat(sql, " `academic_year_id` = @academicYearId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@academicYearId", learningGapAcademicYearScore.AcademicYearId);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.StudentId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `student_id` = @studentId");
                    else
                        sql = String.Concat(sql, " `student_id` = @studentId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@studentId", learningGapAcademicYearScore.StudentId);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.SubjectCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `subject_code` = @subjectCode");
                    else
                        sql = String.Concat(sql, " `subject_code` = @subjectCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectCode", learningGapAcademicYearScore.SubjectCode);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.InitialGradeScore))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `initial_grade_score` = @initialGradeScore");
                    else
                        sql = String.Concat(sql, " `initial_grade_score` = @initialGradeScore");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@initialGradeScore", learningGapAcademicYearScore.InitialGradeScore);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.FinalGradeScore))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `final_grade_score` = @finalGradeScore");
                    else
                        sql = String.Concat(sql, " `final_grade_score` = @finalGradeScore");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@finalGradeScore", learningGapAcademicYearScore.FinalGradeScore);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapAcademicYearScore.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", learningGapAcademicYearScore.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapAcademicYearScore.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapAcademicYearScore.UpdatedAt);
                }
                if (learningGapAcademicYearScoreWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", learningGapAcademicYearScoreWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere.AcademicYearId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearIdWhere");
                        else
                            sql = String.Concat(sql, " `academic_year_id` = @academicYearIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@academicYearIdWhere", learningGapAcademicYearScoreWhere.AcademicYearId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentIdWhere", learningGapAcademicYearScoreWhere.StudentId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCodeWhere", learningGapAcademicYearScoreWhere.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere.InitialGradeScore))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `initial_grade_score` = @initialGradeScoreWhere");
                        else
                            sql = String.Concat(sql, " `initial_grade_score` = @initialGradeScoreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@initialGradeScoreWhere", learningGapAcademicYearScoreWhere.InitialGradeScore);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere.FinalGradeScore))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `final_grade_score` = @finalGradeScoreWhere");
                        else
                            sql = String.Concat(sql, " `final_grade_score` = @finalGradeScoreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@finalGradeScoreWhere", learningGapAcademicYearScoreWhere.FinalGradeScore);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", learningGapAcademicYearScoreWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", learningGapAcademicYearScoreWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", learningGapAcademicYearScoreWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(learningGapAcademicYearScore);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAcademicYearScoreModel>();
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

        public List<LearningGapAcademicYearScoreModel> Update(string connectionString, List<LearningGapAcademicYearScoreModel> learningGapAcademicYearScoreList, List<LearningGapAcademicYearScoreModel> learningGapAcademicYearScoreWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAcademicYearScoreModel> response = new List<LearningGapAcademicYearScoreModel>();
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
                        for (int i = 0; i < learningGapAcademicYearScoreList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE learning_gap_academic_year_scores SET ";
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapAcademicYearScoreList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].AcademicYearId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `academic_year_id` = @academicYearId" + i);
                                    else
                                        sql = String.Concat(sql, " `academic_year_id` = @academicYearId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAcademicYearScoreList[i].AcademicYearId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].StudentId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `student_id` = @studentId" + i);
                                    else
                                        sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@studentId" + i, learningGapAcademicYearScoreList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].SubjectCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `subject_code` = @subjectCode" + i);
                                    else
                                        sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapAcademicYearScoreList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].InitialGradeScore))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `initial_grade_score` = @initialGradeScore" + i);
                                    else
                                        sql = String.Concat(sql, " `initial_grade_score` = @initialGradeScore" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@initialGradeScore" + i, learningGapAcademicYearScoreList[i].InitialGradeScore);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].FinalGradeScore))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `final_grade_score` = @finalGradeScore" + i);
                                    else
                                        sql = String.Concat(sql, " `final_grade_score` = @finalGradeScore" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@finalGradeScore" + i, learningGapAcademicYearScoreList[i].FinalGradeScore);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAcademicYearScoreList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAcademicYearScoreList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapAcademicYearScoreList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAcademicYearScoreList[i].UpdatedAt);
                                }
                                if (learningGapAcademicYearScoreWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, learningGapAcademicYearScoreWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereList[i].AcademicYearId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `academic_year_id` = @academicYearIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@academicYearIdWhere" + i, learningGapAcademicYearScoreWhereList[i].AcademicYearId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereList[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentIdWhere" + i, learningGapAcademicYearScoreWhereList[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereList[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCodeWhere" + i, learningGapAcademicYearScoreWhereList[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereList[i].InitialGradeScore))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `initial_grade_score` = @initialGradeScoreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `initial_grade_score` = @initialGradeScoreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@initialGradeScoreWhere" + i, learningGapAcademicYearScoreWhereList[i].InitialGradeScore);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereList[i].FinalGradeScore))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `final_grade_score` = @finalGradeScoreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `final_grade_score` = @finalGradeScoreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@finalGradeScoreWhere" + i, learningGapAcademicYearScoreWhereList[i].FinalGradeScore);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, learningGapAcademicYearScoreWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, learningGapAcademicYearScoreWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScoreWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, learningGapAcademicYearScoreWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapAcademicYearScoreList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapAcademicYearScoreModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapAcademicYearScoreModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAcademicYearScoreModel>();
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

        public List<LearningGapAcademicYearScoreModel> Delete(string connectionString, LearningGapAcademicYearScoreModel learningGapAcademicYearScore)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAcademicYearScoreModel> response = new List<LearningGapAcademicYearScoreModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM learning_gap_academic_year_scores  ";
                if (learningGapAcademicYearScore != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", learningGapAcademicYearScore.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore.AcademicYearId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearId");
                        else
                            sql = String.Concat(sql, " `academic_year_id` = @academicYearId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@academicYearId", learningGapAcademicYearScore.AcademicYearId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentId");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentId", learningGapAcademicYearScore.StudentId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCode", learningGapAcademicYearScore.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore.InitialGradeScore))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `initial_grade_score` = @initialGradeScore");
                        else
                            sql = String.Concat(sql, " `initial_grade_score` = @initialGradeScore");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@initialGradeScore", learningGapAcademicYearScore.InitialGradeScore);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore.FinalGradeScore))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `final_grade_score` = @finalGradeScore");
                        else
                            sql = String.Concat(sql, " `final_grade_score` = @finalGradeScore");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@finalGradeScore", learningGapAcademicYearScore.FinalGradeScore);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", learningGapAcademicYearScore.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", learningGapAcademicYearScore.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", learningGapAcademicYearScore.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(learningGapAcademicYearScore);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAcademicYearScoreModel>();
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

        public List<LearningGapAcademicYearScoreModel> Delete(string connectionString, List<LearningGapAcademicYearScoreModel> learningGapAcademicYearScore)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAcademicYearScoreModel> response = new List<LearningGapAcademicYearScoreModel>();
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
                        for (int i = 0; i < learningGapAcademicYearScore.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM learning_gap_academic_year_scores  ";
                                if (learningGapAcademicYearScore[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, learningGapAcademicYearScore[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore[i].AcademicYearId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearId" + i);
                                        else
                                            sql = String.Concat(sql, " `academic_year_id` = @academicYearId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAcademicYearScore[i].AcademicYearId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentId" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentId" + i, learningGapAcademicYearScore[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapAcademicYearScore[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore[i].InitialGradeScore))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `initial_grade_score` = @initialGradeScore" + i);
                                        else
                                            sql = String.Concat(sql, " `initial_grade_score` = @initialGradeScore" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@initialGradeScore" + i, learningGapAcademicYearScore[i].InitialGradeScore);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore[i].FinalGradeScore))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `final_grade_score` = @finalGradeScore" + i);
                                        else
                                            sql = String.Concat(sql, " `final_grade_score` = @finalGradeScore" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@finalGradeScore" + i, learningGapAcademicYearScore[i].FinalGradeScore);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAcademicYearScore[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAcademicYearScore[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAcademicYearScore[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAcademicYearScore[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapAcademicYearScore[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapAcademicYearScoreModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapAcademicYearScoreModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAcademicYearScoreModel>();
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
