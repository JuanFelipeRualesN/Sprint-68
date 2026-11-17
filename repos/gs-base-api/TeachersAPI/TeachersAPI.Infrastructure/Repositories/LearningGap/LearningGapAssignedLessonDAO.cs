using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.LearningGap;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.LearningGap
{
    class LearningGapAssignedLessonDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, LearningGapAssignedLessonModel> ReadD(string connectionString, LearningGapAssignedLessonModel learningGapAssignedLesson, bool firstOne, bool orderByLastDate, LearningGapAssignedLessonModel learningGapAssignedLessonWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapAssignedLessonModel> response = new Dictionary<string, LearningGapAssignedLessonModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `academic_year_id`, `max_tries`, `teacher_id`, `limit_date`, `lesson_id`, `subject_code`, `student_id`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_assigned_lessons ";
                {
                    if (learningGapAssignedLesson != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapAssignedLesson.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapAssignedLesson.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.MaxTries))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `max_tries` = @maxTries ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `max_tries` = @maxTries ");
                            }
                            cmd.Parameters.AddWithValue("@maxTries", learningGapAssignedLesson.MaxTries);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.TeacherId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teacher_id` = @teacherId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teacher_id` = @teacherId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherId", learningGapAssignedLesson.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.LimitDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `limit_date` = @limitDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `limit_date` = @limitDate ");
                            }
                            cmd.Parameters.AddWithValue("@limitDate", learningGapAssignedLesson.LimitDate);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", learningGapAssignedLesson.LessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapAssignedLesson.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapAssignedLesson.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", learningGapAssignedLesson.Completed);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapAssignedLesson.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapAssignedLesson.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapAssignedLesson.UpdatedAt);
                        }
                    }
                    if (learningGapAssignedLessonWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapAssignedLessonWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapAssignedLessonWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.MaxTries))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `max_tries` LIKE @maxTriesWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `max_tries` LIKE @maxTriesWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@maxTriesWhereLike", "%" + learningGapAssignedLessonWhereLike.MaxTries + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.TeacherId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teacher_id` LIKE @teacherIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teacher_id` LIKE @teacherIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + learningGapAssignedLessonWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.LimitDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `limit_date` LIKE @limitDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `limit_date` LIKE @limitDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@limitDateWhereLike", "%" + learningGapAssignedLessonWhereLike.LimitDate + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + learningGapAssignedLessonWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapAssignedLessonWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapAssignedLessonWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.Completed))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `completed` LIKE @completedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `completed` LIKE @completedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@completedWhereLike", "%" + learningGapAssignedLessonWhereLike.Completed + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapAssignedLessonWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapAssignedLessonWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapAssignedLessonWhereLike.UpdatedAt + "%");
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
                                learningGapAssignedLesson = new LearningGapAssignedLessonModel();
                                if (!reader.IsDBNull(0))
                                    learningGapAssignedLesson.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapAssignedLesson.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapAssignedLesson.AcademicYearId = reader.GetValue(1).ToString();
                                else
                                    learningGapAssignedLesson.AcademicYearId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapAssignedLesson.MaxTries = reader.GetValue(2).ToString();
                                else
                                    learningGapAssignedLesson.MaxTries = null;
                                if (!reader.IsDBNull(3))
                                    learningGapAssignedLesson.TeacherId = reader.GetValue(3).ToString();
                                else
                                    learningGapAssignedLesson.TeacherId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapAssignedLesson.LimitDate = reader.GetValue(4).ToString();
                                else
                                    learningGapAssignedLesson.LimitDate = null;
                                if (!reader.IsDBNull(5))
                                    learningGapAssignedLesson.LessonId = reader.GetValue(5).ToString();
                                else
                                    learningGapAssignedLesson.LessonId = null;
                                if (!reader.IsDBNull(6))
                                    learningGapAssignedLesson.SubjectCode = reader.GetValue(6).ToString();
                                else
                                    learningGapAssignedLesson.SubjectCode = null;
                                if (!reader.IsDBNull(7))
                                    learningGapAssignedLesson.StudentId = reader.GetValue(7).ToString();
                                else
                                    learningGapAssignedLesson.StudentId = null;
                                if (!reader.IsDBNull(8))
                                    learningGapAssignedLesson.Completed = reader.GetValue(8).ToString();
                                else
                                    learningGapAssignedLesson.Completed = null;
                                if (!reader.IsDBNull(9))
                                    learningGapAssignedLesson.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    learningGapAssignedLesson.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    learningGapAssignedLesson.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    learningGapAssignedLesson.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    learningGapAssignedLesson.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    learningGapAssignedLesson.UpdatedAt = null;
                                response.Add(learningGapAssignedLesson.Id, learningGapAssignedLesson);
                                learningGapAssignedLesson = new LearningGapAssignedLessonModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapAssignedLessonModel>();
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
                response = new Dictionary<string, LearningGapAssignedLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapAssignedLessonModel> ReadD(string connectionString, LearningGapAssignedLessonModel learningGapAssignedLesson, bool orderByLastDate, int fromRow, int limit, LearningGapAssignedLessonModel learningGapAssignedLessonWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapAssignedLessonModel> response = new Dictionary<string, LearningGapAssignedLessonModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `academic_year_id`, `max_tries`, `teacher_id`, `limit_date`, `lesson_id`, `subject_code`, `student_id`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_assigned_lessons ";
                {
                    if (learningGapAssignedLesson != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapAssignedLesson.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapAssignedLesson.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.MaxTries))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `max_tries` = @maxTries ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `max_tries` = @maxTries ");
                            }
                            cmd.Parameters.AddWithValue("@maxTries", learningGapAssignedLesson.MaxTries);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.TeacherId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teacher_id` = @teacherId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teacher_id` = @teacherId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherId", learningGapAssignedLesson.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.LimitDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `limit_date` = @limitDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `limit_date` = @limitDate ");
                            }
                            cmd.Parameters.AddWithValue("@limitDate", learningGapAssignedLesson.LimitDate);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", learningGapAssignedLesson.LessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapAssignedLesson.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapAssignedLesson.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", learningGapAssignedLesson.Completed);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapAssignedLesson.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapAssignedLesson.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapAssignedLesson.UpdatedAt);
                        }
                    }
                    if (learningGapAssignedLessonWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapAssignedLessonWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapAssignedLessonWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.MaxTries))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `max_tries` LIKE @maxTriesWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `max_tries` LIKE @maxTriesWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@maxTriesWhereLike", "%" + learningGapAssignedLessonWhereLike.MaxTries + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.TeacherId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teacher_id` LIKE @teacherIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teacher_id` LIKE @teacherIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + learningGapAssignedLessonWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.LimitDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `limit_date` LIKE @limitDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `limit_date` LIKE @limitDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@limitDateWhereLike", "%" + learningGapAssignedLessonWhereLike.LimitDate + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + learningGapAssignedLessonWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapAssignedLessonWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapAssignedLessonWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.Completed))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `completed` LIKE @completedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `completed` LIKE @completedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@completedWhereLike", "%" + learningGapAssignedLessonWhereLike.Completed + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapAssignedLessonWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapAssignedLessonWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapAssignedLessonWhereLike.UpdatedAt + "%");
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
                                learningGapAssignedLesson = new LearningGapAssignedLessonModel();
                                if (!reader.IsDBNull(0))
                                    learningGapAssignedLesson.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapAssignedLesson.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapAssignedLesson.AcademicYearId = reader.GetValue(1).ToString();
                                else
                                    learningGapAssignedLesson.AcademicYearId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapAssignedLesson.MaxTries = reader.GetValue(2).ToString();
                                else
                                    learningGapAssignedLesson.MaxTries = null;
                                if (!reader.IsDBNull(3))
                                    learningGapAssignedLesson.TeacherId = reader.GetValue(3).ToString();
                                else
                                    learningGapAssignedLesson.TeacherId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapAssignedLesson.LimitDate = reader.GetValue(4).ToString();
                                else
                                    learningGapAssignedLesson.LimitDate = null;
                                if (!reader.IsDBNull(5))
                                    learningGapAssignedLesson.LessonId = reader.GetValue(5).ToString();
                                else
                                    learningGapAssignedLesson.LessonId = null;
                                if (!reader.IsDBNull(6))
                                    learningGapAssignedLesson.SubjectCode = reader.GetValue(6).ToString();
                                else
                                    learningGapAssignedLesson.SubjectCode = null;
                                if (!reader.IsDBNull(7))
                                    learningGapAssignedLesson.StudentId = reader.GetValue(7).ToString();
                                else
                                    learningGapAssignedLesson.StudentId = null;
                                if (!reader.IsDBNull(8))
                                    learningGapAssignedLesson.Completed = reader.GetValue(8).ToString();
                                else
                                    learningGapAssignedLesson.Completed = null;
                                if (!reader.IsDBNull(9))
                                    learningGapAssignedLesson.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    learningGapAssignedLesson.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    learningGapAssignedLesson.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    learningGapAssignedLesson.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    learningGapAssignedLesson.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    learningGapAssignedLesson.UpdatedAt = null;
                                response.Add(learningGapAssignedLesson.Id, learningGapAssignedLesson);
                                learningGapAssignedLesson = new LearningGapAssignedLessonModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapAssignedLessonModel>();
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
                response = new Dictionary<string, LearningGapAssignedLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapAssignedLessonModel> ReadD(string connectionString, List<LearningGapAssignedLessonModel> learningGapAssignedLessonWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapAssignedLessonModel> response = new Dictionary<string, LearningGapAssignedLessonModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `academic_year_id`, `max_tries`, `teacher_id`, `limit_date`, `lesson_id`, `subject_code`, `student_id`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_assigned_lessons ";
                if (learningGapAssignedLessonWhere != null && learningGapAssignedLessonWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapAssignedLessonWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapAssignedLessonWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAssignedLessonWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].MaxTries))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_tries` = @maxTries" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxTries" + i, learningGapAssignedLessonWhere[i].MaxTries);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, learningGapAssignedLessonWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].LimitDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `limit_date` = @limitDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@limitDate" + i, learningGapAssignedLessonWhere[i].LimitDate);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, learningGapAssignedLessonWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapAssignedLessonWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapAssignedLessonWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].Completed))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `completed` = @completed" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@completed" + i, learningGapAssignedLessonWhere[i].Completed);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAssignedLessonWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAssignedLessonWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAssignedLessonWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapAssignedLessonWhere.Count - 1)
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
                            LearningGapAssignedLessonModel learningGapAssignedLesson = new LearningGapAssignedLessonModel(); if (!reader.IsDBNull(0))
                                learningGapAssignedLesson.Id = reader.GetValue(0).ToString();
                            else
                                learningGapAssignedLesson.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapAssignedLesson.AcademicYearId = reader.GetValue(1).ToString();
                            else
                                learningGapAssignedLesson.AcademicYearId = null;
                            if (!reader.IsDBNull(2))
                                learningGapAssignedLesson.MaxTries = reader.GetValue(2).ToString();
                            else
                                learningGapAssignedLesson.MaxTries = null;
                            if (!reader.IsDBNull(3))
                                learningGapAssignedLesson.TeacherId = reader.GetValue(3).ToString();
                            else
                                learningGapAssignedLesson.TeacherId = null;
                            if (!reader.IsDBNull(4))
                                learningGapAssignedLesson.LimitDate = reader.GetValue(4).ToString();
                            else
                                learningGapAssignedLesson.LimitDate = null;
                            if (!reader.IsDBNull(5))
                                learningGapAssignedLesson.LessonId = reader.GetValue(5).ToString();
                            else
                                learningGapAssignedLesson.LessonId = null;
                            if (!reader.IsDBNull(6))
                                learningGapAssignedLesson.SubjectCode = reader.GetValue(6).ToString();
                            else
                                learningGapAssignedLesson.SubjectCode = null;
                            if (!reader.IsDBNull(7))
                                learningGapAssignedLesson.StudentId = reader.GetValue(7).ToString();
                            else
                                learningGapAssignedLesson.StudentId = null;
                            if (!reader.IsDBNull(8))
                                learningGapAssignedLesson.Completed = reader.GetValue(8).ToString();
                            else
                                learningGapAssignedLesson.Completed = null;
                            if (!reader.IsDBNull(9))
                                learningGapAssignedLesson.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                learningGapAssignedLesson.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                learningGapAssignedLesson.CreatedAt = reader.GetValue(10).ToString();
                            else
                                learningGapAssignedLesson.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                learningGapAssignedLesson.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                learningGapAssignedLesson.UpdatedAt = null;
                            response.Add(learningGapAssignedLesson.Id, learningGapAssignedLesson);
                            learningGapAssignedLesson = new LearningGapAssignedLessonModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapAssignedLessonModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapAssignedLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapAssignedLessonModel> ReadD(string connectionString, List<LearningGapAssignedLessonModel> learningGapAssignedLessonWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapAssignedLessonModel> response = new Dictionary<string, LearningGapAssignedLessonModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `academic_year_id`, `max_tries`, `teacher_id`, `limit_date`, `lesson_id`, `subject_code`, `student_id`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_assigned_lessons ";
                if (learningGapAssignedLessonWhere != null && learningGapAssignedLessonWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapAssignedLessonWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapAssignedLessonWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAssignedLessonWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].MaxTries))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_tries` = @maxTries" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxTries" + i, learningGapAssignedLessonWhere[i].MaxTries);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, learningGapAssignedLessonWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].LimitDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `limit_date` = @limitDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@limitDate" + i, learningGapAssignedLessonWhere[i].LimitDate);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, learningGapAssignedLessonWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapAssignedLessonWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapAssignedLessonWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].Completed))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `completed` = @completed" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@completed" + i, learningGapAssignedLessonWhere[i].Completed);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAssignedLessonWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAssignedLessonWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAssignedLessonWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapAssignedLessonWhere.Count - 1)
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
                            LearningGapAssignedLessonModel learningGapAssignedLesson = new LearningGapAssignedLessonModel(); if (!reader.IsDBNull(0))
                                learningGapAssignedLesson.Id = reader.GetValue(0).ToString();
                            else
                                learningGapAssignedLesson.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapAssignedLesson.AcademicYearId = reader.GetValue(1).ToString();
                            else
                                learningGapAssignedLesson.AcademicYearId = null;
                            if (!reader.IsDBNull(2))
                                learningGapAssignedLesson.MaxTries = reader.GetValue(2).ToString();
                            else
                                learningGapAssignedLesson.MaxTries = null;
                            if (!reader.IsDBNull(3))
                                learningGapAssignedLesson.TeacherId = reader.GetValue(3).ToString();
                            else
                                learningGapAssignedLesson.TeacherId = null;
                            if (!reader.IsDBNull(4))
                                learningGapAssignedLesson.LimitDate = reader.GetValue(4).ToString();
                            else
                                learningGapAssignedLesson.LimitDate = null;
                            if (!reader.IsDBNull(5))
                                learningGapAssignedLesson.LessonId = reader.GetValue(5).ToString();
                            else
                                learningGapAssignedLesson.LessonId = null;
                            if (!reader.IsDBNull(6))
                                learningGapAssignedLesson.SubjectCode = reader.GetValue(6).ToString();
                            else
                                learningGapAssignedLesson.SubjectCode = null;
                            if (!reader.IsDBNull(7))
                                learningGapAssignedLesson.StudentId = reader.GetValue(7).ToString();
                            else
                                learningGapAssignedLesson.StudentId = null;
                            if (!reader.IsDBNull(8))
                                learningGapAssignedLesson.Completed = reader.GetValue(8).ToString();
                            else
                                learningGapAssignedLesson.Completed = null;
                            if (!reader.IsDBNull(9))
                                learningGapAssignedLesson.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                learningGapAssignedLesson.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                learningGapAssignedLesson.CreatedAt = reader.GetValue(10).ToString();
                            else
                                learningGapAssignedLesson.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                learningGapAssignedLesson.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                learningGapAssignedLesson.UpdatedAt = null;
                            response.Add(learningGapAssignedLesson.Id, learningGapAssignedLesson);
                            learningGapAssignedLesson = new LearningGapAssignedLessonModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapAssignedLessonModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapAssignedLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapAssignedLessonModel> Read(string connectionString, LearningGapAssignedLessonModel learningGapAssignedLesson, bool firstOne, bool orderByLastDate, LearningGapAssignedLessonModel learningGapAssignedLessonWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAssignedLessonModel> response = new List<LearningGapAssignedLessonModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `academic_year_id`, `max_tries`, `teacher_id`, `limit_date`, `lesson_id`, `subject_code`, `student_id`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_assigned_lessons ";
                {
                    if (learningGapAssignedLesson != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapAssignedLesson.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapAssignedLesson.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.MaxTries))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `max_tries` = @maxTries ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `max_tries` = @maxTries ");
                            }
                            cmd.Parameters.AddWithValue("@maxTries", learningGapAssignedLesson.MaxTries);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.TeacherId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teacher_id` = @teacherId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teacher_id` = @teacherId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherId", learningGapAssignedLesson.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.LimitDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `limit_date` = @limitDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `limit_date` = @limitDate ");
                            }
                            cmd.Parameters.AddWithValue("@limitDate", learningGapAssignedLesson.LimitDate);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", learningGapAssignedLesson.LessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapAssignedLesson.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapAssignedLesson.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", learningGapAssignedLesson.Completed);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapAssignedLesson.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapAssignedLesson.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapAssignedLesson.UpdatedAt);
                        }
                    }
                    if (learningGapAssignedLessonWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapAssignedLessonWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapAssignedLessonWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.MaxTries))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `max_tries` LIKE @maxTriesWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `max_tries` LIKE @maxTriesWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@maxTriesWhereLike", "%" + learningGapAssignedLessonWhereLike.MaxTries + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.TeacherId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teacher_id` LIKE @teacherIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teacher_id` LIKE @teacherIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + learningGapAssignedLessonWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.LimitDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `limit_date` LIKE @limitDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `limit_date` LIKE @limitDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@limitDateWhereLike", "%" + learningGapAssignedLessonWhereLike.LimitDate + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + learningGapAssignedLessonWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapAssignedLessonWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapAssignedLessonWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.Completed))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `completed` LIKE @completedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `completed` LIKE @completedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@completedWhereLike", "%" + learningGapAssignedLessonWhereLike.Completed + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapAssignedLessonWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapAssignedLessonWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapAssignedLessonWhereLike.UpdatedAt + "%");
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
                                learningGapAssignedLesson = new LearningGapAssignedLessonModel();
                                if (!reader.IsDBNull(0))
                                    learningGapAssignedLesson.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapAssignedLesson.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapAssignedLesson.AcademicYearId = reader.GetValue(1).ToString();
                                else
                                    learningGapAssignedLesson.AcademicYearId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapAssignedLesson.MaxTries = reader.GetValue(2).ToString();
                                else
                                    learningGapAssignedLesson.MaxTries = null;
                                if (!reader.IsDBNull(3))
                                    learningGapAssignedLesson.TeacherId = reader.GetValue(3).ToString();
                                else
                                    learningGapAssignedLesson.TeacherId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapAssignedLesson.LimitDate = reader.GetValue(4).ToString();
                                else
                                    learningGapAssignedLesson.LimitDate = null;
                                if (!reader.IsDBNull(5))
                                    learningGapAssignedLesson.LessonId = reader.GetValue(5).ToString();
                                else
                                    learningGapAssignedLesson.LessonId = null;
                                if (!reader.IsDBNull(6))
                                    learningGapAssignedLesson.SubjectCode = reader.GetValue(6).ToString();
                                else
                                    learningGapAssignedLesson.SubjectCode = null;
                                if (!reader.IsDBNull(7))
                                    learningGapAssignedLesson.StudentId = reader.GetValue(7).ToString();
                                else
                                    learningGapAssignedLesson.StudentId = null;
                                if (!reader.IsDBNull(8))
                                    learningGapAssignedLesson.Completed = reader.GetValue(8).ToString();
                                else
                                    learningGapAssignedLesson.Completed = null;
                                if (!reader.IsDBNull(9))
                                    learningGapAssignedLesson.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    learningGapAssignedLesson.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    learningGapAssignedLesson.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    learningGapAssignedLesson.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    learningGapAssignedLesson.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    learningGapAssignedLesson.UpdatedAt = null;
                                response.Add(learningGapAssignedLesson);
                                learningGapAssignedLesson = new LearningGapAssignedLessonModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapAssignedLessonModel>();
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
                response = new List<LearningGapAssignedLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapAssignedLessonModel> Read(string connectionString, LearningGapAssignedLessonModel learningGapAssignedLesson, bool orderByLastDate, int fromRow, int limit, LearningGapAssignedLessonModel learningGapAssignedLessonWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAssignedLessonModel> response = new List<LearningGapAssignedLessonModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `academic_year_id`, `max_tries`, `teacher_id`, `limit_date`, `lesson_id`, `subject_code`, `student_id`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_assigned_lessons ";
                {
                    if (learningGapAssignedLesson != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapAssignedLesson.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapAssignedLesson.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.MaxTries))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `max_tries` = @maxTries ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `max_tries` = @maxTries ");
                            }
                            cmd.Parameters.AddWithValue("@maxTries", learningGapAssignedLesson.MaxTries);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.TeacherId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teacher_id` = @teacherId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teacher_id` = @teacherId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherId", learningGapAssignedLesson.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.LimitDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `limit_date` = @limitDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `limit_date` = @limitDate ");
                            }
                            cmd.Parameters.AddWithValue("@limitDate", learningGapAssignedLesson.LimitDate);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", learningGapAssignedLesson.LessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapAssignedLesson.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapAssignedLesson.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", learningGapAssignedLesson.Completed);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapAssignedLesson.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapAssignedLesson.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLesson.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapAssignedLesson.UpdatedAt);
                        }
                    }
                    if (learningGapAssignedLessonWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapAssignedLessonWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapAssignedLessonWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.MaxTries))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `max_tries` LIKE @maxTriesWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `max_tries` LIKE @maxTriesWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@maxTriesWhereLike", "%" + learningGapAssignedLessonWhereLike.MaxTries + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.TeacherId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teacher_id` LIKE @teacherIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teacher_id` LIKE @teacherIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + learningGapAssignedLessonWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.LimitDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `limit_date` LIKE @limitDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `limit_date` LIKE @limitDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@limitDateWhereLike", "%" + learningGapAssignedLessonWhereLike.LimitDate + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + learningGapAssignedLessonWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapAssignedLessonWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapAssignedLessonWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.Completed))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `completed` LIKE @completedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `completed` LIKE @completedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@completedWhereLike", "%" + learningGapAssignedLessonWhereLike.Completed + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapAssignedLessonWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapAssignedLessonWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapAssignedLessonWhereLike.UpdatedAt + "%");
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
                                learningGapAssignedLesson = new LearningGapAssignedLessonModel();
                                if (!reader.IsDBNull(0))
                                    learningGapAssignedLesson.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapAssignedLesson.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapAssignedLesson.AcademicYearId = reader.GetValue(1).ToString();
                                else
                                    learningGapAssignedLesson.AcademicYearId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapAssignedLesson.MaxTries = reader.GetValue(2).ToString();
                                else
                                    learningGapAssignedLesson.MaxTries = null;
                                if (!reader.IsDBNull(3))
                                    learningGapAssignedLesson.TeacherId = reader.GetValue(3).ToString();
                                else
                                    learningGapAssignedLesson.TeacherId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapAssignedLesson.LimitDate = reader.GetValue(4).ToString();
                                else
                                    learningGapAssignedLesson.LimitDate = null;
                                if (!reader.IsDBNull(5))
                                    learningGapAssignedLesson.LessonId = reader.GetValue(5).ToString();
                                else
                                    learningGapAssignedLesson.LessonId = null;
                                if (!reader.IsDBNull(6))
                                    learningGapAssignedLesson.SubjectCode = reader.GetValue(6).ToString();
                                else
                                    learningGapAssignedLesson.SubjectCode = null;
                                if (!reader.IsDBNull(7))
                                    learningGapAssignedLesson.StudentId = reader.GetValue(7).ToString();
                                else
                                    learningGapAssignedLesson.StudentId = null;
                                if (!reader.IsDBNull(8))
                                    learningGapAssignedLesson.Completed = reader.GetValue(8).ToString();
                                else
                                    learningGapAssignedLesson.Completed = null;
                                if (!reader.IsDBNull(9))
                                    learningGapAssignedLesson.ModifiedBy = reader.GetValue(9).ToString();
                                else
                                    learningGapAssignedLesson.ModifiedBy = null;
                                if (!reader.IsDBNull(10))
                                    learningGapAssignedLesson.CreatedAt = reader.GetValue(10).ToString();
                                else
                                    learningGapAssignedLesson.CreatedAt = null;
                                if (!reader.IsDBNull(11))
                                    learningGapAssignedLesson.UpdatedAt = reader.GetValue(11).ToString();
                                else
                                    learningGapAssignedLesson.UpdatedAt = null;
                                response.Add(learningGapAssignedLesson);
                                learningGapAssignedLesson = new LearningGapAssignedLessonModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapAssignedLessonModel>();
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
                response = new List<LearningGapAssignedLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapAssignedLessonModel> Read(string connectionString, List<LearningGapAssignedLessonModel> learningGapAssignedLessonWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAssignedLessonModel> response = new List<LearningGapAssignedLessonModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `academic_year_id`, `max_tries`, `teacher_id`, `limit_date`, `lesson_id`, `subject_code`, `student_id`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_assigned_lessons ";
                if (learningGapAssignedLessonWhere != null && learningGapAssignedLessonWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapAssignedLessonWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapAssignedLessonWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAssignedLessonWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].MaxTries))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_tries` = @maxTries" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxTries" + i, learningGapAssignedLessonWhere[i].MaxTries);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, learningGapAssignedLessonWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].LimitDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `limit_date` = @limitDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@limitDate" + i, learningGapAssignedLessonWhere[i].LimitDate);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, learningGapAssignedLessonWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapAssignedLessonWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapAssignedLessonWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].Completed))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `completed` = @completed" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@completed" + i, learningGapAssignedLessonWhere[i].Completed);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAssignedLessonWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAssignedLessonWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAssignedLessonWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapAssignedLessonWhere.Count - 1)
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
                            LearningGapAssignedLessonModel learningGapAssignedLesson = new LearningGapAssignedLessonModel(); if (!reader.IsDBNull(0))
                                learningGapAssignedLesson.Id = reader.GetValue(0).ToString();
                            else
                                learningGapAssignedLesson.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapAssignedLesson.AcademicYearId = reader.GetValue(1).ToString();
                            else
                                learningGapAssignedLesson.AcademicYearId = null;
                            if (!reader.IsDBNull(2))
                                learningGapAssignedLesson.MaxTries = reader.GetValue(2).ToString();
                            else
                                learningGapAssignedLesson.MaxTries = null;
                            if (!reader.IsDBNull(3))
                                learningGapAssignedLesson.TeacherId = reader.GetValue(3).ToString();
                            else
                                learningGapAssignedLesson.TeacherId = null;
                            if (!reader.IsDBNull(4))
                                learningGapAssignedLesson.LimitDate = reader.GetValue(4).ToString();
                            else
                                learningGapAssignedLesson.LimitDate = null;
                            if (!reader.IsDBNull(5))
                                learningGapAssignedLesson.LessonId = reader.GetValue(5).ToString();
                            else
                                learningGapAssignedLesson.LessonId = null;
                            if (!reader.IsDBNull(6))
                                learningGapAssignedLesson.SubjectCode = reader.GetValue(6).ToString();
                            else
                                learningGapAssignedLesson.SubjectCode = null;
                            if (!reader.IsDBNull(7))
                                learningGapAssignedLesson.StudentId = reader.GetValue(7).ToString();
                            else
                                learningGapAssignedLesson.StudentId = null;
                            if (!reader.IsDBNull(8))
                                learningGapAssignedLesson.Completed = reader.GetValue(8).ToString();
                            else
                                learningGapAssignedLesson.Completed = null;
                            if (!reader.IsDBNull(9))
                                learningGapAssignedLesson.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                learningGapAssignedLesson.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                learningGapAssignedLesson.CreatedAt = reader.GetValue(10).ToString();
                            else
                                learningGapAssignedLesson.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                learningGapAssignedLesson.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                learningGapAssignedLesson.UpdatedAt = null;
                            response.Add(learningGapAssignedLesson);
                            learningGapAssignedLesson = new LearningGapAssignedLessonModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapAssignedLessonModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAssignedLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapAssignedLessonModel> Read(string connectionString, List<LearningGapAssignedLessonModel> learningGapAssignedLessonWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAssignedLessonModel> response = new List<LearningGapAssignedLessonModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `academic_year_id`, `max_tries`, `teacher_id`, `limit_date`, `lesson_id`, `subject_code`, `student_id`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_assigned_lessons ";
                if (learningGapAssignedLessonWhere != null && learningGapAssignedLessonWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapAssignedLessonWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapAssignedLessonWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAssignedLessonWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].MaxTries))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `max_tries` = @maxTries" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxTries" + i, learningGapAssignedLessonWhere[i].MaxTries);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, learningGapAssignedLessonWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].LimitDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `limit_date` = @limitDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@limitDate" + i, learningGapAssignedLessonWhere[i].LimitDate);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, learningGapAssignedLessonWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapAssignedLessonWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapAssignedLessonWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].Completed))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `completed` = @completed" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@completed" + i, learningGapAssignedLessonWhere[i].Completed);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAssignedLessonWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAssignedLessonWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAssignedLessonWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapAssignedLessonWhere.Count - 1)
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
                            LearningGapAssignedLessonModel learningGapAssignedLesson = new LearningGapAssignedLessonModel(); if (!reader.IsDBNull(0))
                                learningGapAssignedLesson.Id = reader.GetValue(0).ToString();
                            else
                                learningGapAssignedLesson.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapAssignedLesson.AcademicYearId = reader.GetValue(1).ToString();
                            else
                                learningGapAssignedLesson.AcademicYearId = null;
                            if (!reader.IsDBNull(2))
                                learningGapAssignedLesson.MaxTries = reader.GetValue(2).ToString();
                            else
                                learningGapAssignedLesson.MaxTries = null;
                            if (!reader.IsDBNull(3))
                                learningGapAssignedLesson.TeacherId = reader.GetValue(3).ToString();
                            else
                                learningGapAssignedLesson.TeacherId = null;
                            if (!reader.IsDBNull(4))
                                learningGapAssignedLesson.LimitDate = reader.GetValue(4).ToString();
                            else
                                learningGapAssignedLesson.LimitDate = null;
                            if (!reader.IsDBNull(5))
                                learningGapAssignedLesson.LessonId = reader.GetValue(5).ToString();
                            else
                                learningGapAssignedLesson.LessonId = null;
                            if (!reader.IsDBNull(6))
                                learningGapAssignedLesson.SubjectCode = reader.GetValue(6).ToString();
                            else
                                learningGapAssignedLesson.SubjectCode = null;
                            if (!reader.IsDBNull(7))
                                learningGapAssignedLesson.StudentId = reader.GetValue(7).ToString();
                            else
                                learningGapAssignedLesson.StudentId = null;
                            if (!reader.IsDBNull(8))
                                learningGapAssignedLesson.Completed = reader.GetValue(8).ToString();
                            else
                                learningGapAssignedLesson.Completed = null;
                            if (!reader.IsDBNull(9))
                                learningGapAssignedLesson.ModifiedBy = reader.GetValue(9).ToString();
                            else
                                learningGapAssignedLesson.ModifiedBy = null;
                            if (!reader.IsDBNull(10))
                                learningGapAssignedLesson.CreatedAt = reader.GetValue(10).ToString();
                            else
                                learningGapAssignedLesson.CreatedAt = null;
                            if (!reader.IsDBNull(11))
                                learningGapAssignedLesson.UpdatedAt = reader.GetValue(11).ToString();
                            else
                                learningGapAssignedLesson.UpdatedAt = null;
                            response.Add(learningGapAssignedLesson);
                            learningGapAssignedLesson = new LearningGapAssignedLessonModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapAssignedLessonModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAssignedLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapAssignedLessonModel> Create(string connectionString, LearningGapAssignedLessonModel learningGapAssignedLesson)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAssignedLessonModel> response = new List<LearningGapAssignedLessonModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO learning_gap_assigned_lessons (";
                string values = "(";
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.Id))
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
                    cmd.Parameters.AddWithValue("@id", learningGapAssignedLesson.Id);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.AcademicYearId))
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
                    cmd.Parameters.AddWithValue("@academicYearId", learningGapAssignedLesson.AcademicYearId);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.MaxTries))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `max_tries` ");
                        values = String.Concat(values, ", @maxTries");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `max_tries` ");
                        values = String.Concat(values, " @maxTries");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@maxTries", learningGapAssignedLesson.MaxTries);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.TeacherId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `teacher_id` ");
                        values = String.Concat(values, ", @teacherId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `teacher_id` ");
                        values = String.Concat(values, " @teacherId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teacherId", learningGapAssignedLesson.TeacherId);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.LimitDate))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `limit_date` ");
                        values = String.Concat(values, ", @limitDate");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `limit_date` ");
                        values = String.Concat(values, " @limitDate");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@limitDate", learningGapAssignedLesson.LimitDate);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.LessonId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `lesson_id` ");
                        values = String.Concat(values, ", @lessonId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `lesson_id` ");
                        values = String.Concat(values, " @lessonId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@lessonId", learningGapAssignedLesson.LessonId);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.SubjectCode))
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
                    cmd.Parameters.AddWithValue("@subjectCode", learningGapAssignedLesson.SubjectCode);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.StudentId))
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
                    cmd.Parameters.AddWithValue("@studentId", learningGapAssignedLesson.StudentId);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.Completed))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `completed` ");
                        values = String.Concat(values, ", @completed");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `completed` ");
                        values = String.Concat(values, " @completed");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@completed", learningGapAssignedLesson.Completed);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapAssignedLesson.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", learningGapAssignedLesson.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapAssignedLesson.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                learningGapAssignedLesson.Id = cmd.LastInsertedId.ToString();
                response.Add(learningGapAssignedLesson);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAssignedLessonModel>();
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
        public List<LearningGapAssignedLessonModel> Create(string connectionString, List<LearningGapAssignedLessonModel> learningGapAssignedLessonList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAssignedLessonModel> response = new List<LearningGapAssignedLessonModel>();
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
                        for (int i = 0; i < learningGapAssignedLessonList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO learning_gap_assigned_lessons (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapAssignedLessonList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].AcademicYearId))
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
                                    cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAssignedLessonList[i].AcademicYearId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].MaxTries))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `max_tries` ");
                                        values = String.Concat(values, ", @maxTries" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `max_tries` ");
                                        values = String.Concat(values, "@maxTries" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@maxTries" + i, learningGapAssignedLessonList[i].MaxTries);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].TeacherId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `teacher_id` ");
                                        values = String.Concat(values, ", @teacherId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `teacher_id` ");
                                        values = String.Concat(values, "@teacherId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teacherId" + i, learningGapAssignedLessonList[i].TeacherId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].LimitDate))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `limit_date` ");
                                        values = String.Concat(values, ", @limitDate" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `limit_date` ");
                                        values = String.Concat(values, "@limitDate" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@limitDate" + i, learningGapAssignedLessonList[i].LimitDate);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].LessonId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `lesson_id` ");
                                        values = String.Concat(values, ", @lessonId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `lesson_id` ");
                                        values = String.Concat(values, "@lessonId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@lessonId" + i, learningGapAssignedLessonList[i].LessonId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].SubjectCode))
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
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapAssignedLessonList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].StudentId))
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
                                    cmd.Parameters.AddWithValue("@studentId" + i, learningGapAssignedLessonList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].Completed))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `completed` ");
                                        values = String.Concat(values, ", @completed" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `completed` ");
                                        values = String.Concat(values, "@completed" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@completed" + i, learningGapAssignedLessonList[i].Completed);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAssignedLessonList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAssignedLessonList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAssignedLessonList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(learningGapAssignedLessonList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapAssignedLessonModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapAssignedLessonModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAssignedLessonModel>();
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

        public List<LearningGapAssignedLessonModel> Update(string connectionString, LearningGapAssignedLessonModel learningGapAssignedLesson, LearningGapAssignedLessonModel learningGapAssignedLessonWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAssignedLessonModel> response = new List<LearningGapAssignedLessonModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE learning_gap_assigned_lessons SET ";
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", learningGapAssignedLesson.Id);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.AcademicYearId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `academic_year_id` = @academicYearId");
                    else
                        sql = String.Concat(sql, " `academic_year_id` = @academicYearId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@academicYearId", learningGapAssignedLesson.AcademicYearId);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.MaxTries))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `max_tries` = @maxTries");
                    else
                        sql = String.Concat(sql, " `max_tries` = @maxTries");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@maxTries", learningGapAssignedLesson.MaxTries);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.TeacherId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `teacher_id` = @teacherId");
                    else
                        sql = String.Concat(sql, " `teacher_id` = @teacherId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teacherId", learningGapAssignedLesson.TeacherId);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.LimitDate))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `limit_date` = @limitDate");
                    else
                        sql = String.Concat(sql, " `limit_date` = @limitDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@limitDate", learningGapAssignedLesson.LimitDate);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.LessonId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `lesson_id` = @lessonId");
                    else
                        sql = String.Concat(sql, " `lesson_id` = @lessonId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@lessonId", learningGapAssignedLesson.LessonId);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.SubjectCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `subject_code` = @subjectCode");
                    else
                        sql = String.Concat(sql, " `subject_code` = @subjectCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectCode", learningGapAssignedLesson.SubjectCode);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.StudentId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `student_id` = @studentId");
                    else
                        sql = String.Concat(sql, " `student_id` = @studentId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@studentId", learningGapAssignedLesson.StudentId);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.Completed))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `completed` = @completed");
                    else
                        sql = String.Concat(sql, " `completed` = @completed");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@completed", learningGapAssignedLesson.Completed);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapAssignedLesson.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", learningGapAssignedLesson.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapAssignedLesson.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapAssignedLesson.UpdatedAt);
                }
                if (learningGapAssignedLessonWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", learningGapAssignedLessonWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere.AcademicYearId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearIdWhere");
                        else
                            sql = String.Concat(sql, " `academic_year_id` = @academicYearIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@academicYearIdWhere", learningGapAssignedLessonWhere.AcademicYearId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere.MaxTries))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `max_tries` = @maxTriesWhere");
                        else
                            sql = String.Concat(sql, " `max_tries` = @maxTriesWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxTriesWhere", learningGapAssignedLessonWhere.MaxTries);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere.TeacherId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_id` = @teacherIdWhere");
                        else
                            sql = String.Concat(sql, " `teacher_id` = @teacherIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherIdWhere", learningGapAssignedLessonWhere.TeacherId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere.LimitDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `limit_date` = @limitDateWhere");
                        else
                            sql = String.Concat(sql, " `limit_date` = @limitDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@limitDateWhere", learningGapAssignedLessonWhere.LimitDate);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere.LessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lesson_id` = @lessonIdWhere");
                        else
                            sql = String.Concat(sql, " `lesson_id` = @lessonIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonIdWhere", learningGapAssignedLessonWhere.LessonId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCodeWhere", learningGapAssignedLessonWhere.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentIdWhere", learningGapAssignedLessonWhere.StudentId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere.Completed))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `completed` = @completedWhere");
                        else
                            sql = String.Concat(sql, " `completed` = @completedWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@completedWhere", learningGapAssignedLessonWhere.Completed);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", learningGapAssignedLessonWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", learningGapAssignedLessonWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", learningGapAssignedLessonWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(learningGapAssignedLesson);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAssignedLessonModel>();
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

        public List<LearningGapAssignedLessonModel> Update(string connectionString, List<LearningGapAssignedLessonModel> learningGapAssignedLessonList, List<LearningGapAssignedLessonModel> learningGapAssignedLessonWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAssignedLessonModel> response = new List<LearningGapAssignedLessonModel>();
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
                        for (int i = 0; i < learningGapAssignedLessonList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE learning_gap_assigned_lessons SET ";
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapAssignedLessonList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].AcademicYearId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `academic_year_id` = @academicYearId" + i);
                                    else
                                        sql = String.Concat(sql, " `academic_year_id` = @academicYearId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAssignedLessonList[i].AcademicYearId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].MaxTries))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `max_tries` = @maxTries" + i);
                                    else
                                        sql = String.Concat(sql, " `max_tries` = @maxTries" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@maxTries" + i, learningGapAssignedLessonList[i].MaxTries);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].TeacherId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `teacher_id` = @teacherId" + i);
                                    else
                                        sql = String.Concat(sql, " `teacher_id` = @teacherId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teacherId" + i, learningGapAssignedLessonList[i].TeacherId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].LimitDate))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `limit_date` = @limitDate" + i);
                                    else
                                        sql = String.Concat(sql, " `limit_date` = @limitDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@limitDate" + i, learningGapAssignedLessonList[i].LimitDate);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].LessonId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `lesson_id` = @lessonId" + i);
                                    else
                                        sql = String.Concat(sql, " `lesson_id` = @lessonId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@lessonId" + i, learningGapAssignedLessonList[i].LessonId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].SubjectCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `subject_code` = @subjectCode" + i);
                                    else
                                        sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapAssignedLessonList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].StudentId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `student_id` = @studentId" + i);
                                    else
                                        sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@studentId" + i, learningGapAssignedLessonList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].Completed))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `completed` = @completed" + i);
                                    else
                                        sql = String.Concat(sql, " `completed` = @completed" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@completed" + i, learningGapAssignedLessonList[i].Completed);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAssignedLessonList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAssignedLessonList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapAssignedLessonList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAssignedLessonList[i].UpdatedAt);
                                }
                                if (learningGapAssignedLessonWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, learningGapAssignedLessonWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereList[i].AcademicYearId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `academic_year_id` = @academicYearIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@academicYearIdWhere" + i, learningGapAssignedLessonWhereList[i].AcademicYearId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereList[i].MaxTries))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `max_tries` = @maxTriesWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `max_tries` = @maxTriesWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxTriesWhere" + i, learningGapAssignedLessonWhereList[i].MaxTries);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereList[i].TeacherId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_id` = @teacherIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_id` = @teacherIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherIdWhere" + i, learningGapAssignedLessonWhereList[i].TeacherId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereList[i].LimitDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `limit_date` = @limitDateWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `limit_date` = @limitDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@limitDateWhere" + i, learningGapAssignedLessonWhereList[i].LimitDate);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereList[i].LessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lesson_id` = @lessonIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `lesson_id` = @lessonIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonIdWhere" + i, learningGapAssignedLessonWhereList[i].LessonId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereList[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCodeWhere" + i, learningGapAssignedLessonWhereList[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereList[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentIdWhere" + i, learningGapAssignedLessonWhereList[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereList[i].Completed))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `completed` = @completedWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `completed` = @completedWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@completedWhere" + i, learningGapAssignedLessonWhereList[i].Completed);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, learningGapAssignedLessonWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, learningGapAssignedLessonWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLessonWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, learningGapAssignedLessonWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapAssignedLessonList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapAssignedLessonModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapAssignedLessonModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAssignedLessonModel>();
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

        public List<LearningGapAssignedLessonModel> Delete(string connectionString, LearningGapAssignedLessonModel learningGapAssignedLesson)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAssignedLessonModel> response = new List<LearningGapAssignedLessonModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM learning_gap_assigned_lessons  ";
                if (learningGapAssignedLesson != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(learningGapAssignedLesson.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", learningGapAssignedLesson.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLesson.AcademicYearId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearId");
                        else
                            sql = String.Concat(sql, " `academic_year_id` = @academicYearId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@academicYearId", learningGapAssignedLesson.AcademicYearId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLesson.MaxTries))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `max_tries` = @maxTries");
                        else
                            sql = String.Concat(sql, " `max_tries` = @maxTries");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxTries", learningGapAssignedLesson.MaxTries);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLesson.TeacherId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_id` = @teacherId");
                        else
                            sql = String.Concat(sql, " `teacher_id` = @teacherId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherId", learningGapAssignedLesson.TeacherId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLesson.LimitDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `limit_date` = @limitDate");
                        else
                            sql = String.Concat(sql, " `limit_date` = @limitDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@limitDate", learningGapAssignedLesson.LimitDate);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLesson.LessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lesson_id` = @lessonId");
                        else
                            sql = String.Concat(sql, " `lesson_id` = @lessonId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonId", learningGapAssignedLesson.LessonId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLesson.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCode", learningGapAssignedLesson.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLesson.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentId");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentId", learningGapAssignedLesson.StudentId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLesson.Completed))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `completed` = @completed");
                        else
                            sql = String.Concat(sql, " `completed` = @completed");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@completed", learningGapAssignedLesson.Completed);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLesson.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", learningGapAssignedLesson.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLesson.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", learningGapAssignedLesson.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapAssignedLesson.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", learningGapAssignedLesson.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(learningGapAssignedLesson);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAssignedLessonModel>();
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

        public List<LearningGapAssignedLessonModel> Delete(string connectionString, List<LearningGapAssignedLessonModel> learningGapAssignedLesson)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAssignedLessonModel> response = new List<LearningGapAssignedLessonModel>();
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
                        for (int i = 0; i < learningGapAssignedLesson.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM learning_gap_assigned_lessons  ";
                                if (learningGapAssignedLesson[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(learningGapAssignedLesson[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, learningGapAssignedLesson[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLesson[i].AcademicYearId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearId" + i);
                                        else
                                            sql = String.Concat(sql, " `academic_year_id` = @academicYearId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAssignedLesson[i].AcademicYearId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLesson[i].MaxTries))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `max_tries` = @maxTries" + i);
                                        else
                                            sql = String.Concat(sql, " `max_tries` = @maxTries" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxTries" + i, learningGapAssignedLesson[i].MaxTries);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLesson[i].TeacherId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_id` = @teacherId" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_id` = @teacherId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherId" + i, learningGapAssignedLesson[i].TeacherId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLesson[i].LimitDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `limit_date` = @limitDate" + i);
                                        else
                                            sql = String.Concat(sql, " `limit_date` = @limitDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@limitDate" + i, learningGapAssignedLesson[i].LimitDate);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLesson[i].LessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lesson_id` = @lessonId" + i);
                                        else
                                            sql = String.Concat(sql, " `lesson_id` = @lessonId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonId" + i, learningGapAssignedLesson[i].LessonId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLesson[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapAssignedLesson[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLesson[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentId" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentId" + i, learningGapAssignedLesson[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLesson[i].Completed))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `completed` = @completed" + i);
                                        else
                                            sql = String.Concat(sql, " `completed` = @completed" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@completed" + i, learningGapAssignedLesson[i].Completed);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLesson[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAssignedLesson[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLesson[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAssignedLesson[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAssignedLesson[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAssignedLesson[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapAssignedLesson[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapAssignedLessonModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapAssignedLessonModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAssignedLessonModel>();
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
