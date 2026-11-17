using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Teachers;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    class ForumDateDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, ForumDatesModel> ReadD(string connectionString, ForumDatesModel forumDate, bool firstOne, bool orderByLastDate, ForumDatesModel forumDateWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ForumDatesModel> response = new Dictionary<string, ForumDatesModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `forum_id`, `implementation_date`, `modified_by`, `created_at`, `updated_at`  FROM forum_dates ";
                {
                    if (forumDate != null)
                    {
                        if (!String.IsNullOrEmpty(forumDate.Id))
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
                            cmd.Parameters.AddWithValue("@id", forumDate.Id);
                        }
                        if (!String.IsNullOrEmpty(forumDate.ForumId))
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
                            cmd.Parameters.AddWithValue("@forumId", forumDate.ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumDate.ImplementationDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `implementation_date` = @implementationDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `implementation_date` = @implementationDate ");
                            }
                            cmd.Parameters.AddWithValue("@implementationDate", forumDate.ImplementationDate);
                        }
                        if (!String.IsNullOrEmpty(forumDate.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forumDate.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumDate.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", forumDate.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumDate.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forumDate.UpdatedAt);
                        }
                    }
                    if (forumDateWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(forumDateWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + forumDateWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.ForumId))
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
                            cmd.Parameters.AddWithValue("@forumIdWhereLike", "%" + forumDateWhereLike.ForumId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.ImplementationDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `implementation_date` LIKE @implementationDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `implementation_date` LIKE @implementationDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@implementationDateWhereLike", "%" + forumDateWhereLike.ImplementationDate + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + forumDateWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + forumDateWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + forumDateWhereLike.UpdatedAt + "%");
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
                                forumDate = new ForumDatesModel();
                                if (!reader.IsDBNull(0))
                                    forumDate.Id = reader.GetValue(0).ToString();
                                else
                                    forumDate.Id = null;
                                if (!reader.IsDBNull(1))
                                    forumDate.ForumId = reader.GetValue(1).ToString();
                                else
                                    forumDate.ForumId = null;
                                if (!reader.IsDBNull(2))
                                    forumDate.ImplementationDate = reader.GetValue(2).ToString();
                                else
                                    forumDate.ImplementationDate = null;
                                if (!reader.IsDBNull(3))
                                    forumDate.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    forumDate.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    forumDate.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    forumDate.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    forumDate.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    forumDate.UpdatedAt = null;
                                response.Add(forumDate.Id, forumDate);
                                forumDate = new ForumDatesModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, ForumDatesModel>();
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
                response = new Dictionary<string, ForumDatesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ForumDatesModel> ReadD(string connectionString, ForumDatesModel forumDate, bool orderByLastDate, int fromRow, int limit, ForumDatesModel forumDateWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ForumDatesModel> response = new Dictionary<string, ForumDatesModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `forum_id`, `implementation_date`, `modified_by`, `created_at`, `updated_at`  FROM forum_dates ";
                {
                    if (forumDate != null)
                    {
                        if (!String.IsNullOrEmpty(forumDate.Id))
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
                            cmd.Parameters.AddWithValue("@id", forumDate.Id);
                        }
                        if (!String.IsNullOrEmpty(forumDate.ForumId))
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
                            cmd.Parameters.AddWithValue("@forumId", forumDate.ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumDate.ImplementationDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `implementation_date` = @implementationDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `implementation_date` = @implementationDate ");
                            }
                            cmd.Parameters.AddWithValue("@implementationDate", forumDate.ImplementationDate);
                        }
                        if (!String.IsNullOrEmpty(forumDate.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forumDate.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumDate.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", forumDate.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumDate.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forumDate.UpdatedAt);
                        }
                    }
                    if (forumDateWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(forumDateWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + forumDateWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.ForumId))
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
                            cmd.Parameters.AddWithValue("@forumIdWhereLike", "%" + forumDateWhereLike.ForumId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.ImplementationDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `implementation_date` LIKE @implementationDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `implementation_date` LIKE @implementationDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@implementationDateWhereLike", "%" + forumDateWhereLike.ImplementationDate + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + forumDateWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + forumDateWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + forumDateWhereLike.UpdatedAt + "%");
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
                                forumDate = new ForumDatesModel();
                                if (!reader.IsDBNull(0))
                                    forumDate.Id = reader.GetValue(0).ToString();
                                else
                                    forumDate.Id = null;
                                if (!reader.IsDBNull(1))
                                    forumDate.ForumId = reader.GetValue(1).ToString();
                                else
                                    forumDate.ForumId = null;
                                if (!reader.IsDBNull(2))
                                    forumDate.ImplementationDate = reader.GetValue(2).ToString();
                                else
                                    forumDate.ImplementationDate = null;
                                if (!reader.IsDBNull(3))
                                    forumDate.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    forumDate.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    forumDate.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    forumDate.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    forumDate.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    forumDate.UpdatedAt = null;
                                response.Add(forumDate.Id, forumDate);
                                forumDate = new ForumDatesModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, ForumDatesModel>();
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
                response = new Dictionary<string, ForumDatesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ForumDatesModel> ReadD(string connectionString, List<ForumDatesModel> forumDateWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ForumDatesModel> response = new Dictionary<string, ForumDatesModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `forum_id`, `implementation_date`, `modified_by`, `created_at`, `updated_at`  FROM forum_dates ";
                if (forumDateWhere != null && forumDateWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < forumDateWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(forumDateWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, forumDateWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].ForumId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `forum_id` = @forumId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forumId" + i, forumDateWhere[i].ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].ImplementationDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `implementation_date` = @implementationDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@implementationDate" + i, forumDateWhere[i].ImplementationDate);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, forumDateWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, forumDateWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, forumDateWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != forumDateWhere.Count - 1)
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
                            ForumDatesModel forumDate = new ForumDatesModel(); if (!reader.IsDBNull(0))
                                forumDate.Id = reader.GetValue(0).ToString();
                            else
                                forumDate.Id = null;
                            if (!reader.IsDBNull(1))
                                forumDate.ForumId = reader.GetValue(1).ToString();
                            else
                                forumDate.ForumId = null;
                            if (!reader.IsDBNull(2))
                                forumDate.ImplementationDate = reader.GetValue(2).ToString();
                            else
                                forumDate.ImplementationDate = null;
                            if (!reader.IsDBNull(3))
                                forumDate.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                forumDate.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                forumDate.CreatedAt = reader.GetValue(4).ToString();
                            else
                                forumDate.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                forumDate.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                forumDate.UpdatedAt = null;
                            response.Add(forumDate.Id, forumDate);
                            forumDate = new ForumDatesModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, ForumDatesModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, ForumDatesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, ForumDatesModel> ReadD(string connectionString, List<ForumDatesModel> forumDateWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, ForumDatesModel> response = new Dictionary<string, ForumDatesModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `forum_id`, `implementation_date`, `modified_by`, `created_at`, `updated_at`  FROM forum_dates ";
                if (forumDateWhere != null && forumDateWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < forumDateWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(forumDateWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, forumDateWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].ForumId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `forum_id` = @forumId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forumId" + i, forumDateWhere[i].ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].ImplementationDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `implementation_date` = @implementationDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@implementationDate" + i, forumDateWhere[i].ImplementationDate);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, forumDateWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, forumDateWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, forumDateWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != forumDateWhere.Count - 1)
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
                            ForumDatesModel forumDate = new ForumDatesModel(); if (!reader.IsDBNull(0))
                                forumDate.Id = reader.GetValue(0).ToString();
                            else
                                forumDate.Id = null;
                            if (!reader.IsDBNull(1))
                                forumDate.ForumId = reader.GetValue(1).ToString();
                            else
                                forumDate.ForumId = null;
                            if (!reader.IsDBNull(2))
                                forumDate.ImplementationDate = reader.GetValue(2).ToString();
                            else
                                forumDate.ImplementationDate = null;
                            if (!reader.IsDBNull(3))
                                forumDate.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                forumDate.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                forumDate.CreatedAt = reader.GetValue(4).ToString();
                            else
                                forumDate.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                forumDate.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                forumDate.UpdatedAt = null;
                            response.Add(forumDate.Id, forumDate);
                            forumDate = new ForumDatesModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, ForumDatesModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, ForumDatesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumDatesModel> Read(string connectionString, ForumDatesModel forumDate, bool firstOne, bool orderByLastDate, ForumDatesModel forumDateWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumDatesModel> response = new List<ForumDatesModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `forum_id`, `implementation_date`, `modified_by`, `created_at`, `updated_at`  FROM forum_dates ";
                {
                    if (forumDate != null)
                    {
                        if (!String.IsNullOrEmpty(forumDate.Id))
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
                            cmd.Parameters.AddWithValue("@id", forumDate.Id);
                        }
                        if (!String.IsNullOrEmpty(forumDate.ForumId))
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
                            cmd.Parameters.AddWithValue("@forumId", forumDate.ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumDate.ImplementationDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `implementation_date` = @implementationDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `implementation_date` = @implementationDate ");
                            }
                            cmd.Parameters.AddWithValue("@implementationDate", forumDate.ImplementationDate);
                        }
                        if (!String.IsNullOrEmpty(forumDate.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forumDate.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumDate.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", forumDate.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumDate.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forumDate.UpdatedAt);
                        }
                    }
                    if (forumDateWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(forumDateWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + forumDateWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.ForumId))
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
                            cmd.Parameters.AddWithValue("@forumIdWhereLike", "%" + forumDateWhereLike.ForumId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.ImplementationDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `implementation_date` LIKE @implementationDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `implementation_date` LIKE @implementationDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@implementationDateWhereLike", "%" + forumDateWhereLike.ImplementationDate + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + forumDateWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + forumDateWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + forumDateWhereLike.UpdatedAt + "%");
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
                                forumDate = new ForumDatesModel();
                                if (!reader.IsDBNull(0))
                                    forumDate.Id = reader.GetValue(0).ToString();
                                else
                                    forumDate.Id = null;
                                if (!reader.IsDBNull(1))
                                    forumDate.ForumId = reader.GetValue(1).ToString();
                                else
                                    forumDate.ForumId = null;
                                if (!reader.IsDBNull(2))
                                    forumDate.ImplementationDate = reader.GetValue(2).ToString();
                                else
                                    forumDate.ImplementationDate = null;
                                if (!reader.IsDBNull(3))
                                    forumDate.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    forumDate.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    forumDate.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    forumDate.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    forumDate.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    forumDate.UpdatedAt = null;
                                response.Add(forumDate);
                                forumDate = new ForumDatesModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ForumDatesModel>();
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
                response = new List<ForumDatesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumDatesModel> Read(string connectionString, ForumDatesModel forumDate, bool orderByLastDate, int fromRow, int limit, ForumDatesModel forumDateWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumDatesModel> response = new List<ForumDatesModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `forum_id`, `implementation_date`, `modified_by`, `created_at`, `updated_at`  FROM forum_dates ";
                {
                    if (forumDate != null)
                    {
                        if (!String.IsNullOrEmpty(forumDate.Id))
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
                            cmd.Parameters.AddWithValue("@id", forumDate.Id);
                        }
                        if (!String.IsNullOrEmpty(forumDate.ForumId))
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
                            cmd.Parameters.AddWithValue("@forumId", forumDate.ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumDate.ImplementationDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `implementation_date` = @implementationDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `implementation_date` = @implementationDate ");
                            }
                            cmd.Parameters.AddWithValue("@implementationDate", forumDate.ImplementationDate);
                        }
                        if (!String.IsNullOrEmpty(forumDate.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", forumDate.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumDate.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", forumDate.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumDate.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", forumDate.UpdatedAt);
                        }
                    }
                    if (forumDateWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(forumDateWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + forumDateWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.ForumId))
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
                            cmd.Parameters.AddWithValue("@forumIdWhereLike", "%" + forumDateWhereLike.ForumId + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.ImplementationDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `implementation_date` LIKE @implementationDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `implementation_date` LIKE @implementationDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@implementationDateWhereLike", "%" + forumDateWhereLike.ImplementationDate + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + forumDateWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + forumDateWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(forumDateWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + forumDateWhereLike.UpdatedAt + "%");
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
                                forumDate = new ForumDatesModel();
                                if (!reader.IsDBNull(0))
                                    forumDate.Id = reader.GetValue(0).ToString();
                                else
                                    forumDate.Id = null;
                                if (!reader.IsDBNull(1))
                                    forumDate.ForumId = reader.GetValue(1).ToString();
                                else
                                    forumDate.ForumId = null;
                                if (!reader.IsDBNull(2))
                                    forumDate.ImplementationDate = reader.GetValue(2).ToString();
                                else
                                    forumDate.ImplementationDate = null;
                                if (!reader.IsDBNull(3))
                                    forumDate.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    forumDate.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    forumDate.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    forumDate.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    forumDate.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    forumDate.UpdatedAt = null;
                                response.Add(forumDate);
                                forumDate = new ForumDatesModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ForumDatesModel>();
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
                response = new List<ForumDatesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumDatesModel> Read(string connectionString, List<ForumDatesModel> forumDateWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumDatesModel> response = new List<ForumDatesModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `forum_id`, `implementation_date`, `modified_by`, `created_at`, `updated_at`  FROM forum_dates ";
                if (forumDateWhere != null && forumDateWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < forumDateWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(forumDateWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, forumDateWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].ForumId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `forum_id` = @forumId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forumId" + i, forumDateWhere[i].ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].ImplementationDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `implementation_date` = @implementationDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@implementationDate" + i, forumDateWhere[i].ImplementationDate);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, forumDateWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, forumDateWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, forumDateWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != forumDateWhere.Count - 1)
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
                            ForumDatesModel forumDate = new ForumDatesModel(); if (!reader.IsDBNull(0))
                                forumDate.Id = reader.GetValue(0).ToString();
                            else
                                forumDate.Id = null;
                            if (!reader.IsDBNull(1))
                                forumDate.ForumId = reader.GetValue(1).ToString();
                            else
                                forumDate.ForumId = null;
                            if (!reader.IsDBNull(2))
                                forumDate.ImplementationDate = reader.GetValue(2).ToString();
                            else
                                forumDate.ImplementationDate = null;
                            if (!reader.IsDBNull(3))
                                forumDate.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                forumDate.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                forumDate.CreatedAt = reader.GetValue(4).ToString();
                            else
                                forumDate.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                forumDate.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                forumDate.UpdatedAt = null;
                            response.Add(forumDate);
                            forumDate = new ForumDatesModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<ForumDatesModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumDatesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumDatesModel> Read(string connectionString, List<ForumDatesModel> forumDateWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumDatesModel> response = new List<ForumDatesModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `forum_id`, `implementation_date`, `modified_by`, `created_at`, `updated_at`  FROM forum_dates ";
                if (forumDateWhere != null && forumDateWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < forumDateWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(forumDateWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, forumDateWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].ForumId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `forum_id` = @forumId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forumId" + i, forumDateWhere[i].ForumId);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].ImplementationDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `implementation_date` = @implementationDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@implementationDate" + i, forumDateWhere[i].ImplementationDate);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, forumDateWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, forumDateWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(forumDateWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, forumDateWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != forumDateWhere.Count - 1)
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
                            ForumDatesModel forumDate = new ForumDatesModel(); if (!reader.IsDBNull(0))
                                forumDate.Id = reader.GetValue(0).ToString();
                            else
                                forumDate.Id = null;
                            if (!reader.IsDBNull(1))
                                forumDate.ForumId = reader.GetValue(1).ToString();
                            else
                                forumDate.ForumId = null;
                            if (!reader.IsDBNull(2))
                                forumDate.ImplementationDate = reader.GetValue(2).ToString();
                            else
                                forumDate.ImplementationDate = null;
                            if (!reader.IsDBNull(3))
                                forumDate.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                forumDate.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                forumDate.CreatedAt = reader.GetValue(4).ToString();
                            else
                                forumDate.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                forumDate.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                forumDate.UpdatedAt = null;
                            response.Add(forumDate);
                            forumDate = new ForumDatesModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<ForumDatesModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumDatesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ForumDatesModel> Create(string connectionString, ForumDatesModel forumDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumDatesModel> response = new List<ForumDatesModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO forum_dates (";
                string values = "(";
                if (!String.IsNullOrEmpty(forumDate.Id))
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
                    cmd.Parameters.AddWithValue("@id", forumDate.Id);
                }
                if (!String.IsNullOrEmpty(forumDate.ForumId))
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
                    cmd.Parameters.AddWithValue("@forumId", forumDate.ForumId);
                }
                if (!String.IsNullOrEmpty(forumDate.ImplementationDate))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `implementation_date` ");
                        values = String.Concat(values, ", @implementationDate");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `implementation_date` ");
                        values = String.Concat(values, " @implementationDate");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@implementationDate", forumDate.ImplementationDate);
                }
                if (!String.IsNullOrEmpty(forumDate.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", forumDate.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(forumDate.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", forumDate.CreatedAt);
                }
                if (!String.IsNullOrEmpty(forumDate.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", forumDate.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                forumDate.Id = cmd.LastInsertedId.ToString();
                response.Add(forumDate);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumDatesModel>();
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
        public List<ForumDatesModel> Create(string connectionString, List<ForumDatesModel> forumDateList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumDatesModel> response = new List<ForumDatesModel>();
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
                        for (int i = 0; i < forumDateList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO forum_dates (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(forumDateList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, forumDateList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(forumDateList[i].ForumId))
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
                                    cmd.Parameters.AddWithValue("@forumId" + i, forumDateList[i].ForumId);
                                }
                                if (!String.IsNullOrEmpty(forumDateList[i].ImplementationDate))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `implementation_date` ");
                                        values = String.Concat(values, ", @implementationDate" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `implementation_date` ");
                                        values = String.Concat(values, "@implementationDate" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@implementationDate" + i, forumDateList[i].ImplementationDate);
                                }
                                if (!String.IsNullOrEmpty(forumDateList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, forumDateList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(forumDateList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, forumDateList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(forumDateList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, forumDateList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(forumDateList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<ForumDatesModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ForumDatesModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumDatesModel>();
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

        public List<ForumDatesModel> Update(string connectionString, ForumDatesModel forumDate, ForumDatesModel forumDateWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumDatesModel> response = new List<ForumDatesModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE forum_dates SET ";
                if (!String.IsNullOrEmpty(forumDate.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", forumDate.Id);
                }
                if (!String.IsNullOrEmpty(forumDate.ForumId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `forum_id` = @forumId");
                    else
                        sql = String.Concat(sql, " `forum_id` = @forumId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@forumId", forumDate.ForumId);
                }
                if (!String.IsNullOrEmpty(forumDate.ImplementationDate))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `implementation_date` = @implementationDate");
                    else
                        sql = String.Concat(sql, " `implementation_date` = @implementationDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@implementationDate", forumDate.ImplementationDate);
                }
                if (!String.IsNullOrEmpty(forumDate.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", forumDate.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(forumDate.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", forumDate.CreatedAt);
                }
                if (!String.IsNullOrEmpty(forumDate.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", forumDate.UpdatedAt);
                }
                if (forumDateWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(forumDateWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", forumDateWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(forumDateWhere.ForumId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `forum_id` = @forumIdWhere");
                        else
                            sql = String.Concat(sql, " `forum_id` = @forumIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@forumIdWhere", forumDateWhere.ForumId);
                    }
                    if (!String.IsNullOrEmpty(forumDateWhere.ImplementationDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `implementation_date` = @implementationDateWhere");
                        else
                            sql = String.Concat(sql, " `implementation_date` = @implementationDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@implementationDateWhere", forumDateWhere.ImplementationDate);
                    }
                    if (!String.IsNullOrEmpty(forumDateWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", forumDateWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(forumDateWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", forumDateWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(forumDateWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", forumDateWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(forumDate);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ForumDatesModel>();
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

        public List<ForumDatesModel> Update(string connectionString, List<ForumDatesModel> forumDateList, List<ForumDatesModel> forumDateWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumDatesModel> response = new List<ForumDatesModel>();
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
                        for (int i = 0; i < forumDateList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE forum_dates SET ";
                                if (!String.IsNullOrEmpty(forumDateList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, forumDateList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(forumDateList[i].ForumId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `forum_id` = @forumId" + i);
                                    else
                                        sql = String.Concat(sql, " `forum_id` = @forumId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@forumId" + i, forumDateList[i].ForumId);
                                }
                                if (!String.IsNullOrEmpty(forumDateList[i].ImplementationDate))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `implementation_date` = @implementationDate" + i);
                                    else
                                        sql = String.Concat(sql, " `implementation_date` = @implementationDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@implementationDate" + i, forumDateList[i].ImplementationDate);
                                }
                                if (!String.IsNullOrEmpty(forumDateList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, forumDateList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(forumDateList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, forumDateList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(forumDateList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, forumDateList[i].UpdatedAt);
                                }
                                if (forumDateWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(forumDateWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, forumDateWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(forumDateWhereList[i].ForumId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `forum_id` = @forumIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `forum_id` = @forumIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@forumIdWhere" + i, forumDateWhereList[i].ForumId);
                                    }
                                    if (!String.IsNullOrEmpty(forumDateWhereList[i].ImplementationDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `implementation_date` = @implementationDateWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `implementation_date` = @implementationDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@implementationDateWhere" + i, forumDateWhereList[i].ImplementationDate);
                                    }
                                    if (!String.IsNullOrEmpty(forumDateWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, forumDateWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(forumDateWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, forumDateWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(forumDateWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, forumDateWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(forumDateList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ForumDatesModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ForumDatesModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumDatesModel>();
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

        public List<ForumDatesModel> Delete(string connectionString, ForumDatesModel forumDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumDatesModel> response = new List<ForumDatesModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM forum_dates  ";
                if (forumDate != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(forumDate.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", forumDate.Id);
                    }
                    if (!String.IsNullOrEmpty(forumDate.ForumId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `forum_id` = @forumId");
                        else
                            sql = String.Concat(sql, " `forum_id` = @forumId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@forumId", forumDate.ForumId);
                    }
                    if (!String.IsNullOrEmpty(forumDate.ImplementationDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `implementation_date` = @implementationDate");
                        else
                            sql = String.Concat(sql, " `implementation_date` = @implementationDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@implementationDate", forumDate.ImplementationDate);
                    }
                    if (!String.IsNullOrEmpty(forumDate.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", forumDate.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(forumDate.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", forumDate.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(forumDate.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", forumDate.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(forumDate);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ForumDatesModel>();
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

        public List<ForumDatesModel> Delete(string connectionString, List<ForumDatesModel> forumDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ForumDatesModel> response = new List<ForumDatesModel>();
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
                        for (int i = 0; i < forumDate.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM forum_dates  ";
                                if (forumDate[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(forumDate[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, forumDate[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(forumDate[i].ForumId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `forum_id` = @forumId" + i);
                                        else
                                            sql = String.Concat(sql, " `forum_id` = @forumId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@forumId" + i, forumDate[i].ForumId);
                                    }
                                    if (!String.IsNullOrEmpty(forumDate[i].ImplementationDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `implementation_date` = @implementationDate" + i);
                                        else
                                            sql = String.Concat(sql, " `implementation_date` = @implementationDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@implementationDate" + i, forumDate[i].ImplementationDate);
                                    }
                                    if (!String.IsNullOrEmpty(forumDate[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, forumDate[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(forumDate[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, forumDate[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(forumDate[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, forumDate[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(forumDate[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ForumDatesModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ForumDatesModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ForumDatesModel>();
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
