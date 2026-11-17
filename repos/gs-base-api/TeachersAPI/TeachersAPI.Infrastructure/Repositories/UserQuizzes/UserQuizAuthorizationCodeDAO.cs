using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.UserQuizzes;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.UserQuizzes
{
    class UserQuizAuthorizationCodeDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, UserQuizAuthorizationCodeModel> ReadD(string connectionString, UserQuizAuthorizationCodeModel userQuizAuthorizationCode, bool firstOne, bool orderByLastDate, UserQuizAuthorizationCodeModel userQuizAuthorizationCodeWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserQuizAuthorizationCodeModel> response = new Dictionary<string, UserQuizAuthorizationCodeModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `authorization_token`, `custom_quiz_id`, `modified_by`, `created_at`, `updated_at`  FROM user_quiz_authorization_codes ";
                {
                    if (userQuizAuthorizationCode != null)
                    {
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.Id))
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
                            cmd.Parameters.AddWithValue("@id", userQuizAuthorizationCode.Id);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userQuizAuthorizationCode.UserId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.AuthorizationToken))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `authorization_token` = @authorizationToken ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `authorization_token` = @authorizationToken ");
                            }
                            cmd.Parameters.AddWithValue("@authorizationToken", userQuizAuthorizationCode.AuthorizationToken);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.CustomQuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `custom_quiz_id` = @customQuizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `custom_quiz_id` = @customQuizId ");
                            }
                            cmd.Parameters.AddWithValue("@customQuizId", userQuizAuthorizationCode.CustomQuizId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userQuizAuthorizationCode.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userQuizAuthorizationCode.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userQuizAuthorizationCode.UpdatedAt);
                        }
                    }
                    if (userQuizAuthorizationCodeWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userQuizAuthorizationCodeWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userQuizAuthorizationCodeWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.AuthorizationToken))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `authorization_token` LIKE @authorizationTokenWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `authorization_token` LIKE @authorizationTokenWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@authorizationTokenWhereLike", "%" + userQuizAuthorizationCodeWhereLike.AuthorizationToken + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.CustomQuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `custom_quiz_id` LIKE @customQuizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `custom_quiz_id` LIKE @customQuizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customQuizIdWhereLike", "%" + userQuizAuthorizationCodeWhereLike.CustomQuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userQuizAuthorizationCodeWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userQuizAuthorizationCodeWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userQuizAuthorizationCodeWhereLike.UpdatedAt + "%");
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
                                userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel();
                                if (!reader.IsDBNull(0))
                                    userQuizAuthorizationCode.Id = reader.GetValue(0).ToString();
                                else
                                    userQuizAuthorizationCode.Id = null;
                                if (!reader.IsDBNull(1))
                                    userQuizAuthorizationCode.UserId = reader.GetValue(1).ToString();
                                else
                                    userQuizAuthorizationCode.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userQuizAuthorizationCode.AuthorizationToken = reader.GetValue(2).ToString();
                                else
                                    userQuizAuthorizationCode.AuthorizationToken = null;
                                if (!reader.IsDBNull(3))
                                    userQuizAuthorizationCode.CustomQuizId = reader.GetValue(3).ToString();
                                else
                                    userQuizAuthorizationCode.CustomQuizId = null;
                                if (!reader.IsDBNull(4))
                                    userQuizAuthorizationCode.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    userQuizAuthorizationCode.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    userQuizAuthorizationCode.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    userQuizAuthorizationCode.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    userQuizAuthorizationCode.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    userQuizAuthorizationCode.UpdatedAt = null;
                                response.Add(userQuizAuthorizationCode.Id, userQuizAuthorizationCode);
                                userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserQuizAuthorizationCodeModel>();
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
                response = new Dictionary<string, UserQuizAuthorizationCodeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserQuizAuthorizationCodeModel> ReadD(string connectionString, UserQuizAuthorizationCodeModel userQuizAuthorizationCode, bool orderByLastDate, int fromRow, int limit, UserQuizAuthorizationCodeModel userQuizAuthorizationCodeWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserQuizAuthorizationCodeModel> response = new Dictionary<string, UserQuizAuthorizationCodeModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `authorization_token`, `custom_quiz_id`, `modified_by`, `created_at`, `updated_at`  FROM user_quiz_authorization_codes ";
                {
                    if (userQuizAuthorizationCode != null)
                    {
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.Id))
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
                            cmd.Parameters.AddWithValue("@id", userQuizAuthorizationCode.Id);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userQuizAuthorizationCode.UserId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.AuthorizationToken))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `authorization_token` = @authorizationToken ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `authorization_token` = @authorizationToken ");
                            }
                            cmd.Parameters.AddWithValue("@authorizationToken", userQuizAuthorizationCode.AuthorizationToken);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.CustomQuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `custom_quiz_id` = @customQuizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `custom_quiz_id` = @customQuizId ");
                            }
                            cmd.Parameters.AddWithValue("@customQuizId", userQuizAuthorizationCode.CustomQuizId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userQuizAuthorizationCode.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userQuizAuthorizationCode.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userQuizAuthorizationCode.UpdatedAt);
                        }
                    }
                    if (userQuizAuthorizationCodeWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userQuizAuthorizationCodeWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userQuizAuthorizationCodeWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.AuthorizationToken))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `authorization_token` LIKE @authorizationTokenWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `authorization_token` LIKE @authorizationTokenWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@authorizationTokenWhereLike", "%" + userQuizAuthorizationCodeWhereLike.AuthorizationToken + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.CustomQuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `custom_quiz_id` LIKE @customQuizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `custom_quiz_id` LIKE @customQuizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customQuizIdWhereLike", "%" + userQuizAuthorizationCodeWhereLike.CustomQuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userQuizAuthorizationCodeWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userQuizAuthorizationCodeWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userQuizAuthorizationCodeWhereLike.UpdatedAt + "%");
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
                                userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel();
                                if (!reader.IsDBNull(0))
                                    userQuizAuthorizationCode.Id = reader.GetValue(0).ToString();
                                else
                                    userQuizAuthorizationCode.Id = null;
                                if (!reader.IsDBNull(1))
                                    userQuizAuthorizationCode.UserId = reader.GetValue(1).ToString();
                                else
                                    userQuizAuthorizationCode.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userQuizAuthorizationCode.AuthorizationToken = reader.GetValue(2).ToString();
                                else
                                    userQuizAuthorizationCode.AuthorizationToken = null;
                                if (!reader.IsDBNull(3))
                                    userQuizAuthorizationCode.CustomQuizId = reader.GetValue(3).ToString();
                                else
                                    userQuizAuthorizationCode.CustomQuizId = null;
                                if (!reader.IsDBNull(4))
                                    userQuizAuthorizationCode.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    userQuizAuthorizationCode.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    userQuizAuthorizationCode.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    userQuizAuthorizationCode.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    userQuizAuthorizationCode.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    userQuizAuthorizationCode.UpdatedAt = null;
                                response.Add(userQuizAuthorizationCode.Id, userQuizAuthorizationCode);
                                userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserQuizAuthorizationCodeModel>();
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
                response = new Dictionary<string, UserQuizAuthorizationCodeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserQuizAuthorizationCodeModel> ReadD(string connectionString, List<UserQuizAuthorizationCodeModel> userQuizAuthorizationCodeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserQuizAuthorizationCodeModel> response = new Dictionary<string, UserQuizAuthorizationCodeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `authorization_token`, `custom_quiz_id`, `modified_by`, `created_at`, `updated_at`  FROM user_quiz_authorization_codes ";
                if (userQuizAuthorizationCodeWhere != null && userQuizAuthorizationCodeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userQuizAuthorizationCodeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userQuizAuthorizationCodeWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userQuizAuthorizationCodeWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].AuthorizationToken))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `authorization_token` = @authorizationToken" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@authorizationToken" + i, userQuizAuthorizationCodeWhere[i].AuthorizationToken);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].CustomQuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `custom_quiz_id` = @customQuizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customQuizId" + i, userQuizAuthorizationCodeWhere[i].CustomQuizId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizAuthorizationCodeWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userQuizAuthorizationCodeWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizAuthorizationCodeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userQuizAuthorizationCodeWhere.Count - 1)
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
                            UserQuizAuthorizationCodeModel userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel(); if (!reader.IsDBNull(0))
                                userQuizAuthorizationCode.Id = reader.GetValue(0).ToString();
                            else
                                userQuizAuthorizationCode.Id = null;
                            if (!reader.IsDBNull(1))
                                userQuizAuthorizationCode.UserId = reader.GetValue(1).ToString();
                            else
                                userQuizAuthorizationCode.UserId = null;
                            if (!reader.IsDBNull(2))
                                userQuizAuthorizationCode.AuthorizationToken = reader.GetValue(2).ToString();
                            else
                                userQuizAuthorizationCode.AuthorizationToken = null;
                            if (!reader.IsDBNull(3))
                                userQuizAuthorizationCode.CustomQuizId = reader.GetValue(3).ToString();
                            else
                                userQuizAuthorizationCode.CustomQuizId = null;
                            if (!reader.IsDBNull(4))
                                userQuizAuthorizationCode.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                userQuizAuthorizationCode.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                userQuizAuthorizationCode.CreatedAt = reader.GetValue(5).ToString();
                            else
                                userQuizAuthorizationCode.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                userQuizAuthorizationCode.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                userQuizAuthorizationCode.UpdatedAt = null;
                            response.Add(userQuizAuthorizationCode.Id, userQuizAuthorizationCode);
                            userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserQuizAuthorizationCodeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserQuizAuthorizationCodeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserQuizAuthorizationCodeModel> ReadD(string connectionString, List<UserQuizAuthorizationCodeModel> userQuizAuthorizationCodeWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserQuizAuthorizationCodeModel> response = new Dictionary<string, UserQuizAuthorizationCodeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `authorization_token`, `custom_quiz_id`, `modified_by`, `created_at`, `updated_at`  FROM user_quiz_authorization_codes ";
                if (userQuizAuthorizationCodeWhere != null && userQuizAuthorizationCodeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userQuizAuthorizationCodeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userQuizAuthorizationCodeWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userQuizAuthorizationCodeWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].AuthorizationToken))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `authorization_token` = @authorizationToken" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@authorizationToken" + i, userQuizAuthorizationCodeWhere[i].AuthorizationToken);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].CustomQuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `custom_quiz_id` = @customQuizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customQuizId" + i, userQuizAuthorizationCodeWhere[i].CustomQuizId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizAuthorizationCodeWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userQuizAuthorizationCodeWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizAuthorizationCodeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userQuizAuthorizationCodeWhere.Count - 1)
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
                            UserQuizAuthorizationCodeModel userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel(); if (!reader.IsDBNull(0))
                                userQuizAuthorizationCode.Id = reader.GetValue(0).ToString();
                            else
                                userQuizAuthorizationCode.Id = null;
                            if (!reader.IsDBNull(1))
                                userQuizAuthorizationCode.UserId = reader.GetValue(1).ToString();
                            else
                                userQuizAuthorizationCode.UserId = null;
                            if (!reader.IsDBNull(2))
                                userQuizAuthorizationCode.AuthorizationToken = reader.GetValue(2).ToString();
                            else
                                userQuizAuthorizationCode.AuthorizationToken = null;
                            if (!reader.IsDBNull(3))
                                userQuizAuthorizationCode.CustomQuizId = reader.GetValue(3).ToString();
                            else
                                userQuizAuthorizationCode.CustomQuizId = null;
                            if (!reader.IsDBNull(4))
                                userQuizAuthorizationCode.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                userQuizAuthorizationCode.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                userQuizAuthorizationCode.CreatedAt = reader.GetValue(5).ToString();
                            else
                                userQuizAuthorizationCode.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                userQuizAuthorizationCode.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                userQuizAuthorizationCode.UpdatedAt = null;
                            response.Add(userQuizAuthorizationCode.Id, userQuizAuthorizationCode);
                            userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserQuizAuthorizationCodeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserQuizAuthorizationCodeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserQuizAuthorizationCodeModel> Read(string connectionString, UserQuizAuthorizationCodeModel userQuizAuthorizationCode, bool firstOne, bool orderByLastDate, UserQuizAuthorizationCodeModel userQuizAuthorizationCodeWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizAuthorizationCodeModel> response = new List<UserQuizAuthorizationCodeModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `authorization_token`, `custom_quiz_id`, `modified_by`, `created_at`, `updated_at`  FROM user_quiz_authorization_codes ";
                {
                    if (userQuizAuthorizationCode != null)
                    {
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.Id))
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
                            cmd.Parameters.AddWithValue("@id", userQuizAuthorizationCode.Id);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userQuizAuthorizationCode.UserId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.AuthorizationToken))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `authorization_token` = @authorizationToken ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `authorization_token` = @authorizationToken ");
                            }
                            cmd.Parameters.AddWithValue("@authorizationToken", userQuizAuthorizationCode.AuthorizationToken);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.CustomQuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `custom_quiz_id` = @customQuizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `custom_quiz_id` = @customQuizId ");
                            }
                            cmd.Parameters.AddWithValue("@customQuizId", userQuizAuthorizationCode.CustomQuizId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userQuizAuthorizationCode.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userQuizAuthorizationCode.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userQuizAuthorizationCode.UpdatedAt);
                        }
                    }
                    if (userQuizAuthorizationCodeWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userQuizAuthorizationCodeWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userQuizAuthorizationCodeWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.AuthorizationToken))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `authorization_token` LIKE @authorizationTokenWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `authorization_token` LIKE @authorizationTokenWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@authorizationTokenWhereLike", "%" + userQuizAuthorizationCodeWhereLike.AuthorizationToken + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.CustomQuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `custom_quiz_id` LIKE @customQuizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `custom_quiz_id` LIKE @customQuizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customQuizIdWhereLike", "%" + userQuizAuthorizationCodeWhereLike.CustomQuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userQuizAuthorizationCodeWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userQuizAuthorizationCodeWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userQuizAuthorizationCodeWhereLike.UpdatedAt + "%");
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
                                userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel();
                                if (!reader.IsDBNull(0))
                                    userQuizAuthorizationCode.Id = reader.GetValue(0).ToString();
                                else
                                    userQuizAuthorizationCode.Id = null;
                                if (!reader.IsDBNull(1))
                                    userQuizAuthorizationCode.UserId = reader.GetValue(1).ToString();
                                else
                                    userQuizAuthorizationCode.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userQuizAuthorizationCode.AuthorizationToken = reader.GetValue(2).ToString();
                                else
                                    userQuizAuthorizationCode.AuthorizationToken = null;
                                if (!reader.IsDBNull(3))
                                    userQuizAuthorizationCode.CustomQuizId = reader.GetValue(3).ToString();
                                else
                                    userQuizAuthorizationCode.CustomQuizId = null;
                                if (!reader.IsDBNull(4))
                                    userQuizAuthorizationCode.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    userQuizAuthorizationCode.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    userQuizAuthorizationCode.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    userQuizAuthorizationCode.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    userQuizAuthorizationCode.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    userQuizAuthorizationCode.UpdatedAt = null;
                                response.Add(userQuizAuthorizationCode);
                                userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserQuizAuthorizationCodeModel>();
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
                response = new List<UserQuizAuthorizationCodeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserQuizAuthorizationCodeModel> Read(string connectionString, UserQuizAuthorizationCodeModel userQuizAuthorizationCode, bool orderByLastDate, int fromRow, int limit, UserQuizAuthorizationCodeModel userQuizAuthorizationCodeWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizAuthorizationCodeModel> response = new List<UserQuizAuthorizationCodeModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `authorization_token`, `custom_quiz_id`, `modified_by`, `created_at`, `updated_at`  FROM user_quiz_authorization_codes ";
                {
                    if (userQuizAuthorizationCode != null)
                    {
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.Id))
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
                            cmd.Parameters.AddWithValue("@id", userQuizAuthorizationCode.Id);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userQuizAuthorizationCode.UserId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.AuthorizationToken))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `authorization_token` = @authorizationToken ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `authorization_token` = @authorizationToken ");
                            }
                            cmd.Parameters.AddWithValue("@authorizationToken", userQuizAuthorizationCode.AuthorizationToken);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.CustomQuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `custom_quiz_id` = @customQuizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `custom_quiz_id` = @customQuizId ");
                            }
                            cmd.Parameters.AddWithValue("@customQuizId", userQuizAuthorizationCode.CustomQuizId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userQuizAuthorizationCode.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userQuizAuthorizationCode.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCode.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userQuizAuthorizationCode.UpdatedAt);
                        }
                    }
                    if (userQuizAuthorizationCodeWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userQuizAuthorizationCodeWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userQuizAuthorizationCodeWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.AuthorizationToken))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `authorization_token` LIKE @authorizationTokenWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `authorization_token` LIKE @authorizationTokenWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@authorizationTokenWhereLike", "%" + userQuizAuthorizationCodeWhereLike.AuthorizationToken + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.CustomQuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `custom_quiz_id` LIKE @customQuizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `custom_quiz_id` LIKE @customQuizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customQuizIdWhereLike", "%" + userQuizAuthorizationCodeWhereLike.CustomQuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userQuizAuthorizationCodeWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userQuizAuthorizationCodeWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userQuizAuthorizationCodeWhereLike.UpdatedAt + "%");
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
                                userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel();
                                if (!reader.IsDBNull(0))
                                    userQuizAuthorizationCode.Id = reader.GetValue(0).ToString();
                                else
                                    userQuizAuthorizationCode.Id = null;
                                if (!reader.IsDBNull(1))
                                    userQuizAuthorizationCode.UserId = reader.GetValue(1).ToString();
                                else
                                    userQuizAuthorizationCode.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userQuizAuthorizationCode.AuthorizationToken = reader.GetValue(2).ToString();
                                else
                                    userQuizAuthorizationCode.AuthorizationToken = null;
                                if (!reader.IsDBNull(3))
                                    userQuizAuthorizationCode.CustomQuizId = reader.GetValue(3).ToString();
                                else
                                    userQuizAuthorizationCode.CustomQuizId = null;
                                if (!reader.IsDBNull(4))
                                    userQuizAuthorizationCode.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    userQuizAuthorizationCode.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    userQuizAuthorizationCode.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    userQuizAuthorizationCode.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    userQuizAuthorizationCode.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    userQuizAuthorizationCode.UpdatedAt = null;
                                response.Add(userQuizAuthorizationCode);
                                userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserQuizAuthorizationCodeModel>();
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
                response = new List<UserQuizAuthorizationCodeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserQuizAuthorizationCodeModel> Read(string connectionString, List<UserQuizAuthorizationCodeModel> userQuizAuthorizationCodeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizAuthorizationCodeModel> response = new List<UserQuizAuthorizationCodeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `authorization_token`, `custom_quiz_id`, `modified_by`, `created_at`, `updated_at`  FROM user_quiz_authorization_codes ";
                if (userQuizAuthorizationCodeWhere != null && userQuizAuthorizationCodeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userQuizAuthorizationCodeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userQuizAuthorizationCodeWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userQuizAuthorizationCodeWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].AuthorizationToken))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `authorization_token` = @authorizationToken" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@authorizationToken" + i, userQuizAuthorizationCodeWhere[i].AuthorizationToken);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].CustomQuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `custom_quiz_id` = @customQuizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customQuizId" + i, userQuizAuthorizationCodeWhere[i].CustomQuizId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizAuthorizationCodeWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userQuizAuthorizationCodeWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizAuthorizationCodeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userQuizAuthorizationCodeWhere.Count - 1)
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
                            UserQuizAuthorizationCodeModel userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel(); if (!reader.IsDBNull(0))
                                userQuizAuthorizationCode.Id = reader.GetValue(0).ToString();
                            else
                                userQuizAuthorizationCode.Id = null;
                            if (!reader.IsDBNull(1))
                                userQuizAuthorizationCode.UserId = reader.GetValue(1).ToString();
                            else
                                userQuizAuthorizationCode.UserId = null;
                            if (!reader.IsDBNull(2))
                                userQuizAuthorizationCode.AuthorizationToken = reader.GetValue(2).ToString();
                            else
                                userQuizAuthorizationCode.AuthorizationToken = null;
                            if (!reader.IsDBNull(3))
                                userQuizAuthorizationCode.CustomQuizId = reader.GetValue(3).ToString();
                            else
                                userQuizAuthorizationCode.CustomQuizId = null;
                            if (!reader.IsDBNull(4))
                                userQuizAuthorizationCode.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                userQuizAuthorizationCode.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                userQuizAuthorizationCode.CreatedAt = reader.GetValue(5).ToString();
                            else
                                userQuizAuthorizationCode.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                userQuizAuthorizationCode.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                userQuizAuthorizationCode.UpdatedAt = null;
                            response.Add(userQuizAuthorizationCode);
                            userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserQuizAuthorizationCodeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizAuthorizationCodeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserQuizAuthorizationCodeModel> Read(string connectionString, List<UserQuizAuthorizationCodeModel> userQuizAuthorizationCodeWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizAuthorizationCodeModel> response = new List<UserQuizAuthorizationCodeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `authorization_token`, `custom_quiz_id`, `modified_by`, `created_at`, `updated_at`  FROM user_quiz_authorization_codes ";
                if (userQuizAuthorizationCodeWhere != null && userQuizAuthorizationCodeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userQuizAuthorizationCodeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userQuizAuthorizationCodeWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userQuizAuthorizationCodeWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].AuthorizationToken))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `authorization_token` = @authorizationToken" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@authorizationToken" + i, userQuizAuthorizationCodeWhere[i].AuthorizationToken);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].CustomQuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `custom_quiz_id` = @customQuizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customQuizId" + i, userQuizAuthorizationCodeWhere[i].CustomQuizId);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizAuthorizationCodeWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userQuizAuthorizationCodeWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizAuthorizationCodeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userQuizAuthorizationCodeWhere.Count - 1)
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
                            UserQuizAuthorizationCodeModel userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel(); if (!reader.IsDBNull(0))
                                userQuizAuthorizationCode.Id = reader.GetValue(0).ToString();
                            else
                                userQuizAuthorizationCode.Id = null;
                            if (!reader.IsDBNull(1))
                                userQuizAuthorizationCode.UserId = reader.GetValue(1).ToString();
                            else
                                userQuizAuthorizationCode.UserId = null;
                            if (!reader.IsDBNull(2))
                                userQuizAuthorizationCode.AuthorizationToken = reader.GetValue(2).ToString();
                            else
                                userQuizAuthorizationCode.AuthorizationToken = null;
                            if (!reader.IsDBNull(3))
                                userQuizAuthorizationCode.CustomQuizId = reader.GetValue(3).ToString();
                            else
                                userQuizAuthorizationCode.CustomQuizId = null;
                            if (!reader.IsDBNull(4))
                                userQuizAuthorizationCode.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                userQuizAuthorizationCode.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                userQuizAuthorizationCode.CreatedAt = reader.GetValue(5).ToString();
                            else
                                userQuizAuthorizationCode.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                userQuizAuthorizationCode.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                userQuizAuthorizationCode.UpdatedAt = null;
                            response.Add(userQuizAuthorizationCode);
                            userQuizAuthorizationCode = new UserQuizAuthorizationCodeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserQuizAuthorizationCodeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizAuthorizationCodeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserQuizAuthorizationCodeModel> Create(string connectionString, UserQuizAuthorizationCodeModel userQuizAuthorizationCode)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizAuthorizationCodeModel> response = new List<UserQuizAuthorizationCodeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO user_quiz_authorization_codes (";
                string values = "(";
                if (!String.IsNullOrEmpty(userQuizAuthorizationCode.Id))
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
                    cmd.Parameters.AddWithValue("@id", userQuizAuthorizationCode.Id);
                }
                if (!String.IsNullOrEmpty(userQuizAuthorizationCode.UserId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `user_id` ");
                        values = String.Concat(values, ", @userId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `user_id` ");
                        values = String.Concat(values, " @userId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userQuizAuthorizationCode.UserId);
                }
                if (!String.IsNullOrEmpty(userQuizAuthorizationCode.AuthorizationToken))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `authorization_token` ");
                        values = String.Concat(values, ", @authorizationToken");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `authorization_token` ");
                        values = String.Concat(values, " @authorizationToken");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@authorizationToken", userQuizAuthorizationCode.AuthorizationToken);
                }
                if (!String.IsNullOrEmpty(userQuizAuthorizationCode.CustomQuizId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `custom_quiz_id` ");
                        values = String.Concat(values, ", @customQuizId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `custom_quiz_id` ");
                        values = String.Concat(values, " @customQuizId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@customQuizId", userQuizAuthorizationCode.CustomQuizId);
                }
                if (!String.IsNullOrEmpty(userQuizAuthorizationCode.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userQuizAuthorizationCode.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userQuizAuthorizationCode.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userQuizAuthorizationCode.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userQuizAuthorizationCode.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userQuizAuthorizationCode.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userQuizAuthorizationCode.Id = cmd.LastInsertedId.ToString();
                response.Add(userQuizAuthorizationCode);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizAuthorizationCodeModel>();
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
        public List<UserQuizAuthorizationCodeModel> Create(string connectionString, List<UserQuizAuthorizationCodeModel> userQuizAuthorizationCodeList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizAuthorizationCodeModel> response = new List<UserQuizAuthorizationCodeModel>();
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
                        for (int i = 0; i < userQuizAuthorizationCodeList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO user_quiz_authorization_codes (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(userQuizAuthorizationCodeList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userQuizAuthorizationCodeList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userQuizAuthorizationCodeList[i].UserId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `user_id` ");
                                        values = String.Concat(values, ", @userId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `user_id` ");
                                        values = String.Concat(values, "@userId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userQuizAuthorizationCodeList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userQuizAuthorizationCodeList[i].AuthorizationToken))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `authorization_token` ");
                                        values = String.Concat(values, ", @authorizationToken" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `authorization_token` ");
                                        values = String.Concat(values, "@authorizationToken" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@authorizationToken" + i, userQuizAuthorizationCodeList[i].AuthorizationToken);
                                }
                                if (!String.IsNullOrEmpty(userQuizAuthorizationCodeList[i].CustomQuizId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `custom_quiz_id` ");
                                        values = String.Concat(values, ", @customQuizId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `custom_quiz_id` ");
                                        values = String.Concat(values, "@customQuizId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@customQuizId" + i, userQuizAuthorizationCodeList[i].CustomQuizId);
                                }
                                if (!String.IsNullOrEmpty(userQuizAuthorizationCodeList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizAuthorizationCodeList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userQuizAuthorizationCodeList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userQuizAuthorizationCodeList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userQuizAuthorizationCodeList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizAuthorizationCodeList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userQuizAuthorizationCodeList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserQuizAuthorizationCodeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserQuizAuthorizationCodeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizAuthorizationCodeModel>();
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

        public List<UserQuizAuthorizationCodeModel> Update(string connectionString, UserQuizAuthorizationCodeModel userQuizAuthorizationCode, UserQuizAuthorizationCodeModel userQuizAuthorizationCodeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizAuthorizationCodeModel> response = new List<UserQuizAuthorizationCodeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE user_quiz_authorization_codes SET ";
                if (!String.IsNullOrEmpty(userQuizAuthorizationCode.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userQuizAuthorizationCode.Id);
                }
                if (!String.IsNullOrEmpty(userQuizAuthorizationCode.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userQuizAuthorizationCode.UserId);
                }
                if (!String.IsNullOrEmpty(userQuizAuthorizationCode.AuthorizationToken))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `authorization_token` = @authorizationToken");
                    else
                        sql = String.Concat(sql, " `authorization_token` = @authorizationToken");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@authorizationToken", userQuizAuthorizationCode.AuthorizationToken);
                }
                if (!String.IsNullOrEmpty(userQuizAuthorizationCode.CustomQuizId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `custom_quiz_id` = @customQuizId");
                    else
                        sql = String.Concat(sql, " `custom_quiz_id` = @customQuizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@customQuizId", userQuizAuthorizationCode.CustomQuizId);
                }
                if (!String.IsNullOrEmpty(userQuizAuthorizationCode.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userQuizAuthorizationCode.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userQuizAuthorizationCode.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userQuizAuthorizationCode.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userQuizAuthorizationCode.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userQuizAuthorizationCode.UpdatedAt);
                }
                if (userQuizAuthorizationCodeWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userQuizAuthorizationCodeWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userQuizAuthorizationCodeWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere.AuthorizationToken))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `authorization_token` = @authorizationTokenWhere");
                        else
                            sql = String.Concat(sql, " `authorization_token` = @authorizationTokenWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@authorizationTokenWhere", userQuizAuthorizationCodeWhere.AuthorizationToken);
                    }
                    if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere.CustomQuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `custom_quiz_id` = @customQuizIdWhere");
                        else
                            sql = String.Concat(sql, " `custom_quiz_id` = @customQuizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@customQuizIdWhere", userQuizAuthorizationCodeWhere.CustomQuizId);
                    }
                    if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userQuizAuthorizationCodeWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userQuizAuthorizationCodeWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userQuizAuthorizationCodeWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userQuizAuthorizationCode);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizAuthorizationCodeModel>();
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

        public List<UserQuizAuthorizationCodeModel> Update(string connectionString, List<UserQuizAuthorizationCodeModel> userQuizAuthorizationCodeList, List<UserQuizAuthorizationCodeModel> userQuizAuthorizationCodeWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizAuthorizationCodeModel> response = new List<UserQuizAuthorizationCodeModel>();
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
                        for (int i = 0; i < userQuizAuthorizationCodeList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE user_quiz_authorization_codes SET ";
                                if (!String.IsNullOrEmpty(userQuizAuthorizationCodeList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userQuizAuthorizationCodeList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userQuizAuthorizationCodeList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userQuizAuthorizationCodeList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userQuizAuthorizationCodeList[i].AuthorizationToken))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `authorization_token` = @authorizationToken" + i);
                                    else
                                        sql = String.Concat(sql, " `authorization_token` = @authorizationToken" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@authorizationToken" + i, userQuizAuthorizationCodeList[i].AuthorizationToken);
                                }
                                if (!String.IsNullOrEmpty(userQuizAuthorizationCodeList[i].CustomQuizId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `custom_quiz_id` = @customQuizId" + i);
                                    else
                                        sql = String.Concat(sql, " `custom_quiz_id` = @customQuizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@customQuizId" + i, userQuizAuthorizationCodeList[i].CustomQuizId);
                                }
                                if (!String.IsNullOrEmpty(userQuizAuthorizationCodeList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizAuthorizationCodeList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userQuizAuthorizationCodeList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userQuizAuthorizationCodeList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userQuizAuthorizationCodeList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizAuthorizationCodeList[i].UpdatedAt);
                                }
                                if (userQuizAuthorizationCodeWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userQuizAuthorizationCodeWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userQuizAuthorizationCodeWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereList[i].AuthorizationToken))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `authorization_token` = @authorizationTokenWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `authorization_token` = @authorizationTokenWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@authorizationTokenWhere" + i, userQuizAuthorizationCodeWhereList[i].AuthorizationToken);
                                    }
                                    if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereList[i].CustomQuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `custom_quiz_id` = @customQuizIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `custom_quiz_id` = @customQuizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@customQuizIdWhere" + i, userQuizAuthorizationCodeWhereList[i].CustomQuizId);
                                    }
                                    if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userQuizAuthorizationCodeWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userQuizAuthorizationCodeWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userQuizAuthorizationCodeWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userQuizAuthorizationCodeWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userQuizAuthorizationCodeList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserQuizAuthorizationCodeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserQuizAuthorizationCodeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizAuthorizationCodeModel>();
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

        public List<UserQuizAuthorizationCodeModel> Delete(string connectionString, UserQuizAuthorizationCodeModel userQuizAuthorizationCode)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizAuthorizationCodeModel> response = new List<UserQuizAuthorizationCodeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM user_quiz_authorization_codes  ";
                if (userQuizAuthorizationCode != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(userQuizAuthorizationCode.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userQuizAuthorizationCode.Id);
                    }
                    if (!String.IsNullOrEmpty(userQuizAuthorizationCode.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userQuizAuthorizationCode.UserId);
                    }
                    if (!String.IsNullOrEmpty(userQuizAuthorizationCode.AuthorizationToken))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `authorization_token` = @authorizationToken");
                        else
                            sql = String.Concat(sql, " `authorization_token` = @authorizationToken");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@authorizationToken", userQuizAuthorizationCode.AuthorizationToken);
                    }
                    if (!String.IsNullOrEmpty(userQuizAuthorizationCode.CustomQuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `custom_quiz_id` = @customQuizId");
                        else
                            sql = String.Concat(sql, " `custom_quiz_id` = @customQuizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@customQuizId", userQuizAuthorizationCode.CustomQuizId);
                    }
                    if (!String.IsNullOrEmpty(userQuizAuthorizationCode.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userQuizAuthorizationCode.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userQuizAuthorizationCode.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userQuizAuthorizationCode.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userQuizAuthorizationCode.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userQuizAuthorizationCode.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userQuizAuthorizationCode);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizAuthorizationCodeModel>();
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

        public List<UserQuizAuthorizationCodeModel> Delete(string connectionString, List<UserQuizAuthorizationCodeModel> userQuizAuthorizationCode)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizAuthorizationCodeModel> response = new List<UserQuizAuthorizationCodeModel>();
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
                        for (int i = 0; i < userQuizAuthorizationCode.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM user_quiz_authorization_codes  ";
                                if (userQuizAuthorizationCode[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(userQuizAuthorizationCode[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userQuizAuthorizationCode[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userQuizAuthorizationCode[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userQuizAuthorizationCode[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userQuizAuthorizationCode[i].AuthorizationToken))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `authorization_token` = @authorizationToken" + i);
                                        else
                                            sql = String.Concat(sql, " `authorization_token` = @authorizationToken" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@authorizationToken" + i, userQuizAuthorizationCode[i].AuthorizationToken);
                                    }
                                    if (!String.IsNullOrEmpty(userQuizAuthorizationCode[i].CustomQuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `custom_quiz_id` = @customQuizId" + i);
                                        else
                                            sql = String.Concat(sql, " `custom_quiz_id` = @customQuizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@customQuizId" + i, userQuizAuthorizationCode[i].CustomQuizId);
                                    }
                                    if (!String.IsNullOrEmpty(userQuizAuthorizationCode[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizAuthorizationCode[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userQuizAuthorizationCode[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userQuizAuthorizationCode[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userQuizAuthorizationCode[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizAuthorizationCode[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userQuizAuthorizationCode[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserQuizAuthorizationCodeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserQuizAuthorizationCodeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizAuthorizationCodeModel>();
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
