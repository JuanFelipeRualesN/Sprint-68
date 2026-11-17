using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.PersonalDetails;
using UsersAPI.UsersAPI.Services.PersonalDetails.CustomClasses;
namespace UsersAPI.UsersAPI.Infrastructure.Repositories.PersonalDetails
{
    class UserDobDAO
    {
        private MySqlConnection _connection;
        public List<UserDobModel> Read(string connectionString, UserDobModel userDob, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserDobModel> response = new List<UserDobModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `dob`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_dobs ";
                {
                    if (userDob != null)
                    {
                        if (!String.IsNullOrEmpty(userDob.Id))
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
                            cmd.Parameters.AddWithValue("@id", userDob.Id);
                        }
                        if (!String.IsNullOrEmpty(userDob.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userDob.UserId);
                        }
                        if (!String.IsNullOrEmpty(userDob.Dob))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `dob` = @dob ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `dob` = @dob ");
                            }
                            cmd.Parameters.AddWithValue("@dob", userDob.Dob);
                        }
                        if (!String.IsNullOrEmpty(userDob.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userDob.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userDob.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userDob.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userDob.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userDob.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = String.Concat(sql, " ORDER BY updated_at DESC");
                        if (firstOne)
                            sql = String.Concat(sql, " LIMIT 1 ");
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    userDob.Id = reader.GetValue(0).ToString();
                                else
                                    userDob.Id = null;
                                if (!reader.IsDBNull(1))
                                    userDob.UserId = reader.GetValue(1).ToString();
                                else
                                    userDob.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userDob.Dob = reader.GetValue(2).ToString();
                                else
                                    userDob.Dob = null;
                                if (!reader.IsDBNull(3))
                                    userDob.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    userDob.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    userDob.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    userDob.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    userDob.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    userDob.UpdatedAt = null;
                                response.Add(userDob);
                                userDob = new UserDobModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserDobModel>();
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
                response = new List<UserDobModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserDobModel> Read(string connectionString, List<UserDobModel> userDobWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserDobModel> response = new List<UserDobModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `dob`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_dobs ";
                if (userDobWhere != null && userDobWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userDobWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userDobWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userDobWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userDobWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userDobWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userDobWhere[i].Dob))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `dob` = @dob" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dob" + i, userDobWhere[i].Dob);
                        }
                        if (!String.IsNullOrEmpty(userDobWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userDobWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userDobWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userDobWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userDobWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userDobWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userDobWhere.Count - 1)
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
                            UserDobModel userDob = new UserDobModel(); if (!reader.IsDBNull(0))
                                userDob.Id = reader.GetValue(0).ToString();
                            else
                                userDob.Id = null;
                            if (!reader.IsDBNull(1))
                                userDob.UserId = reader.GetValue(1).ToString();
                            else
                                userDob.UserId = null;
                            if (!reader.IsDBNull(2))
                                userDob.Dob = reader.GetValue(2).ToString();
                            else
                                userDob.Dob = null;
                            if (!reader.IsDBNull(3))
                                userDob.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                userDob.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                userDob.CreatedAt = reader.GetValue(4).ToString();
                            else
                                userDob.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                userDob.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                userDob.UpdatedAt = null;
                            response.Add(userDob);
                            userDob = new UserDobModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserDobModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserDobModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserDobModel> Create(string connectionString, UserDobModel userDob)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserDobModel> response = new List<UserDobModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO upd_user_dobs (";
                string values = "(";
                if (!String.IsNullOrEmpty(userDob.Id))
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
                    cmd.Parameters.AddWithValue("@id", userDob.Id);
                }
                if (!String.IsNullOrEmpty(userDob.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", userDob.UserId);
                }
                if (!String.IsNullOrEmpty(userDob.Dob))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `dob` ");
                        values = String.Concat(values, ", @dob");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `dob` ");
                        values = String.Concat(values, " @dob");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dob", userDob.Dob);
                }
                if (!String.IsNullOrEmpty(userDob.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userDob.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userDob.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userDob.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userDob.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userDob.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userDob.Id = cmd.LastInsertedId.ToString();
                response.Add(userDob);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserDobModel>();
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
        public List<UserDobModel> Create(string connectionString, List<UserDobModel> userDobList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserDobModel> response = new List<UserDobModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userDobList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO upd_user_dobs (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(userDobList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userDobList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userDobList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, userDobList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userDobList[i].Dob))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `dob` ");
                                        values = String.Concat(values, ", @dob" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `dob` ");
                                        values = String.Concat(values, "@dob" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dob" + i, userDobList[i].Dob);
                                }
                                if (!String.IsNullOrEmpty(userDobList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userDobList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userDobList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userDobList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userDobList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userDobList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userDobList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserDobModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserDobModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserDobModel>();
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

        public List<UserDobModel> Update(string connectionString, UserDobModel userDob, UserDobModel userDobWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserDobModel> response = new List<UserDobModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE upd_user_dobs SET ";
                if (!String.IsNullOrEmpty(userDob.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userDob.Id);
                }
                if (!String.IsNullOrEmpty(userDob.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userDob.UserId);
                }
                if (!String.IsNullOrEmpty(userDob.Dob))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `dob` = @dob");
                    else
                        sql = String.Concat(sql, " `dob` = @dob");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dob", userDob.Dob);
                }
                if (!String.IsNullOrEmpty(userDob.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userDob.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userDob.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userDob.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userDob.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userDob.UpdatedAt);
                }
                if (userDobWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(userDobWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userDobWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(userDobWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userDobWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(userDobWhere.Dob))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `dob` = @dobWhere");
                        else
                            sql = String.Concat(sql, " `dob` = @dobWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dobWhere", userDobWhere.Dob);
                    }
                    if (!String.IsNullOrEmpty(userDobWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userDobWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userDobWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userDobWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userDobWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userDobWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userDob);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserDobModel>();
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

        public List<UserDobModel> Update(string connectionString, List<UserDobModel> userDobList, List<UserDobModel> userDobWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserDobModel> response = new List<UserDobModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userDobList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE upd_user_dobs SET ";
                                if (!String.IsNullOrEmpty(userDobList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userDobList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userDobList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userDobList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userDobList[i].Dob))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `dob` = @dob" + i);
                                    else
                                        sql = String.Concat(sql, " `dob` = @dob" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dob" + i, userDobList[i].Dob);
                                }
                                if (!String.IsNullOrEmpty(userDobList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userDobList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userDobList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userDobList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userDobList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userDobList[i].UpdatedAt);
                                }
                                if (userDobWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(userDobWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userDobWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userDobWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userDobWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userDobWhereList[i].Dob))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `dob` = @dobWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `dob` = @dobWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dobWhere" + i, userDobWhereList[i].Dob);
                                    }
                                    if (!String.IsNullOrEmpty(userDobWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userDobWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userDobWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userDobWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userDobWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userDobWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userDobList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserDobModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserDobModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserDobModel>();
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

        public List<UserDobModel> Delete(string connectionString, UserDobModel userDob)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserDobModel> response = new List<UserDobModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM upd_user_dobs  ";
                if (userDob != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(userDob.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userDob.Id);
                    }
                    if (!String.IsNullOrEmpty(userDob.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userDob.UserId);
                    }
                    if (!String.IsNullOrEmpty(userDob.Dob))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `dob` = @dob");
                        else
                            sql = String.Concat(sql, " `dob` = @dob");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dob", userDob.Dob);
                    }
                    if (!String.IsNullOrEmpty(userDob.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userDob.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userDob.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userDob.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userDob.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userDob.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userDob);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserDobModel>();
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

        public List<UserDobModel> Delete(string connectionString, List<UserDobModel> userDob)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserDobModel> response = new List<UserDobModel>();
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
                        for (int i = 0; i < userDob.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM upd_user_dobs  ";
                                if (userDob[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(userDob[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userDob[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userDob[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userDob[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userDob[i].Dob))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `dob` = @dob" + i);
                                        else
                                            sql = String.Concat(sql, " `dob` = @dob" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dob" + i, userDob[i].Dob);
                                    }
                                    if (!String.IsNullOrEmpty(userDob[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userDob[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userDob[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userDob[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userDob[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userDob[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userDob[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserDobModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserDobModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserDobModel>();
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
