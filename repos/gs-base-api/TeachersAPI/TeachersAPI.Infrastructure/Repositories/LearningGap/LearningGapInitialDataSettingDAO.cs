using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.LearningGap;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.LearningGap
{
    class LearningGapInitialDataSettingDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, LearningGapInitialDataSettingModel> ReadD(string connectionString, LearningGapInitialDataSettingModel learningGapInitialDataSetting, bool firstOne, bool orderByLastDate, LearningGapInitialDataSettingModel learningGapInitialDataSettingWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapInitialDataSettingModel> response = new Dictionary<string, LearningGapInitialDataSettingModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_initial_data_settings ";
                {
                    if (learningGapInitialDataSetting != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapInitialDataSetting.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", learningGapInitialDataSetting.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapInitialDataSetting.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapInitialDataSetting.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapInitialDataSetting.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapInitialDataSetting.UpdatedAt);
                        }
                    }
                    if (learningGapInitialDataSettingWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapInitialDataSettingWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + learningGapInitialDataSettingWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapInitialDataSettingWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapInitialDataSettingWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapInitialDataSettingWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapInitialDataSettingWhereLike.UpdatedAt + "%");
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
                                learningGapInitialDataSetting = new LearningGapInitialDataSettingModel();
                                if (!reader.IsDBNull(0))
                                    learningGapInitialDataSetting.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapInitialDataSetting.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapInitialDataSetting.TeacherId = reader.GetValue(1).ToString();
                                else
                                    learningGapInitialDataSetting.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapInitialDataSetting.StudentId = reader.GetValue(2).ToString();
                                else
                                    learningGapInitialDataSetting.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapInitialDataSetting.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    learningGapInitialDataSetting.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    learningGapInitialDataSetting.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    learningGapInitialDataSetting.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    learningGapInitialDataSetting.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    learningGapInitialDataSetting.UpdatedAt = null;
                                response.Add(learningGapInitialDataSetting.Id, learningGapInitialDataSetting);
                                learningGapInitialDataSetting = new LearningGapInitialDataSettingModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapInitialDataSettingModel>();
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
                response = new Dictionary<string, LearningGapInitialDataSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapInitialDataSettingModel> ReadD(string connectionString, LearningGapInitialDataSettingModel learningGapInitialDataSetting, bool orderByLastDate, int fromRow, int limit, LearningGapInitialDataSettingModel learningGapInitialDataSettingWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapInitialDataSettingModel> response = new Dictionary<string, LearningGapInitialDataSettingModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_initial_data_settings ";
                {
                    if (learningGapInitialDataSetting != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapInitialDataSetting.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", learningGapInitialDataSetting.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapInitialDataSetting.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapInitialDataSetting.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapInitialDataSetting.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapInitialDataSetting.UpdatedAt);
                        }
                    }
                    if (learningGapInitialDataSettingWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapInitialDataSettingWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + learningGapInitialDataSettingWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapInitialDataSettingWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapInitialDataSettingWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapInitialDataSettingWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapInitialDataSettingWhereLike.UpdatedAt + "%");
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
                                learningGapInitialDataSetting = new LearningGapInitialDataSettingModel();
                                if (!reader.IsDBNull(0))
                                    learningGapInitialDataSetting.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapInitialDataSetting.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapInitialDataSetting.TeacherId = reader.GetValue(1).ToString();
                                else
                                    learningGapInitialDataSetting.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapInitialDataSetting.StudentId = reader.GetValue(2).ToString();
                                else
                                    learningGapInitialDataSetting.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapInitialDataSetting.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    learningGapInitialDataSetting.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    learningGapInitialDataSetting.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    learningGapInitialDataSetting.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    learningGapInitialDataSetting.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    learningGapInitialDataSetting.UpdatedAt = null;
                                response.Add(learningGapInitialDataSetting.Id, learningGapInitialDataSetting);
                                learningGapInitialDataSetting = new LearningGapInitialDataSettingModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapInitialDataSettingModel>();
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
                response = new Dictionary<string, LearningGapInitialDataSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapInitialDataSettingModel> ReadD(string connectionString, List<LearningGapInitialDataSettingModel> learningGapInitialDataSettingWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapInitialDataSettingModel> response = new Dictionary<string, LearningGapInitialDataSettingModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_initial_data_settings ";
                if (learningGapInitialDataSettingWhere != null && learningGapInitialDataSettingWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapInitialDataSettingWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapInitialDataSettingWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, learningGapInitialDataSettingWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapInitialDataSettingWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapInitialDataSettingWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapInitialDataSettingWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapInitialDataSettingWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapInitialDataSettingWhere.Count - 1)
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
                            LearningGapInitialDataSettingModel learningGapInitialDataSetting = new LearningGapInitialDataSettingModel(); if (!reader.IsDBNull(0))
                                learningGapInitialDataSetting.Id = reader.GetValue(0).ToString();
                            else
                                learningGapInitialDataSetting.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapInitialDataSetting.TeacherId = reader.GetValue(1).ToString();
                            else
                                learningGapInitialDataSetting.TeacherId = null;
                            if (!reader.IsDBNull(2))
                                learningGapInitialDataSetting.StudentId = reader.GetValue(2).ToString();
                            else
                                learningGapInitialDataSetting.StudentId = null;
                            if (!reader.IsDBNull(3))
                                learningGapInitialDataSetting.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                learningGapInitialDataSetting.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                learningGapInitialDataSetting.CreatedAt = reader.GetValue(4).ToString();
                            else
                                learningGapInitialDataSetting.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                learningGapInitialDataSetting.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                learningGapInitialDataSetting.UpdatedAt = null;
                            response.Add(learningGapInitialDataSetting.Id, learningGapInitialDataSetting);
                            learningGapInitialDataSetting = new LearningGapInitialDataSettingModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapInitialDataSettingModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapInitialDataSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapInitialDataSettingModel> ReadD(string connectionString, List<LearningGapInitialDataSettingModel> learningGapInitialDataSettingWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapInitialDataSettingModel> response = new Dictionary<string, LearningGapInitialDataSettingModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_initial_data_settings ";
                if (learningGapInitialDataSettingWhere != null && learningGapInitialDataSettingWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapInitialDataSettingWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapInitialDataSettingWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, learningGapInitialDataSettingWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapInitialDataSettingWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapInitialDataSettingWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapInitialDataSettingWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapInitialDataSettingWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapInitialDataSettingWhere.Count - 1)
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
                            LearningGapInitialDataSettingModel learningGapInitialDataSetting = new LearningGapInitialDataSettingModel(); if (!reader.IsDBNull(0))
                                learningGapInitialDataSetting.Id = reader.GetValue(0).ToString();
                            else
                                learningGapInitialDataSetting.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapInitialDataSetting.TeacherId = reader.GetValue(1).ToString();
                            else
                                learningGapInitialDataSetting.TeacherId = null;
                            if (!reader.IsDBNull(2))
                                learningGapInitialDataSetting.StudentId = reader.GetValue(2).ToString();
                            else
                                learningGapInitialDataSetting.StudentId = null;
                            if (!reader.IsDBNull(3))
                                learningGapInitialDataSetting.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                learningGapInitialDataSetting.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                learningGapInitialDataSetting.CreatedAt = reader.GetValue(4).ToString();
                            else
                                learningGapInitialDataSetting.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                learningGapInitialDataSetting.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                learningGapInitialDataSetting.UpdatedAt = null;
                            response.Add(learningGapInitialDataSetting.Id, learningGapInitialDataSetting);
                            learningGapInitialDataSetting = new LearningGapInitialDataSettingModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapInitialDataSettingModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapInitialDataSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapInitialDataSettingModel> Read(string connectionString, LearningGapInitialDataSettingModel learningGapInitialDataSetting, bool firstOne, bool orderByLastDate, LearningGapInitialDataSettingModel learningGapInitialDataSettingWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapInitialDataSettingModel> response = new List<LearningGapInitialDataSettingModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_initial_data_settings ";
                {
                    if (learningGapInitialDataSetting != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapInitialDataSetting.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", learningGapInitialDataSetting.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapInitialDataSetting.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapInitialDataSetting.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapInitialDataSetting.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapInitialDataSetting.UpdatedAt);
                        }
                    }
                    if (learningGapInitialDataSettingWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapInitialDataSettingWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + learningGapInitialDataSettingWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapInitialDataSettingWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapInitialDataSettingWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapInitialDataSettingWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapInitialDataSettingWhereLike.UpdatedAt + "%");
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
                                learningGapInitialDataSetting = new LearningGapInitialDataSettingModel();
                                if (!reader.IsDBNull(0))
                                    learningGapInitialDataSetting.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapInitialDataSetting.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapInitialDataSetting.TeacherId = reader.GetValue(1).ToString();
                                else
                                    learningGapInitialDataSetting.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapInitialDataSetting.StudentId = reader.GetValue(2).ToString();
                                else
                                    learningGapInitialDataSetting.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapInitialDataSetting.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    learningGapInitialDataSetting.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    learningGapInitialDataSetting.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    learningGapInitialDataSetting.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    learningGapInitialDataSetting.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    learningGapInitialDataSetting.UpdatedAt = null;
                                response.Add(learningGapInitialDataSetting);
                                learningGapInitialDataSetting = new LearningGapInitialDataSettingModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapInitialDataSettingModel>();
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
                response = new List<LearningGapInitialDataSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapInitialDataSettingModel> Read(string connectionString, LearningGapInitialDataSettingModel learningGapInitialDataSetting, bool orderByLastDate, int fromRow, int limit, LearningGapInitialDataSettingModel learningGapInitialDataSettingWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapInitialDataSettingModel> response = new List<LearningGapInitialDataSettingModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_initial_data_settings ";
                {
                    if (learningGapInitialDataSetting != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapInitialDataSetting.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherId", learningGapInitialDataSetting.TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentId", learningGapInitialDataSetting.StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapInitialDataSetting.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapInitialDataSetting.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSetting.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapInitialDataSetting.UpdatedAt);
                        }
                    }
                    if (learningGapInitialDataSettingWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapInitialDataSettingWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.TeacherId))
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
                            cmd.Parameters.AddWithValue("@teacherIdWhereLike", "%" + learningGapInitialDataSettingWhereLike.TeacherId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.StudentId))
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
                            cmd.Parameters.AddWithValue("@studentIdWhereLike", "%" + learningGapInitialDataSettingWhereLike.StudentId + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapInitialDataSettingWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapInitialDataSettingWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapInitialDataSettingWhereLike.UpdatedAt + "%");
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
                                learningGapInitialDataSetting = new LearningGapInitialDataSettingModel();
                                if (!reader.IsDBNull(0))
                                    learningGapInitialDataSetting.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapInitialDataSetting.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapInitialDataSetting.TeacherId = reader.GetValue(1).ToString();
                                else
                                    learningGapInitialDataSetting.TeacherId = null;
                                if (!reader.IsDBNull(2))
                                    learningGapInitialDataSetting.StudentId = reader.GetValue(2).ToString();
                                else
                                    learningGapInitialDataSetting.StudentId = null;
                                if (!reader.IsDBNull(3))
                                    learningGapInitialDataSetting.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    learningGapInitialDataSetting.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    learningGapInitialDataSetting.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    learningGapInitialDataSetting.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    learningGapInitialDataSetting.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    learningGapInitialDataSetting.UpdatedAt = null;
                                response.Add(learningGapInitialDataSetting);
                                learningGapInitialDataSetting = new LearningGapInitialDataSettingModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapInitialDataSettingModel>();
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
                response = new List<LearningGapInitialDataSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapInitialDataSettingModel> Read(string connectionString, List<LearningGapInitialDataSettingModel> learningGapInitialDataSettingWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapInitialDataSettingModel> response = new List<LearningGapInitialDataSettingModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_initial_data_settings ";
                if (learningGapInitialDataSettingWhere != null && learningGapInitialDataSettingWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapInitialDataSettingWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapInitialDataSettingWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, learningGapInitialDataSettingWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapInitialDataSettingWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapInitialDataSettingWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapInitialDataSettingWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapInitialDataSettingWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapInitialDataSettingWhere.Count - 1)
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
                            LearningGapInitialDataSettingModel learningGapInitialDataSetting = new LearningGapInitialDataSettingModel(); if (!reader.IsDBNull(0))
                                learningGapInitialDataSetting.Id = reader.GetValue(0).ToString();
                            else
                                learningGapInitialDataSetting.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapInitialDataSetting.TeacherId = reader.GetValue(1).ToString();
                            else
                                learningGapInitialDataSetting.TeacherId = null;
                            if (!reader.IsDBNull(2))
                                learningGapInitialDataSetting.StudentId = reader.GetValue(2).ToString();
                            else
                                learningGapInitialDataSetting.StudentId = null;
                            if (!reader.IsDBNull(3))
                                learningGapInitialDataSetting.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                learningGapInitialDataSetting.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                learningGapInitialDataSetting.CreatedAt = reader.GetValue(4).ToString();
                            else
                                learningGapInitialDataSetting.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                learningGapInitialDataSetting.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                learningGapInitialDataSetting.UpdatedAt = null;
                            response.Add(learningGapInitialDataSetting);
                            learningGapInitialDataSetting = new LearningGapInitialDataSettingModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapInitialDataSettingModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapInitialDataSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapInitialDataSettingModel> Read(string connectionString, List<LearningGapInitialDataSettingModel> learningGapInitialDataSettingWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapInitialDataSettingModel> response = new List<LearningGapInitialDataSettingModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_id`, `student_id`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_initial_data_settings ";
                if (learningGapInitialDataSettingWhere != null && learningGapInitialDataSettingWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapInitialDataSettingWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapInitialDataSettingWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].TeacherId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_id` = @teacherId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherId" + i, learningGapInitialDataSettingWhere[i].TeacherId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].StudentId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `student_id` = @studentId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@studentId" + i, learningGapInitialDataSettingWhere[i].StudentId);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapInitialDataSettingWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapInitialDataSettingWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapInitialDataSettingWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapInitialDataSettingWhere.Count - 1)
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
                            LearningGapInitialDataSettingModel learningGapInitialDataSetting = new LearningGapInitialDataSettingModel(); if (!reader.IsDBNull(0))
                                learningGapInitialDataSetting.Id = reader.GetValue(0).ToString();
                            else
                                learningGapInitialDataSetting.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapInitialDataSetting.TeacherId = reader.GetValue(1).ToString();
                            else
                                learningGapInitialDataSetting.TeacherId = null;
                            if (!reader.IsDBNull(2))
                                learningGapInitialDataSetting.StudentId = reader.GetValue(2).ToString();
                            else
                                learningGapInitialDataSetting.StudentId = null;
                            if (!reader.IsDBNull(3))
                                learningGapInitialDataSetting.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                learningGapInitialDataSetting.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                learningGapInitialDataSetting.CreatedAt = reader.GetValue(4).ToString();
                            else
                                learningGapInitialDataSetting.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                learningGapInitialDataSetting.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                learningGapInitialDataSetting.UpdatedAt = null;
                            response.Add(learningGapInitialDataSetting);
                            learningGapInitialDataSetting = new LearningGapInitialDataSettingModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapInitialDataSettingModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapInitialDataSettingModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapInitialDataSettingModel> Create(string connectionString, LearningGapInitialDataSettingModel learningGapInitialDataSetting)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapInitialDataSettingModel> response = new List<LearningGapInitialDataSettingModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO learning_gap_initial_data_settings (";
                string values = "(";
                if (!String.IsNullOrEmpty(learningGapInitialDataSetting.Id))
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
                    cmd.Parameters.AddWithValue("@id", learningGapInitialDataSetting.Id);
                }
                if (!String.IsNullOrEmpty(learningGapInitialDataSetting.TeacherId))
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
                    cmd.Parameters.AddWithValue("@teacherId", learningGapInitialDataSetting.TeacherId);
                }
                if (!String.IsNullOrEmpty(learningGapInitialDataSetting.StudentId))
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
                    cmd.Parameters.AddWithValue("@studentId", learningGapInitialDataSetting.StudentId);
                }
                if (!String.IsNullOrEmpty(learningGapInitialDataSetting.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapInitialDataSetting.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapInitialDataSetting.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", learningGapInitialDataSetting.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapInitialDataSetting.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapInitialDataSetting.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                learningGapInitialDataSetting.Id = cmd.LastInsertedId.ToString();
                response.Add(learningGapInitialDataSetting);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapInitialDataSettingModel>();
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
        public List<LearningGapInitialDataSettingModel> Create(string connectionString, List<LearningGapInitialDataSettingModel> learningGapInitialDataSettingList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapInitialDataSettingModel> response = new List<LearningGapInitialDataSettingModel>();
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
                        for (int i = 0; i < learningGapInitialDataSettingList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO learning_gap_initial_data_settings (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(learningGapInitialDataSettingList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapInitialDataSettingList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapInitialDataSettingList[i].TeacherId))
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
                                    cmd.Parameters.AddWithValue("@teacherId" + i, learningGapInitialDataSettingList[i].TeacherId);
                                }
                                if (!String.IsNullOrEmpty(learningGapInitialDataSettingList[i].StudentId))
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
                                    cmd.Parameters.AddWithValue("@studentId" + i, learningGapInitialDataSettingList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(learningGapInitialDataSettingList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapInitialDataSettingList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapInitialDataSettingList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapInitialDataSettingList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapInitialDataSettingList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapInitialDataSettingList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(learningGapInitialDataSettingList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapInitialDataSettingModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapInitialDataSettingModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapInitialDataSettingModel>();
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

        public List<LearningGapInitialDataSettingModel> Update(string connectionString, LearningGapInitialDataSettingModel learningGapInitialDataSetting, LearningGapInitialDataSettingModel learningGapInitialDataSettingWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapInitialDataSettingModel> response = new List<LearningGapInitialDataSettingModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE learning_gap_initial_data_settings SET ";
                if (!String.IsNullOrEmpty(learningGapInitialDataSetting.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", learningGapInitialDataSetting.Id);
                }
                if (!String.IsNullOrEmpty(learningGapInitialDataSetting.TeacherId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `teacher_id` = @teacherId");
                    else
                        sql = String.Concat(sql, " `teacher_id` = @teacherId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teacherId", learningGapInitialDataSetting.TeacherId);
                }
                if (!String.IsNullOrEmpty(learningGapInitialDataSetting.StudentId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `student_id` = @studentId");
                    else
                        sql = String.Concat(sql, " `student_id` = @studentId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@studentId", learningGapInitialDataSetting.StudentId);
                }
                if (!String.IsNullOrEmpty(learningGapInitialDataSetting.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapInitialDataSetting.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapInitialDataSetting.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", learningGapInitialDataSetting.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapInitialDataSetting.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapInitialDataSetting.UpdatedAt);
                }
                if (learningGapInitialDataSettingWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", learningGapInitialDataSettingWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere.TeacherId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_id` = @teacherIdWhere");
                        else
                            sql = String.Concat(sql, " `teacher_id` = @teacherIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherIdWhere", learningGapInitialDataSettingWhere.TeacherId);
                    }
                    if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentIdWhere", learningGapInitialDataSettingWhere.StudentId);
                    }
                    if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", learningGapInitialDataSettingWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", learningGapInitialDataSettingWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", learningGapInitialDataSettingWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(learningGapInitialDataSetting);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapInitialDataSettingModel>();
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

        public List<LearningGapInitialDataSettingModel> Update(string connectionString, List<LearningGapInitialDataSettingModel> learningGapInitialDataSettingList, List<LearningGapInitialDataSettingModel> learningGapInitialDataSettingWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapInitialDataSettingModel> response = new List<LearningGapInitialDataSettingModel>();
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
                        for (int i = 0; i < learningGapInitialDataSettingList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE learning_gap_initial_data_settings SET ";
                                if (!String.IsNullOrEmpty(learningGapInitialDataSettingList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapInitialDataSettingList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapInitialDataSettingList[i].TeacherId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `teacher_id` = @teacherId" + i);
                                    else
                                        sql = String.Concat(sql, " `teacher_id` = @teacherId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teacherId" + i, learningGapInitialDataSettingList[i].TeacherId);
                                }
                                if (!String.IsNullOrEmpty(learningGapInitialDataSettingList[i].StudentId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `student_id` = @studentId" + i);
                                    else
                                        sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@studentId" + i, learningGapInitialDataSettingList[i].StudentId);
                                }
                                if (!String.IsNullOrEmpty(learningGapInitialDataSettingList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapInitialDataSettingList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapInitialDataSettingList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapInitialDataSettingList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapInitialDataSettingList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapInitialDataSettingList[i].UpdatedAt);
                                }
                                if (learningGapInitialDataSettingWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, learningGapInitialDataSettingWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereList[i].TeacherId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_id` = @teacherIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_id` = @teacherIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherIdWhere" + i, learningGapInitialDataSettingWhereList[i].TeacherId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereList[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentIdWhere" + i, learningGapInitialDataSettingWhereList[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, learningGapInitialDataSettingWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, learningGapInitialDataSettingWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapInitialDataSettingWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, learningGapInitialDataSettingWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapInitialDataSettingList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapInitialDataSettingModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapInitialDataSettingModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapInitialDataSettingModel>();
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

        public List<LearningGapInitialDataSettingModel> Delete(string connectionString, LearningGapInitialDataSettingModel learningGapInitialDataSetting)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapInitialDataSettingModel> response = new List<LearningGapInitialDataSettingModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM learning_gap_initial_data_settings  ";
                if (learningGapInitialDataSetting != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(learningGapInitialDataSetting.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", learningGapInitialDataSetting.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapInitialDataSetting.TeacherId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_id` = @teacherId");
                        else
                            sql = String.Concat(sql, " `teacher_id` = @teacherId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherId", learningGapInitialDataSetting.TeacherId);
                    }
                    if (!String.IsNullOrEmpty(learningGapInitialDataSetting.StudentId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `student_id` = @studentId");
                        else
                            sql = String.Concat(sql, " `student_id` = @studentId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@studentId", learningGapInitialDataSetting.StudentId);
                    }
                    if (!String.IsNullOrEmpty(learningGapInitialDataSetting.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", learningGapInitialDataSetting.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapInitialDataSetting.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", learningGapInitialDataSetting.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapInitialDataSetting.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", learningGapInitialDataSetting.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(learningGapInitialDataSetting);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapInitialDataSettingModel>();
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

        public List<LearningGapInitialDataSettingModel> Delete(string connectionString, List<LearningGapInitialDataSettingModel> learningGapInitialDataSetting)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapInitialDataSettingModel> response = new List<LearningGapInitialDataSettingModel>();
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
                        for (int i = 0; i < learningGapInitialDataSetting.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM learning_gap_initial_data_settings  ";
                                if (learningGapInitialDataSetting[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(learningGapInitialDataSetting[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, learningGapInitialDataSetting[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapInitialDataSetting[i].TeacherId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_id` = @teacherId" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_id` = @teacherId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherId" + i, learningGapInitialDataSetting[i].TeacherId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapInitialDataSetting[i].StudentId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `student_id` = @studentId" + i);
                                        else
                                            sql = String.Concat(sql, " `student_id` = @studentId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@studentId" + i, learningGapInitialDataSetting[i].StudentId);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapInitialDataSetting[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapInitialDataSetting[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapInitialDataSetting[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, learningGapInitialDataSetting[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapInitialDataSetting[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapInitialDataSetting[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapInitialDataSetting[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapInitialDataSettingModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapInitialDataSettingModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapInitialDataSettingModel>();
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
