using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Teachers;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    class TeacherStudentAnnotationDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, TeacherStudentAnnotationModel> ReadD(string connectionString, TeacherStudentAnnotationModel teacherStudentAnnotation, bool firstOne, bool orderByLastDate, TeacherStudentAnnotationModel teacherStudentAnnotationWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherStudentAnnotationModel> response = new Dictionary<string, TeacherStudentAnnotationModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `teacher_plan_id`, `annotation`, `modified_by`, `created_at`, `updated_at`  FROM teacher_student_annotations ";
                {
                    if (teacherStudentAnnotation != null)
                    {
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherStudentAnnotation.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", teacherStudentAnnotation.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", teacherStudentAnnotation.StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.TeacherPlanId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teacher_plan_id` = @teacherPlanId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teacher_plan_id` = @teacherPlanId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherPlanId", teacherStudentAnnotation.TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.Annotation))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `annotation` = @annotation ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `annotation` = @annotation ");
                            }
                            cmd.Parameters.AddWithValue("@annotation", teacherStudentAnnotation.Annotation);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherStudentAnnotation.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherStudentAnnotation.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherStudentAnnotation.UpdatedAt);
                        }
                    }
                    if (teacherStudentAnnotationWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherStudentAnnotationWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + teacherStudentAnnotationWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + teacherStudentAnnotationWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.TeacherPlanId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teacher_plan_id` LIKE @teacherPlanIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teacher_plan_id` LIKE @teacherPlanIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@teacherPlanIdWhereLike", "%" + teacherStudentAnnotationWhereLike.TeacherPlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.Annotation))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `annotation` LIKE @annotationWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `annotation` LIKE @annotationWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@annotationWhereLike", "%" + teacherStudentAnnotationWhereLike.Annotation + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherStudentAnnotationWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherStudentAnnotationWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherStudentAnnotationWhereLike.UpdatedAt + "%");
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
                                teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                                if (!reader.IsDBNull(0))
                                    teacherStudentAnnotation.Id = reader.GetValue(0).ToString();
                                else
                                    teacherStudentAnnotation.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherStudentAnnotation.TeacherId = reader.GetValue(1).ToString();
                                else
                                    teacherStudentAnnotation.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    teacherStudentAnnotation.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherStudentAnnotation.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherStudentAnnotation.TeacherPlanId = reader.GetValue(3).ToString();
                                else
                                    teacherStudentAnnotation.TeacherPlanId = null;
                                if (!reader.IsDBNull(4))
                                    teacherStudentAnnotation.Annotation = reader.GetValue(4).ToString();
                                else
                                    teacherStudentAnnotation.Annotation = null;
                                if (!reader.IsDBNull(5))
                                    teacherStudentAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    teacherStudentAnnotation.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    teacherStudentAnnotation.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    teacherStudentAnnotation.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    teacherStudentAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    teacherStudentAnnotation.UpdatedAt = null;
                                response.Add(teacherStudentAnnotation.Id, teacherStudentAnnotation);
                                teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherStudentAnnotationModel>();
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
                response = new Dictionary<string, TeacherStudentAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherStudentAnnotationModel> ReadD(string connectionString, TeacherStudentAnnotationModel teacherStudentAnnotation, bool orderByLastDate, int fromRow, int limit, TeacherStudentAnnotationModel teacherStudentAnnotationWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherStudentAnnotationModel> response = new Dictionary<string, TeacherStudentAnnotationModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `teacher_plan_id`, `annotation`, `modified_by`, `created_at`, `updated_at`  FROM teacher_student_annotations ";
                {
                    if (teacherStudentAnnotation != null)
                    {
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherStudentAnnotation.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", teacherStudentAnnotation.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", teacherStudentAnnotation.StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.TeacherPlanId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teacher_plan_id` = @teacherPlanId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teacher_plan_id` = @teacherPlanId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherPlanId", teacherStudentAnnotation.TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.Annotation))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `annotation` = @annotation ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `annotation` = @annotation ");
                            }
                            cmd.Parameters.AddWithValue("@annotation", teacherStudentAnnotation.Annotation);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherStudentAnnotation.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherStudentAnnotation.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherStudentAnnotation.UpdatedAt);
                        }
                    }
                    if (teacherStudentAnnotationWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherStudentAnnotationWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + teacherStudentAnnotationWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + teacherStudentAnnotationWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.TeacherPlanId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teacher_plan_id` LIKE @teacherPlanIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teacher_plan_id` LIKE @teacherPlanIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@teacherPlanIdWhereLike", "%" + teacherStudentAnnotationWhereLike.TeacherPlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.Annotation))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `annotation` LIKE @annotationWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `annotation` LIKE @annotationWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@annotationWhereLike", "%" + teacherStudentAnnotationWhereLike.Annotation + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherStudentAnnotationWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherStudentAnnotationWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherStudentAnnotationWhereLike.UpdatedAt + "%");
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
                                teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                                if (!reader.IsDBNull(0))
                                    teacherStudentAnnotation.Id = reader.GetValue(0).ToString();
                                else
                                    teacherStudentAnnotation.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherStudentAnnotation.TeacherId = reader.GetValue(1).ToString();
                                else
                                    teacherStudentAnnotation.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    teacherStudentAnnotation.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherStudentAnnotation.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherStudentAnnotation.TeacherPlanId = reader.GetValue(3).ToString();
                                else
                                    teacherStudentAnnotation.TeacherPlanId = null;
                                if (!reader.IsDBNull(4))
                                    teacherStudentAnnotation.Annotation = reader.GetValue(4).ToString();
                                else
                                    teacherStudentAnnotation.Annotation = null;
                                if (!reader.IsDBNull(5))
                                    teacherStudentAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    teacherStudentAnnotation.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    teacherStudentAnnotation.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    teacherStudentAnnotation.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    teacherStudentAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    teacherStudentAnnotation.UpdatedAt = null;
                                response.Add(teacherStudentAnnotation.Id, teacherStudentAnnotation);
                                teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherStudentAnnotationModel>();
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
                response = new Dictionary<string, TeacherStudentAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherStudentAnnotationModel> ReadD(string connectionString, TeacherStudentAnnotationModel teacherStudentAnnotation, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherStudentAnnotationModel> response = new Dictionary<string, TeacherStudentAnnotationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `teacher_plan_id`, `annotation`, `modified_by`, `created_at`, `updated_at`  FROM teacher_student_annotations ";
                {
                    if (teacherStudentAnnotation != null)
                    {
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherStudentAnnotation.Id);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.TeacherId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teacher_id` = @teacherId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teacher_id` = @teacherId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherId", teacherStudentAnnotation.TeacherId);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.StudentId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `student_id` = @studentId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `student_id` = @studentId ");
                            }
                            cmd.Parameters.AddWithValue("@studentId", teacherStudentAnnotation.StudentId);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.TeacherPlanId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teacher_plan_id` = @teacherPlanId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teacher_plan_id` = @teacherPlanId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherPlanId", teacherStudentAnnotation.TeacherPlanId);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.Annotation))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `annotation` = @annotation ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `annotation` = @annotation ");
                            }
                            cmd.Parameters.AddWithValue("@annotation", teacherStudentAnnotation.Annotation);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherStudentAnnotation.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherStudentAnnotation.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherStudentAnnotation.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        if (firstOne)
                            sql = string.Concat(sql, " LIMIT 1 ");
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    teacherStudentAnnotation.Id = reader.GetValue(0).ToString();
                                else
                                    teacherStudentAnnotation.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherStudentAnnotation.TeacherId = reader.GetValue(1).ToString();
                                else
                                    teacherStudentAnnotation.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    teacherStudentAnnotation.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherStudentAnnotation.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherStudentAnnotation.TeacherPlanId = reader.GetValue(3).ToString();
                                else
                                    teacherStudentAnnotation.TeacherPlanId = null;
                                if (!reader.IsDBNull(4))
                                    teacherStudentAnnotation.Annotation = reader.GetValue(4).ToString();
                                else
                                    teacherStudentAnnotation.Annotation = null;
                                if (!reader.IsDBNull(5))
                                    teacherStudentAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    teacherStudentAnnotation.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    teacherStudentAnnotation.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    teacherStudentAnnotation.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    teacherStudentAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    teacherStudentAnnotation.UpdatedAt = null;
                                response.Add(teacherStudentAnnotation.Id, teacherStudentAnnotation);
                                teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherStudentAnnotationModel>();
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
                response = new Dictionary<string, TeacherStudentAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherStudentAnnotationModel> ReadD(string connectionString, TeacherStudentAnnotationModel teacherStudentAnnotation, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherStudentAnnotationModel> response = new Dictionary<string, TeacherStudentAnnotationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `teacher_plan_id`, `annotation`, `modified_by`, `created_at`, `updated_at`  FROM teacher_student_annotations ";
                {
                    if (teacherStudentAnnotation != null)
                    {
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherStudentAnnotation.Id);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.TeacherId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teacher_id` = @teacherId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teacher_id` = @teacherId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherId", teacherStudentAnnotation.TeacherId);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.StudentId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `student_id` = @studentId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `student_id` = @studentId ");
                            }
                            cmd.Parameters.AddWithValue("@studentId", teacherStudentAnnotation.StudentId);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.TeacherPlanId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teacher_plan_id` = @teacherPlanId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teacher_plan_id` = @teacherPlanId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherPlanId", teacherStudentAnnotation.TeacherPlanId);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.Annotation))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `annotation` = @annotation ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `annotation` = @annotation ");
                            }
                            cmd.Parameters.AddWithValue("@annotation", teacherStudentAnnotation.Annotation);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherStudentAnnotation.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherStudentAnnotation.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherStudentAnnotation.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                        cmd.Parameters.AddWithValue("@fromRow", fromRow);
                        cmd.Parameters.AddWithValue("@limit", limit);
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    teacherStudentAnnotation.Id = reader.GetValue(0).ToString();
                                else
                                    teacherStudentAnnotation.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherStudentAnnotation.TeacherId = reader.GetValue(1).ToString();
                                else
                                    teacherStudentAnnotation.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    teacherStudentAnnotation.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherStudentAnnotation.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherStudentAnnotation.TeacherPlanId = reader.GetValue(3).ToString();
                                else
                                    teacherStudentAnnotation.TeacherPlanId = null;
                                if (!reader.IsDBNull(4))
                                    teacherStudentAnnotation.Annotation = reader.GetValue(4).ToString();
                                else
                                    teacherStudentAnnotation.Annotation = null;
                                if (!reader.IsDBNull(5))
                                    teacherStudentAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    teacherStudentAnnotation.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    teacherStudentAnnotation.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    teacherStudentAnnotation.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    teacherStudentAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    teacherStudentAnnotation.UpdatedAt = null;
                                response.Add(teacherStudentAnnotation.Id, teacherStudentAnnotation);
                                teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherStudentAnnotationModel>();
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
                response = new Dictionary<string, TeacherStudentAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherStudentAnnotationModel> ReadD(string connectionString, List<TeacherStudentAnnotationModel> teacherStudentAnnotationWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherStudentAnnotationModel> response = new Dictionary<string, TeacherStudentAnnotationModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `teacher_plan_id`, `annotation`, `modified_by`, `created_at`, `updated_at`  FROM teacher_student_annotations ";
                if (teacherStudentAnnotationWhere != null && teacherStudentAnnotationWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherStudentAnnotationWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherStudentAnnotationWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, teacherStudentAnnotationWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, teacherStudentAnnotationWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].TeacherPlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_plan_id` = @teacherPlanId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherPlanId" + i, teacherStudentAnnotationWhere[i].TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].Annotation))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `annotation` = @annotation" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@annotation" + i, teacherStudentAnnotationWhere[i].Annotation);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherStudentAnnotationWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherStudentAnnotationWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherStudentAnnotationWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherStudentAnnotationWhere.Count - 1)
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
                            TeacherStudentAnnotationModel teacherStudentAnnotation = new TeacherStudentAnnotationModel(); if (!reader.IsDBNull(0))
                                teacherStudentAnnotation.Id = reader.GetValue(0).ToString();
                            else
                                teacherStudentAnnotation.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherStudentAnnotation.TeacherId = reader.GetValue(1).ToString();
                            else
                                teacherStudentAnnotation.TeacherId = null;
                            if (!reader.IsDBNull(2))
                                teacherStudentAnnotation.StudentId = reader.GetValue(2).ToString();
                            else
                                teacherStudentAnnotation.StudentId = null;
                            if (!reader.IsDBNull(3))
                                teacherStudentAnnotation.TeacherPlanId = reader.GetValue(3).ToString();
                            else
                                teacherStudentAnnotation.TeacherPlanId = null;
                            if (!reader.IsDBNull(4))
                                teacherStudentAnnotation.Annotation = reader.GetValue(4).ToString();
                            else
                                teacherStudentAnnotation.Annotation = null;
                            if (!reader.IsDBNull(5))
                                teacherStudentAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                teacherStudentAnnotation.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                teacherStudentAnnotation.CreatedAt = reader.GetValue(6).ToString();
                            else
                                teacherStudentAnnotation.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                teacherStudentAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                teacherStudentAnnotation.UpdatedAt = null;
                            response.Add(teacherStudentAnnotation.Id, teacherStudentAnnotation);
                            teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherStudentAnnotationModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherStudentAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherStudentAnnotationModel> ReadD(string connectionString, List<TeacherStudentAnnotationModel> teacherStudentAnnotationWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherStudentAnnotationModel> response = new Dictionary<string, TeacherStudentAnnotationModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `teacher_plan_id`, `annotation`, `modified_by`, `created_at`, `updated_at`  FROM teacher_student_annotations ";
                if (teacherStudentAnnotationWhere != null && teacherStudentAnnotationWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherStudentAnnotationWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherStudentAnnotationWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, teacherStudentAnnotationWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, teacherStudentAnnotationWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].TeacherPlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_plan_id` = @teacherPlanId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherPlanId" + i, teacherStudentAnnotationWhere[i].TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].Annotation))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `annotation` = @annotation" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@annotation" + i, teacherStudentAnnotationWhere[i].Annotation);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherStudentAnnotationWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherStudentAnnotationWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherStudentAnnotationWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherStudentAnnotationWhere.Count - 1)
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
                            TeacherStudentAnnotationModel teacherStudentAnnotation = new TeacherStudentAnnotationModel(); if (!reader.IsDBNull(0))
                                teacherStudentAnnotation.Id = reader.GetValue(0).ToString();
                            else
                                teacherStudentAnnotation.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherStudentAnnotation.TeacherId = reader.GetValue(1).ToString();
                            else
                                teacherStudentAnnotation.TeacherId = null;
                            if (!reader.IsDBNull(2))
                                teacherStudentAnnotation.StudentId = reader.GetValue(2).ToString();
                            else
                                teacherStudentAnnotation.StudentId = null;
                            if (!reader.IsDBNull(3))
                                teacherStudentAnnotation.TeacherPlanId = reader.GetValue(3).ToString();
                            else
                                teacherStudentAnnotation.TeacherPlanId = null;
                            if (!reader.IsDBNull(4))
                                teacherStudentAnnotation.Annotation = reader.GetValue(4).ToString();
                            else
                                teacherStudentAnnotation.Annotation = null;
                            if (!reader.IsDBNull(5))
                                teacherStudentAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                teacherStudentAnnotation.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                teacherStudentAnnotation.CreatedAt = reader.GetValue(6).ToString();
                            else
                                teacherStudentAnnotation.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                teacherStudentAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                teacherStudentAnnotation.UpdatedAt = null;
                            response.Add(teacherStudentAnnotation.Id, teacherStudentAnnotation);
                            teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherStudentAnnotationModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherStudentAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherStudentAnnotationModel> Read(string connectionString, TeacherStudentAnnotationModel teacherStudentAnnotation, bool firstOne, bool orderByLastDate, TeacherStudentAnnotationModel teacherStudentAnnotationWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherStudentAnnotationModel> response = new List<TeacherStudentAnnotationModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `teacher_plan_id`, `annotation`, `modified_by`, `created_at`, `updated_at`  FROM teacher_student_annotations ";
                {
                    if (teacherStudentAnnotation != null)
                    {
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherStudentAnnotation.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", teacherStudentAnnotation.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", teacherStudentAnnotation.StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.TeacherPlanId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teacher_plan_id` = @teacherPlanId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teacher_plan_id` = @teacherPlanId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherPlanId", teacherStudentAnnotation.TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.Annotation))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `annotation` = @annotation ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `annotation` = @annotation ");
                            }
                            cmd.Parameters.AddWithValue("@annotation", teacherStudentAnnotation.Annotation);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherStudentAnnotation.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherStudentAnnotation.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherStudentAnnotation.UpdatedAt);
                        }
                    }
                    if (teacherStudentAnnotationWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherStudentAnnotationWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + teacherStudentAnnotationWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + teacherStudentAnnotationWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.TeacherPlanId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teacher_plan_id` LIKE @teacherPlanIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teacher_plan_id` LIKE @teacherPlanIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@teacherPlanIdWhereLike", "%" + teacherStudentAnnotationWhereLike.TeacherPlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.Annotation))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `annotation` LIKE @annotationWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `annotation` LIKE @annotationWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@annotationWhereLike", "%" + teacherStudentAnnotationWhereLike.Annotation + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherStudentAnnotationWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherStudentAnnotationWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherStudentAnnotationWhereLike.UpdatedAt + "%");
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
                                teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                                if (!reader.IsDBNull(0))
                                    teacherStudentAnnotation.Id = reader.GetValue(0).ToString();
                                else
                                    teacherStudentAnnotation.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherStudentAnnotation.TeacherId = reader.GetValue(1).ToString();
                                else
                                    teacherStudentAnnotation.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    teacherStudentAnnotation.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherStudentAnnotation.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherStudentAnnotation.TeacherPlanId = reader.GetValue(3).ToString();
                                else
                                    teacherStudentAnnotation.TeacherPlanId = null;
                                if (!reader.IsDBNull(4))
                                    teacherStudentAnnotation.Annotation = reader.GetValue(4).ToString();
                                else
                                    teacherStudentAnnotation.Annotation = null;
                                if (!reader.IsDBNull(5))
                                    teacherStudentAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    teacherStudentAnnotation.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    teacherStudentAnnotation.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    teacherStudentAnnotation.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    teacherStudentAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    teacherStudentAnnotation.UpdatedAt = null;
                                response.Add(teacherStudentAnnotation);
                                teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherStudentAnnotationModel>();
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
                response = new List<TeacherStudentAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherStudentAnnotationModel> Read(string connectionString, TeacherStudentAnnotationModel teacherStudentAnnotation, bool orderByLastDate, int fromRow, int limit, TeacherStudentAnnotationModel teacherStudentAnnotationWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherStudentAnnotationModel> response = new List<TeacherStudentAnnotationModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `teacher_plan_id`, `annotation`, `modified_by`, `created_at`, `updated_at`  FROM teacher_student_annotations ";
                {
                    if (teacherStudentAnnotation != null)
                    {
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherStudentAnnotation.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", teacherStudentAnnotation.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", teacherStudentAnnotation.StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.TeacherPlanId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teacher_plan_id` = @teacherPlanId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teacher_plan_id` = @teacherPlanId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherPlanId", teacherStudentAnnotation.TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.Annotation))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `annotation` = @annotation ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `annotation` = @annotation ");
                            }
                            cmd.Parameters.AddWithValue("@annotation", teacherStudentAnnotation.Annotation);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherStudentAnnotation.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherStudentAnnotation.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherStudentAnnotation.UpdatedAt);
                        }
                    }
                    if (teacherStudentAnnotationWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherStudentAnnotationWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + teacherStudentAnnotationWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + teacherStudentAnnotationWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.TeacherPlanId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teacher_plan_id` LIKE @teacherPlanIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teacher_plan_id` LIKE @teacherPlanIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@teacherPlanIdWhereLike", "%" + teacherStudentAnnotationWhereLike.TeacherPlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.Annotation))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `annotation` LIKE @annotationWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `annotation` LIKE @annotationWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@annotationWhereLike", "%" + teacherStudentAnnotationWhereLike.Annotation + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherStudentAnnotationWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherStudentAnnotationWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherStudentAnnotationWhereLike.UpdatedAt + "%");
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
                                teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                                if (!reader.IsDBNull(0))
                                    teacherStudentAnnotation.Id = reader.GetValue(0).ToString();
                                else
                                    teacherStudentAnnotation.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherStudentAnnotation.TeacherId = reader.GetValue(1).ToString();
                                else
                                    teacherStudentAnnotation.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    teacherStudentAnnotation.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherStudentAnnotation.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherStudentAnnotation.TeacherPlanId = reader.GetValue(3).ToString();
                                else
                                    teacherStudentAnnotation.TeacherPlanId = null;
                                if (!reader.IsDBNull(4))
                                    teacherStudentAnnotation.Annotation = reader.GetValue(4).ToString();
                                else
                                    teacherStudentAnnotation.Annotation = null;
                                if (!reader.IsDBNull(5))
                                    teacherStudentAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    teacherStudentAnnotation.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    teacherStudentAnnotation.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    teacherStudentAnnotation.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    teacherStudentAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    teacherStudentAnnotation.UpdatedAt = null;
                                response.Add(teacherStudentAnnotation);
                                teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherStudentAnnotationModel>();
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
                response = new List<TeacherStudentAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherStudentAnnotationModel> Read(string connectionString, TeacherStudentAnnotationModel teacherStudentAnnotation, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherStudentAnnotationModel> response = new List<TeacherStudentAnnotationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `teacher_plan_id`, `annotation`, `modified_by`, `created_at`, `updated_at`  FROM teacher_student_annotations ";
                {
                    if (teacherStudentAnnotation != null)
                    {
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherStudentAnnotation.Id);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.TeacherId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teacher_id` = @teacherId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teacher_id` = @teacherId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherId", teacherStudentAnnotation.TeacherId);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.StudentId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `student_id` = @studentId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `student_id` = @studentId ");
                            }
                            cmd.Parameters.AddWithValue("@studentId", teacherStudentAnnotation.StudentId);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.TeacherPlanId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teacher_plan_id` = @teacherPlanId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teacher_plan_id` = @teacherPlanId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherPlanId", teacherStudentAnnotation.TeacherPlanId);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.Annotation))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `annotation` = @annotation ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `annotation` = @annotation ");
                            }
                            cmd.Parameters.AddWithValue("@annotation", teacherStudentAnnotation.Annotation);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherStudentAnnotation.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherStudentAnnotation.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherStudentAnnotation.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        if (firstOne)
                            sql = string.Concat(sql, " LIMIT 1 ");
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    teacherStudentAnnotation.Id = reader.GetValue(0).ToString();
                                else
                                    teacherStudentAnnotation.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherStudentAnnotation.TeacherId = reader.GetValue(1).ToString();
                                else
                                    teacherStudentAnnotation.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    teacherStudentAnnotation.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherStudentAnnotation.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherStudentAnnotation.TeacherPlanId = reader.GetValue(3).ToString();
                                else
                                    teacherStudentAnnotation.TeacherPlanId = null;
                                if (!reader.IsDBNull(4))
                                    teacherStudentAnnotation.Annotation = reader.GetValue(4).ToString();
                                else
                                    teacherStudentAnnotation.Annotation = null;
                                if (!reader.IsDBNull(5))
                                    teacherStudentAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    teacherStudentAnnotation.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    teacherStudentAnnotation.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    teacherStudentAnnotation.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    teacherStudentAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    teacherStudentAnnotation.UpdatedAt = null;
                                response.Add(teacherStudentAnnotation);
                                teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherStudentAnnotationModel>();
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
                response = new List<TeacherStudentAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherStudentAnnotationModel> Read(string connectionString, TeacherStudentAnnotationModel teacherStudentAnnotation, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherStudentAnnotationModel> response = new List<TeacherStudentAnnotationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `teacher_plan_id`, `annotation`, `modified_by`, `created_at`, `updated_at`  FROM teacher_student_annotations ";
                {
                    if (teacherStudentAnnotation != null)
                    {
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherStudentAnnotation.Id);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.TeacherId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teacher_id` = @teacherId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teacher_id` = @teacherId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherId", teacherStudentAnnotation.TeacherId);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.StudentId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `student_id` = @studentId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `student_id` = @studentId ");
                            }
                            cmd.Parameters.AddWithValue("@studentId", teacherStudentAnnotation.StudentId);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.TeacherPlanId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teacher_plan_id` = @teacherPlanId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teacher_plan_id` = @teacherPlanId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherPlanId", teacherStudentAnnotation.TeacherPlanId);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.Annotation))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `annotation` = @annotation ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `annotation` = @annotation ");
                            }
                            cmd.Parameters.AddWithValue("@annotation", teacherStudentAnnotation.Annotation);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherStudentAnnotation.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherStudentAnnotation.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(teacherStudentAnnotation.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherStudentAnnotation.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                        cmd.Parameters.AddWithValue("@fromRow", fromRow);
                        cmd.Parameters.AddWithValue("@limit", limit);
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    teacherStudentAnnotation.Id = reader.GetValue(0).ToString();
                                else
                                    teacherStudentAnnotation.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherStudentAnnotation.TeacherId = reader.GetValue(1).ToString();
                                else
                                    teacherStudentAnnotation.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    teacherStudentAnnotation.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherStudentAnnotation.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherStudentAnnotation.TeacherPlanId = reader.GetValue(3).ToString();
                                else
                                    teacherStudentAnnotation.TeacherPlanId = null;
                                if (!reader.IsDBNull(4))
                                    teacherStudentAnnotation.Annotation = reader.GetValue(4).ToString();
                                else
                                    teacherStudentAnnotation.Annotation = null;
                                if (!reader.IsDBNull(5))
                                    teacherStudentAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    teacherStudentAnnotation.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    teacherStudentAnnotation.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    teacherStudentAnnotation.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    teacherStudentAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    teacherStudentAnnotation.UpdatedAt = null;
                                response.Add(teacherStudentAnnotation);
                                teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherStudentAnnotationModel>();
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
                response = new List<TeacherStudentAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherStudentAnnotationModel> Read(string connectionString, List<TeacherStudentAnnotationModel> teacherStudentAnnotationWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherStudentAnnotationModel> response = new List<TeacherStudentAnnotationModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `teacher_plan_id`, `annotation`, `modified_by`, `created_at`, `updated_at`  FROM teacher_student_annotations ";
                if (teacherStudentAnnotationWhere != null && teacherStudentAnnotationWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherStudentAnnotationWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherStudentAnnotationWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, teacherStudentAnnotationWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, teacherStudentAnnotationWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].TeacherPlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_plan_id` = @teacherPlanId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherPlanId" + i, teacherStudentAnnotationWhere[i].TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].Annotation))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `annotation` = @annotation" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@annotation" + i, teacherStudentAnnotationWhere[i].Annotation);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherStudentAnnotationWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherStudentAnnotationWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherStudentAnnotationWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherStudentAnnotationWhere.Count - 1)
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
                            TeacherStudentAnnotationModel teacherStudentAnnotation = new TeacherStudentAnnotationModel(); if (!reader.IsDBNull(0))
                                teacherStudentAnnotation.Id = reader.GetValue(0).ToString();
                            else
                                teacherStudentAnnotation.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherStudentAnnotation.TeacherId = reader.GetValue(1).ToString();
                            else
                                teacherStudentAnnotation.TeacherId = null;
                            if (!reader.IsDBNull(2))
                                teacherStudentAnnotation.StudentId = reader.GetValue(2).ToString();
                            else
                                teacherStudentAnnotation.StudentId = null;
                            if (!reader.IsDBNull(3))
                                teacherStudentAnnotation.TeacherPlanId = reader.GetValue(3).ToString();
                            else
                                teacherStudentAnnotation.TeacherPlanId = null;
                            if (!reader.IsDBNull(4))
                                teacherStudentAnnotation.Annotation = reader.GetValue(4).ToString();
                            else
                                teacherStudentAnnotation.Annotation = null;
                            if (!reader.IsDBNull(5))
                                teacherStudentAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                teacherStudentAnnotation.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                teacherStudentAnnotation.CreatedAt = reader.GetValue(6).ToString();
                            else
                                teacherStudentAnnotation.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                teacherStudentAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                teacherStudentAnnotation.UpdatedAt = null;
                            response.Add(teacherStudentAnnotation);
                            teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherStudentAnnotationModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherStudentAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherStudentAnnotationModel> Read(string connectionString, List<TeacherStudentAnnotationModel> teacherStudentAnnotationWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherStudentAnnotationModel> response = new List<TeacherStudentAnnotationModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `teacher_plan_id`, `annotation`, `modified_by`, `created_at`, `updated_at`  FROM teacher_student_annotations ";
                if (teacherStudentAnnotationWhere != null && teacherStudentAnnotationWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherStudentAnnotationWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherStudentAnnotationWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, teacherStudentAnnotationWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, teacherStudentAnnotationWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].TeacherPlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_plan_id` = @teacherPlanId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherPlanId" + i, teacherStudentAnnotationWhere[i].TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].Annotation))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `annotation` = @annotation" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@annotation" + i, teacherStudentAnnotationWhere[i].Annotation);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherStudentAnnotationWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherStudentAnnotationWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherStudentAnnotationWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherStudentAnnotationWhere.Count - 1)
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
                            TeacherStudentAnnotationModel teacherStudentAnnotation = new TeacherStudentAnnotationModel(); if (!reader.IsDBNull(0))
                                teacherStudentAnnotation.Id = reader.GetValue(0).ToString();
                            else
                                teacherStudentAnnotation.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherStudentAnnotation.TeacherId = reader.GetValue(1).ToString();
                            else
                                teacherStudentAnnotation.TeacherId = null;
                            if (!reader.IsDBNull(2))
                                teacherStudentAnnotation.StudentId = reader.GetValue(2).ToString();
                            else
                                teacherStudentAnnotation.StudentId = null;
                            if (!reader.IsDBNull(3))
                                teacherStudentAnnotation.TeacherPlanId = reader.GetValue(3).ToString();
                            else
                                teacherStudentAnnotation.TeacherPlanId = null;
                            if (!reader.IsDBNull(4))
                                teacherStudentAnnotation.Annotation = reader.GetValue(4).ToString();
                            else
                                teacherStudentAnnotation.Annotation = null;
                            if (!reader.IsDBNull(5))
                                teacherStudentAnnotation.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                teacherStudentAnnotation.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                teacherStudentAnnotation.CreatedAt = reader.GetValue(6).ToString();
                            else
                                teacherStudentAnnotation.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                teacherStudentAnnotation.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                teacherStudentAnnotation.UpdatedAt = null;
                            response.Add(teacherStudentAnnotation);
                            teacherStudentAnnotation = new TeacherStudentAnnotationModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherStudentAnnotationModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherStudentAnnotationModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherStudentAnnotationModel> Create(string connectionString, TeacherStudentAnnotationModel teacherStudentAnnotation)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherStudentAnnotationModel> response = new List<TeacherStudentAnnotationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO teacher_student_annotations (";
                string values = "(";
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.Id))
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
                    cmd.Parameters.AddWithValue("@id", teacherStudentAnnotation.Id);
                }
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.TeacherId))
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
                    cmd.Parameters.AddWithValue("@teacherId", teacherStudentAnnotation.TeacherId);
                }
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.StudentId))
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
                    cmd.Parameters.AddWithValue("@studentId", teacherStudentAnnotation.StudentId);
                }
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.TeacherPlanId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `teacher_plan_id` ");
                        values = String.Concat(values, ", @teacherPlanId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `teacher_plan_id` ");
                        values = String.Concat(values, " @teacherPlanId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teacherPlanId", teacherStudentAnnotation.TeacherPlanId);
                }
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.Annotation))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `annotation` ");
                        values = String.Concat(values, ", @annotation");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `annotation` ");
                        values = String.Concat(values, " @annotation");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@annotation", teacherStudentAnnotation.Annotation);
                }
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherStudentAnnotation.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", teacherStudentAnnotation.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", teacherStudentAnnotation.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                teacherStudentAnnotation.Id = cmd.LastInsertedId.ToString();
                response.Add(teacherStudentAnnotation);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherStudentAnnotationModel>();
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
        public List<TeacherStudentAnnotationModel> Create(string connectionString, List<TeacherStudentAnnotationModel> teacherStudentAnnotationList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherStudentAnnotationModel> response = new List<TeacherStudentAnnotationModel>();
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
                        for (int i = 0; i < teacherStudentAnnotationList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO teacher_student_annotations (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, teacherStudentAnnotationList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].TeacherId))
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
                                    cmd.Parameters.AddWithValue("@teacherId" + i, teacherStudentAnnotationList[i].TeacherId);
                                }
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].StudentId))
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
                                    cmd.Parameters.AddWithValue("@studentId" + i, teacherStudentAnnotationList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].TeacherPlanId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `teacher_plan_id` ");
                                        values = String.Concat(values, ", @teacherPlanId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `teacher_plan_id` ");
                                        values = String.Concat(values, "@teacherPlanId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teacherPlanId" + i, teacherStudentAnnotationList[i].TeacherPlanId);
                                }
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].Annotation))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `annotation` ");
                                        values = String.Concat(values, ", @annotation" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `annotation` ");
                                        values = String.Concat(values, "@annotation" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@annotation" + i, teacherStudentAnnotationList[i].Annotation);
                                }
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherStudentAnnotationList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherStudentAnnotationList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherStudentAnnotationList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(teacherStudentAnnotationList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherStudentAnnotationModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherStudentAnnotationModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherStudentAnnotationModel>();
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

        public List<TeacherStudentAnnotationModel> Update(string connectionString, TeacherStudentAnnotationModel teacherStudentAnnotation, TeacherStudentAnnotationModel teacherStudentAnnotationWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherStudentAnnotationModel> response = new List<TeacherStudentAnnotationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE teacher_student_annotations SET ";
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", teacherStudentAnnotation.Id);
                }
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.TeacherId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `teacher_id` = @teacherId");
                    else
                        sql = String.Concat(sql, " `teacher_id` = @teacherId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teacherId", teacherStudentAnnotation.TeacherId);
                }
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.StudentId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `student_id` = @studentId");
                    else
                        sql = String.Concat(sql, " `student_id` = @studentId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@studentId", teacherStudentAnnotation.StudentId);
                }
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.TeacherPlanId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `teacher_plan_id` = @teacherPlanId");
                    else
                        sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teacherPlanId", teacherStudentAnnotation.TeacherPlanId);
                }
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.Annotation))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `annotation` = @annotation");
                    else
                        sql = String.Concat(sql, " `annotation` = @annotation");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@annotation", teacherStudentAnnotation.Annotation);
                }
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherStudentAnnotation.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", teacherStudentAnnotation.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherStudentAnnotation.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", teacherStudentAnnotation.UpdatedAt);
                }
                if (teacherStudentAnnotationWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", teacherStudentAnnotationWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere.TeacherId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_id` = @teacherIdWhere");
                        else
                            sql = String.Concat(sql, " `teacher_id` = @teacherIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherIdWhere", teacherStudentAnnotationWhere.TeacherId);
                    }
                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentIdWhere", teacherStudentAnnotationWhere.StudentId);
                    }
                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere.TeacherPlanId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_plan_id` = @teacherPlanIdWhere");
                        else
                            sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherPlanIdWhere", teacherStudentAnnotationWhere.TeacherPlanId);
                    }
                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere.Annotation))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `annotation` = @annotationWhere");
                        else
                            sql = String.Concat(sql, " `annotation` = @annotationWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@annotationWhere", teacherStudentAnnotationWhere.Annotation);
                    }
                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", teacherStudentAnnotationWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", teacherStudentAnnotationWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", teacherStudentAnnotationWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(teacherStudentAnnotation);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherStudentAnnotationModel>();
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

        public List<TeacherStudentAnnotationModel> Update(string connectionString, List<TeacherStudentAnnotationModel> teacherStudentAnnotationList, List<TeacherStudentAnnotationModel> teacherStudentAnnotationWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherStudentAnnotationModel> response = new List<TeacherStudentAnnotationModel>();
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
                        for (int i = 0; i < teacherStudentAnnotationList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE teacher_student_annotations SET ";
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, teacherStudentAnnotationList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].TeacherId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `teacher_id` = @teacherId" + i);
                                    else
                                        sql = String.Concat(sql, " `teacher_id` = @teacherId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teacherId" + i, teacherStudentAnnotationList[i].TeacherId);
                                }
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].StudentId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `student_id` = @studentId" + i);
                                    else
                                        sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@studentId" + i, teacherStudentAnnotationList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].TeacherPlanId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `teacher_plan_id` = @teacherPlanId" + i);
                                    else
                                        sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teacherPlanId" + i, teacherStudentAnnotationList[i].TeacherPlanId);
                                }
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].Annotation))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `annotation` = @annotation" + i);
                                    else
                                        sql = String.Concat(sql, " `annotation` = @annotation" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@annotation" + i, teacherStudentAnnotationList[i].Annotation);
                                }
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherStudentAnnotationList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherStudentAnnotationList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherStudentAnnotationList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherStudentAnnotationList[i].UpdatedAt);
                                }
                                if (teacherStudentAnnotationWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, teacherStudentAnnotationWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereList[i].TeacherId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_id` = @teacherIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_id` = @teacherIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherIdWhere" + i, teacherStudentAnnotationWhereList[i].TeacherId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereList[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentIdWhere" + i, teacherStudentAnnotationWhereList[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereList[i].TeacherPlanId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_plan_id` = @teacherPlanIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherPlanIdWhere" + i, teacherStudentAnnotationWhereList[i].TeacherPlanId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereList[i].Annotation))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `annotation` = @annotationWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `annotation` = @annotationWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@annotationWhere" + i, teacherStudentAnnotationWhereList[i].Annotation);
                                    }
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, teacherStudentAnnotationWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, teacherStudentAnnotationWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotationWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, teacherStudentAnnotationWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherStudentAnnotationList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherStudentAnnotationModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherStudentAnnotationModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherStudentAnnotationModel>();
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

        public List<TeacherStudentAnnotationModel> Delete(string connectionString, TeacherStudentAnnotationModel teacherStudentAnnotation)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherStudentAnnotationModel> response = new List<TeacherStudentAnnotationModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM teacher_student_annotations  ";
                if (teacherStudentAnnotation != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(teacherStudentAnnotation.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", teacherStudentAnnotation.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherStudentAnnotation.TeacherId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_id` = @teacherId");
                        else
                            sql = String.Concat(sql, " `teacher_id` = @teacherId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherId", teacherStudentAnnotation.TeacherId);
                    }
                    if (!String.IsNullOrEmpty(teacherStudentAnnotation.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentId");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentId", teacherStudentAnnotation.StudentId);
                    }
                    if (!String.IsNullOrEmpty(teacherStudentAnnotation.TeacherPlanId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_plan_id` = @teacherPlanId");
                        else
                            sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherPlanId", teacherStudentAnnotation.TeacherPlanId);
                    }
                    if (!String.IsNullOrEmpty(teacherStudentAnnotation.Annotation))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `annotation` = @annotation");
                        else
                            sql = String.Concat(sql, " `annotation` = @annotation");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@annotation", teacherStudentAnnotation.Annotation);
                    }
                    if (!String.IsNullOrEmpty(teacherStudentAnnotation.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", teacherStudentAnnotation.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherStudentAnnotation.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", teacherStudentAnnotation.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherStudentAnnotation.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", teacherStudentAnnotation.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(teacherStudentAnnotation);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherStudentAnnotationModel>();
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

        public List<TeacherStudentAnnotationModel> Delete(string connectionString, List<TeacherStudentAnnotationModel> teacherStudentAnnotation)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherStudentAnnotationModel> response = new List<TeacherStudentAnnotationModel>();
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
                        for (int i = 0; i < teacherStudentAnnotation.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM teacher_student_annotations  ";
                                if (teacherStudentAnnotation[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotation[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, teacherStudentAnnotation[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotation[i].TeacherId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_id` = @teacherId" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_id` = @teacherId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherId" + i, teacherStudentAnnotation[i].TeacherId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotation[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentId" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentId" + i, teacherStudentAnnotation[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotation[i].TeacherPlanId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_plan_id` = @teacherPlanId" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherPlanId" + i, teacherStudentAnnotation[i].TeacherPlanId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotation[i].Annotation))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `annotation` = @annotation" + i);
                                        else
                                            sql = String.Concat(sql, " `annotation` = @annotation" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@annotation" + i, teacherStudentAnnotation[i].Annotation);
                                    }
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotation[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherStudentAnnotation[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotation[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, teacherStudentAnnotation[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherStudentAnnotation[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, teacherStudentAnnotation[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherStudentAnnotation[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherStudentAnnotationModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherStudentAnnotationModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherStudentAnnotationModel>();
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
