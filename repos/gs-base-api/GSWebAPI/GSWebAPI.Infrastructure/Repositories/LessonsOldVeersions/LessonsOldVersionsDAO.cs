using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Domain.Models.LessonsOldVersions;
namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.LessonsOldVeersions
{
    class LessonsOldVersionDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, LessonsOldVersionModel> ReadD(string connectionString, LessonsOldVersionModel lessonsOldVersion, bool firstOne, bool orderByLastDate, LessonsOldVersionModel lessonsOldVersionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LessonsOldVersionModel> response = new Dictionary<string, LessonsOldVersionModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `level_code`, `subject_code`, `lesson_title`  FROM lessons_old_versions ";
                {
                    if (lessonsOldVersion != null)
                    {
                        if (!String.IsNullOrEmpty(lessonsOldVersion.Id))
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
                            cmd.Parameters.AddWithValue("@id", lessonsOldVersion.Id);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersion.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCode", lessonsOldVersion.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersion.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", lessonsOldVersion.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersion.LessonTitle))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_title` = @lessonTitle ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_title` = @lessonTitle ");
                            }
                            cmd.Parameters.AddWithValue("@lessonTitle", lessonsOldVersion.LessonTitle);
                        }
                    }
                    if (lessonsOldVersionWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + lessonsOldVersionWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + lessonsOldVersionWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + lessonsOldVersionWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.LessonTitle))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_title` LIKE @lessonTitleWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_title` LIKE @lessonTitleWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonTitleWhereLike", "%" + lessonsOldVersionWhereLike.LessonTitle + "%");
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
                                lessonsOldVersion = new LessonsOldVersionModel();
                                if (!reader.IsDBNull(0))
                                    lessonsOldVersion.Id = reader.GetValue(0).ToString();
                                else
                                    lessonsOldVersion.Id = null;
                                if (!reader.IsDBNull(1))
                                    lessonsOldVersion.LevelCode = reader.GetValue(1).ToString();
                                else
                                    lessonsOldVersion.LevelCode = null;
                                if (!reader.IsDBNull(2))
                                    lessonsOldVersion.SubjectCode = reader.GetValue(2).ToString();
                                else
                                    lessonsOldVersion.SubjectCode = null;
                                if (!reader.IsDBNull(3))
                                    lessonsOldVersion.LessonTitle = reader.GetValue(3).ToString();
                                else
                                    lessonsOldVersion.LessonTitle = null;
                                response.Add(lessonsOldVersion.Id, lessonsOldVersion);
                                lessonsOldVersion = new LessonsOldVersionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LessonsOldVersionModel>();
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
                response = new Dictionary<string, LessonsOldVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LessonsOldVersionModel> ReadD(string connectionString, LessonsOldVersionModel lessonsOldVersion, bool orderByLastDate, int fromRow, int limit, LessonsOldVersionModel lessonsOldVersionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LessonsOldVersionModel> response = new Dictionary<string, LessonsOldVersionModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `level_code`, `subject_code`, `lesson_title`  FROM lessons_old_versions ";
                {
                    if (lessonsOldVersion != null)
                    {
                        if (!String.IsNullOrEmpty(lessonsOldVersion.Id))
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
                            cmd.Parameters.AddWithValue("@id", lessonsOldVersion.Id);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersion.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCode", lessonsOldVersion.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersion.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", lessonsOldVersion.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersion.LessonTitle))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_title` = @lessonTitle ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_title` = @lessonTitle ");
                            }
                            cmd.Parameters.AddWithValue("@lessonTitle", lessonsOldVersion.LessonTitle);
                        }
                    }
                    if (lessonsOldVersionWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + lessonsOldVersionWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + lessonsOldVersionWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + lessonsOldVersionWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.LessonTitle))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_title` LIKE @lessonTitleWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_title` LIKE @lessonTitleWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonTitleWhereLike", "%" + lessonsOldVersionWhereLike.LessonTitle + "%");
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
                                lessonsOldVersion = new LessonsOldVersionModel();
                                if (!reader.IsDBNull(0))
                                    lessonsOldVersion.Id = reader.GetValue(0).ToString();
                                else
                                    lessonsOldVersion.Id = null;
                                if (!reader.IsDBNull(1))
                                    lessonsOldVersion.LevelCode = reader.GetValue(1).ToString();
                                else
                                    lessonsOldVersion.LevelCode = null;
                                if (!reader.IsDBNull(2))
                                    lessonsOldVersion.SubjectCode = reader.GetValue(2).ToString();
                                else
                                    lessonsOldVersion.SubjectCode = null;
                                if (!reader.IsDBNull(3))
                                    lessonsOldVersion.LessonTitle = reader.GetValue(3).ToString();
                                else
                                    lessonsOldVersion.LessonTitle = null;
                                response.Add(lessonsOldVersion.Id, lessonsOldVersion);
                                lessonsOldVersion = new LessonsOldVersionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, LessonsOldVersionModel>();
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
                response = new Dictionary<string, LessonsOldVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LessonsOldVersionModel> ReadD(string connectionString, List<LessonsOldVersionModel> lessonsOldVersionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LessonsOldVersionModel> response = new Dictionary<string, LessonsOldVersionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `level_code`, `subject_code`, `lesson_title`  FROM lessons_old_versions ";
                if (lessonsOldVersionWhere != null && lessonsOldVersionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < lessonsOldVersionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, lessonsOldVersionWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, lessonsOldVersionWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, lessonsOldVersionWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].LessonTitle))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_title` = @lessonTitle" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonTitle" + i, lessonsOldVersionWhere[i].LessonTitle);
                        }
                        sqlIn += ")";
                        if (i != lessonsOldVersionWhere.Count - 1)
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
                            LessonsOldVersionModel lessonsOldVersion = new LessonsOldVersionModel(); if (!reader.IsDBNull(0))
                                lessonsOldVersion.Id = reader.GetValue(0).ToString();
                            else
                                lessonsOldVersion.Id = null;
                            if (!reader.IsDBNull(1))
                                lessonsOldVersion.LevelCode = reader.GetValue(1).ToString();
                            else
                                lessonsOldVersion.LevelCode = null;
                            if (!reader.IsDBNull(2))
                                lessonsOldVersion.SubjectCode = reader.GetValue(2).ToString();
                            else
                                lessonsOldVersion.SubjectCode = null;
                            if (!reader.IsDBNull(3))
                                lessonsOldVersion.LessonTitle = reader.GetValue(3).ToString();
                            else
                                lessonsOldVersion.LessonTitle = null;
                            response.Add(lessonsOldVersion.Id, lessonsOldVersion);
                            lessonsOldVersion = new LessonsOldVersionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LessonsOldVersionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LessonsOldVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, LessonsOldVersionModel> ReadD(string connectionString, List<LessonsOldVersionModel> lessonsOldVersionWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, LessonsOldVersionModel> response = new Dictionary<string, LessonsOldVersionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `level_code`, `subject_code`, `lesson_title`  FROM lessons_old_versions ";
                if (lessonsOldVersionWhere != null && lessonsOldVersionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < lessonsOldVersionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, lessonsOldVersionWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, lessonsOldVersionWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, lessonsOldVersionWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].LessonTitle))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_title` = @lessonTitle" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonTitle" + i, lessonsOldVersionWhere[i].LessonTitle);
                        }
                        sqlIn += ")";
                        if (i != lessonsOldVersionWhere.Count - 1)
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
                            LessonsOldVersionModel lessonsOldVersion = new LessonsOldVersionModel(); if (!reader.IsDBNull(0))
                                lessonsOldVersion.Id = reader.GetValue(0).ToString();
                            else
                                lessonsOldVersion.Id = null;
                            if (!reader.IsDBNull(1))
                                lessonsOldVersion.LevelCode = reader.GetValue(1).ToString();
                            else
                                lessonsOldVersion.LevelCode = null;
                            if (!reader.IsDBNull(2))
                                lessonsOldVersion.SubjectCode = reader.GetValue(2).ToString();
                            else
                                lessonsOldVersion.SubjectCode = null;
                            if (!reader.IsDBNull(3))
                                lessonsOldVersion.LessonTitle = reader.GetValue(3).ToString();
                            else
                                lessonsOldVersion.LessonTitle = null;
                            response.Add(lessonsOldVersion.Id, lessonsOldVersion);
                            lessonsOldVersion = new LessonsOldVersionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, LessonsOldVersionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, LessonsOldVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LessonsOldVersionModel> Read(string connectionString, LessonsOldVersionModel lessonsOldVersion, bool firstOne, bool orderByLastDate, LessonsOldVersionModel lessonsOldVersionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LessonsOldVersionModel> response = new List<LessonsOldVersionModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `level_code`, `subject_code`, `lesson_title`  FROM lessons_old_versions ";
                {
                    if (lessonsOldVersion != null)
                    {
                        if (!String.IsNullOrEmpty(lessonsOldVersion.Id))
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
                            cmd.Parameters.AddWithValue("@id", lessonsOldVersion.Id);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersion.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCode", lessonsOldVersion.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersion.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", lessonsOldVersion.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersion.LessonTitle))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_title` = @lessonTitle ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_title` = @lessonTitle ");
                            }
                            cmd.Parameters.AddWithValue("@lessonTitle", lessonsOldVersion.LessonTitle);
                        }
                    }
                    if (lessonsOldVersionWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + lessonsOldVersionWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + lessonsOldVersionWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + lessonsOldVersionWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.LessonTitle))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_title` LIKE @lessonTitleWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_title` LIKE @lessonTitleWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonTitleWhereLike", "%" + lessonsOldVersionWhereLike.LessonTitle + "%");
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
                                lessonsOldVersion = new LessonsOldVersionModel();
                                if (!reader.IsDBNull(0))
                                    lessonsOldVersion.Id = reader.GetValue(0).ToString();
                                else
                                    lessonsOldVersion.Id = null;
                                if (!reader.IsDBNull(1))
                                    lessonsOldVersion.LevelCode = reader.GetValue(1).ToString();
                                else
                                    lessonsOldVersion.LevelCode = null;
                                if (!reader.IsDBNull(2))
                                    lessonsOldVersion.SubjectCode = reader.GetValue(2).ToString();
                                else
                                    lessonsOldVersion.SubjectCode = null;
                                if (!reader.IsDBNull(3))
                                    lessonsOldVersion.LessonTitle = reader.GetValue(3).ToString();
                                else
                                    lessonsOldVersion.LessonTitle = null;
                                response.Add(lessonsOldVersion);
                                lessonsOldVersion = new LessonsOldVersionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LessonsOldVersionModel>();
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
                response = new List<LessonsOldVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LessonsOldVersionModel> Read(string connectionString, LessonsOldVersionModel lessonsOldVersion, bool orderByLastDate, int fromRow, int limit, LessonsOldVersionModel lessonsOldVersionWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LessonsOldVersionModel> response = new List<LessonsOldVersionModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `level_code`, `subject_code`, `lesson_title`  FROM lessons_old_versions ";
                {
                    if (lessonsOldVersion != null)
                    {
                        if (!String.IsNullOrEmpty(lessonsOldVersion.Id))
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
                            cmd.Parameters.AddWithValue("@id", lessonsOldVersion.Id);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersion.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCode", lessonsOldVersion.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersion.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCode", lessonsOldVersion.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersion.LessonTitle))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_title` = @lessonTitle ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_title` = @lessonTitle ");
                            }
                            cmd.Parameters.AddWithValue("@lessonTitle", lessonsOldVersion.LessonTitle);
                        }
                    }
                    if (lessonsOldVersionWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + lessonsOldVersionWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.LevelCode))
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
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + lessonsOldVersionWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.SubjectCode))
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
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + lessonsOldVersionWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhereLike.LessonTitle))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_title` LIKE @lessonTitleWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_title` LIKE @lessonTitleWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonTitleWhereLike", "%" + lessonsOldVersionWhereLike.LessonTitle + "%");
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
                                lessonsOldVersion = new LessonsOldVersionModel();
                                if (!reader.IsDBNull(0))
                                    lessonsOldVersion.Id = reader.GetValue(0).ToString();
                                else
                                    lessonsOldVersion.Id = null;
                                if (!reader.IsDBNull(1))
                                    lessonsOldVersion.LevelCode = reader.GetValue(1).ToString();
                                else
                                    lessonsOldVersion.LevelCode = null;
                                if (!reader.IsDBNull(2))
                                    lessonsOldVersion.SubjectCode = reader.GetValue(2).ToString();
                                else
                                    lessonsOldVersion.SubjectCode = null;
                                if (!reader.IsDBNull(3))
                                    lessonsOldVersion.LessonTitle = reader.GetValue(3).ToString();
                                else
                                    lessonsOldVersion.LessonTitle = null;
                                response.Add(lessonsOldVersion);
                                lessonsOldVersion = new LessonsOldVersionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<LessonsOldVersionModel>();
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
                response = new List<LessonsOldVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LessonsOldVersionModel> Read(string connectionString, List<LessonsOldVersionModel> lessonsOldVersionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LessonsOldVersionModel> response = new List<LessonsOldVersionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `level_code`, `subject_code`, `lesson_title`  FROM lessons_old_versions ";
                if (lessonsOldVersionWhere != null && lessonsOldVersionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < lessonsOldVersionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, lessonsOldVersionWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, lessonsOldVersionWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, lessonsOldVersionWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].LessonTitle))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_title` = @lessonTitle" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonTitle" + i, lessonsOldVersionWhere[i].LessonTitle);
                        }
                        sqlIn += ")";
                        if (i != lessonsOldVersionWhere.Count - 1)
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
                            LessonsOldVersionModel lessonsOldVersion = new LessonsOldVersionModel(); if (!reader.IsDBNull(0))
                                lessonsOldVersion.Id = reader.GetValue(0).ToString();
                            else
                                lessonsOldVersion.Id = null;
                            if (!reader.IsDBNull(1))
                                lessonsOldVersion.LevelCode = reader.GetValue(1).ToString();
                            else
                                lessonsOldVersion.LevelCode = null;
                            if (!reader.IsDBNull(2))
                                lessonsOldVersion.SubjectCode = reader.GetValue(2).ToString();
                            else
                                lessonsOldVersion.SubjectCode = null;
                            if (!reader.IsDBNull(3))
                                lessonsOldVersion.LessonTitle = reader.GetValue(3).ToString();
                            else
                                lessonsOldVersion.LessonTitle = null;
                            response.Add(lessonsOldVersion);
                            lessonsOldVersion = new LessonsOldVersionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LessonsOldVersionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LessonsOldVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LessonsOldVersionModel> Read(string connectionString, List<LessonsOldVersionModel> lessonsOldVersionWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LessonsOldVersionModel> response = new List<LessonsOldVersionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `level_code`, `subject_code`, `lesson_title`  FROM lessons_old_versions ";
                if (lessonsOldVersionWhere != null && lessonsOldVersionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < lessonsOldVersionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, lessonsOldVersionWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, lessonsOldVersionWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, lessonsOldVersionWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(lessonsOldVersionWhere[i].LessonTitle))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_title` = @lessonTitle" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonTitle" + i, lessonsOldVersionWhere[i].LessonTitle);
                        }
                        sqlIn += ")";
                        if (i != lessonsOldVersionWhere.Count - 1)
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
                            LessonsOldVersionModel lessonsOldVersion = new LessonsOldVersionModel(); if (!reader.IsDBNull(0))
                                lessonsOldVersion.Id = reader.GetValue(0).ToString();
                            else
                                lessonsOldVersion.Id = null;
                            if (!reader.IsDBNull(1))
                                lessonsOldVersion.LevelCode = reader.GetValue(1).ToString();
                            else
                                lessonsOldVersion.LevelCode = null;
                            if (!reader.IsDBNull(2))
                                lessonsOldVersion.SubjectCode = reader.GetValue(2).ToString();
                            else
                                lessonsOldVersion.SubjectCode = null;
                            if (!reader.IsDBNull(3))
                                lessonsOldVersion.LessonTitle = reader.GetValue(3).ToString();
                            else
                                lessonsOldVersion.LessonTitle = null;
                            response.Add(lessonsOldVersion);
                            lessonsOldVersion = new LessonsOldVersionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<LessonsOldVersionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LessonsOldVersionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<LessonsOldVersionModel> Create(string connectionString, LessonsOldVersionModel lessonsOldVersion)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LessonsOldVersionModel> response = new List<LessonsOldVersionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO lessons_old_versions (";
                string values = "(";
                if (!String.IsNullOrEmpty(lessonsOldVersion.Id))
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
                    cmd.Parameters.AddWithValue("@id", lessonsOldVersion.Id);
                }
                if (!String.IsNullOrEmpty(lessonsOldVersion.LevelCode))
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
                    cmd.Parameters.AddWithValue("@levelCode", lessonsOldVersion.LevelCode);
                }
                if (!String.IsNullOrEmpty(lessonsOldVersion.SubjectCode))
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
                    cmd.Parameters.AddWithValue("@subjectCode", lessonsOldVersion.SubjectCode);
                }
                if (!String.IsNullOrEmpty(lessonsOldVersion.LessonTitle))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `lesson_title` ");
                        values = String.Concat(values, ", @lessonTitle");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `lesson_title` ");
                        values = String.Concat(values, " @lessonTitle");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@lessonTitle", lessonsOldVersion.LessonTitle);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                lessonsOldVersion.Id = cmd.LastInsertedId.ToString();
                response.Add(lessonsOldVersion);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LessonsOldVersionModel>();
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
        public List<LessonsOldVersionModel> Create(string connectionString, List<LessonsOldVersionModel> lessonsOldVersionList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LessonsOldVersionModel> response = new List<LessonsOldVersionModel>();
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
                        for (int i = 0; i < lessonsOldVersionList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO lessons_old_versions (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(lessonsOldVersionList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, lessonsOldVersionList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(lessonsOldVersionList[i].LevelCode))
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
                                    cmd.Parameters.AddWithValue("@levelCode" + i, lessonsOldVersionList[i].LevelCode);
                                }
                                if (!String.IsNullOrEmpty(lessonsOldVersionList[i].SubjectCode))
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
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, lessonsOldVersionList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(lessonsOldVersionList[i].LessonTitle))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `lesson_title` ");
                                        values = String.Concat(values, ", @lessonTitle" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `lesson_title` ");
                                        values = String.Concat(values, "@lessonTitle" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@lessonTitle" + i, lessonsOldVersionList[i].LessonTitle);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(lessonsOldVersionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<LessonsOldVersionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LessonsOldVersionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LessonsOldVersionModel>();
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

        public List<LessonsOldVersionModel> Update(string connectionString, LessonsOldVersionModel lessonsOldVersion, LessonsOldVersionModel lessonsOldVersionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LessonsOldVersionModel> response = new List<LessonsOldVersionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE lessons_old_versions SET ";
                if (!String.IsNullOrEmpty(lessonsOldVersion.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", lessonsOldVersion.Id);
                }
                if (!String.IsNullOrEmpty(lessonsOldVersion.LevelCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `level_code` = @levelCode");
                    else
                        sql = String.Concat(sql, " `level_code` = @levelCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@levelCode", lessonsOldVersion.LevelCode);
                }
                if (!String.IsNullOrEmpty(lessonsOldVersion.SubjectCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `subject_code` = @subjectCode");
                    else
                        sql = String.Concat(sql, " `subject_code` = @subjectCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectCode", lessonsOldVersion.SubjectCode);
                }
                if (!String.IsNullOrEmpty(lessonsOldVersion.LessonTitle))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `lesson_title` = @lessonTitle");
                    else
                        sql = String.Concat(sql, " `lesson_title` = @lessonTitle");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@lessonTitle", lessonsOldVersion.LessonTitle);
                }
                if (lessonsOldVersionWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(lessonsOldVersionWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", lessonsOldVersionWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(lessonsOldVersionWhere.LevelCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `level_code` = @levelCodeWhere");
                        else
                            sql = String.Concat(sql, " `level_code` = @levelCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelCodeWhere", lessonsOldVersionWhere.LevelCode);
                    }
                    if (!String.IsNullOrEmpty(lessonsOldVersionWhere.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCodeWhere", lessonsOldVersionWhere.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(lessonsOldVersionWhere.LessonTitle))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lesson_title` = @lessonTitleWhere");
                        else
                            sql = String.Concat(sql, " `lesson_title` = @lessonTitleWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonTitleWhere", lessonsOldVersionWhere.LessonTitle);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(lessonsOldVersion);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LessonsOldVersionModel>();
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

        public List<LessonsOldVersionModel> Update(string connectionString, List<LessonsOldVersionModel> lessonsOldVersionList, List<LessonsOldVersionModel> lessonsOldVersionWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LessonsOldVersionModel> response = new List<LessonsOldVersionModel>();
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
                        for (int i = 0; i < lessonsOldVersionList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE lessons_old_versions SET ";
                                if (!String.IsNullOrEmpty(lessonsOldVersionList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, lessonsOldVersionList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(lessonsOldVersionList[i].LevelCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `level_code` = @levelCode" + i);
                                    else
                                        sql = String.Concat(sql, " `level_code` = @levelCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@levelCode" + i, lessonsOldVersionList[i].LevelCode);
                                }
                                if (!String.IsNullOrEmpty(lessonsOldVersionList[i].SubjectCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `subject_code` = @subjectCode" + i);
                                    else
                                        sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, lessonsOldVersionList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(lessonsOldVersionList[i].LessonTitle))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `lesson_title` = @lessonTitle" + i);
                                    else
                                        sql = String.Concat(sql, " `lesson_title` = @lessonTitle" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@lessonTitle" + i, lessonsOldVersionList[i].LessonTitle);
                                }
                                if (lessonsOldVersionWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(lessonsOldVersionWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, lessonsOldVersionWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(lessonsOldVersionWhereList[i].LevelCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `level_code` = @levelCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `level_code` = @levelCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelCodeWhere" + i, lessonsOldVersionWhereList[i].LevelCode);
                                    }
                                    if (!String.IsNullOrEmpty(lessonsOldVersionWhereList[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCodeWhere" + i, lessonsOldVersionWhereList[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(lessonsOldVersionWhereList[i].LessonTitle))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lesson_title` = @lessonTitleWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `lesson_title` = @lessonTitleWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonTitleWhere" + i, lessonsOldVersionWhereList[i].LessonTitle);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(lessonsOldVersionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LessonsOldVersionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LessonsOldVersionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LessonsOldVersionModel>();
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

        public List<LessonsOldVersionModel> Delete(string connectionString, LessonsOldVersionModel lessonsOldVersion)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LessonsOldVersionModel> response = new List<LessonsOldVersionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM lessons_old_versions  ";
                if (lessonsOldVersion != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(lessonsOldVersion.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", lessonsOldVersion.Id);
                    }
                    if (!String.IsNullOrEmpty(lessonsOldVersion.LevelCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `level_code` = @levelCode");
                        else
                            sql = String.Concat(sql, " `level_code` = @levelCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelCode", lessonsOldVersion.LevelCode);
                    }
                    if (!String.IsNullOrEmpty(lessonsOldVersion.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCode", lessonsOldVersion.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(lessonsOldVersion.LessonTitle))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lesson_title` = @lessonTitle");
                        else
                            sql = String.Concat(sql, " `lesson_title` = @lessonTitle");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonTitle", lessonsOldVersion.LessonTitle);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(lessonsOldVersion);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<LessonsOldVersionModel>();
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

        public List<LessonsOldVersionModel> Delete(string connectionString, List<LessonsOldVersionModel> lessonsOldVersion)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<LessonsOldVersionModel> response = new List<LessonsOldVersionModel>();
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
                        for (int i = 0; i < lessonsOldVersion.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM lessons_old_versions  ";
                                if (lessonsOldVersion[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(lessonsOldVersion[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, lessonsOldVersion[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(lessonsOldVersion[i].LevelCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `level_code` = @levelCode" + i);
                                        else
                                            sql = String.Concat(sql, " `level_code` = @levelCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelCode" + i, lessonsOldVersion[i].LevelCode);
                                    }
                                    if (!String.IsNullOrEmpty(lessonsOldVersion[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCode" + i, lessonsOldVersion[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(lessonsOldVersion[i].LessonTitle))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lesson_title` = @lessonTitle" + i);
                                        else
                                            sql = String.Concat(sql, " `lesson_title` = @lessonTitle" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonTitle" + i, lessonsOldVersion[i].LessonTitle);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(lessonsOldVersion[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<LessonsOldVersionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<LessonsOldVersionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<LessonsOldVersionModel>();
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
