using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Teachers;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    class TeacherGroupDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, TeacherGroupModel> ReadD(string connectionString, TeacherGroupModel teacherGroup, bool firstOne, bool orderByLastDate, TeacherGroupModel teacherGroupWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherGroupModel> response = new Dictionary<string, TeacherGroupModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `subject_code`, `level_code`, `name`, `subscription_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_groups ";
                {
                    if (teacherGroup != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroup.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherGroup.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", teacherGroup.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", teacherGroup.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCode", teacherGroup.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", teacherGroup.Name);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.SubscriptionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `subscription_id` = @subscriptionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `subscription_id` = @subscriptionId ");
                            }
                            cmd.Parameters.AddWithValue("@subscriptionId", teacherGroup.SubscriptionId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherGroup.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherGroup.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherGroup.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherGroup.UpdatedAt);
                        }
                    }
                    if (teacherGroupWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherGroupWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + teacherGroupWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + teacherGroupWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + teacherGroupWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + teacherGroupWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.SubscriptionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + teacherGroupWhereLike.SubscriptionId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherGroupWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherGroupWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherGroupWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherGroupWhereLike.UpdatedAt + "%");
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
                                teacherGroup = new TeacherGroupModel();
                                if (!reader.IsDBNull(0))
                                    teacherGroup.Id = reader.GetValue(0).ToString();
                                else
                                    teacherGroup.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherGroup.TeacherId = reader.GetValue(1).ToString();
                                else
                                    teacherGroup.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    teacherGroup.SubjectCode = reader.GetValue(2).ToString();
                                else
                                    teacherGroup.SubjectCode = null;
                                if (!reader.IsDBNull(3))
                                    teacherGroup.LevelCode = reader.GetValue(3).ToString();
                                else
                                    teacherGroup.LevelCode = null;
                                if (!reader.IsDBNull(4))
                                    teacherGroup.Name = reader.GetValue(4).ToString();
                                else
                                    teacherGroup.Name = null;
                                if (!reader.IsDBNull(5))
                                    teacherGroup.SubscriptionId = reader.GetValue(5).ToString();
                                else
                                    teacherGroup.SubscriptionId = null;
                                if (!reader.IsDBNull(6))
                                    teacherGroup.Available = reader.GetValue(6).ToString();
                                else
                                    teacherGroup.Available = null;
                                if (!reader.IsDBNull(7))
                                    teacherGroup.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    teacherGroup.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    teacherGroup.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    teacherGroup.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    teacherGroup.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherGroup.UpdatedAt = null;
                                response.Add(teacherGroup.Id, teacherGroup);
                                teacherGroup = new TeacherGroupModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherGroupModel>();
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
                response = new Dictionary<string, TeacherGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherGroupModel> ReadD(string connectionString, TeacherGroupModel teacherGroup, bool orderByLastDate, int fromRow, int limit, TeacherGroupModel teacherGroupWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherGroupModel> response = new Dictionary<string, TeacherGroupModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `subject_code`, `level_code`, `name`, `subscription_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_groups ";
                {
                    if (teacherGroup != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroup.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherGroup.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", teacherGroup.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", teacherGroup.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCode", teacherGroup.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", teacherGroup.Name);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.SubscriptionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `subscription_id` = @subscriptionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `subscription_id` = @subscriptionId ");
                            }
                            cmd.Parameters.AddWithValue("@subscriptionId", teacherGroup.SubscriptionId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherGroup.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherGroup.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherGroup.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherGroup.UpdatedAt);
                        }
                    }
                    if (teacherGroupWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherGroupWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + teacherGroupWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + teacherGroupWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + teacherGroupWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + teacherGroupWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.SubscriptionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + teacherGroupWhereLike.SubscriptionId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherGroupWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherGroupWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherGroupWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherGroupWhereLike.UpdatedAt + "%");
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
                                teacherGroup = new TeacherGroupModel();
                                if (!reader.IsDBNull(0))
                                    teacherGroup.Id = reader.GetValue(0).ToString();
                                else
                                    teacherGroup.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherGroup.TeacherId = reader.GetValue(1).ToString();
                                else
                                    teacherGroup.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    teacherGroup.SubjectCode = reader.GetValue(2).ToString();
                                else
                                    teacherGroup.SubjectCode = null;
                                if (!reader.IsDBNull(3))
                                    teacherGroup.LevelCode = reader.GetValue(3).ToString();
                                else
                                    teacherGroup.LevelCode = null;
                                if (!reader.IsDBNull(4))
                                    teacherGroup.Name = reader.GetValue(4).ToString();
                                else
                                    teacherGroup.Name = null;
                                if (!reader.IsDBNull(5))
                                    teacherGroup.SubscriptionId = reader.GetValue(5).ToString();
                                else
                                    teacherGroup.SubscriptionId = null;
                                if (!reader.IsDBNull(6))
                                    teacherGroup.Available = reader.GetValue(6).ToString();
                                else
                                    teacherGroup.Available = null;
                                if (!reader.IsDBNull(7))
                                    teacherGroup.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    teacherGroup.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    teacherGroup.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    teacherGroup.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    teacherGroup.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherGroup.UpdatedAt = null;
                                response.Add(teacherGroup.Id, teacherGroup);
                                teacherGroup = new TeacherGroupModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherGroupModel>();
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
                response = new Dictionary<string, TeacherGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherGroupModel> ReadD(string connectionString, List<TeacherGroupModel> teacherGroupWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherGroupModel> response = new Dictionary<string, TeacherGroupModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_id`, `subject_code`, `level_code`, `name`, `subscription_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_groups ";
                if (teacherGroupWhere != null && teacherGroupWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherGroupWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherGroupWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, teacherGroupWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, teacherGroupWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, teacherGroupWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, teacherGroupWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, teacherGroupWhere[i].SubscriptionId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherGroupWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherGroupWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherGroupWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherGroupWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherGroupWhere.Count - 1)
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
                            TeacherGroupModel teacherGroup = new TeacherGroupModel(); if (!reader.IsDBNull(0))
                                teacherGroup.Id = reader.GetValue(0).ToString();
                            else
                                teacherGroup.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherGroup.TeacherId = reader.GetValue(1).ToString();
                            else
                                teacherGroup.TeacherId = null;
                            if (!reader.IsDBNull(2))
                                teacherGroup.SubjectCode = reader.GetValue(2).ToString();
                            else
                                teacherGroup.SubjectCode = null;
                            if (!reader.IsDBNull(3))
                                teacherGroup.LevelCode = reader.GetValue(3).ToString();
                            else
                                teacherGroup.LevelCode = null;
                            if (!reader.IsDBNull(4))
                                teacherGroup.Name = reader.GetValue(4).ToString();
                            else
                                teacherGroup.Name = null;
                            if (!reader.IsDBNull(5))
                                teacherGroup.SubscriptionId = reader.GetValue(5).ToString();
                            else
                                teacherGroup.SubscriptionId = null;
                            if (!reader.IsDBNull(6))
                                teacherGroup.Available = reader.GetValue(6).ToString();
                            else
                                teacherGroup.Available = null;
                            if (!reader.IsDBNull(7))
                                teacherGroup.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                teacherGroup.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                teacherGroup.CreatedAt = reader.GetValue(8).ToString();
                            else
                                teacherGroup.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                teacherGroup.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                teacherGroup.UpdatedAt = null;
                            response.Add(teacherGroup.Id, teacherGroup);
                            teacherGroup = new TeacherGroupModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherGroupModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherGroupModel> ReadD(string connectionString, List<TeacherGroupModel> teacherGroupWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherGroupModel> response = new Dictionary<string, TeacherGroupModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_id`, `subject_code`, `level_code`, `name`, `subscription_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_groups ";
                if (teacherGroupWhere != null && teacherGroupWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherGroupWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherGroupWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, teacherGroupWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, teacherGroupWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, teacherGroupWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, teacherGroupWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, teacherGroupWhere[i].SubscriptionId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherGroupWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherGroupWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherGroupWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherGroupWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherGroupWhere.Count - 1)
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
                            TeacherGroupModel teacherGroup = new TeacherGroupModel(); if (!reader.IsDBNull(0))
                                teacherGroup.Id = reader.GetValue(0).ToString();
                            else
                                teacherGroup.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherGroup.TeacherId = reader.GetValue(1).ToString();
                            else
                                teacherGroup.TeacherId = null;
                            if (!reader.IsDBNull(2))
                                teacherGroup.SubjectCode = reader.GetValue(2).ToString();
                            else
                                teacherGroup.SubjectCode = null;
                            if (!reader.IsDBNull(3))
                                teacherGroup.LevelCode = reader.GetValue(3).ToString();
                            else
                                teacherGroup.LevelCode = null;
                            if (!reader.IsDBNull(4))
                                teacherGroup.Name = reader.GetValue(4).ToString();
                            else
                                teacherGroup.Name = null;
                            if (!reader.IsDBNull(5))
                                teacherGroup.SubscriptionId = reader.GetValue(5).ToString();
                            else
                                teacherGroup.SubscriptionId = null;
                            if (!reader.IsDBNull(6))
                                teacherGroup.Available = reader.GetValue(6).ToString();
                            else
                                teacherGroup.Available = null;
                            if (!reader.IsDBNull(7))
                                teacherGroup.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                teacherGroup.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                teacherGroup.CreatedAt = reader.GetValue(8).ToString();
                            else
                                teacherGroup.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                teacherGroup.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                teacherGroup.UpdatedAt = null;
                            response.Add(teacherGroup.Id, teacherGroup);
                            teacherGroup = new TeacherGroupModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherGroupModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherGroupModel> Read(string connectionString, TeacherGroupModel teacherGroup, bool firstOne, bool orderByLastDate, TeacherGroupModel teacherGroupWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupModel> response = new List<TeacherGroupModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `subject_code`, `level_code`, `name`, `subscription_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_groups ";
                {
                    if (teacherGroup != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroup.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherGroup.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", teacherGroup.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", teacherGroup.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCode", teacherGroup.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", teacherGroup.Name);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.SubscriptionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `subscription_id` = @subscriptionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `subscription_id` = @subscriptionId ");
                            }
                            cmd.Parameters.AddWithValue("@subscriptionId", teacherGroup.SubscriptionId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherGroup.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherGroup.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherGroup.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherGroup.UpdatedAt);
                        }
                    }
                    if (teacherGroupWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherGroupWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + teacherGroupWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + teacherGroupWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + teacherGroupWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + teacherGroupWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.SubscriptionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + teacherGroupWhereLike.SubscriptionId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherGroupWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherGroupWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherGroupWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherGroupWhereLike.UpdatedAt + "%");
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
                                teacherGroup = new TeacherGroupModel();
                                if (!reader.IsDBNull(0))
                                    teacherGroup.Id = reader.GetValue(0).ToString();
                                else
                                    teacherGroup.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherGroup.TeacherId = reader.GetValue(1).ToString();
                                else
                                    teacherGroup.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    teacherGroup.SubjectCode = reader.GetValue(2).ToString();
                                else
                                    teacherGroup.SubjectCode = null;
                                if (!reader.IsDBNull(3))
                                    teacherGroup.LevelCode = reader.GetValue(3).ToString();
                                else
                                    teacherGroup.LevelCode = null;
                                if (!reader.IsDBNull(4))
                                    teacherGroup.Name = reader.GetValue(4).ToString();
                                else
                                    teacherGroup.Name = null;
                                if (!reader.IsDBNull(5))
                                    teacherGroup.SubscriptionId = reader.GetValue(5).ToString();
                                else
                                    teacherGroup.SubscriptionId = null;
                                if (!reader.IsDBNull(6))
                                    teacherGroup.Available = reader.GetValue(6).ToString();
                                else
                                    teacherGroup.Available = null;
                                if (!reader.IsDBNull(7))
                                    teacherGroup.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    teacherGroup.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    teacherGroup.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    teacherGroup.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    teacherGroup.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherGroup.UpdatedAt = null;
                                response.Add(teacherGroup);
                                teacherGroup = new TeacherGroupModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherGroupModel>();
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
                response = new List<TeacherGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherGroupModel> Read(string connectionString, TeacherGroupModel teacherGroup, bool orderByLastDate, int fromRow, int limit, TeacherGroupModel teacherGroupWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupModel> response = new List<TeacherGroupModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `subject_code`, `level_code`, `name`, `subscription_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_groups ";
                {
                    if (teacherGroup != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroup.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherGroup.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", teacherGroup.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", teacherGroup.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCode", teacherGroup.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", teacherGroup.Name);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.SubscriptionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `subscription_id` = @subscriptionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `subscription_id` = @subscriptionId ");
                            }
                            cmd.Parameters.AddWithValue("@subscriptionId", teacherGroup.SubscriptionId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherGroup.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherGroup.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherGroup.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroup.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherGroup.UpdatedAt);
                        }
                    }
                    if (teacherGroupWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherGroupWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + teacherGroupWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + teacherGroupWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + teacherGroupWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + teacherGroupWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.SubscriptionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `subscription_id` LIKE @subscriptionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subscriptionIdWhereLike", "%" + teacherGroupWhereLike.SubscriptionId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherGroupWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherGroupWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherGroupWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherGroupWhereLike.UpdatedAt + "%");
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
                                teacherGroup = new TeacherGroupModel();
                                if (!reader.IsDBNull(0))
                                    teacherGroup.Id = reader.GetValue(0).ToString();
                                else
                                    teacherGroup.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherGroup.TeacherId = reader.GetValue(1).ToString();
                                else
                                    teacherGroup.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    teacherGroup.SubjectCode = reader.GetValue(2).ToString();
                                else
                                    teacherGroup.SubjectCode = null;
                                if (!reader.IsDBNull(3))
                                    teacherGroup.LevelCode = reader.GetValue(3).ToString();
                                else
                                    teacherGroup.LevelCode = null;
                                if (!reader.IsDBNull(4))
                                    teacherGroup.Name = reader.GetValue(4).ToString();
                                else
                                    teacherGroup.Name = null;
                                if (!reader.IsDBNull(5))
                                    teacherGroup.SubscriptionId = reader.GetValue(5).ToString();
                                else
                                    teacherGroup.SubscriptionId = null;
                                if (!reader.IsDBNull(6))
                                    teacherGroup.Available = reader.GetValue(6).ToString();
                                else
                                    teacherGroup.Available = null;
                                if (!reader.IsDBNull(7))
                                    teacherGroup.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    teacherGroup.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    teacherGroup.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    teacherGroup.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    teacherGroup.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherGroup.UpdatedAt = null;
                                response.Add(teacherGroup);
                                teacherGroup = new TeacherGroupModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherGroupModel>();
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
                response = new List<TeacherGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherGroupModel> Read(string connectionString, List<TeacherGroupModel> teacherGroupWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupModel> response = new List<TeacherGroupModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_id`, `subject_code`, `level_code`, `name`, `subscription_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_groups ";
                if (teacherGroupWhere != null && teacherGroupWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherGroupWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherGroupWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, teacherGroupWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, teacherGroupWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, teacherGroupWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, teacherGroupWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, teacherGroupWhere[i].SubscriptionId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherGroupWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherGroupWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherGroupWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherGroupWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherGroupWhere.Count - 1)
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
                            TeacherGroupModel teacherGroup = new TeacherGroupModel(); if (!reader.IsDBNull(0))
                                teacherGroup.Id = reader.GetValue(0).ToString();
                            else
                                teacherGroup.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherGroup.TeacherId = reader.GetValue(1).ToString();
                            else
                                teacherGroup.TeacherId = null;
                            if (!reader.IsDBNull(2))
                                teacherGroup.SubjectCode = reader.GetValue(2).ToString();
                            else
                                teacherGroup.SubjectCode = null;
                            if (!reader.IsDBNull(3))
                                teacherGroup.LevelCode = reader.GetValue(3).ToString();
                            else
                                teacherGroup.LevelCode = null;
                            if (!reader.IsDBNull(4))
                                teacherGroup.Name = reader.GetValue(4).ToString();
                            else
                                teacherGroup.Name = null;
                            if (!reader.IsDBNull(5))
                                teacherGroup.SubscriptionId = reader.GetValue(5).ToString();
                            else
                                teacherGroup.SubscriptionId = null;
                            if (!reader.IsDBNull(6))
                                teacherGroup.Available = reader.GetValue(6).ToString();
                            else
                                teacherGroup.Available = null;
                            if (!reader.IsDBNull(7))
                                teacherGroup.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                teacherGroup.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                teacherGroup.CreatedAt = reader.GetValue(8).ToString();
                            else
                                teacherGroup.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                teacherGroup.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                teacherGroup.UpdatedAt = null;
                            response.Add(teacherGroup);
                            teacherGroup = new TeacherGroupModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherGroupModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherGroupModel> Read(string connectionString, List<TeacherGroupModel> teacherGroupWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupModel> response = new List<TeacherGroupModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_id`, `subject_code`, `level_code`, `name`, `subscription_id`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_groups ";
                if (teacherGroupWhere != null && teacherGroupWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherGroupWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherGroupWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, teacherGroupWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, teacherGroupWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, teacherGroupWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, teacherGroupWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, teacherGroupWhere[i].SubscriptionId);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherGroupWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherGroupWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherGroupWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherGroupWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherGroupWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherGroupWhere.Count - 1)
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
                            TeacherGroupModel teacherGroup = new TeacherGroupModel(); if (!reader.IsDBNull(0))
                                teacherGroup.Id = reader.GetValue(0).ToString();
                            else
                                teacherGroup.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherGroup.TeacherId = reader.GetValue(1).ToString();
                            else
                                teacherGroup.TeacherId = null;
                            if (!reader.IsDBNull(2))
                                teacherGroup.SubjectCode = reader.GetValue(2).ToString();
                            else
                                teacherGroup.SubjectCode = null;
                            if (!reader.IsDBNull(3))
                                teacherGroup.LevelCode = reader.GetValue(3).ToString();
                            else
                                teacherGroup.LevelCode = null;
                            if (!reader.IsDBNull(4))
                                teacherGroup.Name = reader.GetValue(4).ToString();
                            else
                                teacherGroup.Name = null;
                            if (!reader.IsDBNull(5))
                                teacherGroup.SubscriptionId = reader.GetValue(5).ToString();
                            else
                                teacherGroup.SubscriptionId = null;
                            if (!reader.IsDBNull(6))
                                teacherGroup.Available = reader.GetValue(6).ToString();
                            else
                                teacherGroup.Available = null;
                            if (!reader.IsDBNull(7))
                                teacherGroup.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                teacherGroup.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                teacherGroup.CreatedAt = reader.GetValue(8).ToString();
                            else
                                teacherGroup.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                teacherGroup.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                teacherGroup.UpdatedAt = null;
                            response.Add(teacherGroup);
                            teacherGroup = new TeacherGroupModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherGroupModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherGroupModel> Create(string connectionString, TeacherGroupModel teacherGroup)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupModel> response = new List<TeacherGroupModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO teacher_groups (";
                string values = "(";
                if (!String.IsNullOrEmpty(teacherGroup.Id))
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
                    cmd.Parameters.AddWithValue("@id", teacherGroup.Id);
                }
                if (!String.IsNullOrEmpty(teacherGroup.TeacherId))
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
                    cmd.Parameters.AddWithValue("@teacherId", teacherGroup.TeacherId);
                }
                if (!String.IsNullOrEmpty(teacherGroup.SubjectCode))
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
                    cmd.Parameters.AddWithValue("@subjectCode", teacherGroup.SubjectCode);
                }
                if (!String.IsNullOrEmpty(teacherGroup.LevelCode))
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
                    cmd.Parameters.AddWithValue("@levelCode", teacherGroup.LevelCode);
                }
                if (!String.IsNullOrEmpty(teacherGroup.Name))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `name` ");
                        values = String.Concat(values, ", @name");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `name` ");
                        values = String.Concat(values, " @name");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", teacherGroup.Name);
                }
                if (!String.IsNullOrEmpty(teacherGroup.SubscriptionId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `subscription_id` ");
                        values = String.Concat(values, ", @subscriptionId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `subscription_id` ");
                        values = String.Concat(values, " @subscriptionId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subscriptionId", teacherGroup.SubscriptionId);
                }
                if (!String.IsNullOrEmpty(teacherGroup.Available))
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
                    cmd.Parameters.AddWithValue("@available", teacherGroup.Available);
                }
                if (!String.IsNullOrEmpty(teacherGroup.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherGroup.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherGroup.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", teacherGroup.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherGroup.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", teacherGroup.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                teacherGroup.Id = cmd.LastInsertedId.ToString();
                response.Add(teacherGroup);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupModel>();
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
        public List<TeacherGroupModel> Create(string connectionString, List<TeacherGroupModel> teacherGroupList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupModel> response = new List<TeacherGroupModel>();
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
                        for (int i = 0; i < teacherGroupList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO teacher_groups (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(teacherGroupList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, teacherGroupList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].TeacherId))
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
                                    cmd.Parameters.AddWithValue("@teacherId" + i, teacherGroupList[i].TeacherId);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].SubjectCode))
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
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, teacherGroupList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].LevelCode))
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
                                    cmd.Parameters.AddWithValue("@levelCode" + i, teacherGroupList[i].LevelCode);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].Name))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `name` ");
                                        values = String.Concat(values, ", @name" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `name` ");
                                        values = String.Concat(values, "@name" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, teacherGroupList[i].Name);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].SubscriptionId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `subscription_id` ");
                                        values = String.Concat(values, ", @subscriptionId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `subscription_id` ");
                                        values = String.Concat(values, "@subscriptionId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subscriptionId" + i, teacherGroupList[i].SubscriptionId);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].Available))
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
                                    cmd.Parameters.AddWithValue("@available" + i, teacherGroupList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherGroupList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherGroupList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherGroupList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(teacherGroupList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherGroupModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherGroupModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupModel>();
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

        public List<TeacherGroupModel> Update(string connectionString, TeacherGroupModel teacherGroup, TeacherGroupModel teacherGroupWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupModel> response = new List<TeacherGroupModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE teacher_groups SET ";
                if (!String.IsNullOrEmpty(teacherGroup.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", teacherGroup.Id);
                }
                if (!String.IsNullOrEmpty(teacherGroup.TeacherId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `teacher_id` = @teacherId");
                    else
                        sql = String.Concat(sql, " `teacher_id` = @teacherId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teacherId", teacherGroup.TeacherId);
                }
                if (!String.IsNullOrEmpty(teacherGroup.SubjectCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `subject_code` = @subjectCode");
                    else
                        sql = String.Concat(sql, " `subject_code` = @subjectCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectCode", teacherGroup.SubjectCode);
                }
                if (!String.IsNullOrEmpty(teacherGroup.LevelCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `level_code` = @levelCode");
                    else
                        sql = String.Concat(sql, " `level_code` = @levelCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@levelCode", teacherGroup.LevelCode);
                }
                if (!String.IsNullOrEmpty(teacherGroup.Name))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `name` = @name");
                    else
                        sql = String.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", teacherGroup.Name);
                }
                if (!String.IsNullOrEmpty(teacherGroup.SubscriptionId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `subscription_id` = @subscriptionId");
                    else
                        sql = String.Concat(sql, " `subscription_id` = @subscriptionId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subscriptionId", teacherGroup.SubscriptionId);
                }
                if (!String.IsNullOrEmpty(teacherGroup.Available))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `available` = @available");
                    else
                        sql = String.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", teacherGroup.Available);
                }
                if (!String.IsNullOrEmpty(teacherGroup.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherGroup.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherGroup.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", teacherGroup.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherGroup.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", teacherGroup.UpdatedAt);
                }
                if (teacherGroupWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(teacherGroupWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", teacherGroupWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupWhere.TeacherId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_id` = @teacherIdWhere");
                        else
                            sql = String.Concat(sql, " `teacher_id` = @teacherIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherIdWhere", teacherGroupWhere.TeacherId);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupWhere.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCodeWhere", teacherGroupWhere.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupWhere.LevelCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `level_code` = @levelCodeWhere");
                        else
                            sql = String.Concat(sql, " `level_code` = @levelCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelCodeWhere", teacherGroupWhere.LevelCode);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupWhere.Name))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = String.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", teacherGroupWhere.Name);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupWhere.SubscriptionId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subscription_id` = @subscriptionIdWhere");
                        else
                            sql = String.Concat(sql, " `subscription_id` = @subscriptionIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionIdWhere", teacherGroupWhere.SubscriptionId);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupWhere.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = String.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", teacherGroupWhere.Available);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", teacherGroupWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", teacherGroupWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherGroupWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", teacherGroupWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(teacherGroup);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupModel>();
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

        public List<TeacherGroupModel> Update(string connectionString, List<TeacherGroupModel> teacherGroupList, List<TeacherGroupModel> teacherGroupWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupModel> response = new List<TeacherGroupModel>();
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
                        for (int i = 0; i < teacherGroupList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE teacher_groups SET ";
                                if (!String.IsNullOrEmpty(teacherGroupList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, teacherGroupList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].TeacherId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `teacher_id` = @teacherId" + i);
                                    else
                                        sql = String.Concat(sql, " `teacher_id` = @teacherId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teacherId" + i, teacherGroupList[i].TeacherId);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].SubjectCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `subject_code` = @subjectCode" + i);
                                    else
                                        sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, teacherGroupList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].LevelCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `level_code` = @levelCode" + i);
                                    else
                                        sql = String.Concat(sql, " `level_code` = @levelCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@levelCode" + i, teacherGroupList[i].LevelCode);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].Name))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = String.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, teacherGroupList[i].Name);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].SubscriptionId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `subscription_id` = @subscriptionId" + i);
                                    else
                                        sql = String.Concat(sql, " `subscription_id` = @subscriptionId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subscriptionId" + i, teacherGroupList[i].SubscriptionId);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].Available))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = String.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, teacherGroupList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherGroupList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherGroupList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherGroupList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherGroupList[i].UpdatedAt);
                                }
                                if (teacherGroupWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(teacherGroupWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, teacherGroupWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupWhereList[i].TeacherId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_id` = @teacherIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_id` = @teacherIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherIdWhere" + i, teacherGroupWhereList[i].TeacherId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupWhereList[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCodeWhere" + i, teacherGroupWhereList[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupWhereList[i].LevelCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `level_code` = @levelCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `level_code` = @levelCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelCodeWhere" + i, teacherGroupWhereList[i].LevelCode);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, teacherGroupWhereList[i].Name);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupWhereList[i].SubscriptionId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subscription_id` = @subscriptionIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `subscription_id` = @subscriptionIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionIdWhere" + i, teacherGroupWhereList[i].SubscriptionId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, teacherGroupWhereList[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, teacherGroupWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, teacherGroupWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroupWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, teacherGroupWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherGroupList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherGroupModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherGroupModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupModel>();
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

        public List<TeacherGroupModel> Delete(string connectionString, TeacherGroupModel teacherGroup)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupModel> response = new List<TeacherGroupModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM teacher_groups  ";
                if (teacherGroup != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(teacherGroup.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", teacherGroup.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherGroup.TeacherId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_id` = @teacherId");
                        else
                            sql = String.Concat(sql, " `teacher_id` = @teacherId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherId", teacherGroup.TeacherId);
                    }
                    if (!String.IsNullOrEmpty(teacherGroup.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCode", teacherGroup.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(teacherGroup.LevelCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `level_code` = @levelCode");
                        else
                            sql = String.Concat(sql, " `level_code` = @levelCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelCode", teacherGroup.LevelCode);
                    }
                    if (!String.IsNullOrEmpty(teacherGroup.Name))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `name` = @name");
                        else
                            sql = String.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", teacherGroup.Name);
                    }
                    if (!String.IsNullOrEmpty(teacherGroup.SubscriptionId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subscription_id` = @subscriptionId");
                        else
                            sql = String.Concat(sql, " `subscription_id` = @subscriptionId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionId", teacherGroup.SubscriptionId);
                    }
                    if (!String.IsNullOrEmpty(teacherGroup.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @available");
                        else
                            sql = String.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", teacherGroup.Available);
                    }
                    if (!String.IsNullOrEmpty(teacherGroup.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", teacherGroup.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherGroup.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", teacherGroup.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherGroup.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", teacherGroup.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(teacherGroup);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupModel>();
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

        public List<TeacherGroupModel> Delete(string connectionString, List<TeacherGroupModel> teacherGroup)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherGroupModel> response = new List<TeacherGroupModel>();
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
                        for (int i = 0; i < teacherGroup.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM teacher_groups  ";
                                if (teacherGroup[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(teacherGroup[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, teacherGroup[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroup[i].TeacherId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_id` = @teacherId" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_id` = @teacherId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherId" + i, teacherGroup[i].TeacherId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroup[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCode" + i, teacherGroup[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroup[i].LevelCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `level_code` = @levelCode" + i);
                                        else
                                            sql = String.Concat(sql, " `level_code` = @levelCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelCode" + i, teacherGroup[i].LevelCode);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroup[i].Name))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = String.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, teacherGroup[i].Name);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroup[i].SubscriptionId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subscription_id` = @subscriptionId" + i);
                                        else
                                            sql = String.Concat(sql, " `subscription_id` = @subscriptionId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionId" + i, teacherGroup[i].SubscriptionId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroup[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, teacherGroup[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroup[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherGroup[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroup[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, teacherGroup[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherGroup[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, teacherGroup[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherGroup[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherGroupModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherGroupModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherGroupModel>();
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
