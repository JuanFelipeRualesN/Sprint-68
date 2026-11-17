using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.LearningGap;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.LearningGap
{
    class LearningGapDiagnosticYearScoreDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, LearningGapDiagnosticYearScoreModel> ReadD(string connectionString, LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScore, bool firstOne, bool orderByLastDate, LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScoreWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapDiagnosticYearScoreModel> response = new Dictionary<string, LearningGapDiagnosticYearScoreModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `level_code`, `initial_diagnostic_result_id`, `initial_diagnostic_date_limit`, `final_diagnostic_result_id`, `final_diagnostic_date_limit`, `initial_diagnostic_score`, `final_diagnostic_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_year_scores ";
                {
                    if (learningGapDiagnosticYearScore != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapDiagnosticYearScore.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapDiagnosticYearScore.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapDiagnosticYearScore.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapDiagnosticYearScore.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCode", learningGapDiagnosticYearScore.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_result_id` = @initialDiagnosticResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_result_id` = @initialDiagnosticResultId ");
                            }
                            cmd.Parameters.AddWithValue("@initialDiagnosticResultId", learningGapDiagnosticYearScore.InitialDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticDateLimit))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit ");
                            }
                            cmd.Parameters.AddWithValue("@initialDiagnosticDateLimit", learningGapDiagnosticYearScore.InitialDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_result_id` = @finalDiagnosticResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_result_id` = @finalDiagnosticResultId ");
                            }
                            cmd.Parameters.AddWithValue("@finalDiagnosticResultId", learningGapDiagnosticYearScore.FinalDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticDateLimit))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_date_limit` = @finalDiagnosticDateLimit ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_date_limit` = @finalDiagnosticDateLimit ");
                            }
                            cmd.Parameters.AddWithValue("@finalDiagnosticDateLimit", learningGapDiagnosticYearScore.FinalDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_score` = @initialDiagnosticScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_score` = @initialDiagnosticScore ");
                            }
                            cmd.Parameters.AddWithValue("@initialDiagnosticScore", learningGapDiagnosticYearScore.InitialDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_score` = @finalDiagnosticScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_score` = @finalDiagnosticScore ");
                            }
                            cmd.Parameters.AddWithValue("@finalDiagnosticScore", learningGapDiagnosticYearScore.FinalDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticYearScore.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticYearScore.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticYearScore.UpdatedAt);
                        }
                    }
                    if (learningGapDiagnosticYearScoreWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_result_id` LIKE @initialDiagnosticResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_result_id` LIKE @initialDiagnosticResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticResultIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticDateLimit))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_date_limit` LIKE @initialDiagnosticDateLimitWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_date_limit` LIKE @initialDiagnosticDateLimitWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticDateLimitWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticDateLimit + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_result_id` LIKE @finalDiagnosticResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_result_id` LIKE @finalDiagnosticResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticResultIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticDateLimit))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_date_limit` LIKE @finalDiagnosticDateLimitWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_date_limit` LIKE @finalDiagnosticDateLimitWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticDateLimitWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticDateLimit + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_score` LIKE @initialDiagnosticScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_score` LIKE @initialDiagnosticScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticScoreWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_score` LIKE @finalDiagnosticScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_score` LIKE @finalDiagnosticScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticScoreWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.UpdatedAt + "%");
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
                                learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel();
                                if (!reader.IsDBNull(0))
                                    learningGapDiagnosticYearScore.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapDiagnosticYearScore.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapDiagnosticYearScore.AcademicYearId = reader.GetValue(1).ToString();
                                else
                                    learningGapDiagnosticYearScore.AcademicYearId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapDiagnosticYearScore.StudentId = reader.GetValue(2).ToString();
                                else
                                    learningGapDiagnosticYearScore.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapDiagnosticYearScore.SubjectCode = reader.GetValue(3).ToString();
                                else
                                    learningGapDiagnosticYearScore.SubjectCode = null;
                                if (!reader.IsDBNull(4))
                                    learningGapDiagnosticYearScore.LevelCode = reader.GetValue(4).ToString();
                                else
                                    learningGapDiagnosticYearScore.LevelCode = null;
                                if (!reader.IsDBNull(5))
                                    learningGapDiagnosticYearScore.InitialDiagnosticResultId = reader.GetValue(5).ToString();
                                else
                                    learningGapDiagnosticYearScore.InitialDiagnosticResultId = null;
                                if (!reader.IsDBNull(6))
                                    learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = reader.GetValue(6).ToString();
                                else
                                    learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = null;
                                if (!reader.IsDBNull(7))
                                    learningGapDiagnosticYearScore.FinalDiagnosticResultId = reader.GetValue(7).ToString();
                                else
                                    learningGapDiagnosticYearScore.FinalDiagnosticResultId = null;
                                if (!reader.IsDBNull(8))
                                    learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = reader.GetValue(8).ToString();
                                else
                                    learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = null;
                                if (!reader.IsDBNull(9))
                                    learningGapDiagnosticYearScore.InitialDiagnosticScore = reader.GetValue(9).ToString();
                                else
                                    learningGapDiagnosticYearScore.InitialDiagnosticScore = null;
                                if (!reader.IsDBNull(10))
                                    learningGapDiagnosticYearScore.FinalDiagnosticScore = reader.GetValue(10).ToString();
                                else
                                    learningGapDiagnosticYearScore.FinalDiagnosticScore = null;
                                if (!reader.IsDBNull(11))
                                    learningGapDiagnosticYearScore.ModifiedBy = reader.GetValue(11).ToString();
                                else
                                    learningGapDiagnosticYearScore.ModifiedBy = null;
                                if (!reader.IsDBNull(12))
                                    learningGapDiagnosticYearScore.CreatedAt = reader.GetValue(12).ToString();
                                else
                                    learningGapDiagnosticYearScore.CreatedAt = null;
                                if (!reader.IsDBNull(13))
                                    learningGapDiagnosticYearScore.UpdatedAt = reader.GetValue(13).ToString();
                                else
                                    learningGapDiagnosticYearScore.UpdatedAt = null;
                                response.Add(learningGapDiagnosticYearScore.Id, learningGapDiagnosticYearScore);
                                learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapDiagnosticYearScoreModel>();
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
                response = new Dictionary<string, LearningGapDiagnosticYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapDiagnosticYearScoreModel> ReadD(string connectionString, LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScore, bool orderByLastDate, int fromRow, int limit, LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScoreWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapDiagnosticYearScoreModel> response = new Dictionary<string, LearningGapDiagnosticYearScoreModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `level_code`, `initial_diagnostic_result_id`, `initial_diagnostic_date_limit`, `final_diagnostic_result_id`, `final_diagnostic_date_limit`, `initial_diagnostic_score`, `final_diagnostic_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_year_scores ";
                {
                    if (learningGapDiagnosticYearScore != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapDiagnosticYearScore.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapDiagnosticYearScore.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapDiagnosticYearScore.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapDiagnosticYearScore.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCode", learningGapDiagnosticYearScore.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_result_id` = @initialDiagnosticResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_result_id` = @initialDiagnosticResultId ");
                            }
                            cmd.Parameters.AddWithValue("@initialDiagnosticResultId", learningGapDiagnosticYearScore.InitialDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticDateLimit))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit ");
                            }
                            cmd.Parameters.AddWithValue("@initialDiagnosticDateLimit", learningGapDiagnosticYearScore.InitialDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_result_id` = @finalDiagnosticResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_result_id` = @finalDiagnosticResultId ");
                            }
                            cmd.Parameters.AddWithValue("@finalDiagnosticResultId", learningGapDiagnosticYearScore.FinalDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticDateLimit))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_date_limit` = @finalDiagnosticDateLimit ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_date_limit` = @finalDiagnosticDateLimit ");
                            }
                            cmd.Parameters.AddWithValue("@finalDiagnosticDateLimit", learningGapDiagnosticYearScore.FinalDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_score` = @initialDiagnosticScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_score` = @initialDiagnosticScore ");
                            }
                            cmd.Parameters.AddWithValue("@initialDiagnosticScore", learningGapDiagnosticYearScore.InitialDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_score` = @finalDiagnosticScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_score` = @finalDiagnosticScore ");
                            }
                            cmd.Parameters.AddWithValue("@finalDiagnosticScore", learningGapDiagnosticYearScore.FinalDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticYearScore.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticYearScore.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticYearScore.UpdatedAt);
                        }
                    }
                    if (learningGapDiagnosticYearScoreWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_result_id` LIKE @initialDiagnosticResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_result_id` LIKE @initialDiagnosticResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticResultIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticDateLimit))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_date_limit` LIKE @initialDiagnosticDateLimitWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_date_limit` LIKE @initialDiagnosticDateLimitWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticDateLimitWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticDateLimit + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_result_id` LIKE @finalDiagnosticResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_result_id` LIKE @finalDiagnosticResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticResultIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticDateLimit))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_date_limit` LIKE @finalDiagnosticDateLimitWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_date_limit` LIKE @finalDiagnosticDateLimitWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticDateLimitWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticDateLimit + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_score` LIKE @initialDiagnosticScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_score` LIKE @initialDiagnosticScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticScoreWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_score` LIKE @finalDiagnosticScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_score` LIKE @finalDiagnosticScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticScoreWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.UpdatedAt + "%");
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
                                learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel();
                                if (!reader.IsDBNull(0))
                                    learningGapDiagnosticYearScore.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapDiagnosticYearScore.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapDiagnosticYearScore.AcademicYearId = reader.GetValue(1).ToString();
                                else
                                    learningGapDiagnosticYearScore.AcademicYearId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapDiagnosticYearScore.StudentId = reader.GetValue(2).ToString();
                                else
                                    learningGapDiagnosticYearScore.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapDiagnosticYearScore.SubjectCode = reader.GetValue(3).ToString();
                                else
                                    learningGapDiagnosticYearScore.SubjectCode = null;
                                if (!reader.IsDBNull(4))
                                    learningGapDiagnosticYearScore.LevelCode = reader.GetValue(4).ToString();
                                else
                                    learningGapDiagnosticYearScore.LevelCode = null;
                                if (!reader.IsDBNull(5))
                                    learningGapDiagnosticYearScore.InitialDiagnosticResultId = reader.GetValue(5).ToString();
                                else
                                    learningGapDiagnosticYearScore.InitialDiagnosticResultId = null;
                                if (!reader.IsDBNull(6))
                                    learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = reader.GetValue(6).ToString();
                                else
                                    learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = null;
                                if (!reader.IsDBNull(7))
                                    learningGapDiagnosticYearScore.FinalDiagnosticResultId = reader.GetValue(7).ToString();
                                else
                                    learningGapDiagnosticYearScore.FinalDiagnosticResultId = null;
                                if (!reader.IsDBNull(8))
                                    learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = reader.GetValue(8).ToString();
                                else
                                    learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = null;
                                if (!reader.IsDBNull(9))
                                    learningGapDiagnosticYearScore.InitialDiagnosticScore = reader.GetValue(9).ToString();
                                else
                                    learningGapDiagnosticYearScore.InitialDiagnosticScore = null;
                                if (!reader.IsDBNull(10))
                                    learningGapDiagnosticYearScore.FinalDiagnosticScore = reader.GetValue(10).ToString();
                                else
                                    learningGapDiagnosticYearScore.FinalDiagnosticScore = null;
                                if (!reader.IsDBNull(11))
                                    learningGapDiagnosticYearScore.ModifiedBy = reader.GetValue(11).ToString();
                                else
                                    learningGapDiagnosticYearScore.ModifiedBy = null;
                                if (!reader.IsDBNull(12))
                                    learningGapDiagnosticYearScore.CreatedAt = reader.GetValue(12).ToString();
                                else
                                    learningGapDiagnosticYearScore.CreatedAt = null;
                                if (!reader.IsDBNull(13))
                                    learningGapDiagnosticYearScore.UpdatedAt = reader.GetValue(13).ToString();
                                else
                                    learningGapDiagnosticYearScore.UpdatedAt = null;
                                response.Add(learningGapDiagnosticYearScore.Id, learningGapDiagnosticYearScore);
                                learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapDiagnosticYearScoreModel>();
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
                response = new Dictionary<string, LearningGapDiagnosticYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapDiagnosticYearScoreModel> ReadD(string connectionString, List<LearningGapDiagnosticYearScoreModel> learningGapDiagnosticYearScoreWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapDiagnosticYearScoreModel> response = new Dictionary<string, LearningGapDiagnosticYearScoreModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `level_code`, `initial_diagnostic_result_id`, `initial_diagnostic_date_limit`, `final_diagnostic_result_id`, `final_diagnostic_date_limit`, `initial_diagnostic_score`, `final_diagnostic_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_year_scores ";
                if (learningGapDiagnosticYearScoreWhere != null && learningGapDiagnosticYearScoreWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapDiagnosticYearScoreWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticYearScoreWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapDiagnosticYearScoreWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapDiagnosticYearScoreWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapDiagnosticYearScoreWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, learningGapDiagnosticYearScoreWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_diagnostic_result_id` = @initialDiagnosticResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticResultId" + i, learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticDateLimit))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticDateLimit" + i, learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_diagnostic_result_id` = @finalDiagnosticResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticResultId" + i, learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticDateLimit))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_diagnostic_date_limit` = @finalDiagnosticDateLimit" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticDateLimit" + i, learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_diagnostic_score` = @initialDiagnosticScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticScore" + i, learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_diagnostic_score` = @finalDiagnosticScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticScore" + i, learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticYearScoreWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticYearScoreWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticYearScoreWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapDiagnosticYearScoreWhere.Count - 1)
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
                            LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel(); if (!reader.IsDBNull(0))
                                learningGapDiagnosticYearScore.Id = reader.GetValue(0).ToString();
                            else
                                learningGapDiagnosticYearScore.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapDiagnosticYearScore.AcademicYearId = reader.GetValue(1).ToString();
                            else
                                learningGapDiagnosticYearScore.AcademicYearId = null;
                            if (!reader.IsDBNull(2))
                                learningGapDiagnosticYearScore.StudentId = reader.GetValue(2).ToString();
                            else
                                learningGapDiagnosticYearScore.StudentId = null;
                            if (!reader.IsDBNull(3))
                                learningGapDiagnosticYearScore.SubjectCode = reader.GetValue(3).ToString();
                            else
                                learningGapDiagnosticYearScore.SubjectCode = null;
                            if (!reader.IsDBNull(4))
                                learningGapDiagnosticYearScore.LevelCode = reader.GetValue(4).ToString();
                            else
                                learningGapDiagnosticYearScore.LevelCode = null;
                            if (!reader.IsDBNull(5))
                                learningGapDiagnosticYearScore.InitialDiagnosticResultId = reader.GetValue(5).ToString();
                            else
                                learningGapDiagnosticYearScore.InitialDiagnosticResultId = null;
                            if (!reader.IsDBNull(6))
                                learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = reader.GetValue(6).ToString();
                            else
                                learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = null;
                            if (!reader.IsDBNull(7))
                                learningGapDiagnosticYearScore.FinalDiagnosticResultId = reader.GetValue(7).ToString();
                            else
                                learningGapDiagnosticYearScore.FinalDiagnosticResultId = null;
                            if (!reader.IsDBNull(8))
                                learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = reader.GetValue(8).ToString();
                            else
                                learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = null;
                            if (!reader.IsDBNull(9))
                                learningGapDiagnosticYearScore.InitialDiagnosticScore = reader.GetValue(9).ToString();
                            else
                                learningGapDiagnosticYearScore.InitialDiagnosticScore = null;
                            if (!reader.IsDBNull(10))
                                learningGapDiagnosticYearScore.FinalDiagnosticScore = reader.GetValue(10).ToString();
                            else
                                learningGapDiagnosticYearScore.FinalDiagnosticScore = null;
                            if (!reader.IsDBNull(11))
                                learningGapDiagnosticYearScore.ModifiedBy = reader.GetValue(11).ToString();
                            else
                                learningGapDiagnosticYearScore.ModifiedBy = null;
                            if (!reader.IsDBNull(12))
                                learningGapDiagnosticYearScore.CreatedAt = reader.GetValue(12).ToString();
                            else
                                learningGapDiagnosticYearScore.CreatedAt = null;
                            if (!reader.IsDBNull(13))
                                learningGapDiagnosticYearScore.UpdatedAt = reader.GetValue(13).ToString();
                            else
                                learningGapDiagnosticYearScore.UpdatedAt = null;
                            response.Add(learningGapDiagnosticYearScore.Id, learningGapDiagnosticYearScore);
                            learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapDiagnosticYearScoreModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapDiagnosticYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapDiagnosticYearScoreModel> ReadD(string connectionString, List<LearningGapDiagnosticYearScoreModel> learningGapDiagnosticYearScoreWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapDiagnosticYearScoreModel> response = new Dictionary<string, LearningGapDiagnosticYearScoreModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `level_code`, `initial_diagnostic_result_id`, `initial_diagnostic_date_limit`, `final_diagnostic_result_id`, `final_diagnostic_date_limit`, `initial_diagnostic_score`, `final_diagnostic_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_year_scores ";
                if (learningGapDiagnosticYearScoreWhere != null && learningGapDiagnosticYearScoreWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapDiagnosticYearScoreWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticYearScoreWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapDiagnosticYearScoreWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapDiagnosticYearScoreWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapDiagnosticYearScoreWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, learningGapDiagnosticYearScoreWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_diagnostic_result_id` = @initialDiagnosticResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticResultId" + i, learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticDateLimit))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticDateLimit" + i, learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_diagnostic_result_id` = @finalDiagnosticResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticResultId" + i, learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticDateLimit))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_diagnostic_date_limit` = @finalDiagnosticDateLimit" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticDateLimit" + i, learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_diagnostic_score` = @initialDiagnosticScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticScore" + i, learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_diagnostic_score` = @finalDiagnosticScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticScore" + i, learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticYearScoreWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticYearScoreWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticYearScoreWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapDiagnosticYearScoreWhere.Count - 1)
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
                            LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel(); if (!reader.IsDBNull(0))
                                learningGapDiagnosticYearScore.Id = reader.GetValue(0).ToString();
                            else
                                learningGapDiagnosticYearScore.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapDiagnosticYearScore.AcademicYearId = reader.GetValue(1).ToString();
                            else
                                learningGapDiagnosticYearScore.AcademicYearId = null;
                            if (!reader.IsDBNull(2))
                                learningGapDiagnosticYearScore.StudentId = reader.GetValue(2).ToString();
                            else
                                learningGapDiagnosticYearScore.StudentId = null;
                            if (!reader.IsDBNull(3))
                                learningGapDiagnosticYearScore.SubjectCode = reader.GetValue(3).ToString();
                            else
                                learningGapDiagnosticYearScore.SubjectCode = null;
                            if (!reader.IsDBNull(4))
                                learningGapDiagnosticYearScore.LevelCode = reader.GetValue(4).ToString();
                            else
                                learningGapDiagnosticYearScore.LevelCode = null;
                            if (!reader.IsDBNull(5))
                                learningGapDiagnosticYearScore.InitialDiagnosticResultId = reader.GetValue(5).ToString();
                            else
                                learningGapDiagnosticYearScore.InitialDiagnosticResultId = null;
                            if (!reader.IsDBNull(6))
                                learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = reader.GetValue(6).ToString();
                            else
                                learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = null;
                            if (!reader.IsDBNull(7))
                                learningGapDiagnosticYearScore.FinalDiagnosticResultId = reader.GetValue(7).ToString();
                            else
                                learningGapDiagnosticYearScore.FinalDiagnosticResultId = null;
                            if (!reader.IsDBNull(8))
                                learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = reader.GetValue(8).ToString();
                            else
                                learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = null;
                            if (!reader.IsDBNull(9))
                                learningGapDiagnosticYearScore.InitialDiagnosticScore = reader.GetValue(9).ToString();
                            else
                                learningGapDiagnosticYearScore.InitialDiagnosticScore = null;
                            if (!reader.IsDBNull(10))
                                learningGapDiagnosticYearScore.FinalDiagnosticScore = reader.GetValue(10).ToString();
                            else
                                learningGapDiagnosticYearScore.FinalDiagnosticScore = null;
                            if (!reader.IsDBNull(11))
                                learningGapDiagnosticYearScore.ModifiedBy = reader.GetValue(11).ToString();
                            else
                                learningGapDiagnosticYearScore.ModifiedBy = null;
                            if (!reader.IsDBNull(12))
                                learningGapDiagnosticYearScore.CreatedAt = reader.GetValue(12).ToString();
                            else
                                learningGapDiagnosticYearScore.CreatedAt = null;
                            if (!reader.IsDBNull(13))
                                learningGapDiagnosticYearScore.UpdatedAt = reader.GetValue(13).ToString();
                            else
                                learningGapDiagnosticYearScore.UpdatedAt = null;
                            response.Add(learningGapDiagnosticYearScore.Id, learningGapDiagnosticYearScore);
                            learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapDiagnosticYearScoreModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapDiagnosticYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapDiagnosticYearScoreModel> Read(string connectionString, LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScore, bool firstOne, bool orderByLastDate, LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScoreWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticYearScoreModel> response = new List<LearningGapDiagnosticYearScoreModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `level_code`, `initial_diagnostic_result_id`, `initial_diagnostic_date_limit`, `final_diagnostic_result_id`, `final_diagnostic_date_limit`, `initial_diagnostic_score`, `final_diagnostic_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_year_scores ";
                {
                    if (learningGapDiagnosticYearScore != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapDiagnosticYearScore.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapDiagnosticYearScore.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapDiagnosticYearScore.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapDiagnosticYearScore.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCode", learningGapDiagnosticYearScore.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_result_id` = @initialDiagnosticResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_result_id` = @initialDiagnosticResultId ");
                            }
                            cmd.Parameters.AddWithValue("@initialDiagnosticResultId", learningGapDiagnosticYearScore.InitialDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticDateLimit))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit ");
                            }
                            cmd.Parameters.AddWithValue("@initialDiagnosticDateLimit", learningGapDiagnosticYearScore.InitialDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_result_id` = @finalDiagnosticResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_result_id` = @finalDiagnosticResultId ");
                            }
                            cmd.Parameters.AddWithValue("@finalDiagnosticResultId", learningGapDiagnosticYearScore.FinalDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticDateLimit))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_date_limit` = @finalDiagnosticDateLimit ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_date_limit` = @finalDiagnosticDateLimit ");
                            }
                            cmd.Parameters.AddWithValue("@finalDiagnosticDateLimit", learningGapDiagnosticYearScore.FinalDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_score` = @initialDiagnosticScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_score` = @initialDiagnosticScore ");
                            }
                            cmd.Parameters.AddWithValue("@initialDiagnosticScore", learningGapDiagnosticYearScore.InitialDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_score` = @finalDiagnosticScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_score` = @finalDiagnosticScore ");
                            }
                            cmd.Parameters.AddWithValue("@finalDiagnosticScore", learningGapDiagnosticYearScore.FinalDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticYearScore.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticYearScore.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticYearScore.UpdatedAt);
                        }
                    }
                    if (learningGapDiagnosticYearScoreWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_result_id` LIKE @initialDiagnosticResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_result_id` LIKE @initialDiagnosticResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticResultIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticDateLimit))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_date_limit` LIKE @initialDiagnosticDateLimitWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_date_limit` LIKE @initialDiagnosticDateLimitWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticDateLimitWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticDateLimit + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_result_id` LIKE @finalDiagnosticResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_result_id` LIKE @finalDiagnosticResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticResultIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticDateLimit))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_date_limit` LIKE @finalDiagnosticDateLimitWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_date_limit` LIKE @finalDiagnosticDateLimitWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticDateLimitWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticDateLimit + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_score` LIKE @initialDiagnosticScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_score` LIKE @initialDiagnosticScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticScoreWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_score` LIKE @finalDiagnosticScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_score` LIKE @finalDiagnosticScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticScoreWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.UpdatedAt + "%");
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
                                learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel();
                                if (!reader.IsDBNull(0))
                                    learningGapDiagnosticYearScore.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapDiagnosticYearScore.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapDiagnosticYearScore.AcademicYearId = reader.GetValue(1).ToString();
                                else
                                    learningGapDiagnosticYearScore.AcademicYearId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapDiagnosticYearScore.StudentId = reader.GetValue(2).ToString();
                                else
                                    learningGapDiagnosticYearScore.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapDiagnosticYearScore.SubjectCode = reader.GetValue(3).ToString();
                                else
                                    learningGapDiagnosticYearScore.SubjectCode = null;
                                if (!reader.IsDBNull(4))
                                    learningGapDiagnosticYearScore.LevelCode = reader.GetValue(4).ToString();
                                else
                                    learningGapDiagnosticYearScore.LevelCode = null;
                                if (!reader.IsDBNull(5))
                                    learningGapDiagnosticYearScore.InitialDiagnosticResultId = reader.GetValue(5).ToString();
                                else
                                    learningGapDiagnosticYearScore.InitialDiagnosticResultId = null;
                                if (!reader.IsDBNull(6))
                                    learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = reader.GetValue(6).ToString();
                                else
                                    learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = null;
                                if (!reader.IsDBNull(7))
                                    learningGapDiagnosticYearScore.FinalDiagnosticResultId = reader.GetValue(7).ToString();
                                else
                                    learningGapDiagnosticYearScore.FinalDiagnosticResultId = null;
                                if (!reader.IsDBNull(8))
                                    learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = reader.GetValue(8).ToString();
                                else
                                    learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = null;
                                if (!reader.IsDBNull(9))
                                    learningGapDiagnosticYearScore.InitialDiagnosticScore = reader.GetValue(9).ToString();
                                else
                                    learningGapDiagnosticYearScore.InitialDiagnosticScore = null;
                                if (!reader.IsDBNull(10))
                                    learningGapDiagnosticYearScore.FinalDiagnosticScore = reader.GetValue(10).ToString();
                                else
                                    learningGapDiagnosticYearScore.FinalDiagnosticScore = null;
                                if (!reader.IsDBNull(11))
                                    learningGapDiagnosticYearScore.ModifiedBy = reader.GetValue(11).ToString();
                                else
                                    learningGapDiagnosticYearScore.ModifiedBy = null;
                                if (!reader.IsDBNull(12))
                                    learningGapDiagnosticYearScore.CreatedAt = reader.GetValue(12).ToString();
                                else
                                    learningGapDiagnosticYearScore.CreatedAt = null;
                                if (!reader.IsDBNull(13))
                                    learningGapDiagnosticYearScore.UpdatedAt = reader.GetValue(13).ToString();
                                else
                                    learningGapDiagnosticYearScore.UpdatedAt = null;
                                response.Add(learningGapDiagnosticYearScore);
                                learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapDiagnosticYearScoreModel>();
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
                response = new List<LearningGapDiagnosticYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapDiagnosticYearScoreModel> Read(string connectionString, LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScore, bool orderByLastDate, int fromRow, int limit, LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScoreWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticYearScoreModel> response = new List<LearningGapDiagnosticYearScoreModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `level_code`, `initial_diagnostic_result_id`, `initial_diagnostic_date_limit`, `final_diagnostic_result_id`, `final_diagnostic_date_limit`, `initial_diagnostic_score`, `final_diagnostic_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_year_scores ";
                {
                    if (learningGapDiagnosticYearScore != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapDiagnosticYearScore.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapDiagnosticYearScore.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapDiagnosticYearScore.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapDiagnosticYearScore.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCode", learningGapDiagnosticYearScore.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_result_id` = @initialDiagnosticResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_result_id` = @initialDiagnosticResultId ");
                            }
                            cmd.Parameters.AddWithValue("@initialDiagnosticResultId", learningGapDiagnosticYearScore.InitialDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticDateLimit))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit ");
                            }
                            cmd.Parameters.AddWithValue("@initialDiagnosticDateLimit", learningGapDiagnosticYearScore.InitialDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_result_id` = @finalDiagnosticResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_result_id` = @finalDiagnosticResultId ");
                            }
                            cmd.Parameters.AddWithValue("@finalDiagnosticResultId", learningGapDiagnosticYearScore.FinalDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticDateLimit))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_date_limit` = @finalDiagnosticDateLimit ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_date_limit` = @finalDiagnosticDateLimit ");
                            }
                            cmd.Parameters.AddWithValue("@finalDiagnosticDateLimit", learningGapDiagnosticYearScore.FinalDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_score` = @initialDiagnosticScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_score` = @initialDiagnosticScore ");
                            }
                            cmd.Parameters.AddWithValue("@initialDiagnosticScore", learningGapDiagnosticYearScore.InitialDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_score` = @finalDiagnosticScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_score` = @finalDiagnosticScore ");
                            }
                            cmd.Parameters.AddWithValue("@finalDiagnosticScore", learningGapDiagnosticYearScore.FinalDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticYearScore.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticYearScore.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticYearScore.UpdatedAt);
                        }
                    }
                    if (learningGapDiagnosticYearScoreWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_result_id` LIKE @initialDiagnosticResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_result_id` LIKE @initialDiagnosticResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticResultIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticDateLimit))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_date_limit` LIKE @initialDiagnosticDateLimitWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_date_limit` LIKE @initialDiagnosticDateLimitWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticDateLimitWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticDateLimit + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_result_id` LIKE @finalDiagnosticResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_result_id` LIKE @finalDiagnosticResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticResultIdWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticDateLimit))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_date_limit` LIKE @finalDiagnosticDateLimitWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_date_limit` LIKE @finalDiagnosticDateLimitWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticDateLimitWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticDateLimit + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `initial_diagnostic_score` LIKE @initialDiagnosticScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `initial_diagnostic_score` LIKE @initialDiagnosticScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticScoreWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.InitialDiagnosticScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `final_diagnostic_score` LIKE @finalDiagnosticScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `final_diagnostic_score` LIKE @finalDiagnosticScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticScoreWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.FinalDiagnosticScore + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapDiagnosticYearScoreWhereLike.UpdatedAt + "%");
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
                                learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel();
                                if (!reader.IsDBNull(0))
                                    learningGapDiagnosticYearScore.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapDiagnosticYearScore.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapDiagnosticYearScore.AcademicYearId = reader.GetValue(1).ToString();
                                else
                                    learningGapDiagnosticYearScore.AcademicYearId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapDiagnosticYearScore.StudentId = reader.GetValue(2).ToString();
                                else
                                    learningGapDiagnosticYearScore.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapDiagnosticYearScore.SubjectCode = reader.GetValue(3).ToString();
                                else
                                    learningGapDiagnosticYearScore.SubjectCode = null;
                                if (!reader.IsDBNull(4))
                                    learningGapDiagnosticYearScore.LevelCode = reader.GetValue(4).ToString();
                                else
                                    learningGapDiagnosticYearScore.LevelCode = null;
                                if (!reader.IsDBNull(5))
                                    learningGapDiagnosticYearScore.InitialDiagnosticResultId = reader.GetValue(5).ToString();
                                else
                                    learningGapDiagnosticYearScore.InitialDiagnosticResultId = null;
                                if (!reader.IsDBNull(6))
                                    learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = reader.GetValue(6).ToString();
                                else
                                    learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = null;
                                if (!reader.IsDBNull(7))
                                    learningGapDiagnosticYearScore.FinalDiagnosticResultId = reader.GetValue(7).ToString();
                                else
                                    learningGapDiagnosticYearScore.FinalDiagnosticResultId = null;
                                if (!reader.IsDBNull(8))
                                    learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = reader.GetValue(8).ToString();
                                else
                                    learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = null;
                                if (!reader.IsDBNull(9))
                                    learningGapDiagnosticYearScore.InitialDiagnosticScore = reader.GetValue(9).ToString();
                                else
                                    learningGapDiagnosticYearScore.InitialDiagnosticScore = null;
                                if (!reader.IsDBNull(10))
                                    learningGapDiagnosticYearScore.FinalDiagnosticScore = reader.GetValue(10).ToString();
                                else
                                    learningGapDiagnosticYearScore.FinalDiagnosticScore = null;
                                if (!reader.IsDBNull(11))
                                    learningGapDiagnosticYearScore.ModifiedBy = reader.GetValue(11).ToString();
                                else
                                    learningGapDiagnosticYearScore.ModifiedBy = null;
                                if (!reader.IsDBNull(12))
                                    learningGapDiagnosticYearScore.CreatedAt = reader.GetValue(12).ToString();
                                else
                                    learningGapDiagnosticYearScore.CreatedAt = null;
                                if (!reader.IsDBNull(13))
                                    learningGapDiagnosticYearScore.UpdatedAt = reader.GetValue(13).ToString();
                                else
                                    learningGapDiagnosticYearScore.UpdatedAt = null;
                                response.Add(learningGapDiagnosticYearScore);
                                learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapDiagnosticYearScoreModel>();
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
                response = new List<LearningGapDiagnosticYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapDiagnosticYearScoreModel> Read(string connectionString, List<LearningGapDiagnosticYearScoreModel> learningGapDiagnosticYearScoreWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticYearScoreModel> response = new List<LearningGapDiagnosticYearScoreModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `level_code`, `initial_diagnostic_result_id`, `initial_diagnostic_date_limit`, `final_diagnostic_result_id`, `final_diagnostic_date_limit`, `initial_diagnostic_score`, `final_diagnostic_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_year_scores ";
                if (learningGapDiagnosticYearScoreWhere != null && learningGapDiagnosticYearScoreWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapDiagnosticYearScoreWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticYearScoreWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapDiagnosticYearScoreWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapDiagnosticYearScoreWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapDiagnosticYearScoreWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, learningGapDiagnosticYearScoreWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_diagnostic_result_id` = @initialDiagnosticResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticResultId" + i, learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticDateLimit))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticDateLimit" + i, learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_diagnostic_result_id` = @finalDiagnosticResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticResultId" + i, learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticDateLimit))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_diagnostic_date_limit` = @finalDiagnosticDateLimit" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticDateLimit" + i, learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_diagnostic_score` = @initialDiagnosticScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticScore" + i, learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_diagnostic_score` = @finalDiagnosticScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticScore" + i, learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticYearScoreWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticYearScoreWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticYearScoreWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapDiagnosticYearScoreWhere.Count - 1)
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
                            LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel(); if (!reader.IsDBNull(0))
                                learningGapDiagnosticYearScore.Id = reader.GetValue(0).ToString();
                            else
                                learningGapDiagnosticYearScore.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapDiagnosticYearScore.AcademicYearId = reader.GetValue(1).ToString();
                            else
                                learningGapDiagnosticYearScore.AcademicYearId = null;
                            if (!reader.IsDBNull(2))
                                learningGapDiagnosticYearScore.StudentId = reader.GetValue(2).ToString();
                            else
                                learningGapDiagnosticYearScore.StudentId = null;
                            if (!reader.IsDBNull(3))
                                learningGapDiagnosticYearScore.SubjectCode = reader.GetValue(3).ToString();
                            else
                                learningGapDiagnosticYearScore.SubjectCode = null;
                            if (!reader.IsDBNull(4))
                                learningGapDiagnosticYearScore.LevelCode = reader.GetValue(4).ToString();
                            else
                                learningGapDiagnosticYearScore.LevelCode = null;
                            if (!reader.IsDBNull(5))
                                learningGapDiagnosticYearScore.InitialDiagnosticResultId = reader.GetValue(5).ToString();
                            else
                                learningGapDiagnosticYearScore.InitialDiagnosticResultId = null;
                            if (!reader.IsDBNull(6))
                                learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = reader.GetValue(6).ToString();
                            else
                                learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = null;
                            if (!reader.IsDBNull(7))
                                learningGapDiagnosticYearScore.FinalDiagnosticResultId = reader.GetValue(7).ToString();
                            else
                                learningGapDiagnosticYearScore.FinalDiagnosticResultId = null;
                            if (!reader.IsDBNull(8))
                                learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = reader.GetValue(8).ToString();
                            else
                                learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = null;
                            if (!reader.IsDBNull(9))
                                learningGapDiagnosticYearScore.InitialDiagnosticScore = reader.GetValue(9).ToString();
                            else
                                learningGapDiagnosticYearScore.InitialDiagnosticScore = null;
                            if (!reader.IsDBNull(10))
                                learningGapDiagnosticYearScore.FinalDiagnosticScore = reader.GetValue(10).ToString();
                            else
                                learningGapDiagnosticYearScore.FinalDiagnosticScore = null;
                            if (!reader.IsDBNull(11))
                                learningGapDiagnosticYearScore.ModifiedBy = reader.GetValue(11).ToString();
                            else
                                learningGapDiagnosticYearScore.ModifiedBy = null;
                            if (!reader.IsDBNull(12))
                                learningGapDiagnosticYearScore.CreatedAt = reader.GetValue(12).ToString();
                            else
                                learningGapDiagnosticYearScore.CreatedAt = null;
                            if (!reader.IsDBNull(13))
                                learningGapDiagnosticYearScore.UpdatedAt = reader.GetValue(13).ToString();
                            else
                                learningGapDiagnosticYearScore.UpdatedAt = null;
                            response.Add(learningGapDiagnosticYearScore);
                            learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapDiagnosticYearScoreModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapDiagnosticYearScoreModel> Read(string connectionString, List<LearningGapDiagnosticYearScoreModel> learningGapDiagnosticYearScoreWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticYearScoreModel> response = new List<LearningGapDiagnosticYearScoreModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `academic_year_id`, `student_id`, `subject_code`, `level_code`, `initial_diagnostic_result_id`, `initial_diagnostic_date_limit`, `final_diagnostic_result_id`, `final_diagnostic_date_limit`, `initial_diagnostic_score`, `final_diagnostic_score`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_diagnostic_year_scores ";
                if (learningGapDiagnosticYearScoreWhere != null && learningGapDiagnosticYearScoreWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapDiagnosticYearScoreWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticYearScoreWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapDiagnosticYearScoreWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapDiagnosticYearScoreWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapDiagnosticYearScoreWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, learningGapDiagnosticYearScoreWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_diagnostic_result_id` = @initialDiagnosticResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticResultId" + i, learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticDateLimit))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticDateLimit" + i, learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_diagnostic_result_id` = @finalDiagnosticResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticResultId" + i, learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticResultId);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticDateLimit))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_diagnostic_date_limit` = @finalDiagnosticDateLimit" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticDateLimit" + i, learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticDateLimit);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `initial_diagnostic_score` = @initialDiagnosticScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@initialDiagnosticScore" + i, learningGapDiagnosticYearScoreWhere[i].InitialDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `final_diagnostic_score` = @finalDiagnosticScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@finalDiagnosticScore" + i, learningGapDiagnosticYearScoreWhere[i].FinalDiagnosticScore);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticYearScoreWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticYearScoreWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticYearScoreWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapDiagnosticYearScoreWhere.Count - 1)
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
                            LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel(); if (!reader.IsDBNull(0))
                                learningGapDiagnosticYearScore.Id = reader.GetValue(0).ToString();
                            else
                                learningGapDiagnosticYearScore.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapDiagnosticYearScore.AcademicYearId = reader.GetValue(1).ToString();
                            else
                                learningGapDiagnosticYearScore.AcademicYearId = null;
                            if (!reader.IsDBNull(2))
                                learningGapDiagnosticYearScore.StudentId = reader.GetValue(2).ToString();
                            else
                                learningGapDiagnosticYearScore.StudentId = null;
                            if (!reader.IsDBNull(3))
                                learningGapDiagnosticYearScore.SubjectCode = reader.GetValue(3).ToString();
                            else
                                learningGapDiagnosticYearScore.SubjectCode = null;
                            if (!reader.IsDBNull(4))
                                learningGapDiagnosticYearScore.LevelCode = reader.GetValue(4).ToString();
                            else
                                learningGapDiagnosticYearScore.LevelCode = null;
                            if (!reader.IsDBNull(5))
                                learningGapDiagnosticYearScore.InitialDiagnosticResultId = reader.GetValue(5).ToString();
                            else
                                learningGapDiagnosticYearScore.InitialDiagnosticResultId = null;
                            if (!reader.IsDBNull(6))
                                learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = reader.GetValue(6).ToString();
                            else
                                learningGapDiagnosticYearScore.InitialDiagnosticDateLimit = null;
                            if (!reader.IsDBNull(7))
                                learningGapDiagnosticYearScore.FinalDiagnosticResultId = reader.GetValue(7).ToString();
                            else
                                learningGapDiagnosticYearScore.FinalDiagnosticResultId = null;
                            if (!reader.IsDBNull(8))
                                learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = reader.GetValue(8).ToString();
                            else
                                learningGapDiagnosticYearScore.FinalDiagnosticDateLimit = null;
                            if (!reader.IsDBNull(9))
                                learningGapDiagnosticYearScore.InitialDiagnosticScore = reader.GetValue(9).ToString();
                            else
                                learningGapDiagnosticYearScore.InitialDiagnosticScore = null;
                            if (!reader.IsDBNull(10))
                                learningGapDiagnosticYearScore.FinalDiagnosticScore = reader.GetValue(10).ToString();
                            else
                                learningGapDiagnosticYearScore.FinalDiagnosticScore = null;
                            if (!reader.IsDBNull(11))
                                learningGapDiagnosticYearScore.ModifiedBy = reader.GetValue(11).ToString();
                            else
                                learningGapDiagnosticYearScore.ModifiedBy = null;
                            if (!reader.IsDBNull(12))
                                learningGapDiagnosticYearScore.CreatedAt = reader.GetValue(12).ToString();
                            else
                                learningGapDiagnosticYearScore.CreatedAt = null;
                            if (!reader.IsDBNull(13))
                                learningGapDiagnosticYearScore.UpdatedAt = reader.GetValue(13).ToString();
                            else
                                learningGapDiagnosticYearScore.UpdatedAt = null;
                            response.Add(learningGapDiagnosticYearScore);
                            learningGapDiagnosticYearScore = new LearningGapDiagnosticYearScoreModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapDiagnosticYearScoreModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticYearScoreModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapDiagnosticYearScoreModel> Create(string connectionString, LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScore)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticYearScoreModel> response = new List<LearningGapDiagnosticYearScoreModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO learning_gap_diagnostic_year_scores (";
                string values = "(";
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.Id))
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
                    cmd.Parameters.AddWithValue("@id", learningGapDiagnosticYearScore.Id);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.AcademicYearId))
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
                    cmd.Parameters.AddWithValue("@academicYearId", learningGapDiagnosticYearScore.AcademicYearId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.StudentId))
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
                    cmd.Parameters.AddWithValue("@studentId", learningGapDiagnosticYearScore.StudentId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.SubjectCode))
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
                    cmd.Parameters.AddWithValue("@subjectCode", learningGapDiagnosticYearScore.SubjectCode);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.LevelCode))
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
                    cmd.Parameters.AddWithValue("@levelCode", learningGapDiagnosticYearScore.LevelCode);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticResultId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `initial_diagnostic_result_id` ");
                        values = String.Concat(values, ", @initialDiagnosticResultId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `initial_diagnostic_result_id` ");
                        values = String.Concat(values, " @initialDiagnosticResultId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@initialDiagnosticResultId", learningGapDiagnosticYearScore.InitialDiagnosticResultId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticDateLimit))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `initial_diagnostic_date_limit` ");
                        values = String.Concat(values, ", @initialDiagnosticDateLimit");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `initial_diagnostic_date_limit` ");
                        values = String.Concat(values, " @initialDiagnosticDateLimit");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@initialDiagnosticDateLimit", learningGapDiagnosticYearScore.InitialDiagnosticDateLimit);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticResultId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `final_diagnostic_result_id` ");
                        values = String.Concat(values, ", @finalDiagnosticResultId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `final_diagnostic_result_id` ");
                        values = String.Concat(values, " @finalDiagnosticResultId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@finalDiagnosticResultId", learningGapDiagnosticYearScore.FinalDiagnosticResultId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticDateLimit))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `final_diagnostic_date_limit` ");
                        values = String.Concat(values, ", @finalDiagnosticDateLimit");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `final_diagnostic_date_limit` ");
                        values = String.Concat(values, " @finalDiagnosticDateLimit");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@finalDiagnosticDateLimit", learningGapDiagnosticYearScore.FinalDiagnosticDateLimit);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticScore))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `initial_diagnostic_score` ");
                        values = String.Concat(values, ", @initialDiagnosticScore");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `initial_diagnostic_score` ");
                        values = String.Concat(values, " @initialDiagnosticScore");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@initialDiagnosticScore", learningGapDiagnosticYearScore.InitialDiagnosticScore);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticScore))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `final_diagnostic_score` ");
                        values = String.Concat(values, ", @finalDiagnosticScore");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `final_diagnostic_score` ");
                        values = String.Concat(values, " @finalDiagnosticScore");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@finalDiagnosticScore", learningGapDiagnosticYearScore.FinalDiagnosticScore);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticYearScore.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticYearScore.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticYearScore.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                learningGapDiagnosticYearScore.Id = cmd.LastInsertedId.ToString();
                response.Add(learningGapDiagnosticYearScore);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticYearScoreModel>();
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
        public List<LearningGapDiagnosticYearScoreModel> Create(string connectionString, List<LearningGapDiagnosticYearScoreModel> learningGapDiagnosticYearScoreList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticYearScoreModel> response = new List<LearningGapDiagnosticYearScoreModel>();
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
                        for (int i = 0; i < learningGapDiagnosticYearScoreList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO learning_gap_diagnostic_year_scores (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticYearScoreList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].AcademicYearId))
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
                                    cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapDiagnosticYearScoreList[i].AcademicYearId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].StudentId))
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
                                    cmd.Parameters.AddWithValue("@studentId" + i, learningGapDiagnosticYearScoreList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].SubjectCode))
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
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapDiagnosticYearScoreList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].LevelCode))
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
                                    cmd.Parameters.AddWithValue("@levelCode" + i, learningGapDiagnosticYearScoreList[i].LevelCode);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].InitialDiagnosticResultId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `initial_diagnostic_result_id` ");
                                        values = String.Concat(values, ", @initialDiagnosticResultId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `initial_diagnostic_result_id` ");
                                        values = String.Concat(values, "@initialDiagnosticResultId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@initialDiagnosticResultId" + i, learningGapDiagnosticYearScoreList[i].InitialDiagnosticResultId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].InitialDiagnosticDateLimit))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `initial_diagnostic_date_limit` ");
                                        values = String.Concat(values, ", @initialDiagnosticDateLimit" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `initial_diagnostic_date_limit` ");
                                        values = String.Concat(values, "@initialDiagnosticDateLimit" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@initialDiagnosticDateLimit" + i, learningGapDiagnosticYearScoreList[i].InitialDiagnosticDateLimit);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].FinalDiagnosticResultId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `final_diagnostic_result_id` ");
                                        values = String.Concat(values, ", @finalDiagnosticResultId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `final_diagnostic_result_id` ");
                                        values = String.Concat(values, "@finalDiagnosticResultId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@finalDiagnosticResultId" + i, learningGapDiagnosticYearScoreList[i].FinalDiagnosticResultId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].FinalDiagnosticDateLimit))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `final_diagnostic_date_limit` ");
                                        values = String.Concat(values, ", @finalDiagnosticDateLimit" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `final_diagnostic_date_limit` ");
                                        values = String.Concat(values, "@finalDiagnosticDateLimit" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@finalDiagnosticDateLimit" + i, learningGapDiagnosticYearScoreList[i].FinalDiagnosticDateLimit);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].InitialDiagnosticScore))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `initial_diagnostic_score` ");
                                        values = String.Concat(values, ", @initialDiagnosticScore" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `initial_diagnostic_score` ");
                                        values = String.Concat(values, "@initialDiagnosticScore" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@initialDiagnosticScore" + i, learningGapDiagnosticYearScoreList[i].InitialDiagnosticScore);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].FinalDiagnosticScore))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `final_diagnostic_score` ");
                                        values = String.Concat(values, ", @finalDiagnosticScore" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `final_diagnostic_score` ");
                                        values = String.Concat(values, "@finalDiagnosticScore" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@finalDiagnosticScore" + i, learningGapDiagnosticYearScoreList[i].FinalDiagnosticScore);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticYearScoreList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticYearScoreList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticYearScoreList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(learningGapDiagnosticYearScoreList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapDiagnosticYearScoreModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapDiagnosticYearScoreModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticYearScoreModel>();
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

        public List<LearningGapDiagnosticYearScoreModel> Update(string connectionString, LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScore, LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScoreWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticYearScoreModel> response = new List<LearningGapDiagnosticYearScoreModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE learning_gap_diagnostic_year_scores SET ";
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", learningGapDiagnosticYearScore.Id);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.AcademicYearId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `academic_year_id` = @academicYearId");
                    else
                        sql = String.Concat(sql, " `academic_year_id` = @academicYearId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@academicYearId", learningGapDiagnosticYearScore.AcademicYearId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.StudentId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `student_id` = @studentId");
                    else
                        sql = String.Concat(sql, " `student_id` = @studentId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@studentId", learningGapDiagnosticYearScore.StudentId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.SubjectCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `subject_code` = @subjectCode");
                    else
                        sql = String.Concat(sql, " `subject_code` = @subjectCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectCode", learningGapDiagnosticYearScore.SubjectCode);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.LevelCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `level_code` = @levelCode");
                    else
                        sql = String.Concat(sql, " `level_code` = @levelCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@levelCode", learningGapDiagnosticYearScore.LevelCode);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticResultId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `initial_diagnostic_result_id` = @initialDiagnosticResultId");
                    else
                        sql = String.Concat(sql, " `initial_diagnostic_result_id` = @initialDiagnosticResultId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@initialDiagnosticResultId", learningGapDiagnosticYearScore.InitialDiagnosticResultId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticDateLimit))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit");
                    else
                        sql = String.Concat(sql, " `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@initialDiagnosticDateLimit", learningGapDiagnosticYearScore.InitialDiagnosticDateLimit);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticResultId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `final_diagnostic_result_id` = @finalDiagnosticResultId");
                    else
                        sql = String.Concat(sql, " `final_diagnostic_result_id` = @finalDiagnosticResultId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@finalDiagnosticResultId", learningGapDiagnosticYearScore.FinalDiagnosticResultId);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticDateLimit))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `final_diagnostic_date_limit` = @finalDiagnosticDateLimit");
                    else
                        sql = String.Concat(sql, " `final_diagnostic_date_limit` = @finalDiagnosticDateLimit");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@finalDiagnosticDateLimit", learningGapDiagnosticYearScore.FinalDiagnosticDateLimit);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticScore))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `initial_diagnostic_score` = @initialDiagnosticScore");
                    else
                        sql = String.Concat(sql, " `initial_diagnostic_score` = @initialDiagnosticScore");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@initialDiagnosticScore", learningGapDiagnosticYearScore.InitialDiagnosticScore);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticScore))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `final_diagnostic_score` = @finalDiagnosticScore");
                    else
                        sql = String.Concat(sql, " `final_diagnostic_score` = @finalDiagnosticScore");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@finalDiagnosticScore", learningGapDiagnosticYearScore.FinalDiagnosticScore);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticYearScore.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticYearScore.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticYearScore.UpdatedAt);
                }
                if (learningGapDiagnosticYearScoreWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", learningGapDiagnosticYearScoreWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere.AcademicYearId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearIdWhere");
                        else
                            sql = String.Concat(sql, " `academic_year_id` = @academicYearIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@academicYearIdWhere", learningGapDiagnosticYearScoreWhere.AcademicYearId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentIdWhere", learningGapDiagnosticYearScoreWhere.StudentId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCodeWhere", learningGapDiagnosticYearScoreWhere.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere.LevelCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `level_code` = @levelCodeWhere");
                        else
                            sql = String.Concat(sql, " `level_code` = @levelCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelCodeWhere", learningGapDiagnosticYearScoreWhere.LevelCode);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere.InitialDiagnosticResultId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `initial_diagnostic_result_id` = @initialDiagnosticResultIdWhere");
                        else
                            sql = String.Concat(sql, " `initial_diagnostic_result_id` = @initialDiagnosticResultIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@initialDiagnosticResultIdWhere", learningGapDiagnosticYearScoreWhere.InitialDiagnosticResultId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere.InitialDiagnosticDateLimit))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `initial_diagnostic_date_limit` = @initialDiagnosticDateLimitWhere");
                        else
                            sql = String.Concat(sql, " `initial_diagnostic_date_limit` = @initialDiagnosticDateLimitWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@initialDiagnosticDateLimitWhere", learningGapDiagnosticYearScoreWhere.InitialDiagnosticDateLimit);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere.FinalDiagnosticResultId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `final_diagnostic_result_id` = @finalDiagnosticResultIdWhere");
                        else
                            sql = String.Concat(sql, " `final_diagnostic_result_id` = @finalDiagnosticResultIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@finalDiagnosticResultIdWhere", learningGapDiagnosticYearScoreWhere.FinalDiagnosticResultId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere.FinalDiagnosticDateLimit))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `final_diagnostic_date_limit` = @finalDiagnosticDateLimitWhere");
                        else
                            sql = String.Concat(sql, " `final_diagnostic_date_limit` = @finalDiagnosticDateLimitWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@finalDiagnosticDateLimitWhere", learningGapDiagnosticYearScoreWhere.FinalDiagnosticDateLimit);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere.InitialDiagnosticScore))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `initial_diagnostic_score` = @initialDiagnosticScoreWhere");
                        else
                            sql = String.Concat(sql, " `initial_diagnostic_score` = @initialDiagnosticScoreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@initialDiagnosticScoreWhere", learningGapDiagnosticYearScoreWhere.InitialDiagnosticScore);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere.FinalDiagnosticScore))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `final_diagnostic_score` = @finalDiagnosticScoreWhere");
                        else
                            sql = String.Concat(sql, " `final_diagnostic_score` = @finalDiagnosticScoreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@finalDiagnosticScoreWhere", learningGapDiagnosticYearScoreWhere.FinalDiagnosticScore);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", learningGapDiagnosticYearScoreWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", learningGapDiagnosticYearScoreWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", learningGapDiagnosticYearScoreWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(learningGapDiagnosticYearScore);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticYearScoreModel>();
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

        public List<LearningGapDiagnosticYearScoreModel> Update(string connectionString, List<LearningGapDiagnosticYearScoreModel> learningGapDiagnosticYearScoreList, List<LearningGapDiagnosticYearScoreModel> learningGapDiagnosticYearScoreWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticYearScoreModel> response = new List<LearningGapDiagnosticYearScoreModel>();
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
                        for (int i = 0; i < learningGapDiagnosticYearScoreList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE learning_gap_diagnostic_year_scores SET ";
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticYearScoreList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].AcademicYearId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `academic_year_id` = @academicYearId" + i);
                                    else
                                        sql = String.Concat(sql, " `academic_year_id` = @academicYearId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapDiagnosticYearScoreList[i].AcademicYearId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].StudentId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `student_id` = @studentId" + i);
                                    else
                                        sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@studentId" + i, learningGapDiagnosticYearScoreList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].SubjectCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `subject_code` = @subjectCode" + i);
                                    else
                                        sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapDiagnosticYearScoreList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].LevelCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `level_code` = @levelCode" + i);
                                    else
                                        sql = String.Concat(sql, " `level_code` = @levelCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@levelCode" + i, learningGapDiagnosticYearScoreList[i].LevelCode);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].InitialDiagnosticResultId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `initial_diagnostic_result_id` = @initialDiagnosticResultId" + i);
                                    else
                                        sql = String.Concat(sql, " `initial_diagnostic_result_id` = @initialDiagnosticResultId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@initialDiagnosticResultId" + i, learningGapDiagnosticYearScoreList[i].InitialDiagnosticResultId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].InitialDiagnosticDateLimit))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit" + i);
                                    else
                                        sql = String.Concat(sql, " `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@initialDiagnosticDateLimit" + i, learningGapDiagnosticYearScoreList[i].InitialDiagnosticDateLimit);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].FinalDiagnosticResultId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `final_diagnostic_result_id` = @finalDiagnosticResultId" + i);
                                    else
                                        sql = String.Concat(sql, " `final_diagnostic_result_id` = @finalDiagnosticResultId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@finalDiagnosticResultId" + i, learningGapDiagnosticYearScoreList[i].FinalDiagnosticResultId);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].FinalDiagnosticDateLimit))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `final_diagnostic_date_limit` = @finalDiagnosticDateLimit" + i);
                                    else
                                        sql = String.Concat(sql, " `final_diagnostic_date_limit` = @finalDiagnosticDateLimit" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@finalDiagnosticDateLimit" + i, learningGapDiagnosticYearScoreList[i].FinalDiagnosticDateLimit);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].InitialDiagnosticScore))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `initial_diagnostic_score` = @initialDiagnosticScore" + i);
                                    else
                                        sql = String.Concat(sql, " `initial_diagnostic_score` = @initialDiagnosticScore" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@initialDiagnosticScore" + i, learningGapDiagnosticYearScoreList[i].InitialDiagnosticScore);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].FinalDiagnosticScore))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `final_diagnostic_score` = @finalDiagnosticScore" + i);
                                    else
                                        sql = String.Concat(sql, " `final_diagnostic_score` = @finalDiagnosticScore" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@finalDiagnosticScore" + i, learningGapDiagnosticYearScoreList[i].FinalDiagnosticScore);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticYearScoreList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticYearScoreList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticYearScoreList[i].UpdatedAt);
                                }
                                if (learningGapDiagnosticYearScoreWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, learningGapDiagnosticYearScoreWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereList[i].AcademicYearId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `academic_year_id` = @academicYearIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@academicYearIdWhere" + i, learningGapDiagnosticYearScoreWhereList[i].AcademicYearId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereList[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentIdWhere" + i, learningGapDiagnosticYearScoreWhereList[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereList[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCodeWhere" + i, learningGapDiagnosticYearScoreWhereList[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereList[i].LevelCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `level_code` = @levelCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `level_code` = @levelCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelCodeWhere" + i, learningGapDiagnosticYearScoreWhereList[i].LevelCode);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereList[i].InitialDiagnosticResultId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `initial_diagnostic_result_id` = @initialDiagnosticResultIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `initial_diagnostic_result_id` = @initialDiagnosticResultIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@initialDiagnosticResultIdWhere" + i, learningGapDiagnosticYearScoreWhereList[i].InitialDiagnosticResultId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereList[i].InitialDiagnosticDateLimit))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `initial_diagnostic_date_limit` = @initialDiagnosticDateLimitWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `initial_diagnostic_date_limit` = @initialDiagnosticDateLimitWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@initialDiagnosticDateLimitWhere" + i, learningGapDiagnosticYearScoreWhereList[i].InitialDiagnosticDateLimit);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereList[i].FinalDiagnosticResultId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `final_diagnostic_result_id` = @finalDiagnosticResultIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `final_diagnostic_result_id` = @finalDiagnosticResultIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@finalDiagnosticResultIdWhere" + i, learningGapDiagnosticYearScoreWhereList[i].FinalDiagnosticResultId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereList[i].FinalDiagnosticDateLimit))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `final_diagnostic_date_limit` = @finalDiagnosticDateLimitWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `final_diagnostic_date_limit` = @finalDiagnosticDateLimitWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@finalDiagnosticDateLimitWhere" + i, learningGapDiagnosticYearScoreWhereList[i].FinalDiagnosticDateLimit);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereList[i].InitialDiagnosticScore))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `initial_diagnostic_score` = @initialDiagnosticScoreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `initial_diagnostic_score` = @initialDiagnosticScoreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@initialDiagnosticScoreWhere" + i, learningGapDiagnosticYearScoreWhereList[i].InitialDiagnosticScore);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereList[i].FinalDiagnosticScore))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `final_diagnostic_score` = @finalDiagnosticScoreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `final_diagnostic_score` = @finalDiagnosticScoreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@finalDiagnosticScoreWhere" + i, learningGapDiagnosticYearScoreWhereList[i].FinalDiagnosticScore);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, learningGapDiagnosticYearScoreWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, learningGapDiagnosticYearScoreWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScoreWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, learningGapDiagnosticYearScoreWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapDiagnosticYearScoreList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapDiagnosticYearScoreModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapDiagnosticYearScoreModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticYearScoreModel>();
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

        public List<LearningGapDiagnosticYearScoreModel> Delete(string connectionString, LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScore)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticYearScoreModel> response = new List<LearningGapDiagnosticYearScoreModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM learning_gap_diagnostic_year_scores  ";
                if (learningGapDiagnosticYearScore != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", learningGapDiagnosticYearScore.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.AcademicYearId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearId");
                        else
                            sql = String.Concat(sql, " `academic_year_id` = @academicYearId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@academicYearId", learningGapDiagnosticYearScore.AcademicYearId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentId");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentId", learningGapDiagnosticYearScore.StudentId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCode", learningGapDiagnosticYearScore.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.LevelCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `level_code` = @levelCode");
                        else
                            sql = String.Concat(sql, " `level_code` = @levelCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelCode", learningGapDiagnosticYearScore.LevelCode);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticResultId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `initial_diagnostic_result_id` = @initialDiagnosticResultId");
                        else
                            sql = String.Concat(sql, " `initial_diagnostic_result_id` = @initialDiagnosticResultId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@initialDiagnosticResultId", learningGapDiagnosticYearScore.InitialDiagnosticResultId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticDateLimit))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit");
                        else
                            sql = String.Concat(sql, " `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@initialDiagnosticDateLimit", learningGapDiagnosticYearScore.InitialDiagnosticDateLimit);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticResultId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `final_diagnostic_result_id` = @finalDiagnosticResultId");
                        else
                            sql = String.Concat(sql, " `final_diagnostic_result_id` = @finalDiagnosticResultId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@finalDiagnosticResultId", learningGapDiagnosticYearScore.FinalDiagnosticResultId);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticDateLimit))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `final_diagnostic_date_limit` = @finalDiagnosticDateLimit");
                        else
                            sql = String.Concat(sql, " `final_diagnostic_date_limit` = @finalDiagnosticDateLimit");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@finalDiagnosticDateLimit", learningGapDiagnosticYearScore.FinalDiagnosticDateLimit);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.InitialDiagnosticScore))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `initial_diagnostic_score` = @initialDiagnosticScore");
                        else
                            sql = String.Concat(sql, " `initial_diagnostic_score` = @initialDiagnosticScore");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@initialDiagnosticScore", learningGapDiagnosticYearScore.InitialDiagnosticScore);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.FinalDiagnosticScore))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `final_diagnostic_score` = @finalDiagnosticScore");
                        else
                            sql = String.Concat(sql, " `final_diagnostic_score` = @finalDiagnosticScore");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@finalDiagnosticScore", learningGapDiagnosticYearScore.FinalDiagnosticScore);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", learningGapDiagnosticYearScore.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", learningGapDiagnosticYearScore.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", learningGapDiagnosticYearScore.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(learningGapDiagnosticYearScore);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticYearScoreModel>();
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

        public List<LearningGapDiagnosticYearScoreModel> Delete(string connectionString, List<LearningGapDiagnosticYearScoreModel> learningGapDiagnosticYearScore)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapDiagnosticYearScoreModel> response = new List<LearningGapDiagnosticYearScoreModel>();
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
                        for (int i = 0; i < learningGapDiagnosticYearScore.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM learning_gap_diagnostic_year_scores  ";
                                if (learningGapDiagnosticYearScore[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, learningGapDiagnosticYearScore[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore[i].AcademicYearId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearId" + i);
                                        else
                                            sql = String.Concat(sql, " `academic_year_id` = @academicYearId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapDiagnosticYearScore[i].AcademicYearId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentId" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentId" + i, learningGapDiagnosticYearScore[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapDiagnosticYearScore[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore[i].LevelCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `level_code` = @levelCode" + i);
                                        else
                                            sql = String.Concat(sql, " `level_code` = @levelCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelCode" + i, learningGapDiagnosticYearScore[i].LevelCode);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore[i].InitialDiagnosticResultId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `initial_diagnostic_result_id` = @initialDiagnosticResultId" + i);
                                        else
                                            sql = String.Concat(sql, " `initial_diagnostic_result_id` = @initialDiagnosticResultId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@initialDiagnosticResultId" + i, learningGapDiagnosticYearScore[i].InitialDiagnosticResultId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore[i].InitialDiagnosticDateLimit))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit" + i);
                                        else
                                            sql = String.Concat(sql, " `initial_diagnostic_date_limit` = @initialDiagnosticDateLimit" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@initialDiagnosticDateLimit" + i, learningGapDiagnosticYearScore[i].InitialDiagnosticDateLimit);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore[i].FinalDiagnosticResultId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `final_diagnostic_result_id` = @finalDiagnosticResultId" + i);
                                        else
                                            sql = String.Concat(sql, " `final_diagnostic_result_id` = @finalDiagnosticResultId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@finalDiagnosticResultId" + i, learningGapDiagnosticYearScore[i].FinalDiagnosticResultId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore[i].FinalDiagnosticDateLimit))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `final_diagnostic_date_limit` = @finalDiagnosticDateLimit" + i);
                                        else
                                            sql = String.Concat(sql, " `final_diagnostic_date_limit` = @finalDiagnosticDateLimit" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@finalDiagnosticDateLimit" + i, learningGapDiagnosticYearScore[i].FinalDiagnosticDateLimit);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore[i].InitialDiagnosticScore))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `initial_diagnostic_score` = @initialDiagnosticScore" + i);
                                        else
                                            sql = String.Concat(sql, " `initial_diagnostic_score` = @initialDiagnosticScore" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@initialDiagnosticScore" + i, learningGapDiagnosticYearScore[i].InitialDiagnosticScore);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore[i].FinalDiagnosticScore))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `final_diagnostic_score` = @finalDiagnosticScore" + i);
                                        else
                                            sql = String.Concat(sql, " `final_diagnostic_score` = @finalDiagnosticScore" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@finalDiagnosticScore" + i, learningGapDiagnosticYearScore[i].FinalDiagnosticScore);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapDiagnosticYearScore[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, learningGapDiagnosticYearScore[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapDiagnosticYearScore[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapDiagnosticYearScore[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapDiagnosticYearScore[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapDiagnosticYearScoreModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapDiagnosticYearScoreModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapDiagnosticYearScoreModel>();
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
