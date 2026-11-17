using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Teachers;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    class TeacherGroupStudentDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, TeacherGroupStudentModel> ReadD(string connectionString, TeacherGroupStudentModel teacherGroupStudent, bool firstOne, bool orderByLastDate, TeacherGroupStudentModel teacherGroupStudentWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherGroupStudentModel> response = new Dictionary<string, TeacherGroupStudentModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_group_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_group_students ";
                {
                    if (teacherGroupStudent != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroupStudent.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherGroupStudent.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.TeacherGroupId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teacher_group_id` = @teacherGroupId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teacher_group_id` = @teacherGroupId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherGroupId", teacherGroupStudent.TeacherGroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", teacherGroupStudent.StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherGroupStudent.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherGroupStudent.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherGroupStudent.UpdatedAt);
                        }
                    }
                    if (teacherGroupStudentWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherGroupStudentWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.TeacherGroupId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teacher_group_id` LIKE @teacherGroupIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teacher_group_id` LIKE @teacherGroupIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@teacherGroupIdWhereLike", "%" + teacherGroupStudentWhereLike.TeacherGroupId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + teacherGroupStudentWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherGroupStudentWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherGroupStudentWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherGroupStudentWhereLike.UpdatedAt + "%");
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
                                teacherGroupStudent = new TeacherGroupStudentModel();
                                if (!reader.IsDBNull(0))
                                    teacherGroupStudent.Id = reader.GetValue(0).ToString();
                                else
                                    teacherGroupStudent.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherGroupStudent.TeacherGroupId = reader.GetValue(1).ToString();
                                else
                                    teacherGroupStudent.TeacherGroupId = null;
                                if (!reader.IsDBNull(2))
                                    teacherGroupStudent.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherGroupStudent.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherGroupStudent.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    teacherGroupStudent.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    teacherGroupStudent.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    teacherGroupStudent.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    teacherGroupStudent.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    teacherGroupStudent.UpdatedAt = null;
                                response.Add(teacherGroupStudent.Id, teacherGroupStudent);
                                teacherGroupStudent = new TeacherGroupStudentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherGroupStudentModel>();
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
                response = new Dictionary<string, TeacherGroupStudentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherGroupStudentModel> ReadD(string connectionString, TeacherGroupStudentModel teacherGroupStudent, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherGroupStudentModel> response = new Dictionary<string, TeacherGroupStudentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_group_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_group_students ";
                {
                    if (teacherGroupStudent != null)
                    {
                        if (!string.IsNullOrEmpty(teacherGroupStudent.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherGroupStudent.Id);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.TeacherGroupId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teacher_group_id` = @teacherGroupId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teacher_group_id` = @teacherGroupId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherGroupId", teacherGroupStudent.TeacherGroupId);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", teacherGroupStudent.StudentId);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherGroupStudent.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherGroupStudent.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherGroupStudent.UpdatedAt);
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
                                    teacherGroupStudent.Id = reader.GetValue(0).ToString();
                                else
                                    teacherGroupStudent.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherGroupStudent.TeacherGroupId = reader.GetValue(1).ToString();
                                else
                                    teacherGroupStudent.TeacherGroupId = null;
                                if (!reader.IsDBNull(2))
                                    teacherGroupStudent.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherGroupStudent.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherGroupStudent.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    teacherGroupStudent.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    teacherGroupStudent.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    teacherGroupStudent.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    teacherGroupStudent.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    teacherGroupStudent.UpdatedAt = null;
                                response.Add(teacherGroupStudent.Id, teacherGroupStudent);
                                teacherGroupStudent = new TeacherGroupStudentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherGroupStudentModel>();
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
                response = new Dictionary<string, TeacherGroupStudentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherGroupStudentModel> ReadD(string connectionString, TeacherGroupStudentModel teacherGroupStudent, bool orderByLastDate, int fromRow, int limit, TeacherGroupStudentModel teacherGroupStudentWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherGroupStudentModel> response = new Dictionary<string, TeacherGroupStudentModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_group_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_group_students ";
                {
                    if (teacherGroupStudent != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroupStudent.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherGroupStudent.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.TeacherGroupId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teacher_group_id` = @teacherGroupId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teacher_group_id` = @teacherGroupId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherGroupId", teacherGroupStudent.TeacherGroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", teacherGroupStudent.StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherGroupStudent.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherGroupStudent.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherGroupStudent.UpdatedAt);
                        }
                    }
                    if (teacherGroupStudentWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherGroupStudentWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.TeacherGroupId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teacher_group_id` LIKE @teacherGroupIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teacher_group_id` LIKE @teacherGroupIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@teacherGroupIdWhereLike", "%" + teacherGroupStudentWhereLike.TeacherGroupId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + teacherGroupStudentWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherGroupStudentWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherGroupStudentWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherGroupStudentWhereLike.UpdatedAt + "%");
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
                                teacherGroupStudent = new TeacherGroupStudentModel();
                                if (!reader.IsDBNull(0))
                                    teacherGroupStudent.Id = reader.GetValue(0).ToString();
                                else
                                    teacherGroupStudent.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherGroupStudent.TeacherGroupId = reader.GetValue(1).ToString();
                                else
                                    teacherGroupStudent.TeacherGroupId = null;
                                if (!reader.IsDBNull(2))
                                    teacherGroupStudent.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherGroupStudent.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherGroupStudent.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    teacherGroupStudent.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    teacherGroupStudent.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    teacherGroupStudent.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    teacherGroupStudent.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    teacherGroupStudent.UpdatedAt = null;
                                response.Add(teacherGroupStudent.Id, teacherGroupStudent);
                                teacherGroupStudent = new TeacherGroupStudentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherGroupStudentModel>();
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
                response = new Dictionary<string, TeacherGroupStudentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherGroupStudentModel> ReadD(string connectionString, TeacherGroupStudentModel teacherGroupStudent, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherGroupStudentModel> response = new Dictionary<string, TeacherGroupStudentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_group_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_group_students ";
                {
                    if (teacherGroupStudent != null)
                    {
                        if (!string.IsNullOrEmpty(teacherGroupStudent.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherGroupStudent.Id);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.TeacherGroupId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teacher_group_id` = @teacherGroupId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teacher_group_id` = @teacherGroupId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherGroupId", teacherGroupStudent.TeacherGroupId);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", teacherGroupStudent.StudentId);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherGroupStudent.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherGroupStudent.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherGroupStudent.UpdatedAt);
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
                                    teacherGroupStudent.Id = reader.GetValue(0).ToString();
                                else
                                    teacherGroupStudent.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherGroupStudent.TeacherGroupId = reader.GetValue(1).ToString();
                                else
                                    teacherGroupStudent.TeacherGroupId = null;
                                if (!reader.IsDBNull(2))
                                    teacherGroupStudent.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherGroupStudent.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherGroupStudent.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    teacherGroupStudent.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    teacherGroupStudent.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    teacherGroupStudent.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    teacherGroupStudent.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    teacherGroupStudent.UpdatedAt = null;
                                response.Add(teacherGroupStudent.Id, teacherGroupStudent);
                                teacherGroupStudent = new TeacherGroupStudentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherGroupStudentModel>();
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
                response = new Dictionary<string, TeacherGroupStudentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherGroupStudentModel> ReadD(string connectionString, List<TeacherGroupStudentModel> teacherGroupStudentWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherGroupStudentModel> response = new Dictionary<string, TeacherGroupStudentModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_group_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_group_students ";
                if (teacherGroupStudentWhere != null && teacherGroupStudentWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherGroupStudentWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherGroupStudentWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].TeacherGroupId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_group_id` = @teacherGroupId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherGroupId" + i, teacherGroupStudentWhere[i].TeacherGroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, teacherGroupStudentWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherGroupStudentWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherGroupStudentWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherGroupStudentWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherGroupStudentWhere.Count - 1)
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
                            TeacherGroupStudentModel teacherGroupStudent = new TeacherGroupStudentModel(); if (!reader.IsDBNull(0))
                                teacherGroupStudent.Id = reader.GetValue(0).ToString();
                            else
                                teacherGroupStudent.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherGroupStudent.TeacherGroupId = reader.GetValue(1).ToString();
                            else
                                teacherGroupStudent.TeacherGroupId = null;
                            if (!reader.IsDBNull(2))
                                teacherGroupStudent.StudentId = reader.GetValue(2).ToString();
                            else
                                teacherGroupStudent.StudentId = null;
                            if (!reader.IsDBNull(3))
                                teacherGroupStudent.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                teacherGroupStudent.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                teacherGroupStudent.CreatedAt = reader.GetValue(4).ToString();
                            else
                                teacherGroupStudent.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                teacherGroupStudent.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                teacherGroupStudent.UpdatedAt = null;
                            response.Add(teacherGroupStudent.Id, teacherGroupStudent);
                            teacherGroupStudent = new TeacherGroupStudentModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherGroupStudentModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherGroupStudentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherGroupStudentModel> ReadD(string connectionString, List<TeacherGroupStudentModel> teacherGroupStudentWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherGroupStudentModel> response = new Dictionary<string, TeacherGroupStudentModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_group_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_group_students ";
                if (teacherGroupStudentWhere != null && teacherGroupStudentWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherGroupStudentWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherGroupStudentWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].TeacherGroupId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_group_id` = @teacherGroupId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherGroupId" + i, teacherGroupStudentWhere[i].TeacherGroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, teacherGroupStudentWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherGroupStudentWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherGroupStudentWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherGroupStudentWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherGroupStudentWhere.Count - 1)
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
                            TeacherGroupStudentModel teacherGroupStudent = new TeacherGroupStudentModel(); if (!reader.IsDBNull(0))
                                teacherGroupStudent.Id = reader.GetValue(0).ToString();
                            else
                                teacherGroupStudent.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherGroupStudent.TeacherGroupId = reader.GetValue(1).ToString();
                            else
                                teacherGroupStudent.TeacherGroupId = null;
                            if (!reader.IsDBNull(2))
                                teacherGroupStudent.StudentId = reader.GetValue(2).ToString();
                            else
                                teacherGroupStudent.StudentId = null;
                            if (!reader.IsDBNull(3))
                                teacherGroupStudent.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                teacherGroupStudent.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                teacherGroupStudent.CreatedAt = reader.GetValue(4).ToString();
                            else
                                teacherGroupStudent.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                teacherGroupStudent.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                teacherGroupStudent.UpdatedAt = null;
                            response.Add(teacherGroupStudent.Id, teacherGroupStudent);
                            teacherGroupStudent = new TeacherGroupStudentModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherGroupStudentModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherGroupStudentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherGroupStudentModel> Read(string connectionString, TeacherGroupStudentModel teacherGroupStudent, bool firstOne, bool orderByLastDate, TeacherGroupStudentModel teacherGroupStudentWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupStudentModel> response = new List<TeacherGroupStudentModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_group_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_group_students ";
                {
                    if (teacherGroupStudent != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroupStudent.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherGroupStudent.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.TeacherGroupId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teacher_group_id` = @teacherGroupId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teacher_group_id` = @teacherGroupId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherGroupId", teacherGroupStudent.TeacherGroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", teacherGroupStudent.StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherGroupStudent.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherGroupStudent.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherGroupStudent.UpdatedAt);
                        }
                    }
                    if (teacherGroupStudentWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherGroupStudentWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.TeacherGroupId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teacher_group_id` LIKE @teacherGroupIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teacher_group_id` LIKE @teacherGroupIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@teacherGroupIdWhereLike", "%" + teacherGroupStudentWhereLike.TeacherGroupId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + teacherGroupStudentWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherGroupStudentWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherGroupStudentWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherGroupStudentWhereLike.UpdatedAt + "%");
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
                                teacherGroupStudent = new TeacherGroupStudentModel();
                                if (!reader.IsDBNull(0))
                                    teacherGroupStudent.Id = reader.GetValue(0).ToString();
                                else
                                    teacherGroupStudent.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherGroupStudent.TeacherGroupId = reader.GetValue(1).ToString();
                                else
                                    teacherGroupStudent.TeacherGroupId = null;
                                if (!reader.IsDBNull(2))
                                    teacherGroupStudent.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherGroupStudent.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherGroupStudent.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    teacherGroupStudent.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    teacherGroupStudent.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    teacherGroupStudent.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    teacherGroupStudent.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    teacherGroupStudent.UpdatedAt = null;
                                response.Add(teacherGroupStudent);
                                teacherGroupStudent = new TeacherGroupStudentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherGroupStudentModel>();
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
                response = new List<TeacherGroupStudentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherGroupStudentModel> Read(string connectionString, TeacherGroupStudentModel teacherGroupStudent, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupStudentModel> response = new List<TeacherGroupStudentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_group_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_group_students ";
                {
                    if (teacherGroupStudent != null)
                    {
                        if (!string.IsNullOrEmpty(teacherGroupStudent.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherGroupStudent.Id);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.TeacherGroupId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teacher_group_id` = @teacherGroupId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teacher_group_id` = @teacherGroupId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherGroupId", teacherGroupStudent.TeacherGroupId);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", teacherGroupStudent.StudentId);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherGroupStudent.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherGroupStudent.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherGroupStudent.UpdatedAt);
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
                                    teacherGroupStudent.Id = reader.GetValue(0).ToString();
                                else
                                    teacherGroupStudent.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherGroupStudent.TeacherGroupId = reader.GetValue(1).ToString();
                                else
                                    teacherGroupStudent.TeacherGroupId = null;
                                if (!reader.IsDBNull(2))
                                    teacherGroupStudent.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherGroupStudent.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherGroupStudent.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    teacherGroupStudent.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    teacherGroupStudent.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    teacherGroupStudent.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    teacherGroupStudent.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    teacherGroupStudent.UpdatedAt = null;
                                response.Add(teacherGroupStudent);
                                teacherGroupStudent = new TeacherGroupStudentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherGroupStudentModel>();
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
                response = new List<TeacherGroupStudentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherGroupStudentModel> Read(string connectionString, TeacherGroupStudentModel teacherGroupStudent, bool orderByLastDate, int fromRow, int limit, TeacherGroupStudentModel teacherGroupStudentWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupStudentModel> response = new List<TeacherGroupStudentModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_group_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_group_students ";
                {
                    if (teacherGroupStudent != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroupStudent.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherGroupStudent.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.TeacherGroupId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `teacher_group_id` = @teacherGroupId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `teacher_group_id` = @teacherGroupId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherGroupId", teacherGroupStudent.TeacherGroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", teacherGroupStudent.StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherGroupStudent.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherGroupStudent.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudent.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherGroupStudent.UpdatedAt);
                        }
                    }
                    if (teacherGroupStudentWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherGroupStudentWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.TeacherGroupId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `teacher_group_id` LIKE @teacherGroupIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `teacher_group_id` LIKE @teacherGroupIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@teacherGroupIdWhereLike", "%" + teacherGroupStudentWhereLike.TeacherGroupId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + teacherGroupStudentWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherGroupStudentWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherGroupStudentWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherGroupStudentWhereLike.UpdatedAt + "%");
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
                                teacherGroupStudent = new TeacherGroupStudentModel();
                                if (!reader.IsDBNull(0))
                                    teacherGroupStudent.Id = reader.GetValue(0).ToString();
                                else
                                    teacherGroupStudent.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherGroupStudent.TeacherGroupId = reader.GetValue(1).ToString();
                                else
                                    teacherGroupStudent.TeacherGroupId = null;
                                if (!reader.IsDBNull(2))
                                    teacherGroupStudent.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherGroupStudent.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherGroupStudent.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    teacherGroupStudent.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    teacherGroupStudent.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    teacherGroupStudent.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    teacherGroupStudent.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    teacherGroupStudent.UpdatedAt = null;
                                response.Add(teacherGroupStudent);
                                teacherGroupStudent = new TeacherGroupStudentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherGroupStudentModel>();
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
                response = new List<TeacherGroupStudentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherGroupStudentModel> Read(string connectionString, TeacherGroupStudentModel teacherGroupStudent, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupStudentModel> response = new List<TeacherGroupStudentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_group_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_group_students ";
                {
                    if (teacherGroupStudent != null)
                    {
                        if (!string.IsNullOrEmpty(teacherGroupStudent.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherGroupStudent.Id);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.TeacherGroupId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teacher_group_id` = @teacherGroupId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teacher_group_id` = @teacherGroupId ");
                            }
                            cmd.Parameters.AddWithValue("@teacherGroupId", teacherGroupStudent.TeacherGroupId);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", teacherGroupStudent.StudentId);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherGroupStudent.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherGroupStudent.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(teacherGroupStudent.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherGroupStudent.UpdatedAt);
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
                                    teacherGroupStudent.Id = reader.GetValue(0).ToString();
                                else
                                    teacherGroupStudent.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherGroupStudent.TeacherGroupId = reader.GetValue(1).ToString();
                                else
                                    teacherGroupStudent.TeacherGroupId = null;
                                if (!reader.IsDBNull(2))
                                    teacherGroupStudent.StudentId = reader.GetValue(2).ToString();
                                else
                                    teacherGroupStudent.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    teacherGroupStudent.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    teacherGroupStudent.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    teacherGroupStudent.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    teacherGroupStudent.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    teacherGroupStudent.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    teacherGroupStudent.UpdatedAt = null;
                                response.Add(teacherGroupStudent);
                                teacherGroupStudent = new TeacherGroupStudentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherGroupStudentModel>();
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
                response = new List<TeacherGroupStudentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherGroupStudentModel> Read(string connectionString, List<TeacherGroupStudentModel> teacherGroupStudentWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupStudentModel> response = new List<TeacherGroupStudentModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_group_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_group_students ";
                if (teacherGroupStudentWhere != null && teacherGroupStudentWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherGroupStudentWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherGroupStudentWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].TeacherGroupId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_group_id` = @teacherGroupId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherGroupId" + i, teacherGroupStudentWhere[i].TeacherGroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, teacherGroupStudentWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherGroupStudentWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherGroupStudentWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherGroupStudentWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherGroupStudentWhere.Count - 1)
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
                            TeacherGroupStudentModel teacherGroupStudent = new TeacherGroupStudentModel(); if (!reader.IsDBNull(0))
                                teacherGroupStudent.Id = reader.GetValue(0).ToString();
                            else
                                teacherGroupStudent.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherGroupStudent.TeacherGroupId = reader.GetValue(1).ToString();
                            else
                                teacherGroupStudent.TeacherGroupId = null;
                            if (!reader.IsDBNull(2))
                                teacherGroupStudent.StudentId = reader.GetValue(2).ToString();
                            else
                                teacherGroupStudent.StudentId = null;
                            if (!reader.IsDBNull(3))
                                teacherGroupStudent.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                teacherGroupStudent.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                teacherGroupStudent.CreatedAt = reader.GetValue(4).ToString();
                            else
                                teacherGroupStudent.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                teacherGroupStudent.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                teacherGroupStudent.UpdatedAt = null;
                            response.Add(teacherGroupStudent);
                            teacherGroupStudent = new TeacherGroupStudentModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherGroupStudentModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupStudentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherGroupStudentModel> Read(string connectionString, List<TeacherGroupStudentModel> teacherGroupStudentWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupStudentModel> response = new List<TeacherGroupStudentModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_group_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_group_students ";
                if (teacherGroupStudentWhere != null && teacherGroupStudentWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherGroupStudentWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherGroupStudentWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].TeacherGroupId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_group_id` = @teacherGroupId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherGroupId" + i, teacherGroupStudentWhere[i].TeacherGroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, teacherGroupStudentWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherGroupStudentWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherGroupStudentWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupStudentWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherGroupStudentWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherGroupStudentWhere.Count - 1)
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
                            TeacherGroupStudentModel teacherGroupStudent = new TeacherGroupStudentModel(); if (!reader.IsDBNull(0))
                                teacherGroupStudent.Id = reader.GetValue(0).ToString();
                            else
                                teacherGroupStudent.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherGroupStudent.TeacherGroupId = reader.GetValue(1).ToString();
                            else
                                teacherGroupStudent.TeacherGroupId = null;
                            if (!reader.IsDBNull(2))
                                teacherGroupStudent.StudentId = reader.GetValue(2).ToString();
                            else
                                teacherGroupStudent.StudentId = null;
                            if (!reader.IsDBNull(3))
                                teacherGroupStudent.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                teacherGroupStudent.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                teacherGroupStudent.CreatedAt = reader.GetValue(4).ToString();
                            else
                                teacherGroupStudent.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                teacherGroupStudent.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                teacherGroupStudent.UpdatedAt = null;
                            response.Add(teacherGroupStudent);
                            teacherGroupStudent = new TeacherGroupStudentModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherGroupStudentModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupStudentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherGroupStudentModel> Create(string connectionString, TeacherGroupStudentModel teacherGroupStudent)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupStudentModel> response = new List<TeacherGroupStudentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO teacher_group_students (";
                string values = "(";
                if (!String.IsNullOrEmpty(teacherGroupStudent.Id))
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
                    cmd.Parameters.AddWithValue("@id", teacherGroupStudent.Id);
                }
                if (!String.IsNullOrEmpty(teacherGroupStudent.TeacherGroupId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `teacher_group_id` ");
                        values = String.Concat(values, ", @teacherGroupId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `teacher_group_id` ");
                        values = String.Concat(values, " @teacherGroupId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teacherGroupId", teacherGroupStudent.TeacherGroupId);
                }
                if (!String.IsNullOrEmpty(teacherGroupStudent.StudentId))
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
                    cmd.Parameters.AddWithValue("@studentId", teacherGroupStudent.StudentId);
                }
                if (!String.IsNullOrEmpty(teacherGroupStudent.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherGroupStudent.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherGroupStudent.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", teacherGroupStudent.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherGroupStudent.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", teacherGroupStudent.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                teacherGroupStudent.Id = cmd.LastInsertedId.ToString();
                response.Add(teacherGroupStudent);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupStudentModel>();
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
        public List<TeacherGroupStudentModel> Create(string connectionString, List<TeacherGroupStudentModel> teacherGroupStudentList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupStudentModel> response = new List<TeacherGroupStudentModel>();
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
                        for (int i = 0; i < teacherGroupStudentList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO teacher_group_students (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(teacherGroupStudentList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, teacherGroupStudentList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupStudentList[i].TeacherGroupId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `teacher_group_id` ");
                                        values = String.Concat(values, ", @teacherGroupId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `teacher_group_id` ");
                                        values = String.Concat(values, "@teacherGroupId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teacherGroupId" + i, teacherGroupStudentList[i].TeacherGroupId);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupStudentList[i].StudentId))
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
                                    cmd.Parameters.AddWithValue("@studentId" + i, teacherGroupStudentList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupStudentList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherGroupStudentList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupStudentList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherGroupStudentList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupStudentList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherGroupStudentList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(teacherGroupStudentList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherGroupStudentModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherGroupStudentModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupStudentModel>();
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

        public List<TeacherGroupStudentModel> Update(string connectionString, TeacherGroupStudentModel teacherGroupStudent, TeacherGroupStudentModel teacherGroupStudentWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupStudentModel> response = new List<TeacherGroupStudentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE teacher_group_students SET ";
                if (!String.IsNullOrEmpty(teacherGroupStudent.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", teacherGroupStudent.Id);
                }
                if (!String.IsNullOrEmpty(teacherGroupStudent.TeacherGroupId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `teacher_group_id` = @teacherGroupId");
                    else
                        sql = String.Concat(sql, " `teacher_group_id` = @teacherGroupId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teacherGroupId", teacherGroupStudent.TeacherGroupId);
                }
                if (!String.IsNullOrEmpty(teacherGroupStudent.StudentId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `student_id` = @studentId");
                    else
                        sql = String.Concat(sql, " `student_id` = @studentId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@studentId", teacherGroupStudent.StudentId);
                }
                if (!String.IsNullOrEmpty(teacherGroupStudent.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherGroupStudent.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherGroupStudent.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", teacherGroupStudent.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherGroupStudent.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", teacherGroupStudent.UpdatedAt);
                }
                if (teacherGroupStudentWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(teacherGroupStudentWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", teacherGroupStudentWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupStudentWhere.TeacherGroupId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_group_id` = @teacherGroupIdWhere");
                        else
                            sql = String.Concat(sql, " `teacher_group_id` = @teacherGroupIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherGroupIdWhere", teacherGroupStudentWhere.TeacherGroupId);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupStudentWhere.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentIdWhere", teacherGroupStudentWhere.StudentId);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupStudentWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", teacherGroupStudentWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupStudentWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", teacherGroupStudentWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupStudentWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", teacherGroupStudentWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(teacherGroupStudent);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupStudentModel>();
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

        public List<TeacherGroupStudentModel> Update(string connectionString, List<TeacherGroupStudentModel> teacherGroupStudentList, List<TeacherGroupStudentModel> teacherGroupStudentWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupStudentModel> response = new List<TeacherGroupStudentModel>();
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
                        for (int i = 0; i < teacherGroupStudentList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE teacher_group_students SET ";
                                if (!String.IsNullOrEmpty(teacherGroupStudentList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, teacherGroupStudentList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupStudentList[i].TeacherGroupId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `teacher_group_id` = @teacherGroupId" + i);
                                    else
                                        sql = String.Concat(sql, " `teacher_group_id` = @teacherGroupId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teacherGroupId" + i, teacherGroupStudentList[i].TeacherGroupId);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupStudentList[i].StudentId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `student_id` = @studentId" + i);
                                    else
                                        sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@studentId" + i, teacherGroupStudentList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupStudentList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherGroupStudentList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupStudentList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherGroupStudentList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupStudentList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherGroupStudentList[i].UpdatedAt);
                                }
                                if (teacherGroupStudentWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(teacherGroupStudentWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, teacherGroupStudentWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupStudentWhereList[i].TeacherGroupId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_group_id` = @teacherGroupIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_group_id` = @teacherGroupIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherGroupIdWhere" + i, teacherGroupStudentWhereList[i].TeacherGroupId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupStudentWhereList[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentIdWhere" + i, teacherGroupStudentWhereList[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupStudentWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, teacherGroupStudentWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupStudentWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, teacherGroupStudentWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupStudentWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, teacherGroupStudentWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherGroupStudentList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherGroupStudentModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherGroupStudentModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupStudentModel>();
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

        public List<TeacherGroupStudentModel> Delete(string connectionString, TeacherGroupStudentModel teacherGroupStudent)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupStudentModel> response = new List<TeacherGroupStudentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM teacher_group_students  ";
                if (teacherGroupStudent != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(teacherGroupStudent.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", teacherGroupStudent.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupStudent.TeacherGroupId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_group_id` = @teacherGroupId");
                        else
                            sql = String.Concat(sql, " `teacher_group_id` = @teacherGroupId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherGroupId", teacherGroupStudent.TeacherGroupId);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupStudent.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentId");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentId", teacherGroupStudent.StudentId);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupStudent.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", teacherGroupStudent.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupStudent.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", teacherGroupStudent.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupStudent.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", teacherGroupStudent.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(teacherGroupStudent);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupStudentModel>();
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

        public List<TeacherGroupStudentModel> Delete(string connectionString, List<TeacherGroupStudentModel> teacherGroupStudent)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupStudentModel> response = new List<TeacherGroupStudentModel>();
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
                        for (int i = 0; i < teacherGroupStudent.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM teacher_group_students  ";
                                if (teacherGroupStudent[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(teacherGroupStudent[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, teacherGroupStudent[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupStudent[i].TeacherGroupId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_group_id` = @teacherGroupId" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_group_id` = @teacherGroupId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherGroupId" + i, teacherGroupStudent[i].TeacherGroupId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupStudent[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentId" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentId" + i, teacherGroupStudent[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupStudent[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherGroupStudent[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupStudent[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, teacherGroupStudent[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupStudent[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, teacherGroupStudent[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherGroupStudent[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherGroupStudentModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherGroupStudentModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupStudentModel>();
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
