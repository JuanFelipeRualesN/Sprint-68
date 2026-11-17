using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.PersonalDetails;
using UsersAPI.UsersAPI.Services.PersonalDetails.CustomClasses;
namespace UsersAPI.UsersAPI.Infrastructure.Repositories.PersonalDetails
{
    class UserGenderDAO
    {
        private MySqlConnection _connection;
        public List<UserGenderModel> Read(string connectionString, UserGenderModel userGender, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserGenderModel> response = new List<UserGenderModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `gender`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_genders ";
                {
                    if (userGender != null)
                    {
                        if (!String.IsNullOrEmpty(userGender.Id))
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
                            cmd.Parameters.AddWithValue("@id", userGender.Id);
                        }
                        if (!String.IsNullOrEmpty(userGender.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userGender.UserId);
                        }
                        if (!String.IsNullOrEmpty(userGender.Gender))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `gender` = @gender ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `gender` = @gender ");
                            }
                            cmd.Parameters.AddWithValue("@gender", userGender.Gender);
                        }
                        if (!String.IsNullOrEmpty(userGender.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userGender.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userGender.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userGender.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userGender.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userGender.UpdatedAt);
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
                                    userGender.Id = reader.GetValue(0).ToString();
                                else
                                    userGender.Id = null;
                                if (!reader.IsDBNull(1))
                                    userGender.UserId = reader.GetValue(1).ToString();
                                else
                                    userGender.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userGender.Gender = reader.GetValue(2).ToString();
                                else
                                    userGender.Gender = null;
                                if (!reader.IsDBNull(3))
                                    userGender.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    userGender.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    userGender.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    userGender.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    userGender.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    userGender.UpdatedAt = null;
                                response.Add(userGender);
                                userGender = new UserGenderModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserGenderModel>();
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
                response = new List<UserGenderModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserGenderModel> Read(string connectionString, List<UserGenderModel> userGenderWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserGenderModel> response = new List<UserGenderModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `gender`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_genders ";
                if (userGenderWhere != null && userGenderWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userGenderWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userGenderWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userGenderWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userGenderWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userGenderWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userGenderWhere[i].Gender))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `gender` = @gender" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@gender" + i, userGenderWhere[i].Gender);
                        }
                        if (!String.IsNullOrEmpty(userGenderWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userGenderWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userGenderWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userGenderWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userGenderWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userGenderWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userGenderWhere.Count - 1)
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
                            UserGenderModel userGender = new UserGenderModel(); if (!reader.IsDBNull(0))
                                userGender.Id = reader.GetValue(0).ToString();
                            else
                                userGender.Id = null;
                            if (!reader.IsDBNull(1))
                                userGender.UserId = reader.GetValue(1).ToString();
                            else
                                userGender.UserId = null;
                            if (!reader.IsDBNull(2))
                                userGender.Gender = reader.GetValue(2).ToString();
                            else
                                userGender.Gender = null;
                            if (!reader.IsDBNull(3))
                                userGender.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                userGender.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                userGender.CreatedAt = reader.GetValue(4).ToString();
                            else
                                userGender.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                userGender.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                userGender.UpdatedAt = null;
                            response.Add(userGender);
                            userGender = new UserGenderModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserGenderModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserGenderModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserGenderModel> Create(string connectionString, UserGenderModel userGender)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserGenderModel> response = new List<UserGenderModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO upd_user_genders (";
                string values = "(";
                if (!String.IsNullOrEmpty(userGender.Id))
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
                    cmd.Parameters.AddWithValue("@id", userGender.Id);
                }
                if (!String.IsNullOrEmpty(userGender.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", userGender.UserId);
                }
                if (!String.IsNullOrEmpty(userGender.Gender))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `gender` ");
                        values = String.Concat(values, ", @gender");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `gender` ");
                        values = String.Concat(values, " @gender");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@gender", userGender.Gender);
                }
                if (!String.IsNullOrEmpty(userGender.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userGender.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userGender.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userGender.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userGender.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userGender.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userGender.Id = cmd.LastInsertedId.ToString();
                response.Add(userGender);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserGenderModel>();
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
        public List<UserGenderModel> Create(string connectionString, List<UserGenderModel> userGenderList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserGenderModel> response = new List<UserGenderModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userGenderList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO upd_user_genders (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(userGenderList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userGenderList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userGenderList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, userGenderList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userGenderList[i].Gender))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `gender` ");
                                        values = String.Concat(values, ", @gender" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `gender` ");
                                        values = String.Concat(values, "@gender" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@gender" + i, userGenderList[i].Gender);
                                }
                                if (!String.IsNullOrEmpty(userGenderList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userGenderList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userGenderList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userGenderList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userGenderList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userGenderList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userGenderList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserGenderModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserGenderModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserGenderModel>();
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

        public List<UserGenderModel> Update(string connectionString, UserGenderModel userGender, UserGenderModel userGenderWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserGenderModel> response = new List<UserGenderModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE upd_user_genders SET ";
                if (!String.IsNullOrEmpty(userGender.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userGender.Id);
                }
                if (!String.IsNullOrEmpty(userGender.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userGender.UserId);
                }
                if (!String.IsNullOrEmpty(userGender.Gender))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `gender` = @gender");
                    else
                        sql = String.Concat(sql, " `gender` = @gender");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@gender", userGender.Gender);
                }
                if (!String.IsNullOrEmpty(userGender.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userGender.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userGender.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userGender.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userGender.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userGender.UpdatedAt);
                }
                if (userGenderWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(userGenderWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userGenderWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(userGenderWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userGenderWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(userGenderWhere.Gender))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `gender` = @genderWhere");
                        else
                            sql = String.Concat(sql, " `gender` = @genderWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@genderWhere", userGenderWhere.Gender);
                    }
                    if (!String.IsNullOrEmpty(userGenderWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userGenderWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userGenderWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userGenderWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userGenderWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userGenderWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userGender);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserGenderModel>();
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

        public List<UserGenderModel> Update(string connectionString, List<UserGenderModel> userGenderList, List<UserGenderModel> userGenderWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserGenderModel> response = new List<UserGenderModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userGenderList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE upd_user_genders SET ";
                                if (!String.IsNullOrEmpty(userGenderList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userGenderList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userGenderList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userGenderList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userGenderList[i].Gender))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `gender` = @gender" + i);
                                    else
                                        sql = String.Concat(sql, " `gender` = @gender" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@gender" + i, userGenderList[i].Gender);
                                }
                                if (!String.IsNullOrEmpty(userGenderList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userGenderList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userGenderList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userGenderList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userGenderList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userGenderList[i].UpdatedAt);
                                }
                                if (userGenderWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(userGenderWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userGenderWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userGenderWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userGenderWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userGenderWhereList[i].Gender))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `gender` = @genderWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `gender` = @genderWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@genderWhere" + i, userGenderWhereList[i].Gender);
                                    }
                                    if (!String.IsNullOrEmpty(userGenderWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userGenderWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userGenderWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userGenderWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userGenderWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userGenderWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userGenderList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserGenderModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserGenderModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserGenderModel>();
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

        public List<UserGenderModel> Delete(string connectionString, UserGenderModel userGender)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserGenderModel> response = new List<UserGenderModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM upd_user_genders  ";
                if (userGender != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(userGender.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userGender.Id);
                    }
                    if (!String.IsNullOrEmpty(userGender.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userGender.UserId);
                    }
                    if (!String.IsNullOrEmpty(userGender.Gender))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `gender` = @gender");
                        else
                            sql = String.Concat(sql, " `gender` = @gender");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@gender", userGender.Gender);
                    }
                    if (!String.IsNullOrEmpty(userGender.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userGender.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userGender.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userGender.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userGender.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userGender.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userGender);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserGenderModel>();
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

        public List<UserGenderModel> Delete(string connectionString, List<UserGenderModel> userGender)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserGenderModel> response = new List<UserGenderModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userGender.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM upd_user_genders  ";
                                if (userGender[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(userGender[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userGender[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userGender[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userGender[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userGender[i].Gender))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `gender` = @gender" + i);
                                        else
                                            sql = String.Concat(sql, " `gender` = @gender" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@gender" + i, userGender[i].Gender);
                                    }
                                    if (!String.IsNullOrEmpty(userGender[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userGender[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userGender[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userGender[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userGender[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userGender[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userGender[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserGenderModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserGenderModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserGenderModel>();
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
