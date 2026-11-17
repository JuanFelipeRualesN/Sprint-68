using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Teachers;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    class TeacherPlanGroupDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, TeacherPlanGroupModel> ReadD(string connectionString, TeacherPlanGroupModel teacherPlanGroup, bool firstOne, bool orderByLastDate, TeacherPlanGroupModel teacherPlanGroupWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanGroupModel> response = new Dictionary<string, TeacherPlanGroupModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_plan_id`, `group_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_groups ";
                {
                    if (teacherPlanGroup != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanGroup.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanGroup.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanId", teacherPlanGroup.TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.GroupId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `group_id` = @groupId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `group_id` = @groupId ");
                            }
                            cmd.Parameters.AddWithValue("@groupId", teacherPlanGroup.GroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanGroup.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanGroup.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanGroup.UpdatedAt);
                        }
                    }
                    if (teacherPlanGroupWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanGroupWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanIdWhereLike", "%" + teacherPlanGroupWhereLike.TeacherPlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.GroupId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `group_id` LIKE @groupIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `group_id` LIKE @groupIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@groupIdWhereLike", "%" + teacherPlanGroupWhereLike.GroupId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanGroupWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanGroupWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanGroupWhereLike.UpdatedAt + "%");
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
                                teacherPlanGroup = new TeacherPlanGroupModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanGroup.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanGroup.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanGroup.TeacherPlanId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanGroup.TeacherPlanId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanGroup.GroupId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanGroup.GroupId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanGroup.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    teacherPlanGroup.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanGroup.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    teacherPlanGroup.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanGroup.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    teacherPlanGroup.UpdatedAt = null;
                                response.Add(teacherPlanGroup.Id, teacherPlanGroup);
                                teacherPlanGroup = new TeacherPlanGroupModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherPlanGroupModel>();
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
                response = new Dictionary<string, TeacherPlanGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherPlanGroupModel> ReadD(string connectionString, TeacherPlanGroupModel teacherPlanGroup, bool orderByLastDate, int fromRow, int limit, TeacherPlanGroupModel teacherPlanGroupWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanGroupModel> response = new Dictionary<string, TeacherPlanGroupModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_plan_id`, `group_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_groups ";
                {
                    if (teacherPlanGroup != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanGroup.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanGroup.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanId", teacherPlanGroup.TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.GroupId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `group_id` = @groupId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `group_id` = @groupId ");
                            }
                            cmd.Parameters.AddWithValue("@groupId", teacherPlanGroup.GroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanGroup.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanGroup.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanGroup.UpdatedAt);
                        }
                    }
                    if (teacherPlanGroupWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanGroupWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanIdWhereLike", "%" + teacherPlanGroupWhereLike.TeacherPlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.GroupId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `group_id` LIKE @groupIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `group_id` LIKE @groupIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@groupIdWhereLike", "%" + teacherPlanGroupWhereLike.GroupId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanGroupWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanGroupWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanGroupWhereLike.UpdatedAt + "%");
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
                                teacherPlanGroup = new TeacherPlanGroupModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanGroup.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanGroup.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanGroup.TeacherPlanId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanGroup.TeacherPlanId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanGroup.GroupId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanGroup.GroupId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanGroup.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    teacherPlanGroup.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanGroup.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    teacherPlanGroup.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanGroup.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    teacherPlanGroup.UpdatedAt = null;
                                response.Add(teacherPlanGroup.Id, teacherPlanGroup);
                                teacherPlanGroup = new TeacherPlanGroupModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherPlanGroupModel>();
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
                response = new Dictionary<string, TeacherPlanGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherPlanGroupModel> ReadD(string connectionString, List<TeacherPlanGroupModel> teacherPlanGroupWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanGroupModel> response = new Dictionary<string, TeacherPlanGroupModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_plan_id`, `group_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_groups ";
                if (teacherPlanGroupWhere != null && teacherPlanGroupWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanGroupWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanGroupWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].TeacherPlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_plan_id` = @teacherPlanId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherPlanId" + i, teacherPlanGroupWhere[i].TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].GroupId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `group_id` = @groupId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@groupId" + i, teacherPlanGroupWhere[i].GroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanGroupWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanGroupWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanGroupWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanGroupWhere.Count - 1)
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
                            TeacherPlanGroupModel teacherPlanGroup = new TeacherPlanGroupModel(); if (!reader.IsDBNull(0))
                                teacherPlanGroup.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanGroup.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanGroup.TeacherPlanId = reader.GetValue(1).ToString();
                            else
                                teacherPlanGroup.TeacherPlanId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanGroup.GroupId = reader.GetValue(2).ToString();
                            else
                                teacherPlanGroup.GroupId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanGroup.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                teacherPlanGroup.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanGroup.CreatedAt = reader.GetValue(4).ToString();
                            else
                                teacherPlanGroup.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanGroup.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                teacherPlanGroup.UpdatedAt = null;
                            response.Add(teacherPlanGroup.Id, teacherPlanGroup);
                            teacherPlanGroup = new TeacherPlanGroupModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherPlanGroupModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherPlanGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherPlanGroupModel> ReadD(string connectionString, List<TeacherPlanGroupModel> teacherPlanGroupWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanGroupModel> response = new Dictionary<string, TeacherPlanGroupModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_plan_id`, `group_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_groups ";
                if (teacherPlanGroupWhere != null && teacherPlanGroupWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanGroupWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanGroupWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].TeacherPlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_plan_id` = @teacherPlanId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherPlanId" + i, teacherPlanGroupWhere[i].TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].GroupId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `group_id` = @groupId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@groupId" + i, teacherPlanGroupWhere[i].GroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanGroupWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanGroupWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanGroupWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanGroupWhere.Count - 1)
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
                            TeacherPlanGroupModel teacherPlanGroup = new TeacherPlanGroupModel(); if (!reader.IsDBNull(0))
                                teacherPlanGroup.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanGroup.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanGroup.TeacherPlanId = reader.GetValue(1).ToString();
                            else
                                teacherPlanGroup.TeacherPlanId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanGroup.GroupId = reader.GetValue(2).ToString();
                            else
                                teacherPlanGroup.GroupId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanGroup.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                teacherPlanGroup.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanGroup.CreatedAt = reader.GetValue(4).ToString();
                            else
                                teacherPlanGroup.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanGroup.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                teacherPlanGroup.UpdatedAt = null;
                            response.Add(teacherPlanGroup.Id, teacherPlanGroup);
                            teacherPlanGroup = new TeacherPlanGroupModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherPlanGroupModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherPlanGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanGroupModel> Read(string connectionString, TeacherPlanGroupModel teacherPlanGroup, bool firstOne, bool orderByLastDate, TeacherPlanGroupModel teacherPlanGroupWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanGroupModel> response = new List<TeacherPlanGroupModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_plan_id`, `group_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_groups ";
                {
                    if (teacherPlanGroup != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanGroup.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanGroup.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanId", teacherPlanGroup.TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.GroupId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `group_id` = @groupId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `group_id` = @groupId ");
                            }
                            cmd.Parameters.AddWithValue("@groupId", teacherPlanGroup.GroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanGroup.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanGroup.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanGroup.UpdatedAt);
                        }
                    }
                    if (teacherPlanGroupWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanGroupWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanIdWhereLike", "%" + teacherPlanGroupWhereLike.TeacherPlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.GroupId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `group_id` LIKE @groupIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `group_id` LIKE @groupIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@groupIdWhereLike", "%" + teacherPlanGroupWhereLike.GroupId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanGroupWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanGroupWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanGroupWhereLike.UpdatedAt + "%");
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
                                teacherPlanGroup = new TeacherPlanGroupModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanGroup.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanGroup.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanGroup.TeacherPlanId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanGroup.TeacherPlanId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanGroup.GroupId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanGroup.GroupId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanGroup.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    teacherPlanGroup.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanGroup.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    teacherPlanGroup.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanGroup.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    teacherPlanGroup.UpdatedAt = null;
                                response.Add(teacherPlanGroup);
                                teacherPlanGroup = new TeacherPlanGroupModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherPlanGroupModel>();
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
                response = new List<TeacherPlanGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanGroupModel> Read(string connectionString, TeacherPlanGroupModel teacherPlanGroup, bool orderByLastDate, int fromRow, int limit, TeacherPlanGroupModel teacherPlanGroupWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanGroupModel> response = new List<TeacherPlanGroupModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_plan_id`, `group_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_groups ";
                {
                    if (teacherPlanGroup != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanGroup.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanGroup.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanId", teacherPlanGroup.TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.GroupId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `group_id` = @groupId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `group_id` = @groupId ");
                            }
                            cmd.Parameters.AddWithValue("@groupId", teacherPlanGroup.GroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanGroup.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanGroup.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroup.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanGroup.UpdatedAt);
                        }
                    }
                    if (teacherPlanGroupWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanGroupWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanIdWhereLike", "%" + teacherPlanGroupWhereLike.TeacherPlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.GroupId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `group_id` LIKE @groupIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `group_id` LIKE @groupIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@groupIdWhereLike", "%" + teacherPlanGroupWhereLike.GroupId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanGroupWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanGroupWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanGroupWhereLike.UpdatedAt + "%");
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
                                teacherPlanGroup = new TeacherPlanGroupModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanGroup.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanGroup.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanGroup.TeacherPlanId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanGroup.TeacherPlanId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanGroup.GroupId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanGroup.GroupId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanGroup.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    teacherPlanGroup.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanGroup.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    teacherPlanGroup.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanGroup.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    teacherPlanGroup.UpdatedAt = null;
                                response.Add(teacherPlanGroup);
                                teacherPlanGroup = new TeacherPlanGroupModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherPlanGroupModel>();
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
                response = new List<TeacherPlanGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanGroupModel> Read(string connectionString, List<TeacherPlanGroupModel> teacherPlanGroupWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanGroupModel> response = new List<TeacherPlanGroupModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_plan_id`, `group_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_groups ";
                if (teacherPlanGroupWhere != null && teacherPlanGroupWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanGroupWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanGroupWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].TeacherPlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_plan_id` = @teacherPlanId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherPlanId" + i, teacherPlanGroupWhere[i].TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].GroupId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `group_id` = @groupId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@groupId" + i, teacherPlanGroupWhere[i].GroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanGroupWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanGroupWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanGroupWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanGroupWhere.Count - 1)
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
                            TeacherPlanGroupModel teacherPlanGroup = new TeacherPlanGroupModel(); if (!reader.IsDBNull(0))
                                teacherPlanGroup.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanGroup.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanGroup.TeacherPlanId = reader.GetValue(1).ToString();
                            else
                                teacherPlanGroup.TeacherPlanId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanGroup.GroupId = reader.GetValue(2).ToString();
                            else
                                teacherPlanGroup.GroupId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanGroup.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                teacherPlanGroup.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanGroup.CreatedAt = reader.GetValue(4).ToString();
                            else
                                teacherPlanGroup.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanGroup.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                teacherPlanGroup.UpdatedAt = null;
                            response.Add(teacherPlanGroup);
                            teacherPlanGroup = new TeacherPlanGroupModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherPlanGroupModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanGroupModel> Read(string connectionString, List<TeacherPlanGroupModel> teacherPlanGroupWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanGroupModel> response = new List<TeacherPlanGroupModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_plan_id`, `group_id`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_groups ";
                if (teacherPlanGroupWhere != null && teacherPlanGroupWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanGroupWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanGroupWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].TeacherPlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_plan_id` = @teacherPlanId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherPlanId" + i, teacherPlanGroupWhere[i].TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].GroupId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `group_id` = @groupId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@groupId" + i, teacherPlanGroupWhere[i].GroupId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanGroupWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanGroupWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanGroupWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanGroupWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanGroupWhere.Count - 1)
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
                            TeacherPlanGroupModel teacherPlanGroup = new TeacherPlanGroupModel(); if (!reader.IsDBNull(0))
                                teacherPlanGroup.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanGroup.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanGroup.TeacherPlanId = reader.GetValue(1).ToString();
                            else
                                teacherPlanGroup.TeacherPlanId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanGroup.GroupId = reader.GetValue(2).ToString();
                            else
                                teacherPlanGroup.GroupId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanGroup.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                teacherPlanGroup.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanGroup.CreatedAt = reader.GetValue(4).ToString();
                            else
                                teacherPlanGroup.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanGroup.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                teacherPlanGroup.UpdatedAt = null;
                            response.Add(teacherPlanGroup);
                            teacherPlanGroup = new TeacherPlanGroupModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherPlanGroupModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanGroupModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanGroupModel> Create(string connectionString, TeacherPlanGroupModel teacherPlanGroup)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanGroupModel> response = new List<TeacherPlanGroupModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO teacher_plan_groups (";
                string values = "(";
                if (!String.IsNullOrEmpty(teacherPlanGroup.Id))
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
                    cmd.Parameters.AddWithValue("@id", teacherPlanGroup.Id);
                }
                if (!String.IsNullOrEmpty(teacherPlanGroup.TeacherPlanId))
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
                    cmd.Parameters.AddWithValue("@teacherPlanId", teacherPlanGroup.TeacherPlanId);
                }
                if (!String.IsNullOrEmpty(teacherPlanGroup.GroupId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `group_id` ");
                        values = String.Concat(values, ", @groupId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `group_id` ");
                        values = String.Concat(values, " @groupId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@groupId", teacherPlanGroup.GroupId);
                }
                if (!String.IsNullOrEmpty(teacherPlanGroup.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanGroup.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherPlanGroup.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", teacherPlanGroup.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherPlanGroup.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", teacherPlanGroup.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                teacherPlanGroup.Id = cmd.LastInsertedId.ToString();
                response.Add(teacherPlanGroup);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanGroupModel>();
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
        public List<TeacherPlanGroupModel> Create(string connectionString, List<TeacherPlanGroupModel> teacherPlanGroupList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanGroupModel> response = new List<TeacherPlanGroupModel>();
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
                        for (int i = 0; i < teacherPlanGroupList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO teacher_plan_groups (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(teacherPlanGroupList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, teacherPlanGroupList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanGroupList[i].TeacherPlanId))
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
                                    cmd.Parameters.AddWithValue("@teacherPlanId" + i, teacherPlanGroupList[i].TeacherPlanId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanGroupList[i].GroupId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `group_id` ");
                                        values = String.Concat(values, ", @groupId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `group_id` ");
                                        values = String.Concat(values, "@groupId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@groupId" + i, teacherPlanGroupList[i].GroupId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanGroupList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanGroupList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanGroupList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanGroupList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanGroupList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanGroupList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(teacherPlanGroupList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherPlanGroupModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherPlanGroupModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanGroupModel>();
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

        public List<TeacherPlanGroupModel> Update(string connectionString, TeacherPlanGroupModel teacherPlanGroup, TeacherPlanGroupModel teacherPlanGroupWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanGroupModel> response = new List<TeacherPlanGroupModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE teacher_plan_groups SET ";
                if (!String.IsNullOrEmpty(teacherPlanGroup.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", teacherPlanGroup.Id);
                }
                if (!String.IsNullOrEmpty(teacherPlanGroup.TeacherPlanId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `teacher_plan_id` = @teacherPlanId");
                    else
                        sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teacherPlanId", teacherPlanGroup.TeacherPlanId);
                }
                if (!String.IsNullOrEmpty(teacherPlanGroup.GroupId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `group_id` = @groupId");
                    else
                        sql = String.Concat(sql, " `group_id` = @groupId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@groupId", teacherPlanGroup.GroupId);
                }
                if (!String.IsNullOrEmpty(teacherPlanGroup.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanGroup.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherPlanGroup.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", teacherPlanGroup.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherPlanGroup.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", teacherPlanGroup.UpdatedAt);
                }
                if (teacherPlanGroupWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(teacherPlanGroupWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", teacherPlanGroupWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanGroupWhere.TeacherPlanId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_plan_id` = @teacherPlanIdWhere");
                        else
                            sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherPlanIdWhere", teacherPlanGroupWhere.TeacherPlanId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanGroupWhere.GroupId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `group_id` = @groupIdWhere");
                        else
                            sql = String.Concat(sql, " `group_id` = @groupIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@groupIdWhere", teacherPlanGroupWhere.GroupId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanGroupWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", teacherPlanGroupWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanGroupWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", teacherPlanGroupWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanGroupWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", teacherPlanGroupWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(teacherPlanGroup);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanGroupModel>();
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

        public List<TeacherPlanGroupModel> Update(string connectionString, List<TeacherPlanGroupModel> teacherPlanGroupList, List<TeacherPlanGroupModel> teacherPlanGroupWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanGroupModel> response = new List<TeacherPlanGroupModel>();
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
                        for (int i = 0; i < teacherPlanGroupList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE teacher_plan_groups SET ";
                                if (!String.IsNullOrEmpty(teacherPlanGroupList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, teacherPlanGroupList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanGroupList[i].TeacherPlanId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `teacher_plan_id` = @teacherPlanId" + i);
                                    else
                                        sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teacherPlanId" + i, teacherPlanGroupList[i].TeacherPlanId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanGroupList[i].GroupId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `group_id` = @groupId" + i);
                                    else
                                        sql = String.Concat(sql, " `group_id` = @groupId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@groupId" + i, teacherPlanGroupList[i].GroupId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanGroupList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanGroupList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanGroupList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanGroupList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanGroupList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanGroupList[i].UpdatedAt);
                                }
                                if (teacherPlanGroupWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(teacherPlanGroupWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, teacherPlanGroupWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanGroupWhereList[i].TeacherPlanId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_plan_id` = @teacherPlanIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherPlanIdWhere" + i, teacherPlanGroupWhereList[i].TeacherPlanId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanGroupWhereList[i].GroupId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `group_id` = @groupIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `group_id` = @groupIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@groupIdWhere" + i, teacherPlanGroupWhereList[i].GroupId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanGroupWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, teacherPlanGroupWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanGroupWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, teacherPlanGroupWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanGroupWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, teacherPlanGroupWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherPlanGroupList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherPlanGroupModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherPlanGroupModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanGroupModel>();
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

        public List<TeacherPlanGroupModel> Delete(string connectionString, TeacherPlanGroupModel teacherPlanGroup)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanGroupModel> response = new List<TeacherPlanGroupModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM teacher_plan_groups  ";
                if (teacherPlanGroup != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(teacherPlanGroup.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", teacherPlanGroup.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanGroup.TeacherPlanId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_plan_id` = @teacherPlanId");
                        else
                            sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherPlanId", teacherPlanGroup.TeacherPlanId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanGroup.GroupId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `group_id` = @groupId");
                        else
                            sql = String.Concat(sql, " `group_id` = @groupId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@groupId", teacherPlanGroup.GroupId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanGroup.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanGroup.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanGroup.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", teacherPlanGroup.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanGroup.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", teacherPlanGroup.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(teacherPlanGroup);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanGroupModel>();
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

        public List<TeacherPlanGroupModel> Delete(string connectionString, List<TeacherPlanGroupModel> teacherPlanGroup)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanGroupModel> response = new List<TeacherPlanGroupModel>();
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
                        for (int i = 0; i < teacherPlanGroup.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM teacher_plan_groups  ";
                                if (teacherPlanGroup[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(teacherPlanGroup[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, teacherPlanGroup[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanGroup[i].TeacherPlanId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_plan_id` = @teacherPlanId" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherPlanId" + i, teacherPlanGroup[i].TeacherPlanId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanGroup[i].GroupId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `group_id` = @groupId" + i);
                                        else
                                            sql = String.Concat(sql, " `group_id` = @groupId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@groupId" + i, teacherPlanGroup[i].GroupId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanGroup[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanGroup[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanGroup[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanGroup[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanGroup[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanGroup[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherPlanGroup[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherPlanGroupModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherPlanGroupModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanGroupModel>();
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
