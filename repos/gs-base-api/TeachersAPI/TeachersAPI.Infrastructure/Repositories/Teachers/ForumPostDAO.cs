using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Teachers;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    class ForumPostDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, ForumPostModel> ReadD(string connectionString, ForumPostModel forumPost, bool firstOne, bool orderByLastDate, ForumPostModel forumPostWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ForumPostModel> response = new Dictionary<string, ForumPostModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `forum_id`, `author`, `available`, `body`, `parent_post_id`, `modified_by`, `created_at`, `updated_at`  FROM forum_posts ";
                {
                    if (forumPost != null)
                    {
                        if (!String.IsNullOrEmpty(forumPost.Id))
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
                            cmd.Parameters.AddWithValue("@id", forumPost.Id);
                        }
                        if (!String.IsNullOrEmpty(forumPost.ForumId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `forum_id` = @forumId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `forum_id` = @forumId ");
                            }
                            cmd.Parameters.AddWithValue("@forumId", forumPost.ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumPost.Author))
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
                            cmd.Parameters.AddWithValue("@author", forumPost.Author);
                        }
                        if (!String.IsNullOrEmpty(forumPost.Available))
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
                            cmd.Parameters.AddWithValue("@available", forumPost.Available);
                        }
                        if (!String.IsNullOrEmpty(forumPost.Body))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `body` = @body ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `body` = @body ");
                            }
                            cmd.Parameters.AddWithValue("@body", forumPost.Body);
                        }
                        if (!String.IsNullOrEmpty(forumPost.ParentPostId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `parent_post_id` = @parentPostId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `parent_post_id` = @parentPostId ");
                            }
                            cmd.Parameters.AddWithValue("@parentPostId", forumPost.ParentPostId);
                        }
                        if (!String.IsNullOrEmpty(forumPost.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forumPost.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumPost.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", forumPost.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumPost.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forumPost.UpdatedAt);
                        }
                    }
                    if (forumPostWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + forumPostWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.ForumId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `forum_id` LIKE @forumIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `forum_id` LIKE @forumIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@forumIdWhereLike", "%" + forumPostWhereLike.ForumId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Author))
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
                            cmd.Parameters.AddWithValue("@authorWhereLike", "%" + forumPostWhereLike.Author + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + forumPostWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Body))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `body` LIKE @bodyWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `body` LIKE @bodyWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@bodyWhereLike", "%" + forumPostWhereLike.Body + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.ParentPostId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `parent_post_id` LIKE @parentPostIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `parent_post_id` LIKE @parentPostIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@parentPostIdWhereLike", "%" + forumPostWhereLike.ParentPostId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + forumPostWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + forumPostWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + forumPostWhereLike.UpdatedAt + "%");
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
                                forumPost = new ForumPostModel();
                                if (!reader.IsDBNull(0))
                                    forumPost.Id = reader.GetValue(0).ToString();
                                else
                                    forumPost.Id = null;
                                if (!reader.IsDBNull(1))
                                    forumPost.ForumId = reader.GetValue(1).ToString();
                                else
                                    forumPost.ForumId = null;
                                if (!reader.IsDBNull(2))
                                    forumPost.Author = reader.GetValue(2).ToString();
                                else
                                    forumPost.Author = null;
                                if (!reader.IsDBNull(3))
                                    forumPost.Available = reader.GetValue(3).ToString();
                                else
                                    forumPost.Available = null;
                                if (!reader.IsDBNull(4))
                                    forumPost.Body = reader.GetValue(4).ToString();
                                else
                                    forumPost.Body = null;
                                if (!reader.IsDBNull(5))
                                    forumPost.ParentPostId = reader.GetValue(5).ToString();
                                else
                                    forumPost.ParentPostId = null;
                                if (!reader.IsDBNull(6))
                                    forumPost.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    forumPost.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    forumPost.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    forumPost.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    forumPost.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    forumPost.UpdatedAt = null;
                                response.Add(forumPost.Id, forumPost);
                                forumPost = new ForumPostModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, ForumPostModel>();
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
                response = new Dictionary<string, ForumPostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ForumPostModel> ReadD(string connectionString, ForumPostModel forumPost, bool orderByLastDate, int fromRow, int limit, ForumPostModel forumPostWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ForumPostModel> response = new Dictionary<string, ForumPostModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `forum_id`, `author`, `available`, `body`, `parent_post_id`, `modified_by`, `created_at`, `updated_at`  FROM forum_posts ";
                {
                    if (forumPost != null)
                    {
                        if (!String.IsNullOrEmpty(forumPost.Id))
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
                            cmd.Parameters.AddWithValue("@id", forumPost.Id);
                        }
                        if (!String.IsNullOrEmpty(forumPost.ForumId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `forum_id` = @forumId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `forum_id` = @forumId ");
                            }
                            cmd.Parameters.AddWithValue("@forumId", forumPost.ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumPost.Author))
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
                            cmd.Parameters.AddWithValue("@author", forumPost.Author);
                        }
                        if (!String.IsNullOrEmpty(forumPost.Available))
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
                            cmd.Parameters.AddWithValue("@available", forumPost.Available);
                        }
                        if (!String.IsNullOrEmpty(forumPost.Body))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `body` = @body ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `body` = @body ");
                            }
                            cmd.Parameters.AddWithValue("@body", forumPost.Body);
                        }
                        if (!String.IsNullOrEmpty(forumPost.ParentPostId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `parent_post_id` = @parentPostId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `parent_post_id` = @parentPostId ");
                            }
                            cmd.Parameters.AddWithValue("@parentPostId", forumPost.ParentPostId);
                        }
                        if (!String.IsNullOrEmpty(forumPost.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forumPost.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumPost.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", forumPost.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumPost.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forumPost.UpdatedAt);
                        }
                    }
                    if (forumPostWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + forumPostWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.ForumId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `forum_id` LIKE @forumIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `forum_id` LIKE @forumIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@forumIdWhereLike", "%" + forumPostWhereLike.ForumId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Author))
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
                            cmd.Parameters.AddWithValue("@authorWhereLike", "%" + forumPostWhereLike.Author + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + forumPostWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Body))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `body` LIKE @bodyWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `body` LIKE @bodyWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@bodyWhereLike", "%" + forumPostWhereLike.Body + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.ParentPostId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `parent_post_id` LIKE @parentPostIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `parent_post_id` LIKE @parentPostIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@parentPostIdWhereLike", "%" + forumPostWhereLike.ParentPostId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + forumPostWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + forumPostWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + forumPostWhereLike.UpdatedAt + "%");
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
                                forumPost = new ForumPostModel();
                                if (!reader.IsDBNull(0))
                                    forumPost.Id = reader.GetValue(0).ToString();
                                else
                                    forumPost.Id = null;
                                if (!reader.IsDBNull(1))
                                    forumPost.ForumId = reader.GetValue(1).ToString();
                                else
                                    forumPost.ForumId = null;
                                if (!reader.IsDBNull(2))
                                    forumPost.Author = reader.GetValue(2).ToString();
                                else
                                    forumPost.Author = null;
                                if (!reader.IsDBNull(3))
                                    forumPost.Available = reader.GetValue(3).ToString();
                                else
                                    forumPost.Available = null;
                                if (!reader.IsDBNull(4))
                                    forumPost.Body = reader.GetValue(4).ToString();
                                else
                                    forumPost.Body = null;
                                if (!reader.IsDBNull(5))
                                    forumPost.ParentPostId = reader.GetValue(5).ToString();
                                else
                                    forumPost.ParentPostId = null;
                                if (!reader.IsDBNull(6))
                                    forumPost.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    forumPost.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    forumPost.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    forumPost.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    forumPost.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    forumPost.UpdatedAt = null;
                                response.Add(forumPost.Id, forumPost);
                                forumPost = new ForumPostModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, ForumPostModel>();
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
                response = new Dictionary<string, ForumPostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ForumPostModel> ReadD(string connectionString, List<ForumPostModel> forumPostWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ForumPostModel> response = new Dictionary<string, ForumPostModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `forum_id`, `author`, `available`, `body`, `parent_post_id`, `modified_by`, `created_at`, `updated_at`  FROM forum_posts ";
                if (forumPostWhere != null && forumPostWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < forumPostWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, forumPostWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].ForumId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `forum_id` = @forumId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forumId" + i, forumPostWhere[i].ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Author))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `author` = @author" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@author" + i, forumPostWhere[i].Author);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, forumPostWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Body))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `body` = @body" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@body" + i, forumPostWhere[i].Body);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].ParentPostId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `parent_post_id` = @parentPostId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@parentPostId" + i, forumPostWhere[i].ParentPostId);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, forumPostWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, forumPostWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, forumPostWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != forumPostWhere.Count - 1)
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
                            ForumPostModel forumPost = new ForumPostModel(); if (!reader.IsDBNull(0))
                                forumPost.Id = reader.GetValue(0).ToString();
                            else
                                forumPost.Id = null;
                            if (!reader.IsDBNull(1))
                                forumPost.ForumId = reader.GetValue(1).ToString();
                            else
                                forumPost.ForumId = null;
                            if (!reader.IsDBNull(2))
                                forumPost.Author = reader.GetValue(2).ToString();
                            else
                                forumPost.Author = null;
                            if (!reader.IsDBNull(3))
                                forumPost.Available = reader.GetValue(3).ToString();
                            else
                                forumPost.Available = null;
                            if (!reader.IsDBNull(4))
                                forumPost.Body = reader.GetValue(4).ToString();
                            else
                                forumPost.Body = null;
                            if (!reader.IsDBNull(5))
                                forumPost.ParentPostId = reader.GetValue(5).ToString();
                            else
                                forumPost.ParentPostId = null;
                            if (!reader.IsDBNull(6))
                                forumPost.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                forumPost.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                forumPost.CreatedAt = reader.GetValue(7).ToString();
                            else
                                forumPost.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                forumPost.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                forumPost.UpdatedAt = null;
                            response.Add(forumPost.Id, forumPost);
                            forumPost = new ForumPostModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, ForumPostModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, ForumPostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ForumPostModel> ReadD(string connectionString, List<ForumPostModel> forumPostWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ForumPostModel> response = new Dictionary<string, ForumPostModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `forum_id`, `author`, `available`, `body`, `parent_post_id`, `modified_by`, `created_at`, `updated_at`  FROM forum_posts ";
                if (forumPostWhere != null && forumPostWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < forumPostWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, forumPostWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].ForumId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `forum_id` = @forumId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forumId" + i, forumPostWhere[i].ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Author))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `author` = @author" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@author" + i, forumPostWhere[i].Author);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, forumPostWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Body))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `body` = @body" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@body" + i, forumPostWhere[i].Body);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].ParentPostId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `parent_post_id` = @parentPostId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@parentPostId" + i, forumPostWhere[i].ParentPostId);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, forumPostWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, forumPostWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, forumPostWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != forumPostWhere.Count - 1)
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
                            ForumPostModel forumPost = new ForumPostModel(); if (!reader.IsDBNull(0))
                                forumPost.Id = reader.GetValue(0).ToString();
                            else
                                forumPost.Id = null;
                            if (!reader.IsDBNull(1))
                                forumPost.ForumId = reader.GetValue(1).ToString();
                            else
                                forumPost.ForumId = null;
                            if (!reader.IsDBNull(2))
                                forumPost.Author = reader.GetValue(2).ToString();
                            else
                                forumPost.Author = null;
                            if (!reader.IsDBNull(3))
                                forumPost.Available = reader.GetValue(3).ToString();
                            else
                                forumPost.Available = null;
                            if (!reader.IsDBNull(4))
                                forumPost.Body = reader.GetValue(4).ToString();
                            else
                                forumPost.Body = null;
                            if (!reader.IsDBNull(5))
                                forumPost.ParentPostId = reader.GetValue(5).ToString();
                            else
                                forumPost.ParentPostId = null;
                            if (!reader.IsDBNull(6))
                                forumPost.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                forumPost.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                forumPost.CreatedAt = reader.GetValue(7).ToString();
                            else
                                forumPost.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                forumPost.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                forumPost.UpdatedAt = null;
                            response.Add(forumPost.Id, forumPost);
                            forumPost = new ForumPostModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, ForumPostModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, ForumPostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumPostModel> Read(string connectionString, ForumPostModel forumPost, bool firstOne, bool orderByLastDate, ForumPostModel forumPostWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumPostModel> response = new List<ForumPostModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `forum_id`, `author`, `available`, `body`, `parent_post_id`, `modified_by`, `created_at`, `updated_at`  FROM forum_posts ";
                {
                    if (forumPost != null)
                    {
                        if (!String.IsNullOrEmpty(forumPost.Id))
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
                            cmd.Parameters.AddWithValue("@id", forumPost.Id);
                        }
                        if (!String.IsNullOrEmpty(forumPost.ForumId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `forum_id` = @forumId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `forum_id` = @forumId ");
                            }
                            cmd.Parameters.AddWithValue("@forumId", forumPost.ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumPost.Author))
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
                            cmd.Parameters.AddWithValue("@author", forumPost.Author);
                        }
                        if (!String.IsNullOrEmpty(forumPost.Available))
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
                            cmd.Parameters.AddWithValue("@available", forumPost.Available);
                        }
                        if (!String.IsNullOrEmpty(forumPost.Body))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `body` = @body ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `body` = @body ");
                            }
                            cmd.Parameters.AddWithValue("@body", forumPost.Body);
                        }
                        if (!String.IsNullOrEmpty(forumPost.ParentPostId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `parent_post_id` = @parentPostId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `parent_post_id` = @parentPostId ");
                            }
                            cmd.Parameters.AddWithValue("@parentPostId", forumPost.ParentPostId);
                        }
                        if (!String.IsNullOrEmpty(forumPost.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forumPost.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumPost.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", forumPost.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumPost.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forumPost.UpdatedAt);
                        }
                    }
                    if (forumPostWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + forumPostWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.ForumId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `forum_id` LIKE @forumIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `forum_id` LIKE @forumIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@forumIdWhereLike", "%" + forumPostWhereLike.ForumId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Author))
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
                            cmd.Parameters.AddWithValue("@authorWhereLike", "%" + forumPostWhereLike.Author + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + forumPostWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Body))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `body` LIKE @bodyWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `body` LIKE @bodyWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@bodyWhereLike", "%" + forumPostWhereLike.Body + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.ParentPostId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `parent_post_id` LIKE @parentPostIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `parent_post_id` LIKE @parentPostIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@parentPostIdWhereLike", "%" + forumPostWhereLike.ParentPostId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + forumPostWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + forumPostWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + forumPostWhereLike.UpdatedAt + "%");
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
                                forumPost = new ForumPostModel();
                                if (!reader.IsDBNull(0))
                                    forumPost.Id = reader.GetValue(0).ToString();
                                else
                                    forumPost.Id = null;
                                if (!reader.IsDBNull(1))
                                    forumPost.ForumId = reader.GetValue(1).ToString();
                                else
                                    forumPost.ForumId = null;
                                if (!reader.IsDBNull(2))
                                    forumPost.Author = reader.GetValue(2).ToString();
                                else
                                    forumPost.Author = null;
                                if (!reader.IsDBNull(3))
                                    forumPost.Available = reader.GetValue(3).ToString();
                                else
                                    forumPost.Available = null;
                                if (!reader.IsDBNull(4))
                                    forumPost.Body = reader.GetValue(4).ToString();
                                else
                                    forumPost.Body = null;
                                if (!reader.IsDBNull(5))
                                    forumPost.ParentPostId = reader.GetValue(5).ToString();
                                else
                                    forumPost.ParentPostId = null;
                                if (!reader.IsDBNull(6))
                                    forumPost.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    forumPost.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    forumPost.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    forumPost.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    forumPost.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    forumPost.UpdatedAt = null;
                                response.Add(forumPost);
                                forumPost = new ForumPostModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ForumPostModel>();
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
                response = new List<ForumPostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumPostModel> Read(string connectionString, ForumPostModel forumPost, bool orderByLastDate, int fromRow, int limit, ForumPostModel forumPostWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumPostModel> response = new List<ForumPostModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `forum_id`, `author`, `available`, `body`, `parent_post_id`, `modified_by`, `created_at`, `updated_at`  FROM forum_posts ";
                {
                    if (forumPost != null)
                    {
                        if (!String.IsNullOrEmpty(forumPost.Id))
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
                            cmd.Parameters.AddWithValue("@id", forumPost.Id);
                        }
                        if (!String.IsNullOrEmpty(forumPost.ForumId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `forum_id` = @forumId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `forum_id` = @forumId ");
                            }
                            cmd.Parameters.AddWithValue("@forumId", forumPost.ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumPost.Author))
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
                            cmd.Parameters.AddWithValue("@author", forumPost.Author);
                        }
                        if (!String.IsNullOrEmpty(forumPost.Available))
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
                            cmd.Parameters.AddWithValue("@available", forumPost.Available);
                        }
                        if (!String.IsNullOrEmpty(forumPost.Body))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `body` = @body ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `body` = @body ");
                            }
                            cmd.Parameters.AddWithValue("@body", forumPost.Body);
                        }
                        if (!String.IsNullOrEmpty(forumPost.ParentPostId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `parent_post_id` = @parentPostId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `parent_post_id` = @parentPostId ");
                            }
                            cmd.Parameters.AddWithValue("@parentPostId", forumPost.ParentPostId);
                        }
                        if (!String.IsNullOrEmpty(forumPost.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forumPost.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumPost.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", forumPost.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumPost.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forumPost.UpdatedAt);
                        }
                    }
                    if (forumPostWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + forumPostWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.ForumId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `forum_id` LIKE @forumIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `forum_id` LIKE @forumIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@forumIdWhereLike", "%" + forumPostWhereLike.ForumId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Author))
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
                            cmd.Parameters.AddWithValue("@authorWhereLike", "%" + forumPostWhereLike.Author + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + forumPostWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.Body))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `body` LIKE @bodyWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `body` LIKE @bodyWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@bodyWhereLike", "%" + forumPostWhereLike.Body + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.ParentPostId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `parent_post_id` LIKE @parentPostIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `parent_post_id` LIKE @parentPostIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@parentPostIdWhereLike", "%" + forumPostWhereLike.ParentPostId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + forumPostWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + forumPostWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(forumPostWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + forumPostWhereLike.UpdatedAt + "%");
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
                                forumPost = new ForumPostModel();
                                if (!reader.IsDBNull(0))
                                    forumPost.Id = reader.GetValue(0).ToString();
                                else
                                    forumPost.Id = null;
                                if (!reader.IsDBNull(1))
                                    forumPost.ForumId = reader.GetValue(1).ToString();
                                else
                                    forumPost.ForumId = null;
                                if (!reader.IsDBNull(2))
                                    forumPost.Author = reader.GetValue(2).ToString();
                                else
                                    forumPost.Author = null;
                                if (!reader.IsDBNull(3))
                                    forumPost.Available = reader.GetValue(3).ToString();
                                else
                                    forumPost.Available = null;
                                if (!reader.IsDBNull(4))
                                    forumPost.Body = reader.GetValue(4).ToString();
                                else
                                    forumPost.Body = null;
                                if (!reader.IsDBNull(5))
                                    forumPost.ParentPostId = reader.GetValue(5).ToString();
                                else
                                    forumPost.ParentPostId = null;
                                if (!reader.IsDBNull(6))
                                    forumPost.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    forumPost.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    forumPost.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    forumPost.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    forumPost.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    forumPost.UpdatedAt = null;
                                response.Add(forumPost);
                                forumPost = new ForumPostModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ForumPostModel>();
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
                response = new List<ForumPostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumPostModel> Read(string connectionString, List<ForumPostModel> forumPostWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumPostModel> response = new List<ForumPostModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `forum_id`, `author`, `available`, `body`, `parent_post_id`, `modified_by`, `created_at`, `updated_at`  FROM forum_posts ";
                if (forumPostWhere != null && forumPostWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < forumPostWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, forumPostWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].ForumId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `forum_id` = @forumId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forumId" + i, forumPostWhere[i].ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Author))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `author` = @author" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@author" + i, forumPostWhere[i].Author);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, forumPostWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Body))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `body` = @body" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@body" + i, forumPostWhere[i].Body);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].ParentPostId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `parent_post_id` = @parentPostId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@parentPostId" + i, forumPostWhere[i].ParentPostId);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, forumPostWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, forumPostWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, forumPostWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != forumPostWhere.Count - 1)
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
                            ForumPostModel forumPost = new ForumPostModel(); if (!reader.IsDBNull(0))
                                forumPost.Id = reader.GetValue(0).ToString();
                            else
                                forumPost.Id = null;
                            if (!reader.IsDBNull(1))
                                forumPost.ForumId = reader.GetValue(1).ToString();
                            else
                                forumPost.ForumId = null;
                            if (!reader.IsDBNull(2))
                                forumPost.Author = reader.GetValue(2).ToString();
                            else
                                forumPost.Author = null;
                            if (!reader.IsDBNull(3))
                                forumPost.Available = reader.GetValue(3).ToString();
                            else
                                forumPost.Available = null;
                            if (!reader.IsDBNull(4))
                                forumPost.Body = reader.GetValue(4).ToString();
                            else
                                forumPost.Body = null;
                            if (!reader.IsDBNull(5))
                                forumPost.ParentPostId = reader.GetValue(5).ToString();
                            else
                                forumPost.ParentPostId = null;
                            if (!reader.IsDBNull(6))
                                forumPost.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                forumPost.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                forumPost.CreatedAt = reader.GetValue(7).ToString();
                            else
                                forumPost.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                forumPost.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                forumPost.UpdatedAt = null;
                            response.Add(forumPost);
                            forumPost = new ForumPostModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<ForumPostModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumPostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumPostModel> Read(string connectionString, List<ForumPostModel> forumPostWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumPostModel> response = new List<ForumPostModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `forum_id`, `author`, `available`, `body`, `parent_post_id`, `modified_by`, `created_at`, `updated_at`  FROM forum_posts ";
                if (forumPostWhere != null && forumPostWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < forumPostWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, forumPostWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].ForumId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `forum_id` = @forumId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forumId" + i, forumPostWhere[i].ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Author))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `author` = @author" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@author" + i, forumPostWhere[i].Author);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, forumPostWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].Body))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `body` = @body" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@body" + i, forumPostWhere[i].Body);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].ParentPostId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `parent_post_id` = @parentPostId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@parentPostId" + i, forumPostWhere[i].ParentPostId);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, forumPostWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, forumPostWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumPostWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, forumPostWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != forumPostWhere.Count - 1)
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
                            ForumPostModel forumPost = new ForumPostModel(); if (!reader.IsDBNull(0))
                                forumPost.Id = reader.GetValue(0).ToString();
                            else
                                forumPost.Id = null;
                            if (!reader.IsDBNull(1))
                                forumPost.ForumId = reader.GetValue(1).ToString();
                            else
                                forumPost.ForumId = null;
                            if (!reader.IsDBNull(2))
                                forumPost.Author = reader.GetValue(2).ToString();
                            else
                                forumPost.Author = null;
                            if (!reader.IsDBNull(3))
                                forumPost.Available = reader.GetValue(3).ToString();
                            else
                                forumPost.Available = null;
                            if (!reader.IsDBNull(4))
                                forumPost.Body = reader.GetValue(4).ToString();
                            else
                                forumPost.Body = null;
                            if (!reader.IsDBNull(5))
                                forumPost.ParentPostId = reader.GetValue(5).ToString();
                            else
                                forumPost.ParentPostId = null;
                            if (!reader.IsDBNull(6))
                                forumPost.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                forumPost.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                forumPost.CreatedAt = reader.GetValue(7).ToString();
                            else
                                forumPost.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                forumPost.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                forumPost.UpdatedAt = null;
                            response.Add(forumPost);
                            forumPost = new ForumPostModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<ForumPostModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumPostModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumPostModel> Create(string connectionString, ForumPostModel forumPost)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumPostModel> response = new List<ForumPostModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO forum_posts (";
                string values = "(";
                if (!String.IsNullOrEmpty(forumPost.Id))
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
                    cmd.Parameters.AddWithValue("@id", forumPost.Id);
                }
                if (!String.IsNullOrEmpty(forumPost.ForumId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `forum_id` ");
                        values = String.Concat(values, ", @forumId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `forum_id` ");
                        values = String.Concat(values, " @forumId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@forumId", forumPost.ForumId);
                }
                if (!String.IsNullOrEmpty(forumPost.Author))
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
                    cmd.Parameters.AddWithValue("@author", forumPost.Author);
                }
                if (!String.IsNullOrEmpty(forumPost.Available))
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
                    cmd.Parameters.AddWithValue("@available", forumPost.Available);
                }
                if (!String.IsNullOrEmpty(forumPost.Body))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `body` ");
                        values = String.Concat(values, ", @body");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `body` ");
                        values = String.Concat(values, " @body");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@body", forumPost.Body);
                }
                if (!String.IsNullOrEmpty(forumPost.ParentPostId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `parent_post_id` ");
                        values = String.Concat(values, ", @parentPostId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `parent_post_id` ");
                        values = String.Concat(values, " @parentPostId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@parentPostId", forumPost.ParentPostId);
                }
                if (!String.IsNullOrEmpty(forumPost.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", forumPost.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(forumPost.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", forumPost.CreatedAt);
                }
                if (!String.IsNullOrEmpty(forumPost.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", forumPost.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                forumPost.Id = cmd.LastInsertedId.ToString();
                response.Add(forumPost);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumPostModel>();
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
        public List<ForumPostModel> Create(string connectionString, List<ForumPostModel> forumPostList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumPostModel> response = new List<ForumPostModel>();
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
                        for (int i = 0; i < forumPostList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO forum_posts (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(forumPostList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, forumPostList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].ForumId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `forum_id` ");
                                        values = String.Concat(values, ", @forumId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `forum_id` ");
                                        values = String.Concat(values, "@forumId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@forumId" + i, forumPostList[i].ForumId);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].Author))
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
                                    cmd.Parameters.AddWithValue("@author" + i, forumPostList[i].Author);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].Available))
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
                                    cmd.Parameters.AddWithValue("@available" + i, forumPostList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].Body))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `body` ");
                                        values = String.Concat(values, ", @body" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `body` ");
                                        values = String.Concat(values, "@body" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@body" + i, forumPostList[i].Body);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].ParentPostId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `parent_post_id` ");
                                        values = String.Concat(values, ", @parentPostId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `parent_post_id` ");
                                        values = String.Concat(values, "@parentPostId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@parentPostId" + i, forumPostList[i].ParentPostId);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, forumPostList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, forumPostList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, forumPostList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(forumPostList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<ForumPostModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ForumPostModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumPostModel>();
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

        public List<ForumPostModel> Update(string connectionString, ForumPostModel forumPost, ForumPostModel forumPostWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumPostModel> response = new List<ForumPostModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE forum_posts SET ";
                if (!String.IsNullOrEmpty(forumPost.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", forumPost.Id);
                }
                if (!String.IsNullOrEmpty(forumPost.ForumId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `forum_id` = @forumId");
                    else
                        sql = String.Concat(sql, " `forum_id` = @forumId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@forumId", forumPost.ForumId);
                }
                if (!String.IsNullOrEmpty(forumPost.Author))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `author` = @author");
                    else
                        sql = String.Concat(sql, " `author` = @author");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@author", forumPost.Author);
                }
                if (!String.IsNullOrEmpty(forumPost.Available))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `available` = @available");
                    else
                        sql = String.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", forumPost.Available);
                }
                if (!String.IsNullOrEmpty(forumPost.Body))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `body` = @body");
                    else
                        sql = String.Concat(sql, " `body` = @body");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@body", forumPost.Body);
                }
                if (!String.IsNullOrEmpty(forumPost.ParentPostId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `parent_post_id` = @parentPostId");
                    else
                        sql = String.Concat(sql, " `parent_post_id` = @parentPostId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@parentPostId", forumPost.ParentPostId);
                }
                if (!String.IsNullOrEmpty(forumPost.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", forumPost.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(forumPost.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", forumPost.CreatedAt);
                }
                if (!String.IsNullOrEmpty(forumPost.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", forumPost.UpdatedAt);
                }
                if (forumPostWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(forumPostWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", forumPostWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(forumPostWhere.ForumId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `forum_id` = @forumIdWhere");
                        else
                            sql = String.Concat(sql, " `forum_id` = @forumIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@forumIdWhere", forumPostWhere.ForumId);
                    }
                    if (!String.IsNullOrEmpty(forumPostWhere.Author))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `author` = @authorWhere");
                        else
                            sql = String.Concat(sql, " `author` = @authorWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@authorWhere", forumPostWhere.Author);
                    }
                    if (!String.IsNullOrEmpty(forumPostWhere.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = String.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", forumPostWhere.Available);
                    }
                    if (!String.IsNullOrEmpty(forumPostWhere.Body))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `body` = @bodyWhere");
                        else
                            sql = String.Concat(sql, " `body` = @bodyWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@bodyWhere", forumPostWhere.Body);
                    }
                    if (!String.IsNullOrEmpty(forumPostWhere.ParentPostId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `parent_post_id` = @parentPostIdWhere");
                        else
                            sql = String.Concat(sql, " `parent_post_id` = @parentPostIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@parentPostIdWhere", forumPostWhere.ParentPostId);
                    }
                    if (!String.IsNullOrEmpty(forumPostWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", forumPostWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(forumPostWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", forumPostWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(forumPostWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", forumPostWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(forumPost);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ForumPostModel>();
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

        public List<ForumPostModel> Update(string connectionString, List<ForumPostModel> forumPostList, List<ForumPostModel> forumPostWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumPostModel> response = new List<ForumPostModel>();
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
                        for (int i = 0; i < forumPostList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE forum_posts SET ";
                                if (!String.IsNullOrEmpty(forumPostList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, forumPostList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].ForumId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `forum_id` = @forumId" + i);
                                    else
                                        sql = String.Concat(sql, " `forum_id` = @forumId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@forumId" + i, forumPostList[i].ForumId);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].Author))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `author` = @author" + i);
                                    else
                                        sql = String.Concat(sql, " `author` = @author" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@author" + i, forumPostList[i].Author);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].Available))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = String.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, forumPostList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].Body))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `body` = @body" + i);
                                    else
                                        sql = String.Concat(sql, " `body` = @body" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@body" + i, forumPostList[i].Body);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].ParentPostId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `parent_post_id` = @parentPostId" + i);
                                    else
                                        sql = String.Concat(sql, " `parent_post_id` = @parentPostId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@parentPostId" + i, forumPostList[i].ParentPostId);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, forumPostList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, forumPostList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(forumPostList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, forumPostList[i].UpdatedAt);
                                }
                                if (forumPostWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(forumPostWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, forumPostWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(forumPostWhereList[i].ForumId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `forum_id` = @forumIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `forum_id` = @forumIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@forumIdWhere" + i, forumPostWhereList[i].ForumId);
                                    }
                                    if (!String.IsNullOrEmpty(forumPostWhereList[i].Author))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `author` = @authorWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `author` = @authorWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@authorWhere" + i, forumPostWhereList[i].Author);
                                    }
                                    if (!String.IsNullOrEmpty(forumPostWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, forumPostWhereList[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(forumPostWhereList[i].Body))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `body` = @bodyWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `body` = @bodyWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@bodyWhere" + i, forumPostWhereList[i].Body);
                                    }
                                    if (!String.IsNullOrEmpty(forumPostWhereList[i].ParentPostId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `parent_post_id` = @parentPostIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `parent_post_id` = @parentPostIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@parentPostIdWhere" + i, forumPostWhereList[i].ParentPostId);
                                    }
                                    if (!String.IsNullOrEmpty(forumPostWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, forumPostWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(forumPostWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, forumPostWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(forumPostWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, forumPostWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(forumPostList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ForumPostModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ForumPostModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumPostModel>();
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

        public List<ForumPostModel> Delete(string connectionString, ForumPostModel forumPost)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumPostModel> response = new List<ForumPostModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM forum_posts  ";
                if (forumPost != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(forumPost.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", forumPost.Id);
                    }
                    if (!String.IsNullOrEmpty(forumPost.ForumId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `forum_id` = @forumId");
                        else
                            sql = String.Concat(sql, " `forum_id` = @forumId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@forumId", forumPost.ForumId);
                    }
                    if (!String.IsNullOrEmpty(forumPost.Author))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `author` = @author");
                        else
                            sql = String.Concat(sql, " `author` = @author");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@author", forumPost.Author);
                    }
                    if (!String.IsNullOrEmpty(forumPost.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @available");
                        else
                            sql = String.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", forumPost.Available);
                    }
                    if (!String.IsNullOrEmpty(forumPost.Body))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `body` = @body");
                        else
                            sql = String.Concat(sql, " `body` = @body");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@body", forumPost.Body);
                    }
                    if (!String.IsNullOrEmpty(forumPost.ParentPostId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `parent_post_id` = @parentPostId");
                        else
                            sql = String.Concat(sql, " `parent_post_id` = @parentPostId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@parentPostId", forumPost.ParentPostId);
                    }
                    if (!String.IsNullOrEmpty(forumPost.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", forumPost.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(forumPost.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", forumPost.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(forumPost.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", forumPost.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(forumPost);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ForumPostModel>();
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

        public List<ForumPostModel> Delete(string connectionString, List<ForumPostModel> forumPost)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumPostModel> response = new List<ForumPostModel>();
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
                        for (int i = 0; i < forumPost.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM forum_posts  ";
                                if (forumPost[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(forumPost[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, forumPost[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(forumPost[i].ForumId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `forum_id` = @forumId" + i);
                                        else
                                            sql = String.Concat(sql, " `forum_id` = @forumId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@forumId" + i, forumPost[i].ForumId);
                                    }
                                    if (!String.IsNullOrEmpty(forumPost[i].Author))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `author` = @author" + i);
                                        else
                                            sql = String.Concat(sql, " `author` = @author" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@author" + i, forumPost[i].Author);
                                    }
                                    if (!String.IsNullOrEmpty(forumPost[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, forumPost[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(forumPost[i].Body))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `body` = @body" + i);
                                        else
                                            sql = String.Concat(sql, " `body` = @body" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@body" + i, forumPost[i].Body);
                                    }
                                    if (!String.IsNullOrEmpty(forumPost[i].ParentPostId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `parent_post_id` = @parentPostId" + i);
                                        else
                                            sql = String.Concat(sql, " `parent_post_id` = @parentPostId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@parentPostId" + i, forumPost[i].ParentPostId);
                                    }
                                    if (!String.IsNullOrEmpty(forumPost[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, forumPost[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(forumPost[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, forumPost[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(forumPost[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, forumPost[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(forumPost[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ForumPostModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ForumPostModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumPostModel>();
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
