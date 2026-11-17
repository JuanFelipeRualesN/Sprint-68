using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.LearningGap;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.LearningGap
{
    class LearningGapAnnotationDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, LearningGapAnnotationModel> ReadD(string connectionString, LearningGapAnnotationModel learningGapAnnotation, bool firstOne, bool orderByLastDate, LearningGapAnnotationModel learningGapAnnotationWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapAnnotationModel> response = new Dictionary<string, LearningGapAnnotationModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `text`, `teacher_id`, `student_id`, `academic_year_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_annotations ";
                {
                    if (learningGapAnnotation != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAnnotation.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapAnnotation.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.Text))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `text` = @text ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `text` = @text ");
                            }
                            cmd.Parameters.AddWithValue("@text", learningGapAnnotation.Text);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", learningGapAnnotation.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapAnnotation.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapAnnotation.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapAnnotation.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapAnnotation.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapAnnotation.UpdatedAt);
                        }
                    }
                    if (learningGapAnnotationWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapAnnotationWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.Text))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `text` LIKE @textWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `text` LIKE @textWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@textWhereLike", "%" + learningGapAnnotationWhereLike.Text + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + learningGapAnnotationWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapAnnotationWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapAnnotationWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapAnnotationWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapAnnotationWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapAnnotationWhereLike.UpdatedAt + "%");
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
                                learningGapAnnotation = new LearningGapAnnotationModel();
                                if (!reader.IsDBNull(0))
                                    learningGapAnnotation.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapAnnotation.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapAnnotation.Text = reader.GetValue(1).ToString();
                                else
                                    learningGapAnnotation.Text = null;
                                if (!reader.IsDBNull(2))
                                    learningGapAnnotation.TeacherId = reader.GetValue(2).ToString();
                                else
                                    learningGapAnnotation.TeacherId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapAnnotation.StudentId = reader.GetValue(3).ToString();
                                else
                                    learningGapAnnotation.StudentId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapAnnotation.AcademicYearId = reader.GetValue(4).ToString();
                                else
                                    learningGapAnnotation.AcademicYearId = null;
                                if (!reader.IsDBNull(5))
                                    learningGapAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    learningGapAnnotation.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    learningGapAnnotation.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    learningGapAnnotation.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    learningGapAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapAnnotation.UpdatedAt = null;
                                response.Add(learningGapAnnotation.Id, learningGapAnnotation);
                                learningGapAnnotation = new LearningGapAnnotationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapAnnotationModel>();
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
                response = new Dictionary<string, LearningGapAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapAnnotationModel> ReadD(string connectionString, LearningGapAnnotationModel learningGapAnnotation, bool orderByLastDate, int fromRow, int limit, LearningGapAnnotationModel learningGapAnnotationWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapAnnotationModel> response = new Dictionary<string, LearningGapAnnotationModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `text`, `teacher_id`, `student_id`, `academic_year_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_annotations ";
                {
                    if (learningGapAnnotation != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAnnotation.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapAnnotation.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.Text))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `text` = @text ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `text` = @text ");
                            }
                            cmd.Parameters.AddWithValue("@text", learningGapAnnotation.Text);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", learningGapAnnotation.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapAnnotation.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapAnnotation.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapAnnotation.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapAnnotation.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapAnnotation.UpdatedAt);
                        }
                    }
                    if (learningGapAnnotationWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapAnnotationWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.Text))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `text` LIKE @textWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `text` LIKE @textWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@textWhereLike", "%" + learningGapAnnotationWhereLike.Text + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + learningGapAnnotationWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapAnnotationWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapAnnotationWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapAnnotationWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapAnnotationWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapAnnotationWhereLike.UpdatedAt + "%");
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
                                learningGapAnnotation = new LearningGapAnnotationModel();
                                if (!reader.IsDBNull(0))
                                    learningGapAnnotation.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapAnnotation.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapAnnotation.Text = reader.GetValue(1).ToString();
                                else
                                    learningGapAnnotation.Text = null;
                                if (!reader.IsDBNull(2))
                                    learningGapAnnotation.TeacherId = reader.GetValue(2).ToString();
                                else
                                    learningGapAnnotation.TeacherId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapAnnotation.StudentId = reader.GetValue(3).ToString();
                                else
                                    learningGapAnnotation.StudentId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapAnnotation.AcademicYearId = reader.GetValue(4).ToString();
                                else
                                    learningGapAnnotation.AcademicYearId = null;
                                if (!reader.IsDBNull(5))
                                    learningGapAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    learningGapAnnotation.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    learningGapAnnotation.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    learningGapAnnotation.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    learningGapAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapAnnotation.UpdatedAt = null;
                                response.Add(learningGapAnnotation.Id, learningGapAnnotation);
                                learningGapAnnotation = new LearningGapAnnotationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapAnnotationModel>();
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
                response = new Dictionary<string, LearningGapAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapAnnotationModel> ReadD(string connectionString, List<LearningGapAnnotationModel> learningGapAnnotationWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapAnnotationModel> response = new Dictionary<string, LearningGapAnnotationModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `text`, `teacher_id`, `student_id`, `academic_year_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_annotations ";
                if (learningGapAnnotationWhere != null && learningGapAnnotationWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapAnnotationWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapAnnotationWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].Text))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `text` = @text" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@text" + i, learningGapAnnotationWhere[i].Text);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, learningGapAnnotationWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapAnnotationWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAnnotationWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAnnotationWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAnnotationWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAnnotationWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapAnnotationWhere.Count - 1)
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
                            LearningGapAnnotationModel learningGapAnnotation = new LearningGapAnnotationModel(); if (!reader.IsDBNull(0))
                                learningGapAnnotation.Id = reader.GetValue(0).ToString();
                            else
                                learningGapAnnotation.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapAnnotation.Text = reader.GetValue(1).ToString();
                            else
                                learningGapAnnotation.Text = null;
                            if (!reader.IsDBNull(2))
                                learningGapAnnotation.TeacherId = reader.GetValue(2).ToString();
                            else
                                learningGapAnnotation.TeacherId = null;
                            if (!reader.IsDBNull(3))
                                learningGapAnnotation.StudentId = reader.GetValue(3).ToString();
                            else
                                learningGapAnnotation.StudentId = null;
                            if (!reader.IsDBNull(4))
                                learningGapAnnotation.AcademicYearId = reader.GetValue(4).ToString();
                            else
                                learningGapAnnotation.AcademicYearId = null;
                            if (!reader.IsDBNull(5))
                                learningGapAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                learningGapAnnotation.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                learningGapAnnotation.CreatedAt = reader.GetValue(6).ToString();
                            else
                                learningGapAnnotation.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                learningGapAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapAnnotation.UpdatedAt = null;
                            response.Add(learningGapAnnotation.Id, learningGapAnnotation);
                            learningGapAnnotation = new LearningGapAnnotationModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapAnnotationModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapAnnotationModel> ReadD(string connectionString, List<LearningGapAnnotationModel> learningGapAnnotationWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapAnnotationModel> response = new Dictionary<string, LearningGapAnnotationModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `text`, `teacher_id`, `student_id`, `academic_year_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_annotations ";
                if (learningGapAnnotationWhere != null && learningGapAnnotationWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapAnnotationWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapAnnotationWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].Text))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `text` = @text" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@text" + i, learningGapAnnotationWhere[i].Text);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, learningGapAnnotationWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapAnnotationWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAnnotationWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAnnotationWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAnnotationWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAnnotationWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapAnnotationWhere.Count - 1)
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
                            LearningGapAnnotationModel learningGapAnnotation = new LearningGapAnnotationModel(); if (!reader.IsDBNull(0))
                                learningGapAnnotation.Id = reader.GetValue(0).ToString();
                            else
                                learningGapAnnotation.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapAnnotation.Text = reader.GetValue(1).ToString();
                            else
                                learningGapAnnotation.Text = null;
                            if (!reader.IsDBNull(2))
                                learningGapAnnotation.TeacherId = reader.GetValue(2).ToString();
                            else
                                learningGapAnnotation.TeacherId = null;
                            if (!reader.IsDBNull(3))
                                learningGapAnnotation.StudentId = reader.GetValue(3).ToString();
                            else
                                learningGapAnnotation.StudentId = null;
                            if (!reader.IsDBNull(4))
                                learningGapAnnotation.AcademicYearId = reader.GetValue(4).ToString();
                            else
                                learningGapAnnotation.AcademicYearId = null;
                            if (!reader.IsDBNull(5))
                                learningGapAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                learningGapAnnotation.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                learningGapAnnotation.CreatedAt = reader.GetValue(6).ToString();
                            else
                                learningGapAnnotation.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                learningGapAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapAnnotation.UpdatedAt = null;
                            response.Add(learningGapAnnotation.Id, learningGapAnnotation);
                            learningGapAnnotation = new LearningGapAnnotationModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapAnnotationModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapAnnotationModel> Read(string connectionString, LearningGapAnnotationModel learningGapAnnotation, bool firstOne, bool orderByLastDate, LearningGapAnnotationModel learningGapAnnotationWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAnnotationModel> response = new List<LearningGapAnnotationModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `text`, `teacher_id`, `student_id`, `academic_year_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_annotations ";
                {
                    if (learningGapAnnotation != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAnnotation.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapAnnotation.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.Text))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `text` = @text ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `text` = @text ");
                            }
                            cmd.Parameters.AddWithValue("@text", learningGapAnnotation.Text);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", learningGapAnnotation.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapAnnotation.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapAnnotation.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapAnnotation.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapAnnotation.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapAnnotation.UpdatedAt);
                        }
                    }
                    if (learningGapAnnotationWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapAnnotationWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.Text))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `text` LIKE @textWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `text` LIKE @textWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@textWhereLike", "%" + learningGapAnnotationWhereLike.Text + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + learningGapAnnotationWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapAnnotationWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapAnnotationWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapAnnotationWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapAnnotationWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapAnnotationWhereLike.UpdatedAt + "%");
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
                                learningGapAnnotation = new LearningGapAnnotationModel();
                                if (!reader.IsDBNull(0))
                                    learningGapAnnotation.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapAnnotation.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapAnnotation.Text = reader.GetValue(1).ToString();
                                else
                                    learningGapAnnotation.Text = null;
                                if (!reader.IsDBNull(2))
                                    learningGapAnnotation.TeacherId = reader.GetValue(2).ToString();
                                else
                                    learningGapAnnotation.TeacherId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapAnnotation.StudentId = reader.GetValue(3).ToString();
                                else
                                    learningGapAnnotation.StudentId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapAnnotation.AcademicYearId = reader.GetValue(4).ToString();
                                else
                                    learningGapAnnotation.AcademicYearId = null;
                                if (!reader.IsDBNull(5))
                                    learningGapAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    learningGapAnnotation.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    learningGapAnnotation.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    learningGapAnnotation.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    learningGapAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapAnnotation.UpdatedAt = null;
                                response.Add(learningGapAnnotation);
                                learningGapAnnotation = new LearningGapAnnotationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapAnnotationModel>();
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
                response = new List<LearningGapAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapAnnotationModel> Read(string connectionString, LearningGapAnnotationModel learningGapAnnotation, bool orderByLastDate, int fromRow, int limit, LearningGapAnnotationModel learningGapAnnotationWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAnnotationModel> response = new List<LearningGapAnnotationModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `text`, `teacher_id`, `student_id`, `academic_year_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_annotations ";
                {
                    if (learningGapAnnotation != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAnnotation.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapAnnotation.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.Text))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `text` = @text ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `text` = @text ");
                            }
                            cmd.Parameters.AddWithValue("@text", learningGapAnnotation.Text);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", learningGapAnnotation.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapAnnotation.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearId", learningGapAnnotation.AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapAnnotation.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapAnnotation.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapAnnotation.UpdatedAt);
                        }
                    }
                    if (learningGapAnnotationWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapAnnotationWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.Text))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `text` LIKE @textWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `text` LIKE @textWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@textWhereLike", "%" + learningGapAnnotationWhereLike.Text + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + learningGapAnnotationWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapAnnotationWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.AcademicYearId))
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
                            cmd.Parameters.AddWithValue("@academicYearIdWhereLike", "%" + learningGapAnnotationWhereLike.AcademicYearId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapAnnotationWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapAnnotationWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapAnnotationWhereLike.UpdatedAt + "%");
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
                                learningGapAnnotation = new LearningGapAnnotationModel();
                                if (!reader.IsDBNull(0))
                                    learningGapAnnotation.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapAnnotation.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapAnnotation.Text = reader.GetValue(1).ToString();
                                else
                                    learningGapAnnotation.Text = null;
                                if (!reader.IsDBNull(2))
                                    learningGapAnnotation.TeacherId = reader.GetValue(2).ToString();
                                else
                                    learningGapAnnotation.TeacherId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapAnnotation.StudentId = reader.GetValue(3).ToString();
                                else
                                    learningGapAnnotation.StudentId = null;
                                if (!reader.IsDBNull(4))
                                    learningGapAnnotation.AcademicYearId = reader.GetValue(4).ToString();
                                else
                                    learningGapAnnotation.AcademicYearId = null;
                                if (!reader.IsDBNull(5))
                                    learningGapAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    learningGapAnnotation.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    learningGapAnnotation.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    learningGapAnnotation.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    learningGapAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    learningGapAnnotation.UpdatedAt = null;
                                response.Add(learningGapAnnotation);
                                learningGapAnnotation = new LearningGapAnnotationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapAnnotationModel>();
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
                response = new List<LearningGapAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapAnnotationModel> Read(string connectionString, List<LearningGapAnnotationModel> learningGapAnnotationWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAnnotationModel> response = new List<LearningGapAnnotationModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `text`, `teacher_id`, `student_id`, `academic_year_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_annotations ";
                if (learningGapAnnotationWhere != null && learningGapAnnotationWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapAnnotationWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapAnnotationWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].Text))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `text` = @text" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@text" + i, learningGapAnnotationWhere[i].Text);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, learningGapAnnotationWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapAnnotationWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAnnotationWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAnnotationWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAnnotationWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAnnotationWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapAnnotationWhere.Count - 1)
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
                            LearningGapAnnotationModel learningGapAnnotation = new LearningGapAnnotationModel(); if (!reader.IsDBNull(0))
                                learningGapAnnotation.Id = reader.GetValue(0).ToString();
                            else
                                learningGapAnnotation.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapAnnotation.Text = reader.GetValue(1).ToString();
                            else
                                learningGapAnnotation.Text = null;
                            if (!reader.IsDBNull(2))
                                learningGapAnnotation.TeacherId = reader.GetValue(2).ToString();
                            else
                                learningGapAnnotation.TeacherId = null;
                            if (!reader.IsDBNull(3))
                                learningGapAnnotation.StudentId = reader.GetValue(3).ToString();
                            else
                                learningGapAnnotation.StudentId = null;
                            if (!reader.IsDBNull(4))
                                learningGapAnnotation.AcademicYearId = reader.GetValue(4).ToString();
                            else
                                learningGapAnnotation.AcademicYearId = null;
                            if (!reader.IsDBNull(5))
                                learningGapAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                learningGapAnnotation.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                learningGapAnnotation.CreatedAt = reader.GetValue(6).ToString();
                            else
                                learningGapAnnotation.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                learningGapAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapAnnotation.UpdatedAt = null;
                            response.Add(learningGapAnnotation);
                            learningGapAnnotation = new LearningGapAnnotationModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapAnnotationModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapAnnotationModel> Read(string connectionString, List<LearningGapAnnotationModel> learningGapAnnotationWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAnnotationModel> response = new List<LearningGapAnnotationModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `text`, `teacher_id`, `student_id`, `academic_year_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_annotations ";
                if (learningGapAnnotationWhere != null && learningGapAnnotationWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapAnnotationWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapAnnotationWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].Text))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `text` = @text" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@text" + i, learningGapAnnotationWhere[i].Text);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, learningGapAnnotationWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapAnnotationWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].AcademicYearId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `academic_year_id` = @academicYearId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAnnotationWhere[i].AcademicYearId);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAnnotationWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAnnotationWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapAnnotationWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAnnotationWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapAnnotationWhere.Count - 1)
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
                            LearningGapAnnotationModel learningGapAnnotation = new LearningGapAnnotationModel(); if (!reader.IsDBNull(0))
                                learningGapAnnotation.Id = reader.GetValue(0).ToString();
                            else
                                learningGapAnnotation.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapAnnotation.Text = reader.GetValue(1).ToString();
                            else
                                learningGapAnnotation.Text = null;
                            if (!reader.IsDBNull(2))
                                learningGapAnnotation.TeacherId = reader.GetValue(2).ToString();
                            else
                                learningGapAnnotation.TeacherId = null;
                            if (!reader.IsDBNull(3))
                                learningGapAnnotation.StudentId = reader.GetValue(3).ToString();
                            else
                                learningGapAnnotation.StudentId = null;
                            if (!reader.IsDBNull(4))
                                learningGapAnnotation.AcademicYearId = reader.GetValue(4).ToString();
                            else
                                learningGapAnnotation.AcademicYearId = null;
                            if (!reader.IsDBNull(5))
                                learningGapAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                learningGapAnnotation.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                learningGapAnnotation.CreatedAt = reader.GetValue(6).ToString();
                            else
                                learningGapAnnotation.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                learningGapAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                learningGapAnnotation.UpdatedAt = null;
                            response.Add(learningGapAnnotation);
                            learningGapAnnotation = new LearningGapAnnotationModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapAnnotationModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapAnnotationModel> Create(string connectionString, LearningGapAnnotationModel learningGapAnnotation)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAnnotationModel> response = new List<LearningGapAnnotationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO learning_gap_annotations (";
                string values = "(";
                if (!String.IsNullOrEmpty(learningGapAnnotation.Id))
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
                    cmd.Parameters.AddWithValue("@id", learningGapAnnotation.Id);
                }
                if (!String.IsNullOrEmpty(learningGapAnnotation.Text))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `text` ");
                        values = String.Concat(values, ", @text");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `text` ");
                        values = String.Concat(values, " @text");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@text", learningGapAnnotation.Text);
                }
                if (!String.IsNullOrEmpty(learningGapAnnotation.TeacherId))
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
                    cmd.Parameters.AddWithValue("@teacherId", learningGapAnnotation.TeacherId);
                }
                if (!String.IsNullOrEmpty(learningGapAnnotation.StudentId))
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
                    cmd.Parameters.AddWithValue("@studentId", learningGapAnnotation.StudentId);
                }
                if (!String.IsNullOrEmpty(learningGapAnnotation.AcademicYearId))
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
                    cmd.Parameters.AddWithValue("@academicYearId", learningGapAnnotation.AcademicYearId);
                }
                if (!String.IsNullOrEmpty(learningGapAnnotation.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapAnnotation.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapAnnotation.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", learningGapAnnotation.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapAnnotation.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapAnnotation.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                learningGapAnnotation.Id = cmd.LastInsertedId.ToString();
                response.Add(learningGapAnnotation);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAnnotationModel>();
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
        public List<LearningGapAnnotationModel> Create(string connectionString, List<LearningGapAnnotationModel> learningGapAnnotationList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAnnotationModel> response = new List<LearningGapAnnotationModel>();
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
                        for (int i = 0; i < learningGapAnnotationList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO learning_gap_annotations (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapAnnotationList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].Text))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `text` ");
                                        values = String.Concat(values, ", @text" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `text` ");
                                        values = String.Concat(values, "@text" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@text" + i, learningGapAnnotationList[i].Text);
                                }
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].TeacherId))
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
                                    cmd.Parameters.AddWithValue("@teacherId" + i, learningGapAnnotationList[i].TeacherId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].StudentId))
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
                                    cmd.Parameters.AddWithValue("@studentId" + i, learningGapAnnotationList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].AcademicYearId))
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
                                    cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAnnotationList[i].AcademicYearId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAnnotationList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAnnotationList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAnnotationList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(learningGapAnnotationList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapAnnotationModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapAnnotationModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAnnotationModel>();
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

        public List<LearningGapAnnotationModel> Update(string connectionString, LearningGapAnnotationModel learningGapAnnotation, LearningGapAnnotationModel learningGapAnnotationWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAnnotationModel> response = new List<LearningGapAnnotationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE learning_gap_annotations SET ";
                if (!String.IsNullOrEmpty(learningGapAnnotation.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", learningGapAnnotation.Id);
                }
                if (!String.IsNullOrEmpty(learningGapAnnotation.Text))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `text` = @text");
                    else
                        sql = String.Concat(sql, " `text` = @text");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@text", learningGapAnnotation.Text);
                }
                if (!String.IsNullOrEmpty(learningGapAnnotation.TeacherId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `teacher_id` = @teacherId");
                    else
                        sql = String.Concat(sql, " `teacher_id` = @teacherId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teacherId", learningGapAnnotation.TeacherId);
                }
                if (!String.IsNullOrEmpty(learningGapAnnotation.StudentId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `student_id` = @studentId");
                    else
                        sql = String.Concat(sql, " `student_id` = @studentId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@studentId", learningGapAnnotation.StudentId);
                }
                if (!String.IsNullOrEmpty(learningGapAnnotation.AcademicYearId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `academic_year_id` = @academicYearId");
                    else
                        sql = String.Concat(sql, " `academic_year_id` = @academicYearId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@academicYearId", learningGapAnnotation.AcademicYearId);
                }
                if (!String.IsNullOrEmpty(learningGapAnnotation.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapAnnotation.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapAnnotation.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", learningGapAnnotation.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapAnnotation.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapAnnotation.UpdatedAt);
                }
                if (learningGapAnnotationWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(learningGapAnnotationWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", learningGapAnnotationWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapAnnotationWhere.Text))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `text` = @textWhere");
                        else
                            sql = String.Concat(sql, " `text` = @textWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@textWhere", learningGapAnnotationWhere.Text);
                    }
                    if (!String.IsNullOrEmpty(learningGapAnnotationWhere.TeacherId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_id` = @teacherIdWhere");
                        else
                            sql = String.Concat(sql, " `teacher_id` = @teacherIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherIdWhere", learningGapAnnotationWhere.TeacherId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAnnotationWhere.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentIdWhere", learningGapAnnotationWhere.StudentId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAnnotationWhere.AcademicYearId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearIdWhere");
                        else
                            sql = String.Concat(sql, " `academic_year_id` = @academicYearIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@academicYearIdWhere", learningGapAnnotationWhere.AcademicYearId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAnnotationWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", learningGapAnnotationWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapAnnotationWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", learningGapAnnotationWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapAnnotationWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", learningGapAnnotationWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(learningGapAnnotation);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAnnotationModel>();
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

        public List<LearningGapAnnotationModel> Update(string connectionString, List<LearningGapAnnotationModel> learningGapAnnotationList, List<LearningGapAnnotationModel> learningGapAnnotationWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAnnotationModel> response = new List<LearningGapAnnotationModel>();
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
                        for (int i = 0; i < learningGapAnnotationList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE learning_gap_annotations SET ";
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapAnnotationList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].Text))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `text` = @text" + i);
                                    else
                                        sql = String.Concat(sql, " `text` = @text" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@text" + i, learningGapAnnotationList[i].Text);
                                }
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].TeacherId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `teacher_id` = @teacherId" + i);
                                    else
                                        sql = String.Concat(sql, " `teacher_id` = @teacherId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teacherId" + i, learningGapAnnotationList[i].TeacherId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].StudentId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `student_id` = @studentId" + i);
                                    else
                                        sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@studentId" + i, learningGapAnnotationList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].AcademicYearId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `academic_year_id` = @academicYearId" + i);
                                    else
                                        sql = String.Concat(sql, " `academic_year_id` = @academicYearId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAnnotationList[i].AcademicYearId);
                                }
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAnnotationList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAnnotationList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapAnnotationList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAnnotationList[i].UpdatedAt);
                                }
                                if (learningGapAnnotationWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(learningGapAnnotationWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, learningGapAnnotationWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAnnotationWhereList[i].Text))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `text` = @textWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `text` = @textWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@textWhere" + i, learningGapAnnotationWhereList[i].Text);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAnnotationWhereList[i].TeacherId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_id` = @teacherIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_id` = @teacherIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherIdWhere" + i, learningGapAnnotationWhereList[i].TeacherId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAnnotationWhereList[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentIdWhere" + i, learningGapAnnotationWhereList[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAnnotationWhereList[i].AcademicYearId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `academic_year_id` = @academicYearIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@academicYearIdWhere" + i, learningGapAnnotationWhereList[i].AcademicYearId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAnnotationWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, learningGapAnnotationWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAnnotationWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, learningGapAnnotationWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAnnotationWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, learningGapAnnotationWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapAnnotationList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapAnnotationModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapAnnotationModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAnnotationModel>();
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

        public List<LearningGapAnnotationModel> Delete(string connectionString, LearningGapAnnotationModel learningGapAnnotation)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAnnotationModel> response = new List<LearningGapAnnotationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM learning_gap_annotations  ";
                if (learningGapAnnotation != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(learningGapAnnotation.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", learningGapAnnotation.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapAnnotation.Text))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `text` = @text");
                        else
                            sql = String.Concat(sql, " `text` = @text");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@text", learningGapAnnotation.Text);
                    }
                    if (!String.IsNullOrEmpty(learningGapAnnotation.TeacherId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_id` = @teacherId");
                        else
                            sql = String.Concat(sql, " `teacher_id` = @teacherId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherId", learningGapAnnotation.TeacherId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAnnotation.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentId");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentId", learningGapAnnotation.StudentId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAnnotation.AcademicYearId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearId");
                        else
                            sql = String.Concat(sql, " `academic_year_id` = @academicYearId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@academicYearId", learningGapAnnotation.AcademicYearId);
                    }
                    if (!String.IsNullOrEmpty(learningGapAnnotation.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", learningGapAnnotation.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapAnnotation.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", learningGapAnnotation.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapAnnotation.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", learningGapAnnotation.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(learningGapAnnotation);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAnnotationModel>();
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

        public List<LearningGapAnnotationModel> Delete(string connectionString, List<LearningGapAnnotationModel> learningGapAnnotation)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapAnnotationModel> response = new List<LearningGapAnnotationModel>();
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
                        for (int i = 0; i < learningGapAnnotation.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM learning_gap_annotations  ";
                                if (learningGapAnnotation[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(learningGapAnnotation[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, learningGapAnnotation[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAnnotation[i].Text))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `text` = @text" + i);
                                        else
                                            sql = String.Concat(sql, " `text` = @text" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@text" + i, learningGapAnnotation[i].Text);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAnnotation[i].TeacherId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_id` = @teacherId" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_id` = @teacherId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherId" + i, learningGapAnnotation[i].TeacherId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAnnotation[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentId" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentId" + i, learningGapAnnotation[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAnnotation[i].AcademicYearId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `academic_year_id` = @academicYearId" + i);
                                        else
                                            sql = String.Concat(sql, " `academic_year_id` = @academicYearId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@academicYearId" + i, learningGapAnnotation[i].AcademicYearId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAnnotation[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapAnnotation[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAnnotation[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, learningGapAnnotation[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapAnnotation[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapAnnotation[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapAnnotation[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapAnnotationModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapAnnotationModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapAnnotationModel>();
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
