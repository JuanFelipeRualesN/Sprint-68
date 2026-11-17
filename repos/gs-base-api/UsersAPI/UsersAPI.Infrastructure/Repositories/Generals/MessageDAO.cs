using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Generals;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Generals
{
    class MessageDAO
    {
        private MySqlConnection _connection;
        public List<MessageModel> Read(string connectionString, MessageModel message, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<MessageModel> response = new List<MessageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `code`, `desc`, `lang_code`, `modified_by`, `created_at`, `updated_at`  FROM messages ";
                {
                    if (message != null)
                    {
                        if (!String.IsNullOrEmpty(message.Id))
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
                            cmd.Parameters.AddWithValue("@id", message.Id);
                        }
                        if (!String.IsNullOrEmpty(message.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", message.Code);
                        }
                        if (!String.IsNullOrEmpty(message.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", message.Desc);
                        }
                        if (!String.IsNullOrEmpty(message.LangCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lang_code` = @langCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lang_code` = @langCode ");
                            }
                            cmd.Parameters.AddWithValue("@langCode", message.LangCode);
                        }
                        if (!String.IsNullOrEmpty(message.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", message.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(message.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", message.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(message.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", message.UpdatedAt);
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
                                    message.Id = reader.GetValue(0).ToString();
                                else
                                    message.Id = null;
                                if (!reader.IsDBNull(1))
                                    message.Code = reader.GetValue(1).ToString();
                                else
                                    message.Code = null;
                                if (!reader.IsDBNull(2))
                                    message.Desc = reader.GetValue(2).ToString();
                                else
                                    message.Desc = null;
                                if (!reader.IsDBNull(3))
                                    message.LangCode = reader.GetValue(3).ToString();
                                else
                                    message.LangCode = null;
                                if (!reader.IsDBNull(4))
                                    message.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    message.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    message.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    message.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    message.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    message.UpdatedAt = null;
                                response.Add(message);
                                message = new MessageModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<MessageModel>();
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
                response = new List<MessageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<MessageModel> Read(string connectionString, List<MessageModel> messageWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<MessageModel> response = new List<MessageModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `code`, `desc`, `lang_code`, `modified_by`, `created_at`, `updated_at`  FROM messages ";
                if (messageWhere != null && messageWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < messageWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(messageWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, messageWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(messageWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, messageWhere[i].Code);
                        }
                        if (!String.IsNullOrEmpty(messageWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, messageWhere[i].Desc);
                        }
                        if (!String.IsNullOrEmpty(messageWhere[i].LangCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lang_code` = @langCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@langCode" + i, messageWhere[i].LangCode);
                        }
                        if (!String.IsNullOrEmpty(messageWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, messageWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(messageWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, messageWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(messageWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, messageWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != messageWhere.Count - 1)
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
                            MessageModel message = new MessageModel(); if (!reader.IsDBNull(0))
                                message.Id = reader.GetValue(0).ToString();
                            else
                                message.Id = null;
                            if (!reader.IsDBNull(1))
                                message.Code = reader.GetValue(1).ToString();
                            else
                                message.Code = null;
                            if (!reader.IsDBNull(2))
                                message.Desc = reader.GetValue(2).ToString();
                            else
                                message.Desc = null;
                            if (!reader.IsDBNull(3))
                                message.LangCode = reader.GetValue(3).ToString();
                            else
                                message.LangCode = null;
                            if (!reader.IsDBNull(4))
                                message.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                message.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                message.CreatedAt = reader.GetValue(5).ToString();
                            else
                                message.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                message.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                message.UpdatedAt = null;
                            response.Add(message);
                            message = new MessageModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<MessageModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<MessageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<MessageModel> Create(string connectionString, MessageModel message)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<MessageModel> response = new List<MessageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO messages (";
                string values = "(";
                if (!String.IsNullOrEmpty(message.Id))
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
                    cmd.Parameters.AddWithValue("@id", message.Id);
                }
                if (!String.IsNullOrEmpty(message.Code))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `code` ");
                        values = String.Concat(values, ", @code");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `code` ");
                        values = String.Concat(values, " @code");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", message.Code);
                }
                if (!String.IsNullOrEmpty(message.Desc))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `desc` ");
                        values = String.Concat(values, ", @desc");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `desc` ");
                        values = String.Concat(values, " @desc");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", message.Desc);
                }
                if (!String.IsNullOrEmpty(message.LangCode))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `lang_code` ");
                        values = String.Concat(values, ", @langCode");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `lang_code` ");
                        values = String.Concat(values, " @langCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@langCode", message.LangCode);
                }
                if (!String.IsNullOrEmpty(message.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", message.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(message.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", message.CreatedAt);
                }
                if (!String.IsNullOrEmpty(message.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", message.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                message.Id = cmd.LastInsertedId.ToString();
                response.Add(message);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<MessageModel>();
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
        public List<MessageModel> Create(string connectionString, List<MessageModel> messageList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<MessageModel> response = new List<MessageModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < messageList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO messages (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(messageList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, messageList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(messageList[i].Code))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `code` ");
                                        values = String.Concat(values, ", @code" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `code` ");
                                        values = String.Concat(values, "@code" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, messageList[i].Code);
                                }
                                if (!String.IsNullOrEmpty(messageList[i].Desc))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `desc` ");
                                        values = String.Concat(values, ", @desc" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `desc` ");
                                        values = String.Concat(values, "@desc" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, messageList[i].Desc);
                                }
                                if (!String.IsNullOrEmpty(messageList[i].LangCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `lang_code` ");
                                        values = String.Concat(values, ", @langCode" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `lang_code` ");
                                        values = String.Concat(values, "@langCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@langCode" + i, messageList[i].LangCode);
                                }
                                if (!String.IsNullOrEmpty(messageList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, messageList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(messageList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, messageList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(messageList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, messageList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(messageList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<MessageModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<MessageModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<MessageModel>();
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

        public List<MessageModel> Update(string connectionString, MessageModel message, MessageModel messageWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<MessageModel> response = new List<MessageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE messages SET ";
                if (!String.IsNullOrEmpty(message.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", message.Id);
                }
                if (!String.IsNullOrEmpty(message.Code))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `code` = @code");
                    else
                        sql = String.Concat(sql, " `code` = @code");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", message.Code);
                }
                if (!String.IsNullOrEmpty(message.Desc))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `desc` = @desc");
                    else
                        sql = String.Concat(sql, " `desc` = @desc");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", message.Desc);
                }
                if (!String.IsNullOrEmpty(message.LangCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `lang_code` = @langCode");
                    else
                        sql = String.Concat(sql, " `lang_code` = @langCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@langCode", message.LangCode);
                }
                if (!String.IsNullOrEmpty(message.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", message.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(message.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", message.CreatedAt);
                }
                if (!String.IsNullOrEmpty(message.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", message.UpdatedAt);
                }
                if (messageWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(messageWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", messageWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(messageWhere.Code))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `code` = @codeWhere");
                        else
                            sql = String.Concat(sql, " `code` = @codeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@codeWhere", messageWhere.Code);
                    }
                    if (!String.IsNullOrEmpty(messageWhere.Desc))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `desc` = @descWhere");
                        else
                            sql = String.Concat(sql, " `desc` = @descWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descWhere", messageWhere.Desc);
                    }
                    if (!String.IsNullOrEmpty(messageWhere.LangCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lang_code` = @langCodeWhere");
                        else
                            sql = String.Concat(sql, " `lang_code` = @langCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCodeWhere", messageWhere.LangCode);
                    }
                    if (!String.IsNullOrEmpty(messageWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", messageWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(messageWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", messageWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(messageWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", messageWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(message);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<MessageModel>();
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

        public List<MessageModel> Update(string connectionString, List<MessageModel> messageList, List<MessageModel> messageWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<MessageModel> response = new List<MessageModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < messageList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE messages SET ";
                                if (!String.IsNullOrEmpty(messageList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, messageList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(messageList[i].Code))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `code` = @code" + i);
                                    else
                                        sql = String.Concat(sql, " `code` = @code" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, messageList[i].Code);
                                }
                                if (!String.IsNullOrEmpty(messageList[i].Desc))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `desc` = @desc" + i);
                                    else
                                        sql = String.Concat(sql, " `desc` = @desc" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, messageList[i].Desc);
                                }
                                if (!String.IsNullOrEmpty(messageList[i].LangCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `lang_code` = @langCode" + i);
                                    else
                                        sql = String.Concat(sql, " `lang_code` = @langCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@langCode" + i, messageList[i].LangCode);
                                }
                                if (!String.IsNullOrEmpty(messageList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, messageList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(messageList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, messageList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(messageList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, messageList[i].UpdatedAt);
                                }
                                if (messageWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(messageWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, messageWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(messageWhereList[i].Code))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `code` = @codeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `code` = @codeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@codeWhere" + i, messageWhereList[i].Code);
                                    }
                                    if (!String.IsNullOrEmpty(messageWhereList[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `desc` = @descWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `desc` = @descWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descWhere" + i, messageWhereList[i].Desc);
                                    }
                                    if (!String.IsNullOrEmpty(messageWhereList[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lang_code` = @langCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `lang_code` = @langCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCodeWhere" + i, messageWhereList[i].LangCode);
                                    }
                                    if (!String.IsNullOrEmpty(messageWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, messageWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(messageWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, messageWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(messageWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, messageWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(messageList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<MessageModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<MessageModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<MessageModel>();
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

        public List<MessageModel> Delete(string connectionString, MessageModel message)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<MessageModel> response = new List<MessageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM messages  ";
                if (message != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(message.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", message.Id);
                    }
                    if (!String.IsNullOrEmpty(message.Code))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `code` = @code");
                        else
                            sql = String.Concat(sql, " `code` = @code");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@code", message.Code);
                    }
                    if (!String.IsNullOrEmpty(message.Desc))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `desc` = @desc");
                        else
                            sql = String.Concat(sql, " `desc` = @desc");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@desc", message.Desc);
                    }
                    if (!String.IsNullOrEmpty(message.LangCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lang_code` = @langCode");
                        else
                            sql = String.Concat(sql, " `lang_code` = @langCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCode", message.LangCode);
                    }
                    if (!String.IsNullOrEmpty(message.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", message.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(message.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", message.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(message.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", message.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(message);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<MessageModel>();
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

        public List<MessageModel> Delete(string connectionString, List<MessageModel> message)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<MessageModel> response = new List<MessageModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < message.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM messages  ";
                                if (message[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(message[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, message[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(message[i].Code))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `code` = @code" + i);
                                        else
                                            sql = String.Concat(sql, " `code` = @code" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@code" + i, message[i].Code);
                                    }
                                    if (!String.IsNullOrEmpty(message[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `desc` = @desc" + i);
                                        else
                                            sql = String.Concat(sql, " `desc` = @desc" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@desc" + i, message[i].Desc);
                                    }
                                    if (!String.IsNullOrEmpty(message[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lang_code` = @langCode" + i);
                                        else
                                            sql = String.Concat(sql, " `lang_code` = @langCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCode" + i, message[i].LangCode);
                                    }
                                    if (!String.IsNullOrEmpty(message[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, message[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(message[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, message[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(message[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, message[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(message[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<MessageModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<MessageModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<MessageModel>();
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
