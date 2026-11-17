using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Teachers;
using Dapper;
using System.Linq;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    class ForumDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, ForumModel> ReadD(string connectionString, ForumModel forum, bool firstOne, bool orderByLastDate, ForumModel forumWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ForumModel> response = new Dictionary<string, ForumModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_plan_id`, `title`, `active`, `author`, `description`, `modified_by`, `created_at`, `updated_at`  FROM forums ";
                {
                    if (forum != null)
                    {
                        if (!String.IsNullOrEmpty(forum.Id))
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
                            cmd.Parameters.AddWithValue("@id", forum.Id);
                        }
                        if (!String.IsNullOrEmpty(forum.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanId", forum.TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(forum.Title))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `title` = @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `title` = @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", forum.Title);
                        }
                        if (!String.IsNullOrEmpty(forum.Active))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `active` = @active ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `active` = @active ");
                            }
                            cmd.Parameters.AddWithValue("@active", forum.Active);
                        }
                        if (!String.IsNullOrEmpty(forum.Author))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `author` = @author ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `author` = @author ");
                            }
                            cmd.Parameters.AddWithValue("@author", forum.Author);
                        }
                        if (!String.IsNullOrEmpty(forum.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", forum.Description);
                        }
                        if (!String.IsNullOrEmpty(forum.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forum.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forum.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", forum.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forum.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forum.UpdatedAt);
                        }
                    }
                    if (forumWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(forumWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + forumWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanIdWhereLike", "%" + forumWhereLike.TeacherPlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Title))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `title` LIKE @titleWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `title` LIKE @titleWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@titleWhereLike", "%" + forumWhereLike.Title + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Active))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `active` LIKE @activeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `active` LIKE @activeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@activeWhereLike", "%" + forumWhereLike.Active + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Author))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `author` LIKE @authorWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `author` LIKE @authorWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@authorWhereLike", "%" + forumWhereLike.Author + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Description))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `description` LIKE @descriptionWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `description` LIKE @descriptionWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@descriptionWhereLike", "%" + forumWhereLike.Description + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + forumWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + forumWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + forumWhereLike.UpdatedAt + "%");
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
                                forum = new ForumModel();
                                if (!reader.IsDBNull(0))
                                    forum.Id = reader.GetValue(0).ToString();
                                else
                                    forum.Id = null;
                                if (!reader.IsDBNull(1))
                                    forum.TeacherPlanId = reader.GetValue(1).ToString();
                                else
                                    forum.TeacherPlanId = null;
                                if (!reader.IsDBNull(2))
                                    forum.Title = reader.GetValue(2).ToString();
                                else
                                    forum.Title = null;
                                if (!reader.IsDBNull(3))
                                    forum.Active = reader.GetValue(3).ToString();
                                else
                                    forum.Active = null;
                                if (!reader.IsDBNull(4))
                                    forum.Author = reader.GetValue(4).ToString();
                                else
                                    forum.Author = null;
                                if (!reader.IsDBNull(5))
                                    forum.Description = reader.GetValue(5).ToString();
                                else
                                    forum.Description = null;
                                if (!reader.IsDBNull(6))
                                    forum.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    forum.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    forum.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    forum.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    forum.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    forum.UpdatedAt = null;
                                response.Add(forum.Id, forum);
                                forum = new ForumModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, ForumModel>();
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
                response = new Dictionary<string, ForumModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ForumModel> ReadD(string connectionString, ForumModel forum, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ForumModel> response = new Dictionary<string, ForumModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_plan_id`, `title`, `active`, `author`, `description`, `modified_by`, `created_at`, `updated_at`  FROM forums ";
                {
                    if (forum != null)
                    {
                        if (!string.IsNullOrEmpty(forum.Id))
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
                            cmd.Parameters.AddWithValue("@id", forum.Id);
                        }
                        if (!string.IsNullOrEmpty(forum.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanId", forum.TeacherPlanId);
                        }
                        if (!string.IsNullOrEmpty(forum.Title))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `title` = @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `title` = @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", forum.Title);
                        }
                        if (!string.IsNullOrEmpty(forum.Active))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `active` = @active ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `active` = @active ");
                            }
                            cmd.Parameters.AddWithValue("@active", forum.Active);
                        }
                        if (!string.IsNullOrEmpty(forum.Author))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `author` = @author ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `author` = @author ");
                            }
                            cmd.Parameters.AddWithValue("@author", forum.Author);
                        }
                        if (!string.IsNullOrEmpty(forum.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", forum.Description);
                        }
                        if (!string.IsNullOrEmpty(forum.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forum.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(forum.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", forum.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(forum.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forum.UpdatedAt);
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
                                    forum.Id = reader.GetValue(0).ToString();
                                else
                                    forum.Id = null;
                                if (!reader.IsDBNull(1))
                                    forum.TeacherPlanId = reader.GetValue(1).ToString();
                                else
                                    forum.TeacherPlanId = null;
                                if (!reader.IsDBNull(2))
                                    forum.Title = reader.GetValue(2).ToString();
                                else
                                    forum.Title = null;
                                if (!reader.IsDBNull(3))
                                    forum.Active = reader.GetValue(3).ToString();
                                else
                                    forum.Active = null;
                                if (!reader.IsDBNull(4))
                                    forum.Author = reader.GetValue(4).ToString();
                                else
                                    forum.Author = null;
                                if (!reader.IsDBNull(5))
                                    forum.Description = reader.GetValue(5).ToString();
                                else
                                    forum.Description = null;
                                if (!reader.IsDBNull(6))
                                    forum.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    forum.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    forum.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    forum.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    forum.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    forum.UpdatedAt = null;
                                response.Add(forum.Id, forum);
                                forum = new ForumModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, ForumModel>();
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
                response = new Dictionary<string, ForumModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ForumModel> ReadD(string connectionString, ForumModel forum, bool orderByLastDate, int fromRow, int limit, ForumModel forumWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ForumModel> response = new Dictionary<string, ForumModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_plan_id`, `title`, `active`, `author`, `description`, `modified_by`, `created_at`, `updated_at`  FROM forums ";
                {
                    if (forum != null)
                    {
                        if (!String.IsNullOrEmpty(forum.Id))
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
                            cmd.Parameters.AddWithValue("@id", forum.Id);
                        }
                        if (!String.IsNullOrEmpty(forum.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanId", forum.TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(forum.Title))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `title` = @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `title` = @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", forum.Title);
                        }
                        if (!String.IsNullOrEmpty(forum.Active))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `active` = @active ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `active` = @active ");
                            }
                            cmd.Parameters.AddWithValue("@active", forum.Active);
                        }
                        if (!String.IsNullOrEmpty(forum.Author))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `author` = @author ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `author` = @author ");
                            }
                            cmd.Parameters.AddWithValue("@author", forum.Author);
                        }
                        if (!String.IsNullOrEmpty(forum.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", forum.Description);
                        }
                        if (!String.IsNullOrEmpty(forum.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forum.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forum.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", forum.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forum.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forum.UpdatedAt);
                        }
                    }
                    if (forumWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(forumWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + forumWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanIdWhereLike", "%" + forumWhereLike.TeacherPlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Title))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `title` LIKE @titleWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `title` LIKE @titleWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@titleWhereLike", "%" + forumWhereLike.Title + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Active))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `active` LIKE @activeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `active` LIKE @activeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@activeWhereLike", "%" + forumWhereLike.Active + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Author))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `author` LIKE @authorWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `author` LIKE @authorWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@authorWhereLike", "%" + forumWhereLike.Author + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Description))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `description` LIKE @descriptionWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `description` LIKE @descriptionWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@descriptionWhereLike", "%" + forumWhereLike.Description + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + forumWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + forumWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + forumWhereLike.UpdatedAt + "%");
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
                                forum = new ForumModel();
                                if (!reader.IsDBNull(0))
                                    forum.Id = reader.GetValue(0).ToString();
                                else
                                    forum.Id = null;
                                if (!reader.IsDBNull(1))
                                    forum.TeacherPlanId = reader.GetValue(1).ToString();
                                else
                                    forum.TeacherPlanId = null;
                                if (!reader.IsDBNull(2))
                                    forum.Title = reader.GetValue(2).ToString();
                                else
                                    forum.Title = null;
                                if (!reader.IsDBNull(3))
                                    forum.Active = reader.GetValue(3).ToString();
                                else
                                    forum.Active = null;
                                if (!reader.IsDBNull(4))
                                    forum.Author = reader.GetValue(4).ToString();
                                else
                                    forum.Author = null;
                                if (!reader.IsDBNull(5))
                                    forum.Description = reader.GetValue(5).ToString();
                                else
                                    forum.Description = null;
                                if (!reader.IsDBNull(6))
                                    forum.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    forum.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    forum.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    forum.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    forum.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    forum.UpdatedAt = null;
                                response.Add(forum.Id, forum);
                                forum = new ForumModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, ForumModel>();
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
                response = new Dictionary<string, ForumModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ForumModel> ReadD(string connectionString, ForumModel forum, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ForumModel> response = new Dictionary<string, ForumModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_plan_id`, `title`, `active`, `author`, `description`, `modified_by`, `created_at`, `updated_at`  FROM forums ";
                {
                    if (forum != null)
                    {
                        if (!string.IsNullOrEmpty(forum.Id))
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
                            cmd.Parameters.AddWithValue("@id", forum.Id);
                        }
                        if (!string.IsNullOrEmpty(forum.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanId", forum.TeacherPlanId);
                        }
                        if (!string.IsNullOrEmpty(forum.Title))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `title` = @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `title` = @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", forum.Title);
                        }
                        if (!string.IsNullOrEmpty(forum.Active))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `active` = @active ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `active` = @active ");
                            }
                            cmd.Parameters.AddWithValue("@active", forum.Active);
                        }
                        if (!string.IsNullOrEmpty(forum.Author))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `author` = @author ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `author` = @author ");
                            }
                            cmd.Parameters.AddWithValue("@author", forum.Author);
                        }
                        if (!string.IsNullOrEmpty(forum.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", forum.Description);
                        }
                        if (!string.IsNullOrEmpty(forum.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forum.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(forum.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", forum.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(forum.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forum.UpdatedAt);
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
                                    forum.Id = reader.GetValue(0).ToString();
                                else
                                    forum.Id = null;
                                if (!reader.IsDBNull(1))
                                    forum.TeacherPlanId = reader.GetValue(1).ToString();
                                else
                                    forum.TeacherPlanId = null;
                                if (!reader.IsDBNull(2))
                                    forum.Title = reader.GetValue(2).ToString();
                                else
                                    forum.Title = null;
                                if (!reader.IsDBNull(3))
                                    forum.Active = reader.GetValue(3).ToString();
                                else
                                    forum.Active = null;
                                if (!reader.IsDBNull(4))
                                    forum.Author = reader.GetValue(4).ToString();
                                else
                                    forum.Author = null;
                                if (!reader.IsDBNull(5))
                                    forum.Description = reader.GetValue(5).ToString();
                                else
                                    forum.Description = null;
                                if (!reader.IsDBNull(6))
                                    forum.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    forum.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    forum.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    forum.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    forum.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    forum.UpdatedAt = null;
                                response.Add(forum.Id, forum);
                                forum = new ForumModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, ForumModel>();
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
                response = new Dictionary<string, ForumModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ForumModel> ReadD(string connectionString, List<ForumModel> forumWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ForumModel> response = new Dictionary<string, ForumModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_plan_id`, `title`, `active`, `author`, `description`, `modified_by`, `created_at`, `updated_at`  FROM forums ";
                if (forumWhere != null && forumWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < forumWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(forumWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, forumWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].TeacherPlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_plan_id` = @teacherPlanId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherPlanId" + i, forumWhere[i].TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Title))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `title` = @title" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@title" + i, forumWhere[i].Title);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Active))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `active` = @active" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@active" + i, forumWhere[i].Active);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Author))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `author` = @author" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@author" + i, forumWhere[i].Author);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, forumWhere[i].Description);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, forumWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, forumWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, forumWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != forumWhere.Count - 1)
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
                            ForumModel forum = new ForumModel(); if (!reader.IsDBNull(0))
                                forum.Id = reader.GetValue(0).ToString();
                            else
                                forum.Id = null;
                            if (!reader.IsDBNull(1))
                                forum.TeacherPlanId = reader.GetValue(1).ToString();
                            else
                                forum.TeacherPlanId = null;
                            if (!reader.IsDBNull(2))
                                forum.Title = reader.GetValue(2).ToString();
                            else
                                forum.Title = null;
                            if (!reader.IsDBNull(3))
                                forum.Active = reader.GetValue(3).ToString();
                            else
                                forum.Active = null;
                            if (!reader.IsDBNull(4))
                                forum.Author = reader.GetValue(4).ToString();
                            else
                                forum.Author = null;
                            if (!reader.IsDBNull(5))
                                forum.Description = reader.GetValue(5).ToString();
                            else
                                forum.Description = null;
                            if (!reader.IsDBNull(6))
                                forum.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                forum.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                forum.CreatedAt = reader.GetValue(7).ToString();
                            else
                                forum.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                forum.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                forum.UpdatedAt = null;
                            response.Add(forum.Id, forum);
                            forum = new ForumModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, ForumModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, ForumModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ForumModel> ReadD(string connectionString, List<ForumModel> forumWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ForumModel> response = new Dictionary<string, ForumModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_plan_id`, `title`, `active`, `author`, `description`, `modified_by`, `created_at`, `updated_at`  FROM forums ";
                if (forumWhere != null && forumWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < forumWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(forumWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, forumWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].TeacherPlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_plan_id` = @teacherPlanId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherPlanId" + i, forumWhere[i].TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Title))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `title` = @title" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@title" + i, forumWhere[i].Title);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Active))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `active` = @active" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@active" + i, forumWhere[i].Active);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Author))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `author` = @author" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@author" + i, forumWhere[i].Author);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, forumWhere[i].Description);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, forumWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, forumWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, forumWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != forumWhere.Count - 1)
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
                            ForumModel forum = new ForumModel(); if (!reader.IsDBNull(0))
                                forum.Id = reader.GetValue(0).ToString();
                            else
                                forum.Id = null;
                            if (!reader.IsDBNull(1))
                                forum.TeacherPlanId = reader.GetValue(1).ToString();
                            else
                                forum.TeacherPlanId = null;
                            if (!reader.IsDBNull(2))
                                forum.Title = reader.GetValue(2).ToString();
                            else
                                forum.Title = null;
                            if (!reader.IsDBNull(3))
                                forum.Active = reader.GetValue(3).ToString();
                            else
                                forum.Active = null;
                            if (!reader.IsDBNull(4))
                                forum.Author = reader.GetValue(4).ToString();
                            else
                                forum.Author = null;
                            if (!reader.IsDBNull(5))
                                forum.Description = reader.GetValue(5).ToString();
                            else
                                forum.Description = null;
                            if (!reader.IsDBNull(6))
                                forum.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                forum.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                forum.CreatedAt = reader.GetValue(7).ToString();
                            else
                                forum.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                forum.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                forum.UpdatedAt = null;
                            response.Add(forum.Id, forum);
                            forum = new ForumModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, ForumModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, ForumModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumModel> Read(string connectionString, ForumModel forum, bool firstOne, bool orderByLastDate, ForumModel forumWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumModel> response = new List<ForumModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_plan_id`, `title`, `active`, `author`, `description`, `modified_by`, `created_at`, `updated_at`  FROM forums ";
                {
                    if (forum != null)
                    {
                        if (!String.IsNullOrEmpty(forum.Id))
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
                            cmd.Parameters.AddWithValue("@id", forum.Id);
                        }
                        if (!String.IsNullOrEmpty(forum.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanId", forum.TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(forum.Title))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `title` = @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `title` = @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", forum.Title);
                        }
                        if (!String.IsNullOrEmpty(forum.Active))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `active` = @active ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `active` = @active ");
                            }
                            cmd.Parameters.AddWithValue("@active", forum.Active);
                        }
                        if (!String.IsNullOrEmpty(forum.Author))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `author` = @author ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `author` = @author ");
                            }
                            cmd.Parameters.AddWithValue("@author", forum.Author);
                        }
                        if (!String.IsNullOrEmpty(forum.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", forum.Description);
                        }
                        if (!String.IsNullOrEmpty(forum.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forum.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forum.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", forum.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forum.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forum.UpdatedAt);
                        }
                    }
                    if (forumWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(forumWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + forumWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanIdWhereLike", "%" + forumWhereLike.TeacherPlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Title))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `title` LIKE @titleWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `title` LIKE @titleWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@titleWhereLike", "%" + forumWhereLike.Title + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Active))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `active` LIKE @activeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `active` LIKE @activeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@activeWhereLike", "%" + forumWhereLike.Active + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Author))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `author` LIKE @authorWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `author` LIKE @authorWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@authorWhereLike", "%" + forumWhereLike.Author + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Description))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `description` LIKE @descriptionWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `description` LIKE @descriptionWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@descriptionWhereLike", "%" + forumWhereLike.Description + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + forumWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + forumWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + forumWhereLike.UpdatedAt + "%");
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
                                forum = new ForumModel();
                                if (!reader.IsDBNull(0))
                                    forum.Id = reader.GetValue(0).ToString();
                                else
                                    forum.Id = null;
                                if (!reader.IsDBNull(1))
                                    forum.TeacherPlanId = reader.GetValue(1).ToString();
                                else
                                    forum.TeacherPlanId = null;
                                if (!reader.IsDBNull(2))
                                    forum.Title = reader.GetValue(2).ToString();
                                else
                                    forum.Title = null;
                                if (!reader.IsDBNull(3))
                                    forum.Active = reader.GetValue(3).ToString();
                                else
                                    forum.Active = null;
                                if (!reader.IsDBNull(4))
                                    forum.Author = reader.GetValue(4).ToString();
                                else
                                    forum.Author = null;
                                if (!reader.IsDBNull(5))
                                    forum.Description = reader.GetValue(5).ToString();
                                else
                                    forum.Description = null;
                                if (!reader.IsDBNull(6))
                                    forum.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    forum.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    forum.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    forum.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    forum.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    forum.UpdatedAt = null;
                                response.Add(forum);
                                forum = new ForumModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ForumModel>();
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
                response = new List<ForumModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumModel> Read(string connectionString, ForumModel forum, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumModel> response = new List<ForumModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT f.`id`, f.`teacher_plan_id`, f.`title`, f.`active`, f.`author`, f.`description`, f.`modified_by`, f.`created_at`, f.`updated_at`, pl.name as `plan_name` FROM forums f inner join teacher_plans pl on f.teacher_plan_id = pl.id";
                {
                    if (forum != null)
                    {
                        if (!string.IsNullOrEmpty(forum.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where f.`id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and f.`id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", forum.Id);
                        }
                        if (!string.IsNullOrEmpty(forum.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanId", forum.TeacherPlanId);
                        }
                        if (!string.IsNullOrEmpty(forum.Title))
                        {
                            forum.Title = $"%{forum.Title}%";
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `title` LIKE @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `title` LIKE @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", forum.Title);
                        }
                        if (!string.IsNullOrEmpty(forum.Active))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `active` = @active ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `active` = @active ");
                            }
                            cmd.Parameters.AddWithValue("@active", forum.Active);
                        }
                        if (!string.IsNullOrEmpty(forum.Author))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `author` = @author ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `author` = @author ");
                            }
                            cmd.Parameters.AddWithValue("@author", forum.Author);
                        }
                        if (!string.IsNullOrEmpty(forum.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", forum.Description);
                        }
                        if (!string.IsNullOrEmpty(forum.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forum.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(forum.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where DATE(f.`created_at`) = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and DATE(f.`created_at`) = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", forum.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(forum.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forum.UpdatedAt);
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
                                    forum.Id = reader.GetValue(0).ToString();
                                else
                                    forum.Id = null;
                                if (!reader.IsDBNull(1))
                                    forum.TeacherPlanId = reader.GetValue(1).ToString();
                                else
                                    forum.TeacherPlanId = null;
                                if (!reader.IsDBNull(2))
                                    forum.Title = reader.GetValue(2).ToString();
                                else
                                    forum.Title = null;
                                if (!reader.IsDBNull(3))
                                    forum.Active = reader.GetValue(3).ToString();
                                else
                                    forum.Active = null;
                                if (!reader.IsDBNull(4))
                                    forum.Author = reader.GetValue(4).ToString();
                                else
                                    forum.Author = null;
                                if (!reader.IsDBNull(5))
                                    forum.Description = reader.GetValue(5).ToString();
                                else
                                    forum.Description = null;
                                if (!reader.IsDBNull(6))
                                    forum.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    forum.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    forum.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    forum.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    forum.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    forum.UpdatedAt = null;
                                if (!reader.IsDBNull(9))
                                    forum.PlanName = reader.GetValue(9).ToString();
                                else
                                    forum.PlanName = null;
                                response.Add(forum);
                                forum = new ForumModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ForumModel>();
                        reader.Close();
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }

                try
                {
                    DefaultTypeMap.MatchNamesWithUnderscores = true;
                    DynamicParameters parameters = new DynamicParameters();
                    string queryIn = " IN (";
                    for (int i = 0; i < response.Count; i++)
                    {
                        queryIn += string.Concat("@forumId", i.ToString());
                        parameters.Add(string.Concat("@forumId", i.ToString()), response[i].Id);
                        if (i < response.Count - 1)
                        {
                            queryIn += ",";
                        }
                    }
                    queryIn += ") ";

                    string datesQuery = @"SELECT id, forum_id, `implementation_date`, `modified_by`, `created_at`, `updated_at` 
                                            FROM forum_dates WHERE forum_id " + queryIn + ";";
                    List<ForumDatesModel> forumDates = new List<ForumDatesModel>();

                    forumDates = _connection.Query<ForumDatesModel>(datesQuery, parameters).ToList();


                    foreach (ForumModel forumData in response)
                        forumData.ImplementationDates = forumDates.Where(d => d.ForumId == forumData.Id)?.ToList();
                }
                catch (Exception)
                {

                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumModel> Read(string connectionString, ForumModel forum, bool orderByLastDate, int fromRow, int limit, ForumModel forumWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumModel> response = new List<ForumModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_plan_id`, `title`, `active`, `author`, `description`, `modified_by`, `created_at`, `updated_at`  FROM forums ";
                {
                    if (forum != null)
                    {
                        if (!String.IsNullOrEmpty(forum.Id))
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
                            cmd.Parameters.AddWithValue("@id", forum.Id);
                        }
                        if (!String.IsNullOrEmpty(forum.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanId", forum.TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(forum.Title))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `title` = @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `title` = @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", forum.Title);
                        }
                        if (!String.IsNullOrEmpty(forum.Active))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `active` = @active ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `active` = @active ");
                            }
                            cmd.Parameters.AddWithValue("@active", forum.Active);
                        }
                        if (!String.IsNullOrEmpty(forum.Author))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `author` = @author ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `author` = @author ");
                            }
                            cmd.Parameters.AddWithValue("@author", forum.Author);
                        }
                        if (!String.IsNullOrEmpty(forum.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", forum.Description);
                        }
                        if (!String.IsNullOrEmpty(forum.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forum.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forum.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", forum.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forum.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forum.UpdatedAt);
                        }
                    }
                    if (forumWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(forumWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + forumWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanIdWhereLike", "%" + forumWhereLike.TeacherPlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Title))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `title` LIKE @titleWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `title` LIKE @titleWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@titleWhereLike", "%" + forumWhereLike.Title + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Active))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `active` LIKE @activeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `active` LIKE @activeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@activeWhereLike", "%" + forumWhereLike.Active + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Author))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `author` LIKE @authorWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `author` LIKE @authorWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@authorWhereLike", "%" + forumWhereLike.Author + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.Description))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `description` LIKE @descriptionWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `description` LIKE @descriptionWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@descriptionWhereLike", "%" + forumWhereLike.Description + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + forumWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + forumWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(forumWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + forumWhereLike.UpdatedAt + "%");
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
                                forum = new ForumModel();
                                if (!reader.IsDBNull(0))
                                    forum.Id = reader.GetValue(0).ToString();
                                else
                                    forum.Id = null;
                                if (!reader.IsDBNull(1))
                                    forum.TeacherPlanId = reader.GetValue(1).ToString();
                                else
                                    forum.TeacherPlanId = null;
                                if (!reader.IsDBNull(2))
                                    forum.Title = reader.GetValue(2).ToString();
                                else
                                    forum.Title = null;
                                if (!reader.IsDBNull(3))
                                    forum.Active = reader.GetValue(3).ToString();
                                else
                                    forum.Active = null;
                                if (!reader.IsDBNull(4))
                                    forum.Author = reader.GetValue(4).ToString();
                                else
                                    forum.Author = null;
                                if (!reader.IsDBNull(5))
                                    forum.Description = reader.GetValue(5).ToString();
                                else
                                    forum.Description = null;
                                if (!reader.IsDBNull(6))
                                    forum.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    forum.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    forum.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    forum.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    forum.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    forum.UpdatedAt = null;
                                response.Add(forum);
                                forum = new ForumModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ForumModel>();
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
                response = new List<ForumModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumModel> Read(string connectionString, ForumModel forum, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumModel> response = new List<ForumModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `teacher_plan_id`, `title`, `active`, `author`, `description`, `modified_by`, `created_at`, `updated_at`  FROM forums ";
                {
                    if (forum != null)
                    {
                        if (!string.IsNullOrEmpty(forum.Id))
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
                            cmd.Parameters.AddWithValue("@id", forum.Id);
                        }
                        if (!string.IsNullOrEmpty(forum.TeacherPlanId))
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
                            cmd.Parameters.AddWithValue("@teacherPlanId", forum.TeacherPlanId);
                        }
                        if (!string.IsNullOrEmpty(forum.Title))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `title` = @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `title` = @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", forum.Title);
                        }
                        if (!string.IsNullOrEmpty(forum.Active))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `active` = @active ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `active` = @active ");
                            }
                            cmd.Parameters.AddWithValue("@active", forum.Active);
                        }
                        if (!string.IsNullOrEmpty(forum.Author))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `author` = @author ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `author` = @author ");
                            }
                            cmd.Parameters.AddWithValue("@author", forum.Author);
                        }
                        if (!string.IsNullOrEmpty(forum.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", forum.Description);
                        }
                        if (!string.IsNullOrEmpty(forum.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forum.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(forum.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", forum.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(forum.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forum.UpdatedAt);
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
                                    forum.Id = reader.GetValue(0).ToString();
                                else
                                    forum.Id = null;
                                if (!reader.IsDBNull(1))
                                    forum.TeacherPlanId = reader.GetValue(1).ToString();
                                else
                                    forum.TeacherPlanId = null;
                                if (!reader.IsDBNull(2))
                                    forum.Title = reader.GetValue(2).ToString();
                                else
                                    forum.Title = null;
                                if (!reader.IsDBNull(3))
                                    forum.Active = reader.GetValue(3).ToString();
                                else
                                    forum.Active = null;
                                if (!reader.IsDBNull(4))
                                    forum.Author = reader.GetValue(4).ToString();
                                else
                                    forum.Author = null;
                                if (!reader.IsDBNull(5))
                                    forum.Description = reader.GetValue(5).ToString();
                                else
                                    forum.Description = null;
                                if (!reader.IsDBNull(6))
                                    forum.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    forum.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    forum.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    forum.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    forum.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    forum.UpdatedAt = null;
                                response.Add(forum);
                                forum = new ForumModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ForumModel>();
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
                response = new List<ForumModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumModel> Read(string connectionString, List<ForumModel> forumWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumModel> response = new List<ForumModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_plan_id`, `title`, `active`, `author`, `description`, `modified_by`, `created_at`, `updated_at`  FROM forums ";
                if (forumWhere != null && forumWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < forumWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(forumWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, forumWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].TeacherPlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_plan_id` = @teacherPlanId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherPlanId" + i, forumWhere[i].TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Title))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `title` = @title" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@title" + i, forumWhere[i].Title);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Active))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `active` = @active" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@active" + i, forumWhere[i].Active);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Author))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `author` = @author" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@author" + i, forumWhere[i].Author);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, forumWhere[i].Description);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, forumWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, forumWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, forumWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != forumWhere.Count - 1)
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
                            ForumModel forum = new ForumModel(); if (!reader.IsDBNull(0))
                                forum.Id = reader.GetValue(0).ToString();
                            else
                                forum.Id = null;
                            if (!reader.IsDBNull(1))
                                forum.TeacherPlanId = reader.GetValue(1).ToString();
                            else
                                forum.TeacherPlanId = null;
                            if (!reader.IsDBNull(2))
                                forum.Title = reader.GetValue(2).ToString();
                            else
                                forum.Title = null;
                            if (!reader.IsDBNull(3))
                                forum.Active = reader.GetValue(3).ToString();
                            else
                                forum.Active = null;
                            if (!reader.IsDBNull(4))
                                forum.Author = reader.GetValue(4).ToString();
                            else
                                forum.Author = null;
                            if (!reader.IsDBNull(5))
                                forum.Description = reader.GetValue(5).ToString();
                            else
                                forum.Description = null;
                            if (!reader.IsDBNull(6))
                                forum.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                forum.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                forum.CreatedAt = reader.GetValue(7).ToString();
                            else
                                forum.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                forum.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                forum.UpdatedAt = null;
                            response.Add(forum);
                            forum = new ForumModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<ForumModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumModel> Read(string connectionString, List<ForumModel> forumWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumModel> response = new List<ForumModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `teacher_plan_id`, `title`, `active`, `author`, `description`, `modified_by`, `created_at`, `updated_at`  FROM forums ";
                if (forumWhere != null && forumWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < forumWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(forumWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, forumWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].TeacherPlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `teacher_plan_id` = @teacherPlanId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teacherPlanId" + i, forumWhere[i].TeacherPlanId);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Title))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `title` = @title" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@title" + i, forumWhere[i].Title);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Active))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `active` = @active" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@active" + i, forumWhere[i].Active);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Author))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `author` = @author" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@author" + i, forumWhere[i].Author);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, forumWhere[i].Description);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, forumWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, forumWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, forumWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != forumWhere.Count - 1)
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
                            ForumModel forum = new ForumModel(); if (!reader.IsDBNull(0))
                                forum.Id = reader.GetValue(0).ToString();
                            else
                                forum.Id = null;
                            if (!reader.IsDBNull(1))
                                forum.TeacherPlanId = reader.GetValue(1).ToString();
                            else
                                forum.TeacherPlanId = null;
                            if (!reader.IsDBNull(2))
                                forum.Title = reader.GetValue(2).ToString();
                            else
                                forum.Title = null;
                            if (!reader.IsDBNull(3))
                                forum.Active = reader.GetValue(3).ToString();
                            else
                                forum.Active = null;
                            if (!reader.IsDBNull(4))
                                forum.Author = reader.GetValue(4).ToString();
                            else
                                forum.Author = null;
                            if (!reader.IsDBNull(5))
                                forum.Description = reader.GetValue(5).ToString();
                            else
                                forum.Description = null;
                            if (!reader.IsDBNull(6))
                                forum.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                forum.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                forum.CreatedAt = reader.GetValue(7).ToString();
                            else
                                forum.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                forum.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                forum.UpdatedAt = null;
                            response.Add(forum);
                            forum = new ForumModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<ForumModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumModel> Create(string connectionString, ForumModel forum)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumModel> response = new List<ForumModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO forums (";
                string values = "(";
                if (!String.IsNullOrEmpty(forum.Id))
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
                    cmd.Parameters.AddWithValue("@id", forum.Id);
                }
                if (!String.IsNullOrEmpty(forum.TeacherPlanId))
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
                    cmd.Parameters.AddWithValue("@teacherPlanId", forum.TeacherPlanId);
                }
                if (!String.IsNullOrEmpty(forum.Title))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `title` ");
                        values = String.Concat(values, ", @title");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `title` ");
                        values = String.Concat(values, " @title");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@title", forum.Title);
                }
                if (!String.IsNullOrEmpty(forum.Active))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `active` ");
                        values = String.Concat(values, ", @active");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `active` ");
                        values = String.Concat(values, " @active");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@active", forum.Active);
                }
                if (!String.IsNullOrEmpty(forum.Author))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `author` ");
                        values = String.Concat(values, ", @author");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `author` ");
                        values = String.Concat(values, " @author");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@author", forum.Author);
                }
                if (!String.IsNullOrEmpty(forum.Description))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `description` ");
                        values = String.Concat(values, ", @description");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `description` ");
                        values = String.Concat(values, " @description");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@description", forum.Description);
                }
                if (!String.IsNullOrEmpty(forum.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", forum.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(forum.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", forum.CreatedAt);
                }
                if (!String.IsNullOrEmpty(forum.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", forum.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                forum.Id = cmd.LastInsertedId.ToString();
                response.Add(forum);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumModel>();
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
        
        public List<ForumModel> Create(string connectionString, List<ForumModel> forumList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumModel> response = new List<ForumModel>();
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
                        for (int i = 0; i < forumList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO forums (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(forumList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, forumList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].TeacherPlanId))
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
                                    cmd.Parameters.AddWithValue("@teacherPlanId" + i, forumList[i].TeacherPlanId);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].Title))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `title` ");
                                        values = String.Concat(values, ", @title" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `title` ");
                                        values = String.Concat(values, "@title" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@title" + i, forumList[i].Title);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].Active))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `active` ");
                                        values = String.Concat(values, ", @active" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `active` ");
                                        values = String.Concat(values, "@active" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@active" + i, forumList[i].Active);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].Author))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `author` ");
                                        values = String.Concat(values, ", @author" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `author` ");
                                        values = String.Concat(values, "@author" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@author" + i, forumList[i].Author);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].Description))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `description` ");
                                        values = String.Concat(values, ", @description" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `description` ");
                                        values = String.Concat(values, "@description" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@description" + i, forumList[i].Description);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, forumList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, forumList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, forumList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(forumList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<ForumModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ForumModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumModel>();
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

        public List<ForumModel> Update(string connectionString, ForumModel forum, ForumModel forumWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumModel> response = new List<ForumModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE forums SET ";
                if (!String.IsNullOrEmpty(forum.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", forum.Id);
                }
                if (!String.IsNullOrEmpty(forum.TeacherPlanId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `teacher_plan_id` = @teacherPlanId");
                    else
                        sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teacherPlanId", forum.TeacherPlanId);
                }
                if (!String.IsNullOrEmpty(forum.Title))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `title` = @title");
                    else
                        sql = String.Concat(sql, " `title` = @title");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@title", forum.Title);
                }
                if (!String.IsNullOrEmpty(forum.Active))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `active` = @active");
                    else
                        sql = String.Concat(sql, " `active` = @active");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@active", forum.Active);
                }
                if (!String.IsNullOrEmpty(forum.Author))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `author` = @author");
                    else
                        sql = String.Concat(sql, " `author` = @author");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@author", forum.Author);
                }
                if (!String.IsNullOrEmpty(forum.Description))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `description` = @description");
                    else
                        sql = String.Concat(sql, " `description` = @description");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@description", forum.Description);
                }
                if (!String.IsNullOrEmpty(forum.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", forum.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(forum.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", forum.CreatedAt);
                }
                if (!String.IsNullOrEmpty(forum.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", forum.UpdatedAt);
                }
                if (forumWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(forumWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", forumWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(forumWhere.TeacherPlanId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_plan_id` = @teacherPlanIdWhere");
                        else
                            sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherPlanIdWhere", forumWhere.TeacherPlanId);
                    }
                    if (!String.IsNullOrEmpty(forumWhere.Title))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `title` = @titleWhere");
                        else
                            sql = String.Concat(sql, " `title` = @titleWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@titleWhere", forumWhere.Title);
                    }
                    if (!String.IsNullOrEmpty(forumWhere.Active))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `active` = @activeWhere");
                        else
                            sql = String.Concat(sql, " `active` = @activeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@activeWhere", forumWhere.Active);
                    }
                    if (!String.IsNullOrEmpty(forumWhere.Author))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `author` = @authorWhere");
                        else
                            sql = String.Concat(sql, " `author` = @authorWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@authorWhere", forumWhere.Author);
                    }
                    if (!String.IsNullOrEmpty(forumWhere.Description))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `description` = @descriptionWhere");
                        else
                            sql = String.Concat(sql, " `description` = @descriptionWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descriptionWhere", forumWhere.Description);
                    }
                    if (!String.IsNullOrEmpty(forumWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", forumWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(forumWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", forumWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(forumWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", forumWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(forum);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ForumModel>();
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

        public List<ForumModel> Update(string connectionString, List<ForumModel> forumList, List<ForumModel> forumWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumModel> response = new List<ForumModel>();
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
                        for (int i = 0; i < forumList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE forums SET ";
                                if (!String.IsNullOrEmpty(forumList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, forumList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].TeacherPlanId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `teacher_plan_id` = @teacherPlanId" + i);
                                    else
                                        sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teacherPlanId" + i, forumList[i].TeacherPlanId);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].Title))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `title` = @title" + i);
                                    else
                                        sql = String.Concat(sql, " `title` = @title" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@title" + i, forumList[i].Title);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].Active))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `active` = @active" + i);
                                    else
                                        sql = String.Concat(sql, " `active` = @active" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@active" + i, forumList[i].Active);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].Author))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `author` = @author" + i);
                                    else
                                        sql = String.Concat(sql, " `author` = @author" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@author" + i, forumList[i].Author);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].Description))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `description` = @description" + i);
                                    else
                                        sql = String.Concat(sql, " `description` = @description" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@description" + i, forumList[i].Description);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, forumList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, forumList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(forumList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, forumList[i].UpdatedAt);
                                }
                                if (forumWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(forumWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, forumWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(forumWhereList[i].TeacherPlanId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_plan_id` = @teacherPlanIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherPlanIdWhere" + i, forumWhereList[i].TeacherPlanId);
                                    }
                                    if (!String.IsNullOrEmpty(forumWhereList[i].Title))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `title` = @titleWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `title` = @titleWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@titleWhere" + i, forumWhereList[i].Title);
                                    }
                                    if (!String.IsNullOrEmpty(forumWhereList[i].Active))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `active` = @activeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `active` = @activeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@activeWhere" + i, forumWhereList[i].Active);
                                    }
                                    if (!String.IsNullOrEmpty(forumWhereList[i].Author))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `author` = @authorWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `author` = @authorWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@authorWhere" + i, forumWhereList[i].Author);
                                    }
                                    if (!String.IsNullOrEmpty(forumWhereList[i].Description))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `description` = @descriptionWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `description` = @descriptionWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descriptionWhere" + i, forumWhereList[i].Description);
                                    }
                                    if (!String.IsNullOrEmpty(forumWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, forumWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(forumWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, forumWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(forumWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, forumWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(forumList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ForumModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ForumModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumModel>();
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


        public List<ForumModel> Delete(string connectionString, ForumModel forum)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumModel> response = new List<ForumModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM forums  ";
                if (forum != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(forum.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", forum.Id);
                    }
                    if (!String.IsNullOrEmpty(forum.TeacherPlanId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `teacher_plan_id` = @teacherPlanId");
                        else
                            sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teacherPlanId", forum.TeacherPlanId);
                    }
                    if (!String.IsNullOrEmpty(forum.Title))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `title` = @title");
                        else
                            sql = String.Concat(sql, " `title` = @title");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@title", forum.Title);
                    }
                    if (!String.IsNullOrEmpty(forum.Active))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `active` = @active");
                        else
                            sql = String.Concat(sql, " `active` = @active");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@active", forum.Active);
                    }
                    if (!String.IsNullOrEmpty(forum.Author))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `author` = @author");
                        else
                            sql = String.Concat(sql, " `author` = @author");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@author", forum.Author);
                    }
                    if (!String.IsNullOrEmpty(forum.Description))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `description` = @description");
                        else
                            sql = String.Concat(sql, " `description` = @description");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@description", forum.Description);
                    }
                    if (!String.IsNullOrEmpty(forum.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", forum.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(forum.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", forum.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(forum.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", forum.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(forum);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ForumModel>();
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

        public List<ForumModel> Delete(string connectionString, List<ForumModel> forum)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumModel> response = new List<ForumModel>();
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
                        for (int i = 0; i < forum.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM forums  ";
                                if (forum[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(forum[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, forum[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(forum[i].TeacherPlanId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `teacher_plan_id` = @teacherPlanId" + i);
                                        else
                                            sql = String.Concat(sql, " `teacher_plan_id` = @teacherPlanId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teacherPlanId" + i, forum[i].TeacherPlanId);
                                    }
                                    if (!String.IsNullOrEmpty(forum[i].Title))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `title` = @title" + i);
                                        else
                                            sql = String.Concat(sql, " `title` = @title" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@title" + i, forum[i].Title);
                                    }
                                    if (!String.IsNullOrEmpty(forum[i].Active))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `active` = @active" + i);
                                        else
                                            sql = String.Concat(sql, " `active` = @active" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@active" + i, forum[i].Active);
                                    }
                                    if (!String.IsNullOrEmpty(forum[i].Author))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `author` = @author" + i);
                                        else
                                            sql = String.Concat(sql, " `author` = @author" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@author" + i, forum[i].Author);
                                    }
                                    if (!String.IsNullOrEmpty(forum[i].Description))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `description` = @description" + i);
                                        else
                                            sql = String.Concat(sql, " `description` = @description" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@description" + i, forum[i].Description);
                                    }
                                    if (!String.IsNullOrEmpty(forum[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, forum[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(forum[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, forum[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(forum[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, forum[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(forum[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ForumModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ForumModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumModel>();
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
