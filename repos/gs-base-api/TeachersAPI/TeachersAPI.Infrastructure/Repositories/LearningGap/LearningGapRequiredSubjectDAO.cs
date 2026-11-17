using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.LearningGap;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.LearningGap
{
    class LearningGapRequiredSubjectDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, LearningGapRequiredSubjectModel> ReadD(string connectionString, LearningGapRequiredSubjectModel learningGapRequiredSubject, bool firstOne, bool orderByLastDate, LearningGapRequiredSubjectModel learningGapRequiredSubjectWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapRequiredSubjectModel> response = new Dictionary<string, LearningGapRequiredSubjectModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subject_code`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_required_subjects ";
                {
                    if (learningGapRequiredSubject != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapRequiredSubject.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapRequiredSubject.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapRequiredSubject.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapRequiredSubject.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapRequiredSubject.UpdatedAt);
                        }
                    }
                    if (learningGapRequiredSubjectWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapRequiredSubjectWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapRequiredSubjectWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapRequiredSubjectWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapRequiredSubjectWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapRequiredSubjectWhereLike.UpdatedAt + "%");
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
                                learningGapRequiredSubject = new LearningGapRequiredSubjectModel();
                                if (!reader.IsDBNull(0))
                                    learningGapRequiredSubject.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapRequiredSubject.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapRequiredSubject.SubjectCode = reader.GetValue(1).ToString();
                                else
                                    learningGapRequiredSubject.SubjectCode = null;
                                if (!reader.IsDBNull(2))
                                    learningGapRequiredSubject.ModifiedBy = reader.GetValue(2).ToString();
                                else
                                    learningGapRequiredSubject.ModifiedBy = null;
                                if (!reader.IsDBNull(3))
                                    learningGapRequiredSubject.CreatedAt = reader.GetValue(3).ToString();
                                else
                                    learningGapRequiredSubject.CreatedAt = null;
                                if (!reader.IsDBNull(4))
                                    learningGapRequiredSubject.UpdatedAt = reader.GetValue(4).ToString();
                                else
                                    learningGapRequiredSubject.UpdatedAt = null;
                                response.Add(learningGapRequiredSubject.Id, learningGapRequiredSubject);
                                learningGapRequiredSubject = new LearningGapRequiredSubjectModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapRequiredSubjectModel>();
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
                response = new Dictionary<string, LearningGapRequiredSubjectModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapRequiredSubjectModel> ReadD(string connectionString, LearningGapRequiredSubjectModel learningGapRequiredSubject, bool orderByLastDate, int fromRow, int limit, LearningGapRequiredSubjectModel learningGapRequiredSubjectWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapRequiredSubjectModel> response = new Dictionary<string, LearningGapRequiredSubjectModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subject_code`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_required_subjects ";
                {
                    if (learningGapRequiredSubject != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapRequiredSubject.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapRequiredSubject.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapRequiredSubject.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapRequiredSubject.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapRequiredSubject.UpdatedAt);
                        }
                    }
                    if (learningGapRequiredSubjectWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapRequiredSubjectWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapRequiredSubjectWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapRequiredSubjectWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapRequiredSubjectWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapRequiredSubjectWhereLike.UpdatedAt + "%");
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
                                learningGapRequiredSubject = new LearningGapRequiredSubjectModel();
                                if (!reader.IsDBNull(0))
                                    learningGapRequiredSubject.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapRequiredSubject.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapRequiredSubject.SubjectCode = reader.GetValue(1).ToString();
                                else
                                    learningGapRequiredSubject.SubjectCode = null;
                                if (!reader.IsDBNull(2))
                                    learningGapRequiredSubject.ModifiedBy = reader.GetValue(2).ToString();
                                else
                                    learningGapRequiredSubject.ModifiedBy = null;
                                if (!reader.IsDBNull(3))
                                    learningGapRequiredSubject.CreatedAt = reader.GetValue(3).ToString();
                                else
                                    learningGapRequiredSubject.CreatedAt = null;
                                if (!reader.IsDBNull(4))
                                    learningGapRequiredSubject.UpdatedAt = reader.GetValue(4).ToString();
                                else
                                    learningGapRequiredSubject.UpdatedAt = null;
                                response.Add(learningGapRequiredSubject.Id, learningGapRequiredSubject);
                                learningGapRequiredSubject = new LearningGapRequiredSubjectModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LearningGapRequiredSubjectModel>();
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
                response = new Dictionary<string, LearningGapRequiredSubjectModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapRequiredSubjectModel> ReadD(string connectionString, List<LearningGapRequiredSubjectModel> learningGapRequiredSubjectWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapRequiredSubjectModel> response = new Dictionary<string, LearningGapRequiredSubjectModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subject_code`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_required_subjects ";
                if (learningGapRequiredSubjectWhere != null && learningGapRequiredSubjectWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapRequiredSubjectWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapRequiredSubjectWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapRequiredSubjectWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapRequiredSubjectWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapRequiredSubjectWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapRequiredSubjectWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapRequiredSubjectWhere.Count - 1)
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
                            LearningGapRequiredSubjectModel learningGapRequiredSubject = new LearningGapRequiredSubjectModel(); if (!reader.IsDBNull(0))
                                learningGapRequiredSubject.Id = reader.GetValue(0).ToString();
                            else
                                learningGapRequiredSubject.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapRequiredSubject.SubjectCode = reader.GetValue(1).ToString();
                            else
                                learningGapRequiredSubject.SubjectCode = null;
                            if (!reader.IsDBNull(2))
                                learningGapRequiredSubject.ModifiedBy = reader.GetValue(2).ToString();
                            else
                                learningGapRequiredSubject.ModifiedBy = null;
                            if (!reader.IsDBNull(3))
                                learningGapRequiredSubject.CreatedAt = reader.GetValue(3).ToString();
                            else
                                learningGapRequiredSubject.CreatedAt = null;
                            if (!reader.IsDBNull(4))
                                learningGapRequiredSubject.UpdatedAt = reader.GetValue(4).ToString();
                            else
                                learningGapRequiredSubject.UpdatedAt = null;
                            response.Add(learningGapRequiredSubject.Id, learningGapRequiredSubject);
                            learningGapRequiredSubject = new LearningGapRequiredSubjectModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapRequiredSubjectModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapRequiredSubjectModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LearningGapRequiredSubjectModel> ReadD(string connectionString, List<LearningGapRequiredSubjectModel> learningGapRequiredSubjectWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LearningGapRequiredSubjectModel> response = new Dictionary<string, LearningGapRequiredSubjectModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subject_code`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_required_subjects ";
                if (learningGapRequiredSubjectWhere != null && learningGapRequiredSubjectWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapRequiredSubjectWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapRequiredSubjectWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapRequiredSubjectWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapRequiredSubjectWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapRequiredSubjectWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapRequiredSubjectWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapRequiredSubjectWhere.Count - 1)
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
                            LearningGapRequiredSubjectModel learningGapRequiredSubject = new LearningGapRequiredSubjectModel(); if (!reader.IsDBNull(0))
                                learningGapRequiredSubject.Id = reader.GetValue(0).ToString();
                            else
                                learningGapRequiredSubject.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapRequiredSubject.SubjectCode = reader.GetValue(1).ToString();
                            else
                                learningGapRequiredSubject.SubjectCode = null;
                            if (!reader.IsDBNull(2))
                                learningGapRequiredSubject.ModifiedBy = reader.GetValue(2).ToString();
                            else
                                learningGapRequiredSubject.ModifiedBy = null;
                            if (!reader.IsDBNull(3))
                                learningGapRequiredSubject.CreatedAt = reader.GetValue(3).ToString();
                            else
                                learningGapRequiredSubject.CreatedAt = null;
                            if (!reader.IsDBNull(4))
                                learningGapRequiredSubject.UpdatedAt = reader.GetValue(4).ToString();
                            else
                                learningGapRequiredSubject.UpdatedAt = null;
                            response.Add(learningGapRequiredSubject.Id, learningGapRequiredSubject);
                            learningGapRequiredSubject = new LearningGapRequiredSubjectModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LearningGapRequiredSubjectModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LearningGapRequiredSubjectModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapRequiredSubjectModel> Read(string connectionString, LearningGapRequiredSubjectModel learningGapRequiredSubject, bool firstOne, bool orderByLastDate, LearningGapRequiredSubjectModel learningGapRequiredSubjectWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapRequiredSubjectModel> response = new List<LearningGapRequiredSubjectModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subject_code`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_required_subjects ";
                {
                    if (learningGapRequiredSubject != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapRequiredSubject.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapRequiredSubject.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapRequiredSubject.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapRequiredSubject.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapRequiredSubject.UpdatedAt);
                        }
                    }
                    if (learningGapRequiredSubjectWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapRequiredSubjectWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapRequiredSubjectWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapRequiredSubjectWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapRequiredSubjectWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapRequiredSubjectWhereLike.UpdatedAt + "%");
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
                                learningGapRequiredSubject = new LearningGapRequiredSubjectModel();
                                if (!reader.IsDBNull(0))
                                    learningGapRequiredSubject.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapRequiredSubject.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapRequiredSubject.SubjectCode = reader.GetValue(1).ToString();
                                else
                                    learningGapRequiredSubject.SubjectCode = null;
                                if (!reader.IsDBNull(2))
                                    learningGapRequiredSubject.ModifiedBy = reader.GetValue(2).ToString();
                                else
                                    learningGapRequiredSubject.ModifiedBy = null;
                                if (!reader.IsDBNull(3))
                                    learningGapRequiredSubject.CreatedAt = reader.GetValue(3).ToString();
                                else
                                    learningGapRequiredSubject.CreatedAt = null;
                                if (!reader.IsDBNull(4))
                                    learningGapRequiredSubject.UpdatedAt = reader.GetValue(4).ToString();
                                else
                                    learningGapRequiredSubject.UpdatedAt = null;
                                response.Add(learningGapRequiredSubject);
                                learningGapRequiredSubject = new LearningGapRequiredSubjectModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapRequiredSubjectModel>();
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
                response = new List<LearningGapRequiredSubjectModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapRequiredSubjectModel> Read(string connectionString, LearningGapRequiredSubjectModel learningGapRequiredSubject, bool orderByLastDate, int fromRow, int limit, LearningGapRequiredSubjectModel learningGapRequiredSubjectWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapRequiredSubjectModel> response = new List<LearningGapRequiredSubjectModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `subject_code`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_required_subjects ";
                {
                    if (learningGapRequiredSubject != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.Id))
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
                            cmd.Parameters.AddWithValue("@id", learningGapRequiredSubject.Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", learningGapRequiredSubject.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", learningGapRequiredSubject.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", learningGapRequiredSubject.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubject.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", learningGapRequiredSubject.UpdatedAt);
                        }
                    }
                    if (learningGapRequiredSubjectWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + learningGapRequiredSubjectWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + learningGapRequiredSubjectWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + learningGapRequiredSubjectWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + learningGapRequiredSubjectWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + learningGapRequiredSubjectWhereLike.UpdatedAt + "%");
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
                                learningGapRequiredSubject = new LearningGapRequiredSubjectModel();
                                if (!reader.IsDBNull(0))
                                    learningGapRequiredSubject.Id = reader.GetValue(0).ToString();
                                else
                                    learningGapRequiredSubject.Id = null;
                                if (!reader.IsDBNull(1))
                                    learningGapRequiredSubject.SubjectCode = reader.GetValue(1).ToString();
                                else
                                    learningGapRequiredSubject.SubjectCode = null;
                                if (!reader.IsDBNull(2))
                                    learningGapRequiredSubject.ModifiedBy = reader.GetValue(2).ToString();
                                else
                                    learningGapRequiredSubject.ModifiedBy = null;
                                if (!reader.IsDBNull(3))
                                    learningGapRequiredSubject.CreatedAt = reader.GetValue(3).ToString();
                                else
                                    learningGapRequiredSubject.CreatedAt = null;
                                if (!reader.IsDBNull(4))
                                    learningGapRequiredSubject.UpdatedAt = reader.GetValue(4).ToString();
                                else
                                    learningGapRequiredSubject.UpdatedAt = null;
                                response.Add(learningGapRequiredSubject);
                                learningGapRequiredSubject = new LearningGapRequiredSubjectModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LearningGapRequiredSubjectModel>();
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
                response = new List<LearningGapRequiredSubjectModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapRequiredSubjectModel> Read(string connectionString, List<LearningGapRequiredSubjectModel> learningGapRequiredSubjectWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapRequiredSubjectModel> response = new List<LearningGapRequiredSubjectModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subject_code`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_required_subjects ";
                if (learningGapRequiredSubjectWhere != null && learningGapRequiredSubjectWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapRequiredSubjectWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapRequiredSubjectWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapRequiredSubjectWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapRequiredSubjectWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapRequiredSubjectWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapRequiredSubjectWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapRequiredSubjectWhere.Count - 1)
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
                            LearningGapRequiredSubjectModel learningGapRequiredSubject = new LearningGapRequiredSubjectModel(); if (!reader.IsDBNull(0))
                                learningGapRequiredSubject.Id = reader.GetValue(0).ToString();
                            else
                                learningGapRequiredSubject.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapRequiredSubject.SubjectCode = reader.GetValue(1).ToString();
                            else
                                learningGapRequiredSubject.SubjectCode = null;
                            if (!reader.IsDBNull(2))
                                learningGapRequiredSubject.ModifiedBy = reader.GetValue(2).ToString();
                            else
                                learningGapRequiredSubject.ModifiedBy = null;
                            if (!reader.IsDBNull(3))
                                learningGapRequiredSubject.CreatedAt = reader.GetValue(3).ToString();
                            else
                                learningGapRequiredSubject.CreatedAt = null;
                            if (!reader.IsDBNull(4))
                                learningGapRequiredSubject.UpdatedAt = reader.GetValue(4).ToString();
                            else
                                learningGapRequiredSubject.UpdatedAt = null;
                            response.Add(learningGapRequiredSubject);
                            learningGapRequiredSubject = new LearningGapRequiredSubjectModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapRequiredSubjectModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapRequiredSubjectModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapRequiredSubjectModel> Read(string connectionString, List<LearningGapRequiredSubjectModel> learningGapRequiredSubjectWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapRequiredSubjectModel> response = new List<LearningGapRequiredSubjectModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `subject_code`, `modified_by`, `created_at`, `updated_at`  FROM learning_gap_required_subjects ";
                if (learningGapRequiredSubjectWhere != null && learningGapRequiredSubjectWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < learningGapRequiredSubjectWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, learningGapRequiredSubjectWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapRequiredSubjectWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapRequiredSubjectWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, learningGapRequiredSubjectWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapRequiredSubjectWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != learningGapRequiredSubjectWhere.Count - 1)
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
                            LearningGapRequiredSubjectModel learningGapRequiredSubject = new LearningGapRequiredSubjectModel(); if (!reader.IsDBNull(0))
                                learningGapRequiredSubject.Id = reader.GetValue(0).ToString();
                            else
                                learningGapRequiredSubject.Id = null;
                            if (!reader.IsDBNull(1))
                                learningGapRequiredSubject.SubjectCode = reader.GetValue(1).ToString();
                            else
                                learningGapRequiredSubject.SubjectCode = null;
                            if (!reader.IsDBNull(2))
                                learningGapRequiredSubject.ModifiedBy = reader.GetValue(2).ToString();
                            else
                                learningGapRequiredSubject.ModifiedBy = null;
                            if (!reader.IsDBNull(3))
                                learningGapRequiredSubject.CreatedAt = reader.GetValue(3).ToString();
                            else
                                learningGapRequiredSubject.CreatedAt = null;
                            if (!reader.IsDBNull(4))
                                learningGapRequiredSubject.UpdatedAt = reader.GetValue(4).ToString();
                            else
                                learningGapRequiredSubject.UpdatedAt = null;
                            response.Add(learningGapRequiredSubject);
                            learningGapRequiredSubject = new LearningGapRequiredSubjectModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LearningGapRequiredSubjectModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapRequiredSubjectModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LearningGapRequiredSubjectModel> Create(string connectionString, LearningGapRequiredSubjectModel learningGapRequiredSubject)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapRequiredSubjectModel> response = new List<LearningGapRequiredSubjectModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO learning_gap_required_subjects (";
                string values = "(";
                if (!String.IsNullOrEmpty(learningGapRequiredSubject.Id))
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
                    cmd.Parameters.AddWithValue("@id", learningGapRequiredSubject.Id);
                }
                if (!String.IsNullOrEmpty(learningGapRequiredSubject.SubjectCode))
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
                    cmd.Parameters.AddWithValue("@subjectCode", learningGapRequiredSubject.SubjectCode);
                }
                if (!String.IsNullOrEmpty(learningGapRequiredSubject.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapRequiredSubject.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapRequiredSubject.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", learningGapRequiredSubject.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapRequiredSubject.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapRequiredSubject.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                learningGapRequiredSubject.Id = cmd.LastInsertedId.ToString();
                response.Add(learningGapRequiredSubject);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapRequiredSubjectModel>();
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
        public List<LearningGapRequiredSubjectModel> Create(string connectionString, List<LearningGapRequiredSubjectModel> learningGapRequiredSubjectList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapRequiredSubjectModel> response = new List<LearningGapRequiredSubjectModel>();
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
                        for (int i = 0; i < learningGapRequiredSubjectList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO learning_gap_required_subjects (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(learningGapRequiredSubjectList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapRequiredSubjectList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapRequiredSubjectList[i].SubjectCode))
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
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapRequiredSubjectList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(learningGapRequiredSubjectList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapRequiredSubjectList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapRequiredSubjectList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapRequiredSubjectList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapRequiredSubjectList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapRequiredSubjectList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(learningGapRequiredSubjectList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapRequiredSubjectModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapRequiredSubjectModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapRequiredSubjectModel>();
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

        public List<LearningGapRequiredSubjectModel> Update(string connectionString, LearningGapRequiredSubjectModel learningGapRequiredSubject, LearningGapRequiredSubjectModel learningGapRequiredSubjectWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapRequiredSubjectModel> response = new List<LearningGapRequiredSubjectModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE learning_gap_required_subjects SET ";
                if (!String.IsNullOrEmpty(learningGapRequiredSubject.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", learningGapRequiredSubject.Id);
                }
                if (!String.IsNullOrEmpty(learningGapRequiredSubject.SubjectCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `subject_code` = @subjectCode");
                    else
                        sql = String.Concat(sql, " `subject_code` = @subjectCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectCode", learningGapRequiredSubject.SubjectCode);
                }
                if (!String.IsNullOrEmpty(learningGapRequiredSubject.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", learningGapRequiredSubject.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(learningGapRequiredSubject.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", learningGapRequiredSubject.CreatedAt);
                }
                if (!String.IsNullOrEmpty(learningGapRequiredSubject.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", learningGapRequiredSubject.UpdatedAt);
                }
                if (learningGapRequiredSubjectWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", learningGapRequiredSubjectWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCodeWhere", learningGapRequiredSubjectWhere.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", learningGapRequiredSubjectWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", learningGapRequiredSubjectWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", learningGapRequiredSubjectWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(learningGapRequiredSubject);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapRequiredSubjectModel>();
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

        public List<LearningGapRequiredSubjectModel> Update(string connectionString, List<LearningGapRequiredSubjectModel> learningGapRequiredSubjectList, List<LearningGapRequiredSubjectModel> learningGapRequiredSubjectWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapRequiredSubjectModel> response = new List<LearningGapRequiredSubjectModel>();
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
                        for (int i = 0; i < learningGapRequiredSubjectList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE learning_gap_required_subjects SET ";
                                if (!String.IsNullOrEmpty(learningGapRequiredSubjectList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, learningGapRequiredSubjectList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(learningGapRequiredSubjectList[i].SubjectCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `subject_code` = @subjectCode" + i);
                                    else
                                        sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapRequiredSubjectList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(learningGapRequiredSubjectList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapRequiredSubjectList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(learningGapRequiredSubjectList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, learningGapRequiredSubjectList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(learningGapRequiredSubjectList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapRequiredSubjectList[i].UpdatedAt);
                                }
                                if (learningGapRequiredSubjectWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, learningGapRequiredSubjectWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereList[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCodeWhere" + i, learningGapRequiredSubjectWhereList[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, learningGapRequiredSubjectWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, learningGapRequiredSubjectWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapRequiredSubjectWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, learningGapRequiredSubjectWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapRequiredSubjectList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapRequiredSubjectModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapRequiredSubjectModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapRequiredSubjectModel>();
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

        public List<LearningGapRequiredSubjectModel> Delete(string connectionString, LearningGapRequiredSubjectModel learningGapRequiredSubject)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapRequiredSubjectModel> response = new List<LearningGapRequiredSubjectModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM learning_gap_required_subjects  ";
                if (learningGapRequiredSubject != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(learningGapRequiredSubject.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", learningGapRequiredSubject.Id);
                    }
                    if (!String.IsNullOrEmpty(learningGapRequiredSubject.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCode", learningGapRequiredSubject.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(learningGapRequiredSubject.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", learningGapRequiredSubject.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(learningGapRequiredSubject.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", learningGapRequiredSubject.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(learningGapRequiredSubject.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", learningGapRequiredSubject.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(learningGapRequiredSubject);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapRequiredSubjectModel>();
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

        public List<LearningGapRequiredSubjectModel> Delete(string connectionString, List<LearningGapRequiredSubjectModel> learningGapRequiredSubject)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LearningGapRequiredSubjectModel> response = new List<LearningGapRequiredSubjectModel>();
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
                        for (int i = 0; i < learningGapRequiredSubject.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM learning_gap_required_subjects  ";
                                if (learningGapRequiredSubject[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(learningGapRequiredSubject[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, learningGapRequiredSubject[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapRequiredSubject[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCode" + i, learningGapRequiredSubject[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapRequiredSubject[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, learningGapRequiredSubject[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapRequiredSubject[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, learningGapRequiredSubject[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(learningGapRequiredSubject[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, learningGapRequiredSubject[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(learningGapRequiredSubject[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LearningGapRequiredSubjectModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LearningGapRequiredSubjectModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LearningGapRequiredSubjectModel>();
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
